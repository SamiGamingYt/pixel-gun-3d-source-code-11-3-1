using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200054B RID: 1355
	public sealed class AnalyticsConstants
	{
		// Token: 0x06002F18 RID: 12056 RVA: 0x000F5FD0 File Offset: 0x000F41D0
		internal static string GetSalesName(ShopNGUIController.CategoryNames category)
		{
			string result;
			if (AnalyticsConstants._shopCategoryToLogSalesNamesMapping.TryGetValue(category, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x000F5FF4 File Offset: 0x000F41F4
		internal static IEnumerable<string> GetSalesNames()
		{
			return AnalyticsConstants._shopCategoryToLogSalesNamesMapping.Values;
		}

		// Token: 0x040022C5 RID: 8901
		public const string AppsFlyerInitiatedCheckout = "af_initiated_checkout";

		// Token: 0x040022C6 RID: 8902
		public const string LevelUp = "LevelUp";

		// Token: 0x040022C7 RID: 8903
		public const string SocialEvent = "Social";

		// Token: 0x040022C8 RID: 8904
		public const string ViralityEvent = "Virality";

		// Token: 0x040022C9 RID: 8905
		private static readonly Dictionary<ShopNGUIController.CategoryNames, string> _shopCategoryToLogSalesNamesMapping = new Dictionary<ShopNGUIController.CategoryNames, string>(21, ShopNGUIController.CategoryNameComparer.Instance)
		{
			{
				ShopNGUIController.CategoryNames.PrimaryCategory,
				"Primary"
			},
			{
				ShopNGUIController.CategoryNames.BackupCategory,
				"Back Up"
			},
			{
				ShopNGUIController.CategoryNames.MeleeCategory,
				"Melee"
			},
			{
				ShopNGUIController.CategoryNames.SpecilCategory,
				"Special"
			},
			{
				ShopNGUIController.CategoryNames.SniperCategory,
				"Sniper"
			},
			{
				ShopNGUIController.CategoryNames.PremiumCategory,
				"Premium"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategory,
				"Skins"
			},
			{
				ShopNGUIController.CategoryNames.ArmorCategory,
				"Armor"
			},
			{
				ShopNGUIController.CategoryNames.BootsCategory,
				"Boots"
			},
			{
				ShopNGUIController.CategoryNames.CapesCategory,
				"Capes"
			},
			{
				ShopNGUIController.CategoryNames.HatsCategory,
				"Hats"
			},
			{
				ShopNGUIController.CategoryNames.GearCategory,
				"Gear"
			},
			{
				ShopNGUIController.CategoryNames.MaskCategory,
				"Masks"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategoryEditor,
				"Skins Category Editor"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategoryFemale,
				"SkinsCategoryFamele"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategoryMale,
				"SkinsCategoryMale"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategoryPremium,
				"SkinsCategoryPremium"
			},
			{
				ShopNGUIController.CategoryNames.SkinsCategorySpecial,
				"SkinsCategorySpecial"
			},
			{
				ShopNGUIController.CategoryNames.LeagueHatsCategory,
				"League Hats"
			},
			{
				ShopNGUIController.CategoryNames.LeagueSkinsCategory,
				"League Skins"
			},
			{
				ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory,
				"League Weapon Skins"
			},
			{
				ShopNGUIController.CategoryNames.ThrowingCategory,
				"Throwing Gadgets"
			},
			{
				ShopNGUIController.CategoryNames.ToolsCategoty,
				"Tool Gadgets"
			},
			{
				ShopNGUIController.CategoryNames.SupportCategory,
				"Support Gadgets"
			},
			{
				ShopNGUIController.CategoryNames.PetsCategory,
				"Pets"
			},
			{
				ShopNGUIController.CategoryNames.EggsCategory,
				"Eggs"
			},
			{
				ShopNGUIController.CategoryNames.BestWeapons,
				"Best Weapons"
			},
			{
				ShopNGUIController.CategoryNames.BestWear,
				"Best Wear"
			},
			{
				ShopNGUIController.CategoryNames.BestGadgets,
				"Best Gadgets"
			}
		};

		// Token: 0x0200054C RID: 1356
		public enum AccrualType
		{
			// Token: 0x040022CB RID: 8907
			Earned,
			// Token: 0x040022CC RID: 8908
			Purchased
		}

		// Token: 0x0200054D RID: 1357
		public enum TutorialState
		{
			// Token: 0x040022CE RID: 8910
			Started = 1,
			// Token: 0x040022CF RID: 8911
			Controls_Overview,
			// Token: 0x040022D0 RID: 8912
			Controls_Move,
			// Token: 0x040022D1 RID: 8913
			Controls_Jump,
			// Token: 0x040022D2 RID: 8914
			Kill_Enemy,
			// Token: 0x040022D3 RID: 8915
			Portal,
			// Token: 0x040022D4 RID: 8916
			Rewards,
			// Token: 0x040022D5 RID: 8917
			Open_Shop,
			// Token: 0x040022D6 RID: 8918
			Category_Sniper,
			// Token: 0x040022D7 RID: 8919
			Equip_Sniper,
			// Token: 0x040022D8 RID: 8920
			Category_Armor,
			// Token: 0x040022D9 RID: 8921
			Equip_Armor,
			// Token: 0x040022DA RID: 8922
			Back_Shop,
			// Token: 0x040022DB RID: 8923
			Connect_Scene,
			// Token: 0x040022DC RID: 8924
			Table_Battle,
			// Token: 0x040022DD RID: 8925
			Battle_Start,
			// Token: 0x040022DE RID: 8926
			Battle_End,
			// Token: 0x040022DF RID: 8927
			Finished,
			// Token: 0x040022E0 RID: 8928
			Get_Progress = 100
		}
	}
}
