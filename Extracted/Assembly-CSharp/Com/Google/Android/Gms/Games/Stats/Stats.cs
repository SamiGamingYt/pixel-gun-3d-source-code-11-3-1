using System;
using Com.Google.Android.Gms.Common.Api;

namespace Com.Google.Android.Gms.Games.Stats
{
	// Token: 0x020001BB RID: 443
	public interface Stats
	{
		// Token: 0x06000E8E RID: 3726
		PendingResult<Stats_LoadPlayerStatsResultObject> loadPlayerStats(GoogleApiClient arg_GoogleApiClient_1, bool arg_bool_2);
	}
}
