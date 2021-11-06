using System;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
public class SetHeadTextFromOtherLabel : MonoBehaviour
{
	// Token: 0x060045D4 RID: 17876 RVA: 0x0017975C File Offset: 0x0017795C
	private void Update()
	{
		for (int i = 0; i < this.headLabels.Length; i++)
		{
			this.headLabels[i].text = this.otherLabel.text;
		}
	}

	// Token: 0x0400332F RID: 13103
	public UILabel otherLabel;

	// Token: 0x04003330 RID: 13104
	public UILabel[] headLabels;
}
