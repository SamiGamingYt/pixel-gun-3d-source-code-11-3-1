using System;
using System.Globalization;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000C7 RID: 199
	internal static class Constants
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x0002C534 File Offset: 0x0002A734
		public static Uri GraphUrl
		{
			get
			{
				string uriString = string.Format(CultureInfo.InvariantCulture, "https://graph.{0}/{1}/", new object[]
				{
					FB.FacebookDomain,
					FB.GraphApiVersion
				});
				return new Uri(uriString);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x0002C570 File Offset: 0x0002A770
		public static string GraphApiUserAgent
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[]
				{
					FB.FacebookImpl.SDKUserAgent,
					Constants.UnitySDKUserAgent
				});
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0002C5A8 File Offset: 0x0002A7A8
		public static bool IsMobile
		{
			get
			{
				return Constants.CurrentPlatform == FacebookUnityPlatform.Android || Constants.CurrentPlatform == FacebookUnityPlatform.IOS;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x0002C5C0 File Offset: 0x0002A7C0
		public static bool IsEditor
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0002C5C4 File Offset: 0x0002A7C4
		public static bool IsWeb
		{
			get
			{
				return Constants.CurrentPlatform == FacebookUnityPlatform.WebGL || Constants.CurrentPlatform == FacebookUnityPlatform.WebPlayer;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0002C5DC File Offset: 0x0002A7DC
		public static string UnitySDKUserAgentSuffixLegacy
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "Unity.{0}", new object[]
				{
					FacebookSdkVersion.Build
				});
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0002C5FC File Offset: 0x0002A7FC
		public static string UnitySDKUserAgent
		{
			get
			{
				return Utilities.GetUserAgent("FBUnitySDK", FacebookSdkVersion.Build);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0002C610 File Offset: 0x0002A810
		public static bool DebugMode
		{
			get
			{
				return Debug.isDebugBuild;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x0002C618 File Offset: 0x0002A818
		// (set) Token: 0x060005D2 RID: 1490 RVA: 0x0002C650 File Offset: 0x0002A850
		public static FacebookUnityPlatform CurrentPlatform
		{
			get
			{
				if (Constants.currentPlatform == null)
				{
					Constants.currentPlatform = new FacebookUnityPlatform?(Constants.GetCurrentPlatform());
				}
				return Constants.currentPlatform.Value;
			}
			set
			{
				Constants.currentPlatform = new FacebookUnityPlatform?(value);
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0002C660 File Offset: 0x0002A860
		private static FacebookUnityPlatform GetCurrentPlatform()
		{
			RuntimePlatform platform = Application.platform;
			switch (platform)
			{
			case RuntimePlatform.OSXWebPlayer:
			case RuntimePlatform.WindowsWebPlayer:
				return FacebookUnityPlatform.WebPlayer;
			default:
				if (platform == RuntimePlatform.Android)
				{
					return FacebookUnityPlatform.Android;
				}
				if (platform != RuntimePlatform.WebGLPlayer)
				{
					return FacebookUnityPlatform.Unknown;
				}
				return FacebookUnityPlatform.WebGL;
			case RuntimePlatform.IPhonePlayer:
				return FacebookUnityPlatform.IOS;
			}
		}

		// Token: 0x0400060C RID: 1548
		public const string CallbackIdKey = "callback_id";

		// Token: 0x0400060D RID: 1549
		public const string AccessTokenKey = "access_token";

		// Token: 0x0400060E RID: 1550
		public const string UrlKey = "url";

		// Token: 0x0400060F RID: 1551
		public const string RefKey = "ref";

		// Token: 0x04000610 RID: 1552
		public const string ExtrasKey = "extras";

		// Token: 0x04000611 RID: 1553
		public const string TargetUrlKey = "target_url";

		// Token: 0x04000612 RID: 1554
		public const string CancelledKey = "cancelled";

		// Token: 0x04000613 RID: 1555
		public const string ErrorKey = "error";

		// Token: 0x04000614 RID: 1556
		public const string OnPayCompleteMethodName = "OnPayComplete";

		// Token: 0x04000615 RID: 1557
		public const string OnShareCompleteMethodName = "OnShareLinkComplete";

		// Token: 0x04000616 RID: 1558
		public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";

		// Token: 0x04000617 RID: 1559
		public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";

		// Token: 0x04000618 RID: 1560
		public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";

		// Token: 0x04000619 RID: 1561
		public const string GraphApiVersion = "v2.5";

		// Token: 0x0400061A RID: 1562
		public const string GraphUrlFormat = "https://graph.{0}/{1}/";

		// Token: 0x0400061B RID: 1563
		public const string UserLikesPermission = "user_likes";

		// Token: 0x0400061C RID: 1564
		public const string EmailPermission = "email";

		// Token: 0x0400061D RID: 1565
		public const string PublishActionsPermission = "publish_actions";

		// Token: 0x0400061E RID: 1566
		public const string PublishPagesPermission = "publish_pages";

		// Token: 0x0400061F RID: 1567
		private static FacebookUnityPlatform? currentPlatform;
	}
}
