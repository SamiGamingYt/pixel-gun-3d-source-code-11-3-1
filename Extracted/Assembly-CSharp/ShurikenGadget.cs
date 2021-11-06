using System;
using UnityEngine;

// Token: 0x0200062C RID: 1580
public class ShurikenGadget : ThrowGadget
{
	// Token: 0x06003696 RID: 13974 RVA: 0x00119A90 File Offset: 0x00117C90
	public ShurikenGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x06003697 RID: 13975 RVA: 0x00119A9C File Offset: 0x00117C9C
	public override void PreUse()
	{
		WeaponManager.sharedManager.myPlayerMoveC.GrenadePress(this);
	}

	// Token: 0x06003698 RID: 13976 RVA: 0x00119AB0 File Offset: 0x00117CB0
	public override void Use()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.GrenadeFire();
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x00119AC8 File Offset: 0x00117CC8
	public override void CreateRocket(WeaponSounds weapon)
	{
		this.currentGrenades = new GameObject[3];
		for (int i = 0; i < this.currentGrenades.Length; i++)
		{
			Rocket rocket = Player_move_c.CreateRocket(weapon, WeaponManager.sharedManager.myPlayerMoveC.myCurrentWeaponSounds.grenatePoint.position, WeaponManager.sharedManager.myPlayerMoveC.myCurrentWeaponSounds.grenatePoint.rotation, 1f);
			if (Defs.isMulti && !Defs.isInet)
			{
				rocket.SendNetworkViewMyPlayer(WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.GetComponent<NetworkView>().viewID);
			}
			this.currentGrenades[i] = rocket.gameObject;
			rocket.multiplayerDamage = this.Info.Damage;
			rocket.damage = (float)this.Info.SurvivalDamage;
		}
	}

	// Token: 0x0600369A RID: 13978 RVA: 0x00119B9C File Offset: 0x00117D9C
	public override void ThrowGrenade()
	{
		if (this.currentGrenades == null || this.currentGrenades.Length == 0 || this.currentGrenades[0] == null)
		{
			return;
		}
		this.currentGrenades[0].transform.parent.gameObject.SetActive(true);
		for (int i = 0; i < this.currentGrenades.Length; i++)
		{
			Rocket component = this.currentGrenades[i].GetComponent<Rocket>();
			float d = (!(component.currentRocketSettings != null)) ? 150f : component.currentRocketSettings.startForce;
			component.GetComponent<Rigidbody>().isKinematic = false;
			component.GetComponent<Rigidbody>().AddForce(Quaternion.Euler(0f, -15f + (float)(i * 15), 0f) * (d * WeaponManager.sharedManager.myPlayerMoveC.myTransform.forward));
			component.GetComponent<Rigidbody>().useGravity = false;
			component.RunGrenade();
		}
	}

	// Token: 0x170008F6 RID: 2294
	// (get) Token: 0x0600369B RID: 13979 RVA: 0x00119CA4 File Offset: 0x00117EA4
	public override string GrenadeGadgetId
	{
		get
		{
			return GadgetsInfo.BaseName(this.Info.Id);
		}
	}

	// Token: 0x040027F2 RID: 10226
	public GameObject[] currentGrenades;
}
