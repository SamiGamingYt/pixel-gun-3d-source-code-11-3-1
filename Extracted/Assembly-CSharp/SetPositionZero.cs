using System;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class SetPositionZero : MonoBehaviour
{
	// Token: 0x060045E3 RID: 17891 RVA: 0x00179E7C File Offset: 0x0017807C
	private void Start()
	{
		this.myTransform = base.transform;
		this.myTransform.localPosition = Vector3.zero;
	}

	// Token: 0x060045E4 RID: 17892 RVA: 0x00179E9C File Offset: 0x0017809C
	private void Update()
	{
		this.myTransform.localPosition = Vector3.zero;
	}

	// Token: 0x0400333B RID: 13115
	private Transform myTransform;
}
