using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000668 RID: 1640
	[Serializable]
	public class BonusData
	{
		// Token: 0x0600392B RID: 14635 RVA: 0x00128034 File Offset: 0x00126234
		public void Set(Dictionary<string, object> bonusData)
		{
			if (bonusData == null)
			{
				return;
			}
			if (bonusData.ContainsKey("isGems"))
			{
				this.IsGems = Convert.ToBoolean(bonusData["isGems"]);
			}
			if (bonusData.ContainsKey("action"))
			{
				this.Action = Convert.ToString(bonusData["action"]);
			}
			if (bonusData.ContainsKey("ID"))
			{
				this.BonusId = Convert.ToString(bonusData["ID"]);
			}
			if (bonusData.ContainsKey("End"))
			{
				this.End = Convert.ToInt32(bonusData["End"]);
			}
			if (bonusData.ContainsKey("Type"))
			{
				this.IsTypePack = (bonusData["Type"].ToString() == "packs");
			}
			if (bonusData.ContainsKey("Coins"))
			{
				this.Coins = Convert.ToInt32(bonusData["Coins"]);
			}
			if (bonusData.ContainsKey("GemsCurrency"))
			{
				this.Gems = Convert.ToInt32(bonusData["GemsCurrency"]);
			}
			if (bonusData.ContainsKey("Pack"))
			{
				this.Pack = Convert.ToInt32(bonusData["Pack"]);
			}
			if (this.Action == BalanceController.weaponActionName)
			{
				this.Type = BonusData.BonusType.Weapons;
				if (bonusData.ContainsKey("Weapon"))
				{
					this.WeaponId = Convert.ToString(bonusData["Weapon"]);
				}
			}
			else if (this.Action == BalanceController.petActionName)
			{
				this.Type = BonusData.BonusType.Pets;
				if (bonusData.ContainsKey("Pet"))
				{
					this.PetId = Convert.ToString(bonusData["Pet"]);
				}
				if (bonusData.ContainsKey("Quantity"))
				{
					this.Quantity = Convert.ToInt32(bonusData["Quantity"]);
				}
			}
			else if (this.Action == BalanceController.leprechaunActionName)
			{
				this.Type = BonusData.BonusType.Leprechaunt;
				if (bonusData.ContainsKey("CurrencyLeprechaun"))
				{
					this.LeprechauntCurrency = Convert.ToString(bonusData["CurrencyLeprechaun"]);
				}
				if (bonusData.ContainsKey("PerDayLeprechaun"))
				{
					this.LeprechauntPerDay = Convert.ToInt32(bonusData["PerDayLeprechaun"]);
				}
				if (bonusData.ContainsKey("DaysLeprechaun"))
				{
					this.LeprechauntDays = Convert.ToInt32(bonusData["DaysLeprechaun"]);
				}
			}
			else if (this.Action == BalanceController.gadgetActionName)
			{
				this.Type = BonusData.BonusType.Gadgets;
				if (bonusData.ContainsKey("Gadgets"))
				{
					this.Gadgets = (List<string>)bonusData["Gadgets"];
				}
			}
			if (this.Type == BonusData.BonusType.Unknown)
			{
				this.Type = BonusData.BonusType.Currency;
			}
		}

		// Token: 0x040029D4 RID: 10708
		public bool IsGems;

		// Token: 0x040029D5 RID: 10709
		public BonusData.BonusType Type;

		// Token: 0x040029D6 RID: 10710
		public string Action;

		// Token: 0x040029D7 RID: 10711
		public string BonusId;

		// Token: 0x040029D8 RID: 10712
		public int End;

		// Token: 0x040029D9 RID: 10713
		public bool IsTypePack;

		// Token: 0x040029DA RID: 10714
		public int Pack;

		// Token: 0x040029DB RID: 10715
		public int Coins;

		// Token: 0x040029DC RID: 10716
		public int Gems;

		// Token: 0x040029DD RID: 10717
		public string WeaponId;

		// Token: 0x040029DE RID: 10718
		public string PetId;

		// Token: 0x040029DF RID: 10719
		public int Quantity;

		// Token: 0x040029E0 RID: 10720
		public string LeprechauntCurrency;

		// Token: 0x040029E1 RID: 10721
		public int LeprechauntPerDay;

		// Token: 0x040029E2 RID: 10722
		public int LeprechauntDays;

		// Token: 0x040029E3 RID: 10723
		public List<string> Gadgets;

		// Token: 0x02000669 RID: 1641
		public enum BonusType
		{
			// Token: 0x040029E5 RID: 10725
			Unknown,
			// Token: 0x040029E6 RID: 10726
			Currency,
			// Token: 0x040029E7 RID: 10727
			Weapons,
			// Token: 0x040029E8 RID: 10728
			Pets,
			// Token: 0x040029E9 RID: 10729
			Leprechaunt,
			// Token: 0x040029EA RID: 10730
			Gadgets
		}
	}
}
