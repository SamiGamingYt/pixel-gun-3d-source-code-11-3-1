using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000D3 RID: 211
	internal interface IFacebook
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600065B RID: 1627
		bool LoggedIn { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600065C RID: 1628
		// (set) Token: 0x0600065D RID: 1629
		bool LimitEventUsage { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600065E RID: 1630
		string SDKName { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600065F RID: 1631
		string SDKVersion { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000660 RID: 1632
		string SDKUserAgent { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000661 RID: 1633
		bool Initialized { get; }

		// Token: 0x06000662 RID: 1634
		void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback);

		// Token: 0x06000663 RID: 1635
		void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback);

		// Token: 0x06000664 RID: 1636
		void LogOut();

		// Token: 0x06000665 RID: 1637
		[Obsolete]
		void AppRequest(string message, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback);

		// Token: 0x06000666 RID: 1638
		void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback);

		// Token: 0x06000667 RID: 1639
		void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback);

		// Token: 0x06000668 RID: 1640
		void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback);

		// Token: 0x06000669 RID: 1641
		void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback);

		// Token: 0x0600066A RID: 1642
		void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback);

		// Token: 0x0600066B RID: 1643
		void API(string query, HttpMethod method, IDictionary<string, string> formData, FacebookDelegate<IGraphResult> callback);

		// Token: 0x0600066C RID: 1644
		void API(string query, HttpMethod method, WWWForm formData, FacebookDelegate<IGraphResult> callback);

		// Token: 0x0600066D RID: 1645
		void ActivateApp(string appId = null);

		// Token: 0x0600066E RID: 1646
		void GetAppLink(FacebookDelegate<IAppLinkResult> callback);

		// Token: 0x0600066F RID: 1647
		void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters);

		// Token: 0x06000670 RID: 1648
		void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters);
	}
}
