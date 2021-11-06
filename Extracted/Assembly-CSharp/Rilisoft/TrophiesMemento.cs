using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000846 RID: 2118
	[Serializable]
	public struct TrophiesMemento : IEquatable<TrophiesMemento>
	{
		// Token: 0x06004CE8 RID: 19688 RVA: 0x001BB920 File Offset: 0x001B9B20
		public TrophiesMemento(int trophiesNegative, int trophiesPositive, int season)
		{
			this = new TrophiesMemento(trophiesNegative, trophiesPositive, season, false);
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x001BB92C File Offset: 0x001B9B2C
		public TrophiesMemento(int trophiesNegative, int trophiesPositive, int season, bool conflicted)
		{
			this._conflicted = conflicted;
			this.trophiesNegative = trophiesNegative;
			this.trophiesPositive = trophiesPositive;
			this.season = season;
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004CEA RID: 19690 RVA: 0x001BB94C File Offset: 0x001B9B4C
		public bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004CEB RID: 19691 RVA: 0x001BB954 File Offset: 0x001B9B54
		public int TrophiesNegative
		{
			get
			{
				return this.trophiesNegative;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x001BB95C File Offset: 0x001B9B5C
		public int TrophiesPositive
		{
			get
			{
				return this.trophiesPositive;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004CED RID: 19693 RVA: 0x001BB964 File Offset: 0x001B9B64
		public int Trophies
		{
			get
			{
				return this.trophiesPositive - this.trophiesNegative;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x001BB974 File Offset: 0x001B9B74
		public int Season
		{
			get
			{
				return this.season;
			}
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x001BB97C File Offset: 0x001B9B7C
		internal static TrophiesMemento Merge(TrophiesMemento left, TrophiesMemento right)
		{
			bool conflicted = left.Conflicted || right.Conflicted;
			if (left.Season == right.Season)
			{
				int num = Math.Max(left.TrophiesNegative, right.TrophiesNegative);
				int num2 = Math.Max(left.TrophiesPositive, right.TrophiesPositive);
				int num3 = left.Season;
				return new TrophiesMemento(num, num2, num3, conflicted);
			}
			TrophiesMemento trophiesMemento = (left.Season >= right.Season) ? left : right;
			return new TrophiesMemento(trophiesMemento.TrophiesNegative, trophiesMemento.TrophiesPositive, trophiesMemento.Season, conflicted);
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x001BBA24 File Offset: 0x001B9C24
		public bool Equals(TrophiesMemento other)
		{
			return this.TrophiesNegative == other.TrophiesNegative && this.TrophiesPositive == other.TrophiesPositive && this.Season == other.Season;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x001BBA70 File Offset: 0x001B9C70
		public override bool Equals(object obj)
		{
			if (!(obj is TrophiesMemento))
			{
				return false;
			}
			TrophiesMemento other = (TrophiesMemento)obj;
			return this.Equals(other);
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x001BBA98 File Offset: 0x001B9C98
		public override int GetHashCode()
		{
			return this.TrophiesNegative.GetHashCode() ^ this.TrophiesPositive.GetHashCode() ^ this.Season.GetHashCode();
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x001BBAD4 File Offset: 0x001B9CD4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ \"negative\":{0},\"positive\":{1},\"season\":{2} }}", new object[]
			{
				this.trophiesNegative,
				this.trophiesPositive,
				this.season
			});
		}

		// Token: 0x04003B72 RID: 15218
		private readonly bool _conflicted;

		// Token: 0x04003B73 RID: 15219
		[SerializeField]
		private int trophiesNegative;

		// Token: 0x04003B74 RID: 15220
		[SerializeField]
		private int trophiesPositive;

		// Token: 0x04003B75 RID: 15221
		[SerializeField]
		private int season;
	}
}
