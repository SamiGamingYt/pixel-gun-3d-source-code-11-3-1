using System;
using UnityEngine;

// Token: 0x020002CC RID: 716
public class InboxPresser : MonoBehaviour
{
	// Token: 0x06001915 RID: 6421 RVA: 0x00061B6C File Offset: 0x0005FD6C
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsGUIController component = NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>();
		if (component != null)
		{
			component.friendsPanel.gameObject.SetActive(false);
			component.inboxPanel.gameObject.SetActive(true);
			component.invitationsGrid.Reposition();
			component.sentInvitationsGrid.Reposition();
		}
	}
}
