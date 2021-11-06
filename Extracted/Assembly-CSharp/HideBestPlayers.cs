using System;
using UnityEngine;

// Token: 0x0200029C RID: 668
internal sealed class HideBestPlayers : MonoBehaviour
{
	// Token: 0x0600152F RID: 5423 RVA: 0x00054150 File Offset: 0x00052350
	private void OnClick()
	{
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().leaderboardsView.gameObject.SetActive(false);
		NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>().friendsPanel.gameObject.SetActive(true);
		ButtonClickSound.Instance.PlayClick();
	}
}
