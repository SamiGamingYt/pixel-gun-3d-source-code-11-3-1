using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002F0 RID: 752
internal sealed class LevelCompleteLoader : MonoBehaviour
{
	// Token: 0x06001A37 RID: 6711 RVA: 0x00069F64 File Offset: 0x00068164
	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = true;
		if (!LevelCompleteLoader.sceneName.Equals("LevelComplete"))
		{
			string path = ConnectSceneNGUIController.MainLoadingTexture();
			this.fon = Resources.Load<Texture>(path);
		}
		else
		{
			string path2 = "LevelLoadings" + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi") + "/LevelComplete_back";
			if (Defs.IsSurvival)
			{
				path2 = "GameOver_Coliseum";
			}
			this.fon = Resources.Load<Texture>(path2);
		}
		GameObject gameObject = new GameObject();
		UITexture uitexture = gameObject.AddComponent<UITexture>();
		uitexture.mainTexture = this.fon;
		uitexture.SetRect(0f, 0f, 1366f, 768f);
		uitexture.transform.SetParent(this.myUICam.transform, false);
		uitexture.transform.localScale = Vector3.one;
		uitexture.transform.localPosition = Vector3.zero;
		base.StartCoroutine(this.loadNext());
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0006A074 File Offset: 0x00068274
	private IEnumerator loadNext()
	{
		yield return new WaitForSeconds(0.25f);
		SceneManager.LoadScene(LevelCompleteLoader.sceneName);
		yield break;
	}

	// Token: 0x04000F57 RID: 3927
	public static Action action;

	// Token: 0x04000F58 RID: 3928
	public static string sceneName = string.Empty;

	// Token: 0x04000F59 RID: 3929
	private Texture fon;

	// Token: 0x04000F5A RID: 3930
	public UICamera myUICam;

	// Token: 0x04000F5B RID: 3931
	private Texture loadingNote;
}
