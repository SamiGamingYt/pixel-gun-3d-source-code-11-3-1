using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class BackFromInbox : MonoBehaviour
{
	// Token: 0x060000DC RID: 220 RVA: 0x000080AC File Offset: 0x000062AC
	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().inboxPanel.gameObject.SetActive(false);
		ButtonClickSound.Instance.PlayClick();
	}
}
