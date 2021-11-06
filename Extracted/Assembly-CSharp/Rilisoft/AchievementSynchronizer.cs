using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000829 RID: 2089
	internal sealed class AchievementSynchronizer
	{
		// Token: 0x06004BF2 RID: 19442 RVA: 0x001B585C File Offset: 0x001B3A5C
		private AchievementSynchronizer()
		{
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x001B5880 File Offset: 0x001B3A80
		public static AchievementSynchronizer Instance
		{
			get
			{
				return AchievementSynchronizer.s_instance;
			}
		}

		// Token: 0x06004BF5 RID: 19445 RVA: 0x001B5888 File Offset: 0x001B3A88
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
				this.SyncIos();
			}
			return null;
		}

		// Token: 0x06004BF6 RID: 19446 RVA: 0x001B58E4 File Offset: 0x001B3AE4
		private IEnumerator PushGoogleCoroutine()
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PushGoogleCoroutine()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(thisMethod, false))
			{
				if (!Application.isEditor && GpgFacade.Instance.SavedGame == null)
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("{0}: Waiting while `SavedGame == null`...", new object[]
						{
							base.GetType().Name
						});
					}
					while (GpgFacade.Instance.SavedGame == null)
					{
						yield return null;
					}
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("{0}: `SavedGame != null`...", new object[]
						{
							base.GetType().Name
						});
					}
				}
				AchievementSynchronizerGpgFacade googleSavedGamesFacade = default(AchievementSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					AchievementProgressSyncObject localData = this.ReadLocalData();
					Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localData);
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarningFormat("[{0}] Failed to push with exception: '{1}'", new object[]
						{
							base.GetType().Name,
							ex.Message
						});
						yield return AchievementSynchronizer.s_delay;
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
								Debug.LogFormat("[{0}] Succeeded to push: '{1}'", new object[]
								{
									base.GetType().Name,
									description
								});
							}
							yield break;
						}
						Debug.LogWarningFormat("[{0}] Failed to push with status: '{1}'", new object[]
						{
							base.GetType().Name,
							requestStatus
						});
						yield return AchievementSynchronizer.s_delay;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004BF7 RID: 19447 RVA: 0x001B5900 File Offset: 0x001B3B00
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.SyncGoogleCoroutine('{1}')", new object[]
			{
				base.GetType().Name,
				(!pullOnly) ? "sync" : "pull"
			});
			using (new ScopeLogger(thisMethod, false))
			{
				if (!Application.isEditor && GpgFacade.Instance.SavedGame == null)
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("{0}: Waiting while `SavedGame == null`...", new object[]
						{
							base.GetType().Name
						});
					}
					while (GpgFacade.Instance.SavedGame == null)
					{
						yield return null;
					}
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("{0}: `SavedGame != null`...", new object[]
						{
							base.GetType().Name
						});
					}
				}
				AchievementSynchronizerGpgFacade googleSavedGamesFacade = default(AchievementSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					Task<GoogleSavedGameRequestResult<AchievementProgressSyncObject>> future = googleSavedGamesFacade.Pull();
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarningFormat("[{0}] Failed to pull with exception: '{1}'", new object[]
						{
							base.GetType().Name,
							ex.Message
						});
						yield return AchievementSynchronizer.s_delay;
					}
					else
					{
						SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
						if (requestStatus == SavedGameRequestStatus.Success)
						{
							AchievementProgressSyncObject localData = this.ReadLocalData();
							AchievementProgressSyncObject cloudData = future.Result.Value;
							AchievementProgressSyncObject mergedData = AchievementProgressData.Merge(localData, cloudData);
							if (Defs.IsDeveloperBuild)
							{
								string format = "Local progress: {0}";
								object[] array = new object[1];
								array[0] = Json.Serialize((from p in localData.ProgressData
								select p.AchievementId).ToList<int>());
								Debug.LogFormat(format, array);
								string format2 = "Cloud progress: {0}";
								object[] array2 = new object[1];
								array2[0] = Json.Serialize((from p in cloudData.ProgressData
								select p.AchievementId).ToList<int>());
								Debug.LogFormat(format2, array2);
								string format3 = "Merged progress: {0}";
								object[] array3 = new object[1];
								array3[0] = Json.Serialize((from p in mergedData.ProgressData
								select p.AchievementId).ToList<int>());
								Debug.LogFormat(format3, array3);
							}
							bool localDirty = !localData.Equals(mergedData);
							if (localDirty)
							{
								this.SaveLocalData(mergedData);
							}
							if (!cloudData.Equals(mergedData) || cloudData.Conflicted)
							{
								IEnumerator enumerator = this.PushGoogleCoroutine();
								while (enumerator.MoveNext())
								{
									yield return null;
								}
							}
							yield break;
						}
						Debug.LogWarningFormat("[{0}] Failed to pull with status: '{1}'", new object[]
						{
							base.GetType().Name,
							requestStatus
						});
						yield return AchievementSynchronizer.s_delay;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004BF8 RID: 19448 RVA: 0x001B592C File Offset: 0x001B3B2C
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
							using (AGSSyncableString latestString = gameData.GetLatestString("achievementsJson"))
							{
								this.EnsureNotNull(latestString, "achievementsJson");
								string json = latestString.GetValue() ?? "{}";
								AchievementProgressSyncObject achievementProgressSyncObject = this.ReadLocalData();
								AchievementProgressSyncObject achievementProgressSyncObject2 = AchievementProgressSyncObject.FromJson(json);
								AchievementProgressSyncObject achievementProgressSyncObject3 = AchievementProgressData.Merge(achievementProgressSyncObject, achievementProgressSyncObject2);
								bool flag = !achievementProgressSyncObject.Equals(achievementProgressSyncObject3);
								if (flag)
								{
									this.SaveLocalData(achievementProgressSyncObject3);
								}
								if (!achievementProgressSyncObject2.Equals(achievementProgressSyncObject3) || achievementProgressSyncObject2.Conflicted)
								{
									string val = AchievementProgressSyncObject.ToJson(achievementProgressSyncObject3);
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

		// Token: 0x06004BF9 RID: 19449 RVA: 0x001B5ACC File Offset: 0x001B3CCC
		private void SyncIos()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.ICloudAvailable)
			{
			}
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x001B5AE4 File Offset: 0x001B3CE4
		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x001B5B00 File Offset: 0x001B3D00
		private AchievementProgressSyncObject ReadLocalData()
		{
			List<AchievementProgressData> progressData = AchievementsManager.ReadLocalProgress();
			return new AchievementProgressSyncObject(progressData);
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x001B5B1C File Offset: 0x001B3D1C
		internal void SaveLocalData(AchievementProgressSyncObject achievementMemento)
		{
			if (Singleton<AchievementsManager>.Instance != null)
			{
				foreach (AchievementProgressData progress in achievementMemento.ProgressData)
				{
					Singleton<AchievementsManager>.Instance.SetProgress(progress);
				}
				Singleton<AchievementsManager>.Instance.SaveProgresses();
			}
		}

		// Token: 0x04003B1F RID: 15135
		private const int AttemptCountMax = 3;

		// Token: 0x04003B20 RID: 15136
		private static readonly WaitForRealSeconds s_delay = new WaitForRealSeconds(30f);

		// Token: 0x04003B21 RID: 15137
		private static readonly AchievementSynchronizer s_instance = new AchievementSynchronizer();
	}
}
