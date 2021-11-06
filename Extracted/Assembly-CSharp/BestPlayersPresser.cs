using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000030 RID: 48
internal sealed class BestPlayersPresser : MonoBehaviour
{
	// Token: 0x06000166 RID: 358 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000E914 File Offset: 0x0000CB14
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		FriendsGUIController component = NGUITools.GetRoot(base.gameObject).GetComponent<FriendsGUIController>();
		component.friendsPanel.gameObject.SetActive(false);
		component.leaderboardsView.gameObject.SetActive(true);
		component.RequestLeaderboards();
	}
}
