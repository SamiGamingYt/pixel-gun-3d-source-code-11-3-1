using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class CleanUpAndDoAction : MonoBehaviour
{
	// Token: 0x0600032D RID: 813 RVA: 0x0001B754 File Offset: 0x00019954
	private IEnumerator Start()
	{
		Action handler = CleanUpAndDoAction.action;
		if (ShopNGUIController.GuiActive)
		{
			ShopNGUIController.GuiActive = false;
		}
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.Disconnect();
		yield return null;
		yield return null;
		WeaponManager.sharedManager.UnloadAll();
		yield return null;
		yield return null;
		if (handler != null)
		{
			handler();
		}
		CleanUpAndDoAction.action = null;
		while (FacebookController.LoggingIn)
		{
			yield return null;
		}
		int i = 0;
		while (i < 60)
		{
			i++;
			yield return null;
		}
		while (FacebookController.LoggingIn)
		{
			yield return null;
		}
		Application.LoadLevel(Defs.MainMenuScene);
		yield break;
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0001B768 File Offset: 0x00019968
	private void OnGUI()
	{
		GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), this.blackPixel, ScaleMode.StretchToFill);
		GUI.DrawTexture(AppsMenu.RiliFonRect(), this.riliFon, ScaleMode.StretchToFill);
	}

	// Token: 0x0400036C RID: 876
	public Texture riliFon;

	// Token: 0x0400036D RID: 877
	public Texture blackPixel;

	// Token: 0x0400036E RID: 878
	public static Action action;
}
