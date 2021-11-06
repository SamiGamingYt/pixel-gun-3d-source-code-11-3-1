using System;
using UnityEngine;

// Token: 0x020003BE RID: 958
public sealed class NetworkInterpolatedTransform : MonoBehaviour
{
	// Token: 0x0600225C RID: 8796 RVA: 0x000A5C90 File Offset: 0x000A3E90
	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
		this.correctPlayerPos = new Vector3(0f, -10000f, 0f);
		this.myTransform = base.transform;
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x000A5CE0 File Offset: 0x000A3EE0
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 localPosition = base.transform.localPosition;
			Quaternion localRotation = base.transform.localRotation;
			stream.Serialize(ref localPosition);
			stream.Serialize(ref localRotation);
			this.iskilled = this.playerMovec.isKilled;
			stream.Serialize(ref this.iskilled);
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			stream.Serialize(ref zero);
			stream.Serialize(ref identity);
			this.correctPlayerPos = zero;
			this.correctPlayerRot = identity;
			this.oldIsKilled = this.iskilled;
			stream.Serialize(ref this.iskilled);
			this.playerMovec.isKilled = this.iskilled;
		}
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000A5D98 File Offset: 0x000A3F98
	public void StartAngel()
	{
		this.isStartAngel = true;
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000A5DA4 File Offset: 0x000A3FA4
	private void Update()
	{
		if (!Defs.isInet && !base.GetComponent<NetworkView>().isMine)
		{
			if (this.iskilled)
			{
				if (!this.oldIsKilled)
				{
					this.oldIsKilled = this.iskilled;
					if (!this.isStartAngel)
					{
						this.StartAngel();
					}
					this.isStartAngel = false;
				}
				this.myTransform.position = new Vector3(0f, -1000f, 0f);
			}
			else if (!this.oldIsKilled)
			{
				if (Vector3.SqrMagnitude(this.myTransform.position - this.correctPlayerPos) > 0.04f)
				{
					this.myTransform.position = Vector3.Lerp(this.myTransform.position, this.correctPlayerPos, Time.deltaTime * 5f);
				}
				this.myTransform.rotation = Quaternion.Lerp(this.myTransform.rotation, this.correctPlayerRot, Time.deltaTime * 5f);
			}
			else
			{
				this.myTransform.position = this.correctPlayerPos;
				this.myTransform.rotation = this.correctPlayerRot;
			}
			if (this.isStartAngel)
			{
				this.myTransform.position = new Vector3(0f, -1000f, 0f);
			}
		}
	}

	// Token: 0x04001661 RID: 5729
	private bool iskilled;

	// Token: 0x04001662 RID: 5730
	private bool oldIsKilled;

	// Token: 0x04001663 RID: 5731
	public Player_move_c playerMovec;

	// Token: 0x04001664 RID: 5732
	public bool isStartAngel;

	// Token: 0x04001665 RID: 5733
	public Vector3 correctPlayerPos;

	// Token: 0x04001666 RID: 5734
	public Quaternion correctPlayerRot = Quaternion.identity;

	// Token: 0x04001667 RID: 5735
	private Transform myTransform;
}
