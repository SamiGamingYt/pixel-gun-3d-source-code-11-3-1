using System;
using UnityEngine;

// Token: 0x02000798 RID: 1944
public class SendInvitationsButton : MonoBehaviour
{
	// Token: 0x060045BE RID: 17854 RVA: 0x0017928C File Offset: 0x0017748C
	private void OnClick()
	{
		if (FacebookController.FacebookSupported)
		{
			FacebookController.sharedController.InvitePlayer(null);
		}
		ButtonClickSound.Instance.PlayClick();
	}
}
