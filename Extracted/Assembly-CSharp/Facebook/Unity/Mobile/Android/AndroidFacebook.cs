using System;
using System.Collections.Generic;
using System.Linq;

namespace Facebook.Unity.Mobile.Android
{
	// Token: 0x020000D8 RID: 216
	internal sealed class AndroidFacebook : MobileFacebook
	{
		// Token: 0x0600068F RID: 1679 RVA: 0x0002D6CC File Offset: 0x0002B8CC
		public AndroidFacebook() : this(new FBJavaClass(), new CallbackManager())
		{
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
		public AndroidFacebook(IAndroidJavaClass facebookJavaClass, CallbackManager callbackManager) : base(callbackManager)
		{
			this.KeyHash = string.Empty;
			this.facebookJava = facebookJavaClass;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0002D6FC File Offset: 0x0002B8FC
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0002D704 File Offset: 0x0002B904
		public string KeyHash { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0002D710 File Offset: 0x0002B910
		// (set) Token: 0x06000694 RID: 1684 RVA: 0x0002D718 File Offset: 0x0002B918
		public override bool LimitEventUsage
		{
			get
			{
				return this.limitEventUsage;
			}
			set
			{
				this.limitEventUsage = value;
				this.CallFB("SetLimitEventUsage", value.ToString());
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0002D734 File Offset: 0x0002B934
		public override string SDKName
		{
			get
			{
				return "FBAndroidSDK";
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0002D73C File Offset: 0x0002B93C
		public override string SDKVersion
		{
			get
			{
				return this.facebookJava.CallStatic<string>("GetSdkVersion");
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002D750 File Offset: 0x0002B950
		public void Init(string appId, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			this.CallFB("SetUserAgentSuffix", string.Format("Unity.{0}", Constants.UnitySDKUserAgentSuffixLegacy));
			base.Init(hideUnityDelegate, onInitComplete);
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("appId", appId);
			AndroidFacebook.JavaMethodCall<IResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IResult>(this, "Init");
			javaMethodCall.Call(methodArguments);
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002D7A4 File Offset: 0x0002B9A4
		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			new AndroidFacebook.JavaMethodCall<ILoginResult>(this, "LoginWithReadPermissions")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002D7E0 File Offset: 0x0002B9E0
		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			new AndroidFacebook.JavaMethodCall<ILoginResult>(this, "LoginWithPublishPermissions")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002D81C File Offset: 0x0002BA1C
		public override void LogOut()
		{
			AndroidFacebook.JavaMethodCall<IResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IResult>(this, "Logout");
			javaMethodCall.Call(null);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002D83C File Offset: 0x0002BA3C
		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddNullablePrimitive<OGActionType>("action_type", actionType);
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddCommaSeparatedList("to", to);
			if (filters != null && filters.Any<object>())
			{
				string text = filters.First<object>() as string;
				if (text != null)
				{
					methodArguments.AddString("filters", text);
				}
			}
			methodArguments.AddNullablePrimitive<int>("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			new AndroidFacebook.JavaMethodCall<IAppRequestResult>(this, "AppRequest")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002D90C File Offset: 0x0002BB0C
		public override void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddUri("appLinkUrl", appLinkUrl);
			methodArguments.AddUri("previewImageUrl", previewImageUrl);
			new AndroidFacebook.JavaMethodCall<IAppInviteResult>(this, "AppInvite")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002D954 File Offset: 0x0002BB54
		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddUri("content_url", contentURL);
			methodArguments.AddString("content_title", contentTitle);
			methodArguments.AddString("content_description", contentDescription);
			methodArguments.AddUri("photo_url", photoURL);
			new AndroidFacebook.JavaMethodCall<IShareResult>(this, "ShareLink")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002D9B4 File Offset: 0x0002BBB4
		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("toId", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("linkName", linkName);
			methodArguments.AddString("linkCaption", linkCaption);
			methodArguments.AddString("linkDescription", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("mediaSource", mediaSource);
			new AndroidFacebook.JavaMethodCall<IShareResult>(this, "FeedShare")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002DA3C File Offset: 0x0002BC3C
		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			new AndroidFacebook.JavaMethodCall<IGroupCreateResult>(this, "GameGroupCreate")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002DA90 File Offset: 0x0002BC90
		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("id", id);
			new AndroidFacebook.JavaMethodCall<IGroupJoinResult>(this, "GameGroupJoin")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002DACC File Offset: 0x0002BCCC
		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			new AndroidFacebook.JavaMethodCall<IAppLinkResult>(this, "GetAppLink")
			{
				Callback = callback
			}.Call(null);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002DAF4 File Offset: 0x0002BCF4
		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("logEvent", logEvent);
			methodArguments.AddNullablePrimitive<float>("valueToSum", valueToSum);
			methodArguments.AddDictionary("parameters", parameters);
			AndroidFacebook.JavaMethodCall<IResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0002DB40 File Offset: 0x0002BD40
		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddPrimative<float>("logPurchase", logPurchase);
			methodArguments.AddString("currency", currency);
			methodArguments.AddDictionary("parameters", parameters);
			AndroidFacebook.JavaMethodCall<IResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002DB8C File Offset: 0x0002BD8C
		public override void ActivateApp(string appId)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("app_id", appId);
			AndroidFacebook.JavaMethodCall<IResult> javaMethodCall = new AndroidFacebook.JavaMethodCall<IResult>(this, "ActivateApp");
			javaMethodCall.Call(methodArguments);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002DBC0 File Offset: 0x0002BDC0
		public override void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			MethodArguments args = new MethodArguments();
			new AndroidFacebook.JavaMethodCall<IAppLinkResult>(this, "FetchDeferredAppLinkData")
			{
				Callback = callback
			}.Call(args);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
		public override void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			new AndroidFacebook.JavaMethodCall<IAccessTokenRefreshResult>(this, "RefreshCurrentAccessToken")
			{
				Callback = callback
			}.Call(null);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002DC18 File Offset: 0x0002BE18
		protected override void SetShareDialogMode(ShareDialogMode mode)
		{
			this.CallFB("SetShareDialogMode", mode.ToString());
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002DC30 File Offset: 0x0002BE30
		private void CallFB(string method, string args)
		{
			this.facebookJava.CallStatic(method, new object[]
			{
				args
			});
		}

		// Token: 0x04000646 RID: 1606
		public const string LoginPermissionsKey = "scope";

		// Token: 0x04000647 RID: 1607
		private bool limitEventUsage;

		// Token: 0x04000648 RID: 1608
		private IAndroidJavaClass facebookJava;

		// Token: 0x020000D9 RID: 217
		private class JavaMethodCall<T> : MethodCall<T> where T : IResult
		{
			// Token: 0x060006A9 RID: 1705 RVA: 0x0002DC48 File Offset: 0x0002BE48
			public JavaMethodCall(AndroidFacebook androidImpl, string methodName) : base(androidImpl, methodName)
			{
				this.androidImpl = androidImpl;
			}

			// Token: 0x060006AA RID: 1706 RVA: 0x0002DC5C File Offset: 0x0002BE5C
			public override void Call(MethodArguments args = null)
			{
				MethodArguments methodArguments;
				if (args == null)
				{
					methodArguments = new MethodArguments();
				}
				else
				{
					methodArguments = new MethodArguments(args);
				}
				if (base.Callback != null)
				{
					methodArguments.AddString("callback_id", this.androidImpl.CallbackManager.AddFacebookDelegate<T>(base.Callback));
				}
				this.androidImpl.CallFB(base.MethodName, methodArguments.ToJsonString());
			}

			// Token: 0x0400064A RID: 1610
			private AndroidFacebook androidImpl;
		}
	}
}
