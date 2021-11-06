using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
public class ShadowOnOff : MonoBehaviour
{
	// Token: 0x060045EA RID: 17898 RVA: 0x00179F88 File Offset: 0x00178188
	private void Start()
	{
		if (Device.isWeakDevice || Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}
}
