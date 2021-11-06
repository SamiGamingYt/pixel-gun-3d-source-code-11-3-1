using System;

// Token: 0x020005B7 RID: 1463
public class ChestBonusItemData
{
	// Token: 0x060032A4 RID: 12964 RVA: 0x001067AC File Offset: 0x001049AC
	public string GetTimeLabel(bool isShort = false)
	{
		int num = this.timeLife / 24;
		if (num <= 0)
		{
			return string.Format("{0}h.", this.timeLife);
		}
		if (isShort)
		{
			return string.Format("{0}d.", num);
		}
		return string.Format("{0} {1}", LocalizationStore.Get("Key_1231"), num);
	}

	// Token: 0x0400252D RID: 9517
	public string tag;

	// Token: 0x0400252E RID: 9518
	public int count;

	// Token: 0x0400252F RID: 9519
	public int timeLife;
}
