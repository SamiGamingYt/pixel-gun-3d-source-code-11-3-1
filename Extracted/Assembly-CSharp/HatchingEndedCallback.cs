using System;
using UnityEngine;

// Token: 0x02000293 RID: 659
public class HatchingEndedCallback : MonoBehaviour
{
	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06001502 RID: 5378 RVA: 0x00053334 File Offset: 0x00051534
	// (remove) Token: 0x06001503 RID: 5379 RVA: 0x0005334C File Offset: 0x0005154C
	public static event Action HatchingEnded;

	// Token: 0x06001504 RID: 5380 RVA: 0x00053364 File Offset: 0x00051564
	public void HatchingCompleted()
	{
		Action hatchingEnded = HatchingEndedCallback.HatchingEnded;
		if (hatchingEnded != null)
		{
			hatchingEnded();
		}
	}
}
