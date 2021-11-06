using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public class CancelClanInvitaionPressed : MonoBehaviour
{
	// Token: 0x0600023E RID: 574 RVA: 0x00014194 File Offset: 0x00012394
	private void OnClick()
	{
		FriendsController.sharedController.clanCancelledInvitesLocal.Add(base.transform.parent.GetComponent<FriendPreview>().id);
		FriendsController.sharedController.RejectClanInvite(FriendsController.sharedController.ClanID, base.transform.parent.GetComponent<FriendPreview>().id);
	}
}
