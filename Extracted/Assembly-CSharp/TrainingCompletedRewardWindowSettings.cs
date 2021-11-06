using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000866 RID: 2150
public class TrainingCompletedRewardWindowSettings : MonoBehaviour
{
	// Token: 0x06004DA9 RID: 19881 RVA: 0x001C1084 File Offset: 0x001BF284
	private void Awake()
	{
		foreach (UILabel uilabel in this.exp)
		{
			uilabel.text = string.Format(LocalizationStore.Get("Key_1532"), Defs.ExpForTraining);
		}
		foreach (UILabel uilabel2 in this.gems)
		{
			uilabel2.text = string.Format(LocalizationStore.Get("Key_1531"), Defs.GemsForTraining);
		}
		foreach (UILabel uilabel3 in this.coins)
		{
			uilabel3.text = string.Format(LocalizationStore.Get("Key_1530"), Defs.CoinsForTraining);
		}
		foreach (UILabel uilabel4 in this.armorNameLabels)
		{
			uilabel4.text = LocalizationStore.Get((Storager.getInt("Training.NoviceArmorUsedKey", false) != 1) ? "Key_0724" : "Key_2045");
		}
		if (Storager.getInt("Training.NoviceArmorUsedKey", false) == 1)
		{
			this.armorTexture.mainTexture = Resources.Load<Texture2D>("OfferIcons/Armor_Novice_icon1_big");
		}
	}

	// Token: 0x04003C11 RID: 15377
	public List<UILabel> armorNameLabels;

	// Token: 0x04003C12 RID: 15378
	public UITexture armorTexture;

	// Token: 0x04003C13 RID: 15379
	public List<UILabel> exp;

	// Token: 0x04003C14 RID: 15380
	public List<UILabel> gems;

	// Token: 0x04003C15 RID: 15381
	public List<UILabel> coins;
}
