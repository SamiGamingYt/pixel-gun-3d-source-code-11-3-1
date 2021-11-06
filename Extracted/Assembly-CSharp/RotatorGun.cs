using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class RotatorGun : MonoBehaviour
{
	// Token: 0x06002C3C RID: 11324 RVA: 0x000EAF60 File Offset: 0x000E9160
	private void Update()
	{
		this.playerGun.transform.rotation = base.transform.rotation;
	}

	// Token: 0x0400214B RID: 8523
	public GameObject playerGun;
}
