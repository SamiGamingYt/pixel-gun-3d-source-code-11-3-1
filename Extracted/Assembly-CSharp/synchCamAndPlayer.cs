using System;
using UnityEngine;

// Token: 0x02000898 RID: 2200
internal sealed class synchCamAndPlayer : MonoBehaviour
{
	// Token: 0x06004EFF RID: 20223 RVA: 0x001C9DEC File Offset: 0x001C7FEC
	private void Start()
	{
		this.myTransform = base.transform;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.photonView = base.transform.parent.GetComponent<PhotonView>();
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				this.isMine = base.transform.parent.GetComponent<NetworkView>().isMine;
			}
			else
			{
				this.isMine = this.photonView.isMine;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			base.enabled = false;
		}
		else
		{
			base.SendMessage("SetActiveFalse");
		}
	}

	// Token: 0x06004F00 RID: 20224 RVA: 0x001C9EA8 File Offset: 0x001C80A8
	private void invokeStart()
	{
	}

	// Token: 0x06004F01 RID: 20225 RVA: 0x001C9EAC File Offset: 0x001C80AC
	public void setSynh(bool _isActive)
	{
	}

	// Token: 0x06004F02 RID: 20226 RVA: 0x001C9EB0 File Offset: 0x001C80B0
	private void Update()
	{
		this.myTransform.rotation = this.gameObjectPlayerTrasform.rotation;
	}

	// Token: 0x04003D59 RID: 15705
	private bool isMine;

	// Token: 0x04003D5A RID: 15706
	private PhotonView photonView;

	// Token: 0x04003D5B RID: 15707
	public Transform gameObjectPlayerTrasform;

	// Token: 0x04003D5C RID: 15708
	private bool isMulti;

	// Token: 0x04003D5D RID: 15709
	private bool isInet;

	// Token: 0x04003D5E RID: 15710
	private Transform myTransform;
}
