using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class GadgetsInfoDesigner : MonoBehaviour
{
	// Token: 0x040008F6 RID: 2294
	public PlayerEventScoreController.ScoreEvent killStreakType = PlayerEventScoreController.ScoreEvent.none;

	// Token: 0x040008F7 RID: 2295
	public List<GadgetInfo.Parameter> parameters;

	// Token: 0x040008F8 RID: 2296
	public List<WeaponSounds.Effects> effects;

	// Token: 0x040008F9 RID: 2297
	public string Lkey;

	// Token: 0x040008FA RID: 2298
	public GadgetInfo.GadgetCategory category;

	// Token: 0x040008FB RID: 2299
	public int tier_FROM_1;

	// Token: 0x040008FC RID: 2300
	public GadgetsInfoDesigner previousUpgarde;

	// Token: 0x040008FD RID: 2301
	public GadgetsInfoDesigner nextUpgrade;

	// Token: 0x040008FE RID: 2302
	public string DescriptionLkey;

	// Token: 0x040008FF RID: 2303
	public float Duration = 30f;

	// Token: 0x04000900 RID: 2304
	public float Cooldown = 5f;
}
