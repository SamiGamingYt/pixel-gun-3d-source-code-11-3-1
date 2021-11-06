using System;
using Com.Google.Android.Gms.Common.Api;
using Com.Google.Android.Gms.Games;
using Com.Google.Android.Gms.Games.Stats;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	// Token: 0x020001A8 RID: 424
	internal class AndroidClient : IClientImpl
	{
		// Token: 0x06000DC8 RID: 3528 RVA: 0x00044F50 File Offset: 0x00043150
		public PlatformConfiguration CreatePlatformConfiguration()
		{
			AndroidPlatformConfiguration androidPlatformConfiguration = AndroidPlatformConfiguration.Create();
			using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
			{
				androidPlatformConfiguration.SetActivity(activity.GetRawObject());
				androidPlatformConfiguration.SetOptionalIntentHandlerForUI(delegate(IntPtr intent)
				{
					IntPtr intentRef = AndroidJNI.NewGlobalRef(intent);
					PlayGamesHelperObject.RunOnGameThread(delegate
					{
						try
						{
							AndroidClient.LaunchBridgeIntent(intentRef);
						}
						finally
						{
							AndroidJNI.DeleteGlobalRef(intentRef);
						}
					});
				});
			}
			return androidPlatformConfiguration;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00044FC8 File Offset: 0x000431C8
		public TokenClient CreateTokenClient(string playerId, bool reset)
		{
			if (this.tokenClient == null || reset)
			{
				this.tokenClient = new AndroidTokenClient(playerId);
			}
			return this.tokenClient;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00044FF0 File Offset: 0x000431F0
		private static void LaunchBridgeIntent(IntPtr bridgedIntent)
		{
			object[] args = new object[2];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
				{
					using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
						array[0].l = activity.GetRawObject();
						array[1].l = bridgedIntent;
						AndroidJNI.CallStaticVoidMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching bridge intent: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00045120 File Offset: 0x00043320
		public void GetPlayerStats(IntPtr apiClient, Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			GoogleApiClient arg_GoogleApiClient_ = new GoogleApiClient(apiClient);
			AndroidClient.StatsResultCallback resultCallback;
			try
			{
				resultCallback = new AndroidClient.StatsResultCallback(delegate(int result, Com.Google.Android.Gms.Games.Stats.PlayerStats stats)
				{
					Debug.Log("Result for getStats: " + result);
					GooglePlayGames.BasicApi.PlayerStats playerStats = null;
					if (stats != null)
					{
						playerStats = new GooglePlayGames.BasicApi.PlayerStats();
						playerStats.AvgSessonLength = stats.getAverageSessionLength();
						playerStats.DaysSinceLastPlayed = stats.getDaysSinceLastPlayed();
						playerStats.NumberOfPurchases = stats.getNumberOfPurchases();
						playerStats.NumberOfSessions = stats.getNumberOfSessions();
						playerStats.SessPercentile = stats.getSessionPercentile();
						playerStats.SpendPercentile = stats.getSpendPercentile();
						playerStats.ChurnProbability = stats.getChurnProbability();
						playerStats.SpendProbability = stats.getSpendProbability();
						playerStats.HighSpenderProbability = stats.getHighSpenderProbability();
						playerStats.TotalSpendNext28Days = stats.getTotalSpendNext28Days();
					}
					callback((CommonStatusCodes)result, playerStats);
				});
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				callback(CommonStatusCodes.DeveloperError, null);
				return;
			}
			PendingResult<Stats_LoadPlayerStatsResultObject> pendingResult = Games.Stats.loadPlayerStats(arg_GoogleApiClient_, true);
			pendingResult.setResultCallback(resultCallback);
		}

		// Token: 0x04000A8B RID: 2699
		internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";

		// Token: 0x04000A8C RID: 2700
		private const string LaunchBridgeMethod = "launchBridgeIntent";

		// Token: 0x04000A8D RID: 2701
		private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";

		// Token: 0x04000A8E RID: 2702
		private TokenClient tokenClient;

		// Token: 0x020001A9 RID: 425
		private class StatsResultCallback : ResultCallbackProxy<Stats_LoadPlayerStatsResultObject>
		{
			// Token: 0x06000DCD RID: 3533 RVA: 0x000451DC File Offset: 0x000433DC
			public StatsResultCallback(Action<int, Com.Google.Android.Gms.Games.Stats.PlayerStats> callback)
			{
				this.callback = callback;
			}

			// Token: 0x06000DCE RID: 3534 RVA: 0x000451EC File Offset: 0x000433EC
			public override void OnResult(Stats_LoadPlayerStatsResultObject arg_Result_1)
			{
				this.callback(arg_Result_1.getStatus().getStatusCode(), arg_Result_1.getPlayerStats());
			}

			// Token: 0x04000A90 RID: 2704
			private Action<int, Com.Google.Android.Gms.Games.Stats.PlayerStats> callback;
		}
	}
}
