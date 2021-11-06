using System;
using UnityEngine;

// Token: 0x020005A6 RID: 1446
internal sealed class ZombiManagerSynch : MonoBehaviour
{
	// Token: 0x06003229 RID: 12841 RVA: 0x00104938 File Offset: 0x00102B38
	private void Awake()
	{
		try
		{
			if (!Defs.isMulti || !Defs.isInet)
			{
				base.enabled = false;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x0010499C File Offset: 0x00102B9C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
		}
	}

	// Token: 0x040024E9 RID: 9449
	private ThirdPersonCamera cameraScript;

	// Token: 0x040024EA RID: 9450
	private ThirdPersonController controllerScript;

	// Token: 0x040024EB RID: 9451
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x040024EC RID: 9452
	private Quaternion correctPlayerRot = Quaternion.identity;
}
