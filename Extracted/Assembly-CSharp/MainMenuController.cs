using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Unity;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using I2.Loc;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002FF RID: 767
internal sealed class MainMenuController : ControlsSettingsBase
{
	// Token: 0x06001A88 RID: 6792 RVA: 0x0006BC30 File Offset: 0x00069E30
	public MainMenuController()
	{
		this._newClanIncomingInvitesSprite = new Lazy<UISprite>(() => this.clansButton.Map((GameObject c) => c.GetComponentsInChildren<UISprite>(true).FirstOrDefault((UISprite s) => "NewMessages".Equals(s.name))));
		this._leaderboardsButton = new Lazy<UIButton[]>(() => this.leadersButton.GetComponentsInChildren<UIButton>(true));
		this._leaderboardScript = new Lazy<LeaderboardScript>(new Func<LeaderboardScript>(UnityEngine.Object.FindObjectOfType<LeaderboardScript>));
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0006BCE8 File Offset: 0x00069EE8
	// Note: this type is marked as 'beforefieldinit'.
	static MainMenuController()
	{
		MainMenuController._syncPromise = new TaskCompletionSource<bool>();
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06001A8A RID: 6794 RVA: 0x0006BD00 File Offset: 0x00069F00
	// (remove) Token: 0x06001A8B RID: 6795 RVA: 0x0006BD18 File Offset: 0x00069F18
	public static event Action onLoadMenu;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06001A8C RID: 6796 RVA: 0x0006BD30 File Offset: 0x00069F30
	// (remove) Token: 0x06001A8D RID: 6797 RVA: 0x0006BD48 File Offset: 0x00069F48
	public static event Action onEnableMenuForAskname;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06001A8E RID: 6798 RVA: 0x0006BD60 File Offset: 0x00069F60
	// (remove) Token: 0x06001A8F RID: 6799 RVA: 0x0006BD78 File Offset: 0x00069F78
	public static event Action<bool> onActiveMainMenu;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x06001A90 RID: 6800 RVA: 0x0006BD90 File Offset: 0x00069F90
	// (remove) Token: 0x06001A91 RID: 6801 RVA: 0x0006BDA0 File Offset: 0x00069FA0
	public event EventHandler BackPressed
	{
		add
		{
			this._backSubscribers.Add(value);
		}
		remove
		{
			this._backSubscribers.Remove(value);
		}
	}

	// Token: 0x170004B2 RID: 1202
	// (get) Token: 0x06001A92 RID: 6802 RVA: 0x0006BDB0 File Offset: 0x00069FB0
	internal Task SyncFuture
	{
		get
		{
			return MainMenuController._syncPromise.Task;
		}
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x0006BDBC File Offset: 0x00069FBC
	internal static bool LevelAlreadySaved(int level)
	{
		string key = "currentLevel" + level;
		return Storager.hasKey(key) && Storager.getInt(key, false) > 0;
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x0006BDF4 File Offset: 0x00069FF4
	internal static int FindMaxLevel(IEnumerable<string> itemsToBeSaved)
	{
		int num = 0;
		foreach (string text in itemsToBeSaved)
		{
			if (text.StartsWith("currentLevel"))
			{
				string[] array = text.Split(new string[]
				{
					"currentLevel"
				}, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length > 0)
				{
					string value = array[array.Length - 1];
					if (!string.IsNullOrEmpty(value))
					{
						int num2 = Convert.ToInt32(value);
						if (num2 > num)
						{
							num = num2;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x0006BEBC File Offset: 0x0006A0BC
	private IEnumerator SynchronizeEditorCoroutine()
	{
		if (Application.isEditor)
		{
			MainMenuController._syncPromise.TrySetResult(false);
			yield break;
		}
		Debug.Log("Trying to simulate syncing to cloud...");
		if (PlayerPrefs.GetInt("PendingGooglePlayGamesSync", 0) == 0)
		{
			Debug.Log("No pending GooglePlay Games sync.");
			MainMenuController._syncPromise.TrySetResult(false);
			yield break;
		}
		IEnumerator coroutine = PurchasesSynchronizer.Instance.SimulateSynchronization(delegate(bool succeeded)
		{
			Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[]
			{
				succeeded,
				Time.realtimeSinceStartup
			});
			try
			{
				if (!succeeded)
				{
					MainMenuController._syncPromise.TrySetResult(false);
				}
				else
				{
					Action action = delegate()
					{
						PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
						if (WeaponManager.sharedManager != null)
						{
							CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap, true));
						}
						MainMenuController._syncPromise.TrySetResult(true);
					};
					if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
					{
						int num = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("[Rilisoft] Incoming level: {0}", new object[]
							{
								num
							});
						}
						if (num > 0)
						{
							CoroutineRunner.Instance.StartCoroutine(this.WaitReturnToMainMenuAndShowRestorePanel(action));
						}
						else
						{
							MainMenuController._syncPromise.TrySetResult(false);
						}
					}
					else
					{
						action();
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			finally
			{
				Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[]
				{
					succeeded,
					Time.realtimeSinceStartup
				});
			}
		});
		CoroutineRunner.Instance.StartCoroutine(coroutine);
		yield break;
	}

	// Token: 0x06001A96 RID: 6806 RVA: 0x0006BED8 File Offset: 0x0006A0D8
	private IEnumerator SynchronizeGoogleCoroutine(Action tryUpdateNickname, GameServicesController gameServicesController)
	{
		MainMenuController._socialNetworkingInitilized = true;
		Debug.Log("Trying to authenticate with Google Play Games...");
		try
		{
			if (PlayerPrefs.GetInt("PendingGooglePlayGamesSync", 0) == 0)
			{
				MainMenuController._syncPromise.TrySetResult(false);
			}
			if (PlayerPrefs.GetInt("GoogleSignInDenied", 0) > 0)
			{
				Debug.LogWarning("Skipping sync because authentication has already been denied.");
				yield break;
			}
			Action<bool> authenticateCallback = delegate(bool succeeded)
			{
				Debug.LogFormat("[Rilisoft] MainMenuController.Authenticate.Callback({0}) >: {1:F3}", new object[]
				{
					succeeded,
					Time.realtimeSinceStartup
				});
				string message = (!succeeded) ? "Play Games Services authentication failed." : string.Format("User authenticated after call in SynchronizeGoogleCoroutine(): {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
				Debug.Log(message);
				PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(!succeeded));
				if (!succeeded)
				{
					MainMenuController._syncPromise.TrySetResult(false);
					Debug.LogFormat("IsCompleted: {0} IsCanceled: {1} IsFaulted: {2}", new object[]
					{
						this.SyncFuture.IsCompleted,
						this.SyncFuture.IsCanceled,
						this.SyncFuture.IsFaulted
					});
					return;
				}
				tryUpdateNickname();
				bool pendingGooglePlayGamesSync;
				if (!pendingGooglePlayGamesSync)
				{
					Debug.Log("No pending GooglePlay Games sync.");
					MainMenuController._syncPromise.TrySetResult(false);
					return;
				}
				CampaignProgressSynchronizer.Instance.Sync();
				TrophiesSynchronizer.Instance.Sync();
				SkinsSynchronizer.Instance.Sync();
				AchievementSynchronizer.Instance.Sync();
				PetsSynchronizer.Instance.Sync();
				Debug.Log("Pending synchronization, retrying...");
				PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(delegate(bool succeded)
				{
					Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[]
					{
						succeded,
						Time.realtimeSinceStartup
					});
					try
					{
						if (!succeded)
						{
							MainMenuController._syncPromise.TrySetResult(false);
						}
						else
						{
							Action action = delegate()
							{
								Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.InnerCallback >: {0:F3}", new object[]
								{
									Time.realtimeSinceStartup
								});
								PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
								if (WeaponManager.sharedManager != null)
								{
									IEnumerator routine = WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap, true);
									CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.Instance.WrapCoroutine(routine, MainMenuController._syncPromise));
									if (GiftController.Instance)
									{
										GiftController.Instance.ReCreateSlots();
									}
								}
								else
								{
									MainMenuController._syncPromise.TrySetResult(true);
								}
								Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.InnerCallback <: {0:F3}", new object[]
								{
									Time.realtimeSinceStartup
								});
							};
							Debug.LogFormat("[Rilisoft] PurchasesSynchronizer.HasItemsToBeSaved: {0}", new object[]
							{
								PurchasesSynchronizer.Instance.HasItemsToBeSaved
							});
							if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
							{
								int num = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
								if (Defs.IsDeveloperBuild)
								{
									Debug.LogFormat("[Rilisoft] Incoming level: {0}", new object[]
									{
										num
									});
								}
								if (num > 0)
								{
									CoroutineRunner.Instance.StartCoroutine(this.<>f__this.WaitReturnToMainMenuAndShowRestorePanel(action));
								}
								else
								{
									MainMenuController._syncPromise.TrySetResult(false);
								}
							}
							else
							{
								Debug.LogFormat("[Rilisoft] > MainMenuController.PurchasesSynchronizer.InnerCallback: {0:F3}", new object[]
								{
									Time.realtimeSinceStartup
								});
								action();
								Debug.LogFormat("[Rilisoft] < MainMenuController.PurchasesSynchronizer.InnerCallback: {0:F3}", new object[]
								{
									Time.realtimeSinceStartup
								});
							}
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					finally
					{
						Debug.LogFormat("[Rilisoft] MainMenuController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[]
						{
							succeeded,
							Time.realtimeSinceStartup
						});
					}
				});
				Debug.LogFormat("[Rilisoft] MainMenuController.Authenticate.Callback({0}) <: {1:F3}", new object[]
				{
					succeeded,
					Time.realtimeSinceStartup
				});
			};
			GpgFacade.Instance.Authenticate(authenticateCallback, false);
		}
		catch (InvalidOperationException ex2)
		{
			InvalidOperationException ex = ex2;
			Debug.LogWarning("SettingsMainMenuController: Exception occured while authenticating with Google Play Games. See next exception message for details.");
			Debug.LogException(ex);
		}
		yield return null;
		gameServicesController.WaitAuthenticationAndIncrementBeginnerAchievement();
		yield break;
	}

	// Token: 0x06001A97 RID: 6807 RVA: 0x0006BF10 File Offset: 0x0006A110
	private IEnumerator SynchronizeAmazonCoroutine(Action tryUpdateNickname, GameServicesController gameServicesController)
	{
		Social.Active = GameCircleSocial.Instance;
		Debug.Log("Social user authenticated: " + Social.localUser.authenticated);
		Debug.LogFormat("[Rilisoft] > MainMenuController.PurchasesSynchronizer.SynchronizeAmazonProgress: {0:F3}", new object[]
		{
			Time.realtimeSinceStartup
		});
		ProgressSynchronizer.Instance.SynchronizeAmazonProgress();
		Debug.LogFormat("[Rilisoft] < MainMenuController.PurchasesSynchronizer.SynchronizeAmazonProgress: {0:F3}", new object[]
		{
			Time.realtimeSinceStartup
		});
		yield return new WaitForSeconds(1f);
		PurchasesSynchronizer.Instance.SynchronizeAmazonPurchases();
		if (PurchasesSynchronizer.Instance.HasItemsToBeSaved)
		{
			int maxLevel = MainMenuController.FindMaxLevel(PurchasesSynchronizer.Instance.ItemsToBeSaved);
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] Incoming level: {0}", new object[]
				{
					maxLevel
				});
			}
			if (maxLevel > 0 && !MainMenuController.LevelAlreadySaved(maxLevel))
			{
				CoroutineRunner.Instance.StartCoroutine(this.WaitReturnToMainMenuAndShowRestorePanel(new Action(this.RefreshGui)));
			}
			else
			{
				MainMenuController._syncPromise.TrySetResult(false);
			}
		}
		else
		{
			Debug.LogFormat("[Rilisoft] > MainMenuController.PurchasesSynchronizer.InnerCallback: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
			this.RefreshGui();
			Debug.LogFormat("[Rilisoft] < MainMenuController.PurchasesSynchronizer.InnerCallback: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		yield return null;
		if (GameCircleSocial.Instance.localUser.authenticated)
		{
			tryUpdateNickname();
		}
		gameServicesController.WaitAuthenticationAndIncrementBeginnerAchievement();
		MainMenuController._syncPromise.TrySetResult(false);
		yield break;
	}

	// Token: 0x06001A98 RID: 6808 RVA: 0x0006BF48 File Offset: 0x0006A148
	private bool RestorePanelAllowed()
	{
		return !ShopNGUIController.GuiActive && !(SceneManager.GetActiveScene().name != Defs.MainMenuScene) && !AskNameManager.isShow && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled);
	}

	// Token: 0x06001A99 RID: 6809 RVA: 0x0006BFB0 File Offset: 0x0006A1B0
	private IEnumerator WaitReturnToMainMenuAndShowRestorePanel(Action refreshCallback)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("WaitReturnToMainMenu >: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		yield return new WaitUntil(new Func<bool>(this.RestorePanelAllowed));
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("> WaitReturnToMainMenu.Callback: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		TrainingController.OnGetProgress();
		if (QuestSystem.Instance != null && QuestSystem.Instance.QuestProgress != null)
		{
			QuestSystem.Instance.QuestProgress.FilterFulfilledTutorialQuests();
		}
		if (HintController.instance != null)
		{
			HintController.instance.ShowNext();
		}
		this.RefreshSettingsButton();
		Debug.Log("Trying to fill weapon slots...");
		try
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
			{
				IEnumerable<Weapon> playerWeapons = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>();
				IEnumerable<Weapon> availableWeapons = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>();
				if (!playerWeapons.Any((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 3))
				{
					string prefabName = ItemDb.GetByTag(WeaponTags.BASIC_FLAMETHROWER_Tag).PrefabName;
					WeaponManager.sharedManager.EquipWeapon(availableWeapons.First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == prefabName), true, false);
				}
				if (!playerWeapons.Any((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 5))
				{
					string prefabName2 = ItemDb.GetByTag(WeaponTags.SignalPistol_Tag).PrefabName;
					WeaponManager.sharedManager.EquipWeapon(availableWeapons.First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == prefabName2), true, false);
				}
				if (!playerWeapons.Any((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 4))
				{
					string prefabName3 = ItemDb.GetByTag(WeaponTags.HunterRifleTag).PrefabName;
					WeaponManager.sharedManager.EquipWeapon(availableWeapons.First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == prefabName3), true, false);
				}
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.LogException(ex);
		}
		InfoWindowController.ShowRestorePanel(delegate
		{
			CoroutineRunner.Instance.StartCoroutine(MainMenuController.SaveItemsToStorager(refreshCallback));
		});
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("< WaitReturnToMainMenu.Callback: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
			Debug.LogFormat("WaitReturnToMainMenu <: {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		yield break;
	}

	// Token: 0x06001A9A RID: 6810 RVA: 0x0006BFDC File Offset: 0x0006A1DC
	private void RefreshGui()
	{
		PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
		if (WeaponManager.sharedManager != null)
		{
			CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap, true));
		}
		MainMenuController._syncPromise.TrySetResult(true);
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x06001A9B RID: 6811 RVA: 0x0006C030 File Offset: 0x0006A230
	// (set) Token: 0x06001A9C RID: 6812 RVA: 0x0006C038 File Offset: 0x0006A238
	public static bool trainingCompleted { get; set; }

	// Token: 0x170004B4 RID: 1204
	// (get) Token: 0x06001A9D RID: 6813 RVA: 0x0006C040 File Offset: 0x0006A240
	public bool FreePanelIsActive
	{
		get
		{
			return this._freePanel.ObjectIsActive;
		}
	}

	// Token: 0x170004B5 RID: 1205
	// (get) Token: 0x06001A9E RID: 6814 RVA: 0x0006C050 File Offset: 0x0006A250
	public Camera Camera3D
	{
		get
		{
			return (!(this.rotateCamera != null)) ? null : this.rotateCamera.gameObject.GetComponentInChildren("Main Camera", true);
		}
	}

	// Token: 0x170004B6 RID: 1206
	// (get) Token: 0x06001A9F RID: 6815 RVA: 0x0006C080 File Offset: 0x0006A280
	public static bool ShopOpened
	{
		get
		{
			return MainMenuController.sharedController != null && MainMenuController.sharedController._shopInstance != null;
		}
	}

	// Token: 0x170004B7 RID: 1207
	// (get) Token: 0x06001AA0 RID: 6816 RVA: 0x0006C0A8 File Offset: 0x0006A2A8
	public UIPanel LeaderboardsPanel
	{
		get
		{
			return this._leaderboardScript.Value.Panel;
		}
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x0006C0BC File Offset: 0x0006A2BC
	private void Awake()
	{
		this.stubLoading.SetActive(true);
		if (TrainingController.TrainingCompleted)
		{
			PetsManager.LoadPetsToMemory();
		}
		this._socialBanner = new LazyObject<SocialGunBannerView>(this._socialBannerPrefab.ResourcePath, this._subwindowsHandler);
		this._freePanel = new LazyObject<MainMenuFreePanel>(this._freePanelPrefab.ResourcePath, this._subwindowsHandler);
		this._newsPanel = new LazyObject<NewsLobbyController>(this._newsPrefab.ResourcePath, this._subwindowsHandler);
		this._feedbackPanel = new LazyObject<FeedbackMenuController>(this._feedbackPrefab.ResourcePath, this._subwindowsHandler);
	}

	// Token: 0x06001AA2 RID: 6818 RVA: 0x0006C154 File Offset: 0x0006A354
	public void SaveShowPanelAndClose()
	{
		if (this.mainPanel != null)
		{
			this.saveOpenPanel.Clear();
			for (int i = 0; i < this.mainPanel.transform.childCount; i++)
			{
				GameObject gameObject = this.mainPanel.transform.GetChild(i).gameObject;
				if (!(gameObject.GetComponent<UICamera>() != null))
				{
					if (gameObject.activeSelf)
					{
						this.saveOpenPanel.Add(gameObject);
						gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06001AA3 RID: 6819 RVA: 0x0006C1EC File Offset: 0x0006A3EC
	public void ShowSavePanel(bool needClear = true)
	{
		for (int i = 0; i < this.saveOpenPanel.Count; i++)
		{
			GameObject gameObject = this.saveOpenPanel[i];
			if (gameObject != null)
			{
				gameObject.SetActive(true);
			}
		}
		if (needClear)
		{
			this.saveOpenPanel.Clear();
		}
	}

	// Token: 0x06001AA4 RID: 6820 RVA: 0x0006C248 File Offset: 0x0006A448
	private void InvokeLastBackHandler()
	{
		if (this._backSubscribers.Count == 0)
		{
			return;
		}
		EventHandler o = this._backSubscribers[this._backSubscribers.Count - 1];
		o.Do(delegate(EventHandler lastHandler)
		{
			lastHandler(this, EventArgs.Empty);
		});
	}

	// Token: 0x06001AA5 RID: 6821 RVA: 0x0006C294 File Offset: 0x0006A494
	public static bool IsLevelUpOrBannerShown()
	{
		bool flag = ExperienceController.sharedController != null && ExperienceController.sharedController.isShowNextPlashka;
		bool flag2 = BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown;
		return flag || flag2;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x0006C2E8 File Offset: 0x0006A4E8
	public static bool ShowBannerOrLevelup()
	{
		return MainMenuController.IsLevelUpOrBannerShown() || FriendsWindowGUI.Instance.InterfaceEnabled || MainMenu.BlockInterface || Defs.isShowUserAgrement;
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x0006C318 File Offset: 0x0006A518
	public static void DoMemoryConsumingTaskInEmptyScene(Action action, Action onSeparateSceneCaseAction = null)
	{
		if (Device.IsLoweMemoryDevice)
		{
			CleanUpAndDoAction.action = (onSeparateSceneCaseAction ?? action);
			SceneManager.LoadScene("LoadAnotherApp");
		}
		else if (action != null)
		{
			action();
		}
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x0006C350 File Offset: 0x0006A550
	public void HandleFacebookLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (FB.IsLoggedIn)
		{
			FB.LogOut();
		}
		else
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
			{
				FacebookController.Login(null, null, "Options", null);
			}, null);
		}
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x0006C39C File Offset: 0x0006A59C
	public void HandleTwitterLoginButton()
	{
		ButtonClickSound.TryPlayClick();
		if (TwitterController.IsLoggedIn && TwitterController.Instance != null)
		{
			TwitterController.Instance.Logout();
		}
		else
		{
			MainMenuController.DoMemoryConsumingTaskInEmptyScene(delegate
			{
				if (TwitterController.Instance != null)
				{
					TwitterController.Instance.Login(null, null, "Options");
				}
			}, null);
		}
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x0006C3FC File Offset: 0x0006A5FC
	private IEnumerator OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			yield return new WaitForSeconds(1f);
			string dismissReason = string.Empty;
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
			{
				if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
				{
					MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
					dismissReason = "`SuppressShowOnReturnFromPause`";
				}
				else if (ReplaceAdmobPerelivController.sharedController == null)
				{
					dismissReason = "`ReplaceAdmobPerelivController.sharedController == null`";
				}
				else if (!FreeAwardController.FreeAwardChestIsInIdleState)
				{
					dismissReason = "`FreeAwardChestIsInIdleState == false`";
				}
				else
				{
					dismissReason = ConnectSceneNGUIController.GetReasonToDismissFakeInterstitial();
					if (string.IsNullOrEmpty(dismissReason))
					{
						ReplaceAdmobPerelivController.IncreaseTimesCounter();
						base.StartCoroutine(this.LoadAndShowReplaceAdmobPereliv("On return from pause to Lobby"));
					}
				}
			}
			if (!string.IsNullOrEmpty(dismissReason) && Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "Dismissing fake interstitial. {0}" : "<color=magenta>Dismissing fake interstitial. {0}</color>";
				Debug.LogFormat(format, new object[]
				{
					dismissReason
				});
			}
			MainMenuController.ReloadFacebookFriends();
		}
		yield break;
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x0006C428 File Offset: 0x0006A628
	private IEnumerator LoadAndShowReplaceAdmobPereliv(string context)
	{
		if (!this.loadReplaceAdmobPerelivRunning)
		{
			try
			{
				this.loadReplaceAdmobPerelivRunning = true;
				if (!ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
				{
					ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
				}
				while (!ReplaceAdmobPerelivController.sharedController.DataLoaded)
				{
					if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
					{
						this.loadReplaceAdmobPerelivRunning = false;
						yield break;
					}
					yield return null;
				}
				yield return new WaitForSeconds(0.5f);
				if (FreeAwardController.FreeAwardChestIsInIdleState && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown))
				{
					ReplaceAdmobPerelivController.TryShowPereliv(context);
					ReplaceAdmobPerelivController.sharedController.DestroyImage();
				}
			}
			finally
			{
				this.loadReplaceAdmobPerelivRunning = false;
			}
		}
		yield break;
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x0006C454 File Offset: 0x0006A654
	public void OnSocialGunEventButtonClick()
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		BannerWindowController x = BannerWindowController.SharedController;
		if (x == null)
		{
			return;
		}
		this._socialBanner.Value.Show();
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x0006C4A4 File Offset: 0x0006A6A4
	private void OnDestroy()
	{
		if (NickLabelStack.sharedStack != null && NickLabelStack.sharedStack.gameObject != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(true);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.GetComponent<TrafficForwardingScript>().Do(delegate(TrafficForwardingScript tf)
			{
				tf.Updated = (EventHandler<TrafficForwardingInfo>)Delegate.Remove(tf.Updated, new EventHandler<TrafficForwardingInfo>(this.RefreshTrafficForwardingButton));
			});
		}
		SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult -= this.HandleSocialGunViewLoginCompleted;
		PromoActionsManager.EventX3Updated -= this.OnEventX3Updated;
		StarterPackController.OnStarterPackEnable -= this.OnStarterPackContainerShow;
		PromoActionsManager.OnDayOfValorEnable -= this.OnDayOfValorContainerShow;
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.ChangeLocalizeLabel));
		PromoActionClick.Click -= this.HandlePromoActionClicked;
		SettingsController.ControlsClicked -= base.HandleControlsClicked;
		MainMenuController.sharedController = null;
		if (FreeAwardController.Instance != null)
		{
			FreeAwardController instance = FreeAwardController.Instance;
			instance.transform.root.Map((Transform t) => t.gameObject).Do(new Action<GameObject>(UnityEngine.Object.Destroy));
		}
		if (!TrainingController.TrainingCompleted)
		{
			AskNameManager.onComplete -= HintController.instance.ShowCurrentHintObjectLabel;
		}
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x0006C604 File Offset: 0x0006A804
	private void OnGUI()
	{
		if (!Launcher.UsingNewLauncher && MainMenuController._drawLoadingProgress)
		{
			ActivityIndicator.LoadingProgress = 1f;
			return;
		}
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x0006C628 File Offset: 0x0006A828
	private void InitializeBannerWindow()
	{
		this._advertisementController = base.gameObject.GetComponent<AdvertisementController>();
		if (this._advertisementController == null)
		{
			this._advertisementController = base.gameObject.AddComponent<AdvertisementController>();
		}
		BannerWindowController.SharedController.advertiseController = this._advertisementController;
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x0006C678 File Offset: 0x0006A878
	private void CheckIfPendingAward()
	{
		if (Storager.hasKey("PendingFreeAward"))
		{
			int @int = Storager.getInt("PendingFreeAward", false);
			if (@int > 0)
			{
				int num = FreeAwardController.Instance.GiveAwardAndIncrementCount();
				Storager.setInt("PendingInterstitial", 0, false);
			}
		}
		if (Storager.hasKey("PendingInterstitial"))
		{
			int int2 = Storager.getInt("PendingInterstitial", false);
			if (int2 > 0)
			{
				Storager.setInt("PendingInterstitial", 0, false);
			}
		}
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x0006C6EC File Offset: 0x0006A8EC
	private static void ReloadFacebookFriends()
	{
		if (FacebookController.FacebookSupported && FacebookController.sharedController != null && FB.IsLoggedIn)
		{
			FacebookController.sharedController.InputFacebookFriends(null, true);
		}
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x0006C72C File Offset: 0x0006A92C
	private void RefreshSettingsButton()
	{
		if (this.settingsButton == null)
		{
			return;
		}
		ButtonHandler component = this.settingsButton.GetComponent<ButtonHandler>();
		if (component != null)
		{
			component.Clicked += this.HandleSettingsClicked;
		}
		UIButton component2 = this.settingsButton.GetComponent<UIButton>();
		if (component2 != null)
		{
			component2.isEnabled = TrainingController.TrainingCompleted;
		}
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x0006C798 File Offset: 0x0006A998
	private new IEnumerator Start()
	{
		this.UpdateInappBonusChestActiveState();
		LogsManager.DisableLogsIfAllowed();
		ConnectSceneNGUIController.isReturnFromGame = false;
		if (Storager.hasKey("Analytics:af_tutorial_completion"))
		{
			AnalyticsStuff.TrySendOnceToAppsFlyer("tutorial_lobby");
		}
		string myNick = ProfileController.GetPlayerNameOrDefault();
		string filteredNick = FilterBadWorld.FilterString(myNick);
		if (string.IsNullOrEmpty(filteredNick) || filteredNick.Trim() == string.Empty)
		{
			filteredNick = ProfileController.defaultPlayerName;
		}
		if (filteredNick != myNick)
		{
			if (Application.isEditor)
			{
				Debug.Log("Saving new name:    " + filteredNick);
			}
			PlayerPrefs.SetString("NamePlayer", filteredNick);
		}
		Storager.setInt(Defs.ShownLobbyLevelSN, 3, false);
		base.transform.GetChild(0).GetComponent<UICamera>().allowMultiTouch = false;
		Defs.isDaterRegim = false;
		SocialGunBannerView.SocialGunBannerViewLoginCompletedWithResult += this.HandleSocialGunViewLoginCompleted;
		TwitterController.CheckAndGiveTwitterReward("Start");
		FacebookController.CheckAndGiveFacebookReward("Start");
		MainMenuController.ReloadFacebookFriends();
		if (FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			ClanIncomingInvitesController.FetchClanIncomingInvites(FriendsController.sharedController.id);
		}
		ConnectSceneNGUIController.InterstitialRequest = false;
		this.CheckIfPendingAward();
		if (this.socialButton != null)
		{
			this.socialButton.gameObject.SetActive(true);
			ButtonHandler handler = this.socialButton.GetComponent<ButtonHandler>();
			handler.Clicked += this.HandleSocialButton;
		}
		WeaponManager.RefreshExpControllers();
		PlayerPrefs.SetInt("CountRunMenu", PlayerPrefs.GetInt("CountRunMenu", 0) + 1);
		this.premiumTime.gameObject.SetActive(true);
		this.InitializeBannerWindow();
		bool developerConsoleEnabled = Debug.isDebugBuild;
		if (this.developerConsole != null)
		{
			this.developerConsole.gameObject.SetActive(developerConsoleEnabled);
		}
		this.starterPackPanel.gameObject.SetActive(false);
		TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
		if (trafficForwardingScript != null)
		{
			TrafficForwardingScript trafficForwardingScript2 = trafficForwardingScript;
			trafficForwardingScript2.Updated = (EventHandler<TrafficForwardingInfo>)Delegate.Combine(trafficForwardingScript2.Updated, new EventHandler<TrafficForwardingInfo>(this.RefreshTrafficForwardingButton));
			Task<TrafficForwardingInfo> trafficForwardingResult = trafficForwardingScript.GetTrafficForwardingInfo().Filter((Task<TrafficForwardingInfo> t) => t.IsCompleted && !t.IsCanceled && !t.IsFaulted);
			if (trafficForwardingResult != null)
			{
				this._trafficForwardingUrl = trafficForwardingResult.Result.Url;
			}
			if (this.trafficForwardingButton != null)
			{
				this.RefreshTrafficForwardingButton(this, (trafficForwardingResult == null) ? TrafficForwardingInfo.DisabledInstance : trafficForwardingResult.Result);
			}
		}
		this.dayOfValorContainer.gameObject.SetActive(false);
		string bgTextureName = ConnectSceneNGUIController.MainLoadingTexture();
		this.stubTexture.mainTexture = Resources.Load<Texture>(bgTextureName);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		base.Start();
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		SettingsController.ControlsClicked += base.HandleControlsClicked;
		Defs.isShowUserAgrement = false;
		this.completeTraining.SetActive(!this.shopButton.GetComponent<UIButton>().isEnabled);
		this.mainPanel.SetActive(true);
		this.settingsPanel.SetActive(false);
		if (this._newsPanel.ObjectIsActive)
		{
			this._newsPanel.Value.gameObject.SetActive(false);
		}
		if (this._freePanel.ObjectIsActive)
		{
			this._freePanel.Value.SetVisible(false);
		}
		this.SettingsJoysticksPanel.SetActive(false);
		ConnectSceneNGUIController.NeedShowReviewInConnectScene = false;
		MainMenuController.sharedController = this;
		if (this.campaignButton != null)
		{
			ButtonHandler bh = this.campaignButton.GetComponent<ButtonHandler>();
			if (bh != null)
			{
				bh.Clicked += this.HandleCampaingClicked;
			}
		}
		if (this.multiplayerButton != null)
		{
			ButtonHandler bh2 = this.multiplayerButton.GetComponent<ButtonHandler>();
			if (bh2 != null)
			{
				bh2.Clicked += this.HandleMultiPlayerClicked;
			}
		}
		if (this.skinsMakerButton != null)
		{
			if (MainMenu.SkinsMakerSupproted())
			{
				ButtonHandler bh3 = this.skinsMakerButton.GetComponent<ButtonHandler>();
				if (bh3 != null)
				{
					bh3.Clicked += this.HandleSkinsMakerClicked;
				}
			}
			else
			{
				this.skinsMakerButton.SetActive(false);
			}
		}
		if (this.profileButton != null)
		{
			ButtonHandler bh4 = this.profileButton.GetComponent<ButtonHandler>();
			if (bh4 != null)
			{
				bh4.Clicked += this.HandleProfileClicked;
			}
		}
		if (this.freeButton != null)
		{
			ButtonHandler bh5 = this.freeButton.GetComponent<ButtonHandler>();
			if (bh5 != null)
			{
				bh5.Clicked += this.HandleFreeClicked;
			}
		}
		if (this.shopButton != null)
		{
			ButtonHandler bh6 = this.shopButton.GetComponent<ButtonHandler>();
			if (bh6 != null)
			{
				bh6.Clicked += this.HandleShopClicked;
			}
		}
		if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
		{
			this.HandleShopClicked(null, null);
			PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
			PlayerPrefs.Save();
		}
		this.RefreshSettingsButton();
		if (this._openFeedBackBtn != null)
		{
			this._openFeedBackBtn.Clicked += this.HandleSupportButtonClicked;
		}
		if (this.friendsButton != null)
		{
			ButtonHandler bh7 = this.friendsButton.GetComponent<ButtonHandler>();
			if (bh7 != null)
			{
				bh7.Clicked += this.HandleFriendsClicked;
			}
		}
		if (this._openNewsBtn != null)
		{
			this._openNewsBtn.Clicked += this.HandleNewsButtonClicked;
		}
		if (this.agreeButton != null)
		{
			ButtonHandler bh8 = this.agreeButton.GetComponent<ButtonHandler>();
			if (bh8 != null)
			{
				bh8.Clicked += this.HandleAgreeClicked;
			}
		}
		if (this.diclineButton != null)
		{
			ButtonHandler bh9 = this.diclineButton.GetComponent<ButtonHandler>();
			if (bh9 != null)
			{
				bh9.Clicked += this.HandleDiclineClicked;
			}
		}
		if (this.coinsShopButton != null)
		{
			ButtonHandler bh10 = this.coinsShopButton.GetComponent<ButtonHandler>();
			if (bh10 != null)
			{
				bh10.Clicked += this.HandleBankClicked;
			}
		}
		if (BankController.Instance != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(BankController.Instance.transform.root.gameObject);
		}
		else
		{
			Debug.LogWarning("bankController == null");
		}
		if (MainMenuController.SingleModeOnStart)
		{
			this.OnClickSingleModeButton();
		}
		yield return new WaitForSeconds(0.5f);
		PromoActionClick.Click += this.HandlePromoActionClicked;
		yield return new WaitForSeconds(0.5f);
		if (MainMenuController.friendsOnStart)
		{
			this.HandleFriendsClicked(null, null);
			yield return null;
		}
		MainMenuController._drawLoadingProgress = false;
		this.stubLoading.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
		Debug.Log("Start initializing ProfileGui.");
		ProfileController profileController = UnityEngine.Object.FindObjectOfType<ProfileController>();
		if (profileController == null)
		{
			GameObject profileGuiRequest = Resources.Load<GameObject>("ProfileGui");
			yield return profileGuiRequest;
			GameObject go = UnityEngine.Object.Instantiate<GameObject>(profileGuiRequest);
			UnityEngine.Object.DontDestroyOnLoad(go);
		}
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("Training completed: {0}. Authenticating...", new object[]
			{
				TrainingController.TrainingCompleted
			});
		}
		TrophiesSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
		AchievementSynchronizer.Instance.Sync();
		PetsSynchronizer.Instance.Sync();
		if (!MainMenuController._socialNetworkingInitilized)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Social networking is not initialized.");
			}
			GameServicesController gameServicesController = UnityEngine.Object.FindObjectOfType<GameServicesController>();
			if (gameServicesController == null)
			{
				GameObject gameServicesControllerGo = new GameObject("Rilisoft.GameServicesController");
				gameServicesController = gameServicesControllerGo.AddComponent<GameServicesController>();
			}
			Action tryUpdateNickname = delegate()
			{
				if (PlayerPanel.instance != null)
				{
					PlayerPanel.instance.UpdateNickPlayer();
					PlayerPanel.instance.UpdateExp();
					PlayerPanel.instance.UpdateRating();
				}
			};
			bool playGameServicesDisabled = false;
			if (playGameServicesDisabled)
			{
				Debug.Log("Play Game Services explicitly disabled.");
				MainMenuController._syncPromise.TrySetResult(false);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Application.isEditor)
				{
					yield return CoroutineRunner.Instance.StartCoroutine(this.SynchronizeEditorCoroutine());
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					yield return CoroutineRunner.Instance.StartCoroutine(this.SynchronizeGoogleCoroutine(tryUpdateNickname, gameServicesController));
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					yield return CoroutineRunner.Instance.StartCoroutine(this.SynchronizeAmazonCoroutine(tryUpdateNickname, gameServicesController));
				}
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				GameCenterSingleton initializedInstance = GameCenterSingleton.Instance;
				MainMenuController._socialNetworkingInitilized = true;
				yield return null;
				gameServicesController.WaitAuthenticationAndIncrementBeginnerAchievement();
				MainMenuController._syncPromise.TrySetResult(false);
			}
			else
			{
				MainMenuController._syncPromise.TrySetResult(false);
			}
		}
		if (this.bannerContainer != null)
		{
			InGameGUI.SetLayerRecursively(this.bannerContainer, LayerMask.NameToLayer("Banners"));
		}
		PromoActionsManager.EventX3Updated += this.OnEventX3Updated;
		this.OnEventX3Updated();
		StarterPackController.OnStarterPackEnable += this.OnStarterPackContainerShow;
		this.OnStarterPackContainerShow(StarterPackController.Get.isEventActive);
		PromoActionsManager.OnDayOfValorEnable += this.OnDayOfValorContainerShow;
		this.OnDayOfValorContainerShow(PromoActionsManager.sharedManager.IsDayOfValorEventActive);
		if (ReplaceAdmobPerelivController.sharedController != null && ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby && ReplaceAdmobPerelivController.sharedController.DataLoaded)
		{
			ReplaceAdmobPerelivController.sharedController.ShouldShowInLobby = false;
			ReplaceAdmobPerelivController.TryShowPereliv("Lobby after launch");
			ReplaceAdmobPerelivController.sharedController.DestroyImage();
		}
		string key = MainMenuController.GetAbuseKey_f1a4329e(4054069918U);
		if (Storager.hasKey(key))
		{
			string ticksHalvedString = Storager.getString(key, false);
			if (!string.IsNullOrEmpty(ticksHalvedString) && ticksHalvedString != "0")
			{
				long nowTicksHalved = DateTime.UtcNow.Ticks >> 1;
				long abuseTicksHalved = nowTicksHalved;
				if (long.TryParse(ticksHalvedString, out abuseTicksHalved))
				{
					abuseTicksHalved = Math.Min(nowTicksHalved, abuseTicksHalved);
					Storager.setString(key, abuseTicksHalved.ToString(), false);
				}
				else
				{
					Storager.setString(key, nowTicksHalved.ToString(), false);
				}
				TimeSpan timespan = TimeSpan.FromTicks(nowTicksHalved - abuseTicksHalved);
				bool needApply = (!Defs.IsDeveloperBuild) ? (timespan.TotalDays >= 1.0) : (timespan.TotalMinutes >= 3.0);
				if (needApply && Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PhotonNetwork.PhotonServerSettings.AppID = "68c9fbdb-682a-411f-a229-1a9786b5835c";
					PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
				}
			}
		}
		base.StartCoroutine(this.TryToShowExpiredBanner());
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.ChangeLocalizeLabel));
		this.ChangeLocalizeLabel();
		if (MainMenuController.friendsOnStart)
		{
			if (this.mainPanel != null)
			{
				this.mainPanel.transform.root.gameObject.SetActive(false);
			}
			MainMenuController.friendsOnStart = false;
		}
		this.newsIndicator.SetActive(PlayerPrefs.GetInt("LobbyIsAnyNewsKey", 0) == 1);
		if (!TrainingController.TrainingCompleted)
		{
			AskNameManager.onComplete += HintController.instance.ShowCurrentHintObjectLabel;
		}
		if (MainMenuController.onLoadMenu != null)
		{
			MainMenuController.onLoadMenu();
		}
		if (MainMenuController.onEnableMenuForAskname != null)
		{
			MainMenuController.onEnableMenuForAskname();
		}
		QuestSystem.Instance.QuestCompleted -= MainMenuController.OnCompletedQuest;
		QuestSystem.Instance.QuestCompleted += MainMenuController.OnCompletedQuest;
		yield break;
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x0006C7B4 File Offset: 0x0006A9B4
	internal static IEnumerator SaveItemsToStorager(Action callback)
	{
		Debug.LogFormat("> MainMenuController.SaveItemsToStorager {0:F3}", new object[]
		{
			Time.realtimeSinceStartup
		});
		try
		{
			if (InfoWindowController.Instance.background != null)
			{
				while (InfoWindowController.IsActive)
				{
					yield return null;
				}
			}
			yield return null;
			IDisposable escapeSubscription = BackSystem.Instance.Register(delegate
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("Ignoring [Escape] while syncing weapons.");
				}
			}, "MainMenuWaitingSaving");
			string caption = LocalizationStore.Get("Key_1974");
			ActivityIndicator.SetActiveWithCaption(caption);
			InfoWindowController.BlockAllClick();
			yield return CoroutineRunner.Instance.StartCoroutine(PurchasesSynchronizer.Instance.SavePendingItemsToStorager());
			InfoWindowController.HideCurrentWindow();
			ActivityIndicator.IsActiveIndicator = false;
			escapeSubscription.Dispose();
			if (callback != null)
			{
				callback();
			}
		}
		finally
		{
			Debug.LogFormat("< MainMenuController.SaveItemsToStorager {0:F3}", new object[]
			{
				Time.realtimeSinceStartup
			});
		}
		yield break;
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x0006C7D8 File Offset: 0x0006A9D8
	private static void OnCompletedQuest(object sender, QuestCompletedEventArgs e)
	{
		AccumulativeQuestBase accumulativeQuestBase = e.Quest as AccumulativeQuestBase;
		if (accumulativeQuestBase == null)
		{
			return;
		}
		InfoWindowController.ShowQuestBox(string.Empty, QuestConstants.GetAccumulativeQuestDescriptionByType(accumulativeQuestBase));
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x0006C808 File Offset: 0x0006AA08
	private void HandleSocialGunViewLoginCompleted(bool success)
	{
		if (this.mainPanel == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/" + ((!success) ? "PanelAuthFailed" : "PanelAuthSucces")));
		gameObject.transform.parent = ((!this._freePanel.ObjectIsActive) ? this.mainPanel.transform : this._freePanel.Value.gameObject.transform);
		Player_move_c.SetLayerRecursively(gameObject, this.mainPanel.layer);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x0006C8F0 File Offset: 0x0006AAF0
	public void HandleClansClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		Action action = delegate()
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				this.GoClans();
			}
		};
		action();
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x0006C958 File Offset: 0x0006AB58
	private void ChangeLocalizeLabel()
	{
		this._localizeSaleLabel = LocalizationStore.Get("Key_0419");
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x0006C96C File Offset: 0x0006AB6C
	private void GoClans()
	{
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x0006C9A8 File Offset: 0x0006ABA8
	private static string GetAbuseKey_f1a4329e(uint pad)
	{
		return (894321575U ^ pad).ToString("x");
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x0006C9CC File Offset: 0x0006ABCC
	public static bool IsShowRentExpiredPoint()
	{
		if (MainMenuController.sharedController == null)
		{
			return false;
		}
		Transform rentExpiredPoint = MainMenuController.sharedController.RentExpiredPoint;
		return !(rentExpiredPoint == null) && rentExpiredPoint.childCount > 0;
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x0006CA10 File Offset: 0x0006AC10
	public static bool SavedShwonLobbyLevelIsLessThanActual()
	{
		return Storager.getInt(Defs.ShownLobbyLevelSN, false) < ExpController.LobbyLevel;
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x0006CA24 File Offset: 0x0006AC24
	private IEnumerator TryToShowExpiredBanner()
	{
		while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			try
			{
				if (!ShopNGUIController.GuiActive && (!(FreeAwardController.Instance != null) || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>()) && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown) && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && (!this.settingsPanel.activeInHierarchy && !this._freePanel.ObjectIsActive && !this._feedbackPanel.ObjectIsActive && (!(ProfileController.Instance != null) || !ProfileController.Instance.InterfaceEnabled)) && (!(FriendsWindowGUI.Instance != null) || !FriendsWindowGUI.Instance.InterfaceEnabled) && !this.stubLoading.activeInHierarchy && !this.singleModePanel.activeSelf && !this.UserAgreementPanel.activeInHierarchy && !this.SettingsJoysticksPanel.activeInHierarchy && (!(this.LeaderboardsPanel != null) || !this.LeaderboardsPanel.gameObject.activeInHierarchy))
				{
					if (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown)
					{
						if (this.RentExpiredPoint.childCount == 0)
						{
							if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
							{
								GameObject window = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LobbyLevels/LobbyLevelTips_" + (Storager.getInt(Defs.ShownLobbyLevelSN, false) + 1)));
								window.transform.parent = this.RentExpiredPoint;
								Player_move_c.SetLayerRecursively(window, LayerMask.NameToLayer("NGUI"));
								window.transform.localPosition = new Vector3(0f, 0f, -130f);
								window.transform.localRotation = Quaternion.identity;
								window.transform.localScale = new Vector3(1f, 1f, 1f);
							}
							else
							{
								bool flag = Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1 && ShopNGUIController.ShowPremimAccountExpiredIfPossible(this.RentExpiredPoint, "NGUI", string.Empty, true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogWarning("exception in Lobby  TryToShowExpiredBanner: " + e);
			}
		}
		yield break;
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x0006CA40 File Offset: 0x0006AC40
	public void HandleDeveloperConsoleClicked()
	{
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x0006CA44 File Offset: 0x0006AC44
	public void HandlePromoActionClicked(string tg)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		AnalyticsStuff.LogSpecialOffersPanel("Efficiency", "View", null, null);
		if (tg != null && tg == "StickersPromoActionsPanelKey")
		{
			ButtonClickSound.Instance.PlayClick();
			BuySmileBannerController.openedFromPromoActions = true;
			this.OnBuySmilesClick();
			return;
		}
		int num = -1;
		if (ShopNGUIController.sharedShop != null)
		{
			try
			{
				num = ItemDb.GetItemCategory(tg);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in getting category of promo action item on click: " + arg);
			}
			if (num != -1)
			{
				ShopNGUIController.sharedShop.CategoryToChoose = (ShopNGUIController.CategoryNames)num;
			}
			ShopNGUIController.sharedShop.SetItemToShow(new ShopNGUIController.ShopItem(tg, (ShopNGUIController.CategoryNames)num));
			ShopNGUIController.sharedShop.IsInShopFromPromoPanel(true, tg);
		}
		if (num != -1)
		{
			this.HandleShopClicked(null, EventArgs.Empty);
		}
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x0006CB44 File Offset: 0x0006AD44
	private void CalcBtnRects()
	{
		Transform transform = NGUITools.GetRoot(base.gameObject).transform;
		Camera component = transform.GetChild(0).GetComponent<Camera>();
		Transform transform2 = component.transform;
		float num = 768f;
		float num2 = num * ((float)Screen.width / (float)Screen.height);
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.shopButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		bounds.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.shopRect = new Rect((bounds.center.x - 105.5f) * Defs.Coef, (bounds.center.y - 57f) * Defs.Coef, 211f * Defs.Coef, 114f * Defs.Coef);
		Bounds bounds2 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.survivalButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		bounds2.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.survivalRect = new Rect((bounds2.center.x - 107f) * Defs.Coef, (bounds2.center.y - 35f) * Defs.Coef, 214f * Defs.Coef, 70f * Defs.Coef);
		Bounds bounds3 = NGUIMath.CalculateRelativeWidgetBounds(transform2, this.campaignButton.GetComponent<UIButton>().tweenTarget.transform, true, true);
		bounds3.center += new Vector3(num2 * 0.5f, num * 0.5f, 0f);
		this.campaignRect = new Rect((bounds3.center.x - 107f) * Defs.Coef, (bounds3.center.y - 35f) * Defs.Coef, 214f * Defs.Coef, 70f * Defs.Coef);
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x0006CD78 File Offset: 0x0006AF78
	private void UpdateEventX3RemainedTime()
	{
		long eventX3RemainedTime = PromoActionsManager.sharedManager.EventX3RemainedTime;
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)eventX3RemainedTime);
		string text = string.Empty;
		if (timeSpan.Days > 0)
		{
			text = string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", new object[]
			{
				this._localizeSaleLabel,
				timeSpan.Days,
				(timeSpan.Days != 1) ? "Days" : "Day",
				timeSpan.Hours,
				timeSpan.Minutes,
				timeSpan.Seconds
			});
		}
		else
		{
			text = string.Format("{0}: {1:00}:{2:00}:{3:00}", new object[]
			{
				this._localizeSaleLabel,
				timeSpan.Hours,
				timeSpan.Minutes,
				timeSpan.Seconds
			});
		}
		if (this.eventX3RemainTime != null)
		{
			for (int i = 0; i < this.eventX3RemainTime.Length; i++)
			{
				this.eventX3RemainTime[i].text = text;
			}
		}
		if (this.colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !this.colorBlinkForX3.enabled)
		{
			this.colorBlinkForX3.enabled = true;
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x0006CEE0 File Offset: 0x0006B0E0
	public bool PromoOffersPanelShouldBeShown()
	{
		return this._shopInstance == null && !MainMenuController.ShowBannerOrLevelup();
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x0006CF00 File Offset: 0x0006B100
	private void Update()
	{
		this.UpdateInappBonusChestActiveState();
		if (this.InAdventureScreen && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (this.settingsPanel.activeInHierarchy)
		{
			if (this.facebookConnectedSettings.activeSelf != (FacebookController.FacebookSupported && FB.IsLoggedIn))
			{
				this.facebookConnectedSettings.SetActive(FacebookController.FacebookSupported && FB.IsLoggedIn);
			}
			if (this.facebookDisconnectedSettings.activeSelf != (FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1))
			{
				this.facebookDisconnectedSettings.SetActive(FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 1);
			}
			if (this.facebookConnectSettings.activeSelf != (FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0))
			{
				this.facebookConnectSettings.SetActive(FacebookController.FacebookSupported && !FB.IsLoggedIn && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0);
			}
			if (this.twitterConnectedSettings.activeSelf != (TwitterController.TwitterSupported && TwitterController.IsLoggedIn))
			{
				this.twitterConnectedSettings.SetActive(TwitterController.TwitterSupported && TwitterController.IsLoggedIn);
			}
			if (this.twitterDisconnectedSettings.activeSelf != (TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 1))
			{
				this.twitterDisconnectedSettings.SetActive(TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 1);
			}
			if (this.twitterConnectSettings.activeSelf != (TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0))
			{
				this.twitterConnectSettings.SetActive(TwitterController.TwitterSupported && !TwitterController.IsLoggedIn && Storager.getInt(Defs.IsTwitterLoginRewardaGained, true) == 0);
			}
			if (this.facebookLoginContainer != null)
			{
				this.facebookLoginContainer.SetActive(FacebookController.FacebookSupported);
			}
			if (this.twitterLoginContainer != null)
			{
				this.twitterLoginContainer.SetActive(TwitterController.TwitterSupported);
			}
		}
		bool flag = Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1 || PremiumAccountController.Instance.isAccountActive;
		bool active = flag && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel >= 3;
		this.premium.SetActive(active);
		this.premiumButton.isEnabled = (Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1);
		if (this.premiumUpPlashka.activeSelf != (!(PremiumAccountController.Instance != null) || !PremiumAccountController.Instance.isAccountActive))
		{
			this.premiumUpPlashka.SetActive(!(PremiumAccountController.Instance != null) || !PremiumAccountController.Instance.isAccountActive);
		}
		if (this.premiumbottomPlashka.activeSelf != (PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive))
		{
			this.premiumbottomPlashka.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
		}
		if (PremiumAccountController.Instance != null)
		{
			long num = (long)PremiumAccountController.Instance.GetDaysToEndAllAccounts();
			for (int i = 0; i < this.premiumLevels.Count; i++)
			{
				bool flag2 = false;
				if (num > 0L && num < 3L && i == 0)
				{
					flag2 = true;
				}
				if (num >= 3L && num < 7L && i == 1)
				{
					flag2 = true;
				}
				if (num >= 7L && num < 30L && i == 2)
				{
					flag2 = true;
				}
				if (num >= 30L && i == 3)
				{
					flag2 = true;
				}
				if (this.premiumLevels[i].activeSelf != flag2)
				{
					this.premiumLevels[i].SetActive(flag2);
				}
			}
			if (Time.realtimeSinceStartup - this._timePremiumTimeUpdated >= 1f)
			{
				this.premiumTime.text = PremiumAccountController.Instance.GetTimeToEndAllAccounts();
				this._timePremiumTimeUpdated = Time.realtimeSinceStartup;
			}
		}
		bool flag3 = (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !ShopNGUIController.GuiActive;
		if (this.starParticleStarterPackGaemObject != null && this.starParticleStarterPackGaemObject.activeInHierarchy != flag3)
		{
			this.starParticleStarterPackGaemObject.SetActive(flag3);
		}
		if (Time.realtimeSinceStartup - this._eventX3RemainTimeLastUpdateTime >= 0.5f)
		{
			this._eventX3RemainTimeLastUpdateTime = Time.realtimeSinceStartup;
			this.UpdateEventX3RemainedTime();
			if (this._dayOfValorEnabled)
			{
				this.dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
			}
		}
		if (this._isCancellationRequested)
		{
			MainMenuController mainMenuController = MainMenuController.sharedController;
			if (this.SettingsJoysticksPanel.activeSelf)
			{
				this.SettingsJoysticksPanel.SetActive(false);
				this.settingsPanel.SetActive(true);
			}
			else if (this._freePanel.ObjectIsActive)
			{
				if (this._shopInstance == null && !MainMenuController.ShowBannerOrLevelup())
				{
					this.mainPanel.SetActive(true);
					if (this._freePanel.ObjectIsLoaded)
					{
						this._freePanel.Value.SetVisible(false);
					}
					this.rotateCamera.OnMainMenuCloseOptions();
					AnimationGift.instance.CheckVisibleGift();
				}
			}
			else if (this._newsPanel.ObjectIsActive)
			{
				this._newsPanel.Value.gameObject.SetActive(false);
				this.mainPanel.SetActive(true);
			}
			else if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
			else if (!(this.settingsPanel != null) || !this.settingsPanel.activeInHierarchy)
			{
				if (!this._freePanel.ObjectIsLoaded || !this._freePanel.Value.gameObject.activeInHierarchy)
				{
					if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
					{
						if (!ShopNGUIController.GuiActive)
						{
							if (!(ProfileController.Instance != null) || !ProfileController.Instance.InterfaceEnabled)
							{
								if (PremiumAccountScreenController.Instance != null)
								{
									PremiumAccountScreenController.Instance.Hide();
								}
								else if (mainMenuController != null && mainMenuController.singleModePanel.activeSelf)
								{
									mainMenuController.OnClickBackSingleModeButton();
								}
								else
								{
									PlayerPrefs.Save();
									Application.Quit();
								}
							}
						}
					}
				}
			}
			this._isCancellationRequested = false;
		}
		if (this.rotateCamera != null && !this.rotateCamera.IsAnimPlaying)
		{
			float rotationRateForCharacterInMenues = RilisoftRotator.RotationRateForCharacterInMenues;
			Rect touchZone;
			if (this.settingsPanel.activeInHierarchy)
			{
				touchZone = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
			}
			else
			{
				if (this.campaignRect.width.Equals(0f))
				{
					this.CalcBtnRects();
				}
				if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.IsOpened)
				{
					touchZone = new Rect(0.38f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height);
				}
				else
				{
					touchZone = new Rect(0.2f * (float)Screen.width, 0.25f * (float)Screen.height, 1.4f * (float)Screen.width, 0.65f * (float)Screen.height);
				}
			}
			RilisoftRotator.RotateCharacter(this.pers, rotationRateForCharacterInMenues, touchZone, ref this.idleTimerLastTime, ref this.lastTime, () => MainMenuController.canRotationLobbyPlayer && !ShopNGUIController.GuiActive && !this.SettingsJoysticksPanel.activeInHierarchy);
		}
		if (Time.realtimeSinceStartup - this.idleTimerLastTime > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnPersTonNormState();
		}
		if (this._starterPackEnabled)
		{
			this.starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
		}
		this.RefreshChestButton();
		if (this._newClanIncomingInvitesSprite.Value != null)
		{
			if (ClanIncomingInvitesController.CurrentRequest == null || !ClanIncomingInvitesController.CurrentRequest.IsCompleted)
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else if (ClanIncomingInvitesController.CurrentRequest.IsCanceled || ClanIncomingInvitesController.CurrentRequest.IsFaulted)
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(false);
			}
			else
			{
				this._newClanIncomingInvitesSprite.Value.gameObject.SetActive(ClanIncomingInvitesController.CurrentRequest.Result.Count > 0);
			}
		}
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x0006D8CC File Offset: 0x0006BACC
	private void RefreshChestButton()
	{
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x0006D8D0 File Offset: 0x0006BAD0
	private void HandleEscape()
	{
		if (this._backSubscribers.Count > 0)
		{
			this.InvokeLastBackHandler();
		}
		else
		{
			this._isCancellationRequested = true;
		}
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x0006D8F8 File Offset: 0x0006BAF8
	private void ReturnPersTonNormState()
	{
		HOTween.Kill(this.pers);
		Vector3 p_endVal = new Vector3(-0.33f, 138f, -0.28f);
		this.idleTimerLastTime = Time.realtimeSinceStartup;
		HOTween.To(this.pers, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(p_endVal)).Ease(EaseType.Linear).OnComplete(delegate()
		{
			this.idleTimerLastTime = Time.realtimeSinceStartup;
		}));
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x0006D970 File Offset: 0x0006BB70
	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x0006D990 File Offset: 0x0006BB90
	private new void OnEnable()
	{
		base.OnEnable();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Main Menu Controller");
		RewardedLikeButton[] componentsInChildren = base.GetComponentsInChildren<RewardedLikeButton>(true);
		foreach (RewardedLikeButton rewardedLikeButton in componentsInChildren)
		{
			rewardedLikeButton.Refresh();
		}
		if (ExperienceController.sharedController != null && !ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.InterfaceEnabled = true;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(true);
		}
		if (MainMenuController.onEnableMenuForAskname != null)
		{
			MainMenuController.onEnableMenuForAskname();
		}
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x0006DA6C File Offset: 0x0006BC6C
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		if (MainMenuController.onActiveMainMenu != null)
		{
			MainMenuController.onActiveMainMenu(false);
		}
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x0006DAAC File Offset: 0x0006BCAC
	private void HandleAgreeClicked(object sender, EventArgs e)
	{
		Defs.isShowUserAgrement = false;
		this.UserAgreementPanel.SetActive(false);
		if (this.notShowAgain.value)
		{
			PlayerPrefs.SetInt("UserAgreement", 1);
		}
		if (this.isMultyPress)
		{
			this.GoMulty();
		}
		if (this.isFriendsPress)
		{
			this.GoFriens();
		}
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x0006DB08 File Offset: 0x0006BD08
	private void HandleDiclineClicked(object sender, EventArgs e)
	{
		Defs.isShowUserAgrement = false;
		this.UserAgreementPanel.SetActive(false);
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x0006DB1C File Offset: 0x0006BD1C
	public void ShowBankWindow()
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (this.LeaderboardsIsOpening)
		{
			return;
		}
		if (this._shopInstance != null)
		{
			Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested += this.HandleBackFromBankClicked;
		if ((GiftBannerWindow.instance == null || !GiftBannerWindow.instance.IsShow) && MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this._bankEnteredTime = Time.realtimeSinceStartup;
		ButtonClickSound.Instance.PlayClick();
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(false);
		}
		if (this.nicknameLabel != null)
		{
			this.nicknameLabel.transform.root.gameObject.SetActive(false);
		}
		BankController.Instance.InterfaceEnabled = true;
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x0006DC60 File Offset: 0x0006BE60
	private void HandleBankClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x0006DC68 File Offset: 0x0006BE68
	private void HandleBackFromBankClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			Debug.LogWarning("_shopInstance != null");
			return;
		}
		if (BankController.Instance == null)
		{
			Debug.LogWarning("bankController == null");
			return;
		}
		if (BankController.Instance.InterfaceEnabledCoroutineLocked)
		{
			Debug.LogWarning("InterfaceEnabledCoroutineLocked");
			return;
		}
		BankController.Instance.BackRequested -= this.HandleBackFromBankClicked;
		BankController.Instance.InterfaceEnabled = false;
		if (this.nicknameLabel != null)
		{
			this.nicknameLabel.transform.root.gameObject.SetActive(true);
		}
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(true);
		}
		if (this.singleModePanel != null && this.singleModePanel.activeSelf)
		{
			ExperienceController.SetEnable(true);
		}
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x0006DD68 File Offset: 0x0006BF68
	private void HandleSupportButtonClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.settingsPanel.SetActive(false);
		this._feedbackPanel.Value.gameObject.SetActive(true);
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x0006DDB8 File Offset: 0x0006BFB8
	public void StartCampaingButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		Action action = delegate()
		{
			Defs.isFlag = false;
			Defs.isCOOP = false;
			Defs.isMulti = false;
			Defs.isHunger = false;
			Defs.isDuel = false;
			Defs.isCompany = false;
			Defs.IsSurvival = false;
			Defs.isCapturePoints = false;
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset(0);
			StoreKitEventListener.State.Mode = "Campaign";
			StoreKitEventListener.State.PurchaseKey = "In game";
			StoreKitEventListener.State.Parameters.Clear();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(Defs.RankParameterKey, ExperienceController.sharedController.currentLevel.ToString());
			dictionary.Add(Defs.MultiplayerModesKey, StoreKitEventListener.State.Mode);
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = "CampaignChooseBox";
			LoadConnectScene.noteToShow = null;
			SceneManager.LoadScene(Defs.PromSceneName);
		};
		action();
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x0006DE14 File Offset: 0x0006C014
	private void HandleCampaingClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.StartCampaingButton();
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x0006DE3C File Offset: 0x0006C03C
	public void StartSurvivalButton()
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		Action action = delegate()
		{
			Defs.isFlag = false;
			Defs.isCOOP = false;
			Defs.isMulti = false;
			Defs.isHunger = false;
			Defs.isDuel = false;
			Defs.isCompany = false;
			Defs.isCapturePoints = false;
			Defs.IsSurvival = true;
			CurrentCampaignGame.levelSceneName = string.Empty;
			GlobalGameController.Score = 0;
			WeaponManager.sharedManager.Reset(0);
			StoreKitEventListener.State.Mode = "Survival";
			StoreKitEventListener.State.PurchaseKey = "In game";
			StoreKitEventListener.State.Parameters.Clear();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(Defs.RankParameterKey, ExperienceController.sharedController.currentLevel.ToString());
			dictionary.Add(Defs.MultiplayerModesKey, StoreKitEventListener.State.Mode);
			Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, Defs.SurvivalMaps.Length);
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading", LoadSceneMode.Single);
		};
		action();
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x0006DE98 File Offset: 0x0006C098
	public void HandleSurvivalClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.StartSurvivalButton();
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0006DEC0 File Offset: 0x0006C0C0
	public void HandleSandboxClicked()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!ProtocolListGetter.currentVersionIsSupported)
		{
			BannerWindowController bannerWindowController = BannerWindowController.SharedController;
			if (bannerWindowController != null)
			{
				bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
			}
		}
		else
		{
			this.GoSandBox();
		}
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x0006DF44 File Offset: 0x0006C144
	public void GoSandBox()
	{
		ButtonClickSound.Instance.PlayClick();
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		MenuBackgroundMusic.keepPlaying = true;
		string path = ConnectSceneNGUIController.MainLoadingTexture();
		LoadConnectScene.textureToShow = Resources.Load<Texture>(path);
		LoadConnectScene.sceneToLoad = "ConnectSceneSandbox";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x0006DFC4 File Offset: 0x0006C1C4
	private void GoMulty()
	{
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isMulti = true;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCompany = false;
		Defs.IsSurvival = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		MenuBackgroundMusic.keepPlaying = true;
		string path = ConnectSceneNGUIController.MainLoadingTexture();
		LoadConnectScene.textureToShow = Resources.Load<Texture>(path);
		LoadConnectScene.sceneToLoad = "ConnectScene";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x0006E038 File Offset: 0x0006C238
	public void OnClickMultiplyerButton()
	{
		ButtonClickSound.Instance.PlayClick();
		Action action = delegate()
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				this.GoMulty();
			}
		};
		action();
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x0006E064 File Offset: 0x0006C264
	public void HandleMultiPlayerClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		this.OnClickMultiplyerButton();
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x0006E08C File Offset: 0x0006C28C
	private void HandleSkinsMakerClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 0);
		GlobalGameController.EditingCape = 0;
		GlobalGameController.EditingLogo = 0;
		Singleton<SceneLoader>.Instance.LoadScene("SkinEditor", LoadSceneMode.Single);
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x0006E108 File Offset: 0x0006C308
	private IEnumerator HideMenuInterfaceCoroutine(GameObject nickLabelObj)
	{
		yield return null;
		if (nickLabelObj != null)
		{
			nickLabelObj.SetActive(false);
		}
		this.rotateCamera.gameObject.SetActive(false);
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x0006E134 File Offset: 0x0006C334
	private void GoFriens()
	{
		MenuBackgroundMusic.keepPlaying = true;
		if (FriendsWindowGUI.Instance == null)
		{
			Debug.LogWarning("FriendsWindowController.Instance == null");
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		FriendsController.sharedController.GetFriendsData(true);
		ButtonClickSound.Instance.PlayClick();
		GameObject nickLabelObj = null;
		if (NickLabelStack.sharedStack != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(false);
		}
		if (!MainMenuController.friendsOnStart)
		{
			base.StartCoroutine(this.HideMenuInterfaceCoroutine(nickLabelObj));
		}
		FriendsWindowGUI.Instance.ShowInterface(delegate
		{
			NickLabelStack.sharedStack.gameObject.SetActive(true);
			this.rotateCamera.gameObject.SetActive(true);
			if (this.mainPanel != null)
			{
				this.mainPanel.transform.root.gameObject.SetActive(true);
			}
		});
		FriendsController.sharedController.DownloadDataAboutPossibleFriends();
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x0006E1DC File Offset: 0x0006C3DC
	private void HandleFriendsClicked(object sender, EventArgs e)
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		Action action = delegate()
		{
			if (!ProtocolListGetter.currentVersionIsSupported)
			{
				BannerWindowController bannerWindowController = BannerWindowController.SharedController;
				if (bannerWindowController != null)
				{
					bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
				}
			}
			else
			{
				this.GoFriens();
			}
		};
		action();
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x0006E250 File Offset: 0x0006C450
	private void HandleNewsButtonClicked(object sender, EventArgs e)
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		this._newsPanel.Value.gameObject.SetActive(true);
		this.mainPanel.SetActive(false);
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x0006E2B8 File Offset: 0x0006C4B8
	private void HandleProfileClicked(object sender, EventArgs e)
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		this.GoToProfile();
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x0006E2CC File Offset: 0x0006C4CC
	public void GoToProfile()
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		ButtonClickSound.Instance.PlayClick();
		PlayerPrefs.SetInt(Defs.ProfileEnteredFromMenu, 0);
		if (ProfileController.Instance == null)
		{
			Debug.LogWarning("ProfileController.Instance == null");
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (NickLabelStack.sharedStack.gameObject != null)
		{
			NickLabelStack.sharedStack.gameObject.SetActive(false);
		}
		if (this.mainPanel != null)
		{
			this.mainPanel.transform.root.gameObject.SetActive(false);
		}
		ProfileController.Instance.ShowInterface(new Action[]
		{
			delegate()
			{
				if (NickLabelStack.sharedStack.gameObject != null)
				{
					NickLabelStack.sharedStack.gameObject.SetActive(true);
				}
				if (this.mainPanel != null)
				{
					this.mainPanel.transform.root.gameObject.SetActive(true);
				}
			}
		});
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x0006E3DC File Offset: 0x0006C5DC
	private void HandleFreeClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.settingsPanel.SetActive(false);
		this._freePanel.Value.SetVisible(true);
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x0006E444 File Offset: 0x0006C644
	private void HandleGameServicesClicked(object sender, EventArgs e)
	{
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x0006E448 File Offset: 0x0006C648
	private void HandleResumeFromShop()
	{
		if (this._shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = delegate()
			{
			};
			this._shopInstance = null;
			if (NickLabelStack.sharedStack != null)
			{
				NickLabelStack.sharedStack.gameObject.SetActive(true);
			}
			if (StarterPackController.Get != null && StarterPackController.Get.isEventActive)
			{
				StarterPackController.Get.CheckShowStarterPack();
			}
			base.StartCoroutine(MainMenuController.ShowRanks());
		}
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x0006E4F0 File Offset: 0x0006C6F0
	public static IEnumerator ShowRanks()
	{
		for (int i = 0; i < 9; i++)
		{
			yield return null;
		}
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
		yield break;
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x0006E504 File Offset: 0x0006C704
	private static void UnequipSniperRifleAndArmryArmoIfNeeded()
	{
		try
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
			{
				int trainingStep = AnalyticsStuff.TrainingStep;
				if (Storager.getInt("Training.NoviceArmorUsedKey", false) != 1 && trainingStep < 12 && Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNewNoneEqupped)
				{
					ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
				}
				if (trainingStep < 10 && WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
				{
					if ((from w in WeaponManager.sharedManager.playerWeapons.OfType<Weapon>()
					select w.weaponPrefab.GetComponent<WeaponSounds>()).FirstOrDefault((WeaponSounds ws) => ws.categoryNabor - 1 == 4) != null)
					{
						WeaponManager.sharedManager.SaveWeaponSet(Defs.MultiplayerWSSN, string.Empty, 4);
						WeaponManager.sharedManager.Reset(0);
					}
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in UnequipSniperRifleAndArmryArmoIfNeeded: " + arg);
		}
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x0006E644 File Offset: 0x0006C844
	public void HandleShopClicked(object sender, EventArgs e)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) != 1)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this._shopInstance = ShopNGUIController.sharedShop;
		if (this._shopInstance != null)
		{
			MainMenuController.UnequipSniperRifleAndArmryArmoIfNeeded();
			this._shopInstance.SetInGame(false);
			ShopNGUIController.GuiActive = true;
			this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
		}
		else
		{
			Debug.LogWarning("sharedShop == null");
		}
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x0006E704 File Offset: 0x0006C904
	private void HandleSettingsClicked(object sender, EventArgs e)
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		if (this._shopInstance != null)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.rotateCamera.OnMainMenuOpenOptions();
		ButtonClickSound.Instance.PlayClick();
		base.StartCoroutine(this.OpenSettingPanelWithDelay());
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x0006E77C File Offset: 0x0006C97C
	private IEnumerator OpenSettingPanelWithDelay()
	{
		yield return null;
		this.settingsPanel.SetActive(true);
		this.mainPanel.SetActive(false);
		AnimationGift.instance.CheckVisibleGift();
		yield break;
	}

	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x0006E798 File Offset: 0x0006C998
	public static string RateUsURL
	{
		get
		{
			string result = Defs2.ApplicationUrl;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				result = "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return result;
		}
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x0006E7CC File Offset: 0x0006C9CC
	public void RateUs()
	{
		if (MainMenuController.ShopOpened)
		{
			return;
		}
		if (MainMenuController.ShowBannerOrLevelup())
		{
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		MobileAdManager.Instance.SuppressShowOnReturnFromPause = true;
		Application.OpenURL(MainMenuController.RateUsURL);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x0006E824 File Offset: 0x0006CA24
	public static void SetInputEnabled(bool enabled)
	{
		if (MainMenuController.sharedController != null && !ShopNGUIController.GuiActive)
		{
			MainMenuController.sharedController.uiCamera.enabled = enabled;
		}
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x0006E85C File Offset: 0x0006CA5C
	private void OnEventX3Updated()
	{
		this.eventX3RemainTime[0].gameObject.SetActive(PromoActionsManager.sharedManager.IsEventX3Active);
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x0006E87C File Offset: 0x0006CA7C
	private void OnStarterPackContainerShow(bool enable)
	{
		Task<TrafficForwardingInfo> task = FriendsController.sharedController.Map((FriendsController f) => f.GetComponent<TrafficForwardingScript>()).Map((TrafficForwardingScript t) => t.GetTrafficForwardingInfo()).Filter((Task<TrafficForwardingInfo> t) => t.IsCompleted && !t.IsCanceled && !t.IsFaulted);
		bool flag = (task == null || !this.TrafficForwardingEnabled(task.Result)) && enable;
		this.starterPackPanel.gameObject.SetActive(flag);
		if (flag)
		{
			this.buttonBackground.mainTexture = StarterPackController.Get.GetCurrentPackImage();
		}
		this._starterPackEnabled = flag;
		this.starterPackTimer.text = StarterPackController.Get.GetTimeToEndEvent();
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x0006E960 File Offset: 0x0006CB60
	public void OnStarterPackButtonClick()
	{
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		BannerWindowController bannerWindowController = BannerWindowController.SharedController;
		if (bannerWindowController == null)
		{
			return;
		}
		bannerWindowController.ForceShowBanner(BannerWindowType.StarterPack);
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x0006E99C File Offset: 0x0006CB9C
	public void HandleTrafficForwardingClicked()
	{
		if (string.IsNullOrEmpty(this._trafficForwardingUrl))
		{
			Debug.LogError("HandleTrafficForwardingClicked() called while trafficForwardingUrl is empty.");
			return;
		}
		try
		{
			int @int = PlayerPrefs.GetInt("TrafficForwarded", 0);
			PlayerPrefs.SetInt("TrafficForwarded", @int + 1);
			AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Press);
			FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.click);
		}
		finally
		{
			TrafficForwardingScript trafficForwardingScript = FriendsController.sharedController.Map((FriendsController fc) => fc.GetComponent<TrafficForwardingScript>());
			if (trafficForwardingScript != null)
			{
				Task<TrafficForwardingInfo> trafficForwardingInfo = trafficForwardingScript.GetTrafficForwardingInfo();
				TrafficForwardingInfo e = (!trafficForwardingInfo.IsCompleted || trafficForwardingInfo.IsCanceled || trafficForwardingInfo.IsFaulted) ? TrafficForwardingInfo.DisabledInstance : trafficForwardingInfo.Result;
				this.RefreshTrafficForwardingButton(this, e);
			}
			else
			{
				this.RefreshTrafficForwardingButton(this, TrafficForwardingInfo.DisabledInstance);
			}
		}
		Application.OpenURL(this._trafficForwardingUrl);
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x0006EA9C File Offset: 0x0006CC9C
	private bool TrafficForwardingEnabled(TrafficForwardingInfo e)
	{
		return PlayerPrefs.GetInt("TrafficForwarded", 0) < 1 && !MainMenuController.SavedShwonLobbyLevelIsLessThanActual() && TrainingController.TrainingCompleted && e.Enabled && ExperienceController.sharedController.currentLevel >= e.MinLevel && ExperienceController.sharedController.currentLevel <= e.MaxLevel;
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x0006EB08 File Offset: 0x0006CD08
	private void RefreshTrafficForwardingButton(object sender, TrafficForwardingInfo e)
	{
		if (e == null)
		{
			Debug.LogError("Null TrafficForwardingInfo passed.");
			e = TrafficForwardingInfo.DisabledInstance;
		}
		this._trafficForwardingUrl = e.Url;
		bool enabled = false;
		try
		{
			if (!(this == null))
			{
				enabled = this.TrafficForwardingEnabled(e);
				if (enabled && PlayerPrefs.GetInt(Defs.TrafficForwardingShowAnalyticsSent, 0) == 0)
				{
					AnalyticsStuff.LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode.Show);
					PlayerPrefs.SetInt(Defs.TrafficForwardingShowAnalyticsSent, 1);
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.newView);
				}
				else if (enabled)
				{
					FriendsController.LogPromoTrafficForwarding(FriendsController.TypeTrafficForwardingLog.view);
				}
				MainMenuController.trafficForwardActive = enabled;
				ButtonBannerHUD.OnUpdateBanners();
				this.trafficForwardingButton.Do(delegate(UIButton tf)
				{
					tf.gameObject.SetActive(enabled);
				});
			}
		}
		finally
		{
			this.OnStarterPackContainerShow(!enabled && StarterPackController.Get.isEventActive);
		}
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x0006EC14 File Offset: 0x0006CE14
	public void OnBuySmilesClick()
	{
		if (SkinEditorController.sharedController != null)
		{
			return;
		}
		BannerWindowController bannerWindowController = BannerWindowController.SharedController;
		if (bannerWindowController == null)
		{
			return;
		}
		bannerWindowController.ForceShowBanner(BannerWindowType.buySmiles);
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x0006EC50 File Offset: 0x0006CE50
	public void OnShowBannerGift()
	{
		BannerWindowController bannerWindowController = BannerWindowController.SharedController;
		if (bannerWindowController == null)
		{
			return;
		}
		bannerWindowController.ForceShowBanner(BannerWindowType.GiftBonuse);
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x0006EC78 File Offset: 0x0006CE78
	public void HandleLeaderboardsClicked()
	{
		this.ShowLeaderboards(null);
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x0006EC94 File Offset: 0x0006CE94
	public void ShowLeaderboards(LeaderboardsView.State? state = null)
	{
		if (!this.mainPanel.activeInHierarchy || this._leaderboardsIsOpening || this.InAdventureScreen || FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		base.StartCoroutine(this.HandleLeaderboardsClickedCoroutine(state));
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x0006ECE8 File Offset: 0x0006CEE8
	private IEnumerator ContinueWithCoroutine(Task task, Action<Task> continuation)
	{
		if (task == null)
		{
			throw new ArgumentNullException("task");
		}
		if (continuation == null)
		{
			yield break;
		}
		while (!task.IsCompleted)
		{
			yield return null;
		}
		continuation(task);
		yield break;
	}

	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0006ED18 File Offset: 0x0006CF18
	public bool LeaderboardsIsOpening
	{
		get
		{
			return this._leaderboardsIsOpening;
		}
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x0006ED20 File Offset: 0x0006CF20
	private IEnumerator HandleLeaderboardsClickedCoroutine(LeaderboardsView.State? toState = null)
	{
		this._leaderboardsIsOpening = true;
		this._leaderboardScript.Value.Show();
		if (this.mainPanel == null || this.LeaderboardsPanel == null || !this.mainPanel.activeInHierarchy || this.LeaderboardsPanel.gameObject.activeInHierarchy || this._leaderboardScript.Value == null)
		{
			this._leaderboardsIsOpening = false;
			yield break;
		}
		Action<Task> backHandler = delegate(Task t)
		{
			this.LeaderboardsPanel.gameObject.SetActive(false);
			this.mainPanel.SetActive(true);
			foreach (UIButton uibutton in this._leaderboardsButton.Value)
			{
				uibutton.isEnabled = true;
			}
		};
		base.StartCoroutine(this.ContinueWithCoroutine(this._leaderboardScript.Value.GetReturnFuture(), backHandler));
		this._leaderboardScript.Value.RefreshMyLeaderboardEntries();
		foreach (UIButton b in this._leaderboardsButton.Value)
		{
			b.isEnabled = false;
		}
		this.LeaderboardsPanel.gameObject.SetActive(true);
		this.LeaderboardsPanel.alpha = float.Epsilon;
		LeaderboardsView view = this.LeaderboardsPanel.Map((UIPanel p) => p.GetComponent<LeaderboardsView>());
		if (view != null)
		{
			while (!view.Prepared)
			{
				yield return null;
			}
			if (toState != null)
			{
				view.CurrentState = toState.Value;
			}
			else
			{
				int stateInt = PlayerPrefs.GetInt("Leaderboards.TabCache", 3);
				LeaderboardsView.State state = (LeaderboardsView.State)((!Enum.IsDefined(typeof(LeaderboardsView.State), stateInt)) ? 3 : stateInt);
				view.CurrentState = ((state == LeaderboardsView.State.None) ? LeaderboardsView.State.BestPlayers : state);
			}
		}
		this.mainPanel.SetActive(false);
		this.LeaderboardsPanel.alpha = 1f;
		foreach (UIButton b2 in this._leaderboardsButton.Value)
		{
			b2.isEnabled = true;
		}
		this._leaderboardsIsOpening = false;
		yield break;
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x0006ED4C File Offset: 0x0006CF4C
	public bool InappBonusChestCanShow()
	{
		return !this.SettingsJoysticksPanel.activeInHierarchy && !this.settingsPanel.activeInHierarchy && !this.FreePanelIsActive && !this.singleModePanel.activeInHierarchy && (!(FeedbackMenuController.Instance != null) || !FeedbackMenuController.Instance.gameObject.activeInHierarchy);
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x0006EDBC File Offset: 0x0006CFBC
	private void UpdateInappBonusChestActiveState()
	{
		bool flag = BalanceController.isActiveInnapBonus() && this.InappBonusChestCanShow();
		if (InAppBonusLobbyController.Instance != null && InAppBonusLobbyController.Instance.Enabled != flag)
		{
			InAppBonusLobbyController.Instance.Enabled = flag;
		}
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x0006EE08 File Offset: 0x0006D008
	private void OnDayOfValorContainerShow(bool enable)
	{
		this.dayOfValorContainer.gameObject.SetActive(enable);
		this._dayOfValorEnabled = enable;
		this.dayOfValorTimer.text = PromoActionsManager.sharedManager.GetTimeToEndDaysOfValor();
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x0006EE44 File Offset: 0x0006D044
	public void OnDayOfValorButtonClick()
	{
		BannerWindowController bannerWindowController = BannerWindowController.SharedController;
		if (bannerWindowController == null)
		{
			return;
		}
		bannerWindowController.ForceShowBanner(BannerWindowType.DaysOfValor);
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x0006EE6C File Offset: 0x0006D06C
	public void HandlePremiumClicked()
	{
		ShopNGUIController.ShowPremimAccountExpiredIfPossible(this.RentExpiredPoint, "NGUI", string.Empty, false);
	}

	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06001AFD RID: 6909 RVA: 0x0006EE88 File Offset: 0x0006D088
	// (set) Token: 0x06001AFE RID: 6910 RVA: 0x0006EE90 File Offset: 0x0006D090
	public bool InAdventureScreen
	{
		get
		{
			return this.inAdventureScreen;
		}
		private set
		{
			this.inAdventureScreen = value;
		}
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x0006EE9C File Offset: 0x0006D09C
	private IEnumerator SetActiveSinglePanel(bool isActive)
	{
		this.InAdventureScreen = isActive;
		this.mainPanel.SetActive(!isActive);
		this.singleModePanel.SetActive(isActive);
		FreeAwardShowHandler.CheckShowChest(isActive);
		ExperienceController.SetEnable(isActive && !this.stubLoading.activeSelf);
		if (isActive)
		{
			this.survivalButton.GetComponent<UIButton>().isEnabled = false;
			yield return null;
			this.survivalButton.GetComponent<UIButton>().isEnabled = true;
		}
		yield break;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x0006EEC8 File Offset: 0x0006D0C8
	public void OnClickSingleModeButton()
	{
		if (this.LeaderboardsIsOpening)
		{
			return;
		}
		if (!ProtocolListGetter.currentVersionIsSupported)
		{
			BannerWindowController bannerWindowController = BannerWindowController.SharedController;
			if (bannerWindowController != null)
			{
				bannerWindowController.ForceShowBanner(BannerWindowType.NewVersion);
			}
			return;
		}
		Defs.isDaterRegim = false;
		base.StartCoroutine(this.SetActiveSinglePanel(true));
		this.rotateCamera.OnOpenSingleModePanel();
		this._parentBankPanel = this.coinsShopButton.transform.parent;
		this.coinsShopButton.transform.parent = this.singleModePanel.transform;
		int num = 0;
		foreach (KeyValuePair<string, Dictionary<string, int>> keyValuePair in CampaignProgress.boxesLevelsAndStars)
		{
			foreach (KeyValuePair<string, int> keyValuePair2 in keyValuePair.Value)
			{
				num += keyValuePair2.Value;
			}
		}
		this.singleModeStarsProgress.text = string.Format("{0}: {1}", LocalizationStore.Get("Key_1262"), num + "/60");
		this.singleModeBestScores.text = string.Format("{0} {1}", LocalizationStore.Get("Key_0234"), PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0).ToString());
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x0006F068 File Offset: 0x0006D268
	public void OnClickBackSingleModeButton()
	{
		base.StartCoroutine(MainMenuController.ShowRanks());
		base.StartCoroutine(this.SetActiveSinglePanel(false));
		this.rotateCamera.OnCloseSingleModePanel();
		this.coinsShopButton.transform.parent = this._parentBankPanel;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x0006F0B0 File Offset: 0x0006D2B0
	private void HandleSocialButton(object sender, EventArgs e)
	{
		if (this._leaderboardsIsOpening)
		{
			return;
		}
		if (FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			return;
		}
		this.rotateCamera.OnMainMenuOpenSocial();
		ButtonClickSound.Instance.PlayClick();
		this._freePanel.Value.SetVisible(true);
		this.mainPanel.SetActive(false);
		AnimationGift.instance.CheckVisibleGift();
	}

	// Token: 0x04000FB4 RID: 4020
	internal const string TrafficForwardedKey = "TrafficForwarded";

	// Token: 0x04000FB5 RID: 4021
	private static readonly TaskCompletionSource<bool> _syncPromise;

	// Token: 0x04000FB6 RID: 4022
	public GameObject questButton;

	// Token: 0x04000FB7 RID: 4023
	public GameObject facebookLoginContainer;

	// Token: 0x04000FB8 RID: 4024
	public GameObject twitterLoginContainer;

	// Token: 0x04000FB9 RID: 4025
	public GameObject facebookConnectedSettings;

	// Token: 0x04000FBA RID: 4026
	public GameObject facebookDisconnectedSettings;

	// Token: 0x04000FBB RID: 4027
	public GameObject facebookConnectSettings;

	// Token: 0x04000FBC RID: 4028
	public GameObject twitterConnectedSettings;

	// Token: 0x04000FBD RID: 4029
	public GameObject twitterDisconnectedSettings;

	// Token: 0x04000FBE RID: 4030
	public GameObject twitterConnectSettings;

	// Token: 0x04000FBF RID: 4031
	[SerializeField]
	[Header("subwindows")]
	private GameObject _subwindowsHandler;

	// Token: 0x04000FC0 RID: 4032
	[SerializeField]
	private PrefabHandler _socialBannerPrefab;

	// Token: 0x04000FC1 RID: 4033
	private LazyObject<SocialGunBannerView> _socialBanner;

	// Token: 0x04000FC2 RID: 4034
	[SerializeField]
	private PrefabHandler _freePanelPrefab;

	// Token: 0x04000FC3 RID: 4035
	private LazyObject<MainMenuFreePanel> _freePanel;

	// Token: 0x04000FC4 RID: 4036
	[Header("MainMenuController properties")]
	public Transform topLeftAnchor;

	// Token: 0x04000FC5 RID: 4037
	public GameObject buySmileButton;

	// Token: 0x04000FC6 RID: 4038
	public UIButton premiumButton;

	// Token: 0x04000FC7 RID: 4039
	public GameObject premium;

	// Token: 0x04000FC8 RID: 4040
	public GameObject daysOfValor;

	// Token: 0x04000FC9 RID: 4041
	public GameObject adventureButton;

	// Token: 0x04000FCA RID: 4042
	public GameObject achievementsButton;

	// Token: 0x04000FCB RID: 4043
	public GameObject clansButton;

	// Token: 0x04000FCC RID: 4044
	public GameObject leadersButton;

	// Token: 0x04000FCD RID: 4045
	public UILabel battleNowLabel;

	// Token: 0x04000FCE RID: 4046
	public UILabel trainingNowLabel;

	// Token: 0x04000FCF RID: 4047
	public GameObject friendsGUI;

	// Token: 0x04000FD0 RID: 4048
	public UILabel premiumTime;

	// Token: 0x04000FD1 RID: 4049
	public GameObject premiumUpPlashka;

	// Token: 0x04000FD2 RID: 4050
	public GameObject premiumbottomPlashka;

	// Token: 0x04000FD3 RID: 4051
	public List<GameObject> premiumLevels = new List<GameObject>();

	// Token: 0x04000FD4 RID: 4052
	public GameObject starParticleStarterPackGaemObject;

	// Token: 0x04000FD5 RID: 4053
	public Transform RentExpiredPoint;

	// Token: 0x04000FD6 RID: 4054
	public Transform pers;

	// Token: 0x04000FD7 RID: 4055
	public GameObject completeTraining;

	// Token: 0x04000FD8 RID: 4056
	public GameObject stubLoading;

	// Token: 0x04000FD9 RID: 4057
	public UITexture stubTexture;

	// Token: 0x04000FDA RID: 4058
	public MainMenuHeroCamera rotateCamera;

	// Token: 0x04000FDB RID: 4059
	public static MainMenuController sharedController;

	// Token: 0x04000FDC RID: 4060
	public GameObject campaignButton;

	// Token: 0x04000FDD RID: 4061
	public GameObject survivalButton;

	// Token: 0x04000FDE RID: 4062
	public GameObject multiplayerButton;

	// Token: 0x04000FDF RID: 4063
	public GameObject skinsMakerButton;

	// Token: 0x04000FE0 RID: 4064
	public GameObject friendsButton;

	// Token: 0x04000FE1 RID: 4065
	public GameObject profileButton;

	// Token: 0x04000FE2 RID: 4066
	public GameObject freeButton;

	// Token: 0x04000FE3 RID: 4067
	public GameObject gameCenterButton;

	// Token: 0x04000FE4 RID: 4068
	public GameObject shopButton;

	// Token: 0x04000FE5 RID: 4069
	public GameObject settingsButton;

	// Token: 0x04000FE6 RID: 4070
	public GameObject coinsShopButton;

	// Token: 0x04000FE7 RID: 4071
	public GameObject diclineButton;

	// Token: 0x04000FE8 RID: 4072
	public GameObject agreeButton;

	// Token: 0x04000FE9 RID: 4073
	public GameObject UserAgreementPanel;

	// Token: 0x04000FEA RID: 4074
	public UIButton signOutButton;

	// Token: 0x04000FEB RID: 4075
	public GameObject mainPanel;

	// Token: 0x04000FEC RID: 4076
	public GameObject newsIndicator;

	// Token: 0x04000FED RID: 4077
	[SerializeField]
	[Header("FeedBack")]
	private ButtonHandler _openFeedBackBtn;

	// Token: 0x04000FEE RID: 4078
	[SerializeField]
	private PrefabHandler _feedbackPrefab;

	// Token: 0x04000FEF RID: 4079
	[Header("News")]
	[SerializeField]
	private ButtonHandler _openNewsBtn;

	// Token: 0x04000FF0 RID: 4080
	[SerializeField]
	private PrefabHandler _newsPrefab;

	// Token: 0x04000FF1 RID: 4081
	[Header("Leaderboards")]
	public UIPanel leaderboardsPanel;

	// Token: 0x04000FF2 RID: 4082
	[Header("Misc")]
	public UIToggle notShowAgain;

	// Token: 0x04000FF3 RID: 4083
	public UILabel coinsLabel;

	// Token: 0x04000FF4 RID: 4084
	public GameObject award800to810;

	// Token: 0x04000FF5 RID: 4085
	public UIButton awardOk;

	// Token: 0x04000FF6 RID: 4086
	public GameObject bannerContainer;

	// Token: 0x04000FF7 RID: 4087
	public GameObject nicknameLabel;

	// Token: 0x04000FF8 RID: 4088
	public UIButton developerConsole;

	// Token: 0x04000FF9 RID: 4089
	public UICamera uiCamera;

	// Token: 0x04000FFA RID: 4090
	public GameObject eventX3Window;

	// Token: 0x04000FFB RID: 4091
	public UILabel[] eventX3RemainTime;

	// Token: 0x04000FFC RID: 4092
	public UIButton trafficForwardingButton;

	// Token: 0x04000FFD RID: 4093
	public static bool trafficForwardActive;

	// Token: 0x04000FFE RID: 4094
	private float _eventX3RemainTimeLastUpdateTime;

	// Token: 0x04000FFF RID: 4095
	private readonly Lazy<UISprite> _newClanIncomingInvitesSprite;

	// Token: 0x04001000 RID: 4096
	private AdvertisementController _advertisementController;

	// Token: 0x04001001 RID: 4097
	private ShopNGUIController _shopInstance;

	// Token: 0x04001002 RID: 4098
	private bool isMultyPress;

	// Token: 0x04001003 RID: 4099
	private bool isFriendsPress;

	// Token: 0x04001004 RID: 4100
	private List<GameObject> saveOpenPanel = new List<GameObject>();

	// Token: 0x04001005 RID: 4101
	public static bool canRotationLobbyPlayer = true;

	// Token: 0x04001006 RID: 4102
	private LazyObject<NewsLobbyController> _newsPanel;

	// Token: 0x04001007 RID: 4103
	private LazyObject<FeedbackMenuController> _feedbackPanel;

	// Token: 0x04001008 RID: 4104
	private readonly List<EventHandler> _backSubscribers = new List<EventHandler>();

	// Token: 0x04001009 RID: 4105
	private bool loadReplaceAdmobPerelivRunning;

	// Token: 0x0400100A RID: 4106
	private float _lastTimeInterstitialShown;

	// Token: 0x0400100B RID: 4107
	private static bool _drawLoadingProgress = true;

	// Token: 0x0400100C RID: 4108
	public static bool SingleModeOnStart;

	// Token: 0x0400100D RID: 4109
	public static bool friendsOnStart;

	// Token: 0x0400100E RID: 4110
	private static bool _socialNetworkingInitilized;

	// Token: 0x0400100F RID: 4111
	private Rect campaignRect;

	// Token: 0x04001010 RID: 4112
	private Rect survivalRect;

	// Token: 0x04001011 RID: 4113
	private Rect shopRect;

	// Token: 0x04001012 RID: 4114
	public TweenColor colorBlinkForX3;

	// Token: 0x04001013 RID: 4115
	private string _localizeSaleLabel;

	// Token: 0x04001014 RID: 4116
	private float _timePremiumTimeUpdated;

	// Token: 0x04001015 RID: 4117
	private string lastPrintedDismissReason = string.Empty;

	// Token: 0x04001016 RID: 4118
	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(delegate()
	{
		bool flag = FreeAwardController.Instance.TimeTamperingDetected();
		if (flag)
		{
		}
		return flag;
	});

	// Token: 0x04001017 RID: 4119
	private IDisposable _backSubscription;

	// Token: 0x04001018 RID: 4120
	private float lastTime;

	// Token: 0x04001019 RID: 4121
	private float idleTimerLastTime;

	// Token: 0x0400101A RID: 4122
	private float _bankEnteredTime;

	// Token: 0x0400101B RID: 4123
	private MenuLeaderboardsController _menuLeaderboardsController;

	// Token: 0x0400101C RID: 4124
	public UIPanel starterPackPanel;

	// Token: 0x0400101D RID: 4125
	public UILabel starterPackTimer;

	// Token: 0x0400101E RID: 4126
	public UILabel socialGunEventTimer;

	// Token: 0x0400101F RID: 4127
	public UITexture buttonBackground;

	// Token: 0x04001020 RID: 4128
	private bool _starterPackEnabled;

	// Token: 0x04001021 RID: 4129
	private string _trafficForwardingUrl = "http://pixelgun3d.com/";

	// Token: 0x04001022 RID: 4130
	private bool _leaderboardsIsOpening;

	// Token: 0x04001023 RID: 4131
	private readonly Lazy<UIButton[]> _leaderboardsButton;

	// Token: 0x04001024 RID: 4132
	private readonly Lazy<LeaderboardScript> _leaderboardScript;

	// Token: 0x04001025 RID: 4133
	public UIWidget dayOfValorContainer;

	// Token: 0x04001026 RID: 4134
	public UILabel dayOfValorTimer;

	// Token: 0x04001027 RID: 4135
	private bool _dayOfValorEnabled;

	// Token: 0x04001028 RID: 4136
	public GameObject singleModePanel;

	// Token: 0x04001029 RID: 4137
	public UILabel singleModeBestScores;

	// Token: 0x0400102A RID: 4138
	public UILabel singleModeStarsProgress;

	// Token: 0x0400102B RID: 4139
	private Transform _parentBankPanel;

	// Token: 0x0400102C RID: 4140
	private bool inAdventureScreen;

	// Token: 0x0400102D RID: 4141
	[Header("Social panel settings")]
	public UIButton socialButton;

	// Token: 0x0400102E RID: 4142
	[Header("Chests billboards pointers")]
	public BindedBillboard EggChestBindedBillboard;

	// Token: 0x0400102F RID: 4143
	public BindedBillboard FreeAwardBindedBillboard;

	// Token: 0x04001030 RID: 4144
	public BindedBillboard InAppBonusBindedBillboard;

	// Token: 0x04001031 RID: 4145
	public BindedBillboard LeprechauntBindedBillboard;
}
