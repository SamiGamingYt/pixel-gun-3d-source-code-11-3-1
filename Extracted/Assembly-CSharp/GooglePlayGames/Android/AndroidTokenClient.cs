using System;
using Com.Google.Android.Gms.Common.Api;
using GooglePlayGames.BasicApi;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Android
{
	// Token: 0x020001AA RID: 426
	internal class AndroidTokenClient : TokenClient
	{
		// Token: 0x06000DCF RID: 3535 RVA: 0x00045218 File Offset: 0x00043418
		public AndroidTokenClient(string playerId)
		{
			this.playerId = playerId;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x00045240 File Offset: 0x00043440
		public static AndroidJavaObject GetActivity()
		{
			AndroidJavaObject @static;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				@static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return @static;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00045298 File Offset: 0x00043498
		public void SetRationale(string rationale)
		{
			this.rationale = rationale;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000452A4 File Offset: 0x000434A4
		internal void Fetch(string scope, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<CommonStatusCodes> doneCallback)
		{
			if (this.apiAccessDenied)
			{
				if (this.apiWarningCount++ % this.apiWarningFreq == 0)
				{
					GooglePlayGames.OurUtils.Logger.w("Access to API denied");
					this.apiWarningCount = this.apiWarningCount / this.apiWarningFreq + 1;
				}
				doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
				return;
			}
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				AndroidTokenClient.FetchToken(scope, this.playerId, this.rationale, fetchEmail, fetchAccessToken, fetchIdToken, delegate(int rc, string access, string id, string email)
				{
					if (rc != 0)
					{
						this.apiAccessDenied = (rc == 3001 || rc == 16);
						GooglePlayGames.OurUtils.Logger.w("Non-success returned from fetch: " + rc);
						doneCallback(CommonStatusCodes.AuthApiAccessForbidden);
						return;
					}
					if (fetchAccessToken)
					{
						GooglePlayGames.OurUtils.Logger.d("a = " + access);
					}
					if (fetchEmail)
					{
						GooglePlayGames.OurUtils.Logger.d("email = " + email);
					}
					if (fetchIdToken)
					{
						GooglePlayGames.OurUtils.Logger.d("idt = " + id);
					}
					if (fetchAccessToken && !string.IsNullOrEmpty(access))
					{
						this.accessToken = access;
					}
					if (fetchIdToken && !string.IsNullOrEmpty(id))
					{
						this.idToken = id;
						this.idTokenCb(this.idToken);
					}
					if (fetchEmail && !string.IsNullOrEmpty(email))
					{
						this.accountName = email;
					}
					doneCallback(CommonStatusCodes.Success);
				});
			});
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0004534C File Offset: 0x0004354C
		internal static void FetchToken(string scope, string playerId, string rationale, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, Action<int, string, string, string> callback)
		{
			object[] args = new object[7];
			jvalue[] array = AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (AndroidJavaObject activity = AndroidTokenClient.GetActivity())
					{
						IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "fetchToken", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
						array[0].l = activity.GetRawObject();
						array[1].l = AndroidJNI.NewStringUTF(playerId);
						array[2].l = AndroidJNI.NewStringUTF(rationale);
						array[3].z = fetchEmail;
						array[4].z = fetchAccessToken;
						array[5].z = fetchIdToken;
						array[6].l = AndroidJNI.NewStringUTF(scope);
						IntPtr ptr = AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
						PendingResult<TokenResult> pendingResult = new PendingResult<TokenResult>(ptr);
						pendingResult.setResultCallback(new TokenResultCallback(callback));
					}
				}
			}
			catch (Exception ex)
			{
				GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x000454E8 File Offset: 0x000436E8
		private string GetAccountName(Action<CommonStatusCodes, string> callback)
		{
			if (string.IsNullOrEmpty(this.accountName))
			{
				if (!this.fetchingEmail)
				{
					this.fetchingEmail = true;
					this.Fetch(this.idTokenScope, true, false, false, delegate(CommonStatusCodes status)
					{
						this.fetchingEmail = false;
						if (callback != null)
						{
							callback(status, this.accountName);
						}
					});
				}
			}
			else if (callback != null)
			{
				callback(CommonStatusCodes.Success, this.accountName);
			}
			return this.accountName;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00045570 File Offset: 0x00043770
		public string GetEmail()
		{
			return this.GetAccountName(null);
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0004557C File Offset: 0x0004377C
		public void GetEmail(Action<CommonStatusCodes, string> callback)
		{
			this.GetAccountName(callback);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00045588 File Offset: 0x00043788
		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string GetAccessToken()
		{
			if (string.IsNullOrEmpty(this.accessToken) && !this.fetchingAccessToken)
			{
				this.fetchingAccessToken = true;
				this.Fetch(this.idTokenScope, false, true, false, delegate(CommonStatusCodes rc)
				{
					this.fetchingAccessToken = false;
				});
			}
			return this.accessToken;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000455D8 File Offset: 0x000437D8
		[Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(string serverClientId, Action<string> idTokenCallback)
		{
			if (string.IsNullOrEmpty(serverClientId))
			{
				if (this.webClientWarningCount++ % this.webClientWarningFreq == 0)
				{
					GooglePlayGames.OurUtils.Logger.w("serverClientId is empty, cannot get Id Token");
					this.webClientWarningCount = this.webClientWarningCount / this.webClientWarningFreq + 1;
				}
				idTokenCallback(null);
				return;
			}
			string a = "audience:server:client_id:" + serverClientId;
			if (string.IsNullOrEmpty(this.idToken) || a != this.idTokenScope)
			{
				if (!this.fetchingIdToken)
				{
					this.fetchingIdToken = true;
					this.idTokenScope = a;
					this.idTokenCb = idTokenCallback;
					this.Fetch(this.idTokenScope, false, false, true, delegate(CommonStatusCodes status)
					{
						this.fetchingIdToken = false;
						if (status == CommonStatusCodes.Success)
						{
							this.idTokenCb(null);
						}
						else
						{
							this.idTokenCb(this.idToken);
						}
					});
				}
			}
			else
			{
				idTokenCallback(this.idToken);
			}
		}

		// Token: 0x04000A91 RID: 2705
		private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";

		// Token: 0x04000A92 RID: 2706
		private const string FetchTokenSignature = "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";

		// Token: 0x04000A93 RID: 2707
		private const string FetchTokenMethod = "fetchToken";

		// Token: 0x04000A94 RID: 2708
		private string playerId;

		// Token: 0x04000A95 RID: 2709
		private bool fetchingEmail;

		// Token: 0x04000A96 RID: 2710
		private bool fetchingAccessToken;

		// Token: 0x04000A97 RID: 2711
		private bool fetchingIdToken;

		// Token: 0x04000A98 RID: 2712
		private string accountName;

		// Token: 0x04000A99 RID: 2713
		private string accessToken;

		// Token: 0x04000A9A RID: 2714
		private string idToken;

		// Token: 0x04000A9B RID: 2715
		private string idTokenScope;

		// Token: 0x04000A9C RID: 2716
		private Action<string> idTokenCb;

		// Token: 0x04000A9D RID: 2717
		private string rationale;

		// Token: 0x04000A9E RID: 2718
		private bool apiAccessDenied;

		// Token: 0x04000A9F RID: 2719
		private int apiWarningFreq = 100000;

		// Token: 0x04000AA0 RID: 2720
		private int apiWarningCount;

		// Token: 0x04000AA1 RID: 2721
		private int webClientWarningFreq = 100000;

		// Token: 0x04000AA2 RID: 2722
		private int webClientWarningCount;
	}
}
