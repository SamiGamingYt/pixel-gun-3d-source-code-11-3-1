using System;
using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.PInvoke;

namespace GooglePlayGames
{
	// Token: 0x020001BE RID: 446
	internal interface IClientImpl
	{
		// Token: 0x06000E92 RID: 3730
		PlatformConfiguration CreatePlatformConfiguration();

		// Token: 0x06000E93 RID: 3731
		TokenClient CreateTokenClient(string playerId, bool reset);

		// Token: 0x06000E94 RID: 3732
		void GetPlayerStats(IntPtr apiClientPtr, Action<CommonStatusCodes, PlayerStats> callback);
	}
}
