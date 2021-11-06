using System;

// Token: 0x02000631 RID: 1585
public class PetAdrenalineGadget : BaseEffectGadget
{
	// Token: 0x060036B0 RID: 14000 RVA: 0x00119FE4 File Offset: 0x001181E4
	public PetAdrenalineGadget(GadgetInfo _info) : base(_info, Player_move_c.GadgetEffect.petAdrenaline, false)
	{
	}

	// Token: 0x060036B1 RID: 14001 RVA: 0x00119FF0 File Offset: 0x001181F0
	public override void Use()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnPetKill = new Action(base.ResetUseCounter);
		base.Use();
	}

	// Token: 0x060036B2 RID: 14002 RVA: 0x0011A014 File Offset: 0x00118214
	public override void OnTimeExpire()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GadgetsOnPetKill = null;
		base.OnTimeExpire();
	}
}
