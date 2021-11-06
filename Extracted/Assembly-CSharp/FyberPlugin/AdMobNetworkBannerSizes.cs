using System;

namespace FyberPlugin
{
	// Token: 0x0200012F RID: 303
	public class AdMobNetworkBannerSizes
	{
		// Token: 0x040007C4 RID: 1988
		private const string NETWORK_NAME = "AdMob";

		// Token: 0x040007C5 RID: 1989
		public static readonly NetworkBannerSize BANNER = new NetworkBannerSize("AdMob", new BannerSize(320, 50));

		// Token: 0x040007C6 RID: 1990
		public static readonly NetworkBannerSize LARGE_BANNER = new NetworkBannerSize("AdMob", new BannerSize(320, 100));

		// Token: 0x040007C7 RID: 1991
		public static readonly NetworkBannerSize MEDIUM_RECTANGLE = new NetworkBannerSize("AdMob", new BannerSize(300, 250));

		// Token: 0x040007C8 RID: 1992
		public static readonly NetworkBannerSize FULL_BANNER = new NetworkBannerSize("AdMob", new BannerSize(468, 60));

		// Token: 0x040007C9 RID: 1993
		public static readonly NetworkBannerSize LEADERBOARD = new NetworkBannerSize("AdMob", new BannerSize(728, 90));

		// Token: 0x040007CA RID: 1994
		public static readonly NetworkBannerSize SMART_BANNER = new NetworkBannerSize("AdMob", new BannerSize(-1, -2));
	}
}
