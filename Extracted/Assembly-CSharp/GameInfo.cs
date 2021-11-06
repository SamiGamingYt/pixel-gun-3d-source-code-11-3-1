using System;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class GameInfo : MonoBehaviour
{
	// Token: 0x06000B53 RID: 2899 RVA: 0x000402AC File Offset: 0x0003E4AC
	private void Start()
	{
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x000402B0 File Offset: 0x0003E4B0
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (ConnectSceneNGUIController.sharedController != null)
		{
			if (Defs.isInet)
			{
				ConnectSceneNGUIController.sharedController.JoinToRoomPhoton(this.roomInfo);
			}
			else
			{
				ConnectSceneNGUIController.sharedController.JoinToLocalRoom(this.roomInfoLocal);
			}
		}
	}

	// Token: 0x04000901 RID: 2305
	public GameObject openSprite;

	// Token: 0x04000902 RID: 2306
	public GameObject closeSprite;

	// Token: 0x04000903 RID: 2307
	public SetHeadLabelText mapNameLabel;

	// Token: 0x04000904 RID: 2308
	public UITexture mapTexture;

	// Token: 0x04000905 RID: 2309
	public UILabel countPlayersLabel;

	// Token: 0x04000906 RID: 2310
	public UILabel serverNameLabel;

	// Token: 0x04000907 RID: 2311
	public RoomInfo roomInfo;

	// Token: 0x04000908 RID: 2312
	public LANBroadcastService.ReceivedMessage roomInfoLocal;

	// Token: 0x04000909 RID: 2313
	public int index;

	// Token: 0x0400090A RID: 2314
	public GameObject[] SizeMapNameLbl;
}
