using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200082E RID: 2094
	[Serializable]
	internal struct CampaignProgressMemento : IEquatable<CampaignProgressMemento>
	{
		// Token: 0x06004C14 RID: 19476 RVA: 0x001B6850 File Offset: 0x001B4A50
		internal CampaignProgressMemento(bool conflicted)
		{
			this.levels = new List<LevelProgressMemento>();
			this._conflicted = conflicted;
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x001B6864 File Offset: 0x001B4A64
		internal bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x001B686C File Offset: 0x001B4A6C
		internal List<LevelProgressMemento> Levels
		{
			get
			{
				if (this.levels == null)
				{
					this.levels = new List<LevelProgressMemento>();
				}
				return this.levels;
			}
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x001B688C File Offset: 0x001B4A8C
		internal Dictionary<string, LevelProgressMemento> GetLevelsAsDictionary()
		{
			Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>(this.Levels.Count);
			foreach (LevelProgressMemento levelProgressMemento in this.Levels)
			{
				LevelProgressMemento levelProgressMemento2;
				if (dictionary.TryGetValue(levelProgressMemento.LevelId, out levelProgressMemento2))
				{
					dictionary[levelProgressMemento2.LevelId] = LevelProgressMemento.Merge(levelProgressMemento, levelProgressMemento2);
				}
				else
				{
					dictionary.Add(levelProgressMemento.LevelId, levelProgressMemento);
				}
			}
			return dictionary;
		}

		// Token: 0x06004C18 RID: 19480 RVA: 0x001B6938 File Offset: 0x001B4B38
		internal void SetConflicted()
		{
			this._conflicted = true;
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x001B6944 File Offset: 0x001B4B44
		public bool Equals(CampaignProgressMemento other)
		{
			EqualityComparer<List<LevelProgressMemento>> @default = EqualityComparer<List<LevelProgressMemento>>.Default;
			return @default.Equals(this.Levels, other.Levels);
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x001B6974 File Offset: 0x001B4B74
		public override bool Equals(object obj)
		{
			if (!(obj is CampaignProgressMemento))
			{
				return false;
			}
			CampaignProgressMemento other = (CampaignProgressMemento)obj;
			return this.Equals(other);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x001B699C File Offset: 0x001B4B9C
		public override int GetHashCode()
		{
			return this.Levels.GetHashCode();
		}

		// Token: 0x06004C1C RID: 19484 RVA: 0x001B69AC File Offset: 0x001B4BAC
		public override string ToString()
		{
			string[] value = (from l in this.Levels
			select '"' + l.LevelId + '"').ToArray<string>();
			return string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[]
			{
				string.Join(",", value)
			});
		}

		// Token: 0x06004C1D RID: 19485 RVA: 0x001B6A0C File Offset: 0x001B4C0C
		internal static CampaignProgressMemento Merge(CampaignProgressMemento left, CampaignProgressMemento right)
		{
			Dictionary<string, LevelProgressMemento> dictionary = new Dictionary<string, LevelProgressMemento>();
			IEnumerable<LevelProgressMemento> enumerable = from l in left.Levels.Concat(right.Levels)
			where l != null
			select l;
			foreach (LevelProgressMemento levelProgressMemento in enumerable)
			{
				LevelProgressMemento left2;
				if (dictionary.TryGetValue(levelProgressMemento.LevelId, out left2))
				{
					dictionary[levelProgressMemento.LevelId] = LevelProgressMemento.Merge(left2, levelProgressMemento);
				}
				else
				{
					dictionary.Add(levelProgressMemento.LevelId, levelProgressMemento);
				}
			}
			bool conflicted = left.Conflicted || right.Conflicted;
			CampaignProgressMemento result = new CampaignProgressMemento(conflicted);
			result.Levels.AddRange(dictionary.Values);
			return result;
		}

		// Token: 0x04003B29 RID: 15145
		[SerializeField]
		private List<LevelProgressMemento> levels;

		// Token: 0x04003B2A RID: 15146
		private bool _conflicted;
	}
}
