using System;
using UnityEngine;

// Token: 0x02000775 RID: 1909
internal sealed class TrainingHelper : MonoBehaviour
{
	// Token: 0x0600432D RID: 17197 RVA: 0x00166D3C File Offset: 0x00164F3C
	private void Start()
	{
		float num = 211f * Defs.Coef;
		float num2 = 114f * Defs.Coef;
		float num3 = 12f * Defs.Coef;
		this._buttonRect = new Rect((float)Screen.width - num - num3, num2 + 64f * Defs.Coef + 3f * num3, num, num2);
	}

	// Token: 0x0400312D RID: 12589
	private Rect _buttonRect;
}
