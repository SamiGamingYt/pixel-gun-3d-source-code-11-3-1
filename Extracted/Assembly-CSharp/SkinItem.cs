using System;
using UnityEngine;

// Token: 0x020007C2 RID: 1986
public class SkinItem : ScriptableObject
{
	// Token: 0x0400353C RID: 13628
	public string skinStr = string.Empty;

	// Token: 0x0400353D RID: 13629
	public int price = 20;

	// Token: 0x0400353E RID: 13630
	public string currency = "Coins";

	// Token: 0x0400353F RID: 13631
	public string localizeKey = string.Empty;

	// Token: 0x04003540 RID: 13632
	public SkinItem.CategoryNames category = SkinItem.CategoryNames.SkinsCategoryMale;

	// Token: 0x04003541 RID: 13633
	public RatingSystem.RatingLeague currentLeague = RatingSystem.RatingLeague.none;

	// Token: 0x020007C3 RID: 1987
	public enum CategoryNames
	{
		// Token: 0x04003543 RID: 13635
		SkinsCategoryMale = 1100,
		// Token: 0x04003544 RID: 13636
		SkinsCategoryFamele = 1200,
		// Token: 0x04003545 RID: 13637
		SkinsCategorySpecial = 1300,
		// Token: 0x04003546 RID: 13638
		SkinsCategoryPremium = 1400,
		// Token: 0x04003547 RID: 13639
		LeagueSkinsCategory = 2200
	}
}
