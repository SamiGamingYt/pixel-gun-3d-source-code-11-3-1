using System;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x020001A5 RID: 421
	public class Logger
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00044930 File Offset: 0x00042B30
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00044938 File Offset: 0x00042B38
		public static bool DebugLogEnabled
		{
			get
			{
				return Logger.debugLogEnabled;
			}
			set
			{
				Logger.debugLogEnabled = value;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00044940 File Offset: 0x00042B40
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x00044948 File Offset: 0x00042B48
		public static bool WarningLogEnabled
		{
			get
			{
				return Logger.warningLogEnabled;
			}
			set
			{
				Logger.warningLogEnabled = value;
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00044950 File Offset: 0x00042B50
		public static void d(string msg)
		{
			if (Logger.debugLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.Log(Logger.ToLogMessage(string.Empty, "DEBUG", msg));
				});
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00044988 File Offset: 0x00042B88
		public static void w(string msg)
		{
			if (Logger.warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(Logger.ToLogMessage("!!!", "WARNING", msg));
				});
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x000449C0 File Offset: 0x00042BC0
		public static void e(string msg)
		{
			if (Logger.warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(Logger.ToLogMessage("***", "ERROR", msg));
				});
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000449F8 File Offset: 0x00042BF8
		public static string describe(byte[] b)
		{
			return (b != null) ? ("byte[" + b.Length + "]") : "(null)";
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00044A24 File Offset: 0x00042C24
		private static string ToLogMessage(string prefix, string logType, string msg)
		{
			return string.Format("{0} [Play Games Plugin DLL] {1} {2}: {3}", new object[]
			{
				prefix,
				DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz"),
				logType,
				msg
			});
		}

		// Token: 0x04000A82 RID: 2690
		private static bool debugLogEnabled;

		// Token: 0x04000A83 RID: 2691
		private static bool warningLogEnabled = true;
	}
}
