using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public sealed class JoinClanPressed : MonoBehaviour
{
	// Token: 0x0600197A RID: 6522 RVA: 0x000657AC File Offset: 0x000639AC
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		Invitation component = base.transform.parent.GetComponent<Invitation>();
		if (component == null)
		{
			return;
		}
		FriendsController.sharedController.AcceptClanInvite(component.recordId);
		component.DisableButtons();
		component.KeepClanData();
	}
}
