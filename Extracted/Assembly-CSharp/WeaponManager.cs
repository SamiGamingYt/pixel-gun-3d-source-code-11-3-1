using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public sealed class WeaponManager : MonoBehaviour
{
	// Token: 0x060043C5 RID: 17349 RVA: 0x0016AFB8 File Offset: 0x001691B8
	public WeaponManager()
	{
		this.tryGunPromos = new Dictionary<string, long>();
		this.tryGunDiscounts = new Dictionary<string, SaltedLong>();
		this.TryGuns = new Dictionary<string, Dictionary<string, object>>();
		this.ExpiredTryGuns = new List<string>();
		base..ctor();
	}

	// Token: 0x060043C6 RID: 17350 RVA: 0x0016B0C0 File Offset: 0x001692C0
	static WeaponManager()
	{
		WeaponManager.WeaponEquipped = null;
		WeaponManager.WeaponEquipped_AllCases = null;
		WeaponManager._buffsPAramsInitialized = false;
		WeaponManager._defaultTryGunsTable = new Dictionary<ShopNGUIController.CategoryNames, List<List<string>>>
		{
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				new List<List<string>>
				{
					new List<string>
					{
						"Weapon127",
						"Weapon142",
						"Weapon206",
						"Weapon167"
					},
					new List<string>
					{
						"Weapon163",
						"Weapon141"
					},
					new List<string>
					{
						"Weapon84"
					},
					new List<string>(),
					new List<string>(),
					new List<string>
					{
						"Weapon220"
					}
				}
			},
			{
				ShopNGUIController.CategoryNames.BackupCategory,
				new List<List<string>>
				{
					new List<string>
					{
						"Weapon160",
						"Weapon203"
					},
					new List<string>(),
					new List<string>(),
					new List<string>(),
					new List<string>
					{
						"Weapon308"
					},
					new List<string>
					{
						"Weapon223"
					}
				}
			},
			{
				ShopNGUIController.CategoryNames.MeleeCategory,
				new List<List<string>>
				{
					new List<string>(),
					new List<string>(),
					new List<string>(),
					new List<string>(),
					new List<string>(),
					new List<string>()
				}
			},
			{
				ShopNGUIController.CategoryNames.SpecilCategory,
				new List<List<string>>
				{
					new List<string>
					{
						"Weapon178"
					},
					new List<string>
					{
						"Weapon105"
					},
					new List<string>(),
					new List<string>(),
					new List<string>
					{
						"Weapon306"
					},
					new List<string>()
				}
			},
			{
				ShopNGUIController.CategoryNames.SniperCategory,
				new List<List<string>>
				{
					new List<string>
					{
						"Weapon77",
						"Weapon209"
					},
					new List<string>
					{
						"Weapon339"
					},
					new List<string>(),
					new List<string>
					{
						"Weapon251"
					},
					new List<string>(),
					new List<string>
					{
						"Weapon221"
					}
				}
			},
			{
				ShopNGUIController.CategoryNames.PremiumCategory,
				new List<List<string>>
				{
					new List<string>
					{
						"Weapon82",
						"Weapon212"
					},
					new List<string>
					{
						"Weapon180"
					},
					new List<string>
					{
						"Weapon133",
						"Weapon253",
						"Weapon99"
					},
					new List<string>(),
					new List<string>
					{
						"Weapon161"
					},
					new List<string>()
				}
			}
		};
		ItemDb.Fill_tagToStoreIDMapping(WeaponManager.tagToStoreIDMapping);
		ItemDb.Fill_storeIDtoDefsSNMapping(WeaponManager.storeIDtoDefsSNMapping);
		WeaponManager._purchasableWeaponSet.UnionWith(WeaponManager.storeIDtoDefsSNMapping.Values);
	}

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x060043C7 RID: 17351 RVA: 0x0016BAE8 File Offset: 0x00169CE8
	// (remove) Token: 0x060043C8 RID: 17352 RVA: 0x0016BB00 File Offset: 0x00169D00
	public static event Action TryGunRemoved;

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x060043C9 RID: 17353 RVA: 0x0016BB18 File Offset: 0x00169D18
	// (remove) Token: 0x060043CA RID: 17354 RVA: 0x0016BB30 File Offset: 0x00169D30
	public static event Action TryGunExpired;

	// Token: 0x1400009E RID: 158
	// (add) Token: 0x060043CB RID: 17355 RVA: 0x0016BB48 File Offset: 0x00169D48
	// (remove) Token: 0x060043CC RID: 17356 RVA: 0x0016BB60 File Offset: 0x00169D60
	public static event Action<WeaponSounds> WeaponEquipped;

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x060043CD RID: 17357 RVA: 0x0016BB78 File Offset: 0x00169D78
	// (remove) Token: 0x060043CE RID: 17358 RVA: 0x0016BB90 File Offset: 0x00169D90
	public static event Action<WeaponSounds> WeaponEquipped_AllCases;

	// Token: 0x17000B31 RID: 2865
	// (get) Token: 0x060043CF RID: 17359 RVA: 0x0016BBA8 File Offset: 0x00169DA8
	public Dictionary<string, long> TryGunPromos
	{
		get
		{
			return this.tryGunPromos;
		}
	}

	// Token: 0x060043D0 RID: 17360 RVA: 0x0016BBB0 File Offset: 0x00169DB0
	public long DiscountForTryGun(string tg)
	{
		if (tg == null)
		{
			return 0L;
		}
		if (this.tryGunDiscounts == null || !this.tryGunDiscounts.ContainsKey(tg))
		{
			return (long)WeaponManager.BaseTryGunDiscount();
		}
		return this.tryGunDiscounts[tg].Value;
	}

	// Token: 0x060043D1 RID: 17361 RVA: 0x0016BC00 File Offset: 0x00169E00
	public void AddTryGunPromo(string tg)
	{
		if (tg == null)
		{
			Debug.LogError("AddTryGunPromo tg == null");
			return;
		}
		this.tryGunPromos.Add(tg, PromoActionsManager.CurrentUnixTime);
		int num = WeaponManager.BaseTryGunDiscount();
		try
		{
			ItemRecord byTag = ItemDb.GetByTag(tg);
			string currency = byTag.Price.Currency;
			int @int = Storager.getInt(currency, false);
			int num2 = ShopNGUIController.PriceIfGunWillBeTryGun(tg);
			bool flag = currency == "GemsCurrency";
			IList<PurchaseEventArgs> list;
			if (flag)
			{
				IList<PurchaseEventArgs> gemsPurchasesInfo = AbstractBankView.gemsPurchasesInfo;
				list = gemsPurchasesInfo;
			}
			else
			{
				list = AbstractBankView.goldPurchasesInfo;
			}
			int index = list[0].Index;
			int num3 = (!flag) ? VirtualCurrencyHelper.GetCoinInappsQuantity(index) : VirtualCurrencyHelper.GetGemsInappsQuantity(index);
			if (num2 > @int + num3)
			{
				int num4 = @int + num3 - 1;
				ItemPrice itemPrice = ShopNGUIController.GetItemPrice(tg, (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(tg), false, false, false);
				num = (int)((1f - (float)num4 / (float)itemPrice.Price) * 100f) + 1;
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in AddTryGunPromo: " + arg);
		}
		num = Mathf.Min(70, num);
		this.tryGunDiscounts.Add(tg, new SaltedLong(685488L, (long)num));
	}

	// Token: 0x060043D2 RID: 17362 RVA: 0x0016BD44 File Offset: 0x00169F44
	public static int BaseTryGunDiscount()
	{
		int num = (!ABTestController.useBuffSystem) ? 50 : 50;
		try
		{
			num = ((!ABTestController.useBuffSystem) ? KillRateCheck.instance.discountValue : BuffSystem.instance.discountValue);
			num = Math.Max(0, num);
			num = Math.Min(75, num);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in getting KillRateCheck.instance.discountValue: " + arg);
		}
		return num;
	}

	// Token: 0x060043D3 RID: 17363 RVA: 0x0016BDD4 File Offset: 0x00169FD4
	public void AddTryGun(string tg)
	{
		try
		{
			int value = 3;
			try
			{
				value = ((!ABTestController.useBuffSystem) ? KillRateCheck.instance.GetRoundsForGun() : BuffSystem.instance.GetRoundsForGun());
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in numOfMatches = KillRateCheck.instance.GetRoundsForGun(): " + arg);
			}
			this.TryGuns.Add(tg, new Dictionary<string, object>
			{
				{
					"NumberOfMatchesKey",
					new SaltedInt(52394, value)
				}
			});
			Weapon weapon = this.AddWeaponWithTagToAllAvailable(tg);
			WeaponSounds weaponWS = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			string text = null;
			try
			{
				text = ItemDb.GetByPrefabName(this.playerWeapons.OfType<Weapon>().FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == weaponWS.categoryNabor).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
			}
			catch (Exception arg2)
			{
				Debug.LogWarning("Exception in try guns get equipped before: " + arg2);
			}
			this.TryGuns[tg].Add("EquippedBeforeKey", text ?? string.Empty);
			this.EquipWeapon(weapon, true, false);
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
		}
		catch (Exception arg3)
		{
			Debug.LogError("Exception in AddTryGun: " + arg3);
		}
	}

	// Token: 0x060043D4 RID: 17364 RVA: 0x0016BF80 File Offset: 0x0016A180
	public void DecreaseTryGunsMatchesCount()
	{
		if (Defs.isHunger)
		{
			return;
		}
		try
		{
			List<string> list = new List<string>();
			KeyValuePair<string, Dictionary<string, object>> tryGunKvp;
			foreach (KeyValuePair<string, Dictionary<string, object>> tryGunKvp2 in this.TryGuns)
			{
				tryGunKvp = tryGunKvp2;
				if (!(this.weaponsInGame.FirstOrDefault((UnityEngine.Object w) => ItemDb.GetByPrefabName(w.name).Tag == tryGunKvp.Key) == null))
				{
					int num = Math.Max(0, ((SaltedInt)tryGunKvp.Value["NumberOfMatchesKey"]).Value - 1);
					tryGunKvp.Value["NumberOfMatchesKey"] = new SaltedInt(838318, num);
					if (num == 0)
					{
						list.Add(tryGunKvp.Key);
					}
				}
			}
			foreach (string tryGunTag in list)
			{
				this.RemoveTryGun(tryGunTag);
			}
			if (list.Count > 0)
			{
				Action tryGunRemoved = WeaponManager.TryGunRemoved;
				if (tryGunRemoved != null)
				{
					tryGunRemoved();
				}
				if (ABTestController.useBuffSystem)
				{
					BuffSystem.instance.OnGunTakeOff();
				}
				else
				{
					KillRateCheck.OnGunTakeOff();
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in DecreaseTryGunsMatchesCount: " + arg);
		}
	}

	// Token: 0x060043D5 RID: 17365 RVA: 0x0016C158 File Offset: 0x0016A358
	public bool IsAvailableTryGun(string tryGunTag)
	{
		bool result;
		try
		{
			result = (tryGunTag != null && this.TryGuns != null && this.TryGuns.Keys.Contains(tryGunTag) && this.TryGuns[tryGunTag].ContainsKey("NumberOfMatchesKey") && ((SaltedInt)this.TryGuns[tryGunTag]["NumberOfMatchesKey"]).Value > 0);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in IsAvailableTryGun: " + arg);
			result = false;
		}
		return result;
	}

	// Token: 0x060043D6 RID: 17366 RVA: 0x0016C218 File Offset: 0x0016A418
	public void RemoveTryGun(string tryGunTag)
	{
		if (this.TryGuns == null || !this.TryGuns.ContainsKey(tryGunTag))
		{
			return;
		}
		try
		{
			Dictionary<string, object> dict = this.TryGuns[tryGunTag];
			string text;
			if (dict.TryGetValue("EquippedBeforeKey", out text))
			{
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						string lastBoughtTag = WeaponManager.LastBoughtTag(text, null);
						Weapon weapon = this.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault((Weapon w) => ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag == lastBoughtTag);
						if (weapon != null)
						{
							this.EquipWeapon(weapon, true, false);
						}
						else
						{
							int cat = ItemDb.GetItemCategory(lastBoughtTag);
							Weapon weapon2 = this.allAvailablePlayerWeapons.OfType<Weapon>().FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == cat && ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag != tryGunTag && !this.IsAvailableTryGun(tryGunTag));
							if (weapon2 != null)
							{
								this.EquipWeapon(weapon2, true, false);
							}
							else
							{
								this.SaveWeaponSet(Defs.CampaignWSSN, string.Empty, cat);
								int num = -1;
								for (int i = 0; i < this.playerWeapons.Count; i++)
								{
									if (this.playerWeapons[i] != null && ItemDb.GetByPrefabName(((Weapon)this.playerWeapons[i]).weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag == tryGunTag)
									{
										num = i;
										break;
									}
								}
								if (num != -1)
								{
									this.playerWeapons.RemoveAt(num);
								}
								else
								{
									Debug.LogError("RemoveTryGun: error removing weapon from playerWeapons");
								}
								this.SetWeaponsSet(0);
								if (cat == 4)
								{
									this.SaveWeaponSet("WeaponManager.SniperModeWSSN", WeaponManager.CampaignRifle_WN, cat);
								}
								if (cat == 2)
								{
									this.SaveWeaponSet("WeaponManager.KnifesModeWSSN", WeaponManager.KnifeWN, cat);
								}
								this.SaveWeaponSet(Defs.MultiplayerWSSN, WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet().Split(new char[]
								{
									"#"[0]
								})[cat], cat);
							}
						}
					}
					catch (Exception arg)
					{
						Debug.LogError("tryGun.TryGetValue(EquippedBeforeKey, out gunBefore) exception: " + arg);
					}
				}
			}
			else
			{
				Debug.LogError("RemoveTryGun: No EquippedBeforeKey for " + tryGunTag);
			}
			this.allAvailablePlayerWeapons = new ArrayList((from w in this.allAvailablePlayerWeapons.OfType<Weapon>()
			where ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag != tryGunTag
			select w).ToList<Weapon>());
			this.TryGuns.Remove(tryGunTag);
			if (!this.ExpiredTryGuns.Contains(tryGunTag))
			{
				this.ExpiredTryGuns.Add(tryGunTag);
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.ReloadGridOrCarousel(ShopNGUIController.sharedShop.CurrentItem);
				ShopNGUIController.sharedShop.UpdateIcons(false);
			}
		}
		catch (Exception arg2)
		{
			Debug.LogError("Exception in RemoveTryGun: " + arg2);
		}
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x0016C5AC File Offset: 0x0016A7AC
	private void SaveTryGunsDiscounts()
	{
		try
		{
			Storager.setString("WeaponManager.TryGunsDiscountsKey", Rilisoft.MiniJson.Json.Serialize(this.tryGunPromos), false);
			Storager.setString("WeaponManager.TryGunsDiscountsValuesKey", Rilisoft.MiniJson.Json.Serialize(this.tryGunDiscounts.ToDictionary((KeyValuePair<string, SaltedLong> kvp) => kvp.Key, (KeyValuePair<string, SaltedLong> kvp) => kvp.Value.Value)), false);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in SaveTryGunsDiscounts: " + arg);
		}
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x0016C65C File Offset: 0x0016A85C
	private void SaveTryGunsInfo()
	{
		try
		{
			Dictionary<string, Dictionary<string, object>> value = (from kvp in this.TryGuns
			select new KeyValuePair<string, Dictionary<string, object>>(kvp.Key, new Dictionary<string, object>
			{
				{
					"NumberOfMatchesKey",
					((SaltedInt)kvp.Value["NumberOfMatchesKey"]).Value
				},
				{
					"EquippedBeforeKey",
					kvp.Value["EquippedBeforeKey"]
				}
			})).ToDictionary((KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Key, (KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Value);
			Dictionary<string, object> obj = new Dictionary<string, object>
			{
				{
					"TryGunsDictionaryKey",
					value
				},
				{
					"ExpiredTryGunsListKey",
					this.ExpiredTryGuns
				}
			};
			Storager.setString("WeaponManager.TryGunsKey", Rilisoft.MiniJson.Json.Serialize(obj), false);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in SaveTryGunsInfo: " + arg);
		}
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x0016C740 File Offset: 0x0016A940
	private void LoadTryGunDiscounts()
	{
		try
		{
			if (!Storager.hasKey("WeaponManager.TryGunsDiscountsKey"))
			{
				Storager.setString("WeaponManager.TryGunsDiscountsKey", "{}", false);
			}
			Dictionary<string, object> dictionary = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("WeaponManager.TryGunsDiscountsKey", false)) as Dictionary<string, object>;
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				this.tryGunPromos.Add(keyValuePair.Key, (long)keyValuePair.Value);
			}
			if (!Storager.hasKey("WeaponManager.TryGunsDiscountsValuesKey"))
			{
				Storager.setString("WeaponManager.TryGunsDiscountsValuesKey", "{}", false);
			}
			Dictionary<string, object> dictionary2 = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("WeaponManager.TryGunsDiscountsValuesKey", false)) as Dictionary<string, object>;
			foreach (KeyValuePair<string, object> keyValuePair2 in dictionary2)
			{
				this.tryGunDiscounts.Add(keyValuePair2.Key, new SaltedLong(17425L, (long)((int)((long)keyValuePair2.Value))));
			}
			this.RemoveExpiredPromosForTryGuns();
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in LoadTryGunDiscounts: " + arg);
		}
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x0016C8D4 File Offset: 0x0016AAD4
	private void LoadTryGunsInfo()
	{
		try
		{
			if (!Storager.hasKey("WeaponManager.TryGunsKey"))
			{
				Storager.setString("WeaponManager.TryGunsKey", "{}", false);
			}
			Dictionary<string, object> dictionary = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("WeaponManager.TryGunsKey", false)) as Dictionary<string, object>;
			object obj;
			if (dictionary.TryGetValue("TryGunsDictionaryKey", out obj))
			{
				this.TryGuns = (obj as Dictionary<string, object>).Select(delegate(KeyValuePair<string, object> kvp)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2.Add("NumberOfMatchesKey", new SaltedInt(52394, (int)((long)(kvp.Value as Dictionary<string, object>)["NumberOfMatchesKey"])));
					dictionary2.Add("EquippedBeforeKey", (kvp.Value as Dictionary<string, object>)["EquippedBeforeKey"]);
					return new KeyValuePair<string, Dictionary<string, object>>(kvp.Key, dictionary2);
				}).ToDictionary((KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Key, (KeyValuePair<string, Dictionary<string, object>> kvp) => kvp.Value);
			}
			if (dictionary.ContainsKey("ExpiredTryGunsListKey"))
			{
				this.ExpiredTryGuns = (dictionary["ExpiredTryGunsListKey"] as List<object>).OfType<string>().ToList<string>();
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in LoadTryGunsInfo: " + arg);
		}
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x0016C9FC File Offset: 0x0016ABFC
	public void RemoveDiscountForTryGun(string tg)
	{
		this.tryGunPromos.Remove(tg);
		this.tryGunDiscounts.Remove(tg);
	}

	// Token: 0x17000B32 RID: 2866
	// (get) Token: 0x060043DC RID: 17372 RVA: 0x0016CA18 File Offset: 0x0016AC18
	public bool AnyDiscountForTryGuns
	{
		get
		{
			return this.tryGunPromos != null && this.tryGunPromos.Count > 0;
		}
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x0016CA38 File Offset: 0x0016AC38
	public bool IsWeaponDiscountedAsTryGun(string tg)
	{
		return this.tryGunPromos != null && this.tryGunPromos.ContainsKey(tg);
	}

	// Token: 0x060043DE RID: 17374 RVA: 0x0016CA54 File Offset: 0x0016AC54
	public long StartTimeForTryGunDiscount(string tg)
	{
		if (tg != null && this.tryGunPromos != null && this.tryGunPromos.ContainsKey(tg))
		{
			return this.tryGunPromos[tg];
		}
		return 0L;
	}

	// Token: 0x060043DF RID: 17375 RVA: 0x0016CA88 File Offset: 0x0016AC88
	public static float TryGunPromoDuration()
	{
		float num = (float)((!ABTestController.useBuffSystem) ? 3600 : 3600);
		try
		{
			num = ((!ABTestController.useBuffSystem) ? KillRateCheck.instance.timeForDiscount : BuffSystem.instance.timeForDiscount);
			num = Math.Max(60f, num);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in duration = KillRateCheck.instance.timeForDiscount: " + arg);
		}
		return num;
	}

	// Token: 0x060043E0 RID: 17376 RVA: 0x0016CB18 File Offset: 0x0016AD18
	public void RemoveExpiredPromosForTryGuns()
	{
		try
		{
			float duration = WeaponManager.TryGunPromoDuration();
			List<KeyValuePair<string, long>> list = (from kvp in this.tryGunPromos
			where (float)(PromoActionsManager.CurrentUnixTime - kvp.Value) >= duration
			select kvp).ToList<KeyValuePair<string, long>>();
			foreach (KeyValuePair<string, long> keyValuePair in list)
			{
				this.RemoveDiscountForTryGun(keyValuePair.Key);
			}
			if (list.Count<KeyValuePair<string, long>>() > 0)
			{
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateButtons();
				}
				Action tryGunExpired = WeaponManager.TryGunExpired;
				if (tryGunExpired != null)
				{
					tryGunExpired();
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in RemoveExpiredPromosForTryGuns: " + arg);
		}
	}

	// Token: 0x060043E1 RID: 17377 RVA: 0x0016CC28 File Offset: 0x0016AE28
	private IEnumerator Step()
	{
		for (;;)
		{
			yield return base.StartCoroutine(CoroutineRunner.WaitForSeconds(1f));
			this.RemoveExpiredPromosForTryGuns();
		}
		yield break;
	}

	// Token: 0x17000B33 RID: 2867
	// (get) Token: 0x060043E2 RID: 17378 RVA: 0x0016CC44 File Offset: 0x0016AE44
	public static Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> tryGunsTable
	{
		get
		{
			Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> dictionary = null;
			try
			{
				if (!WeaponManager._buffsPAramsInitialized && !Storager.hasKey("BuffsParam"))
				{
					Storager.setString("BuffsParam", "{}", false);
				}
				WeaponManager._buffsPAramsInitialized = true;
				Dictionary<string, object> dictionary2 = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("BuffsParam", false)) as Dictionary<string, object>;
				if (dictionary2 != null && dictionary2.ContainsKey("TryGuns"))
				{
					Dictionary<string, object> source = dictionary2["TryGuns"] as Dictionary<string, object>;
					dictionary = source.ToDictionary((KeyValuePair<string, object> kvp) => (ShopNGUIController.CategoryNames)((int)Enum.Parse(typeof(ShopNGUIController.CategoryNames), kvp.Key, true)), (KeyValuePair<string, object> kvp) => (from listObject in (kvp.Value as List<object>).OfType<List<object>>()
					select listObject.OfType<string>().ToList<string>()).ToList<List<string>>());
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in reading try guns table from storager: " + arg);
			}
			return (dictionary == null) ? WeaponManager._defaultTryGunsTable : dictionary;
		}
	}

	// Token: 0x060043E3 RID: 17379 RVA: 0x0016CD48 File Offset: 0x0016AF48
	public static Dictionary<ShopNGUIController.CategoryNames, List<string>> GetWeaponTagsByCategoriesFromItems(List<string> items)
	{
		try
		{
			return (from weaponTag in items.Intersect(ItemDb.RecordsByTag.Keys)
			group weaponTag by (ShopNGUIController.CategoryNames)(ItemDb.GetWeaponInfo(weaponTag).categoryNabor - 1)).ToDictionary((IGrouping<ShopNGUIController.CategoryNames, string> grouping) => grouping.Key, (IGrouping<ShopNGUIController.CategoryNames, string> grouping) => grouping.ToList<string>());
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GetWeaponTagsFromItems: {0}", new object[]
			{
				ex
			});
		}
		return new Dictionary<ShopNGUIController.CategoryNames, List<string>>();
	}

	// Token: 0x060043E4 RID: 17380 RVA: 0x0016CE10 File Offset: 0x0016B010
	public static List<string> GetNewWeaponsForTier(int tier)
	{
		try
		{
			IEnumerable<ItemRecord> canBuyWeapon = ItemDb.GetCanBuyWeapon(false);
			IEnumerable<ItemRecord> source = from record in canBuyWeapon
			where ItemDb.GetWeaponInfo(record.Tag).tier == tier
			select record;
			return (from record in source.Where(delegate(ItemRecord record)
			{
				List<string> list = WeaponUpgrades.ChainForTag(record.Tag);
				return list == null || (list.Count > 0 && list[0] == record.Tag);
			})
			select record.Tag).ToList<string>();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GetNewWeaponsForTier: {0}", new object[]
			{
				ex
			});
		}
		return new List<string>();
	}

	// Token: 0x060043E5 RID: 17381 RVA: 0x0016CEE0 File Offset: 0x0016B0E0
	public void UnloadAll()
	{
		this._rocketCache = null;
		this._turretCache = null;
		this._playerWeaponsSetInnerPrefabsCache.Clear();
		this._turretWeaponCache = null;
		this._playerWeapons.Clear();
		this._allAvailablePlayerWeapons.Clear();
		this._weaponsInGame = null;
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060043E6 RID: 17382 RVA: 0x0016CF30 File Offset: 0x0016B130
	public static bool IsExclusiveWeapon(string weaponTag)
	{
		return WeaponManager.GotchaGuns.Contains(weaponTag) || weaponTag == WeaponManager.SocialGunWN;
	}

	// Token: 0x060043E7 RID: 17383 RVA: 0x0016CF50 File Offset: 0x0016B150
	public static void ProvideExclusiveWeaponByTag(string weaponTag)
	{
		if (string.IsNullOrEmpty(weaponTag))
		{
			Debug.LogError("Error in ProvideExclusiveWeaponByTag: string.IsNullOrEmpty(weaponTag)");
			return;
		}
		if (Storager.getInt(weaponTag, true) > 0)
		{
			Debug.LogError("Error in ProvideExclusiveWeaponByTag: Storager.getInt (weaponTag, true) > 0");
			return;
		}
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null)
		{
			Debug.LogError("Error in ProvideExclusiveWeaponByTag: weaponRecord == null");
			return;
		}
		if (byTag.PrefabName == null)
		{
			Debug.LogError("Error in ProvideExclusiveWeaponByTag: weaponRecord.PrefabName == null");
			return;
		}
		Storager.setInt(weaponTag, 1, true);
		WeaponManager.AddExclusiveWeaponToWeaponStructures(byTag.PrefabName);
	}

	// Token: 0x060043E8 RID: 17384 RVA: 0x0016CFCC File Offset: 0x0016B1CC
	public static void RefreshExpControllers()
	{
		if (ExperienceController.sharedController != null)
		{
			ExperienceController.sharedController.Refresh();
		}
		else
		{
			Debug.LogError("RefreshLevelAndSetRememberedTiersFromCloud ExperienceController.sharedController == null");
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.Refresh();
		}
		else
		{
			Debug.LogError("RefreshLevelAndSetRememberedTiersFromCloud ExpController.Instance == null");
		}
	}

	// Token: 0x060043E9 RID: 17385 RVA: 0x0016D02C File Offset: 0x0016B22C
	public static void RefreshLevelAndSetRememberedTiersFromCloud(List<string> weaponsForWhichSetRememberedTier)
	{
		try
		{
			WeaponManager.SetRememberedTiersForWeaponsComesFromCloud(weaponsForWhichSetRememberedTier);
		}
		catch (Exception arg)
		{
			Debug.LogError("RefreshLevelAndSetRememberedTiersFromCloud exception: " + arg);
		}
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x0016D078 File Offset: 0x0016B278
	public static void SetRememberedTiersForWeaponsComesFromCloud(List<string> weaponsForWhichSetRememberedTier)
	{
		try
		{
			foreach (string tag in weaponsForWhichSetRememberedTier)
			{
				ItemRecord byTag = ItemDb.GetByTag(tag);
				if (byTag != null)
				{
					string prefabName = byTag.PrefabName;
					if (prefabName != null)
					{
						WeaponManager.SetRememberedTierForWeapon(prefabName);
					}
					else
					{
						Debug.LogError("SetRememberedTiersForWeaponsComesFromCloud prefabName == null");
					}
				}
				else
				{
					Debug.LogWarning("SetRememberedTiersForWeaponsComesFromCloud record == null");
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("SetRememberedTiersForWeaponsComesFromCloud exception: " + arg);
		}
	}

	// Token: 0x060043EB RID: 17387 RVA: 0x0016D148 File Offset: 0x0016B348
	public static void SetRememberedTierForWeapon(string prefabName)
	{
		Storager.setInt("RememberedTierWhenObtainGun_" + prefabName, ExpController.OurTierForAnyPlace(), false);
	}

	// Token: 0x060043EC RID: 17388 RVA: 0x0016D160 File Offset: 0x0016B360
	public static void AddExclusiveWeaponToWeaponStructures(string prefabName)
	{
		if (string.IsNullOrEmpty(prefabName))
		{
			Debug.LogError("Error in AddExclusiveWeaponToWeaponStructures: string.IsNullOrEmpty(prefabName)");
			return;
		}
		WeaponManager.SetRememberedTierForWeapon(prefabName);
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.Initialized)
		{
			GameObject gameObject = null;
			try
			{
				gameObject = WeaponManager.sharedManager.weaponsInGame.OfType<GameObject>().FirstOrDefault((GameObject w) => w.name.Equals(prefabName));
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in AddExclusiveWeaponToWeaponStructures: " + arg);
			}
			if (gameObject != null)
			{
				int num;
				WeaponManager.sharedManager.AddWeapon(gameObject, out num);
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.UpdateIcons(false);
			}
		}
	}

	// Token: 0x17000B34 RID: 2868
	// (get) Token: 0x060043ED RID: 17389 RVA: 0x0016D258 File Offset: 0x0016B458
	public bool ResetLockSet
	{
		get
		{
			return this._resetLock;
		}
	}

	// Token: 0x060043EE RID: 17390 RVA: 0x0016D260 File Offset: 0x0016B460
	public static GameObject AddRay(Vector3 pos, Vector3 forw, string nm, float len = 150f)
	{
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(ResPath.Combine("Rays", nm));
		if (objectFromName == null)
		{
			return null;
		}
		Transform transform = objectFromName.transform;
		Transform transform2 = (transform.childCount <= 0) ? null : transform.GetChild(0);
		if (transform2 != null)
		{
			transform2.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, len));
		}
		objectFromName.transform.position = pos;
		objectFromName.transform.forward = forw;
		return objectFromName;
	}

	// Token: 0x060043EF RID: 17391 RVA: 0x0016D2F4 File Offset: 0x0016B4F4
	public static void SetGunFlashActive(GameObject gunFlash, bool _a)
	{
		if (gunFlash == null)
		{
			return;
		}
		Transform transform = null;
		if (gunFlash.transform.childCount > 0)
		{
			transform = gunFlash.transform.GetChild(0);
		}
		if (transform != null && transform.gameObject.activeSelf != _a)
		{
			transform.gameObject.SetActive(_a);
		}
	}

	// Token: 0x060043F0 RID: 17392 RVA: 0x0016D358 File Offset: 0x0016B558
	public static void ClearCachedInnerPrefabs()
	{
		WeaponManager.cachedInnerPrefabsForCurrentShopCategory.Clear();
	}

	// Token: 0x060043F1 RID: 17393 RVA: 0x0016D364 File Offset: 0x0016B564
	public static GameObject InnerPrefabForWeaponBuffered(GameObject weapon)
	{
		return LoadAsyncTool.Get(Defs.InnerWeaponsFolder + "/" + weapon.name + Defs.InnerWeapons_Suffix, true).asset as GameObject;
	}

	// Token: 0x060043F2 RID: 17394 RVA: 0x0016D39C File Offset: 0x0016B59C
	public static string FirstUnboughtOrForOurTier(string tg)
	{
		string text = WeaponManager.FirstUnboughtTag(tg);
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			string text2 = WeaponManager.FirstTagForOurTier(tg, null);
			List<string> list = WeaponUpgrades.ChainForTag(tg);
			if (text2 != null && list != null && list.IndexOf(text2) > list.IndexOf(text))
			{
				text = text2;
			}
		}
		return text;
	}

	// Token: 0x060043F3 RID: 17395 RVA: 0x0016D3F4 File Offset: 0x0016B5F4
	public static ResourceRequest InnerPrefabForWeaponAsync(string weapon)
	{
		return Resources.LoadAsync<GameObject>(Defs.InnerWeaponsFolder + "/" + weapon + Defs.InnerWeapons_Suffix);
	}

	// Token: 0x060043F4 RID: 17396 RVA: 0x0016D410 File Offset: 0x0016B610
	public static bool PurchasableWeaponSetContains(string weaponTag)
	{
		return WeaponManager._purchasableWeaponSet.Contains(weaponTag);
	}

	// Token: 0x17000B35 RID: 2869
	// (get) Token: 0x060043F5 RID: 17397 RVA: 0x0016D420 File Offset: 0x0016B620
	public static string PistolWN
	{
		get
		{
			return "Weapon1";
		}
	}

	// Token: 0x17000B36 RID: 2870
	// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0016D428 File Offset: 0x0016B628
	public static string ShotgunWN
	{
		get
		{
			return "Weapon2";
		}
	}

	// Token: 0x17000B37 RID: 2871
	// (get) Token: 0x060043F7 RID: 17399 RVA: 0x0016D430 File Offset: 0x0016B630
	public static string MP5WN
	{
		get
		{
			return "Weapon3";
		}
	}

	// Token: 0x17000B38 RID: 2872
	// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0016D438 File Offset: 0x0016B638
	public static string RevolverWN
	{
		get
		{
			return "Weapon4";
		}
	}

	// Token: 0x17000B39 RID: 2873
	// (get) Token: 0x060043F9 RID: 17401 RVA: 0x0016D440 File Offset: 0x0016B640
	public static string MachinegunWN
	{
		get
		{
			return "Weapon5";
		}
	}

	// Token: 0x17000B3A RID: 2874
	// (get) Token: 0x060043FA RID: 17402 RVA: 0x0016D448 File Offset: 0x0016B648
	public static string AK47WN
	{
		get
		{
			return "Weapon8";
		}
	}

	// Token: 0x17000B3B RID: 2875
	// (get) Token: 0x060043FB RID: 17403 RVA: 0x0016D450 File Offset: 0x0016B650
	public static string KnifeWN
	{
		get
		{
			return "Weapon9";
		}
	}

	// Token: 0x17000B3C RID: 2876
	// (get) Token: 0x060043FC RID: 17404 RVA: 0x0016D458 File Offset: 0x0016B658
	public static string ObrezWN
	{
		get
		{
			return "Weapon51";
		}
	}

	// Token: 0x17000B3D RID: 2877
	// (get) Token: 0x060043FD RID: 17405 RVA: 0x0016D460 File Offset: 0x0016B660
	public static string AlienGunWN
	{
		get
		{
			return "Weapon52";
		}
	}

	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x060043FE RID: 17406 RVA: 0x0016D468 File Offset: 0x0016B668
	public static string BugGunWN
	{
		get
		{
			return "Weapon250";
		}
	}

	// Token: 0x17000B3F RID: 2879
	// (get) Token: 0x060043FF RID: 17407 RVA: 0x0016D470 File Offset: 0x0016B670
	public static string SocialGunWN
	{
		get
		{
			return "Weapon302";
		}
	}

	// Token: 0x17000B40 RID: 2880
	// (get) Token: 0x06004400 RID: 17408 RVA: 0x0016D478 File Offset: 0x0016B678
	public static string _initialWeaponName
	{
		get
		{
			return "FirstPistol";
		}
	}

	// Token: 0x17000B41 RID: 2881
	// (get) Token: 0x06004401 RID: 17409 RVA: 0x0016D480 File Offset: 0x0016B680
	public static string PickWeaponName
	{
		get
		{
			return "Weapon6";
		}
	}

	// Token: 0x17000B42 RID: 2882
	// (get) Token: 0x06004402 RID: 17410 RVA: 0x0016D488 File Offset: 0x0016B688
	public static string MultiplayerMeleeTag
	{
		get
		{
			return "Knife";
		}
	}

	// Token: 0x17000B43 RID: 2883
	// (get) Token: 0x06004403 RID: 17411 RVA: 0x0016D490 File Offset: 0x0016B690
	public static string SwordWeaponName
	{
		get
		{
			return "Weapon7";
		}
	}

	// Token: 0x17000B44 RID: 2884
	// (get) Token: 0x06004404 RID: 17412 RVA: 0x0016D498 File Offset: 0x0016B698
	public static string CombatRifleWeaponName
	{
		get
		{
			return "Weapon10";
		}
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x06004405 RID: 17413 RVA: 0x0016D4A0 File Offset: 0x0016B6A0
	public static string GoldenEagleWeaponName
	{
		get
		{
			return "Weapon11";
		}
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x06004406 RID: 17414 RVA: 0x0016D4A8 File Offset: 0x0016B6A8
	public static string MagicBowWeaponName
	{
		get
		{
			return "Weapon12";
		}
	}

	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x06004407 RID: 17415 RVA: 0x0016D4B0 File Offset: 0x0016B6B0
	public static string SpasWeaponName
	{
		get
		{
			return "Weapon13";
		}
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06004408 RID: 17416 RVA: 0x0016D4B8 File Offset: 0x0016B6B8
	public static string GoldenAxeWeaponnName
	{
		get
		{
			return "Weapon14";
		}
	}

	// Token: 0x17000B49 RID: 2889
	// (get) Token: 0x06004409 RID: 17417 RVA: 0x0016D4C0 File Offset: 0x0016B6C0
	public static string ChainsawWN
	{
		get
		{
			return "Weapon15";
		}
	}

	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x0600440A RID: 17418 RVA: 0x0016D4C8 File Offset: 0x0016B6C8
	public static string FAMASWN
	{
		get
		{
			return "Weapon16";
		}
	}

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x0600440B RID: 17419 RVA: 0x0016D4D0 File Offset: 0x0016B6D0
	public static string GlockWN
	{
		get
		{
			return "Weapon17";
		}
	}

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x0600440C RID: 17420 RVA: 0x0016D4D8 File Offset: 0x0016B6D8
	public static string ScytheWN
	{
		get
		{
			return "Weapon18";
		}
	}

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x0600440D RID: 17421 RVA: 0x0016D4E0 File Offset: 0x0016B6E0
	public static string Scythe_2_WN
	{
		get
		{
			return "Weapon68";
		}
	}

	// Token: 0x17000B4E RID: 2894
	// (get) Token: 0x0600440E RID: 17422 RVA: 0x0016D4E8 File Offset: 0x0016B6E8
	public static string ShovelWN
	{
		get
		{
			return "Weapon19";
		}
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x0600440F RID: 17423 RVA: 0x0016D4F0 File Offset: 0x0016B6F0
	public static string HammerWN
	{
		get
		{
			return "Weapon20";
		}
	}

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x06004410 RID: 17424 RVA: 0x0016D4F8 File Offset: 0x0016B6F8
	public static string Sword_2_WN
	{
		get
		{
			return "Weapon21";
		}
	}

	// Token: 0x17000B51 RID: 2897
	// (get) Token: 0x06004411 RID: 17425 RVA: 0x0016D500 File Offset: 0x0016B700
	public static string StaffWN
	{
		get
		{
			return "Weapon22";
		}
	}

	// Token: 0x17000B52 RID: 2898
	// (get) Token: 0x06004412 RID: 17426 RVA: 0x0016D508 File Offset: 0x0016B708
	public static string LaserRifleWN
	{
		get
		{
			return "Weapon23";
		}
	}

	// Token: 0x17000B53 RID: 2899
	// (get) Token: 0x06004413 RID: 17427 RVA: 0x0016D510 File Offset: 0x0016B710
	public static string LightSwordWN
	{
		get
		{
			return "Weapon24";
		}
	}

	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x06004414 RID: 17428 RVA: 0x0016D518 File Offset: 0x0016B718
	public static string BerettaWN
	{
		get
		{
			return "Weapon25";
		}
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x06004415 RID: 17429 RVA: 0x0016D520 File Offset: 0x0016B720
	public static string Beretta_2_WN
	{
		get
		{
			return "Weapon71";
		}
	}

	// Token: 0x17000B56 RID: 2902
	// (get) Token: 0x06004416 RID: 17430 RVA: 0x0016D528 File Offset: 0x0016B728
	public static string MaceWN
	{
		get
		{
			return "Weapon26";
		}
	}

	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x06004417 RID: 17431 RVA: 0x0016D530 File Offset: 0x0016B730
	public static string CrossbowWN
	{
		get
		{
			return "Weapon27";
		}
	}

	// Token: 0x17000B58 RID: 2904
	// (get) Token: 0x06004418 RID: 17432 RVA: 0x0016D538 File Offset: 0x0016B738
	public static string MinigunWN
	{
		get
		{
			return "Weapon28";
		}
	}

	// Token: 0x17000B59 RID: 2905
	// (get) Token: 0x06004419 RID: 17433 RVA: 0x0016D540 File Offset: 0x0016B740
	public static string GoldenPickWN
	{
		get
		{
			return "Weapon29";
		}
	}

	// Token: 0x17000B5A RID: 2906
	// (get) Token: 0x0600441A RID: 17434 RVA: 0x0016D548 File Offset: 0x0016B748
	public static string CrystalPickWN
	{
		get
		{
			return "Weapon30";
		}
	}

	// Token: 0x17000B5B RID: 2907
	// (get) Token: 0x0600441B RID: 17435 RVA: 0x0016D550 File Offset: 0x0016B750
	public static string IronSwordWN
	{
		get
		{
			return "Weapon31";
		}
	}

	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x0600441C RID: 17436 RVA: 0x0016D558 File Offset: 0x0016B758
	public static string GoldenSwordWN
	{
		get
		{
			return "Weapon32";
		}
	}

	// Token: 0x17000B5D RID: 2909
	// (get) Token: 0x0600441D RID: 17437 RVA: 0x0016D560 File Offset: 0x0016B760
	public static string GoldenRed_StoneWN
	{
		get
		{
			return "Weapon33";
		}
	}

	// Token: 0x17000B5E RID: 2910
	// (get) Token: 0x0600441E RID: 17438 RVA: 0x0016D568 File Offset: 0x0016B768
	public static string GoldenSPASWN
	{
		get
		{
			return "Weapon34";
		}
	}

	// Token: 0x17000B5F RID: 2911
	// (get) Token: 0x0600441F RID: 17439 RVA: 0x0016D570 File Offset: 0x0016B770
	public static string GoldenGlockWN
	{
		get
		{
			return "Weapon35";
		}
	}

	// Token: 0x17000B60 RID: 2912
	// (get) Token: 0x06004420 RID: 17440 RVA: 0x0016D578 File Offset: 0x0016B778
	public static string RedMinigunWN
	{
		get
		{
			return "Weapon36";
		}
	}

	// Token: 0x17000B61 RID: 2913
	// (get) Token: 0x06004421 RID: 17441 RVA: 0x0016D580 File Offset: 0x0016B780
	public static string CrystalCrossbowWN
	{
		get
		{
			return "Weapon37";
		}
	}

	// Token: 0x17000B62 RID: 2914
	// (get) Token: 0x06004422 RID: 17442 RVA: 0x0016D588 File Offset: 0x0016B788
	public static string RedLightSaberWN
	{
		get
		{
			return "Weapon38";
		}
	}

	// Token: 0x17000B63 RID: 2915
	// (get) Token: 0x06004423 RID: 17443 RVA: 0x0016D590 File Offset: 0x0016B790
	public static string SandFamasWN
	{
		get
		{
			return "Weapon39";
		}
	}

	// Token: 0x17000B64 RID: 2916
	// (get) Token: 0x06004424 RID: 17444 RVA: 0x0016D598 File Offset: 0x0016B798
	public static string WhiteBerettaWN
	{
		get
		{
			return "Weapon40";
		}
	}

	// Token: 0x17000B65 RID: 2917
	// (get) Token: 0x06004425 RID: 17445 RVA: 0x0016D5A0 File Offset: 0x0016B7A0
	public static string BlackEagleWN
	{
		get
		{
			return "Weapon41";
		}
	}

	// Token: 0x17000B66 RID: 2918
	// (get) Token: 0x06004426 RID: 17446 RVA: 0x0016D5A8 File Offset: 0x0016B7A8
	public static string CrystalAxeWN
	{
		get
		{
			return "Weapon42";
		}
	}

	// Token: 0x17000B67 RID: 2919
	// (get) Token: 0x06004427 RID: 17447 RVA: 0x0016D5B0 File Offset: 0x0016B7B0
	public static string SteelAxeWN
	{
		get
		{
			return "Weapon43";
		}
	}

	// Token: 0x17000B68 RID: 2920
	// (get) Token: 0x06004428 RID: 17448 RVA: 0x0016D5B8 File Offset: 0x0016B7B8
	public static string WoodenBowWN
	{
		get
		{
			return "Weapon44";
		}
	}

	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x06004429 RID: 17449 RVA: 0x0016D5C0 File Offset: 0x0016B7C0
	public static string Chainsaw2WN
	{
		get
		{
			return "Weapon45";
		}
	}

	// Token: 0x17000B6A RID: 2922
	// (get) Token: 0x0600442A RID: 17450 RVA: 0x0016D5C8 File Offset: 0x0016B7C8
	public static string SteelCrossbowWN
	{
		get
		{
			return "Weapon46";
		}
	}

	// Token: 0x17000B6B RID: 2923
	// (get) Token: 0x0600442B RID: 17451 RVA: 0x0016D5D0 File Offset: 0x0016B7D0
	public static string Hammer2WN
	{
		get
		{
			return "Weapon47";
		}
	}

	// Token: 0x17000B6C RID: 2924
	// (get) Token: 0x0600442C RID: 17452 RVA: 0x0016D5D8 File Offset: 0x0016B7D8
	public static string Mace2WN
	{
		get
		{
			return "Weapon48";
		}
	}

	// Token: 0x17000B6D RID: 2925
	// (get) Token: 0x0600442D RID: 17453 RVA: 0x0016D5E0 File Offset: 0x0016B7E0
	public static string Sword_22WN
	{
		get
		{
			return "Weapon49";
		}
	}

	// Token: 0x17000B6E RID: 2926
	// (get) Token: 0x0600442E RID: 17454 RVA: 0x0016D5E8 File Offset: 0x0016B7E8
	public static string Staff2WN
	{
		get
		{
			return "Weapon50";
		}
	}

	// Token: 0x17000B6F RID: 2927
	// (get) Token: 0x0600442F RID: 17455 RVA: 0x0016D5F0 File Offset: 0x0016B7F0
	public static string M16_2WN
	{
		get
		{
			return "Weapon53";
		}
	}

	// Token: 0x17000B70 RID: 2928
	// (get) Token: 0x06004430 RID: 17456 RVA: 0x0016D5F8 File Offset: 0x0016B7F8
	public static string M16_3WN
	{
		get
		{
			return "Weapon69";
		}
	}

	// Token: 0x17000B71 RID: 2929
	// (get) Token: 0x06004431 RID: 17457 RVA: 0x0016D600 File Offset: 0x0016B800
	public static string M16_4WN
	{
		get
		{
			return "Weapon70";
		}
	}

	// Token: 0x17000B72 RID: 2930
	// (get) Token: 0x06004432 RID: 17458 RVA: 0x0016D608 File Offset: 0x0016B808
	public static string CrystalGlockWN
	{
		get
		{
			return "Weapon54";
		}
	}

	// Token: 0x17000B73 RID: 2931
	// (get) Token: 0x06004433 RID: 17459 RVA: 0x0016D610 File Offset: 0x0016B810
	public static string CrystalSPASWN
	{
		get
		{
			return "Weapon55";
		}
	}

	// Token: 0x17000B74 RID: 2932
	// (get) Token: 0x06004434 RID: 17460 RVA: 0x0016D618 File Offset: 0x0016B818
	public static string TreeWN
	{
		get
		{
			return "Weapon56";
		}
	}

	// Token: 0x17000B75 RID: 2933
	// (get) Token: 0x06004435 RID: 17461 RVA: 0x0016D620 File Offset: 0x0016B820
	public static string Tree_2_WN
	{
		get
		{
			return "Weapon72";
		}
	}

	// Token: 0x17000B76 RID: 2934
	// (get) Token: 0x06004436 RID: 17462 RVA: 0x0016D628 File Offset: 0x0016B828
	public static string FireAxeWN
	{
		get
		{
			return "Weapon57";
		}
	}

	// Token: 0x17000B77 RID: 2935
	// (get) Token: 0x06004437 RID: 17463 RVA: 0x0016D630 File Offset: 0x0016B830
	public static string _3pl_shotgunWN
	{
		get
		{
			return "Weapon58";
		}
	}

	// Token: 0x17000B78 RID: 2936
	// (get) Token: 0x06004438 RID: 17464 RVA: 0x0016D638 File Offset: 0x0016B838
	public static string Revolver2WN
	{
		get
		{
			return "Weapon59";
		}
	}

	// Token: 0x17000B79 RID: 2937
	// (get) Token: 0x06004439 RID: 17465 RVA: 0x0016D640 File Offset: 0x0016B840
	public static string BarrettWN
	{
		get
		{
			return "Weapon60";
		}
	}

	// Token: 0x17000B7A RID: 2938
	// (get) Token: 0x0600443A RID: 17466 RVA: 0x0016D648 File Offset: 0x0016B848
	public static string svdWN
	{
		get
		{
			return "Weapon61";
		}
	}

	// Token: 0x17000B7B RID: 2939
	// (get) Token: 0x0600443B RID: 17467 RVA: 0x0016D650 File Offset: 0x0016B850
	public static string NavyFamasWN
	{
		get
		{
			return "Weapon62";
		}
	}

	// Token: 0x17000B7C RID: 2940
	// (get) Token: 0x0600443C RID: 17468 RVA: 0x0016D658 File Offset: 0x0016B858
	public static string svd_2WN
	{
		get
		{
			return "Weapon63";
		}
	}

	// Token: 0x17000B7D RID: 2941
	// (get) Token: 0x0600443D RID: 17469 RVA: 0x0016D660 File Offset: 0x0016B860
	public static string Eagle_3WN
	{
		get
		{
			return "Weapon64";
		}
	}

	// Token: 0x17000B7E RID: 2942
	// (get) Token: 0x0600443E RID: 17470 RVA: 0x0016D668 File Offset: 0x0016B868
	public static string Barrett_2WN
	{
		get
		{
			return "Weapon65";
		}
	}

	// Token: 0x17000B7F RID: 2943
	// (get) Token: 0x0600443F RID: 17471 RVA: 0x0016D670 File Offset: 0x0016B870
	public static string UZI_WN
	{
		get
		{
			return "Weapon66";
		}
	}

	// Token: 0x17000B80 RID: 2944
	// (get) Token: 0x06004440 RID: 17472 RVA: 0x0016D678 File Offset: 0x0016B878
	public static string CampaignRifle_WN
	{
		get
		{
			return "Weapon67";
		}
	}

	// Token: 0x17000B81 RID: 2945
	// (get) Token: 0x06004441 RID: 17473 RVA: 0x0016D680 File Offset: 0x0016B880
	public static string SimpleFlamethrower_WN
	{
		get
		{
			return "Weapon333";
		}
	}

	// Token: 0x17000B82 RID: 2946
	// (get) Token: 0x06004442 RID: 17474 RVA: 0x0016D688 File Offset: 0x0016B888
	public static string Flamethrower_WN
	{
		get
		{
			return "Weapon73";
		}
	}

	// Token: 0x17000B83 RID: 2947
	// (get) Token: 0x06004443 RID: 17475 RVA: 0x0016D690 File Offset: 0x0016B890
	public static string Flamethrower_2_WN
	{
		get
		{
			return "Weapon74";
		}
	}

	// Token: 0x17000B84 RID: 2948
	// (get) Token: 0x06004444 RID: 17476 RVA: 0x0016D698 File Offset: 0x0016B898
	public static string Bazooka_WN
	{
		get
		{
			return "Weapon75";
		}
	}

	// Token: 0x17000B85 RID: 2949
	// (get) Token: 0x06004445 RID: 17477 RVA: 0x0016D6A0 File Offset: 0x0016B8A0
	public static string Bazooka_2_WN
	{
		get
		{
			return "Weapon76";
		}
	}

	// Token: 0x17000B86 RID: 2950
	// (get) Token: 0x06004446 RID: 17478 RVA: 0x0016D6A8 File Offset: 0x0016B8A8
	public static string Railgun_WN
	{
		get
		{
			return "Weapon77";
		}
	}

	// Token: 0x17000B87 RID: 2951
	// (get) Token: 0x06004447 RID: 17479 RVA: 0x0016D6B0 File Offset: 0x0016B8B0
	public static string Tesla_WN
	{
		get
		{
			return "Weapon78";
		}
	}

	// Token: 0x17000B88 RID: 2952
	// (get) Token: 0x06004448 RID: 17480 RVA: 0x0016D6B8 File Offset: 0x0016B8B8
	public static string GrenadeLunacher_WN
	{
		get
		{
			return "Weapon79";
		}
	}

	// Token: 0x17000B89 RID: 2953
	// (get) Token: 0x06004449 RID: 17481 RVA: 0x0016D6C0 File Offset: 0x0016B8C0
	public static string GrenadeLunacher_2_WN
	{
		get
		{
			return "Weapon80";
		}
	}

	// Token: 0x17000B8A RID: 2954
	// (get) Token: 0x0600444A RID: 17482 RVA: 0x0016D6C8 File Offset: 0x0016B8C8
	public static string Tesla_2_WN
	{
		get
		{
			return "Weapon81";
		}
	}

	// Token: 0x17000B8B RID: 2955
	// (get) Token: 0x0600444B RID: 17483 RVA: 0x0016D6D0 File Offset: 0x0016B8D0
	public static string Bazooka_3_WN
	{
		get
		{
			return "Weapon82";
		}
	}

	// Token: 0x17000B8C RID: 2956
	// (get) Token: 0x0600444C RID: 17484 RVA: 0x0016D6D8 File Offset: 0x0016B8D8
	public static string Gravigun_WN
	{
		get
		{
			return "Weapon83";
		}
	}

	// Token: 0x17000B8D RID: 2957
	// (get) Token: 0x0600444D RID: 17485 RVA: 0x0016D6E0 File Offset: 0x0016B8E0
	public static string AUG_WN
	{
		get
		{
			return "Weapon84";
		}
	}

	// Token: 0x17000B8E RID: 2958
	// (get) Token: 0x0600444E RID: 17486 RVA: 0x0016D6E8 File Offset: 0x0016B8E8
	public static string AUG_2_WN
	{
		get
		{
			return "Weapon85";
		}
	}

	// Token: 0x17000B8F RID: 2959
	// (get) Token: 0x0600444F RID: 17487 RVA: 0x0016D6F0 File Offset: 0x0016B8F0
	public static string Razer_WN
	{
		get
		{
			return "Weapon86";
		}
	}

	// Token: 0x17000B90 RID: 2960
	// (get) Token: 0x06004450 RID: 17488 RVA: 0x0016D6F8 File Offset: 0x0016B8F8
	public static string Razer_2_WN
	{
		get
		{
			return "Weapon87";
		}
	}

	// Token: 0x17000B91 RID: 2961
	// (get) Token: 0x06004451 RID: 17489 RVA: 0x0016D700 File Offset: 0x0016B900
	public static string katana_WN
	{
		get
		{
			return "Weapon88";
		}
	}

	// Token: 0x17000B92 RID: 2962
	// (get) Token: 0x06004452 RID: 17490 RVA: 0x0016D708 File Offset: 0x0016B908
	public static string katana_2_WN
	{
		get
		{
			return "Weapon89";
		}
	}

	// Token: 0x17000B93 RID: 2963
	// (get) Token: 0x06004453 RID: 17491 RVA: 0x0016D710 File Offset: 0x0016B910
	public static string katana_3_WN
	{
		get
		{
			return "Weapon90";
		}
	}

	// Token: 0x17000B94 RID: 2964
	// (get) Token: 0x06004454 RID: 17492 RVA: 0x0016D718 File Offset: 0x0016B918
	public static string plazma_WN
	{
		get
		{
			return "Weapon91";
		}
	}

	// Token: 0x17000B95 RID: 2965
	// (get) Token: 0x06004455 RID: 17493 RVA: 0x0016D720 File Offset: 0x0016B920
	public static string plazma_pistol_WN
	{
		get
		{
			return "Weapon92";
		}
	}

	// Token: 0x17000B96 RID: 2966
	// (get) Token: 0x06004456 RID: 17494 RVA: 0x0016D728 File Offset: 0x0016B928
	public static string Flower_WN
	{
		get
		{
			return "Weapon93";
		}
	}

	// Token: 0x17000B97 RID: 2967
	// (get) Token: 0x06004457 RID: 17495 RVA: 0x0016D730 File Offset: 0x0016B930
	public static string Buddy_WN
	{
		get
		{
			return "Weapon94";
		}
	}

	// Token: 0x17000B98 RID: 2968
	// (get) Token: 0x06004458 RID: 17496 RVA: 0x0016D738 File Offset: 0x0016B938
	public static string Mauser_WN
	{
		get
		{
			return "Weapon95";
		}
	}

	// Token: 0x17000B99 RID: 2969
	// (get) Token: 0x06004459 RID: 17497 RVA: 0x0016D740 File Offset: 0x0016B940
	public static string Shmaiser_WN
	{
		get
		{
			return "Weapon96";
		}
	}

	// Token: 0x17000B9A RID: 2970
	// (get) Token: 0x0600445A RID: 17498 RVA: 0x0016D748 File Offset: 0x0016B948
	public static string Thompson_WN
	{
		get
		{
			return "Weapon97";
		}
	}

	// Token: 0x17000B9B RID: 2971
	// (get) Token: 0x0600445B RID: 17499 RVA: 0x0016D750 File Offset: 0x0016B950
	public static string Thompson_2_WN
	{
		get
		{
			return "Weapon98";
		}
	}

	// Token: 0x17000B9C RID: 2972
	// (get) Token: 0x0600445C RID: 17500 RVA: 0x0016D758 File Offset: 0x0016B958
	public static string BassCannon_WN
	{
		get
		{
			return "Weapon99";
		}
	}

	// Token: 0x17000B9D RID: 2973
	// (get) Token: 0x0600445D RID: 17501 RVA: 0x0016D760 File Offset: 0x0016B960
	public static string SpakrlyBlaster_WN
	{
		get
		{
			return "Weapon100";
		}
	}

	// Token: 0x17000B9E RID: 2974
	// (get) Token: 0x0600445E RID: 17502 RVA: 0x0016D768 File Offset: 0x0016B968
	public static string CherryGun_WN
	{
		get
		{
			return "Weapon101";
		}
	}

	// Token: 0x17000B9F RID: 2975
	// (get) Token: 0x0600445F RID: 17503 RVA: 0x0016D770 File Offset: 0x0016B970
	public static string AK74_WN
	{
		get
		{
			return "Weapon102";
		}
	}

	// Token: 0x17000BA0 RID: 2976
	// (get) Token: 0x06004460 RID: 17504 RVA: 0x0016D778 File Offset: 0x0016B978
	public static string AK74_2_WN
	{
		get
		{
			return "Weapon103";
		}
	}

	// Token: 0x17000BA1 RID: 2977
	// (get) Token: 0x06004461 RID: 17505 RVA: 0x0016D780 File Offset: 0x0016B980
	public static string AK74_3_WN
	{
		get
		{
			return "Weapon104";
		}
	}

	// Token: 0x17000BA2 RID: 2978
	// (get) Token: 0x06004462 RID: 17506 RVA: 0x0016D788 File Offset: 0x0016B988
	public static string FreezeGun_WN
	{
		get
		{
			return "Weapon105";
		}
	}

	// Token: 0x17000BA3 RID: 2979
	// (get) Token: 0x06004463 RID: 17507 RVA: 0x0016D790 File Offset: 0x0016B990
	// (set) Token: 0x06004464 RID: 17508 RVA: 0x0016D798 File Offset: 0x0016B998
	public int CurrentWeaponIndex
	{
		get
		{
			return this.currentWeaponIndex;
		}
		set
		{
			this.currentWeaponIndex = value;
		}
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x0016D7A4 File Offset: 0x0016B9A4
	public void SaveWeaponAsLastUsed(int index)
	{
		if (Defs.isMulti && (!Defs.isHunger || SceneLoader.ActiveSceneName == "ConnectScene" || SceneLoader.ActiveSceneName == "ConnectSceneSandbox") && this.playerWeapons != null && this.playerWeapons.Count > index && index >= 0)
		{
			try
			{
				int value = (this.playerWeapons[index] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1;
				if (this.lastUsedWeaponsForFilterMaps.ContainsKey(this._currentFilterMap.ToString()))
				{
					this.lastUsedWeaponsForFilterMaps[this._currentFilterMap.ToString()] = value;
				}
				else
				{
					this.lastUsedWeaponsForFilterMaps.Add(this._currentFilterMap.ToString(), value);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Exception in SaveWeaponAsLastUsed index = " + index);
			}
		}
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x0016D8C0 File Offset: 0x0016BAC0
	public int CurrentIndexOfLastUsedWeaponInPlayerWeapons()
	{
		if (Defs.isHunger)
		{
			return 0;
		}
		int result = 0;
		try
		{
			if (this.lastUsedWeaponsForFilterMaps.ContainsKey(this._currentFilterMap.ToString()))
			{
				int lastUsedCategory = this.lastUsedWeaponsForFilterMaps[this._currentFilterMap.ToString()];
				int num = this.playerWeapons.Cast<Weapon>().ToList<Weapon>().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == lastUsedCategory);
				if (num != -1)
				{
					result = num;
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in CurrentIndexOfLastUsedWeaponInPlayerWeapons: " + arg);
			result = 0;
		}
		return result;
	}

	// Token: 0x17000BA4 RID: 2980
	// (get) Token: 0x06004467 RID: 17511 RVA: 0x0016D980 File Offset: 0x0016BB80
	public int ShopListsTierConstraint
	{
		get
		{
			return 10000;
		}
	}

	// Token: 0x06004468 RID: 17512 RVA: 0x0016D988 File Offset: 0x0016BB88
	private void UpdateFilteredShopLists()
	{
		this.FilteredShopListsForPromos = new List<List<GameObject>>();
		this.FilteredShopListsNoUpgrades = new List<List<GameObject>>();
		for (int i = 0; i < this._weaponsByCat.Count; i++)
		{
			this.FilteredShopListsForPromos.Add(new List<GameObject>());
			this.FilteredShopListsNoUpgrades.Add(new List<GameObject>());
			for (int j = 0; j < this._weaponsByCat[i].Count; j++)
			{
				bool flag = true;
				bool flag2 = true;
				try
				{
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(this._weaponsByCat[i][j].name.Replace("(Clone)", string.Empty));
					if (byPrefabName.CanBuy)
					{
						List<string> list = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
						string tag = (list == null || list.Count <= 0) ? byPrefabName.Tag : list[0];
						ItemRecord byTag = ItemDb.GetByTag(tag);
						string text = WeaponManager.LastBoughtTag(byPrefabName.Tag, null);
						if ((Storager.getInt(byPrefabName.StorageId, true) == 0 && list != null && list.IndexOf(byPrefabName.Tag) > list.IndexOf(WeaponManager.FirstTagForOurTier(byPrefabName.Tag, null))) || (text != null && list != null && list.IndexOf(text) > list.IndexOf(byPrefabName.Tag)))
						{
							flag2 = false;
						}
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("Exception in UpdateFilteredShopLists: " + arg);
				}
				if (flag)
				{
					this.FilteredShopListsForPromos[i].Add(this._weaponsByCat[i][j]);
				}
				if (flag2)
				{
					this.FilteredShopListsNoUpgrades[i].Add(this._weaponsByCat[i][j]);
				}
			}
		}
	}

	// Token: 0x06004469 RID: 17513 RVA: 0x0016DB90 File Offset: 0x0016BD90
	public void SaveWeaponSet(string sn, string wn, int pos)
	{
		string text = this.LoadWeaponSet(sn);
		string[] array = text.Split(new char[]
		{
			'#'
		});
		array[pos] = wn;
		string text2 = string.Join("#", array);
		if (!Application.isEditor)
		{
			if (!Storager.hasKey(sn))
			{
			}
			Storager.setString(sn, text2, false);
		}
		else
		{
			PlayerPrefs.SetString(sn, text2);
		}
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x0016DBF0 File Offset: 0x0016BDF0
	public static string _KnifeSet()
	{
		return "##" + WeaponManager.KnifeWN + "###";
	}

	// Token: 0x0600446B RID: 17515 RVA: 0x0016DC14 File Offset: 0x0016BE14
	public static string _KnifeAndPistolSet()
	{
		return string.Concat(new string[]
		{
			"#",
			WeaponManager.PistolWN,
			"#",
			WeaponManager.KnifeWN,
			"###"
		});
	}

	// Token: 0x0600446C RID: 17516 RVA: 0x0016DC58 File Offset: 0x0016BE58
	public static string _KnifeAndPistolAndShotgunSet()
	{
		return string.Concat(new string[]
		{
			WeaponManager.ShotgunWN,
			"#",
			WeaponManager.PistolWN,
			"#",
			WeaponManager.KnifeWN,
			"###"
		});
	}

	// Token: 0x0600446D RID: 17517 RVA: 0x0016DCA4 File Offset: 0x0016BEA4
	public static string _KnifeAndPistolAndSniperSet()
	{
		return string.Concat(new string[]
		{
			"#",
			WeaponManager.PistolWN,
			"#",
			WeaponManager.KnifeWN,
			"##",
			WeaponManager.CampaignRifle_WN,
			"#"
		});
	}

	// Token: 0x0600446E RID: 17518 RVA: 0x0016DCF8 File Offset: 0x0016BEF8
	public static string _KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet()
	{
		return string.Concat(new string[]
		{
			WeaponManager.MP5WN,
			"#",
			WeaponManager.PistolWN,
			"#",
			WeaponManager.KnifeWN,
			"#",
			(!TrainingController.TrainingCompleted) ? string.Empty : WeaponManager.SimpleFlamethrower_WN,
			"#",
			(!TrainingController.TrainingCompleted) ? string.Empty : WeaponManager.CampaignRifle_WN,
			"#",
			(!TrainingController.TrainingCompleted) ? string.Empty : WeaponManager.Rocketnitza_WN
		});
	}

	// Token: 0x0600446F RID: 17519 RVA: 0x0016DDAC File Offset: 0x0016BFAC
	public static string _InitialDaterSet()
	{
		return "##" + WeaponManager.DaterFreeWeaponPrefabName + "###";
	}

	// Token: 0x06004470 RID: 17520 RVA: 0x0016DDD0 File Offset: 0x0016BFD0
	private string DefaultSetForWeaponSetSettingName(string sn)
	{
		string result = WeaponManager._KnifeAndPistolAndShotgunSet();
		if (sn != Defs.CampaignWSSN)
		{
			try
			{
				result = (from kvp in WeaponManager.WeaponSetSettingNamesForFilterMaps
				where kvp.Value.settingName == sn
				select kvp).FirstOrDefault<KeyValuePair<int, FilterMapSettings>>().Value.defaultWeaponSet();
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Exception in LoadWeaponSet: sn = ",
					sn,
					"    exception: ",
					ex
				}));
			}
		}
		return result;
	}

	// Token: 0x06004471 RID: 17521 RVA: 0x0016DE88 File Offset: 0x0016C088
	public string LoadWeaponSet(string sn)
	{
		if (!Application.isEditor)
		{
			if (!Storager.hasKey(sn))
			{
				Storager.setString(sn, this.DefaultSetForWeaponSetSettingName(sn), false);
			}
			return Storager.getString(sn, false);
		}
		return PlayerPrefs.GetString(sn, this.DefaultSetForWeaponSetSettingName(sn));
	}

	// Token: 0x06004472 RID: 17522 RVA: 0x0016DED0 File Offset: 0x0016C0D0
	public void SetWeaponsSet(int filterMap = 0)
	{
		this._playerWeapons.Clear();
		bool isMulti = Defs.isMulti;
		bool isHunger = Defs.isHunger;
		string text = null;
		if (isMulti)
		{
			if (!isHunger)
			{
				if (WeaponManager.WeaponSetSettingNamesForFilterMaps.ContainsKey(filterMap))
				{
					text = this.LoadWeaponSet(WeaponManager.WeaponSetSettingNamesForFilterMaps[filterMap].settingName);
				}
				else
				{
					Debug.LogError("WeaponSetSettingNamesForFilterMaps.ContainsKey (filterMap): filterMap = " + filterMap);
				}
			}
			else
			{
				text = WeaponManager._KnifeSet();
			}
		}
		else if (!Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			text = this.LoadWeaponSet(Defs.CampaignWSSN);
		}
		else if (Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			text = this.LoadWeaponSet(Defs.MultiplayerWSSN);
		}
		else
		{
			text = WeaponManager._KnifeAndPistolSet();
		}
		string[] array = text.Split(new char[]
		{
			'#'
		});
		foreach (string value in array)
		{
			if (!string.IsNullOrEmpty(value))
			{
				foreach (object obj in this.allAvailablePlayerWeapons)
				{
					Weapon weapon = (Weapon)obj;
					if (weapon.weaponPrefab.name.Equals(value))
					{
						this.EquipWeapon(weapon, false, false);
						break;
					}
				}
			}
		}
		if (filterMap == 2)
		{
			foreach (object obj2 in this.allAvailablePlayerWeapons)
			{
				Weapon weapon2 = (Weapon)obj2;
				if (weapon2.weaponPrefab.name.Equals(WeaponManager.KnifeWN))
				{
					this.EquipWeapon(weapon2, false, false);
					break;
				}
			}
			foreach (object obj3 in this.allAvailablePlayerWeapons)
			{
				Weapon weapon3 = (Weapon)obj3;
				if (weapon3.weaponPrefab.name.Equals(WeaponManager.PistolWN))
				{
					this.EquipWeapon(weapon3, false, false);
					break;
				}
			}
		}
		if (filterMap == 2 && this.playerWeapons.Count == 2)
		{
			foreach (object obj4 in this.allAvailablePlayerWeapons)
			{
				Weapon weapon4 = (Weapon)obj4;
				if (weapon4.weaponPrefab.name.Equals(WeaponManager.CampaignRifle_WN))
				{
					this.EquipWeapon(weapon4, false, false);
					break;
				}
			}
		}
		if (filterMap == 3)
		{
			if (this.playerWeapons.OfType<Weapon>().FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 2) == null)
			{
				foreach (object obj5 in this.allAvailablePlayerWeapons)
				{
					Weapon weapon5 = (Weapon)obj5;
					if (weapon5.weaponPrefab.name.Equals(WeaponManager.DaterFreeWeaponPrefabName))
					{
						this.EquipWeapon(weapon5, false, false);
						break;
					}
				}
			}
		}
		if (this.playerWeapons.Count == 0)
		{
			this.UpdatePlayersWeaponSetCache();
		}
	}

	// Token: 0x06004473 RID: 17523 RVA: 0x0016E308 File Offset: 0x0016C508
	public static string LastBoughtTag(string tg, List<string> weaponUpgradesHint = null)
	{
		if (tg == null)
		{
			return null;
		}
		if (tg == "Armor_Novice")
		{
			return (!ShopNGUIController.NoviceArmorAvailable) ? null : "Armor_Novice";
		}
		if (!WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
			{
				foreach (List<string> list in keyValuePair.Value)
				{
					if (list.Contains(tg))
					{
						if (TempItemsController.PriceCoefs.ContainsKey(tg))
						{
							return (!(TempItemsController.sharedController != null) || !TempItemsController.sharedController.ContainsItem(tg)) ? null : tg;
						}
						if (Storager.getInt(list[0], true) == 0)
						{
							return null;
						}
						for (int i = 1; i < list.Count; i++)
						{
							if (Storager.getInt(list[i], true) == 0)
							{
								return list[i - 1];
							}
						}
						return list[list.Count - 1];
					}
				}
			}
			return tg;
		}
		bool flag = weaponUpgradesHint != null;
		List<string> list2 = weaponUpgradesHint;
		if (list2 == null)
		{
			foreach (List<string> list3 in WeaponUpgrades.upgrades)
			{
				if (list3.Contains(tg))
				{
					list2 = list3;
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list2[j]]], true) == 1)
				{
					return list2[j];
				}
			}
			return null;
		}
		bool flag2 = ItemDb.IsTemporaryGun(tg);
		if ((!flag2 && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[tg]], true) == 1) || (flag2 && TempItemsController.sharedController.ContainsItem(tg)))
		{
			return tg;
		}
		return null;
	}

	// Token: 0x06004474 RID: 17524 RVA: 0x0016E5D0 File Offset: 0x0016C7D0
	public static string FirstUnboughtTag(string tg)
	{
		if (tg == null)
		{
			return null;
		}
		if (tg == "Armor_Novice")
		{
			return "Armor_Novice";
		}
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(tg))
		{
			bool flag = false;
			List<string> list = null;
			foreach (List<string> list2 in WeaponUpgrades.upgrades)
			{
				if (list2.Contains(tg))
				{
					list = list2;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int i = list.Count - 1;
				while (i >= 0)
				{
					if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list[i]]], true) == 1)
					{
						if (i < list.Count - 1)
						{
							return list[i + 1];
						}
						return list[i];
					}
					else
					{
						i--;
					}
				}
				return list[0];
			}
			return tg;
		}
		else
		{
			if (TempItemsController.PriceCoefs.ContainsKey(tg))
			{
				return tg;
			}
			foreach (KeyValuePair<ShopNGUIController.CategoryNames, List<List<string>>> keyValuePair in Wear.wear)
			{
				foreach (List<string> list3 in keyValuePair.Value)
				{
					if (list3.Contains(tg))
					{
						for (int j = 0; j < list3.Count; j++)
						{
							if (Storager.getInt(list3[j], true) == 0)
							{
								return list3[j];
							}
						}
						return list3[list3.Count - 1];
					}
				}
			}
			return tg;
		}
	}

	// Token: 0x06004475 RID: 17525 RVA: 0x0016E80C File Offset: 0x0016CA0C
	private void UpdatePlayersWeaponSetCache()
	{
		if (Device.IsLoweMemoryDevice)
		{
			Resources.UnloadUnusedAssets();
		}
	}

	// Token: 0x06004476 RID: 17526 RVA: 0x0016E820 File Offset: 0x0016CA20
	private void SetWeaponInAppropriateMultyModes(WeaponSounds ws)
	{
		List<int> list = new List<int>
		{
			0
		}.Concat((ws.filterMap == null) ? new int[0] : ws.filterMap).Distinct<int>().ToList<int>();
		foreach (int num in list)
		{
			if (WeaponManager.WeaponSetSettingNamesForFilterMaps.ContainsKey(num))
			{
				this.SaveWeaponSet(WeaponManager.WeaponSetSettingNamesForFilterMaps[num].settingName, ws.gameObject.name, ws.categoryNabor - 1);
			}
			else
			{
				Debug.LogError("WeaponSetSettingNamesForFilterMaps.ContainsKey (mode): " + num);
			}
		}
	}

	// Token: 0x06004477 RID: 17527 RVA: 0x0016E908 File Offset: 0x0016CB08
	public void EquipWeapon(Weapon w, bool shouldSave = true, bool shouldEquipToDaterSetOnly = false)
	{
		if (w == null)
		{
			Debug.LogWarning("Exiting from EquipWeapon(), because weapon is null.");
			return;
		}
		WeaponSounds component = w.weaponPrefab.GetComponent<WeaponSounds>();
		int categoryNabor = component.categoryNabor;
		bool flag = false;
		for (int i = 0; i < this.playerWeapons.Count; i++)
		{
			if ((this.playerWeapons[i] as Weapon).weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == categoryNabor)
			{
				flag = true;
				this.playerWeapons[i] = w;
				this.UpdatePlayersWeaponSetCache();
				break;
			}
		}
		if (!flag)
		{
			this.playerWeapons.Add(w);
			this.UpdatePlayersWeaponSetCache();
		}
		this.playerWeapons.Sort(new WeaponComparer());
		this.playerWeapons.Reverse();
		this.CurrentWeaponIndex = this.playerWeapons.IndexOf(w);
		Action<WeaponSounds> weaponEquipped_AllCases = WeaponManager.WeaponEquipped_AllCases;
		if (weaponEquipped_AllCases != null)
		{
			weaponEquipped_AllCases(component);
		}
		if (!shouldSave)
		{
			return;
		}
		string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
		{
			'#'
		});
		List<string> list = new List<string>();
		foreach (string item in array)
		{
			list.Add(item);
		}
		bool flag2 = (!(w.weaponPrefab.name == WeaponManager.Rocketnitza_WN) || list.Contains(WeaponManager.Rocketnitza_WN)) && (!w.weaponPrefab.name.Equals(WeaponManager.MP5WN) || list.Contains(WeaponManager.MP5WN)) && (!w.weaponPrefab.name.Equals(WeaponManager.CampaignRifle_WN) || list.Contains(WeaponManager.CampaignRifle_WN)) && (!w.weaponPrefab.name.Equals(WeaponManager.SimpleFlamethrower_WN) || list.Contains(WeaponManager.SimpleFlamethrower_WN));
		if (Defs.isMulti)
		{
			if (Defs.isHunger)
			{
				if (SceneLoader.ActiveSceneName == "ConnectScene" || SceneLoader.ActiveSceneName == "ConnectSceneSandbox")
				{
					this.SetWeaponInAppropriateMultyModes(component);
					if (flag2)
					{
						this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, categoryNabor - 1);
					}
				}
			}
			else
			{
				if (shouldEquipToDaterSetOnly && Defs.isDaterRegim)
				{
					this.SaveWeaponSet(Defs.DaterWSSN, w.weaponPrefab.name, categoryNabor - 1);
				}
				else
				{
					this.SetWeaponInAppropriateMultyModes(component);
				}
				if (flag2)
				{
					this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, categoryNabor - 1);
				}
			}
		}
		else if (!Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			if (!component.campaignOnly && !w.weaponPrefab.name.Equals(WeaponManager.AlienGunWN))
			{
				this.SetWeaponInAppropriateMultyModes(component);
			}
			this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, categoryNabor - 1);
		}
		else if (Defs.IsSurvival && TrainingController.TrainingCompleted && !component.campaignOnly && !w.weaponPrefab.name.Equals(WeaponManager.AlienGunWN))
		{
			this.SetWeaponInAppropriateMultyModes(component);
			if (flag2)
			{
				this.SaveWeaponSet(Defs.CampaignWSSN, w.weaponPrefab.name, categoryNabor - 1);
			}
		}
		if (WeaponManager.WeaponEquipped != null)
		{
			WeaponManager.WeaponEquipped(component);
		}
	}

	// Token: 0x17000BA5 RID: 2981
	// (get) Token: 0x06004478 RID: 17528 RVA: 0x0016EC98 File Offset: 0x0016CE98
	public UnityEngine.Object[] weaponsInGame
	{
		get
		{
			return this._weaponsInGame;
		}
	}

	// Token: 0x17000BA6 RID: 2982
	// (get) Token: 0x06004479 RID: 17529 RVA: 0x0016ECA0 File Offset: 0x0016CEA0
	public ArrayList playerWeapons
	{
		get
		{
			return this._playerWeapons;
		}
	}

	// Token: 0x17000BA7 RID: 2983
	// (get) Token: 0x0600447A RID: 17530 RVA: 0x0016ECA8 File Offset: 0x0016CEA8
	// (set) Token: 0x0600447B RID: 17531 RVA: 0x0016ECB0 File Offset: 0x0016CEB0
	public ArrayList allAvailablePlayerWeapons
	{
		get
		{
			return this._allAvailablePlayerWeapons;
		}
		private set
		{
			this._allAvailablePlayerWeapons = value;
		}
	}

	// Token: 0x17000BA8 RID: 2984
	// (get) Token: 0x0600447C RID: 17532 RVA: 0x0016ECBC File Offset: 0x0016CEBC
	// (set) Token: 0x0600447D RID: 17533 RVA: 0x0016ECC4 File Offset: 0x0016CEC4
	public WeaponSounds currentWeaponSounds
	{
		get
		{
			return this._currentWeaponSounds;
		}
		set
		{
			this._currentWeaponSounds = value;
		}
	}

	// Token: 0x17000BA9 RID: 2985
	// (get) Token: 0x0600447E RID: 17534 RVA: 0x0016ECD0 File Offset: 0x0016CED0
	public int LockGetWeaponPrefabs
	{
		get
		{
			return this._lockGetWeaponPrefabs;
		}
	}

	// Token: 0x0600447F RID: 17535 RVA: 0x0016ECD8 File Offset: 0x0016CED8
	public void GetWeaponPrefabs(int filterMap = 0)
	{
		IEnumerator weaponPrefabsCoroutine = this.GetWeaponPrefabsCoroutine(filterMap);
		while (weaponPrefabsCoroutine.MoveNext())
		{
			object obj = weaponPrefabsCoroutine.Current;
		}
	}

	// Token: 0x06004480 RID: 17536 RVA: 0x0016ED04 File Offset: 0x0016CF04
	private static string GetWeaponPathByName(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			return "Weapons/";
		}
		WeaponManager._sharedStringBuilder.Length = 0;
		WeaponManager._sharedStringBuilder.Append("Weapons/").Append(weaponName);
		string result = WeaponManager._sharedStringBuilder.ToString();
		WeaponManager._sharedStringBuilder.Length = 0;
		return result;
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x0016ED5C File Offset: 0x0016CF5C
	private IEnumerator GetWeaponPrefabsCoroutine(int filterMap = 0)
	{
		this._lockGetWeaponPrefabs++;
		int counter = 0;
		if (this.outerWeaponPrefabs == null)
		{
			List<string> weaponNames = (from rec in ItemDb.allRecords
			where !rec.Deactivated
			select rec.PrefabName).ToList<string>();
			int weaponNameCount = weaponNames.Count;
			this.outerWeaponPrefabs = new List<WeaponSounds>(weaponNameCount);
			for (int weaponIndex = 0; weaponIndex < weaponNameCount; weaponIndex++)
			{
				string wn = weaponNames[weaponIndex];
				if (!wn.IsNullOrEmpty())
				{
					string weaponPath = WeaponManager.GetWeaponPathByName(wn);
					WeaponSounds weaponPrefab = Resources.Load<WeaponSounds>(weaponPath);
					if (weaponPrefab == null)
					{
						Debug.LogError("No weapon " + wn);
					}
					else
					{
						this.outerWeaponPrefabs.Add(weaponPrefab);
						counter++;
						if (counter % 10 == 0)
						{
							yield return null;
						}
					}
				}
			}
		}
		bool isMulti = Defs.isMulti;
		bool isHungry = isMulti && Defs.isHunger;
		List<UnityEngine.Object> wInG = new List<UnityEngine.Object>(this.outerWeaponPrefabs.Count);
		foreach (WeaponSounds ws in this.outerWeaponPrefabs)
		{
			if (ws.IsAvalibleFromFilter(filterMap))
			{
				if (isMulti)
				{
					if (!isHungry)
					{
						if (!ws.campaignOnly)
						{
							wInG.Add(ws.gameObject);
						}
					}
					else
					{
						int num = int.Parse(ws.gameObject.name.Substring("Weapon".Length));
						if (num == 9 || ChestController.weaponForHungerGames.Contains(num))
						{
							wInG.Add(ws.gameObject);
						}
					}
				}
				else
				{
					wInG.Add(ws.gameObject);
				}
			}
		}
		this._weaponsInGame = wInG.ToArray();
		this._lockGetWeaponPrefabs--;
		yield break;
	}

	// Token: 0x06004482 RID: 17538 RVA: 0x0016ED88 File Offset: 0x0016CF88
	private bool _WeaponAvailable(GameObject prefab, List<string> weaponsGotInCampaign, int filterMap)
	{
		string tag = ItemDb.GetByPrefabName(prefab.name.Replace("(Clone)", string.Empty)).Tag;
		bool isMulti = Defs.isMulti;
		bool isHunger = Defs.isHunger;
		bool flag = !Defs.IsSurvival && TrainingController.TrainingCompleted && !isMulti;
		if (isMulti && filterMap == 3 && prefab.name.Equals(WeaponManager.DaterFreeWeaponPrefabName))
		{
			return true;
		}
		if (prefab.name.Replace("(Clone)", string.Empty) == WeaponManager.KnifeWN)
		{
			return true;
		}
		if (prefab.name.Replace("(Clone)", string.Empty) == WeaponManager.PistolWN && !isHunger)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.ShotgunWN) && !isHunger)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.MP5WN) && (isMulti || Defs.IsSurvival) && !isHunger)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.CampaignRifle_WN) && (isMulti || Defs.IsSurvival) && !isHunger)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.SimpleFlamethrower_WN) && (isMulti || Defs.IsSurvival) && !isHunger)
		{
			return true;
		}
		if (prefab.name.Equals(WeaponManager.Rocketnitza_WN) && (isMulti || Defs.IsSurvival) && !isHunger)
		{
			return true;
		}
		WeaponSounds component = prefab.GetComponent<WeaponSounds>();
		if (!isHunger && tag != null && TempItemsController.sharedController.ContainsItem(tag) && (filterMap == 0 || (component.filterMap != null && component.filterMap.Contains(filterMap))))
		{
			return true;
		}
		if (flag && LevelBox.weaponsFromBosses.ContainsValue(prefab.name) && weaponsGotInCampaign.Contains(prefab.name))
		{
			return true;
		}
		bool flag2 = prefab.name.Equals(WeaponManager.BugGunWN) && weaponsGotInCampaign.Contains(WeaponManager.BugGunWN);
		if (Defs.IsSurvival && TrainingController.TrainingCompleted && !isMulti && flag2)
		{
			return true;
		}
		if (!Defs.IsSurvival && TrainingController.TrainingCompleted && isMulti && !isHunger && flag2)
		{
			return true;
		}
		bool flag3 = (prefab.name.Equals(WeaponManager.SocialGunWN) && Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) > 0) || (tag != null && WeaponManager.GotchaGuns.Contains(tag) && Storager.getInt(tag, true) > 0);
		return ((Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && !isMulti) || (!Defs.IsSurvival && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && isMulti && !isHunger) || (flag && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None))) && flag3;
	}

	// Token: 0x06004483 RID: 17539 RVA: 0x0016F0D0 File Offset: 0x0016D2D0
	public static float ShotgunShotsCountModif()
	{
		return 0.6666667f;
	}

	// Token: 0x06004484 RID: 17540 RVA: 0x0016F0D8 File Offset: 0x0016D2D8
	private void _SortShopLists()
	{
		for (int i = 0; i < 6; i++)
		{
			Dictionary<string, List<GameObject>> dictionary = new Dictionary<string, List<GameObject>>();
			foreach (GameObject gameObject in this._weaponsByCat[i])
			{
				string key = WeaponUpgrades.TagOfFirstUpgrade(ItemDb.GetByPrefabName(gameObject.name.Replace("(Clone)", string.Empty)).Tag);
				if (dictionary.ContainsKey(key))
				{
					dictionary[key].Add(gameObject);
				}
				else
				{
					dictionary.Add(key, new List<GameObject>
					{
						gameObject
					});
				}
			}
			List<List<GameObject>> list = dictionary.Values.ToList<List<GameObject>>();
			foreach (List<GameObject> list2 in list)
			{
				if (list2.Count > 1)
				{
					list2.Sort(this.dpsComparer);
				}
			}
			List<List<GameObject>> list3 = new List<List<GameObject>>();
			List<List<GameObject>> list4 = new List<List<GameObject>>();
			foreach (List<GameObject> list5 in list)
			{
				string tag = WeaponUpgrades.TagOfFirstUpgrade(ItemDb.GetByPrefabName(list5[0].name.Replace("(Clone)", string.Empty)).Tag);
				((!ItemDb.IsCanBuy(tag)) ? list4 : list3).Add(list5);
			}
			Comparison<List<GameObject>> comparison = delegate(List<GameObject> leftList, List<GameObject> rightList)
			{
				if (leftList == null || rightList == null || leftList.Count < 1 || rightList.Count < 1)
				{
					return 0;
				}
				WeaponSounds component = leftList[0].GetComponent<WeaponSounds>();
				WeaponSounds component2 = rightList[0].GetComponent<WeaponSounds>();
				return WeaponManager.dpsComparerWS(component, component2);
			};
			list3.Sort(comparison);
			list4.Sort(comparison);
			List<GameObject> list6 = new List<GameObject>();
			foreach (List<GameObject> collection in list4)
			{
				list6.AddRange(collection);
			}
			foreach (List<GameObject> collection2 in list3)
			{
				list6.AddRange(collection2);
			}
			this._weaponsByCat[i] = list6;
		}
	}

	// Token: 0x06004485 RID: 17541 RVA: 0x0016F3C0 File Offset: 0x0016D5C0
	private static void InitializeRemoved150615Weapons()
	{
		WeaponManager._Removed150615_GunsPrefabNAmes = new List<string>
		{
			"Weapon20",
			"Weapon47",
			"Weapon50",
			"Weapon57",
			"Weapon95",
			"Weapon96",
			"Weapon97",
			"Weapon98",
			"Weapon101",
			"Weapon110",
			"Weapon120",
			"Weapon123",
			"Weapon129",
			"Weapon132",
			"Weapon137",
			"Weapon139",
			"Weapon165",
			"Weapon170",
			"Weapon171",
			"Weapon189",
			"Weapon190",
			"Weapon191",
			"Weapon241",
			"Weapon247",
			"Weapon94",
			"Weapon244",
			"Weapon245",
			"Weapon285",
			"Weapon289",
			"Weapon290",
			"Weapon134",
			"Weapon181",
			"Weapon182",
			"Weapon183",
			"Weapon310",
			"Weapon315",
			"Weapon316",
			"Weapon312",
			"Weapon313",
			"Weapon314",
			"Weapon284",
			"Weapon287",
			"Weapon288",
			"Weapon198",
			"Weapon199",
			"Weapon200",
			"Weapon179",
			"Weapon184",
			"Weapon236",
			"Weapon342",
			"Weapon343",
			"Weapon344",
			"Weapon166",
			"Weapon168",
			"Weapon169",
			"Weapon377",
			"Weapon378",
			"Weapon379",
			"Weapon364",
			"Weapon365",
			"Weapon366",
			"Weapon261",
			"Weapon272",
			"Weapon273",
			"Weapon345",
			"Weapon346",
			"Weapon347",
			"Weapon336",
			"Weapon337",
			"Weapon338",
			"Weapon125",
			"Weapon239",
			"Weapon240",
			"Weapon355",
			"Weapon356",
			"Weapon357",
			"Weapon164",
			"Weapon176",
			"Weapon235"
		};
		WeaponManager._Removed150615_Guns = new List<string>(WeaponManager._Removed150615_GunsPrefabNAmes.Count);
		foreach (string prefabName in WeaponManager._Removed150615_GunsPrefabNAmes)
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(prefabName);
			if (byPrefabName != null && byPrefabName.Tag != null)
			{
				WeaponManager._Removed150615_Guns.Add(byPrefabName.Tag);
			}
		}
	}

	// Token: 0x17000BAA RID: 2986
	// (get) Token: 0x06004486 RID: 17542 RVA: 0x0016F7CC File Offset: 0x0016D9CC
	public static List<string> Removed150615_PrefabNames
	{
		get
		{
			if (WeaponManager._Removed150615_Guns == null)
			{
				WeaponManager.InitializeRemoved150615Weapons();
			}
			return WeaponManager._Removed150615_GunsPrefabNAmes;
		}
	}

	// Token: 0x17000BAB RID: 2987
	// (get) Token: 0x06004487 RID: 17543 RVA: 0x0016F7E4 File Offset: 0x0016D9E4
	public static List<string> Removed150615_Guns
	{
		get
		{
			if (WeaponManager._Removed150615_Guns == null)
			{
				WeaponManager.InitializeRemoved150615Weapons();
			}
			return WeaponManager._Removed150615_Guns;
		}
	}

	// Token: 0x06004488 RID: 17544 RVA: 0x0016F7FC File Offset: 0x0016D9FC
	private void _AddWeaponToShopListsIfNeeded(GameObject w)
	{
		WeaponSounds component = w.GetComponent<WeaponSounds>();
		bool flag = false;
		bool flag2 = false;
		List<string> list = null;
		string wtag = "Undefined";
		try
		{
			wtag = ItemDb.GetByPrefabName(w.name.Replace("(Clone)", string.Empty)).Tag;
		}
		catch (UnityException exception)
		{
			Debug.LogError("Tag issue encountered.");
			Debug.LogException(exception);
		}
		foreach (List<string> list2 in WeaponUpgrades.upgrades)
		{
			if (list2.Contains(wtag))
			{
				flag2 = true;
				list = list2;
				break;
			}
		}
		if (flag2)
		{
			int num = list.IndexOf(wtag);
			if (Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[wtag]], true) == 1)
			{
				if (num == list.Count - 1)
				{
					flag = true;
				}
				else if (num < list.Count - 1 && Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list[num + 1]]], true) == 0)
				{
					flag = true;
				}
			}
			else
			{
				string text = WeaponManager.FirstTagForOurTier(wtag, null);
				if (((num > 0 && ((text != null && text.Equals(wtag)) || Storager.getInt(WeaponManager.storeIDtoDefsSNMapping[WeaponManager.tagToStoreIDMapping[list[num - 1]]], true) == 1) && component.tier < 100) || (num == 0 && text != null && text.Equals(wtag) && ExpController.Instance != null && ExpController.Instance.OurTier >= component.tier)) && (!WeaponManager.Removed150615_Guns.Contains(WeaponUpgrades.TagOfFirstUpgrade(wtag)) || WeaponManager.LastBoughtTag(wtag, null) != null))
				{
					flag = true;
				}
			}
		}
		else
		{
			Lazy<string> lazy = new Lazy<string>(delegate()
			{
				string key;
				if (!WeaponManager.tagToStoreIDMapping.TryGetValue(wtag, out key))
				{
					Debug.LogError("Weapon tag not found in tagToStoreIDMapping: " + wtag);
					return string.Empty;
				}
				string text2;
				if (!WeaponManager.storeIDtoDefsSNMapping.TryGetValue(key, out text2))
				{
					Debug.LogError("Weapon name not found in storeIDtoDefsSNMapping: " + text2);
					return string.Empty;
				}
				return text2;
			});
			flag = ((TrainingController.TrainingCompleted || (!(wtag == WeaponTags.BASIC_FLAMETHROWER_Tag) && !(wtag == WeaponTags.SignalPistol_Tag))) && ((ExpController.Instance != null && ExpController.Instance.OurTier >= component.tier) || Storager.getInt(lazy.Value, true) == 1) && (!WeaponManager.Removed150615_Guns.Contains(WeaponUpgrades.TagOfFirstUpgrade(wtag)) || WeaponManager.LastBoughtTag(wtag, null) != null));
		}
		if (flag)
		{
			try
			{
				this._weaponsByCat[component.categoryNabor - 1].Add(w);
			}
			catch (Exception arg)
			{
				if (Application.isEditor || Debug.isDebugBuild)
				{
					Debug.LogError("WeaponManager: exception: " + arg);
				}
			}
		}
	}

	// Token: 0x06004489 RID: 17545 RVA: 0x0016FB90 File Offset: 0x0016DD90
	private void AddTempGunsToShopCategoryLists(int filterMap, bool isHungry)
	{
		if (!isHungry)
		{
			if (!TrainingController.TrainingCompleted)
			{
				if (TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
				{
					return;
				}
			}
			try
			{
				IEnumerable<WeaponSounds> enumerable = from o in this.weaponsInGame.OfType<GameObject>()
				select o.GetComponent<WeaponSounds>() into ws
				where ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag) && TempItemsController.sharedController != null && TempItemsController.sharedController.ContainsItem(ItemDb.GetByPrefabName(ws.name.Replace("(Clone)", string.Empty)).Tag)
				select ws;
				if (filterMap != 0)
				{
					enumerable = from ws in enumerable
					where ws.filterMap != null && ws.filterMap.Contains(filterMap)
					select ws;
				}
				foreach (WeaponSounds weaponSounds in enumerable)
				{
					this._weaponsByCat[weaponSounds.categoryNabor - 1].Add(weaponSounds.gameObject);
				}
			}
			catch (Exception arg)
			{
				Debug.LogWarning("Exception " + arg);
			}
		}
	}

	// Token: 0x0600448A RID: 17546 RVA: 0x0016FCD4 File Offset: 0x0016DED4
	private void _InitShopCategoryLists(int filterMap = 0)
	{
		bool isMulti = Defs.isMulti;
		bool flag = isMulti && Defs.isHunger;
		bool flag2 = !Defs.IsSurvival && TrainingController.TrainingCompleted && !isMulti;
		string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
		{
			'#'
		});
		List<string> list = new List<string>();
		foreach (string item in array)
		{
			list.Add(item);
		}
		foreach (List<GameObject> list2 in this._weaponsByCat)
		{
			list2.Clear();
		}
		this.AddTempGunsToShopCategoryLists(filterMap, flag);
		if ((isMulti && !flag) || (Defs.IsSurvival && TrainingController.TrainingCompleted))
		{
			foreach (GameObject gameObject in this.weaponsInGame)
			{
				string tag = ItemDb.GetByPrefabName(gameObject.name).Tag;
				WeaponSounds component = gameObject.GetComponent<WeaponSounds>();
				if (gameObject.name == WeaponManager.DaterFreeWeaponPrefabName)
				{
					if (filterMap == 3)
					{
						this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
					}
				}
				else if (!component.campaignOnly)
				{
					if (gameObject.name.Equals(WeaponManager.AlienGunWN))
					{
						if (list.Contains(WeaponManager.AlienGunWN))
						{
						}
					}
					else if (gameObject.name.Equals(WeaponManager.BugGunWN))
					{
						if (list.Contains(WeaponManager.BugGunWN))
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (gameObject.name.Equals(WeaponManager.SocialGunWN))
					{
						if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) > 0)
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (tag != null && WeaponManager.GotchaGuns.Contains(tag))
					{
						if (Storager.getInt(tag, true) > 0)
						{
							this._weaponsByCat[component.categoryNabor - 1].Add(gameObject);
						}
					}
					else if (!ItemDb.IsTemporaryGun(tag))
					{
						this._AddWeaponToShopListsIfNeeded(gameObject);
					}
				}
			}
			this._SortShopLists();
			return;
		}
		if (flag2)
		{
			foreach (GameObject gameObject2 in this.weaponsInGame)
			{
				string tag2 = ItemDb.GetByPrefabName(gameObject2.name).Tag;
				WeaponSounds component2 = gameObject2.GetComponent<WeaponSounds>();
				if (!(gameObject2.name == WeaponManager.DaterFreeWeaponPrefabName))
				{
					if (component2.campaignOnly || gameObject2.name.Equals(WeaponManager.BugGunWN) || gameObject2.name.Equals(WeaponManager.AlienGunWN) || gameObject2.name.Equals(WeaponManager.MP5WN) || gameObject2.name.Equals(WeaponManager.CampaignRifle_WN) || gameObject2.name.Equals(WeaponManager.SimpleFlamethrower_WN) || gameObject2.name.Equals(WeaponManager.Rocketnitza_WN))
					{
						if (list.Contains(gameObject2.name))
						{
							this._weaponsByCat[component2.categoryNabor - 1].Add(gameObject2);
						}
					}
					else if (gameObject2.name.Equals(WeaponManager.SocialGunWN))
					{
						if (Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) > 0)
						{
							this._weaponsByCat[component2.categoryNabor - 1].Add(gameObject2);
						}
					}
					else if (tag2 != null && WeaponManager.GotchaGuns.Contains(tag2))
					{
						if (Storager.getInt(tag2, true) > 0)
						{
							this._weaponsByCat[component2.categoryNabor - 1].Add(gameObject2);
						}
					}
					else if (!ItemDb.IsTemporaryGun(tag2))
					{
						this._AddWeaponToShopListsIfNeeded(gameObject2);
					}
				}
			}
			this._SortShopLists();
			return;
		}
		if (flag)
		{
			foreach (GameObject gameObject3 in this.weaponsInGame)
			{
				if (gameObject3.name.Equals(WeaponManager.KnifeWN))
				{
					this._AddWeaponToShopListsIfNeeded(gameObject3);
					break;
				}
			}
			this._SortShopLists();
			return;
		}
	}

	// Token: 0x0600448B RID: 17547 RVA: 0x001701CC File Offset: 0x0016E3CC
	private static bool OldChainThatAlwaysShownFromStart(string tg)
	{
		string value = WeaponUpgrades.TagOfFirstUpgrade(tg);
		return WeaponManager.oldTags.Contains(value);
	}

	// Token: 0x0600448C RID: 17548 RVA: 0x001701EC File Offset: 0x0016E3EC
	private static void SaveFirstTagsToDisc()
	{
		Storager.setString("FirstTagsForOurTier", Rilisoft.MiniJson.Json.Serialize(WeaponManager.firstTagsWithRespecToOurTier), false);
	}

	// Token: 0x0600448D RID: 17549 RVA: 0x00170204 File Offset: 0x0016E404
	private void FixFirstTags()
	{
		try
		{
			bool flag = false;
			foreach (List<string> list in WeaponUpgrades.upgrades)
			{
				if (list == null || list.Count != 0)
				{
					string text = list.First<string>();
					ItemRecord byTag = ItemDb.GetByTag(text);
					if (byTag != null && byTag.StorageId != null && Storager.getInt(byTag.StorageId, true) != 0)
					{
						string text2 = WeaponManager.LastBoughtTag(text, list);
						if (text2 != null)
						{
							string item = WeaponManager.FirstTagForOurTier(text, list);
							int num = list.IndexOf(text2);
							int num2 = list.IndexOf(item);
							if (num < num2)
							{
								flag = true;
								WeaponManager.firstTagsWithRespecToOurTier[text] = text2;
							}
						}
					}
				}
			}
			if (flag)
			{
				WeaponManager.SaveFirstTagsToDisc();
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in FixFirstTags: " + arg);
		}
	}

	// Token: 0x0600448E RID: 17550 RVA: 0x00170340 File Offset: 0x0016E540
	private static void InitFirstTagsData()
	{
		if (!Storager.hasKey("FirstTagsForOurTier"))
		{
			Storager.setString("FirstTagsForOurTier", "{}", false);
		}
		string @string = Storager.getString("FirstTagsForOurTier", false);
		try
		{
			Dictionary<string, object> dictionary = Rilisoft.MiniJson.Json.Deserialize(@string) as Dictionary<string, object>;
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				WeaponManager.firstTagsWithRespecToOurTier.Add(keyValuePair.Key, (string)keyValuePair.Value);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return;
		}
		bool flag = false;
		int ourTier = ExpController.GetOurTier();
		foreach (List<string> list in WeaponUpgrades.upgrades)
		{
			if (list.Count != 0)
			{
				if (!WeaponManager.firstTagsWithRespecToOurTier.ContainsKey(list[0]))
				{
					flag = true;
					if (WeaponManager.OldChainThatAlwaysShownFromStart(list[0]))
					{
						WeaponManager.firstTagsWithRespecToOurTier.Add(list[0], list[0]);
					}
					else
					{
						List<WeaponSounds> list2 = (from tg in list
						select ItemDb.GetWeaponInfo(tg)).ToList<WeaponSounds>();
						bool flag2 = false;
						for (int i = 0; i < list.Count; i++)
						{
							if (list2[i] != null && list2[i].tier > ourTier)
							{
								WeaponManager.firstTagsWithRespecToOurTier.Add(list[0], list[Math.Max(0, i - 1)]);
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							WeaponManager.firstTagsWithRespecToOurTier.Add(list[0], list[list.Count - 1]);
						}
					}
				}
			}
		}
		if (flag)
		{
			WeaponManager.SaveFirstTagsToDisc();
		}
	}

	// Token: 0x0600448F RID: 17551 RVA: 0x001705A8 File Offset: 0x0016E7A8
	public static string FirstTagForOurTier(string tg, List<string> upgradesHint = null)
	{
		if (tg == null)
		{
			return null;
		}
		if (!WeaponManager.firstTagsForTiersInitialized)
		{
			WeaponManager.InitFirstTagsData();
			WeaponManager.firstTagsForTiersInitialized = true;
		}
		List<string> list = upgradesHint ?? WeaponUpgrades.ChainForTag(tg);
		if (list != null && list.Count > 0)
		{
			string result = null;
			WeaponManager.firstTagsWithRespecToOurTier.TryGetValue(list[0], out result);
			return result;
		}
		return null;
	}

	// Token: 0x06004490 RID: 17552 RVA: 0x0017060C File Offset: 0x0016E80C
	private void _UpdateShopCategList(Weapon w)
	{
		WeaponSounds component = w.weaponPrefab.GetComponent<WeaponSounds>();
		string tag = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
		if (WeaponManager.tagToStoreIDMapping.ContainsKey(tag))
		{
			bool flag = false;
			List<string> list = null;
			foreach (List<string> list2 in WeaponUpgrades.upgrades)
			{
				if (list2.Contains(tag))
				{
					list = list2;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				int num = list.IndexOf(ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag);
				int num2 = -1;
				foreach (GameObject gameObject in this._weaponsByCat[component.categoryNabor - 1])
				{
					if (gameObject.name.Replace("(Clone)", string.Empty) == w.weaponPrefab.name.Replace("(Clone)", string.Empty))
					{
						num2 = this._weaponsByCat[component.categoryNabor - 1].IndexOf(gameObject);
						break;
					}
				}
				if (num < list.Count - 1)
				{
					GameObject item = null;
					try
					{
						string path = string.Format("Weapons/{0}", ItemDb.GetByTag(list[num + 1]).PrefabName);
						item = Resources.Load<GameObject>(path);
					}
					catch (Exception ex)
					{
						Debug.LogErrorFormat("Exception in setting newW in _UpdateShopCategList: {0}", new object[]
						{
							ex
						});
					}
					if (num2 != -1)
					{
						string tag2 = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
						string text = WeaponManager.FirstTagForOurTier(tag2, null);
						if (num > 0 && (text == null || !text.Equals(tag2)))
						{
							this._weaponsByCat[component.categoryNabor - 1].RemoveAt(num2 - 1);
						}
						this._weaponsByCat[component.categoryNabor - 1].Insert(num2, item);
					}
					else
					{
						Debug.LogWarning("_UpdateShopCategList: prevInd = -1   ws.categoryNabor - 1: " + (component.categoryNabor - 1));
					}
				}
				else
				{
					string tag3 = ItemDb.GetByPrefabName(w.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
					string text2 = WeaponManager.FirstTagForOurTier(tag3, null);
					if (text2 == null || !text2.Equals(tag3))
					{
						this._weaponsByCat[component.categoryNabor - 1].RemoveAt(num2 - 1);
					}
				}
			}
		}
		else
		{
			this._weaponsByCat[component.categoryNabor - 1].Add(w.weaponPrefab);
		}
		this._SortShopLists();
	}

	// Token: 0x06004491 RID: 17553 RVA: 0x00170968 File Offset: 0x0016EB68
	public void Reset(int filterMap = 0)
	{
		IEnumerator enumerator = this.ResetCoroutine(filterMap, true);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
		}
	}

	// Token: 0x06004492 RID: 17554 RVA: 0x00170998 File Offset: 0x0016EB98
	private static List<string> AllWeaponSetsSettingNames()
	{
		return new List<string>
		{
			Defs.CampaignWSSN
		}.Concat(from fms in WeaponManager.WeaponSetSettingNamesForFilterMaps.Values
		select fms.settingName).ToList<string>();
	}

	// Token: 0x06004493 RID: 17555 RVA: 0x001709F0 File Offset: 0x0016EBF0
	private void Update()
	{
	}

	// Token: 0x06004494 RID: 17556 RVA: 0x001709F4 File Offset: 0x0016EBF4
	public bool ActualizeEquippedWeapons()
	{
		bool result = false;
		List<string> list = WeaponManager.AllWeaponSetsSettingNames();
		foreach (string sn in list)
		{
			string text = this.LoadWeaponSet(sn);
			string[] array = text.Split(new char[]
			{
				'#'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				if (!string.IsNullOrEmpty(text2))
				{
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(text2);
					if (byPrefabName != null && byPrefabName.Tag != null && byPrefabName.CanBuy)
					{
						string text3 = WeaponManager.LastBoughtTag(byPrefabName.Tag, null);
						if (text3 != null)
						{
							if (!(text3 == byPrefabName.Tag))
							{
								ItemRecord byTag = ItemDb.GetByTag(text3);
								if (byTag != null && byTag.PrefabName != null)
								{
									this.SaveWeaponSet(sn, byTag.PrefabName, i);
									result = true;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06004495 RID: 17557 RVA: 0x00170B38 File Offset: 0x0016ED38
	private bool ReequipItemsAfterCloudSync()
	{
		bool flag = WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null;
		List<ShopNGUIController.CategoryNames> list = new List<ShopNGUIController.CategoryNames>();
		foreach (ShopNGUIController.CategoryNames categoryNames in new ShopNGUIController.CategoryNames[]
		{
			ShopNGUIController.CategoryNames.ArmorCategory,
			ShopNGUIController.CategoryNames.BootsCategory,
			ShopNGUIController.CategoryNames.CapesCategory,
			ShopNGUIController.CategoryNames.HatsCategory,
			ShopNGUIController.CategoryNames.MaskCategory
		})
		{
			string text = ShopNGUIController.NoneEquippedForWearCategory(categoryNames);
			string @string = Storager.getString(ShopNGUIController.SnForWearCategory(categoryNames), false);
			if (@string != null && text != null && !@string.Equals(text) && @string != "Armor_Novice")
			{
				string text2 = WeaponManager.LastBoughtTag(@string, null);
				if (!string.IsNullOrEmpty(text2) && text2 != @string)
				{
					ShopNGUIController.EquipWearInCategoryIfNotEquiped(text2, categoryNames, flag);
					list.Add(categoryNames);
				}
			}
		}
		GadgetsInfo.ActualizeEquippedGadgets();
		bool result = this.ActualizeEquippedWeapons();
		if (flag)
		{
			if (this.myPlayerMoveC.mySkinName != null)
			{
				this.myPlayerMoveC.mySkinName.SetWearVisible(null);
				if (list.Contains(ShopNGUIController.CategoryNames.ArmorCategory))
				{
					this.myPlayerMoveC.mySkinName.SetArmor(null);
				}
				if (list.Contains(ShopNGUIController.CategoryNames.BootsCategory))
				{
					this.myPlayerMoveC.mySkinName.SetBoots(null);
				}
				if (list.Contains(ShopNGUIController.CategoryNames.CapesCategory))
				{
					this.myPlayerMoveC.mySkinName.SetCape(null);
				}
				if (list.Contains(ShopNGUIController.CategoryNames.HatsCategory))
				{
					this.myPlayerMoveC.mySkinName.SetHat(null);
				}
				if (list.Contains(ShopNGUIController.CategoryNames.MaskCategory))
				{
					this.myPlayerMoveC.mySkinName.SetMask(null);
				}
			}
		}
		else if (PersConfigurator.currentConfigurator != null && list.Count > 0)
		{
			PersConfigurator.currentConfigurator.UpdateWear();
		}
		return result;
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x00170D1C File Offset: 0x0016EF1C
	private void ReequipWeaponsForGuiAndRpcAndUpdateIcons()
	{
		if (this.myPlayerMoveC != null && ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.equipAction != null)
		{
			foreach (object obj in this.playerWeapons)
			{
				Weapon weapon = (Weapon)obj;
				if (weapon != null && weapon.weaponPrefab != null)
				{
					ShopNGUIController.sharedShop.equipAction(ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag);
				}
				if (ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateIcons(false);
				}
			}
		}
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x00170E14 File Offset: 0x0016F014
	private Weapon AddWeaponWithTagToAllAvailable(string tagToAdd)
	{
		Weapon result;
		try
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + ItemDb.GetByTag(tagToAdd).PrefabName);
			Weapon weapon = new Weapon();
			weapon.weaponPrefab = weaponSounds.gameObject;
			weapon.currentAmmoInBackpack = weaponSounds.InitialAmmoWithEffectsApplied;
			weapon.currentAmmoInClip = weaponSounds.ammoInClip;
			this.allAvailablePlayerWeapons.Add(weapon);
			result = weapon;
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in AddWeaponWithTagToAllAvailable: " + arg);
			result = null;
		}
		return result;
	}

	// Token: 0x17000BAC RID: 2988
	// (get) Token: 0x06004498 RID: 17560 RVA: 0x00170EBC File Offset: 0x0016F0BC
	public int CurrentFilterMap
	{
		get
		{
			return this._currentFilterMap;
		}
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x00170EC4 File Offset: 0x0016F0C4
	private static void GivePreviousUpgradesOfCrystalSword()
	{
		try
		{
			string storageId = ItemDb.GetByTag(WeaponTags.CrystalSwordTag).StorageId;
			if (Storager.getInt(storageId, true) == 1)
			{
				Storager.setInt(ItemDb.GetByTag(WeaponTags.IronSwordTag).StorageId, 1, true);
				Storager.setInt(ItemDb.GetByTag(WeaponTags.GoldenSwordTag).StorageId, 1, true);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in giving previous upgrades of crystal sword in Reset: " + arg);
		}
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x00170F54 File Offset: 0x0016F154
	private IEnumerator DoMigrationsIfNeeded()
	{
		if (!WeaponManager._migrationsCompletedAtThisLaunch)
		{
			bool needUpdate = !Storager.hasKey(Defs.Weapons800to801);
			if (needUpdate)
			{
				yield return base.StartCoroutine(this.UpdateWeapons800To801());
			}
			needUpdate = !Storager.hasKey(Defs.FixWeapons911);
			if (needUpdate)
			{
				this.FixWeaponsDueToCategoriesMoved911();
				yield return null;
			}
			needUpdate = !Storager.hasKey(Defs.ReturnAlienGun930);
			if (needUpdate)
			{
				this.ReturnAlienGunToCampaignBack();
				yield return null;
			}
			WeaponManager._migrationsCompletedAtThisLaunch = true;
		}
		yield break;
	}

	// Token: 0x0600449B RID: 17563 RVA: 0x00170F70 File Offset: 0x0016F170
	public IEnumerator ResetCoroutine(int filterMap = 0, bool doIosCloudSync = true)
	{
		if (this._resetLock)
		{
			Debug.LogWarning("Simultaneous executing of WeaponManagers ResetCoroutines");
		}
		this._resetLock = true;
		using (new ActionDisposable(delegate()
		{
			this._resetLock = false;
		}))
		{
			bool newWeaponsComeFromCloudInWeaponSet = false;
			if (doIosCloudSync)
			{
				List<string> weaponsForWhichSetRememberedTier = new List<string>();
				bool armorArmy1Comes;
				Storager.SynchronizeIosWithCloud(ref weaponsForWhichSetRememberedTier, out armorArmy1Comes);
				WeaponManager.GivePreviousUpgradesOfCrystalSword();
				int levelBefore = (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel;
				WeaponManager.RefreshExpControllers();
				ExperienceController.SendAnalyticsForLevelsFromCloud(levelBefore);
				WeaponManager.RefreshLevelAndSetRememberedTiersFromCloud(weaponsForWhichSetRememberedTier);
				if ((ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel > 1) || armorArmy1Comes)
				{
					if (!TrainingController.TrainingCompleted)
					{
						TrainingController.OnGetProgress();
					}
					else if (Storager.getInt("Training.ShouldRemoveNoviceArmorInShopKey", false) == 1 && armorArmy1Comes)
					{
						if (ShopNGUIController.NoviceArmorAvailable)
						{
							ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
							ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
						}
						Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
					}
				}
				newWeaponsComeFromCloudInWeaponSet = this.ReequipItemsAfterCloudSync();
			}
			this.FixFirstTags();
			GadgetsInfo.FixFirstsForOurTier();
			this._currentFilterMap = filterMap;
			bool isMulti = Defs.isMulti;
			bool isHungry = Defs.isHunger;
			if (!isHungry)
			{
				if (!this._initialized)
				{
					yield return base.StartCoroutine(this.GetWeaponPrefabsCoroutine(filterMap));
				}
				else
				{
					this.GetWeaponPrefabs(filterMap);
				}
				yield return null;
			}
			yield return null;
			yield return base.StartCoroutine(this.DoMigrationsIfNeeded());
			this.allAvailablePlayerWeapons.Clear();
			this.CurrentWeaponIndex = 0;
			string[] _arr = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
			{
				'#'
			});
			List<string> weaponsGotInCampaign = new List<string>();
			foreach (string s in _arr)
			{
				weaponsGotInCampaign.Add(s);
			}
			foreach (GameObject prefab in this.weaponsInGame)
			{
				if (this._WeaponAvailable(prefab, weaponsGotInCampaign, filterMap))
				{
					Weapon pistol = new Weapon();
					pistol.weaponPrefab = prefab;
					pistol.currentAmmoInBackpack = pistol.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
					pistol.currentAmmoInClip = pistol.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
					this.allAvailablePlayerWeapons.Add(pistol);
				}
			}
			yield return null;
			if ((isMulti && isHungry) || (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None))
			{
				this.SetWeaponsSet(filterMap);
				this._InitShopCategoryLists(filterMap);
				this.UpdateFilteredShopLists();
				this.CurrentWeaponIndex = 0;
				if (newWeaponsComeFromCloudInWeaponSet)
				{
					this.ReequipWeaponsForGuiAndRpcAndUpdateIcons();
				}
				yield break;
			}
			HashSet<string> addedWeaponTags = new HashSet<string>();
			Func<string, bool> weaponWithTagIsBought = delegate(string tg)
			{
				ItemRecord byTag = ItemDb.GetByTag(tg);
				if (byTag != null)
				{
					if (byTag.TemporaryGun)
					{
						return false;
					}
					if (byTag.StorageId != null)
					{
						return Storager.getInt(byTag.StorageId, true) > 0;
					}
					Debug.LogError("lastBoughtUpgrade: StorageId returns null for tag " + tg);
				}
				else
				{
					Debug.LogError("lastBoughtUpgrade: GetByTag returns null for tag " + tg);
				}
				return false;
			};
			try
			{
				List<List<string>> allUpgrades = WeaponUpgrades.upgrades;
				foreach (List<string> weaponUpgrades in allUpgrades)
				{
					addedWeaponTags.UnionWith(weaponUpgrades);
					string lastBoughtUpgrade = weaponUpgrades.FindLast((string tg) => weaponWithTagIsBought(tg));
					if (lastBoughtUpgrade == null && weaponUpgrades.Count > 0 && this.IsAvailableTryGun(weaponUpgrades[0]))
					{
						lastBoughtUpgrade = weaponUpgrades[0];
					}
					if (lastBoughtUpgrade != null)
					{
						this.AddWeaponWithTagToAllAvailable(lastBoughtUpgrade);
					}
				}
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogError("lastBoughtUpgrade: Exception " + e);
			}
			yield return null;
			try
			{
				List<string> canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(false).Except(addedWeaponTags).ToList<string>();
				for (int i = 0; i < canBuyWeaponTags.Count; i++)
				{
					if (weaponWithTagIsBought(canBuyWeaponTags[i]) || this.IsAvailableTryGun(canBuyWeaponTags[i]))
					{
						this.AddWeaponWithTagToAllAvailable(canBuyWeaponTags[i]);
					}
				}
			}
			catch (Exception ex2)
			{
				Exception e2 = ex2;
				Debug.LogError("lastBoughtUpgrade: Exception " + e2);
			}
			yield return null;
			this.SetWeaponsSet(filterMap);
			this._InitShopCategoryLists(filterMap);
			this.UpdateFilteredShopLists();
			this.CurrentWeaponIndex = 0;
			if (newWeaponsComeFromCloudInWeaponSet)
			{
				this.ReequipWeaponsForGuiAndRpcAndUpdateIcons();
			}
		}
		yield break;
	}

	// Token: 0x0600449C RID: 17564 RVA: 0x00170FA8 File Offset: 0x0016F1A8
	public bool AddWeapon(GameObject weaponPrefab, out int score)
	{
		score = 0;
		WeaponSounds component = weaponPrefab.GetComponent<WeaponSounds>();
		if (component != null && WeaponManager.sharedManager != null && !component.IsAvalibleFromFilter(WeaponManager.sharedManager.CurrentFilterMap))
		{
			return false;
		}
		int num;
		if (Defs.isHunger && component != null && int.TryParse(component.nameNoClone().Substring("Weapon".Length), out num) && num != 9 && !ChestController.weaponForHungerGames.Contains(num))
		{
			return false;
		}
		bool flag = false;
		foreach (object obj in this.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			if (weapon.weaponPrefab.name.Replace("(Clone)", string.Empty) == weaponPrefab.name.Replace("(Clone)", string.Empty))
			{
				int idx = this.allAvailablePlayerWeapons.IndexOf(weapon);
				if (!this.AddAmmo(idx))
				{
					score += Defs.ScoreForSurplusAmmo;
				}
				if (!ItemDb.IsTemporaryGun(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag) && !this.IsAvailableTryGun(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag))
				{
					return false;
				}
				flag = true;
			}
		}
		Weapon weapon2 = new Weapon();
		weapon2.weaponPrefab = weaponPrefab;
		weapon2.currentAmmoInBackpack = weapon2.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
		weapon2.currentAmmoInClip = weapon2.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
		if (!flag)
		{
			this.allAvailablePlayerWeapons.Add(weapon2);
		}
		else
		{
			int num2 = -1;
			foreach (object obj2 in this.allAvailablePlayerWeapons)
			{
				Weapon weapon3 = (Weapon)obj2;
				if (weapon3.weaponPrefab.name.Equals(weaponPrefab.name))
				{
					num2 = this.allAvailablePlayerWeapons.IndexOf(weapon3);
					break;
				}
			}
			if (num2 > -1 && num2 < this.allAvailablePlayerWeapons.Count)
			{
				this.allAvailablePlayerWeapons[num2] = weapon2;
			}
		}
		string tag = ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag;
		int num3 = this._RemovePrevVersionsOfUpgrade(tag);
		bool flag2 = true;
		List<string> list = new List<string>
		{
			WeaponManager.CampaignRifle_WN,
			WeaponManager.AlienGunWN,
			WeaponManager.SimpleFlamethrower_WN,
			WeaponManager.BugGunWN,
			WeaponManager.Rocketnitza_WN
		};
		WeaponSounds weaponSettingsOfNewWeapon = weapon2.weaponPrefab.GetComponent<WeaponSounds>();
		if (!weaponSettingsOfNewWeapon.campaignOnly && !(weapon2.weaponPrefab.name.Replace("(Clone)", string.Empty) == WeaponManager.MP5WN))
		{
			if (!list.Contains(weapon2.weaponPrefab.name.Replace("(Clone)", string.Empty)))
			{
				goto IL_4ED;
			}
		}
		try
		{
			if (this.CurrentWeaponIndex >= 0 && this.CurrentWeaponIndex < this.playerWeapons.Count)
			{
				Weapon weapon4 = this.playerWeapons[this.CurrentWeaponIndex] as Weapon;
				if (weapon4 != null)
				{
					GameObject weaponPrefab2 = weapon4.weaponPrefab;
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponPrefab2.nameNoClone());
					if (byPrefabName != null && (WeaponManager.tagToStoreIDMapping.ContainsKey(byPrefabName.Tag) || WeaponManager.GotchaGuns.Contains(byPrefabName.Tag) || byPrefabName.Tag == WeaponTags.FriendsUzi_Tag))
					{
						flag2 = false;
					}
				}
			}
			WeaponSounds weaponSounds = (from w in this.playerWeapons.OfType<Weapon>()
			select w.weaponPrefab.GetComponent<WeaponSounds>()).FirstOrDefault((WeaponSounds ws) => ws.categoryNabor == weaponSettingsOfNewWeapon.categoryNabor);
			if (weaponSounds != null)
			{
				ItemRecord byPrefabName2 = ItemDb.GetByPrefabName(weaponSounds.nameNoClone());
				if (byPrefabName2 != null && (WeaponManager.tagToStoreIDMapping.ContainsKey(byPrefabName2.Tag) || WeaponManager.GotchaGuns.Contains(byPrefabName2.Tag) || byPrefabName2.Tag == WeaponTags.FriendsUzi_Tag))
				{
					flag2 = false;
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in finding weapon of checking notBoughtToCampaign: " + arg);
			flag2 = false;
		}
		IL_4ED:
		if (flag2)
		{
			this.EquipWeapon(weapon2, true, false);
			this.SaveWeaponAsLastUsed(this.CurrentWeaponIndex);
		}
		this._UpdateShopCategList(weapon2);
		this.UpdateFilteredShopLists();
		return flag2;
	}

	// Token: 0x0600449D RID: 17565 RVA: 0x00171520 File Offset: 0x0016F720
	private int _RemovePrevVersionsOfUpgrade(string tg)
	{
		int num = 0;
		foreach (List<string> list in WeaponUpgrades.upgrades)
		{
			int num2 = list.IndexOf(tg);
			if (num2 != -1)
			{
				for (int i = 0; i < num2; i++)
				{
					List<Weapon> list2 = new List<Weapon>();
					for (int j = 0; j < this.allAvailablePlayerWeapons.Count; j++)
					{
						Weapon weapon = this.allAvailablePlayerWeapons[j] as Weapon;
						if (ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag.Equals(list[i]))
						{
							list2.Add(weapon);
						}
					}
					for (int k = 0; k < list2.Count; k++)
					{
						this.allAvailablePlayerWeapons.Remove(list2[k]);
					}
					num += list2.Count;
				}
				break;
			}
		}
		return num;
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x00171660 File Offset: 0x0016F860
	public GameObject GetPrefabByTag(string weaponTag)
	{
		if (weaponTag.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("GetPrefabByTag: weaponTag.IsNullOrEmpty()", new object[0]);
			return null;
		}
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null)
		{
			Debug.LogErrorFormat("GetPrefabByTag: rec == null, weaponTag = {0}", new object[]
			{
				weaponTag
			});
			return null;
		}
		if (byTag.PrefabName.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("GetPrefabByTag: rec.PrefabName.IsNullOrEmpty(), weaponTag = {0}", new object[]
			{
				weaponTag
			});
			return null;
		}
		GameObject gameObject = Resources.Load<GameObject>(string.Format("Weapons/{0}", byTag.PrefabName));
		if (gameObject == null)
		{
			Debug.LogErrorFormat("GetPrefabByTag: prefab == null, weaponTag = {0}", new object[]
			{
				weaponTag
			});
			return null;
		}
		return gameObject;
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x0017170C File Offset: 0x0016F90C
	public bool AddAmmo(int idx = -1)
	{
		if (idx == -1)
		{
			idx = this.allAvailablePlayerWeapons.IndexOf(this.playerWeapons[this.CurrentWeaponIndex]);
		}
		if (this.allAvailablePlayerWeapons[idx] == this.playerWeapons[this.CurrentWeaponIndex] && this.currentWeaponSounds.isMelee && !this.currentWeaponSounds.isShotMelee)
		{
			return false;
		}
		Weapon weapon = (Weapon)this.allAvailablePlayerWeapons[idx];
		WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
		if (weapon.currentAmmoInBackpack < component.MaxAmmoWithEffectApplied)
		{
			weapon.currentAmmoInBackpack += ((!(this.currentWeaponSounds != null) || (!this.currentWeaponSounds.isShotMelee && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon335") && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon353") && !(this.currentWeaponSounds.name.Replace("(Clone)", string.Empty) == "Weapon354"))) ? component.ammoInClip : component.ammoForBonusShotMelee);
			if (weapon.currentAmmoInBackpack > component.MaxAmmoWithEffectApplied)
			{
				weapon.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
			}
			return true;
		}
		return false;
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x0017188C File Offset: 0x0016FA8C
	public bool AddAmmoForAllGuns()
	{
		bool result = false;
		for (int i = 0; i < this.playerWeapons.Count; i++)
		{
			Weapon weapon = (Weapon)this.playerWeapons[i];
			WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			if (!component.isMelee || component.isShotMelee)
			{
				if (weapon.currentAmmoInBackpack < component.MaxAmmoWithEffectApplied)
				{
					weapon.currentAmmoInBackpack += ((!(component != null) || (!component.isShotMelee && !(component.name.Replace("(Clone)", string.Empty) == "Weapon335") && !(component.name.Replace("(Clone)", string.Empty) == "Weapon353") && !(component.name.Replace("(Clone)", string.Empty) == "Weapon354"))) ? component.ammoInClip : component.ammoForBonusShotMelee);
					if (weapon.currentAmmoInBackpack > component.MaxAmmoWithEffectApplied)
					{
						weapon.currentAmmoInBackpack = component.MaxAmmoWithEffectApplied;
					}
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x001719C4 File Offset: 0x0016FBC4
	public void SetMaxAmmoFrAllWeapons()
	{
		foreach (object obj in this.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			weapon.currentAmmoInClip = weapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			weapon.currentAmmoInBackpack = weapon.weaponPrefab.GetComponent<WeaponSounds>().MaxAmmoWithEffectApplied;
		}
	}

	// Token: 0x17000BAD RID: 2989
	// (get) Token: 0x060044A2 RID: 17570 RVA: 0x00171A58 File Offset: 0x0016FC58
	public bool Initialized
	{
		get
		{
			return this._initialized;
		}
	}

	// Token: 0x060044A3 RID: 17571 RVA: 0x00171A60 File Offset: 0x0016FC60
	private void Awake()
	{
		using (new ScopeLogger("WeaponManager.Awake()", Defs.IsDeveloperBuild))
		{
			if (Storager.getInt("WeaponManager_SniperCategoryAddedToWeaponSetsKey", false) == 0)
			{
				string wssn;
				foreach (string wssn2 in WeaponManager.AllWeaponSetsSettingNames())
				{
					wssn = wssn2;
					try
					{
						if (Storager.hasKey(wssn))
						{
							string weaponSet = Storager.getString(wssn, false);
							if (weaponSet == null)
							{
								Debug.LogError("Adding sniper category to weapon sets error: weaponSet == null  wssn = " + wssn);
							}
							int num = weaponSet.LastIndexOf("#");
							if (num == -1)
							{
								Debug.LogError("Adding sniper category to weapon sets error: lastIndexOfHash == -1  wssn = " + wssn + "  weaponSet = " + weaponSet);
							}
							weaponSet = weaponSet.Insert(num, "#");
							string[] splittedWeaponSet = weaponSet.Split(new char[]
							{
								'#'
							});
							if (splittedWeaponSet == null)
							{
								Debug.LogError("Adding sniper category to weapon sets error: splittedWeaponSet == null  wssn = " + wssn + "  weaponSet = " + weaponSet);
							}
							bool flag = true;
							if (splittedWeaponSet.Length > 6)
							{
								Debug.LogError("Adding sniper category to weapon sets error: splittedWeaponSet.Length > NumOfWeaponCategories  wssn = " + wssn + "  weaponSet = " + weaponSet);
								Storager.setString(wssn, this.DefaultSetForWeaponSetSettingName(wssn), false);
								flag = false;
							}
							if (splittedWeaponSet.Length < 6)
							{
								Debug.LogError("Adding sniper category to weapon sets error: splittedWeaponSet.Length < NumOfWeaponCategories  wssn = " + wssn + "  weaponSet = " + weaponSet);
								Storager.setString(wssn, this.DefaultSetForWeaponSetSettingName(wssn), false);
								flag = false;
							}
							if (flag)
							{
								for (int i = 0; i < splittedWeaponSet.Length; i++)
								{
									if (splittedWeaponSet[i] == null)
									{
										splittedWeaponSet[i] = string.Empty;
									}
								}
								Dictionary<ShopNGUIController.CategoryNames, string> dictionary = new Dictionary<ShopNGUIController.CategoryNames, string>();
								for (int j = 0; j < splittedWeaponSet.Length; j++)
								{
									if (splittedWeaponSet[j] != null && WeaponManager.weaponsMovedToSniperCategory.Contains(splittedWeaponSet[j]))
									{
										dictionary.Add((ShopNGUIController.CategoryNames)j, splittedWeaponSet[j]);
										splittedWeaponSet[j] = string.Empty;
										if (j == 3)
										{
											if (wssn == Defs.MultiplayerWSSN)
											{
												splittedWeaponSet[j] = WeaponManager.SimpleFlamethrower_WN;
											}
											else if (wssn == Defs.CampaignWSSN)
											{
												Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars = CampaignProgress.boxesLevelsAndStars;
												Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
												bool flag2 = false;
												if (boxesLevelsAndStars.TryGetValue("minecraft", out dictionary2) && dictionary2.ContainsKey("Maze"))
												{
													splittedWeaponSet[j] = WeaponManager.SimpleFlamethrower_WN;
													flag2 = true;
												}
												if (!flag2 && boxesLevelsAndStars.TryGetValue("Real", out dictionary2) && dictionary2.ContainsKey("Jail"))
												{
													splittedWeaponSet[j] = WeaponManager.MachinegunWN;
												}
											}
										}
										else if (j == 0)
										{
											if (wssn == Defs.MultiplayerWSSN)
											{
												splittedWeaponSet[j] = WeaponManager.MP5WN;
											}
											else if (wssn == Defs.CampaignWSSN)
											{
												splittedWeaponSet[j] = "Weapon2";
											}
											else if (wssn == Defs.DaterWSSN)
											{
												splittedWeaponSet[j] = string.Empty;
											}
										}
									}
								}
								int newSniperIndex = 4;
								Action<string> action = delegate(string weaponName)
								{
									if (splittedWeaponSet.Length > newSniperIndex)
									{
										splittedWeaponSet[newSniperIndex] = weaponName;
									}
									else
									{
										Debug.LogError(string.Concat(new object[]
										{
											"Adding sniper category to weapon sets error: splittedWeaponSet.Length > newSniperIndex    newSniperIndex: ",
											newSniperIndex,
											"   wssn = ",
											wssn,
											"   weaponSet = ",
											weaponSet
										}));
									}
								};
								if (dictionary.Values.Count > 0)
								{
									action(dictionary.Values.FirstOrDefault<string>() ?? string.Empty);
								}
								else if (wssn == Defs.MultiplayerWSSN)
								{
									action(WeaponManager.CampaignRifle_WN);
								}
								else if (wssn == Defs.CampaignWSSN)
								{
									Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars2 = CampaignProgress.boxesLevelsAndStars;
									Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
									if (boxesLevelsAndStars2.TryGetValue("minecraft", out dictionary3) && dictionary3.ContainsKey("Utopia"))
									{
										action(WeaponManager.CampaignRifle_WN);
									}
								}
								if (splittedWeaponSet[3] == "Weapon317" || splittedWeaponSet[3] == "Weapon318" || splittedWeaponSet[3] == "Weapon319")
								{
									if (wssn == Defs.MultiplayerWSSN)
									{
										splittedWeaponSet[3] = WeaponManager.SimpleFlamethrower_WN;
									}
									else if (wssn == Defs.CampaignWSSN)
									{
										Dictionary<string, Dictionary<string, int>> boxesLevelsAndStars3 = CampaignProgress.boxesLevelsAndStars;
										Dictionary<string, int> dictionary4 = new Dictionary<string, int>();
										bool flag3 = false;
										if (boxesLevelsAndStars3.TryGetValue("minecraft", out dictionary4) && dictionary4.ContainsKey("Maze"))
										{
											splittedWeaponSet[3] = WeaponManager.SimpleFlamethrower_WN;
											flag3 = true;
										}
										if (!flag3 && boxesLevelsAndStars3.TryGetValue("Real", out dictionary4) && dictionary4.ContainsKey("Jail"))
										{
											splittedWeaponSet[3] = WeaponManager.MachinegunWN;
										}
									}
								}
								Storager.setString(wssn, string.Join("#", splittedWeaponSet), false);
							}
						}
						else
						{
							Storager.setString(wssn, this.DefaultSetForWeaponSetSettingName(wssn), false);
						}
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Exceptio in foreach (var wssn in AllWeaponSetsSettingNames())  wssn = ",
							wssn,
							"   exception: ",
							ex
						}));
						try
						{
							Storager.setString(wssn, this.DefaultSetForWeaponSetSettingName(wssn), false);
						}
						catch (Exception ex2)
						{
							Debug.LogError(string.Concat(new object[]
							{
								"Exceptio in Storager.setString (wssn, DefaultSetForWeaponSetSettingName(wssn),false);  wssn = ",
								wssn,
								"   exception: ",
								ex2
							}));
						}
					}
				}
				Storager.setInt("WeaponManager_SniperCategoryAddedToWeaponSetsKey", 1, false);
			}
			if (!Storager.hasKey("WeaponManager.LastUsedWeaponsKey"))
			{
				this.SaveLastUsedWeapons();
			}
			else
			{
				try
				{
					Dictionary<string, object> dictionary5 = Rilisoft.MiniJson.Json.Deserialize(Storager.getString("WeaponManager.LastUsedWeaponsKey", false)) as Dictionary<string, object>;
					foreach (string key in dictionary5.Keys)
					{
						this.lastUsedWeaponsForFilterMaps[key] = (int)((long)dictionary5[key]);
					}
				}
				catch (Exception arg)
				{
					Debug.LogError("Loading last used weapons: " + arg);
				}
			}
			this.LoadTryGunsInfo();
			this.LoadTryGunDiscounts();
		}
		this.LoadWearInfoPrefabsToCache();
	}

	// Token: 0x060044A4 RID: 17572 RVA: 0x00172278 File Offset: 0x00170478
	private void LoadWearInfoPrefabsToCache()
	{
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Armor_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Capes_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Shop_Boots_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Masks_Info"));
		this._wearInfoPrefabsToCache.AddRange(Resources.LoadAll<ShopPositionParams>("Hats_Info"));
	}

	// Token: 0x060044A5 RID: 17573 RVA: 0x001722F0 File Offset: 0x001704F0
	private void SaveLastUsedWeapons()
	{
		Storager.setString("WeaponManager.LastUsedWeaponsKey", Rilisoft.MiniJson.Json.Serialize(this.lastUsedWeaponsForFilterMaps), false);
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x00172308 File Offset: 0x00170508
	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			try
			{
				this.SaveLastUsedWeapons();
			}
			catch (Exception arg)
			{
				Debug.LogError("Saving last used weapons: " + arg);
			}
			this.SaveTryGunsInfo();
			this.SaveTryGunsDiscounts();
		}
		else
		{
			this.LoadTryGunsInfo();
			this.LoadTryGunDiscounts();
		}
	}

	// Token: 0x060044A7 RID: 17575 RVA: 0x00172378 File Offset: 0x00170578
	private IEnumerator LoadRocketToCache()
	{
		ResourceRequest request = Resources.LoadAsync<GameObject>("Rocket");
		yield return request;
		if (request.isDone)
		{
			this._rocketCache = (GameObject)request.asset;
		}
		yield break;
	}

	// Token: 0x060044A8 RID: 17576 RVA: 0x00172394 File Offset: 0x00170594
	private IEnumerator LoadTurretToCache()
	{
		ResourceRequest request = Resources.LoadAsync<GameObject>("Turret");
		yield return request;
		if (request.isDone)
		{
			this._turretCache = (GameObject)request.asset;
		}
		yield break;
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x001723B0 File Offset: 0x001705B0
	private IEnumerator Start()
	{
		using (new ScopeLogger("WeaponManager.Start()", Defs.IsDeveloperBuild))
		{
			base.StartCoroutine(this.Step());
			yield return null;
			this._turretWeaponCache = WeaponManager.InnerPrefabForWeaponSync("WeaponTurret");
			base.StartCoroutine(this.LoadRocketToCache());
			base.StartCoroutine(this.LoadTurretToCache());
			Defs.gameSecondFireButtonMode = (Defs.GameSecondFireButtonMode)PlayerPrefs.GetInt("GameSecondFireButtonMode", 0);
			WeaponManager.sharedManager = this;
			for (int i = 0; i < 6; i++)
			{
				this._weaponsByCat.Add(new List<GameObject>());
			}
			string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(true);
			for (int j = 0; j < canBuyWeaponTags.Length; j++)
			{
				string shopId = ItemDb.GetShopIdByTag(canBuyWeaponTags[j]);
				this._purchaseActinos.Add(shopId, new Action<string, int>(this.AddWeaponToInv));
			}
			yield return null;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (!Application.isEditor && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				GoogleIABManager.purchaseSucceededEvent += this.AddWeapon;
			}
			GlobalGameController.SetMultiMode();
			yield return base.StartCoroutine(this.ResetCoroutine(0, false));
			this._initialized = true;
		}
		yield break;
	}

	// Token: 0x060044AA RID: 17578 RVA: 0x001723CC File Offset: 0x001705CC
	public void AddWeaponToInv(string shopId, int timeForRentIndex = 0)
	{
		string tagByShopId = ItemDb.GetTagByShopId(shopId);
		Player_move_c.SaveWeaponInPrefs(tagByShopId, timeForRentIndex);
		GameObject prefabByTag = this.GetPrefabByTag(tagByShopId);
		if (prefabByTag != null)
		{
			int num;
			this.AddWeapon(prefabByTag, out num);
		}
	}

	// Token: 0x060044AB RID: 17579 RVA: 0x00172408 File Offset: 0x00170608
	public void AddNewWeapon(string id, int timeForRentIndex = 0)
	{
		if (id == null)
		{
			throw new ArgumentNullException("id");
		}
		if (this._purchaseActinos.ContainsKey(id))
		{
			this._purchaseActinos[id](id, timeForRentIndex);
		}
	}

	// Token: 0x060044AC RID: 17580 RVA: 0x00172440 File Offset: 0x00170640
	private void AddWeapon(GooglePurchase p)
	{
		try
		{
			this.AddNewWeapon(p.productId, 0);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x060044AD RID: 17581 RVA: 0x00172488 File Offset: 0x00170688
	private void OnDestroy()
	{
		if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
		{
			GoogleIABManager.purchaseSucceededEvent -= this.AddWeapon;
		}
	}

	// Token: 0x060044AE RID: 17582 RVA: 0x001724A8 File Offset: 0x001706A8
	public void ReloadWeaponFromSet(int index)
	{
		int num = ((Weapon)this.playerWeapons[index]).weaponPrefab.GetComponent<WeaponSounds>().ammoInClip - ((Weapon)this.playerWeapons[index]).currentAmmoInClip;
		if (((Weapon)this.playerWeapons[index]).currentAmmoInBackpack >= num)
		{
			((Weapon)this.playerWeapons[index]).currentAmmoInClip += num;
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None)
			{
				((Weapon)this.playerWeapons[index]).currentAmmoInBackpack -= num;
			}
		}
		else
		{
			((Weapon)this.playerWeapons[index]).currentAmmoInClip += ((Weapon)this.playerWeapons[index]).currentAmmoInBackpack;
			((Weapon)this.playerWeapons[index]).currentAmmoInBackpack = 0;
		}
	}

	// Token: 0x060044AF RID: 17583 RVA: 0x001725A8 File Offset: 0x001707A8
	public void ReloadAmmo()
	{
		this.ReloadWeaponFromSet(this.CurrentWeaponIndex);
		if (this.myPlayerMoveC != null)
		{
			this.myPlayerMoveC.isReloading = false;
		}
	}

	// Token: 0x060044B0 RID: 17584 RVA: 0x001725D4 File Offset: 0x001707D4
	public void Reload()
	{
		if (!this.currentWeaponSounds.isShotMelee)
		{
			this.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Empty");
			if (!this.currentWeaponSounds.isDoubleShot)
			{
				this.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Shoot");
			}
			this.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Reload");
			this.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].speed = this.myPlayerMoveC._currentReloadAnimationSpeed;
		}
	}

	// Token: 0x060044B1 RID: 17585 RVA: 0x0017267C File Offset: 0x0017087C
	private void ReturnAlienGunToCampaignBack()
	{
		Storager.setInt(Defs.ReturnAlienGun930, 1, false);
		Storager.setString(Defs.MultiplayerWSSN, this.DefaultSetForWeaponSetSettingName(Defs.MultiplayerWSSN), false);
		Storager.setString(Defs.CampaignWSSN, this.DefaultSetForWeaponSetSettingName(Defs.CampaignWSSN), false);
	}

	// Token: 0x060044B2 RID: 17586 RVA: 0x001726C4 File Offset: 0x001708C4
	private void FixWeaponsDueToCategoriesMoved911()
	{
		Storager.setInt(Defs.FixWeapons911, 1, false);
		Storager.setString(Defs.MultiplayerWSSN, this.DefaultSetForWeaponSetSettingName(Defs.MultiplayerWSSN), false);
		Storager.setString(Defs.CampaignWSSN, this.DefaultSetForWeaponSetSettingName(Defs.CampaignWSSN), false);
	}

	// Token: 0x060044B3 RID: 17587 RVA: 0x0017270C File Offset: 0x0017090C
	public void RemoveTemporaryItem(string tg)
	{
		if (tg == null)
		{
			return;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		if (byTag == null || byTag.PrefabName == null)
		{
			return;
		}
		string text = this.LoadWeaponSet(Defs.MultiplayerWSSN);
		string[] array = text.Split(new char[]
		{
			'#'
		});
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				array[i] = string.Empty;
			}
		}
		int num = -1;
		foreach (object obj in this.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			if (ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag.Equals(tg))
			{
				num = this.allAvailablePlayerWeapons.IndexOf(weapon);
				break;
			}
		}
		if (num != -1)
		{
			this.allAvailablePlayerWeapons.RemoveAt(num);
		}
		int num2 = Array.IndexOf<string>(array, byTag.PrefabName);
		if (num2 != -1)
		{
			WeaponManager.sharedManager.SaveWeaponSet(Defs.MultiplayerWSSN, this.TopWeaponForCat(num2, false), num2);
			WeaponManager.sharedManager.SaveWeaponSet(Defs.CampaignWSSN, this.TopWeaponForCat(num2, true), num2);
		}
		this.SetWeaponsSet(this._currentFilterMap);
		this._InitShopCategoryLists(this._currentFilterMap);
		this.UpdateFilteredShopLists();
	}

	// Token: 0x060044B4 RID: 17588 RVA: 0x001728A4 File Offset: 0x00170AA4
	private string TopWeaponForCat(int ind, bool campaign = false)
	{
		string result = WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet();
		if (campaign)
		{
			result = WeaponManager._KnifeAndPistolAndShotgunSet();
		}
		List<WeaponSounds> list = new List<WeaponSounds>();
		foreach (object obj in this.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
			if (component.categoryNabor - 1 == ind)
			{
				ItemRecord byPrefabName = ItemDb.GetByPrefabName(component.name.Replace("(Clone)", string.Empty));
				if (byPrefabName != null && byPrefabName.CanBuy)
				{
					list.Add(component);
				}
			}
		}
		list.Sort(WeaponManager.dpsComparerWS);
		if (list.Count > 0)
		{
			result = list[list.Count - 1].gameObject.name;
		}
		return result;
	}

	// Token: 0x060044B5 RID: 17589 RVA: 0x001729AC File Offset: 0x00170BAC
	public static List<string> GetWeaponsForBuy()
	{
		List<string> list = new List<string>();
		string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(false);
		foreach (string text in canBuyWeaponTags)
		{
			if (WeaponManager.tagToStoreIDMapping.ContainsKey(text) && !ItemDb.IsTemporaryGun(text))
			{
				list.Add(text);
			}
		}
		bool filterNextTierUpgrades = true;
		List<string> second = PromoActionsGUIController.FilterPurchases(list, filterNextTierUpgrades, true, false, false);
		return list.Except(second).ToList<string>();
	}

	// Token: 0x060044B6 RID: 17590 RVA: 0x00172A24 File Offset: 0x00170C24
	public static GameObject InnerPrefabForWeaponSync(string weapon)
	{
		return Resources.Load<GameObject>(Defs.InnerWeaponsFolder + "/" + weapon + Defs.InnerWeapons_Suffix);
	}

	// Token: 0x060044B7 RID: 17591 RVA: 0x00172A40 File Offset: 0x00170C40
	public static bool RemoveGunFromAllTryGunRelated(string tg)
	{
		if (tg == null)
		{
			Debug.LogError("RemoveGunFromAllTryGunRelated: tg == null");
			return false;
		}
		bool result = WeaponManager.sharedManager.TryGuns.Remove(tg);
		WeaponManager.sharedManager.ExpiredTryGuns.RemoveAll((string expiredGunTag) => expiredGunTag == tg);
		WeaponManager.sharedManager.RemoveDiscountForTryGun(tg);
		return result;
	}

	// Token: 0x060044B8 RID: 17592 RVA: 0x00172AB4 File Offset: 0x00170CB4
	public static void ActualizeWeaponsForCampaignProgress()
	{
		try
		{
			string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
			{
				'#'
			});
			List<string> list = new List<string>();
			foreach (string item in array)
			{
				list.Add(item);
			}
			foreach (string key in CampaignProgress.boxesLevelsAndStars.Keys)
			{
				foreach (string key2 in CampaignProgress.boxesLevelsAndStars[key].Keys)
				{
					string item2;
					if (LevelBox.weaponsFromBosses.TryGetValue(key2, out item2) && !list.Contains(item2))
					{
						list.Add(item2);
					}
				}
			}
			if (list.Contains(WeaponManager.ShotgunWN))
			{
				list[list.IndexOf(WeaponManager.ShotgunWN)] = WeaponManager.UZI_WN;
			}
			string val = string.Join("#", list.ToArray());
			Storager.setString(Defs.WeaponsGotInCampaign, val, false);
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in ActualizeWeaponsForCampaignProgress: " + arg);
		}
	}

	// Token: 0x060044B9 RID: 17593 RVA: 0x00172C60 File Offset: 0x00170E60
	public static bool AllUpgradesOfWeaponAreBought(string weapnoTag)
	{
		if (weapnoTag == null)
		{
			return false;
		}
		List<string> list = WeaponUpgrades.ChainForTag(weapnoTag);
		if (list == null)
		{
			return WeaponManager.LastBoughtTag(weapnoTag, null) != null;
		}
		return WeaponManager.LastBoughtTag(weapnoTag, null) == list.LastOrDefault<string>();
	}

	// Token: 0x060044BA RID: 17594 RVA: 0x00172CA4 File Offset: 0x00170EA4
	private IEnumerator UpdateWeapons800To801()
	{
		Storager.setInt(Defs.Weapons800to801, 1, false);
		Storager.setString(Defs.MultiplayerWSSN, this.DefaultSetForWeaponSetSettingName(Defs.MultiplayerWSSN), false);
		Storager.setString(Defs.CampaignWSSN, this.DefaultSetForWeaponSetSettingName(Defs.CampaignWSSN), false);
		if (Storager.getInt(Defs.BarrettSN, true) > 0)
		{
			Storager.setInt(Defs.Barrett2SN, 1, true);
		}
		if (Storager.getInt(Defs.plazma_pistol_SN, true) > 0)
		{
			Storager.setInt(Defs.plazma_pistol_2, 1, true);
		}
		if (Storager.getInt(Defs.StaffSN, true) > 0)
		{
			Storager.setInt(Defs.Staff2SN, 1, true);
		}
		if (Storager.getInt(Defs.MagicBowSett, true) > 0)
		{
			Storager.setInt(Defs.Bow_3, 1, true);
		}
		if (Storager.getInt(Defs.MaceSN, true) > 0)
		{
			Storager.setInt(Defs.Mace2SN, 1, true);
		}
		if (Storager.getInt(Defs.ChainsawS, true) > 0)
		{
			Storager.setInt(Defs.Chainsaw2SN, 1, true);
		}
		if (!this._initialized)
		{
			yield return null;
		}
		if (Storager.getInt(Defs.FlowePowerSN, true) > 0)
		{
			Storager.setInt(Defs.flower_3, 1, true);
		}
		if (Storager.getInt(Defs.flower_2, true) > 0)
		{
			Storager.setInt(Defs.flower_3, 1, true);
		}
		if (Storager.getInt(Defs.ScytheSN, true) > 0)
		{
			Storager.setInt(Defs.scythe_3, 1, true);
		}
		if (Storager.getInt(Defs.Scythe_2_SN, true) > 0)
		{
			Storager.setInt(Defs.scythe_3, 1, true);
		}
		if (Storager.getInt(Defs.FlameThrowerSN, true) > 0)
		{
			Storager.setInt(Defs.Flamethrower_3, 1, true);
		}
		if (Storager.getInt(Defs.FlameThrower_2SN, true) > 0)
		{
			Storager.setInt(Defs.Flamethrower_3, 1, true);
		}
		if (!this._initialized)
		{
			yield return null;
		}
		if (Storager.getInt(Defs.RazerSN, true) > 0)
		{
			Storager.setInt(Defs.Razer_3, 1, true);
		}
		if (Storager.getInt(Defs.Razer_2SN, true) > 0)
		{
			Storager.setInt(Defs.Razer_3, 1, true);
		}
		if (Storager.getInt(Defs.Revolver2SN, true) > 0)
		{
			Storager.setInt(Defs.revolver_2_3, 1, true);
		}
		if (Storager.getInt(Defs.revolver_2_2, true) > 0)
		{
			Storager.setInt(Defs.revolver_2_3, 1, true);
		}
		if (Storager.getInt(Defs.Sword_2_SN, true) > 0)
		{
			Storager.setInt(Defs.Sword_2_3, 1, true);
		}
		if (Storager.getInt(Defs.Sword_22SN, true) > 0)
		{
			Storager.setInt(Defs.Sword_2_3, 1, true);
		}
		if (!this._initialized)
		{
			yield return null;
		}
		if (Storager.getInt(Defs.MinigunSN, true) > 0)
		{
			Storager.setInt(Defs.minigun_3, 1, true);
		}
		if (Storager.getInt(Defs.RedMinigunSN, true) > 0)
		{
			Storager.setInt(Defs.minigun_3, 1, true);
		}
		if (Storager.getInt(Defs.m79_2, true) > 0)
		{
			Storager.setInt(Defs.m79_3, 1, true);
		}
		if (Storager.getInt(Defs.Bazooka_2_1, true) > 0)
		{
			Storager.setInt(Defs.Bazooka_2_3, 1, true);
		}
		if (Storager.getInt(Defs.plazmaSN, true) > 0)
		{
			Storager.setInt(Defs.plazma_3, 1, true);
		}
		if (Storager.getInt(Defs.plazma_2, true) > 0)
		{
			Storager.setInt(Defs.plazma_3, 1, true);
		}
		if (!this._initialized)
		{
			yield return null;
		}
		if (Storager.getInt(Defs._3PLShotgunSN, true) > 0)
		{
			Storager.setInt(Defs._3_shotgun_3, 1, true);
		}
		if (Storager.getInt(Defs._3_shotgun_2, true) > 0)
		{
			Storager.setInt(Defs._3_shotgun_3, 1, true);
		}
		if (Storager.getInt(Defs.LaserRifleSN, true) > 0)
		{
			Storager.setInt(Defs.Red_Stone_3, 1, true);
		}
		if (Storager.getInt(Defs.GoldenRed_StoneSN, true) > 0)
		{
			Storager.setInt(Defs.Red_Stone_3, 1, true);
		}
		if (Storager.getInt(Defs.LightSwordSN, true) > 0)
		{
			Storager.setInt(Defs.LightSword_3, 1, true);
		}
		if (Storager.getInt(Defs.RedLightSaberSN, true) > 0)
		{
			Storager.setInt(Defs.LightSword_3, 1, true);
		}
		if (Storager.getInt(Defs.katana_SN, true) > 0)
		{
			Storager.setInt(Defs.katana_3_SN, 1, true);
		}
		if (Storager.getInt(Defs.katana_2_SN, true) > 0)
		{
			Storager.setInt(Defs.katana_3_SN, 1, true);
		}
		yield break;
	}

	// Token: 0x04003177 RID: 12663
	public const int DefaultNumberOfMatchesForTryGuns = 3;

	// Token: 0x04003178 RID: 12664
	private const string TryGunsTableServerKey = "TryGuns";

	// Token: 0x04003179 RID: 12665
	public const string NumberOfMatchesKey = "NumberOfMatchesKey";

	// Token: 0x0400317A RID: 12666
	public const string EquippedBeforeKey = "EquippedBeforeKey";

	// Token: 0x0400317B RID: 12667
	public const string TryGunsDictionaryKey = "TryGunsDictionaryKey";

	// Token: 0x0400317C RID: 12668
	public const string ExpiredTryGunsListKey = "ExpiredTryGunsListKey";

	// Token: 0x0400317D RID: 12669
	public const string TryGunsKey = "WeaponManager.TryGunsKey";

	// Token: 0x0400317E RID: 12670
	public const string TryGunsDiscountsKey = "WeaponManager.TryGunsDiscountsKey";

	// Token: 0x0400317F RID: 12671
	public const string TryGunsDiscountsValuesKey = "WeaponManager.TryGunsDiscountsValuesKey";

	// Token: 0x04003180 RID: 12672
	public const int NumOfWeaponCategories = 6;

	// Token: 0x04003181 RID: 12673
	private const string FirstTagForOurTierKey = "FirstTagsForOurTier";

	// Token: 0x04003182 RID: 12674
	private const string SniperCategoryAddedToWeaponSetsKey = "WeaponManager_SniperCategoryAddedToWeaponSetsKey";

	// Token: 0x04003183 RID: 12675
	private const string LastUsedWeaponsKey = "WeaponManager.LastUsedWeaponsKey";

	// Token: 0x04003184 RID: 12676
	private static bool _buffsPAramsInitialized;

	// Token: 0x04003185 RID: 12677
	private static Dictionary<ShopNGUIController.CategoryNames, List<List<string>>> _defaultTryGunsTable;

	// Token: 0x04003186 RID: 12678
	private Dictionary<string, long> tryGunPromos;

	// Token: 0x04003187 RID: 12679
	private Dictionary<string, SaltedLong> tryGunDiscounts;

	// Token: 0x04003188 RID: 12680
	public Dictionary<string, Dictionary<string, object>> TryGuns;

	// Token: 0x04003189 RID: 12681
	public List<string> ExpiredTryGuns;

	// Token: 0x0400318A RID: 12682
	public static Dictionary<int, FilterMapSettings> WeaponSetSettingNamesForFilterMaps = new Dictionary<int, FilterMapSettings>
	{
		{
			0,
			new FilterMapSettings
			{
				settingName = Defs.MultiplayerWSSN,
				defaultWeaponSet = new Func<string>(WeaponManager._KnifeAndPistolAndMP5AndSniperAndRocketnitzaSet)
			}
		},
		{
			1,
			new FilterMapSettings
			{
				settingName = "WeaponManager.KnifesModeWSSN",
				defaultWeaponSet = new Func<string>(WeaponManager._KnifeSet)
			}
		},
		{
			2,
			new FilterMapSettings
			{
				settingName = "WeaponManager.SniperModeWSSN",
				defaultWeaponSet = new Func<string>(WeaponManager._KnifeAndPistolAndSniperSet)
			}
		},
		{
			3,
			new FilterMapSettings
			{
				settingName = Defs.DaterWSSN,
				defaultWeaponSet = new Func<string>(WeaponManager._InitialDaterSet)
			}
		}
	};

	// Token: 0x0400318B RID: 12683
	public static List<string> GotchaGuns = new List<string>
	{
		"gift_gun",
		"Candy_Baton",
		"mp5_gold_gift",
		WeaponTags.spark_shark_Tag,
		WeaponTags.power_claw_Tag
	};

	// Token: 0x0400318C RID: 12684
	public static List<KeyValuePair<string, string>> replaceConstWithTemp = new List<KeyValuePair<string, string>>();

	// Token: 0x0400318D RID: 12685
	public GameObject _grenadeWeaponCache;

	// Token: 0x0400318E RID: 12686
	public GameObject _turretWeaponCache;

	// Token: 0x0400318F RID: 12687
	public GameObject _rocketCache;

	// Token: 0x04003190 RID: 12688
	public GameObject _turretCache;

	// Token: 0x04003191 RID: 12689
	public static string WeaponPreviewsPath = "WeaponPreviews";

	// Token: 0x04003192 RID: 12690
	public static string DaterFreeWeaponPrefabName = "Weapon298";

	// Token: 0x04003193 RID: 12691
	public static List<GameObject> cachedInnerPrefabsForCurrentShopCategory = new List<GameObject>();

	// Token: 0x04003194 RID: 12692
	public static Dictionary<string, string> campaignBonusWeapons = new Dictionary<string, string>();

	// Token: 0x04003195 RID: 12693
	public static Dictionary<string, string> tagToStoreIDMapping = new Dictionary<string, string>(200);

	// Token: 0x04003196 RID: 12694
	public static Dictionary<string, string> storeIDtoDefsSNMapping = new Dictionary<string, string>(200);

	// Token: 0x04003197 RID: 12695
	private static readonly HashSet<string> _purchasableWeaponSet = new HashSet<string>();

	// Token: 0x04003198 RID: 12696
	public static string _3_shotgun_2_WN = "Weapon107";

	// Token: 0x04003199 RID: 12697
	public static string _3_shotgun_3_WN = "Weapon108";

	// Token: 0x0400319A RID: 12698
	public static string flower_2_WN = "Weapon109";

	// Token: 0x0400319B RID: 12699
	public static string flower_3_WN = "Weapon110";

	// Token: 0x0400319C RID: 12700
	public static string gravity_2_WN = "Weapon111";

	// Token: 0x0400319D RID: 12701
	public static string gravity_3_WN = "Weapon112";

	// Token: 0x0400319E RID: 12702
	public static string grenade_launcher_3_WN = "Weapon113";

	// Token: 0x0400319F RID: 12703
	public static string revolver_2_2_WN = "Weapon114";

	// Token: 0x040031A0 RID: 12704
	public static string revolver_2_3_WN = "Weapon115";

	// Token: 0x040031A1 RID: 12705
	public static string scythe_3_WN = "Weapon116";

	// Token: 0x040031A2 RID: 12706
	public static string plazma_2_WN = "Weapon117";

	// Token: 0x040031A3 RID: 12707
	public static string plazma_3_WN = "Weapon118";

	// Token: 0x040031A4 RID: 12708
	public static string plazma_pistol_2_WN = "Weapon119";

	// Token: 0x040031A5 RID: 12709
	public static string plazma_pistol_3_WN = "Weapon120";

	// Token: 0x040031A6 RID: 12710
	public static string railgun_2_WN = "Weapon121";

	// Token: 0x040031A7 RID: 12711
	public static string railgun_3_WN = "Weapon122";

	// Token: 0x040031A8 RID: 12712
	public static string Razer_3_WN = "Weapon123";

	// Token: 0x040031A9 RID: 12713
	public static string tesla_3_WN = "Weapon124";

	// Token: 0x040031AA RID: 12714
	public static string Flamethrower_3_WN = "Weapon125";

	// Token: 0x040031AB RID: 12715
	public static string FreezeGun_0_WN = "Weapon126";

	// Token: 0x040031AC RID: 12716
	public static string svd_3_WN = "Weapon128";

	// Token: 0x040031AD RID: 12717
	public static string barret_3_WN = "Weapon129";

	// Token: 0x040031AE RID: 12718
	public static string minigun_3_WN = "Weapon127";

	// Token: 0x040031AF RID: 12719
	public static string LightSword_3_WN = "Weapon130";

	// Token: 0x040031B0 RID: 12720
	public static string Sword_2_3_WN = "Weapon131";

	// Token: 0x040031B1 RID: 12721
	public static string Staff_3_WN = "Weapon132";

	// Token: 0x040031B2 RID: 12722
	public static string DragonGun_WN = "Weapon133";

	// Token: 0x040031B3 RID: 12723
	public static string Bow_3_WN = "Weapon134";

	// Token: 0x040031B4 RID: 12724
	public static string Bazooka_1_3_WN = "Weapon135";

	// Token: 0x040031B5 RID: 12725
	public static string Bazooka_2_1_WN = "Weapon136";

	// Token: 0x040031B6 RID: 12726
	public static string Bazooka_2_3_WN = "Weapon137";

	// Token: 0x040031B7 RID: 12727
	public static string m79_2_WN = "Weapon138";

	// Token: 0x040031B8 RID: 12728
	public static string m79_3_WN = "Weapon139";

	// Token: 0x040031B9 RID: 12729
	public static string m32_1_2_WN = "Weapon140";

	// Token: 0x040031BA RID: 12730
	public static string Red_Stone_3_WN = "Weapon141";

	// Token: 0x040031BB RID: 12731
	public static string XM8_1_WN = "Weapon142";

	// Token: 0x040031BC RID: 12732
	public static string PumpkinGun_1_WN = "Weapon143";

	// Token: 0x040031BD RID: 12733
	public static string XM8_2_WN = "Weapon144";

	// Token: 0x040031BE RID: 12734
	public static string XM8_3_WN = "Weapon145";

	// Token: 0x040031BF RID: 12735
	public static string PumpkinGun_2_WN = "Weapon147";

	// Token: 0x040031C0 RID: 12736
	public static string Rocketnitza_WN = "Weapon162";

	// Token: 0x040031C1 RID: 12737
	public static WeaponManager sharedManager = null;

	// Token: 0x040031C2 RID: 12738
	public static readonly int LastNotNewWeapon = 76;

	// Token: 0x040031C3 RID: 12739
	public List<string> shownWeapons = new List<string>();

	// Token: 0x040031C4 RID: 12740
	public HostData hostDataServer;

	// Token: 0x040031C5 RID: 12741
	public string ServerIp;

	// Token: 0x040031C6 RID: 12742
	public GameObject myPlayer;

	// Token: 0x040031C7 RID: 12743
	public Player_move_c myPlayerMoveC;

	// Token: 0x040031C8 RID: 12744
	public GameObject myGun;

	// Token: 0x040031C9 RID: 12745
	public GameObject myTable;

	// Token: 0x040031CA RID: 12746
	public NetworkStartTable myNetworkStartTable;

	// Token: 0x040031CB RID: 12747
	private UnityEngine.Object[] _weaponsInGame;

	// Token: 0x040031CC RID: 12748
	private UnityEngine.Object[] _multiWeapons;

	// Token: 0x040031CD RID: 12749
	private UnityEngine.Object[] _hungerWeapons;

	// Token: 0x040031CE RID: 12750
	private ArrayList _playerWeapons = new ArrayList();

	// Token: 0x040031CF RID: 12751
	private ArrayList _allAvailablePlayerWeapons = new ArrayList();

	// Token: 0x040031D0 RID: 12752
	private int currentWeaponIndex;

	// Token: 0x040031D1 RID: 12753
	private Dictionary<string, int> lastUsedWeaponsForFilterMaps = new Dictionary<string, int>
	{
		{
			"0",
			0
		},
		{
			"1",
			2
		},
		{
			"2",
			4
		},
		{
			"3",
			2
		}
	};

	// Token: 0x040031D2 RID: 12754
	public Camera useCam;

	// Token: 0x040031D3 RID: 12755
	private WeaponSounds _currentWeaponSounds = new WeaponSounds();

	// Token: 0x040031D4 RID: 12756
	private Dictionary<string, Action<string, int>> _purchaseActinos = new Dictionary<string, Action<string, int>>(300);

	// Token: 0x040031D5 RID: 12757
	public List<WeaponManager.infoClient> players = new List<WeaponManager.infoClient>();

	// Token: 0x040031D6 RID: 12758
	public List<List<GameObject>> _weaponsByCat = new List<List<GameObject>>();

	// Token: 0x040031D7 RID: 12759
	public List<List<GameObject>> FilteredShopListsForPromos;

	// Token: 0x040031D8 RID: 12760
	public List<List<GameObject>> FilteredShopListsNoUpgrades;

	// Token: 0x040031D9 RID: 12761
	private List<GameObject> _playerWeaponsSetInnerPrefabsCache = new List<GameObject>();

	// Token: 0x040031DA RID: 12762
	private int _lockGetWeaponPrefabs;

	// Token: 0x040031DB RID: 12763
	private List<WeaponSounds> outerWeaponPrefabs;

	// Token: 0x040031DC RID: 12764
	private static List<string> _Removed150615_Guns = null;

	// Token: 0x040031DD RID: 12765
	private static List<string> _Removed150615_GunsPrefabNAmes = null;

	// Token: 0x040031DE RID: 12766
	private static bool firstTagsForTiersInitialized = false;

	// Token: 0x040031DF RID: 12767
	private static Dictionary<string, string> firstTagsWithRespecToOurTier = new Dictionary<string, string>();

	// Token: 0x040031E0 RID: 12768
	private static string[] oldTags = new string[]
	{
		WeaponTags.MinersWeaponTag,
		WeaponTags.Sword_2_3_Tag,
		WeaponTags.RailgunTag,
		WeaponTags.SteelAxeTag,
		WeaponTags.IronSwordTag,
		WeaponTags.Red_Stone_3_Tag,
		WeaponTags.SPASTag,
		WeaponTags.SteelCrossbowTag,
		WeaponTags.minigun_3_Tag,
		WeaponTags.LightSword_3_Tag,
		WeaponTags.FAMASTag,
		WeaponTags.FreezeGunTag,
		WeaponTags.BerettaTag,
		WeaponTags.EagleTag,
		WeaponTags.GlockTag,
		WeaponTags.svdTag,
		WeaponTags.m16Tag,
		WeaponTags.TreeTag,
		WeaponTags.revolver_2_3_Tag,
		WeaponTags.FreezeGun_0_Tag,
		WeaponTags.TeslaTag,
		WeaponTags.Bazooka_3Tag,
		WeaponTags.GrenadeLuancher_2Tag,
		WeaponTags.BazookaTag,
		WeaponTags.AUGTag,
		WeaponTags.AK74Tag,
		WeaponTags.GravigunTag,
		WeaponTags.XM8_1_Tag,
		WeaponTags.PumpkinGun_1_Tag,
		WeaponTags.SnowballMachingun_Tag,
		WeaponTags.SnowballGun_Tag,
		WeaponTags.HeavyShotgun_Tag,
		WeaponTags.TwoBolters_Tag,
		WeaponTags.TwoRevolvers_Tag,
		WeaponTags.AutoShotgun_Tag,
		WeaponTags.Solar_Ray_Tag,
		WeaponTags.Water_Pistol_Tag,
		WeaponTags.Solar_Power_Cannon_Tag,
		WeaponTags.Water_Rifle_Tag,
		WeaponTags.Valentine_Shotgun_Tag,
		WeaponTags.Needle_Throw_Tag,
		WeaponTags.Needle_Throw_Tag,
		WeaponTags.Carrot_Sword_Tag,
		WeaponTags._3_shotgun_3_Tag,
		WeaponTags.plazma_3_Tag,
		WeaponTags.katana_3_Tag,
		WeaponTags.DragonGun_Tag,
		WeaponTags.Bazooka_2_3_Tag,
		WeaponTags.buddy_Tag,
		WeaponTags.barret_3_Tag,
		WeaponTags.Flamethrower_3_Tag,
		WeaponTags.SparklyBlasterTag,
		WeaponTags.Thompson_2_Tag
	};

	// Token: 0x040031E1 RID: 12769
	private bool _resetLock;

	// Token: 0x040031E2 RID: 12770
	private static bool _migrationsCompletedAtThisLaunch = false;

	// Token: 0x040031E3 RID: 12771
	public int _currentFilterMap;

	// Token: 0x040031E4 RID: 12772
	private bool _initialized;

	// Token: 0x040031E5 RID: 12773
	private static List<string> weaponsMovedToSniperCategory = new List<string>
	{
		"Weapon299",
		"Weapon322",
		"Weapon323",
		WeaponManager.CampaignRifle_WN,
		"Weapon44",
		"Weapon46",
		"Weapon61",
		"Weapon256",
		"Weapon77",
		"Weapon209",
		"Weapon65",
		"Weapon27",
		"Weapon63",
		"Weapon134",
		"Weapon37",
		"Weapon268",
		"Weapon121",
		"Weapon210",
		"Weapon251",
		"Weapon128",
		"Weapon269",
		"Weapon122",
		"Weapon211",
		"Weapon271",
		"Weapon221",
		"Weapon188",
		"Weapon192",
		"Weapon129",
		"Weapon241"
	};

	// Token: 0x040031E6 RID: 12774
	private List<ShopPositionParams> _wearInfoPrefabsToCache = new List<ShopPositionParams>();

	// Token: 0x040031E7 RID: 12775
	private AnimationClip[] _profileAnimClips;

	// Token: 0x040031E8 RID: 12776
	private static Comparison<WeaponSounds> dpsComparerWS = delegate(WeaponSounds leftWS, WeaponSounds rightWS)
	{
		if (ExpController.Instance == null || leftWS == null || rightWS == null)
		{
			return 0;
		}
		float num = leftWS.DPS - rightWS.DPS;
		if (num > 0f)
		{
			return 1;
		}
		if (num < 0f)
		{
			return -1;
		}
		int result;
		try
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(leftWS.name.Replace("(Clone)", string.Empty));
			ItemPrice itemPrice = (!byPrefabName.CanBuy) ? new ItemPrice(10, "Coins") : byPrefabName.Price;
			ItemRecord byPrefabName2 = ItemDb.GetByPrefabName(rightWS.name.Replace("(Clone)", string.Empty));
			ItemPrice itemPrice2 = (!byPrefabName2.CanBuy) ? new ItemPrice(10, "Coins") : byPrefabName2.Price;
			if (itemPrice.Currency == "GemsCurrency" && itemPrice2.Currency == "Coins")
			{
				result = 1;
			}
			else if (itemPrice.Currency == "Coins" && itemPrice2.Currency == "GemsCurrency")
			{
				result = -1;
			}
			else if (itemPrice.Price.CompareTo(itemPrice2.Price) != 0)
			{
				result = itemPrice.Price.CompareTo(itemPrice2.Price);
			}
			else
			{
				result = Array.IndexOf<string>(WeaponComparer.multiplayerWeaponsOrd, ItemDb.GetByPrefabName(leftWS.name.Replace("(Clone)", string.Empty)).Tag).CompareTo(Array.IndexOf<string>(WeaponComparer.multiplayerWeaponsOrd, ItemDb.GetByPrefabName(rightWS.name.Replace("(Clone)", string.Empty)).Tag));
			}
		}
		catch
		{
			result = 0;
		}
		return result;
	};

	// Token: 0x040031E9 RID: 12777
	private Comparison<GameObject> dpsComparer = delegate(GameObject leftw, GameObject rightw)
	{
		if (leftw == null || rightw == null)
		{
			return 0;
		}
		WeaponSounds component = leftw.GetComponent<WeaponSounds>();
		WeaponSounds component2 = rightw.GetComponent<WeaponSounds>();
		return WeaponManager.dpsComparerWS(component, component2);
	};

	// Token: 0x040031EA RID: 12778
	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();

	// Token: 0x02000788 RID: 1928
	public enum WeaponTypeForLow
	{
		// Token: 0x04003209 RID: 12809
		AssaultRifle_1,
		// Token: 0x0400320A RID: 12810
		AssaultRifle_2,
		// Token: 0x0400320B RID: 12811
		Shotgun_1,
		// Token: 0x0400320C RID: 12812
		Shotgun_2,
		// Token: 0x0400320D RID: 12813
		Machinegun,
		// Token: 0x0400320E RID: 12814
		Pistol_1,
		// Token: 0x0400320F RID: 12815
		Pistol_2,
		// Token: 0x04003210 RID: 12816
		Submachinegun,
		// Token: 0x04003211 RID: 12817
		Knife,
		// Token: 0x04003212 RID: 12818
		Sword,
		// Token: 0x04003213 RID: 12819
		Flamethrower_1,
		// Token: 0x04003214 RID: 12820
		Flamethrower_2,
		// Token: 0x04003215 RID: 12821
		SniperRifle_1,
		// Token: 0x04003216 RID: 12822
		SniperRifle_2,
		// Token: 0x04003217 RID: 12823
		Bow,
		// Token: 0x04003218 RID: 12824
		RocketLauncher_1,
		// Token: 0x04003219 RID: 12825
		RocketLauncher_2,
		// Token: 0x0400321A RID: 12826
		RocketLauncher_3,
		// Token: 0x0400321B RID: 12827
		GrenadeLauncher,
		// Token: 0x0400321C RID: 12828
		Snaryad,
		// Token: 0x0400321D RID: 12829
		Snaryad_Otskok,
		// Token: 0x0400321E RID: 12830
		Snaryad_Disk,
		// Token: 0x0400321F RID: 12831
		Railgun,
		// Token: 0x04003220 RID: 12832
		Ray,
		// Token: 0x04003221 RID: 12833
		AOE,
		// Token: 0x04003222 RID: 12834
		Instant_Area_Damage,
		// Token: 0x04003223 RID: 12835
		X3_Snaryad,
		// Token: 0x04003224 RID: 12836
		NOT_CHANGE
	}

	// Token: 0x02000789 RID: 1929
	internal sealed class WeaponTypeForLowComparer : IEqualityComparer<WeaponManager.WeaponTypeForLow>
	{
		// Token: 0x060044D5 RID: 17621 RVA: 0x00173250 File Offset: 0x00171450
		public bool Equals(WeaponManager.WeaponTypeForLow x, WeaponManager.WeaponTypeForLow y)
		{
			return x == y;
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x00173258 File Offset: 0x00171458
		public int GetHashCode(WeaponManager.WeaponTypeForLow obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x0200078A RID: 1930
	public struct infoClient
	{
		// Token: 0x04003225 RID: 12837
		public string ipAddress;

		// Token: 0x04003226 RID: 12838
		public string name;

		// Token: 0x04003227 RID: 12839
		public string coments;
	}
}
