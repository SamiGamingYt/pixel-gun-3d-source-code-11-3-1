using System;

namespace Rilisoft
{
	// Token: 0x02000518 RID: 1304
	public class AchievementProgress
	{
		// Token: 0x06002D68 RID: 11624 RVA: 0x000EE8BC File Offset: 0x000ECABC
		public AchievementProgress(AchievementProgressData data)
		{
			if (data == null)
			{
				Achievement.LogMsg("AchievementProgressData is null");
				return;
			}
			this.Data = data;
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x000EE908 File Offset: 0x000ECB08
		public int Points
		{
			get
			{
				return this._points.Value;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x000EE918 File Offset: 0x000ECB18
		public int Stage
		{
			get
			{
				return this._stage.Value;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002D6B RID: 11627 RVA: 0x000EE928 File Offset: 0x000ECB28
		// (set) Token: 0x06002D6C RID: 11628 RVA: 0x000EE968 File Offset: 0x000ECB68
		public AchievementProgressData Data
		{
			get
			{
				this._data.Points = this._points.Value;
				this._data.Stage = this._stage.Value;
				return this._data;
			}
			set
			{
				this._data = value;
				this._points.Value = this._data.Points;
				this._stage.Value = this._data.Stage;
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x000EE9A0 File Offset: 0x000ECBA0
		public void IncrementPoints(int inc = 1)
		{
			this._points.Value = this._points.Value + inc;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x000EE9B8 File Offset: 0x000ECBB8
		public void IncrementStage(int inc = 1)
		{
			this._stage.Value = this._stage.Value + inc;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x000EE9D0 File Offset: 0x000ECBD0
		public bool CustomDataExists(string key)
		{
			return !key.IsNullOrEmpty() && this.Data.CustomData.ContainsKey(key);
		}

		// Token: 0x06002D70 RID: 11632 RVA: 0x000EE9F0 File Offset: 0x000ECBF0
		public object CustomDataGet(string key)
		{
			if (key.IsNullOrEmpty())
			{
				return null;
			}
			return (!this._data.CustomData.ContainsKey(key)) ? null : this.Data.CustomData[key];
		}

		// Token: 0x06002D71 RID: 11633 RVA: 0x000EEA38 File Offset: 0x000ECC38
		public void CustomDataSet(string key, object val = null)
		{
			if (key.IsNullOrEmpty())
			{
				return;
			}
			if (val == null)
			{
				val = string.Empty;
			}
			if (this.Data.CustomData.ContainsKey(key))
			{
				if (this.Data.CustomData[key] == val)
				{
					return;
				}
				this.Data.CustomData[key] = val;
			}
			else
			{
				this.Data.CustomData.Add(key, val);
			}
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x000EEAB8 File Offset: 0x000ECCB8
		public void CustomDataRemove(string key)
		{
			if (key.IsNullOrEmpty())
			{
				return;
			}
			key = key.ToLower();
			if (this.Data.CustomData.ContainsKey(key))
			{
				this.Data.CustomData.Remove(key);
			}
		}

		// Token: 0x040021FE RID: 8702
		private SaltedInt _points = new SaltedInt(19077818);

		// Token: 0x040021FF RID: 8703
		private SaltedInt _stage = new SaltedInt(250606678);

		// Token: 0x04002200 RID: 8704
		private AchievementProgressData _data;
	}
}
