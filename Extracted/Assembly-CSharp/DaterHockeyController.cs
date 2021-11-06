using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class DaterHockeyController : MonoBehaviour
{
	// Token: 0x0600042D RID: 1069 RVA: 0x00023C08 File Offset: 0x00021E08
	private void Awake()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.thisRigidbody = base.GetComponent<Rigidbody>();
		this.thisTransform = base.transform;
		this.resetPositionPoint = this.thisTransform.position;
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00023C4C File Offset: 0x00021E4C
	private void Start()
	{
		this.isMine = (!Defs.isMulti || (!Defs.isInet && base.GetComponent<NetworkView>().isMine) || (Defs.isInet && this.photonView.isMine));
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00023CA0 File Offset: 0x00021EA0
	private void Update()
	{
		if (this.isForceMyPlayer && WeaponManager.sharedManager.myPlayer == null)
		{
			this.isForceMyPlayer = false;
		}
		if (this.isForceMyPlayer)
		{
			this.timerToSendForce -= Time.deltaTime;
			if (this.timerToSendForce < 0f)
			{
				this.timerToSendForce = this.timeSendForce;
				this.AddForce(Vector3.Normalize(this.thisTransform.position - WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.position) * this.coefForce);
			}
		}
		if (!this.isMine)
		{
			this.thisTransform.position = Vector3.Lerp(this.thisTransform.position, this.synchPos, Time.deltaTime * 5f);
			this.thisTransform.rotation = Quaternion.Lerp(this.thisTransform.rotation, this.synchRot, Time.deltaTime * 5f);
		}
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00023DAC File Offset: 0x00021FAC
	private void OnTriggerEnter(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			this.isForceMyPlayer = true;
			return;
		}
		if (this.isMine && collider.gameObject.name.Equals("Gates1"))
		{
			this.ResetPosition();
		}
		if (this.isMine && collider.gameObject.name.Equals("Gates2"))
		{
			this.ResetPosition();
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00023E70 File Offset: 0x00022070
	private void OnTriggerExit(Collider collider)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform))
		{
			this.isForceMyPlayer = false;
		}
		if (this.isMine && collider.gameObject.name.Equals("Stadium"))
		{
			this.ResetPosition();
		}
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00023F08 File Offset: 0x00022108
	[RPC]
	[PunRPC]
	private void AddForceRPC(Vector3 _force)
	{
		base.GetComponent<Rigidbody>().AddForce(_force);
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x00023F18 File Offset: 0x00022118
	private void AddForce(Vector3 _force)
	{
		if (Defs.isInet)
		{
			this.photonView.RPC("AddForceRPC", PhotonTargets.All, new object[]
			{
				_force
			});
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("AddForceRPC", RPCMode.Server, new object[]
			{
				_force
			});
			this.AddForceRPC(_force);
		}
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x00023F7C File Offset: 0x0002217C
	private void ResetPosition()
	{
		this.thisTransform.position = this.resetPositionPoint;
		this.thisRigidbody.velocity = Vector3.zero;
		this.thisRigidbody.angularVelocity = Vector3.zero;
		this.isResetPosition = true;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00023FC4 File Offset: 0x000221C4
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(this.thisTransform.position);
			stream.SendNext(this.thisTransform.rotation);
			stream.SendNext(this.thisRigidbody.velocity);
			stream.SendNext(this.thisRigidbody.angularVelocity);
			stream.SendNext(this.isResetPosition);
		}
		else
		{
			this.synchPos = (Vector3)stream.ReceiveNext();
			this.synchRot = (Quaternion)stream.ReceiveNext();
			this.thisRigidbody.velocity = (Vector3)stream.ReceiveNext();
			this.thisRigidbody.angularVelocity = (Vector3)stream.ReceiveNext();
			this.isResetPosition = (bool)stream.ReceiveNext();
			if (this.isFirstSynhPos)
			{
				this.thisTransform.position = this.synchPos;
				this.thisTransform.rotation = this.synchRot;
				this.isFirstSynhPos = false;
				this.isResetPosition = false;
			}
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x000240E4 File Offset: 0x000222E4
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 position = this.thisTransform.position;
			Quaternion rotation = this.thisTransform.rotation;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
			bool flag = this.isResetPosition;
			stream.Serialize(ref flag);
			if (this.isResetPosition)
			{
				this.isResetPosition = false;
			}
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			bool flag2 = false;
			stream.Serialize(ref zero);
			stream.Serialize(ref identity);
			stream.Serialize(ref flag2);
			this.synchPos = zero;
			this.synchRot = identity;
			this.isResetPosition = flag2;
			if (this.isFirstSynhPos || this.isResetPosition)
			{
				this.thisTransform.position = this.synchPos;
				this.thisTransform.rotation = this.synchRot;
				this.isFirstSynhPos = false;
				this.isResetPosition = false;
			}
		}
	}

	// Token: 0x040004B8 RID: 1208
	public float coefForce = 200f;

	// Token: 0x040004B9 RID: 1209
	public int score1;

	// Token: 0x040004BA RID: 1210
	public int score2;

	// Token: 0x040004BB RID: 1211
	private bool isForceMyPlayer;

	// Token: 0x040004BC RID: 1212
	private float timeSendForce = 0.3f;

	// Token: 0x040004BD RID: 1213
	private float timerToSendForce = -1f;

	// Token: 0x040004BE RID: 1214
	private PhotonView photonView;

	// Token: 0x040004BF RID: 1215
	private Rigidbody thisRigidbody;

	// Token: 0x040004C0 RID: 1216
	private Transform thisTransform;

	// Token: 0x040004C1 RID: 1217
	private Vector3 resetPositionPoint;

	// Token: 0x040004C2 RID: 1218
	private bool isFirstSynhPos = true;

	// Token: 0x040004C3 RID: 1219
	private bool isResetPosition;

	// Token: 0x040004C4 RID: 1220
	private Vector3 synchPos;

	// Token: 0x040004C5 RID: 1221
	private Quaternion synchRot;

	// Token: 0x040004C6 RID: 1222
	private bool isMine;
}
