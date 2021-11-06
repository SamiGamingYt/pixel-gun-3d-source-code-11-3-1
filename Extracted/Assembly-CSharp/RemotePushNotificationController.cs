using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Prime31;
using Rilisoft;
using UnityEngine;

// Token: 0x020004BF RID: 1215
public sealed class RemotePushNotificationController : MonoBehaviour
{
	// Token: 0x06002B84 RID: 11140 RVA: 0x000E5410 File Offset: 0x000E3610
	private IEnumerator Start()
	{
		string thisMethod = string.Format("{0}.Start()", base.GetType().Name);
		using (new ScopeLogger(thisMethod, Defs.IsDeveloperBuild && !Application.isEditor))
		{
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				yield break;
			}
			string remotePushRegistrationJson = PlayerPrefs.GetString("RemotePushRegistration", "{}");
			RemotePushRegistrationMemento remotePushRegistrationMemento = RemotePushNotificationController.ParseRemotePushRegistrationMemento(remotePushRegistrationJson);
			if (!string.IsNullOrEmpty(remotePushRegistrationMemento.RegistrationId) && !RemotePushNotificationController.CheckIfExpired(remotePushRegistrationMemento))
			{
				Debug.LogFormat("Remote push notifications, already registered: '{0}'", new object[]
				{
					remotePushRegistrationJson
				});
				yield break;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (Application.isEditor)
			{
				this.HandleRegistered(DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));
				yield break;
			}
			yield return new WaitForSeconds(1f);
			using (new ScopeLogger(thisMethod, "GoogleCloudMessaging.register()", Defs.IsDeveloperBuild && !Application.isEditor))
			{
				GoogleCloudMessagingManager.registrationSucceededEvent += this.HandleRegistered;
				GoogleCloudMessagingManager.registrationFailedEvent += this.HandleError;
				GoogleCloudMessaging.register("339873998127");
			}
		}
		yield break;
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x000E542C File Offset: 0x000E362C
	private void HandleError(string error)
	{
		Debug.LogError(error);
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x000E5434 File Offset: 0x000E3634
	private void HandleRegistered(string registrationId)
	{
		string callee = string.Format("{0}.HandleRegistered('{1}')", base.GetType().Name, registrationId);
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
		{
			if (!string.IsNullOrEmpty(registrationId))
			{
				base.StartCoroutine(this.ReciveUpdateDataToServer(registrationId));
			}
		}
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000E54BC File Offset: 0x000E36BC
	public void UpdateDataOnServer()
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento = RemotePushNotificationController.LoadRemotePushRegistrationMemento();
		if (string.IsNullOrEmpty(remotePushRegistrationMemento.RegistrationId))
		{
			return;
		}
		base.StartCoroutine(this.ReciveUpdateDataToServer(remotePushRegistrationMemento.RegistrationId));
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x000E54F8 File Offset: 0x000E36F8
	private static RemotePushRegistrationMemento ParseRemotePushRegistrationMemento(string remotePushRegistrationJson)
	{
		RemotePushRegistrationMemento result;
		try
		{
			result = JsonUtility.FromJson<RemotePushRegistrationMemento>(remotePushRegistrationJson);
		}
		catch (Exception message)
		{
			Debug.LogWarning(message);
			result = new RemotePushRegistrationMemento(string.Empty, DateTime.MinValue, string.Empty);
		}
		return result;
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x000E555C File Offset: 0x000E375C
	private static RemotePushRegistrationMemento LoadRemotePushRegistrationMemento()
	{
		string @string = PlayerPrefs.GetString("RemotePushRegistration", "{}");
		return RemotePushNotificationController.ParseRemotePushRegistrationMemento(@string);
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x000E5584 File Offset: 0x000E3784
	private static bool IsDeviceRegistred()
	{
		string remotePushNotificationToken = RemotePushNotificationController.GetRemotePushNotificationToken();
		return !string.IsNullOrEmpty(remotePushNotificationToken);
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x000E55A0 File Offset: 0x000E37A0
	private static bool CheckIfExpired(RemotePushRegistrationMemento remotePushRegistrationMemento)
	{
		DateTime d;
		return !DateTime.TryParse(remotePushRegistrationMemento.RegistrationTime, out d) || !(DateTime.UtcNow - d < TimeSpan.FromDays(2.0));
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x000E55E8 File Offset: 0x000E37E8
	internal static string GetRemotePushNotificationToken()
	{
		RemotePushRegistrationMemento remotePushRegistrationMemento = RemotePushNotificationController.LoadRemotePushRegistrationMemento();
		if (RemotePushNotificationController.CheckIfExpired(remotePushRegistrationMemento))
		{
			return string.Empty;
		}
		return remotePushRegistrationMemento.RegistrationId;
	}

	// Token: 0x17000789 RID: 1929
	// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000E5614 File Offset: 0x000E3814
	private string UrlPushNotificationServer
	{
		get
		{
			return "https://secure.pixelgunserver.com/push_service";
		}
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x000E561C File Offset: 0x000E381C
	private IEnumerator ReciveUpdateDataToServer(string deviceToken)
	{
		string thisMethod = string.Format("{0}.ReciveUpdateDataToServer('{1}')", base.GetType().Name, deviceToken);
		using (new ScopeLogger(thisMethod, Defs.IsDeveloperBuild && !Application.isEditor))
		{
			if (this._isResponceRuning)
			{
				yield break;
			}
			this._isResponceRuning = true;
			bool friendsControllerIsNotInitialized = FriendsController.sharedController == null;
			if (Defs.IsDeveloperBuild && FriendsController.sharedController == null)
			{
				Debug.Log("Waiting FriendsController being initialized...");
			}
			while (FriendsController.sharedController == null)
			{
				yield return null;
			}
			if (friendsControllerIsNotInitialized)
			{
				yield return null;
			}
			if (Defs.IsDeveloperBuild && FriendsController.sharedController.id == null)
			{
				Debug.Log("Waiting FriendsController.id being initialized...");
			}
			while (string.IsNullOrEmpty(FriendsController.sharedController.id))
			{
				yield return null;
			}
			this._isStartUpdateRecive = true;
			WWWForm form = new WWWForm();
			string appVersion = string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion);
			string playerId = FriendsController.sharedController.id;
			string languageCode = LocalizationStore.GetCurrentLanguageCode();
			string isPayingPlayer = Storager.getInt("PayingUser", true).ToString();
			string dateLastPaying = PlayerPrefs.GetString("Last Payment Time", string.Empty);
			if (string.IsNullOrEmpty(dateLastPaying))
			{
				dateLastPaying = "None";
			}
			string timeUtcOffsetString = DateTimeOffset.Now.Offset.Hours.ToString();
			string countMoney = Storager.getInt("Coins", false).ToString();
			string countGems = Storager.getInt("GemsCurrency", false).ToString();
			string playerLevel = ExperienceController.GetCurrentLevel().ToString();
			form.AddField("app_version", appVersion);
			form.AddField("device_token", deviceToken);
			form.AddField("uniq_id", playerId);
			form.AddField("is_paying", isPayingPlayer);
			form.AddField("last_payment_date", dateLastPaying);
			form.AddField("utc_shift", timeUtcOffsetString);
			form.AddField("coins", countMoney);
			form.AddField("gems", countGems);
			form.AddField("level", playerLevel);
			form.AddField("language", languageCode);
			form.AddField("allow_invites", (!Defs.isEnableRemoteInviteFromFriends) ? 0 : 1);
			int androidSdkLevel = 0;
			if (Application.platform == RuntimePlatform.Android)
			{
				try
				{
					androidSdkLevel = AndroidSystem.GetSdkVersion();
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.LogException(ex);
				}
			}
			if (Application.platform == RuntimePlatform.Android)
			{
				form.AddField("os", androidSdkLevel);
			}
			else
			{
				form.AddField("os", SystemInfo.operatingSystem);
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): form data");
				StringBuilder dataLog = new StringBuilder();
				dataLog.AppendLine("app_version: " + appVersion);
				dataLog.AppendLine("device_token: " + deviceToken);
				dataLog.AppendLine("uniq_id: " + playerId);
				dataLog.AppendLine("is_paying: " + isPayingPlayer);
				dataLog.AppendLine("last_payment_date: " + dateLastPaying);
				dataLog.AppendLine("utc_shift: " + timeUtcOffsetString);
				dataLog.AppendLine("coins: " + countMoney);
				dataLog.AppendLine("gems: " + countGems);
				dataLog.AppendLine("level: " + playerLevel);
				dataLog.AppendLine("language: " + languageCode);
				dataLog.AppendLine("androidSdkLevel: " + androidSdkLevel);
				Debug.Log(dataLog.ToString());
			}
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", FriendsController.HashForPush(form.data));
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Trying to send device token to server: " + deviceToken);
			}
			WWW request = Tools.CreateWwwIf(true, this.UrlPushNotificationServer, form, "RemotePushNotificationController.ReciveUpdateDataToServer()", headers);
			if (request == null)
			{
				yield break;
			}
			yield return request;
			try
			{
				if (!string.IsNullOrEmpty(request.error))
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): error = " + request.error);
					}
					yield break;
				}
				if (!string.IsNullOrEmpty(request.text))
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.Log("RemotePushNotificationController(ReciveDeviceTokenToServer): request.text = " + request.text);
					}
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
					{
						RemotePushRegistrationMemento remotePushRegistrationMemento = new RemotePushRegistrationMemento(deviceToken, DateTime.UtcNow, GlobalGameController.AppVersion);
						string remotePushRegistrationJson = JsonUtility.ToJson(remotePushRegistrationMemento);
						if (Defs.IsDeveloperBuild)
						{
							Debug.LogFormat("Saving remote push registration: '{0}'", new object[]
							{
								remotePushRegistrationJson
							});
						}
						PlayerPrefs.SetString("RemotePushRegistration", remotePushRegistrationJson);
					}
				}
			}
			finally
			{
				this._isResponceRuning = false;
			}
		}
		yield break;
	}

	// Token: 0x04002088 RID: 8328
	private const string RemotePushRegistrationKey = "RemotePushRegistration";

	// Token: 0x04002089 RID: 8329
	public static RemotePushNotificationController Instance;

	// Token: 0x0400208A RID: 8330
	private bool _isResponceRuning;

	// Token: 0x0400208B RID: 8331
	private bool _isStartUpdateRecive;
}
