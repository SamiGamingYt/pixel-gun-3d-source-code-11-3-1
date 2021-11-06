using System;

namespace FyberPlugin
{
	// Token: 0x02000130 RID: 304
	public class FacebookNetworkBannerSizes
	{
		// Token: 0x040007CB RID: 1995
		private const string NETWORK_NAME = "FacebookAudienceNetwork";

		// Token: 0x040007CC RID: 1996
		public static readonly NetworkBannerSize BANNER_50 = new NetworkBannerSize("FacebookAudienceNetwork", new BannerSize(320, 50));

		// Token: 0x040007CD RID: 1997
		public static readonly NetworkBannerSize BANNER_90 = new NetworkBannerSize("FacebookAudienceNetwork", new BannerSize(320, 90));

		// Token: 0x040007CE RID: 1998
		public static readonly NetworkBannerSize RECTANGLE_HEIGHT_250 = new NetworkBannerSize("FacebookAudienceNetwork", new BannerSize(300, 250));
	}
}
