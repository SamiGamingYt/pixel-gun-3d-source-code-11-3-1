using System;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class SpectorModeOnOffBtn : MonoBehaviour
{
	// Token: 0x06004B86 RID: 19334 RVA: 0x001B280C File Offset: 0x001B0A0C
	private void OnClick()
	{
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		if (!this.isOnBtn)
		{
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				NetworkStartTableNGUIController.sharedController.StartSpectatorMode();
			}
		}
		else if (NetworkStartTableNGUIController.sharedController != null)
		{
			NetworkStartTableNGUIController.sharedController.EndSpectatorMode();
		}
	}

	// Token: 0x04003A9C RID: 15004
	public bool isOnBtn;
}
