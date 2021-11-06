using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000160 RID: 352
[Obsolete]
internal sealed class GoToMainNeuFromFriends : MonoBehaviour
{
	// Token: 0x06000B80 RID: 2944 RVA: 0x00040C68 File Offset: 0x0003EE68
	private void HandleClick()
	{
		ButtonClickSound.Instance.PlayClick();
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
		LoadConnectScene.noteToShow = null;
		Singleton<SceneLoader>.Instance.LoadScene(Defs.PromSceneName, LoadSceneMode.Single);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00040CAC File Offset: 0x0003EEAC
	private void OnPress(bool isDown)
	{
		if (isDown)
		{
			this.firstFrame = false;
		}
		else
		{
			if (this.firstFrame)
			{
				return;
			}
			this.HandleClick();
		}
	}

	// Token: 0x0400091D RID: 2333
	private bool firstFrame = true;
}
