using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200073B RID: 1851
public class RestartOnTap : MonoBehaviour
{
	// Token: 0x06004117 RID: 16663 RVA: 0x0015B82C File Offset: 0x00159A2C
	private void Start()
	{
	}

	// Token: 0x06004118 RID: 16664 RVA: 0x0015B830 File Offset: 0x00159A30
	private void Update()
	{
		if (Input.touchCount > 0)
		{
			Singleton<SceneLoader>.Instance.LoadScene("Level2", LoadSceneMode.Single);
		}
	}
}
