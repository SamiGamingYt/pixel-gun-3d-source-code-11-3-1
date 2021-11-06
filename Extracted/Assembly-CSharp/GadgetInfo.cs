using System;
using System.Collections.Generic;

// Token: 0x0200063B RID: 1595
public class GadgetInfo
{
	// Token: 0x06003704 RID: 14084 RVA: 0x0011CB80 File Offset: 0x0011AD80
	public GadgetInfo(GadgetInfo.GadgetCategory cat, string id, string locKey, int tier_FROM_ONE, string previousUpgradeId, string nextUpgradeId, string descriptionLkey, List<GadgetInfo.Parameter> parameters, List<WeaponSounds.Effects> effects, PlayerEventScoreController.ScoreEvent killStreakType)
	{
		this.Category = cat;
		this.Id = id;
		this.Lkey = locKey;
		this._tier_FROM_ZERO = tier_FROM_ONE - 1;
		this.PreviousUpgradeId = previousUpgradeId;
		this.NextUpgradeId = nextUpgradeId;
		this.DescriptionLkey = descriptionLkey;
		this.Parameters = parameters;
		this.Effects = effects;
		this.KillStreakType = killStreakType;
	}

	// Token: 0x170008FE RID: 2302
	// (get) Token: 0x06003705 RID: 14085 RVA: 0x0011CBE4 File Offset: 0x0011ADE4
	// (set) Token: 0x06003706 RID: 14086 RVA: 0x0011CBEC File Offset: 0x0011ADEC
	public List<GadgetInfo.Parameter> Parameters { get; protected set; }

	// Token: 0x170008FF RID: 2303
	// (get) Token: 0x06003707 RID: 14087 RVA: 0x0011CBF8 File Offset: 0x0011ADF8
	// (set) Token: 0x06003708 RID: 14088 RVA: 0x0011CC00 File Offset: 0x0011AE00
	public List<WeaponSounds.Effects> Effects { get; protected set; }

	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06003709 RID: 14089 RVA: 0x0011CC0C File Offset: 0x0011AE0C
	// (set) Token: 0x0600370A RID: 14090 RVA: 0x0011CC14 File Offset: 0x0011AE14
	public GadgetInfo.GadgetCategory Category { get; protected set; }

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x0600370B RID: 14091 RVA: 0x0011CC20 File Offset: 0x0011AE20
	// (set) Token: 0x0600370C RID: 14092 RVA: 0x0011CC28 File Offset: 0x0011AE28
	public string Id { get; protected set; }

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x0600370D RID: 14093 RVA: 0x0011CC34 File Offset: 0x0011AE34
	// (set) Token: 0x0600370E RID: 14094 RVA: 0x0011CC3C File Offset: 0x0011AE3C
	public string PreviousUpgradeId { get; protected set; }

	// Token: 0x17000903 RID: 2307
	// (get) Token: 0x0600370F RID: 14095 RVA: 0x0011CC48 File Offset: 0x0011AE48
	// (set) Token: 0x06003710 RID: 14096 RVA: 0x0011CC50 File Offset: 0x0011AE50
	public string NextUpgradeId { get; protected set; }

	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06003711 RID: 14097 RVA: 0x0011CC5C File Offset: 0x0011AE5C
	// (set) Token: 0x06003712 RID: 14098 RVA: 0x0011CC64 File Offset: 0x0011AE64
	public string Lkey { get; protected set; }

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06003713 RID: 14099 RVA: 0x0011CC70 File Offset: 0x0011AE70
	// (set) Token: 0x06003714 RID: 14100 RVA: 0x0011CC78 File Offset: 0x0011AE78
	public string DescriptionLkey { get; protected set; }

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x0011CC84 File Offset: 0x0011AE84
	// (set) Token: 0x06003716 RID: 14102 RVA: 0x0011CC8C File Offset: 0x0011AE8C
	public PlayerEventScoreController.ScoreEvent KillStreakType { get; protected set; }

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x06003717 RID: 14103 RVA: 0x0011CC98 File Offset: 0x0011AE98
	public float Heal
	{
		get
		{
			if (BalanceController.healGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.healGadgetes[this.Id];
			}
			return 5f;
		}
	}

