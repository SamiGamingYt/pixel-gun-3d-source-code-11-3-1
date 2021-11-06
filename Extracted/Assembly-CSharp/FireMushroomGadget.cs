using System;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class FireMushroomGadget : ImmediateGadget
{
	// Token: 0x060036CB RID: 14027 RVA: 0x0011A1E4 File Offset: 0x001183E4
	public FireMushroomGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x0011A1F0 File Offset: 0x001183F0
	public override void PreUse()
	{
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x0011A1F4 File Offset: 0x001183F4
	public override void Update()
	{
		if (this.nextHitTime < Time.time && this._durationTime.value > 0f)
		{
			this.nextHitTime = Time.time + 2f;
			WeaponManager.sharedManager.myPlayerMoveC.FireMushroomShot(this.Info);
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x0011A24C File Offset: 0x0011844C
	public override void Use()
	{
		this.StartUseTimer();
		this.nextHitTime = Time.time + 2f;
		WeaponManager.sharedManager.myPlayerMoveC.ActivateFireMushroom();
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x0011A280 File Offset: 0x00118480
	public override void OnTimeExpire()
	{
		this.StartCooldown();
	}

	// Token: 0x040027F8 RID: 10232
	private float nextHitTime;
}
