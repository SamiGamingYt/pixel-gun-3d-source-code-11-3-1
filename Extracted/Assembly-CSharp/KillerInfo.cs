using System;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class KillerInfo
{
	// Token: 0x060019CB RID: 6603 RVA: 0x00067DCC File Offset: 0x00065FCC
	public void CopyTo(KillerInfo killerInfo)
	{
		killerInfo.isSuicide = this.isSuicide;
		killerInfo.nickname = this.nickname;
		killerInfo.rankTex = this.rankTex;
		killerInfo.rank = this.rank;
		killerInfo.clanLogoTex = this.clanLogoTex;
		killerInfo.clanName = this.clanName;
		killerInfo.weapon = this.weapon;
		killerInfo.skinTex = this.skinTex;
		killerInfo.hat = this.hat;
		killerInfo.mask = this.mask;
		killerInfo.armor = this.armor;
		killerInfo.cape = this.cape;
		killerInfo.capeTex = this.capeTex;
		killerInfo.boots = this.boots;
		killerInfo.mechUpgrade = this.mechUpgrade;
		killerInfo.turretUpgrade = this.turretUpgrade;
		killerInfo.killerTransform = this.killerTransform;
		killerInfo.healthValue = this.healthValue;
		killerInfo.armorValue = this.armorValue;
		killerInfo.pet = this.pet;
		killerInfo.gadgetSupport = this.gadgetSupport;
		killerInfo.gadgetTrowing = this.gadgetTrowing;
		killerInfo.gadgetTools = this.gadgetTools;
	}

	// Token: 0x060019CC RID: 6604 RVA: 0x00067EF0 File Offset: 0x000660F0
	public void Reset()
	{
		this.isSuicide = false;
		this.isGrenade = false;
		this.isMech = false;
		this.isTurret = false;
		this.nickname = string.Empty;
		this.rankTex = null;
		this.rank = 0;
		this.clanLogoTex = null;
		this.clanName = string.Empty;
		this.weapon = string.Empty;
		this.skinTex = null;
		this.hat = string.Empty;
		this.mask = string.Empty;
		this.armor = string.Empty;
		this.cape = string.Empty;
		this.capeTex = null;
		this.boots = string.Empty;
		this.pet = string.Empty;
		this.gadgetSupport = string.Empty;
		this.gadgetTrowing = string.Empty;
		this.gadgetTools = string.Empty;
		this.mechUpgrade = 0;
		this.turretUpgrade = 0;
		this.killerTransform = null;
		this.healthValue = 0;
		this.armorValue = 0;
	}

	// Token: 0x04000F09 RID: 3849
	public bool isSuicide;

	// Token: 0x04000F0A RID: 3850
	public bool isGrenade;

	// Token: 0x04000F0B RID: 3851
	public bool isMech;

	// Token: 0x04000F0C RID: 3852
	public bool isTurret;

	// Token: 0x04000F0D RID: 3853
	public string nickname;

	// Token: 0x04000F0E RID: 3854
	public Texture2D rankTex;

	// Token: 0x04000F0F RID: 3855
	public int rank;

	// Token: 0x04000F10 RID: 3856
	public Texture clanLogoTex;

	// Token: 0x04000F11 RID: 3857
	public string clanName;

	// Token: 0x04000F12 RID: 3858
	public string weapon;

	// Token: 0x04000F13 RID: 3859
	public Texture skinTex;

	// Token: 0x04000F14 RID: 3860
	public string hat;

	// Token: 0x04000F15 RID: 3861
	public string mask;

	// Token: 0x04000F16 RID: 3862
	public string armor;

	// Token: 0x04000F17 RID: 3863
	public string cape;

	// Token: 0x04000F18 RID: 3864
	public Texture capeTex;

	// Token: 0x04000F19 RID: 3865
	public string boots;

	// Token: 0x04000F1A RID: 3866
	public int mechUpgrade;

	// Token: 0x04000F1B RID: 3867
	public int turretUpgrade;

	// Token: 0x04000F1C RID: 3868
	public Transform killerTransform;

	// Token: 0x04000F1D RID: 3869
	public int healthValue;

	// Token: 0x04000F1E RID: 3870
	public int armorValue;

	// Token: 0x04000F1F RID: 3871
	public string pet;

	// Token: 0x04000F20 RID: 3872
	public string gadgetSupport;

	// Token: 0x04000F21 RID: 3873
	public string gadgetTrowing;

	// Token: 0x04000F22 RID: 3874
	public string gadgetTools;
}
