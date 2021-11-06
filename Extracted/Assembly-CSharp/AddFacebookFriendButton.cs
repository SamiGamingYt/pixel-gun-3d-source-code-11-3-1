using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000008 RID: 8
internal sealed class AddFacebookFriendButton : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x00002D90 File Offset: 0x00000F90
	private void OnClick()
	{
		FriendPreview component = base.transform.parent.GetComponent<FriendPreview>();
		ButtonClickSound.Instance.PlayClick();
		string id = component.id;
		if (id != null)
		{
			if (component.ClanInvite)
			{
				FriendsController.SendPlayerInviteToClan(id, null);
				FriendsController.sharedController.clanSentInvitesLocal.Add(id);
			}
			else
			{
				Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
				{
					{
						"Added Friends",
						"Find Friends: Facebook"
					},
					{
						"Deleted Friends",
						"Add"
					},
					{
						"Search Friends",
						"Add"
					}
				};
				FriendsController.sharedController.SendInvitation(id, socialEventParameters);
			}
		}
		if (!component.ClanInvite)
		{
			component.DisableButtons();
		}
	}
}
