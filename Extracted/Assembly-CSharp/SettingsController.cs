using System;
using System.Collections;
using System.Threading.Tasks;
using I2.Loc;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000755 RID: 1877
internal sealed class SettingsController : MonoBehaviour
{
	// Token: 0x14000096 RID: 150
	// (add) Token: 0x060041D0 RID: 16848 RVA: 0x0015DF88 File Offset: 0x0015C188
	// (remove) Token: 0x060041D1 RID: 16849 RVA: 0x0015DFA0 File Offset: 0x0015C1A0
	public static event Action ControlsClicked;

	// Token: 0x060041D2 RID: 16850 RVA: 0x0015DFB8 File Offset: 0x0015C1B8
	private IEnumerator SynchronizeAmazonCoroutine(UIButton syncButton)
	{
		if (syncButton != null)
		{
			syncButton.isEnabled = false;
		}
		try
		{
			if (!GameCircleSocial.Instance.localUser.authenticated)
			{
				Debug.LogFormat("[Rilisoft] Sign in to GameCircle ({0})", new object[]
				{
					base.GetType().Name
				});
				AGSClient.ShowSignInPage();
			}
			Scene activeScene = SceneManager.GetActiveScene();
			float endTime = Time.realtimeSinceStartup + 60f;
			while (!GameCircleSocial.Instance.localUser.authenticated && Time.realtimeSinceStartup < endTime)
			{
				yield return null;
			}
			if (!GameCircleSocial.Instance.localUser.authenticated || !activeScene.IsValid() || !activeScene.isLoaded)
			{
				Debug.LogWarningFormat("Stop syncing attempt. Scene {0} valid: {1}, loaded: {2}. User authenticated: {3}", new object[]
				{
					activeScene.name,
					activeScene.IsValid(),
					activeScene.isLoaded,
					GameCircleSocial.Instance.localUser.authenticated
				});
				yield break;
			}
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
				if (maxLevel > 0)
				{
					if (ShopNGUIController.GuiActive)
					{
						Debug.LogWarning("Skipping saving to storager while in Shop.");
						yield break;
					}
					if (!StringComparer.Ordinal.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
					{
						Debug.LogWarning("Skipping saving to storager while not Main Menu.");
						yield break;
					}
					TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
					InfoWindowController.ShowRestorePanel(delegate
					{
						CoroutineRunner.Instance.StartCoroutine(MainMenuController.SaveItemsToStorager(delegate
						{
							Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback >: {0:F3}", new object[]
							{
								Time.realtimeSinceStartup
							});
							PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
							if (WeaponManager.sharedManager != null)
							{
								base.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(WeaponManager.sharedManager.CurrentFilterMap, true));
							}
							Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback <: {0:F3}", new object[]
							{
								Time.realtimeSinceStartup
							});
							promise.TrySetResult(true);
						}));
					});
					Task<bool> future = promise.Task;
					while (!future.IsCompleted)
					{
						yield return null;
					}
					ProgressSynchronizer.Instance.SynchronizeAmazonProgress();
					if (WeaponManager.sharedManager != null)
					{
						WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
					}
				}
			}
			StarterPackController.Get.RestoreStarterPackForAmazon();
			this.SetSyncLabelText();
		}
		finally
		{
			if (syncButton != null)
			{
				syncButton.isEnabled = true;
			}
		}
		yield break;
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x0015DFE4 File Offset: 0x0015C1E4
	private void Awake()
	{
		this._lobbySoundsToggleGroup.OnSelectedToggleChanged += this.OnLobbySoundToggleChanged;
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x0015E000 File Offset: 0x0015C200
	private void OnLobbySoundToggleChanged(string name, bool active)
	{
		if (active)
		{
			MenuBackgroundMusic.LobbyBackgroundClip? lobbyBackgroundClip = name.ToEnum(new MenuBackgroundMusic.LobbyBackgroundClip?(MenuBackgroundMusic.LobbyBackgroundClip.None));
			if (lobbyBackgroundClip != null)
			{
				MenuBackgroundMusic.SetBackgroundClip(lobbyBackgroundClip.Value);
			}
		}
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x0015E038 File Offset: 0x0015C238
	public static void SwitchChatSetting(bool on, Action additional = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Chat] button clicked: " + on);
		}
		bool isChatOn = Defs.IsChatOn;
		if (isChatOn != on)
		{
			Defs.IsChatOn = on;
			if (additional != null)
			{
				additional();
			}
		}
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x0015E084 File Offset: 0x0015C284
	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x0015E090 File Offset: 0x0015C290
	public static void ChangeLeftHandedRightHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Left Handed] button clicked: " + isChecked);
		}
		if (GlobalGameController.LeftHanded != isChecked)
		{
			GlobalGameController.LeftHanded = isChecked;
			PlayerPrefs.SetInt(Defs.LeftHandedSN, (!isChecked) ? 0 : 1);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
			if (SettingsController.ControlsClicked != null)
			{
				SettingsController.ControlsClicked();
			}
			if (!isChecked && Application.isEditor)
			{
				Debug.Log("Left-handed Layout Enabled");
			}
		}
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x0015E124 File Offset: 0x0015C324
	public static void ChangeSwitchingWeaponHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Switching Weapon button clicked: " + isChecked);
		}
		if (GlobalGameController.switchingWeaponSwipe == isChecked)
		{
			GlobalGameController.switchingWeaponSwipe = !isChecked;
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, (!GlobalGameController.switchingWeaponSwipe) ? 0 : 1);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
		}
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x0015E190 File Offset: 0x0015C390
	private void SetSyncLabelText()
	{
		UILabel uilabel = null;
		Transform transform = this.syncButton.transform.FindChild("Label");
		if (transform != null)
		{
			uilabel = transform.gameObject.GetComponent<UILabel>();
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (uilabel != null)
			{
				uilabel.text = LocalizationStore.Get("Key_0080");
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && uilabel != null)
		{
			uilabel.text = LocalizationStore.Get("Key_0935");
		}
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x0015E224 File Offset: 0x0015C424
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			this.HandleBackFromSettings(this, EventArgs.Empty);
		}, "Settings");
		this.RefreshSignOutButton();
	}

	// Token: 0x060041DB RID: 16859 RVA: 0x0015E264 File Offset: 0x0015C464
	internal void RefreshSignOutButton()
	{
		if (this.signOutButton != null)
		{
			if (Application.isEditor)
			{
				this.signOutButton.gameObject.SetActive(true);
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				this.signOutButton.gameObject.SetActive(GpgFacade.Instance.IsAuthenticated());
			}
		}
	}

	// Token: 0x060041DC RID: 16860 RVA: 0x0015E2D4 File Offset: 0x0015C4D4
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x0015E2F4 File Offset: 0x0015C4F4
	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		if (this.backButton != null)
		{
			ButtonHandler component = this.backButton.GetComponent<ButtonHandler>();
			component.Clicked += this.HandleBackFromSettings;
		}
		if (this.controlsButton != null)
		{
			ButtonHandler component2 = this.controlsButton.GetComponent<ButtonHandler>();
			component2.Clicked += this.HandleControlsClicked;
		}
		if (this.syncButton != null)
		{
			ButtonHandler component3 = this.syncButton.GetComponent<ButtonHandler>();
			this.SetSyncLabelText();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.syncButton.gameObject.SetActive(true);
				component3.Clicked += this.HandleRestoreClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				this.syncButton.gameObject.SetActive(true);
				component3.Clicked += this.HandleSyncClicked;
			}
			else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				bool active = false;
				this.syncButton.gameObject.SetActive(active);
				component3.Clicked += this.HandleSyncClicked;
			}
		}
		if (this.sensitivitySlider != null)
		{
			float sensitivity = Defs.Sensitivity;
			float num = Mathf.Clamp(sensitivity, 6f, 19f);
			float num2 = num - 6f;
			this.sensitivitySlider.value = num2 / 13f;
			this._cachedSensitivity = num;
		}
		else
		{
			Debug.LogWarning("sensitivitySlider == null");
		}
		if (this.pressureSensitivitySlider != null)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				float touchPressurePower = Defs.touchPressurePower;
				float num3 = Mathf.Clamp(touchPressurePower, 0.3f, 0.88f);
				float num4 = num3 - 0.3f;
				this.pressureSensitivitySlider.value = num4 / 0.58f;
				this._cachedPressure = num3;
			}
			else
			{
				this.pressureSensitivitySlider.gameObject.SetActive(false);
			}
		}
		else
		{
			Debug.LogWarning("sensitivitySlider == null");
		}
		if (this.inviteLocalToggleButton != null)
		{
			this.inviteLocalToggleButton.IsChecked = Defs.isEnableLocalInviteFromFriends;
			this.inviteLocalToggleButton.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				Defs.isEnableLocalInviteFromFriends = e.IsChecked;
			};
		}
		if (this.inviteRemoteToogleButton != null)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer || (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite))
			{
				this.inviteRemoteToogleButton.gameObject.SetActive(true);
				this.inviteRemoteToogleButton.IsChecked = Defs.isEnableRemoteInviteFromFriends;
				this.inviteRemoteToogleButton.Clicked += delegate(object sender, ToggleButtonEventArgs e)
				{
					Defs.isEnableRemoteInviteFromFriends = e.IsChecked;
					RemotePushNotificationController.Instance.UpdateDataOnServer();
				};
			}
			else
			{
				this.inviteRemoteToogleButton.gameObject.SetActive(false);
			}
		}
		this.musicToggleButtons.IsChecked = Defs.isSoundMusic;
		this.musicToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("[Music] button clicked: " + e.IsChecked);
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("MenuBackgroundMusic");
			MenuBackgroundMusic menuBackgroundMusic = (!(gameObject != null)) ? null : gameObject.GetComponent<MenuBackgroundMusic>();
			if (Defs.isSoundMusic != e.IsChecked)
			{
				Defs.isSoundMusic = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
				PlayerPrefs.Save();
				if (menuBackgroundMusic != null)
				{
					if (e.IsChecked)
					{
						menuBackgroundMusic.Play();
					}
					else
					{
						menuBackgroundMusic.Stop();
					}
				}
				else
				{
					Debug.LogWarning("menuBackgroundMusic == null");
				}
			}
		};
		this.soundToggleButtons.IsChecked = Defs.isSoundFX;
		this.soundToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("[Sound] button clicked: " + e.IsChecked);
			}
			if (Defs.isSoundFX != e.IsChecked)
			{
				Defs.isSoundFX = e.IsChecked;
				PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
				PlayerPrefs.Save();
			}
		};
		this.chatToggleButtons.IsChecked = Defs.IsChatOn;
		this.chatToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			SettingsController.SwitchChatSetting(e.IsChecked, null);
		};
		this.invertCameraToggleButtons.IsChecked = (PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1);
		this.invertCameraToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("[Invert Camera] button clicked: " + e.IsChecked);
			}
			bool flag = PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1;
			if (flag != e.IsChecked)
			{
				PlayerPrefs.SetInt(Defs.InvertCamSN, Convert.ToInt32(e.IsChecked));
				PlayerPrefs.Save();
			}
		};
		if (this.leftHandedToggleButtons != null)
		{
			this.leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			this.leftHandedToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				SettingsController.ChangeLeftHandedRightHanded(e.IsChecked, null);
			};
		}
		if (this.fps60ToggleButtons != null)
		{
			this.fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			this.fps60ToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				SettingsController.Set60FPSEnable(e.IsChecked, null);
			};
		}
		if (this.switchingWeaponsToggleButtons != null)
		{
			this.switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
			this.switchingWeaponsToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				SettingsController.ChangeSwitchingWeaponHanded(e.IsChecked, null);
			};
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.shoot3dTouchToggleButtons.gameObject.SetActive(true);
			this.shoot3dTouchToggleButtons.IsChecked = Defs.isUseShoot3DTouch;
			this.shoot3dTouchToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isUseShoot3DTouch = e.IsChecked;
				this.hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			};
		}
		else
		{
			this.shoot3dTouchToggleButtons.gameObject.SetActive(false);
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.jump3dTouchToggleButtons.gameObject.SetActive(true);
			this.jump3dTouchToggleButtons.IsChecked = Defs.isUseJump3DTouch;
			this.jump3dTouchToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isUseJump3DTouch = e.IsChecked;
				this.hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			};
		}
		else
		{
			this.jump3dTouchToggleButtons.gameObject.SetActive(false);
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			this.hideJumpAndShootButtons.gameObject.SetActive(Defs.isUseJump3DTouch || Defs.isUseShoot3DTouch);
			this.hideJumpAndShootButtons.IsChecked = Defs.isJumpAndShootButtonOn;
			this.hideJumpAndShootButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				if (Application.isEditor)
				{
					Debug.Log("3D touche button clicked: " + e.IsChecked);
				}
				Defs.isJumpAndShootButtonOn = e.IsChecked;
			};
		}
		else
		{
			this.hideJumpAndShootButtons.gameObject.SetActive(false);
		}
		this._lobbySoundsToggleGroup.SelectToggle(MenuBackgroundMusic.SettedLobbyBackgrounClip);
	}

	// Token: 0x060041DE RID: 16862 RVA: 0x0015E910 File Offset: 0x0015CB10
	private void Update()
	{
		if (this._backRequested)
		{
			this._backRequested = false;
			this.mainPanel.SetActive(true);
			base.gameObject.SetActive(false);
			if (this.rotateCamera == null)
			{
				GameObject gameObject = GameObject.Find("Camera_Rotate");
				if (gameObject != null)
				{
					this.rotateCamera = gameObject.GetComponent<MainMenuHeroCamera>();
				}
			}
			if (this.rotateCamera != null)
			{
				this.rotateCamera.OnMainMenuCloseOptions();
			}
			AnimationGift.instance.CheckVisibleGift();
			return;
		}
		float num = this.sensitivitySlider.value * 13f;
		float num2 = Mathf.Clamp(num + 6f, 6f, 19f);
		if (this._cachedSensitivity != num2)
		{
			if (Application.isEditor)
			{
				Debug.Log("New sensitivity: " + num2);
			}
			Defs.Sensitivity = num2;
			this._cachedSensitivity = num2;
		}
		if (Defs.touchPressureSupported || Application.isEditor)
		{
			float num3 = this.pressureSensitivitySlider.value * 0.58f;
			float num4 = Mathf.Clamp(num3 + 0.3f, 0.3f, 0.88f);
			if (this._cachedPressure != num4)
			{
				if (Application.isEditor)
				{
					Debug.Log("New pressure: " + num4);
				}
				Defs.touchPressurePower = num4;
				this._cachedPressure = num4;
			}
		}
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x0015EA80 File Offset: 0x0015CC80
	private void HandleBackFromSettings(object sender, EventArgs e)
	{
		this._backRequested = true;
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x0015EA8C File Offset: 0x0015CC8C
	private void HandleControlsClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Controls] button clicked.");
		}
		this.controlsSettings.SetActive(true);
		this.tapPanel.SetActive(!GlobalGameController.switchingWeaponSwipe);
		this.swipePanel.SetActive(false);
		this.swipePanel.transform.parent.gameObject.SetActive(!GlobalGameController.switchingWeaponSwipe);
		base.gameObject.SetActive(false);
		if (SettingsController.ControlsClicked != null)
		{
			SettingsController.ControlsClicked();
		}
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x0015EB1C File Offset: 0x0015CD1C
	private void HandleRestoreClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Restore] button clicked.");
		}
		WeaponManager.RefreshExpControllers();
		ProgressSynchronizer.Instance.SynchronizeIosProgress();
		CoroutineRunner.Instance.StartCoroutine(WeaponManager.sharedManager.ResetCoroutine(0, true));
		CampaignProgressSynchronizer.Instance.Sync();
		SkinsSynchronizer.Instance.Sync();
		AchievementSynchronizer.Instance.Sync();
		PetsSynchronizer.Instance.Sync();
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x0015EB90 File Offset: 0x0015CD90
	private void HandleSyncClicked(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Sync] button clicked.");
			this.RefreshSignOutButton();
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				UIButton uibutton = (sender as MonoBehaviour).Map((MonoBehaviour o) => o.GetComponent<UIButton>());
				CoroutineRunner.Instance.StartCoroutine(this.SynchronizeAmazonCoroutine(uibutton));
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				UIButton syncButton = (sender as MonoBehaviour).Map((MonoBehaviour o) => o.GetComponent<UIButton>());
				if (syncButton != null)
				{
					syncButton.isEnabled = false;
				}
				Action afterAuth = delegate()
				{
					Action<bool> callback = delegate(bool succeeded)
					{
						try
						{
							Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) >: {1:F3}", new object[]
							{
								succeeded,
								Time.realtimeSinceStartup
							});
							if (succeeded && WeaponManager.sharedManager != null)
							{
								WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
							}
							StoreKitEventListener.purchaseInProcess = false;
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
									if (ShopNGUIController.GuiActive)
									{
										Debug.LogWarning("Skipping saving to storager while in Shop.");
										return;
									}
									if (!StringComparer.Ordinal.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
									{
										Debug.LogWarning("Skipping saving to storager while not Main Menu.");
										return;
									}
									TrainingController.OnGetProgress();
									if (HintController.instance != null)
									{
										HintController.instance.ShowNext();
									}
									string text = LocalizationStore.Get("Key_1977");
									Debug.LogFormat("[Rilisoft] > StartCoroutine(SaveItemsToStorager): {1} {0:F3}", new object[]
									{
										Time.realtimeSinceStartup,
										text
									});
									InfoWindowController.ShowRestorePanel(delegate
									{
										CoroutineRunner.Instance.StartCoroutine(MainMenuController.SaveItemsToStorager(delegate
										{
											Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback >: {0:F3}", new object[]
											{
												Time.realtimeSinceStartup
											});
											PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
											if (WeaponManager.sharedManager != null)
											{
												WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
											}
											Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.InnerCallback <: {0:F3}", new object[]
											{
												Time.realtimeSinceStartup
											});
										}));
									});
									Debug.LogFormat("[Rilisoft] < StartCoroutine(SaveItemsToStorager): {1} {0:F3}", new object[]
									{
										Time.realtimeSinceStartup,
										text
									});
								}
							}
							PlayerPrefs.DeleteKey("PendingGooglePlayGamesSync");
							Debug.LogFormat("[Rilisoft] SettingsController.PurchasesSynchronizer.Callback({0}) <: {1:F3}", new object[]
							{
								succeeded,
								Time.realtimeSinceStartup
							});
						}
						finally
						{
							if (syncButton != null)
							{
								syncButton.isEnabled = true;
							}
						}
					};
					if (Application.isEditor)
					{
						Debug.Log("Simulating sync...");
						IEnumerator routine = PurchasesSynchronizer.Instance.SimulateSynchronization(callback);
						CoroutineRunner.Instance.StartCoroutine(routine);
					}
					else
					{
						if (!PurchasesSynchronizer.Instance.SynchronizeIfAuthenticated(callback))
						{
							syncButton.Do(delegate(UIButton s)
							{
								s.isEnabled = true;
							});
						}
						ProgressSynchronizer.Instance.SynchronizeIfAuthenticated(delegate
						{
							if (WeaponManager.sharedManager != null)
							{
								WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
							}
						});
					}
					CampaignProgressSynchronizer.Instance.Sync();
					TrophiesSynchronizer.Instance.Sync();
					SkinsSynchronizer.Instance.Sync();
					AchievementSynchronizer.Instance.Sync();
					PetsSynchronizer.Instance.Sync();
					this.RefreshSignOutButton();
					this.SetSyncLabelText();
				};
				StoreKitEventListener.purchaseInProcess = true;
				CoroutineRunner.Instance.StartCoroutine(this.RestoreProgressIndicator(5f));
				if (GpgFacade.Instance.IsAuthenticated())
				{
					string message = string.Format("Already authenticated: {0}, {1}, {2}", Social.localUser.id, Social.localUser.userName, Social.localUser.state);
					Debug.Log(message);
					afterAuth();
				}
				else
				{
					if (Application.isEditor)
					{
						afterAuth();
						return;
					}
					try
					{
						GpgFacade.Instance.Authenticate(delegate(bool succeeded)
						{
							PlayerPrefs.SetInt("GoogleSignInDenied", Convert.ToInt32(!succeeded));
							if (succeeded)
							{
								Debug.LogFormat("Authentication succeeded: {0}, {1}, {2}", new object[]
								{
									Social.localUser.id,
									Social.localUser.userName,
									Social.localUser.state
								});
								afterAuth();
							}
							else
							{
								Debug.LogWarning("Authentication failed.");
								StoreKitEventListener.purchaseInProcess = false;
								if (syncButton != null)
								{
									syncButton.isEnabled = true;
								}
							}
						}, false);
					}
					catch (InvalidOperationException exception)
					{
						Debug.LogWarning("SettingsController: Exception occured while authenticating with Google Play Games. See next exception message for details.");
						Debug.LogException(exception);
						if (syncButton != null)
						{
							syncButton.isEnabled = true;
						}
					}
				}
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
		}
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x0015ED84 File Offset: 0x0015CF84
	private IEnumerator RestoreProgressIndicator(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		StoreKitEventListener.purchaseInProcess = false;
		yield break;
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x0015EDA8 File Offset: 0x0015CFA8
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
		this._lobbySoundsToggleGroup.OnSelectedToggleChanged -= this.OnLobbySoundToggleChanged;
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0015EDE0 File Offset: 0x0015CFE0
	private void HandleLocalizationChanged()
	{
		this.SetSyncLabelText();
	}

	// Token: 0x04003016 RID: 12310
	public const int SensitivityLowerBound = 6;

	// Token: 0x04003017 RID: 12311
	public const int SensitivityUpperBound = 19;

	// Token: 0x04003018 RID: 12312
	public const float PressureLowerBound = 0.3f;

	// Token: 0x04003019 RID: 12313
	public const float PressureUpperBound = 0.88f;

	// Token: 0x0400301A RID: 12314
	public MainMenuHeroCamera rotateCamera;

	// Token: 0x0400301B RID: 12315
	public UIButton backButton;

	// Token: 0x0400301C RID: 12316
	public UIButton controlsButton;

	// Token: 0x0400301D RID: 12317
	public UIButton syncButton;

	// Token: 0x0400301E RID: 12318
	public UIButton signOutButton;

	// Token: 0x0400301F RID: 12319
	public GameObject controlsSettings;

	// Token: 0x04003020 RID: 12320
	public GameObject tapPanel;

	// Token: 0x04003021 RID: 12321
	public GameObject swipePanel;

	// Token: 0x04003022 RID: 12322
	public GameObject mainPanel;

	// Token: 0x04003023 RID: 12323
	public UISlider sensitivitySlider;

	// Token: 0x04003024 RID: 12324
	public UISlider pressureSensitivitySlider;

	// Token: 0x04003025 RID: 12325
	public SettingsToggleButtons chatToggleButtons;

	// Token: 0x04003026 RID: 12326
	public SettingsToggleButtons musicToggleButtons;

	// Token: 0x04003027 RID: 12327
	public SettingsToggleButtons soundToggleButtons;

	// Token: 0x04003028 RID: 12328
	public SettingsToggleButtons invertCameraToggleButtons;

	// Token: 0x04003029 RID: 12329
	public SettingsToggleButtons jump3dTouchToggleButtons;

	// Token: 0x0400302A RID: 12330
	public SettingsToggleButtons shoot3dTouchToggleButtons;

	// Token: 0x0400302B RID: 12331
	public SettingsToggleButtons hideJumpAndShootButtons;

	// Token: 0x0400302C RID: 12332
	public SettingsToggleButtons leftHandedToggleButtons;

	// Token: 0x0400302D RID: 12333
	public SettingsToggleButtons switchingWeaponsToggleButtons;

	// Token: 0x0400302E RID: 12334
	public SettingsToggleButtons fps60ToggleButtons;

	// Token: 0x0400302F RID: 12335
	public SettingsToggleButtons inviteLocalToggleButton;

	// Token: 0x04003030 RID: 12336
	public SettingsToggleButtons inviteRemoteToogleButton;

	// Token: 0x04003031 RID: 12337
	public Texture googlePlayServicesTexture;

	// Token: 0x04003032 RID: 12338
	[SerializeField]
	private ToggleGroupHalper _lobbySoundsToggleGroup;

	// Token: 0x04003033 RID: 12339
	private IDisposable _backSubscription;

	// Token: 0x04003034 RID: 12340
	private bool _backRequested;

	// Token: 0x04003035 RID: 12341
	private float _cachedSensitivity;

	// Token: 0x04003036 RID: 12342
	private float _cachedPressure;
}
