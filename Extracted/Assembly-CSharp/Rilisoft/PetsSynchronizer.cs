using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200083A RID: 2106
	internal sealed class PetsSynchronizer
	{
		// Token: 0x06004C7C RID: 19580 RVA: 0x001B897C File Offset: 0x001B6B7C
		private PetsSynchronizer()
		{
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004C7E RID: 19582 RVA: 0x001B89A0 File Offset: 0x001B6BA0
		public static PetsSynchronizer Instance
		{
			get
			{
				return PetsSynchronizer.s_instance;
			}
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x001B89A8 File Offset: 0x001B6BA8
		public Coroutine Sync()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(this.SyncGoogleCoroutine(false));
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.SyncAmazon();
				}
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				this.SyncPetsIos();
				return null;
			}
			return null;
		}

		// Token: 0x06004C80 RID: 19584 RVA: 0x001B8A04 File Offset: 0x001B6C04
		private void SyncAmazon()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(callee, false))
			{
				try
				{
					if (!Application.isEditor)
					{
						AGSWhispersyncClient.Synchronize();
						using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
						{
							this.EnsureNotNull(gameData, "dataMap");
							using (AGSSyncableString latestString = gameData.GetLatestString("petsJson"))
							{
								this.EnsureNotNull(latestString, "petsJson");
								string json = latestString.GetValue() ?? JsonUtility.ToJson(new PlayerPets());
								PlayerPets playerPets = PetsManager.LoadPlayerPetsMemento();
								PlayerPets playerPets2 = JsonUtility.FromJson<PlayerPets>(json);
								PlayerPets playerPets3 = PlayerPets.Merge(playerPets, playerPets2);
								bool flag = PetsSynchronizer.IsDirtyComparedToMergedMemento(playerPets, playerPets3);
								if (flag)
								{
									PetsManager.OverwritePlayerPetsMemento(playerPets3);
									PetsManager.LoadPetsToMemory();
								}
								if (PetsSynchronizer.IsDirtyComparedToMergedMemento(playerPets2, playerPets3) || playerPets2.Conflicted)
								{
									string val = JsonUtility.ToJson(playerPets3);
									latestString.Set(val);
									AGSWhispersyncClient.Synchronize();
								}
							}
						}
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06004C81 RID: 19585 RVA: 0x001B8BA8 File Offset: 0x001B6DA8
		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		// Token: 0x06004C82 RID: 19586 RVA: 0x001B8BC4 File Offset: 0x001B6DC4
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.SyncGoogleCoroutine('{1}')", new object[]
			{
				base.GetType().Name,
				(!pullOnly) ? "sync" : "pull"
			});
			using (new ScopeLogger(thisMethod, false))
			{
				PetsSynchronizerGpgFacade googleSavedGamesFacade = default(PetsSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					Task<GoogleSavedGameRequestResult<PlayerPets>> future = googleSavedGamesFacade.Pull();
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarning("Failed to pull pets with exception: " + ex.Message);
						yield return PetsSynchronizer.s_delay;
					}
					else
					{
						SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
						if (requestStatus != SavedGameRequestStatus.Success)
						{
							Debug.LogWarning("Failed to pull pets with status: " + requestStatus);
							yield return PetsSynchronizer.s_delay;
						}
						else
						{
							PlayerPets localPetsMemento = PetsManager.LoadPlayerPetsMemento();
							PlayerPets cloudPetsMemento = future.Result.Value;
							PlayerPets mergedPetsMemento = PlayerPets.Merge(localPetsMemento, cloudPetsMemento);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Local pets: {0}", new object[]
								{
									JsonUtility.ToJson(localPetsMemento)
								});
								Debug.LogFormat("Cloud pets: {0}", new object[]
								{
									JsonUtility.ToJson(cloudPetsMemento)
								});
								Debug.LogFormat("Merged pets: {0}", new object[]
								{
									JsonUtility.ToJson(mergedPetsMemento)
								});
							}
							bool localDirty = PetsSynchronizer.IsDirtyComparedToMergedMemento(localPetsMemento, mergedPetsMemento);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Local pets are dirty: {0}", new object[]
								{
									localDirty
								});
							}
							if (localDirty)
							{
								PetsManager.OverwritePlayerPetsMemento(mergedPetsMemento);
								PetsManager.LoadPetsToMemory();
							}
							if (pullOnly)
							{
								yield break;
							}
							bool cloudDirty = PetsSynchronizer.IsDirtyComparedToMergedMemento(cloudPetsMemento, mergedPetsMemento);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Cloud pets are dirty: {0}, conflicted: {1}", new object[]
								{
									cloudDirty,
									cloudPetsMemento.Conflicted
								});
							}
							if (cloudDirty || cloudPetsMemento.Conflicted)
							{
								IEnumerator enumerator = this.PushGoogleCoroutine();
								while (enumerator.MoveNext())
								{
									yield return null;
								}
							}
							yield break;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06004C83 RID: 19587 RVA: 0x001B8BF0 File Offset: 0x001B6DF0
		private IEnumerator PushGoogleCoroutine()
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PushGoogleCoroutine()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(thisMethod, false))
			{
				PetsSynchronizerGpgFacade googleSavedGamesFacade = default(PetsSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					PlayerPets localPetsMemento = PetsManager.LoadPlayerPetsMemento();
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Local pets: {0}", new object[]
						{
							JsonUtility.ToJson(localPetsMemento)
						});
					}
					Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localPetsMemento);
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarning("Failed to push pets with exception: " + ex.Message);
						yield return PetsSynchronizer.s_delay;
					}
					else
					{
						SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
						if (requestStatus == SavedGameRequestStatus.Success)
						{
							if (Defs.IsDeveloperBuild)
							{
								ISavedGameMetadata metadata = future.Result.Value;
								string description = (metadata == null) ? string.Empty : metadata.Description;
								Debug.LogFormat("[Pets] Succeeded to push pets {0}: '{1}'", new object[]
								{
									localPetsMemento,
									description
								});
							}
							yield break;
						}
						Debug.LogWarning("Failed to push pets with status: " + requestStatus);
						yield return PetsSynchronizer.s_delay;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004C84 RID: 19588 RVA: 0x001B8C0C File Offset: 0x001B6E0C
		private void SyncPetsIos()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.ICloudAvailable)
			{
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x001B8C24 File Offset: 0x001B6E24
		private static bool IsDirtyComparedToMergedMemento(PlayerPets otherPetsMemento, PlayerPets mergedMemento)
		{
			try
			{
				bool flag = (from pet in mergedMemento.Pets
				select pet.InfoId).Except(from pet in otherPetsMemento.Pets
				select pet.InfoId).Any<string>();
				if (flag)
				{
					return true;
				}
				IOrderedEnumerable<PlayerPet> source = from pet in mergedMemento.Pets
				orderby pet.InfoId
				select pet;
				IOrderedEnumerable<PlayerPet> source2 = from pet in otherPetsMemento.Pets
				orderby pet.InfoId
				select pet;
				bool result;
				if ((from pet in source
				select pet.PetName).SequenceEqual(from pet in source2
				select pet.PetName))
				{
					result = !(from pet in source
					select pet.NameTimestamp).SequenceEqual(from pet in source2
					select pet.NameTimestamp);
				}
				else
				{
					result = true;
				}
				return result;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in SyncPetsIos dirtyCondition: {0}", new object[]
				{
					ex
				});
			}
			return false;
		}

		// Token: 0x04003B4B RID: 15179
		private const int AttemptCountMax = 3;

		// Token: 0x04003B4C RID: 15180
		private static readonly WaitForRealSeconds s_delay = new WaitForRealSeconds(30f);

		// Token: 0x04003B4D RID: 15181
		private static readonly PetsSynchronizer s_instance = new PetsSynchronizer();
	}
}
