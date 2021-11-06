using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class BackButtonFromEnterPasswordInFriensJoin : MonoBehaviour
{
	// Token: 0x060000D5 RID: 213 RVA: 0x00007FAC File Offset: 0x000061AC
	private void Start()
	{
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x00007FB0 File Offset: 0x000061B0
	private void OnClick()
	{
		if (JoinRoomFromFrends.sharedJoinRoomFromFrends != null)
		{
			JoinRoomFromFrends.sharedJoinRoomFromFrends.BackFromPasswordButton();
		}
	}
}
