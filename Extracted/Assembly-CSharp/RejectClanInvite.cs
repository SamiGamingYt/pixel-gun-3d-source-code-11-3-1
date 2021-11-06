using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class RejectClanInvite : MonoBehaviour
{
	// Token: 0x06002B80 RID: 11136 RVA: 0x000E5348 File Offset: 0x000E3548
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsController.sharedController.RejectClanInvite(base.transform.parent.GetComponent<Invitation>().recordId, null);
		base.transform.parent.GetComponent<Invitation>().DisableButtons();
	}
}
