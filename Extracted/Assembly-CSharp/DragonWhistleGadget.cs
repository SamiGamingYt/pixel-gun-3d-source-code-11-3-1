using System;

// Token: 0x02000639 RID: 1593
public class DragonWhistleGadget : ImmediateGadget
{
	// Token: 0x060036D0 RID: 14032 RVA: 0x0011A288 File Offset: 0x00118488
	public DragonWhistleGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x0011A294 File Offset: 0x00118494
	public override void PreUse()
	{
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x0011A298 File Offset: 0x00118498
	public override void Use()
	{
		WeaponManager.sharedManager.myPlayerMoveC.UseDragonWhistle(this.Info);
		this.StartCooldown();
	}
}
