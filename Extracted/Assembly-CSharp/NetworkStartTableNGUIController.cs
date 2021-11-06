using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Facebook.Unity;
using Rilisoft;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public sealed class NetworkStartTableNGUIController : MonoBehaviour
{
	// Token: 0x060022D2 RID: 8914 RVA: 0x000AB80C File Offset: 0x000A9A0C
	private void Awake()
	{
		this.interfaceAnimator.runtimeAnimatorController = ((!Defs.isDuel) ? this.standartAnimController : this.DuelAnimController);
		NetworkStartTableNGUIController.sharedController = this;
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000AB848 File Offset: 0x000A9A48
	private void OnDestroy()
	{
		NetworkStartTableNGUIController.sharedController = null;
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000AB850 File Offset: 0x000A9A50
	private void Start()
	{
		if (BuffSystem.instance != null && !BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1 && HintController.instance != null)
		{
			HintController.instance.ShowHintByName("shop_remove_novice_armor", 0f);
		}
		this.cameraObj = base.transform.GetChild(0).gameObject;
		if (this.SexualButton != null)
		{
			ButtonHandler component = this.SexualButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.SexualButtonHandler;
			}
		}
		if (this.InAppropriateActButton != null)
		{
			ButtonHandler component2 = this.InAppropriateActButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.InAppropriateActButtonHandler;
			}
		}
		if (this.OtherButton != null)
		{
			ButtonHandler component3 = this.OtherButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.OtherButtonHandler;
			}
		}
		if (this.ReportButton != null)
		{
			ButtonHandler component4 = this.ReportButton.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += this.ShowReasonPanel;
			}
		}
		if (this.AddButton != null)
		{
			ButtonHandler component5 = this.AddButton.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += this.AddButtonHandler;
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			this.listOfPlayers.transform.localPosition -= 50f * Vector3.up;
			if (NetworkStartTable.LocalOrPasswordRoom())
			{
				this.MapSelectPanel.transform.localPosition += 80f * Vector3.up;
			}
		}
		if (this.duelPanel != null)
		{
			this.duelUI = this.duelPanel.GetComponent<DuelUIController>();
		}
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000ABA94 File Offset: 0x000A9C94
	private void Update()
	{
		if (ExpController.Instance != null && ExpController.Instance.experienceView != null)
		{
			bool levelUpPanelOpened = ExpController.Instance.experienceView.LevelUpPanelOpened;
			if (this.cameraObj.activeSelf == levelUpPanelOpened)
			{
				this.cameraObj.SetActive(!levelUpPanelOpened);
			}
		}
		if ((Defs.isHunger || Defs.isRegimVidosDebug) && this.spectatorModeBtnPnl.activeSelf && Initializer.players.Count == 0)
		{
			this.spectatorModeBtnPnl.SetActive(false);
			this.spectratorModePnl.SetActive(false);
			this.ShowTable(false);
		}
		this.facebookButton.SetActive(FacebookController.FacebookSupported && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn);
		this.twitterButton.SetActive(TwitterController.TwitterSupported && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		bool flag = this.facebookButton.activeSelf || this.twitterButton.activeSelf;
		if (this.socialPnl.activeSelf != flag)
		{
			this.socialPnl.SetActive(flag);
		}
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000ABBD4 File Offset: 0x000A9DD4
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Network Start Table GUI");
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000ABC10 File Offset: 0x000A9E10
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000ABC30 File Offset: 0x000A9E30
	public void HandleEscape()
	{
		if (this.ReasonsPanel != null && this.ReasonsPanel.activeInHierarchy)
		{
			this.BackFromReasonPanel();
		}
		else if (this.ActionPanel != null && this.ActionPanel.activeInHierarchy)
		{
			this.CancelFromActionPanel();
		}
		else if (ShopNGUIController.GuiActive)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().HandleResumeFromShop();
			}
		}
		else if (this.hideOldRanksButton.activeInHierarchy)
		{
			List<EventDelegate> onClick = this.hideOldRanksButton.GetComponent<UIButton>().onClick;
			EventDelegate.Execute(onClick);
		}
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000ABD04 File Offset: 0x000A9F04
	public void ShowActionPanel(string _pixelbookID, string _nick)
	{
		this.pixelbookID = _pixelbookID;
		this.nick = _nick;
		this.HideTable();
		for (int i = 0; i < this.actionPanelNicklabel.Length; i++)
		{
			this.actionPanelNicklabel[i].text = this.nick;
		}
		this.ActionPanel.SetActive(true);
		this.spectatorModeBtnPnl.SetActive(false);
		if (FriendsController.sharedController.IsShowAdd(this.pixelbookID) && this.CountAddFriens < 3)
		{
			this.AddButton.GetComponent<UIButton>().isEnabled = true;
		}
		else
		{
			this.AddButton.GetComponent<UIButton>().isEnabled = false;
		}
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000ABDB4 File Offset: 0x000A9FB4
	public void HideActionPanel()
	{
		this.ActionPanel.SetActive(false);
		this.ShowTable(this.updateRealTableAfterActionPanel);
		if ((Defs.isHunger || Defs.isRegimVidosDebug) && Initializer.players.Count > 0)
		{
			this.spectatorModeBtnPnl.SetActive(Initializer.players.Count != 0);
		}
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000ABE18 File Offset: 0x000AA018
	public void ShowReasonPanel(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Debug.Log("ShowReasonPanel");
		this.ReasonsPanel.SetActive(true);
		this.ActionPanel.SetActive(false);
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000ABE84 File Offset: 0x000AA084
	public void HideReasonPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(true);
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000ABEE4 File Offset: 0x000AA0E4
	public bool CheckHideInternalPanel()
	{
		if (this.ActionPanel.activeInHierarchy)
		{
			this.CancelFromActionPanel();
			return true;
		}
		if (this.ReasonsPanel.activeInHierarchy)
		{
			this.BackFromReasonPanel();
			return true;
		}
		return false;
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000ABF24 File Offset: 0x000AA124
	public void AddButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Debug.Log("[Add] " + this.pixelbookID);
		this.CountAddFriens++;
		string value = (!Defs.isDaterRegim) ? "Multiplayer Battle" : "Sandbox (Dating)";
		Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
		{
			{
				"Added Friends",
				value
			},
			{
				"Deleted Friends",
				"Add"
			}
		};
		FriendsController.sharedController.SendInvitation(this.pixelbookID, socialEventParameters);
		if (!FriendsController.sharedController.notShowAddIds.Contains(this.pixelbookID))
		{
			FriendsController.sharedController.notShowAddIds.Add(this.pixelbookID);
		}
		this.AddButton.GetComponent<UIButton>().isEnabled = false;
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x000AC020 File Offset: 0x000AA220
	public void CancelFromActionPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.HideActionPanel();
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000AC070 File Offset: 0x000AA270
	public void BackFromReasonPanel()
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		this.HideReasonPanel();
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000AC0C0 File Offset: 0x000AA2C0
	public void InAppropriateActButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Action handler = delegate()
		{
			string value = this._versionString.Value;
			string text = string.Concat(new object[]
			{
				"mailto:",
				Defs.SupportMail,
				"?subject=INAPPROPRIATE ACT ",
				this.nick,
				"(",
				this.pixelbookID,
				")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20",
				DateTime.Now.ToString(),
				"%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20",
				value,
				"%0D%0APlayerID:%20",
				FriendsController.sharedController.id,
				"%0D%0ACategory:%20INAPPROPRIATE ACT ",
				this.nick,
				"(",
				this.pixelbookID,
				")%0D%0ADevice%20Type:%20",
				SystemInfo.deviceType,
				"%20",
				SystemInfo.deviceModel,
				"%0D%0AOS%20Version:%20",
				SystemInfo.operatingSystem,
				"%0D%0A------------------------"
			});
			text = text.Replace(" ", "%20");
			Application.OpenURL(text);
		};
		FeedbackMenuController.ShowDialogWithCompletion(handler);
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000AC11C File Offset: 0x000AA31C
	public void SexualButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Action handler = delegate()
		{
			string value = this._versionString.Value;
			string text = string.Concat(new object[]
			{
				"mailto:",
				Defs.SupportMail,
				"?subject=CHEATING ",
				this.nick,
				"(",
				this.pixelbookID,
				")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20",
				DateTime.Now.ToString(),
				"%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20",
				value,
				"%0D%0APlayerID:%20",
				FriendsController.sharedController.id,
				"%0D%0ACategory:%20CHEATING ",
				this.nick,
				"(",
				this.pixelbookID,
				")%0D%0ADevice%20Type:%20",
				SystemInfo.deviceType,
				"%20",
				SystemInfo.deviceModel,
				"%0D%0AOS%20Version:%20",
				SystemInfo.operatingSystem,
				"%0D%0A------------------------"
			});
			text = text.Replace(" ", "%20");
			Application.OpenURL(text);
		};
		FeedbackMenuController.ShowDialogWithCompletion(handler);
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000AC178 File Offset: 0x000AA378
	public void OtherButtonHandler(object sender, EventArgs e)
	{
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		Action handler = delegate()
		{
			string value = this._versionString.Value;
			string text = string.Concat(new object[]
			{
				"mailto:",
				Defs.SupportMail,
				"?subject=Report ",
				this.nick,
				"(",
				this.pixelbookID,
				")&body=%0D%0A%0D%0A%0D%0A%0D%0A%0D%0A------------%20DO NOT DELETE%20------------%0D%0AUTC%20Time:%20",
				DateTime.Now.ToString(),
				"%0D%0AGame:%20PixelGun3D%0D%0AVersion:%20",
				value,
				"%0D%0APlayerID:%20",
				FriendsController.sharedController.id,
				"%0D%0ACategory:%20Report ",
				this.nick,
				"(",
				this.pixelbookID,
				")%0D%0ADevice%20Type:%20",
				SystemInfo.deviceType,
				"%20",
				SystemInfo.deviceModel,
				"%0D%0AOS%20Version:%20",
				SystemInfo.operatingSystem,
				"%0D%0A------------------------"
			});
			text = text.Replace(" ", "%20");
			Application.OpenURL(text);
		};
		FeedbackMenuController.ShowDialogWithCompletion(handler);
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000AC1D4 File Offset: 0x000AA3D4
	public void StartSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(true);
		}
		this.spectatorModeOnBtn.SetActive(true);
		this.spectatorModeOffBtn.SetActive(false);
		this.spectratorModePnl.SetActive(true);
		this.socialPnl.SetActive(false);
		this.MapSelectPanel.SetActive(false);
		this.HideTable();
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = true;
		}
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000AC270 File Offset: 0x000AA470
	public void EndSpectatorMode()
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		this.spectatorModeOnBtn.SetActive(false);
		this.spectatorModeOffBtn.SetActive(true);
		this.spectratorModePnl.SetActive(false);
		this.MapSelectPanel.SetActive(true);
		if (WeaponManager.sharedManager.myTable != null)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer != null)
			{
				Player_move_c.SetLayerRecursively(WeaponManager.sharedManager.myNetworkStartTable.currentGameObjectPlayer.transform.GetChild(0).gameObject, 0);
			}
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isRegimVidos = false;
		}
		this.ShowTable(true);
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000AC340 File Offset: 0x000AA540
	[Obfuscation(Exclude = true)]
	public void HideAvardPanel()
	{
		if (this.isCancelHideAvardPanel)
		{
			return;
		}
		this.rewardWindow = null;
		this.ShowEndInterface(this.winner, this.winnerCommand, false);
		if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
		}
		else
		{
			UnityEngine.Object.Destroy(NetworkStartTableNGUIController.sharedController.gameObject);
		}
		this.isCancelHideAvardPanel = true;
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000AC3B8 File Offset: 0x000AA5B8
	public static RewardWindowBase ShowRewardWindow(bool win, Transform par)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/WinWindowNGUI"));
		RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority priority = FacebookController.StoryPriority.Red;
		component.priority = priority;
		component.twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		component.shareAction = delegate()
		{
			FacebookController.PostOpenGraphStory("win", "battle", priority, new Dictionary<string, string>
			{
				{
					"battle",
					ConnectSceneNGUIController.regim.ToString().ToLower()
				}
			});
		};
		component.HasReward = true;
		component.CollectOnlyNoShare = !win;
		component.twitterStatus = (() => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u");
		component.EventTitle = "Won Batlle";
		gameObject.transform.parent = par;
		Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUITable"));
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000AC4B8 File Offset: 0x000AA6B8
	// (set) Token: 0x060022E9 RID: 8937 RVA: 0x000AC4C0 File Offset: 0x000AA6C0
	public RewardWindowBase rewardWindow { get; set; }

	// Token: 0x060022EA RID: 8938 RVA: 0x000AC4CC File Offset: 0x000AA6CC
	public void ShowFinishedInterface(bool isWinner, bool deadHeat)
	{
		bool flag = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
		this.finishedInterface.SetActive(true);
		string text = LocalizationStore.Get((!Defs.isDaterRegim) ? ((!deadHeat) ? ((!isWinner) ? ((!flag) ? "Key_1976" : "Key_1116") : "Key_1115") : ((!Defs.isDuel || Initializer.networkTables.Count >= 2) ? "Key_0571" : "Key_2436")) : "Key_1987");
		foreach (GameObject gameObject in this.finishDraw)
		{
			gameObject.SetActive((deadHeat || (!isWinner && !flag)) && !Defs.isDaterRegim);
		}
		foreach (GameObject gameObject2 in this.finishWin)
		{
			gameObject2.SetActive(isWinner && !deadHeat && !Defs.isDaterRegim);
		}
		foreach (GameObject gameObject3 in this.finishDefeat)
		{
			gameObject3.SetActive(flag && !isWinner && !deadHeat && !Defs.isDaterRegim);
		}
		for (int l = 0; l < this.finishedInterfaceLabels.Length; l++)
		{
			this.finishedInterfaceLabels[l].text = text;
		}
		this.finishedInterfaceLabels[0].gameObject.SetActive(true);
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000AC694 File Offset: 0x000AA894
	[Obsolete]
	public void showAvardPanel(string _winner, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, int _winnerCommand)
	{
		this.isCancelHideAvardPanel = false;
		if (_isCustom)
		{
			this.addCoins = 0;
			this.addExperience = 0;
		}
		else
		{
			this.addCoins = _addCoin;
			this.addExperience = _addExpierence;
		}
		string text = string.Format("+ {0} {1}", _addCoin, LocalizationStore.Key_0275);
		string text2 = string.Format("+ {0} {1}", _addExpierence, LocalizationStore.Key_0204);
		ConnectSceneNGUIController.RegimGame regim = ConnectSceneNGUIController.regim;
		PremiumAccountController instance = PremiumAccountController.Instance;
		bool flag = regim == ConnectSceneNGUIController.RegimGame.Deathmatch || regim == ConnectSceneNGUIController.RegimGame.FlagCapture || regim == ConnectSceneNGUIController.RegimGame.TeamFight || regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
		bool flag2 = PromoActionsManager.sharedManager.IsDayOfValorEventActive && flag;
		bool flag3 = instance.IsActiveOrWasActiveBeforeStartMatch();
		int num = 1;
		int num2 = 1;
		if (flag3 || flag2)
		{
			num = ((!Defs.isCOOP && !Defs.isHunger) ? AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false) : PremiumAccountController.Instance.RewardCoeff);
			num2 = ((!Defs.isCOOP && !Defs.isHunger) ? AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true) : PremiumAccountController.Instance.RewardCoeff);
		}
		this.rewardWindow = NetworkStartTableNGUIController.ShowRewardWindow(firstPlace, NetworkStartTableNGUIController.sharedController.allInterfacePanel.transform.parent);
		this.rewardWindow.customHide = delegate()
		{
			this.rewardWindow.CancelInvoke("Hide");
			this.HideAvardPanel();
		};
		RewardWindowAfterMatch component = this.rewardWindow.GetComponent<RewardWindowAfterMatch>();
		component.victory.SetActive(true);
		component.lose.SetActive(false);
		if (flag3 && flag2)
		{
			component.daysAndPremiumBack.SetActive(true);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(false);
		}
		else if (flag3)
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(true);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(false);
		}
		else if (flag2)
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(true);
			component.normlaBeckground.SetActive(false);
		}
		else
		{
			component.daysAndPremiumBack.SetActive(false);
			component.premiumBackground.SetActive(false);
			component.daysOfValorBackground.SetActive(false);
			component.normlaBeckground.SetActive(true);
		}
		component.coinsMultiplierContainer.SetActive(num2 > 1 && _addCoin > 0);
		component.coinsMultiplier.text = "x" + num2.ToString();
		component.expMultiplierContainer.SetActive(num > 1);
		component.expMilyiplier.text = "x" + num.ToString();
		foreach (UILabel uilabel in component.coins)
		{
			uilabel.text = text;
		}
		foreach (UILabel uilabel2 in component.exp)
		{
			uilabel2.text = text2;
		}
		if (_addCoin == 0)
		{
			component.coinsContainer.SetActive(false);
			component.expContainer.transform.localPosition = new Vector3(0f, component.expContainer.transform.localPosition.y, component.expContainer.transform.localPosition.z);
		}
		this.endInterfacePanel.SetActive(true);
		this.finishedInterface.SetActive(false);
		this.MapSelectPanel.SetActive(false);
		this.socialPnl.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
		this.winnerCommand = _winnerCommand;
		this.winner = _winner;
		if (Defs.isDaterRegim)
		{
		}
		if (this.addExperience > 0)
		{
			ExperienceController.sharedController.addExperience(this.addExperience);
		}
		if (this.addCoins > 0)
		{
			int @int = Storager.getInt("Coins", false);
			Storager.setInt("Coins", @int + this.addCoins, false);
			AnalyticsFacade.CurrencyAccrual(this.addCoins, "Coins", AnalyticsConstants.AccrualType.Earned);
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000ACB38 File Offset: 0x000AAD38
	public void ShowFinishedDuelInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool isWinner, bool deadheat)
	{
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000ACB3C File Offset: 0x000AAD3C
	public void ShowStartDuelInterface()
	{
		this.duelUI = this.duelPanel.GetComponent<DuelUIController>();
		this.startInterfacePanel.SetActive(false);
		this.allInterfacePanel.SetActive(false);
		this.HideTable();
		this.duelPanel.SetActive(true);
		this.duelUI.ShowStartInterface();
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000ACB90 File Offset: 0x000AAD90
	public void ShowStartInterface()
	{
		if (Defs.isDuel)
		{
			this.ShowStartDuelInterface();
			return;
		}
		string term;
		if (Defs.isDaterRegim)
		{
			foreach (UILabel uilabel in this.gameModeLabel)
			{
				uilabel.text = LocalizationStore.Get("Key_1567");
			}
		}
		else if (ConnectSceneNGUIController.gameModesLocalizeKey.TryGetValue(Convert.ToInt32(ConnectSceneNGUIController.regim).ToString(), out term))
		{
			foreach (UILabel uilabel2 in this.gameModeLabel)
			{
				uilabel2.text = LocalizationStore.Get(term);
			}
		}
		this.questsButton.SetActive(TrainingController.TrainingCompleted && !Defs.isHunger);
		this.MapSelectPanel.SetActive(false);
		this.goBattleLabel.SetActive(!Defs.isDaterRegim);
		this.daterButtonLabel.SetActive(Defs.isDaterRegim);
		this.allInterfacePanel.SetActive(true);
		this.startInterfacePanel.SetActive(true);
		this.rewardPanel.SetActive(false);
		this.isRewardShow = false;
		this.ShowTable(true);
		base.StartCoroutine("TryToShowExpiredBanner");
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000ACCD8 File Offset: 0x000AAED8
	public void ShowNewMatchInterface()
	{
		this.isRewardShow = false;
		this.rewardPanel.SetActive(false);
		this.allInterfacePanel.SetActive(true);
		this.startInterfacePanel.SetActive(true);
		this.ShowTable(true);
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000ACD18 File Offset: 0x000AAF18
	public void HideStartInterface()
	{
		this.isRewardShow = false;
		this.rewardPanel.SetActive(false);
		Debug.Log("HideStartInterface");
		this.finishedInterface.SetActive(false);
		this.allInterfacePanel.SetActive(false);
		this.startInterfacePanel.SetActive(false);
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		this.updateRealTableAfterActionPanel = true;
		this.HideTable();
		base.StopCoroutine("TryToShowExpiredBanner");
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000ACD98 File Offset: 0x000AAF98
	public void ShowEndInterfaceDeadInHunger(string _winner, RatingSystem.RatingChange ratingChange)
	{
		base.StartCoroutine(this.MatchFinishedInterface(_winner, ratingChange, false, 0, 0, false, false, false, 0, 0, 0, true));
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000ACDC0 File Offset: 0x000AAFC0
	public void MathFinishedDeadInHunger()
	{
		if (this.spectratorModePnl.activeSelf)
		{
			this.EndSpectatorMode();
		}
		else
		{
			this.spectatorModeOnBtn.SetActive(false);
			this.spectatorModeOffBtn.SetActive(true);
			this.spectratorModePnl.SetActive(false);
		}
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000ACE0C File Offset: 0x000AB00C
	public void FreezePlayer()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.BlockPlayerInEnd();
			InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
			if (ChatViewrController.sharedController != null)
			{
				UnityEngine.Object.Destroy(ChatViewrController.sharedController.gameObject);
			}
		}
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000ACE6C File Offset: 0x000AB06C
	public IEnumerator MatchFinishedInterface(string _winner, RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool iAmWinnerInTeam, int _winnerCommand, int blueTotal, int redTotal, bool deadInHunger = false)
	{
		for (int i = 0; i < this.totalBlue.Length; i++)
		{
			this.totalBlue[i].text = blueTotal.ToString();
		}
		for (int j = 0; j < this.totalRed.Length; j++)
		{
			this.totalRed[j].text = redTotal.ToString();
		}
		this.ranksTable.totalBlue = blueTotal;
		this.ranksTable.totalRed = redTotal;
		bool isTeamMode = ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
		this.interfaceAnimator.ResetTrigger("RewardTaken");
		this.interfaceAnimator.ResetTrigger("GetReward");
		this.interfaceAnimator.SetBool("IsTwoTeams", isTeamMode);
		this.interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
		this.interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 31 && _addCoin > 0));
		this.interfaceAnimator.SetBool("isHunger", Defs.isHunger);
		this.interfaceAnimator.SetBool("isDater", Defs.isDaterRegim);
		this.interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
		this.interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
		this.interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
		this.interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
		this.interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
		this.interfaceAnimator.SetBool("isTrophyAdded", false);
		this.interfaceAnimator.SetBool("isRatingSystem", true);
		this.interfaceAnimator.SetBool("interfaceAnimationDone", false);
		this.isUsed = false;
		this.trophiAddIcon.SetActive(ratingChange.addRating > 0);
		this.trophiMinusIcon.SetActive(ratingChange.addRating < 0);
		this.trophyPanel.SetActive(ratingChange.addRating != 0);
		this.currentCup.spriteName = ratingChange.oldLeague.ToString() + " " + (3 - ratingChange.oldDivision).ToString();
		this.NewCup.spriteName = ratingChange.newLeague.ToString() + " " + (3 - ratingChange.newDivision).ToString();
		if (ratingChange.addRating > 0)
		{
			this.currentBarFillAmount = ratingChange.oldRatingAmount;
			this.nextBarFillAmount = ratingChange.newRatingAmount;
			this.currentBar.fillAmount = this.currentBarFillAmount;
			this.nextBar.fillAmount = this.currentBarFillAmount;
			this.nextBar.color = Color.yellow;
		}
		else
		{
			this.currentBarFillAmount = ratingChange.oldRatingAmount;
			this.nextBarFillAmount = ratingChange.newRatingAmount;
			this.currentBar.fillAmount = this.nextBarFillAmount;
			this.nextBar.fillAmount = this.nextBarFillAmount;
			this.nextBar.color = Color.red;
		}
		this.leagueUp = (ratingChange.newLeague > ratingChange.oldLeague);
		if (ratingChange.maxRating == 2147483647)
		{
			this.trophyPoints.text = ratingChange.newRating.ToString();
		}
		else
		{
			this.trophyPoints.text = ratingChange.newRating.ToString() + "/" + ratingChange.maxRating.ToString();
		}
		this.trophyShine.SetActive(ratingChange.isUp);
		this.trophyRewardValue = ratingChange.addRating;
		string trophyAwardText = string.Format((ratingChange.addRating <= 0) ? "{0}" : "+{0}", this.trophyRewardValue);
		foreach (UILabel label in this.rewardTrophy)
		{
			label.gameObject.SetActive(ratingChange.addRating != 0);
			label.text = trophyAwardText;
		}
		string leagueChangeText = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), RatingSystem.divisionByIndex[ratingChange.newDivision]);
		foreach (UILabel label2 in this.textLeagueUp)
		{
			label2.text = leagueChangeText;
		}
		foreach (UILabel label3 in this.textLeagueDown)
		{
			label3.text = leagueChangeText;
		}
		this.shareToggle.value = (firstPlace && showAward && (FB.IsLoggedIn || TwitterController.IsLoggedIn));
		this.shareToggle.gameObject.SetActive(this.shareToggle.value);
		if (this.defaultTeamOneState == Vector3.zero)
		{
			this.defaultTeamOneState = this.teamOneLabel.transform.localPosition;
		}
		if (this.defaultTeamTwoState == Vector3.zero)
		{
			this.defaultTeamTwoState = this.teamTwoLabel.transform.localPosition;
		}
		if (!isTeamMode || _winnerCommand == 0)
		{
			this.teamOneLabel.transform.localPosition = this.defaultTeamOneState;
			this.teamTwoLabel.transform.localPosition = this.defaultTeamTwoState;
		}
		else
		{
			this.teamOneLabel.transform.localPosition = this.defaultTeamOneState + Vector3.right * 55f;
			this.teamTwoLabel.transform.localPosition = this.defaultTeamTwoState + Vector3.left * 55f;
		}
		if (Defs.isHunger)
		{
			this.EndSpectatorMode();
			this.HideTable();
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			this.FreezePlayer();
			this.interfaceAnimator.SetTrigger("MatchEnd");
			this.shopButton.GetComponent<Collider>().enabled = Defs.isDaterRegim;
			this.ShowFinishedInterface(iAmWinnerInTeam, (Defs.isCompany || Defs.isFlag || Defs.isCapturePoints) && _winnerCommand == 0);
			this.waitForAnimationDone = true;
			while (this.waitForAnimationDone)
			{
				yield return null;
			}
		}
		this.rewardCoinsObject.gameObject.SetActive(false);
		this.rewardExpObject.gameObject.SetActive(false);
		if (showAward || ratingChange.addRating != 0)
		{
			this.interfaceAnimator.SetTrigger("Reward");
			if (ExperienceController.sharedController.currentLevel == 31 && _addCoin > 0)
			{
				this.expRewardValue = 0;
			}
			this.expRewardValue = _addExpierence;
			this.coinsRewardValue = _addCoin;
		}
		else
		{
			this.expRewardValue = 0;
			this.coinsRewardValue = 0;
		}
		this.isRewardShow = (showAward || ratingChange.addRating != 0);
		if (Defs.isDaterRegim)
		{
			for (int k = 0; k < this.finishedInterfaceLabels.Length; k++)
			{
				this.finishedInterfaceLabels[k].text = _winner;
			}
		}
		ExperienceController.sharedController.isShowRanks = true;
		WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
		if (showAward)
		{
			this.ShowAwardEndInterface(_winner, _addCoin, _addExpierence, _isCustom, firstPlace, _winnerCommand);
		}
		else
		{
			this.ShowEndInterface(_winner, _winnerCommand, deadInHunger);
		}
		if (Defs.isHunger)
		{
			this.backButtonInHunger.SetActive(false);
			this.randomBtn.SetActive(false);
			this.MapSelectPanel.SetActive(false);
			this.questsButton.SetActive(false);
			this.spectatorModeBtnPnl.SetActive(false);
		}
		if (this.leagueUp)
		{
			this.rewardButton.SetActive(true);
			string newLeagueEggID = this.GiveLeagueEggAndReturnId();
			List<Texture> items = this.GetLeagueItems(newLeagueEggID);
			for (int l = 0; l < this.trophyItems.Length; l++)
			{
				if (l >= items.Count)
				{
					this.trophyItems[l].gameObject.SetActive(false);
				}
				else
				{
					this.trophyItems[l].gameObject.SetActive(true);
					this.trophyItems[l].mainTexture = items[l];
				}
			}
			this.rewardPanel.SetActive(this.trophyItems[0].gameObject.activeSelf);
			this.labelNewItems.SetActive(this.trophyItems[0].gameObject.activeSelf);
			this.rewardFrame.ResizeFrame();
			base.Invoke("OnTrophyOkButtonPress", 60f);
		}
		else
		{
			this.rewardButton.SetActive(false);
			this.rewardPanel.SetActive(false);
			this.labelNewItems.SetActive(false);
		}
		this.waitForTrophyAnimationDone = (ratingChange.addRating != 0);
		while (this.waitForTrophyAnimationDone)
		{
			yield return null;
		}
		if (Defs.isHunger)
		{
			this.backButtonInHunger.SetActive(true);
			this.randomBtn.SetActive(true);
			this.spectatorModeBtnPnl.SetActive(true);
		}
		for (int m = 0; m < this.trophyItems.Length; m++)
		{
			this.trophyItems[m].gameObject.SetActive(false);
		}
		if (showAward)
		{
			this.rewardExpObject.color = Color.white;
			this.rewardCoinsObject.color = Color.white;
			this.rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
			this.rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 31);
			this.rewardPanel.SetActive(this.rewardCoinsObject.gameObject.activeSelf || this.rewardExpObject.gameObject.activeSelf);
			this.rewardFrame.ResizeFrame();
		}
		yield break;
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000ACF44 File Offset: 0x000AB144
	private string GiveLeagueEggAndReturnId()
	{
		string result = string.Empty;
		try
		{
			RatingSystem.RatingLeague nextLeague = RatingSystem.instance.currentLeague + 1;
			string key = this.EggGivenForLeagueKey(nextLeague);
			if (Storager.getInt(key, false) == 0)
			{
				EggData eggData2 = Singleton<EggsManager>.Instance.GetAllEggs().FirstOrDefault((EggData eggData) => eggData.League == nextLeague);
				if (eggData2 != null)
				{
					result = eggData2.Id;
					Singleton<EggsManager>.Instance.AddEgg(eggData2);
				}
				else
				{
					Debug.LogErrorFormat("Giving league egg: No egg found for league {0}", new object[]
					{
						nextLeague.ToString()
					});
				}
				Storager.setInt(key, 1, false);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in giving league egg: {0}", new object[]
			{
				ex
			});
		}
		return result;
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000AD030 File Offset: 0x000AB230
	private string EggGivenForLeagueKey(RatingSystem.RatingLeague league)
	{
		return string.Format("{0}_egg_given", league.ToString());
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000AD048 File Offset: 0x000AB248
	private List<Texture> GetLeagueItems(string newLeagueEggID)
	{
		RatingSystem.RatingLeague league = RatingSystem.instance.currentLeague;
		List<Texture> list = new List<Texture>();
		List<WeaponSkin> list2 = WeaponSkinsManager.SkinsForLeague(league);
		foreach (WeaponSkin weaponSkin in list2)
		{
			list.Add(ItemDb.GetItemIcon(weaponSkin.Id, ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory, null, true));
		}
		List<string> list3 = Wear.UnboughtLeagueItemsByLeagues()[league];
		for (int i = 0; i < list3.Count; i++)
		{
			list.Add(ItemDb.GetTextureForShopItem(list3[i]));
		}
		List<SkinItem> list4 = (from kvp in SkinsController.sharedController.skinItemsDict
		where kvp.Value.currentLeague == league
		select kvp.Value).ToList<SkinItem>();
		foreach (SkinItem skinItem in list4)
		{
			list.Add(Resources.Load<Texture>(string.Format("LeagueSkinsProfileImages/league{0}_skin_profile", (int)(league + 1))));
		}
		try
		{
			if (!newLeagueEggID.IsNullOrEmpty())
			{
				list.Add(ItemDb.GetItemIcon(newLeagueEggID, ShopNGUIController.CategoryNames.EggsCategory, null, true));
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GetLEaguueItems, adding egg texture: {0}", new object[]
			{
				ex
			});
		}
		return list;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000AD248 File Offset: 0x000AB448
	public void OnTrophyAnimationDone()
	{
		if (this.isUsed)
		{
			return;
		}
		if (!this.leagueUp)
		{
			this.waitForTrophyAnimationDone = false;
			this.interfaceAnimator.SetBool("isTrophyAdded", true);
		}
		this.isUsed = true;
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000AD28C File Offset: 0x000AB48C
	public void OnMatchEndAnimationDone()
	{
		this.interfaceAnimator.SetBool("interfaceAnimationDone", true);
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000AD2A0 File Offset: 0x000AB4A0
	public IEnumerator MatchFinishedInDuelInterface(RatingSystem.RatingChange ratingChange, bool showAward, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, bool deadheat)
	{
		this.interfaceAnimator.ResetTrigger("RewardTaken");
		this.interfaceAnimator.ResetTrigger("GetReward");
		this.interfaceAnimator.ResetTrigger("AnimationEnds");
		this.interfaceAnimator.SetBool("isRewarded", showAward || ratingChange.addRating != 0);
		this.interfaceAnimator.SetBool("isExpOnly", _addCoin == 0 || (ExperienceController.sharedController.currentLevel == 31 && _addCoin > 0));
		this.interfaceAnimator.SetBool("IsTrophyUp", ratingChange.isUp);
		this.interfaceAnimator.SetBool("IsTrophyDown", ratingChange.isDown);
		this.interfaceAnimator.SetBool("IsTrophyAdd", ratingChange.addRating != 0 && !ratingChange.isUp && !ratingChange.isDown);
		this.interfaceAnimator.SetBool("NoRaiting", ratingChange.addRating == 0);
		this.interfaceAnimator.SetBool("isTrophyOnly", ratingChange.addRating != 0 && _addCoin == 0 && _addExpierence == 0);
		this.interfaceAnimator.SetBool("isTrophyAdded", false);
		this.interfaceAnimator.SetBool("interfaceAnimationDone", false);
		this.isUsed = false;
		this.trophiAddIcon.SetActive(ratingChange.addRating > 0);
		this.trophiMinusIcon.SetActive(ratingChange.addRating < 0);
		this.trophyPanel.SetActive(ratingChange.addRating != 0);
		this.currentCup.spriteName = ratingChange.oldLeague.ToString() + " " + (3 - ratingChange.oldDivision).ToString();
		this.NewCup.spriteName = ratingChange.newLeague.ToString() + " " + (3 - ratingChange.newDivision).ToString();
		if (ratingChange.addRating > 0)
		{
			this.currentBarFillAmount = ratingChange.oldRatingAmount;
			this.nextBarFillAmount = ratingChange.newRatingAmount;
			this.currentBar.fillAmount = this.currentBarFillAmount;
			this.nextBar.fillAmount = this.currentBarFillAmount;
			this.nextBar.color = Color.yellow;
		}
		else
		{
			this.currentBarFillAmount = ratingChange.oldRatingAmount;
			this.nextBarFillAmount = ratingChange.newRatingAmount;
			this.currentBar.fillAmount = this.nextBarFillAmount;
			this.nextBar.fillAmount = this.nextBarFillAmount;
			this.nextBar.color = Color.red;
		}
		this.leagueUp = (ratingChange.newLeague > ratingChange.oldLeague);
		if (ratingChange.maxRating == 2147483647)
		{
			this.trophyPoints.text = ratingChange.newRating.ToString();
		}
		else
		{
			this.trophyPoints.text = ratingChange.newRating.ToString() + "/" + ratingChange.maxRating.ToString();
		}
		this.trophyShine.SetActive(ratingChange.isUp);
		this.trophyRewardValue = ratingChange.addRating;
		string trophyAwardText = string.Format((ratingChange.addRating <= 0) ? "{0}" : "+{0}", this.trophyRewardValue);
		foreach (UILabel label in this.rewardTrophy)
		{
			label.gameObject.SetActive(ratingChange.addRating != 0);
			label.text = trophyAwardText;
		}
		string leagueChangeText = string.Format(LocalizationStore.Get(RatingSystem.leagueChangeLocalizations[(int)ratingChange.newLeague]), RatingSystem.divisionByIndex[ratingChange.newDivision]);
		foreach (UILabel label2 in this.textLeagueUp)
		{
			label2.text = leagueChangeText;
		}
		foreach (UILabel label3 in this.textLeagueDown)
		{
			label3.text = leagueChangeText;
		}
		this.shareToggle.value = false;
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			this.FreezePlayer();
			this.interfaceAnimator.SetTrigger("MatchEnd");
			this.shopButton.GetComponent<Collider>().enabled = false;
			this.ShowFinishedInterface(firstPlace, deadheat);
			this.waitForAnimationDone = true;
			while (this.waitForAnimationDone)
			{
				yield return null;
			}
		}
		this.rewardCoinsObject.gameObject.SetActive(false);
		this.rewardExpObject.gameObject.SetActive(false);
		if (showAward || ratingChange.addRating != 0)
		{
			this.interfaceAnimator.SetTrigger("Reward");
			if (ExperienceController.sharedController.currentLevel == 31 && _addCoin > 0)
			{
				this.expRewardValue = 0;
			}
			this.expRewardValue = _addExpierence;
			this.coinsRewardValue = _addCoin;
		}
		else
		{
			this.expRewardValue = 0;
			this.coinsRewardValue = 0;
		}
		this.isRewardShow = (showAward || ratingChange.addRating != 0);
		ExperienceController.sharedController.isShowRanks = true;
		WeaponManager.sharedManager.myNetworkStartTable.DestroyPlayer();
		if (showAward)
		{
			this.ShowAwardEndInterface(string.Empty, _addCoin, _addExpierence, _isCustom, firstPlace, 0);
		}
		else
		{
			this.ShowEndInterface(string.Empty, 0, false);
		}
		this.duelUI.ShowFinishedInterface(ratingChange, showAward, _addCoin, _addExpierence, firstPlace, deadheat);
		if (this.leagueUp)
		{
			this.rewardButton.SetActive(true);
			string newLeagueEggID = this.GiveLeagueEggAndReturnId();
			List<Texture> items = this.GetLeagueItems(newLeagueEggID);
			for (int i = 0; i < this.trophyItems.Length; i++)
			{
				if (i >= items.Count)
				{
					this.trophyItems[i].gameObject.SetActive(false);
				}
				else
				{
					this.trophyItems[i].gameObject.SetActive(true);
					this.trophyItems[i].mainTexture = items[i];
				}
			}
			this.rewardPanel.SetActive(this.trophyItems[0].gameObject.activeSelf);
			this.labelNewItems.SetActive(this.trophyItems[0].gameObject.activeSelf);
			this.rewardFrame.ResizeFrame();
			base.Invoke("OnTrophyOkButtonPress", 60f);
		}
		else
		{
			this.rewardButton.SetActive(false);
			this.rewardPanel.SetActive(false);
			this.labelNewItems.SetActive(false);
		}
		this.waitForTrophyAnimationDone = (ratingChange.addRating != 0);
		while (this.waitForTrophyAnimationDone)
		{
			yield return null;
		}
		for (int j = 0; j < this.trophyItems.Length; j++)
		{
			this.trophyItems[j].gameObject.SetActive(false);
		}
		if (showAward)
		{
			this.rewardExpObject.color = Color.white;
			this.rewardCoinsObject.color = Color.white;
			this.rewardCoinsObject.gameObject.SetActive(_addCoin > 0);
			this.rewardExpObject.gameObject.SetActive(ExperienceController.sharedController.currentLevel < 31);
			this.rewardPanel.SetActive(this.rewardCoinsObject.gameObject.activeSelf || this.rewardExpObject.gameObject.activeSelf);
			this.rewardFrame.ResizeFrame();
		}
		yield break;
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000AD328 File Offset: 0x000AB528
	public void OnTablesShow()
	{
		this.waitForAnimationDone = false;
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000AD334 File Offset: 0x000AB534
	public void OnRewardShow()
	{
		base.StartCoroutine(this.StartRewardAnimation());
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000AD344 File Offset: 0x000AB544
	public IEnumerator StartRewardAnimation()
	{
		this.rewardFrame.ResizeFrame();
		float animTime = 0f;
		while (ShopNGUIController.GuiActive || (BankController.Instance != null && BankController.Instance.InterfaceEnabled))
		{
			yield return null;
		}
		if (this.expRewardValue > 0)
		{
			Vector3 expStart = this.rewardExpObject.transform.localPosition;
			while (animTime < 1f)
			{
				this.rewardExpObject.transform.localPosition = Vector3.Lerp(expStart, this.rewardExpAnimPoint.localPosition, Mathf.Min(animTime, 1f));
				this.rewardExpObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(animTime, 1f));
				animTime += Time.deltaTime / 0.4f;
				yield return null;
			}
			ExperienceController.sharedController.addExperience(this.expRewardValue);
			if (WeaponManager.sharedManager.myNetworkStartTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.UpdateRanks();
			}
			this.expRewardValue = 0;
		}
		this.rewardExpObject.gameObject.SetActive(false);
		animTime = 0f;
		if (this.coinsRewardValue > 0)
		{
			Vector3 coinsStart = this.rewardCoinsObject.transform.localPosition;
			while (animTime < 1f)
			{
				if (!Defs.isHunger)
				{
					this.rewardCoinsObject.transform.localPosition = Vector3.Lerp(coinsStart, this.rewardCoinsAnimPoint.localPosition, Mathf.Min(animTime, 1f));
				}
				this.rewardCoinsObject.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), Mathf.Min(animTime, 1f));
				animTime += Time.deltaTime / 0.4f;
				yield return null;
			}
			BankController.AddCoins(this.coinsRewardValue, true, AnalyticsConstants.AccrualType.Earned);
			this.coinsRewardValue = 0;
		}
		this.rewardCoinsObject.gameObject.SetActive(false);
		animTime = 0f;
		yield break;
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x000AD360 File Offset: 0x000AB560
	public void StartTrophyAnim()
	{
		base.StartCoroutine(this.TrophyFillAnimation());
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000AD370 File Offset: 0x000AB570
	private IEnumerator TrophyFillAnimation()
	{
		float animTime = 0f;
		if (this.trophyRewardValue != 0)
		{
			while (animTime < 1f)
			{
				this.nextBar.fillAmount = Mathf.Lerp(this.currentBarFillAmount, this.nextBarFillAmount, Mathf.Min(animTime, 1f));
				animTime += Time.deltaTime;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000AD38C File Offset: 0x000AB58C
	public void ShowAwardEndInterface(string _winner, int _addCoin, int _addExpierence, bool _isCustom, bool firstPlace, int _winnerCommand)
	{
		if (_isCustom)
		{
			this.addCoins = 0;
			this.addExperience = 0;
		}
		else
		{
			this.addCoins = _addCoin;
			this.addExperience = _addExpierence;
		}
		string text = string.Format("+{0} {1}", _addCoin, LocalizationStore.Key_0275);
		string text2 = string.Format("+{0} {1}", _addExpierence, LocalizationStore.Key_0204);
		ConnectSceneNGUIController.RegimGame regim = ConnectSceneNGUIController.regim;
		PremiumAccountController instance = PremiumAccountController.Instance;
		bool flag = regim == ConnectSceneNGUIController.RegimGame.Deathmatch || regim == ConnectSceneNGUIController.RegimGame.FlagCapture || regim == ConnectSceneNGUIController.RegimGame.TeamFight || regim == ConnectSceneNGUIController.RegimGame.CapturePoints;
		bool flag2 = PromoActionsManager.sharedManager.IsDayOfValorEventActive && flag;
		bool flag3 = instance.IsActiveOrWasActiveBeforeStartMatch();
		if (flag3 || flag2)
		{
			int num = (!Defs.isCOOP && !Defs.isHunger) ? AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false) : PremiumAccountController.Instance.RewardCoeff;
			int num2 = (!Defs.isCOOP && !Defs.isHunger) ? AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true) : PremiumAccountController.Instance.RewardCoeff;
		}
		if (!flag3 || !flag2)
		{
			if (!flag3)
			{
				if (flag2)
				{
				}
			}
		}
		foreach (UILabel uilabel in this.rewardCoins)
		{
			uilabel.text = text;
		}
		foreach (UILabel uilabel2 in this.rewardExperience)
		{
			uilabel2.text = text2;
		}
		this.ShowEndInterface(_winner, _winnerCommand, false);
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000AD534 File Offset: 0x000AB734
	public void ShowEndInterface(string _winner, int _winnerCommand, bool deadInHunger = false)
	{
		if (!ShopNGUIController.NoviceArmorAvailable)
		{
			base.GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
		}
		NotificationController.instance.SaveTimeValues();
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.EndRound();
		}
		else
		{
			KillRateCheck.instance.CheckKillRate();
		}
		WeaponManager.sharedManager.myNetworkStartTable.ClearKillrate();
		this.endInterfacePanel.SetActive(!Defs.isDaterRegim);
		this.backButtonInHunger.SetActive(Defs.isHunger);
		if (Defs.isDuel)
		{
			return;
		}
		if (Defs.isCompany || Defs.isFlag || Defs.isCapturePoints)
		{
			this.winnerPanelCom1.SetActive(_winnerCommand == 1);
			this.winnerPanelCom2.SetActive(_winnerCommand == 2);
		}
		this.startInterfacePanel.SetActive(Defs.isDaterRegim);
		this.goBattleLabel.SetActive(!Defs.isDaterRegim);
		this.daterButtonLabel.SetActive(Defs.isDaterRegim);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.aimPanel.SetActive(false);
		}
		this.socialPnl.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64);
		this.winner = _winner;
		this.allInterfacePanel.SetActive(true);
		this.ranksTable.UpdateRanksFromOldSpisok();
		if (Defs.isHunger || Defs.isRegimVidosDebug)
		{
			if (Defs.isHunger)
			{
				this.randomBtn.SetActive(true);
				this.questsButton.SetActive(true);
			}
			this.spectatorModeBtnPnl.SetActive(true);
			this.updateRealTableAfterActionPanel = deadInHunger;
			if (!this.ActionPanel.activeSelf && !this.ReasonsPanel.activeSelf)
			{
				this.ShowTable(deadInHunger);
			}
		}
		else
		{
			this.updateRealTableAfterActionPanel = false;
			this.ShowTable(false);
			this.MapSelectPanel.SetActive(false);
			this.questsButton.SetActive(false);
		}
		base.StartCoroutine("TryToShowExpiredBanner");
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06002302 RID: 8962 RVA: 0x000AD730 File Offset: 0x000AB930
	// (set) Token: 0x06002303 RID: 8963 RVA: 0x000AD738 File Offset: 0x000AB938
	public FacebookController.StoryPriority facebookPriority
	{
		get
		{
			return this._facebookPriority;
		}
		set
		{
			this._facebookPriority = value;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x06002304 RID: 8964 RVA: 0x000AD744 File Offset: 0x000AB944
	// (set) Token: 0x06002305 RID: 8965 RVA: 0x000AD74C File Offset: 0x000AB94C
	public FacebookController.StoryPriority twitterPriority
	{
		get
		{
			return this._twiiterPriority;
		}
		set
		{
			this._twiiterPriority = value;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (set) Token: 0x06002306 RID: 8966 RVA: 0x000AD758 File Offset: 0x000AB958
	public FacebookController.StoryPriority faceBookPriority
	{
		set
		{
			this.facebookPriority = value;
			this.twitterPriority = value;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06002307 RID: 8967 RVA: 0x000AD768 File Offset: 0x000AB968
	// (set) Token: 0x06002308 RID: 8968 RVA: 0x000AD770 File Offset: 0x000AB970
	public string EventTitle { get; set; }

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x06002309 RID: 8969 RVA: 0x000AD77C File Offset: 0x000AB97C
	// (set) Token: 0x0600230A RID: 8970 RVA: 0x000AD784 File Offset: 0x000AB984
	public Func<string> twitterStatus { get; set; }

	// Token: 0x0600230B RID: 8971 RVA: 0x000AD790 File Offset: 0x000AB990
	private void ShareResults()
	{
		this.faceBookPriority = FacebookController.StoryPriority.Red;
		this.twitterPriority = FacebookController.StoryPriority.MultyWinLimit;
		this.twitterStatus = (() => "I've won a battle in @PixelGun3D! Join the fight now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u");
		this.EventTitle = "Won Batlle";
		if (TwitterController.TwitterSupported && TwitterController.IsLoggedIn && TwitterController.Instance.CanPostStatusUpdateWithPriority(this.twitterPriority))
		{
			TwitterController.Instance.PostStatusUpdate(this.twitterStatus(), this.twitterPriority);
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{
					"Post Twitter",
					this.EventTitle
				},
				{
					"Total Twitter",
					"Posts"
				}
			});
		}
		if (FacebookController.FacebookSupported && FB.IsLoggedIn && FacebookController.sharedController.CanPostStoryWithPriority(this.facebookPriority))
		{
			FacebookController.PostOpenGraphStory("win", "battle", this.facebookPriority, new Dictionary<string, string>
			{
				{
					"battle",
					ConnectSceneNGUIController.regim.ToString().ToLower()
				}
			});
			AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
			{
				{
					"Post Facebook",
					this.EventTitle
				},
				{
					"Total Facebook",
					"Posts"
				}
			});
		}
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000AD8E8 File Offset: 0x000ABAE8
	public void OnTablesShown()
	{
		if (!Defs.isDaterRegim)
		{
			if (!Defs.isHunger || this.expRewardValue > 0 || this.coinsRewardValue > 0)
			{
				if (Defs.isDuel)
				{
					this.oldRanksIsActive = true;
					this.HideOldRanks();
				}
				else
				{
					base.Invoke("HideOldRanks", 60f);
					this.oldRanksIsActive = true;
				}
				this.hideOldRanksButton.SetActive(!Defs.isDuel);
				if (Defs.isHunger)
				{
					this.backButtonInHunger.SetActive(false);
					this.randomBtn.SetActive(false);
					this.MapSelectPanel.SetActive(false);
					this.questsButton.SetActive(false);
					this.spectatorModeBtnPnl.SetActive(false);
				}
			}
			else
			{
				this.hideOldRanksButton.SetActive(false);
				this.MapSelectPanel.SetActive(true);
				this.questsButton.SetActive(true);
			}
		}
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000AD9D8 File Offset: 0x000ABBD8
	public void HideOldRanks()
	{
		if (this.oldRanksIsActive && (this.hideOldRanksButton.activeSelf || Defs.isDuel))
		{
			base.CancelInvoke("HideOldRanks");
			this.interfaceAnimator.SetTrigger("OkPressed");
			if (this.expRewardValue > 0 || this.coinsRewardValue > 0 || this.trophyRewardValue != 0)
			{
				this.interfaceAnimator.SetTrigger("GetReward");
			}
			this.hideOldRanksButton.SetActive(false);
		}
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000ADA64 File Offset: 0x000ABC64
	public void HandleHideOldRanksClick()
	{
		if (this.oldRanksIsActive)
		{
			if (this.shareToggle.value && (this.expRewardValue > 0 || this.coinsRewardValue > 0))
			{
				this.ShareResults();
			}
			this.HideOldRanks();
		}
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000ADAB0 File Offset: 0x000ABCB0
	public void FinishHideOldRanks()
	{
		this.shopButton.GetComponent<UIButton>().isEnabled = true;
		this.shopButton.GetComponent<Collider>().enabled = true;
		this.interfaceAnimator.SetTrigger("RewardTaken");
		if (this.oldRanksIsActive || Defs.isHunger)
		{
			this.trophyRewardValue = 0;
			this.oldRanksIsActive = false;
			this.questsButton.SetActive(TrainingController.TrainingCompleted);
			this.isRewardShow = false;
			if (Defs.isMulti)
			{
				this.MapSelectPanel.SetActive(true);
			}
			if (Defs.isDuel)
			{
				this.duelUI.ShowEndInterface();
			}
			else if (!Defs.isHunger)
			{
				this.finishedInterface.SetActive(false);
				this.HideEndInterface();
				this.ShowNewMatchInterface();
				WeaponManager.sharedManager.myNetworkStartTable.ResetOldScore();
			}
			else
			{
				this.backButtonInHunger.SetActive(true);
				this.randomBtn.SetActive(true);
				this.spectatorModeBtnPnl.SetActive(true);
			}
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().isShowAvard = false;
			}
			else
			{
				base.StartCoroutine(this.WaitAndRemoveInterfaceOnReconnect());
			}
		}
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000ADBF0 File Offset: 0x000ABDF0
	private IEnumerator WaitAndRemoveInterfaceOnReconnect()
	{
		yield return null;
		UnityEngine.Object.Destroy(NetworkStartTableNGUIController.sharedController.gameObject);
		yield break;
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000ADC04 File Offset: 0x000ABE04
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
				if (!ShopNGUIController.GuiActive && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && (!(ExpController.Instance != null) || !ExpController.Instance.WaitingForLevelUpView) && (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !this.waitForAnimationDone && !this.waitForTrophyAnimationDone)
				{
					if (this.rentScreenPoint.childCount == 0)
					{
						if (BuffSystem.instance != null && BuffSystem.instance.haveAllInteractons && Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1)
						{
							GameObject window = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/WeRemoveNoviceArmorBanner"));
							window.transform.parent = this.rentScreenPoint;
							Player_move_c.SetLayerRecursively(window, LayerMask.NameToLayer("NGUITable"));
							window.transform.localPosition = new Vector3(0f, 0f, -130f);
							window.transform.localRotation = Quaternion.identity;
							window.transform.localScale = new Vector3(1f, 1f, 1f);
							base.GetComponent<HintController>().HideHintByName("shop_remove_novice_armor");
							try
							{
								ShopNGUIController.EquipWearInCategoryIfNotEquiped("Armor_Army_1", ShopNGUIController.CategoryNames.ArmorCategory, false);
							}
							catch (Exception ex)
							{
								Exception e = ex;
								Debug.LogError("Exception in NetworkStartTableNguiController: ShopNGUIController.EquipWearInCategoryIfNotEquiped: " + e);
							}
						}
						else
						{
							ShopNGUIController.ShowTryGunIfPossible(this.startInterfacePanel.activeSelf, this.rentScreenPoint, "NGUITable", null, null, null, null);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Exception e2 = ex2;
				Debug.LogWarning("exception in NetworkTableNGUI  TryToShowExpiredBanner: " + e2);
			}
		}
		yield break;
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000ADC20 File Offset: 0x000ABE20
	public static bool IsStartInterfaceShown()
	{
		return NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.startInterfacePanel != null && NetworkStartTableNGUIController.sharedController.startInterfacePanel.activeSelf;
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000ADC5C File Offset: 0x000ABE5C
	public static bool IsEndInterfaceShown()
	{
		return NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.endInterfacePanel != null && NetworkStartTableNGUIController.sharedController.endInterfacePanel.activeSelf;
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000ADC98 File Offset: 0x000ABE98
	public void HideEndInterface()
	{
		Debug.Log("HideEndInterface");
		this.socialPnl.SetActive(false);
		this.allInterfacePanel.SetActive(false);
		this.endInterfacePanel.SetActive(false);
		this.winnerPanelCom1.SetActive(false);
		this.winnerPanelCom2.SetActive(false);
		if (this.defaultTeamOneState == Vector3.zero)
		{
			this.defaultTeamOneState = this.teamOneLabel.transform.localPosition;
		}
		if (this.defaultTeamTwoState == Vector3.zero)
		{
			this.defaultTeamTwoState = this.teamTwoLabel.transform.localPosition;
		}
		this.teamOneLabel.transform.localPosition = this.defaultTeamOneState;
		this.teamTwoLabel.transform.localPosition = this.defaultTeamTwoState;
		this.HideTable();
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		this.updateRealTableAfterActionPanel = true;
		base.StopCoroutine("TryToShowExpiredBanner");
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000ADDA0 File Offset: 0x000ABFA0
	private void ShowTable(bool _isRealUpdate = true)
	{
		this.ranksTable.isShowRanks = _isRealUpdate;
		this.ranksTable.tekPanel.SetActive(true);
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000ADDC0 File Offset: 0x000ABFC0
	public void HideTable()
	{
		this.ranksTable.isShowRanks = false;
		this.ranksTable.tekPanel.SetActive(false);
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000ADDE0 File Offset: 0x000ABFE0
	public void ShowRanksTable()
	{
		this.ShowTable(true);
		this.ranksInterface.SetActive(true);
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000ADDF8 File Offset: 0x000ABFF8
	public void HideRanksTable(bool isHideTable = true)
	{
		if (isHideTable)
		{
			this.HideTable();
		}
		this.ranksInterface.SetActive(false);
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000ADE14 File Offset: 0x000AC014
	public void BackPressFromRanksTable(bool isHideTable = true)
	{
		if (this.CheckHideInternalPanel())
		{
			return;
		}
		this.HideRanksTable(isHideTable);
		this.ReasonsPanel.SetActive(false);
		this.ActionPanel.SetActive(false);
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.BackRanksPressed();
		}
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000ADE70 File Offset: 0x000AC070
	public void UpdateGoMapButtonsInDuel(bool show = true)
	{
		bool flag = !show || ConnectSceneNGUIController.gameTier != ExpController.Instance.OurTier;
		for (int i = 0; i < this.goMapInEndGameButtonsDuel.Length; i++)
		{
			this.goMapInEndGameButtonsDuel[i].gameObject.SetActive(!flag);
		}
		this.changeMapLabel.SetActive(!flag);
		if (flag)
		{
			return;
		}
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		SceneInfo[] array = new SceneInfo[this.goMapInEndGameButtonsDuel.Length];
		this.goMapInEndGameButtonsDuel[0].SetMap(null);
		for (int j = 1; j < array.Length; j++)
		{
			int num = 0;
			bool flag2 = true;
			int num2 = UnityEngine.Random.Range(0, listScenesForMode.avaliableScenes.Count);
			while (flag2)
			{
				flag2 = false;
				SceneInfo sceneInfo = listScenesForMode.avaliableScenes[num2];
				for (int k = 0; k < j; k++)
				{
					if (array[k] == sceneInfo)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && (sceneInfo.NameScene.Equals(Application.loadedLevelName) || sceneInfo.AvaliableWeapon == ModeWeapon.dater || (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene))))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					array[j] = sceneInfo;
				}
				else
				{
					num2++;
					num++;
					if (num2 > listScenesForMode.avaliableScenes.Count - 1)
					{
						num2 = 0;
					}
				}
				if (num > listScenesForMode.avaliableScenes.Count)
				{
					Debug.LogWarning("no map");
					break;
				}
			}
			if (array[j] != null)
			{
				this.goMapInEndGameButtonsDuel[j].SetMap(array[j]);
			}
			else
			{
				this.goMapInEndGameButtonsDuel[j].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000AE080 File Offset: 0x000AC280
	public void UpdateGoMapButtons(bool show = true)
	{
		if (Defs.isDuel)
		{
			this.UpdateGoMapButtonsInDuel(show);
			return;
		}
		bool flag = !show || ConnectSceneNGUIController.gameTier != ExpController.Instance.OurTier;
		for (int i = 0; i < this.goMapInEndGameButtons.Length; i++)
		{
			this.goMapInEndGameButtons[i].gameObject.SetActive(!flag);
		}
		this.changeMapLabel.SetActive(!flag);
		if (flag)
		{
			return;
		}
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		SceneInfo[] array = new SceneInfo[this.goMapInEndGameButtons.Length];
		this.goMapInEndGameButtons[0].SetMap(null);
		for (int j = 1; j < array.Length; j++)
		{
			int num = 0;
			bool flag2 = true;
			int num2 = UnityEngine.Random.Range(0, listScenesForMode.avaliableScenes.Count);
			while (flag2)
			{
				flag2 = false;
				SceneInfo sceneInfo = listScenesForMode.avaliableScenes[num2];
				for (int k = 0; k < j; k++)
				{
					if (array[k] == sceneInfo)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && (sceneInfo.NameScene.Equals(Application.loadedLevelName) || sceneInfo.AvaliableWeapon == ModeWeapon.dater || (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene))))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					array[j] = sceneInfo;
				}
				else
				{
					num2++;
					num++;
					if (num2 > listScenesForMode.avaliableScenes.Count - 1)
					{
						num2 = 0;
					}
				}
				if (num > listScenesForMode.avaliableScenes.Count)
				{
					Debug.LogWarning("no map");
					break;
				}
			}
			this.goMapInEndGameButtons[j].SetMap(array[j]);
		}
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000AE27C File Offset: 0x000AC47C
	public void OnRewardAnimationEnds()
	{
		this.interfaceAnimator.SetTrigger("AnimationEnds");
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000AE290 File Offset: 0x000AC490
	public void OnTrophyOkButtonPress()
	{
		base.CancelInvoke("OnTrophyOkButtonPress");
		this.waitForTrophyAnimationDone = false;
		this.interfaceAnimator.SetBool("isTrophyAdded", true);
	}

	// Token: 0x040016F4 RID: 5876
	public static NetworkStartTableNGUIController sharedController;

	// Token: 0x040016F5 RID: 5877
	public GameObject facebookButton;

	// Token: 0x040016F6 RID: 5878
	public GameObject twitterButton;

	// Token: 0x040016F7 RID: 5879
	public Transform rentScreenPoint;

	// Token: 0x040016F8 RID: 5880
	public GameObject ranksInterface;

	// Token: 0x040016F9 RID: 5881
	public RanksTable ranksTable;

	// Token: 0x040016FA RID: 5882
	public GameObject shopAnchor;

	// Token: 0x040016FB RID: 5883
	public GameObject finishedInterface;

	// Token: 0x040016FC RID: 5884
	public UILabel[] finishedInterfaceLabels;

	// Token: 0x040016FD RID: 5885
	public GameObject startInterfacePanel;

	// Token: 0x040016FE RID: 5886
	public GameObject winnerPanelCom1;

	// Token: 0x040016FF RID: 5887
	public GameObject winnerPanelCom2;

	// Token: 0x04001700 RID: 5888
	public GameObject endInterfacePanel;

	// Token: 0x04001701 RID: 5889
	public Animator interfaceAnimator;

	// Token: 0x04001702 RID: 5890
	public GameObject allInterfacePanel;

	// Token: 0x04001703 RID: 5891
	public GameObject randomBtn;

	// Token: 0x04001704 RID: 5892
	public GameObject socialPnl;

	// Token: 0x04001705 RID: 5893
	public GameObject spectratorModePnl;

	// Token: 0x04001706 RID: 5894
	public GameObject spectatorModeBtnPnl;

	// Token: 0x04001707 RID: 5895
	public GameObject spectatorModeOnBtn;

	// Token: 0x04001708 RID: 5896
	public GameObject spectatorModeOffBtn;

	// Token: 0x04001709 RID: 5897
	public GameObject MapSelectPanel;

	// Token: 0x0400170A RID: 5898
	public string winner;

	// Token: 0x0400170B RID: 5899
	public int winnerCommand;

	// Token: 0x0400170C RID: 5900
	public UILabel HungerStartLabel;

	// Token: 0x0400170D RID: 5901
	private int addCoins;

	// Token: 0x0400170E RID: 5902
	private int addExperience;

	// Token: 0x0400170F RID: 5903
	private bool isCancelHideAvardPanel;

	// Token: 0x04001710 RID: 5904
	private bool updateRealTableAfterActionPanel = true;

	// Token: 0x04001711 RID: 5905
	public GameObject SexualButton;

	// Token: 0x04001712 RID: 5906
	public GameObject InAppropriateActButton;

	// Token: 0x04001713 RID: 5907
	public GameObject OtherButton;

	// Token: 0x04001714 RID: 5908
	public GameObject ReasonsPanel;

	// Token: 0x04001715 RID: 5909
	public GameObject ActionPanel;

	// Token: 0x04001716 RID: 5910
	public GameObject AddButton;

	// Token: 0x04001717 RID: 5911
	public GameObject ReportButton;

	// Token: 0x04001718 RID: 5912
	public GameObject questsButton;

	// Token: 0x04001719 RID: 5913
	public GameObject hideOldRanksButton;

	// Token: 0x0400171A RID: 5914
	public GameObject rewardButton;

	// Token: 0x0400171B RID: 5915
	public GameObject shopButton;

	// Token: 0x0400171C RID: 5916
	public GameObject labelNewItems;

	// Token: 0x0400171D RID: 5917
	public UILabel[] actionPanelNicklabel;

	// Token: 0x0400171E RID: 5918
	public GameObject trophiAddIcon;

	// Token: 0x0400171F RID: 5919
	public GameObject trophiMinusIcon;

	// Token: 0x04001720 RID: 5920
	public string pixelbookID;

	// Token: 0x04001721 RID: 5921
	public string nick;

	// Token: 0x04001722 RID: 5922
	public GoMapInEndGame[] goMapInEndGameButtons = new GoMapInEndGame[3];

	// Token: 0x04001723 RID: 5923
	public GoMapInEndGame[] goMapInEndGameButtonsDuel = new GoMapInEndGame[2];

	// Token: 0x04001724 RID: 5924
	public int CountAddFriens;

	// Token: 0x04001725 RID: 5925
	public UILabel[] totalBlue;

	// Token: 0x04001726 RID: 5926
	public UILabel[] totalRed;

	// Token: 0x04001727 RID: 5927
	private GameObject cameraObj;

	// Token: 0x04001728 RID: 5928
	public GameObject changeMapLabel;

	// Token: 0x04001729 RID: 5929
	public GameObject rewardPanel;

	// Token: 0x0400172A RID: 5930
	public GameObject listOfPlayers;

	// Token: 0x0400172B RID: 5931
	public GameObject backButtonInHunger;

	// Token: 0x0400172C RID: 5932
	public GameObject goBattleLabel;

	// Token: 0x0400172D RID: 5933
	public GameObject daterButtonLabel;

	// Token: 0x0400172E RID: 5934
	public UITexture rewardCoinsObject;

	// Token: 0x0400172F RID: 5935
	public UITexture rewardExpObject;

	// Token: 0x04001730 RID: 5936
	public UISprite rewardTrophysObject;

	// Token: 0x04001731 RID: 5937
	public UITexture[] trophyItems;

	// Token: 0x04001732 RID: 5938
	public UISprite currentCup;

	// Token: 0x04001733 RID: 5939
	public UISprite NewCup;

	// Token: 0x04001734 RID: 5940
	public GameObject trophyPanel;

	// Token: 0x04001735 RID: 5941
	public GameObject trophyShine;

	// Token: 0x04001736 RID: 5942
	public UISprite currentBar;

	// Token: 0x04001737 RID: 5943
	public UISprite nextBar;

	// Token: 0x04001738 RID: 5944
	public UILabel trophyPoints;

	// Token: 0x04001739 RID: 5945
	public Transform rewardCoinsAnimPoint;

	// Token: 0x0400173A RID: 5946
	public Transform rewardExpAnimPoint;

	// Token: 0x0400173B RID: 5947
	public UILabel[] rewardCoins;

	// Token: 0x0400173C RID: 5948
	public UILabel[] rewardExperience;

	// Token: 0x0400173D RID: 5949
	public UILabel[] gameModeLabel;

	// Token: 0x0400173E RID: 5950
	public UILabel[] rewardTrophy;

	// Token: 0x0400173F RID: 5951
	public GameObject[] finishWin;

	// Token: 0x04001740 RID: 5952
	public GameObject[] finishDefeat;

	// Token: 0x04001741 RID: 5953
	public GameObject[] finishDraw;

	// Token: 0x04001742 RID: 5954
	public UILabel teamOneLabel;

	// Token: 0x04001743 RID: 5955
	public UILabel teamTwoLabel;

	// Token: 0x04001744 RID: 5956
	private Vector3 defaultTeamOneState;

	// Token: 0x04001745 RID: 5957
	private Vector3 defaultTeamTwoState;

	// Token: 0x04001746 RID: 5958
	public UIToggle shareToggle;

	// Token: 0x04001747 RID: 5959
	public UILabel[] textLeagueUp;

	// Token: 0x04001748 RID: 5960
	public UILabel[] textLeagueDown;

	// Token: 0x04001749 RID: 5961
	public GameObject duelPanel;

	// Token: 0x0400174A RID: 5962
	[HideInInspector]
	public DuelUIController duelUI;

	// Token: 0x0400174B RID: 5963
	public FrameResizer rewardFrame;

	// Token: 0x0400174C RID: 5964
	public bool isRewardShow;

	// Token: 0x0400174D RID: 5965
	public RuntimeAnimatorController standartAnimController;

	// Token: 0x0400174E RID: 5966
	public RuntimeAnimatorController DuelAnimController;

	// Token: 0x0400174F RID: 5967
	private readonly Lazy<string> _versionString = new Lazy<string>(() => typeof(NetworkStartTableNGUIController).Assembly.GetName().Version.ToString());

	// Token: 0x04001750 RID: 5968
	private IDisposable _backSubscription;

	// Token: 0x04001751 RID: 5969
	private bool waitForAnimationDone;

	// Token: 0x04001752 RID: 5970
	private bool leagueUp;

	// Token: 0x04001753 RID: 5971
	private int expRewardValue;

	// Token: 0x04001754 RID: 5972
	private int coinsRewardValue;

	// Token: 0x04001755 RID: 5973
	private int trophyRewardValue;

	// Token: 0x04001756 RID: 5974
	private float currentBarFillAmount;

	// Token: 0x04001757 RID: 5975
	private float nextBarFillAmount;

	// Token: 0x04001758 RID: 5976
	private bool isUsed;

	// Token: 0x04001759 RID: 5977
	private bool waitForTrophyAnimationDone;

	// Token: 0x0400175A RID: 5978
	private bool oldRanksIsActive;

	// Token: 0x0400175B RID: 5979
	private FacebookController.StoryPriority _facebookPriority;

	// Token: 0x0400175C RID: 5980
	private FacebookController.StoryPriority _twiiterPriority;

	// Token: 0x0400175D RID: 5981
	public Action shareAction;

	// Token: 0x0400175E RID: 5982
	public Action customHide;
}
