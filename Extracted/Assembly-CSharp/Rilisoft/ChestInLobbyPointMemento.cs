using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200052B RID: 1323
	[Serializable]
	internal sealed class ChestInLobbyPointMemento : AdPointMementoBase
	{
		// Token: 0x06002E10 RID: 11792 RVA: 0x000F1548 File Offset: 0x000EF748
		public ChestInLobbyPointMemento(string id) : base(id)
		{
			this.Award = ChestInLobbyPointMemento.DefaultAward;
			this.AwardCurrency = ChestInLobbyPointMemento.DefaultCurrency;
			this.SimplifiedInterface = ChestInLobbyPointMemento.DefaultSimplifiedInterface;
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x000F1588 File Offset: 0x000EF788
		public static int DefaultAward
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002E12 RID: 11794 RVA: 0x000F158C File Offset: 0x000EF78C
		public static string DefaultCurrency
		{
			get
			{
				return "Coins";
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x000F1594 File Offset: 0x000EF794
		public static bool DefaultSimplifiedInterface
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002E14 RID: 11796 RVA: 0x000F1598 File Offset: 0x000EF798
		// (set) Token: 0x06002E15 RID: 11797 RVA: 0x000F15A0 File Offset: 0x000EF7A0
		public int Award { get; private set; }

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002E16 RID: 11798 RVA: 0x000F15AC File Offset: 0x000EF7AC
		// (set) Token: 0x06002E17 RID: 11799 RVA: 0x000F15B4 File Offset: 0x000EF7B4
		public string AwardCurrency { get; private set; }

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002E18 RID: 11800 RVA: 0x000F15C0 File Offset: 0x000EF7C0
		// (set) Token: 0x06002E19 RID: 11801 RVA: 0x000F15C8 File Offset: 0x000EF7C8
		public bool SimplifiedInterface { get; private set; }

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x000F15D4 File Offset: 0x000EF7D4
		public List<double> RewardedVideoDelayMinutes
		{
			get
			{
				return this._rewardedVideoDelayMinutes;
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000F15DC File Offset: 0x000EF7DC
		public int GetFinalAward(string category)
		{
			int? int32Override = base.GetInt32Override("award", category);
			if (int32Override != null)
			{
				return int32Override.Value;
			}
			return this.Award;
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000F1610 File Offset: 0x000EF810
		public string GetFinalAwardCurrency(string category)
		{
			string stringOverride = base.GetStringOverride("awardCurrency", category);
			if (stringOverride != null)
			{
				return stringOverride;
			}
			return this.AwardCurrency;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000F1638 File Offset: 0x000EF838
		public bool GetFinalSimplifiedInterface(string category)
		{
			bool? booleanOverride = base.GetBooleanOverride("simplifiedInterface", category);
			if (booleanOverride != null)
			{
				return booleanOverride.Value;
			}
			return this.SimplifiedInterface;
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000F166C File Offset: 0x000EF86C
		public List<double> GetFinalRewardedVideoDelayMinutes(string category)
		{
			List<object> list = base.GetNodeObjectOverride("rewardedVideoDelayMinutes", category) as List<object>;
			if (list == null)
			{
				return this.RewardedVideoDelayMinutes;
			}
			List<double> list2 = new List<double>();
			foreach (object value in list)
			{
				try
				{
					list2.Add(Convert.ToDouble(value));
				}
				catch
				{
				}
			}
			return list2;
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000F1720 File Offset: 0x000EF920
		internal static ChestInLobbyPointMemento FromObject(string id, object obj)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (obj == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			ChestInLobbyPointMemento chestInLobbyPointMemento = new ChestInLobbyPointMemento(id);
			chestInLobbyPointMemento.Reset(dictionary);
			int? @int = ParsingHelper.GetInt32(dictionary, "award");
			if (@int != null)
			{
				chestInLobbyPointMemento.Award = @int.Value;
			}
			string @string = ParsingHelper.GetString(dictionary, "awardCurrency");
			if (@string != null)
			{
				chestInLobbyPointMemento.AwardCurrency = @string;
			}
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "simplifiedInterface");
			if (boolean != null)
			{
				chestInLobbyPointMemento.SimplifiedInterface = boolean.Value;
			}
			List<object> list = ParsingHelper.GetObject(dictionary, "rewardedVideoDelayMinutes") as List<object>;
			if (list != null)
			{
				foreach (object value in list)
				{
					try
					{
						double item = Convert.ToDouble(value);
						chestInLobbyPointMemento.RewardedVideoDelayMinutes.Add(item);
					}
					catch
					{
					}
				}
			}
			return chestInLobbyPointMemento;
		}

		// Token: 0x04002242 RID: 8770
		private readonly List<double> _rewardedVideoDelayMinutes = new List<double>();
	}
}
