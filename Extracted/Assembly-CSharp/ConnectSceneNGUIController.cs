using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using FyberPlugin;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000077 RID: 119
public class ConnectSceneNGUIController : MonoBehaviour
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000361 RID: 865 RVA: 0x0001C518 File Offset: 0x0001A718
	// (set) Token: 0x06000362 RID: 866 RVA: 0x0001C520 File Offset: 0x0001A720
	public static ConnectSceneNGUIController.RegimGame regim
	{
		get
		{
			return ConnectSceneNGUIController._regim;
		}
		set
		{
			ConnectSceneNGUIController._regim = value;
			ConnectSceneNGUIController.UpdateUseMasMaps();
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000363 RID: 867 RVA: 0x0001C530 File Offset: 0x0001A730
	public static bool isTeamRegim
	{
		get
		{
			return Defs.isMulti && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
		}
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0001C56C File Offset: 0x0001A76C
	public static string MainLoadingTexture()
	{
		return (!Device.isRetinaAndStrong) ? "main_loading" : "main_loading_Hi";
	}

	// Token: 0x06000365 RID: 869 RVA: 0x0001C588 File Offset: 0x0001A788
	public static void GoToClans()
	{
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x06000366 RID: 870 RVA: 0x0001C5B8 File Offset: 0x0001A7B8
	public static void GoToFriends()
	{
		FriendsController friendsController = FriendsController.sharedController;
		if (friendsController != null)
		{
			friendsController.GetFriendsData(false);
		}
		MainMenuController.friendsOnStart = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
		Defs.isDaterRegim = false;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x0001C60C File Offset: 0x0001A80C
	public static void Local()
	{
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		if (Defs.isGameFromFriends)
		{
			ConnectSceneNGUIController.GoToFriends();
		}
		else if (Defs.isGameFromClans)
		{
			ConnectSceneNGUIController.GoToClans();
		}
		else
		{
			LoadConnectScene.textureToShow = null;
			if (!Defs.isDaterRegim)
			{
				LoadConnectScene.sceneToLoad = "ConnectScene";
			}
			else
			{
				LoadConnectScene.sceneToLoad = "ConnectSceneSandbox";
			}
			LoadConnectScene.noteToShow = null;
			SceneManager.LoadScene(Defs.PromSceneName);
		}
	}

	// Token: 0x06000368 RID: 872 RVA: 0x0001C688 File Offset: 0x0001A888
	public static void GoToProfile()
	{
		PlayerPrefs.SetInt(Defs.SkinEditorMode, 1);
		GlobalGameController.EditingLogo = 0;
		GlobalGameController.EditingCape = 0;
		SceneManager.LoadScene("SkinEditor");
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001C6AC File Offset: 0x0001A8AC
	public void StopFingerAnim()
	{
		if (this.finger != null && this.finger.activeSelf)
		{
			this.fingerStopped = true;
			this.finger.SetActive(false);
			UIScrollView component = this.scrollViewSelectMapTransform.GetComponent<UIScrollView>();
			component.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Remove(component.onDragStarted, new UIScrollView.OnDragNotification(this.StopFingerAnim));
		}
	}

	// Token: 0x0600036A RID: 874 RVA: 0x0001C71C File Offset: 0x0001A91C
	private void OnEnableWhenAnimate()
	{
		if (this.animationStarted)
		{
			this.StopFingerAnim();
			this.modeAnimObj.SetActive(false);
			this.fingerStopped = false;
			base.StartCoroutine(this.AnimateModeOpen());
		}
	}

	// Token: 0x0600036B RID: 875 RVA: 0x0001C75C File Offset: 0x0001A95C
	private IEnumerator AnimateModeOpen()
	{
		this.modeAnimObj.GetComponent<AudioSource>().enabled = Defs.isSoundFX;
		this.animationStarted = true;
		if (!TrainingController.TrainingCompleted)
		{
			this.localBtn.GetComponent<UIButton>().isEnabled = false;
			this.randomBtn.GetComponent<UIButton>().isEnabled = false;
			this.customBtn.GetComponent<UIButton>().isEnabled = false;
			this.goBtn.GetComponent<UIButton>().isEnabled = false;
		}
		int storagedStageDuel = Storager.getInt("ModeUnlockDuel", false);
		int currentStateDuel = (RatingSystem.instance.currentRating < 1200) ? 0 : storagedStageDuel;
		this.modeDuelRatingNeedLabel.text = 1200.ToString();
		this.modeDuelRatingNeed.SetActive(RatingSystem.instance.currentRating < 1200);
		this.modeButtonDuel.isEnable = (currentStateDuel == 1);
		int storagedStage = Storager.getInt("ModeUnlockStage", false);
		if (Storager.getInt("TrainingCompleted_4_4_Sett", false) == 1)
		{
			storagedStage = this.modeButtonByLevel.Length;
		}
		int currentStage = Mathf.Clamp(storagedStage, 0, this.modeButtonByLevel.Length);
		for (int i = 0; i < this.modeButtonByLevel.Length; i++)
		{
			this.modeButtonByLevel[i].isEnable = (i < currentStage);
		}
		int currentLevel = (!(ExperienceController.sharedController != null)) ? 31 : ExperienceController.sharedController.currentLevel;
		if (currentLevel >= 4)
		{
			currentLevel = this.modeButtonByLevel.Length;
		}
		if (this.modeUnlockLabelByLevel != null)
		{
			for (int j = 0; j < this.modeUnlockLabelByLevel.Length; j++)
			{
				this.modeUnlockLabelByLevel[j].gameObject.SetActive(j > Mathf.Max(currentStage, currentLevel) - 2);
				this.modeUnlockLabelByLevel[j].text = string.Format(LocalizationStore.Get("Key_1923"), Mathf.Min(j + 2, 4));
			}
		}
		if (currentStage < Mathf.Min(currentLevel, this.modeButtonByLevel.Length))
		{
			BannerWindowController.SharedController.AddBannersTimeout(20.1f);
		}
		if (currentStage == 0 && !TrainingController.TrainingCompleted)
		{
			UIScrollView component = this.scrollViewSelectMapTransform.GetComponent<UIScrollView>();
			component.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(component.onDragStarted, new UIScrollView.OnDragNotification(this.StopFingerAnim));
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Connect_Scene, 0);
		}
		yield return new WaitForSeconds(0.5f);
		while (currentStage < Mathf.Min(currentLevel, this.modeButtonByLevel.Length))
		{
			BannerWindowController.SharedController.AddBannersTimeout(20.1f);
			if (currentStage == 1)
			{
				BannerWindowController.firstScreen = true;
				BannerWindowController.SharedController.ClearBannerStates();
			}
			BtnCategory currentMode = this.modeButtonByLevel[currentStage];
			if (currentStage != 0)
			{
				this.modeAnimObj.transform.SetParent(this.categoryButtonsController.transform.parent);
				this.modeAnimObj.transform.position = currentMode.transform.position;
				this.modeAnimObj.transform.localScale = currentMode.transform.localScale;
				this.modeAnimObj.SetActive(true);
				yield return new WaitForSeconds(0.1f);
				this.modeButtonByLevel[currentStage].isEnable = true;
				yield return new WaitForSeconds(1.4f);
				this.modeAnimObj.SetActive(false);
			}
			if (currentStage == 0 && !TrainingController.TrainingCompleted)
			{
				yield return this.FingerAnimationCoroutine();
			}
			if (currentStage == 1)
			{
				HintController.instance.ShowHintByName("deathmatch", 0f);
				HintController.instance.ShowHintByName("gobattletimeout", 0f);
			}
			currentStage++;
			Storager.setInt("ModeUnlockStage", currentStage, false);
		}
		if (storagedStage != currentStage)
		{
			Storager.setInt("ModeUnlockStage", currentStage, false);
		}
		if (currentStateDuel == 0 && RatingSystem.instance.currentRating >= 1200)
		{
			this.modeAnimObj.transform.SetParent(this.categoryButtonsController.transform.parent);
			this.modeAnimObj.transform.position = this.modeButtonDuel.transform.position;
			this.modeAnimObj.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			this.modeButtonDuel.isEnable = true;
			yield return new WaitForSeconds(1.4f);
			this.modeAnimObj.SetActive(false);
			currentStateDuel = 1;
		}
		if (storagedStageDuel != currentStateDuel)
		{
			Storager.setInt("ModeUnlockDuel", currentStateDuel, false);
		}
		if (!TrainingController.TrainingCompleted)
		{
			this.goBtn.GetComponent<UIButton>().isEnabled = true;
			HintController.instance.ShowHintByName("gobattle", 0f);
		}
		this.animationStarted = false;
		yield break;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x0001C778 File Offset: 0x0001A978
	public void StopFingerAnimation()
	{
		this.finger.SetActive(false);
		this._stopFingerAnimation = true;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x0001C790 File Offset: 0x0001A990
	private IEnumerator FingerAnimationCoroutine()
	{
		this.finger.SetActive(true);
		string fromName = this.grid.transform.GetChild(1).GetComponent<MapPreviewController>().sceneMapName;
		string toName = this.grid.transform.GetChild(2).GetComponent<MapPreviewController>().sceneMapName;
		yield return this.finger.MoveOverTime(this.finger.transform.position, this.grid.transform.GetChild(1).transform.position, 1f, null);
		if (this._stopFingerAnimation)
		{
			yield break;
		}
		Animator fingerAnimator = this.finger.GetComponentInChildren<Animator>(true);
		fingerAnimator.SetTrigger("touch");
		yield return new WaitForSeconds(0.8f);
		if (this._stopFingerAnimation)
		{
			yield break;
		}
		this.SelectMap(fromName);
		yield return new WaitForSeconds(1f);
		if (this._stopFingerAnimation)
		{
			yield break;
		}
		yield return this.finger.MoveOverTime(this.grid.transform.GetChild(1).transform.position, this.grid.transform.GetChild(2).transform.position, 1f, null);
		fingerAnimator.SetTrigger("touch");
		yield return new WaitForSeconds(0.8f);
		if (this._stopFingerAnimation)
		{
			yield break;
		}
		this.SelectMap(toName);
		yield return new WaitForSeconds(1f);
		this.finger.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001C7AC File Offset: 0x0001A9AC
	private void Start()
	{
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendOurDataInConnectScene();
		}
		Defs.isGameFromFriends = false;
		this.gameInfoItemPrefab.SetActive(false);
		this.mapPreviewTexture.SetActive(false);
		this.startPosNameServerNameInput = this.nameServerInput.transform.localPosition;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(0);
		}
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.profileInfo.Clear();
		}
		Defs.isDaterRegim = SceneLoader.ActiveSceneName.Equals("ConnectSceneSandbox");
		GlobalGameController.CountKills = 0;
		GlobalGameController.Score = 0;
		WeaponManager.RefreshExpControllers();
		this.rulesDeadmatch = LocalizationStore.Key_0550;
		this.rulesTeamFight = LocalizationStore.Key_0551;
		this.rulesTimeBattle = LocalizationStore.Key_0552;
		this.rulesDeadlyGames = LocalizationStore.Key_0553;
		this.rulesFlagCapture = LocalizationStore.Key_0554;
		this.rulesCapturePoint = LocalizationStore.Get("Key_1368");
		this.rulesDater = LocalizationStore.Get("Key_1538");
		this.rulesDuel = LocalizationStore.Get("Key_2406");
		ConnectSceneNGUIController.sharedController = this;
		this.myLevelGame = ((!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 2) ? ((!(ExperienceController.sharedController != null) || ExperienceController.sharedController.currentLevel > 5) ? 2 : 1) : 0);
		this.mainPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.createPanel.SetActive(false);
		this.customPanel.SetActive(false);
		this.searchPanel.SetActive(false);
		this.setPasswordPanel.SetActive(false);
		this.enterPasswordPanel.SetActive(false);
		this.StartSearchLocalServers();
		PlayerPrefs.SetString("TypeGame", "client");
		this.gameIsfullLabel.gameObject.SetActive(false);
		this.accountBlockedLabel.gameObject.SetActive(false);
		this.serverIsNotAvalible.gameObject.SetActive(false);
		this.nameAlreadyUsedLabel.gameObject.SetActive(false);
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		this.unlockMapBtn.SetActive(false);
		this.unlockMapBtnInCreate.SetActive(false);
		this.unlockBtn.SetActive(false);
		string path = ConnectSceneNGUIController.MainLoadingTexture();
		this.loadingToDraw.mainTexture = Resources.Load<Texture>(path);
		this.loadingMapPanel.SetActive(true);
		this.connectToPhotonPanel.SetActive(false);
		if (PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			this.firstConnectToPhoton = true;
		}
		this.SetPosSelectMapPanelInMainMenu();
		ConnectSceneNGUIController.regim = (ConnectSceneNGUIController.RegimGame)(TrainingController.TrainingCompleted ? ((!Defs.isDaterRegim) ? PlayerPrefs.GetInt("RegimMulty", 2) : 0) : 2);
		ConnectSceneNGUIController.directedFromQuests = false;
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel && RatingSystem.instance.currentRating < 1200)
		{
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
		}
		SceneInfo sceneInfo = (!(SceneInfoController.instance != null)) ? null : SceneInfoController.instance.GetInfoScene(ConnectSceneNGUIController.selectedMap);
		if (sceneInfo != null)
		{
			if (sceneInfo.IsAvaliableForMode(TypeModeGame.TeamFight))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
			}
			else if (sceneInfo.IsAvaliableForMode(TypeModeGame.Deathmatch))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
			}
			else if (sceneInfo.IsAvaliableForMode(TypeModeGame.FlagCapture))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
			}
			else if (sceneInfo.IsAvaliableForMode(TypeModeGame.CapturePoints))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
			}
			else if (sceneInfo.IsAvaliableForMode(TypeModeGame.DeadlyGames))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
			}
			else if (sceneInfo.IsAvaliableForMode(TypeModeGame.TimeBattle))
			{
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
			}
		}
		if (!Defs.isDaterRegim)
		{
			this.teamFightToogle.wasPressed = false;
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
			{
				this.categoryButtonsController.BtnClicked(this.deathmatchToggle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
			{
				this.categoryButtonsController.BtnClicked(this.timeBattleToogle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
			{
				this.categoryButtonsController.BtnClicked(this.deadlyGamesToogle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
			{
				this.categoryButtonsController.BtnClicked(this.flagCaptureToogle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel)
			{
				this.categoryButtonsController.BtnClicked(this.duelToggle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
			{
				this.categoryButtonsController.BtnClicked(this.teamFightToogle.btnName, false);
			}
			if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				this.categoryButtonsController.BtnClicked(this.capturePointsToogle.btnName, false);
			}
			this.deathmatchToggle.Clicked += this.SetRegimDeathmatch;
			this.timeBattleToogle.Clicked += this.SetRegimTimeBattle;
			this.teamFightToogle.Clicked += this.SetRegimTeamFight;
			this.deadlyGamesToogle.Clicked += this.SetRegimDeadleGames;
			this.flagCaptureToogle.Clicked += this.SetRegimFlagCapture;
			this.capturePointsToogle.Clicked += this.SetRegimCapturePoints;
			this.duelToggle.Clicked += this.SetRegimDuel;
		}
		base.StartCoroutine(this.LoadMapPreview());
		if (this.localBtn != null)
		{
			ButtonHandler component = this.localBtn.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleLocalBtnClicked;
			}
		}
		if (this.customBtn != null)
		{
			ButtonHandler component2 = this.customBtn.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleCustomBtnClicked;
			}
		}
		if (this.randomBtn != null)
		{
			ButtonHandler component3 = this.randomBtn.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleRandomBtnClicked;
			}
		}
		if (this.goBtn != null)
		{
			ButtonHandler component4 = this.goBtn.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += this.HandleGoBtnClicked;
			}
		}
		if (this.backBtn != null)
		{
			ButtonHandler component5 = this.backBtn.GetComponent<ButtonHandler>();
			if (component5 != null)
			{
				component5.Clicked += this.HandleBackBtnClicked;
			}
		}
		if (this.unlockBtn != null)
		{
			ButtonHandler component6 = this.unlockBtn.GetComponent<ButtonHandler>();
			if (component6 != null)
			{
				component6.Clicked += this.HandleUnlockBtnClicked;
			}
		}
		if (this.unlockMapBtn != null)
		{
			ButtonHandler component7 = this.unlockMapBtn.GetComponent<ButtonHandler>();
			if (component7 != null)
			{
				component7.Clicked += this.HandleUnlockMapBtnClicked;
			}
		}
		if (this.unlockMapBtnInCreate != null)
		{
			ButtonHandler component8 = this.unlockMapBtnInCreate.GetComponent<ButtonHandler>();
			if (component8 != null)
			{
				component8.Clicked += this.HandleUnlockMapBtnClicked;
			}
		}
		if (this.cancelFromConnectToPhotonBtn != null)
		{
			ButtonHandler component9 = this.cancelFromConnectToPhotonBtn.GetComponent<ButtonHandler>();
			if (component9 != null)
			{
				component9.Clicked += this.HandleCancelFromConnectToPhotonBtnClicked;
			}
		}
		if (this.clearBtn != null)
		{
			ButtonHandler component10 = this.clearBtn.GetComponent<ButtonHandler>();
			if (component10 != null)
			{
				component10.Clicked += this.HandleClearBtnClicked;
			}
		}
		if (this.searchBtn != null)
		{
			ButtonHandler component11 = this.searchBtn.GetComponent<ButtonHandler>();
			if (component11 != null)
			{
				component11.Clicked += this.HandleSearchBtnClicked;
			}
		}
		if (this.showSearchPanelBtn != null)
		{
			ButtonHandler component12 = this.showSearchPanelBtn.GetComponent<ButtonHandler>();
			if (component12 != null)
			{
				component12.Clicked += this.HandleShowSearchPanelBtnClicked;
			}
		}
		if (this.goToCreateRoomBtn != null)
		{
			ButtonHandler component13 = this.goToCreateRoomBtn.GetComponent<ButtonHandler>();
			if (component13 != null)
			{
				component13.Clicked += this.HandleGoToCreateRoomBtnClicked;
			}
		}
		if (this.createRoomBtn != null)
		{
			this.createRoomUIBtn = this.createRoomBtn.GetComponent<UIButton>();
			ButtonHandler component14 = this.createRoomBtn.GetComponent<ButtonHandler>();
			if (component14 != null)
			{
				component14.Clicked += this.HandleCreateRoomBtnClicked;
			}
		}
		if (this.clearInSetPasswordBtn != null)
		{
			ButtonHandler component15 = this.clearInSetPasswordBtn.GetComponent<ButtonHandler>();
			if (component15 != null)
			{
				component15.Clicked += this.HandleClearInSetPasswordBtnClicked;
			}
		}
		if (this.okInsetPasswordBtn != null)
		{
			ButtonHandler component16 = this.okInsetPasswordBtn.GetComponent<ButtonHandler>();
			if (component16 != null)
			{
				component16.Clicked += delegate(object sender, EventArgs e)
				{
					this.OnPaswordSelected();
				};
			}
		}
		if (this.joinRoomFromEnterPasswordBtn != null)
		{
			ButtonHandler component17 = this.joinRoomFromEnterPasswordBtn.GetComponent<ButtonHandler>();
			if (component17 != null)
			{
				component17.Clicked += this.HandleJoinRoomFromEnterPasswordBtnClicked;
			}
		}
		this.InitializeBannerWindow();
		InterstitialManager.Instance.ResetAdProvider();
		string text = string.Empty;
		if (ConnectSceneNGUIController.NeedShowReviewInConnectScene)
		{
			text = "NeedShowReviewInConnectScene";
		}
		else if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			string text2 = (!ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest) ? "Fake interstitial request not performed." : ConnectSceneNGUIController.GetReasonToDismissFakeInterstitial();
			if (string.IsNullOrEmpty(text2))
			{
				ConnectSceneNGUIController.ReplaceAdmobWithPerelivRequest = false;
				base.StartCoroutine(this.WaitLoadingAndShowReplaceAdmobPereliv("Connect Scene", false));
				text = "ReplaceAdmobWithPereliv";
			}
			else
			{
				string format = (!Application.isEditor) ? "Dismissing fake interstitial. {0}" : "<color=magenta>Dismissing fake interstitial. {0}</color>";
				Debug.LogFormat(format, new object[]
				{
					text2
				});
				if (!ConnectSceneNGUIController.InterstitialRequest)
				{
					text = "InterstitialRequest == false";
				}
				else
				{
					text = ConnectSceneNGUIController.GetReasonToDismissInterstitialConnectScene();
					if (string.IsNullOrEmpty(text))
					{
						this.isStartShowAdvert = true;
						base.StartCoroutine(this.WaitLoadingAndShowInterstitialCoroutine("Connect Scene", false));
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			string format2 = (!Application.isEditor) ? "Dismissing interstitial. {0}" : "<color=magenta>Dismissing interstitial. {0}</color>";
			Debug.LogFormat(format2, new object[]
			{
				text
			});
		}
		this.enterPasswordInput.onSubmit.Add(new EventDelegate(new EventDelegate.Callback(this.EnterPassInputSubmit)));
	}

	// Token: 0x0600036F RID: 879 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
	private IEnumerator OnApplicationPause(bool pausing)
	{
		if (pausing)
		{
			this.lanScan.StopBroadCasting();
		}
		else
		{
			yield return new WaitForSeconds(1f);
			this.StartSearchLocalServers();
			InterstitialManager.Instance.ResetAdProvider();
			if (MobileAdManager.Instance.SuppressShowOnReturnFromPause)
			{
				MobileAdManager.Instance.SuppressShowOnReturnFromPause = false;
			}
			else
			{
				string reasonToDismissFakeInterstitial = ConnectSceneNGUIController.GetReasonToDismissFakeInterstitial();
				if (string.IsNullOrEmpty(reasonToDismissFakeInterstitial))
				{
					ReplaceAdmobPerelivController.IncreaseTimesCounter();
					if (!this.loadAdmobRunning)
					{
						base.StartCoroutine(this.WaitLoadingAndShowReplaceAdmobPereliv("On return from pause to Connect Scene", true));
					}
				}
				else
				{
					string format = (!Application.isEditor) ? "Dismissing fake interstitial. {0}" : "<color=magenta>Dismissing fake interstitial. {0}</color>";
					Debug.LogFormat(format, new object[]
					{
						reasonToDismissFakeInterstitial
					});
				}
			}
		}
		yield break;
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
	private IEnumerator WaitLoadingAndShowReplaceAdmobPereliv(string context, bool loadData = true)
	{
		if (!this.loadReplaceAdmobPerelivRunning)
		{
			try
			{
				this.loadReplaceAdmobPerelivRunning = true;
				if (loadData && !ReplaceAdmobPerelivController.sharedController.DataLoading && !ReplaceAdmobPerelivController.sharedController.DataLoaded)
				{
					ReplaceAdmobPerelivController.sharedController.LoadPerelivData();
				}
				while (ReplaceAdmobPerelivController.sharedController == null || !ReplaceAdmobPerelivController.sharedController.DataLoaded)
				{
					if (!ReplaceAdmobPerelivController.sharedController.DataLoading)
					{
						this.loadReplaceAdmobPerelivRunning = false;
						yield break;
					}
					yield return null;
				}
				if (this.mainPanel != null)
				{
					while (!this.mainPanel.activeInHierarchy)
					{
						yield return null;
					}
					yield return new WaitForSeconds(0.5f);
				}
				ReplaceAdmobPerelivController.TryShowPereliv(context);
				ReplaceAdmobPerelivController.sharedController.DestroyImage();
			}
			finally
			{
				this.loadReplaceAdmobPerelivRunning = false;
			}
		}
		yield break;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x0001D308 File Offset: 0x0001B508
	private IEnumerator WaitLoadingAndShowInterstitialCoroutine(string context, bool loadData = true)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Starting WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
		}
		if (this.loadAdmobRunning)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Quitting WaitLoadingAndShowInterstitialCoroutine() because loadAdmobRunning==true");
			}
			yield break;
		}
		this.loadAdmobRunning = true;
		try
		{
			float loadAttemptTime = Time.realtimeSinceStartup;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("FyberFacade.Instance.Requests.Count: " + FyberFacade.Instance.Requests.Count);
			}
			if (FyberFacade.Instance.Requests.Count == 0)
			{
				Task<Ad> r2 = FyberFacade.Instance.RequestImageInterstitial("WaitLoadingAndShowInterstitialCoroutine(), requests count: 0");
				FyberFacade.Instance.Requests.AddLast(r2);
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Waiting either at least one loading request completed successfully, or all failed...");
			}
			for (;;)
			{
				if (FyberFacade.Instance.Requests.Any((Task<Ad> r) => r.IsCompleted && !r.IsFaulted))
				{
					break;
				}
				if (FyberFacade.Instance.Requests.All((Task<Ad> r) => r.IsCompleted))
				{
					goto Block_13;
				}
				if (Time.realtimeSinceStartup - loadAttemptTime > 5.2f)
				{
					goto Block_15;
				}
				yield return null;
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Found successfully completed request among {0}", new object[]
				{
					FyberFacade.Instance.Requests.Count
				});
			}
			goto IL_224;
			Block_13:
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("All requests are completed.");
			}
			goto IL_224;
			Block_15:
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Loading timed out.");
			}
			IL_224:
			List<Task<Ad>> completedRequests = (from r in FyberFacade.Instance.Requests
			where r.IsCompleted
			select r).ToList<Task<Ad>>();
			List<Task<Ad>> noOffersRequests = (from r in completedRequests
			where r.IsFaulted && r.Exception.InnerException is AdNotAwailableException
			select r).ToList<Task<Ad>>();
			if (noOffersRequests.Count > 0)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("Removing not filled requests: " + noOffersRequests.Count);
				}
				foreach (Task<Ad> noOffersRequest in noOffersRequests)
				{
					FyberFacade.Instance.Requests.Remove(noOffersRequest);
					completedRequests = null;
				}
			}
			if (completedRequests == null)
			{
				completedRequests = (from r in FyberFacade.Instance.Requests
				where r.IsCompleted
				select r).ToList<Task<Ad>>();
			}
			List<Task<Ad>> errorRequests = (from r in completedRequests
			where r.IsFaulted && r.Exception.InnerException is AdRequestException
			select r).ToList<Task<Ad>>();
			if (errorRequests.Count > 0)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("Removing failed requests: " + errorRequests.Count);
				}
				foreach (Task<Ad> errorRequest in errorRequests)
				{
					FyberFacade.Instance.Requests.Remove(errorRequest);
					completedRequests = null;
				}
			}
			if (this.mainPanel != null)
			{
				while (!this.mainPanel.activeInHierarchy)
				{
					yield return null;
				}
				yield return new WaitForSeconds(0.5f);
			}
			if (!PhotonNetwork.inRoom)
			{
				Dictionary<string, string> attributes = new Dictionary<string, string>
				{
					{
						"af_content_type",
						"Interstitial"
					},
					{
						"af_content_id",
						"Interstitial (ConnectScene)"
					}
				};
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", attributes);
				MenuBackgroundMusic.sharedMusic.Stop();
				Task<AdResult> showTask = FyberFacade.Instance.ShowInterstitial(new Dictionary<string, string>
				{
					{
						"Context",
						"Connect Scene"
					}
				}, "WaitLoadingAndShowInterstitialCoroutine()");
				InterstitialCounter.Instance.IncrementRealInterstitialCount();
				Storager.setInt("PendingInterstitial", 8, false);
				showTask.ContinueWith(delegate(Task<AdResult> t)
				{
					MenuBackgroundMusic.sharedMusic.Start();
					Storager.setInt("PendingInterstitial", 0, false);
					this.isStartShowAdvert = false;
					if (t.IsFaulted)
					{
						Debug.LogWarningFormat("[Rilisoft] Showing interstitial failed: {0}", new object[]
						{
							t.Exception.InnerException.Message
						});
						return;
					}
				});
			}
			this._lastTimeInterstitialShown = Time.realtimeSinceStartup;
		}
		finally
		{
			this.loadAdmobRunning = false;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Finishing WaitLoadingAndShowInterstitialCoroutine()    " + InterstitialManager.Instance.Provider);
			}
		}
		yield break;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000372 RID: 882 RVA: 0x0001D324 File Offset: 0x0001B524
	// (set) Token: 0x06000373 RID: 883 RVA: 0x0001D32C File Offset: 0x0001B52C
	internal static bool InterstitialRequest { get; set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000374 RID: 884 RVA: 0x0001D334 File Offset: 0x0001B534
	// (set) Token: 0x06000375 RID: 885 RVA: 0x0001D33C File Offset: 0x0001B53C
	internal static bool ReplaceAdmobWithPerelivRequest { get; set; }

	// Token: 0x06000376 RID: 886 RVA: 0x0001D344 File Offset: 0x0001B544
	private void InitializeBannerWindow()
	{
		this._advertisementController = base.gameObject.GetComponent<AdvertisementController>();
		if (this._advertisementController == null)
		{
			this._advertisementController = base.gameObject.AddComponent<AdvertisementController>();
		}
		BannerWindowController.SharedController.advertiseController = this._advertisementController;
	}

	// Token: 0x06000377 RID: 887 RVA: 0x0001D394 File Offset: 0x0001B594
	private void SetUnLockedButton(UIToggle butToogle)
	{
		UIButton component = butToogle.gameObject.GetComponent<UIButton>();
		component.normalSprite = "yell_btn";
		component.hoverSprite = "yell_btn";
		component.pressedSprite = "green_btn_n";
		butToogle.transform.FindChild("LockedSprite").gameObject.SetActive(false);
		butToogle.transform.FindChild("Checkmark").GetComponent<UISprite>().spriteName = "green_btn";
	}

	// Token: 0x06000378 RID: 888 RVA: 0x0001D408 File Offset: 0x0001B608
	private void SetRegimDeathmatch(object sender, EventArgs e)
	{
		HintController.instance.HideHintByName("deathmatch");
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.Deathmatch);
	}

	// Token: 0x06000379 RID: 889 RVA: 0x0001D42C File Offset: 0x0001B62C
	private void SetRegimTeamFight(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.TeamFight);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0001D444 File Offset: 0x0001B644
	private void SetRegimTimeBattle(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.TimeBattle);
	}

	// Token: 0x0600037B RID: 891 RVA: 0x0001D45C File Offset: 0x0001B65C
	private void SetRegimDeadleGames(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.DeadlyGames);
	}

	// Token: 0x0600037C RID: 892 RVA: 0x0001D474 File Offset: 0x0001B674
	private void SetRegimFlagCapture(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.FlagCapture);
	}

	// Token: 0x0600037D RID: 893 RVA: 0x0001D48C File Offset: 0x0001B68C
	private void SetRegimCapturePoints(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.CapturePoints);
	}

	// Token: 0x0600037E RID: 894 RVA: 0x0001D4A4 File Offset: 0x0001B6A4
	private void SetRegimDuel(object sender, EventArgs e)
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel)
		{
			return;
		}
		this.SetRegim(ConnectSceneNGUIController.RegimGame.Duel);
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0001D4BC File Offset: 0x0001B6BC
	private void HandleJoinRoomFromEnterPasswordBtnClicked(object sender, EventArgs e)
	{
		if (this.enterPasswordInput.value.Equals(this.joinRoomInfoFromCustom.customProperties[ConnectSceneNGUIController.passwordProperty].ToString()))
		{
			this.JoinToRoomPhotonAfterCheck();
		}
		else
		{
			this.enterPasswordPanel.SetActive(false);
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
			this.customPanel.SetActive(true);
			base.Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x0001D548 File Offset: 0x0001B748
	private void HandleSetPasswordBtnClicked(object sender, EventArgs e)
	{
		this.createPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.setPasswordInput.value = this.password;
		this.setPasswordPanel.SetActive(true);
	}

	// Token: 0x06000381 RID: 897 RVA: 0x0001D58C File Offset: 0x0001B78C
	private void HandleClearInSetPasswordBtnClicked(object sender, EventArgs e)
	{
		this.setPasswordInput.value = string.Empty;
	}

	// Token: 0x06000382 RID: 898 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
	private void OnPaswordSelected()
	{
		this.password = this.setPasswordInput.value;
		this.BackFromSetPasswordPanel();
	}

	// Token: 0x06000383 RID: 899 RVA: 0x0001D5BC File Offset: 0x0001B7BC
	private void BackFromSetPasswordPanel()
	{
		this.createPanel.SetActive(true);
		this.selectMapPanel.SetActive(true);
		this.setPasswordPanel.SetActive(false);
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0001D5F0 File Offset: 0x0001B7F0
	public static void CreateGameRoom(string roomName, int playerLimit, int mapIndex, int MaxKill, string _password, ConnectSceneNGUIController.RegimGame gameMode)
	{
		int num = 11;
		string[] array = new string[num];
		array[0] = ConnectSceneNGUIController.mapProperty;
		array[1] = ConnectSceneNGUIController.passwordProperty;
		array[2] = ConnectSceneNGUIController.platformProperty;
		array[3] = ConnectSceneNGUIController.endingProperty;
		array[4] = ConnectSceneNGUIController.maxKillProperty;
		array[5] = "TimeMatchEnd";
		array[6] = "tier";
		array[7] = ConnectSceneNGUIController.ABTestProperty;
		array[8] = ConnectSceneNGUIController.ABTestEnum;
		array[9] = "SpecialBonus";
		array[10] = ConnectSceneNGUIController.roomStatusProperty;
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[ConnectSceneNGUIController.mapProperty] = mapIndex;
		hashtable[ConnectSceneNGUIController.passwordProperty] = _password;
		hashtable[ConnectSceneNGUIController.platformProperty] = (int)((!string.IsNullOrEmpty(_password)) ? ConnectSceneNGUIController.PlatformConnect.custom : ConnectSceneNGUIController.myPlatformConnect);
		hashtable[ConnectSceneNGUIController.endingProperty] = 0;
		hashtable[ConnectSceneNGUIController.maxKillProperty] = MaxKill;
		hashtable["TimeMatchEnd"] = PhotonNetwork.time;
		hashtable["tier"] = ((!(ExpController.Instance != null)) ? 0 : ExpController.Instance.OurTier);
		if (ExpController.Instance.OurTier == 0)
		{
			hashtable[ConnectSceneNGUIController.ABTestProperty] = ((!Defs.isABTestBalansCohortActual) ? 0 : 1);
		}
		if (Defs.isActivABTestBuffSystem)
		{
			hashtable[ConnectSceneNGUIController.ABTestEnum] = ((!ABTestController.useBuffSystem) ? 1 : 3);
		}
		hashtable["SpecialBonus"] = 0;
		hashtable[ConnectSceneNGUIController.roomStatusProperty] = 0;
		ConnectSceneNGUIController.PhotonCreateRoom(roomName, true, true, (playerLimit <= 10) ? playerLimit : 10, hashtable, array);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0001D7AC File Offset: 0x0001B9AC
	public static void PhotonCreateRoom(string roomName, bool isVisible, bool isOpen, int maxPlayers, ExitGames.Client.Photon.Hashtable roomProps, string[] roomPropsInLobby)
	{
		PlayerPrefs.SetString("TypeGame", "server");
		RoomOptions roomOptions = new RoomOptions
		{
			customRoomProperties = roomProps,
			customRoomPropertiesForLobby = roomPropsInLobby
		};
		roomOptions.maxPlayers = (byte)maxPlayers;
		roomOptions.isOpen = isOpen;
		roomOptions.isVisible = isVisible;
		if (!Defs.useSqlLobby)
		{
			PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
		}
		else
		{
			TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
			PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby);
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x0001D828 File Offset: 0x0001BA28
	public static void JoinRandomGameRoom(int mapIndex, ConnectSceneNGUIController.RegimGame gameMode, int joinToNewRound, bool abTestSeparate = false)
	{
		string text = string.Empty;
		if (Defs.useSqlLobby)
		{
			if (mapIndex == -1)
			{
				TypeModeGame needMode = (TypeModeGame)((!Defs.isDaterRegim) ? ((int)Enum.Parse(typeof(TypeModeGame), gameMode.ToString())) : 6);
				int[] array = (from m in SceneInfoController.instance.GetListScenesForMode(needMode).avaliableScenes
				select m.indexMap).ToArray<int>();
				text += "( ";
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						ConnectSceneNGUIController.mapProperty,
						" = ",
						array[i]
					});
					if (i + 1 < array.Length)
					{
						text += " OR ";
					}
				}
				text += " )";
			}
			else
			{
				text = ConnectSceneNGUIController.mapProperty + " = " + mapIndex;
			}
			text = text + " AND " + ConnectSceneNGUIController.passwordProperty + " = \"\"";
			if (!Defs.isDaterRegim)
			{
				string text2 = text;
				string[] array2 = new string[5];
				array2[0] = text2;
				array2[1] = " AND ";
				array2[2] = ConnectSceneNGUIController.platformProperty;
				array2[3] = " = ";
				int num = 4;
				int num2 = (int)ConnectSceneNGUIController.myPlatformConnect;
				array2[num] = num2.ToString();
				text = string.Concat(array2);
			}
			if (joinToNewRound != 0)
			{
				if (joinToNewRound == 1)
				{
					text = text + " AND " + ConnectSceneNGUIController.endingProperty + " = 2";
				}
			}
			else
			{
				text = text + " AND " + ConnectSceneNGUIController.endingProperty + " = 0";
			}
			if (ExpController.Instance != null && ExpController.Instance.OurTier == 0)
			{
				if (Defs.isABTestBalansCohortActual)
				{
					text = text + " AND " + ConnectSceneNGUIController.ABTestProperty + " = 1";
				}
				else
				{
					text = text + " AND " + ConnectSceneNGUIController.ABTestProperty + " = 0";
				}
			}
			if (Defs.isActivABTestBuffSystem)
			{
				text = text + " AND " + ConnectSceneNGUIController.ABTestEnum + " = ";
				if (abTestSeparate)
				{
					if (Defs.isActivABTestBuffSystem)
					{
						text += ((!ABTestController.useBuffSystem) ? 1 : 3);
					}
				}
				else if (Defs.isActivABTestBuffSystem)
				{
					text += ((!ABTestController.useBuffSystem) ? 3 : 1);
				}
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[ConnectSceneNGUIController.passwordProperty] = string.Empty;
		if (!Defs.useSqlLobby)
		{
			hashtable[ConnectSceneNGUIController.mapProperty] = mapIndex;
		}
		if (!Defs.isDaterRegim && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			hashtable[ConnectSceneNGUIController.maxKillProperty] = 3;
		}
		if (joinToNewRound == 0)
		{
			hashtable[ConnectSceneNGUIController.endingProperty] = 0;
		}
		if (!Defs.isDaterRegim)
		{
			hashtable[ConnectSceneNGUIController.platformProperty] = (int)ConnectSceneNGUIController.myPlatformConnect;
		}
		if (ExpController.Instance != null && ExpController.Instance.OurTier == 0)
		{
			if (Defs.isABTestBalansCohortActual)
			{
				hashtable[ConnectSceneNGUIController.ABTestProperty] = 1;
			}
			else
			{
				hashtable[ConnectSceneNGUIController.ABTestProperty] = 0;
			}
		}
		PlayerPrefs.SetString("TypeGame", "client");
		if (Defs.useSqlLobby)
		{
			TypedLobby typedLobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
			Debug.Log(text);
			PhotonNetwork.JoinRandomRoom(hashtable, 0, MatchmakingMode.FillRoom, typedLobby, text, null);
		}
		else
		{
			PhotonNetwork.JoinRandomRoom(hashtable, 0);
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001DBF8 File Offset: 0x0001BDF8
	private void JoinRandomRoom(int mapIndex, ConnectSceneNGUIController.RegimGame gameMode)
	{
		this.joinNewRoundTries = 0;
		this.tryJoinRoundMap = mapIndex;
		if (mapIndex != -1)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(mapIndex);
			if (infoScene == null)
			{
				Debug.LogError("scInfo == null");
				return;
			}
			this.goMapName = infoScene.NameScene;
		}
		else if (!Defs.useSqlLobby)
		{
			mapIndex = ConnectSceneNGUIController.GetRandomMapIndex();
			if (mapIndex == -1)
			{
				return;
			}
			SceneInfo infoScene2 = SceneInfoController.instance.GetInfoScene(mapIndex);
			if (infoScene2 == null)
			{
				Debug.LogError("scInfo == null");
				return;
			}
			this.goMapName = infoScene2.NameScene;
		}
		else
		{
			this.goMapName = string.Empty;
		}
		if (!string.IsNullOrEmpty(this.goMapName))
		{
			if (WeaponManager.sharedManager != null)
			{
				WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
			}
			base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
			this.loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
		}
		ConnectSceneNGUIController.JoinRandomGameRoom(mapIndex, gameMode, this.joinNewRoundTries, this.abTestConnect);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x0001DD34 File Offset: 0x0001BF34
	private void HandleCreateRoomBtnClicked(object sender, EventArgs e)
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		string name = infoScene.gameObject.name;
		bool isPremium = infoScene.isPremium;
		if (isPremium && Storager.getInt(name + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(name))
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
			return;
		}
		string text = FilterBadWorld.FilterString(this.nameServerInput.value);
		bool flag = false;
		if (Defs.isInet)
		{
			RoomInfo[] roomList = PhotonNetwork.GetRoomList();
			for (int i = 0; i < roomList.Length; i++)
			{
				if (roomList[i].name.Equals(text))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			this.nameAlreadyUsedLabel.timer = 3f;
			this.nameAlreadyUsedLabel.gameObject.SetActive(true);
			return;
		}
		this.goMapName = name;
		PlayerPrefs.SetString("MapName", this.goMapName);
		if (this.killToWin.value.Value > this.killToWin.maxValue.Value)
		{
			this.killToWin.value = this.killToWin.maxValue;
		}
		if (this.killToWin.value.Value < this.killToWin.minValue.Value)
		{
			this.killToWin.value = this.killToWin.minValue;
		}
		PlayerPrefs.SetString("MaxKill", "4");
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(name)) ? 0 : Defs.filterMaps[name]);
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
		this.loadingMapPanel.SetActive(true);
		int num = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Deathmatch && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.TimeBattle && ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.Duel) ? this.teamCountPlayer.value : 2) : this.numberOfPlayer.value.Value;
		int maxKill = (!Defs.isDaterRegim) ? ((ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames) ? ((!Defs.isDaterRegim) ? 4 : 5) : 10) : this.killToWin.value.Value;
		if (Defs.isInet)
		{
			this.loadingMapPanel.SetActive(true);
			ActivityIndicator.IsActiveIndicator = true;
			ConnectSceneNGUIController.CreateGameRoom(text, num, infoScene.indexMap, maxKill, this.setPasswordInput.value, ConnectSceneNGUIController.regim);
		}
		else
		{
			bool useNat = Network.HavePublicAddress();
			Network.InitializeServer(num - 1, 25002, useNat);
			PlayerPrefs.SetString("ServerName", text);
			PlayerPrefs.SetString("PlayersLimits", num.ToString());
			Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		}
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0001E044 File Offset: 0x0001C244
	private void ShowKillToWinPanel(bool show)
	{
		if (show)
		{
			this.numberOfPlayer.transform.localPosition = new Vector3(this.posNumberOffPlayersX, this.numberOfPlayer.transform.localPosition.y, this.numberOfPlayer.transform.localPosition.z);
			this.teamCountPlayer.transform.localPosition = new Vector3(this.posNumberOffPlayersX, this.teamCountPlayer.transform.localPosition.y, this.teamCountPlayer.transform.localPosition.z);
			this.killToWin.headLabel.text = LocalizationStore.Get("Key_0953");
			this.killToWin.gameObject.SetActive(true);
		}
		else
		{
			this.numberOfPlayer.transform.localPosition = new Vector3(0f, this.numberOfPlayer.transform.localPosition.y, this.numberOfPlayer.transform.localPosition.z);
			this.teamCountPlayer.transform.localPosition = new Vector3(0f, this.teamCountPlayer.transform.localPosition.y, this.teamCountPlayer.transform.localPosition.z);
			this.killToWin.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x0001E1C8 File Offset: 0x0001C3C8
	private void HandleGoToCreateRoomBtnClicked(object sender, EventArgs e)
	{
		PlayerPrefs.SetString("TypeGame", "server");
		this.password = string.Empty;
		this.SetPosSelectMapPanelInCreatePanel();
		this.createPanel.SetActive(true);
		this.setPasswordInput.gameObject.SetActive(Defs.isInet);
		this.nameServerInput.transform.localPosition = ((!Defs.isInet) ? new Vector3(0f, this.startPosNameServerNameInput.y, this.startPosNameServerNameInput.z) : this.startPosNameServerNameInput);
		this.selectMapPanel.SetActive(true);
		base.StartCoroutine(this.SetUseMasMap(false, false));
		this.customPanel.SetActive(false);
		this.nameAlreadyUsedLabel.timer = -1f;
		this.nameAlreadyUsedLabel.gameObject.SetActive(false);
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 2;
			this.numberOfPlayer.maxValue.Value = 10;
			this.numberOfPlayer.value.Value = 10;
			if (Defs.isDaterRegim)
			{
				this.ShowKillToWinPanel(true);
				this.killToWin.minValue.Value = this.daterMinValue;
				this.killToWin.maxValue.Value = this.daterMaxValue;
				this.killToWin.value.Value = this.daterMinValue;
				this.killToWin.stepValue = this.daterStep;
			}
			else
			{
				this.ShowKillToWinPanel(false);
				if (ExperienceController.sharedController != null)
				{
					if (ExperienceController.sharedController.currentLevel <= 2)
					{
						this.killToWin.minValue.Value = 3;
						this.killToWin.maxValue.Value = 7;
						this.killToWin.value.Value = 3;
						this.killToWin.stepValue = 2;
					}
					else
					{
						this.killToWin.minValue.Value = 3;
						this.killToWin.maxValue.Value = 7;
						this.killToWin.value.Value = 3;
						this.killToWin.stepValue = 2;
					}
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 2;
			this.numberOfPlayer.maxValue.Value = 4;
			this.numberOfPlayer.value.Value = 4;
			this.ShowKillToWinPanel(false);
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel <= 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel <= 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.teamCountPlayer.SetValue(10);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 2;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel <= 2)
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
				else
				{
					this.killToWin.minValue.Value = 3;
					this.killToWin.maxValue.Value = 7;
					this.killToWin.value.Value = 3;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 3;
			this.numberOfPlayer.maxValue.Value = 8;
			this.numberOfPlayer.value.Value = 6;
			this.ShowKillToWinPanel(false);
			this.killToWin.stepValue = 5;
			if (ExperienceController.sharedController != null)
			{
				if (ExperienceController.sharedController.currentLevel <= 2)
				{
					this.killToWin.minValue.Value = 5;
					this.killToWin.maxValue.Value = 10;
					this.killToWin.value.Value = 10;
				}
				else
				{
					this.killToWin.minValue.Value = 5;
					this.killToWin.maxValue.Value = 10;
					this.killToWin.value.Value = 10;
				}
			}
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel)
		{
			this.teamCountPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.gameObject.SetActive(false);
			this.numberOfPlayer.minValue.Value = 2;
			this.numberOfPlayer.maxValue.Value = 2;
			this.numberOfPlayer.value.Value = 2;
			this.ShowKillToWinPanel(false);
			this.killToWin.minValue.Value = 3;
			this.killToWin.maxValue.Value = 7;
			this.killToWin.value.Value = 3;
			this.killToWin.stepValue = 2;
		}
	}

	// Token: 0x0600038B RID: 907 RVA: 0x0001E8B0 File Offset: 0x0001CAB0
	private void HandleShowSearchPanelBtnClicked(object sender, EventArgs e)
	{
		this.customPanel.SetActive(false);
		if (this.searchInput != null)
		{
			this.searchInput.value = this.gameNameFilter;
		}
		this.searchPanel.SetActive(true);
	}

	// Token: 0x0600038C RID: 908 RVA: 0x0001E8F8 File Offset: 0x0001CAF8
	private void HandleClearBtnClicked(object sender, EventArgs e)
	{
		if (this.searchInput != null)
		{
			this.searchInput.value = string.Empty;
		}
	}

	// Token: 0x0600038D RID: 909 RVA: 0x0001E91C File Offset: 0x0001CB1C
	private void HandleSearchBtnClicked(object sender, EventArgs e)
	{
		this.customPanel.SetActive(true);
		if (this.searchInput != null)
		{
			this.gameNameFilter = this.searchInput.value;
		}
		this.updateFilteredRoomList(this.gameNameFilter);
		this.scrollGames.ResetPosition();
		this.searchPanel.SetActive(false);
	}

	// Token: 0x0600038E RID: 910 RVA: 0x0001E97C File Offset: 0x0001CB7C
	private void HandleCancelFromConnectToPhotonBtnClicked()
	{
		if (this._someWindowSubscription != null)
		{
			this._someWindowSubscription.Dispose();
		}
		if (this.failInternetLabel != null)
		{
			this.failInternetLabel.SetActive(false);
		}
		if (this.connectToPhotonPanel != null)
		{
			this.connectToPhotonPanel.SetActive(false);
		}
		if (this.actAfterConnectToPhoton != null)
		{
			this.actAfterConnectToPhoton = null;
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x0600038F RID: 911 RVA: 0x0001E9FC File Offset: 0x0001CBFC
	private void HandleCancelFromConnectToPhotonBtnClicked(object sender, EventArgs e)
	{
		this.HandleCancelFromConnectToPhotonBtnClicked();
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0001EA04 File Offset: 0x0001CC04
	private void LogBuyMap(string context)
	{
		try
		{
			AnalyticsStuff.LogSales(context, "Premium Maps", false);
		}
		catch (Exception arg)
		{
			Debug.LogError("LogBuyMap exception: " + arg);
		}
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0001EA54 File Offset: 0x0001CC54
	private void HandleUnlockMapBtnClicked(object sender, EventArgs e)
	{
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (scInfo == null)
		{
			return;
		}
		int mapPrice = Defs.PremiumMaps[scInfo.NameScene];
		Action action = delegate()
		{
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int @int = Storager.getInt("Coins", false);
			int num = @int - mapPrice;
			string nameScene = scInfo.NameScene;
			if (num >= 0)
			{
				this.LogBuyMap(nameScene);
				AnalyticsFacade.InAppPurchase(nameScene, "Premium Maps", 1, mapPrice, "Coins");
				Storager.setInt(nameScene + "Key", 1, true);
				this.selectMap.mapPreviewTexture.mainTexture = this.mapPreview[nameScene];
				Storager.setInt("Coins", num, false);
				ShopNGUIController.SpendBoughtCurrency("Coins", mapPrice);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				ShopNGUIController.SynchronizeAndroidPurchases("Map unlocked from connect scene: " + nameScene);
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
			}
			else
			{
				StoreKitEventListener.State.PurchaseKey = "In map selection";
				if (BankController.Instance != null)
				{
					EventHandler handleBackFromBank = null;
					handleBackFromBank = delegate(object sender_, EventArgs e_)
					{
						BankController.Instance.BackRequested -= handleBackFromBank;
						this.mainPanel.transform.root.gameObject.SetActive(true);
						coinsShop.thisScript.notEnoughCurrency = null;
						BankController.Instance.InterfaceEnabled = false;
					};
					BankController.Instance.BackRequested += handleBackFromBank;
					this.mainPanel.transform.root.gameObject.SetActive(false);
					coinsShop.thisScript.notEnoughCurrency = "Coins";
					BankController.Instance.InterfaceEnabled = true;
				}
				else
				{
					Debug.LogWarning("BankController.Instance == null");
				}
			}
		};
		action();
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0001EACC File Offset: 0x0001CCCC
	public void ShowBankWindow()
	{
		if (BankController.Instance != null)
		{
			EventHandler backFromBankHandler = null;
			backFromBankHandler = delegate(object backSender, EventArgs backArgs)
			{
				BankController.Instance.BackRequested -= backFromBankHandler;
				this.mainPanel.transform.root.gameObject.SetActive(true);
				BankController.Instance.InterfaceEnabled = false;
			};
			BankController.Instance.BackRequested += backFromBankHandler;
			this.mainPanel.transform.root.gameObject.SetActive(false);
			BankController.Instance.InterfaceEnabled = true;
		}
		else
		{
			Debug.LogWarning("BankController.Instance == null");
		}
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0001EB54 File Offset: 0x0001CD54
	private void HandleCoinsShopClicked(object sender, EventArgs e)
	{
		this.ShowBankWindow();
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001EB5C File Offset: 0x0001CD5C
	private void HandleLocalBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		Defs.isInet = false;
		this.CustomBtnAct();
	}

	// Token: 0x06000395 RID: 917 RVA: 0x0001EB98 File Offset: 0x0001CD98
	private void ShowConnectToPhotonPanel()
	{
		this._someWindowSubscription = BackSystem.Instance.Register(new Action(this.HandleCancelFromConnectToPhotonBtnClicked), "Connect to Photon panel");
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			this.accountBlockedLabel.timer = 3f;
			this.accountBlockedLabel.gameObject.SetActive(true);
			return;
		}
		this.ConnectToPhoton();
		this.connectToPhotonPanel.SetActive(true);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001EC1C File Offset: 0x0001CE1C
	private void HandleCustomBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.CustomBtnAct);
		PhotonNetwork.autoJoinLobby = true;
		this.ShowConnectToPhotonPanel();
	}

	// Token: 0x06000397 RID: 919 RVA: 0x0001EC68 File Offset: 0x0001CE68
	private void CustomBtnAct()
	{
		this.gameNameFilter = string.Empty;
		if (Defs.isInet)
		{
			base.Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
		this.showSearchPanelBtn.SetActive(Defs.isInet);
		this.mainPanel.SetActive(false);
		this.selectMapPanel.SetActive(false);
		this.customPanel.SetActive(true);
		if (!Defs.isDaterRegim)
		{
			this.headCustomPanel.SetText(LocalizationStore.Get(ConnectSceneNGUIController.gameModesLocalizeKey[((int)ConnectSceneNGUIController.regim).ToString()]));
		}
		else
		{
			this.headCustomPanel.gameObject.SetActive(false);
		}
		this.password = string.Empty;
		this.incorrectPasswordLabel.timer = -1f;
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		this.gameIsfullLabel.timer = -1f;
		this.gameIsfullLabel.gameObject.SetActive(false);
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0001ED64 File Offset: 0x0001CF64
	[Obfuscation(Exclude = true)]
	private void UpdateFilteredRoomListInvoke()
	{
		this.updateFilteredRoomList(this.gameNameFilter);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001ED74 File Offset: 0x0001CF74
	private void HandleRandomBtnClicked(object sender, EventArgs e)
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.RandomBtnAct);
		PhotonNetwork.autoJoinLobby = false;
		this.ShowConnectToPhotonPanel();
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001EDC0 File Offset: 0x0001CFC0
	private void RandomBtnAct()
	{
		this.JoinRandomRoom(-1, ConnectSceneNGUIController.regim);
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0001EDD0 File Offset: 0x0001CFD0
	public static int GetRandomMapIndex()
	{
		bool flag = true;
		AllScenesForMode listScenesForMode = SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		if (listScenesForMode == null)
		{
			return -1;
		}
		int count = listScenesForMode.avaliableScenes.Count;
		int num = UnityEngine.Random.Range(0, count);
		int i = 0;
		while (i <= count)
		{
			SceneInfo sceneInfo = listScenesForMode.avaliableScenes[num];
			if (!(sceneInfo == null))
			{
				num++;
				i++;
				if (num >= count)
				{
					num = 0;
				}
				flag = (sceneInfo.isPremium && Storager.getInt(sceneInfo.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(sceneInfo.NameScene));
			}
			if (!flag)
			{
				return sceneInfo.indexMap;
			}
		}
		return -1;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0001EE98 File Offset: 0x0001D098
	public void HandleGoBtnClicked(object sender, EventArgs e)
	{
		if (this.selectMap.mapID == -1)
		{
			this.HandleRandomBtnClicked(sender, e);
			return;
		}
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		this.actAfterConnectToPhoton = new Action(this.GoBtnAct);
		PhotonNetwork.autoJoinLobby = false;
		this.ShowConnectToPhotonPanel();
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001EF00 File Offset: 0x0001D100
	private void GoBtnAct()
	{
		if (this.selectMap.mapID == -1)
		{
			this.RandomBtnAct();
			return;
		}
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
		if (infoScene == null)
		{
			return;
		}
		bool isPremium = infoScene.isPremium;
		if (!isPremium || (isPremium && (Storager.getInt(infoScene.NameScene + "Key", true) == 1 || PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))))
		{
			this.JoinRandomRoom(infoScene.indexMap, ConnectSceneNGUIController.regim);
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001EFA8 File Offset: 0x0001D1A8
	private void HandleBackBtnClicked(object sender, EventArgs e)
	{
		if (this.mainPanel != null && this.mainPanel.activeSelf)
		{
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.GetFriendsData(false);
			}
			this.mapPreview.Clear();
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
			LoadConnectScene.noteToShow = null;
			Application.LoadLevel(Defs.PromSceneName);
			this.isGoInPhotonGame = false;
		}
		if (this.customPanel != null && this.customPanel.activeSelf)
		{
			this.connectToWiFIInCreateLabel.SetActive(false);
			this.connectToWiFIInCustomLabel.SetActive(false);
			this.createRoomUIBtn.isEnabled = true;
			Defs.isInet = true;
			this.customPanel.SetActive(false);
			while (this.gridGamesTransform.childCount > 0)
			{
				Transform child = this.gridGamesTransform.GetChild(0);
				child.parent = null;
				this.gamesInfo.Remove(child.gameObject);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			this.mainPanel.SetActive(true);
			this.selectMapPanel.SetActive(true);
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
		if (this.searchPanel != null && this.searchPanel.activeSelf)
		{
			this.searchInput.value = this.gameNameFilter;
			this.searchPanel.SetActive(false);
			this.customPanel.SetActive(true);
		}
		if (this.createPanel != null && this.createPanel.activeSelf)
		{
			PlayerPrefs.SetString("TypeGame", "client");
			this.createPanel.SetActive(false);
			base.StartCoroutine(this.SetUseMasMap(true, true));
			this.customPanel.SetActive(true);
		}
		if (this.setPasswordPanel != null && this.setPasswordPanel.activeSelf)
		{
			this.BackFromSetPasswordPanel();
		}
		if (this.enterPasswordPanel != null && this.enterPasswordPanel.activeSelf)
		{
			this.enterPasswordPanel.SetActive(false);
			this.customPanel.SetActive(true);
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
		}
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001F208 File Offset: 0x0001D408
	private void HandleUnlockBtnClicked(object sender, EventArgs e)
	{
		int _price = 0;
		string _storagerPurchasedKey = string.Empty;
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			_price = Defs.CaptureFlagPrice;
			_storagerPurchasedKey = Defs.CaptureFlagPurchasedKey;
		}
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
		{
			_price = Defs.HungerGamesPrice;
			_storagerPurchasedKey = Defs.hungerGamesPurchasedKey;
		}
		Action action = delegate()
		{
			coinsShop.thisScript.notEnoughCurrency = null;
			coinsShop.thisScript.onReturnAction = null;
			int @int = Storager.getInt("Coins", false);
			int num = @int - _price;
			if (num >= 0)
			{
				Storager.setInt(_storagerPurchasedKey, 1, true);
				Storager.setInt("Coins", num, false);
				ShopNGUIController.SpendBoughtCurrency("Coins", _price);
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
				ShopNGUIController.SynchronizeAndroidPurchases("Mode enabled");
				if (coinsPlashka.thisScript != null)
				{
					coinsPlashka.thisScript.enabled = false;
				}
				this.unlockBtn.SetActive(false);
				this.customBtn.SetActive(true);
				this.randomBtn.SetActive(true);
				this.conditionLabel.gameObject.SetActive(false);
				this.goBtn.SetActive(true);
			}
			else
			{
				StoreKitEventListener.State.PurchaseKey = "Mode opened";
				if (BankController.Instance != null)
				{
					EventHandler handleBackFromBank = null;
					handleBackFromBank = delegate(object sender_, EventArgs e_)
					{
						BankController.Instance.BackRequested -= handleBackFromBank;
						this.mainPanel.transform.root.gameObject.SetActive(true);
						coinsShop.thisScript.notEnoughCurrency = null;
						BankController.Instance.InterfaceEnabled = false;
					};
					BankController.Instance.BackRequested += handleBackFromBank;
					this.mainPanel.transform.root.gameObject.SetActive(false);
					coinsShop.thisScript.notEnoughCurrency = "Coins";
					BankController.Instance.InterfaceEnabled = true;
				}
				else
				{
					Debug.LogWarning("BankController.Instance == null");
				}
			}
		};
		action();
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001F28C File Offset: 0x0001D48C
	private static void SetFlagsForDeathmatchRegim()
	{
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isFlag = false;
		Defs.isCapturePoints = false;
		Defs.IsSurvival = false;
		Defs.isDuel = false;
		StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
		StoreKitEventListener.State.Parameters.Clear();
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
	private void SetRegim(ConnectSceneNGUIController.RegimGame _regim)
	{
		PlayerPrefs.SetInt("RegimMulty", (int)_regim);
		ConnectSceneNGUIController.regim = _regim;
		if (!Defs.isDaterRegim)
		{
			this.unlockMapBtn.SetActive(false);
			this.unlockMapBtnInCreate.SetActive(false);
		}
		this.createRoomBtn.SetActive(true);
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isCOOP = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TimeBattle);
		Defs.isCompany = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight);
		Defs.isHunger = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.DeadlyGames);
		Defs.isFlag = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture);
		Defs.isCapturePoints = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints);
		Defs.isDuel = (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Duel);
		Defs.IsSurvival = false;
		this.localBtn.SetActive(BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 && (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.Deathmatch || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight));
		this.customButtonsGrid.Reposition();
		if (this.conditionLabel != null)
		{
			this.conditionLabel.gameObject.SetActive(false);
		}
		if (this.unlockBtn != null)
		{
			this.unlockBtn.SetActive(false);
		}
		if (this.randomBtn != null)
		{
			this.randomBtn.SetActive(true);
		}
		if (this.customBtn != null)
		{
			this.customBtn.SetActive(true);
		}
		if (this.goBtn != null)
		{
			this.goBtn.SetActive(true);
		}
		switch (ConnectSceneNGUIController.regim)
		{
		case ConnectSceneNGUIController.RegimGame.Deathmatch:
			StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = ((!Defs.isDaterRegim) ? this.rulesDeadmatch : this.rulesDater);
			break;
		case ConnectSceneNGUIController.RegimGame.TimeBattle:
			StoreKitEventListener.State.Mode = "Time Survival";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesTimeBattle;
			break;
		case ConnectSceneNGUIController.RegimGame.TeamFight:
			StoreKitEventListener.State.Mode = "Team Battle";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesTeamFight;
			break;
		case ConnectSceneNGUIController.RegimGame.DeadlyGames:
			StoreKitEventListener.State.Mode = "Deadly Games";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesDeadlyGames;
			break;
		case ConnectSceneNGUIController.RegimGame.FlagCapture:
			StoreKitEventListener.State.Mode = "Flag Capture";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesFlagCapture;
			break;
		case ConnectSceneNGUIController.RegimGame.CapturePoints:
			StoreKitEventListener.State.Mode = "Capture points";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesCapturePoint;
			break;
		case ConnectSceneNGUIController.RegimGame.Duel:
			StoreKitEventListener.State.Mode = "Duel";
			StoreKitEventListener.State.Parameters.Clear();
			this.rulesLabel.text = this.rulesDuel;
			break;
		}
		base.StartCoroutine(this.SetUseMasMap(true, false));
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0001F634 File Offset: 0x0001D834
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			if (BannerWindowController.SharedController != null && BannerWindowController.SharedController.IsAnyBannerShown)
			{
				BannerWindowController.SharedController.HideBannerWindow();
			}
			else
			{
				this.HandleBackBtnClicked(null, EventArgs.Empty);
			}
		}, "Connect Scene");
		this.OnEnableWhenAnimate();
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0001F674 File Offset: 0x0001D874
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001F694 File Offset: 0x0001D894
	private void Update()
	{
		if (Defs.IsDeveloperBuild)
		{
			Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text = string.Concat(new string[]
				{
					text,
					array[i].transform.root.gameObject.name,
					" ",
					array[i].gameObject.name,
					" ",
					LayerMask.LayerToName(array[i].transform.root.gameObject.layer),
					" - "
				});
			}
			if (this._logCache != text)
			{
				this._logCache = text;
				Debug.Log("Cameras:" + text);
			}
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			this.countNote++;
		}
		bool flag = this.deathmatchToggle != null && this.deathmatchToggle.isEnable;
		if (this.armoryButton != null && this.armoryButton.activeSelf != flag)
		{
			this.armoryButton.SetActive(flag);
		}
		if (this.customPanel.activeSelf && !Defs.isInet)
		{
			if (this.isFirstUpdateLocalServerList)
			{
				this.UpdateLocalServersList();
			}
			else
			{
				this.isFirstUpdateLocalServerList = true;
			}
		}
		if (!Defs.isInet)
		{
			this.connectToWiFIInCreateLabel.SetActive(!this.CheckLocalAvailability());
			this.connectToWiFIInCustomLabel.SetActive(!this.CheckLocalAvailability());
			if (this.createRoomUIBtn.isEnabled != this.CheckLocalAvailability())
			{
				this.createRoomUIBtn.isEnabled = this.CheckLocalAvailability();
			}
		}
		else
		{
			if (this.connectToWiFIInCreateLabel.activeSelf)
			{
				this.connectToWiFIInCreateLabel.SetActive(false);
			}
			if (this.connectToWiFIInCreateLabel.activeSelf)
			{
				this.connectToWiFIInCustomLabel.SetActive(false);
			}
		}
		if (!this.unlockBtn.activeSelf && (this.mainPanel.activeSelf || this.createPanel.activeSelf) && this.selectMap != null)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.selectMap.mapID);
			if (infoScene == null)
			{
				return;
			}
			bool flag2 = !this.isSetUseMap && infoScene.isPremium;
			if (flag2 && Storager.getInt(infoScene.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
			{
				if (!this.unlockMapBtn.activeSelf)
				{
					this.priceMapLabel.text = Defs.PremiumMaps[infoScene.NameScene].ToString();
					this.unlockMapBtn.SetActive(true);
					this.goBtn.SetActive(false);
					this.priceMapLabelInCreate.text = Defs.PremiumMaps[infoScene.NameScene].ToString();
					this.unlockMapBtnInCreate.SetActive(true);
					this.createRoomBtn.SetActive(false);
				}
			}
			else if (this.unlockMapBtn.activeSelf)
			{
				this.unlockMapBtn.SetActive(false);
				this.goBtn.SetActive(true);
				this.unlockMapBtnInCreate.SetActive(false);
				this.createRoomBtn.SetActive(true);
			}
		}
		if ((!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && (!(BannerWindowController.SharedController != null) || !BannerWindowController.SharedController.IsAnyBannerShown) && (!(this.loadingToDraw != null) || !this.loadingToDraw.gameObject.activeInHierarchy) && (!(this._loadingNGUIController != null) || !this._loadingNGUIController.gameObject.activeInHierarchy) && SkinEditorController.sharedController == null && ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = true;
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0001FAD4 File Offset: 0x0001DCD4
	private bool IsUseMap(int indMap)
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(ConnectSceneNGUIController.curSelectMode, indMap);
		if (infoScene != null)
		{
			bool flag = infoScene.isPremium && Storager.getInt(infoScene.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene);
			return !flag;
		}
		return false;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0001FB3C File Offset: 0x0001DD3C
	private static void ResetWeaponManagerForDeathmatch()
	{
		ConnectSceneNGUIController.SetFlagsForDeathmatchRegim();
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(0);
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0001FB6C File Offset: 0x0001DD6C
	private IEnumerator LoadMapPreview()
	{
		List<SceneInfo> listAllNeedMap = new List<SceneInfo>();
		if (Defs.isDaterRegim)
		{
			AllScenesForMode scMode = SceneInfoController.instance.GetListScenesForMode(TypeModeGame.Dater);
			if (scMode != null)
			{
				listAllNeedMap.AddRange(scMode.avaliableScenes);
			}
		}
		else
		{
			TypeModeGame[] arrNeedMode = new TypeModeGame[]
			{
				TypeModeGame.Deathmatch,
				TypeModeGame.TeamFight,
				TypeModeGame.TimeBattle,
				TypeModeGame.FlagCapture,
				TypeModeGame.DeadlyGames,
				TypeModeGame.CapturePoints,
				TypeModeGame.Duel
			};
			foreach (TypeModeGame curMode in arrNeedMode)
			{
				AllScenesForMode scMode2 = (!(SceneInfoController.instance != null)) ? null : SceneInfoController.instance.GetListScenesForMode(curMode);
				if (scMode2 != null)
				{
					listAllNeedMap.AddRange(scMode2.avaliableScenes);
				}
			}
		}
		string allScene = string.Empty;
		for (int scI = 0; scI < listAllNeedMap.Count; scI++)
		{
			if (!this.mapPreview.ContainsKey(listAllNeedMap[scI].NameScene))
			{
				allScene = allScene + listAllNeedMap[scI].NameScene + "\n";
				this.mapPreview.Add(listAllNeedMap[scI].NameScene, Resources.Load("LevelLoadingsPreview" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + listAllNeedMap[scI].NameScene) as Texture);
				bool _isClose = listAllNeedMap[scI].isPremium && Storager.getInt(listAllNeedMap[scI].NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(listAllNeedMap[scI].NameScene);
				if (_isClose)
				{
					this.mapPreview.Add(listAllNeedMap[scI].NameScene + "_off", Resources.Load<Texture>(string.Concat(new string[]
					{
						"LevelLoadingsPreview",
						(!Device.isRetinaAndStrong) ? string.Empty : "/Hi",
						"/Loading_",
						listAllNeedMap[scI].NameScene,
						"_off"
					})));
				}
				yield return null;
			}
		}
		this.mapPreview.Add("Random", Resources.Load("LevelLoadingsPreview" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Random_Map") as Texture);
		if (Application.isEditor)
		{
			Debug.Log(allScene);
		}
		yield return null;
		this.mainPanel.SetActive(true);
		this.selectMapPanel.SetActive(true);
		ConnectSceneNGUIController.ResetWeaponManagerForDeathmatch();
		this.SetRegim(ConnectSceneNGUIController.regim);
		yield return null;
		string dismissReason = ConnectSceneNGUIController.GetReasonToDismissInterstitialConnectScene();
		if (string.IsNullOrEmpty(dismissReason))
		{
			string format = (!Application.isEditor) ? "{0}.LoadMapPreview(), InterstitialRequest: {1}" : "<color=magenta>{0}.LoadMapPreview(), InterstitialRequest: {1}</color>";
			Debug.LogFormat(format, new object[]
			{
				base.GetType().Name,
				ConnectSceneNGUIController.InterstitialRequest
			});
			if (ConnectSceneNGUIController.InterstitialRequest)
			{
				AdsConfigMemento adsConfig = AdsConfigManager.Instance.LastLoadedConfig;
				string category = AdsConfigManager.GetPlayerCategory(adsConfig);
				ReturnInConnectSceneAdPointMemento pointConfig = adsConfig.AdPointsConfig.ReturnInConnectScene;
				double delayInSeconds = pointConfig.GetFinalDelayInSeconds(category);
				float startWaitingTime = Time.realtimeSinceStartup;
				while ((double)(Time.realtimeSinceStartup - startWaitingTime) < delayInSeconds)
				{
					yield return null;
				}
			}
		}
		else
		{
			string format2 = (!Application.isEditor) ? "Dismissing wait for interstitial. {0}" : "<color=magenta>Dismissing wait for interstitial. {0}</color>";
			Debug.LogFormat(format2, new object[]
			{
				dismissReason
			});
		}
		ConnectSceneNGUIController.InterstitialRequest = false;
		ActivityIndicator.IsActiveIndicator = false;
		if (!Defs.isDaterRegim)
		{
			base.StartCoroutine(this.AnimateModeOpen());
		}
		yield return null;
		this.loadingMapPanel.SetActive(false);
		if (ConnectSceneNGUIController.NeedShowReviewInConnectScene)
		{
			BannerWindowController.firstScreen = true;
		}
		yield return new WaitForSeconds(1f);
		if (ConnectSceneNGUIController.NeedShowReviewInConnectScene)
		{
			ConnectSceneNGUIController.NeedShowReviewInConnectScene = false;
			ReviewHUDWindow.Instance.ShowWindowRating();
		}
		yield break;
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001FB88 File Offset: 0x0001DD88
	internal static string GetReasonToDismissFakeInterstitial()
	{
		string result;
		try
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				result = "Ads config is `null`.";
			}
			else if (lastLoadedConfig.Exception != null)
			{
				result = lastLoadedConfig.Exception.Message;
			}
			else
			{
				string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
				InterstitialConfigMemento interstitialConfig = lastLoadedConfig.InterstitialConfig;
				bool enabled = interstitialConfig.GetEnabled(playerCategory);
				FakeInterstitialConfigMemento fakeInterstitialConfig = lastLoadedConfig.FakeInterstitialConfig;
				double timeSpanSinceLastShowInMinutes = AdsConfigManager.GetTimeSpanSinceLastShowInMinutes();
				double timeoutBetweenShowInMinutes = interstitialConfig.GetTimeoutBetweenShowInMinutes(playerCategory);
				if (timeSpanSinceLastShowInMinutes < timeoutBetweenShowInMinutes)
				{
					result = "TimeoutBetweenShowInMinutes";
				}
				else
				{
					string disabledReason = fakeInterstitialConfig.GetDisabledReason(playerCategory, ExperienceController.GetCurrentLevel(), InterstitialCounter.Instance.FakeInterstitialCount, InterstitialCounter.Instance.FakeInterstitialCount + InterstitialCounter.Instance.RealInterstitialCount, enabled);
					result = disabledReason;
				}
			}
		}
		catch (Exception ex)
		{
			result = ex.ToString();
		}
		return result;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001FC84 File Offset: 0x0001DE84
	internal static int GetReasonCodeToDismissInterstitialConnectScene()
	{
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return 100;
		}
		if (lastLoadedConfig.Exception != null)
		{
			return 200;
		}
		int interstitialDisabledReasonCode = AdsConfigManager.GetInterstitialDisabledReasonCode(lastLoadedConfig);
		if (interstitialDisabledReasonCode != 0)
		{
			return 300 + interstitialDisabledReasonCode;
		}
		ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
		if (returnInConnectScene == null)
		{
			return 400;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		int disabledReasonCode = returnInConnectScene.GetDisabledReasonCode(playerCategory);
		if (disabledReasonCode != 0)
		{
			return 500 + disabledReasonCode;
		}
		int currentDailyInterstitialCount = FyberFacade.Instance.GetCurrentDailyInterstitialCount();
		int finalImpressionMaxCountPerDay = returnInConnectScene.GetFinalImpressionMaxCountPerDay(playerCategory);
		if (currentDailyInterstitialCount >= finalImpressionMaxCountPerDay)
		{
			return 600;
		}
		double totalMinutes = InGameTimeKeeper.Instance.CurrentInGameTime.TotalMinutes;
		double finalMinInGameTimePerDayInMinutes = returnInConnectScene.GetFinalMinInGameTimePerDayInMinutes(playerCategory);
		if (totalMinutes < finalMinInGameTimePerDayInMinutes)
		{
			return 700;
		}
		return 0;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001FD58 File Offset: 0x0001DF58
	internal static string GetReasonToDismissInterstitialConnectScene()
	{
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return "Ads config is `null`.";
		}
		if (lastLoadedConfig.Exception != null)
		{
			return lastLoadedConfig.Exception.Message;
		}
		string interstitialDisabledReason = AdsConfigManager.GetInterstitialDisabledReason(lastLoadedConfig);
		if (!string.IsNullOrEmpty(interstitialDisabledReason))
		{
			return interstitialDisabledReason;
		}
		ReturnInConnectSceneAdPointMemento returnInConnectScene = lastLoadedConfig.AdPointsConfig.ReturnInConnectScene;
		if (returnInConnectScene == null)
		{
			return string.Format("`{0}` config is `null`", returnInConnectScene.Id);
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		string disabledReason = returnInConnectScene.GetDisabledReason(playerCategory);
		if (!string.IsNullOrEmpty(disabledReason))
		{
			return disabledReason;
		}
		int currentDailyInterstitialCount = FyberFacade.Instance.GetCurrentDailyInterstitialCount();
		int finalImpressionMaxCountPerDay = returnInConnectScene.GetFinalImpressionMaxCountPerDay(playerCategory);
		if (currentDailyInterstitialCount >= finalImpressionMaxCountPerDay)
		{
			return string.Format(CultureInfo.InvariantCulture, "`interstitialCount: {0}` >= `maxInterstitialCount: {1}` for `{2}`", new object[]
			{
				currentDailyInterstitialCount,
				finalImpressionMaxCountPerDay,
				playerCategory
			});
		}
		double totalMinutes = InGameTimeKeeper.Instance.CurrentInGameTime.TotalMinutes;
		double finalMinInGameTimePerDayInMinutes = returnInConnectScene.GetFinalMinInGameTimePerDayInMinutes(playerCategory);
		if (totalMinutes < finalMinInGameTimePerDayInMinutes)
		{
			return string.Format(CultureInfo.InvariantCulture, "`inGameTimeMinutes: {0:f2}` < `minInGameTimePerDayInMinutes: {1:f2}` for `{2}`", new object[]
			{
				totalMinutes,
				finalMinInGameTimePerDayInMinutes,
				playerCategory
			});
		}
		return string.Empty;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0001FE90 File Offset: 0x0001E090
	public static void UpdateUseMasMaps()
	{
		if (Defs.isDaterRegim)
		{
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.Dater;
			return;
		}
		switch (ConnectSceneNGUIController.regim)
		{
		case ConnectSceneNGUIController.RegimGame.TimeBattle:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.TimeBattle;
			return;
		case ConnectSceneNGUIController.RegimGame.TeamFight:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.TeamFight;
			return;
		case ConnectSceneNGUIController.RegimGame.DeadlyGames:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.DeadlyGames;
			return;
		case ConnectSceneNGUIController.RegimGame.FlagCapture:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.FlagCapture;
			return;
		case ConnectSceneNGUIController.RegimGame.CapturePoints:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.CapturePoints;
			return;
		case ConnectSceneNGUIController.RegimGame.Duel:
			ConnectSceneNGUIController.curSelectMode = TypeModeGame.Duel;
			return;
		}
		ConnectSceneNGUIController.curSelectMode = TypeModeGame.Deathmatch;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0001FF30 File Offset: 0x0001E130
	private IEnumerator SetUseMasMap(bool isUseRandom = true, bool isOffSelectMapPanel = false)
	{
		if (isOffSelectMapPanel)
		{
			this.scrollViewSelectMapTransform.GetComponent<UIPanel>().alpha = 0.001f;
		}
		this.isSetUseMap = true;
		SpringPanel _spr = this.ScrollTransform.GetComponent<SpringPanel>();
		if (_spr != null)
		{
			UnityEngine.Object.Destroy(_spr);
		}
		this.ScrollTransform.GetComponent<UIPanel>().clipOffset = new Vector2(0f, 0f);
		if (isUseRandom && !isOffSelectMapPanel)
		{
			this.SetPosSelectMapPanelInMainMenu();
		}
		int maxCountMaps = SceneInfoController.instance.GetMaxCountMapsInRegims();
		maxCountMaps++;
		AllScenesForMode modeInfo = (!(SceneInfoController.instance != null)) ? null : SceneInfoController.instance.GetListScenesForMode(ConnectSceneNGUIController.curSelectMode);
		if (modeInfo == null)
		{
			Debug.LogError("modeInfo == null");
			yield break;
		}
		yield return null;
		if (this.grid.transform.childCount < maxCountMaps)
		{
			float widthBorder = 15f;
			int countColumn = ((double)((float)Screen.width / (float)Screen.height) >= 1.5) ? 4 : 3;
			float _widthCell = (this.fonMapPreview.localSize.x - (float)countColumn * widthBorder - widthBorder) / (float)countColumn;
			float _heightCell = 1f;
			float _scale = 1f;
			this.mapPreviewTexture.SetActive(true);
			int startCountMapsPreview = this.grid.transform.childCount;
			for (int i = startCountMapsPreview; i < maxCountMaps; i++)
			{
				GameObject newTexture = UnityEngine.Object.Instantiate<GameObject>(this.mapPreviewTexture);
				newTexture.transform.SetParent(this.grid.transform, false);
				MapPreviewController currentMapPreviewController = newTexture.GetComponent<MapPreviewController>();
				_scale = _widthCell / currentMapPreviewController.mapPreviewTexture.localSize.x;
				_heightCell = currentMapPreviewController.mapPreviewTexture.localSize.y * _scale;
				newTexture.transform.GetChild(0).localScale = new Vector3(_scale, _scale, 1f);
				newTexture.name = "Map_" + i;
			}
			this.mapPreviewTexture.SetActive(false);
			this.grid.GetComponent<UIGrid>().cellWidth = _widthCell + widthBorder;
			this.grid.GetComponent<UIGrid>().cellHeight = _heightCell + widthBorder;
			this.grid.GetComponent<UIGrid>().maxPerLine = countColumn;
			this.grid.GetComponent<UIGrid>().Reposition();
		}
		List<SceneInfo> mapsLst = (ExperienceController.sharedController.currentLevel >= 2) ? modeInfo.avaliableScenes : modeInfo.avaliableScenes.Shuffle<SceneInfo>().ToList<SceneInfo>();
		if (isUseRandom)
		{
			GameObject randomCell = this.grid.transform.GetChild(0).gameObject;
			MapPreviewController randomCellPreviewController = randomCell.GetComponent<MapPreviewController>();
			randomCellPreviewController.mapPreviewTexture.mainTexture = ((!this.mapPreview.ContainsKey("Random")) ? null : this.mapPreview["Random"]);
			randomCellPreviewController.NameMapLbl.GetComponent<SetHeadLabelText>().SetText(LocalizationStore.Get("Key_2463"));
			randomCellPreviewController.bottomPanel.SetActive(false);
			randomCellPreviewController.mapID = -1;
			randomCellPreviewController.sceneMapName = "Random";
		}
		for (int j = 0; j < mapsLst.Count; j++)
		{
			SceneInfo scInfo = mapsLst[j];
			GameObject newTexture2 = this.grid.transform.GetChild(j + ((!isUseRandom) ? 0 : 1)).gameObject;
			if (!newTexture2.activeSelf)
			{
				newTexture2.SetActive(true);
			}
			MapPreviewController currentMapPreviewController2 = newTexture2.GetComponent<MapPreviewController>();
			bool flag = scInfo.isPremium && Storager.getInt(scInfo.NameScene + "Key", true) == 0 && !PremiumAccountController.MapAvailableDueToPremiumAccount(scInfo.NameScene);
			if (!currentMapPreviewController2.bottomPanel.activeSelf)
			{
				currentMapPreviewController2.bottomPanel.SetActive(true);
			}
			currentMapPreviewController2.mapPreviewTexture.mainTexture = ((!this.mapPreview.ContainsKey(scInfo.NameScene)) ? null : this.mapPreview[scInfo.NameScene]);
			currentMapPreviewController2.NameMapLbl.GetComponent<SetHeadLabelText>().SetText(scInfo.TranslatePreviewName.ToUpper());
			currentMapPreviewController2.SizeMapNameLbl[0].SetActive(scInfo.sizeMap == InfoSizeMap.small);
			currentMapPreviewController2.SizeMapNameLbl[1].SetActive(scInfo.sizeMap == InfoSizeMap.normal);
			currentMapPreviewController2.SizeMapNameLbl[2].SetActive(scInfo.sizeMap == InfoSizeMap.big || scInfo.sizeMap == InfoSizeMap.veryBig);
			currentMapPreviewController2.mapID = scInfo.indexMap;
			currentMapPreviewController2.sceneMapName = scInfo.NameScene;
			if (scInfo.AvaliableWeapon == ModeWeapon.knifes)
			{
				currentMapPreviewController2.milee.SetActive(true);
				currentMapPreviewController2.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0096");
			}
			else if (scInfo.AvaliableWeapon == ModeWeapon.sniper)
			{
				currentMapPreviewController2.milee.SetActive(true);
				currentMapPreviewController2.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_0949");
			}
			else if (Defs.isDaterRegim)
			{
				currentMapPreviewController2.milee.SetActive(true);
				currentMapPreviewController2.milee.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1421");
			}
			else
			{
				currentMapPreviewController2.milee.SetActive(false);
			}
			currentMapPreviewController2.UpdatePopularity();
		}
		this.scrollViewSelectMapTransform.GetComponent<UIScrollView>().ResetPosition();
		this.grid.transform.localPosition = new Vector3(this.ScrollTransform.GetComponent<UIPanel>().baseClipRegion.x, this.grid.transform.localPosition.y, this.grid.transform.localPosition.z);
		for (int k = mapsLst.Count + ((!isUseRandom) ? 0 : 1); k < this.grid.transform.childCount; k++)
		{
			GameObject newTexture3 = this.grid.transform.GetChild(k).gameObject;
			if (newTexture3.activeSelf)
			{
				newTexture3.SetActive(false);
			}
		}
		this.SelectMap(ConnectSceneNGUIController.selectedMap);
		yield return null;
		this.scrollViewSelectMapTransform.GetComponent<UIPanel>().SetDirty();
		this.scrollViewSelectMapTransform.GetComponent<UIPanel>().Refresh();
		if (isOffSelectMapPanel)
		{
			this.selectMapPanel.SetActive(false);
			this.scrollViewSelectMapTransform.GetComponent<UIPanel>().alpha = 1f;
			this.SetPosSelectMapPanelInMainMenu();
		}
		this.isSetUseMap = false;
		yield break;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0001FF68 File Offset: 0x0001E168
	private void SelectMap(string map)
	{
		ConnectSceneNGUIController.selectedMap = map;
		float num = this.scrollViewSelectMapTransform.GetComponent<UIScrollView>().bounds.extents.y * 2f;
		float y = this.scrollViewSelectMapTransform.GetComponent<UIPanel>().GetViewSize().y;
		if (!string.IsNullOrEmpty(ConnectSceneNGUIController.selectedMap))
		{
			Transform child = this.grid.transform.GetChild(0);
			foreach (object obj in this.grid.transform)
			{
				Transform transform = (Transform)obj;
				string sceneMapName = transform.GetComponent<MapPreviewController>().sceneMapName;
				if (ConnectSceneNGUIController.selectedMap.Equals(sceneMapName))
				{
					if (num > y)
					{
						float num2 = -1f * (transform.localPosition.y + this._heightCell * 0.5f);
						if (num2 < 0f)
						{
							num2 = 0f;
						}
						float y2 = this.scrollViewSelectMapTransform.GetComponent<UIPanel>().clipSoftness.y;
						float num3 = num - y + y2;
						if (num2 > num3)
						{
							num2 = num3;
						}
						this.scrollViewSelectMapTransform.localPosition = new Vector3(this.scrollViewSelectMapTransform.localPosition.x, num2, this.scrollViewSelectMapTransform.localPosition.z);
					}
					transform.GetComponent<UIToggle>().value = true;
					this.selectMap = transform.GetComponent<MapPreviewController>();
					break;
				}
			}
			ConnectSceneNGUIController.selectedMap = string.Empty;
		}
		else
		{
			this.grid.transform.GetChild(0).GetComponent<UIToggle>().value = true;
			this.selectMap = this.grid.transform.GetChild(0).GetComponent<MapPreviewController>();
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x00020170 File Offset: 0x0001E370
	public void OnReceivedRoomListUpdate()
	{
		if (!this.customPanel.activeSelf || !Defs.isInet)
		{
			return;
		}
		if (Defs.isInet)
		{
			base.Invoke("UpdateFilteredRoomListInvoke", 0.03f);
		}
	}

	// Token: 0x060003AF RID: 943 RVA: 0x000201A8 File Offset: 0x0001E3A8
	private void SetRoomInfo(GameInfo _gameInfo, int index)
	{
		_gameInfo.index = index;
		if (this.filteredRoomList.Count > index)
		{
			_gameInfo.gameObject.SetActive(true);
			RoomInfo roomInfo = this.filteredRoomList[index];
			string text = roomInfo.name;
			if (text.Length == 36 && text.IndexOf("-") == 8 && text.LastIndexOf("-") == 23)
			{
				text = LocalizationStore.Get("Key_0088");
			}
			_gameInfo.serverNameLabel.text = text;
			_gameInfo.countPlayersLabel.text = roomInfo.playerCount + "/" + roomInfo.maxPlayers;
			bool flag = string.IsNullOrEmpty(roomInfo.customProperties[ConnectSceneNGUIController.passwordProperty].ToString());
			_gameInfo.openSprite.SetActive(flag);
			_gameInfo.closeSprite.SetActive(!flag);
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(roomInfo.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
			string text2 = infoScene.TranslatePreviewName.ToUpper();
			_gameInfo.mapNameLabel.SetText(text2);
			_gameInfo.roomInfo = roomInfo;
			_gameInfo.mapTexture.mainTexture = this.mapPreview[infoScene.NameScene];
			_gameInfo.SizeMapNameLbl[0].SetActive(infoScene.sizeMap == InfoSizeMap.small);
			_gameInfo.SizeMapNameLbl[1].SetActive(infoScene.sizeMap == InfoSizeMap.normal);
			_gameInfo.SizeMapNameLbl[2].SetActive(infoScene.sizeMap == InfoSizeMap.big || infoScene.sizeMap == InfoSizeMap.veryBig);
		}
		else
		{
			_gameInfo.gameObject.SetActive(false);
		}
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0002035C File Offset: 0x0001E55C
	public void updateFilteredRoomList(string gFilter)
	{
		this.filteredRoomList.Clear();
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		bool flag = !string.IsNullOrEmpty(gFilter);
		for (int i = 0; i < roomList.Length; i++)
		{
			if (flag || roomList[i].playerCount != (int)roomList[i].maxPlayers)
			{
				if (!Defs.isDaterRegim && roomList[i].customProperties[ConnectSceneNGUIController.platformProperty] != null)
				{
					string text = roomList[i].customProperties[ConnectSceneNGUIController.platformProperty].ToString();
					int num = (int)ConnectSceneNGUIController.myPlatformConnect;
					if (!text.Equals(num.ToString()) && !roomList[i].customProperties[ConnectSceneNGUIController.platformProperty].ToString().Equals(3.ToString()))
					{
						goto IL_225;
					}
				}
				if (!Defs.isABTestBalansCohortActual || !(ExpController.Instance != null) || ExpController.Instance.OurTier != 0 || (roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] != null && (int)roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] == 1))
				{
					if (Defs.isABTestBalansCohortActual || !(ExpController.Instance != null) || ExpController.Instance.OurTier != 0 || roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] == null || (int)roomList[i].customProperties[ConnectSceneNGUIController.ABTestProperty] != 1)
					{
						bool flag2 = true;
						if (flag)
						{
							flag2 = (roomList[i].name.StartsWith(gFilter, true, null) && (roomList[i].name.Length != 36 || roomList[i].name.IndexOf("-") != 8 || roomList[i].name.LastIndexOf("-") != 23));
						}
						if (flag2 && this.IsUseMap((int)roomList[i].customProperties[ConnectSceneNGUIController.mapProperty]))
						{
							this.filteredRoomList.Add(roomList[i]);
						}
					}
				}
			}
			IL_225:;
		}
		if (this.countNote > 10)
		{
			this.countNote = 1;
		}
		this.countNote = 50;
		if (this.filteredRoomList.Count < this.countNote)
		{
			this.countNote = this.filteredRoomList.Count;
		}
		while (this.countNote < this.gamesInfo.Count)
		{
			UnityEngine.Object.Destroy(this.gamesInfo[this.gamesInfo.Count - 1]);
			this.gamesInfo.RemoveAt(this.gamesInfo.Count - 1);
		}
		if (this.countNote > this.gamesInfo.Count)
		{
			this.countColumn = (((double)((float)Screen.width / (float)Screen.height) >= 1.5) ? 4 : 3);
			this._widthCell = (this.fonGames.localSize.x - (float)(this.countColumn * 10)) / (float)this.countColumn;
			if (this.countNote > this.gamesInfo.Count)
			{
				this.gameInfoItemPrefab.SetActive(true);
			}
			while (this.countNote > this.gamesInfo.Count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.gameInfoItemPrefab);
				gameObject.name = "GameInfo_" + this.gamesInfo.Count;
				gameObject.transform.SetParent(this.gridGamesTransform, false);
				this._scale = this._widthCell / gameObject.GetComponent<GameInfo>().mapTexture.localSize.x;
				this._heightCell = gameObject.GetComponent<GameInfo>().mapTexture.localSize.y * this._scale;
				gameObject.transform.GetChild(0).transform.localScale = new Vector3(this._scale, this._scale, this._scale);
				this.gamesInfo.Add(gameObject);
				this.gameInfoItemPrefab.SetActive(false);
			}
			if (this.gameInfoItemPrefab.activeSelf)
			{
				this.gameInfoItemPrefab.SetActive(false);
			}
			this.gridGames.GetComponent<UIGrid>().cellWidth = this._widthCell + this.borderWidth;
			this.gridGames.GetComponent<UIGrid>().cellHeight = this._heightCell + this.borderWidth;
			this.gridGames.GetComponent<UIGrid>().maxPerLine = this.countColumn;
		}
		float num2 = this.scrollGames.bounds.extents.y * 2f;
		float y = this.scrollGames.GetComponent<UIPanel>().GetViewSize().y;
		this.gridGames.Reposition();
		if (!this.isFirstGamesReposition || num2 < y)
		{
			this.gridGames.transform.localPosition = new Vector3(-(this._widthCell + this.borderWidth) * ((float)this.countColumn * 0.5f - 0.5f), this.gridGames.transform.localPosition.y, this.gridGames.transform.localPosition.z);
			this.scrollGames.ResetPosition();
			this.isFirstGamesReposition = true;
		}
		for (int j = 0; j < this.countNote; j++)
		{
			this.SetRoomInfo(this.gamesInfo[j].GetComponent<GameInfo>(), j);
		}
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x00020928 File Offset: 0x0001EB28
	private void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed");
		if (string.IsNullOrEmpty(this.goMapName))
		{
			int randomMapIndex = ConnectSceneNGUIController.GetRandomMapIndex();
			if (randomMapIndex == -1)
			{
				return;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(randomMapIndex);
			if (infoScene == null)
			{
				return;
			}
			this.goMapName = infoScene.name;
		}
		SceneInfo infoScene2 = SceneInfoController.instance.GetInfoScene(this.goMapName);
		if (infoScene2 == null)
		{
			return;
		}
		if (this.joinNewRoundTries >= 2 && this.abTestConnect)
		{
			this.abTestConnect = false;
			this.joinNewRoundTries = 0;
		}
		if (this.joinNewRoundTries < 2)
		{
			Debug.Log("No rooms with new round: " + this.joinNewRoundTries + ((!this.abTestConnect) ? string.Empty : " <color=yellow>AbTestSeparate</color>"));
			this.joinNewRoundTries++;
			ConnectSceneNGUIController.JoinRandomGameRoom(this.tryJoinRoundMap, ConnectSceneNGUIController.regim, this.joinNewRoundTries, this.abTestConnect);
			return;
		}
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(this.goMapName, false));
		int maxKill = (ConnectSceneNGUIController.regim != ConnectSceneNGUIController.RegimGame.DeadlyGames) ? ((!Defs.isDaterRegim) ? 4 : 5) : 10;
		int playerLimit = (!Defs.isCOOP) ? ((!Defs.isCompany) ? ((!Defs.isHunger) ? ((!Defs.isDuel) ? 10 : 2) : 6) : 10) : 4;
		ConnectSceneNGUIController.CreateGameRoom(null, playerLimit, infoScene2.indexMap, maxKill, string.Empty, ConnectSceneNGUIController.regim);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x00020B0C File Offset: 0x0001ED0C
	private void OnPhotonJoinRoomFailed()
	{
		ActivityIndicator.IsActiveIndicator = false;
		this.loadingMapPanel.SetActive(false);
		this.gameIsfullLabel.timer = 3f;
		this.gameIsfullLabel.gameObject.SetActive(true);
		this.incorrectPasswordLabel.timer = -1f;
		this.incorrectPasswordLabel.gameObject.SetActive(false);
		Debug.Log("OnPhotonJoinRoomFailed");
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00020B78 File Offset: 0x0001ED78
	private void OnJoinedRoom()
	{
		AnalyticsStuff.LogMultiplayer();
		Debug.Log("OnJoinedRoom " + PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString());
		PhotonNetwork.isMessageQueueRunning = false;
		NotificationController.ResetPaused();
		GlobalGameController.healthMyPlayer = 0f;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		this.goMapName = infoScene.NameScene;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(this.goMapName)) ? 0 : Defs.filterMaps[this.goMapName]);
		}
		base.StartCoroutine(this.MoveToGameScene(infoScene.NameScene));
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00020C54 File Offset: 0x0001EE54
	private void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00020C60 File Offset: 0x0001EE60
	private void OnPhotonCreateRoomFailed()
	{
		Debug.Log("OnPhotonCreateRoomFailed");
		this.nameAlreadyUsedLabel.timer = 3f;
		this.nameAlreadyUsedLabel.gameObject.SetActive(true);
		this.loadingMapPanel.SetActive(false);
		ActivityIndicator.IsActiveIndicator = false;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00020CAC File Offset: 0x0001EEAC
	private void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");
		if ((!this.mainPanel.activeSelf || this.loadingMapPanel.activeSelf) && this.firstConnectToPhoton && Defs.isInet)
		{
			this.mainPanel.SetActive(true);
			this.selectMapPanel.SetActive(true);
			this.createPanel.SetActive(false);
			base.StartCoroutine(this.SetUseMasMap(true, false));
			this.customPanel.SetActive(false);
			while (this.gridGamesTransform.childCount > 0)
			{
				Transform child = this.gridGamesTransform.GetChild(0);
				child.parent = null;
				this.gamesInfo.Remove(child.gameObject);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			this.searchPanel.SetActive(false);
			this.setPasswordPanel.SetActive(false);
			this.enterPasswordPanel.SetActive(false);
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = true;
			}
			this.loadingMapPanel.SetActive(false);
			this.SetPosSelectMapPanelInMainMenu();
			this.serverIsNotAvalible.timer = 3f;
			this.serverIsNotAvalible.gameObject.SetActive(true);
			UICamera.selectedObject = null;
			ConnectSceneNGUIController.RegimGame regim = ConnectSceneNGUIController.regim;
			ConnectSceneNGUIController.ResetWeaponManagerForDeathmatch();
			this.SetRegim(regim);
		}
		if (this.actAfterConnectToPhoton != null)
		{
			base.Invoke("ConnectToPhoton", 0.5f);
		}
		if (this.connectToPhotonPanel.activeSelf)
		{
			this.failInternetLabel.SetActive(true);
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00020E40 File Offset: 0x0001F040
	private void OnFailedToConnectToPhoton(object parameters)
	{
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
		if (this.connectToPhotonPanel.activeSelf)
		{
			this.failInternetLabel.SetActive(true);
		}
		if (!this.isCancelConnectingToPhoton)
		{
			base.Invoke("ConnectToPhoton", 1f);
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00020E94 File Offset: 0x0001F094
	public void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
		this.firstConnectToPhoton = true;
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		if (this.connectToPhotonPanel.activeSelf && this.actAfterConnectToPhoton != new Action(this.RandomBtnAct))
		{
			this.connectToPhotonPanel.SetActive(false);
		}
		if (this.actAfterConnectToPhoton != null)
		{
			this.actAfterConnectToPhoton();
			this.actAfterConnectToPhoton = null;
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
		}
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00020F24 File Offset: 0x0001F124
	public void OnConnectedToPhoton()
	{
		Debug.Log("OnConnectedToPhoton");
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00020F30 File Offset: 0x0001F130
	public void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby: " + PhotonNetwork.lobby.Name);
		this.OnConnectedToMaster();
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00020F54 File Offset: 0x0001F154
	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		this.GetMapName(_mapName, isAddCountRun);
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
		while (WeaponManager.sharedManager == null)
		{
			yield return null;
		}
		while (WeaponManager.sharedManager.LockGetWeaponPrefabs > 0)
		{
			yield return null;
		}
		this.ShowLoadingGUI(_mapName);
		yield break;
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00020F8C File Offset: 0x0001F18C
	private void SetFonLoading(string _mapName = "", bool isAddCountRun = false)
	{
		this.GetMapName(_mapName, isAddCountRun);
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
		this.ShowLoadingGUI(_mapName);
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00020FD0 File Offset: 0x0001F1D0
	private void ShowLoadingGUI(string _mapName)
	{
		BannerWindowController.SharedController.HideBannerWindowNoShowNext();
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = LoadConnectScene.textureToShow;
		this._loadingNGUIController.transform.parent = this.loadingMapPanel.transform;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00021060 File Offset: 0x0001F260
	private void GetMapName(string _mapName, bool isAddCountRun)
	{
		Debug.Log("setFonLoading " + _mapName);
		if (Defs.isCOOP)
		{
			int @int = PlayerPrefs.GetInt("CountRunCoop", 0);
			bool flag = @int < 5;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCoop", PlayerPrefs.GetInt("CountRunCoop", 0) + 1);
			}
			Texture texture = Resources.Load("NoteLoadings/note_Time_Survival_" + @int % this.countNoteCaptureCOOP) as Texture;
		}
		else if (Defs.isCompany)
		{
			int int2 = PlayerPrefs.GetInt("CountRunCompany", 0);
			bool flag = int2 < 5;
			Texture texture = Resources.Load("NoteLoadings/note_Team_Battle_" + int2 % this.countNoteCaptureCompany) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunCompany", PlayerPrefs.GetInt("CountRunCompany", 0) + 1);
			}
		}
		else if (Defs.isHunger)
		{
			int int3 = PlayerPrefs.GetInt("CountRunHunger", 0);
			bool flag = int3 < 5;
			Texture texture = Resources.Load("NoteLoadings/note_Deadly_Games_" + int3 % this.countNoteCaptureHunger) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunHunger", PlayerPrefs.GetInt("CountRunHunger", 0) + 1);
			}
		}
		else if (Defs.isFlag)
		{
			int int4 = PlayerPrefs.GetInt("CountRunFlag", 0);
			bool flag = int4 < 5;
			Texture texture = Resources.Load("NoteLoadings/note_Flag_Capture_" + int4 % this.countNoteCaptureFlag) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunFlag", PlayerPrefs.GetInt("CountRunFlag", 0) + 1);
			}
		}
		else
		{
			int int5 = PlayerPrefs.GetInt("CountRunDeadmath", 0);
			bool flag = int5 < 5;
			Texture texture = Resources.Load("NoteLoadings/note_Deathmatch_" + int5 % this.countNoteCaptureDeadmatch) as Texture;
			if (isAddCountRun)
			{
				PlayerPrefs.SetInt("CountRunDeadmath", PlayerPrefs.GetInt("CountRunDeadmath", 0) + 1);
			}
		}
		LoadConnectScene.textureToShow = (Resources.Load("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + _mapName) as Texture2D);
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = _mapName;
		LoadConnectScene.noteToShow = null;
		this.loadingToDraw.gameObject.SetActive(false);
		this.loadingToDraw.mainTexture = null;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x000212C4 File Offset: 0x0001F4C4
	private IEnumerator MoveToGameScene(string _goMapName)
	{
		Debug.Log("MoveToGameScene=" + _goMapName);
		Defs.isGameFromFriends = false;
		Defs.isGameFromClans = false;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(_goMapName)) ? 0 : Defs.filterMaps[_goMapName]);
		}
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		PlayerPrefs.SetString("RoomName", PhotonNetwork.room.name);
		PlayerPrefs.SetInt("CustomGame", 0);
		PhotonNetwork.isMessageQueueRunning = false;
		this.mapPreview.Clear();
		yield return null;
		yield return Resources.UnloadUnusedAssets();
		yield return base.StartCoroutine(this.SetFonLoadingWaitForReset(_goMapName, true));
		this.loadingMapPanel.SetActive(true);
		this.isGoInPhotonGame = true;
		AsyncOperation async = Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		if (FriendsController.sharedController != null)
		{
			FriendsController.sharedController.GetFriendsData(false);
		}
		yield return async;
		for (int i = 0; i < this.grid.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.grid.transform.GetChild(i).gameObject);
		}
		this.mapPreview.Clear();
		yield break;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x000212F0 File Offset: 0x0001F4F0
	[Obfuscation(Exclude = true)]
	private void ConnectToPhoton()
	{
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			return;
		}
		if (PhotonNetwork.connectionState == ConnectionState.Connecting || PhotonNetwork.connectionState == ConnectionState.Connected)
		{
			Debug.Log("ConnectToPhoton return");
			return;
		}
		Debug.Log("ConnectToPhoton");
		if (FriendsController.sharedController != null && FriendsController.sharedController.Banned == 1)
		{
			this.timerShowBan = 3f;
			return;
		}
		this.isConnectingToPhoton = true;
		this.isCancelConnectingToPhoton = false;
		ConnectSceneNGUIController.gameTier = ((!(ExpController.Instance != null)) ? 1 : ExpController.Instance.OurTier);
		if (Defs.useSqlLobby)
		{
			PhotonNetwork.lobby = new TypedLobby("PixelGun3D", LobbyType.SqlLobby);
		}
		PhotonNetwork.ConnectUsingSettings(string.Concat(new string[]
		{
			Initializer.Separator,
			ConnectSceneNGUIController.regim.ToString(),
			(!Defs.isDaterRegim) ? ((!Defs.isHunger) ? ConnectSceneNGUIController.gameTier.ToString() : "0") : "Dater",
			"v",
			GlobalGameController.MultiplayerProtocolVersion
		}));
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00021434 File Offset: 0x0001F634
	private void StartSearchLocalServers()
	{
		this.lanScan.StartSearchBroadCasting(new LANBroadcastService.delJoinServer(this.SeachServer));
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00021450 File Offset: 0x0001F650
	private void SeachServer(string ipServerSeaches)
	{
		bool flag = false;
		if (this.servers.Count > 0)
		{
			foreach (ConnectSceneNGUIController.infoServer infoServer in this.servers)
			{
				if (infoServer.ipAddress.Equals(ipServerSeaches))
				{
					flag = true;
				}
			}
		}
		if (!flag)
		{
			ConnectSceneNGUIController.infoServer item = default(ConnectSceneNGUIController.infoServer);
			item.ipAddress = ipServerSeaches;
			this.servers.Add(item);
		}
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x000214F8 File Offset: 0x0001F6F8
	private int LocalServerComparison(LANBroadcastService.ReceivedMessage msg1, LANBroadcastService.ReceivedMessage msg2)
	{
		return msg1.ipAddress.CompareTo(msg2.ipAddress);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00021510 File Offset: 0x0001F710
	private void SetLocalRoomInfo(GameInfo _gameInfo, LANBroadcastService.ReceivedMessage _roomInfo)
	{
		string text = _roomInfo.name;
		if (string.IsNullOrEmpty(text))
		{
			text = LocalizationStore.Get("Key_0948");
		}
		_gameInfo.serverNameLabel.text = text;
		_gameInfo.countPlayersLabel.text = _roomInfo.connectedPlayers.ToString() + "/" + _roomInfo.playerLimit.ToString();
		_gameInfo.openSprite.SetActive(true);
		_gameInfo.closeSprite.SetActive(false);
		string map = _roomInfo.map;
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(_roomInfo.map);
		string text2 = infoScene.TranslatePreviewName.ToUpper();
		_gameInfo.mapNameLabel.SetText(text2);
		_gameInfo.roomInfoLocal = _roomInfo;
		_gameInfo.mapTexture.mainTexture = this.mapPreview[infoScene.NameScene];
		_gameInfo.SizeMapNameLbl[0].SetActive(infoScene.sizeMap == InfoSizeMap.small);
		_gameInfo.SizeMapNameLbl[1].SetActive(infoScene.sizeMap == InfoSizeMap.normal);
		_gameInfo.SizeMapNameLbl[2].SetActive(infoScene.sizeMap == InfoSizeMap.big || infoScene.sizeMap == InfoSizeMap.veryBig);
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00021634 File Offset: 0x0001F834
	private void UpdateLocalServersList()
	{
		List<LANBroadcastService.ReceivedMessage> list = new List<LANBroadcastService.ReceivedMessage>();
		for (int i = 0; i < this.lanScan.lstReceivedMessages.Count; i++)
		{
			bool flag = Defs.filterMaps.ContainsKey(this.lanScan.lstReceivedMessages[i].map) && Defs.filterMaps[this.lanScan.lstReceivedMessages[i].map] == 3;
			if (((Defs.isDaterRegim && flag) || (!Defs.isDaterRegim && !flag)) && this.lanScan.lstReceivedMessages[i].regim == (int)ConnectSceneNGUIController.regim)
			{
				list.Add(this.lanScan.lstReceivedMessages[i]);
			}
		}
		this.countNote = 50;
		if (list.Count < this.countNote)
		{
			this.countNote = list.Count;
		}
		while (this.countNote < this.gamesInfo.Count)
		{
			UnityEngine.Object.Destroy(this.gamesInfo[this.gamesInfo.Count - 1]);
			this.gamesInfo.RemoveAt(this.gamesInfo.Count - 1);
		}
		if (this.countNote > this.gamesInfo.Count)
		{
			this.countColumn = (((double)((float)Screen.width / (float)Screen.height) >= 1.5) ? 4 : 3);
			this._widthCell = (this.fonGames.localSize.x - (float)(this.countColumn * 10)) / (float)this.countColumn;
			if (this.countNote > this.gamesInfo.Count)
			{
				this.gameInfoItemPrefab.SetActive(true);
			}
			while (this.countNote > this.gamesInfo.Count)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.gameInfoItemPrefab);
				gameObject.name = "GameInfo_" + this.gamesInfo.Count;
				gameObject.transform.SetParent(this.gridGamesTransform, false);
				this._scale = this._widthCell / gameObject.GetComponent<GameInfo>().mapTexture.localSize.x;
				this._heightCell = gameObject.GetComponent<GameInfo>().mapTexture.localSize.y * this._scale;
				gameObject.transform.GetChild(0).localScale = new Vector3(this._scale, this._scale, this._scale);
				this.gamesInfo.Add(gameObject);
				this.gameInfoItemPrefab.SetActive(false);
			}
			if (this.gameInfoItemPrefab.activeSelf)
			{
				this.gameInfoItemPrefab.SetActive(false);
			}
			this.gridGames.GetComponent<UIGrid>().cellWidth = this._widthCell + this.borderWidth;
			this.gridGames.GetComponent<UIGrid>().cellHeight = this._heightCell + this.borderWidth;
			this.gridGames.GetComponent<UIGrid>().maxPerLine = this.countColumn;
		}
		float num = this.scrollGames.bounds.extents.y * 2f;
		float y = this.scrollGames.GetComponent<UIPanel>().GetViewSize().y;
		this.gridGames.Reposition();
		if (!this.isFirstGamesReposition || num < y)
		{
			this.gridGames.transform.localPosition = new Vector3(-(this._widthCell + this.borderWidth) * ((float)this.countColumn * 0.5f - 0.5f), this.gridGames.transform.localPosition.y, this.gridGames.transform.localPosition.z);
			this.scrollGames.ResetPosition();
			this.isFirstGamesReposition = true;
		}
		for (int j = 0; j < this.countNote; j++)
		{
			this.SetLocalRoomInfo(this.gamesInfo[j].GetComponent<GameInfo>(), list[j]);
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00021A80 File Offset: 0x0001FC80
	public void JoinToLocalRoom(LANBroadcastService.ReceivedMessage _roomInfo)
	{
		if (_roomInfo.connectedPlayers == _roomInfo.playerLimit)
		{
			this.gameIsfullLabel.timer = 3f;
			this.gameIsfullLabel.gameObject.SetActive(true);
			this.incorrectPasswordLabel.timer = -1f;
			this.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		GlobalGameController.countKillsBlue = 0;
		GlobalGameController.countKillsRed = 0;
		Defs.ServerIp = _roomInfo.ipAddress;
		PlayerPrefs.SetString("MaxKill", _roomInfo.comment);
		PlayerPrefs.SetString("MapName", _roomInfo.map);
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(_roomInfo.map)) ? 0 : Defs.filterMaps[_roomInfo.map]);
		}
		base.StartCoroutine(this.SetFonLoadingWaitForReset(_roomInfo.map, false));
		base.Invoke("goGame", 0.1f);
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00021B8C File Offset: 0x0001FD8C
	private bool CheckLocalAvailability()
	{
		return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00021B9C File Offset: 0x0001FD9C
	public void JoinToRoomPhoton(RoomInfo _roomInfo)
	{
		if (_roomInfo.playerCount == (int)_roomInfo.maxPlayers)
		{
			this.gameIsfullLabel.timer = 3f;
			this.gameIsfullLabel.gameObject.SetActive(true);
			this.incorrectPasswordLabel.timer = -1f;
			this.incorrectPasswordLabel.gameObject.SetActive(false);
			return;
		}
		this.joinRoomInfoFromCustom = _roomInfo;
		if (string.IsNullOrEmpty(_roomInfo.customProperties[ConnectSceneNGUIController.passwordProperty].ToString()))
		{
			this.JoinToRoomPhotonAfterCheck();
		}
		else
		{
			this.gameIsfullLabel.timer = -1f;
			this.gameIsfullLabel.gameObject.SetActive(false);
			this.incorrectPasswordLabel.timer = 3f;
			this.incorrectPasswordLabel.gameObject.SetActive(true);
			this.enterPasswordInput.value = string.Empty;
			this.enterPasswordPanel.SetActive(true);
			this.enterPasswordInput.isSelected = false;
			this.enterPasswordInput.isSelected = true;
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
			}
			this.customPanel.SetActive(false);
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00021CCC File Offset: 0x0001FECC
	private void EnterPassInputSubmit()
	{
		this.enterPasswordInput.RemoveFocus();
		this.enterPasswordInput.isSelected = false;
		base.Invoke("EnterPassInput", 0.1f);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00021CF8 File Offset: 0x0001FEF8
	private void EnterPassInput()
	{
		this.HandleJoinRoomFromEnterPasswordBtnClicked(null, null);
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00021D04 File Offset: 0x0001FF04
	public void JoinToRoomPhotonAfterCheck()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(this.joinRoomInfoFromCustom.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		this.loadingMapPanel.SetActive(true);
		PhotonNetwork.JoinRoom(this.joinRoomInfoFromCustom.name);
		ActivityIndicator.IsActiveIndicator = true;
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00021D74 File Offset: 0x0001FF74
	private void SetPosSelectMapPanelInMainMenu()
	{
		if (!Defs.isDaterRegim)
		{
			if (this.posSelectMapPanelInMainMenu.y < 9000f)
			{
				this.selectMapPanelTransform.localPosition = this.posSelectMapPanelInMainMenu;
			}
		}
		else
		{
			this.selectMapPanelTransform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00021DC8 File Offset: 0x0001FFC8
	private void SetPosSelectMapPanelInCreatePanel()
	{
		this.posSelectMapPanelInMainMenu = this.selectMapPanelTransform.localPosition;
		this.selectMapPanelTransform.localPosition = Vector3.zero;
		if (Defs.isDaterRegim)
		{
			this.selectMapPanelTransform.localPosition = new Vector3(0f, -90f, 0f);
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00021E20 File Offset: 0x00020020
	[Obfuscation(Exclude = true)]
	private void goGame()
	{
		WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(PlayerPrefs.GetString("MapName"))) ? 0 : Defs.filterMaps[PlayerPrefs.GetString("MapName")]);
		Singleton<SceneLoader>.Instance.LoadScene(PlayerPrefs.GetString("MapName"), LoadSceneMode.Single);
	}

	// Token: 0x060003CF RID: 975 RVA: 0x00021E80 File Offset: 0x00020080
	private void Awake()
	{
		this.abTestConnect = Defs.isActivABTestBuffSystem;
		if (ConnectSceneNGUIController.isReturnFromGame)
		{
			Defs.countReturnInConnectScene++;
		}
		PhotonObjectCacher.AddObject(base.gameObject);
		this.setPasswordInput.onSubmit.Add(new EventDelegate(delegate()
		{
			this.OnPaswordSelected();
		}));
		SceneInfoController.instance.UpdateListAvaliableMap();
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00021EE4 File Offset: 0x000200E4
	private void OnDestroy()
	{
		Debug.Log("OnDestroy ConnectSceneController");
		if (!Defs.isInet || (!this.isGoInPhotonGame && PhotonNetwork.connectionState == ConnectionState.Connected) || PhotonNetwork.connectionState == ConnectionState.Connecting)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
			Debug.Log("PhotonNetwork.Disconnect()");
		}
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.isShowRanks = false;
			ExperienceController.sharedController.isMenu = false;
			ExperienceController.sharedController.isConnectScene = false;
		}
		this.lanScan.StopBroadCasting();
		ConnectSceneNGUIController.sharedController = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00021F88 File Offset: 0x00020188
	public void HandleShopClicked()
	{
		if (ShopNGUIController.GuiActive)
		{
			return;
		}
		if (MainMenuController.IsLevelUpOrBannerShown() || (this.connectToPhotonPanel != null && this.connectToPhotonPanel.activeInHierarchy))
		{
			return;
		}
		ShopNGUIController.sharedShop.SetInGame(false);
		ShopNGUIController.GuiActive = true;
		ShopNGUIController.sharedShop.resumeAction = new Action(this.HandleResumeFromShop);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00021FF4 File Offset: 0x000201F4
	public void HandleResumeFromShop()
	{
		ShopNGUIController.GuiActive = false;
		ShopNGUIController.sharedShop.resumeAction = delegate()
		{
		};
		base.StartCoroutine(MainMenuController.ShowRanks());
	}

	// Token: 0x0400039B RID: 923
	public const string PendingInterstitialKey = "PendingInterstitial";

	// Token: 0x0400039C RID: 924
	public static ConnectSceneNGUIController.PlatformConnect myPlatformConnect = ConnectSceneNGUIController.PlatformConnect.ios;

	// Token: 0x0400039D RID: 925
	private string rulesDeadmatch;

	// Token: 0x0400039E RID: 926
	private string rulesDater;

	// Token: 0x0400039F RID: 927
	private string rulesTeamFight;

	// Token: 0x040003A0 RID: 928
	private string rulesTimeBattle;

	// Token: 0x040003A1 RID: 929
	private string rulesDeadlyGames;

	// Token: 0x040003A2 RID: 930
	private string rulesFlagCapture;

	// Token: 0x040003A3 RID: 931
	private string rulesCapturePoint;

	// Token: 0x040003A4 RID: 932
	private string rulesDuel;

	// Token: 0x040003A5 RID: 933
	public GameObject armoryButton;

	// Token: 0x040003A6 RID: 934
	public int myLevelGame;

	// Token: 0x040003A7 RID: 935
	public UILabel rulesLabel;

	// Token: 0x040003A8 RID: 936
	public static int gameTier = 1;

	// Token: 0x040003A9 RID: 937
	public static readonly IDictionary<string, string> gameModesLocalizeKey = new Dictionary<string, string>
	{
		{
			0.ToString(),
			"Key_0104"
		},
		{
			1.ToString(),
			"Key_0135"
		},
		{
			2.ToString(),
			"Key_0130"
		},
		{
			3.ToString(),
			"Key_0121"
		},
		{
			4.ToString(),
			"Key_0113"
		},
		{
			5.ToString(),
			"Key_1263"
		},
		{
			6.ToString(),
			"Key_1465"
		},
		{
			7.ToString(),
			"Key_1466"
		},
		{
			8.ToString(),
			"Key_2428"
		}
	};

	// Token: 0x040003AA RID: 938
	public UITable customButtonsGrid;

	// Token: 0x040003AB RID: 939
	public List<ConnectSceneNGUIController.infoServer> servers = new List<ConnectSceneNGUIController.infoServer>();

	// Token: 0x040003AC RID: 940
	private float posNumberOffPlayersX = -139f;

	// Token: 0x040003AD RID: 941
	private string goMapName;

	// Token: 0x040003AE RID: 942
	public SetHeadLabelText headCustomPanel;

	// Token: 0x040003AF RID: 943
	public static TypeModeGame curSelectMode;

	// Token: 0x040003B0 RID: 944
	private Dictionary<string, Texture> mapPreview = new Dictionary<string, Texture>();

	// Token: 0x040003B1 RID: 945
	public UILabel priceRegimLabel;

	// Token: 0x040003B2 RID: 946
	public UILabel priceMapLabel;

	// Token: 0x040003B3 RID: 947
	public UILabel priceMapLabelInCreate;

	// Token: 0x040003B4 RID: 948
	public GameObject mapPreviewTexture;

	// Token: 0x040003B5 RID: 949
	public GameObject grid;

	// Token: 0x040003B6 RID: 950
	public MyCenterOnChild centerScript;

	// Token: 0x040003B7 RID: 951
	public Transform ScrollTransform;

	// Token: 0x040003B8 RID: 952
	public Transform selectMapPanelTransform;

	// Token: 0x040003B9 RID: 953
	public MapPreviewController selectMap;

	// Token: 0x040003BA RID: 954
	public float widthCell;

	// Token: 0x040003BB RID: 955
	public int countMap;

	// Token: 0x040003BC RID: 956
	public UIButton createRoomUIBtn;

	// Token: 0x040003BD RID: 957
	public UISprite fonMapPreview;

	// Token: 0x040003BE RID: 958
	public UIPanel mapPreviewPanel;

	// Token: 0x040003BF RID: 959
	public GameObject mainPanel;

	// Token: 0x040003C0 RID: 960
	public GameObject localBtn;

	// Token: 0x040003C1 RID: 961
	public GameObject customBtn;

	// Token: 0x040003C2 RID: 962
	public GameObject randomBtn;

	// Token: 0x040003C3 RID: 963
	public GameObject goBtn;

	// Token: 0x040003C4 RID: 964
	public GameObject backBtn;

	// Token: 0x040003C5 RID: 965
	public GameObject unlockBtn;

	// Token: 0x040003C6 RID: 966
	public GameObject unlockMapBtnInCreate;

	// Token: 0x040003C7 RID: 967
	public GameObject unlockMapBtn;

	// Token: 0x040003C8 RID: 968
	public GameObject cancelFromConnectToPhotonBtn;

	// Token: 0x040003C9 RID: 969
	public GameObject connectToPhotonPanel;

	// Token: 0x040003CA RID: 970
	public GameObject failInternetLabel;

	// Token: 0x040003CB RID: 971
	public GameObject customPanel;

	// Token: 0x040003CC RID: 972
	public GameObject gameInfoItemPrefab;

	// Token: 0x040003CD RID: 973
	public GameObject loadingMapPanel;

	// Token: 0x040003CE RID: 974
	public GameObject searchPanel;

	// Token: 0x040003CF RID: 975
	public GameObject clearBtn;

	// Token: 0x040003D0 RID: 976
	public GameObject searchBtn;

	// Token: 0x040003D1 RID: 977
	public GameObject showSearchPanelBtn;

	// Token: 0x040003D2 RID: 978
	public GameObject selectMapPanel;

	// Token: 0x040003D3 RID: 979
	public GameObject createPanel;

	// Token: 0x040003D4 RID: 980
	public GameObject goToCreateRoomBtn;

	// Token: 0x040003D5 RID: 981
	public GameObject createRoomBtn;

	// Token: 0x040003D6 RID: 982
	public GameObject clearInSetPasswordBtn;

	// Token: 0x040003D7 RID: 983
	public GameObject okInsetPasswordBtn;

	// Token: 0x040003D8 RID: 984
	public GameObject setPasswordPanel;

	// Token: 0x040003D9 RID: 985
	public GameObject enterPasswordPanel;

	// Token: 0x040003DA RID: 986
	public GameObject joinRoomFromEnterPasswordBtn;

	// Token: 0x040003DB RID: 987
	public GameObject connectToWiFIInCreateLabel;

	// Token: 0x040003DC RID: 988
	public GameObject connectToWiFIInCustomLabel;

	// Token: 0x040003DD RID: 989
	public Transform scrollViewSelectMapTransform;

	// Token: 0x040003DE RID: 990
	public PlusMinusController numberOfPlayer;

	// Token: 0x040003DF RID: 991
	public PlusMinusController killToWin;

	// Token: 0x040003E0 RID: 992
	public TeamNumberOfPlayer teamCountPlayer;

	// Token: 0x040003E1 RID: 993
	public UIGrid gridGames;

	// Token: 0x040003E2 RID: 994
	public UISprite fonGames;

	// Token: 0x040003E3 RID: 995
	public UIInput searchInput;

	// Token: 0x040003E4 RID: 996
	public UIInput nameServerInput;

	// Token: 0x040003E5 RID: 997
	public UIInput setPasswordInput;

	// Token: 0x040003E6 RID: 998
	public UIInput enterPasswordInput;

	// Token: 0x040003E7 RID: 999
	public Transform gridGamesTransform;

	// Token: 0x040003E8 RID: 1000
	public UITexture loadingToDraw;

	// Token: 0x040003E9 RID: 1001
	public UILabel conditionLabel;

	// Token: 0x040003EA RID: 1002
	private static ConnectSceneNGUIController.RegimGame _regim = ConnectSceneNGUIController.RegimGame.Deathmatch;

	// Token: 0x040003EB RID: 1003
	public static bool isReturnFromGame;

	// Token: 0x040003EC RID: 1004
	public int nRegim;

	// Token: 0x040003ED RID: 1005
	private bool isSetUseMap;

	// Token: 0x040003EE RID: 1006
	public string gameNameFilter;

	// Token: 0x040003EF RID: 1007
	public List<GameObject> gamesInfo = new List<GameObject>();

	// Token: 0x040003F0 RID: 1008
	public DisableObjectFromTimer gameIsfullLabel;

	// Token: 0x040003F1 RID: 1009
	public DisableObjectFromTimer incorrectPasswordLabel;

	// Token: 0x040003F2 RID: 1010
	public DisableObjectFromTimer serverIsNotAvalible;

	// Token: 0x040003F3 RID: 1011
	public DisableObjectFromTimer accountBlockedLabel;

	// Token: 0x040003F4 RID: 1012
	public DisableObjectFromTimer nameAlreadyUsedLabel;

	// Token: 0x040003F5 RID: 1013
	private float timerShowBan = -1f;

	// Token: 0x040003F6 RID: 1014
	private bool isConnectingToPhoton;

	// Token: 0x040003F7 RID: 1015
	private bool isCancelConnectingToPhoton;

	// Token: 0x040003F8 RID: 1016
	private int pressButton;

	// Token: 0x040003F9 RID: 1017
	private List<RoomInfo> filteredRoomList = new List<RoomInfo>();

	// Token: 0x040003FA RID: 1018
	private int countNoteCaptureDeadmatch = 5;

	// Token: 0x040003FB RID: 1019
	private int countNoteCaptureCOOP = 5;

	// Token: 0x040003FC RID: 1020
	private int countNoteCaptureHunger = 5;

	// Token: 0x040003FD RID: 1021
	private int countNoteCaptureFlag = 5;

	// Token: 0x040003FE RID: 1022
	private int countNoteCaptureCompany = 5;

	// Token: 0x040003FF RID: 1023
	public static ConnectSceneNGUIController sharedController;

	// Token: 0x04000400 RID: 1024
	private string password = string.Empty;

	// Token: 0x04000401 RID: 1025
	public LANBroadcastService lanScan;

	// Token: 0x04000402 RID: 1026
	private RoomInfo joinRoomInfoFromCustom;

	// Token: 0x04000403 RID: 1027
	private bool firstConnectToPhoton;

	// Token: 0x04000404 RID: 1028
	private bool isGoInPhotonGame;

	// Token: 0x04000405 RID: 1029
	private bool isMainPanelActiv = true;

	// Token: 0x04000406 RID: 1030
	public GameObject ChooseMapLabelSmall;

	// Token: 0x04000407 RID: 1031
	private AdvertisementController _advertisementController;

	// Token: 0x04000408 RID: 1032
	public CategoryButtonsController categoryButtonsController;

	// Token: 0x04000409 RID: 1033
	public BtnCategory deathmatchToggle;

	// Token: 0x0400040A RID: 1034
	public BtnCategory teamFightToogle;

	// Token: 0x0400040B RID: 1035
	public BtnCategory timeBattleToogle;

	// Token: 0x0400040C RID: 1036
	public BtnCategory deadlyGamesToogle;

	// Token: 0x0400040D RID: 1037
	public BtnCategory flagCaptureToogle;

	// Token: 0x0400040E RID: 1038
	public BtnCategory capturePointsToogle;

	// Token: 0x0400040F RID: 1039
	public BtnCategory duelToggle;

	// Token: 0x04000410 RID: 1040
	public bool isStartShowAdvert;

	// Token: 0x04000411 RID: 1041
	private Action actAfterConnectToPhoton;

	// Token: 0x04000412 RID: 1042
	private GameInfo[] roomFields;

	// Token: 0x04000413 RID: 1043
	public UIWrapContent wrapGames;

	// Token: 0x04000414 RID: 1044
	public UIScrollView scrollGames;

	// Token: 0x04000415 RID: 1045
	public static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>> mapStatistics = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, int>>>>();

	// Token: 0x04000416 RID: 1046
	public static string selectedMap = string.Empty;

	// Token: 0x04000417 RID: 1047
	public static bool directedFromQuests = false;

	// Token: 0x04000418 RID: 1048
	public GameObject modeAnimObj;

	// Token: 0x04000419 RID: 1049
	public GameObject finger;

	// Token: 0x0400041A RID: 1050
	public BtnCategory[] modeButtonByLevel;

	// Token: 0x0400041B RID: 1051
	public UILabel[] modeUnlockLabelByLevel;

	// Token: 0x0400041C RID: 1052
	public BtnCategory modeButtonDuel;

	// Token: 0x0400041D RID: 1053
	public UILabel modeDuelRatingNeedLabel;

	// Token: 0x0400041E RID: 1054
	public GameObject modeDuelRatingNeed;

	// Token: 0x0400041F RID: 1055
	private bool fingerStopped;

	// Token: 0x04000420 RID: 1056
	private bool animationStarted;

	// Token: 0x04000421 RID: 1057
	private bool _stopFingerAnimation;

	// Token: 0x04000422 RID: 1058
	private bool loadReplaceAdmobPerelivRunning;

	// Token: 0x04000423 RID: 1059
	private bool loadAdmobRunning;

	// Token: 0x04000424 RID: 1060
	private int _countOfLoopsRequestAdThisTime;

	// Token: 0x04000425 RID: 1061
	private float _lastTimeInterstitialShown;

	// Token: 0x04000426 RID: 1062
	public static bool NeedShowReviewInConnectScene = false;

	// Token: 0x04000427 RID: 1063
	public static readonly string mapProperty = "C0";

	// Token: 0x04000428 RID: 1064
	public static readonly string passwordProperty = "C1";

	// Token: 0x04000429 RID: 1065
	public static readonly string platformProperty = "C2";

	// Token: 0x0400042A RID: 1066
	public static readonly string endingProperty = "C3";

	// Token: 0x0400042B RID: 1067
	public static readonly string maxKillProperty = "C4";

	// Token: 0x0400042C RID: 1068
	public static readonly string ABTestProperty = "C5";

	// Token: 0x0400042D RID: 1069
	public static readonly string ABTestEnum = "C6";

	// Token: 0x0400042E RID: 1070
	public static readonly string roomStatusProperty = "Closed";

	// Token: 0x0400042F RID: 1071
	private bool abTestConnect;

	// Token: 0x04000430 RID: 1072
	private int joinNewRoundTries;

	// Token: 0x04000431 RID: 1073
	private int tryJoinRoundMap;

	// Token: 0x04000432 RID: 1074
	private Vector3 startPosNameServerNameInput = Vector3.zero;

	// Token: 0x04000433 RID: 1075
	private IDisposable _someWindowSubscription;

	// Token: 0x04000434 RID: 1076
	private int _tempMinValue = 3;

	// Token: 0x04000435 RID: 1077
	private int _tempMaxValue = 7;

	// Token: 0x04000436 RID: 1078
	private int _tempStep = 2;

	// Token: 0x04000437 RID: 1079
	private int daterStep = 5;

	// Token: 0x04000438 RID: 1080
	private int daterMinValue = 5;

	// Token: 0x04000439 RID: 1081
	private int daterMaxValue = 10;

	// Token: 0x0400043A RID: 1082
	private IDisposable _backSubscription;

	// Token: 0x0400043B RID: 1083
	private int countNote = 1;

	// Token: 0x0400043C RID: 1084
	private bool isFirstUpdateLocalServerList;

	// Token: 0x0400043D RID: 1085
	private string _logCache = string.Empty;

	// Token: 0x0400043E RID: 1086
	private float startPosX;

	// Token: 0x0400043F RID: 1087
	private int maxcount = 1;

	// Token: 0x04000440 RID: 1088
	private bool isFirstGamesReposition;

	// Token: 0x04000441 RID: 1089
	private int countColumn = 3;

	// Token: 0x04000442 RID: 1090
	private float _widthCell = 282f;

	// Token: 0x04000443 RID: 1091
	private float _heightCell = 1f;

	// Token: 0x04000444 RID: 1092
	private float _scale = 1f;

	// Token: 0x04000445 RID: 1093
	private float borderWidth = 10f;

	// Token: 0x04000446 RID: 1094
	private LoadingNGUIController _loadingNGUIController;

	// Token: 0x04000447 RID: 1095
	private LANBroadcastService.ReceivedMessage[] _copy;

	// Token: 0x04000448 RID: 1096
	private Vector3 posSelectMapPanelInMainMenu = Vector3.up * 10000f;

	// Token: 0x02000078 RID: 120
	public enum PlatformConnect
	{
		// Token: 0x0400044E RID: 1102
		ios = 1,
		// Token: 0x0400044F RID: 1103
		android,
		// Token: 0x04000450 RID: 1104
		custom
	}

	// Token: 0x02000079 RID: 121
	public enum RegimGame
	{
		// Token: 0x04000452 RID: 1106
		Deathmatch,
		// Token: 0x04000453 RID: 1107
		TimeBattle,
		// Token: 0x04000454 RID: 1108
		TeamFight,
		// Token: 0x04000455 RID: 1109
		DeadlyGames,
		// Token: 0x04000456 RID: 1110
		FlagCapture,
		// Token: 0x04000457 RID: 1111
		CapturePoints,
		// Token: 0x04000458 RID: 1112
		InFriendWindow,
		// Token: 0x04000459 RID: 1113
		InClanWindow,
		// Token: 0x0400045A RID: 1114
		Duel
	}

	// Token: 0x0200007A RID: 122
	public struct infoServer
	{
		// Token: 0x0400045B RID: 1115
		public string ipAddress;

		// Token: 0x0400045C RID: 1116
		public int port;

		// Token: 0x0400045D RID: 1117
		public string name;

		// Token: 0x0400045E RID: 1118
		public string map;

		// Token: 0x0400045F RID: 1119
		public int playerLimit;

		// Token: 0x04000460 RID: 1120
		public int connectedPlayers;

		// Token: 0x04000461 RID: 1121
		public string coments;
	}

	// Token: 0x0200007B RID: 123
	public struct infoClient
	{
		// Token: 0x04000462 RID: 1122
		public string ipAddress;

		// Token: 0x04000463 RID: 1123
		public string name;

		// Token: 0x04000464 RID: 1124
		public string coments;
	}

	// Token: 0x0200007C RID: 124
	public enum ABTestParams
	{
		// Token: 0x04000466 RID: 1126
		Old = 1,
		// Token: 0x04000467 RID: 1127
		Rating,
		// Token: 0x04000468 RID: 1128
		Buff
	}
}
