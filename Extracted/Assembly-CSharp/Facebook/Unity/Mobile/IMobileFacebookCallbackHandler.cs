using System;

namespace Facebook.Unity.Mobile
{
	// Token: 0x020000DF RID: 223
	internal interface IMobileFacebookCallbackHandler : IFacebookCallbackHandler
	{
		// Token: 0x060006B9 RID: 1721
		void OnAppInviteComplete(string message);

		// Token: 0x060006BA RID: 1722
		void OnFetchDeferredAppLinkComplete(string message);

		// Token: 0x060006BB RID: 1723
		void OnRefreshCurrentAccessTokenComplete(string message);
	}
}
