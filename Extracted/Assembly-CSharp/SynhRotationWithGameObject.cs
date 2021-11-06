using System;
using UnityEngine;

// Token: 0x0200084C RID: 2124
public class SynhRotationWithGameObject : MonoBehaviour
{
	// Token: 0x06004D14 RID: 19732 RVA: 0x001BCD3C File Offset: 0x001BAF3C
	private void Start()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x06004D15 RID: 19733 RVA: 0x001BCD4C File Offset: 0x001BAF4C
	private void Update()
	{
		this.myTransform.rotation = this.gameObject.rotation;
		if (this.transformPos)
		{
			this.myTransform.position = this.gameObject.TransformPoint(this.addpos);
		}
	}

	// Token: 0x04003B7E RID: 15230
	public new Transform gameObject;

	// Token: 0x04003B7F RID: 15231
	public bool transformPos;

	// Token: 0x04003B80 RID: 15232
	private Transform myTransform;

	// Token: 0x04003B81 RID: 15233
	public Vector3 addpos = Vector3.zero;
}
