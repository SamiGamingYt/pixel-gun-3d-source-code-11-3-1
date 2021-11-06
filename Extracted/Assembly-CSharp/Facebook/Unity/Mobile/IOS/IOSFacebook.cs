using System;
using System.Collections.Generic;
using System.Linq;

namespace Facebook.Unity.Mobile.IOS
{
	// Token: 0x020000E2 RID: 226
	internal class IOSFacebook : MobileFacebook
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0002DD48 File Offset: 0x0002BF48
		public IOSFacebook() : this(new IOSWrapper(), new CallbackManager())
		{
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002DD5C File Offset: 0x0002BF5C
		public IOSFacebook(IIOSWrapper iosWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.iosWrapper = iosWrapper;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x0002DD6C File Offset: 0x0002BF6C
		// (set) Token: 0x060006D2 RID: 1746 RVA: 0x0002DD74 File Offset: 0x0002BF74
		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
				this.iosWrapper.FBAppEventsSetLimitEventUsage(value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x0002DD8C File Offset: 0x0002BF8C
		public override string SDKName
		{
			get
			{
				return "FBiOSSDK";
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002DD94 File Offset: 0x0002BF94
		public override string SDKVersion
		{
			get
			{
				return this.iosWrapper.FBSdkVersion();
			}
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002DDA4 File Offset: 0x0002BFA4
		public void Init(string appId, bool frictionlessRequests, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			base.Init(hideUnityDelegate, onInitComplete);
			this.iosWrapper.Init(appId, frictionlessRequests, FacebookSettings.IosURLSuffix, Constants.UnitySDKUserAgentSuffixLegacy);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithReadPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002DDF0 File Offset: 0x0002BFF0
		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.iosWrapper.LogInWithPublishPermissions(this.AddCallback<ILoginResult>(callback), permissions.ToCommaSeparateList());
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002DE0C File Offset: 0x0002C00C
		public override void LogOut()
		{
			base.LogOut();
			this.iosWrapper.LogOut();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002DE20 File Offset: 0x0002C020
		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			string text = null;
			if (filters != null && filters.Any<object>())
			{
				text = (filters.First<object>() as string);
			}
			this.iosWrapper.AppRequest(this.AddCallback<IAppRequestResult>(callback), message, (actionType == null) ? string.Empty : actionType.ToString(), (objectId == null) ? string.Empty : objectId, (to == null) ? null : to.ToArray<string>(), (to == null) ? 0 : to.Count<string>(), (text == null) ? string.Empty : text, (excludeIds == null) ? null : excludeIds.ToArray<string>(), (excludeIds == null) ? 0 : excludeIds.Count<string>(), maxRecipients != null, (maxRecipients == null) ? 0 : maxRecipients.Value, data, title);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002DF30 File Offset: 0x0002C130
		public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			string appLinkUrl2 = string.Empty;
			string previewImageUrl2 = string.Empty;
			if (appLinkUrl != null && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
			{
				appLinkUrl2 = appLinkUrl.AbsoluteUri;
			}
			if (previewImageUrl != null && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
			{
				previewImageUrl2 = previewImageUrl.AbsoluteUri;
			}
			this.iosWrapper.AppInvite(this.AddCallback<IAppInviteResult>(callback), appLinkUrl2, previewImageUrl2);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.iosWrapper.ShareLink(this.AddCallback<IShareResult>(callback), contentURL.AbsoluteUrlOrEmptyString(), contentTitle, contentDescription, photoURL.AbsoluteUrlOrEmptyString());
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002DFD4 File Offset: 0x0002C1D4
		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			string link2 = (!(link != null)) ? string.Empty : link.ToString();
			string picture2 = (!(picture != null)) ? string.Empty : picture.ToString();
			this.iosWrapper.FeedShare(this.AddCallback<IShareResult>(callback), toId, link2, linkName, linkCaption, linkDescription, picture2, mediaSource);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002E03C File Offset: 0x0002C23C
		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			this.iosWrapper.CreateGameGroup(this.AddCallback<IGroupCreateResult>(callback), name, description, privacy);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002E054 File Offset: 0x0002C254
		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			this.iosWrapper.JoinGameGroup(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IGroupJoinResult>(callback)), id);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002E080 File Offset: 0x0002C280
		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			if (valueToSum != null)
			{
				this.iosWrapper.LogAppEvent(logEvent, (double)valueToSum.Value, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
			else
			{
				this.iosWrapper.LogAppEvent(logEvent, 0.0, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002E0F4 File Offset: 0x0002C2F4
		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			IOSFacebook.NativeDict nativeDict = IOSFacebook.MarshallDict(parameters);
			this.iosWrapper.LogPurchaseAppEvent((double)logPurchase, currency, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002E128 File Offset: 0x0002C328
		public override void ActivateApp(string appId)
		{
			this.iosWrapper.FBSettingsActivateApp(appId);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002E138 File Offset: 0x0002C338
		public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.FetchDeferredAppLink(this.AddCallback<IAppLinkResult>(callback));
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002E14C File Offset: 0x0002C34C
		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			this.iosWrapper.GetAppLink(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback)));
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002E16C File Offset: 0x0002C36C
		public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			this.iosWrapper.RefreshCurrentAccessToken(Convert.ToInt32(base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback)));
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002E18C File Offset: 0x0002C38C
		protected override void SetShareDialogMode(ShareDialogMode mode)
		{
			this.iosWrapper.SetShareDialogMode((int)mode);
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002E19C File Offset: 0x0002C39C
		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, object> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, object> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value.ToString();
					nativeDict.NumEntries++;
				}
			}
			return nativeDict;
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0002E274 File Offset: 0x0002C474
		private static IOSFacebook.NativeDict MarshallDict(Dictionary<string, string> dict)
		{
			IOSFacebook.NativeDict nativeDict = new IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (KeyValuePair<string, string> keyValuePair in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = keyValuePair.Key;
					nativeDict.Values[nativeDict.NumEntries] = keyValuePair.Value;
					nativeDict.NumEntries++;
				}
			}
			return nativeDict;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0002E348 File Offset: 0x0002C548
		private int AddCallback<T>(FacebookDelegate<T> callback) where T : IResult
		{
			string value = base.CallbackManager.AddFacebookDelegate<T>(callback);
			return Convert.ToInt32(value);
		}

		// Token: 0x0400064D RID: 1613
		private const string CancelledResponse = "{\"cancelled\":true}";

		// Token: 0x0400064E RID: 1614
		private bool limitEventUsage;

		// Token: 0x0400064F RID: 1615
		private IIOSWrapper iosWrapper;

		// Token: 0x020000E3 RID: 227
		public enum FBInsightsFlushBehavior
		{
			// Token: 0x04000651 RID: 1617
			FBInsightsFlushBehaviorAuto,
			// Token: 0x04000652 RID: 1618
			FBInsightsFlushBehaviorExplicitOnly
		}

		// Token: 0x020000E4 RID: 228
		private class NativeDict
		{
			// Token: 0x060006E9 RID: 1769 RVA: 0x0002E368 File Offset: 0x0002C568
			public NativeDict()
			{
				this.NumEntries = 0;
				this.Keys = null;
				this.Values = null;
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002E390 File Offset: 0x0002C590
			// (set) Token: 0x060006EB RID: 1771 RVA: 0x0002E398 File Offset: 0x0002C598
			public int NumEntries { get; set; }

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002E3A4 File Offset: 0x0002C5A4
			// (set) Token: 0x060006ED RID: 1773 RVA: 0x0002E3AC File Offset: 0x0002C5AC
			public string[] Keys { get; set; }

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002E3B8 File Offset: 0x0002C5B8
			// (set) Token: 0x060006EF RID: 1775 RVA: 0x0002E3C0 File Offset: 0x0002C5C0
			public string[] Values { get; set; }
		}
	}
}
