using System;
using UnityEngine;

// Token: 0x020003C4 RID: 964
public class NetworkStartTableStartPlashka : MonoBehaviour
{
	// Token: 0x0600232B RID: 9003 RVA: 0x000AE68C File Offset: 0x000AC88C
	private void Start()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
		{
			base.gameObject.SetActive(false);
			this.plashka.SetActive(false);
		}
		else if (Defs.isCOOP)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0555;
		}
		else if (Defs.isHunger)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0556;
		}
		else if (Defs.isDaterRegim)
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Get("Key_1539");
		}
		else
		{
			base.GetComponent<UILabel>().text = LocalizationStore.Key_0557;
		}
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000AE74C File Offset: 0x000AC94C
	private void Update()
	{
	}

	// Token: 0x04001767 RID: 5991
	public GameObject plashka;
}
