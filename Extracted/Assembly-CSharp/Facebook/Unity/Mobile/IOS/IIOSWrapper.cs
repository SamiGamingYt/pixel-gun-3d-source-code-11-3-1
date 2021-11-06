using System;

namespace Facebook.Unity.Mobile.IOS
{
	// Token: 0x020000E1 RID: 225
	internal interface IIOSWrapper
	{
		// Token: 0x060006BC RID: 1724
		void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix);

		// Token: 0x060006BD RID: 1725
		void LogInWithReadPermissions(int requestId, string scope);

		// Token: 0x060006BE RID: 1726
		void LogInWithPublishPermissions(int requestId, string scope);

		// Token: 0x060006BF RID: 1727
		void LogOut();

		// Token: 0x060006C0 RID: 1728
		void SetShareDialogMode(int mode);

		// Token: 0x060006C1 RID: 1729
		void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL);

		// Token: 0x060006C2 RID: 1730
		void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource);

		// Token: 0x060006C3 RID: 1731
		void AppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "");

		// Token: 0x060006C4 RID: 1732
		void AppInvite(int requestId, string appLinkUrl, string previewImageUrl);

		// Token: 0x060006C5 RID: 1733
		void CreateGameGroup(int requestId, string name, string description, string privacy);

		// Token: 0x060006C6 RID: 1734
		void JoinGameGroup(int requestId, string groupId);

		// Token: 0x060006C7 RID: 1735
		void FBSettingsActivateApp(string appId);

		// Token: 0x060006C8 RID: 1736
		void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals);

		// Token: 0x060006C9 RID: 1737
		void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals);

		// Token: 0x060006CA RID: 1738
		void FBAppEventsSetLimitEventUsage(bool limitEventUsage);

		// Token: 0x060006CB RID: 1739
		void GetAppLink(int requestId);

		// Token: 0x060006CC RID: 1740
		void RefreshCurrentAccessToken(int requestId);

		// Token: 0x060006CD RID: 1741
		string FBSdkVersion();

		// Token: 0x060006CE RID: 1742
		void FetchDeferredAppLink(int requestId);
	}
}
