using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020004DD RID: 1245
public class SceneAndURLLoader : MonoBehaviour
{
	// Token: 0x06002C5D RID: 11357 RVA: 0x000EB4AC File Offset: 0x000E96AC
	private void Awake()
	{
		this.m_PauseMenu = base.GetComponentInChildren<PauseMenu>();
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000EB4BC File Offset: 0x000E96BC
	public void SceneLoad(string sceneName)
	{
		this.m_PauseMenu.MenuOff();
		Singleton<SceneLoader>.Instance.LoadScene(sceneName, LoadSceneMode.Single);
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000EB4D8 File Offset: 0x000E96D8
	public void LoadURL(string url)
	{
		Application.OpenURL(url);
	}

	// Token: 0x0400215E RID: 8542
	private PauseMenu m_PauseMenu;
}
