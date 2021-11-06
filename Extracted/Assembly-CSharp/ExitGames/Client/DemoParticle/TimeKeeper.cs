using System;

namespace ExitGames.Client.DemoParticle
{
	// Token: 0x02000462 RID: 1122
	public class TimeKeeper
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x000C4B18 File Offset: 0x000C2D18
		public TimeKeeper(int interval)
		{
			this.IsEnabled = true;
			this.Interval = interval;
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06002751 RID: 10065 RVA: 0x000C4B3C File Offset: 0x000C2D3C
		// (set) Token: 0x06002752 RID: 10066 RVA: 0x000C4B44 File Offset: 0x000C2D44
		public int Interval { get; set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06002753 RID: 10067 RVA: 0x000C4B50 File Offset: 0x000C2D50
		// (set) Token: 0x06002754 RID: 10068 RVA: 0x000C4B58 File Offset: 0x000C2D58
		public bool IsEnabled { get; set; }

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000C4B64 File Offset: 0x000C2D64
		// (set) Token: 0x06002756 RID: 10070 RVA: 0x000C4BA4 File Offset: 0x000C2DA4
		public bool ShouldExecute
		{
			get
			{
				return this.IsEnabled && (this.shouldExecute || Environment.TickCount - this.lastExecutionTime > this.Interval);
			}
			set
			{
				this.shouldExecute = value;
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x000C4BB0 File Offset: 0x000C2DB0
		public void Reset()
		{
			this.shouldExecute = false;
			this.lastExecutionTime = Environment.TickCount;
		}

		// Token: 0x04001B88 RID: 7048
		private int lastExecutionTime = Environment.TickCount;

		// Token: 0x04001B89 RID: 7049
		private bool shouldExecute;
	}
}
