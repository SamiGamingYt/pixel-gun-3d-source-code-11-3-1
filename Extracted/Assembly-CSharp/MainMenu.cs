using System;
using System.Collections;
using Prime31;
using Rilisoft;
using UnityEngine;

// Token: 0x020002FE RID: 766
public sealed class MainMenu : MonoBehaviour
{
	// Token: 0x170004AF RID: 1199
	// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0006B590 File Offset: 0x00069790
	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	// Token: 0x170004B0 RID: 1200
	// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0006B5A4 File Offset: 0x000697A4
	public static string RateUsURL
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return Defs2.ApplicationUrl;
		}
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x0006B5D4 File Offset: 0x000697D4
	public static string GetEndermanUrl()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.isEditor)
		{
			return "https://itunes.apple.com/app/apple-store/id811995374?pt=1579002&ct=pgapp&mt=8-";
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://play.google.com/store/apps/details?id=com.slender.android" : "http://www.amazon.com/Pocket-Slenderman-Rising-your-virtual/dp/B00I6IXU5A/ref=sr_1_5?s=mobile-apps&ie=UTF8&qid=1395990920&sr=1-5&keywords=slendy";
		}
		return string.Empty;
	}

	// Token: 0x06001A7F RID: 6783 RVA: 0x0006B628 File Offset: 0x00069828
	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x0006B644 File Offset: 0x00069844
	private void Awake()
	{
		Defs.isDaterRegim = false;
		if (MainMenu.firstEnterLobbyAtThisLaunch)
		{
			MainMenu.firstEnterLobbyAtThisLaunch = false;
			GlobalGameController.SetMultiMode();
		}
		else
		{
			using (new StopwatchLogger("MainMenu.Awake()"))
			{
				GlobalGameController.SetMultiMode();
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.Reset(0);
				}
				else if (!WeaponManager.sharedManager && this.weaponManagerPrefab)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(this.weaponManagerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					gameObject.GetComponent<WeaponManager>().Reset(0);
				}
			}
		}
	}

	// Token: 0x06001A81 RID: 6785 RVA: 0x0006B718 File Offset: 0x00069918
	private IEnumerator WaitForExperienceGuiAndAdd(ExperienceController legacyExperienceController, int addend)
	{
		while (ExpController.Instance == null)
		{
			yield return null;
		}
		legacyExperienceController.addExperience(addend);
		yield break;
	}

	// Token: 0x06001A82 RID: 6786 RVA: 0x0006B748 File Offset: 0x00069948
	private void Start()
	{
		using (new StopwatchLogger("MainMenu.Start()"))
		{
			MainMenu.sharedMenu = this;
			StoreKitEventListener.State.Mode = "In_main_menu";
			StoreKitEventListener.State.PurchaseKey = "In shop";
			StoreKitEventListener.State.Parameters.Clear();
			if (!FriendsController.sharedController.dataSent)
			{
				FriendsController.sharedController.InitOurInfo();
				FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.WaitForReadyToOperateAndUpdatePlayer());
				FriendsController.sharedController.dataSent = true;
			}
			if (NotificationController.isGetEveryDayMoney)
			{
				this.isShowAvard = true;
			}
			bool flag = false;
			this.expController = ExperienceController.sharedController;
			if (this.expController == null)
			{
				Debug.LogError("MainMenu.Start():    expController == null");
			}
			if (this.expController != null)
			{
				this.expController.isMenu = true;
			}
			float coef = Defs.Coef;
			if (this.expController != null)
			{
				this.expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
			}
			string @string = PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, string.Empty);
			if (@string.Equals(Defs.GoToProfileAction))
			{
				PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, string.Empty);
				PlayerPrefs.Save();
			}
			Storager.setInt(Defs.EarnedCoins, 0, false);
			base.Invoke("setEnabledGUI", 0.1f);
			ActivityIndicator.IsActiveIndicator = true;
			PlayerPrefs.SetInt("typeConnect__", -1);
			if (!GameObject.FindGameObjectWithTag("SkinsManager") && this.skinsManagerPrefab)
			{
				UnityEngine.Object.Instantiate(this.skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			if (!WeaponManager.sharedManager && this.weaponManagerPrefab)
			{
				UnityEngine.Object.Instantiate(this.weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			GlobalGameController.ResetParameters();
			GlobalGameController.Score = 0;
			bool flag2 = false;
			if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
			{
				flag2 = true;
				PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
				PlayerPrefs.Save();
			}
			if (Tools.RuntimePlatform != RuntimePlatform.MetroPlayerX64 && (Application.platform != RuntimePlatform.Android || Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) && Defs.EnderManAvailable && !flag2 && !flag && !this.isShowAvard && PlayerPrefs.GetInt(Defs.ShowEnder_SN, 0) == 1)
			{
				float @float = PlayerPrefs.GetFloat(Defs.TimeFromWhichShowEnder_SN, 0f);
				float num = Switcher.SecondsFrom1970() - @float;
				Debug.Log("diff mainmenu: " + num);
				if (num >= ((!Application.isEditor && !Debug.isDebugBuild) ? 86400f : 0f))
				{
					PlayerPrefs.SetInt(Defs.ShowEnder_SN, 0);
					UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Ender") as GameObject);
				}
			}
		}
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x0006BA60 File Offset: 0x00069C60
	private void SetInApp()
	{
		this.isInappWinOpen = !this.isInappWinOpen;
		if (this.expController != null)
		{
			this.expController.isShowRanks = !this.isInappWinOpen;
			this.expController.isMenu = !this.isInappWinOpen;
		}
		if (this.isInappWinOpen)
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!Defs.isMulti)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0006BAF0 File Offset: 0x00069CF0
	public static bool SkinsMakerSupproted()
	{
		bool result = BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			result = (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite);
		}
		return result;
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0006BB24 File Offset: 0x00069D24
	private void Update()
	{
		float num = ((float)Screen.width - 42f * Defs.Coef - Defs.Coef * (672f + (float)((!MainMenu.SkinsMakerSupproted()) ? 0 : 262))) / ((!MainMenu.SkinsMakerSupproted()) ? 2f : 3f);
		if (this.expController != null)
		{
			this.expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0006BBB8 File Offset: 0x00069DB8
	private void OnDestroy()
	{
		MainMenu.sharedMenu = null;
		if (this.expController != null)
		{
			this.expController.isShowRanks = false;
			this.expController.isMenu = false;
		}
	}

	// Token: 0x170004B1 RID: 1201
	// (get) Token: 0x06001A87 RID: 6791 RVA: 0x0006BBEC File Offset: 0x00069DEC
	public static float iOSVersion
	{
		get
		{
			float result = -1f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				string text = SystemInfo.operatingSystem.Replace("iPhone OS ", string.Empty);
				float.TryParse(text.Substring(0, 1), out result);
			}
			return result;
		}
	}

	// Token: 0x04000F98 RID: 3992
	public static MainMenu sharedMenu;

	// Token: 0x04000F99 RID: 3993
	public GameObject JoysticksUIRoot;

	// Token: 0x04000F9A RID: 3994
	public static bool BlockInterface;

	// Token: 0x04000F9B RID: 3995
	public static bool IsAdvertRun;

	// Token: 0x04000F9C RID: 3996
	private bool isShowDeadMatch;

	// Token: 0x04000F9D RID: 3997
	private bool isShowCOOP;

	// Token: 0x04000F9E RID: 3998
	public bool isFirstFrame = true;

	// Token: 0x04000F9F RID: 3999
	public bool isInappWinOpen;

	// Token: 0x04000FA0 RID: 4000
	private bool musicOld;

	// Token: 0x04000FA1 RID: 4001
	private bool fxOld;

	// Token: 0x04000FA2 RID: 4002
	public Texture inAppFon;

	// Token: 0x04000FA3 RID: 4003
	public GUIStyle puliInApp;

	// Token: 0x04000FA4 RID: 4004
	public GUIStyle healthInApp;

	// Token: 0x04000FA5 RID: 4005
	public GUIStyle pulemetInApp;

	// Token: 0x04000FA6 RID: 4006
	public GUIStyle crystalSwordInapp;

	// Token: 0x04000FA7 RID: 4007
	public GUIStyle elixirInapp;

	// Token: 0x04000FA8 RID: 4008
	private bool showUnlockDialog;

	// Token: 0x04000FA9 RID: 4009
	private bool isPressFullOnMulty;

	// Token: 0x04000FAA RID: 4010
	private float _timeWhenPurchShown;

	// Token: 0x04000FAB RID: 4011
	public GameObject skinsManagerPrefab;

	// Token: 0x04000FAC RID: 4012
	public GameObject weaponManagerPrefab;

	// Token: 0x04000FAD RID: 4013
	public GUIStyle backBut;

	// Token: 0x04000FAE RID: 4014
	private ExperienceController expController;

	// Token: 0x04000FAF RID: 4015
	private AdvertisementController _advertisementController;

	// Token: 0x04000FB0 RID: 4016
	public bool isShowAvard;

	// Token: 0x04000FB1 RID: 4017
	public static readonly string iTunesEnderManID = "811995374";

	// Token: 0x04000FB2 RID: 4018
	private static bool firstEnterLobbyAtThisLaunch = true;

	// Token: 0x04000FB3 RID: 4019
	private bool _skinsMakerQuerySucceeded;
}
