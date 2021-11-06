using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
[Obsolete]
public sealed class HideInbox : MonoBehaviour
{
	// Token: 0x06001533 RID: 5427 RVA: 0x000541E8 File Offset: 0x000523E8
	private void OnClick()
	{
		FriendsGUIController component = NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>();
		if (component != null)
		{
			component.inboxPanel.gameObject.SetActive(false);
			component.friendsPanel.gameObject.SetActive(true);
		}
		ButtonClickSound.Instance.PlayClick();
	}
}
