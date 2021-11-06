using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
internal sealed class CamPlayerObzorController : MonoBehaviour
{
	// Token: 0x0600022A RID: 554 RVA: 0x00013A28 File Offset: 0x00011C28
	private void Start()
	{
		if (Defs.isMulti && Defs.isInet && !base.transform.parent.GetComponent<PhotonView>().isMine)
		{
			this.isMine = true;
		}
		if (this.isMine)
		{
			base.SendMessage("SetActiveFalse");
		}
		else
		{
			base.enabled = false;
		}
		this.playerGameObject = base.transform.parent.GetComponent<SkinName>().playerGameObject;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00013AA8 File Offset: 0x00011CA8
	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(new Vector3(this.playerGameObject.transform.rotation.x, base.transform.rotation.y, base.transform.rotation.z));
	}

	// Token: 0x0400024C RID: 588
	private bool isMine;

	// Token: 0x0400024D RID: 589
	public GameObject playerGameObject;
}
