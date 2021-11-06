using System;
using Photon;
using UnityEngine;

// Token: 0x0200044F RID: 1103
[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : Photon.MonoBehaviour
{
	// Token: 0x060026FA RID: 9978 RVA: 0x000C357C File Offset: 0x000C177C
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x060026FB RID: 9979 RVA: 0x000C35B8 File Offset: 0x000C17B8
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x060026FC RID: 9980 RVA: 0x000C35F0 File Offset: 0x000C17F0
	[PunRPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (!this.SentPickup || !msgInfo.sender.isLocal || base.gameObject.GetActive())
		{
		}
		this.SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		float num2 = this.SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(false);
			base.Invoke("RespawnAfter", num2);
		}
	}

	// Token: 0x060026FD RID: 9981 RVA: 0x000C3698 File Offset: 0x000C1898
	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04001B67 RID: 7015
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x04001B68 RID: 7016
	public bool PickupOnCollide;

	// Token: 0x04001B69 RID: 7017
	public bool SentPickup;
}
