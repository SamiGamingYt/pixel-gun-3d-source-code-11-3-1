using System;

// Token: 0x0200062F RID: 1583
public class BaseEffectGadget : ImmediateGadget
{
	// Token: 0x060036A9 RID: 13993 RVA: 0x00119EFC File Offset: 0x001180FC
	public BaseEffectGadget(GadgetInfo _info, Player_move_c.GadgetEffect effect, bool sendInfoID = false) : base(_info)
	{
		this.effect = effect;
		this.sendInfoID = sendInfoID;
	}

	// Token: 0x060036AA RID: 13994 RVA: 0x00119F14 File Offset: 0x00118114
	public override void PreUse()
	{
	}

	// Token: 0x060036AB RID: 13995 RVA: 0x00119F18 File Offset: 0x00118118
	public override void Use()
	{
		this.StartUseTimer();
		if (this.sendInfoID)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetGadgetEffectActivation(this.effect, true, this.Info.Id);
		}
		else
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetGadgetEffectActivation(this.effect, true, string.Empty);
		}
	}

	// Token: 0x060036AC RID: 13996 RVA: 0x00119F78 File Offset: 0x00118178
	public override void OnTimeExpire()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.SetGadgetEffectActivation(this.effect, false, string.Empty);
	}

	// Token: 0x040027F5 RID: 10229
	public Player_move_c.GadgetEffect effect;

	// Token: 0x040027F6 RID: 10230
	public bool sendInfoID;
}
