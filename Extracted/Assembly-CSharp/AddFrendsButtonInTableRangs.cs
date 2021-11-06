using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000009 RID: 9
internal sealed class AddFrendsButtonInTableRangs : MonoBehaviour
{
	// Token: 0x06000024 RID: 36 RVA: 0x00002E4C File Offset: 0x0000104C
	private void OnPress(bool isDown)
	{
		if (!isDown)
		{
			Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
			{
				{
					"Added Friends",
					"AddFrendsButtonInTableRangs"
				},
				{
					"Deleted Friends",
					"Add"
				}
			};
			FriendsController.sharedController.SendInvitation(this.ID.ToString(), socialEventParameters);
			if (!FriendsController.sharedController.notShowAddIds.Contains(this.ID.ToString()))
			{
				FriendsController.sharedController.notShowAddIds.Add(this.ID.ToString());
			}
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000020 RID: 32
	public int ID;
}
