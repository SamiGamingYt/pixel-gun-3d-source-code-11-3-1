using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200049C RID: 1180
public class PrivateMessageItem : MonoBehaviour
{
	// Token: 0x06002A3C RID: 10812 RVA: 0x000DF878 File Offset: 0x000DDA78
	private void Awake()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06002A3D RID: 10813 RVA: 0x000DF888 File Offset: 0x000DDA88
	private void Start()
	{
		if (this.myTransform.parent != null)
		{
			this.myPanel = base.transform.parent.parent.GetComponent<UIPanel>();
		}
	}

	// Token: 0x06002A3E RID: 10814 RVA: 0x000DF8C8 File Offset: 0x000DDAC8
	public void SetFon(bool isMine)
	{
		this.yourWidget.gameObject.SetActive(isMine);
		this.otherWidget.gameObject.SetActive(!isMine);
	}

	// Token: 0x06002A3F RID: 10815 RVA: 0x000DF8FC File Offset: 0x000DDAFC
	public void SetWidth(int width)
	{
		this.otherWidget.width = width;
		this.yourWidget.width = width;
	}

	// Token: 0x06002A40 RID: 10816 RVA: 0x000DF918 File Offset: 0x000DDB18
	private void Update()
	{
		if (!this.isRead && this.myTransform.localPosition.y >= this.myPanel.clipOffset.y - this.myPanel.baseClipRegion.w * 0.5f + this.myPanel.baseClipRegion.y && this.myTransform.localPosition.y <= this.myPanel.clipOffset.y + this.myPanel.baseClipRegion.w * 0.5f + this.myPanel.baseClipRegion.y)
		{
			Dictionary<string, List<Dictionary<string, string>>> dictionary = new Dictionary<string, List<Dictionary<string, string>>>();
			foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair in ChatController.privateMessages)
			{
				List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
				foreach (ChatController.PrivateMessage privateMessage in keyValuePair.Value)
				{
					list.Add(new Dictionary<string, string>
					{
						{
							"playerIDFrom",
							privateMessage.playerIDFrom
						},
						{
							"message",
							privateMessage.message
						},
						{
							"timeStamp",
							string.Empty
						},
						{
							"isRead",
							privateMessage.isRead.ToString()
						},
						{
							"isSending",
							privateMessage.isSending.ToString()
						}
					});
				}
				dictionary.Add(keyValuePair.Key, list);
			}
			string text = Json.Serialize(dictionary);
			for (int i = 0; i < ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID].Count; i++)
			{
				if (ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][i].timeStamp.ToString("F8", CultureInfo.InvariantCulture).Equals(this.timeStamp) && !ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][i].isRead)
				{
					this.isRead = true;
					ChatController.PrivateMessage value = ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][i];
					value.isRead = true;
					ChatController.countNewPrivateMessage--;
					ChatController.privateMessages[PrivateChatController.sharedController.selectedPlayerID][i] = value;
					PrivateChatController.sharedController.selectedPlayerItem.UpdateCountNewMessage();
					ChatController.SavePrivatMessageInPrefs();
					break;
				}
			}
			Dictionary<string, List<Dictionary<string, string>>> dictionary2 = new Dictionary<string, List<Dictionary<string, string>>>();
			foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair2 in ChatController.privateMessages)
			{
				List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
				foreach (ChatController.PrivateMessage privateMessage2 in keyValuePair2.Value)
				{
					list2.Add(new Dictionary<string, string>
					{
						{
							"playerIDFrom",
							privateMessage2.playerIDFrom
						},
						{
							"message",
							privateMessage2.message
						},
						{
							"timeStamp",
							string.Empty
						},
						{
							"isRead",
							privateMessage2.isRead.ToString()
						},
						{
							"isSending",
							privateMessage2.isSending.ToString()
						}
					});
				}
				dictionary2.Add(keyValuePair2.Key, list2);
			}
		}
	}

	// Token: 0x06002A41 RID: 10817 RVA: 0x000DFD64 File Offset: 0x000DDF64
	private void OnEnable()
	{
	}

	// Token: 0x06002A42 RID: 10818 RVA: 0x000DFD68 File Offset: 0x000DDF68
	private void DetectNew()
	{
	}

	// Token: 0x04001F40 RID: 8000
	[Header("Your message obj")]
	public GameObject yourFonSprite;

	// Token: 0x04001F41 RID: 8001
	public UILabel yourMessageLabel;

	// Token: 0x04001F42 RID: 8002
	public UILabel yourTimeLabel;

	// Token: 0x04001F43 RID: 8003
	public UISprite yourSmileSprite;

	// Token: 0x04001F44 RID: 8004
	public UIWidget yourWidget;

	// Token: 0x04001F45 RID: 8005
	[Header("Other message obj")]
	public GameObject otherFonSprite;

	// Token: 0x04001F46 RID: 8006
	public UILabel otherMessageLabel;

	// Token: 0x04001F47 RID: 8007
	public UILabel otherTimeLabel;

	// Token: 0x04001F48 RID: 8008
	public UISprite otherSmileSprite;

	// Token: 0x04001F49 RID: 8009
	public UIWidget otherWidget;

	// Token: 0x04001F4A RID: 8010
	[NonSerialized]
	public bool isRead;

	// Token: 0x04001F4B RID: 8011
	public string timeStamp;

	// Token: 0x04001F4C RID: 8012
	private UIPanel myPanel;

	// Token: 0x04001F4D RID: 8013
	private Transform myTransform;

	// Token: 0x04001F4E RID: 8014
	public int myWrapIndex;
}
