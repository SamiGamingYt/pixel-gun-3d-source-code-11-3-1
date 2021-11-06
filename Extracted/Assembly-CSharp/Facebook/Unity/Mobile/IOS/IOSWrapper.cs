using System;

namespace Facebook.Unity.Mobile.IOS
{
	// Token: 0x020000E7 RID: 231
	internal class IOSWrapper : IIOSWrapper
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0002E410 File Offset: 0x0002C610
		public void Init(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix)
		{
			IOSWrapper.IOSInit(appId, frictionlessRequests, urlSuffix, unityUserAgentSuffix);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0002E41C File Offset: 0x0002C61C
		public void LogInWithReadPermissions(int requestId, string scope)
		{
			IOSWrapper.IOSLogInWithReadPermissions(requestId, scope);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0002E428 File Offset: 0x0002C628
		public void LogInWithPublishPermissions(int requestId, string scope)
		{
			IOSWrapper.IOSLogInWithPublishPermissions(requestId, scope);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0002E434 File Offset: 0x0002C634
		public void LogOut()
		{
			IOSWrapper.IOSLogOut();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002E43C File Offset: 0x0002C63C
		public void SetShareDialogMode(int mode)
		{
			IOSWrapper.IOSSetShareDialogMode(mode);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002E444 File Offset: 0x0002C644
		public void ShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL)
		{
			IOSWrapper.IOSShareLink(requestId, contentURL, contentTitle, contentDescription, photoURL);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002E454 File Offset: 0x0002C654
		public void FeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource)
		{
			IOSWrapper.IOSFeedShare(requestId, toId, link, linkName, linkCaption, linkDescription, picture, mediaSource);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0002E474 File Offset: 0x0002C674
		public void AppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "")
		{
			IOSWrapper.IOSAppRequest(requestId, message, actionType, objectId, to, toLength, filters, excludeIds, excludeIdsLength, hasMaxRecipients, maxRecipients, data, title);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0002E4A0 File Offset: 0x0002C6A0
		public void AppInvite(int requestId, string appLinkUrl, string previewImageUrl)
		{
			IOSWrapper.IOSAppInvite(requestId, appLinkUrl, previewImageUrl);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002E4AC File Offset: 0x0002C6AC
		public void CreateGameGroup(int requestId, string name, string description, string privacy)
		{
			IOSWrapper.IOSCreateGameGroup(requestId, name, description, privacy);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002E4B8 File Offset: 0x0002C6B8
		public void JoinGameGroup(int requestId, string groupId)
		{
			IOSWrapper.IOSJoinGameGroup(requestId, groupId);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002E4C4 File Offset: 0x0002C6C4
		public void FBSettingsActivateApp(string appId)
		{
			IOSWrapper.IOSFBSettingsActivateApp(appId);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0002E4CC File Offset: 0x0002C6CC
		public void LogAppEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
		{
			IOSWrapper.IOSFBAppEventsLogEvent(logEvent, valueToSum, numParams, paramKeys, paramVals);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0002E4DC File Offset: 0x0002C6DC
		public void LogPurchaseAppEvent(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
		{
			IOSWrapper.IOSFBAppEventsLogPurchase(logPurchase, currency, numParams, paramKeys, paramVals);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0002E4EC File Offset: 0x0002C6EC
		public void FBAppEventsSetLimitEventUsage(bool limitEventUsage)
		{
			IOSWrapper.IOSFBAppEventsSetLimitEventUsage(limitEventUsage);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
		public void GetAppLink(int requestId)
		{
			IOSWrapper.IOSGetAppLink(requestId);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002E4FC File Offset: 0x0002C6FC
		public string FBSdkVersion()
		{
			return IOSWrapper.IOSFBSdkVersion();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002E504 File Offset: 0x0002C704
		public void FetchDeferredAppLink(int requestId)
		{
			IOSWrapper.IOSFetchDeferredAppLink(requestId);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002E50C File Offset: 0x0002C70C
		public void RefreshCurrentAccessToken(int requestId)
		{
			IOSWrapper.IOSRefreshCurrentAccessToken(requestId);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002E514 File Offset: 0x0002C714
		private static void IOSInit(string appId, bool frictionlessRequests, string urlSuffix, string unityUserAgentSuffix)
		{
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002E518 File Offset: 0x0002C718
		private static void IOSLogInWithReadPermissions(int requestId, string scope)
		{
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002E51C File Offset: 0x0002C71C
		private static void IOSLogInWithPublishPermissions(int requestId, string scope)
		{
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002E520 File Offset: 0x0002C720
		private static void IOSLogOut()
		{
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002E524 File Offset: 0x0002C724
		private static void IOSSetShareDialogMode(int mode)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002E528 File Offset: 0x0002C728
		private static void IOSShareLink(int requestId, string contentURL, string contentTitle, string contentDescription, string photoURL)
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002E52C File Offset: 0x0002C72C
		private static void IOSFeedShare(int requestId, string toId, string link, string linkName, string linkCaption, string linkDescription, string picture, string mediaSource)
		{
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002E530 File Offset: 0x0002C730
		private static void IOSAppRequest(int requestId, string message, string actionType, string objectId, string[] to = null, int toLength = 0, string filters = "", string[] excludeIds = null, int excludeIdsLength = 0, bool hasMaxRecipients = false, int maxRecipients = 0, string data = "", string title = "")
		{
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002E534 File Offset: 0x0002C734
		private static void IOSAppInvite(int requestId, string appLinkUrl, string previewImageUrl)
		{
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002E538 File Offset: 0x0002C738
		private static void IOSCreateGameGroup(int requestId, string name, string description, string privacy)
		{
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002E53C File Offset: 0x0002C73C
		private static void IOSJoinGameGroup(int requestId, string groupId)
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002E540 File Offset: 0x0002C740
		private static void IOSFBSettingsActivateApp(string appId)
		{
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002E544 File Offset: 0x0002C744
		private static void IOSFBAppEventsLogEvent(string logEvent, double valueToSum, int numParams, string[] paramKeys, string[] paramVals)
		{
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002E548 File Offset: 0x0002C748
		private static void IOSFBAppEventsLogPurchase(double logPurchase, string currency, int numParams, string[] paramKeys, string[] paramVals)
		{
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002E54C File Offset: 0x0002C74C
		private static void IOSFBAppEventsSetLimitEventUsage(bool limitEventUsage)
		{
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002E550 File Offset: 0x0002C750
		private static void IOSGetAppLink(int requestId)
		{
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002E554 File Offset: 0x0002C754
		private static string IOSFBSdkVersion()
		{
			return "NONE";
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002E55C File Offset: 0x0002C75C
		private static void IOSFetchDeferredAppLink(int requestId)
		{
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002E560 File Offset: 0x0002C760
		private static void IOSRefreshCurrentAccessToken(int requestId)
		{
		}
	}
}
