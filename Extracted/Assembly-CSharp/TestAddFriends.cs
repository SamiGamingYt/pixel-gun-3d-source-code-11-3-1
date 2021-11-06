using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000857 RID: 2135
internal sealed class TestAddFriends : MonoBehaviour
{
	// Token: 0x06004D52 RID: 19794 RVA: 0x001BE7E4 File Offset: 0x001BC9E4
	private void OnClick()
	{
		Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
		{
			{
				"Added Friends",
				"Test"
			},
			{
				"Deleted Friends",
				"Add"
			}
		};
		FriendsController.sharedController.SendInvitation("123", socialEventParameters);
	}
}
