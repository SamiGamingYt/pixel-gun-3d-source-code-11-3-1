using System;
using UnityEngine;

// Token: 0x02000476 RID: 1142
public sealed class PixelView : MonoBehaviour
{
	// Token: 0x17000701 RID: 1793
	// (get) Token: 0x060027D7 RID: 10199 RVA: 0x000C6EF8 File Offset: 0x000C50F8
	public int viewID
	{
		get
		{
			if (!Defs.isMulti)
			{
				return 0;
			}
			if (Defs.isInet)
			{
				return this.photonView.viewID;
			}
			return this.localViewID;
		}
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000C6F30 File Offset: 0x000C5130
	private void Awake()
	{
		if (!Defs.isMulti)
		{
			return;
		}
		if (Defs.isInet)
		{
			this.photonView = base.GetComponent<PhotonView>();
			if (this.photonView == null)
			{
				Debug.LogError("GetComponent<PhotonView>() == null");
			}
		}
		else
		{
			this._networkView = base.GetComponent<NetworkView>();
			if (Network.isServer)
			{
				this._networkView.RPC("SendViewID", RPCMode.AllBuffered, new object[]
				{
					PixelView.viewIdCount++
				});
			}
		}
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000C6FC0 File Offset: 0x000C51C0
	[RPC]
	private void SendViewID(int id)
	{
		if (this.localViewID != -1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Local id is already set! ",
				this.localViewID,
				" (new: ",
				id,
				")"
			}));
		}
		this.localViewID = id;
	}

	// Token: 0x04001C18 RID: 7192
	private static int viewIdCount = 1000;

	// Token: 0x04001C19 RID: 7193
	private PhotonView photonView;

	// Token: 0x04001C1A RID: 7194
	private NetworkView _networkView;

	// Token: 0x04001C1B RID: 7195
	private int localViewID = -1;
}
