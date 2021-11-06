using System;
using System.Collections.Generic;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor;
using Facebook.Unity.Mobile;
using Facebook.Unity.Mobile.Android;
using Facebook.Unity.Mobile.IOS;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000C8 RID: 200
	public sealed class FB : ScriptableObject
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0002C6D4 File Offset: 0x0002A8D4
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0002C6DC File Offset: 0x0002A8DC
		public static string AppId { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0002C6E4 File Offset: 0x0002A8E4
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0002C6EC File Offset: 0x0002A8EC
		public static string GraphApiVersion
		{
			get
			{
				return FB.graphApiVersion;
			}
			set
			{
				FB.graphApiVersion = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0002C6F4 File Offset: 0x0002A8F4
		public static bool IsLoggedIn
		{
			get
			{
				return FB.facebook != null && FB.FacebookImpl.LoggedIn;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0002C710 File Offset: 0x0002A910
		public static bool IsInitialized
		{
			get
			{
				return FB.facebook != null && FB.facebook.Initialized;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0002C72C File Offset: 0x0002A92C
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x0002C748 File Offset: 0x0002A948
		public static bool LimitAppEventUsage
		{
			get
			{
				return FB.facebook != null && FB.facebook.LimitEventUsage;
			}
			set
			{
				if (FB.facebook != null)
				{
					FB.facebook.LimitEventUsage = value;
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0002C760 File Offset: 0x0002A960
		// (set) Token: 0x060005DF RID: 1503 RVA: 0x0002C77C File Offset: 0x0002A97C
		internal static IFacebook FacebookImpl
		{
			get
			{
				if (FB.facebook == null)
				{
					throw new NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
				}
				return FB.facebook;
			}
			set
			{
				FB.facebook = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0002C784 File Offset: 0x0002A984
		// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0002C78C File Offset: 0x0002A98C
		internal static string FacebookDomain
		{
			get
			{
				return FB.facebookDomain;
			}
			set
			{
				FB.facebookDomain = value;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0002C794 File Offset: 0x0002A994
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x0002C79C File Offset: 0x0002A99C
		private static FB.OnDLLLoaded OnDLLLoadedDelegate { get; set; }

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002C7A4 File Offset: 0x0002A9A4
		public static void Init(InitDelegate onInitComplete = null, HideUnityDelegate onHideUnity = null, string authResponse = null)
		{
			FB.Init(FacebookSettings.AppId, FacebookSettings.Cookie, FacebookSettings.Logging, FacebookSettings.Status, FacebookSettings.Xfbml, FacebookSettings.FrictionlessRequests, authResponse, "en_US", onHideUnity, onInitComplete);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002C7DC File Offset: 0x0002A9DC
		public static void Init(string appId, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string jsSDKLocale = "en_US", HideUnityDelegate onHideUnity = null, InitDelegate onInitComplete = null)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new ArgumentException("appId cannot be null or empty!");
			}
			FB.AppId = appId;
			if (!FB.isInitCalled)
			{
				FB.isInitCalled = true;
				if (Constants.IsEditor)
				{
					FB.OnDLLLoadedDelegate = delegate()
					{
						((EditorFacebook)FB.facebook).Init(onHideUnity, onInitComplete);
					};
					ComponentFactory.GetComponent<EditorFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
				}
				else
				{
					switch (Constants.CurrentPlatform)
					{
					case FacebookUnityPlatform.Android:
						FB.OnDLLLoadedDelegate = delegate()
						{
							((AndroidFacebook)FB.facebook).Init(appId, onHideUnity, onInitComplete);
						};
						ComponentFactory.GetComponent<AndroidFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
						break;
					case FacebookUnityPlatform.IOS:
						FB.OnDLLLoadedDelegate = delegate()
						{
							((IOSFacebook)FB.facebook).Init(appId, frictionlessRequests, onHideUnity, onInitComplete);
						};
						ComponentFactory.GetComponent<IOSFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
						break;
					case FacebookUnityPlatform.WebGL:
					case FacebookUnityPlatform.WebPlayer:
						FB.OnDLLLoadedDelegate = delegate()
						{
							((CanvasFacebook)FB.facebook).Init(appId, cookie, logging, status, xfbml, FacebookSettings.ChannelUrl, authResponse, frictionlessRequests, jsSDKLocale, onHideUnity, onInitComplete);
						};
						ComponentFactory.GetComponent<CanvasFacebookLoader>(ComponentFactory.IfNotExist.AddNew);
						break;
					default:
						throw new NotImplementedException("Facebook API does not yet support this platform");
					}
				}
			}
			else
			{
				FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002C92C File Offset: 0x0002AB2C
		public static void LogInWithPublishPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null)
		{
			FB.FacebookImpl.LogInWithPublishPermissions(permissions, callback);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0002C93C File Offset: 0x0002AB3C
		public static void LogInWithReadPermissions(IEnumerable<string> permissions = null, FacebookDelegate<ILoginResult> callback = null)
		{
			FB.FacebookImpl.LogInWithReadPermissions(permissions, callback);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0002C94C File Offset: 0x0002AB4C
		public static void LogOut()
		{
			FB.FacebookImpl.LogOut();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002C958 File Offset: 0x0002AB58
		public static void AppRequest(string message, OGActionType actionType, string objectId, IEnumerable<string> to, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FB.FacebookImpl.AppRequest(message, new OGActionType?(actionType), objectId, to, null, null, null, data, title, callback);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002C98C File Offset: 0x0002AB8C
		public static void AppRequest(string message, OGActionType actionType, string objectId, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FB.FacebookImpl.AppRequest(message, new OGActionType?(actionType), objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0002C9B8 File Offset: 0x0002ABB8
		public static void AppRequest(string message, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			FB.FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0002C9E8 File Offset: 0x0002ABE8
		public static void ShareLink(Uri contentURL = null, string contentTitle = "", string contentDescription = "", Uri photoURL = null, FacebookDelegate<IShareResult> callback = null)
		{
			FB.FacebookImpl.ShareLink(contentURL, contentTitle, contentDescription, photoURL, callback);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0002C9FC File Offset: 0x0002ABFC
		public static void FeedShare(string toId = "", Uri link = null, string linkName = "", string linkCaption = "", string linkDescription = "", Uri picture = null, string mediaSource = "", FacebookDelegate<IShareResult> callback = null)
		{
			FB.FacebookImpl.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, callback);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002CA20 File Offset: 0x0002AC20
		public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback = null, IDictionary<string, string> formData = null)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FB.FacebookImpl.API(query, method, formData, callback);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002CA4C File Offset: 0x0002AC4C
		public static void API(string query, HttpMethod method, FacebookDelegate<IGraphResult> callback, WWWForm formData)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FB.FacebookImpl.API(query, method, formData, callback);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002CA78 File Offset: 0x0002AC78
		public static void ActivateApp()
		{
			FB.FacebookImpl.ActivateApp(FB.AppId);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002CA8C File Offset: 0x0002AC8C
		public static void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			if (callback == null)
			{
				return;
			}
			FB.FacebookImpl.GetAppLink(callback);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002CAA0 File Offset: 0x0002ACA0
		public static void GameGroupCreate(string name, string description, string privacy = "CLOSED", FacebookDelegate<IGroupCreateResult> callback = null)
		{
			FB.FacebookImpl.GameGroupCreate(name, description, privacy, callback);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002CAB0 File Offset: 0x0002ACB0
		public static void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback = null)
		{
			FB.FacebookImpl.GameGroupJoin(id, callback);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002CAC0 File Offset: 0x0002ACC0
		public static void LogAppEvent(string logEvent, float? valueToSum = null, Dictionary<string, object> parameters = null)
		{
			FB.FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		public static void LogPurchase(float logPurchase, string currency = null, Dictionary<string, object> parameters = null)
		{
			if (string.IsNullOrEmpty(currency))
			{
				currency = "USD";
			}
			FB.FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002CAF4 File Offset: 0x0002ACF4
		private static void LogVersion()
		{
			if (FB.facebook != null)
			{
				FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0} with {1}", FacebookSdkVersion.Build, FB.FacebookImpl.SDKUserAgent));
			}
			else
			{
				FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0}", FacebookSdkVersion.Build));
			}
		}

		// Token: 0x04000620 RID: 1568
		private const string DefaultJSSDKLocale = "en_US";

		// Token: 0x04000621 RID: 1569
		private static IFacebook facebook;

		// Token: 0x04000622 RID: 1570
		private static bool isInitCalled;

		// Token: 0x04000623 RID: 1571
		private static string facebookDomain = "facebook.com";

		// Token: 0x04000624 RID: 1572
		private static string graphApiVersion = "v2.5";

		// Token: 0x020000C9 RID: 201
		public sealed class Canvas
		{
			// Token: 0x1700007A RID: 122
			// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0002CB4C File Offset: 0x0002AD4C
			private static ICanvasFacebook CanvasFacebookImpl
			{
				get
				{
					ICanvasFacebook canvasFacebook = FB.FacebookImpl as ICanvasFacebook;
					if (canvasFacebook == null)
					{
						throw new InvalidOperationException("Attempt to call Canvas interface on non canvas platform");
					}
					return canvasFacebook;
				}
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x0002CB78 File Offset: 0x0002AD78
			public static void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, FacebookDelegate<IPayResult> callback = null)
			{
				FB.Canvas.CanvasFacebookImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
			}
		}

		// Token: 0x020000CA RID: 202
		public sealed class Mobile
		{
			// Token: 0x1700007B RID: 123
			// (get) Token: 0x060005FB RID: 1531 RVA: 0x0002CBA8 File Offset: 0x0002ADA8
			// (set) Token: 0x060005FC RID: 1532 RVA: 0x0002CBB4 File Offset: 0x0002ADB4
			public static ShareDialogMode ShareDialogMode
			{
				get
				{
					return FB.Mobile.MobileFacebookImpl.ShareDialogMode;
				}
				set
				{
					FB.Mobile.MobileFacebookImpl.ShareDialogMode = value;
				}
			}

			// Token: 0x1700007C RID: 124
			// (get) Token: 0x060005FD RID: 1533 RVA: 0x0002CBC4 File Offset: 0x0002ADC4
			private static IMobileFacebook MobileFacebookImpl
			{
				get
				{
					IMobileFacebook mobileFacebook = FB.FacebookImpl as IMobileFacebook;
					if (mobileFacebook == null)
					{
						throw new InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
					}
					return mobileFacebook;
				}
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x0002CBF0 File Offset: 0x0002ADF0
			public static void AppInvite(Uri appLinkUrl, Uri previewImageUrl = null, FacebookDelegate<IAppInviteResult> callback = null)
			{
				FB.Mobile.MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x0002CC00 File Offset: 0x0002AE00
			public static void FetchDeferredAppLinkData(FacebookDelegate<IAppLinkResult> callback = null)
			{
				if (callback == null)
				{
					return;
				}
				FB.Mobile.MobileFacebookImpl.FetchDeferredAppLink(callback);
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x0002CC14 File Offset: 0x0002AE14
			public static void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback = null)
			{
				FB.Mobile.MobileFacebookImpl.RefreshCurrentAccessToken(callback);
			}
		}

		// Token: 0x020000CB RID: 203
		public sealed class Android
		{
			// Token: 0x1700007D RID: 125
			// (get) Token: 0x06000602 RID: 1538 RVA: 0x0002CC2C File Offset: 0x0002AE2C
			public static string KeyHash
			{
				get
				{
					AndroidFacebook androidFacebook = FB.FacebookImpl as AndroidFacebook;
					return (androidFacebook == null) ? string.Empty : androidFacebook.KeyHash;
				}
			}
		}

		// Token: 0x020000CC RID: 204
		internal abstract class CompiledFacebookLoader : MonoBehaviour
		{
			// Token: 0x1700007E RID: 126
			// (get) Token: 0x06000604 RID: 1540
			protected abstract FacebookGameObject FBGameObject { get; }

			// Token: 0x06000605 RID: 1541 RVA: 0x0002CC64 File Offset: 0x0002AE64
			public void Start()
			{
				FB.facebook = this.FBGameObject.Facebook;
				FB.OnDLLLoadedDelegate();
				FB.LogVersion();
				UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x02000899 RID: 2201
		// (Invoke) Token: 0x06004F04 RID: 20228
		private delegate void OnDLLLoaded();
	}
}
