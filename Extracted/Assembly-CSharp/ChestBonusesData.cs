using System;
using System.Collections.Generic;

// Token: 0x020005B9 RID: 1465
public class ChestBonusesData
{
	// Token: 0x060032A9 RID: 12969 RVA: 0x00106910 File Offset: 0x00104B10
	public void Clear()
	{
		if (this.bonuses == null)
		{
			return;
		}
		for (int i = 0; i < this.bonuses.Count; i++)
		{
			this.bonuses[i].items.Clear();
		}
		this.bonuses.Clear();
	}

	// Token: 0x04002533 RID: 9523
	public int timeStart;

	// Token: 0x04002534 RID: 9524
	public int duration;

	// Token: 0x04002535 RID: 9525
	public List<ChestBonusData> bonuses;
}
