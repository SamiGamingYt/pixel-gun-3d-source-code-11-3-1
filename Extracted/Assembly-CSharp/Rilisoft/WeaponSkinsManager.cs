using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000889 RID: 2185
	public class WeaponSkinsManager
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x001C7C5C File Offset: 0x001C5E5C
		// Note: this type is marked as 'beforefieldinit'.
		static WeaponSkinsManager()
		{
			WeaponSkinsManager.EquippedSkinForWeapon = null;
		}

		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06004E97 RID: 20119 RVA: 0x001C7D00 File Offset: 0x001C5F00
		// (remove) Token: 0x06004E98 RID: 20120 RVA: 0x001C7D18 File Offset: 0x001C5F18
		public static event Action<ItemRecord, string> EquippedSkinForWeapon;

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004E99 RID: 20121 RVA: 0x001C7D30 File Offset: 0x001C5F30
		public static List<WeaponSkin> AllSkins
		{
			get
			{
				if (WeaponSkinsManager._allSkins == null)
				{
					WeaponSkinsManager._allSkins = WeaponSkinsManager.LoadSkins();
					WeaponSkinsManager._allSkins.Sort(delegate(WeaponSkin ws1, WeaponSkin ws2)
					{
						int num = ws1.ForLeague.CompareTo(ws2.ForLeague);
						if (num == 0)
						{
							return ws1.Price.CompareTo(ws2.Price);
						}
						return num;
					});
				}
				return WeaponSkinsManager._allSkins;
			}
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x001C7D80 File Offset: 0x001C5F80
		public static WeaponSkin GetSkin(string skinId)
		{
			return WeaponSkinsManager.AllSkins.FirstOrDefault((WeaponSkin s) => s.Id == skinId);
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x001C7DB0 File Offset: 0x001C5FB0
		public static List<WeaponSkin> SkinsForWeapon(string weaponName)
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
			if (byPrefabName == null)
			{
				return (from s in WeaponSkinsManager.AllSkins
				where s.ToWeapons[0] == weaponName
				select s).ToList<WeaponSkin>();
			}
			List<string> list = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
			if (list == null)
			{
				return (from s in WeaponSkinsManager.AllSkins
				where s.ToWeapons[0] == weaponName
				select s).ToList<WeaponSkin>();
			}
			ItemRecord recOfFirstUpgrade = ItemDb.GetByTag(list[0]);
			return (from s in WeaponSkinsManager.AllSkins
			where s.ToWeapons[0] == recOfFirstUpgrade.PrefabName
			select s).ToList<WeaponSkin>();
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x001C7E5C File Offset: 0x001C605C
		public static bool UnequipSkin(string skinId)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin == null)
			{
				Debug.LogError("WeaponSkinsManager UnequipSkin weaponSkinInfo == null, skinId = " + skinId);
				return false;
			}
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(skin.ToWeapons[0]);
			if (byPrefabName == null)
			{
				Debug.LogError("WeaponSkinsManager UnequipSkin itemRecord == null, skinId = " + skinId);
				return false;
			}
			Storager.setString(WeaponSkinsManager.StoragerNameForEquippedSkinForWeapon(byPrefabName.Tag), string.Empty, false);
			Action<ItemRecord, string> equippedSkinForWeapon = WeaponSkinsManager.EquippedSkinForWeapon;
			if (equippedSkinForWeapon != null)
			{
				equippedSkinForWeapon(byPrefabName, string.Empty);
			}
			WeaponSkinsManager.UpdateWeaponSkins(byPrefabName.Tag);
			return true;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x001C7EE8 File Offset: 0x001C60E8
		public static bool SetSkinToCurrentWeapon(string skinId)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin == null)
			{
				return false;
			}
			if (Storager.getInt(skin.Id, true) < 1)
			{
				return false;
			}
			if (WeaponManager.sharedManager == null || WeaponManager.sharedManager.currentWeaponSounds == null)
			{
				return false;
			}
			WeaponSounds currentWeaponSounds = WeaponManager.sharedManager.currentWeaponSounds;
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(currentWeaponSounds.nameNoClone());
			if (byPrefabName == null)
			{
				return false;
			}
			List<string> list = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
			ItemRecord itemRecord = byPrefabName;
			if (list != null)
			{
				itemRecord = ItemDb.GetByTag(list[0]);
			}
			if (skin.ToWeapons[0] != itemRecord.PrefabName)
			{
				return false;
			}
			bool flag = skin.SetTo(WeaponManager.sharedManager.currentWeaponSounds.gameObject);
			if (flag)
			{
				Storager.setString(WeaponSkinsManager.StoragerNameForEquippedSkinForWeapon(itemRecord.Tag), skin.Id, false);
				WeaponSkinsManager.UpdateWeaponSkins(byPrefabName.Tag);
			}
			return flag;
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x001C7FE0 File Offset: 0x001C61E0
		public static bool SetSkinToWeapon(string skinId, string weaponName)
		{
			if (!WeaponSkinsManager.IsBoughtSkin(skinId))
			{
				return false;
			}
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
			if (byPrefabName == null)
			{
				return false;
			}
			string text = byPrefabName.Tag;
			List<string> list = WeaponUpgrades.ChainForTag(text);
			if (list != null)
			{
				text = list[0];
			}
			Storager.setString(WeaponSkinsManager.StoragerNameForEquippedSkinForWeapon(text), skinId, false);
			Action<ItemRecord, string> equippedSkinForWeapon = WeaponSkinsManager.EquippedSkinForWeapon;
			if (equippedSkinForWeapon != null)
			{
				equippedSkinForWeapon(ItemDb.GetByTag(text), skinId);
			}
			WeaponSkinsManager.UpdateWeaponSkins(text);
			return true;
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x001C8054 File Offset: 0x001C6254
		private static void UpdateWeaponSkins(string weaponTag = null)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.ChangeWeapon(WeaponManager.sharedManager.CurrentWeaponIndex, false);
			}
			if (weaponTag != null && ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.SetWeapon(weaponTag, null);
			}
			if (PersConfigurator.currentConfigurator != null)
			{
				PersConfigurator.currentConfigurator.ResetWeapon();
			}
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.UpdateWeaponIconsForWrap();
			}
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x001C80F8 File Offset: 0x001C62F8
		public static WeaponSkin GetSkinForWeapon(string weaponName)
		{
			string settedSkinId = WeaponSkinsManager.GetSettedSkinId(weaponName);
			if (settedSkinId.IsNullOrEmpty())
			{
				return null;
			}
			return WeaponSkinsManager.GetSkin(settedSkinId);
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001C8120 File Offset: 0x001C6320
		public static string StoragerNameForEquippedSkinForWeapon(string weaponTag)
		{
			return weaponTag + "_skin";
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x001C8130 File Offset: 0x001C6330
		public static string GetSettedSkinId(string weaponName)
		{
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
			if (byPrefabName == null)
			{
				return null;
			}
			string tag = byPrefabName.Tag;
			return WeaponSkinsManager.GetSettedSkinIdByWeaponTag(tag);
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x001C815C File Offset: 0x001C635C
		public static string GetSettedSkinIdByWeaponTag(string weaponTag)
		{
			List<string> list = WeaponUpgrades.ChainForTag(weaponTag);
			if (list != null)
			{
				weaponTag = list[0];
			}
			return Storager.getString(WeaponSkinsManager.StoragerNameForEquippedSkinForWeapon(weaponTag), false);
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x001C818C File Offset: 0x001C638C
		public static List<WeaponSkin> BoughtSkins()
		{
			return (from s in WeaponSkinsManager.AllSkins
			where WeaponSkinsManager.IsBoughtSkin(s.Id)
			select s).ToList<WeaponSkin>();
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x001C81C8 File Offset: 0x001C63C8
		public static bool IsBoughtSkin(string skinId)
		{
			return Storager.getInt(skinId, true) > 0;
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x001C81D4 File Offset: 0x001C63D4
		public static bool ProvideSkin(string skinId)
		{
			if (!WeaponSkinsManager.AllSkins.Any((WeaponSkin s) => s.Id == skinId))
			{
				return false;
			}
			Storager.setInt(skinId, 1, true);
			return true;
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x001C821C File Offset: 0x001C641C
		public static List<WeaponSkin> GetAvailableForBuySkins()
		{
			return (from s in WeaponSkinsManager.AllSkins
			where WeaponSkinsManager.AvailableForBuy(s)
			select s).ToList<WeaponSkin>();
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x001C8258 File Offset: 0x001C6458
		public static bool AvailableForBuy(WeaponSkin skin)
		{
			return skin != null && !WeaponSkinsManager.IsBoughtSkin(skin.Id) && skin.ForLeague <= RatingSystem.instance.currentLeague;
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x001C8294 File Offset: 0x001C6494
		public static bool IsAvailableByLeague(string skinId)
		{
			if (skinId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("IsAvailableByLeague: skinId.isNullOrEmpty()", new object[0]);
				return true;
			}
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin == null)
			{
				Debug.LogErrorFormat("IsAvailableByLeague: skin == null , skinId = {0}", new object[]
				{
					skinId
				});
				return true;
			}
			return WeaponSkinsManager.IsAvailableByLeague(skin);
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x001C82E8 File Offset: 0x001C64E8
		public static bool IsAvailableByLeague(WeaponSkin skin)
		{
			return skin != null && skin.ForLeague <= RatingSystem.instance.currentLeague;
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x001C8308 File Offset: 0x001C6508
		public static List<WeaponSkin> SkinsForLeague(RatingSystem.RatingLeague league)
		{
			return (from s in WeaponSkinsManager.AllSkins
			where s.ForLeague == league
			select s).ToList<WeaponSkin>();
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x001C8340 File Offset: 0x001C6540
		private static List<WeaponSkin> LoadSkins()
		{
			WeaponSkinsData weaponSkinsData = Resources.Load<WeaponSkinsData>("WeaponSkins/weapon_skins_data");
			if (weaponSkinsData == null)
			{
				Debug.LogError("[WEAPON_SKINS] skins data not found");
				return null;
			}
			return weaponSkinsData.Data;
		}

		// Token: 0x04003D32 RID: 15666
		private static List<WeaponSkin> _allSkins;

		// Token: 0x04003D33 RID: 15667
		public static List<string> SkinIds = new List<string>
		{
			"weaponskin_acid_canon",
			"weaponskin_antihero_rifle",
			"weaponskin_dragon_breath",
			"weaponskin_ghost_lantern",
			"weaponskin_loud_piggy",
			"weaponskin_peacemaker",
			"weaponskin_prototype",
			"weaponskin_secret_forces_rifle",
			"weaponskin_shotgun_pistol",
			"weaponskin_steam_revolver",
			"weaponskin_storm_hammer",
			"weaponskin_toy_bomber"
		};
	}
}
