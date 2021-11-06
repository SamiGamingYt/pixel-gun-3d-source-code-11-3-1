using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200006A RID: 106
public class ClansClicked : MonoBehaviour
{
	// Token: 0x060002F3 RID: 755 RVA: 0x00019558 File Offset: 0x00017758
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = Resources.Load<Texture>("Friends_Loading");
		LoadConnectScene.sceneToLoad = "Clans";
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}
}
