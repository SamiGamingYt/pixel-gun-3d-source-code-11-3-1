using System;
using UnityEngine;

// Token: 0x0200062B RID: 1579
public class BaseGrenadeGadget : ThrowGadget
{
	// Token: 0x06003690 RID: 13968 RVA: 0x001198E0 File Offset: 0x00117AE0
	public BaseGrenadeGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x001198EC File Offset: 0x00117AEC
	public override void PreUse()
	{
		base.KillCurrentRocket();
		WeaponManager.sharedManager.myPlayerMoveC.GrenadePress(this);
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x00119904 File Offset: 0x00117B04
	public override void Use()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.GrenadeFire();
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x0011991C File Offset: 0x00117B1C
	public override void CreateRocket(WeaponSounds weapon)
	{
		Rocket rocket = Player_move_c.CreateRocket(weapon, Vector3.down * 10000f, Quaternion.identity, 1f);
		if (Defs.isMulti && !Defs.isInet)
		{
			rocket.SendNetworkViewMyPlayer(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.GetComponent<NetworkView>().viewID);
		}
		this.currentGrenade = rocket.gameObject;
		rocket.multiplayerDamage = this.Info.Damage;
		rocket.damage = (float)this.Info.SurvivalDamage;
		base.SetCurrentRocket(this.currentGrenade.GetComponent<Rocket>());
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x001199BC File Offset: 0x00117BBC
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

	// Token: 0x170008F5 RID: 2293
	// (get) Token: 0x06003695 RID: 13973 RVA: 0x00119A7C File Offset: 0x00117C7C
	public override string GrenadeGadgetId
	{
		get
		{
			return GadgetsInfo.BaseName(this.Info.Id);
		}
	}

	// Token: 0x040027F1 RID: 10225
	public GameObject currentGrenade;
}
