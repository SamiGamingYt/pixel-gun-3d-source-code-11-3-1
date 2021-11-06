using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using com.amazon.mas.cpt.ads;
using Rilisoft;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

// Token: 0x020002C9 RID: 713
[Obfuscation(Exclude = true)]
internal sealed class InAppInstancer : MonoBehaviour
{
	// Token: 0x060018C2 RID: 6338 RVA: 0x0005D1B0 File Offset: 0x0005B3B0
	private IEnumerator Start()
	{
		if (Launcher.UsingNewLauncher)
		{
			yield break;
		}
		if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
		{
			UnityEngine.Object.Instantiate(this.inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
			yield return null;
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (!this._amazonGamecircleManagerInitialized)
			{
				base.StartCoroutine(this.InitializeAmazonGamecircleManager());
				this._amazonGamecircleManagerInitialized = true;
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		}
		yield break;
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x0005D1CC File Offset: 0x0005B3CC
	private IEnumerator InitializeAmazonGamecircleManager()
	{
		string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.InitializeAmazonGamecircleManager()", new object[]
		{
			base.GetType().Name
		});
		using (new ScopeLogger(thisMethod, true))
		{
			GameObject amazonGameCircleManager = new GameObject("Rilisoft.AmazonGameCircleManager", new Type[]
			{
				typeof(GameCircleManager)
			});
			UnityEngine.Object.DontDestroyOnLoad(amazonGameCircleManager);
			yield return null;
			this._leaderboardId = ((Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite) ? "best_survival_scores" : "CgkIr8rGkPIJEAIQCg");
			using (new ScopeLogger(thisMethod, "Initializing Amazon Ads", true))
			{
				try
				{
					IAmazonMobileAds mobileAds = AmazonMobileAdsImpl.Instance;
					ShouldEnable loggingEnabled = new ShouldEnable
					{
						BooleanValue = Defs.IsDeveloperBuild
					};
					mobileAds.EnableLogging(loggingEnabled);
					ApplicationKey applicationKey = new ApplicationKey
					{
						StringValue = "1bb979bc6c9e4059a318370a68dcaeea"
					};
					mobileAds.SetApplicationKey(applicationKey);
					mobileAds.RegisterApplication();
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.LogException(ex);
				}
			}
			if (!AGSClient.IsServiceReady())
			{
				using (new ScopeLogger(thisMethod, "Initializing Amazon GameCircle service", true))
				{
					AGSClient.ServiceReadyEvent += this.HandleAmazonGamecircleServiceReady;
					AGSClient.ServiceNotReadyEvent += this.HandleAmazonGamecircleServiceNotReady;
					AGSClient.Init(true, true, true);
					AGSWhispersyncClient.OnNewCloudDataEvent += this.HandleAmazonPotentialProgressConflicts;
					AGSWhispersyncClient.OnDataUploadedToCloudEvent += this.HandleAmazonPotentialProgressConflicts;
					AGSWhispersyncClient.OnSyncFailedEvent += this.HandleAmazonSyncFailed;
					AGSWhispersyncClient.OnThrottledEvent += this.HandleAmazonThrottled;
				}
			}
			else
			{
				Debug.Log("Amazon GameCircle was already initialized.");
				AGSLeaderboardsClient.SubmitScoreSucceededEvent += this.HandleSubmitScoreSucceeded;
				AGSLeaderboardsClient.SubmitScoreFailedEvent += this.HandleSubmitScoreFailed;
				AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
			}
			using (new ScopeLogger(thisMethod, "Waiting AGSClient.IsServiceReady()", Defs.IsDeveloperBuild))
			{
				while (!AGSClient.IsServiceReady())
				{
					yield return null;
				}
			}
			if (!GameCircleSocial.Instance.localUser.authenticated)
			{
				using (new ScopeLogger(thisMethod, "Sign in to GameCircle", Defs.IsDeveloperBuild))
				{
					AGSClient.ShowSignInPage();
				}
			}
		}
		yield break;
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x0005D1E8 File Offset: 0x0005B3E8
	private void HandleAmazonGamecircleServiceReady()
	{
		AGSClient.ServiceReadyEvent -= this.HandleAmazonGamecircleServiceReady;
		AGSClient.ServiceNotReadyEvent -= this.HandleAmazonGamecircleServiceNotReady;
		Debug.Log("Amazon GameCircle service is initialized.");
		AGSAchievementsClient.UpdateAchievementCompleted += this.HandleUpdateAchievementCompleted;
		AGSLeaderboardsClient.SubmitScoreSucceededEvent += this.HandleSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent += this.HandleSubmitScoreFailed;
		AGSLeaderboardsClient.SubmitScore(this._leaderboardId, (long)PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0), 0);
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x0005D26C File Offset: 0x0005B46C
	private void HandleAmazonPotentialProgressConflicts()
	{
		Debug.Log("HandleAmazonPotentialProgressConflicts()");
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x0005D278 File Offset: 0x0005B478
	private void HandleAmazonSyncFailed()
	{
		Debug.LogWarning("HandleAmazonSyncFailed(): " + AGSWhispersyncClient.failReason);
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x0005D290 File Offset: 0x0005B490
	private void HandleAmazonThrottled()
	{
		Debug.LogWarning("HandleAmazonThrottled().");
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x0005D29C File Offset: 0x0005B49C
	private void HandleAmazonGamecircleServiceNotReady(string message)
	{
		Debug.LogError("Amazon GameCircle service is not ready:\n" + message);
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x0005D2B0 File Offset: 0x0005B4B0
	private void HandleUpdateAchievementCompleted(AGSUpdateAchievementResponse response)
	{
		string message = (!string.IsNullOrEmpty(response.error)) ? string.Format("Achievement {0} failed. {1}", response.achievementId, response.error) : string.Format("Achievement {0} succeeded.", response.achievementId);
		Debug.Log(message);
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x0005D300 File Offset: 0x0005B500
	private void HandleSubmitScoreSucceeded(string leaderbordId)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= this.HandleSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= this.HandleSubmitScoreFailed;
		if (Debug.isDebugBuild)
		{
			Debug.Log("Submit score succeeded for leaderboard " + leaderbordId);
		}
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x0005D34C File Offset: 0x0005B54C
	private void HandleSubmitScoreFailed(string leaderbordId, string error)
	{
		AGSLeaderboardsClient.SubmitScoreSucceededEvent -= this.HandleSubmitScoreSucceeded;
		AGSLeaderboardsClient.SubmitScoreFailedEvent -= this.HandleSubmitScoreFailed;
		string message = string.Format("Submit score failed for leaderboard {0}:\n{1}", leaderbordId, error);
		Debug.LogError(message);
	}

	// Token: 0x04000D35 RID: 3381
	public GameObject inAppGameObjectPrefab;

	// Token: 0x04000D36 RID: 3382
	private bool _amazonGamecircleManagerInitialized;

	// Token: 0x04000D37 RID: 3383
	private bool _amazonIapManagerInitialized;

	// Token: 0x04000D38 RID: 3384
	private string _leaderboardId = string.Empty;
}
