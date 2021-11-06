using System;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class Recenter : MonoBehaviour
{
	// Token: 0x06002B7C RID: 11132 RVA: 0x000E52C0 File Offset: 0x000E34C0
	public void CenterOn(int child)
	{
		int num = this.centerOnChildScript.centeredObject.transform.GetSiblingIndex() + child;
		if (num >= 0 && num < this.centerOnChildScript.transform.childCount)
		{
			Transform child2 = this.centerOnChildScript.transform.GetChild(num);
			this.centerOnChildScript.CenterOn(child2);
		}
	}

	// Token: 0x04002084 RID: 8324
	public UICenterOnChild centerOnChildScript;

	// Token: 0x04002085 RID: 8325
	[ReadOnly]
	public int nextChild = 1;

	// Token: 0x04002086 RID: 8326
	[ReadOnly]
	public int prevChild = -1;
}
