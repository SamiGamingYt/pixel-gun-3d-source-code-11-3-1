using System;
using System.Collections;
using Photon;
using UnityEngine;

// Token: 0x02000446 RID: 1094
[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : Photon.MonoBehaviour
{
	// Token: 0x060026D5 RID: 9941 RVA: 0x000C2B80 File Offset: 0x000C0D80
	public void OnClick()
	{
		if (!this.DestroyByRpc)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered, new object[0]);
		}
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x000C2BC0 File Offset: 0x000C0DC0
	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		PhotonNetwork.UnAllocateViewID(base.photonView.viewID);
		yield break;
	}

	// Token: 0x04001B4B RID: 6987
	public bool DestroyByRpc;
}
