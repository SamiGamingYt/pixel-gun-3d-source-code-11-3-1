using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000559 RID: 1369
	internal sealed class SalesConstants
	{
		// Token: 0x06002F95 RID: 12181 RVA: 0x000F9020 File Offset: 0x000F7220
		private SalesConstants()
		{
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002F97 RID: 12183 RVA: 0x000F9164 File Offset: 0x000F7364
		public static SalesConstants Instance
		{
			get
			{
				return SalesConstants.s_instance.Value;
			}
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x000F9170 File Offset: 0x000F7370
		public string GetSalesCategory(string category)
		{
			if (category == null)
			{
				return string.Empty;
			}
			if (this._weaponSales.Contains(category))
			{
				return "Weapons Sales";
			}
			if (this._equipmentSales.Contains(category))
			{
				return "Equipment Sales";
			}
			if (this._gadgetsSales.Contains(category))
			{
				return "Gadgets Sales";
			}
			if (this._petsEggsSales.Contains(category))
			{
				return "Pets and Eggs Sales";
			}
			return "Other";
		}

		// Token: 0x040022FE RID: 8958
		private readonly List<string> _weaponSales = new List<string>
		{
			"Primary",
			"Back Up",
			"Melee",
			"Special",
			"Sniper",
			"Premium"
		};

		// Token: 0x040022FF RID: 8959
		private readonly List<string> _equipmentSales = new List<string>
		{
			"Skins",
			"Armor",
			"Boots",
			"Capes",
			"Hats",
			"Gear",
			"Masks",
			"League"
		};

		// Token: 0x04002300 RID: 8960
		private readonly List<string> _gadgetsSales = new List<string>
		{
			"Throwing Gadgets",
			"Tool Gadgets",
			"Support Gadgets"
		};

		// Token: 0x04002301 RID: 8961
		private readonly List<string> _petsEggsSales = new List<string>
		{
			"Pets",
			"Eggs"
		};

		// Token: 0x04002302 RID: 8962
		private static readonly Lazy<SalesConstants> s_instance = new Lazy<SalesConstants>(() => new SalesConstants());
	}
}
