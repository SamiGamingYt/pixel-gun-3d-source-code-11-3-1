using System;

// Token: 0x02000635 RID: 1589
public class RessurectionGadget : PassiveGadget
{
	// Token: 0x060036BF RID: 14015 RVA: 0x0011A158 File Offset: 0x00118358
	public RessurectionGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x0011A164 File Offset: 0x00118364
	public override void PreUse()
	{
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x0011A168 File Offset: 0x00118368
	public override void Use()
	{
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x0011A16C File Offset: 0x0011836C
	public override void PostUse()
	{
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x0011A170 File Offset: 0x00118370
	public override void OnKill(bool inDeathCollider)
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.ApplyResurrection(inDeathCollider);
	}
}
