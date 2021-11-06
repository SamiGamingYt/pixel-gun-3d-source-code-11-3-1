using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class LightFXOnOff : MonoBehaviour
{
	// Token: 0x06001A4E RID: 6734 RVA: 0x0006A660 File Offset: 0x00068860
	private void Start()
	{
		if (Device.isWeakDevice || Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			base.gameObject.SetActive(false);
		}
	}
}
