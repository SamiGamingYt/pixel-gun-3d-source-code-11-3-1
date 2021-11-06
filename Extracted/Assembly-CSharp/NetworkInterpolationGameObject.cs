using System;
using UnityEngine;

// Token: 0x020003BF RID: 959
public class NetworkInterpolationGameObject : MonoBehaviour
{
	// Token: 0x06002261 RID: 8801 RVA: 0x000A5F18 File Offset: 0x000A4118
	private void Awake()
	{
		if (!Defs.isMulti || Defs.isInet)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000A5F38 File Offset: 0x000A4138
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Quaternion localRotation = base.transform.localRotation;
			stream.Serialize(ref localRotation);
		}
		else
		{
			Quaternion identity = Quaternion.identity;
			stream.Serialize(ref identity);
			this.correctPlayerRot = identity;
		}
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000A5F80 File Offset: 0x000A4180
	private void Update()
	{
		if (!base.GetComponent<NetworkView>().isMine)
		{
			base.transform.localRotation = this.correctPlayerRot;
		}
	}

	// Token: 0x04001668 RID: 5736
	private Quaternion correctPlayerRot = Quaternion.identity;
}
