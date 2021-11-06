using System;
using System.Reflection;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002F7 RID: 759
public sealed class LoadConnectScene : MonoBehaviour
{
	// Token: 0x06001A5E RID: 6750 RVA: 0x0006A9AC File Offset: 0x00068BAC
	private void Awake()
	{
		this.loading = LoadConnectScene.textureToShow;
		if (this.loading == null)
		{
			string path = ConnectSceneNGUIController.MainLoadingTexture();
			this.loading = Resources.Load<Texture>(path);
		}
		this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
		this._loadingNGUIController.SceneToLoad = LoadConnectScene.sceneToLoad;
		this._loadingNGUIController.loadingNGUITexture.mainTexture = this.loading;
		this._loadingNGUIController.Init();
		NotificationController.instance.SaveTimeValues();
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x0006AA3C File Offset: 0x00068C3C
	private void Start()
	{
		LoadConnectScene.Instance = this;
		if (LoadConnectScene.interval != -1f)
		{
			base.Invoke("_loadConnectScene", LoadConnectScene.interval);
		}
		LoadConnectScene.interval = LoadConnectScene._defaultInterval;
		ActivityIndicator.IsActiveIndicator = true;
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x0006AA74 File Offset: 0x00068C74
	private void OnGUI()
	{
		ActivityIndicator.IsActiveIndicator = true;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0006AA7C File Offset: 0x00068C7C
	[Obfuscation(Exclude = true)]
	private void _loadConnectScene()
	{
		if (LoadConnectScene.sceneToLoad.Equals("ConnectScene"))
		{
			Singleton<SceneLoader>.Instance.LoadScene(LoadConnectScene.sceneToLoad, LoadSceneMode.Single);
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadSceneAsync(LoadConnectScene.sceneToLoad, LoadSceneMode.Single);
		}
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0006AAC4 File Offset: 0x00068CC4
	public static void LoadScene()
	{
		if (LoadConnectScene.Instance == null)
		{
			return;
		}
		LoadConnectScene.Instance._loadConnectScene();
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x0006AAE4 File Offset: 0x00068CE4
	private void OnDestroy()
	{
		LoadConnectScene.Instance = null;
		if (!LoadConnectScene.sceneToLoad.Equals("ConnectScene"))
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
		this.loading = null;
		LoadConnectScene.textureToShow = null;
	}

	// Token: 0x04000F76 RID: 3958
	public static string sceneToLoad = string.Empty;

	// Token: 0x04000F77 RID: 3959
	public static Texture textureToShow = null;

	// Token: 0x04000F78 RID: 3960
	public static Texture noteToShow = null;

	// Token: 0x04000F79 RID: 3961
	public static float interval = LoadConnectScene._defaultInterval;

	// Token: 0x04000F7A RID: 3962
	public Texture loadingNote;

	// Token: 0x04000F7B RID: 3963
	private static readonly float _defaultInterval = 1f;

	// Token: 0x04000F7C RID: 3964
	private Texture loading;

	// Token: 0x04000F7D RID: 3965
	private LoadingNGUIController _loadingNGUIController;

	// Token: 0x04000F7E RID: 3966
	public static LoadConnectScene Instance;
}
