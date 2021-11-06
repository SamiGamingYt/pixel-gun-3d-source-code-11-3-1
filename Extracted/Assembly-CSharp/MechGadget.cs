using System;

// Token: 0x02000630 RID: 1584
public class MechGadget : BaseEffectGadget
{
	// Token: 0x060036AD RID: 13997 RVA: 0x00119F9C File Offset: 0x0011819C
	public MechGadget(GadgetInfo _info, Player_move_c.GadgetEffect effect) : base(_info, effect, true)
	{
	}

	// Token: 0x060036AE RID: 13998 RVA: 0x00119FA8 File Offset: 0x001181A8
	public override void Use()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnMechKill = new Action(base.ResetUseCounter);
		base.Use();
	}

	// Token: 0x060036AF RID: 13999 RVA: 0x00119FCC File Offset: 0x001181CC
	public override void OnTimeExpire()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnMechKill = null;
		base.OnTimeExpire();
	}
}
