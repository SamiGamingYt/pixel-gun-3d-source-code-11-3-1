using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class CameraFacingBilloard : MonoBehaviour
{
	// Token: 0x0600022D RID: 557 RVA: 0x00013B10 File Offset: 0x00011D10
	private void Awake()
	{
		this.thisTransform = base.transform;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00013B20 File Offset: 0x00011D20
	private void Update()
	{
		if (NickLabelController.currentCamera != null)
		{
			this.thisTransform.rotation = NickLabelController.currentCamera.transform.rotation;
			if (this.Invert)
			{
				this.thisTransform.rotation *= new Quaternion(1f, 180f, 1f, 1f);
			}
		}
	}

	// Token: 0x0400024E RID: 590
	private Transform thisTransform;

	// Token: 0x0400024F RID: 591
	public bool Invert;
}
