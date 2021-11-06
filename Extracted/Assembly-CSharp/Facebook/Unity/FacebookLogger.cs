using System;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x0200010B RID: 267
	internal static class FacebookLogger
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002FA80 File Offset: 0x0002DC80
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0002FA88 File Offset: 0x0002DC88
		internal static IFacebookLogger Instance { private get; set; } = new FacebookLogger.CustomLogger();

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002FA90 File Offset: 0x0002DC90
		public static void Log(string msg)
		{
			FacebookLogger.Instance.Log(msg);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002FAA0 File Offset: 0x0002DCA0
		public static void Log(string format, params string[] args)
		{
			FacebookLogger.Log(string.Format(format, args));
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0002FAB0 File Offset: 0x0002DCB0
		public static void Info(string msg)
		{
			FacebookLogger.Instance.Info(msg);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0002FAC0 File Offset: 0x0002DCC0
		public static void Info(string format, params string[] args)
		{
			FacebookLogger.Info(string.Format(format, args));
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0002FAD0 File Offset: 0x0002DCD0
		public static void Warn(string msg)
		{
			FacebookLogger.Instance.Warn(msg);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0002FAE0 File Offset: 0x0002DCE0
		public static void Warn(string format, params string[] args)
		{
			FacebookLogger.Warn(string.Format(format, args));
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002FAF0 File Offset: 0x0002DCF0
		public static void Error(string msg)
		{
			FacebookLogger.Instance.Error(msg);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0002FB00 File Offset: 0x0002DD00
		public static void Error(string format, params string[] args)
		{
			FacebookLogger.Error(string.Format(format, args));
		}

		// Token: 0x0400068A RID: 1674
		private const string UnityAndroidTag = "Facebook.Unity.FBDebug";

		// Token: 0x0200010C RID: 268
		private class CustomLogger : IFacebookLogger
		{
			// Token: 0x060007D8 RID: 2008 RVA: 0x0002FB10 File Offset: 0x0002DD10
			public CustomLogger()
			{
				this.logger = new FacebookLogger.AndroidLogger();
			}

			// Token: 0x060007D9 RID: 2009 RVA: 0x0002FB24 File Offset: 0x0002DD24
			public void Log(string msg)
			{
				if (Debug.isDebugBuild)
				{
					Debug.Log(msg);
					this.logger.Log(msg);
				}
			}

			// Token: 0x060007DA RID: 2010 RVA: 0x0002FB44 File Offset: 0x0002DD44
			public void Info(string msg)
			{
				Debug.Log(msg);
				this.logger.Info(msg);
			}

			// Token: 0x060007DB RID: 2011 RVA: 0x0002FB58 File Offset: 0x0002DD58
			public void Warn(string msg)
			{
				Debug.LogWarning(msg);
				this.logger.Warn(msg);
			}

			// Token: 0x060007DC RID: 2012 RVA: 0x0002FB6C File Offset: 0x0002DD6C
			public void Error(string msg)
			{
				Debug.LogError(msg);
				this.logger.Error(msg);
			}

			// Token: 0x0400068C RID: 1676
			private IFacebookLogger logger;
		}

		// Token: 0x0200010D RID: 269
		private class AndroidLogger : IFacebookLogger
		{
			// Token: 0x060007DE RID: 2014 RVA: 0x0002FB88 File Offset: 0x0002DD88
			public void Log(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("v", new object[]
					{
						"Facebook.Unity.FBDebug",
						msg
					});
				}
			}

			// Token: 0x060007DF RID: 2015 RVA: 0x0002FBEC File Offset: 0x0002DDEC
			public void Info(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("i", new object[]
					{
						"Facebook.Unity.FBDebug",
						msg
					});
				}
			}

			// Token: 0x060007E0 RID: 2016 RVA: 0x0002FC50 File Offset: 0x0002DE50
			public void Warn(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("w", new object[]
					{
						"Facebook.Unity.FBDebug",
						msg
					});
				}
			}

			// Token: 0x060007E1 RID: 2017 RVA: 0x0002FCB4 File Offset: 0x0002DEB4
			public void Error(string msg)
			{
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.util.Log"))
				{
					androidJavaClass.CallStatic<int>("e", new object[]
					{
						"Facebook.Unity.FBDebug",
						msg
					});
				}
			}
		}
	}
}
