using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public abstract class Gadget
{
	// Token: 0x06003671 RID: 13937 RVA: 0x00119168 File Offset: 0x00117368
	protected Gadget(GadgetInfo _info)
	{
		this.Info = _info;
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x00119198 File Offset: 0x00117398
	public static Gadget Create(GadgetInfo gadgetInfo)
	{
		if (gadgetInfo == null)
		{
			return null;
		}
		if (string.IsNullOrEmpty(gadgetInfo.Id))
		{
			return null;
		}
		Gadget result = null;
		try
		{
			if (GadgetsInfo.Upgrades["gadget_fraggrenade"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_molotov"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_mine"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_firework"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_fakebonus"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_singularity"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_nucleargrenade"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_nutcracker"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_Blizzard_generator"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_black_label"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_stickycandy"].Contains(gadgetInfo.Id))
			{
				result = new BaseGrenadeGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_ninjashurickens"].Contains(gadgetInfo.Id))
			{
				result = new ShurikenGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_turret"].Contains(gadgetInfo.Id))
			{
				result = new BaseTurretGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_shield"].Contains(gadgetInfo.Id))
			{
				result = new BaseTurretGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_medicalstation"].Contains(gadgetInfo.Id))
			{
				result = new BaseTurretGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_snowman"].Contains(gadgetInfo.Id))
			{
				result = new BaseTurretGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_christmastreeturret"].Contains(gadgetInfo.Id))
			{
				result = new BaseTurretGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_stealth"].Contains(gadgetInfo.Id))
			{
				result = new BaseEffectGadget(gadgetInfo, Player_move_c.GadgetEffect.invisible, false);
			}
			if (GadgetsInfo.Upgrades["gadget_jetpack"].Contains(gadgetInfo.Id))
			{
				result = new BaseEffectGadget(gadgetInfo, Player_move_c.GadgetEffect.jetpack, false);
			}
			if (GadgetsInfo.Upgrades["gadget_reflector"].Contains(gadgetInfo.Id))
			{
				result = new BaseEffectGadget(gadgetInfo, Player_move_c.GadgetEffect.reflector, true);
			}
			if (GadgetsInfo.Upgrades["gadget_leaderdrum"].Contains(gadgetInfo.Id))
			{
				result = new BaseEffectGadget(gadgetInfo, Player_move_c.GadgetEffect.drumSupport, true);
			}
			if (GadgetsInfo.Upgrades["gadget_petbooster"].Contains(gadgetInfo.Id))
			{
				result = new PetAdrenalineGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_mech"].Contains(gadgetInfo.Id))
			{
				result = new MechGadget(gadgetInfo, Player_move_c.GadgetEffect.mech);
			}
			if (GadgetsInfo.Upgrades["gadget_demon_stone"].Contains(gadgetInfo.Id))
			{
				result = new MechGadget(gadgetInfo, Player_move_c.GadgetEffect.demon);
			}
			if (GadgetsInfo.Upgrades["gadget_medkit"].Contains(gadgetInfo.Id))
			{
				result = new HealthGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_timewatch"].Contains(gadgetInfo.Id))
			{
				result = new TimeWatchGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_resurrection"].Contains(gadgetInfo.Id))
			{
				result = new RessurectionGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_disabler"].Contains(gadgetInfo.Id))
			{
				result = new DisablerGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_firemushroom"].Contains(gadgetInfo.Id))
			{
				result = new FireMushroomGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_dragonwhistle"].Contains(gadgetInfo.Id))
			{
				result = new DragonWhistleGadget(gadgetInfo);
			}
			if (GadgetsInfo.Upgrades["gadget_pandorabox"].Contains(gadgetInfo.Id))
			{
				result = new PandoraBoxGadget(gadgetInfo);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in Gadget.Create: {0}", new object[]
			{
				ex
			});
		}
		return result;
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x001196A4 File Offset: 0x001178A4
	public void ResetUseCounter()
	{
		this._durationTime.value = 0f;
		this.OnTimeExpire();
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x001196BC File Offset: 0x001178BC
	public void ResetCooldownCounter()
	{
		this._cooldownTime.value = 0f;
	}

	// Token: 0x06003675 RID: 13941
	public abstract void PreUse();

	// Token: 0x06003676 RID: 13942
	public abstract void Use();

	// Token: 0x06003677 RID: 13943 RVA: 0x001196D0 File Offset: 0x001178D0
	public virtual void PostUse()
	{
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x001196D4 File Offset: 0x001178D4
	public virtual void Update()
	{
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x001196D8 File Offset: 0x001178D8
	public virtual void StartCooldown()
	{
		this._cooldownTime.value = this.Info.Cooldown;
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x001196F0 File Offset: 0x001178F0
	public virtual void StartUseTimer()
	{
		this._durationTime.value = this.Info.Duration;
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00119708 File Offset: 0x00117908
	public virtual void Step(float time)
	{
		if (this._cooldownTime.value > 0f)
		{
			this._cooldownTime.value -= time;
			if (this._cooldownTime.value < 0f)
			{
				this._cooldownTime.value = 0f;
			}
		}
		if (this._durationTime.value > 0f)
		{
			this._durationTime.value -= time;
			if (this._durationTime.value < 0f)
			{
				this._durationTime.value = 0f;
				this.OnTimeExpire();
			}
		}
		this.Update();
	}

	// Token: 0x170008F0 RID: 2288
	// (get) Token: 0x0600367C RID: 13948 RVA: 0x001197BC File Offset: 0x001179BC
	public virtual bool CanUse
	{
		get
		{
			return this._cooldownTime.value == 0f && this._durationTime.value == 0f;
		}
	}

	// Token: 0x170008F1 RID: 2289
	// (get) Token: 0x0600367D RID: 13949 RVA: 0x001197F4 File Offset: 0x001179F4
	public float CooldownProgress
	{
		get
		{
			return this._cooldownTime.value / this.Info.Cooldown;
		}
	}

	// Token: 0x170008F2 RID: 2290
	// (get) Token: 0x0600367E RID: 13950 RVA: 0x00119818 File Offset: 0x00117A18
	public float ExpirationProgress
	{
		get
		{
			return this._durationTime.value / this.Info.Duration;
		}
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x0011983C File Offset: 0x00117A3C
	public virtual void OnKill(bool inDeathCollider)
	{
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00119840 File Offset: 0x00117A40
	public virtual void OnTimeExpire()
	{
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x00119844 File Offset: 0x00117A44
	public virtual void OnMatchEnd()
	{
	}

	// Token: 0x170008F3 RID: 2291
	// (get) Token: 0x06003682 RID: 13954 RVA: 0x00119848 File Offset: 0x00117A48
	// (set) Token: 0x06003683 RID: 13955 RVA: 0x00119850 File Offset: 0x00117A50
	public virtual GadgetInfo Info { get; protected set; }

	// Token: 0x040027ED RID: 10221
	protected SaltedFloat _cooldownTime = new SaltedFloat(0f);

	// Token: 0x040027EE RID: 10222
	protected SaltedFloat _durationTime = new SaltedFloat(0f);
}
