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
	// Token: 0x0200082F RID: 2095
	internal sealed class CampaignProgressSynchronizer
	{
		// Token: 0x06004C20 RID: 19488 RVA: 0x001B6B38 File Offset: 0x001B4D38
		private CampaignProgressSynchronizer()
		{
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x06004C22 RID: 19490 RVA: 0x001B6B5C File Offset: 0x001B4D5C
		public static CampaignProgressSynchronizer Instance
		{
			get
			{
				return CampaignProgressSynchronizer.s_instance;
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x001B6B64 File Offset: 0x001B4D64
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
				this.SyncCampaignBonusesIos();
				return null;
			}
			return null;
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x001B6BC0 File Offset: 0x001B4DC0
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
							using (AGSGameDataMap map = gameData.GetMap("campaignProgressMap"))
							{
								this.EnsureNotNull(map, "campaignProgressMap");
								CampaignProgressMemento campaignProgressMemento = this.LoadMemento();
								Dictionary<string, LevelProgressMemento> levelsAsDictionary = campaignProgressMemento.GetLevelsAsDictionary();
								CampaignProgressMemento campaignProgressMemento2 = default(CampaignProgressMemento);
								using (AGSGameDataMap map2 = map.GetMap("levels"))
								{
									this.EnsureNotNull(map2, "levelsMap");
									HashSet<string> mapKeys = map2.GetMapKeys();
									mapKeys.UnionWith(levelsAsDictionary.Keys);
									foreach (string text in mapKeys)
									{
										AGSGameDataMap map3 = map2.GetMap(text);
										if (map3 != null)
										{
											LevelProgressMemento levelProgressMemento = new LevelProgressMemento(text);
											AGSSyncableNumber highestNumber = map3.GetHighestNumber("coinCount");
											levelProgressMemento.CoinCount = ((highestNumber == null) ? 0 : highestNumber.AsInt());
											AGSSyncableNumber highestNumber2 = map3.GetHighestNumber("gemCount");
											levelProgressMemento.GemCount = ((highestNumber2 == null) ? 0 : highestNumber2.AsInt());
											campaignProgressMemento2.Levels.Add(levelProgressMemento);
											LevelProgressMemento levelProgressMemento2;
											if (levelsAsDictionary.TryGetValue(text, out levelProgressMemento2))
											{
												if (highestNumber != null && levelProgressMemento.CoinCount < levelProgressMemento2.CoinCount)
												{
													highestNumber.Set(levelProgressMemento2.CoinCount);
													highestNumber.Dispose();
												}
												if (highestNumber2 != null && levelProgressMemento.GemCount < levelProgressMemento2.GemCount)
												{
													highestNumber2.Set(levelProgressMemento2.GemCount);
													highestNumber2.Dispose();
												}
											}
										}
									}
								}
								CampaignProgressMemento campaignProgressMemento3 = CampaignProgressMemento.Merge(campaignProgressMemento, campaignProgressMemento2);
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("Local campaign progress: {0}", new object[]
									{
										campaignProgressMemento
									});
									Debug.LogFormat("Cloud campaign progress: {0}", new object[]
									{
										campaignProgressMemento2
									});
									Debug.LogFormat("Merged campaign progress: {0}", new object[]
									{
										campaignProgressMemento3
									});
								}
								this.OverwriteMemento(campaignProgressMemento3);
							}
							AGSWhispersyncClient.Synchronize();
						}
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x001B6EFC File Offset: 0x001B50FC
		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x001B6F18 File Offset: 0x001B5118
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.SyncGoogleCoroutine('{1}')", new object[]
			{
				base.GetType().Name,
				(!pullOnly) ? "sync" : "pull"
			});
			using (new ScopeLogger(thisMethod, false))
			{
				CampaignProgressSynchronizerGpgFacade googleSavedGamesFacade = default(CampaignProgressSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					Task<GoogleSavedGameRequestResult<CampaignProgressMemento>> future = googleSavedGamesFacade.Pull();
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarning("Failed to pull campaign progress with exception: " + ex.Message);
						yield return CampaignProgressSynchronizer.s_delay;
					}
					else
					{
						SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
						if (requestStatus != SavedGameRequestStatus.Success)
						{
							Debug.LogWarning("Failed to pull campaign progress with status: " + requestStatus);
							yield return CampaignProgressSynchronizer.s_delay;
						}
						else
						{
							CampaignProgressMemento localCampaignProgress = this.LoadMemento();
							CampaignProgressMemento cloudCampaignProgress = future.Result.Value;
							CampaignProgressMemento mergedCampaignProgress = CampaignProgressMemento.Merge(localCampaignProgress, cloudCampaignProgress);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Local campaign progress: {0}", new object[]
								{
									JsonUtility.ToJson(localCampaignProgress)
								});
								Debug.LogFormat("Cloud campaign progress: {0}", new object[]
								{
									JsonUtility.ToJson(cloudCampaignProgress)
								});
								Debug.LogFormat("Merged campaign progress: {0}", new object[]
								{
									JsonUtility.ToJson(mergedCampaignProgress)
								});
							}
							Dictionary<string, LevelProgressMemento> mergedLevels = mergedCampaignProgress.GetLevelsAsDictionary();
							Func<LevelProgressMemento, bool> dirtyCondition = (LevelProgressMemento level) => level.CoinCount < mergedLevels[level.LevelId].CoinCount || level.GemCount < mergedLevels[level.LevelId].GemCount;
							bool localDirty = localCampaignProgress.Levels.Count < mergedCampaignProgress.Levels.Count || localCampaignProgress.Levels.Any(dirtyCondition);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Local progress is dirty: {0}", new object[]
								{
									localDirty
								});
							}
							if (localDirty)
							{
								this.OverwriteMemento(mergedCampaignProgress);
							}
							if (pullOnly)
							{
								yield break;
							}
							bool cloudDirty = cloudCampaignProgress.Levels.Count < mergedCampaignProgress.Levels.Count || cloudCampaignProgress.Levels.Any(dirtyCondition);
							if (Defs.IsDeveloperBuild)
							{
								Debug.LogFormat("Cloud progress is dirty: {0}, conflicted: {1}", new object[]
								{
									cloudDirty,
									cloudCampaignProgress.Conflicted
								});
							}
							if (cloudDirty || cloudCampaignProgress.Conflicted)
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

		// Token: 0x06004C27 RID: 19495 RVA: 0x001B6F44 File Offset: 0x001B5144
		private IEnumerator PushGoogleCoroutine()
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PushGoogleCoroutine()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(thisMethod, false))
			{
				CampaignProgressSynchronizerGpgFacade googleSavedGamesFacade = default(CampaignProgressSynchronizerGpgFacade);
				for (int i = 0; i != 3; i++)
				{
					CampaignProgressMemento localCampaignProgress = this.LoadMemento();
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Local progress: {0}", new object[]
						{
							JsonUtility.ToJson(localCampaignProgress)
						});
					}
					Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localCampaignProgress);
					while (!future.IsCompleted)
					{
						yield return null;
					}
					if (future.IsFaulted)
					{
						Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
						Debug.LogWarning("Failed to push campaign progress with exception: " + ex.Message);
						yield return CampaignProgressSynchronizer.s_delay;
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
								Debug.LogFormat("[CampaignProgress] Succeeded to push campaign progress {0}: '{1}'", new object[]
								{
									localCampaignProgress,
									description
								});
							}
							yield break;
						}
						Debug.LogWarning("Failed to push campaign progress with status: " + requestStatus);
						yield return CampaignProgressSynchronizer.s_delay;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x001B6F60 File Offset: 0x001B5160
		internal CampaignProgressMemento LoadMemento()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.LoadMemento()", new object[]
			{
				base.GetType().Name
			});
			CampaignProgressMemento result;
			using (new ScopeLogger(callee, false))
			{
				Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>();
				string @string = Storager.getString(Defs.LevelsWhereGetCoinS, false);
				string[] array = @string.Split(new char[]
				{
					'#'
				}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string text in array)
				{
					LevelProgressMemento levelProgressMemento;
					if (dictionary.TryGetValue(text, out levelProgressMemento))
					{
						levelProgressMemento.CoinCount = 1;
					}
					else
					{
						levelProgressMemento = new LevelProgressMemento(text)
						{
							CoinCount = 1
						};
						dictionary.Add(text, levelProgressMemento);
					}
				}
				string string2 = Storager.getString(Defs.LevelsWhereGotGems, false);
				List<object> list = Json.Deserialize(string2) as List<object>;
				List<string> list2 = (list == null) ? new List<string>() : list.OfType<string>().ToList<string>();
				foreach (string text2 in list2)
				{
					LevelProgressMemento levelProgressMemento2;
					if (dictionary.TryGetValue(text2, out levelProgressMemento2))
					{
						levelProgressMemento2.GemCount = 1;
					}
					else
					{
						levelProgressMemento2 = new LevelProgressMemento(text2)
						{
							GemCount = 1
						};
						dictionary.Add(text2, levelProgressMemento2);
					}
				}
				CampaignProgressMemento campaignProgressMemento = default(CampaignProgressMemento);
				campaignProgressMemento.Levels.AddRange(dictionary.Values);
				result = campaignProgressMemento;
			}
			return result;
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x001B7130 File Offset: 0x001B5330
		internal void OverwriteMemento(CampaignProgressMemento campaignProgressMemento)
		{
			string[] value = (from l in campaignProgressMemento.Levels
			where l.CoinCount > 0
			select l.LevelId).ToArray<string>();
			string val = string.Join("#", value);
			Storager.setString(Defs.LevelsWhereGetCoinS, val, false);
			List<string> obj = (from l in campaignProgressMemento.Levels
			where l.GemCount > 0
			select l.LevelId).ToList<string>();
			string val2 = Json.Serialize(obj);
			Storager.setString(Defs.LevelsWhereGotGems, val2, false);
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x001B720C File Offset: 0x001B540C
		private void SyncCampaignBonusesIos()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.ICloudAvailable)
			{
			}
		}

		// Token: 0x04003B2D RID: 15149
		private const int AttemptCountMax = 3;

		// Token: 0x04003B2E RID: 15150
		private static readonly WaitForRealSeconds s_delay = new WaitForRealSeconds(30f);

		// Token: 0x04003B2F RID: 15151
		private static readonly CampaignProgressSynchronizer s_instance = new CampaignProgressSynchronizer();
	}
}
