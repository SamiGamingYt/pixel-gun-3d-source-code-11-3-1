using System;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class SetHeadLabelText : MonoBehaviour
{
	// Token: 0x060045D2 RID: 17874 RVA: 0x00179720 File Offset: 0x00177920
	public void SetText(string text)
	{
		for (int i = 0; i < this.labels.Length; i++)
		{
			this.labels[i].text = text;
		}
	}

	// Token: 0x0400332E RID: 13102
	public UILabel[] labels;
}
