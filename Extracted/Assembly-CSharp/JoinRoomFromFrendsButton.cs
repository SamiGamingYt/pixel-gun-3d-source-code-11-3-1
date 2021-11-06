using System;
using UnityEngine;

// Token: 0x020002DB RID: 731
public sealed class JoinRoomFromFrendsButton : MonoBehaviour
{
	// Token: 0x0600199A RID: 6554 RVA: 0x00066444 File Offset: 0x00064644
	private void OnClick()
	{
		ButtonClickSound.Instance.PlayClick();
		string id = base.transform.parent.GetComponent<FriendPreview>().id;
		if (FriendsController.sharedController.onlineInfo.ContainsKey(id))
		{
			int game_mode = int.Parse(FriendsController.sharedController.onlineInfo[id]["game_mode"]);
			string room_name = FriendsController.sharedController.onlineInfo[id]["room_name"];
			string text = FriendsController.sharedController.onlineInfo[id]["map"];
			if (this.joinRoomFromFrends == null)
			{
				this.joinRoomFromFrends = JoinRoomFromFrends.sharedJoinRoomFromFrends;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(text));
			if (infoScene != null)
			{
				this.joinRoomFromFrends.ConnectToRoom(game_mode, room_name, text);
			}
		}
	}

	// Token: 0x04000EB4 RID: 3764
	public JoinRoomFromFrends joinRoomFromFrends;
}
