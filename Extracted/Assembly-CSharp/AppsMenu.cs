using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200001B RID: 27
[Obfuscation(Exclude = true)]
internal sealed class AppsMenu : MonoBehaviour
{
	// Token: 0x0600006C RID: 108 RVA: 0x00004B28 File Offset: 0x00002D28
	internal IEnumerable<float> AppsMenuAwakeCoroutine()
	{
		yield return 0.1f;
		Device.isPixelGunLow = Device.isPixelGunLowDevice;
		Application.targetFrameRate = ((!GlobalGameController.is60FPSEnable) ? 30 : 60);
		AppsMenu._startFrameIndex = Time.frameCount;
		yield return 0.2f;
		if (!Launcher.UsingNewLauncher)
		{
			AppsMenu.m_Material = this.fadeMaterial;
		}
		if ((float)Screen.width / (float)Screen.height > 1.7777778f)
		{
			Screen.SetResolution(Mathf.RoundToInt((float)Screen.height * 16f / 9f), Mathf.RoundToInt((float)Screen.height), false);
		}
		yield break;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00004B4C File Offset: 0x00002D4C
	private static IEnumerator MeetTheCoroutine(string sceneName, long abuseTicks, long nowTicks)
	{
		TimeSpan timespan = TimeSpan.FromTicks(Math.Abs(nowTicks - abuseTicks));
		if (Defs.IsDeveloperBuild)
		{
			if (timespan.TotalMinutes < 3.0)
			{
				yield break;
			}
		}
		else if (timespan.TotalDays < 1.0)
		{
			yield break;
		}
		System.Random prng = new System.Random(nowTicks.GetHashCode());
		float delaySeconds = (float)prng.Next(15, 60);
		yield return new WaitForSeconds(delaySeconds);
		SceneManager.LoadScene(sceneName);
		yield break;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00004B8C File Offset: 0x00002D8C
	private static string GetAbuseKey_53232de5(uint pad)
	{
		uint num = 2546556124U ^ pad;
		AppsMenu._preventInlining += 1U;
		return num.ToString("x");
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004BC0 File Offset: 0x00002DC0
	private static string GetAbuseKey_21493d18(uint pad)
	{
		uint num = 3852684321U ^ pad;
		AppsMenu._preventInlining += 1U;
		return num.ToString("x");
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004BF4 File Offset: 0x00002DF4
	private static string GetTerminalSceneName_4de1(uint gamma)
	{
		return "Closing4de1Scene".Replace(gamma.ToString("x"), string.Empty);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00004C14 File Offset: 0x00002E14
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(Application.Quit), "AppsMenu");
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00004C50 File Offset: 0x00002E50
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000073 RID: 115 RVA: 0x00004C70 File Offset: 0x00002E70
	internal static bool ApplicationBinarySplitted
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004C74 File Offset: 0x00002E74
	private void Awake()
	{
		LogsManager.Initialize();
		WeaponManager.FirstTagForOurTier(WeaponTags.PistolTag, null);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			foreach (GadgetInfo.GadgetCategory gadgetCategory in Enum.GetValues(typeof(GadgetInfo.GadgetCategory)).OfType<GadgetInfo.GadgetCategory>())
			{
				if (gadgetCategory != GadgetInfo.GadgetCategory.Throwing)
				{
					if (!Storager.hasKey(GadgetsInfo.SNForCategory(gadgetCategory)))
					{
						Storager.setString(GadgetsInfo.SNForCategory(gadgetCategory), string.Empty, false);
					}
				}
			}
		}
		if (!Storager.hasKey(GadgetsInfo.SNForCategory(GadgetInfo.GadgetCategory.Throwing)))
		{
			Storager.setString(GadgetsInfo.SNForCategory(GadgetInfo.GadgetCategory.Throwing), "gadget_fraggrenade", false);
			GadgetsInfo.ProvideGadget("gadget_fraggrenade");
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && PlayerPrefs.GetInt("shop_tutorial_state_passed_VER_12_1", 0) >= 9)
		{
			TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShopCompleted;
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Back_Shop, 0);
		}
		this.currentFon = this.riliFon;
		if (ActivityIndicator.instance == null && this.activityIndikatorPrefab != null)
		{
			UnityEngine.Object target = UnityEngine.Object.Instantiate<GameObject>(this.activityIndikatorPrefab);
			UnityEngine.Object.DontDestroyOnLoad(target);
		}
		ActivityIndicator.SetLoadingFon(this.currentFon);
		ActivityIndicator.IsShowWindowLoading = true;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004DE4 File Offset: 0x00002FE4
	private IEnumerator Start()
	{
		yield return null;
		Switcher.timer.Start();
		if (Defs.IsDeveloperBuild && Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			StringBuilder message = new StringBuilder("[Rilisoft] Trying to instantiate `android.os.AsyncTask`... ");
			try
			{
				using (new AndroidJavaClass("android.os.AsyncTask"))
				{
					message.Append("Done.");
				}
			}
			catch (Exception ex3)
			{
				Exception ex = ex3;
				message.Append("Failed.");
				Debug.LogException(ex);
			}
			Debug.Log(message.ToString());
		}
		yield return null;
		if (!Storager.hasKey(Defs.PremiumEnabledFromServer))
		{
			Storager.setInt(Defs.PremiumEnabledFromServer, 0, false);
		}
		ActivityIndicator.IsActiveIndicator = false;
		foreach (float num in this.AppsMenuAwakeCoroutine())
		{
			float step = num;
			Switcher.timer.Reset();
			Switcher.timer.Start();
			yield return null;
			this._preventAggressiveOptimisation = step;
		}
		Switcher.timer.Reset();
		Switcher.timer.Start();
		if (Launcher.UsingNewLauncher)
		{
			yield break;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Action<string> handle = delegate(string sceneName)
				{
					if (Application.platform != RuntimePlatform.Android)
					{
						return;
					}
					if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
					{
						return;
					}
					string abuseKey_21493d = AppsMenu.GetAbuseKey_21493d18(558447896U);
					long num2 = DateTime.UtcNow.Ticks >> 1;
					long num3 = num2;
					if (!Storager.hasKey(abuseKey_21493d))
					{
						Storager.setString(abuseKey_21493d, num2.ToString(), false);
					}
					else if (long.TryParse(Storager.getString(abuseKey_21493d, false), out num3))
					{
						Storager.setString(abuseKey_21493d, Math.Min(num2, num3).ToString(), false);
					}
					else
					{
						Storager.setString(abuseKey_21493d, num2.ToString(), false);
					}
					CoroutineRunner.Instance.StartCoroutine(AppsMenu.MeetTheCoroutine(sceneName, num3 << 1, num2 << 1));
				};
				LicenseVerificationController.PackageInfo actualPackageInfo = default(LicenseVerificationController.PackageInfo);
				try
				{
					actualPackageInfo = LicenseVerificationController.GetPackageInfo();
					Launcher.PackageInfo = new LicenseVerificationController.PackageInfo?(actualPackageInfo);
				}
				catch (Exception ex4)
				{
					Exception ex2 = ex4;
					Debug.Log("LicenseVerificationController.GetPackageInfo() failed:    " + ex2);
					handle(AppsMenu.GetTerminalSceneName_4de1(19937U));
				}
				finally
				{
					if (actualPackageInfo.SignatureHash == null)
					{
						Debug.Log("actualPackageInfo.SignatureHash == null");
						handle(AppsMenu.GetTerminalSceneName_4de1(19937U));
					}
				}
				string actualPackageName = actualPackageInfo.PackageName;
				if (string.Compare(actualPackageName, Defs.GetIntendedAndroidPackageName(), StringComparison.Ordinal) != 0)
				{
					Debug.LogWarning("Verification FakeBundleDetected:    " + actualPackageName);
					handle(AppsMenu.GetTerminalSceneName_4de1(19937U));
				}
				else
				{
					Debug.Log("Package check passed.");
				}
				if (string.IsNullOrEmpty(this.intendedSignatureHash))
				{
					Debug.LogWarning("String.IsNullOrEmpty(intendedSignatureHash)");
					handle(AppsMenu.GetTerminalSceneName_4de1(19937U));
				}
				string actualSignatureHash = actualPackageInfo.SignatureHash;
				if (string.Compare(actualSignatureHash, this.intendedSignatureHash, StringComparison.Ordinal) != 0)
				{
					Debug.LogWarning("Verification FakeSignatureDetected:    " + actualSignatureHash);
					Switcher.AppendAbuseMethod(AbuseMetod.AndroidPackageSignature);
					handle(AppsMenu.GetTerminalSceneName_4de1(19937U));
				}
				else
				{
					Debug.Log("Signature check passed.");
				}
			}
		}
		if (!Application.isEditor && AppsMenu.ApplicationBinarySplitted)
		{
			Debug.LogFormat("Expansion file path: '{0}'", new object[]
			{
				this._expansionFilePath.Value
			});
			string mainPath = GooglePlayDownloader.GetMainOBBPath(this._expansionFilePath.Value);
			if (mainPath == null)
			{
				if (this._fetchObbPromise != null)
				{
					this._fetchObbPromise.TrySetCanceled();
				}
				this._fetchObbPromise = new TaskCompletionSource<string>();
				Debug.LogWarning("Waiting mainPath...");
				if (!this._storagePermissionRequested)
				{
					this._storagePermissionRequested = true;
					NoodlePermissionGranter.PermissionRequestCallback = new Action<bool>(this.HandleStoragePermissionDialog);
					NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.WRITE_EXTERNAL_STORAGE);
				}
				while (!this.StoragePermissionFuture.IsCompleted)
				{
					yield return null;
				}
				if (!this.StoragePermissionFuture.Result)
				{
					Application.Quit();
					yield break;
				}
				GooglePlayDownloader.FetchOBB();
				for (;;)
				{
					mainPath = GooglePlayDownloader.GetMainOBBPath(this._expansionFilePath.Value);
					if (mainPath != null)
					{
						break;
					}
					yield return new WaitForRealSeconds(0.5f);
				}
				this._fetchObbPromise.TrySetResult(mainPath);
				Debug.LogFormat("Main path: '{0}'", new object[]
				{
					mainPath
				});
			}
			else
			{
				Debug.LogFormat("OBB already exists: '{0}'", new object[]
				{
					mainPath
				});
			}
		}
		yield return null;
		NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.ACCESS_COARSE_LOCATION);
		base.StartCoroutine(this.Fade(1f, 1f));
		AppsMenu.SetCurrentLanguage();
		yield break;
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000076 RID: 118 RVA: 0x00004E00 File Offset: 0x00003000
	private Task<bool> StoragePermissionFuture
	{
		get
		{
			return this._storagePermissionGrantedPromise.Task;
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004E10 File Offset: 0x00003010
	private void HandleStoragePermissionDialog(bool permissionGranted)
	{
		this._storagePermissionGrantedPromise.TrySetResult(permissionGranted);
		NoodlePermissionGranter.PermissionRequestCallback = null;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00004E28 File Offset: 0x00003028
	private IEnumerator OnApplicationPause(bool pause)
	{
		bool fetchingObb = this.FetchObbFuture != null && !this.FetchObbFuture.IsCompleted;
		Debug.LogFormat("AppsMenu pause: {0}; fetching OBB: {1}", new object[]
		{
			pause,
			fetchingObb
		});
		if (pause)
		{
			yield break;
		}
		if (this.FetchObbFuture == null)
		{
			yield break;
		}
		if (this.FetchObbFuture.IsCompleted && !this.FetchObbFuture.IsFaulted && !this.FetchObbFuture.IsCanceled && !string.IsNullOrEmpty(this.FetchObbFuture.Result))
		{
			yield break;
		}
		if (this._fetchObbPromise != null)
		{
			this._fetchObbPromise.TrySetCanceled();
		}
		this._fetchObbPromise = new TaskCompletionSource<string>();
		if (this.StoragePermissionFuture.IsCompleted)
		{
			if (!this.StoragePermissionFuture.Result)
			{
				Application.Quit();
			}
			yield break;
		}
		if (!this._storagePermissionRequested)
		{
			this._storagePermissionRequested = true;
			NoodlePermissionGranter.PermissionRequestCallback = new Action<bool>(this.HandleStoragePermissionDialog);
			NoodlePermissionGranter.GrantPermission(NoodlePermissionGranter.NoodleAndroidPermission.WRITE_EXTERNAL_STORAGE);
		}
		while (!this.StoragePermissionFuture.IsCompleted)
		{
			yield return null;
		}
		if (!this.StoragePermissionFuture.Result)
		{
			Application.Quit();
			yield break;
		}
		GooglePlayDownloader.FetchOBB();
		yield break;
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00004E54 File Offset: 0x00003054
	private Task<string> FetchObbFuture
	{
		get
		{
			if (this._fetchObbPromise == null)
			{
				return null;
			}
			return this._fetchObbPromise.Task;
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004E70 File Offset: 0x00003070
	private static void CheckRenameOldLanguageName()
	{
		if (Storager.IsInitialized(Defs.ChangeOldLanguageName))
		{
			return;
		}
		Storager.SetInitialized(Defs.ChangeOldLanguageName);
		string @string = PlayerPrefs.GetString(Defs.CurrentLanguage, string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return;
		}
		string text = @string;
		switch (text)
		{
		case "Français":
			PlayerPrefs.SetString(Defs.CurrentLanguage, "French");
			PlayerPrefs.Save();
			break;
		case "Deutsch":
			PlayerPrefs.SetString(Defs.CurrentLanguage, "German");
			PlayerPrefs.Save();
			break;
		case "日本人":
			PlayerPrefs.SetString(Defs.CurrentLanguage, "Japanese");
			PlayerPrefs.Save();
			break;
		case "Español":
			PlayerPrefs.SetString(Defs.CurrentLanguage, "Spanish");
			PlayerPrefs.Save();
			break;
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004F94 File Offset: 0x00003194
	internal static void SetCurrentLanguage()
	{
		AppsMenu.CheckRenameOldLanguageName();
		string text = PlayerPrefs.GetString(Defs.CurrentLanguage);
		if (string.IsNullOrEmpty(text))
		{
			text = LocalizationStore.CurrentLanguage;
			return;
		}
		LocalizationStore.CurrentLanguage = text;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00004FCC File Offset: 0x000031CC
	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		Debug.LogFormat("GameThrive HandleNotification('{0}', ..., {1})", new object[]
		{
			message,
			isActive
		});
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004FEC File Offset: 0x000031EC
	private void LoadLoading()
	{
		Switcher.timer.Reset();
		Switcher.timer.Start();
		GlobalGameController.currentLevel = -1;
		SceneManager.LoadSceneAsync("Loading");
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00005014 File Offset: 0x00003214
	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		if (AppsMenu.m_Material != null && AppsMenu.m_Material.SetPass(0))
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(aColor);
			GL.Vertex3(0f, 0f, -1f);
			GL.Vertex3(0f, 1f, -1f);
			GL.Vertex3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, -1f);
			GL.End();
			GL.PopMatrix();
		}
		else
		{
			Debug.LogWarning("Couldnot set pass for material.");
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000050C8 File Offset: 0x000032C8
	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime)
	{
		Color aColor = Color.black;
		for (float t = 0f; t < aFadeOutTime; t += Time.deltaTime)
		{
			float alpha = Mathf.InverseLerp(0f, aFadeOutTime, t);
			this.DrawQuad(aColor, alpha);
			yield return null;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.currentFon = this.commicsFon;
			if (ActivityIndicator.instance != null)
			{
				ActivityIndicator.instance.legendLabel.gameObject.SetActive(true);
				string legendLocalization = LocalizationStore.Get("Key_1925");
				bool localizationFailed = "Key_1925" == legendLocalization;
				string legendText = (!localizationFailed) ? legendLocalization : "PLEASE REBOOT YOUR DEVICE IF FROZEN.";
				if (localizationFailed)
				{
					ActivityIndicator.instance.legendLabel.effectStyle = UILabel.Effect.None;
					ActivityIndicator.instance.legendLabel.color = Color.white;
				}
				ActivityIndicator.instance.legendLabel.text = legendText;
			}
			else
			{
				Debug.LogWarning("ActivityIndicator.instance is null.");
			}
			ActivityIndicator.IsActiveIndicator = false;
		}
		else
		{
			this.currentFon = this.androidFon;
			ActivityIndicator.IsActiveIndicator = true;
		}
		ActivityIndicator.SetLoadingFon(this.currentFon);
		for (float t2 = 0f; t2 < aFadeInTime; t2 += Time.deltaTime)
		{
			float alpha2 = Mathf.InverseLerp(0f, aFadeInTime, t2);
			this.DrawQuad(aColor, 1f - alpha2);
			yield return null;
		}
		this.LoadLoading();
		yield break;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00005100 File Offset: 0x00003300
	public static Rect RiliFonRect()
	{
		float num = (float)Screen.height * 1.7766234f;
		return new Rect((float)Screen.width / 2f - num / 2f, 0f, num, (float)Screen.height);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00005140 File Offset: 0x00003340
	private void OnGUI()
	{
		if (Launcher.UsingNewLauncher)
		{
			return;
		}
		if (!Application.isEditor && !GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00005194 File Offset: 0x00003394
	private IEnumerator LoadLoadingScene()
	{
		yield return new WaitForRealSeconds(0.5f);
		Singleton<SceneLoader>.Instance.LoadScene("Loading", LoadSceneMode.Single);
		yield break;
	}

	// Token: 0x04000062 RID: 98
	private const string _suffix = "Scene";

	// Token: 0x04000063 RID: 99
	public Texture androidFon;

	// Token: 0x04000064 RID: 100
	public Texture riliFon;

	// Token: 0x04000065 RID: 101
	public Texture commicsFon;

	// Token: 0x04000066 RID: 102
	public Material fadeMaterial;

	// Token: 0x04000067 RID: 103
	public GameObject activityIndikatorPrefab;

	// Token: 0x04000068 RID: 104
	public string intendedSignatureHash;

	// Token: 0x04000069 RID: 105
	private Texture currentFon;

	// Token: 0x0400006A RID: 106
	private static Material m_Material;

	// Token: 0x0400006B RID: 107
	private static int _startFrameIndex;

	// Token: 0x0400006C RID: 108
	internal volatile object _preventAggressiveOptimisation;

	// Token: 0x0400006D RID: 109
	private static volatile uint _preventInlining = 3565584061U;

	// Token: 0x0400006E RID: 110
	private IDisposable _backSubscription;

	// Token: 0x0400006F RID: 111
	private Lazy<string> _expansionFilePath = new Lazy<string>(new Func<string>(GooglePlayDownloader.GetExpansionFilePath));

	// Token: 0x04000070 RID: 112
	private readonly TaskCompletionSource<bool> _storagePermissionGrantedPromise = new TaskCompletionSource<bool>();

	// Token: 0x04000071 RID: 113
	private bool _storagePermissionRequested;

	// Token: 0x04000072 RID: 114
	private TaskCompletionSource<string> _fetchObbPromise;
}
