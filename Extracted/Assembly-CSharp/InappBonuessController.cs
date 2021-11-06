using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200066F RID: 1647
internal sealed class InappBonuessController
{
	// Token: 0x0600394C RID: 14668 RVA: 0x001295CC File Offset: 0x001277CC
	private InappBonuessController()
	{
	}

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x0600394E RID: 14670 RVA: 0x001295D8 File Offset: 0x001277D8
	// (remove) Token: 0x0600394F RID: 14671 RVA: 0x001295F0 File Offset: 0x001277F0
	public static event Action<InappRememberedBonus> OnGiveInappBonus;

	// Token: 0x17000965 RID: 2405
	// (get) Token: 0x06003950 RID: 14672 RVA: 0x00129608 File Offset: 0x00127808
	public static InappBonuessController Instance
	{
		get
		{
			if (InappBonuessController.s_instance == null)
			{
				InappBonuessController.s_instance = new InappBonuessController();
			}
			return InappBonuessController.s_instance;
		}
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x00129624 File Offset: 0x00127824
	public bool InappBonusAlreadyBought(Dictionary<string, object> bonus)
	{
		if (bonus == null)
		{
			return false;
		}
		bool result;
		try
		{
			string item = bonus["Key"] as string;
			result = BalanceController.keysInappBonusActionGiven.Contains(item);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonusAlreadyBought: {0}", new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x001296A0 File Offset: 0x001278A0
	public static Dictionary<string, object> FindInappBonusInBonuses(Dictionary<string, object> bonusToFind, List<Dictionary<string, object>> whereToFind)
	{
		if (bonusToFind == null)
		{
			Debug.LogErrorFormat("FindInappBonusInBonuses: bonusToFind = null", new object[0]);
			return null;
		}
		if (whereToFind == null)
		{
			Debug.LogWarning("FindInappBonusInBonuses: whereToFind = null");
			return null;
		}
		Dictionary<string, object> result;
		try
		{
			string keyOfToFind = bonusToFind["Key"] as string;
			Dictionary<string, object> dictionary = whereToFind.FirstOrDefault(delegate(Dictionary<string, object> bonus)
			{
				string a = bonus["Key"] as string;
				return a == keyOfToFind;
			});
			if (dictionary == null)
			{
				result = null;
			}
			else
			{
				object obj;
				if (bonusToFind.TryGetValue("Weapon", out obj))
				{
					string text = obj as string;
					if (!text.IsNullOrEmpty() && dictionary["Weapon"] as string != text)
					{
						return null;
					}
				}
				object obj2;
				if (bonusToFind.TryGetValue("Gadgets", out obj2))
				{
					List<string> list = obj2 as List<string>;
					if (list != null)
					{
						List<string> source = dictionary["Gadgets"] as List<string>;
						if (!(from x in list
						orderby x
						select x).SequenceEqual(from x in source
						orderby x
						select x))
						{
							return null;
						}
					}
				}
				result = dictionary;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in FindInappBonusInBonuses: {0}", new object[]
			{
				ex
			});
			result = null;
		}
		return result;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x0012983C File Offset: 0x00127A3C
	public static bool AreInappBonusesEquals(List<Dictionary<string, object>> left, List<Dictionary<string, object>> right)
	{
		if (left == null && right == null)
		{
			return true;
		}
		if (left == null || right == null)
		{
			return false;
		}
		if (left.Count != right.Count)
		{
			return false;
		}
		foreach (Dictionary<string, object> bonusToFind in left)
		{
			if (InappBonuessController.FindInappBonusInBonuses(bonusToFind, right) == null)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003954 RID: 14676 RVA: 0x001298DC File Offset: 0x00127ADC
	public void RememberCurrentBonusForInapp(string inappId, Dictionary<string, object> currentInAppBonus)
	{
		if (inappId.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("RememberCurrentBonusForInapp: inappId.IsNullOrEmpty()", new object[0]);
			return;
		}
		try
		{
			InappBonuessController.InappRememberedBonuses inappRememberedBonuses = this.LoadBonusesFromDisk();
			inappRememberedBonuses.Bonuses.RemoveAll((InappRememberedBonus rememberedBonus) => rememberedBonus != null && rememberedBonus.InappId == inappId);
			if (currentInAppBonus != null)
			{
				if (Convert.ToString(currentInAppBonus["ID"]) == inappId)
				{
					InappRememberedBonus actualBonusSizeForInappBonus = this.GetActualBonusSizeForInappBonus(currentInAppBonus);
					if (actualBonusSizeForInappBonus != null)
					{
						inappRememberedBonuses.Bonuses.Add(actualBonusSizeForInappBonus);
					}
					else
					{
						Debug.LogErrorFormat("RememberCurrentBonusForInapp: bonusForThisInapp == null inappId = {0}, currentInAppBonus = {1}", new object[]
						{
							inappId,
							Json.Serialize(currentInAppBonus)
						});
					}
				}
				else
				{
					Debug.LogErrorFormat("RememberCurrentBonusForInapp Convert.ToString( currentInAppBonus[ \"ID\" ] ) != inappId, inappId = {0}, currentInAppBonus = {1}", new object[]
					{
						inappId,
						Json.Serialize(currentInAppBonus)
					});
				}
			}
			this.SaveBonusesToDisk(inappRememberedBonuses);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in RememberCurrentBonusForInapp: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06003955 RID: 14677 RVA: 0x00129A08 File Offset: 0x00127C08
	internal static void GiveWeapon(string weaponId)
	{
		if (!weaponId.IsNullOrEmpty())
		{
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(weaponId);
			WeaponSounds weaponInfo = ItemDb.GetWeaponInfo(weaponId);
			ShopNGUIController.ProvideItem(itemCategory, weaponId, 1, false, 0, delegate(string item)
			{
				bool flag = Defs.isDaterRegim && weaponInfo != null && !weaponInfo.IsAvalibleFromFilter(3);
				if (flag)
				{
					return;
				}
				if (Defs.isHunger)
				{
					return;
				}
				if (ShopNGUIController.sharedShop != null)
				{
					ShopNGUIController.sharedShop.FireBuyAction(item);
				}
			}, null, true, true, false);
			if (WeaponManager.sharedManager != null)
			{
				bool isHunger = false;
				if (SceneManagerHelper.ActiveSceneName == "ConnectScene")
				{
					isHunger = Defs.isHunger;
					Defs.isHunger = false;
				}
				int currentWeaponIndex = WeaponManager.sharedManager.CurrentWeaponIndex;
				WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
				WeaponManager.sharedManager.CurrentWeaponIndex = currentWeaponIndex;
				if (SceneManagerHelper.ActiveSceneName == "ConnectScene")
				{
					Defs.isHunger = isHunger;
				}
			}
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
			{
				ShopNGUIController.sharedShop.UpdateIcons(false);
				ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory, null, false);
				if (ArmoryInfoScreenController.sharedController != null)
				{
					ArmoryInfoScreenController.sharedController.DestroyWindow();
				}
			}
		}
	}

	// Token: 0x06003956 RID: 14678 RVA: 0x00129B24 File Offset: 0x00127D24
	internal static void GiveLeprechaun(int daysLeprechaun, string currencyLeprechaun, int perDayLeprechaun)
	{
		try
		{
			Singleton<LeprechauntManager>.Instance.RemoveLeprechaunt();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveLeprechaunForInapp RemoveLeprechaunt: {0}", new object[]
			{
				ex
			});
		}
		try
		{
			Singleton<LeprechauntManager>.Instance.SetLeprechaunt(86400 * daysLeprechaun, currencyLeprechaun, perDayLeprechaun, 86400);
		}
		catch (Exception ex2)
		{
			Debug.LogErrorFormat("Exception in GiveLeprechaunForInapp SetLeprechaunt: {0}", new object[]
			{
				ex2
			});
		}
	}

	// Token: 0x06003957 RID: 14679 RVA: 0x00129BC8 File Offset: 0x00127DC8
	internal static void GivePets(string petId, int quantity)
	{
		if (!petId.IsNullOrEmpty())
		{
			if (quantity <= 0)
			{
				Debug.LogErrorFormat("GiveBonusForInapp: giving pet, quantity <= 0", new object[0]);
			}
			PetUpdateInfo petUpdateInfo = null;
			for (int i = 0; i < quantity; i++)
			{
				try
				{
					petUpdateInfo = Singleton<PetsManager>.Instance.AddOrUpdatePet(petId);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in GiveBonusForInapp giving pet: {0}", new object[]
					{
						ex
					});
				}
			}
			try
			{
				if (petUpdateInfo != null)
				{
					ShopNGUIController.sharedShop.EquipPetAndUpdate(petUpdateInfo.PetNew.InfoId);
					Singleton<PetsManager>.Instance.ActualizeEquippedPet();
				}
				else
				{
					Debug.LogErrorFormat("GiveBonusForInapp petUpdateInfo == null", new object[0]);
				}
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					ShopNGUIController.sharedShop.UpdateIcons(false);
					ShopNGUIController.sharedShop.UpdatePetsCategoryIfNeeded();
					if (ArmoryInfoScreenController.sharedController != null)
					{
						ArmoryInfoScreenController.sharedController.DestroyWindow();
					}
				}
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in GiveBonusForInapp equip pet and update: {0}", new object[]
				{
					ex2
				});
			}
		}
	}

	// Token: 0x06003958 RID: 14680 RVA: 0x00129D0C File Offset: 0x00127F0C
	private static bool IsInMatch()
	{
		return WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null;
	}

	// Token: 0x06003959 RID: 14681 RVA: 0x00129D40 File Offset: 0x00127F40
	internal static void GiveGadgets(List<string> gadgetIds)
	{
		bool flag = InappBonuessController.IsInMatch();
		for (int i = 0; i < gadgetIds.Count; i++)
		{
			string text = GadgetsInfo.FirstUnboughtOrForOurTier(gadgetIds[i]);
			if (text == null)
			{
				Debug.LogErrorFormat("GiveGadgets: firstUnboughtOrForOurTier == null", new object[0]);
			}
			else
			{
				GadgetsInfo.ProvideGadget(text);
				if (!flag)
				{
					ShopNGUIController.EquipGadget(text, (GadgetInfo.GadgetCategory)ItemDb.GetItemCategory(text));
					GadgetsInfo.ActualizeEquippedGadgets();
				}
			}
		}
		if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
		{
			ShopNGUIController.sharedShop.UpdateIcons(false);
			ShopNGUIController.sharedShop.ChooseCategory(ShopNGUIController.sharedShop.CurrentCategory, null, false);
			if (ArmoryInfoScreenController.sharedController != null)
			{
				ArmoryInfoScreenController.sharedController.DestroyWindow();
			}
		}
	}

	// Token: 0x0600395A RID: 14682 RVA: 0x00129E04 File Offset: 0x00128004
	public string GiveBonusForInapp(string inappId)
	{
		if (inappId.IsNullOrEmpty())
		{
			Debug.LogErrorFormat("GiveBonusForInapp: inappId.IsNullOrEmpty()", new object[0]);
			return null;
		}
		try
		{
			InappBonuessController.InappRememberedBonuses inappRememberedBonuses = this.LoadBonusesFromDisk();
			InappRememberedBonus inappRememberedBonus = inappRememberedBonuses.Bonuses.FirstOrDefault((InappRememberedBonus bonus) => bonus != null && bonus.InappId == inappId);
			if (inappRememberedBonus == null)
			{
				return null;
			}
			if (inappRememberedBonus.Coins > 0)
			{
				BankController.AddCoins(inappRememberedBonus.Coins, true, AnalyticsConstants.AccrualType.Purchased);
			}
			if (inappRememberedBonus.Gems > 0)
			{
				BankController.AddGems(inappRememberedBonus.Gems, true, AnalyticsConstants.AccrualType.Purchased);
			}
			InappBonuessController.GivePetForInapp(inappRememberedBonus);
			if (!inappRememberedBonus.ActionStartTime.IsNullOrEmpty())
			{
				BalanceController.AddKeysInappBonusActionGiven(inappRememberedBonus.ActionStartTime);
			}
			else
			{
				Debug.LogErrorFormat("GiveBonusForInapp: bonusForThisInapp.ActionStartTime.IsNullOrEmpty(), inappId = {0}", new object[]
				{
					inappRememberedBonus.InappId ?? "null"
				});
			}
			InappBonuessController.GiveWeaponForInapp(inappRememberedBonus);
			InappBonuessController.GiveGadgetsForInapp(inappRememberedBonus);
			InappBonuessController.GiveLeprechaunForInapp(inappRememberedBonus);
			Action<InappRememberedBonus> onGiveInappBonus = InappBonuessController.OnGiveInappBonus;
			if (onGiveInappBonus != null)
			{
				onGiveInappBonus(inappRememberedBonus);
			}
			return inappRememberedBonus.ActionStartTime;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveBonusForInapp: {0}, inappId = {1}", new object[]
			{
				ex,
				inappId
			});
		}
		return null;
	}

	// Token: 0x0600395B RID: 14683 RVA: 0x00129F6C File Offset: 0x0012816C
	public InappRememberedBonus GetActualBonusSizeForInappBonus(Dictionary<string, object> inappBonus)
	{
		InappRememberedBonus result;
		try
		{
			if (inappBonus == null)
			{
				Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: inappBonus == null", new object[0]);
				result = null;
			}
			else
			{
				int num = 1;
				int num2 = 0;
				object value;
				if (inappBonus.TryGetValue("Coins", out value))
				{
					num2 = Convert.ToInt32(value);
				}
				int num3 = 0;
				object value2;
				if (inappBonus.TryGetValue("GemsCurrency", out value2))
				{
					num3 = Convert.ToInt32(value2);
				}
				if (num2 == 0 && num3 == 0)
				{
					Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: coins == 0 && gems == 0, inappBonus = {0}", new object[]
					{
						Json.Serialize(inappBonus)
					});
				}
				string text;
				if (inappBonus.TryGetValue("Key", out text))
				{
					text = (text ?? string.Empty);
				}
				else
				{
					Debug.LogErrorFormat("GetActualBonusSizeForInappBonus: !inappBonus.TryGetValue( \"Key\" , out actionStartTime ), inappBonus = {0}", new object[]
					{
						Json.Serialize(inappBonus)
					});
					text = string.Empty;
				}
				string petId = string.Empty;
				try
				{
					object obj;
					if (inappBonus.TryGetValue("Pet", out obj))
					{
						petId = ((obj as string) ?? string.Empty);
					}
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting petId: {0}", new object[]
					{
						ex
					});
				}
				int quantity = 0;
				try
				{
					object value3;
					if (inappBonus.TryGetValue("Quantity", out value3))
					{
						quantity = Convert.ToInt32(value3);
					}
				}
				catch (Exception ex2)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting quantity: {0}", new object[]
					{
						ex2
					});
				}
				string weaponId = string.Empty;
				try
				{
					object obj2;
					if (inappBonus.TryGetValue("Weapon", out obj2))
					{
						weaponId = ((obj2 as string) ?? string.Empty);
					}
				}
				catch (Exception ex3)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting weapon: {0}", new object[]
					{
						ex3
					});
				}
				List<string> list = null;
				try
				{
					object obj3;
					if (inappBonus.TryGetValue("Gadgets", out obj3))
					{
						list = (obj3 as List<string>).OfType<string>().ToList<string>();
					}
				}
				catch (Exception ex4)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting gadget ids: {0}", new object[]
					{
						ex4
					});
				}
				if (list == null)
				{
					list = new List<string>();
				}
				string currencyLeprechaun = string.Empty;
				try
				{
					object obj4;
					if (inappBonus.TryGetValue("CurrencyLeprechaun", out obj4))
					{
						currencyLeprechaun = ((obj4 as string) ?? string.Empty);
					}
				}
				catch (Exception ex5)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr currency: {0}", new object[]
					{
						ex5
					});
				}
				int daysLeprechaun = 0;
				try
				{
					object value4;
					if (inappBonus.TryGetValue("DaysLeprechaun", out value4))
					{
						daysLeprechaun = Convert.ToInt32(value4);
					}
				}
				catch (Exception ex6)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr days: {0}", new object[]
					{
						ex6
					});
				}
				int perDayLeprechaun = 0;
				try
				{
					object value5;
					if (inappBonus.TryGetValue("PerDayLeprechaun", out value5))
					{
						perDayLeprechaun = Convert.ToInt32(value5);
					}
				}
				catch (Exception ex7)
				{
					Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus, getting lepr per day: {0}", new object[]
					{
						ex7
					});
				}
				InappRememberedBonus inappRememberedBonus = new InappRememberedBonus
				{
					InappId = Convert.ToString(inappBonus["ID"]),
					Coins = num2 * num,
					Gems = num3 * num,
					ActionStartTime = text,
					PetId = petId,
					Quantity = quantity,
					WeaponId = weaponId,
					GadgetIds = list,
					CurrencyLeprechaun = currencyLeprechaun,
					DaysLeprechaun = daysLeprechaun,
					PerDayLeprechaun = perDayLeprechaun
				};
				result = inappRememberedBonus;
			}
		}
		catch (Exception ex8)
		{
			Debug.LogErrorFormat("Exception in GetActualBonusSizeForInappBonus: {0}", new object[]
			{
				ex8
			});
			result = null;
		}
		return result;
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x0012A3B4 File Offset: 0x001285B4
	private static void GivePetForInapp(InappRememberedBonus bonusForThisInapp)
	{
		string petId = bonusForThisInapp.PetId;
		int quantity = bonusForThisInapp.Quantity;
		InappBonuessController.GivePets(petId, quantity);
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x0012A3D8 File Offset: 0x001285D8
	private static void GiveWeaponForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			string weaponId = bonusForThisInapp.WeaponId;
			InappBonuessController.GiveWeapon(weaponId);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveBonusForInapp: giving weapon: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x0012A430 File Offset: 0x00128630
	private static void GiveLeprechaunForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			if (bonusForThisInapp != null && bonusForThisInapp.DaysLeprechaun > 0 && bonusForThisInapp.PerDayLeprechaun > 0 && (bonusForThisInapp.CurrencyLeprechaun == "Coins" || bonusForThisInapp.CurrencyLeprechaun == "GemsCurrency"))
			{
				int daysLeprechaun = bonusForThisInapp.DaysLeprechaun;
				string currencyLeprechaun = bonusForThisInapp.CurrencyLeprechaun;
				int perDayLeprechaun = bonusForThisInapp.PerDayLeprechaun;
				InappBonuessController.GiveLeprechaun(daysLeprechaun, currencyLeprechaun, perDayLeprechaun);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveGadgetsForInapp: giving weapon: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x0012A4E4 File Offset: 0x001286E4
	private static void GiveGadgetsForInapp(InappRememberedBonus bonusForThisInapp)
	{
		try
		{
			if (bonusForThisInapp != null && bonusForThisInapp.GadgetIds != null && bonusForThisInapp.GadgetIds.Count > 0)
			{
				List<string> gadgetIds = bonusForThisInapp.GadgetIds;
				InappBonuessController.GiveGadgets(gadgetIds);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GiveGadgetsForInapp: giving weapon: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x0012A560 File Offset: 0x00128760
	private void SaveBonusesToDisk(InappBonuessController.InappRememberedBonuses bonuses)
	{
		if (bonuses == null)
		{
			Debug.LogErrorFormat("SaveBonusesToDisk: bonuses == null", new object[0]);
			return;
		}
		try
		{
			string val = JsonUtility.ToJson(bonuses);
			Storager.setString("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY", val, false);
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonuessController.SaveBonusesToDisk: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x0012A5D4 File Offset: 0x001287D4
	private InappBonuessController.InappRememberedBonuses LoadBonusesFromDisk()
	{
		if (!Storager.hasKey("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY"))
		{
			InappBonuessController.InappRememberedBonuses bonuses = new InappBonuessController.InappRememberedBonuses
			{
				Bonuses = new List<InappRememberedBonus>()
			};
			this.SaveBonusesToDisk(bonuses);
		}
		InappBonuessController.InappRememberedBonuses result;
		try
		{
			string @string = Storager.getString("InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY", false);
			InappBonuessController.InappRememberedBonuses inappRememberedBonuses = JsonUtility.FromJson<InappBonuessController.InappRememberedBonuses>(@string);
			result = inappRememberedBonuses;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in InappBonuessController.LoadBonusesFromDisk: {0}", new object[]
			{
				ex
			});
			result = new InappBonuessController.InappRememberedBonuses
			{
				Bonuses = new List<InappRememberedBonus>()
			};
		}
		return result;
	}

	// Token: 0x04002A14 RID: 10772
	private const string REMEMBERED_BONUSES_STORAGE_KEY = "InappBonuessController.REMEMBERED_BONUSES_STORAGE_KEY";

	// Token: 0x04002A15 RID: 10773
	private static InappBonuessController s_instance;

	// Token: 0x02000670 RID: 1648
	[Serializable]
	private class InappRememberedBonuses
	{
		// Token: 0x04002A19 RID: 10777
		public List<InappRememberedBonus> Bonuses;
	}
}
