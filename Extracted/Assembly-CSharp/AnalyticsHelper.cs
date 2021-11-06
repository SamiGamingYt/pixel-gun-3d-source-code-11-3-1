using System;
using Rilisoft;

// Token: 0x0200055B RID: 1371
internal sealed class AnalyticsHelper
{
	// Token: 0x1700083B RID: 2107
	// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000F92A0 File Offset: 0x000F74A0
	public static AnalyticsHelper Instance
	{
		get
		{
			return AnalyticsHelper._instance.Value;
		}
	}

	// Token: 0x1700083C RID: 2108
	// (get) Token: 0x06002FA4 RID: 12196 RVA: 0x000F92AC File Offset: 0x000F74AC
	// (set) Token: 0x06002FA5 RID: 12197 RVA: 0x000F92B4 File Offset: 0x000F74B4
	public AdvertisementInfo AdvertisementContext
	{
		get
		{
			return this._advertisementContext;
		}
		set
		{
			this._advertisementContext = (value ?? AdvertisementInfo.Default);
		}
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000F92CC File Offset: 0x000F74CC
	public static string GetAdProviderName(AdProvider provider)
	{
		return (provider != AdProvider.GoogleMobileAds) ? provider.ToString() : "AdMob";
	}

	// Token: 0x04002309 RID: 8969
	public const string AdStatisticsTotalEventName = "ADS Statistics Total";

	// Token: 0x0400230A RID: 8970
	private AdvertisementInfo _advertisementContext = AdvertisementInfo.Default;

	// Token: 0x0400230B RID: 8971
	private static readonly Lazy<AnalyticsHelper> _instance = new Lazy<AnalyticsHelper>(() => new AnalyticsHelper());
}
