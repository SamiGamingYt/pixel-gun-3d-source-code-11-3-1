using System;

// Token: 0x02000636 RID: 1590
public class TimeWatchGadget : ImmediateGadget
{
	// Token: 0x060036C4 RID: 14020 RVA: 0x0011A188 File Offset: 0x00118388
	public TimeWatchGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x0011A194 File Offset: 0x00118394
	public override void PreUse()
	{
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x0011A198 File Offset: 0x00118398
	public override void Use()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.ApplyTimeWatch();
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x0011A1B0 File Offset: 0x001183B0
	public override void PostUse()
	{
	}
}
