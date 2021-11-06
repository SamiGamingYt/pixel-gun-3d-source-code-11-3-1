using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000611 RID: 1553
internal sealed class FriendProfileExample : MonoBehaviour
{
	// Token: 0x0600353B RID: 13627 RVA: 0x00113C74 File Offset: 0x00111E74
	private void Start()
	{
		if (this.friendProfileView != null)
		{
			this.friendProfileView.Reset();
			this.friendProfileView.IsCanConnectToFriend = true;
			this.friendProfileView.FriendLocation = "Deathmatch/Bridge";
			this.friendProfileView.FriendCount = 42;
			this.friendProfileView.FriendName = "Дуэйн «Rock» Джонсон";
			this.friendProfileView.Online = OnlineState.playing;
			this.friendProfileView.Rank = 4;
			this.friendProfileView.SurvivalScore = 4376;
			this.friendProfileView.Username = "John Doe";
			this.friendProfileView.WinCount = 13;
			this.friendProfileView.SetBoots("boots_blue");
			this.friendProfileView.SetHat("hat_KingsCrown");
			this.friendProfileView.SetStockCape("cape_BloodyDemon");
		}
	}

	// Token: 0x04002709 RID: 9993
	public FriendProfileView friendProfileView;
}
