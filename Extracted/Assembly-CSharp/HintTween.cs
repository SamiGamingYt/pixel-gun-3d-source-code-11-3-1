using System;
using UnityEngine;

// Token: 0x020002A6 RID: 678
public class HintTween : MonoBehaviour
{
	// Token: 0x06001552 RID: 5458 RVA: 0x00054F64 File Offset: 0x00053164
	private void Start()
	{
		base.transform.localScale = Vector3.one * 0.3f;
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x00054F80 File Offset: 0x00053180
	private void Update()
	{
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), 3f * Time.unscaledDeltaTime);
	}
}
