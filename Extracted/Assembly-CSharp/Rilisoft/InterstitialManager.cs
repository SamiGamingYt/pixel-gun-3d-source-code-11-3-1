using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200067A RID: 1658
	internal sealed class InterstitialManager
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x0012B2F4 File Offset: 0x001294F4
		private InterstitialManager()
		{
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060039AB RID: 14763 RVA: 0x0012B328 File Offset: 0x00129528
		public static InterstitialManager Instance
		{
			get
			{
				return InterstitialManager._instance.Value;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060039AC RID: 14764 RVA: 0x0012B334 File Offset: 0x00129534
		public AdProvider Provider
		{
			get
			{
				return this.GetProviderByIndex(this._interstitialProviderIndex);
			}
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0012B344 File Offset: 0x00129544
		public AdProvider GetProviderByIndex(int index)
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return AdProvider.None;
			}
			if (PromoActionsManager.MobileAdvert.InterstitialProviders.Count == 0)
			{
				return AdProvider.None;
			}
			return (AdProvider)PromoActionsManager.MobileAdvert.InterstitialProviders[index % PromoActionsManager.MobileAdvert.InterstitialProviders.Count];
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0012B394 File Offset: 0x00129594
		internal int SwitchAdProvider()
		{
			int interstitialProviderIndex = this._interstitialProviderIndex;
			AdProvider provider = this.Provider;
			this._interstitialProviderIndex++;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.SwitchImageIdGroup();
			}
			if (Defs.IsDeveloperBuild)
			{
				string message = string.Format("Switching interstitial provider from {0} ({1}) to {2} ({3})", new object[]
				{
					interstitialProviderIndex,
					provider,
					this._interstitialProviderIndex,
					this.Provider
				});
				Debug.Log(message);
			}
			return this._interstitialProviderIndex;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x0012B43C File Offset: 0x0012963C
		internal void ResetAdProvider()
		{
			int interstitialProviderIndex = this._interstitialProviderIndex;
			AdProvider provider = this.Provider;
			this._interstitialProviderIndex = 0;
			if (provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyImageInterstitial();
			}
			if (this.Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.ResetImageAdUnitId();
			}
			if (Defs.IsDeveloperBuild)
			{
				string message = string.Format("Resetting image interstitial provider from {0} ({1}) to {2} ({3})", new object[]
				{
					interstitialProviderIndex,
					provider,
					this._interstitialProviderIndex,
					this.Provider
				});
				Debug.Log(message);
			}
		}

		// Token: 0x04002A6E RID: 10862
		private static readonly Lazy<InterstitialManager> _instance = new Lazy<InterstitialManager>(() => new InterstitialManager());

		// Token: 0x04002A6F RID: 10863
		private int _interstitialProviderIndex;
	}
}
