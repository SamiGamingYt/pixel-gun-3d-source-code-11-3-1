using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000516 RID: 1302
	public class AchievementData
	{
		// Token: 0x06002D35 RID: 11573 RVA: 0x000EE148 File Offset: 0x000EC348
		public AchievementData(Dictionary<string, object> raw)
		{
			if (raw == null)
			{
				Debug.LogError("Achievement parse error");
				return;
			}
			this.RawData = raw;
			this.Id = raw.ParseJSONField("id", new Func<object, int>(AchievementData.ConvertToInt32Invariant), false);
			this.GroupId = raw.ParseJSONField("group", new Func<object, int>(AchievementData.ConvertToInt32Invariant), true);
			this.Type = raw.ParseJSONField("type", (object o) => o.ToString().ToEnum(null).Value, false);
			this.ClassType = raw.ParseJSONField("classType", (object o) => o.ToString().ToEnum(new AchievementClassType?(AchievementClassType.Unknown)).Value, false);
			this.Icon = raw.ParseJSONField("icon", new Func<object, string>(AchievementData.ConvertToString), false);
			this.LKeyName = raw.ParseJSONField("keyName", new Func<object, string>(AchievementData.ConvertToString), false);
			this.LKeyDesc = raw.ParseJSONField("keyDesc", new Func<object, string>(AchievementData.ConvertToString), false);
			this.Thresholds = raw.ParseJSONField("thresholds", (object o) => (o as List<object>).Select(new Func<object, int>(AchievementData.ConvertToInt32Invariant)).ToArray<int>(), false);
			this.WeaponCategory = raw.ParseJSONField("weaponCategory", (object o) => o.ToString().ToEnum(null), true);
			this.RegimGame = raw.ParseJSONField("regimGame", (object o) => o.ToString().ToEnum(null), true);
			this.Currency = raw.ParseJSONField("currency", delegate(object o)
			{
				string x = o.ToString();
				StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
				if (ordinalIgnoreCase.Equals(x, "gems"))
				{
					return "GemsCurrency";
				}
				if (ordinalIgnoreCase.Equals(x, "coins"))
				{
					return "Coins";
				}
				return null;
			}, true);
			this.League = raw.ParseJSONField("league", (object o) => o.ToString().ToEnum(null), true);
			this.ItemId = raw.ParseJSONField("itemId", new Func<object, string>(AchievementData.ConvertToString), true);
			this.WeaponCategories = raw.ParseJSONField("weaponCategories", delegate(object o)
			{
				List<object> list = o as List<object>;
				if (list == null)
				{
					return null;
				}
				return (from itm in list
				select itm.ToString().ToEnum(null)).ToList<ShopNGUIController.CategoryNames?>();
			}, true);
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002D37 RID: 11575 RVA: 0x000EE3B0 File Offset: 0x000EC5B0
		// (set) Token: 0x06002D38 RID: 11576 RVA: 0x000EE3B8 File Offset: 0x000EC5B8
		public int Id { get; private set; }

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x000EE3C4 File Offset: 0x000EC5C4
		// (set) Token: 0x06002D3A RID: 11578 RVA: 0x000EE3CC File Offset: 0x000EC5CC
		public int GroupId { get; private set; }

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x000EE3D8 File Offset: 0x000EC5D8
		// (set) Token: 0x06002D3C RID: 11580 RVA: 0x000EE3E0 File Offset: 0x000EC5E0
		public AchievementClassType ClassType { get; private set; }

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002D3D RID: 11581 RVA: 0x000EE3EC File Offset: 0x000EC5EC
		// (set) Token: 0x06002D3E RID: 11582 RVA: 0x000EE3F4 File Offset: 0x000EC5F4
		public AchievementType Type { get; private set; }

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000EE400 File Offset: 0x000EC600
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000EE408 File Offset: 0x000EC608
		public string LKeyName { get; private set; }

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000EE414 File Offset: 0x000EC614
		// (set) Token: 0x06002D42 RID: 11586 RVA: 0x000EE41C File Offset: 0x000EC61C
		public string LKeyDesc { get; private set; }

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000EE428 File Offset: 0x000EC628
		// (set) Token: 0x06002D44 RID: 11588 RVA: 0x000EE430 File Offset: 0x000EC630
		public int[] Thresholds { get; private set; }

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002D45 RID: 11589 RVA: 0x000EE43C File Offset: 0x000EC63C
		// (set) Token: 0x06002D46 RID: 11590 RVA: 0x000EE444 File Offset: 0x000EC644
		public string Icon { get; set; }

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x000EE450 File Offset: 0x000EC650
		// (set) Token: 0x06002D48 RID: 11592 RVA: 0x000EE458 File Offset: 0x000EC658
		public ShopNGUIController.CategoryNames? WeaponCategory { get; private set; }

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x000EE464 File Offset: 0x000EC664
		// (set) Token: 0x06002D4A RID: 11594 RVA: 0x000EE46C File Offset: 0x000EC66C
		public ConnectSceneNGUIController.RegimGame? RegimGame { get; private set; }

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x000EE478 File Offset: 0x000EC678
		// (set) Token: 0x06002D4C RID: 11596 RVA: 0x000EE480 File Offset: 0x000EC680
		public string Currency { get; private set; }

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x000EE48C File Offset: 0x000EC68C
		// (set) Token: 0x06002D4E RID: 11598 RVA: 0x000EE494 File Offset: 0x000EC694
		public RatingSystem.RatingLeague? League { get; private set; }

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002D4F RID: 11599 RVA: 0x000EE4A0 File Offset: 0x000EC6A0
		// (set) Token: 0x06002D50 RID: 11600 RVA: 0x000EE4A8 File Offset: 0x000EC6A8
		public string ItemId { get; private set; }

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x000EE4B4 File Offset: 0x000EC6B4
		// (set) Token: 0x06002D52 RID: 11602 RVA: 0x000EE4BC File Offset: 0x000EC6BC
		public List<ShopNGUIController.CategoryNames?> WeaponCategories { get; private set; }

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002D53 RID: 11603 RVA: 0x000EE4C8 File Offset: 0x000EC6C8
		// (set) Token: 0x06002D54 RID: 11604 RVA: 0x000EE4D0 File Offset: 0x000EC6D0
		public Dictionary<string, object> RawData { get; private set; }

		// Token: 0x06002D55 RID: 11605 RVA: 0x000EE4DC File Offset: 0x000EC6DC
		public override string ToString()
		{
			return Json.Serialize(this);
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000EE4E4 File Offset: 0x000EC6E4
		private static int ConvertToInt32Invariant(object o)
		{
			return Convert.ToInt32(o, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000EE4F4 File Offset: 0x000EC6F4
		private static string ConvertToString(object o)
		{
			return o.ToString();
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000EE4FC File Offset: 0x000EC6FC
		internal static IEnumerable<AchievementData> ParseAllAsEnumerable(string rawData)
		{
			List<object> list = Json.Deserialize(rawData) as List<object>;
			if (list == null)
			{
				Debug.LogError("[Achievements] parse error");
				return AchievementData.s_empty;
			}
			return from d in list.OfType<Dictionary<string, object>>()
			select new AchievementData(d);
		}

		// Token: 0x040021DF RID: 8671
		private static readonly AchievementData[] s_empty = new AchievementData[0];
	}
}
