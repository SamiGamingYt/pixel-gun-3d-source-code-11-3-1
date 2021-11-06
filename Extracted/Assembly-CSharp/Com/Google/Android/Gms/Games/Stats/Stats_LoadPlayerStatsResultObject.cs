using System;
using Com.Google.Android.Gms.Common.Api;
using Google.Developers;

namespace Com.Google.Android.Gms.Games.Stats
{
	// Token: 0x020001B8 RID: 440
	public class Stats_LoadPlayerStatsResultObject : JavaObjWrapper, Result, Stats_LoadPlayerStatsResult
	{
		// Token: 0x06000E73 RID: 3699 RVA: 0x00046CE8 File Offset: 0x00044EE8
		public Stats_LoadPlayerStatsResultObject(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00046CF4 File Offset: 0x00044EF4
		public PlayerStats getPlayerStats()
		{
			IntPtr ptr = base.InvokeCall<IntPtr>("getPlayerStats", "()Lcom/google/android/gms/games/stats/PlayerStats;", new object[0]);
			return new PlayerStatsObject(ptr);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00046D20 File Offset: 0x00044F20
		public Status getStatus()
		{
			IntPtr ptr = base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
			return new Status(ptr);
		}

		// Token: 0x04000AAD RID: 2733
		private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats$LoadPlayerStatsResult";
	}
}
