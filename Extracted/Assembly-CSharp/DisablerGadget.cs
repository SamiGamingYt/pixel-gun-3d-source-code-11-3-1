using System;

// Token: 0x02000637 RID: 1591
public class DisablerGadget : ImmediateGadget
{
	// Token: 0x060036C8 RID: 14024 RVA: 0x0011A1B4 File Offset: 0x001183B4
	public DisablerGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x0011A1C0 File Offset: 0x001183C0
	public override void PreUse()
	{
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x0011A1C4 File Offset: 0x001183C4
	public override void Use()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.DisablerGadget(this.Info);
	}
}
