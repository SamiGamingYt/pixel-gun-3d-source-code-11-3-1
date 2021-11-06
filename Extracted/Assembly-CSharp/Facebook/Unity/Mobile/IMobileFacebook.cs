using System;

namespace Facebook.Unity.Mobile
{
	// Token: 0x020000DE RID: 222
	internal interface IMobileFacebook : IFacebook
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060006B4 RID: 1716
		// (set) Token: 0x060006B5 RID: 1717
		ShareDialogMode ShareDialogMode { get; set; }

		// Token: 0x060006B6 RID: 1718
		void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback);

		// Token: 0x060006B7 RID: 1719
		void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback);

		// Token: 0x060006B8 RID: 1720
		void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback);
	}
}
