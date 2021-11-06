using System;
using Photon;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class SmoothSyncMovement : Photon.MonoBehaviour, IPunObservable
{
	// Token: 0x0600273C RID: 10044 RVA: 0x000C471C File Offset: 0x000C291C
	public void Awake()
	{
		if (base.photonView == null || base.photonView.observed != this)
		{
			Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
		}
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000C4760 File Offset: 0x000C2960
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000C47CC File Offset: 0x000C29CC
	public void Update()
	{
		if (!base.photonView.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
		}
	}

	// Token: 0x04001B83 RID: 7043
	public float SmoothingDelay = 5f;

	// Token: 0x04001B84 RID: 7044
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x04001B85 RID: 7045
	private Quaternion correctPlayerRot = Quaternion.identity;
}
