using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020003DB RID: 987
public sealed class PauseNGUIController : ControlsSettingsBase
{
	// Token: 0x14000022 RID: 34
	// (add) Token: 0x06002386 RID: 9094 RVA: 0x000B0C24 File Offset: 0x000AEE24
	// (remove) Token: 0x06002387 RID: 9095 RVA: 0x000B0C3C File Offset: 0x000AEE3C
	public static event Action InvertCamUpdated;

	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06002388 RID: 9096 RVA: 0x000B0C54 File Offset: 0x000AEE54
	// (remove) Token: 0x06002389 RID: 9097 RVA: 0x000B0C6C File Offset: 0x000AEE6C
	public static event Action ChatSettUpdated;

	// Token: 0x14000024 RID: 36
	// (add) Token: 0x0600238A RID: 9098 RVA: 0x000B0C84 File Offset: 0x000AEE84
	// (remove) Token: 0x0600238B RID: 9099 RVA: 0x000B0C9C File Offset: 0x000AEE9C
	public static event Action PlayerHandUpdated;

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x0600238C RID: 9100 RVA: 0x000B0CB4 File Offset: 0x000AEEB4
	// (remove) Token: 0x0600238D RID: 9101 RVA: 0x000B0CCC File Offset: 0x000AEECC
	public static event Action SwitchingWeaponsUpdated;

