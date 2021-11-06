using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x0200087A RID: 2170
internal sealed class UpdatesChecker : MonoBehaviour
{
	// Token: 0x06004E65 RID: 20069 RVA: 0x001C6914 File Offset: 0x001C4B14
	private IEnumerator CheckUpdatesCoroutine(UpdatesChecker.Store store)
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			yield return null;
		}
		string version = string.Format("{0}:{1}", (int)store, GlobalGameController.AppVersion);
		if (Application.isEditor)
		{
			Debug.LogFormat("Sending version: {0}", new object[]
			{
				version
			});
		}
		WWWForm form = new WWWForm();
		form.AddField("action", "check_shop_version");
		form.AddField("app_version", version);
		WWW request = Tools.CreateWwwIfNotConnected("https://pixelgunserver.com/~rilisoft/action.php", form, string.Empty, null);
		if (request == null)
		{
			yield break;
		}
		yield return request;
		if (!string.IsNullOrEmpty(request.error))
		{
			Debug.LogWarningFormat("Error while receiving version: {0}", new object[]
			{
				request.error
			});
			yield break;
		}
		string response = URLs.Sanitize(request);
		if (string.IsNullOrEmpty(response))
		{
			Debug.Log("response is empty");
			yield break;
		}
		if (Application.isEditor)
		{
			Debug.Log("UpdatesChecker: " + response);
		}
		if (response.Equals("no"))
		{
			GlobalGameController.NewVersionAvailable = true;
			Debug.Log("NewVersionAvailable: true");
		}
		yield break;
	}

	// Token: 0x06004E66 RID: 20070 RVA: 0x001C6938 File Offset: 0x001C4B38
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._currentStore = UpdatesChecker.Store.Unknown;
		RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
		switch (buildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			this._currentStore = UpdatesChecker.Store.Ios;
			break;
		default:
			if (buildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				this._currentStore = UpdatesChecker.Store.Wp8;
			}
			break;
		case RuntimePlatform.Android:
		{
			Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
			if (androidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				if (androidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					this._currentStore = UpdatesChecker.Store.Play;
				}
			}
			else
			{
				this._currentStore = UpdatesChecker.Store.Amazon;
			}
			break;
		}
		}
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001C69D0 File Offset: 0x001C4BD0
	private void Start()
	{
		base.StartCoroutine(this.CheckUpdatesCoroutine(this._currentStore));
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001C69E8 File Offset: 0x001C4BE8
	private void OnApplicationPause(bool pause)
	{
		if (Application.isEditor)
		{
			Debug.Log(">>> UpdatesChecker.OnApplicationPause()");
		}
		if (!pause)
		{
			base.StartCoroutine(this.CheckUpdatesCoroutine(this._currentStore));
		}
	}

	// Token: 0x04003CFB RID: 15611
	private const string ActionAddress = "https://pixelgunserver.com/~rilisoft/action.php";

	// Token: 0x04003CFC RID: 15612
	private UpdatesChecker.Store _currentStore;

	// Token: 0x0200087B RID: 2171
	private enum Store
	{
		// Token: 0x04003CFE RID: 15614
		Ios,
		// Token: 0x04003CFF RID: 15615
		Play,
		// Token: 0x04003D00 RID: 15616
		Wp8,
		// Token: 0x04003D01 RID: 15617
		Amazon,
		// Token: 0x04003D02 RID: 15618
		Unknown
	}
}
