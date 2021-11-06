using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000839 RID: 2105
	[Serializable]
	internal sealed class LevelProgressMemento : IEquatable<LevelProgressMemento>
	{
		// Token: 0x06004C6E RID: 19566 RVA: 0x001B8750 File Offset: 0x001B6950
		public LevelProgressMemento(string levelId)
		{
			if (levelId == null)
			{
				throw new ArgumentNullException("levelId");
			}
			this.levelId = levelId;
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06004C6F RID: 19567 RVA: 0x001B8770 File Offset: 0x001B6970
		// (set) Token: 0x06004C70 RID: 19568 RVA: 0x001B8790 File Offset: 0x001B6990
		internal string LevelId
		{
			get
			{
				if (this.levelId == null)
				{
					this.levelId = string.Empty;
				}
				return this.levelId;
			}
			set
			{
				this.levelId = (value ?? string.Empty);
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06004C71 RID: 19569 RVA: 0x001B87A8 File Offset: 0x001B69A8
		// (set) Token: 0x06004C72 RID: 19570 RVA: 0x001B87B0 File Offset: 0x001B69B0
		internal int CoinCount
		{
			get
			{
				return this.coinCount;
			}
			set
			{
				this.coinCount = value;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004C73 RID: 19571 RVA: 0x001B87BC File Offset: 0x001B69BC
		// (set) Token: 0x06004C74 RID: 19572 RVA: 0x001B87C4 File Offset: 0x001B69C4
		internal int GemCount
		{
			get
			{
				return this.gemCount;
			}
			set
			{
				this.gemCount = value;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004C75 RID: 19573 RVA: 0x001B87D0 File Offset: 0x001B69D0
		// (set) Token: 0x06004C76 RID: 19574 RVA: 0x001B87D8 File Offset: 0x001B69D8
		internal int StarCount
		{
			get
			{
				return this.starCount;
			}
			set
			{
				this.starCount = value;
			}
		}

		// Token: 0x06004C77 RID: 19575 RVA: 0x001B87E4 File Offset: 0x001B69E4
		public bool Equals(LevelProgressMemento other)
		{
			return other != null && this.CoinCount == other.CoinCount && this.GemCount == other.GemCount && this.StarCount == other.StarCount && !(this.LevelId != other.LevelId);
		}

		// Token: 0x06004C78 RID: 19576 RVA: 0x001B884C File Offset: 0x001B6A4C
		public override bool Equals(object obj)
		{
			LevelProgressMemento levelProgressMemento = obj as LevelProgressMemento;
			return !object.ReferenceEquals(levelProgressMemento, null) && this.Equals(levelProgressMemento);
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x001B8878 File Offset: 0x001B6A78
		public override int GetHashCode()
		{
			return this.LevelId.GetHashCode() ^ this.CoinCount.GetHashCode() ^ this.GemCount.GetHashCode() ^ this.StarCount.GetHashCode();
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x001B88C0 File Offset: 0x001B6AC0
		public override string ToString()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x001B88D8 File Offset: 0x001B6AD8
		internal static LevelProgressMemento Merge(LevelProgressMemento left, LevelProgressMemento right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (left.LevelId != right.LevelId)
			{
				throw new ArgumentException("Level ids shoud be equal.");
			}
			return new LevelProgressMemento(left.LevelId)
			{
				CoinCount = Math.Max(left.CoinCount, right.CoinCount),
				GemCount = Math.Max(left.GemCount, right.GemCount),
				StarCount = Math.Max(left.StarCount, right.StarCount)
			};
		}

		// Token: 0x04003B47 RID: 15175
		[SerializeField]
		private string levelId;

		// Token: 0x04003B48 RID: 15176
		[SerializeField]
		private int coinCount;

		// Token: 0x04003B49 RID: 15177
		[SerializeField]
		private int gemCount;

		// Token: 0x04003B4A RID: 15178
		[SerializeField]
		private int starCount;
	}
}
