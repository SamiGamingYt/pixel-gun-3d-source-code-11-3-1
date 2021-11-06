using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200080F RID: 2063
public sealed class WeaponSounds : MonoBehaviour
{
	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06004B43 RID: 19267 RVA: 0x001AE42C File Offset: 0x001AC62C
	public GameObject BearWeaponObject
	{
		get
		{
			return this.BearWeapon;
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x06004B44 RID: 19268 RVA: 0x001AE434 File Offset: 0x001AC634
	public float DPS
	{
		get
		{
			if (ExpController.Instance == null)
			{
				return 0f;
			}
			int ourTier = ExpController.Instance.OurTier;
			int num = Math.Max(ourTier, this.tier);
			if (this.dpsesCorrectedByRememberedGun.Length <= num)
			{
				return 0f;
			}
			return this.dpsesCorrectedByRememberedGun[num];
		}
	}

	// Token: 0x06004B45 RID: 19269 RVA: 0x001AE48C File Offset: 0x001AC68C
	public void SetDaterBearHandsAnim(bool set)
	{
		this.bearActive = (set && this.BearWeapon != null);
		this._innerPars.animationObject.SetActive(!this.bearActive);
		if (this.BearWeapon != null)
		{
			this.BearWeapon.SetActive(this.bearActive);
		}
	}

	// Token: 0x06004B46 RID: 19270 RVA: 0x001AE4F0 File Offset: 0x001AC6F0
	public void Initialize()
	{
		string text = base.gameObject.name.Replace("(Clone)", string.Empty);
		string a;
		if (text.Contains("Weapon"))
		{
			a = Defs.InnerWeaponsFolder;
		}
		else
		{
			a = Defs.GadgetContentFolder;
		}
		string path = ResPath.Combine(a, base.gameObject.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		LoadAsyncTool.ObjectRequest objectRequest = LoadAsyncTool.Get(path, true);
		this.Initialize(objectRequest.asset as GameObject);
		if (this._innerWeaponPars != null)
		{
			Player_move_c.SetLayerRecursively(this._innerWeaponPars.gameObject, base.gameObject.layer);
		}
	}

	// Token: 0x06004B47 RID: 19271 RVA: 0x001AE5AC File Offset: 0x001AC7AC
	public void Initialize(GameObject pref)
	{
		if (this._innerWeaponPars != null)
		{
			return;
		}
		if (pref != null)
		{
			this._innerWeaponPars = (UnityEngine.Object.Instantiate(pref, new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject).GetComponent<InnerWeaponPars>();
			if (Defs.isDaterRegim)
			{
				string path = "MechBearWeapons/" + base.gameObject.name.Replace("(Clone)", string.Empty) + "_MechBear";
				UnityEngine.Object @object = Resources.Load(path);
				if (@object != null)
				{
					this.BearWeapon = (GameObject)UnityEngine.Object.Instantiate(@object, new Vector3(0f, 0f, 0f), Quaternion.identity);
					this._bearPars = this.BearWeapon.GetComponent<BearInnerWeaponPars>();
					this.BearWeapon.transform.SetParent(base.gameObject.transform, false);
					this.BearWeapon.SetActive(false);
				}
			}
			this._innerWeaponPars.gameObject.transform.SetParent(base.gameObject.transform, false);
		}
		if (!this.isMelee)
		{
			this.gunFlash = ((base.transform.childCount <= 0 || base.transform.GetChild(0).childCount <= 0) ? null : base.transform.GetChild(0).GetChild(0));
		}
	}

	// Token: 0x06004B48 RID: 19272 RVA: 0x001AE724 File Offset: 0x001AC924
	private void OnDestroy()
	{
		if (this._innerPars != null)
		{
			UnityEngine.Object.Destroy(this._innerPars.gameObject);
		}
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06004B49 RID: 19273 RVA: 0x001AE754 File Offset: 0x001AC954
	public GameObject animationObject
	{
		get
		{
			return (!this.bearActive) ? ((!(this._innerPars != null)) ? null : this._innerPars.animationObject) : this.BearWeapon;
		}
	}

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06004B4A RID: 19274 RVA: 0x001AE79C File Offset: 0x001AC99C
	public Texture preview
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.preview;
		}
	}

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06004B4B RID: 19275 RVA: 0x001AE7CC File Offset: 0x001AC9CC
	public AudioClip shoot
	{
		get
		{
			return (!this.bearActive || !(this._bearPars != null) || !(this._bearPars.shoot != null)) ? ((!(this._innerPars != null)) ? null : this._innerPars.shoot) : this._bearPars.shoot;
		}
	}

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06004B4C RID: 19276 RVA: 0x001AE840 File Offset: 0x001ACA40
	public AudioClip reload
	{
		get
		{
			return (!this.bearActive || !(this._bearPars != null) || !(this._bearPars.reload != null)) ? ((!(this._innerPars != null)) ? null : this._innerPars.reload) : this._bearPars.reload;
		}
	}

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06004B4D RID: 19277 RVA: 0x001AE8B4 File Offset: 0x001ACAB4
	public AudioClip empty
	{
		get
		{
			return (!this.bearActive || !(this._bearPars != null) || !(this._bearPars.empty != null)) ? ((!(this._innerPars != null)) ? null : this._innerPars.empty) : this._bearPars.empty;
		}
	}

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x06004B4E RID: 19278 RVA: 0x001AE928 File Offset: 0x001ACB28
	public AudioClip idle
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.idle;
		}
	}

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06004B4F RID: 19279 RVA: 0x001AE958 File Offset: 0x001ACB58
	public AudioClip zoomIn
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.zoomIn;
		}
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06004B50 RID: 19280 RVA: 0x001AE988 File Offset: 0x001ACB88
	public AudioClip zoomOut
	{
		get
		{
			return (!(this._innerPars != null)) ? null : ((!(this._innerPars.zoomOut != null)) ? this._innerPars.zoomIn : this._innerPars.zoomOut);
		}
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06004B51 RID: 19281 RVA: 0x001AE9E0 File Offset: 0x001ACBE0
	public AudioClip charge
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.charge;
		}
	}

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x06004B52 RID: 19282 RVA: 0x001AEA10 File Offset: 0x001ACC10
	public GameObject bonusPrefab
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.bonusPrefab;
		}
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06004B53 RID: 19283 RVA: 0x001AEA40 File Offset: 0x001ACC40
	public GameObject fakeGrenade
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.fakeGrenade;
		}
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06004B54 RID: 19284 RVA: 0x001AEA70 File Offset: 0x001ACC70
	public Texture2D aimTextureV
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.aimTextureV;
		}
	}

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06004B55 RID: 19285 RVA: 0x001AEAA0 File Offset: 0x001ACCA0
	public Texture2D aimTextureH
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.aimTextureH;
		}
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06004B56 RID: 19286 RVA: 0x001AEAD0 File Offset: 0x001ACCD0
	public Transform LeftArmorHand
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.LeftArmorHand;
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x06004B57 RID: 19287 RVA: 0x001AEB00 File Offset: 0x001ACD00
	public Transform RightArmorHand
	{
		get
		{
			return (!(this._innerPars != null)) ? null : this._innerPars.RightArmorHand;
		}
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x06004B58 RID: 19288 RVA: 0x001AEB30 File Offset: 0x001ACD30
	public Transform grenatePoint
	{
		get
		{
			return (!this.bearActive || !(this._bearPars != null)) ? ((!(this._innerPars != null)) ? null : this._innerPars.grenatePoint) : this._bearPars.grenatePoint;
		}
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x06004B59 RID: 19289 RVA: 0x001AEB8C File Offset: 0x001ACD8C
	[HideInInspector]
	public InnerWeaponPars _innerPars
	{
		get
		{
			if (this._innerWeaponPars == null)
			{
				this.Initialize();
			}
			return this._innerWeaponPars;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06004B5A RID: 19290 RVA: 0x001AEBAC File Offset: 0x001ACDAC
	public float[] DamageByTier
	{
		get
		{
			string text = base.gameObject.name.Replace("(Clone)", string.Empty);
			if (this.DPSRememberWhenGet)
			{
				if (this.damageByTierRememberedTierWhereGet == null)
				{
					int @int = Storager.getInt("RememberedTierWhenObtainGun_" + text, false);
					this.damageByTierRememberedTierWhereGet = new float[this.damageByTier.Length];
					float[] array = (!BalanceController.damageWeapons.ContainsKey(text)) ? this.damageByTier : BalanceController.damageWeapons[text];
					for (int i = 0; i <= @int; i++)
					{
						this.damageByTierRememberedTierWhereGet[i] = array[i];
					}
					for (int j = @int + 1; j < this.damageByTierRememberedTierWhereGet.Length; j++)
					{
						this.damageByTierRememberedTierWhereGet[j] = array[@int];
					}
				}
				return this.damageByTierRememberedTierWhereGet;
			}
			if (BalanceController.damageWeapons.ContainsKey(text))
			{
				return BalanceController.damageWeapons[text];
			}
			return this.damageByTier;
		}
	}

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x06004B5B RID: 19291 RVA: 0x001AECA8 File Offset: 0x001ACEA8
	public float[] dpsesCorrectedByRememberedGun
	{
		get
		{
			string text = base.gameObject.name.Replace("(Clone)", string.Empty);
			if (this.DPSRememberWhenGet)
			{
				if (!this._dpsesCorrectedByRememberedGunInitialized)
				{
					int @int = Storager.getInt("RememberedTierWhenObtainGun_" + text, false);
					this._dpsesCorrectedByRememberedGun = new float[this.dpses.Length];
					float[] array = (!BalanceController.dpsWeapons.ContainsKey(text)) ? this.dpses : BalanceController.dpsWeapons[text];
					for (int i = 0; i <= @int; i++)
					{
						this._dpsesCorrectedByRememberedGun[i] = array[i];
					}
					for (int j = @int + 1; j < this._dpsesCorrectedByRememberedGun.Length; j++)
					{
						this._dpsesCorrectedByRememberedGun[j] = array[@int];
					}
					this._dpsesCorrectedByRememberedGunInitialized = true;
				}
				return this._dpsesCorrectedByRememberedGun;
			}
			if (BalanceController.dpsWeapons.ContainsKey(text))
			{
				float[] array2 = BalanceController.dpsWeapons[text];
				return BalanceController.dpsWeapons[text];
			}
			return this.dpses;
		}
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x06004B5C RID: 19292 RVA: 0x001AEDB8 File Offset: 0x001ACFB8
	public int CapacityShop
	{
		get
		{
			return this.ammoInClip;
		}
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x06004B5D RID: 19293 RVA: 0x001AEDC0 File Offset: 0x001ACFC0
	public int mobilityShop
	{
		get
		{
			return (int)(this.speedModifier * 100f);
		}
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x06004B5E RID: 19294 RVA: 0x001AEDD0 File Offset: 0x001ACFD0
	public int damageShop
	{
		get
		{
			return Mathf.RoundToInt(this.dpsesCorrectedByRememberedGun[this.dpsesCorrectedByRememberedGun.Length - 1]);
		}
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x06004B5F RID: 19295 RVA: 0x001AEDE8 File Offset: 0x001ACFE8
	public int survivalDamage
	{
		get
		{
			string key = base.gameObject.name.Replace("(Clone)", string.Empty);
			if (BalanceController.survivalDamageWeapons.ContainsKey(key))
			{
				return BalanceController.survivalDamageWeapons[key];
			}
			return this.damage;
		}
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x06004B60 RID: 19296 RVA: 0x001AEE34 File Offset: 0x001AD034
	public int MaxAmmoWithEffectApplied
	{
		get
		{
			return (int)((float)this.maxAmmo * EffectsController.AmmoModForCategory(this.categoryNabor - 1));
		}
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x06004B61 RID: 19297 RVA: 0x001AEE4C File Offset: 0x001AD04C
	public int InitialAmmoWithEffectsApplied
	{
		get
		{
			return (int)((float)this.InitialAmmo * EffectsController.AmmoModForCategory(this.categoryNabor - 1));
		}
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x06004B62 RID: 19298 RVA: 0x001AEE64 File Offset: 0x001AD064
	public string shopName
	{
		get
		{
			return LocalizationStore.Get(this.localizeWeaponKey);
		}
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x06004B63 RID: 19299 RVA: 0x001AEE74 File Offset: 0x001AD074
	public string shopNameNonLocalized
	{
		get
		{
			return LocalizationStore.GetByDefault(this.localizeWeaponKey).ToUpper();
		}
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x001AEE88 File Offset: 0x001AD088
	private void Start()
	{
		if (string.IsNullOrEmpty(this.bazookaExplosionName))
		{
			this.bazookaExplosionName = base.gameObject.name.Replace("(Clone)", string.Empty);
		}
		if (this.isDoubleShot)
		{
			if (this.animationObject != null && this.animationObject.GetComponent<Animation>()["Shoot1"] != null)
			{
				this.animLength = this.animationObject.GetComponent<Animation>()["Shoot1"].length;
			}
		}
		else if (this.animationObject != null && this.animationObject.GetComponent<Animation>()["Shoot"] != null)
		{
			this.animLength = this.animationObject.GetComponent<Animation>()["Shoot"].length;
		}
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x001AEF78 File Offset: 0x001AD178
	private void Update()
	{
		if (this.timeFromFire < this.animLength)
		{
			this.timeFromFire += Time.deltaTime;
			if (this.tekKoof > 1f)
			{
				this.tekKoof -= this.downKoofFirst * Time.deltaTime / this.animLength;
			}
			if (this.tekKoof < 1f)
			{
				this.tekKoof = 1f;
			}
		}
		else
		{
			if (this.tekKoof > 1f)
			{
				this.tekKoof -= this.downKoof * Time.deltaTime / this.animLength;
			}
			if (this.tekKoof < 1f)
			{
				this.tekKoof = 1f;
			}
		}
		this.CheckPlayDefaultAnimInMulti();
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x001AF04C File Offset: 0x001AD24C
	private void LateUpdate()
	{
		this.CheckForInvisible();
	}

	// Token: 0x06004B67 RID: 19303 RVA: 0x001AF054 File Offset: 0x001AD254
	public void CheckForInvisible()
	{
		if (base.transform.parent != null)
		{
			if (this.myPlayerC == null)
			{
				this.myPlayerC = base.transform.parent.GetComponent<Player_move_c>();
			}
			if (base.transform.parent != null && this.myPlayerC != null && !this.myPlayerC.isMine && this.myPlayerC.isMulti && this.animationObject.activeSelf == this.myPlayerC.isInvisible)
			{
				this.animationObject.SetActive(!this.myPlayerC.isInvisible);
			}
		}
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x001AF11C File Offset: 0x001AD31C
	private void CheckPlayDefaultAnimInMulti()
	{
		if (!Defs.isInet)
		{
			return;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		Player_move_c component = base.transform.parent.GetComponent<Player_move_c>();
		if (component != null && !component.isMine && !this._innerPars.GetComponent<Animation>().isPlaying)
		{
			this._innerPars.GetComponent<Animation>().Play("Idle");
		}
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x001AF194 File Offset: 0x001AD394
	public bool IsAvalibleFromFilter(int filter)
	{
		return filter == 0 || (this.filterMap != null && this.filterMap.Contains(filter));
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x001AF1C0 File Offset: 0x001AD3C0
	public void fire()
	{
		this.timeFromFire = 0f;
		this.tekKoof += this.upKoofFire + this.downKoofFirst;
		if (this.tekKoof > this.maxKoof + this.downKoofFirst)
		{
			this.tekKoof = this.maxKoof + this.downKoofFirst;
		}
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x001AF220 File Offset: 0x001AD420
	public List<GameObject> GetListWeaponAnimEffects()
	{
		if (this._innerPars == null)
		{
			return null;
		}
		WeaponAnimParticleEffects component = this._innerPars.GetComponent<WeaponAnimParticleEffects>();
		if (component == null)
		{
			return null;
		}
		return component.GetListAnimEffects();
	}

	// Token: 0x040037D6 RID: 14294
	public const string RememberedTierWhereGetGunKey = "RememberedTierWhenObtainGun_";

	// Token: 0x040037D7 RID: 14295
	public ItemDb.ItemRarity rarity;

	// Token: 0x040037D8 RID: 14296
	public WeaponManager.WeaponTypeForLow typeForLow;

	// Token: 0x040037D9 RID: 14297
	public static readonly Dictionary<WeaponSounds.Effects, KeyValuePair<string, string>> keysAndSpritesForEffects = new Dictionary<WeaponSounds.Effects, KeyValuePair<string, string>>(29, new WeaponSounds.EffectComparer())
	{
		{
			WeaponSounds.Effects.Automatic,
			new KeyValuePair<string, string>("shop_stats_auto", "Key_1391")
		},
		{
			WeaponSounds.Effects.SingleShot,
			new KeyValuePair<string, string>("shop_stats_sngl", "Key_1392")
		},
		{
			WeaponSounds.Effects.Rockets,
			new KeyValuePair<string, string>("shop_stats_rkt", "Key_1394")
		},
		{
			WeaponSounds.Effects.Mortar,
			new KeyValuePair<string, string>("shop_stats_grav", "Key_1396")
		},
		{
			WeaponSounds.Effects.Laser,
			new KeyValuePair<string, string>("shop_stats_lsr", "Key_1393")
		},
		{
			WeaponSounds.Effects.Shotgun,
			new KeyValuePair<string, string>("shop_stats_shtgn", "Key_1390")
		},
		{
			WeaponSounds.Effects.Chainsaw,
			new KeyValuePair<string, string>("shop_stats_chain", "Key_1383")
		},
		{
			WeaponSounds.Effects.Flamethrower,
			new KeyValuePair<string, string>("shop_stats_fire", "Key_1387")
		},
		{
			WeaponSounds.Effects.ElectroThrower,
			new KeyValuePair<string, string>("shop_stats_elctrc", "Key_1395")
		},
		{
			WeaponSounds.Effects.WallBreak,
			new KeyValuePair<string, string>("shop_stats_no_wall", "Key_0402")
		},
		{
			WeaponSounds.Effects.AreaDamage,
			new KeyValuePair<string, string>("shop_stats_area_dmg", "Key_0403")
		},
		{
			WeaponSounds.Effects.Zoom,
			new KeyValuePair<string, string>("shop_stats_zoom", "Key_0404")
		},
		{
			WeaponSounds.Effects.ThroughEnemies,
			new KeyValuePair<string, string>("shop_stats_mtpl_enms", "Key_1388")
		},
		{
			WeaponSounds.Effects.Detonation,
			new KeyValuePair<string, string>("shop_stats_det", "Key_1385")
		},
		{
			WeaponSounds.Effects.GuidedAmmunition,
			new KeyValuePair<string, string>("shop_stats_cntrl", "Key_1384")
		},
		{
			WeaponSounds.Effects.Ricochet,
			new KeyValuePair<string, string>("shop_stats_refl", "Key_1389")
		},
		{
			WeaponSounds.Effects.SeveralMissiles,
			new KeyValuePair<string, string>("shop_stats_few", "Key_1386")
		},
		{
			WeaponSounds.Effects.Silent,
			new KeyValuePair<string, string>("shop_stats_g_slnt", "Key_1397")
		},
		{
			WeaponSounds.Effects.ForSandbox,
			new KeyValuePair<string, string>("shop_stats_sandbox", "Key_1603")
		},
		{
			WeaponSounds.Effects.SlowTheTarget,
			new KeyValuePair<string, string>("shop_stats_slow_target", "Key_1759")
		},
		{
			WeaponSounds.Effects.SemiAuto,
			new KeyValuePair<string, string>("shop_stats_semi_auto", "Key_2138")
		},
		{
			WeaponSounds.Effects.ChargingShot,
			new KeyValuePair<string, string>("shop_stats_charging_shot", "Key_2226")
		},
		{
			WeaponSounds.Effects.StickyMines,
			new KeyValuePair<string, string>("shop_stats_sticky_mines", "Key_2227")
		},
		{
			WeaponSounds.Effects.DamageAbsorbtion,
			new KeyValuePair<string, string>("shop_stats_damage_absorbtion", "Key_2228")
		},
		{
			WeaponSounds.Effects.ChainShot,
			new KeyValuePair<string, string>("shop_stats_cahin_shot", "Key_2229")
		},
		{
			WeaponSounds.Effects.AutoHoming,
			new KeyValuePair<string, string>("shop_stats_auto_homing", "Key_2230")
		},
		{
			WeaponSounds.Effects.DamageSphere,
			new KeyValuePair<string, string>("shop_stats_dsphr", "Key_2424")
		},
		{
			WeaponSounds.Effects.PoisonShots,
			new KeyValuePair<string, string>("shop_stats_psn", "Key_2795")
		},
		{
			WeaponSounds.Effects.PoisonMines,
			new KeyValuePair<string, string>("shop_stats_txc", "Key_2423")
		},
		{
			WeaponSounds.Effects.GravitationForce,
			new KeyValuePair<string, string>("shop_stats_gravitation_force", "Key_2516")
		},
		{
			WeaponSounds.Effects.Healing,
			new KeyValuePair<string, string>("shop_stats_healing", "Key_2517")
		},
		{
			WeaponSounds.Effects.Flying,
			new KeyValuePair<string, string>("shop_stats_flying", "Key_2518")
		},
		{
			WeaponSounds.Effects.Radiation,
			new KeyValuePair<string, string>("shop_stats_radiation", "Key_2519")
		},
		{
			WeaponSounds.Effects.DamageReflection,
			new KeyValuePair<string, string>("shop_stats_damage_reflection", "Key_2520")
		},
		{
			WeaponSounds.Effects.Invisibility,
			new KeyValuePair<string, string>("shop_stats_invis", "Key_2521")
		},
		{
			WeaponSounds.Effects.Resurrection,
			new KeyValuePair<string, string>("shop_stats_resurrection", "Key_2522")
		},
		{
			WeaponSounds.Effects.Burning,
			new KeyValuePair<string, string>("shop_stats_burning", "Key_2523")
		},
		{
			WeaponSounds.Effects.TimeShift,
			new KeyValuePair<string, string>("shop_stats_time_shift", "Key_2531")
		},
		{
			WeaponSounds.Effects.LifeSteal,
			new KeyValuePair<string, string>("shop_stats_lifesteal", "Key_2532")
		},
		{
			WeaponSounds.Effects.Melee,
			new KeyValuePair<string, string>("shop_stats_melee", "Key_2591")
		},
		{
			WeaponSounds.Effects.GadgetBlocker,
			new KeyValuePair<string, string>("shop_stats_lock_gadgets", "Key_2601")
		},
		{
			WeaponSounds.Effects.AreaOfEffects,
			new KeyValuePair<string, string>("shop_stats_area_of_effect", "Key_2590")
		},
		{
			WeaponSounds.Effects.Bleeding,
			new KeyValuePair<string, string>("shop_stats_bleeding", "Key_2729")
		},
		{
			WeaponSounds.Effects.CriticalDamage,
			new KeyValuePair<string, string>("shop_stats_crit", "Key_2730")
		},
		{
			WeaponSounds.Effects.DamageBoost,
			new KeyValuePair<string, string>("shop_stats_damage_boost", "Key_2781")
		},
		{
			WeaponSounds.Effects.DamageTransfer,
			new KeyValuePair<string, string>("shop_stats_damage_transfer", "Key_2782")
		},
		{
			WeaponSounds.Effects.DisableJump,
			new KeyValuePair<string, string>("shop_stats_disable_jump", "Key_2783")
		}
	};

	// Token: 0x040037DA RID: 14298
	public List<WeaponSounds.Effects> InShopEffects = new List<WeaponSounds.Effects>();

	// Token: 0x040037DB RID: 14299
	public int zoomShop;

	// Token: 0x040037DC RID: 14300
	public bool isSlowdown;

	// Token: 0x040037DD RID: 14301
	[Range(0.01f, 10f)]
	public float slowdownCoeff;

	// Token: 0x040037DE RID: 14302
	public float slowdownTime;

	// Token: 0x040037DF RID: 14303
	public GameObject[] noFillObjects;

	// Token: 0x040037E0 RID: 14304
	private GameObject BearWeapon;

	// Token: 0x040037E1 RID: 14305
	private bool bearActive;

	// Token: 0x040037E2 RID: 14306
	public WeaponSounds.TypeTracer typeTracer;

	// Token: 0x040037E3 RID: 14307
	private InnerWeaponPars _innerWeaponPars;

	// Token: 0x040037E4 RID: 14308
	private BearInnerWeaponPars _bearPars;

	// Token: 0x040037E5 RID: 14309
	public WeaponSounds.TypeDead typeDead;

	// Token: 0x040037E6 RID: 14310
	public Transform gunFlash;

	// Token: 0x040037E7 RID: 14311
	public Transform[] gunFlashDouble;

	// Token: 0x040037E8 RID: 14312
	public float lengthForShot;

	// Token: 0x040037E9 RID: 14313
	private float[] damageByTierRememberedTierWhereGet;

	// Token: 0x040037EA RID: 14314
	public float[] damageByTier = new float[]
	{
		6f,
		6f,
		6f,
		6f,
		6f,
		6f
	};

	// Token: 0x040037EB RID: 14315
	public float[] dpses = new float[]
	{
		6f,
		6f,
		6f,
		6f,
		6f,
		6f
	};

	// Token: 0x040037EC RID: 14316
	private float[] _dpsesCorrectedByRememberedGun;

	// Token: 0x040037ED RID: 14317
	private bool _dpsesCorrectedByRememberedGunInitialized;

	// Token: 0x040037EE RID: 14318
	public int tier;

	// Token: 0x040037EF RID: 14319
	public int categoryNabor = 1;

	// Token: 0x040037F0 RID: 14320
	public bool isMechWeapon;

	// Token: 0x040037F1 RID: 14321
	public bool isGrenadeWeapon;

	// Token: 0x040037F2 RID: 14322
	public float grenadeUseTime = 0.4f;

	// Token: 0x040037F3 RID: 14323
	public float grenadeThrowTime = 0.2667f;

	// Token: 0x040037F4 RID: 14324
	public int fireRateShop;

	// Token: 0x040037F5 RID: 14325
	public int[] filterMap;

	// Token: 0x040037F6 RID: 14326
	public string alternativeName = WeaponManager.PistolWN;

	// Token: 0x040037F7 RID: 14327
	public bool isBurstShooting;

	// Token: 0x040037F8 RID: 14328
	public int countShootInBurst = 3;

	// Token: 0x040037F9 RID: 14329
	public float delayInBurstShooting = 1f;

	// Token: 0x040037FA RID: 14330
	public bool isDaterWeapon;

	// Token: 0x040037FB RID: 14331
	public string daterMessage = string.Empty;

	// Token: 0x040037FC RID: 14332
	public int ammoInClip = 12;

	// Token: 0x040037FD RID: 14333
	public int InitialAmmo = 24;

	// Token: 0x040037FE RID: 14334
	public int maxAmmo = 84;

	// Token: 0x040037FF RID: 14335
	public int ammoForBonusShotMelee = 10;

	// Token: 0x04003800 RID: 14336
	public bool isMelee;

	// Token: 0x04003801 RID: 14337
	public bool isRoundMelee;

	// Token: 0x04003802 RID: 14338
	public bool isFrostSword;

	// Token: 0x04003803 RID: 14339
	public float frostRadius;

	// Token: 0x04003804 RID: 14340
	public float frostDamageMultiplier;

	// Token: 0x04003805 RID: 14341
	public float radiusRoundMelee = 5f;

	// Token: 0x04003806 RID: 14342
	public bool isLoopShoot;

	// Token: 0x04003807 RID: 14343
	public bool isCharging;

	// Token: 0x04003808 RID: 14344
	public bool chargeLoop;

	// Token: 0x04003809 RID: 14345
	public int chargeMax = 25;

	// Token: 0x0400380A RID: 14346
	public float chargeTime = 2f;

	// Token: 0x0400380B RID: 14347
	public bool invisWhenCharged;

	// Token: 0x0400380C RID: 14348
	public int criticalHitChance;

	// Token: 0x0400380D RID: 14349
	public float criticalHitCoef = 2f;

	// Token: 0x0400380E RID: 14350
	public bool isDamageHeal;

	// Token: 0x0400380F RID: 14351
	public float damageHealMultiplier = 0.1f;

	// Token: 0x04003810 RID: 14352
	public bool isPoisoning;

	// Token: 0x04003811 RID: 14353
	public float poisonDamageMultiplier = 0.1f;

	// Token: 0x04003812 RID: 14354
	public int poisonCount = 3;

	// Token: 0x04003813 RID: 14355
	public float poisonTime = 2f;

	// Token: 0x04003814 RID: 14356
	public Player_move_c.PoisonType poisonType = Player_move_c.PoisonType.Toxic;

	// Token: 0x04003815 RID: 14357
	public bool isShotGun;

	// Token: 0x04003816 RID: 14358
	public bool isDoubleShot;

	// Token: 0x04003817 RID: 14359
	public int countShots = 15;

	// Token: 0x04003818 RID: 14360
	public bool isShotMelee;

	// Token: 0x04003819 RID: 14361
	public bool isZooming;

	// Token: 0x0400381A RID: 14362
	public float fieldOfViewZomm = 75f;

	// Token: 0x0400381B RID: 14363
	public bool isMagic;

	// Token: 0x0400381C RID: 14364
	public bool flamethrower;

	// Token: 0x0400381D RID: 14365
	public bool shocker;

	// Token: 0x0400381E RID: 14366
	public bool snowStorm;

	// Token: 0x0400381F RID: 14367
	public bool bulletExplode;

	// Token: 0x04003820 RID: 14368
	public bool bazooka;

	// Token: 0x04003821 RID: 14369
	public int countInSeriaBazooka = 1;

	// Token: 0x04003822 RID: 14370
	public float stepTimeInSeriaBazooka = 0.2f;

	// Token: 0x04003823 RID: 14371
	public bool railgun;

	// Token: 0x04003824 RID: 14372
	public string railName = "Weapon77";

	// Token: 0x04003825 RID: 14373
	public bool freezer;

	// Token: 0x04003826 RID: 14374
	public int countReflectionRay = 1;

	// Token: 0x04003827 RID: 14375
	public bool grenadeLauncher;

	// Token: 0x04003828 RID: 14376
	public string bazookaExplosionName = "Weapon75";

	// Token: 0x04003829 RID: 14377
	public float bazookaExplosionRadius = 5f;

	// Token: 0x0400382A RID: 14378
	public float bazookaExplosionRadiusSelf = 2.5f;

	// Token: 0x0400382B RID: 14379
	public float bazookaImpulseRadius = 6f;

	// Token: 0x0400382C RID: 14380
	public float impulseForce = 90f;

	// Token: 0x0400382D RID: 14381
	public float impulseForceSelf = 133.4f;

	// Token: 0x0400382E RID: 14382
	public float range = 3f;

	// Token: 0x0400382F RID: 14383
	public float shockerRange = 2.5f;

	// Token: 0x04003830 RID: 14384
	public float shockerDamageMultiplier = 0.1f;

	// Token: 0x04003831 RID: 14385
	public float snowStormBonusRange = 2.5f;

	// Token: 0x04003832 RID: 14386
	public float snowStormBonusMultiplier = 0.1f;

	// Token: 0x04003833 RID: 14387
	public int damage = 50;

	// Token: 0x04003834 RID: 14388
	public float speedModifier = 1f;

	// Token: 0x04003835 RID: 14389
	public int Probability = 1;

	// Token: 0x04003836 RID: 14390
	public Vector2 damageRange = new Vector2(-15f, 15f);

	// Token: 0x04003837 RID: 14391
	public Vector3 gunPosition = new Vector3(0.35f, -0.25f, 0.6f);

	// Token: 0x04003838 RID: 14392
	public int inAppExtensionModifier = 10;

	// Token: 0x04003839 RID: 14393
	public float meleeAngle = 50f;

	// Token: 0x0400383A RID: 14394
	public float multiplayerDamage = 1f;

	// Token: 0x0400383B RID: 14395
	public float meleeAttackTimeModifier = 0.57f;

	// Token: 0x0400383C RID: 14396
	public Vector2 startZone;

	// Token: 0x0400383D RID: 14397
	public float tekKoof = 1f;

	// Token: 0x0400383E RID: 14398
	public float upKoofFire = 0.5f;

	// Token: 0x0400383F RID: 14399
	public float maxKoof = 4f;

	// Token: 0x04003840 RID: 14400
	public float downKoofFirst = 0.2f;

	// Token: 0x04003841 RID: 14401
	public float downKoof = 0.2f;

	// Token: 0x04003842 RID: 14402
	public bool campaignOnly;

	// Token: 0x04003843 RID: 14403
	public int rocketNum;

	// Token: 0x04003844 RID: 14404
	public int scopeNum;

	// Token: 0x04003845 RID: 14405
	public float scaleShop = 150f;

	// Token: 0x04003846 RID: 14406
	public Vector3 positionShop;

	// Token: 0x04003847 RID: 14407
	public Vector3 rotationShop;

	// Token: 0x04003848 RID: 14408
	public WeaponSounds.SpecialEffects specialEffect = WeaponSounds.SpecialEffects.None;

	// Token: 0x04003849 RID: 14409
	public float protectionEffectValue = 1f;

	// Token: 0x0400384A RID: 14410
	public string localizeWeaponKey;

	// Token: 0x0400384B RID: 14411
	private float animLength;

	// Token: 0x0400384C RID: 14412
	private float timeFromFire = 1000f;

	// Token: 0x0400384D RID: 14413
	private Player_move_c myPlayerC;

	// Token: 0x0400384E RID: 14414
	public bool DPSRememberWhenGet;

	// Token: 0x02000810 RID: 2064
	public enum Effects
	{
		// Token: 0x04003850 RID: 14416
		Automatic,
		// Token: 0x04003851 RID: 14417
		SingleShot,
		// Token: 0x04003852 RID: 14418
		Rockets,
		// Token: 0x04003853 RID: 14419
		Mortar,
		// Token: 0x04003854 RID: 14420
		Laser,
		// Token: 0x04003855 RID: 14421
		Shotgun,
		// Token: 0x04003856 RID: 14422
		Chainsaw,
		// Token: 0x04003857 RID: 14423
		Flamethrower,
		// Token: 0x04003858 RID: 14424
		ElectroThrower,
		// Token: 0x04003859 RID: 14425
		WallBreak,
		// Token: 0x0400385A RID: 14426
		AreaDamage,
		// Token: 0x0400385B RID: 14427
		Zoom,
		// Token: 0x0400385C RID: 14428
		ThroughEnemies,
		// Token: 0x0400385D RID: 14429
		Detonation,
		// Token: 0x0400385E RID: 14430
		GuidedAmmunition,
		// Token: 0x0400385F RID: 14431
		Ricochet,
		// Token: 0x04003860 RID: 14432
		SeveralMissiles,
		// Token: 0x04003861 RID: 14433
		Silent,
		// Token: 0x04003862 RID: 14434
		ForSandbox,
		// Token: 0x04003863 RID: 14435
		SlowTheTarget,
		// Token: 0x04003864 RID: 14436
		SemiAuto,
		// Token: 0x04003865 RID: 14437
		ChargingShot,
		// Token: 0x04003866 RID: 14438
		StickyMines,
		// Token: 0x04003867 RID: 14439
		DamageAbsorbtion,
		// Token: 0x04003868 RID: 14440
		ChainShot,
		// Token: 0x04003869 RID: 14441
		AutoHoming,
		// Token: 0x0400386A RID: 14442
		DamageSphere,
		// Token: 0x0400386B RID: 14443
		PoisonShots,
		// Token: 0x0400386C RID: 14444
		PoisonMines,
		// Token: 0x0400386D RID: 14445
		GravitationForce,
		// Token: 0x0400386E RID: 14446
		Healing,
		// Token: 0x0400386F RID: 14447
		Flying,
		// Token: 0x04003870 RID: 14448
		Radiation,
		// Token: 0x04003871 RID: 14449
		DamageReflection,
		// Token: 0x04003872 RID: 14450
		Invisibility,
		// Token: 0x04003873 RID: 14451
		Resurrection,
		// Token: 0x04003874 RID: 14452
		Burning,
		// Token: 0x04003875 RID: 14453
		TimeShift,
		// Token: 0x04003876 RID: 14454
		LifeSteal,
		// Token: 0x04003877 RID: 14455
		Melee,
		// Token: 0x04003878 RID: 14456
		GadgetBlocker,
		// Token: 0x04003879 RID: 14457
		AreaOfEffects,
		// Token: 0x0400387A RID: 14458
		Bleeding,
		// Token: 0x0400387B RID: 14459
		CriticalDamage,
		// Token: 0x0400387C RID: 14460
		DamageBoost,
		// Token: 0x0400387D RID: 14461
		DamageTransfer,
		// Token: 0x0400387E RID: 14462
		DisableJump
	}

	// Token: 0x02000811 RID: 2065
	private sealed class EffectComparer : IEqualityComparer<WeaponSounds.Effects>
	{
		// Token: 0x06004B6D RID: 19309 RVA: 0x001AF268 File Offset: 0x001AD468
		public bool Equals(WeaponSounds.Effects x, WeaponSounds.Effects y)
		{
			return x == y;
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x001AF270 File Offset: 0x001AD470
		public int GetHashCode(WeaponSounds.Effects obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000812 RID: 2066
	public enum TypeTracer
	{
		// Token: 0x04003880 RID: 14464
		none = -1,
		// Token: 0x04003881 RID: 14465
		standart,
		// Token: 0x04003882 RID: 14466
		red,
		// Token: 0x04003883 RID: 14467
		for252,
		// Token: 0x04003884 RID: 14468
		turquoise,
		// Token: 0x04003885 RID: 14469
		green,
		// Token: 0x04003886 RID: 14470
		violet
	}

	// Token: 0x02000813 RID: 2067
	public enum TypeDead
	{
		// Token: 0x04003888 RID: 14472
		angel,
		// Token: 0x04003889 RID: 14473
		explosion,
		// Token: 0x0400388A RID: 14474
		energyBlue,
		// Token: 0x0400388B RID: 14475
		energyRed,
		// Token: 0x0400388C RID: 14476
		energyPink,
		// Token: 0x0400388D RID: 14477
		energyCyan,
		// Token: 0x0400388E RID: 14478
		energyLight,
		// Token: 0x0400388F RID: 14479
		energyGreen,
		// Token: 0x04003890 RID: 14480
		energyOrange,
		// Token: 0x04003891 RID: 14481
		energyWhite,
		// Token: 0x04003892 RID: 14482
		like
	}

	// Token: 0x02000814 RID: 2068
	public enum SpecialEffects
	{
		// Token: 0x04003894 RID: 14484
		None = -1,
		// Token: 0x04003895 RID: 14485
		PlayerShield
	}
}
