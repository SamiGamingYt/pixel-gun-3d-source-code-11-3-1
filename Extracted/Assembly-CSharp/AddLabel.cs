using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
public sealed class AddLabel : MonoBehaviour
{
	// Token: 0x06000026 RID: 38 RVA: 0x00002EEC File Offset: 0x000010EC
	private void Start()
	{
		if (Defs.isCompany || Defs.isCOOP)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000021 RID: 33
	private bool isBigPorog;

	// Token: 0x04000022 RID: 34
	private bool isBigPorogOld;
}
