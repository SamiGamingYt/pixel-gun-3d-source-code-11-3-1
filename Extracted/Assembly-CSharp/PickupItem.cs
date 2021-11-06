using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x0200044E RID: 1102
[RequireComponent(typeof(PhotonView))]
public class PickupItem : Photon.MonoBehaviour, IPunObservable
{
	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x060026EF RID: 9967 RVA: 0x000C3218 File Offset: 0x000C1418
	public int ViewID
	{
		get
		{
			return base.photonView.viewID;
		}
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x000C3228 File Offset: 0x000C1428
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x060026F1 RID: 9969 RVA: 0x000C3264 File Offset: 0x000C1464
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && this.SecondsBeforeRespawn <= 0f)
		{
			stream.SendNext(base.gameObject.transform.position);
		}
		else
		{
			Vector3 position = (Vector3)stream.ReceiveNext();
			base.gameObject.transform.position = position;
		}
	}

	// Token: 0x060026F2 RID: 9970 RVA: 0x000C32CC File Offset: 0x000C14CC
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x060026F3 RID: 9971 RVA: 0x000C3304 File Offset: 0x000C1504
	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[0]);
		}
	}

	// Token: 0x060026F4 RID: 9972 RVA: 0x000C3334 File Offset: 0x000C1534
	public void Drop(Vector3 newPosition)
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[]
			{
				newPosition
			});
		}
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x000C336C File Offset: 0x000C156C
	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender.isLocal)
		{
			this.SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat(new object[]
			{
				"Ignored PU RPC, cause item is inactive. ",
				base.gameObject,
				" SecondsBeforeRespawn: ",
				this.SecondsBeforeRespawn,
				" TimeOfRespawn: ",
				this.TimeOfRespawn,
				" respawn in future: ",
				this.TimeOfRespawn > PhotonNetwork.time
			}));
			return;
		}
		this.PickupIsMine = msgInfo.sender.isLocal;
		if (this.OnPickedUpCall != null)
		{
			this.OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (this.SecondsBeforeRespawn <= 0f)
		{
			this.PickedUp(0f);
		}
		else
		{
			double num = PhotonNetwork.time - msgInfo.timestamp;
			double num2 = (double)this.SecondsBeforeRespawn - num;
			if (num2 > 0.0)
			{
				this.PickedUp((float)num2);
			}
		}
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x000C3490 File Offset: 0x000C1690
	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		PickupItem.DisabledPickupItems.Add(this);
		this.TimeOfRespawn = 0.0;
		if (timeUntilRespawn > 0f)
		{
			this.TimeOfRespawn = PhotonNetwork.time + (double)timeUntilRespawn;
			base.Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	// Token: 0x060026F7 RID: 9975 RVA: 0x000C34EC File Offset: 0x000C16EC
	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	// Token: 0x060026F8 RID: 9976 RVA: 0x000C351C File Offset: 0x000C171C
	[PunRPC]
	internal void PunRespawn()
	{
		PickupItem.DisabledPickupItems.Remove(this);
		this.TimeOfRespawn = 0.0;
		this.PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x04001B60 RID: 7008
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x04001B61 RID: 7009
	public bool PickupOnTrigger;

	// Token: 0x04001B62 RID: 7010
	public bool PickupIsMine;

	// Token: 0x04001B63 RID: 7011
	public UnityEngine.MonoBehaviour OnPickedUpCall;

	// Token: 0x04001B64 RID: 7012
	public bool SentPickup;

	// Token: 0x04001B65 RID: 7013
	public double TimeOfRespawn;

	// Token: 0x04001B66 RID: 7014
	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
}
