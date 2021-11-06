using System;
using UnityEngine;

// Token: 0x020002EB RID: 747
public class LabelAddToFrieadsNote : MonoBehaviour
{
	// Token: 0x06001A22 RID: 6690 RVA: 0x00069930 File Offset: 0x00067B30
	private void Update()
	{
		this.isBigPorog = !Defs2.IsAvalibleAddFrends();
		if (this.isBigPorog != this.isBigPorogOld)
		{
			if (!this.isBigPorog)
			{
				base.GetComponent<UILabel>().text = Defs.smallPorogString;
			}
			else
			{
				base.GetComponent<UILabel>().text = Defs.bigPorogString;
			}
		}
		this.isBigPorogOld = this.isBigPorog;
	}

	// Token: 0x04000F3D RID: 3901
	private bool isBigPorog;

	// Token: 0x04000F3E RID: 3902
	private bool isBigPorogOld;
}
