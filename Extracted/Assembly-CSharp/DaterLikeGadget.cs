using System;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class DaterLikeGadget : ThrowGadget
{
	// Token: 0x060036A3 RID: 13987 RVA: 0x00119D94 File Offset: 0x00117F94
	public DaterLikeGadget() : base(null)
	{
	}

	// Token: 0x060036A4 RID: 13988 RVA: 0x00119DA0 File Offset: 0x00117FA0
	public override void PreUse()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GrenadePress(this);
	}

	// Token: 0x060036A5 RID: 13989 RVA: 0x00119DB4 File Offset: 0x00117FB4
	public override void Use()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GrenadeFire();
	}

	// Token: 0x060036A6 RID: 13990 RVA: 0x00119DC8 File Offset: 0x00117FC8
	public override void CreateRocket(WeaponSounds weapon)
	{
		Rocket rocket = Player_move_c.CreateRocket(weapon, Vector3.down * 10000f, Quaternion.identity, 1f);
		if (Defs.isMulti && !Defs.isInet)
		{
			rocket.SendNetworkViewMyPlayer(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.GetComponent<NetworkView>().viewID);
		}
		this.currentGrenade = rocket.gameObject;
	}

	// Token: 0x060036A7 RID: 13991 RVA: 0x00119E34 File Offset: 0x00118034
	public override void ThrowGrenade()
	{
		if (this.currentGrenade == null)
		{
			return;
		}
		Rocket component = this.currentGrenade.GetComponent<Rocket>();
		float d = (!(component.currentRocketSettings != null)) ? 150f : component.currentRocketSettings.startForce;
		this.currentGrenade.GetComponent<Rigidbody>().isKinematic = false;
		this.currentGrenade.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(0f, -5f, 0f) * (d * WeaponManager.sharedManager.myPlayerMoveC.myTransform.forward));
		this.currentGrenade.GetComponent<Rigidbody>().useGravity = true;
		component.RunGrenade();
	}

	// Token: 0x170008F8 RID: 2296
	// (get) Token: 0x060036A8 RID: 13992 RVA: 0x00119EF4 File Offset: 0x001180F4
	public override string GrenadeGadgetId
	{
		get
		{
			return "Like";
		}
	}

	// Token: 0x040027F4 RID: 10228
	public GameObject currentGrenade;
}
