using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class GadgetArmoryItem : MonoBehaviour
{
	// Token: 0x040008D5 RID: 2261
	public bool isReplaceOnlyHands = true;

	// Token: 0x040008D6 RID: 2262
	public GameObject gadgetPoint;

	// Token: 0x040008D7 RID: 2263
	public List<GameObject> noFillPersSkinObjects;

	// Token: 0x040008D8 RID: 2264
	[Header("For ReplaceOnlyHands")]
	public Transform LeftArmorHand;

	// Token: 0x040008D9 RID: 2265
	public Transform RightArmorHand;
}
