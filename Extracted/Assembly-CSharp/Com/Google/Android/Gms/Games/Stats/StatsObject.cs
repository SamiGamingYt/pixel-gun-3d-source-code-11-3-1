using System;
using Com.Google.Android.Gms.Common.Api;
using Google.Developers;

namespace Com.Google.Android.Gms.Games.Stats
{
	// Token: 0x020001BD RID: 445
	public class StatsObject : JavaObjWrapper, Stats
	{
		// Token: 0x06000E90 RID: 3728 RVA: 0x00046E84 File Offset: 0x00045084
		public StatsObject(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00046E90 File Offset: 0x00045090
		public PendingResult<Stats_LoadPlayerStatsResultObject> loadPlayerStats(GoogleApiClient arg_GoogleApiClient_1, bool arg_bool_2)
		{
			IntPtr ptr = base.InvokeCall<IntPtr>("loadPlayerStats", "(Lcom/google/android/gms/common/api/GoogleApiClient;Z)Lcom/google/android/gms/common/api/PendingResult;", new object[]
			{
				arg_GoogleApiClient_1,
				arg_bool_2
			});
			return new PendingResult<Stats_LoadPlayerStatsResultObject>(ptr);
		}

		// Token: 0x04000AAF RID: 2735
		private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats";
	}
}
