using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020003C7 RID: 967
public class NewMessageIconController : MonoBehaviour
{
	// Token: 0x06002332 RID: 9010 RVA: 0x000AE8D0 File Offset: 0x000ACAD0
	private void Start()
	{
		this.UpdateStateNewMessage();
	}

	// Token: 0x06002333 RID: 9011 RVA: 0x000AE8D8 File Offset: 0x000ACAD8
	private void UpdateStateNewMessage()
	{
		bool flag = false;
		int num = 0;
		if (this.privateMessageFriends && ChatController.countNewPrivateMessage > 0)
		{
			flag = true;
			num += ChatController.countNewPrivateMessage;
		}
		if (this.inviteToFriends)
		{
			HashSet<string> hashSet = BattleInviteListener.Instance.GetFriendIds() as HashSet<string>;
			if (hashSet != null && hashSet.Count > 0)
			{
				num++;
				flag = true;
			}
			else if (FriendsController.sharedController.invitesToUs.Count > 0)
			{
				for (int i = 0; i < FriendsController.sharedController.invitesToUs.Count; i++)
				{
					string key = FriendsController.sharedController.invitesToUs[i];
					if (FriendsController.sharedController.friendsInfo.ContainsKey(key))
					{
						flag = true;
						num++;
					}
					else if (FriendsController.sharedController.clanFriendsInfo.ContainsKey(key))
					{
						flag = true;
						num++;
					}
					else if (FriendsController.sharedController.profileInfo.ContainsKey(key))
					{
						flag = true;
						num++;
					}
				}
			}
		}
		if (this.newMessageSprite != null && flag != this.newMessageSprite.activeSelf)
		{
			this.newMessageSprite.SetActive(flag);
		}
		if (this.countLabel != null)
		{
			this.countLabel.text = num.ToString();
		}
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000AEA3C File Offset: 0x000ACC3C
	private void Update()
	{
		this.UpdateStateNewMessage();
	}

	// Token: 0x04001775 RID: 6005
	public bool privateMessageFriends;

	// Token: 0x04001776 RID: 6006
	public bool inviteToFriends;

	// Token: 0x04001777 RID: 6007
	public bool inviteToClan;

	// Token: 0x04001778 RID: 6008
	public bool clanMessages;

	// Token: 0x04001779 RID: 6009
	public bool privateMessageClan;

	// Token: 0x0400177A RID: 6010
	public GameObject newMessageSprite;

	// Token: 0x0400177B RID: 6011
	public UILabel countLabel;
}
