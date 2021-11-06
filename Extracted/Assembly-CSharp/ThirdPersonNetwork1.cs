using System;
using Photon;
using UnityEngine;

// Token: 0x0200085B RID: 2139
public class ThirdPersonNetwork1 : Photon.MonoBehaviour
{
	// Token: 0x06004D62 RID: 19810 RVA: 0x001BEECC File Offset: 0x001BD0CC
	private void Awake()
	{
		if (!Defs.isMulti)
		{
			base.enabled = false;
		}
		this.myTransform = base.transform;
		this.correctPlayerPos = new Vector3(0f, -10000f, 0f);
		this.movementHistory = new ThirdPersonNetwork1.MovementHistoryEntry[this.historyLengh];
		for (int i = 0; i < this.historyLengh; i++)
		{
			this.movementHistory[i].timeStamp = 0.0;
		}
		this.myTime = 1.0;
	}

	// Token: 0x06004D63 RID: 19811 RVA: 0x001BEF64 File Offset: 0x001BD164
	private void Start()
	{
		if ((Defs.isInet && base.photonView.isMine) || (!Defs.isInet && base.GetComponent<NetworkView>().isMine))
		{
			this.isMine = true;
		}
	}

	// Token: 0x06004D64 RID: 19812 RVA: 0x001BEFAC File Offset: 0x001BD1AC
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			this.iskilled = this.playerMovec.isKilled;
			if (this.playerMovec.CurHealth <= 0f)
			{
				this.iskilled = true;
			}
			stream.SendNext(this.myTransform.position);
			stream.SendNext(this.myTransform.rotation.eulerAngles.y);
			stream.SendNext(this.iskilled);
			stream.SendNext(PhotonNetwork.time);
			stream.SendNext(this.myAnim);
			stream.SendNext(EffectsController.WeAreStealth);
			stream.SendNext(this.playerMovec.isImmortality);
			stream.SendNext(this.isTeleported || this.playerMovec.wasTimeJump);
			this.isTeleported = false;
		}
		else if (!this.isFirstSnapshot)
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = Quaternion.Euler(0f, (float)stream.ReceiveNext(), 0f);
			this.oldIsKilled = this.iskilled;
			this.iskilled = (bool)stream.ReceiveNext();
			this.playerMovec.isKilled = this.iskilled;
			this.correctPlayerTime = (double)stream.ReceiveNext();
			if (this.iskilled || Mathf.Abs((float)this.myTime - (float)this.correctPlayerTime) > 1000f)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
			}
			int anim = (int)stream.ReceiveNext();
			bool flag = (bool)stream.ReceiveNext();
			this.playerMovec.isImmortality = (bool)stream.ReceiveNext();
			this.isTeleported = (bool)stream.ReceiveNext();
			if (this.isTeleported)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
				this.myTransform.position = this.correctPlayerPos;
				this.myTransform.rotation = this.correctPlayerRot;
			}
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, anim, flag);
		}
		else
		{
			this.isFirstSnapshot = false;
		}
	}

	// Token: 0x06004D65 RID: 19813 RVA: 0x001BF218 File Offset: 0x001BD418
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 position = this.myTransform.position;
			Quaternion rotation = this.myTransform.rotation;
			stream.Serialize(ref position);
			stream.Serialize(ref rotation);
			this.iskilled = this.playerMovec.isKilled;
			stream.Serialize(ref this.iskilled);
			float num = (float)Network.time;
			stream.Serialize(ref num);
			int num2 = this.myAnim;
			stream.Serialize(ref num2);
			bool weAreStealth = EffectsController.WeAreStealth;
			stream.Serialize(ref weAreStealth);
			bool isImmortality = this.playerMovec.isImmortality;
			stream.Serialize(ref isImmortality);
			stream.Serialize(ref this.isTeleported);
			this.isTeleported = false;
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			float num3 = 0f;
			stream.Serialize(ref zero);
			stream.Serialize(ref identity);
			this.correctPlayerPos = zero;
			this.correctPlayerRot = identity;
			this.oldIsKilled = this.iskilled;
			stream.Serialize(ref this.iskilled);
			this.playerMovec.isKilled = this.iskilled;
			stream.Serialize(ref num3);
			this.correctPlayerTime = (double)num3;
			if (this.iskilled)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
			}
			int anim = 0;
			stream.Serialize(ref anim);
			bool flag = false;
			stream.Serialize(ref flag);
			bool isImmortality2 = false;
			stream.Serialize(ref isImmortality2);
			this.playerMovec.isImmortality = isImmortality2;
			stream.Serialize(ref this.isTeleported);
			if (this.isTeleported)
			{
				this.isHitoryClear = true;
				this.myTime = this.correctPlayerTime;
				this.myTransform.position = this.correctPlayerPos;
				this.myTransform.rotation = this.correctPlayerRot;
			}
			this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, anim, flag);
		}
	}

	// Token: 0x06004D66 RID: 19814 RVA: 0x001BF3F4 File Offset: 0x001BD5F4
	public void StartAngel()
	{
		this.isStartAngel = true;
	}

	// Token: 0x06004D67 RID: 19815 RVA: 0x001BF400 File Offset: 0x001BD600
	private void Update()
	{
		if (!this.isMine)
		{
			if (!this.playerMovec.isWeaponSet && this.myTransform.position.y > -8000f)
			{
				this.myTransform.position = new Vector3(0f, -10000f, 0f);
				return;
			}
			if (this.iskilled)
			{
				if (!this.oldIsKilled)
				{
					this.oldIsKilled = this.iskilled;
					this.isStartAngel = false;
				}
				if (this.myTransform.position.y > -8000f)
				{
					this.myTransform.position = new Vector3(0f, -10000f, 0f);
				}
			}
			else if (!this.oldIsKilled && !this.isHitoryClear && (this.sglajEnabled || this.sglajEnabledVidos || this.playerMovec.isInvisible))
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
				if (this.movementHistory[num2].timeStamp - this.myTime > 4.0 && num2 > 0)
				{
					num2--;
					this.myTransform.position = this.movementHistory[num2].playerPos;
					this.myTransform.rotation = this.movementHistory[num2].playerRot;
					this.myTime = this.movementHistory[num2].timeStamp;
				}
				else
				{
					float t = (float)((num - this.myTime) / (this.movementHistory[num2].timeStamp - this.myTime));
					this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.movementHistory[num2].playerPos, t);
					if (!Device.isPixelGunLow)
					{
						this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.movementHistory[num2].playerRot, t);
					}
					else
					{
						this.myTransform.rotation = this.movementHistory[num2].playerRot;
					}
					this.myTime = num;
					if (this.myAnim != this.movementHistory[num2].anim)
					{
						this.skinName.SetAnim(this.movementHistory[num2].anim, this.movementHistory[num2].weAreSteals);
						this.myAnim = this.movementHistory[num2].anim;
					}
				}
			}
			else if (!this.isHitoryClear)
			{
				this.myTransform.position = this.movementHistory[this.movementHistory.Length - 1].playerPos;
				this.myTransform.rotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
				this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
			}
			if (this.isStartAngel && this.myTransform.position.y > -8000f)
			{
				this.myTransform.position = new Vector3(0f, -10000f, 0f);
			}
		}
	}

	// Token: 0x06004D68 RID: 19816 RVA: 0x001BF808 File Offset: 0x001BDA08
	private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp, int _anim, bool _weAreSteals)
	{
		for (int i = this.movementHistory.Length - 1; i > 0; i--)
		{
			this.movementHistory[i] = this.movementHistory[i - 1];
		}
		this.movementHistory[0].playerPos = playerPos;
		this.movementHistory[0].playerRot = playerRot;
		this.movementHistory[0].timeStamp = timeStamp;
		this.movementHistory[0].anim = _anim;
		this.movementHistory[0].weAreSteals = _weAreSteals;
		if (this.isHitoryClear && this.movementHistory[this.movementHistory.Length - 1].timeStamp > this.myTime)
		{
			this.isHitoryClear = false;
			this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
			if (!this.isFirstHistoryFull)
			{
				this.myTransform.position = this.movementHistory[this.movementHistory.Length - 1].playerPos;
				this.myTransform.rotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
				this.isFirstHistoryFull = true;
			}
		}
	}

	// Token: 0x04003BC4 RID: 15300
	private bool iskilled;

	// Token: 0x04003BC5 RID: 15301
	private bool oldIsKilled;

	// Token: 0x04003BC6 RID: 15302
	public bool sglajEnabled;

	// Token: 0x04003BC7 RID: 15303
	public bool sglajEnabledVidos;

	// Token: 0x04003BC8 RID: 15304
	private Vector3 correctPlayerPos;

	// Token: 0x04003BC9 RID: 15305
	private double correctPlayerTime;

	// Token: 0x04003BCA RID: 15306
	private Quaternion correctPlayerRot = Quaternion.identity;

	// Token: 0x04003BCB RID: 15307
	public Player_move_c playerMovec;

	// Token: 0x04003BCC RID: 15308
	public bool isStartAngel;

	// Token: 0x04003BCD RID: 15309
	private Transform myTransform;

	// Token: 0x04003BCE RID: 15310
	private double myTime;

	// Token: 0x04003BCF RID: 15311
	private ThirdPersonNetwork1.MovementHistoryEntry[] movementHistory;

	// Token: 0x04003BD0 RID: 15312
	private int historyLengh = 5;

	// Token: 0x04003BD1 RID: 15313
	private bool isHitoryClear = true;

	// Token: 0x04003BD2 RID: 15314
	public int myAnim;

	// Token: 0x04003BD3 RID: 15315
	private int myAnimOld;

	// Token: 0x04003BD4 RID: 15316
	public SkinName skinName;

	// Token: 0x04003BD5 RID: 15317
	public bool weAreSteals;

	// Token: 0x04003BD6 RID: 15318
	public bool isTeleported;

	// Token: 0x04003BD7 RID: 15319
	private bool isFirstSnapshot = true;

	// Token: 0x04003BD8 RID: 15320
	private bool isMine;

	// Token: 0x04003BD9 RID: 15321
	private bool isFirstHistoryFull;

	// Token: 0x0200085C RID: 2140
	private struct MovementHistoryEntry
	{
		// Token: 0x04003BDA RID: 15322
		public Vector3 playerPos;

		// Token: 0x04003BDB RID: 15323
		public Quaternion playerRot;

		// Token: 0x04003BDC RID: 15324
		public int anim;

		// Token: 0x04003BDD RID: 15325
		public bool weAreSteals;

		// Token: 0x04003BDE RID: 15326
		public double timeStamp;
	}
}
