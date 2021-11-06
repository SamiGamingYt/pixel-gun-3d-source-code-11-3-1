using System;
using Photon;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class InterolationGameObject : Photon.MonoBehaviour
{
	// Token: 0x06001958 RID: 6488 RVA: 0x00064478 File Offset: 0x00062678
	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		this.myTransform = base.transform;
		this.movementHistory = new InterolationGameObject.MovementHistoryEntry[this.historyLengh];
		for (int i = 0; i < this.historyLengh; i++)
		{
			this.movementHistory[i].timeStamp = 0.0;
		}
		this.myTime = 1.0;
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000644F4 File Offset: 0x000626F4
	private void Start()
	{
		if ((Defs.isInet && base.photonView.isMine) || (!Defs.isInet && base.GetComponent<NetworkView>().isMine))
		{
			this.isMine = true;
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0006453C File Offset: 0x0006273C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.isSynchPosition)
			{
				stream.SendNext((!this.isLocalTrasformSynch) ? this.myTransform.position : this.myTransform.localPosition);
			}
			if (this.isSynchRotation)
			{
				if (this.syncOneAxis)
				{
					stream.SendNext(this.myTransform.localRotation.eulerAngles.x);
				}
				else
				{
					stream.SendNext((!this.isLocalTrasformSynch) ? this.myTransform.rotation : this.myTransform.localRotation);
				}
			}
			stream.SendNext(PhotonNetwork.time);
		}
		else
		{
			if (this.isSynchPosition)
			{
				this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			}
			if (this.isSynchRotation)
			{
				if (this.syncOneAxis)
				{
					this.correctPlayerRot = Quaternion.Euler((float)stream.ReceiveNext(), 0f, 0f);
				}
				else
				{
					this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
				}
			}
			this.correctPlayerTime = (double)stream.ReceiveNext();
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime);
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000646A8 File Offset: 0x000628A8
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 vector = (!this.isLocalTrasformSynch) ? this.myTransform.position : this.myTransform.localPosition;
			Quaternion quaternion = (!this.isLocalTrasformSynch) ? this.myTransform.rotation : this.myTransform.localRotation;
			if (this.isSynchPosition)
			{
				stream.Serialize(ref vector);
			}
			if (this.isSynchRotation)
			{
				stream.Serialize(ref quaternion);
			}
			float num = (float)Network.time;
			stream.Serialize(ref num);
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			float num2 = 0f;
			if (this.isSynchPosition)
			{
				stream.Serialize(ref zero);
			}
			if (this.isSynchRotation)
			{
				stream.Serialize(ref identity);
			}
			this.correctPlayerPos = zero;
			this.correctPlayerRot = identity;
			stream.Serialize(ref num2);
			this.correctPlayerTime = (double)num2;
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime);
		}
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x000647BC File Offset: 0x000629BC
	private void Update()
	{
		if (!this.isMine)
		{
			if (this.sglajEnabled && !this.isHitoryClear)
			{
				double num;
				if (this.myTime + (double)Time.deltaTime < this.movementHistory[this.movementHistory.Length - 1].timeStamp)
				{
					num = this.myTime + (double)(Time.deltaTime * 1.5f);
				}
				else
				{
					num = this.myTime + (double)Time.deltaTime;
				}
				int num2 = 0;
				for (int i = 0; i < this.movementHistory.Length; i++)
				{
					if (this.movementHistory[i].timeStamp <= this.myTime)
					{
						break;
					}
					num2 = i;
				}
				if (num2 == 0)
				{
					this.isHitoryClear = true;
				}
				float t = (float)((num - this.myTime) / (this.movementHistory[num2].timeStamp - this.myTime));
				if (this.isLocalTrasformSynch)
				{
					if (this.isSynchPosition)
					{
						this.myTransform.localPosition = Vector3.Lerp(this.myTransform.localPosition, this.movementHistory[num2].playerPos, t);
					}
					if (this.isSynchRotation)
					{
						this.myTransform.localRotation = Quaternion.Lerp(this.myTransform.localRotation, this.movementHistory[num2].playerRot, t);
					}
				}
				else
				{
					if (this.isSynchPosition)
					{
						this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.movementHistory[num2].playerPos, t);
					}
					if (this.isSynchRotation)
					{
						this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.movementHistory[num2].playerRot, t);
					}
				}
				this.myTime = num;
			}
			else if (!this.isHitoryClear)
			{
				if (this.isLocalTrasformSynch)
				{
					if (this.isSynchPosition)
					{
						this.myTransform.localPosition = this.movementHistory[this.movementHistory.Length - 1].playerPos;
					}
					if (this.isSynchRotation)
					{
						this.myTransform.localRotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
					}
				}
				else
				{
					if (this.isSynchPosition)
					{
						this.myTransform.position = this.movementHistory[this.movementHistory.Length - 1].playerPos;
					}
					if (this.isSynchRotation)
					{
						this.myTransform.rotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
					}
				}
				this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
			}
		}
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x00064AA8 File Offset: 0x00062CA8
	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp)
	{
		for (int i = this.movementHistory.Length - 1; i > 0; i--)
		{
			this.movementHistory[i] = this.movementHistory[i - 1];
		}
		this.movementHistory[0].playerPos = playerPos;
		this.movementHistory[0].playerRot = playerRot;
		this.movementHistory[0].timeStamp = timeStamp;
		if (this.isHitoryClear && this.movementHistory[this.movementHistory.Length - 1].timeStamp > this.myTime)
		{
			this.isHitoryClear = false;
			this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
		}
	}

	// Token: 0x04000E5E RID: 3678
	public int historyLengh = 5;

	// Token: 0x04000E5F RID: 3679
	public bool isSynchPosition;

	// Token: 0x04000E60 RID: 3680
	public bool isSynchRotation;

	// Token: 0x04000E61 RID: 3681
	public bool isLocalTrasformSynch;

	// Token: 0x04000E62 RID: 3682
	public bool syncOneAxis;

	// Token: 0x04000E63 RID: 3683
	public bool sglajEnabled;

	// Token: 0x04000E64 RID: 3684
	private Quaternion correctPlayerRot;

	// Token: 0x04000E65 RID: 3685
	private Vector3 correctPlayerPos;

	// Token: 0x04000E66 RID: 3686
	private double correctPlayerTime;

	// Token: 0x04000E67 RID: 3687
	private double myTime;

	// Token: 0x04000E68 RID: 3688
	private Transform myTransform;

	// Token: 0x04000E69 RID: 3689
	private InterolationGameObject.MovementHistoryEntry[] movementHistory;

	// Token: 0x04000E6A RID: 3690
	private bool isHitoryClear = true;

	// Token: 0x04000E6B RID: 3691
	private bool isMine;

	// Token: 0x020002D2 RID: 722
	private struct MovementHistoryEntry
	{
		// Token: 0x04000E6C RID: 3692
		public Vector3 playerPos;

		// Token: 0x04000E6D RID: 3693
		public Quaternion playerRot;

		// Token: 0x04000E6E RID: 3694
		public double timeStamp;
	}
}
