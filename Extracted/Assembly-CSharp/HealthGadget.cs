using System;

// Token: 0x02000632 RID: 1586
public class HealthGadget : ImmediateGadget
{
	// Token: 0x060036B3 RID: 14003 RVA: 0x0011A02C File Offset: 0x0011822C
	public HealthGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036B4 RID: 14004 RVA: 0x0011A038 File Offset: 0x00118238
	public override void PreUse()
	{
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x0011A03C File Offset: 0x0011823C
	public override void Use()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC.ApplyMedkit(this.Info))
		{
			this.StartCooldown();
		}
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x0011A06C File Offset: 0x0011826C
	public override void PostUse()
	{
	}
}
