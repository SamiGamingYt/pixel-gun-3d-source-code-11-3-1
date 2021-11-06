using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000121 RID: 289
public class FriendPrevInChatItem : MonoBehaviour
{
	// Token: 0x0600084D RID: 2125 RVA: 0x00032674 File Offset: 0x00030874
	private void Start()
	{
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00032678 File Offset: 0x00030878
	private void Update()
	{
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003267C File Offset: 0x0003087C
	public void UpdateCountNewMessage()
	{
		int num = 0;
		if (ChatController.privateMessages.ContainsKey(this.playerID))
		{
			List<ChatController.PrivateMessage> list = ChatController.privateMessages[this.playerID];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].isRead)
				{
					num++;
				}
			}
		}
		this.contNewMessage = num;
		if (this.contNewMessage == 0)
		{
			this.newMessageObj.SetActive(false);
		}
		else
		{
			this.newMessageObj.SetActive(true);
			this.countNewMessageLabel.text = this.contNewMessage.ToString();
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00032728 File Offset: 0x00030928
	public void SetActivePlayer()
	{
		if (PrivateChatController.sharedController.selectedPlayerID == this.playerID)
		{
			return;
		}
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		PrivateChatController.sharedController.SetSelectedPlayer(this.playerID, false);
	}

	// Token: 0x040006ED RID: 1773
	public UILabel nickLabel;

	// Token: 0x040006EE RID: 1774
	public UITexture previewTexture;

	// Token: 0x040006EF RID: 1775
	public UISprite rank;

	// Token: 0x040006F0 RID: 1776
	public string playerID;

	// Token: 0x040006F1 RID: 1777
	public GameObject newMessageObj;

	// Token: 0x040006F2 RID: 1778
	public UILabel countNewMessageLabel;

	// Token: 0x040006F3 RID: 1779
	private int contNewMessage;

	// Token: 0x040006F4 RID: 1780
	public int myWrapIndex;
}