	// Token: 0x0600238E RID: 9102 RVA: 0x000B0CE4 File Offset: 0x000AEEE4
	public static void Set60FPSEnable(bool isChecked, Action handler = null)
	{
		GlobalGameController.is60FPSEnable = !isChecked;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000B0CF0 File Offset: 0x000AEEF0
	private new void Start()
	{
		base.Start();
		PauseNGUIController.sharedController = this;
		this.resumeButton.GetComponent<ButtonHandler>().Clicked += this.HandleResumeButton;
		this.musicToggleButtons.IsChecked = Defs.isSoundMusic;
		this.musicToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			bool isSoundMusic = Defs.isSoundMusic;
			Defs.isSoundMusic = e.IsChecked;
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundMusicSetting, Defs.isSoundMusic);
			PlayerPrefs.Save();
			if (isSoundMusic != Defs.isSoundMusic && isSoundMusic != Defs.isSoundMusic)
			{
				if (Defs.isSoundMusic)
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
					if (MenuBackgroundMusic.sharedMusic != null && gameObject != null)
					{
						AudioSource component = gameObject.GetComponent<AudioSource>();
						if (component != null)
						{
							MenuBackgroundMusic.sharedMusic.PlayMusic(component);
						}
					}
				}
				else
				{
					GameObject gameObject2 = GameObject.FindGameObjectWithTag("BackgroundMusic");
					if (MenuBackgroundMusic.sharedMusic != null && gameObject2 != null)
					{
						AudioSource component2 = gameObject2.GetComponent<AudioSource>();
						if (component2 != null)
						{
							MenuBackgroundMusic.sharedMusic.StopMusic(component2);
						}
					}
				}
			}
		};
		this.soundToggleButtons.IsChecked = Defs.isSoundFX;
		this.soundToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			Defs.isSoundFX = e.IsChecked;
			PlayerPrefsX.SetBool(PlayerPrefsX.SoundFXSetting, Defs.isSoundFX);
			PlayerPrefs.Save();
		};
		this.chatToggleButtons.IsChecked = Defs.IsChatOn;
		this.chatToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			SettingsController.SwitchChatSetting(e.IsChecked, delegate
			{
				Action chatSettUpdated = PauseNGUIController.ChatSettUpdated;
				if (chatSettUpdated != null)
				{
					chatSettUpdated();
				}
			});
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
				Action invertCamUpdated = PauseNGUIController.InvertCamUpdated;
				if (invertCamUpdated != null)
				{
					invertCamUpdated();
				}
			}
		};
		if (this.fps60ToggleButtons != null)
		{
			this.fps60ToggleButtons.IsChecked = !GlobalGameController.is60FPSEnable;
			this.fps60ToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				PauseNGUIController.Set60FPSEnable(e.IsChecked, null);
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
		if (this.leftHandedToggleButtons != null)
		{
			this.leftHandedToggleButtons.IsChecked = GlobalGameController.LeftHanded;
			this.leftHandedToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
			{
				SettingsController.ChangeLeftHandedRightHanded(e.IsChecked, delegate
				{
					Action playerHandUpdated = PauseNGUIController.PlayerHandUpdated;
					if (playerHandUpdated != null)
					{
						playerHandUpdated();
					}
				});
			};
		}
		this.switchingWeaponsToggleButtons.IsChecked = !GlobalGameController.switchingWeaponSwipe;
		this.switchingWeaponsToggleButtons.Clicked += delegate(object sender, ToggleButtonEventArgs e)
		{
			SettingsController.ChangeSwitchingWeaponHanded(e.IsChecked, delegate
			{
				Action switchingWeaponsUpdated = PauseNGUIController.SwitchingWeaponsUpdated;
				if (switchingWeaponsUpdated != null)
				{
					switchingWeaponsUpdated();
				}
			});
		};
		if (this.controlsButton != null)
		{
			this.controlsButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				if (this.InPauseShop())
				{
					return;
				}
				ButtonClickSound.Instance.PlayClick();
				this.settingsPanel.SetActive(false);
				this.SettingsJoysticksPanel.SetActive(true);
				this.swipePanelInSettings.transform.parent.gameObject.SetActive(!Defs.isDaterRegim || !GlobalGameController.switchingWeaponSwipe);
				this.swipePanelInSettings.SetActive(Defs.isDaterRegim && GlobalGameController.switchingWeaponSwipe);
				this.tapPanelInSettings.SetActive(!GlobalGameController.switchingWeaponSwipe);
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
				base.HandleControlsClicked();
			};
		}
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000B10B0 File Offset: 0x000AF2B0
	private void HandleResumeButton(object sender, EventArgs e)
	{
		if (this.InPauseShop())
		{
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000B10CC File Offset: 0x000AF2CC
	private bool InPauseShop()
	{
		return InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.playerMoveC != null && InGameGUI.sharedInGameGUI.playerMoveC.isInappWinOpen;
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000B1108 File Offset: 0x000AF308
	private void Update()
	{
		if (this._isCancellationRequested)
		{
			if (this.SettingsJoysticksPanel.activeInHierarchy)
			{
				this.HandleCancelPosJoystikClicked(this, null);
			}
			else if (!this.InPauseShop())
			{
				this.HandleResumeButton(this, null);
				this._isCancellationRequested = false;
				return;
			}
			this._isCancellationRequested = false;
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
		if (!this.InPauseShop())
		{
			if (this._shopOpened)
			{
				this._lastBackFromShopTime = Time.realtimeSinceStartup;
			}
			this._shopOpened = false;
			if (!Defs.isMulti)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
		else
		{
			this._shopOpened = true;
			this._lastBackFromShopTime = float.PositiveInfinity;
		}
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000B1214 File Offset: 0x000AF414
	private new void OnEnable()
	{
		base.OnEnable();
		if (!this.InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = true;
			ExperienceController.sharedController.posRanks = NetworkStartTable.ExperiencePosRanks;
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Pause");
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000B1284 File Offset: 0x000AF484
	private void OnDestroy()
	{
		PauseNGUIController.sharedController = null;
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000B128C File Offset: 0x000AF48C
	private void OnDisable()
	{
		if (!this.InPauseShop())
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000B12C4 File Offset: 0x000AF4C4
	private void HandleEscape()
	{
		if (!this.InPauseShop())
		{
			this._isCancellationRequested = true;
		}
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000B12D8 File Offset: 0x000AF4D8
	protected override void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		base.HandleSavePosJoystikClicked(sender, e);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x000B12F8 File Offset: 0x000AF4F8
	protected override void HandleCancelPosJoystikClicked(object sender, EventArgs e)
	{
		this.SettingsJoysticksPanel.SetActive(false);
		this.settingsPanel.SetActive(true);
		ExperienceController.sharedController.isShowRanks = true;
		ExpController.Instance.InterfaceEnabled = true;
	}

	// Token: 0x04001803 RID: 6147
	public static PauseNGUIController sharedController;

	// Token: 0x04001804 RID: 6148
	public SettingsToggleButtons switchingWeaponsToggleButtons;

	// Token: 0x04001805 RID: 6149
	public SettingsToggleButtons chatToggleButtons;

	// Token: 0x04001806 RID: 6150
	public SettingsToggleButtons musicToggleButtons;

	// Token: 0x04001807 RID: 6151
	public SettingsToggleButtons soundToggleButtons;

	// Token: 0x04001808 RID: 6152
	public SettingsToggleButtons invertCameraToggleButtons;

	// Token: 0x04001809 RID: 6153
	public SettingsToggleButtons shoot3dTouchToggleButtons;

	// Token: 0x0400180A RID: 6154
	public SettingsToggleButtons jump3dTouchToggleButtons;

	// Token: 0x0400180B RID: 6155
	public SettingsToggleButtons hideJumpAndShootButtons;

	// Token: 0x0400180C RID: 6156
	public SettingsToggleButtons leftHandedToggleButtons;

	// Token: 0x0400180D RID: 6157
	public SettingsToggleButtons fps60ToggleButtons;

	// Token: 0x0400180E RID: 6158
	public UIButton controlsButton;

	// Token: 0x0400180F RID: 6159
	public GameObject tapPanelInSettings;

	// Token: 0x04001810 RID: 6160
	public GameObject swipePanelInSettings;

	// Token: 0x04001811 RID: 6161
	public UISlider sensitivitySlider;

	// Token: 0x04001812 RID: 6162
	public UIButton resumeButton;

	// Token: 0x04001813 RID: 6163
	private IDisposable _backSubscription;

	// Token: 0x04001814 RID: 6164
	private float _cachedSensitivity;

	// Token: 0x04001815 RID: 6165
	private bool _shopOpened;

	// Token: 0x04001816 RID: 6166
	private float _lastBackFromShopTime;
}
