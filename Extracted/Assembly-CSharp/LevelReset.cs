using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Token: 0x020004DF RID: 1247
public class LevelReset : MonoBehaviour, IEventSystemHandler, IPointerClickHandler
{
	// Token: 0x06002C64 RID: 11364 RVA: 0x000EB590 File Offset: 0x000E9790
	public void OnPointerClick(PointerEventData data)
	{
		Singleton<SceneLoader>.Instance.LoadSceneAsync(Application.loadedLevelName, LoadSceneMode.Single);
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000EB5A4 File Offset: 0x000E97A4
	private void Update()
	{
	}
}
