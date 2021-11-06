using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
[AddComponentMenu("NGUI/UI/Orthographic Camera")]
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class UIOrthoCamera : MonoBehaviour
{
	// Token: 0x06002198 RID: 8600 RVA: 0x0009FB70 File Offset: 0x0009DD70
	private void Start()
	{
		this.mCam = base.GetComponent<Camera>();
		this.mTrans = base.transform;
		this.mCam.orthographic = true;
	}

	// Token: 0x06002199 RID: 8601 RVA: 0x0009FBA4 File Offset: 0x0009DDA4
	private void Update()
	{
		float num = this.mCam.rect.yMin * (float)Screen.height;
		float num2 = this.mCam.rect.yMax * (float)Screen.height;
		float num3 = (num2 - num) * 0.5f * this.mTrans.lossyScale.y;
		if (!Mathf.Approximately(this.mCam.orthographicSize, num3))
		{
			this.mCam.orthographicSize = num3;
		}
	}

	// Token: 0x040015C2 RID: 5570
	private Camera mCam;

	// Token: 0x040015C3 RID: 5571
	private Transform mTrans;
}
