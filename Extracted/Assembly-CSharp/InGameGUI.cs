using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Holoville.HOTween;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002CA RID: 714
[DisallowMultipleComponent]
public sealed class InGameGUI : MonoBehaviour
{
	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x060018CD RID: 6349 RVA: 0x0005D538 File Offset: 0x0005B738
	public RespawnWindow respawnWindow
	{
		get
		{
			if (this._lazyRespWindow == null)
			{
				this._lazyRespWindow = new LazyObject<RespawnWindow>(this._respawnWindowPrefab.ResourcePath, this.SubpanelsContainer);
			}
			return this._lazyRespWindow.Value;
		}
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x0005D578 File Offset: 0x0005B778
	public void ShowCircularIndicatorOnReload(float length)
	{
		this.StopAllCircularIndicators();
		this.reloadBar.SetActive(true);
		this.reloadLabel.gameObject.SetActive(true);
		base.Invoke("ReloadAmmo", length);
		if (this.playerMoveC != null)
		{
			this.playerMoveC.isReloading = true;
		}
		this.RunCircularSpriteOn(this.reloadCircularSprite, length, delegate
		{
		});
	}

	// Token: 0x060018CF RID: 6351 RVA: 0x0005D5FC File Offset: 0x0005B7FC
	[Obfuscation(Exclude = true)]
	private void ReloadAmmo()
	{
		this.reloadLabel.gameObject.SetActive(false);
		this.reloadBar.SetActive(false);
		WeaponManager.sharedManager.ReloadAmmo();
	}

	// Token: 0x060018D0 RID: 6352 RVA: 0x0005D628 File Offset: 0x0005B828
	public void StartFireCircularIndicators(float length)
	{
		this.StopAllCircularIndicators();
		this.RunCircularSpriteOn(this.fireCircularSprite, length, null);
		this.RunCircularSpriteOn(this.fireAdditionalCrcualrSprite, length, null);
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x0005D658 File Offset: 0x0005B858
	private void RunCircularSpriteOn(UITexture sprite, float length, Action onComplete = null)
	{
		sprite.fillAmount = 0f;
		HOTween.To(sprite, length, new TweenParms().Prop("fillAmount", 1f).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(delegate()
		{
			sprite.fillAmount = 0f;
			if (onComplete != null)
			{
				onComplete();
			}
		}));
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x0005D6CC File Offset: 0x0005B8CC
	public void StopAllCircularIndicators()
	{
		base.CancelInvoke("ReloadAmmo");
		if (this.playerMoveC != null)
		{
			this.playerMoveC.isReloading = false;
		}
		if (this.circularSprites == null)
		{
			Debug.LogWarning("Circular sprites is null!");
			return;
		}
		foreach (UITexture uitexture in this.circularSprites)
		{
			HOTween.Kill(uitexture);
			uitexture.fillAmount = 0f;
		}
		this.reloadLabel.gameObject.SetActive(false);
		this.reloadBar.SetActive(false);
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x0005D768 File Offset: 0x0005B968
	public void PlayLowResourceBeep(int count)
	{
		this.StopPlayingLowResourceBeep();
		this._lowResourceBeepRoutine = this.PlayLowResourceBeepCoroutine(count);
		base.StartCoroutine(this._lowResourceBeepRoutine);
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x0005D798 File Offset: 0x0005B998
	public void SetEnablePerfectLabel(bool enabled)
	{
		if (this.perfectLabels == null)
		{
			return;
		}
		this.perfectLabels.gameObject.SetActive(enabled);
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x0005D7C0 File Offset: 0x0005B9C0
	public void PlayLowResourceBeepIfNotPlaying(int count)
	{
		if (this._lowResourceBeepRoutine != null)
		{
			return;
		}
		this.PlayLowResourceBeep(count);
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x0005D7D8 File Offset: 0x0005B9D8
	public void StopPlayingLowResourceBeep()
	{
		if (this._lowResourceBeepRoutine != null)
		{
			base.StopCoroutine(this._lowResourceBeepRoutine);
			this._lowResourceBeepRoutine = null;
		}
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x0005D7F8 File Offset: 0x0005B9F8
	private IEnumerator PlayLowResourceBeepCoroutine(int count)
	{
		for (int i = 0; i < count; i++)
		{
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.lowResourceBeep);
			}
			yield return new WaitForSeconds(1f);
		}
		this._lowResourceBeepRoutine = null;
		yield break;
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x0005D824 File Offset: 0x0005BA24
	private void HandleChatSettUpdated()
	{
		this.isChatOn = Defs.IsChatOn;
	}

	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x060018D9 RID: 6361 RVA: 0x0005D834 File Offset: 0x0005BA34
	public GameObject SubpanelsContainer
	{
		get
		{
			return this._subpanelsContainer;
		}
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x0005D83C File Offset: 0x0005BA3C
	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[]
		{
			base.GetType().Name
		});
		using (new ScopeLogger(callee, false))
		{
			InGameGUI.sharedInGameGUI = this;
			this.circularSprites = new UITexture[]
			{
				this.reloadCircularSprite,
				this.fireCircularSprite,
				this.fireAdditionalCrcualrSprite
			};
			this.changeWeaponScroll.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, (float)this.widthWeaponScrollPreview * 1.3f, (float)this.widthWeaponScrollPreview * 1.3f);
			this.changeWeaponWrap.itemSize = this.widthWeaponScrollPreview;
			this.HandleChatSettUpdated();
			PauseNGUIController.ChatSettUpdated += this.HandleChatSettUpdated;
			ControlsSettingsBase.ControlsChanged += this.AdjustToPlayerHands;
			if (Defs.isDaterRegim)
			{
				this.shopPanelForTap = this.shopPanelForTapDater;
				this.shopPanelForSwipe = this.shopPanelForSwipeDater;
				this.ammoAddButton = this.ammoAddButtonDater;
				this.healthAddButton = this.healthAddButtonDater;
				for (int i = 0; i < this.weaponCategoriesButtons.Length; i++)
				{
					this.weaponCategoriesButtons[i] = this.weaponCategoriesButtonsDater[i];
				}
				for (int j = 0; j < this.ammoCategoriesLabels.Length; j++)
				{
					this.ammoCategoriesLabels[j] = this.ammoCategoriesLabelsDater[j];
				}
				for (int k = 0; k < this.weaponIcons.Length; k++)
				{
					this.weaponIcons[k] = this.weaponIconsDater[k];
				}
			}
			this.shopPanelForTap.gameObject.SetActive(true);
			this.shopPanelForSwipe.gameObject.SetActive(true);
			InGameGUI.swipeWeaponPanelPos = this.swipeWeaponPanel.localPosition;
			InGameGUI.shopPanelForTapPos = this.shopPanelForTap.localPosition;
			InGameGUI.shopPanelForSwipePos = this.shopPanelForSwipe.localPosition;
			this.SetSwitchingWeaponPanel();
			this.isMulti = Defs.isMulti;
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				this.centerAnhor.SetActive(false);
			}
			this.isInet = Defs.isInet;
			this.isHunger = Defs.isHunger;
			if (this.isHunger)
			{
				HungerGameController instance = HungerGameController.Instance;
				if (instance == null)
				{
					Debug.LogError("hungerGameControllerObject == null");
				}
				else
				{
					this.hungerGameController = instance.GetComponent<HungerGameController>();
				}
			}
			this.aimUpSprite = this.aimUp.GetComponent<UISprite>();
			this.aimDownSprite = this.aimDown.GetComponent<UISprite>();
			this.aimRightSprite = this.aimRight.GetComponent<UISprite>();
			this.aimLeftSprite = this.aimLeft.GetComponent<UISprite>();
			this.aimCenterSprite = this.aimCenter.GetComponent<UISprite>();
			this.aimUpLeftSprite = this.aimUpLeft.GetComponent<UISprite>();
			this.aimDownLeftSprite = this.aimDownLeft.GetComponent<UISprite>();
			this.aimDownRightSprite = this.aimDownRight.GetComponent<UISprite>();
			this.aimUpRightSprite = this.aimUpRight.GetComponent<UISprite>();
			this.impactTween.gameObject.SetActive(false);
		}
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0005DB78 File Offset: 0x0005BD78
	public void ShowImpact()
	{
		this.impactTween.gameObject.SetActive(true);
		this.impactTween.Play(true);
		if (Defs.isSoundFX)
		{
			this.impactTween.GetComponent<UIPlaySound>().Play();
		}
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x0005DBBC File Offset: 0x0005BDBC
	public void SetSwipeWeaponPanelVisibility(bool visible)
	{
		this.swipeWeaponPanel.localPosition = ((!visible) ? (InGameGUI.swipeWeaponPanelPos + new Vector3(10000f, 0f, 0f)) : InGameGUI.swipeWeaponPanelPos);
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x0005DBF8 File Offset: 0x0005BDF8
	public void SetSwitchingWeaponPanel()
	{
		if (GlobalGameController.switchingWeaponSwipe)
		{
			InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition = InGameGUI.swipeWeaponPanelPos;
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(true);
		}
		else
		{
			InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition = new Vector3(10000f, InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition.y, InGameGUI.sharedInGameGUI.swipeWeaponPanel.localPosition.z);
			InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(true);
			InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
			for (int i = 0; i < InGameGUI.sharedInGameGUI.upButtonsInShopPanel.Length; i++)
			{
				if (!PotionsController.sharedController.PotionIsActive(InGameGUI.sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myPotion.name))
				{
					InGameGUI.sharedInGameGUI.upButtonsInShopPanel[i].GetComponent<ElexirInGameButtonController>().myLabelTime.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x0005DD2C File Offset: 0x0005BF2C
	public void AddDamageTaken(float alpha)
	{
		this.curDamageTakenController++;
		if (this.curDamageTakenController >= this.damageTakenControllers.Length)
		{
			this.curDamageTakenController = 0;
		}
		this.damageTakenControllers[this.curDamageTakenController].reset(alpha);
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x0005DD6C File Offset: 0x0005BF6C
	public void ResetDamageTaken()
	{
		for (int i = 0; i < this.damageTakenControllers.Length; i++)
		{
			this.damageTakenControllers[i].Remove();
		}
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x0005DDA0 File Offset: 0x0005BFA0
	private void AdjustToPlayerHands()
	{
		float num = (float)((!GlobalGameController.LeftHanded) ? -1 : 1);
		Vector3[] array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (array == null || array.Length < 7)
		{
			Defs.InitCoordsIphone();
			this.zoomButton.transform.localPosition = new Vector3((float)Defs.ZoomButtonX * num, (float)Defs.ZoomButtonY, this.zoomButton.transform.localPosition.z);
			this.reloadButton.transform.localPosition = new Vector3((float)Defs.ReloadButtonX * num, (float)Defs.ReloadButtonY, this.reloadButton.transform.localPosition.z);
			this.jumpButton.transform.localPosition = new Vector3((float)Defs.JumpButtonX * num, (float)Defs.JumpButtonY, this.jumpButton.transform.localPosition.z);
			this.fireButton.transform.localPosition = new Vector3((float)Defs.FireButtonX * num, (float)Defs.FireButtonY, this.fireButton.transform.localPosition.z);
			this.joystick.transform.localPosition = new Vector3((float)Defs.JoyStickX * num, (float)Defs.JoyStickY, this.joystick.transform.localPosition.z);
			this.grenadeButton.transform.localPosition = new Vector3((float)Defs.GrenadeX * num, (float)Defs.GrenadeY, this.grenadeButton.transform.localPosition.z);
			this.chooseGadgetPanel.transform.localPosition = this.grenadeButton.transform.localPosition;
			this.fireButtonInJoystick.transform.localPosition = new Vector3((float)Defs.FireButton2X * num, (float)Defs.FireButton2Y, this.fireButtonInJoystick.transform.localPosition.z);
		}
		else
		{
			for (int i = 0; i < array.Length; i++)
			{
				Vector3[] array2 = array;
				int num2 = i;
				array2[num2].x = array2[num2].x * num;
			}
			this.zoomButton.transform.localPosition = array[0];
			this.reloadButton.transform.localPosition = array[1];
			this.jumpButton.transform.localPosition = array[2];
			this.fireButton.transform.localPosition = array[3];
			this.joystick.transform.localPosition = array[4];
			this.grenadeButton.transform.localPosition = array[5];
			this.chooseGadgetPanel.transform.localPosition = this.grenadeButton.transform.localPosition;
			this.fireButtonInJoystick.transform.localPosition = array[6];
		}
		UISprite[] array3 = (from go in new GameObject[]
		{
			this.zoomButton,
			this.reloadButton,
			this.jumpButton,
			this.fireButton,
			this.joystick,
			this.grenadeButton,
			this.fireButtonInJoystick,
			this.bottomPanel
		}
		select go.GetComponent<UISprite>()).ToArray<UISprite>();
		object obj = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]"));
		List<object> list = obj as List<object>;
		if (list == null)
		{
			list = new List<object>(array3.Length);
			Debug.LogWarning(list.GetType().FullName);
		}
		int num3 = Math.Min(list.Count, array3.Length);
		for (int num4 = 0; num4 != num3; num4++)
		{
			int num5 = Convert.ToInt32(list[num4]);
			if (num5 > 0)
			{
				UISprite uisprite = array3[num4];
				if (!(uisprite == null))
				{
					array3[num4].keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
					array3[num4].width = num5;
					if (uisprite.gameObject == this.grenadeButton)
					{
						this.chooseGadgetPanel.GetComponent<ChooseGadgetPanel>().gadgetButtonScript.cachedSprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnWidth;
						this.chooseGadgetPanel.GetComponent<ChooseGadgetPanel>().gadgetButtonScript.cachedSprite.width = num5;
					}
					if (uisprite.gameObject == this.joystick)
					{
						UIJoystick component = uisprite.GetComponent<UIJoystick>();
						if (!(component == null))
						{
							float radius = component.radius;
							float num6 = radius / 144f;
							component.ActualRadius = num6 * (float)num5;
						}
					}
				}
			}
		}
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x0005E278 File Offset: 0x0005C478
	private void Start()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.SetSwipeWeaponPanelVisibility(false);
		}
		bool flag = !Device.isPixelGunLow && !Defs.isDuel;
		this.bankView.SetActive(flag);
		this.bankViewLow.SetActive(!flag);
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		if (!Defs.isMulti && !Defs.IsSurvival)
		{
			this.CampaignContainer.SetActive(true);
		}
		if (!Defs.isMulti && Defs.IsSurvival)
		{
			this.survivalContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			if (Defs.isDaterRegim)
			{
				this.daterContainer.SetActive(true);
			}
			else
			{
				this.deathmatchContainer.SetActive(true);
			}
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel)
		{
			this.topPanelsTapReceiver.SetActive(false);
			this.duelContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.timeBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			this.teamBattleContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.flagCaptureContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			this.deadlygamesContainer.SetActive(true);
		}
		if (Defs.isMulti && ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			this.CapturePointContainer.SetActive(true);
		}
		this.turretPanel.SetActive(false);
		if (this.runTurrelButton != null)
		{
			this.runTurrelButton.Clicked += this.RunTurret;
		}
		if (this.cancelTurrelButton != null)
		{
			this.cancelTurrelButton.Clicked += this.CancelTurret;
		}
		if (this.isMulti)
		{
			this.enemiesLeftLabel.SetActive(false);
		}
		else
		{
			this.zombieCreator = ZombieCreator.sharedCreator;
		}
		this.AdjustToPlayerHands();
		PauseNGUIController.PlayerHandUpdated += this.AdjustToPlayerHands;
		PauseNGUIController.SwitchingWeaponsUpdated += this.SetSwitchingWeaponPanel;
		WeaponManager.WeaponEquipped += this.HandleWeaponEquipped;
		int num = (!this.isMulti) ? WeaponManager.sharedManager.CurrentWeaponIndex : WeaponManager.sharedManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons();
		this.HandleWeaponEquipped(((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>());
		if (num < this.changeWeaponWrap.transform.childCount)
		{
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(this.changeWeaponWrap.transform.GetChild(num));
			this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
		}
		else
		{
			Debug.LogError("InGameGUI: not weapon icon with index " + (((Weapon)WeaponManager.sharedManager.playerWeapons[num]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1));
		}
		if (this.gearToogle != null)
		{
			this.gearToogle.gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleGearToogleClicked;
		}
		if (this.weaponCategoriesButtons[0] != null)
		{
			this.weaponCategoriesButtons[0].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandlePrimaryToogleClicked;
		}
		if (this.weaponCategoriesButtons[1] != null)
		{
			this.weaponCategoriesButtons[1].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleBackupToogleClicked;
		}
		if (this.weaponCategoriesButtons[2] != null)
		{
			this.weaponCategoriesButtons[2].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleMeleeToogleClicked;
		}
		if (this.weaponCategoriesButtons[3] != null)
		{
			this.weaponCategoriesButtons[3].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleSpecialToogleClicked;
		}
		if (this.weaponCategoriesButtons[4] != null)
		{
			this.weaponCategoriesButtons[4].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleSniperToogleClicked;
		}
		if (this.weaponCategoriesButtons[5] != null)
		{
			this.weaponCategoriesButtons[5].gameObject.GetComponent<ButtonHandler>().Clicked += this.HandlePremiumToogleClicked;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.gearToogle.GetComponent<UIToggle>().value = false;
			this.HandleGearToogleClicked(null, null);
		}
		for (int i = 0; i < this.upButtonsInShopPanel.Length; i++)
		{
			this.StartUpdatePotionButton(this.upButtonsInShopPanel[i]);
		}
		for (int j = 0; j < this.upButtonsInShopPanelSwipeRegim.Length; j++)
		{
			this.StartUpdatePotionButton(this.upButtonsInShopPanelSwipeRegim[j]);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(-1000f, -1000f, -1f);
			this.gearToogle.isEnabled = false;
		}
		this.SetNGUITouchDragThreshold(1f);
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x0005E814 File Offset: 0x0005CA14
	public void ShowTurretInterface(string nameTurret)
	{
		if (this.turretPanel.activeSelf)
		{
			return;
		}
		this.isTurretInterfaceActive = true;
		this.swipeWeaponPanel.gameObject.SetActive(false);
		this.shopPanelForSwipe.gameObject.SetActive(false);
		this.shopPanelForTap.gameObject.SetActive(false);
		this.runTurrelButton.GetComponent<UIButton>().isEnabled = false;
		this.turretPanel.SetActive(true);
		if (this.playerMoveC != null)
		{
			this.playerMoveC.turretGadgetPrefabName = nameTurret;
		}
		this.playerMoveC.ChangeWeapon(1001, false);
		this._kBlockPauseShopButton = true;
	}

	// Token: 0x060018E3 RID: 6371 RVA: 0x0005E8C0 File Offset: 0x0005CAC0
	public void HideTurretInterface()
	{
		this.isTurretInterfaceActive = false;
		if (GlobalGameController.switchingWeaponSwipe)
		{
			this.shopPanelForSwipe.gameObject.SetActive(true);
		}
		else
		{
			this.shopPanelForTap.gameObject.SetActive(true);
		}
		this.swipeWeaponPanel.gameObject.SetActive(true);
		this.turretPanel.SetActive(false);
		this._kBlockPauseShopButton = false;
	}

	// Token: 0x060018E4 RID: 6372 RVA: 0x0005E92C File Offset: 0x0005CB2C
	private void RunTurret(object sender, EventArgs e)
	{
		if (this.playerMoveC != null)
		{
			this.playerMoveC.RunTurret();
		}
		if (!Defs.isDaterRegim)
		{
			Gadget gadget = null;
			if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget))
			{
				TurretGadget turretGadget = gadget as TurretGadget;
				turretGadget.StartedCurrentTurret(this.playerMoveC.currentTurret.GetComponent<TurretController>());
			}
		}
		this.HideTurretInterface();
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x0005E99C File Offset: 0x0005CB9C
	private void CancelTurret(object sender, EventArgs e)
	{
		if (this.playerMoveC != null)
		{
			this.playerMoveC.CancelTurret();
		}
		this.HideTurretInterface();
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x0005E9CC File Offset: 0x0005CBCC
	private void StartUpdatePotionButton(GameObject potionButton)
	{
		if (potionButton != null)
		{
			potionButton.gameObject.GetComponent<ButtonHandler>().Clicked += this.HandlePotionClicked;
			ElexirInGameButtonController component = potionButton.GetComponent<ElexirInGameButtonController>();
			string name = component.myPotion.name;
			string key = (!Defs.isDaterRegim) ? name : GearManager.HolderQuantityForID(component.idForPriceInDaterRegim);
			if (PotionsController.sharedController.PotionIsActive(name))
			{
				UIButton component2 = potionButton.GetComponent<UIButton>();
				component.isActivePotion = true;
				component.myLabelTime.gameObject.SetActive(true);
				component.myLabelTime.enabled = true;
				component.priceLabel.SetActive(false);
				component.myLabelCount.gameObject.SetActive(true);
				component.plusSprite.SetActive(false);
				component.myLabelCount.text = Storager.getInt(key, false).ToString();
				component2.isEnabled = false;
			}
		}
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
	public void HandleBuyGrenadeClicked(object sender, EventArgs e)
	{
		if (Defs.isDaterRegim)
		{
			string text = GearManager.AnalyticsIDForOneItemOfGear(GearManager.Like, true);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear("LikeID", GearManager.CurrentNumberOfUphradesForGear("LikeID")), ShopNGUIController.CategoryNames.GearCategory);
			ItemPrice itemPrice = new ItemPrice(priceByShopId.Price * 1, priceByShopId.Currency);
			int priceAmount = itemPrice.Price;
			string priceCurrency = itemPrice.Currency;
			ShopNGUIController.TryToBuy(base.gameObject, itemPrice, delegate
			{
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount++;
				}
				AnalyticsStuff.LogSales(GearManager.Like, "Gear", false);
				AnalyticsFacade.InAppPurchase(GearManager.Like, "Gear", 1, priceAmount, priceCurrency);
			}, new Action(JoystickController.leftJoystick.Reset), null, null, null, null);
		}
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x0005EB58 File Offset: 0x0005CD58
	private void ClickPotionButton(int index)
	{
		this.timerShowPotion = this.timerShowPotionMax;
		ElexirInGameButtonController myController = this.upButtonsInShopPanel[index].GetComponent<ElexirInGameButtonController>();
		ElexirInGameButtonController myController2 = this.upButtonsInShopPanelSwipeRegim[index].GetComponent<ElexirInGameButtonController>();
		UIButton myButton = this.upButtonsInShopPanel[index].GetComponent<UIButton>();
		UIButton myButton2 = this.upButtonsInShopPanelSwipeRegim[index].GetComponent<UIButton>();
		string name = myController.myPotion.name;
		string myStaragerKey = (!Defs.isDaterRegim) ? name : GearManager.HolderQuantityForID(myController.idForPriceInDaterRegim);
		int @int = Storager.getInt(myStaragerKey, false);
		if (@int > 0)
		{
			if (name.Equals(GearManager.Turret))
			{
				this.ShowTurretInterface("MusicBox");
			}
			else
			{
				if (Defs.isDaterRegim)
				{
					Storager.setInt(myStaragerKey, Storager.getInt(myStaragerKey, false) - 1, false);
				}
				PotionsController.sharedController.ActivatePotion(name, this.playerMoveC, new Dictionary<string, object>(), false);
			}
			string text = Storager.getInt(myStaragerKey, false).ToString();
			myController.myLabelCount.gameObject.SetActive(true);
			myController.plusSprite.SetActive(false);
			myController.myLabelCount.text = text;
			myController.isActivePotion = true;
			myButton.isEnabled = false;
			myController.myLabelTime.enabled = true;
			myController.myLabelTime.gameObject.SetActive(true);
			myController2.myLabelCount.gameObject.SetActive(true);
			myController2.plusSprite.SetActive(false);
			myController2.myLabelCount.text = text;
			myController2.isActivePotion = true;
			myButton2.isEnabled = false;
			myController2.myLabelTime.enabled = true;
			myController2.myLabelTime.gameObject.SetActive(true);
		}
		else
		{
			string text2 = GearManager.AnalyticsIDForOneItemOfGear(myStaragerKey ?? "Potion", true);
			ItemPrice priceByShopId = ItemDb.GetPriceByShopId(GearManager.OneItemIDForGear(myStaragerKey, GearManager.CurrentNumberOfUphradesForGear(myStaragerKey)), ShopNGUIController.CategoryNames.GearCategory);
			int priceAmount = priceByShopId.Price;
			string priceCurrency = priceByShopId.Currency;
			ShopNGUIController.TryToBuy(base.gameObject, priceByShopId, delegate
			{
				Storager.setInt(myStaragerKey, Storager.getInt(myStaragerKey, false) + 1, false);
				myButton.normalSprite = "game_clear";
				myButton.pressedSprite = "game_clear_n";
				myController.myLabelCount.gameObject.SetActive(true);
				myController.plusSprite.SetActive(false);
				myController.priceLabel.SetActive(false);
				myController.myLabelCount.text = Storager.getInt(myStaragerKey, false).ToString();
				myButton2.normalSprite = "game_clear";
				myButton2.pressedSprite = "game_clear_n";
				myController2.myLabelCount.gameObject.SetActive(true);
				myController2.plusSprite.SetActive(false);
				myController2.priceLabel.SetActive(false);
				myController2.myLabelCount.text = Storager.getInt(myStaragerKey, false).ToString();
				if (myStaragerKey != null)
				{
					AnalyticsStuff.LogSales(GearManager.HolderQuantityForID(myStaragerKey), "Gear", false);
					AnalyticsFacade.InAppPurchase(GearManager.HolderQuantityForID(myStaragerKey), "Gear", 1, priceAmount, priceCurrency);
				}
			}, new Action(JoystickController.leftJoystick.Reset), null, null, null, null);
		}
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x0005EE24 File Offset: 0x0005D024
	private void HandlePotionClicked(object sender, EventArgs e)
	{
		int index = 0;
		for (int i = 0; i < this.upButtonsInShopPanel.Length; i++)
		{
			if (this.upButtonsInShopPanel[i].name.Equals(((ButtonHandler)sender).gameObject.name))
			{
				index = i;
				break;
			}
		}
		this.ClickPotionButton(index);
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x0005EE84 File Offset: 0x0005D084
	private void HandleGearToogleClicked(object sender, EventArgs e)
	{
		bool value = this.gearToogle.GetComponent<UIToggle>().value;
		this.fonBig.SetActive(value);
		if (value)
		{
			this.timerShowPotion = this.timerShowPotionMax;
		}
		else
		{
			this.timerShowPotion = -1f;
		}
		for (int i = 0; i < this.upButtonsInShopPanel.Length; i++)
		{
			this.upButtonsInShopPanel[i].SetActive(value);
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x0005EEF8 File Offset: 0x0005D0F8
	private void HandlePrimaryToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(1, true);
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x0005EF04 File Offset: 0x0005D104
	private void HandleBackupToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(2, true);
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x0005EF10 File Offset: 0x0005D110
	private void HandleMeleeToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(3, true);
	}

	// Token: 0x060018EE RID: 6382 RVA: 0x0005EF1C File Offset: 0x0005D11C
	private void HandleSpecialToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(4, true);
	}

	// Token: 0x060018EF RID: 6383 RVA: 0x0005EF28 File Offset: 0x0005D128
	private void HandleSniperToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(5, true);
	}

	// Token: 0x060018F0 RID: 6384 RVA: 0x0005EF34 File Offset: 0x0005D134
	private void HandlePremiumToogleClicked(object sender, EventArgs e)
	{
		this.SelectWeaponFromCategory(6, true);
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x0005EF40 File Offset: 0x0005D140
	private void SelectWeaponFromCategory(int category, bool isUpdateSwipe = true)
	{
		for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
		{
			Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[i];
			if (weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == category)
			{
				this.SelectWeaponFromIndex(i, isUpdateSwipe);
				break;
			}
		}
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x0005EFA8 File Offset: 0x0005D1A8
	private void SelectWeaponFromIndex(int _index, bool updateSwipe = true)
	{
		bool[] array = new bool[6];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = false;
		}
		int num = 0;
		foreach (object obj in WeaponManager.sharedManager.playerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			int num2 = weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			array[num2] = true;
			num++;
		}
		for (int j = 0; j < this.weaponCategoriesButtons.Length; j++)
		{
			this.weaponCategoriesButtons[j].isEnabled = array[j];
			if (j == ((Weapon)WeaponManager.sharedManager.playerWeapons[_index]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1)
			{
				this.weaponCategoriesButtons[j].GetComponent<UIToggle>().value = true;
			}
			else
			{
				this.weaponCategoriesButtons[j].GetComponent<UIToggle>().value = false;
			}
		}
		this.SetChangeWeapon(_index, updateSwipe);
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x0005F0EC File Offset: 0x0005D2EC
	private void SetChangeWeapon(int index, bool isUpdateSwipe)
	{
		if (isUpdateSwipe)
		{
			if (index < this.changeWeaponWrap.transform.childCount)
			{
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 1E+11f;
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().CenterOn(this.changeWeaponWrap.transform.GetChild(index));
				this.changeWeaponWrap.GetComponent<MyCenterOnChild>().springStrength = 8f;
			}
			else
			{
				Debug.LogError("InGameGUI: not weapon icon with index " + index);
			}
		}
		if (WeaponManager.sharedManager.CurrentWeaponIndex == index)
		{
			return;
		}
		WeaponManager.sharedManager.CurrentWeaponIndex = index;
		WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
		if (this.playerMoveC != null)
		{
			if (this.playerMoveC.currentWeaponBeforeTurret >= 0)
			{
				this.playerMoveC.currentWeaponBeforeTurret = index;
				return;
			}
			if (this.playerMoveC.CurrentWeaponBeforeGrenade > -1)
			{
				this.playerMoveC.CurrentWeaponBeforeGrenade = index;
				return;
			}
			this.playerMoveC.ChangeWeapon(index, false);
			this.playerMoveC.HideChangeWeaponTrainingHint();
		}
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x0005F210 File Offset: 0x0005D410
	[Obfuscation(Exclude = true)]
	private void GenerateMiganie()
	{
		CoinsMessage.FireCoinsAddedEvent(false, 2);
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x0005F21C File Offset: 0x0005D41C
	private void CheckWeaponScrollChanged()
	{
		if (this._disabled)
		{
			return;
		}
		if (this.changeWeaponScroll.transform.localPosition.x > 0f)
		{
			this.weaponIndexInScroll = Mathf.RoundToInt((this.changeWeaponScroll.transform.localPosition.x - (float)(Mathf.FloorToInt(this.changeWeaponScroll.transform.localPosition.x / (float)this.widthWeaponScrollPreview / (float)this.changeWeaponWrap.transform.childCount) * this.widthWeaponScrollPreview * this.changeWeaponWrap.transform.childCount)) / (float)this.widthWeaponScrollPreview);
			this.weaponIndexInScroll = this.changeWeaponWrap.transform.childCount - this.weaponIndexInScroll;
		}
		else
		{
			this.weaponIndexInScroll = -1 * Mathf.RoundToInt((this.changeWeaponScroll.transform.localPosition.x - (float)(Mathf.CeilToInt(this.changeWeaponScroll.transform.localPosition.x / (float)this.widthWeaponScrollPreview / (float)this.changeWeaponWrap.transform.childCount) * this.widthWeaponScrollPreview * this.changeWeaponWrap.transform.childCount)) / (float)this.widthWeaponScrollPreview);
		}
		if (this.weaponIndexInScroll == this.changeWeaponWrap.transform.childCount)
		{
			this.weaponIndexInScroll = 0;
		}
		if (this.weaponIndexInScroll != this.weaponIndexInScrollOld)
		{
			this.SelectWeaponFromCategory(((Weapon)WeaponManager.sharedManager.playerWeapons[this.weaponIndexInScroll]).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor, false);
		}
		this.weaponIndexInScrollOld = this.weaponIndexInScroll;
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x0005F3E4 File Offset: 0x0005D5E4
	public IEnumerator _DisableSwiping(float tm)
	{
		MyCenterOnChild _center = this.changeWeaponWrap.GetComponent<MyCenterOnChild>();
		if (_center == null || _center.centeredObject == null)
		{
			yield break;
		}
		int bef;
		if (!int.TryParse(_center.centeredObject.name.Replace("WeaponCat_", string.Empty), out bef))
		{
			yield break;
		}
		this._disabled = true;
		yield return new WaitForSeconds(tm);
		this._disabled = false;
		if (_center.centeredObject == null || _center.centeredObject.name.Equals("WeaponCat_" + bef))
		{
			yield break;
		}
		Transform goToCent = null;
		foreach (object obj in _center.transform)
		{
			Transform t = (Transform)obj;
			if (t.gameObject.name.Equals("WeaponCat_" + bef))
			{
				goToCent = t;
				break;
			}
		}
		if (goToCent != null)
		{
			_center.CenterOn(goToCent);
		}
		yield break;
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x0005F410 File Offset: 0x0005D610
	private void UpdateCrosshairPositions()
	{
		if (this.playerMoveC == null || !this.playerMoveC.isMechActive)
		{
			float num = WeaponManager.sharedManager.currentWeaponSounds.tekKoof * WeaponManager.sharedManager.currentWeaponSounds.startZone.y * 0.5f;
			this.aimDown.transform.localPosition = new Vector3(0f, -this.aimPositions[1].y - num, 0f);
			this.aimUp.transform.localPosition = new Vector3(0f, this.aimPositions[2].y + num, 0f);
			this.aimLeft.transform.localPosition = new Vector3(-this.aimPositions[3].x - num, 0f, 0f);
			this.aimDownLeft.transform.localPosition = new Vector3(-this.aimPositions[4].x - num, -this.aimPositions[4].y - num, 0f);
			this.aimUpLeft.transform.localPosition = new Vector3(-this.aimPositions[5].x - num, this.aimPositions[5].y + num, 0f);
			this.aimRight.transform.localPosition = new Vector3(this.aimPositions[3].x + num, 0f, 0f);
			this.aimDownRight.transform.localPosition = new Vector3(this.aimPositions[4].x + num, -this.aimPositions[4].y - num, 0f);
			this.aimUpRight.transform.localPosition = new Vector3(this.aimPositions[5].x + num, this.aimPositions[5].y + num, 0f);
		}
		else
		{
			float num = 12f + this.playerMoveC.mechWeaponSounds.tekKoof * this.playerMoveC.mechWeaponSounds.startZone.y * 0.5f;
			this.aimUp.transform.localPosition = new Vector3(0f, num, 0f);
			this.aimUpRight.transform.localPosition = new Vector3(num, num, 0f);
			this.aimRight.transform.localPosition = new Vector3(num, 0f, 0f);
			this.aimDownRight.transform.localPosition = new Vector3(num, -num, 0f);
			this.aimDown.transform.localPosition = new Vector3(0f, -num, 0f);
			this.aimDownLeft.transform.localPosition = new Vector3(-num, -num, 0f);
			this.aimLeft.transform.localPosition = new Vector3(-num, 0f, 0f);
			this.aimUpLeft.transform.localPosition = new Vector3(-num, num, 0f);
		}
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x0005F768 File Offset: 0x0005D968
	private void SetCrosshairVisibility(bool visible)
	{
		if (this.crosshairVisible == visible)
		{
			return;
		}
		this.crosshairVisible = visible;
		this.aimCenterSprite.enabled = visible;
		this.aimDownSprite.enabled = visible;
		this.aimUpSprite.enabled = visible;
		this.aimLeftSprite.enabled = visible;
		this.aimRightSprite.enabled = visible;
		this.aimUpLeftSprite.enabled = visible;
		this.aimUpRightSprite.enabled = visible;
		this.aimDownLeftSprite.enabled = visible;
		this.aimDownRightSprite.enabled = visible;
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x0005F7F8 File Offset: 0x0005D9F8
	private void SetCrosshairPart(UISprite sprite, CrosshairData.aimSprite param, bool mirror = false)
	{
		if (!string.IsNullOrEmpty(param.spriteName))
		{
			sprite.gameObject.SetActive(true);
			sprite.spriteName = param.spriteName;
			sprite.width = Mathf.RoundToInt(param.spriteSize.x);
			sprite.height = Mathf.RoundToInt(param.spriteSize.y);
			sprite.transform.localPosition = ((!mirror) ? param.offset : new Vector2(param.offset.x, -param.offset.y));
		}
		else
		{
			sprite.gameObject.SetActive(false);
		}
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x0005F8A8 File Offset: 0x0005DAA8
	public void SetCrosshair(WeaponSounds weaponSounds)
	{
		WeaponCustomCrosshair component = weaponSounds.GetComponent<WeaponCustomCrosshair>();
		if (component != null)
		{
			this.SetCrosshairPart(this.aimCenterSprite, component.Data.center, false);
			this.SetCrosshairPart(this.aimDownSprite, component.Data.down, false);
			this.SetCrosshairPart(this.aimUpSprite, component.Data.up, false);
			this.SetCrosshairPart(this.aimLeftSprite, component.Data.left, false);
			this.SetCrosshairPart(this.aimRightSprite, component.Data.left, true);
			this.SetCrosshairPart(this.aimUpLeftSprite, component.Data.leftUp, false);
			this.SetCrosshairPart(this.aimUpRightSprite, component.Data.leftUp, true);
			this.SetCrosshairPart(this.aimDownLeftSprite, component.Data.leftDown, false);
			this.SetCrosshairPart(this.aimDownRightSprite, component.Data.leftDown, true);
			this.aimPositions[0] = component.Data.center.offset;
			this.aimPositions[1] = component.Data.down.offset;
			this.aimPositions[2] = component.Data.up.offset;
			this.aimPositions[3] = component.Data.left.offset;
			this.aimPositions[4] = component.Data.leftDown.offset;
			this.aimPositions[5] = component.Data.leftUp.offset;
		}
		else
		{
			this.SetCrosshairPart(this.aimCenterSprite, this.defaultAimCenter, false);
			this.SetCrosshairPart(this.aimDownSprite, this.defaultAimDown, false);
			this.SetCrosshairPart(this.aimUpSprite, this.defaultAimUp, false);
			this.SetCrosshairPart(this.aimLeftSprite, this.defaultAimLeftCenter, false);
			this.SetCrosshairPart(this.aimRightSprite, this.defaultAimLeftCenter, true);
			this.SetCrosshairPart(this.aimUpLeftSprite, this.defaultAimLeftUp, false);
			this.SetCrosshairPart(this.aimUpRightSprite, this.defaultAimLeftUp, true);
			this.SetCrosshairPart(this.aimDownLeftSprite, this.defaultAimLeftDown, false);
			this.SetCrosshairPart(this.aimDownRightSprite, this.defaultAimLeftDown, true);
			this.aimPositions[0] = this.defaultAimCenter.offset;
			this.aimPositions[1] = this.defaultAimDown.offset;
			this.aimPositions[2] = this.defaultAimUp.offset;
			this.aimPositions[3] = this.defaultAimLeftCenter.offset;
			this.aimPositions[4] = this.defaultAimLeftDown.offset;
			this.aimPositions[5] = this.defaultAimLeftUp.offset;
		}
		this.UpdateCrosshairPositions();
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x0005FBC4 File Offset: 0x0005DDC4
	private string FormatMeleeAmmoLabel(int index, int currentAmmoInClip, int currentAmmoInBackpack)
	{
		if (this._formatMeleeAmmoMemo == null || this._formatMeleeAmmoMemo.Length < this.ammoCategoriesLabels.Length)
		{
			this._formatMeleeAmmoMemo = new KeyValuePair<int, string>[this.ammoCategoriesLabels.Length];
		}
		int num = currentAmmoInClip + currentAmmoInBackpack;
		if (num != this._formatMeleeAmmoMemo[index].Key)
		{
			string value = num.ToString(CultureInfo.InvariantCulture);
			this._formatMeleeAmmoMemo[index] = new KeyValuePair<int, string>(num, value);
		}
		return this._formatMeleeAmmoMemo[index].Value ?? "0";
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x0005FC64 File Offset: 0x0005DE64
	private string FormatShootingAmmoLabel(int index, int currentAmmoInClip, int currentAmmoInBackpack)
	{
		if (this._formatShootingAmmoMemo == null || this._formatShootingAmmoMemo.Length < this.ammoCategoriesLabels.Length)
		{
			this._formatShootingAmmoMemo = new KeyValuePair<Ammo, string>[this.ammoCategoriesLabels.Length];
		}
		Ammo key = new Ammo(currentAmmoInClip, currentAmmoInBackpack);
		if (!key.Equals(this._formatShootingAmmoMemo[index].Key))
		{
			this._stringBuilder.Length = 0;
			this._stringBuilder.Append(currentAmmoInClip).Append("/").Append(currentAmmoInBackpack);
			string value = this._stringBuilder.ToString();
			this._stringBuilder.Length = 0;
			this._formatShootingAmmoMemo[index] = new KeyValuePair<Ammo, string>(key, value);
		}
		return this._formatShootingAmmoMemo[index].Value ?? "0/0";
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0005FD40 File Offset: 0x0005DF40
	private void Update()
	{
		this.CheckWeaponScrollChanged();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			this.fastShopPanel.transform.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (this.timerBlinkNoAmmo > 0f)
		{
			this.timerBlinkNoAmmo -= Time.deltaTime;
		}
		if (this.timerBlinkNoAmmo > 0f && this.playerMoveC != null && !this.playerMoveC.isMechActive)
		{
			this.blinkNoAmmoLabel.gameObject.SetActive(true);
			float num = this.timerBlinkNoAmmo % this.periodBlink / this.periodBlink;
			this.blinkNoAmmoLabel.color = new Color(this.blinkNoAmmoLabel.color.r, this.blinkNoAmmoLabel.color.g, this.blinkNoAmmoLabel.color.b, (num >= 0.5f) ? ((1f - num) * 2f) : (num * 2f));
		}
		if ((this.timerBlinkNoAmmo < 0f || (this.playerMoveC != null && this.playerMoveC.isMechActive)) && this.blinkNoAmmoLabel.gameObject.activeSelf)
		{
			this.blinkNoAmmoLabel.gameObject.SetActive(false);
		}
		if (this.playerMoveC != null)
		{
			int num2 = Mathf.FloorToInt(this.playerMoveC.CurHealth);
			if (num2 < this.oldCountHeath && this.timerBlinkNoHeath < 0f && num2 < 3)
			{
				this.timerBlinkNoHeath = this.periodBlink * 3f;
			}
			if (num2 > 2)
			{
				this.timerBlinkNoHeath = -1f;
			}
			this.oldCountHeath = num2;
			if (this.timerBlinkNoHeath > 0f)
			{
				this.timerBlinkNoHeath -= Time.deltaTime;
			}
			if (this.timerBlinkNoHeath > 0f && !this.playerMoveC.isMechActive)
			{
				if (num2 > 0)
				{
					this.PlayLowResourceBeepIfNotPlaying(1);
				}
				this.blinkNoHeathLabel.gameObject.SetActive(true);
				float num3 = this.timerBlinkNoHeath % this.periodBlink / this.periodBlink;
				float a = (num3 >= 0.5f) ? ((1f - num3) * 2f) : (num3 * 2f);
				this.blinkNoHeathLabel.color = new Color(this.blinkNoHeathLabel.color.r, this.blinkNoHeathLabel.color.g, this.blinkNoHeathLabel.color.b, a);
				for (int i = 0; i < this.blinkNoHeathFrames.Length; i++)
				{
					this.blinkNoHeathFrames[i].gameObject.SetActive(true);
					this.blinkNoHeathFrames[i].color = new Color(1f, 1f, 1f, a);
				}
			}
		}
		if ((this.timerBlinkNoHeath < 0f || this.playerMoveC == null || (this.playerMoveC != null && this.playerMoveC.isMechActive)) && this.blinkNoHeathLabel.gameObject.activeSelf)
		{
			this.blinkNoHeathLabel.gameObject.SetActive(false);
			for (int j = 0; j < this.blinkNoHeathFrames.Length; j++)
			{
				this.blinkNoHeathFrames[j].gameObject.SetActive(false);
			}
		}
		for (int k = 0; k < this.ammoCategoriesLabels.Length; k++)
		{
			if (this.ammoCategoriesLabels[k] != null)
			{
				bool flag = false;
				if (this.weaponCategoriesButtons[k].isEnabled)
				{
					for (int l = 0; l < WeaponManager.sharedManager.playerWeapons.Count; l++)
					{
						Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[l];
						WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
						if ((!component.isMelee || component.isShotMelee) && component.categoryNabor == k + 1)
						{
							this.ammoCategoriesLabels[k].text = ((!component.isShotMelee) ? this.FormatShootingAmmoLabel(k, weapon.currentAmmoInClip, weapon.currentAmmoInBackpack) : this.FormatMeleeAmmoLabel(k, weapon.currentAmmoInClip, weapon.currentAmmoInBackpack));
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.ammoCategoriesLabels[k].text = string.Empty;
				}
			}
		}
		if (this.timerShowNow > 0f)
		{
			this.timerShowNow -= Time.deltaTime;
			if (!this.message_now.activeSelf)
			{
				this.message_now.SetActive(true);
			}
		}
		else if (this.message_now.activeSelf)
		{
			this.message_now.SetActive(false);
		}
		if (this.isMulti && this.playerMoveC == null && WeaponManager.sharedManager.myPlayer != null)
		{
			this.playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (!this.isMulti && this.playerMoveC == null)
		{
			this.playerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
		if (this.isMulti && this.playerMoveC != null)
		{
			for (int m = 0; m < 3; m++)
			{
				this.messageAddScore[m].GetComponent<UIPlaySound>().volume = (float)((!Defs.isSoundFX) ? 0 : 1);
				float num4 = 0.3f;
				float num5 = 0.2f;
				if (m == 0)
				{
					float num6 = 1f;
					if (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[m] < num4)
					{
						num6 = 1f + num5 * (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[m]) / num4;
					}
					if (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[m] - num4 < num4)
					{
						num6 = 1f + num5 * (1f - (this.playerMoveC.myScoreController.maxTimerSumMessage - this.playerMoveC.myScoreController.timerAddScoreShow[m] - num4) / num4);
					}
					this.messageAddScore[m].transform.localScale = new Vector3(num6, num6, num6);
				}
				if (this.playerMoveC.timerShow[m] > 0f)
				{
					this.killLabels[m].gameObject.SetActive(true);
					this.killLabels[m].SetChatLabelText(this.playerMoveC.killedSpisok[m]);
				}
				else
				{
					this.killLabels[m].gameObject.SetActive(false);
				}
				if (this.playerMoveC.myScoreController.timerAddScoreShow[m] > 0f)
				{
					if (!this.messageAddScore[m].gameObject.activeSelf)
					{
						this.messageAddScore[m].gameObject.SetActive(true);
					}
					this.messageAddScore[m].text = this.playerMoveC.myScoreController.addScoreString[m];
					this.messageAddScore[m].color = new Color(1f, 1f, 1f, (this.playerMoveC.myScoreController.timerAddScoreShow[m] <= 1f) ? this.playerMoveC.myScoreController.timerAddScoreShow[m] : 1f);
				}
				else if (this.messageAddScore[m].gameObject.activeSelf)
				{
					this.messageAddScore[m].gameObject.SetActive(false);
				}
			}
			if (this.isChatOn)
			{
				int num7 = 0;
				int num8 = this.playerMoveC.messages.Count - 1;
				while (num8 >= 0 && this.playerMoveC.messages.Count - num8 - 1 < 3)
				{
					if (Time.time - this.playerMoveC.messages[num8].time < 10f)
					{
						if ((!this.isInet && this.playerMoveC.messages[num8].IDLocal == WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID) || (this.isInet && this.playerMoveC.messages[num8].ID == WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID))
						{
							this.chatLabels[num7].GetComponent<UILabel>().color = new Color(0f, 1f, 0.15f, 1f);
						}
						else
						{
							if (this.playerMoveC.messages[num8].command == 0)
							{
								this.chatLabels[num7].GetComponent<UILabel>().color = new Color(1f, 1f, 0.15f, 1f);
							}
							if (this.playerMoveC.messages[num8].command == 1)
							{
								this.chatLabels[num7].GetComponent<UILabel>().color = new Color(0f, 0f, 0.9f, 1f);
							}
							if (this.playerMoveC.messages[num8].command == 2)
							{
								this.chatLabels[num7].GetComponent<UILabel>().color = new Color(1f, 0f, 0f, 1f);
							}
						}
						ChatLabel component2 = this.chatLabels[num7].GetComponent<ChatLabel>();
						component2.nickLabel.text = this.playerMoveC.messages[num8].text;
						component2.iconSprite.spriteName = this.playerMoveC.messages[num8].iconName;
						Transform transform = component2.iconSprite.transform;
						transform.localPosition = new Vector3((float)(component2.nickLabel.width + 20), transform.localPosition.y, transform.localPosition.z);
						component2.clanTexture.mainTexture = this.playerMoveC.messages[num8].clanLogo;
						this.chatLabels[num7].SetActive(true);
					}
					else
					{
						this.chatLabels[num7].SetActive(false);
					}
					num7++;
					num8--;
				}
				for (int n = num7; n < 3; n++)
				{
					this.chatLabels[num7].SetActive(false);
				}
			}
			if (this.timerShowScorePict > 0f)
			{
				this.timerShowScorePict -= Time.deltaTime;
			}
			if (this.isHunger && Initializer.players.Count == 2 && this.hungerGameController.isGo && this.playerMoveC.timeHingerGame > 10f)
			{
				this.duel.SetActive(true);
				this.multyKillPanel.gameObject.SetActive(false);
			}
			else
			{
				if (this.duel.activeSelf)
				{
					this.duel.SetActive(false);
				}
				if (this.timerShowScorePict > 0f)
				{
					if ((!this.multyKillPanel.gameObject.activeSelf || this.scorePictName != this.multyKillSprite.spriteName) && (PauseGUIController.Instance == null || !PauseGUIController.Instance.IsPaused))
					{
						this.multyKillSprite.spriteName = this.scorePictName;
						this.multyKillPanel.gameObject.SetActive(true);
						this.multyKillPanel.GetComponent<MultyKill>().PlayTween();
					}
				}
				else if (this.multyKillPanel.gameObject.activeSelf)
				{
					this.multyKillPanel.gameObject.SetActive(false);
				}
			}
			if (this.isHunger && !this.hungerGameController.isGo)
			{
				this.timerStartHungerLabel.gameObject.SetActive(true);
				int num9 = Mathf.FloorToInt(this.hungerGameController.goTimer);
				string text;
				if (num9 == 0)
				{
					text = "GO!";
					this.timerStartHungerLabel.color = new Color(0f, 1f, 0f, 1f);
				}
				else
				{
					text = string.Empty + num9;
					this.timerStartHungerLabel.color = new Color(1f, 0f, 0f, 1f);
				}
				this.timerStartHungerLabel.text = text;
			}
			else if (this.isHunger && this.hungerGameController.isGo && this.hungerGameController.isShowGo)
			{
				this.timerStartHungerLabel.gameObject.SetActive(true);
				this.timerStartHungerLabel.text = "GO!";
			}
			else
			{
				this.timerStartHungerLabel.gameObject.SetActive(false);
			}
		}
		if (this.playerMoveC != null)
		{
			if (this.playerMoveC.timerShowDown > 0f && this.playerMoveC.timerShowDown < this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.downBloodTexture.SetActive(true);
			}
			else
			{
				this.downBloodTexture.SetActive(false);
			}
			if (this.playerMoveC.timerShowUp > 0f && this.playerMoveC.timerShowUp < this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.upBloodTexture.SetActive(true);
			}
			else
			{
				this.upBloodTexture.SetActive(false);
			}
			if (this.playerMoveC.timerShowLeft > 0f && this.playerMoveC.timerShowLeft < this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.leftBloodTexture.SetActive(true);
			}
			else
			{
				this.leftBloodTexture.SetActive(false);
			}
			if (this.playerMoveC.timerShowRight > 0f && this.playerMoveC.timerShowRight < this.playerMoveC.maxTimeSetTimerShow - 0.03f)
			{
				this.rightBloodTexture.SetActive(true);
			}
			else
			{
				this.rightBloodTexture.SetActive(false);
			}
			if (!this.playerMoveC.isZooming && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None || !TrainingController.isPressSkip))
			{
				this.SetCrosshairVisibility(true);
				this.UpdateCrosshairPositions();
			}
			else
			{
				this.SetCrosshairVisibility(false);
			}
		}
		string name = SceneManager.GetActiveScene().name;
		bool flag2 = true;
		if (name == Defs.TrainingSceneName)
		{
			flag2 = false;
		}
		this.shopButton.GetComponent<UIButton>().isEnabled = (flag2 && !this.turretPanel.activeSelf);
		this.shopButtonInPause.GetComponent<UIButton>().isEnabled = (flag2 && !this._kBlockPauseShopButton);
		if (!this.isMulti && this.zombieCreator != null)
		{
			int num10 = GlobalGameController.GetEnemiesToKill(name) - this.zombieCreator.NumOfDeadZombies;
			if (!Defs.IsSurvival && num10 == 0)
			{
				string text2;
				if (LevelBox.weaponsFromBosses.ContainsKey(name))
				{
					text2 = LocalizationStore.Get("Key_0192");
				}
				else
				{
					text2 = LocalizationStore.Get("Key_0854");
				}
				if (this.zombieCreator.bossShowm)
				{
					text2 = LocalizationStore.Get("Key_0855");
				}
				this.enemiesLeftLabel.SetActive(this.perfectLabels == null || !this.perfectLabels.gameObject.activeInHierarchy);
				this.enemiesLeftLabel.GetComponent<UILabel>().text = text2;
			}
			else
			{
				this.enemiesLeftLabel.SetActive(false);
			}
		}
		if (this.playerMoveC != null && this.playerMoveC.isMechActive)
		{
			if (!this.mechWasActive)
			{
				this.currentHealthStep = Mathf.CeilToInt(this.health());
				this.currentArmorStep = Mathf.CeilToInt(this.armor());
				this.pastMechHealth = this.playerMoveC.liveMech;
				this.SetMechHealth();
				this.mechWasActive = true;
			}
			else if (!this.mechInAnim && this.pastMechHealth != this.playerMoveC.liveMech)
			{
				base.StartCoroutine(this.AnimateMechHealth());
			}
			this.pastMechHealth = this.playerMoveC.liveMech;
		}
		else
		{
			if (Defs.isDaterRegim)
			{
				for (int num11 = 0; num11 < Player_move_c.MaxPlayerGUIHealth; num11++)
				{
					this.hearts[num11].gameObject.SetActive(false);
				}
			}
			else
			{
				if (this.playerMoveC.respawnedForGUI || this.mechWasActive)
				{
					this.currentMechHealthStep = Mathf.CeilToInt(this.playerMoveC.liveMech);
					this.pastHealth = this.health();
					this.SetHealth();
				}
				else if (!this.healthInAnim && this.pastHealth != this.health())
				{
					base.StartCoroutine(this.AnimateHealth());
				}
				this.pastHealth = this.health();
			}
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				if (Defs.isDaterRegim)
				{
					for (int num12 = 0; num12 < Player_move_c.MaxPlayerGUIHealth; num12++)
					{
						this.armorShields[num12].gameObject.SetActive(false);
					}
					this.pastArmor = 0f;
				}
				else
				{
					if (this.playerMoveC.respawnedForGUI || this.mechWasActive)
					{
						this.currentMechHealthStep = Mathf.CeilToInt(this.playerMoveC.liveMech);
						this.pastArmor = this.armor();
						this.SetArmor();
					}
					else if (!this.armorInAnim && this.pastArmor != this.armor())
					{
						base.StartCoroutine(this.AnimateArmor());
					}
					this.pastArmor = this.armor();
				}
			}
			else
			{
				for (int num13 = 0; num13 < Player_move_c.MaxPlayerGUIHealth; num13++)
				{
					this.armorShields[num13].gameObject.SetActive(false);
				}
			}
			this.mechWasActive = false;
			this.playerMoveC.respawnedForGUI = false;
		}
		if (Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight))
		{
			int winningTeam = WeaponManager.sharedManager.myNetworkStartTable.GetWinningTeam();
			this.mineBlue.SetActive(WeaponManager.sharedManager.myNetworkStartTable.myCommand > 0);
			bool flag3 = WeaponManager.sharedManager.myNetworkStartTable.myCommand == winningTeam;
			this.winningBlue.SetActive(winningTeam != 0 && flag3);
			this.winningRed.SetActive(winningTeam != 0 && !flag3);
		}
		if (!Defs.isDaterRegim && Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel))
		{
			int placeInTable = WeaponManager.sharedManager.myNetworkStartTable.GetPlaceInTable();
			string text3 = this.FormatRank(placeInTable);
			this.placeDeathmatchLabel.text = text3;
			this.placeCoopLabel.text = text3;
			this.firstPlaceGO.SetActive(placeInTable == 0);
			this.firstPlaceCoop.SetActive(placeInTable == 0);
		}
		if (PauseGUIController.Instance != null)
		{
			bool flag4 = !PauseGUIController.Instance.IsPaused;
			if (this.leftAnchor != null && this.leftAnchor.activeInHierarchy != flag4)
			{
				this.leftAnchor.SetActive(flag4);
			}
			if (this.swipeWeaponPanel != null && this.swipeWeaponPanel.gameObject.activeInHierarchy != flag4)
			{
				this.swipeWeaponPanel.gameObject.SetActive(flag4);
			}
		}
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x00061318 File Offset: 0x0005F518
	private string FormatRank(int rank)
	{
		if (rank != this._rankMemo.Key)
		{
			string value = (rank + 1).ToString();
			this._rankMemo = new KeyValuePair<int, string>(rank, value);
		}
		return this._rankMemo.Value;
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0006135C File Offset: 0x0005F55C
	private void SetMechHealth()
	{
		this.currentHealthStep = Mathf.FloorToInt(this.pastMechHealth);
		for (int i = 0; i < this.mechShields.Length; i++)
		{
			this.mechShields[i].SetIndex(Mathf.CeilToInt((this.pastMechHealth - (float)i) / 18f), HeartEffect.IndicatorType.Mech);
		}
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x000613B8 File Offset: 0x0005F5B8
	private void SetHealth()
	{
		this.currentHealthStep = Mathf.FloorToInt(this.pastHealth);
		for (int i = 0; i < this.hearts.Length; i++)
		{
			this.hearts[i].SetIndex(Mathf.CeilToInt((this.pastHealth - (float)i) / 9f), HeartEffect.IndicatorType.Hearts);
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x00061414 File Offset: 0x0005F614
	private void SetArmor()
	{
		this.currentArmorStep = Mathf.FloorToInt(this.pastArmor);
		for (int i = 0; i < this.armorShields.Length; i++)
		{
			this.armorShields[i].SetIndex(Mathf.CeilToInt((this.pastArmor - (float)i) / 9f), HeartEffect.IndicatorType.Armor);
		}
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x00061470 File Offset: 0x0005F670
	private IEnumerator AnimateHealth()
	{
		this.healthInAnim = true;
		this.currentHealthStep = Mathf.CeilToInt(this.pastHealth);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (this.currentHealthStep != Mathf.CeilToInt(this.health()))
		{
			int heartsStart = this.currentHealthStep - 9 * Mathf.FloorToInt((float)this.currentHealthStep / 9f);
			int currentHealth = Mathf.CeilToInt(this.health());
			if (currentHealth < 0)
			{
				currentHealth = 0;
			}
			bool minus = this.currentHealthStep > currentHealth;
			if (minus)
			{
				heartsStart--;
				if (heartsStart < 0)
				{
					heartsStart = 8;
				}
			}
			this.currentHealthStep += ((!minus) ? 1 : -1);
			this.hearts[heartsStart].Animate((!minus) ? Mathf.CeilToInt((float)this.currentHealthStep / 9f) : Mathf.FloorToInt((float)this.currentHealthStep / 9f), HeartEffect.IndicatorType.Hearts);
			yield return awaiter;
		}
		this.healthInAnim = false;
		yield break;
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x0006148C File Offset: 0x0005F68C
	private IEnumerator AnimateArmor()
	{
		this.armorInAnim = true;
		this.currentArmorStep = Mathf.CeilToInt(this.pastArmor);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (this.currentArmorStep != Mathf.CeilToInt(this.armor()))
		{
			int armorStart = this.currentArmorStep - 9 * Mathf.FloorToInt((float)this.currentArmorStep / 9f);
			int currentArmor = Mathf.CeilToInt(this.armor());
			if (currentArmor < 0)
			{
				currentArmor = 0;
			}
			bool minus = this.currentArmorStep > currentArmor;
			if (minus)
			{
				armorStart--;
				if (armorStart < 0)
				{
					armorStart = 8;
				}
			}
			this.currentArmorStep += ((!minus) ? 1 : -1);
			this.armorShields[armorStart].Animate((!minus) ? Mathf.CeilToInt((float)this.currentArmorStep / 9f) : Mathf.FloorToInt((float)this.currentArmorStep / 9f), HeartEffect.IndicatorType.Armor);
			yield return awaiter;
		}
		this.armorInAnim = false;
		yield break;
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x000614A8 File Offset: 0x0005F6A8
	private IEnumerator AnimateMechHealth()
	{
		this.mechInAnim = true;
		this.currentMechHealthStep = Mathf.CeilToInt(this.pastMechHealth);
		WaitForSeconds awaiter = new WaitForSeconds(0.05f);
		while (this.currentMechHealthStep != Mathf.CeilToInt(this.playerMoveC.liveMech))
		{
			int mechStart = this.currentMechHealthStep - 18 * Mathf.FloorToInt((float)this.currentMechHealthStep / 18f);
			int currentMech = Mathf.CeilToInt(this.playerMoveC.liveMech);
			if (currentMech < 0)
			{
				currentMech = 0;
			}
			bool minus = this.currentMechHealthStep > currentMech;
			if (minus)
			{
				mechStart--;
				if (mechStart < 0)
				{
					mechStart = 17;
				}
			}
			this.currentMechHealthStep += ((!minus) ? 1 : -1);
			this.mechShields[mechStart].Animate((!minus) ? Mathf.CeilToInt((float)this.currentMechHealthStep / 18f) : Mathf.FloorToInt((float)this.currentMechHealthStep / 18f), HeartEffect.IndicatorType.Mech);
			yield return awaiter;
		}
		this.mechInAnim = false;
		yield break;
	}

	// Token: 0x06001905 RID: 6405 RVA: 0x000614C4 File Offset: 0x0005F6C4
	public void SetScopeForWeapon(string num)
	{
		this.scopeText.SetActive(true);
		string path = (!Device.isWeakDevice && BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64) ? ResPath.Combine("Scopes", "Scope_" + num) : ResPath.Combine("Scopes", "Scope_" + num + "_small");
		this.scopeText.GetComponent<UITexture>().mainTexture = Resources.Load<Texture>(path);
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00061540 File Offset: 0x0005F740
	public void ResetScope()
	{
		this.scopeText.GetComponent<UITexture>().mainTexture = null;
		this.scopeText.SetActive(false);
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x00061560 File Offset: 0x0005F760
	public void HandleWeaponEquipped(WeaponSounds ws)
	{
		if (ws != null && WeaponManager.sharedManager != null && !ws.IsAvalibleFromFilter(WeaponManager.sharedManager.CurrentFilterMap))
		{
			return;
		}
		int num;
		if (Defs.isHunger && ws != null && int.TryParse(ws.nameNoClone().Substring("Weapon".Length), out num) && num != 9 && !ChestController.weaponForHungerGames.Contains(num))
		{
			return;
		}
		int num2 = 0;
		foreach (object obj in WeaponManager.sharedManager.playerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			int num3 = weapon.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
			num2++;
		}
		int childCount = this.changeWeaponWrap.transform.childCount;
		for (int i = childCount; i < num2; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.weaponPreviewPrefab);
			gameObject.name = "WeaponCat_" + i.ToString();
			gameObject.transform.parent = this.changeWeaponWrap.transform;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<UITexture>().width = Mathf.RoundToInt((float)this.widthWeaponScrollPreview * 0.7f);
			gameObject.GetComponent<UITexture>().height = Mathf.RoundToInt((float)this.widthWeaponScrollPreview * 0.7f);
			gameObject.GetComponent<BoxCollider>().size = new Vector3((float)this.widthWeaponScrollPreview * 1.3f, (float)this.widthWeaponScrollPreview * 1.3f, 1f);
		}
		this.changeWeaponWrap.SortAlphabetically();
		this.changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = false;
		this.changeWeaponWrap.GetComponent<MyCenterOnChild>().enabled = true;
		this.UpdateWeaponIconsForWrap();
		for (int j = 0; j < WeaponManager.sharedManager.playerWeapons.Count; j++)
		{
			this.changeWeaponWrap.transform.GetChild(j).GetComponent<WeaponIconController>().myWeaponSounds = ((Weapon)WeaponManager.sharedManager.playerWeapons[j]).weaponPrefab.GetComponent<WeaponSounds>();
		}
		this.SelectWeaponFromCategory(ws.categoryNabor, true);
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x000617FC File Offset: 0x0005F9FC
	public IEnumerator ShowRespawnWindow(KillerInfo killerInfo, float seconds)
	{
		CameraSceneController.sharedController.killCamController.lastDistance = 1f;
		CameraSceneController.sharedController.SetTargetKillCam(killerInfo.killerTransform);
		Defs.inRespawnWindow = true;
		this.respawnWindow.Show(killerInfo);
		this.respawnWindow.characterDrag.SetActive(false);
		this.respawnWindow.cameraDrag.SetActive(true);
		SkinName skinNameComponent = killerInfo.killerTransform.GetComponent<SkinName>();
		float closeTime = seconds;
		bool showCharacter = false;
		while (Defs.inRespawnWindow)
		{
			if (!showCharacter && (killerInfo.killerTransform == null || killerInfo.killerTransform.position.y <= -5000f || skinNameComponent.playerMoveC.isKilled))
			{
				showCharacter = true;
				this.respawnWindow.characterDrag.SetActive(true);
				this.respawnWindow.cameraDrag.SetActive(false);
				RespawnWindow.Instance.ShowCharacter(killerInfo);
				CameraSceneController.sharedController.SetTargetKillCam(null);
			}
			closeTime -= Time.deltaTime;
			this.respawnWindow.SetCurrentTime(closeTime);
			if (closeTime <= 0f)
			{
				this.respawnWindow.CloseRespawnWindow();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x00061834 File Offset: 0x0005FA34
	public void UpdateWeaponIconsForWrap()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			Texture texture = ShopNGUIController.TextureForEquippedWeaponOrWear(i);
			if (texture != null)
			{
				this.weaponIcons[i].mainTexture = texture;
				foreach (object obj in this.changeWeaponWrap.transform)
				{
					Transform transform = (Transform)obj;
					if (transform.name.Equals("WeaponCat_" + num))
					{
						transform.GetComponent<UITexture>().mainTexture = texture;
						break;
					}
				}
				num++;
			}
		}
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x00061914 File Offset: 0x0005FB14
	public void BlinkNoAmmo(int count)
	{
		if (count == 0)
		{
			this.StopPlayingLowResourceBeep();
		}
		this.timerBlinkNoAmmo = (float)count * this.periodBlink;
		this.blinkNoAmmoLabel.color = new Color(this.blinkNoAmmoLabel.color.r, this.blinkNoAmmoLabel.color.g, this.blinkNoAmmoLabel.color.b, 0f);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x0006198C File Offset: 0x0005FB8C
	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
		{
			transform.gameObject.layer = layerNumber;
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000619C8 File Offset: 0x0005FBC8
	private void OnDestroy()
	{
		this.SetNGUITouchDragThreshold(40f);
		InGameGUI.sharedInGameGUI = null;
		WeaponManager.WeaponEquipped -= this.HandleWeaponEquipped;
		PauseNGUIController.ChatSettUpdated -= this.HandleChatSettUpdated;
		PauseNGUIController.PlayerHandUpdated -= this.AdjustToPlayerHands;
		ControlsSettingsBase.ControlsChanged -= this.AdjustToPlayerHands;
		PauseNGUIController.SwitchingWeaponsUpdated -= this.SetSwitchingWeaponPanel;
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x00061A3C File Offset: 0x0005FC3C
	public void SetInterfaceVisible(bool visible)
	{
		this.interfacePanel.GetComponent<UIPanel>().gameObject.SetActive(visible);
		this.joystikPanel.gameObject.SetActive(visible);
		this.shopPanel.gameObject.SetActive(visible);
		this.bloodPanel.gameObject.SetActive(visible);
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x00061A94 File Offset: 0x0005FC94
	private void SetNGUITouchDragThreshold(float newValue)
	{
		if (UICamera.mainCamera != null && UICamera.mainCamera.GetComponent<UICamera>() != null)
		{
			UICamera.mainCamera.GetComponent<UICamera>().touchDragThreshold = newValue;
		}
		else
		{
			Debug.LogWarning("UICamera.mainCamera is null");
		}
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x00061AE8 File Offset: 0x0005FCE8
	public void ShowControlSchemeConfigurator()
	{
	}

	// Token: 0x04000D39 RID: 3385
	private const string weaponCat = "WeaponCat_";

	// Token: 0x04000D3A RID: 3386
	public UILabel Wave1_And_Counter;

	// Token: 0x04000D3B RID: 3387
	public UILabel reloadLabel;

	// Token: 0x04000D3C RID: 3388
	public GameObject reloadBar;

	// Token: 0x04000D3D RID: 3389
	public UIPlayTween impactTween;

	// Token: 0x04000D3E RID: 3390
	public UITexture reloadCircularSprite;

	// Token: 0x04000D3F RID: 3391
	public UITexture fireCircularSprite;

	// Token: 0x04000D40 RID: 3392
	public UITexture fireAdditionalCrcualrSprite;

	// Token: 0x04000D41 RID: 3393
	private UITexture[] circularSprites;

	// Token: 0x04000D42 RID: 3394
	public GameObject centerAnhor;

	// Token: 0x04000D43 RID: 3395
	public UILabel newWave;

	// Token: 0x04000D44 RID: 3396
	public UILabel waveDone;

	// Token: 0x04000D45 RID: 3397
	public UILabel SurvivalWaveNumber;

	// Token: 0x04000D46 RID: 3398
	public GameObject deathmatchContainer;

	// Token: 0x04000D47 RID: 3399
	public GameObject daterContainer;

	// Token: 0x04000D48 RID: 3400
	public GameObject teamBattleContainer;

	// Token: 0x04000D49 RID: 3401
	public GameObject timeBattleContainer;

	// Token: 0x04000D4A RID: 3402
	public GameObject deadlygamesContainer;

	// Token: 0x04000D4B RID: 3403
	public GameObject flagCaptureContainer;

	// Token: 0x04000D4C RID: 3404
	public GameObject survivalContainer;

	// Token: 0x04000D4D RID: 3405
	public GameObject CampaignContainer;

	// Token: 0x04000D4E RID: 3406
	public GameObject CapturePointContainer;

	// Token: 0x04000D4F RID: 3407
	public GameObject duelContainer;

	// Token: 0x04000D50 RID: 3408
	public GameObject waitDuelLabel;

	// Token: 0x04000D51 RID: 3409
	public UILabel waitDuelLabelTimer;

	// Token: 0x04000D52 RID: 3410
	public GameObject[] hidesPanelInTurrel;

	// Token: 0x04000D53 RID: 3411
	public GameObject turretPanel;

	// Token: 0x04000D54 RID: 3412
	public GameObject topPanelsTapReceiver;

	// Token: 0x04000D55 RID: 3413
	public ButtonHandler runTurrelButton;

	// Token: 0x04000D56 RID: 3414
	public ButtonHandler cancelTurrelButton;

	// Token: 0x04000D57 RID: 3415
	[Range(1f, 1000f)]
	public float minLength = 300f;

	// Token: 0x04000D58 RID: 3416
	[Range(1f, 1000f)]
	public float maxLength = 550f;

	// Token: 0x04000D59 RID: 3417
	[Range(1f, 1000f)]
	public float defaultPanelLength = 486f;

	// Token: 0x04000D5A RID: 3418
	public Transform sideObjGearShop;

	// Token: 0x04000D5B RID: 3419
	public static InGameGUI sharedInGameGUI;

	// Token: 0x04000D5C RID: 3420
	public GameObject pausePanel;

	// Token: 0x04000D5D RID: 3421
	public Transform shopPanelForTap;

	// Token: 0x04000D5E RID: 3422
	public Transform shopPanelForSwipe;

	// Token: 0x04000D5F RID: 3423
	public Transform shopPanelForTapDater;

	// Token: 0x04000D60 RID: 3424
	public Transform shopPanelForSwipeDater;

	// Token: 0x04000D61 RID: 3425
	public Transform swipeWeaponPanel;

	// Token: 0x04000D62 RID: 3426
	public Transform swipeArrowLeft;

	// Token: 0x04000D63 RID: 3427
	public Transform swipeArrowRight;

	// Token: 0x04000D64 RID: 3428
	public static Vector3 swipeWeaponPanelPos;

	// Token: 0x04000D65 RID: 3429
	public static Vector3 shopPanelForTapPos;

	// Token: 0x04000D66 RID: 3430
	public static Vector3 shopPanelForSwipePos;

	// Token: 0x04000D67 RID: 3431
	public GameObject blockedCollider;

	// Token: 0x04000D68 RID: 3432
	public GameObject blockedCollider2;

	// Token: 0x04000D69 RID: 3433
	public GameObject blockedColliderDater;

	// Token: 0x04000D6A RID: 3434
	public GameObject zoomButton;

	// Token: 0x04000D6B RID: 3435
	public GameObject reloadButton;

	// Token: 0x04000D6C RID: 3436
	public GameObject jumpButton;

	// Token: 0x04000D6D RID: 3437
	public GameObject fireButton;

	// Token: 0x04000D6E RID: 3438
	public GameObject fireButtonInJoystick;

	// Token: 0x04000D6F RID: 3439
	public GameObject joystick;

	// Token: 0x04000D70 RID: 3440
	public GameObject grenadeButton;

	// Token: 0x04000D71 RID: 3441
	public GameObject chooseGadgetPanel;

	// Token: 0x04000D72 RID: 3442
	public GameObject bottomPanel;

	// Token: 0x04000D73 RID: 3443
	public UISprite fireButtonSprite;

	// Token: 0x04000D74 RID: 3444
	public UISprite fireButtonSprite2;

	// Token: 0x04000D75 RID: 3445
	public GameObject aimPanel;

	// Token: 0x04000D76 RID: 3446
	public GameObject flagBlueCaptureTexture;

	// Token: 0x04000D77 RID: 3447
	public GameObject flagRedCaptureTexture;

	// Token: 0x04000D78 RID: 3448
	public GameObject message_draw;

	// Token: 0x04000D79 RID: 3449
	public GameObject message_now;

	// Token: 0x04000D7A RID: 3450
	public GameObject message_wait;

	// Token: 0x04000D7B RID: 3451
	public GameObject message_returnFlag;

	// Token: 0x04000D7C RID: 3452
	public float timerShowNow;

	// Token: 0x04000D7D RID: 3453
	public GameObject interfacePanel;

	// Token: 0x04000D7E RID: 3454
	public UILabel timerStartHungerLabel;

	// Token: 0x04000D7F RID: 3455
	public GameObject shopButton;

	// Token: 0x04000D80 RID: 3456
	public GameObject shopButtonInPause;

	// Token: 0x04000D81 RID: 3457
	public GameObject enemiesLeftLabel;

	// Token: 0x04000D82 RID: 3458
	public GameObject duel;

	// Token: 0x04000D83 RID: 3459
	public GameObject downBloodTexture;

	// Token: 0x04000D84 RID: 3460
	public GameObject upBloodTexture;

	// Token: 0x04000D85 RID: 3461
	public GameObject leftBloodTexture;

	// Token: 0x04000D86 RID: 3462
	public GameObject rightBloodTexture;

	// Token: 0x04000D87 RID: 3463
	public GameObject aimUp;

	// Token: 0x04000D88 RID: 3464
	public GameObject aimDown;

	// Token: 0x04000D89 RID: 3465
	public GameObject aimRight;

	// Token: 0x04000D8A RID: 3466
	public GameObject aimLeft;

	// Token: 0x04000D8B RID: 3467
	public GameObject aimCenter;

	// Token: 0x04000D8C RID: 3468
	public GameObject aimUpLeft;

	// Token: 0x04000D8D RID: 3469
	public GameObject aimDownLeft;

	// Token: 0x04000D8E RID: 3470
	public GameObject aimDownRight;

	// Token: 0x04000D8F RID: 3471
	public GameObject aimUpRight;

	// Token: 0x04000D90 RID: 3472
	[HideInInspector]
	public UISprite aimUpSprite;

	// Token: 0x04000D91 RID: 3473
	[HideInInspector]
	public UISprite aimDownSprite;

	// Token: 0x04000D92 RID: 3474
	[HideInInspector]
	public UISprite aimRightSprite;

	// Token: 0x04000D93 RID: 3475
	[HideInInspector]
	public UISprite aimLeftSprite;

	// Token: 0x04000D94 RID: 3476
	[HideInInspector]
	public UISprite aimCenterSprite;

	// Token: 0x04000D95 RID: 3477
	[HideInInspector]
	public UISprite aimUpLeftSprite;

	// Token: 0x04000D96 RID: 3478
	[HideInInspector]
	public UISprite aimDownLeftSprite;

	// Token: 0x04000D97 RID: 3479
	[HideInInspector]
	public UISprite aimDownRightSprite;

	// Token: 0x04000D98 RID: 3480
	[HideInInspector]
	public UISprite aimUpRightSprite;

	// Token: 0x04000D99 RID: 3481
	public UISprite aimRect;

	// Token: 0x04000D9A RID: 3482
	public GameObject topAnchor;

	// Token: 0x04000D9B RID: 3483
	public GameObject leftAnchor;

	// Token: 0x04000D9C RID: 3484
	public GameObject rightAnchor;

	// Token: 0x04000D9D RID: 3485
	public GameObject bottomAnchor;

	// Token: 0x04000D9E RID: 3486
	public InGameGUI.GetFloatVAlue health;

	// Token: 0x04000D9F RID: 3487
	public InGameGUI.GetFloatVAlue armor;

	// Token: 0x04000DA0 RID: 3488
	public InGameGUI.GetIntVAlue armorType;

	// Token: 0x04000DA1 RID: 3489
	public InGameGUI.GetString killsToMaxKills;

	// Token: 0x04000DA2 RID: 3490
	public InGameGUI.GetString timeLeft;

	// Token: 0x04000DA3 RID: 3491
	public UIButton gearToogle;

	// Token: 0x04000DA4 RID: 3492
	public UIButton[] weaponCategoriesButtons;

	// Token: 0x04000DA5 RID: 3493
	public UILabel[] ammoCategoriesLabels;

	// Token: 0x04000DA6 RID: 3494
	public UIButton[] weaponCategoriesButtonsDater;

	// Token: 0x04000DA7 RID: 3495
	public UILabel[] ammoCategoriesLabelsDater;

	// Token: 0x04000DA8 RID: 3496
	public GameObject fonBig;

	// Token: 0x04000DA9 RID: 3497
	public GameObject fonSmall;

	// Token: 0x04000DAA RID: 3498
	public GameObject pointCaptureBar;

	// Token: 0x04000DAB RID: 3499
	public UISprite teamColorSprite;

	// Token: 0x04000DAC RID: 3500
	public UISprite captureBarSprite;

	// Token: 0x04000DAD RID: 3501
	public UILabel pointCaptureName;

	// Token: 0x04000DAE RID: 3502
	public HeartEffect[] hearts;

	// Token: 0x04000DAF RID: 3503
	public HeartEffect[] armorShields;

	// Token: 0x04000DB0 RID: 3504
	public HeartEffect[] mechShields;

	// Token: 0x04000DB1 RID: 3505
	public DamageTakenController[] damageTakenControllers;

	// Token: 0x04000DB2 RID: 3506
	private int curDamageTakenController;

	// Token: 0x04000DB3 RID: 3507
	private float timerShowPotion = -1f;

	// Token: 0x04000DB4 RID: 3508
	private float timerShowPotionMax = 10f;

	// Token: 0x04000DB5 RID: 3509
	public SetChatLabelController[] killLabels;

	// Token: 0x04000DB6 RID: 3510
	public GameObject[] chatLabels;

	// Token: 0x04000DB7 RID: 3511
	public UILabel[] messageAddScore;

	// Token: 0x04000DB8 RID: 3512
	public GameObject elixir;

	// Token: 0x04000DB9 RID: 3513
	public GameObject scoreLabel;

	// Token: 0x04000DBA RID: 3514
	public GameObject enemiesLabel;

	// Token: 0x04000DBB RID: 3515
	public GameObject timeLabel;

	// Token: 0x04000DBC RID: 3516
	public GameObject killsLabel;

	// Token: 0x04000DBD RID: 3517
	public GameObject scopeText;

	// Token: 0x04000DBE RID: 3518
	public GameObject joystickContainer;

	// Token: 0x04000DBF RID: 3519
	public GameObject nightVisionEffect;

	// Token: 0x04000DC0 RID: 3520
	public UILabel rulesLabel;

	// Token: 0x04000DC1 RID: 3521
	public Player_move_c playerMoveC;

	// Token: 0x04000DC2 RID: 3522
	private ZombieCreator zombieCreator;

	// Token: 0x04000DC3 RID: 3523
	public UIPanel multyKillPanel;

	// Token: 0x04000DC4 RID: 3524
	public UISprite multyKillSprite;

	// Token: 0x04000DC5 RID: 3525
	private bool isMulti;

	// Token: 0x04000DC6 RID: 3526
	private bool isChatOn;

	// Token: 0x04000DC7 RID: 3527
	private bool isInet;

	// Token: 0x04000DC8 RID: 3528
	private bool isHunger;

	// Token: 0x04000DC9 RID: 3529
	private HungerGameController hungerGameController;

	// Token: 0x04000DCA RID: 3530
	public GameObject[] upButtonsInShopPanel;

	// Token: 0x04000DCB RID: 3531
	public GameObject[] upButtonsInShopPanelSwipeRegim;

	// Token: 0x04000DCC RID: 3532
	public GameObject healthAddButton;

	// Token: 0x04000DCD RID: 3533
	public GameObject healthAddButtonDater;

	// Token: 0x04000DCE RID: 3534
	public GameObject ammoAddButton;

	// Token: 0x04000DCF RID: 3535
	public GameObject ammoAddButtonDater;

	// Token: 0x04000DD0 RID: 3536
	public UITexture[] weaponIcons;

	// Token: 0x04000DD1 RID: 3537
	public UITexture[] weaponIconsDater;

	// Token: 0x04000DD2 RID: 3538
	public GameObject fastShopPanel;

	// Token: 0x04000DD3 RID: 3539
	public UIScrollView changeWeaponScroll;

	// Token: 0x04000DD4 RID: 3540
	public UIWrapContent changeWeaponWrap;

	// Token: 0x04000DD5 RID: 3541
	public GameObject weaponPreviewPrefab;

	// Token: 0x04000DD6 RID: 3542
	public int weaponIndexInScroll;

	// Token: 0x04000DD7 RID: 3543
	public int weaponIndexInScrollOld;

	// Token: 0x04000DD8 RID: 3544
	public int widthWeaponScrollPreview;

	// Token: 0x04000DD9 RID: 3545
	public AudioClip lowResourceBeep;

	// Token: 0x04000DDA RID: 3546
	public UIPanel joystikPanel;

	// Token: 0x04000DDB RID: 3547
	public UIPanel shopPanel;

	// Token: 0x04000DDC RID: 3548
	public UIPanel bloodPanel;

	// Token: 0x04000DDD RID: 3549
	public UILabel perfectLabels;

	// Token: 0x04000DDE RID: 3550
	[SerializeField]
	private PrefabHandler _respawnWindowPrefab;

	// Token: 0x04000DDF RID: 3551
	private LazyObject<RespawnWindow> _lazyRespWindow;

	// Token: 0x04000DE0 RID: 3552
	public UIPanel offGameGuiPanel;

	// Token: 0x04000DE1 RID: 3553
	public UIButton pauseButton;

	// Token: 0x04000DE2 RID: 3554
	public UIButton exitButton;

	// Token: 0x04000DE3 RID: 3555
	public GameObject mineRed;

	// Token: 0x04000DE4 RID: 3556
	public GameObject mineBlue;

	// Token: 0x04000DE5 RID: 3557
	public GameObject winningBlue;

	// Token: 0x04000DE6 RID: 3558
	public GameObject winningRed;

	// Token: 0x04000DE7 RID: 3559
	public GameObject firstPlaceGO;

	// Token: 0x04000DE8 RID: 3560
	public GameObject firstPlaceCoop;

	// Token: 0x04000DE9 RID: 3561
	public UILabel placeDeathmatchLabel;

	// Token: 0x04000DEA RID: 3562
	public UILabel placeCoopLabel;

	// Token: 0x04000DEB RID: 3563
	public GameObject bankView;

	// Token: 0x04000DEC RID: 3564
	public GameObject bankViewLow;

	// Token: 0x04000DED RID: 3565
	private IEnumerator _lowResourceBeepRoutine;

	// Token: 0x04000DEE RID: 3566
	private float timerBlinkNoAmmo;

	// Token: 0x04000DEF RID: 3567
	private float periodBlink = 2f;

	// Token: 0x04000DF0 RID: 3568
	public UILabel blinkNoAmmoLabel;

	// Token: 0x04000DF1 RID: 3569
	private float timerBlinkNoHeath;

	// Token: 0x04000DF2 RID: 3570
	public UILabel blinkNoHeathLabel;

	// Token: 0x04000DF3 RID: 3571
	public UISprite[] blinkNoHeathFrames;

	// Token: 0x04000DF4 RID: 3572
	private int oldCountHeath;

	// Token: 0x04000DF5 RID: 3573
	public float timerShowScorePict;

	// Token: 0x04000DF6 RID: 3574
	public float maxTimerShowScorePict = 3f;

	// Token: 0x04000DF7 RID: 3575
	public string scorePictName = string.Empty;

	// Token: 0x04000DF8 RID: 3576
	public UISprite ChargeValue;

	// Token: 0x04000DF9 RID: 3577
	public PlayGadgetSFX timeTravelEffect;

	// Token: 0x04000DFA RID: 3578
	public PlayGadgetSFX burningEffect;

	// Token: 0x04000DFB RID: 3579
	public PlayGadgetSFX reviveEffect;

	// Token: 0x04000DFC RID: 3580
	public PlayGadgetSFX healEffect;

	// Token: 0x04000DFD RID: 3581
	public PlayGadgetSFX pandoraSuccessEffect;

	// Token: 0x04000DFE RID: 3582
	public PlayGadgetSFX pandoraFailEffect;

	// Token: 0x04000DFF RID: 3583
	public PlayGadgetSFX disablerEffect;

	// Token: 0x04000E00 RID: 3584
	public PlayGadgetSFX blackMarkEffect;

	// Token: 0x04000E01 RID: 3585
	public PlayGadgetSFX medStationEffect;

	// Token: 0x04000E02 RID: 3586
	public PlayGadgetSFX shieldEffect;

	// Token: 0x04000E03 RID: 3587
	public PlayGadgetSFX drumEffect;

	// Token: 0x04000E04 RID: 3588
	public PlayGadgetSFX frozeEffect;

	// Token: 0x04000E05 RID: 3589
	public PlayGadgetSFX bleedEffect;

	// Token: 0x04000E06 RID: 3590
	public PlayGadgetSFX poisonEffect;

	// Token: 0x04000E07 RID: 3591
	[SerializeField]
	private GameObject _subpanelsContainer;

	// Token: 0x04000E08 RID: 3592
	private bool _kBlockPauseShopButton;

	// Token: 0x04000E09 RID: 3593
	public bool isTurretInterfaceActive;

	// Token: 0x04000E0A RID: 3594
	private bool _disabled;

	// Token: 0x04000E0B RID: 3595
	private bool crosshairVisible;

	// Token: 0x04000E0C RID: 3596
	private bool aimRectVisible;

	// Token: 0x04000E0D RID: 3597
	private Vector2[] aimPositions = new Vector2[6];

	// Token: 0x04000E0E RID: 3598
	private CrosshairData.aimSprite defaultAimCenter = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(0f, 0f));

	// Token: 0x04000E0F RID: 3599
	private CrosshairData.aimSprite defaultAimDown = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	// Token: 0x04000E10 RID: 3600
	private CrosshairData.aimSprite defaultAimUp = new CrosshairData.aimSprite("pricel_v", new Vector2(12f, 12f), new Vector2(0f, 8f));

	// Token: 0x04000E11 RID: 3601
	private CrosshairData.aimSprite defaultAimLeftCenter = new CrosshairData.aimSprite("pricel_h", new Vector2(12f, 12f), new Vector2(8f, 0f));

	// Token: 0x04000E12 RID: 3602
	private CrosshairData.aimSprite defaultAimLeftDown = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	// Token: 0x04000E13 RID: 3603
	private CrosshairData.aimSprite defaultAimLeftUp = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	// Token: 0x04000E14 RID: 3604
	private KeyValuePair<int, string>[] _formatMeleeAmmoMemo;

	// Token: 0x04000E15 RID: 3605
	private KeyValuePair<Ammo, string>[] _formatShootingAmmoMemo;

	// Token: 0x04000E16 RID: 3606
	private readonly StringBuilder _stringBuilder = new StringBuilder();

	// Token: 0x04000E17 RID: 3607
	private KeyValuePair<int, string> _rankMemo = new KeyValuePair<int, string>(0, "1");

	// Token: 0x04000E18 RID: 3608
	private float pastHealth;

	// Token: 0x04000E19 RID: 3609
	private float pastMechHealth;

	// Token: 0x04000E1A RID: 3610
	private float pastArmor;

	// Token: 0x04000E1B RID: 3611
	private bool mechWasActive;

	// Token: 0x04000E1C RID: 3612
	private int currentHealthStep;

	// Token: 0x04000E1D RID: 3613
	private int currentMechHealthStep;

	// Token: 0x04000E1E RID: 3614
	private int currentArmorStep;

	// Token: 0x04000E1F RID: 3615
	private bool healthInAnim;

	// Token: 0x04000E20 RID: 3616
	private bool armorInAnim;

	// Token: 0x04000E21 RID: 3617
	private bool mechInAnim;

	// Token: 0x020008E8 RID: 2280
	// (Invoke) Token: 0x06005040 RID: 20544
	public delegate float GetFloatVAlue();

	// Token: 0x020008E9 RID: 2281
	// (Invoke) Token: 0x06005044 RID: 20548
	public delegate string GetString();

	// Token: 0x020008EA RID: 2282
	// (Invoke) Token: 0x06005048 RID: 20552
	public delegate int GetIntVAlue();
}
