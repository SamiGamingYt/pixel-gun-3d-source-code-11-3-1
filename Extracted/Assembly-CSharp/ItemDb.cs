using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public static class ItemDb
{
	// Token: 0x06004A39 RID: 19001 RVA: 0x0019B9C0 File Offset: 0x00199BC0
	static ItemDb()
	{
		for (int i = 0; i < ItemDb._records.Count; i++)
		{
			ItemRecord itemRecord = ItemDb._records[i];
			ItemDb._recordsById[itemRecord.Id] = itemRecord;
			if (!string.IsNullOrEmpty(itemRecord.Tag))
			{
				ItemDb._recordsByTag[itemRecord.Tag] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.StorageId))
			{
				ItemDb._recordsByStorageId[itemRecord.StorageId] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.ShopId))
			{
				ItemDb._recordsByShopId[itemRecord.ShopId] = itemRecord;
			}
			if (!string.IsNullOrEmpty(itemRecord.PrefabName))
			{
				ItemDb._recordsByPrefabName[itemRecord.PrefabName] = itemRecord;
			}
		}
	}

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0019BB70 File Offset: 0x00199D70
	public static Dictionary<string, ItemRecord> RecordsByTag
	{
		get
		{
			return ItemDb._recordsByTag;
		}
	}

	// Token: 0x06004A3B RID: 19003 RVA: 0x0019BB78 File Offset: 0x00199D78
	public static string GetItemRarityLocalizeName(ItemDb.ItemRarity _itemRarity)
	{
		return ItemDb.ItemRarityBBCodes[(int)_itemRarity] + LocalizationStore.Get(ItemDb.ItemRarityLocalizeKeys[(int)_itemRarity]);
	}

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0019BB94 File Offset: 0x00199D94
	public static List<ItemRecord> allRecords
	{
		get
		{
			return ItemDb._records;
		}
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x0019BB9C File Offset: 0x00199D9C
	public static Dictionary<string, ItemRecord> allRecordsWithStorageIds
	{
		get
		{
			return ItemDb._recordsByStorageId;
		}
	}

	// Token: 0x06004A3E RID: 19006 RVA: 0x0019BBA4 File Offset: 0x00199DA4
	public static ItemRecord GetById(int id)
	{
		if (ItemDb._recordsById.ContainsKey(id))
		{
			return ItemDb._recordsById[id];
		}
		return null;
	}

	// Token: 0x06004A3F RID: 19007 RVA: 0x0019BBC4 File Offset: 0x00199DC4
	public static ItemRecord GetByTag(string tag)
	{
		if (tag == null)
		{
			return null;
		}
		ItemRecord result;
		if (ItemDb._recordsByTag.TryGetValue(tag, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004A40 RID: 19008 RVA: 0x0019BBF0 File Offset: 0x00199DF0
	public static ItemRecord GetByPrefabName(string prefabName)
	{
		if (prefabName == null)
		{
			return null;
		}
		ItemRecord result;
		if (ItemDb._recordsByPrefabName.TryGetValue(prefabName, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004A41 RID: 19009 RVA: 0x0019BC1C File Offset: 0x00199E1C
	public static ItemRecord GetByShopId(string shopId)
	{
		if (shopId == null)
		{
			return null;
		}
		ItemRecord result;
		if (ItemDb._recordsByShopId.TryGetValue(shopId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06004A42 RID: 19010 RVA: 0x0019BC48 File Offset: 0x00199E48
	public static ItemPrice GetPriceByShopId(string shopId, ShopNGUIController.CategoryNames category)
	{
		if (shopId == null)
		{
			return null;
		}
		if (BalanceController.pricesFromServer.ContainsKey(shopId))
		{
			return BalanceController.pricesFromServer[shopId];
		}
		if (category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(shopId);
			if (skin != null)
			{
				return new ItemPrice(skin.Price, (skin.Currency != VirtualCurrencyBonusType.Gem) ? "Coins" : "GemsCurrency");
			}
			return new ItemPrice(0, "Coins");
		}
		else
		{
			ItemRecord itemRecord;
			if (ItemDb._recordsByShopId.TryGetValue(shopId, out itemRecord))
			{
				return itemRecord.Price;
			}
			return VirtualCurrencyHelper.Price(shopId);
		}
	}

	// Token: 0x06004A43 RID: 19011 RVA: 0x0019BCE4 File Offset: 0x00199EE4
	public static bool IsCanBuy(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		return byTag != null && byTag.CanBuy;
	}

	// Token: 0x06004A44 RID: 19012 RVA: 0x0019BD08 File Offset: 0x00199F08
	public static string GetShopIdByTag(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		return (byTag == null) ? null : byTag.ShopId;
	}

	// Token: 0x06004A45 RID: 19013 RVA: 0x0019BD30 File Offset: 0x00199F30
	public static string GetTagByShopId(string shopId)
	{
		ItemRecord byShopId = ItemDb.GetByShopId(shopId);
		return (byShopId == null) ? null : byShopId.Tag;
	}

	// Token: 0x06004A46 RID: 19014 RVA: 0x0019BD58 File Offset: 0x00199F58
	public static string GetStorageIdByTag(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		return (byTag == null) ? null : byTag.StorageId;
	}

	// Token: 0x06004A47 RID: 19015 RVA: 0x0019BD80 File Offset: 0x00199F80
	public static string GetStorageIdByShopId(string shopId)
	{
		ItemRecord byShopId = ItemDb.GetByShopId(shopId);
		return (byShopId == null) ? null : byShopId.StorageId;
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x0019BDA8 File Offset: 0x00199FA8
	public static IEnumerable<ItemRecord> GetCanBuyWeapon(bool includeDeactivated = false)
	{
		if (includeDeactivated)
		{
			return from item in ItemDb._records
			where item.CanBuy
			select item;
		}
		return from item in ItemDb._records
		where item.CanBuy && !item.Deactivated
		select item;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x0019BE0C File Offset: 0x0019A00C
	public static string[] GetCanBuyWeaponTags(bool includeDeactivated = false)
	{
		return (from item in ItemDb.GetCanBuyWeapon(includeDeactivated)
		select item.Tag).ToArray<string>();
	}

	// Token: 0x06004A4A RID: 19018 RVA: 0x0019BE3C File Offset: 0x0019A03C
	public static List<string> GetCanBuyWeaponStorageIds(bool includeDeactivated = false)
	{
		return (from item in ItemDb.GetCanBuyWeapon(includeDeactivated)
		where item.StorageId != null
		select item.StorageId).ToList<string>();
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x0019BE98 File Offset: 0x0019A098
	public static void Fill_tagToStoreIDMapping(Dictionary<string, string> tagToStoreIDMapping)
	{
		tagToStoreIDMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> keyValuePair in ItemDb._recordsByTag)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Value.ShopId))
			{
				tagToStoreIDMapping[keyValuePair.Key] = keyValuePair.Value.ShopId;
			}
		}
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x0019BF2C File Offset: 0x0019A12C
	public static void Fill_storeIDtoDefsSNMapping(Dictionary<string, string> storeIDtoDefsSNMapping)
	{
		storeIDtoDefsSNMapping.Clear();
		foreach (KeyValuePair<string, ItemRecord> keyValuePair in ItemDb._recordsByShopId)
		{
			if (!string.IsNullOrEmpty(keyValuePair.Value.StorageId))
			{
				storeIDtoDefsSNMapping[keyValuePair.Key] = keyValuePair.Value.StorageId;
			}
		}
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x0019BFC0 File Offset: 0x0019A1C0
	public static bool IsTemporaryGun(string tg)
	{
		if (tg == null)
		{
			return false;
		}
		ItemRecord byTag = ItemDb.GetByTag(tg);
		return byTag != null && byTag.TemporaryGun;
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x0019BFF0 File Offset: 0x0019A1F0
	public static bool IsWeaponCanDrop(string tag)
	{
		if (tag == "Knife")
		{
			return false;
		}
		ItemRecord byTag = ItemDb.GetByTag(tag);
		return byTag != null && !byTag.CanBuy;
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x0019C028 File Offset: 0x0019A228
	public static void InitStorageForTag(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag == null)
		{
			Debug.LogWarning("item didn't found for tag: " + tag);
			return;
		}
		if (string.IsNullOrEmpty(byTag.StorageId))
		{
			Debug.LogWarning("StoragId is null or empty for tag: " + tag);
			return;
		}
		Storager.setInt(byTag.StorageId, 0, false);
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x0019C084 File Offset: 0x0019A284
	public static int GetItemCategory(string tag)
	{
		int num = -1;
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>(string.Format("Weapons/{0}", byTag.PrefabName));
			return (!(weaponSounds != null)) ? -1 : (weaponSounds.categoryNabor - 1);
		}
		if (num == -1)
		{
			GadgetInfo gadgetInfo = null;
			if (GadgetsInfo.info.TryGetValue(tag, out gadgetInfo) && gadgetInfo != null)
			{
				num = (int)gadgetInfo.Category;
			}
		}
		if (num == -1)
		{
			ShopNGUIController.CategoryNames categoryNames = Wear.wear.Keys.FirstOrDefault((ShopNGUIController.CategoryNames wearCategory) => Wear.wear[wearCategory].FirstOrDefault((List<string> l) => l.Contains(tag)) != null);
			if (ShopNGUIController.IsWearCategory(categoryNames))
			{
				num = (int)categoryNames;
			}
		}
		if (num == -1 && (SkinsController.skinsNamesForPers.ContainsKey(tag) || tag.Equals("CustomSkinID")))
		{
			num = 8;
		}
		if (num == -1 && WeaponSkinsManager.SkinIds.Contains(tag))
		{
			num = 2000;
		}
		if (num == -1)
		{
			try
			{
				if ((from eggData in Singleton<EggsManager>.Instance.GetAllEggs()
				select eggData.Id).Contains(tag))
				{
					num = 30000;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in checking is eggs category: {0}", new object[]
				{
					ex
				});
			}
		}
		if (num == -1)
		{
			try
			{
				if (PetsInfo.info.ContainsKey(tag))
				{
					num = 25000;
				}
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in checking is pets category: {0}", new object[]
				{
					ex2
				});
			}
		}
		if (num == -1 && GearManager.IsItemGear(tag))
		{
			num = 11;
		}
		return num;
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x0019C2A0 File Offset: 0x0019A4A0
	public static int[] GetItemFilterMap(string tag)
	{
		ItemRecord byTag = ItemDb.GetByTag(tag);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			return (!(weaponSounds != null)) ? new int[0] : ((weaponSounds.filterMap == null) ? new int[0] : weaponSounds.filterMap);
		}
		return new int[0];
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x0019C30C File Offset: 0x0019A50C
	public static ShopPositionParams GetInfoForNonWeaponItem(string name, ShopNGUIController.CategoryNames category)
	{
		ShopPositionParams result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.Load<ShopPositionParams>("Armor_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.Load<ShopPositionParams>("Hats_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.Load<ShopPositionParams>("Capes_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.Load<ShopPositionParams>("Shop_Boots_Info/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.Load<ShopPositionParams>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.Load<ShopPositionParams>("Masks_Info/" + name);
		}
		return result;
	}

	// Token: 0x06004A53 RID: 19027 RVA: 0x0019C3B0 File Offset: 0x0019A5B0
	public static GameObject GetWearFromResources(string name, ShopNGUIController.CategoryNames category)
	{
		GameObject result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.Load<GameObject>("Armor/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.Load<GameObject>("Hats/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.Load<GameObject>("Capes/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.Load<GameObject>("Shop_Boots/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.Load<GameObject>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.Load<GameObject>("Masks/" + name);
		}
		return result;
	}

	// Token: 0x06004A54 RID: 19028 RVA: 0x0019C454 File Offset: 0x0019A654
	public static ResourceRequest GetWearFromResourcesAsync(string name, ShopNGUIController.CategoryNames category)
	{
		ResourceRequest result = null;
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			result = Resources.LoadAsync<GameObject>("Armor/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			result = Resources.LoadAsync<GameObject>("Hats/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.CapesCategory)
		{
			result = Resources.LoadAsync<GameObject>("Capes/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.BootsCategory)
		{
			result = Resources.LoadAsync<GameObject>("Shop_Boots/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.GearCategory)
		{
			result = Resources.LoadAsync<GameObject>("Shop_Gear/" + name);
		}
		if (category == ShopNGUIController.CategoryNames.MaskCategory)
		{
			result = Resources.LoadAsync<GameObject>("Masks/" + name);
		}
		return result;
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x0019C4F8 File Offset: 0x0019A6F8
	public static string GetItemName(string tag, ShopNGUIController.CategoryNames category)
	{
		if (string.IsNullOrEmpty(tag))
		{
			return string.Empty;
		}
		if (category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(tag);
			if (playerPet != null)
			{
				return playerPet.PetName;
			}
			return string.Empty;
		}
		else if (ShopNGUIController.IsGadgetsCategory(category))
		{
			List<string> list = GadgetsInfo.Upgrades[tag];
			GadgetInfo gadgetInfo;
			if (list != null && GadgetsInfo.info.TryGetValue(list[0], out gadgetInfo))
			{
				return LocalizationStore.Get(gadgetInfo.Lkey);
			}
			return string.Empty;
		}
		else
		{
			if (category == ShopNGUIController.CategoryNames.EggsCategory)
			{
				try
				{
					return LocalizationStore.Get(Singleton<EggsManager>.Instance.GetEggData(tag).LKey);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in getting egg localized name: {0}", new object[]
					{
						ex
					});
				}
				return tag;
			}
			ItemRecord byTag = ItemDb.GetByTag(tag);
			if (byTag != null)
			{
				WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
				string result = string.Empty;
				if (weaponSounds != null)
				{
					try
					{
						ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponSounds.name.Replace("(Clone)", string.Empty));
						string tag2 = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
						ItemRecord byTag2 = ItemDb.GetByTag(tag2);
						WeaponSounds weaponSounds2 = Resources.Load<WeaponSounds>(string.Format("Weapons/{0}", byTag2.PrefabName));
						result = weaponSounds2.shopName;
					}
					catch (Exception arg)
					{
						Debug.LogError("Error in getting shop name of first upgrade: " + arg);
						result = weaponSounds.shopName;
					}
				}
				return result;
			}
			ShopPositionParams infoForNonWeaponItem = ItemDb.GetInfoForNonWeaponItem(tag, category);
			if (infoForNonWeaponItem != null)
			{
				return infoForNonWeaponItem.shopName;
			}
			return string.Empty;
		}
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x0019C6E8 File Offset: 0x0019A8E8
	public static string GetItemNameNonLocalized(string itemId, string shopId, ShopNGUIController.CategoryNames category, string defaultDesc = null)
	{
		ItemRecord byTag = ItemDb.GetByTag(itemId);
		if (byTag != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + byTag.PrefabName);
			return (!(weaponSounds != null)) ? string.Empty : weaponSounds.shopNameNonLocalized;
		}
		if (category == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			return Resources.Load<ShopPositionParams>("Armor_Info/" + itemId).shopNameNonLocalized;
		}
		if (category == ShopNGUIController.CategoryNames.HatsCategory)
		{
			ShopPositionParams shopPositionParams = Resources.Load<ShopPositionParams>("Hats_Info/" + itemId);
			return (!(shopPositionParams != null)) ? string.Empty : shopPositionParams.shopNameNonLocalized;
		}
		if (category == ShopNGUIController.CategoryNames.SkinsCategory)
		{
			return (!SkinsController.shopKeyFromNameSkin.ContainsKey(shopId)) ? shopId : SkinsController.shopKeyFromNameSkin[shopId];
		}
		if (category == ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(itemId);
			return (skin == null || skin.Lkey == null) ? itemId : LocalizationStore.GetByDefault(skin.Lkey).ToUpper();
		}
		if (category == ShopNGUIController.CategoryNames.EggsCategory)
		{
			try
			{
				EggData eggData = Singleton<EggsManager>.Instance.GetEggData(itemId);
				return (eggData == null || eggData.LKey == null) ? itemId : LocalizationStore.GetByDefault(eggData.LKey).ToUpper();
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in getting egg non localized name: {0}", new object[]
				{
					ex
				});
			}
		}
		if (category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			return itemId;
		}
		if (ShopNGUIController.IsGadgetsCategory(category))
		{
			return itemId;
		}
		if (InAppData.inappReadableNames.ContainsKey(shopId))
		{
			return InAppData.inappReadableNames[shopId];
		}
		return defaultDesc ?? shopId;
	}

	// Token: 0x06004A57 RID: 19031 RVA: 0x0019C8B4 File Offset: 0x0019AAB4
	public static Texture GetItemIcon(string tag, ShopNGUIController.CategoryNames category, int? upgradeNumForWeapons = null, bool useWeaponSkins = true)
	{
		if (category == (ShopNGUIController.CategoryNames)(-1))
		{
			return null;
		}
		string text = tag;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			foreach (List<string> list in WeaponUpgrades.upgrades)
			{
				int num = list.IndexOf(tag);
				if (num != -1)
				{
					text = list[0];
					if (upgradeNumForWeapons == null)
					{
						upgradeNumForWeapons = new int?(1 + num);
					}
					break;
				}
			}
		}
		int num2 = 1;
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			ItemRecord byTag = ItemDb.GetByTag(tag);
			if (!byTag.UseImagesFromFirstUpgrade && !byTag.TemporaryGun)
			{
				num2 = ((upgradeNumForWeapons == null || upgradeNumForWeapons == null) ? 1 : upgradeNumForWeapons.Value);
			}
		}
		try
		{
			if (useWeaponSkins && ShopNGUIController.IsWeaponCategory(category))
			{
				string settedSkinIdByWeaponTag = WeaponSkinsManager.GetSettedSkinIdByWeaponTag(text);
				if (!string.IsNullOrEmpty(settedSkinIdByWeaponTag))
				{
					text = settedSkinIdByWeaponTag;
					num2 = 1;
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in getting weapon skin id in GetItemIcon: " + arg);
		}
		if (ShopNGUIController.IsGadgetsCategory(category))
		{
			text = GadgetsInfo.BaseName(tag);
		}
		if (category == ShopNGUIController.CategoryNames.PetsCategory)
		{
			if (!PetsManager.IsEmptySlotId(tag))
			{
				text = Singleton<PetsManager>.Instance.GetFirstUpgrade(tag).Id;
			}
			else
			{
				text = tag;
			}
		}
		string text2 = text + "_icon" + num2.ToString() + "_big";
		if (!string.IsNullOrEmpty(text2))
		{
			return Resources.Load<Texture>("OfferIcons/" + text2);
		}
		return null;
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x0019CA8C File Offset: 0x0019AC8C
	public static Texture GetTextureItemByTag(string tag, int? upgradeNum = null)
	{
		int itemCategory = ItemDb.GetItemCategory(tag);
		return ItemDb.GetItemIcon(tag, (ShopNGUIController.CategoryNames)itemCategory, upgradeNum, true);
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x0019CAAC File Offset: 0x0019ACAC
	public static bool IsItemInInventory(string tag)
	{
		string key = tag;
		string storageIdByTag = ItemDb.GetStorageIdByTag(tag);
		if (!string.IsNullOrEmpty(storageIdByTag))
		{
			key = storageIdByTag;
		}
		return Storager.hasKey(key) && Storager.getInt(key, true) == 1;
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x0019CAE8 File Offset: 0x0019ACE8
	public static bool HasWeaponNeedUpgradesForBuyNext(string tag)
	{
		foreach (List<string> list in WeaponUpgrades.upgrades)
		{
			int num = list.IndexOf(tag);
			if (num != -1)
			{
				bool flag = true;
				for (int i = 0; i < num; i++)
				{
					flag = (flag && ItemDb.IsItemInInventory(list[i]));
				}
				return flag;
			}
		}
		return true;
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0019CB90 File Offset: 0x0019AD90
	public static string GetItemNameByTag(string tag)
	{
		int itemCategory = ItemDb.GetItemCategory(tag);
		return ItemDb.GetItemName(tag, (ShopNGUIController.CategoryNames)itemCategory);
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0019CBAC File Offset: 0x0019ADAC
	public static WeaponSounds GetWeaponInfoByPrefabName(string prefabName)
	{
		if (prefabName != null)
		{
			WeaponSounds weaponSounds = Resources.Load<WeaponSounds>("Weapons/" + prefabName);
			return (!(weaponSounds != null)) ? null : weaponSounds;
		}
		return null;
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x0019CBE8 File Offset: 0x0019ADE8
	public static WeaponSounds GetWeaponInfo(string weaponTag)
	{
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null)
		{
			return null;
		}
		return ItemDb.GetWeaponInfoByPrefabName(byTag.PrefabName);
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0019CC10 File Offset: 0x0019AE10
	public static Texture GetTextureForShopItem(string itemTag)
	{
		int value = 0;
		string text = itemTag;
		bool flag = GearManager.IsItemGear(itemTag);
		if (flag)
		{
			text = GearManager.HolderQuantityForID(itemTag);
			value = GearManager.CurrentNumberOfUphradesForGear(text) + 1;
		}
		if (flag && (text == GearManager.Turret || text == GearManager.Mech))
		{
			int? upgradeNum = new int?(value);
			if (upgradeNum != null && upgradeNum.Value > 0)
			{
				return ItemDb.GetTextureItemByTag(text, upgradeNum);
			}
		}
		return ItemDb.GetTextureItemByTag(text, null);
	}

	// Token: 0x040036FE RID: 14078
	public const int CrystalCrossbowCoinsPrice = 150;

	// Token: 0x040036FF RID: 14079
	private static string[] ItemRarityLocalizeKeys = new string[]
	{
		"Key_2394",
		"Key_2395",
		"Key_2396",
		"Key_2397",
		"Key_2398",
		"Key_2399"
	};

	// Token: 0x04003700 RID: 14080
	private static string[] ItemRarityBBCodes = new string[]
	{
		"[E3E3E3]",
		"[8DFF03]",
		"[03CDFF]",
		"[FFDD03]",
		"[FF5D5D]",
		"[C602CD]"
	};

	// Token: 0x04003701 RID: 14081
	private static List<ItemRecord> _records = ItemDbRecords.GetRecords();

	// Token: 0x04003702 RID: 14082
	private static Dictionary<int, ItemRecord> _recordsById = new Dictionary<int, ItemRecord>(ItemDb._records.Count);

	// Token: 0x04003703 RID: 14083
	private static Dictionary<string, ItemRecord> _recordsByTag = new Dictionary<string, ItemRecord>(ItemDb._records.Count);

	// Token: 0x04003704 RID: 14084
	private static Dictionary<string, ItemRecord> _recordsByStorageId = new Dictionary<string, ItemRecord>(ItemDb._records.Count);

	// Token: 0x04003705 RID: 14085
	private static Dictionary<string, ItemRecord> _recordsByShopId = new Dictionary<string, ItemRecord>(ItemDb._records.Count);

	// Token: 0x04003706 RID: 14086
	private static Dictionary<string, ItemRecord> _recordsByPrefabName = new Dictionary<string, ItemRecord>(ItemDb._records.Count);

	// Token: 0x020007FA RID: 2042
	public enum ItemRarity
	{
		// Token: 0x0400370E RID: 14094
		Common,
		// Token: 0x0400370F RID: 14095
		Uncommon,
		// Token: 0x04003710 RID: 14096
		Rare,
		// Token: 0x04003711 RID: 14097
		Epic,
		// Token: 0x04003712 RID: 14098
		Legendary,
		// Token: 0x04003713 RID: 14099
		Mythic
	}
}
