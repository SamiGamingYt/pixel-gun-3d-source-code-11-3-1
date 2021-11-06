using System;
using UnityEngine;

// Token: 0x02000053 RID: 83
public sealed class CamFXSetting : MonoBehaviour
{
	// Token: 0x06000228 RID: 552 RVA: 0x000139F0 File Offset: 0x00011BF0
	private void Start()
	{
		this.CamFX = base.transform.GetChild(0).gameObject;
		this.CamFX.SetActive(false);
	}

	// Token: 0x0400024B RID: 587
	public GameObject CamFX;
}
