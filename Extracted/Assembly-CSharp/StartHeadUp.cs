using System;
using UnityEngine;

// Token: 0x0200081F RID: 2079
public class StartHeadUp : MonoBehaviour
{
	// Token: 0x06004BB1 RID: 19377 RVA: 0x001B3BD8 File Offset: 0x001B1DD8
	private void Start()
	{
		if (Defs.isDaterRegim)
		{
			base.GetComponent<UILabel>().text = string.Empty;
			return;
		}
		if (!Defs.isInet || (Defs.isInet && PhotonNetwork.room != null && !PhotonNetwork.room.customProperties[ConnectSceneNGUIController.passwordProperty].Equals(string.Empty)))
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0560;
		}
		else if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0561;
		}
		else if (Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0562;
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0563;
		}
	}
}
