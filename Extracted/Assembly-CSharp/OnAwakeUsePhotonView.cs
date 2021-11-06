using System;
using Photon;
using UnityEngine;

// Token: 0x02000445 RID: 1093
[RequireComponent(typeof(PhotonView))]
public class OnAwakeUsePhotonView : Photon.MonoBehaviour
{
	// Token: 0x060026D0 RID: 9936 RVA: 0x000C2AA8 File Offset: 0x000C0CA8
	private void Awake()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[0]);
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x000C2AE0 File Offset: 0x000C0CE0
	private void Start()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[]
		{
			1
		});
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x000C2B20 File Offset: 0x000C0D20
	[PunRPC]
	public void OnAwakeRPC()
	{
		Debug.Log("RPC: 'OnAwakeRPC' PhotonView: " + base.photonView);
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x000C2B38 File Offset: 0x000C0D38
	[PunRPC]
	public void OnAwakeRPC(byte myParameter)
	{
		Debug.Log(string.Concat(new object[]
		{
			"RPC: 'OnAwakeRPC' Parameter: ",
			myParameter,
			" PhotonView: ",
			base.photonView
		}));
	}
}
