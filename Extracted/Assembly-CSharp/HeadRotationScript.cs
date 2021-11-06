using System;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class HeadRotationScript : MonoBehaviour
{
	// Token: 0x06001506 RID: 5382 RVA: 0x0005338C File Offset: 0x0005158C
	private void Start()
	{
		this.prevX = 0f;
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x0005339C File Offset: 0x0005159C
	private void LateUpdate()
	{
		Quaternion localRotation = default(Quaternion);
		float num = -this.gunPoint.localRotation.eulerAngles.x / 2f;
		num = ((num >= -45f) ? num : (num + 180f));
		num = Mathf.Lerp(this.prevX, num, 0.2f);
		localRotation.eulerAngles = new Vector3(0f, 0f, num);
		base.transform.localRotation = localRotation;
		this.prevX = num;
	}

	// Token: 0x04000C44 RID: 3140
	public Transform gunPoint;

	// Token: 0x04000C45 RID: 3141
	private float prevX;
}
