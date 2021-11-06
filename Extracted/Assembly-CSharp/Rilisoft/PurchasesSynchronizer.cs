using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000717 RID: 1815
	internal sealed class PurchasesSynchronizer
	{
		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06003F46 RID: 16198 RVA: 0x001529C4 File Offset: 0x00150BC4
		// (remove) Token: 0x06003F47 RID: 16199 RVA: 0x001529E0 File Offset: 0x00150BE0
		public event EventHandler<PurchasesSavingEventArgs> PurchasesSavingStarted;

		// Token: 0x06003F48 RID: 16200 RVA: 0x001529FC File Offset: 0x00150BFC
		public static IEnumerable<string> AllItemIds()
		{
			if (PurchasesSynchronizer._allItemIds == null)
			{
				Dictionary<string, string>.ValueCollection values = WeaponManager.storeIDtoDefsSNMapping.Values;
				List<string> list = new List<string>();
				foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
				{
					foreach (List<string> collection in keyValuePair.Value)
					{
						list.AddRange(collection);
					}
				}
				Dictionary<string, string>.ValueCollection values2 = SkinsController.shopKeyFromNameSkin.Values;
				IEnumerable<string> second = from i in Enumerable.Range(1, 31)
				select "currentLevel" + i;
				string[] second2 = new string[]
				{
					Defs.SkinsMakerInProfileBought,
					Defs.hungerGamesPurchasedKey,
					Defs.CaptureFlagPurchasedKey,
					Defs.smallAsAntKey,
					Defs.code010110_Key,
					Defs.UnderwaterKey
				};
				string[] second3 = new string[]
				{
					"PayingUser"
				};
				string[] second4 = new string[]
				{
					Defs.IsFacebookLoginRewardaGained
				};
				string[] second5 = new string[]
				{
					Defs.IsTwitterLoginRewardaGained
				};
				PurchasesSynchronizer._allItemIds = values.Concat(list).Concat(values2).Concat(second).Concat(second2).Concat(second3).Concat(second4).Concat(second5).Concat(WeaponManager.GotchaGuns).Concat(WeaponSkinsManager.SkinIds).Concat(GadgetsInfo.info.Keys);
			}
			return PurchasesSynchronizer._allItemIds;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x00152BD0 File Offset: 0x00150DD0
		public static IEnumerable<string> GetPurchasesIds()
		{
			IEnumerable<string> source = PurchasesSynchronizer.AllItemIds();
			return from id in source
			where Storager.getInt(id, false) != 0
			select id;
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x00152C08 File Offset: 0x00150E08
		public void SynchronizeAmazonPurchases()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android || Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarning("SynchronizeAmazonPurchases() is not implemented for current target.");
				return;
			}
			if (!AGSClient.IsServiceReady())
			{
				Debug.LogWarning("SynchronizeAmazonPurchases(): service is not ready.");
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
					using (AGSSyncableStringSet stringSet = gameData.GetStringSet("purchases"))
					{
						List<string> list = (from s in stringSet.GetValues()
						select s.GetValue()).ToList<string>();
						Debug.Log("Trying to sync purchases cloud -> local:    " + Json.Serialize(list));
						List<string> list2 = new List<string>();
						foreach (string text in list)
						{
							if (Storager.getInt(text, false) == 0 && (text == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(text)))
							{
								list2.Add(text);
							}
							this._itemsToBeSaved.Add(text);
						}
						string[] array = PurchasesSynchronizer.GetPurchasesIds().ToArray<string>();
						Debug.Log("Trying to sync purchases local -> cloud:    " + Json.Serialize(array));
						foreach (string val in array)
						{
							stringSet.Add(val);
						}
						AGSWhispersyncClient.Synchronize();
						WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list2);
					}
				}
			}
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x00152E08 File Offset: 0x00151008
		public void AuthenticateAndSynchronize(Action<bool> callback, bool silent)
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
				GpgFacade.Instance.Authenticate(delegate(bool succeeded)
				{
					bool value = !silent && !succeeded;
					PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(value));
					if (succeeded)
					{
						string message = string.Format("Authentication succeeded: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
						Debug.Log(message);
						this.SynchronizeIfAuthenticated(callback);
					}
					else
					{
						Debug.LogWarning("Authentication failed.");
					}
				}, silent);
			}
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00152EAC File Offset: 0x001510AC
		private void HandleReadBinaryData(ISavedGameMetadata openMetadata, SavedGameRequestStatus readStatus, byte[] data, Action<bool> callback, List<string> traceContext)
		{
			traceContext.Add(string.Format("ReadBinaryData.Callback >: {0:F3}", Time.realtimeSinceStartup));
			try
			{
				data = (data ?? new byte[0]);
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				if (openMetadata != null)
				{
					Debug.LogFormat("====== Read '{0}' {4:F3}: {1} '{2}'    '{3}'", new object[]
					{
						"Purchases",
						readStatus,
						openMetadata.GetDescription(),
						@string,
						Time.realtimeSinceStartup
					});
				}
				if (readStatus != SavedGameRequestStatus.Success)
				{
					traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback({0}): {1:F3}", readStatus, Time.realtimeSinceStartup));
					try
					{
						callback(false);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					traceContext.Add(string.Format("< OpenWithManualConflictResolution.InnerCallback({0}): {1:F3}", readStatus, Time.realtimeSinceStartup));
				}
				else
				{
					traceContext.Add(string.Format("> Deserializing JSON string, characters {0}: {1:F3}", @string.Length, Time.realtimeSinceStartup));
					List<object> list = (Json.Deserialize(@string) as List<object>) ?? new List<object>();
					IEnumerable<string> enumerable = from i in list.OfType<string>()
					where !string.IsNullOrEmpty(i)
					select i;
					traceContext.Add(string.Format("< Deserializing JSON string, items {0}: {1:F3}", list.Count, Time.realtimeSinceStartup));
					List<string> list2 = new List<string>();
					traceContext.Add(string.Format("> Prepare for saving: {0:F3}", Time.realtimeSinceStartup));
					float num = 0f;
					float num2 = 0f;
					int frameCount = Time.frameCount;
					foreach (string text in enumerable)
					{
						if (text == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(text))
						{
							float realtimeSinceStartup = Time.realtimeSinceStartup;
							int @int = Storager.getInt(text, false);
							num += Time.realtimeSinceStartup - realtimeSinceStartup;
							if (@int == 0)
							{
								list2.Add(text);
							}
						}
						this._itemsToBeSaved.Add(text);
					}
					Debug.LogFormat("Items to be saved: {0}", new object[]
					{
						this._itemsToBeSaved.Count
					});
					traceContext.Add(string.Format("< Prepare for saving (r: {1:F3}, w: {2:F3}): {0:F3}", Time.realtimeSinceStartup, num, num2));
					Storager.RefreshWeaponDigestIfDirty();
					PlayerPrefs.Save();
					WeaponManager.RefreshExpControllers();
					WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list2);
					HashSet<string> hashSet = new HashSet<string>(PurchasesSynchronizer.GetPurchasesIds());
					string outputString = Json.Serialize(hashSet.ToList<string>());
					hashSet.ExceptWith(enumerable);
					string text2 = Json.Serialize(hashSet.ToList<string>());
					Debug.LogFormat("====== Trying to send new items '{0}' {2:F3}: '{1}'...", new object[]
					{
						"Purchases",
						text2,
						Time.realtimeSinceStartup
					});
					if (hashSet.Count == 0)
					{
						Debug.LogFormat("====== Nothing to write '{0}' {1:F3}", new object[]
						{
							"Purchases",
							Time.realtimeSinceStartup
						});
						traceContext.Add(string.Format("> ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
						try
						{
							callback(true);
						}
						catch (Exception exception2)
						{
							Debug.LogException(exception2);
						}
						traceContext.Add(string.Format("< ReadBinaryData.InnerCallback(true): {0:F3}", Time.realtimeSinceStartup));
					}
					else if (openMetadata != null)
					{
						byte[] bytes = Encoding.UTF8.GetBytes(outputString);
						string description = string.Format("Added by '{0}': {1}", SystemInfo.deviceModel, text2.Substring(0, Math.Min(32, text2.Length)));
						traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						GpgFacade.Instance.SavedGame.CommitUpdate(openMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
						{
							Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", new object[]
							{
								"Purchases",
								writeStatus,
								closeMetadata.GetDescription(),
								outputString
							});
							traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
							try
							{
								callback(writeStatus == SavedGameRequestStatus.Success);
							}
							catch (Exception exception3)
							{
								Debug.LogException(exception3);
							}
							finally
							{
								traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
									{
										"Purchases",
										Json.Serialize(traceContext)
									});
								}
							}
						});
						traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					}
				}
			}
			finally
			{
				traceContext.Add(string.Format("ReadBinaryData.Callback <: {0:F3}", Time.realtimeSinceStartup));
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
					{
						"Purchases",
						Json.Serialize(traceContext)
					});
				}
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x00153418 File Offset: 0x00151618
		private void SynchronizeIfAuthenticatedWithSavedGamesService(Action<bool> callback)
		{
			if (GpgFacade.Instance.SavedGame == null)
			{
				Debug.LogWarning("Saved game client is null.");
				callback(false);
			}
			List<string> traceContext = new List<string>();
			Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback = delegate(SavedGameRequestStatus openStatus, ISavedGameMetadata openMetadata)
			{
				traceContext.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback >: {0:F3}", Time.realtimeSinceStartup));
				try
				{
					Debug.LogFormat("====== Open '{0}' {3:F3}: {1} '{2}'", new object[]
					{
						"Purchases",
						openStatus,
						openMetadata.GetDescription(),
						Time.realtimeSinceStartup
					});
					if (openStatus != SavedGameRequestStatus.Success)
					{
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
						callback(false);
						traceContext.Add(string.Format("> OpenWithManualConflictResolution.InnerCallback(openStatus): {0:F3}", Time.realtimeSinceStartup));
					}
					else
					{
						Debug.LogFormat("====== Trying to read '{0}' {2:F3}: '{1}'...", new object[]
						{
							"Purchases",
							openMetadata.GetDescription(),
							Time.realtimeSinceStartup
						});
						traceContext.Add(string.Format("> ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						GpgFacade.Instance.SavedGame.ReadBinaryData(openMetadata, delegate(SavedGameRequestStatus readStatus, byte[] data)
						{
							this.HandleReadBinaryData(openMetadata, readStatus, data, callback, traceContext);
						});
						traceContext.Add(string.Format("< ReadBinaryData: {0:F3}", Time.realtimeSinceStartup));
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
							{
								"Purchases",
								Json.Serialize(traceContext)
							});
						}
					}
				}
				finally
				{
					traceContext.Add(string.Format("OpenWithManualConflictResolution.CompletedCallback <: {0:F3}", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
						{
							"Purchases",
							Json.Serialize(traceContext)
						});
					}
				}
			};
			ConflictCallback conflictCallback = delegate(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData)
			{
				traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} >", Time.realtimeSinceStartup));
				try
				{
					string @string = Encoding.UTF8.GetString(originalData);
					string string2 = Encoding.UTF8.GetString(unmergedData);
					HashSet<string> hashSet = new HashSet<string>(((Json.Deserialize(@string) as List<object>) ?? new List<object>()).Select(new Func<object, string>(Convert.ToString)));
					HashSet<string> hashSet2 = new HashSet<string>(((Json.Deserialize(string2) as List<object>) ?? new List<object>()).Select(new Func<object, string>(Convert.ToString)));
					if (hashSet.IsSupersetOf(hashSet2))
					{
						resolver.ChooseMetadata(original);
						Debug.LogFormat("====== Fully resolved using original metadata '{0}': '{1}'", new object[]
						{
							"Purchases",
							original.GetDescription()
						});
						callback(true);
					}
					else if (hashSet2.IsSupersetOf(hashSet))
					{
						resolver.ChooseMetadata(unmerged);
						Debug.LogFormat("====== Fully resolved using unmerged metadata '{0}': '{1}'", new object[]
						{
							"Purchases",
							unmerged.GetDescription()
						});
						callback(true);
					}
					else
					{
						ISavedGameMetadata savedGameMetadata = null;
						if (hashSet.Count > hashSet2.Count)
						{
							savedGameMetadata = original;
							resolver.ChooseMetadata(savedGameMetadata);
							Debug.LogFormat("====== Partially resolved using original metadata '{0}': '{1}'", new object[]
							{
								"Purchases",
								original.GetDescription()
							});
						}
						else
						{
							savedGameMetadata = unmerged;
							resolver.ChooseMetadata(savedGameMetadata);
							Debug.LogFormat("====== Partially resolved using unmerged metadata '{0}': '{1}'", new object[]
							{
								"Purchases",
								unmerged.GetDescription()
							});
						}
						HashSet<string> mergedItems = new HashSet<string>(hashSet);
						mergedItems.UnionWith(hashSet2);
						try
						{
							Dictionary<string, string> dictionary = (from r in ItemDb.allRecords
							where r.StorageId != null && mergedItems.Contains(r.StorageId)
							select r).ToDictionary((ItemRecord rec) => rec.StorageId, (ItemRecord rec) => rec.Tag);
						}
						catch (Exception arg)
						{
							Debug.LogError("exception in initializing storageIdsToTagsOfItemsToBeSaved: " + arg);
							Dictionary<string, string> dictionary = new Dictionary<string, string>();
						}
						List<string> list = new List<string>();
						foreach (string text in mergedItems)
						{
							if (PromoActionsManager.sharedManager != null)
							{
								PromoActionsManager.sharedManager.RemoveItemFromUnlocked(text);
							}
							else
							{
								Debug.LogErrorFormat("SynchronizeIosWithCloud: PromoActionsManager.sharedManager == null", new object[0]);
							}
							int @int = Storager.getInt(text, false);
							Storager.setInt(text, 1, false);
							if (@int == 0 && (text == Defs.IsFacebookLoginRewardaGained || WeaponManager.GotchaGuns.Contains(text)))
							{
								list.Add(text);
							}
							try
							{
								ItemRecord itemRecord;
								if (@int == 0 && ItemDb.allRecordsWithStorageIds.TryGetValue(text, out itemRecord))
								{
									bool flag = WeaponManager.RemoveGunFromAllTryGunRelated(itemRecord.Tag);
									if (flag)
									{
										try
										{
											if (ABTestController.useBuffSystem)
											{
												BuffSystem.instance.RemoveGunBuff();
											}
											else
											{
												KillRateCheck.RemoveGunBuff();
											}
										}
										catch
										{
										}
									}
								}
							}
							catch
							{
							}
						}
						PlayerPrefs.Save();
						PromoActionsManager.FireUnlockedItemsUpdated();
						int levelBefore = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
						WeaponManager.RefreshExpControllers();
						ExperienceController.SendAnalyticsForLevelsFromCloud(levelBefore);
						WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(list);
						HashSet<string> source = new HashSet<string>(PurchasesSynchronizer.GetPurchasesIds());
						string outputString = Json.Serialize(source.ToArray<string>());
						byte[] bytes = Encoding.UTF8.GetBytes(outputString);
						string description = string.Format("Merged by '{0}': '{1}' and '{2}'", SystemInfo.deviceModel, original.GetDescription(), unmerged.GetDescription());
						SavedGameMetadataUpdate updateForMetadata = default(SavedGameMetadataUpdate.Builder).WithUpdatedDescription(description).Build();
						traceContext.Add(string.Format("> CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
						GpgFacade.Instance.SavedGame.CommitUpdate(savedGameMetadata, updateForMetadata, bytes, delegate(SavedGameRequestStatus writeStatus, ISavedGameMetadata closeMetadata)
						{
							traceContext.Add(string.Format("CommitUpdate.Callback >: {0:F3}", Time.realtimeSinceStartup));
							try
							{
								Debug.LogFormat("====== Written '{0}': {1} '{2}'    '{3}'", new object[]
								{
									"Purchases",
									writeStatus,
									closeMetadata.GetDescription(),
									outputString
								});
								callback(writeStatus == SavedGameRequestStatus.Success);
							}
							finally
							{
								traceContext.Add(string.Format("CommitUpdate.Callback <: {0:F3}", Time.realtimeSinceStartup));
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
									{
										"Purchases",
										Json.Serialize(traceContext)
									});
								}
							}
						});
						traceContext.Add(string.Format("< CommitUpdate: {0:F3}", Time.realtimeSinceStartup));
					}
				}
				finally
				{
					traceContext.Add(string.Format("OpenWithManualConflictResolution.ConflictCallback: {0:F3} <", Time.realtimeSinceStartup));
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
						{
							"Purchases",
							Json.Serialize(traceContext)
						});
					}
				}
			};
			Debug.LogFormat("====== Trying to open '{0}'...", new object[]
			{
				"Purchases"
			});
			traceContext.Add(string.Format("> OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			GpgFacade.Instance.SavedGame.OpenWithManualConflictResolution("Purchases", DataSource.ReadNetworkOnly, true, conflictCallback, completedCallback);
			traceContext.Add(string.Format("< OpenWithManualConflictResolution: {0:F3}", Time.realtimeSinceStartup));
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] SavedGame ({0}): {1}", new object[]
				{
					"Purchases",
					Json.Serialize(traceContext)
				});
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003F4E RID: 16206 RVA: 0x00153524 File Offset: 0x00151724
		public bool HasItemsToBeSaved
		{
			get
			{
				return this._itemsToBeSaved.Count > 0;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003F4F RID: 16207 RVA: 0x00153534 File Offset: 0x00151734
		public ICollection<string> ItemsToBeSaved
		{
			get
			{
				return this._itemsToBeSaved;
			}
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x0015353C File Offset: 0x0015173C
		public IEnumerator SavePendingItemsToStorager()
		{
			TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
			EventHandler<PurchasesSavingEventArgs> handler = this.PurchasesSavingStarted;
			if (handler != null)
			{
				handler(this, new PurchasesSavingEventArgs(promise.Task));
			}
			try
			{
				if (this._itemsToBeSaved.Count <= 0)
				{
					yield break;
				}
				if (Application.isEditor)
				{
					yield return new WaitForSeconds(3f);
				}
				float writeTime = 0f;
				while (this._itemsToBeSaved.Count > 0)
				{
					int index = this._itemsToBeSaved.Count - 1;
					string item = this._itemsToBeSaved[index];
					if (PromoActionsManager.sharedManager != null)
					{
						PromoActionsManager.sharedManager.RemoveItemFromUnlocked(item);
					}
					else
					{
						Debug.LogErrorFormat("SynchronizeIosWithCloud: PromoActionsManager.sharedManager == null", new object[0]);
					}
					float startWrite = Time.realtimeSinceStartup;
					int valueBefore = Storager.getInt(item, false);
					Storager.setInt(item, 1, false);
					try
					{
						ItemRecord recordForGun;
						if (valueBefore == 0 && ItemDb.allRecordsWithStorageIds.TryGetValue(item, out recordForGun))
						{
							bool truGunRemoved = WeaponManager.RemoveGunFromAllTryGunRelated(recordForGun.Tag);
							if (truGunRemoved)
							{
								try
								{
									if (ABTestController.useBuffSystem)
									{
										BuffSystem.instance.RemoveGunBuff();
									}
									else
									{
										KillRateCheck.RemoveGunBuff();
									}
								}
								catch
								{
								}
							}
						}
					}
					catch
					{
					}
					writeTime += Time.realtimeSinceStartup - startWrite;
					if (index % 2 == 1)
					{
						yield return null;
					}
					this._itemsToBeSaved.RemoveAt(index);
				}
				PromoActionsManager.FireUnlockedItemsUpdated();
			}
			finally
			{
				promise.TrySetResult(true);
			}
			yield break;
		}

		// Token: 0x06003F51 RID: 16209 RVA: 0x00153558 File Offset: 0x00151758
		public bool SynchronizeIfAuthenticated(Action<bool> callback)
		{
			if (!GpgFacade.Instance.IsAuthenticated())
			{
				return false;
			}
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this.SynchronizeIfAuthenticatedWithSavedGamesService(callback);
			return true;
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00153590 File Offset: 0x00151790
		internal IEnumerator SimulateSynchronization(Action<bool> callback)
		{
			Debug.Log("Waiting for syncing...");
			yield return new WaitForSeconds(3f);
			List<string> traceContext = new List<string>();
			traceContext.Add(string.Format("SimulateSynchronization >: {0:F3}", Time.realtimeSinceStartup));
			try
			{
				List<string> simulatedInventory = new List<string>
				{
					"currentLevel1",
					"currentLevel2",
					"currentLevel3",
					"currentLevel4",
					"currentLevel5",
					"BerettaSN",
					"gravity_2",
					"IsFacebookLoginRewardaGained"
				};
				string inputString = Json.Serialize(simulatedInventory);
				byte[] data = Encoding.UTF8.GetBytes(inputString);
				this.HandleReadBinaryData(null, SavedGameRequestStatus.Success, data, callback, traceContext);
				callback(true);
			}
			finally
			{
				traceContext.Add(string.Format("SimulateSynchronization <: {0:F3}", Time.realtimeSinceStartup));
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] SimulateSynchronization ({0}): {1}", new object[]
				{
					"Purchases",
					Json.Serialize(traceContext)
				});
			}
			yield break;
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x001535BC File Offset: 0x001517BC
		public static PurchasesSynchronizer Instance
		{
			get
			{
				if (PurchasesSynchronizer._instance == null)
				{
					PurchasesSynchronizer._instance = new PurchasesSynchronizer();
				}
				return PurchasesSynchronizer._instance;
			}
		}

		// Token: 0x04002E91 RID: 11921
		public const string Filename = "Purchases";

		// Token: 0x04002E92 RID: 11922
		private readonly List<string> _itemsToBeSaved = new List<string>();

		// Token: 0x04002E93 RID: 11923
		private static PurchasesSynchronizer _instance;

		// Token: 0x04002E94 RID: 11924
		private static IEnumerable<string> _allItemIds;
	}
}
