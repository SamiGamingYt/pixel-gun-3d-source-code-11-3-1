using System;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public class EnterPasswordInFriensJoinButton : MonoBehaviour
{
	// Token: 0x06000532 RID: 1330 RVA: 0x0002A168 File Offset: 0x00028368
	private void Start()
	{
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0002A16C File Offset: 0x0002836C
	private void OnClick()
	{
		if (this.joinRoomFromFrends == null)
		{
			this.joinRoomFromFrends = JoinRoomFromFrends.sharedJoinRoomFromFrends;
		}
		if (this.joinRoomFromFrends != null)
		{
			this.joinRoomFromFrends.EnterPassword(this.passwordLabel.text);
		}
	}

	// Token: 0x040005AE RID: 1454
	public UILabel passwordLabel;

	// Token: 0x040005AF RID: 1455
	public JoinRoomFromFrends joinRoomFromFrends;
}
