using System;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public class SendSmileButtonHundler : MonoBehaviour
{
	// Token: 0x060045C0 RID: 17856 RVA: 0x001792C4 File Offset: 0x001774C4
	private void Awake()
	{
		this.smileName = base.GetComponent<UISprite>().spriteName;
	}

	// Token: 0x060045C1 RID: 17857 RVA: 0x001792D8 File Offset: 0x001774D8
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (PrivateChatController.sharedController != null)
		{
			PrivateChatController.sharedController.SendSmile(this.smileName);
		}
		if (ChatViewrController.sharedController != null && InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SendChat(string.Empty, false, this.smileName);
			ChatViewrController.sharedController.HideSmilePannelOnClick();
		}
	}

	// Token: 0x04003327 RID: 13095
	private string smileName = string.Empty;
}
