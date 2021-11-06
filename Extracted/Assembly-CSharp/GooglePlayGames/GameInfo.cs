using System;

namespace GooglePlayGames
{
	// Token: 0x0200019E RID: 414
	public static class GameInfo
	{
		// Token: 0x06000D15 RID: 3349 RVA: 0x00042E7C File Offset: 0x0004107C
		public static bool RequireGooglePlus()
		{
			return false;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00042E80 File Offset: 0x00041080
		public static bool ApplicationIdInitialized()
		{
			return !string.IsNullOrEmpty("339873998127") && !"339873998127".Equals(GameInfo.ToEscapedToken("APP_ID"));
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00042EAC File Offset: 0x000410AC
		public static bool IosClientIdInitialized()
		{
			return !string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(GameInfo.ToEscapedToken("IOS_CLIENTID"));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00042ED8 File Offset: 0x000410D8
		public static bool WebClientIdInitialized()
		{
			return !string.IsNullOrEmpty(string.Empty) && !string.Empty.Equals(GameInfo.ToEscapedToken("WEB_CLIENTID"));
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00042F04 File Offset: 0x00041104
		public static bool NearbyConnectionsInitialized()
		{
			return !string.IsNullOrEmpty("com.pixel.gun3d") && !"com.pixel.gun3d".Equals(GameInfo.ToEscapedToken("NEARBY_SERVICE_ID"));
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00042F30 File Offset: 0x00041130
		private static string ToEscapedToken(string token)
		{
			return string.Format("__{0}__", token);
		}

		// Token: 0x04000A4A RID: 2634
		private const string UnescapedApplicationId = "APP_ID";

		// Token: 0x04000A4B RID: 2635
		private const string UnescapedIosClientId = "IOS_CLIENTID";

		// Token: 0x04000A4C RID: 2636
		private const string UnescapedWebClientId = "WEB_CLIENTID";

		// Token: 0x04000A4D RID: 2637
		private const string UnescapedNearbyServiceId = "NEARBY_SERVICE_ID";

		// Token: 0x04000A4E RID: 2638
		public const string ApplicationId = "339873998127";

		// Token: 0x04000A4F RID: 2639
		public const string IosClientId = "";

		// Token: 0x04000A50 RID: 2640
		public const string WebClientId = "";

		// Token: 0x04000A51 RID: 2641
		public const string NearbyConnectionServiceId = "com.pixel.gun3d";
	}
}
