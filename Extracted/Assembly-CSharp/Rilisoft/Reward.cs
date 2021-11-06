using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000737 RID: 1847
	public struct Reward
	{
		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060040F9 RID: 16633 RVA: 0x0015ADDC File Offset: 0x00158FDC
		// (set) Token: 0x060040FA RID: 16634 RVA: 0x0015ADE4 File Offset: 0x00158FE4
		public int Coins { get; set; }

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060040FB RID: 16635 RVA: 0x0015ADF0 File Offset: 0x00158FF0
		// (set) Token: 0x060040FC RID: 16636 RVA: 0x0015ADF8 File Offset: 0x00158FF8
		public int Gems { get; set; }

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060040FD RID: 16637 RVA: 0x0015AE04 File Offset: 0x00159004
		// (set) Token: 0x060040FE RID: 16638 RVA: 0x0015AE0C File Offset: 0x0015900C
		public int Experience { get; set; }

		// Token: 0x060040FF RID: 16639 RVA: 0x0015AE18 File Offset: 0x00159018
		public static Reward Create(Dictionary<string, object> reward)
		{
			Reward result = default(Reward);
			if (reward == null)
			{
				return result;
			}
			try
			{
				object value;
				if (reward.TryGetValue("coins", out value))
				{
					result.Coins = Convert.ToInt32(value);
				}
				object value2;
				if (reward.TryGetValue("gems", out value2))
				{
					result.Gems = Convert.ToInt32(value2);
				}
				object value3;
				if (reward.TryGetValue("xp", out value3))
				{
					result.Experience = Convert.ToInt32(value3);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return result;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0015AEC4 File Offset: 0x001590C4
		public static Reward Create(List<object> reward)
		{
			Reward result = default(Reward);
			if (reward == null)
			{
				return result;
			}
			try
			{
				for (int num = 0; num != Math.Max(reward.Count, 3); num++)
				{
					int num2 = Convert.ToInt32(reward[num]);
					switch (num)
					{
					case 0:
						result.Coins = num2;
						break;
					case 1:
						result.Gems = num2;
						break;
					case 2:
						result.Experience = num2;
						break;
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			return result;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x0015AF7C File Offset: 0x0015917C
		public List<int> ToJson()
		{
			return new List<int>(3)
			{
				this.Coins,
				this.Gems,
				this.Experience
			};
		}
	}
}
