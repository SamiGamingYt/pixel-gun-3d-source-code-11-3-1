using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000710 RID: 1808
	internal sealed class ProgressSynchronizer
	{
		// Token: 0x06003F21 RID: 16161 RVA: 0x00151C98 File Offset: 0x0014FE98
		public void SynchronizeIosProgress()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.ICloudAvailable)
			{
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00151CB0 File Offset: 0x0014FEB0
		public void SynchronizeAmazonProgress()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarning("SynchronizeAmazonProgress() is not implemented for current target.");
				return;
			}
			AGSWhispersyncClient.Synchronize();
			using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
			{
				if (gameData == null)
				{
					Debug.LogWarning("dataMap == null");
				}
				else
				{
					using (AGSGameDataMap map = gameData.GetMap("progressMap"))
					{
						if (map == null)
						{
							Debug.LogWarning("syncableProgressMap == null");
						}
						else
						{
							string[] array = (from k in map.GetMapKeys()
							where !string.IsNullOrEmpty(k)
							select k).ToArray<string>();
							string message = string.Format("Trying to sync progress.    Local: {0}    Cloud keys: {1}", CampaignProgress.GetCampaignProgressString(), Json.Serialize(array));
							Debug.Log(message);
							foreach (string text in array)
							{
								Dictionary<string, int> dictionary;
								if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(text, out dictionary))
								{
									Debug.LogWarning("boxesLevelsAndStars doesn't contain “" + text + "”");
									dictionary = new Dictionary<string, int>();
									CampaignProgress.boxesLevelsAndStars.Add(text, dictionary);
								}
								else if (dictionary == null)
								{
									Debug.LogWarning("localBox == null");
									dictionary = new Dictionary<string, int>();
									CampaignProgress.boxesLevelsAndStars[text] = dictionary;
								}
								using (AGSGameDataMap map2 = map.GetMap(text))
								{
									if (map2 == null)
									{
										Debug.LogWarning("boxMap == null");
									}
									else
									{
										string[] array3 = map2.GetHighestNumberKeys().ToArray<string>();
										string message2 = string.Format("“{0}” levels: {1}", text, Json.Serialize(array3));
										Debug.Log(message2);
										foreach (string text2 in array3)
										{
											using (AGSSyncableNumber highestNumber = map2.GetHighestNumber(text2))
											{
												if (highestNumber == null)
												{
													Debug.LogWarning("syncableCloudValue == null");
												}
												else
												{
													if (Debug.isDebugBuild)
													{
														Debug.Log("Synchronizing from cloud “" + text2 + "”...");
													}
													int num = highestNumber.AsInt();
													int val = 0;
													if (dictionary.TryGetValue(text2, out val))
													{
														dictionary[text2] = Math.Max(val, num);
													}
													else
													{
														dictionary.Add(text2, num);
													}
													if (Debug.isDebugBuild)
													{
														Debug.Log("Synchronized from cloud “" + text2 + "”...");
													}
												}
											}
										}
									}
								}
							}
							CampaignProgress.OpenNewBoxIfPossible();
							CampaignProgress.ActualizeComicsViews();
							WeaponManager.ActualizeWeaponsForCampaignProgress();
							Debug.Log("Trying to sync progress.    Merged: " + CampaignProgress.GetCampaignProgressString());
							foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in CampaignProgress.boxesLevelsAndStars)
							{
								if (Debug.isDebugBuild)
								{
									string message3 = string.Format("Synchronizing to cloud: “{0}”", keyValuePair);
									Debug.Log(message3);
								}
								using (AGSGameDataMap map3 = map.GetMap(keyValuePair.Key))
								{
									if (map3 == null)
									{
										Debug.LogWarning("boxMap == null");
									}
									else
									{
										Dictionary<string, int> dictionary2 = keyValuePair.Value ?? new Dictionary<string, int>();
										foreach (KeyValuePair<string, int> keyValuePair2 in dictionary2)
										{
											using (AGSSyncableNumber highestNumber2 = map3.GetHighestNumber(keyValuePair2.Key))
											{
												if (highestNumber2 == null)
												{
													Debug.LogWarning("syncableCloudValue == null");
												}
												else
												{
													highestNumber2.Set(keyValuePair2.Value);
												}
											}
										}
									}
								}
							}
							AGSWhispersyncClient.Synchronize();
						}
					}
				}
			}
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x00152164 File Offset: 0x00150364
		public void AuthenticateAndSynchronize(Action callback, bool silent)
		{
			if (GpgFacade.Instance.IsAuthenticated())
			{
				Debug.LogFormat("Already authenticated: {0}, {1}, {2}", new object[]
				{
					Social.localUser.id,
					Social.localUser.userName,
					Social.localUser.state
				});
				this.SynchronizeIfAuthenticated(callback);
			}
			else
			{
				Action<bool> callback2 = delegate(bool succeeded)
				{
					bool value = !silent && !succeeded;
					PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(value));
					if (succeeded)
					{
						string message = string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
						Debug.Log(message);
						this.SynchronizeIfAuthenticated(callback);
					}
					else if (!Application.isEditor)
					{
						Debug.LogWarning("Authentication failed.");
					}
				};
				GpgFacade.Instance.Authenticate(callback2, silent);
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x00152208 File Offset: 0x00150408
		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action callback)
		{
			if (GpgFacade.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				return;
			}
			List<string> list = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = delegate(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				Debug.LogFormat("****** Open '{0}': {1} '{2}'", new object[]
				{
					"Progress",
					openStatus,
					openMetadata.GetDescription()
				});
				if (openStatus != SavedGameRequestStatus.Success)
				{
					return;
				}
				Debug.LogFormat("****** Trying to read '{0}' '{1}'...", new object[]
				{
					"Progress",
					openMetadata.GetDescription()
				});
				GpgFacade.Instance.SavedGame.ReadBinaryData(openMetadata, delegate(SavedGameRequestStatus readStatus, byte[] data)
				{
					string @string = Encoding.UTF8.GetString(data ?? new byte[0]);
					Debug.Log(string.Format("****** Read '{0}': {1} '{2}'    '{3}'", new object[]
					{
						"Progress",
						readStatus,
						openMetadata.GetDescription(),
						@string
					}));
					if (readStatus != SavedGameRequestStatus.Success)
					{
						return;
					}
					Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(@string);
					if (dictionary == null)
					{
						Debug.LogWarning("serverProgress == null");
						return;
					}
					HashSet<string> hashSet = new HashSet<string>(CampaignProgress.boxesLevelsAndStars.SelectMany((KeyValuePair<string, Dictionary<string, int>> kv) => kv.Value.Keys));
					hashSet.ExceptWith(dictionary.SelectMany((KeyValuePair<string, Dictionary<string, int>> kv) => kv.Value.Keys));
					string text = Json.Serialize(hashSet.ToArray<string>());
					ProgressSynchronizer.MergeUpdateLocalProgress(dictionary);
					CampaignProgress.ActualizeComicsViews();
					WeaponManager.ActualizeWeaponsForCampaignProgress();
					string outgoingProgressString = CampaignProgress.GetCampaignProgressString();
					Debug.Log(string.Format("****** Trying to write '{0}': '{1}'...", "Progress", outgoingProgressString));
					byte[] bytes = Encoding.UTF8.GetBytes(outgoingProgressString);
					string description = string.Format("Added levels by '{0}': {1}", SystemInfo.deviceModel, text.Substring(0, Math.Min(32, text.Length)));
					SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
					GpgFacade.Instance.SavedGame.CommitUpdate(openMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
					{
						Debug.Log(string.Format("****** Written '{0}': {1} '{2}'    '{3}'", new object[]
						{
							"Progress",
							writeStatus,
							closeMetadata.GetDescription(),
							outgoingProgressString
						}));
						callback();
					});
				});
			};
			ConflictCallback conflictCallback = delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				string @string = Encoding.UTF8.GetString(originalData);
				string string2 = Encoding.UTF8.GetString(unmergedData);
				ISavedGameMetadata savedGameMetadata;
				if (@string.Length > string2.Length)
				{
					savedGameMetadata = original;
					resolver.ChooseMetadata(savedGameMetadata);
					Debug.Log(string.Format("****** Partially resolved using original metadata '{0}': '{1}'", "Progress", original.GetDescription()));
				}
				else
				{
					savedGameMetadata = unmerged;
					resolver.ChooseMetadata(savedGameMetadata);
					Debug.Log(string.Format("****** Partially resolved using unmerged metadata '{0}': '{1}'", "Progress", unmerged.GetDescription()));
				}
				string mergedString = DictionaryLoadedListener.MergeProgress(@string, string2);
				Dictionary<string, Dictionary<string, int>> dictionary = CampaignProgress.DeserializeProgress(mergedString);
				if (dictionary == null)
				{
					Debug.LogWarning("mergedProgress == null");
					return;
				}
				ProgressSynchronizer.MergeUpdateLocalProgress(dictionary);
				CampaignProgress.ActualizeComicsViews();
				WeaponManager.ActualizeWeaponsForCampaignProgress();
				string description = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
				SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
				byte[] bytes = Encoding.UTF8.GetBytes(mergedString);
				GpgFacade.Instance.SavedGame.CommitUpdate(savedGameMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
				{
					Debug.LogFormat("****** Written '{0}': {1} '{2}'    '{3}'", new object[]
					{
						"Progress",
						writeStatus,
						closeMetadata.GetDescription(),
						mergedString
					});
					callback();
				});
			};
			Debug.LogFormat("****** Trying to open '{0}'...", new object[]
			{
				"Progress"
			});
			GpgFacade.Instance.SavedGame.OpenWithManualConflictResolution("Progress", DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
		}

		// Token: 0x06003F25 RID: 16165 RVA: 0x0015228C File Offset: 0x0015048C
		private static void MergeUpdateLocalProgress(IDictionary<string, Dictionary<string, int>> incomingProgress)
		{
			foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in incomingProgress)
			{
				Dictionary<string, int> dictionary;
				if (CampaignProgress.boxesLevelsAndStars.TryGetValue(keyValuePair.Key, out dictionary))
				{
					foreach (KeyValuePair<string, int> keyValuePair2 in keyValuePair.Value)
					{
						int val;
						if (dictionary.TryGetValue(keyValuePair2.Key, out val))
						{
							dictionary[keyValuePair2.Key] = Math.Max(val, keyValuePair2.Value);
						}
						else
						{
							dictionary.Add(keyValuePair2.Key, keyValuePair2.Value);
						}
					}
				}
				else
				{
					CampaignProgress.boxesLevelsAndStars.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			CampaignProgress.OpenNewBoxIfPossible();
		}

		// Token: 0x06003F26 RID: 16166 RVA: 0x001523B8 File Offset: 0x001505B8
		public void SynchronizeIfAuthenticated(Action callback)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				return;
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			using (new StopwatchLogger("SynchronizeIfAuthenticated(...)"))
			{
				this.SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x00152428 File Offset: 0x00150628
		public static ProgressSynchronizer Instance
		{
			get
			{
				if (ProgressSynchronizer._instance == null)
				{
					ProgressSynchronizer._instance = new ProgressSynchronizer();
				}
				return ProgressSynchronizer._instance;
			}
		}

		// Token: 0x04002E84 RID: 11908
		public const string Filename = "Progress";

		// Token: 0x04002E85 RID: 11909
		private static ProgressSynchronizer _instance;
	}
}
