using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020004DC RID: 1244
public class ReturnToMainMenu : MonoBehaviour
{
	// Token: 0x06002C58 RID: 11352 RVA: 0x000EB438 File Offset: 0x000E9638
	public void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000EB440 File Offset: 0x000E9640
	private void OnLevelWasLoaded(int level)
	{
		this.m_Levelloaded = true;
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x000EB44C File Offset: 0x000E964C
	private void Update()
	{
		if (this.m_Levelloaded)
		{
			Canvas component = base.gameObject.GetComponent<Canvas>();
			component.enabled = false;
			component.enabled = true;
			this.m_Levelloaded = false;
		}
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000EB488 File Offset: 0x000E9688
	public void GoBackToMainMenu()
	{
		Debug.Log("going back to main menu");
		Singleton<SceneLoader>.Instance.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	// Token: 0x0400215D RID: 8541
	private bool m_Levelloaded;
}
