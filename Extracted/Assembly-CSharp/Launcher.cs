using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

// Token: 0x0200067F RID: 1663
internal sealed class Launcher : MonoBehaviour
{
	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x060039CA RID: 14794 RVA: 0x0012BC20 File Offset: 0x00129E20
	// (set) Token: 0x060039CB RID: 14795 RVA: 0x0012BC28 File Offset: 0x00129E28
	internal static LicenseVerificationController.PackageInfo? PackageInfo { get; set; }

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x060039CC RID: 14796 RVA: 0x0012BC30 File Offset: 0x00129E30
	internal static bool UsingNewLauncher
	{
		get
		{
			return Launcher._usingNewLauncher != null && Launcher._usingNewLauncher.Value;
		}
	}

	// Token: 0x060039CD RID: 14797 RVA: 0x0012BC50 File Offset: 0x00129E50
	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			Application.targetFrameRate = 30;
		}
		this._targetFramerate = ((Application.targetFrameRate != -1) ? Mathf.Clamp(Application.targetFrameRate, 30, 60) : 300);
		if (Launcher._usingNewLauncher == null)
		{
			Launcher._usingNewLauncher = new bool?(SceneLoader.ActiveSceneName.Equals("Launcher"));
		}
		if (this.ProgressLabel != null)
		{
			this.ProgressLabel.text = 0f.ToString("P0");
		}
	}

	// Token: 0x060039CE RID: 14798 RVA: 0x0012BCFC File Offset: 0x00129EFC
	private IEnumerable<float> SplashScreenFadeOut()
	{
		if (this.SplashScreen != null)
		{
			int splashScreenFadeOutFrameCount = 1 * this._targetFramerate;
			this.SplashScreen.gameObject.SetActive(true);
			for (int i = 0; i != splashScreenFadeOutFrameCount; i++)
			{
				Color newColor = Color.Lerp(Color.white, Color.black, (float)i / (float)splashScreenFadeOutFrameCount);
				this.SplashScreen.color = newColor;
				yield return 0f;
			}
			this.SplashScreen.color = Color.black;
			yield return 1f;
		}
		yield break;
	}

	// Token: 0x060039CF RID: 14799 RVA: 0x0012BD20 File Offset: 0x00129F20
	private IEnumerable<float> LoadingProgressFadeIn()
	{
		if (this.SplashScreen != null)
		{
			int loadingFadeInFrameCount = 1 * this._targetFramerate;
			Color transparentColor = new Color(0f, 0f, 0f, 0f);
			for (int i = 0; i != loadingFadeInFrameCount; i++)
			{
				float alpha = Mathf.Pow((float)i / (float)loadingFadeInFrameCount, 2.2f);
				Color newColor = Color.Lerp(Color.black, transparentColor, alpha);
				this.SplashScreen.color = newColor;
				yield return 0.5f;
			}
			UnityEngine.Object.Destroy(this.SplashScreen.gameObject);
			yield return 1f;
		}
		this._crossfadeFinished = true;
		yield break;
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x0012BD44 File Offset: 0x00129F44
	private IEnumerator LoadingProgressFadeInCoroutine()
	{
		foreach (float num in this.LoadingProgressFadeIn())
		{
			float step = num;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x0012BD60 File Offset: 0x00129F60
	private IEnumerator Start()
	{
		if (Launcher._progress == null)
		{
			foreach (float num in this.SplashScreenFadeOut())
			{
				float step = num;
				yield return null;
			}
			foreach (float num2 in this.LoadingProgressFadeIn())
			{
				float step2 = num2;
				yield return null;
			}
			Launcher._progress = new float?(0f);
			FrameStopwatchScript stopwatch = base.GetComponent<FrameStopwatchScript>();
			if (stopwatch == null)
			{
				stopwatch = base.gameObject.AddComponent<FrameStopwatchScript>();
			}
			foreach (float num3 in this.InitRootCoroutine())
			{
				float step3 = num3;
				if (step3 >= 0f)
				{
					Launcher._progress = new float?(step3);
				}
				if (stopwatch != null)
				{
					float elapsedSeconds = stopwatch.GetSecondsSinceFrameStarted();
					if (step3 >= 0f && elapsedSeconds < 1.618f / (float)this._targetFramerate)
					{
						continue;
					}
				}
				if (this.ProgressSlider != null)
				{
					this.ProgressSlider.value = Launcher._progress.Value;
				}
				if (this.ProgressLabel != null)
				{
					this.ProgressLabel.text = Launcher._progress.Value.ToString("P0");
				}
				if (!ActivityIndicator.IsActiveIndicator)
				{
					ActivityIndicator.IsActiveIndicator = this._crossfadeFinished;
				}
				yield return null;
			}
			if (this.Canvas != null)
			{
				UnityEngine.Object.Destroy(this.Canvas.gameObject);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			for (;;)
			{
				float? progress = Launcher._progress;
				if (progress == null || progress.Value >= 1f)
				{
					break;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x0012BD7C File Offset: 0x00129F7C
	private static void LogMessageWithBounds(string prefix, Launcher.Bounds bounds)
	{
		string message = string.Format("{0}: [{1:P0}, {2:P0}]\t\t{3}", new object[]
		{
			prefix,
			bounds.Lower,
			bounds.Upper,
			Time.frameCount
		});
		Debug.Log(message);
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x0012BDD0 File Offset: 0x00129FD0
	private IEnumerable<float> InitRootCoroutine()
	{
		Launcher.Bounds bounds = new Launcher.Bounds(0f, 0.04f);
		Launcher.LogMessageWithBounds("AppsMenuAwakeCoroutine()", bounds);
		Launcher.Bounds bounds2 = new Launcher.Bounds(0.05f, 0.09f);
		Launcher.LogMessageWithBounds("AppsMenuStartCoroutine()", bounds2);
		foreach (float num in this.AppsMenuStartCoroutine())
		{
			float step = num;
			yield return bounds2.Lerp(step);
		}
		Launcher.Bounds bounds3 = new Launcher.Bounds(0.1f, 0.19f);
		Launcher.LogMessageWithBounds("InAppInstancerStartCoroutine()", bounds3);
		foreach (float num2 in this.InAppInstancerStartCoroutine())
		{
			float step2 = num2;
			yield return bounds3.Lerp(step2);
		}
		Launcher.Bounds bounds4 = new Launcher.Bounds(0.2f, 0.24f);
		Launcher.LogMessageWithBounds("Application.LoadLevelAdditiveAsync(\"AppCenter\")", bounds4);
		AsyncOperation loadingCoroutine = Application.LoadLevelAdditiveAsync("AppCenter");
		while (!loadingCoroutine.isDone)
		{
			yield return bounds4.Lerp(loadingCoroutine.progress);
		}
		yield return -1f;
		Launcher.Bounds bounds5 = new Launcher.Bounds(0.25f, 0.29f);
		Launcher.LogMessageWithBounds("Application.LoadLevelAdditiveAsync(\"Loading\")", bounds5);
		AsyncOperation loadingCoroutine2 = Application.LoadLevelAdditiveAsync("Loading");
		while (!loadingCoroutine2.isDone)
		{
			yield return bounds5.Lerp(loadingCoroutine2.progress);
		}
		yield return -1f;
		Switcher switcher = UnityEngine.Object.FindObjectOfType<Switcher>();
		if (switcher != null)
		{
			Launcher.Bounds bounds6 = new Launcher.Bounds(0.3f, 0.89f);
			Launcher.LogMessageWithBounds("Switcher.InitializeSwitcher()", bounds6);
			foreach (float num3 in switcher.InitializeSwitcher(null))
			{
				float step3 = num3;
				yield return (step3 >= 0f) ? bounds6.Lerp(step3) : step3;
			}
		}
		Launcher.Bounds bounds7 = new Launcher.Bounds(0.9f, 0.99f);
		Launcher.LogMessageWithBounds("Switcher.LoadMainMenu()", bounds7);
		foreach (float num4 in switcher.LoadMainMenu(false))
		{
			float step4 = num4;
			yield return bounds7.Lerp(step4);
		}
		yield return 1f;
		yield break;
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x0012BDF4 File Offset: 0x00129FF4
	private static string GetTerminalSceneName_3afcc97c(uint gamma)
	{
		return "ClosingScene";
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x0012BDFC File Offset: 0x00129FFC
	private IEnumerable<float> AppsMenuStartCoroutine()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				LicenseVerificationController.PackageInfo actualPackageInfo = default(LicenseVerificationController.PackageInfo);
				try
				{
					actualPackageInfo = LicenseVerificationController.GetPackageInfo();
					Launcher.PackageInfo = new LicenseVerificationController.PackageInfo?(actualPackageInfo);
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.Log("LicenseVerificationController.GetPackageInfo() failed:    " + ex);
					Singleton<SceneLoader>.Instance.LoadScene(Launcher.GetTerminalSceneName_3afcc97c(989645180U), LoadSceneMode.Single);
				}
				finally
				{
					if (actualPackageInfo.SignatureHash == null)
					{
						Debug.Log("actualPackageInfo.SignatureHash == null");
						Singleton<SceneLoader>.Instance.LoadScene(Launcher.GetTerminalSceneName_3afcc97c(989645180U), LoadSceneMode.Single);
					}
				}
				string actualPackageName = actualPackageInfo.PackageName;
				if (string.Compare(actualPackageName, Defs.GetIntendedAndroidPackageName(), StringComparison.Ordinal) != 0)
				{
					Debug.LogWarning("Verification FakeBundleDetected:    " + actualPackageName);
					Singleton<SceneLoader>.Instance.LoadScene(Launcher.GetTerminalSceneName_3afcc97c(989645180U), LoadSceneMode.Single);
				}
				else
				{
					Debug.Log("Package check passed.");
				}
				if (string.IsNullOrEmpty(this.intendedSignatureHash))
				{
					Debug.LogWarning("String.IsNullOrEmpty(intendedSignatureHash)");
					Singleton<SceneLoader>.Instance.LoadScene(Launcher.GetTerminalSceneName_3afcc97c(989645180U), LoadSceneMode.Single);
				}
				string actualSignatureHash = actualPackageInfo.SignatureHash;
				if (string.Compare(actualSignatureHash, this.intendedSignatureHash, StringComparison.Ordinal) != 0)
				{
					Debug.LogWarning("Verification FakeSignatureDetected:    " + actualSignatureHash);
					Singleton<SceneLoader>.Instance.LoadScene(Launcher.GetTerminalSceneName_3afcc97c(989645180U), LoadSceneMode.Single);
				}
				else
				{
					Debug.Log("Signature check passed.");
				}
				yield return 0.2f;
			}
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			if (AppsMenu.ApplicationBinarySplitted && !Application.isEditor)
			{
				string expPath = GooglePlayDownloader.GetExpansionFilePath();
				if (string.IsNullOrEmpty(expPath))
				{
					Debug.LogError(string.Format("ExpPath: “{0}”", expPath));
				}
				else if (Defs.IsDeveloperBuild)
				{
					Debug.Log(string.Format("ExpPath: “{0}”", expPath));
				}
				string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				if (mainPath == null)
				{
					Debug.Log("Trying to fetch OBB...");
					GooglePlayDownloader.FetchOBB();
				}
				mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
				if (mainPath == null)
				{
					Debug.Log("Waiting OBB fetch...");
				}
				while (mainPath == null)
				{
					yield return 0.6f;
					if (Time.frameCount % 120 == 0)
					{
						mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
					}
				}
			}
			yield return 0.6f;
		}
		yield return 0.8f;
		AppsMenu.SetCurrentLanguage();
		yield return 1f;
		yield break;
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x0012BE20 File Offset: 0x0012A020
	private IEnumerable<float> InAppInstancerStartCoroutine()
	{
		if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
		{
			UnityEngine.Object.Instantiate(this.inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
			yield return 0.1f;
		}
		if (this.amazonIapManagerPrefab == null)
		{
			Debug.LogWarning("amazonIapManager == null");
		}
		else if (!this._amazonIapManagerInitialized)
		{
			UnityEngine.Object.Instantiate(this.amazonIapManagerPrefab, Vector3.zero, Quaternion.identity);
			this._amazonIapManagerInitialized = true;
			yield return 0.2f;
		}
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (this.amazonGameCircleManager == null)
			{
				Debug.LogWarning("amazonGamecircleManager == null");
			}
			else if (!this._amazonGamecircleManagerInitialized)
			{
				UnityEngine.Object.DontDestroyOnLoad(this.amazonGameCircleManager);
				this._leaderboardId = ((Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite) ? "best_survival_scores" : "CgkIr8rGkPIJEAIQCg");
				if (!AGSClient.IsServiceReady())
				{
					Debug.Log("Trying to initialize Amazon GameCircle service...");
					AGSClient.ServiceReadyEvent += this.HandleAmazonGamecircleServiceReady;
					AGSClient.ServiceNotReadyEvent += this.HandleAmazonGamecircleServiceNotReady;
					AGSClient.Init(true, true, true);
					AGSWhispersyncClient.OnNewCloudDataEvent += this.HandleAmazonPotentialProgressConflicts;
					AGSWhispersyncClient.OnDataUploadedToCloudEvent += this.HandleAmazonPotentialProgressConflicts;
					AGSWhispersyncClient.OnSyncFailedEvent += this.HandleAmazonSyncFailed;
					AGSWhispersyncClient.OnThrottledEvent += this.HandleAmazonThrottled;
				}
				else
				{
					Debug.Log("Amazon GameCircle was already initialized.");
					AGSLeaderboardsClient.SubmitScoreSucceededEvent += this.HandleAmazonSubmitScoreSucceeded;
					AGSLeaderboardsClient.SubmitScoreFailedEvent += this.HandleAmazonSubmitScoreFailed;
					AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
				}
				this._amazonGamecircleManagerInitialized = true;
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		}
		yield return 1f;
		yield break;
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x0012BE44 File Offset: 0x0012A044
	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		Debug.Log(string.Format("GameThrive HandleNotification(“{0}”, ..., {1})", message, isActive));
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x0012BE5C File Offset: 0x0012A05C
	private void HandleAmazonGamecircleServiceReady()
	{
		AGSClient.ServiceReadyEvent -= this.HandleAmazonGamecircleServiceReady;
		AGSClient.ServiceNotReadyEvent -= this.HandleAmazonGamecircleServiceNotReady;
		Debug.Log("Amazon GameCircle service is initialized.");
		AGSAchievementsClient.UpdateAchievementCompleted += this.HandleUpdateAchievementCompleted;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += this.HandleAmazonSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent += this.HandleAmazonSubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x0012BEE0 File Offset: 0x0012A0E0
	private void HandleAmazonPotentialProgressConflicts()
	{
		Debug.Log("HandleAmazonPotentialProgressConflicts()");
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x0012BEEC File Offset: 0x0012A0EC
	private void HandleAmazonSyncFailed()
	{
		Debug.LogWarning("HandleAmazonSyncFailed(): " + AGSWhispersyncClient.failReason);
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x0012BF04 File Offset: 0x0012A104
	private void HandleAmazonThrottled()
	{
		Debug.LogWarning("HandleAmazonThrottled().");
	}

	// Token: 0x060039DC RID: 14812 RVA: 0x0012BF10 File Offset: 0x0012A110
	private void HandleAmazonGamecircleServiceNotReady(string message)
	{
		Debug.LogError("Amazon GameCircle service is not ready:\n" + message);
	}

	// Token: 0x060039DD RID: 14813 RVA: 0x0012BF24 File Offset: 0x0012A124
	private void HandleUpdateAchievementCompleted(AGSUpdateAchievementResponse response)
	{
		string message = (!string.IsNullOrEmpty(response.error)) ? string.Format("Achievement {0} failed. {1}", response.achievementId, response.error) : string.Format("Achievement {0} succeeded.", response.achievementId);
		Debug.Log(message);
	}

	// Token: 0x060039DE RID: 14814 RVA: 0x0012BF74 File Offset: 0x0012A174
	private void HandleAmazonSubmitScoreSucceeded(string leaderbordId)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= this.HandleAmazonSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= this.HandleAmazonSubmitScoreFailed;
		if (Debug.isDebugBuild)
		{
			Debug.Log("Submit score succeeded for leaderboard " + leaderbordId);
		}
	}

	// Token: 0x060039DF RID: 14815 RVA: 0x0012BFC0 File Offset: 0x0012A1C0
	private void HandleAmazonSubmitScoreFailed(string leaderbordId, string error)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= this.HandleAmazonSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= this.HandleAmazonSubmitScoreFailed;
		string message = string.Format("Submit score failed for leaderboard {0}:\n{1}", leaderbordId, error);
		Debug.LogError(message);
	}

	// Token: 0x04002A8F RID: 10895
	public string intendedSignatureHash;

	// Token: 0x04002A90 RID: 10896
	public GameObject inAppGameObjectPrefab;

	// Token: 0x04002A91 RID: 10897
	public Canvas Canvas;

	// Token: 0x04002A92 RID: 10898
	public Slider ProgressSlider;

	// Token: 0x04002A93 RID: 10899
	public Text ProgressLabel;

	// Token: 0x04002A94 RID: 10900
	public RawImage SplashScreen;

	// Token: 0x04002A95 RID: 10901
	public GameObject amazonIapManagerPrefab;

	// Token: 0x04002A96 RID: 10902
	private GameObject amazonGameCircleManager;

	// Token: 0x04002A97 RID: 10903
	private static float? _progress;

	// Token: 0x04002A98 RID: 10904
	private bool _amazonGamecircleManagerInitialized;

	// Token: 0x04002A99 RID: 10905
	private bool _amazonIapManagerInitialized;

	// Token: 0x04002A9A RID: 10906
	private bool _crossfadeFinished;

	// Token: 0x04002A9B RID: 10907
	private static bool? _usingNewLauncher;

	// Token: 0x04002A9C RID: 10908
	private string _leaderboardId = string.Empty;

	// Token: 0x04002A9D RID: 10909
	private int _targetFramerate = 30;

	// Token: 0x02000680 RID: 1664
	private struct Bounds
	{
		// Token: 0x060039E0 RID: 14816 RVA: 0x0012C004 File Offset: 0x0012A204
		public Bounds(float lower, float upper)
		{
			this._lower = Mathf.Min(lower, upper);
			this._upper = Mathf.Max(lower, upper);
		}

		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0012C020 File Offset: 0x0012A220
		public float Lower
		{
			get
			{
				return this._lower;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x0012C028 File Offset: 0x0012A228
		public float Upper
		{
			get
			{
				return this._upper;
			}
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0012C030 File Offset: 0x0012A230
		private float Clamp(float value)
		{
			return Mathf.Clamp(value, this._lower, this._upper);
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0012C044 File Offset: 0x0012A244
		public float Lerp(float value, float t)
		{
			return Mathf.Lerp(this.Clamp(value), this._upper, t);
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x0012C05C File Offset: 0x0012A25C
		public float Lerp(float t)
		{
			return this.Lerp(this._lower, t);
		}

		// Token: 0x04002A9F RID: 10911
		private readonly float _lower;

		// Token: 0x04002AA0 RID: 10912
		private readonly float _upper;
	}
}
