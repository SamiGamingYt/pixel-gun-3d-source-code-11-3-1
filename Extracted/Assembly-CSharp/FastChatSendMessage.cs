using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class FastChatSendMessage : MonoBehaviour
{
	// Token: 0x0600080D RID: 2061 RVA: 0x00030B40 File Offset: 0x0002ED40
	private void Awake()
	{
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00030B44 File Offset: 0x0002ED44
	private void OnClick()
	{
		if (InGameGUI.sharedInGameGUI.playerMoveC != null)
		{
			InGameGUI.sharedInGameGUI.playerMoveC.SendChat(this.message, false, string.Empty);
			if (ChatViewrController.sharedController)
			{
				ChatViewrController.sharedController.CloseChat(false);
			}
		}
	}

	// Token: 0x040006A6 RID: 1702
	public string message = "-=GO!=-";

	// Token: 0x040006A7 RID: 1703
	public UISprite iconSprite;
}
