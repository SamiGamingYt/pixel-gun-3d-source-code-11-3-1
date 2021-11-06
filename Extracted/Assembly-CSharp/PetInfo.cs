using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006F5 RID: 1781
[Serializable]
public class PetInfo
{
	// Token: 0x17000A47 RID: 2631
	// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0014C430 File Offset: 0x0014A630
	public float HP
	{
		get
		{
			if (BalanceController.hpPets.ContainsKey(this.Id))
			{
				return BalanceController.hpPets[this.Id];
			}
			return 9f;
		}
	}

	// Token: 0x17000A48 RID: 2632
	// (get) Token: 0x06003DDD RID: 15837 RVA: 0x0014C460 File Offset: 0x0014A660
	public float Attack
	{
		get
		{
			if (BalanceController.damagePets.ContainsKey(this.Id))
			{
				return BalanceController.damagePets[this.Id];
			}
			return 1f;
		}
	}

	// Token: 0x17000A49 RID: 2633
	// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0014C490 File Offset: 0x0014A690
	public float SurvivalAttack
	{
		get
		{
			if (BalanceController.survivalDamagePets.ContainsKey(this.Id))
			{
				return (float)BalanceController.survivalDamagePets[this.Id];
			}
			return 1f;
		}
	}

	// Token: 0x17000A4A RID: 2634
	// (get) Token: 0x06003DDF RID: 15839 RVA: 0x0014C4CC File Offset: 0x0014A6CC
	public int DPS
	{
		get
		{
			if (BalanceController.dpsPets.ContainsKey(this.Id))
			{
				return BalanceController.dpsPets[this.Id];
			}
			return 1;
		}
	}

	// Token: 0x17000A4B RID: 2635
	// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x0014C4F8 File Offset: 0x0014A6F8
	public float SpeedModif
	{
		get
		{
			if (BalanceController.speedPets.ContainsKey(this.Id))
			{
				return BalanceController.speedPets[this.Id];
			}
			return 4f;
		}
	}

	// Token: 0x17000A4C RID: 2636
	// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x0014C528 File Offset: 0x0014A728
	public float RespawnTime
	{
		get
		{
			if (BalanceController.respawnTimePets.ContainsKey(this.Id))
			{
				return BalanceController.respawnTimePets[this.Id];
			}
			return 4f;
		}
	}

	// Token: 0x17000A4D RID: 2637
	// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0014C558 File Offset: 0x0014A758
	public float Cashback
	{
		get
		{
			if (BalanceController.cashbackPets.ContainsKey(this.Id))
			{
				return (float)BalanceController.cashbackPets[this.Id];
			}
			return 4f;
		}
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x0014C594 File Offset: 0x0014A794
	public string GetRelativePrefabPath()
	{
		return string.Format("Pets/Content/{0}_up0", this.IdWithoutUp);
	}

	// Token: 0x17000A4E RID: 2638
	// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0014C5A8 File Offset: 0x0014A7A8
	// (set) Token: 0x06003DE5 RID: 15845 RVA: 0x0014C5B0 File Offset: 0x0014A7B0
	public Vector3 PositionInBanners
	{
		get
		{
			return this.m_positionInBanners;
		}
		set
		{
			this.m_positionInBanners = value;
		}
	}

	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0014C5BC File Offset: 0x0014A7BC
	// (set) Token: 0x06003DE7 RID: 15847 RVA: 0x0014C5C4 File Offset: 0x0014A7C4
	public Vector3 RotationInBanners
	{
		get
		{
			return this.m_rotationInBanners;
		}
		set
		{
			this.m_rotationInBanners = value;
		}
	}

	// Token: 0x04002DB5 RID: 11701
	public List<GadgetInfo.Parameter> Parameters;

	// Token: 0x04002DB6 RID: 11702
	public List<WeaponSounds.Effects> Effects;

	// Token: 0x04002DB7 RID: 11703
	public string Id;

	// Token: 0x04002DB8 RID: 11704
	public int Up;

	// Token: 0x04002DB9 RID: 11705
	public string IdWithoutUp;

	// Token: 0x04002DBA RID: 11706
	public PetInfo.BehaviourType Behaviour;

	// Token: 0x04002DBB RID: 11707
	public ItemDb.ItemRarity Rarity;

	// Token: 0x04002DBC RID: 11708
	public int Tier;

	// Token: 0x04002DBD RID: 11709
	public string Lkey;

	// Token: 0x04002DBE RID: 11710
	public int ToUpPoints;

	// Token: 0x04002DBF RID: 11711
	public float AttackDistance;

	// Token: 0x04002DC0 RID: 11712
	public float AttackStopDistance;

	// Token: 0x04002DC1 RID: 11713
	public float MinToOwnerDistance;

	// Token: 0x04002DC2 RID: 11714
	public float MaxToOwnerDistance;

	// Token: 0x04002DC3 RID: 11715
	public float TargetDetectRange;

	// Token: 0x04002DC4 RID: 11716
	public float OffenderDetectRange = 10f;

	// Token: 0x04002DC5 RID: 11717
	public float ToTargetTeleportDistance;

	// Token: 0x04002DC6 RID: 11718
	public bool poisonEnabled;

	// Token: 0x04002DC7 RID: 11719
	public Player_move_c.PoisonType poisonType;

	// Token: 0x04002DC8 RID: 11720
	public int poisonCount;

	// Token: 0x04002DC9 RID: 11721
	public float poisonTime;

	// Token: 0x04002DCA RID: 11722
	public float poisonDamagePercent;

	// Token: 0x04002DCB RID: 11723
	public int criticalHitChance;

	// Token: 0x04002DCC RID: 11724
	public float criticalHitCoef;

	// Token: 0x04002DCD RID: 11725
	private Vector3 m_positionInBanners;

	// Token: 0x04002DCE RID: 11726
	private Vector3 m_rotationInBanners;

	// Token: 0x020006F6 RID: 1782
	public enum BehaviourType
	{
		// Token: 0x04002DD0 RID: 11728
		Ground,
		// Token: 0x04002DD1 RID: 11729
		Flying
	}

	// Token: 0x020006F7 RID: 1783
	public enum Parameter
	{
		// Token: 0x04002DD3 RID: 11731
		Durability,
		// Token: 0x04002DD4 RID: 11732
		Attack,
		// Token: 0x04002DD5 RID: 11733
		Cooldown
	}

	// Token: 0x020006F8 RID: 1784
	public enum Effect
	{
		// Token: 0x04002DD7 RID: 11735
		Healing,
		// Token: 0x04002DD8 RID: 11736
		Flamethrower,
		// Token: 0x04002DD9 RID: 11737
		Poison
	}
}
