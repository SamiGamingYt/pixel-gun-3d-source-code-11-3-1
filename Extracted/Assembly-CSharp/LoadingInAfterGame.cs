using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002F9 RID: 761
public class LoadingInAfterGame : MonoBehaviour
{
	// Token: 0x170004AD RID: 1197
	// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0006AB94 File Offset: 0x00068D94
	private bool ShouldShowLoading
	{
		get
		{
			return this.timerShow > 0f && !(LoadingInAfterGame.loadingTexture == null) && Defs.isMulti && !Defs.isHunger;
		}
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x0006ABCC File Offset: 0x00068DCC
	private void Awake()
	{
		if (this.ShouldShowLoading)
		{
			this._loadingNGUIController = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("LoadingGUI")).GetComponent<LoadingNGUIController>();
			this._loadingNGUIController.SceneToLoad = SceneManager.GetActiveScene().name;
			this._loadingNGUIController.loadingNGUITexture.mainTexture = LoadingInAfterGame.loadingTexture;
			this._loadingNGUIController.transform.localPosition = Vector3.zero;
			this._loadingNGUIController.Init();
			LoadingInAfterGame.isShowLoading = true;
		}
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x0006AC54 File Offset: 0x00068E54
	private void Update()
	{
		if (this.timerShow > 0f)
		{
			this.timerShow -= Time.deltaTime;
		}
		if (!ActivityIndicator.IsActiveIndicator)
		{
			ActivityIndicator.IsActiveIndicator = true;
		}
		if (!this.ShouldShowLoading)
		{
			LoadingInAfterGame.isShowLoading = false;
			base.enabled = false;
			LoadingInAfterGame.loadingTexture = null;
			ActivityIndicator.IsActiveIndicator = false;
			if (this._loadingNGUIController != null)
			{
				UnityEngine.Object.Destroy(this._loadingNGUIController.gameObject);
				this._loadingNGUIController = null;
				Resources.UnloadUnusedAssets();
			}
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x0006ACE8 File Offset: 0x00068EE8
	private void OnDestroy()
	{
		LoadingInAfterGame.loadingTexture = null;
		LoadingInAfterGame.isShowLoading = false;
	}

	// Token: 0x04000F80 RID: 3968
	public static Texture loadingTexture;

	// Token: 0x04000F81 RID: 3969
	public static bool isShowLoading;

	// Token: 0x04000F82 RID: 3970
	private float timerShow = 2f;

	// Token: 0x04000F83 RID: 3971
	private LoadingNGUIController _loadingNGUIController;
}
