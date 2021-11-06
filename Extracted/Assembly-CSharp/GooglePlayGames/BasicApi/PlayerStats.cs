using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x0200018B RID: 395
	public class PlayerStats
	{
		// Token: 0x06000CBF RID: 3263 RVA: 0x00042B70 File Offset: 0x00040D70
		public PlayerStats()
		{
			this.Valid = false;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00042B8C File Offset: 0x00040D8C
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00042B94 File Offset: 0x00040D94
		public bool Valid { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00042BA0 File Offset: 0x00040DA0
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00042BA8 File Offset: 0x00040DA8
		public int NumberOfPurchases { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00042BB4 File Offset: 0x00040DB4
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x00042BBC File Offset: 0x00040DBC
		public float AvgSessonLength { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00042BC8 File Offset: 0x00040DC8
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00042BD0 File Offset: 0x00040DD0
		public int DaysSinceLastPlayed { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00042BDC File Offset: 0x00040DDC
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00042BE4 File Offset: 0x00040DE4
		public int NumberOfSessions { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00042BF0 File Offset: 0x00040DF0
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00042BF8 File Offset: 0x00040DF8
		public float SessPercentile { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00042C04 File Offset: 0x00040E04
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00042C0C File Offset: 0x00040E0C
		public float SpendPercentile { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00042C18 File Offset: 0x00040E18
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x00042C20 File Offset: 0x00040E20
		public float SpendProbability { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00042C2C File Offset: 0x00040E2C
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x00042C34 File Offset: 0x00040E34
		public float ChurnProbability { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00042C40 File Offset: 0x00040E40
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x00042C48 File Offset: 0x00040E48
		public float HighSpenderProbability { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00042C54 File Offset: 0x00040E54
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x00042C5C File Offset: 0x00040E5C
		public float TotalSpendNext28Days { get; set; }

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00042C68 File Offset: 0x00040E68
		public bool HasNumberOfPurchases()
		{
			return this.NumberOfPurchases != (int)PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x00042C7C File Offset: 0x00040E7C
		public bool HasAvgSessonLength()
		{
			return this.AvgSessonLength != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00042C90 File Offset: 0x00040E90
		public bool HasDaysSinceLastPlayed()
		{
			return this.DaysSinceLastPlayed != (int)PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00042CA4 File Offset: 0x00040EA4
		public bool HasNumberOfSessions()
		{
			return this.NumberOfSessions != (int)PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x00042CB8 File Offset: 0x00040EB8
		public bool HasSessPercentile()
		{
			return this.SessPercentile != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00042CCC File Offset: 0x00040ECC
		public bool HasSpendPercentile()
		{
			return this.SpendPercentile != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00042CE0 File Offset: 0x00040EE0
		public bool HasChurnProbability()
		{
			return this.ChurnProbability != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x00042CF4 File Offset: 0x00040EF4
		public bool HasHighSpenderProbability()
		{
			return this.HighSpenderProbability != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x00042D08 File Offset: 0x00040F08
		public bool HasTotalSpendNext28Days()
		{
			return this.TotalSpendNext28Days != PlayerStats.UNSET_VALUE;
		}

		// Token: 0x040009EF RID: 2543
		private static float UNSET_VALUE = -1f;
	}
}
