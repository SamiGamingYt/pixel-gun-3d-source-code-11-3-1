using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public sealed class ProtocolListGetter : MonoBehaviour
{
	// Token: 0x17000772 RID: 1906
	// (get) Token: 0x06002B19 RID: 11033 RVA: 0x000E2D48 File Offset: 0x000E0F48
	public static int CurrentPlatform
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return 0;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return 1;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return 2;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return 3;
			}
			return 101;
		}
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x000E2DB4 File Offset: 0x000E0FB4
	private IEnumerator Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Storager.hasKey(this.CurrentVersionSupportedKey))
		{
			Storager.setInt(this.CurrentVersionSupportedKey, 1, false);
		}
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		ProtocolListGetter.currentVersionIsSupported = (Storager.getInt(this.CurrentVersionSupportedKey, false) == 1);
		WaitForSeconds waitForSeconds = new WaitForSeconds(10f);
		string appVersionField = ProtocolListGetter.CurrentPlatform + ":" + GlobalGameController.AppVersion;
		WWWForm form = new WWWForm();
		form.AddField("action", "check_version");
		form.AddField("app_version", appVersionField);
		string response;
		for (;;)
		{
			WWW download = Tools.CreateWwwIfNotConnected(FriendsController.actionAddress, form, string.Empty, null);
			if (download == null)
			{
				yield return waitForSeconds;
			}
			else
			{
				yield return download;
				response = URLs.Sanitize(download);
				if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response) && Debug.isDebugBuild)
				{
					Debug.Log(response);
				}
				if (!string.IsNullOrEmpty(download.error))
				{
					if (Debug.isDebugBuild)
					{
						Debug.LogWarning("ProtocolListGetter error: " + download.error);
					}
					yield return waitForSeconds;
				}
				else
				{
					if (string.IsNullOrEmpty(download.error) && !string.IsNullOrEmpty(response))
					{
						break;
					}
					yield return waitForSeconds;
				}
			}
		}
		if ("no".Equals(response))
		{
			ProtocolListGetter.currentVersionIsSupported = false;
			Storager.setInt(this.CurrentVersionSupportedKey, 0, false);
		}
		else
		{
			ProtocolListGetter.currentVersionIsSupported = true;
			Storager.setInt(this.CurrentVersionSupportedKey, 1, false);
		}
		yield break;
	}

	// Token: 0x04002029 RID: 8233
	public static bool currentVersionIsSupported = true;

	// Token: 0x0400202A RID: 8234
	private string CurrentVersionSupportedKey = "CurrentVersionSupportedKey" + GlobalGameController.AppVersion;
}
