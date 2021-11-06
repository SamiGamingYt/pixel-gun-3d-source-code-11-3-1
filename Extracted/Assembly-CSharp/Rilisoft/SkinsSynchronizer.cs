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
	// Token: 0x02000845 RID: 2117
	internal sealed class SkinsSynchronizer
	{
		// Token: 0x06004CD5 RID: 19669 RVA: 0x001BAEDC File Offset: 0x001B90DC
		private SkinsSynchronizer()
		{
		}

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06004CD7 RID: 19671 RVA: 0x001BAEF0 File Offset: 0x001B90F0
		// (remove) Token: 0x06004CD8 RID: 19672 RVA: 0x001BAF0C File Offset: 0x001B910C
		public event EventHandler Updated;

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004CD9 RID: 19673 RVA: 0x001BAF28 File Offset: 0x001B9128
		public static SkinsSynchronizer Instance
		{
			get
			{
				return SkinsSynchronizer.s_instance;
			}
		}

		// Token: 0x06004CDA RID: 19674 RVA: 0x001BAF30 File Offset: 0x001B9130
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

		// Token: 0x06004CDB RID: 19675 RVA: 0x001BAF84 File Offset: 0x001B9184
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
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				this.SyncSkinsAndCapeIos();
				return null;
			}
			return null;
		}

		// Token: 0x06004CDC RID: 19676 RVA: 0x001BAFEC File Offset: 0x001B91EC
		private void SyncAmazon()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.SyncAmazon()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				try
				{
					if (!Application.isEditor)
					{
						AGSWhispersyncClient.Synchronize();
						using (AGSGameDataMap gameData = AGSWhispersyncClient.GetGameData())
						{
							this.EnsureNotNull(gameData, "dataMap");
							using (AGSSyncableString latestString = gameData.GetLatestString("skinsJson"))
							{
								this.EnsureNotNull(latestString, "skinsJson");
								string json = latestString.GetValue() ?? "{}";
								SkinsMemento skinsMemento = JsonUtility.FromJson<SkinsMemento>(json);
								SkinsMemento skinsMemento2 = this.LoadLocalSkins();
								SkinsMemento skinsMemento3 = SkinsMemento.Merge(skinsMemento2, skinsMemento);
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("Local skins: {0}", new object[]
									{
										skinsMemento2
									});
									Debug.LogFormat("Cloud skins: {0}", new object[]
									{
										skinsMemento
									});
									Debug.LogFormat("Merged skins: {0}", new object[]
									{
										skinsMemento3
									});
								}
								int num = skinsMemento3.DeletedSkins.Distinct<string>().Count<string>();
								int num2 = (from s in skinsMemento3.Skins
								select s.Id).Distinct<string>().Count<string>();
								long id = skinsMemento3.Cape.Id;
								int num3 = skinsMemento2.DeletedSkins.Distinct<string>().Count<string>();
								int num4 = (from s in skinsMemento2.Skins
								select s.Id).Distinct<string>().Count<string>();
								long id2 = skinsMemento2.Cape.Id;
								bool flag = num3 < num || num4 < num2 || id2 < id;
								if (flag)
								{
									if (num4 < num2)
									{
										Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
									}
									this.OverwriteLocalSkins(skinsMemento2, skinsMemento3);
									EventHandler updated = this.Updated;
									if (updated != null)
									{
										updated(this, EventArgs.Empty);
									}
								}
								int num5 = skinsMemento.DeletedSkins.Distinct<string>().Count<string>();
								int num6 = (from s in skinsMemento.Skins
								select s.Id).Distinct<string>().Count<string>();
								long id3 = skinsMemento.Cape.Id;
								bool flag2 = num5 < num || num6 < num2 || id3 < id;
								if (flag2)
								{
									string val = JsonUtility.ToJson(skinsMemento3);
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

		// Token: 0x06004CDD RID: 19677 RVA: 0x001BB340 File Offset: 0x001B9540
		private void EnsureNotNull(object value, string name)
		{
			if (value == null)
			{
				throw new InvalidOperationException(name ?? string.Empty);
			}
		}

		// Token: 0x06004CDE RID: 19678 RVA: 0x001BB35C File Offset: 0x001B955C
		private IEnumerator SyncGoogleCoroutine(bool pullOnly)
		{
			if (!this.Ready)
			{
				yield break;
			}
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PullGoogleCoroutine('{1}')", new object[]
			{
				base.GetType().Name,
				(!pullOnly) ? "sync" : "pull"
			});
			using (new ScopeLogger(thisMethod, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				SkinsSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(SkinsSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				for (;;)
				{
					string callee = string.Format(CultureInfo.InvariantCulture, "Pull and wait ({0})", new object[]
					{
						i
					});
					using (ScopeLogger logger = new ScopeLogger(thisMethod, callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<SkinsMemento>> future = googleSavedGamesFacade.Pull();
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
							Debug.LogWarning("Failed to pull skins with exception: " + ex.Message);
							yield return delay;
						}
						else
						{
							SavedGameRequestStatus requestStatus = future.Result.RequestStatus;
							if (requestStatus != SavedGameRequestStatus.Success)
							{
								Debug.LogWarning("Failed to push skins with status: " + requestStatus);
								yield return delay;
							}
							else
							{
								SkinsMemento cloudSkins = future.Result.Value;
								SkinsMemento localSkins = this.LoadLocalSkins();
								HashSet<string> cloudDeletedSkins = new HashSet<string>(cloudSkins.DeletedSkins);
								HashSet<string> localDeletedSkins = new HashSet<string>(localSkins.DeletedSkins);
								HashSet<string> mergedDeletedSkins = new HashSet<string>(cloudDeletedSkins);
								mergedDeletedSkins.UnionWith(localDeletedSkins);
								HashSet<string> cloudSkinIds = new HashSet<string>(from s in cloudSkins.Skins
								select s.Id);
								HashSet<string> localSkinIds = new HashSet<string>(from s in localSkins.Skins
								select s.Id);
								HashSet<string> mergedSkinIds = new HashSet<string>(cloudSkinIds);
								mergedSkinIds.UnionWith(localSkinIds);
								CapeMemento chosenCape = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
								bool localDirty = localDeletedSkins.Count < mergedDeletedSkins.Count || localSkinIds.Count < mergedSkinIds.Count || localSkins.Cape.Id < chosenCape.Id;
								if (localDirty)
								{
									if (localSkinIds.Count < mergedSkinIds.Count)
									{
										Storager.setInt(Defs.SkinsMakerInProfileBought, 1, true);
									}
									this.OverwriteLocalSkins(localSkins, cloudSkins);
									EventHandler handler = this.Updated;
									if (handler != null)
									{
										handler(this, EventArgs.Empty);
									}
								}
								bool cloudDirty = cloudDeletedSkins.Count < mergedDeletedSkins.Count || cloudSkinIds.Count < mergedSkinIds.Count || cloudSkins.Cape.Id < chosenCape.Id;
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Skins] Succeeded to pull skins: {0}, 'pullOnly':{1}, 'conflicted':{2}, 'cloudDirty':{3}", new object[]
									{
										cloudSkins,
										pullOnly,
										cloudSkins.Conflicted,
										cloudDirty
									});
								}
								if (pullOnly)
								{
									yield break;
								}
								if (cloudSkins.Conflicted || cloudDirty)
								{
									using (new ScopeLogger(thisMethod, "PushGoogleCoroutine(conflict)", Defs.IsDeveloperBuild && !Application.isEditor))
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

		// Token: 0x06004CDF RID: 19679 RVA: 0x001BB388 File Offset: 0x001B9588
		internal IEnumerator PushGoogleCoroutine()
		{
			if (!this.Ready)
			{
				yield break;
			}
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.PushGoogleCoroutine()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(thisMethod, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				SkinsSynchronizerGoogleSavedGameFacade googleSavedGamesFacade = default(SkinsSynchronizerGoogleSavedGameFacade);
				WaitForSeconds delay = new WaitForSeconds(30f);
				int i = 0;
				for (;;)
				{
					SkinsMemento localSkins = this.LoadLocalSkins();
					string localSkinsAsString = localSkins.ToString();
					string callee = string.Format(CultureInfo.InvariantCulture, "Push and wait {0} ({1})", new object[]
					{
						localSkinsAsString,
						i
					});
					using (ScopeLogger logger = new ScopeLogger(thisMethod, callee, Defs.IsDeveloperBuild && !Application.isEditor))
					{
						Task<GoogleSavedGameRequestResult<ISavedGameMetadata>> future = googleSavedGamesFacade.Push(localSkins);
						while (!future.IsCompleted)
						{
							yield return null;
						}
						logger.Dispose();
						if (future.IsFaulted)
						{
							Exception ex = future.Exception.InnerExceptions.FirstOrDefault<Exception>() ?? future.Exception;
							Debug.LogWarning("Failed to push skins with exception: " + ex.Message);
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
									Debug.LogFormat("[Skins] Succeeded to push skins {0}: '{1}'", new object[]
									{
										localSkinsAsString,
										description
									});
								}
								yield break;
							}
							Debug.LogWarning("Failed to push skins with status: " + requestStatus);
							yield return delay;
						}
					}
					i++;
				}
			}
			yield break;
		}

		// Token: 0x06004CE0 RID: 19680 RVA: 0x001BB3A4 File Offset: 0x001B95A4
		private void OverwriteLocalSkins(SkinsMemento localSkins, SkinsMemento cloudSkins)
		{
			HashSet<string> hashSet = new HashSet<string>(localSkins.DeletedSkins.Concat(cloudSkins.DeletedSkins));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(localSkins.Skins.Count);
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>(localSkins.Skins.Count);
			foreach (SkinMemento skinMemento in cloudSkins.Skins)
			{
				if (!hashSet.Contains(skinMemento.Id))
				{
					dictionary[skinMemento.Id] = skinMemento.Skin;
					dictionary2[skinMemento.Id] = skinMemento.Name;
				}
			}
			foreach (SkinMemento skinMemento2 in localSkins.Skins)
			{
				if (!hashSet.Contains(skinMemento2.Id))
				{
					dictionary[skinMemento2.Id] = skinMemento2.Skin;
					dictionary2[skinMemento2.Id] = skinMemento2.Name;
				}
			}
			string value = Json.Serialize(dictionary);
			PlayerPrefs.SetString("User Skins", value);
			string value2 = Json.Serialize(dictionary2);
			PlayerPrefs.SetString("User Name Skins", value2);
			CapeMemento capeMemento = CapeMemento.ChooseCape(localSkins.Cape, cloudSkins.Cape);
			string value3 = JsonUtility.ToJson(capeMemento);
			PlayerPrefs.SetString("NewUserCape", value3);
			this.RefreshGui(dictionary, dictionary2, capeMemento);
			PlayerPrefs.Save();
		}

		// Token: 0x06004CE1 RID: 19681 RVA: 0x001BB57C File Offset: 0x001B977C
		private void RefreshGui(Dictionary<string, string> skins, Dictionary<string, string> skinNames, CapeMemento cape)
		{
			if (ShopNGUIController.sharedShop == null)
			{
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in skins)
			{
				if (!SkinsController.skinsForPers.ContainsKey(keyValuePair.Key))
				{
					Texture2D value = SkinsController.TextureFromString(keyValuePair.Value, 64, 32);
					SkinsController.skinsForPers.Add(keyValuePair.Key, value);
					SkinsController.customSkinIds.Add(keyValuePair.Key);
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in skinNames)
			{
				SkinsController.skinsNamesForPers[keyValuePair2.Key] = keyValuePair2.Value;
			}
			SkinsController.capeUserTexture = SkinsController.TextureFromString(cape.Cape, 32, 32);
			if (ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.ReloadGridOrCarousel(ShopNGUIController.sharedShop.CurrentItem);
				ShopNGUIController.sharedShop.ShowLockOrPropertiesAndButtons();
			}
		}

		// Token: 0x06004CE2 RID: 19682 RVA: 0x001BB6D8 File Offset: 0x001B98D8
		private SkinsMemento LoadLocalSkins()
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.LoadLocalSkins()", new object[]
			{
				base.GetType().Name
			});
			SkinsMemento result;
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				string @string = PlayerPrefs.GetString("DeletedSkins", string.Empty);
				List<object> list = Json.Deserialize(@string) as List<object>;
				List<string> deletedSkins = (list == null) ? new List<string>() : list.OfType<string>().ToList<string>();
				string string2 = PlayerPrefs.GetString("User Skins", string.Empty);
				string string3 = PlayerPrefs.GetString("NewUserCape", string.Empty);
				CapeMemento cape = Tools.DeserializeJson<CapeMemento>(string3);
				Dictionary<string, object> dictionary = (Json.Deserialize(string2) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				List<SkinMemento> list2 = new List<SkinMemento>(dictionary.Count);
				if (dictionary.Count == 0)
				{
					Debug.LogFormat("Deserialized skins are empty: {0}", new object[]
					{
						string2
					});
					result = new SkinsMemento(list2, deletedSkins, cape);
				}
				else
				{
					string string4 = PlayerPrefs.GetString("User Name Skins", string.Empty);
					Dictionary<string, object> dict = (Json.Deserialize(string4) as Dictionary<string, object>) ?? new Dictionary<string, object>();
					foreach (KeyValuePair<string, object> keyValuePair in dictionary)
					{
						string key = keyValuePair.Key;
						string empty;
						if (!dict.TryGetValue(key, out empty))
						{
							empty = string.Empty;
						}
						string skin = (keyValuePair.Value as string) ?? string.Empty;
						SkinMemento item = new SkinMemento(key, empty, skin);
						list2.Add(item);
					}
					SkinsMemento skinsMemento = new SkinsMemento(list2, deletedSkins, cape);
					result = skinsMemento;
				}
			}
			return result;
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004CE3 RID: 19683 RVA: 0x001BB8E0 File Offset: 0x001B9AE0
		internal bool Ready
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004CE4 RID: 19684 RVA: 0x001BB8E4 File Offset: 0x001B9AE4
		private void SyncSkinsAndCapeIos()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer || Storager.ICloudAvailable)
			{
			}
		}

		// Token: 0x04003B6D RID: 15213
		private static readonly SkinsSynchronizer s_instance = new SkinsSynchronizer();
	}
}
