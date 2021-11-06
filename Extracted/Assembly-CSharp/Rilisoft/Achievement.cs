using System;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x020004F2 RID: 1266
	public abstract class Achievement : IDisposable
	{
		// Token: 0x06002CA5 RID: 11429 RVA: 0x000EC61C File Offset: 0x000EA81C
		public Achievement(AchievementData data, AchievementProgressData progressData)
		{
			this._data = data;
			this._progress = new AchievementProgress(progressData);
			this.ActiveCheckers = (Func<bool>)Delegate.Combine(this.ActiveCheckers, new Func<bool>(Achievement.CheckTrainigPolygonDisabled));
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06002CA6 RID: 11430 RVA: 0x000EC664 File Offset: 0x000EA864
		// (remove) Token: 0x06002CA7 RID: 11431 RVA: 0x000EC680 File Offset: 0x000EA880
		protected event Func<bool> ActiveCheckers;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06002CA8 RID: 11432 RVA: 0x000EC69C File Offset: 0x000EA89C
		// (remove) Token: 0x06002CA9 RID: 11433 RVA: 0x000EC6B8 File Offset: 0x000EA8B8
		public event Action<bool, bool> OnProgressChanged;

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000EC6D4 File Offset: 0x000EA8D4
		// (set) Token: 0x06002CAB RID: 11435 RVA: 0x000EC6DC File Offset: 0x000EA8DC
		private protected AchievementData _data { protected get; private set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000EC6E8 File Offset: 0x000EA8E8
		public AchievementData Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x000EC6F0 File Offset: 0x000EA8F0
		protected AchievementProgress Progress
		{
			get
			{
				return this._progress;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000EC6F8 File Offset: 0x000EA8F8
		public int Points
		{
			get
			{
				return this.Progress.Points;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000EC708 File Offset: 0x000EA908
		public int Stage
		{
			get
			{
				return this.Progress.Stage;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000EC718 File Offset: 0x000EA918
		public AchievementType Type
		{
			get
			{
				return this._data.Type;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x000EC728 File Offset: 0x000EA928
		public bool IsActive
		{
			get
			{
				if (this.ActiveCheckers == null)
				{
					return true;
				}
				foreach (Delegate @delegate in this.ActiveCheckers.GetInvocationList())
				{
					if (!((Func<bool>)@delegate)())
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000EC77C File Offset: 0x000EA97C
		protected static bool CheckTrainigPolygonDisabled()
		{
			return true;
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000EC780 File Offset: 0x000EA980
		public int Id
		{
			get
			{
				return this._data.Id;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x000EC790 File Offset: 0x000EA990
		public int MaxStage
		{
			get
			{
				return this._data.Thresholds.Length;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000EC7A0 File Offset: 0x000EA9A0
		public int ToNextStagePointsLeft
		{
			get
			{
				int toNextStagePointsTotal = this.ToNextStagePointsTotal;
				return (toNextStagePointsTotal <= 0) ? -1 : (toNextStagePointsTotal - this._progress.Points);
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000EC7D0 File Offset: 0x000EA9D0
		public int ToNextStagePointsTotal
		{
			get
			{
				return (this._data.Thresholds.Length <= this._progress.Stage) ? -1 : this._data.Thresholds[this._progress.Stage];
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x000EC818 File Offset: 0x000EAA18
		public int ToNextStagePoints
		{
			get
			{
				if (this._data.Thresholds.Length > this._progress.Stage)
				{
					return (this._progress.Stage <= 0) ? this._data.Thresholds[0] : (this._data.Thresholds[this._progress.Stage] - this._data.Thresholds[this._progress.Stage - 1]);
				}
				return -1;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000EC898 File Offset: 0x000EAA98
		public int PointsLeft
		{
			get
			{
				return this._data.Thresholds.Sum() - this._progress.Points;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06002CB9 RID: 11449 RVA: 0x000EC8C4 File Offset: 0x000EAAC4
		public bool IsCompleted
		{
			get
			{
				return this.Progress.Points >= this.PointsLeft;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x000EC8E8 File Offset: 0x000EAAE8
		public int CurrentStage
		{
			get
			{
				return this._progress.Stage;
			}
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000EC8F8 File Offset: 0x000EAAF8
		public int PointsToStage(int stageIdx)
		{
			if (this._data.Thresholds.Length <= stageIdx)
			{
				return -1;
			}
			return this._data.Thresholds[stageIdx] - this.Progress.Points;
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000EC934 File Offset: 0x000EAB34
		public int MaxStageForPoints(int pointsCount)
		{
			int result = -1;
			for (int i = 0; i < this._data.Thresholds.Length; i++)
			{
				if (this._data.Thresholds[i] > pointsCount)
				{
					break;
				}
				result = i;
			}
			return result;
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000EC97C File Offset: 0x000EAB7C
		protected void Gain(int increment = 1)
		{
			if (increment == 0)
			{
				return;
			}
			if (!this.IsActive)
			{
				return;
			}
			this._progress.IncrementPoints(increment);
			int stage = this.Stage;
			this.IncrementStageIfNeeded();
			if (this.OnProgressChanged != null)
			{
				this.OnProgressChanged(true, stage != this.Stage);
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000EC9D8 File Offset: 0x000EABD8
		protected void SetProgress(int totalPoints)
		{
			if (totalPoints < 0)
			{
				return;
			}
			if (totalPoints == this._progress.Points)
			{
				return;
			}
			this._progress.IncrementPoints(this._progress.Points * -1);
			this.Gain(totalPoints);
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000ECA20 File Offset: 0x000EAC20
		private void IncrementStageIfNeeded()
		{
			if (this._progress.Stage < this._data.Thresholds.Length && this._data.Thresholds[this._progress.Stage] <= this._progress.Points)
			{
				this._progress.IncrementStage(1);
				this.IncrementStageIfNeeded();
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000ECA84 File Offset: 0x000EAC84
		public virtual void Dispose()
		{
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000ECA88 File Offset: 0x000EAC88
		public void Log(string msg)
		{
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000ECA8C File Offset: 0x000EAC8C
		public static void LogMsg(string msg)
		{
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000ECA90 File Offset: 0x000EAC90
		// (set) Token: 0x06002CC4 RID: 11460 RVA: 0x000ECAA0 File Offset: 0x000EACA0
		public AchievementProgressData ProgressData
		{
			get
			{
				return this._progress.Data;
			}
			set
			{
				int points = this._progress.Points;
				int stage = this._progress.Stage;
				this._progress.Data = value;
				this._progress.Data.AchievementId = this._data.Id;
				if ((this._progress.Points != points || this._progress.Stage != stage) && this.OnProgressChanged != null)
				{
					this.OnProgressChanged(this._progress.Points != points, this._progress.Stage != stage);
				}
			}
		}

		// Token: 0x040021A9 RID: 8617
		private readonly AchievementProgress _progress;
	}
}
