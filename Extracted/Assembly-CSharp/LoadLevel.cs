using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020002F8 RID: 760
public class LoadLevel : MonoBehaviour
{
	// Token: 0x06001A65 RID: 6757 RVA: 0x0006AB1C File Offset: 0x00068D1C
	private void Start()
	{
		Singleton<SceneLoader>.Instance.LoadScene("Level3", LoadSceneMode.Single);
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x0006AB30 File Offset: 0x00068D30
	private void OnGUI()
	{
		Rect position = new Rect(((float)Screen.width - 1366f * Defs.Coef) / 2f, 0f, 1366f * Defs.Coef, (float)Screen.height);
		GUI.DrawTexture(position, this.fon, ScaleMode.StretchToFill);
	}

	// Token: 0x04000F7F RID: 3967
	public Texture fon;
}
