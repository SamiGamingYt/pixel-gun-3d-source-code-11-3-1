using System;
using System.Collections;
using System.Reflection;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002DA RID: 730
public sealed class JoinRoomFromFrends : MonoBehaviour
{
	// Token: 0x0600197D RID: 6525 RVA: 0x0006580C File Offset: 0x00063A0C
	private void Start()
	{
		JoinRoomFromFrends.sharedJoinRoomFromFrends = this;
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x00065814 File Offset: 0x00063A14
	private void OnEnable()
	{
		PhotonObjectCacher.AddObject(base.gameObject);
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x00065824 File Offset: 0x00063A24
	private void OnEsc()
	{
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		this.closeConnectPanel();
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x00065838 File Offset: 0x00063A38
	private void OnDisable()
	{
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x00065848 File Offset: 0x00063A48
	private void OnDestroy()
	{
		JoinRoomFromFrends.sharedJoinRoomFromFrends = null;
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x0006585C File Offset: 0x00063A5C
	public void BackFromPasswordButton()
	{
		this.isBackFromPassword = true;
		this.SetEnabledPasswordPanel(false);
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		Debug.Log("BackFromPasswordButton");
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x00065884 File Offset: 0x00063A84
	public void EnterPassword(string pass)
	{
		if (pass == this.passwordRoom)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			base.StartCoroutine(this.MoveToGameScene());
			ActivityIndicator.IsActiveIndicator = true;
		}
		else
		{
			this.timerShowWrongPassword = 3f;
			this.WrongPasswordLabel.SetActive(true);
		}
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000658D8 File Offset: 0x00063AD8
	private void ShowLoadingGUI(string _mapName)
	{
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = _mapName;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = Resources.Load<Texture2D>("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + _mapName);
		this._loadingNGUIController.transform.parent = this.fonConnectTexture.transform.parent;
		this._loadingNGUIController.transform.localPosition = Vector3.zero;
		this._loadingNGUIController.Init();
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x0006598C File Offset: 0x00063B8C
	private void RemoveLoadingGUI()
	{
		if (this._loadingNGUIController != null)
		{
			UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
			this._loadingNGUIController = null;
		}
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x000659C4 File Offset: 0x00063BC4
	private IEnumerator SetFonLoadingWaitForReset(string _mapName = "", bool isAddCountRun = false)
	{
		this.RemoveLoadingGUI();
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

	// Token: 0x06001987 RID: 6535 RVA: 0x000659F0 File Offset: 0x00063BF0
	public void ConnectToRoom(int _game_mode, string _room_name, string _map)
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.OnEsc), "Connect To Friend");
		InfoWindowController.HideCurrentWindow();
		this.SetEnabledPasswordPanel(false);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(_map));
		bool isPremium = infoScene.isPremium;
		if (isPremium && Storager.getInt(infoScene.NameScene + "Key", true) != 1 && !PremiumAccountController.MapAvailableDueToPremiumAccount(infoScene.NameScene))
		{
			if (this.objectForOffWhenUlockDialog != null)
			{
				this.objectForOffWhenUlockDialog.SetActive(false);
			}
			Action successfulUnlockCallback = delegate()
			{
			};
			this.ShowUnlockMapDialog(successfulUnlockCallback, infoScene.NameScene);
			return;
		}
		int gameTier = (_game_mode <= 99) ? (_game_mode / 10) : (_game_mode % 100 / 10);
		this.game_mode = _game_mode % 10;
		this.room_name = _room_name;
		Defs.isMulti = true;
		Defs.isInet = true;
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isCapturePoints = false;
		Defs.isDuel = false;
		switch (this.game_mode)
		{
		case 0:
			StoreKitEventListener.State.Mode = "Deathmatch Wordwide";
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Deathmatch;
			goto IL_281;
		case 1:
			StoreKitEventListener.State.Mode = "Time Survival";
			Defs.isCOOP = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TimeBattle;
			goto IL_281;
		case 2:
			StoreKitEventListener.State.Mode = "Team Battle";
			Defs.isCompany = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.TeamFight;
			goto IL_281;
		case 3:
		{
			bool flag = true;
			if (flag)
			{
				Defs.isHunger = true;
				StoreKitEventListener.State.Mode = "Deadly Games";
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.DeadlyGames;
				goto IL_281;
			}
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(3);
			}
			return;
		}
		case 4:
		{
			bool flag2 = true;
			if (flag2)
			{
				Defs.isFlag = true;
				ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.FlagCapture;
				goto IL_281;
			}
			StoreKitEventListener.State.Mode = "Flag Capture";
			if (ShowNoJoinConnectFromRanks.sharedController != null)
			{
				ShowNoJoinConnectFromRanks.sharedController.resetShow(4);
			}
			return;
		}
		case 5:
			Defs.isCapturePoints = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.CapturePoints;
			goto IL_281;
		case 8:
			Defs.isDuel = true;
			ConnectSceneNGUIController.regim = ConnectSceneNGUIController.RegimGame.Duel;
			goto IL_281;
		}
		return;
		IL_281:
		ActivityIndicator.IsActiveIndicator = true;
		this.oldActivFriendPanel = this.friendsPanel.activeSelf;
		if (JoinRoomFromFrends.friendProfilePanel != null)
		{
			this.oldActivProfileProfile = JoinRoomFromFrends.friendProfilePanel.activeSelf;
		}
		this.connectPanel.SetActive(true);
		this.friendsPanel.SetActive(false);
		if (JoinRoomFromFrends.friendProfilePanel != null)
		{
			JoinRoomFromFrends.friendProfilePanel.SetActive(false);
		}
		this.label.gameObject.SetActive(false);
		this.plashkaLabel.SetActive(false);
		Debug.Log("fonConnectTexture.mainTexture=" + _map + " " + infoScene.NameScene);
		Defs.isDaterRegim = (Defs.filterMaps.ContainsKey(infoScene.NameScene) && infoScene.AvaliableWeapon == ModeWeapon.dater);
		WeaponManager.sharedManager.Reset((!Defs.isDaterRegim) ? 0 : 3);
		base.StartCoroutine(this.SetFonLoadingWaitForReset(infoScene.NameScene, false));
		string text = string.Concat(new string[]
		{
			Initializer.Separator,
			ConnectSceneNGUIController.regim.ToString(),
			(!Defs.isDaterRegim) ? ((!Defs.isHunger) ? gameTier.ToString() : "0") : "Dater",
			"v",
			GlobalGameController.MultiplayerProtocolVersion
		});
		ConnectSceneNGUIController.gameTier = gameTier;
		Debug.Log("Connect -" + text);
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.ConnectUsingSettings(text);
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x00065E04 File Offset: 0x00064004
	private void Update()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = (coinsShop.thisScript != null && coinsShop.thisScript.enabled);
		}
		if (this.timerShowWrongPassword > 0f && this.WrongPasswordLabel.activeSelf)
		{
			this.timerShowWrongPassword -= Time.deltaTime;
		}
		if (this.timerShowWrongPassword <= 0f && this.WrongPasswordLabel.activeSelf)
		{
			this.WrongPasswordLabel.SetActive(false);
		}
	}

	// Token: 0x06001989 RID: 6537 RVA: 0x00065EA8 File Offset: 0x000640A8
	private void ShowUnlockMapDialog(Action successfulUnlockCallback, string levelName)
	{
		if (string.IsNullOrEmpty(levelName))
		{
			Debug.LogWarning("Level name shoul not be empty.");
			return;
		}
		UnityEngine.Object original = Resources.Load("UnlockPremiumMapView");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		Tools.SetLayerRecursively(gameObject, base.gameObject.layer);
		ActivityIndicator.IsActiveIndicator = false;
		UnlockPremiumMapView unlockPremiumMapView = gameObject.GetComponent<UnlockPremiumMapView>();
		if (unlockPremiumMapView == null)
		{
			Debug.LogError("UnlockPremiumMapView should not be null.");
			return;
		}
		int price = 0;
		Defs.PremiumMaps.TryGetValue(levelName, out price);
		unlockPremiumMapView.Price = price;
		EventHandler value = delegate(object sender, EventArgs e)
		{
			this.HandleCloseUnlockDialog(unlockPremiumMapView);
		};
		EventHandler value2 = delegate(object sender, EventArgs e)
		{
			this.HandleUnlockPressed(unlockPremiumMapView, successfulUnlockCallback, levelName);
		};
		unlockPremiumMapView.ClosePressed += value;
		unlockPremiumMapView.UnlockPressed += value2;
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x00065FD4 File Offset: 0x000641D4
	private void HandleCloseUnlockDialog(UnlockPremiumMapView unlockPremiumMapView)
	{
		if (this.objectForOffWhenUlockDialog != null)
		{
			this.objectForOffWhenUlockDialog.SetActive(true);
		}
		this.closeConnectPanel();
		UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x00066010 File Offset: 0x00064210
	private void HandleUnlockPressed(UnlockPremiumMapView unlockPremiumMapView, Action successfulUnlockCallback, string levelName)
	{
		int priceAmount = unlockPremiumMapView.Price;
		ShopNGUIController.TryToBuy((!(FriendsWindowGUI.Instance != null)) ? unlockPremiumMapView.gameObject : FriendsWindowGUI.Instance.gameObject, new ItemPrice(unlockPremiumMapView.Price, "Coins"), delegate
		{
			Storager.setInt(levelName + "Key", 1, true);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
			ShopNGUIController.SynchronizeAndroidPurchases("Friend's map unlocked: " + levelName);
			AnalyticsStuff.LogSales(levelName, "Premium Maps", false);
			AnalyticsFacade.InAppPurchase(levelName, "Premium Maps", 1, priceAmount, "Coins");
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			successfulUnlockCallback();
			UnityEngine.Object.Destroy(unlockPremiumMapView.gameObject);
		}, delegate
		{
			StoreKitEventListener.State.PurchaseKey = "In map selection In Friends";
		}, null, null, null, null);
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x000660BC File Offset: 0x000642BC
	[Obfuscation(Exclude = true)]
	public void closeConnectPanel()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
		this.fonConnectTexture.mainTexture = null;
		this.RemoveLoadingGUI();
		this.connectPanel.SetActive(false);
		this.label.gameObject.SetActive(false);
		this.plashkaLabel.SetActive(false);
		this.friendsPanel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x00066134 File Offset: 0x00064334
	private void ShowLabel(string text)
	{
		this.label.text = text;
		this.label.gameObject.SetActive(true);
		this.plashkaLabel.SetActive(true);
		ActivityIndicator.IsActiveIndicator = false;
		base.Invoke("closeConnectPanel", 3f);
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x00066180 File Offset: 0x00064380
	private void OnDisconnectedFromPhoton()
	{
		if (this.isFaledConnectToRoom)
		{
			this.ShowLabel("Game is unavailable...");
		}
		else if (this.isBackFromPassword)
		{
			this.closeConnectPanel();
		}
		else
		{
			this.ShowLabel("Can't connect ...");
		}
		this.isFaledConnectToRoom = false;
		this.isBackFromPassword = false;
		Debug.Log("OnDisconnectedFromPhoton");
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000661E4 File Offset: 0x000643E4
	private void OnFailedToConnectToPhoton(object parameters)
	{
		this.ShowLabel("Can't connect ...");
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x00066204 File Offset: 0x00064404
	public void OnConnectedToMaster()
	{
		this.ConnectToRoom();
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x0006620C File Offset: 0x0006440C
	public void OnJoinedLobby()
	{
		this.ConnectToRoom();
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x00066214 File Offset: 0x00064414
	[Obfuscation(Exclude = true)]
	private void ConnectToRoom()
	{
		PhotonNetwork.playerName = ProfileController.GetPlayerNameOrDefault();
		Debug.Log("OnJoinedLobby " + this.room_name);
		PhotonNetwork.JoinRoom(this.room_name);
		PlayerPrefs.SetString("RoomName", this.room_name);
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x00066254 File Offset: 0x00064454
	private void OnPhotonJoinRoomFailed()
	{
		Debug.Log("OnPhotonJoinRoomFailed - init");
		this.isFaledConnectToRoom = true;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		InfoWindowController.ShowInfoBox(LocalizationStore.Get("Key_0137"));
		InfoWindowController.HideProcessing(3f);
		Defs.isFlag = false;
		Defs.isCOOP = false;
		Defs.isCompany = false;
		Defs.isHunger = false;
		Defs.isDuel = false;
		Defs.isCapturePoints = false;
		Defs.isDaterRegim = false;
		WeaponManager.sharedManager.Reset(0);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000662CC File Offset: 0x000644CC
	private void SetEnabledPasswordPanel(bool enabled)
	{
		this.PasswordPanel.SetActive(enabled);
		if (this._loadingNGUIController != null)
		{
			this.fonConnectTexture.gameObject.SetActive(enabled);
			this.fonConnectTexture.mainTexture = ((!enabled) ? null : this._loadingNGUIController.loadingNGUITexture.mainTexture);
			this._loadingNGUIController.gameObject.SetActive(!enabled);
		}
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00066344 File Offset: 0x00064544
	private void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom - init");
		GlobalGameController.healthMyPlayer = 0f;
		if (PhotonNetwork.room != null)
		{
			this.passwordRoom = PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].ToString();
			PhotonNetwork.isMessageQueueRunning = false;
			if (this.passwordRoom.Equals(string.Empty))
			{
				PhotonNetwork.isMessageQueueRunning = false;
				base.StartCoroutine(this.MoveToGameScene());
			}
			else
			{
				Debug.Log("Show Password Panel " + this.passwordRoom);
				ActivityIndicator.IsActiveIndicator = false;
				this.inputPassworLabel.value = string.Empty;
				this.SetEnabledPasswordPanel(true);
			}
		}
		else
		{
			PhotonNetwork.isMessageQueueRunning = true;
			PhotonNetwork.Disconnect();
			this.ShowLabel("Game is unavailable...");
		}
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x00066410 File Offset: 0x00064610
	private IEnumerator MoveToGameScene()
	{
		AnalyticsStuff.LogMultiplayer();
		if (SceneLoader.ActiveSceneName.Equals("Clans"))
		{
			Defs.isGameFromFriends = false;
			Defs.isGameFromClans = true;
		}
		else
		{
			Defs.isGameFromFriends = true;
			Defs.isGameFromClans = false;
		}
		SceneInfo scInfo = SceneInfoController.instance.GetInfoScene(int.Parse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString()));
		WeaponManager.sharedManager.Reset((int)((!(scInfo != null)) ? ModeWeapon.all : scInfo.AvaliableWeapon));
		Debug.Log("MoveToGameScene");
		while (PhotonNetwork.room == null)
		{
			yield return 0;
		}
		Debug.Log("map=" + PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty].ToString());
		Debug.Log(scInfo.NameScene);
		LoadConnectScene.textureToShow = (Resources.Load("LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/Loading_" + scInfo.NameScene) as Texture2D);
		LoadingInAfterGame.loadingTexture = LoadConnectScene.textureToShow;
		LoadConnectScene.sceneToLoad = scInfo.NameScene;
		LoadConnectScene.noteToShow = null;
		AsyncOperation async = Singleton<SceneLoader>.Instance.LoadSceneAsync("PromScene", LoadSceneMode.Single);
		yield return async;
		yield break;
	}

	// Token: 0x04000E9D RID: 3741
	public int game_mode;

	// Token: 0x04000E9E RID: 3742
	public string room_name;

	// Token: 0x04000E9F RID: 3743
	public static JoinRoomFromFrends sharedJoinRoomFromFrends;

	// Token: 0x04000EA0 RID: 3744
	public GameObject friendsPanel;

	// Token: 0x04000EA1 RID: 3745
	public GameObject connectPanel;

	// Token: 0x04000EA2 RID: 3746
	public static GameObject friendProfilePanel;

	// Token: 0x04000EA3 RID: 3747
	public UILabel label;

	// Token: 0x04000EA4 RID: 3748
	public GameObject plashkaLabel;

	// Token: 0x04000EA5 RID: 3749
	private bool isFaledConnectToRoom;

	// Token: 0x04000EA6 RID: 3750
	private bool oldActivFriendPanel;

	// Token: 0x04000EA7 RID: 3751
	private bool oldActivProfileProfile;

	// Token: 0x04000EA8 RID: 3752
	public UITexture fonConnectTexture;

	// Token: 0x04000EA9 RID: 3753
	private string passwordRoom;

	// Token: 0x04000EAA RID: 3754
	public GameObject WrongPasswordLabel;

	// Token: 0x04000EAB RID: 3755
	private float timerShowWrongPassword;

	// Token: 0x04000EAC RID: 3756
	public GameObject PasswordPanel;

	// Token: 0x04000EAD RID: 3757
	private bool isBackFromPassword;

	// Token: 0x04000EAE RID: 3758
	public UIInput inputPassworLabel;

	// Token: 0x04000EAF RID: 3759
	public GameObject objectForOffWhenUlockDialog;

	// Token: 0x04000EB0 RID: 3760
	private IDisposable _backSubscription;

	// Token: 0x04000EB1 RID: 3761
	private LoadingNGUIController _loadingNGUIController;
}
