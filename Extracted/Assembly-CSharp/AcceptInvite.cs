using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000003 RID: 3
internal sealed class AcceptInvite : MonoBehaviour
{
	// Token: 0x06000004 RID: 4 RVA: 0x000021E4 File Offset: 0x000003E4
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component == null)
		{
			Debug.LogWarning("invitation == null");
			return;
		}
		FriendsController.sharedController.AcceptInvite(component.id, null);
		component.DisableButtons();
	}
}
