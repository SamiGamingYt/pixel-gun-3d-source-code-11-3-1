using System;
using UnityEngine;

// Token: 0x020007C1 RID: 1985
public class SkinInfo : MonoBehaviour
{
	// Token: 0x04003537 RID: 13623
	[NonSerialized]
	public Texture skin;

	// Token: 0x04003538 RID: 13624
	public string skinStr = string.Empty;

	// Token: 0x04003539 RID: 13625
	public int price = 20;

	// Token: 0x0400353A RID: 13626
	public string currency = "Coins";

	// Token: 0x0400353B RID: 13627
	public string localizeKey = string.Empty;
}
