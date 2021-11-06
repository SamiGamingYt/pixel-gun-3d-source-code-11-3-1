using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000847 RID: 2119
	internal sealed class TrophiesSynchronizer
	{
		// Token: 0x06004CF4 RID: 19700 RVA: 0x001BBB20 File Offset: 0x001B9D20
		private TrophiesSynchronizer()
		{
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06004CF6 RID: 19702 RVA: 0x001BBB34 File Offset: 0x001B9D34
		// (remove) Token: 0x06004CF7 RID: 19703 RVA: 0x001BBB50 File Offset: 0x001B9D50
		public event EventHandler Updated;

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004CF8 RID: 19704 RVA: 0x001BBB6C File Offset: 0x001B9D6C
		public static TrophiesSynchronizer Instance
		{
			get
			{
				return TrophiesSynchronizer._instance;
			}
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x001BBB74 File Offset: 0x001B9D74
		public Coroutine Push()
		{
			if (!this.Ready)
			{
				return null;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return CoroutineRunner.Instance.StartCoroutine(this.PushGoogleCoroutine());
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.SyncAmazon();
				}
			}
			return null;
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x001BBBC8 File Offset: 0x001B9DC8
		public Coroutine Sync()
		{
			if (!this.Ready)
			{
				return null;
			}
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
			return null;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x001BBC20 File Offset: 0x001B9E20
		private void SyncAmazon()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				AGSWhispersyncClient.Synchronize();
				using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
				{
					if (gameData == null)
					{
						Debug.LogWarning("dataMap == null");
					}
					else
					{
						using (AGSGameDataMap map = gameData.GetMap("trophiesMap"))
						{
							if (map == null)
							{
								Debug.LogWarning("trophiesMap == null");
								return;
							}
							AGSSyncableNumber highestNumber = map.GetHighestNumber("trophiesNegative");
							AGSSyncableNumber highestNumber2 = map.GetHighestNumber("trophiesPositive");
							AGSSyncableNumber highestNumber3 = map.GetHighestNumber("season");
							int num = (highestNumber == null) ? 0 : highestNumber.AsInt();
							int num2 = (highestNumber2 == null) ? 0 : highestNumber2.AsInt();
							int num3 = (highestNumber3 == null) ? 0 : highestNumber3.AsInt();
							int negativeRating = RatingSystem.instance.negativeRating;
							int positiveRating = RatingSystem.instance.positiveRating;
							int num4 = positiveRating - negativeRating;
							int currentCompetition = FriendsController.sharedController.currentCompetition;
							bool flag = false;
							if (num3 == 0)
							{
								if (RatingSystem.instance.currentLeague != RatingSystem.RatingLeague.Adamant)
								{
									int num5 = negativeRating;
									if (num > negativeRating)
									{
										num5 = num;
										RatingSystem.instance.negativeRating = num5;
										flag = true;
									}
									int num6 = positiveRating;
									if (num2 > positiveRating)
									{
										num6 = num2;
										RatingSystem.instance.positiveRating = num6;
										flag = true;
									}
									int num7 = num6 - num5;
									int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
									if (num7 > trophiesSeasonThreshold)
									{
										int num8 = num7 - trophiesSeasonThreshold;
										num5 += num8;
										RatingSystem.instance.negativeRating = num5;
										flag = true;
										TournamentAvailableBannerWindow.CanShow = true;
									}
								}
							}
							else if (num3 > currentCompetition)
							{
								FriendsController.sharedController.currentCompetition = num3;
								RatingSystem.instance.negativeRating = num;
								RatingSystem.instance.positiveRating = num2;
								flag = true;
							}
							else if (num3 == currentCompetition)
							{
								if (num > negativeRating)
								{
									int negativeRating2 = num;
									RatingSystem.instance.negativeRating = negativeRating2;
									flag = true;
								}
								if (num2 > positiveRating)
								{
									int positiveRating2 = num2;
									RatingSystem.instance.positiveRating = positiveRating2;
									flag = true;
								}
							}
							EventHandler updated = this.Updated;
							if (flag && updated != null)
							{
								updated(this, EventArgs.Empty);
							}
							bool flag2 = true;
							if (flag2)
							{
								highestNumber.Set(RatingSystem.instance.negativeRating);
								highestNumber2.Set(RatingSystem.instance.positiveRating);
								highestNumber3.Set(FriendsController.sharedController.currentCompetition);
							}
						}
						AGSWhispersyncClient.Synchronize();
					}
				}
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x001BBF3C File Offset: 0x001BA13C
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			if (!this.Ready)
			{
				yield break;
			}
			string thisName = string.Format(CultureInfo.InvariantCulture, "TrophiesSynchronizer.SyncGoogleCoroutine('{0}')", new object[]
			{
				(!pullOnly) ? "sync" : "pull"
			});
			using (new ScopeLogger(thisName, Defs.IsDeveloperBuild && !Application.isEditor))
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
				TrophiesSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(TrophiesSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				for (;;)
				{
					string callee = string.Format(CultureInfo.InvariantCulture, "Pull and wait ({0})", new object[]
					{
						i
					});
					using (ScopeLogger logger = new ScopeLogger(thisName, callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<TrophiesMemento>> future = googleSavedGamesFacade.Pull();
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
							Debug.LogWarning("Failed to pull trophies with exception: " + ex.Message);
							yield return delay;
						}
						else
						{
							SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
							if (requestStatus != SavedGameRequestStatus.Success)
							{
								Debug.LogWarning("Failed to push trophies with status: " + requestStatus);
								yield return delay;
							}
							else
							{
								TrophiesMemento cloudTrophies = future.Result.Value;
								int localTrophiesNegative = RatingSystem.instance.negativeRating;
								int localTrophiesPositive = RatingSystem.instance.positiveRating;
								int localTrophies = localTrophiesPositive - localTrophiesNegative;
								bool cloudDirty = true;
								int localSeason = FriendsController.sharedController.currentCompetition;
								bool localDirty = false;
								if (cloudTrophies.Season == 0)
								{
									if (RatingSystem.instance.currentLeague != RatingSystem.RatingLeague.Adamant)
									{
										int newLocalTrophiesNegative = localTrophiesNegative;
										if (cloudTrophies.TrophiesNegative > localTrophiesNegative)
										{
											newLocalTrophiesNegative = cloudTrophies.TrophiesNegative;
											RatingSystem.instance.negativeRating = newLocalTrophiesNegative;
											localDirty = true;
										}
										int newLocalTrophiesPositive = localTrophiesPositive;
										if (cloudTrophies.TrophiesPositive > localTrophiesPositive)
										{
											newLocalTrophiesPositive = cloudTrophies.TrophiesPositive;
											RatingSystem.instance.positiveRating = newLocalTrophiesPositive;
											localDirty = true;
										}
										int newLocalTrophies = newLocalTrophiesPositive - newLocalTrophiesNegative;
										int threshold = RatingSystem.instance.TrophiesSeasonThreshold;
										if (newLocalTrophies > threshold)
										{
											int compensate = newLocalTrophies - threshold;
											newLocalTrophiesNegative += compensate;
											RatingSystem.instance.negativeRating = newLocalTrophiesNegative;
											localDirty = true;
											TournamentAvailableBannerWindow.CanShow = true;
										}
									}
								}
								else if (cloudTrophies.Season > localSeason)
								{
									FriendsController.sharedController.currentCompetition = cloudTrophies.Season;
									RatingSystem.instance.negativeRating = cloudTrophies.TrophiesNegative;
									RatingSystem.instance.positiveRating = cloudTrophies.TrophiesPositive;
									localDirty = true;
								}
								else if (cloudTrophies.Season == localSeason)
								{
									int newLocalTrophiesNegative2 = localTrophiesNegative;
									if (cloudTrophies.TrophiesNegative > localTrophiesNegative)
									{
										newLocalTrophiesNegative2 = cloudTrophies.TrophiesNegative;
										RatingSystem.instance.negativeRating = newLocalTrophiesNegative2;
										localDirty = true;
									}
									int newLocalTrophiesPositive2 = localTrophiesPositive;
									if (cloudTrophies.TrophiesPositive > localTrophiesPositive)
									{
										newLocalTrophiesPositive2 = cloudTrophies.TrophiesPositive;
										RatingSystem.instance.positiveRating = newLocalTrophiesPositive2;
										localDirty = true;
									}
								}
								EventHandler handler = this.Updated;
								if (localDirty && handler != null)
								{
									handler(this, EventArgs.Empty);
								}
								if (pullOnly)
								{
									yield break;
								}
								if (cloudTrophies.Conflicted || cloudDirty)
								{
									using (new ScopeLogger("TrophiesSynchronizer.PullGoogleCoroutine()", "PushGoogleCoroutine(conflict)", Defs.IsDeveloperBuild && !Application.isEditor))
									{
										IEnumerator enumerator = this.PushGoogleCoroutine();
										while (enumerator.MoveNext())
										{
											yield return null;
										}
									}
								}
								yield break;
							}
						}
					}
					i++;
				}
			}
			yield break;
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x001BBF68 File Offset: 0x001BA168
		private IEnumerator PushGoogleCoroutine()
		{
			if (!this.Ready)
			{
				yield break;
			}
			using (new ScopeLogger("TrophiesSynchronizer.PushGoogleCoroutine()", Defs.IsDeveloperBuild && !Application.isEditor))
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
				TrophiesSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(TrophiesSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				for (;;)
				{
					int trophiesNegative = RatingSystem.instance.negativeRating;
					int trophiesPositive = RatingSystem.instance.positiveRating;
					int localSeason = FriendsController.sharedController.currentCompetition;
					TrophiesMemento localTrophies = new TrophiesMemento(trophiesNegative, trophiesPositive, localSeason);
					string callee = string.Format(CultureInfo.InvariantCulture, "Push and wait {0} ({1})", new object[]
					{
						localTrophies,
						i
					});
					using (ScopeLogger logger = new ScopeLogger("TrophiesSynchronizer.PushGoogleCoroutine()", callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localTrophies);
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
							Debug.LogWarning("Failed to push trophies with exception: " + ex.Message);
							yield return delay;
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
									Debug.LogFormat("[Trophies] Succeeded to push trophies with status: '{0}'", new object[]
									{
										description
									});
								}
								yield break;
							}
							Debug.LogWarning("Failed to push trophies with status: " + requestStatus);
							yield return delay;
						}
					}
					i++;
				}
			}
			yield break;
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004CFE RID: 19710 RVA: 0x001BBF84 File Offset: 0x001BA184
		private bool Ready
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04003B76 RID: 15222
		private static readonly TrophiesSynchronizer _instance = new TrophiesSynchronizer();
	}
}
