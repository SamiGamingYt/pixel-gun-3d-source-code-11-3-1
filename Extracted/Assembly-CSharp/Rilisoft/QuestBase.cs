using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;

namespace Rilisoft
{
	// Token: 0x0200071F RID: 1823
	public abstract class QuestBase
	{
		// Token: 0x06003F95 RID: 16277 RVA: 0x00154DB0 File Offset: 0x00152FB0
		public QuestBase(string id, long day, int slot, Difficulty difficulty, Reward reward, bool active, bool rewarded)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException("Id should not be empty.");
			}
			this._id = id;
			this._day = day;
			this._slot = slot;
			this._difficulty = difficulty;
			this._reward = reward;
			this._active = active;
			this._rewarded = rewarded;
		}

		// Token: 0x14000082 RID: 130
		// (add) Token: 0x06003F96 RID: 16278 RVA: 0x00154E10 File Offset: 0x00153010
		// (remove) Token: 0x06003F97 RID: 16279 RVA: 0x00154E2C File Offset: 0x0015302C
		public event EventHandler Changed;

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06003F98 RID: 16280 RVA: 0x00154E48 File Offset: 0x00153048
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x06003F99 RID: 16281 RVA: 0x00154E50 File Offset: 0x00153050
		public long Day
		{
			get
			{
				return this._day;
			}
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06003F9A RID: 16282 RVA: 0x00154E58 File Offset: 0x00153058
		public int Slot
		{
			get
			{
				return this._slot;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06003F9B RID: 16283 RVA: 0x00154E60 File Offset: 0x00153060
		public Difficulty Difficulty
		{
			get
			{
				return this._difficulty;
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06003F9C RID: 16284 RVA: 0x00154E68 File Offset: 0x00153068
		public Reward Reward
		{
			get
			{
				return this._reward;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06003F9D RID: 16285 RVA: 0x00154E70 File Offset: 0x00153070
		public bool Dirty
		{
			get
			{
				return this._dirty;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06003F9E RID: 16286 RVA: 0x00154E78 File Offset: 0x00153078
		public bool Rewarded
		{
			get
			{
				return this._rewarded;
			}
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00154E80 File Offset: 0x00153080
		public void SetClean()
		{
			this._dirty = false;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x00154E8C File Offset: 0x0015308C
		public void SetRewarded()
		{
			this._rewarded = true;
			this._dirty = true;
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x00154E9C File Offset: 0x0015309C
		public bool SetActive()
		{
			if (this._active)
			{
				return false;
			}
			this._active = true;
			this._dirty = true;
			return true;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00154EBC File Offset: 0x001530BC
		public Dictionary<string, object> ToJson()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(2)
			{
				{
					"reward",
					this.Reward.ToJson()
				}
			};
			this.ApppendDifficultyProperties(dictionary);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>(3)
			{
				{
					"id",
					this.Id
				},
				{
					"day",
					this.Day
				},
				{
					"slot",
					this.Slot
				},
				{
					QuestConstants.GetDifficultyKey(this.Difficulty),
					dictionary
				},
				{
					"active",
					Convert.ToInt32(this._active)
				},
				{
					"rewarded",
					Convert.ToInt32(this.Rewarded)
				}
			};
			this.AppendProperties(dictionary2);
			return dictionary2;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00154F88 File Offset: 0x00153188
		public override string ToString()
		{
			return Json.Serialize(this.ToJson());
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00154F98 File Offset: 0x00153198
		protected void SetDirty()
		{
			this._dirty = true;
			this.Changed.Do(delegate(EventHandler h)
			{
				h(this, EventArgs.Empty);
			});
		}

		// Token: 0x06003FA5 RID: 16293
		public abstract decimal CalculateProgress();

		// Token: 0x06003FA6 RID: 16294 RVA: 0x00154FBC File Offset: 0x001531BC
		protected virtual void ApppendDifficultyProperties(Dictionary<string, object> difficultyProperties)
		{
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x00154FC0 File Offset: 0x001531C0
		protected virtual void AppendProperties(Dictionary<string, object> properties)
		{
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x00154FC4 File Offset: 0x001531C4
		internal void DebugSetDay(long day)
		{
			this._day = day;
			this._dirty = true;
		}

		// Token: 0x04002ED3 RID: 11987
		private readonly string _id;

		// Token: 0x04002ED4 RID: 11988
		private long _day;

		// Token: 0x04002ED5 RID: 11989
		private readonly int _slot;

		// Token: 0x04002ED6 RID: 11990
		private readonly Difficulty _difficulty;

		// Token: 0x04002ED7 RID: 11991
		private readonly Reward _reward;

		// Token: 0x04002ED8 RID: 11992
		private bool _dirty;

		// Token: 0x04002ED9 RID: 11993
		private bool _active;

		// Token: 0x04002EDA RID: 11994
		private bool _rewarded;
	}
}
