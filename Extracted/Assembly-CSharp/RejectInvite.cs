using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004BE RID: 1214
internal sealed class RejectInvite : MonoBehaviour
{
	// Token: 0x06002B82 RID: 11138 RVA: 0x000E539C File Offset: 0x000E359C
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component != null)
		{
			FriendsController.sharedController.RejectInvite(component.id, null);
			base.transform.parent.GetComponent<Invitation>().DisableButtons();
		}
		else
		{
			Debug.LogWarning("invitation == null");
		}
	}
}
