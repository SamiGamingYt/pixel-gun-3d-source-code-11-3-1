using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E0 RID: 992
public class PetsInfoDesigner : MonoBehaviour
{
	// Token: 0x04001835 RID: 6197
	[Header("Common settings")]
	public PetInfo.BehaviourType Behaviour;

	// Token: 0x04001836 RID: 6198
	public ItemDb.ItemRarity Rarity;

	// Token: 0x04001837 RID: 6199
	public int Tier;

	// Token: 0x04001838 RID: 6200
	public string Lkey;

	// Token: 0x04001839 RID: 6201
	public int ToUpPoints;

	// Token: 0x0400183A RID: 6202
	[Header("Shop settings")]
	public Vector3 positionInBanners;

	// Token: 0x0400183B RID: 6203
	public Vector3 rotationInBanners;

	// Token: 0x0400183C RID: 6204
	public List<GadgetInfo.Parameter> parameters;

	// Token: 0x0400183D RID: 6205
	public List<WeaponSounds.Effects> effects;

	// Token: 0x0400183E RID: 6206
	[Header("AI settings")]
	public float AttackDistance;

	// Token: 0x0400183F RID: 6207
	public float AttackStopDistance;

	// Token: 0x04001840 RID: 6208
	public float MinToOwnerDistance;

	// Token: 0x04001841 RID: 6209
	public float MaxToOwnerDistance;

	// Token: 0x04001842 RID: 6210
	public float TargetDetectRange;

	// Token: 0x04001843 RID: 6211
	public float OffenderDetectRange = 10f;

	// Token: 0x04001844 RID: 6212
	public float ToTargetTeleportDistance = 7f;

	// Token: 0x04001845 RID: 6213
	[Header("Poison settings")]
	public bool poisonEnabled;

	// Token: 0x04001846 RID: 6214
	public Player_move_c.PoisonType poisonType;

	// Token: 0x04001847 RID: 6215
	public int poisonCount = 3;

	// Token: 0x04001848 RID: 6216
	public float poisonTime = 2f;

	// Token: 0x04001849 RID: 6217
	public float poisonDamagePercent = 0.033f;

	// Token: 0x0400184A RID: 6218
	[Header("Critical hit settings")]
	public int criticalHitChance;

	// Token: 0x0400184B RID: 6219
	public float criticalHitCoef = 2f;
}