	// Token: 0x17000908 RID: 2312
	// (get) Token: 0x06003718 RID: 14104 RVA: 0x0011CCD0 File Offset: 0x0011AED0
	public float HPS
	{
		get
		{
			if (BalanceController.hpsGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.hpsGadgetes[this.Id];
			}
			return 1f;
		}
	}

	// Token: 0x17000909 RID: 2313
	// (get) Token: 0x06003719 RID: 14105 RVA: 0x0011CD08 File Offset: 0x0011AF08
	public float Durability
	{
		get
		{
			if (BalanceController.durabilityGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.durabilityGadgetes[this.Id];
			}
			return 5f;
		}
	}

	// Token: 0x1700090A RID: 2314
	// (get) Token: 0x0600371A RID: 14106 RVA: 0x0011CD40 File Offset: 0x0011AF40
	public float DPS
	{
		get
		{
			if (BalanceController.dpsGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.dpsGadgetes[this.Id];
			}
			return 1f;
		}
	}

	// Token: 0x1700090B RID: 2315
	// (get) Token: 0x0600371B RID: 14107 RVA: 0x0011CD78 File Offset: 0x0011AF78
	public float Damage
	{
		get
		{
			if (BalanceController.damageGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.damageGadgetes[this.Id];
			}
			return 3f;
		}
	}

	// Token: 0x1700090C RID: 2316
	// (get) Token: 0x0600371C RID: 14108 RVA: 0x0011CDB0 File Offset: 0x0011AFB0
	public float Amplification
	{
		get
		{
			if (BalanceController.amplificationGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.amplificationGadgetes[this.Id];
			}
			return 25f;
		}
	}

	// Token: 0x1700090D RID: 2317
	// (get) Token: 0x0600371D RID: 14109 RVA: 0x0011CDE8 File Offset: 0x0011AFE8
	public int SurvivalDamage
	{
		get
		{
			if (BalanceController.survivalDamageGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.survivalDamageGadgetes[this.Id];
			}
			return 100;
		}
	}

	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x0600371E RID: 14110 RVA: 0x0011CE20 File Offset: 0x0011B020
	public float Duration
	{
		get
		{
			if (BalanceController.durationGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.durationGadgetes[this.Id];
			}
			return 15f;
		}
	}

	// Token: 0x1700090F RID: 2319
	// (get) Token: 0x0600371F RID: 14111 RVA: 0x0011CE58 File Offset: 0x0011B058
	public float Cooldown
	{
		get
		{
			if (BalanceController.cooldownGadgetes.ContainsKey(this.Id))
			{
				return BalanceController.cooldownGadgetes[this.Id];
			}
			return 10f;
		}
	}

	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06003720 RID: 14112 RVA: 0x0011CE90 File Offset: 0x0011B090
	// (set) Token: 0x06003721 RID: 14113 RVA: 0x0011CE98 File Offset: 0x0011B098
	public int Tier
	{
		get
		{
			return this._tier_FROM_ZERO;
		}
		protected set
		{
			this._tier_FROM_ZERO = value;
		}
	}

	// Token: 0x04002818 RID: 10264
	private int _tier_FROM_ZERO;

	// Token: 0x0200063C RID: 1596
	public enum Parameter
	{
		// Token: 0x04002823 RID: 10275
		Damage,
		// Token: 0x04002824 RID: 10276
		Durability,
		// Token: 0x04002825 RID: 10277
		Healing,
		// Token: 0x04002826 RID: 10278
		Lifetime,
		// Token: 0x04002827 RID: 10279
		Cooldown,
		// Token: 0x04002828 RID: 10280
		Attack,
		// Token: 0x04002829 RID: 10281
		HP,
		// Token: 0x0400282A RID: 10282
		Speed,
		// Token: 0x0400282B RID: 10283
		Respawn
	}

	// Token: 0x0200063D RID: 1597
	public enum GadgetCategory
	{
		// Token: 0x0400282D RID: 10285
		Throwing = 12500,
		// Token: 0x0400282E RID: 10286
		Support = 13500,
		// Token: 0x0400282F RID: 10287
		Tools = 13000
	}
}
