using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006A2 RID: 1698
	public sealed class IosLogsHandler : ILogHandler
	{
		// Token: 0x06003B79 RID: 15225 RVA: 0x001352CC File Offset: 0x001334CC
		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			if (format == null || args == null)
			{
				return;
			}
			string text = string.Format(format, args);
			if (text == null)
			{
				return;
			}
			string arg = Environment.StackTrace ?? "empty stack trace";
			string text2 = string.Format("{0}: {1}\n{2}", logType.ToString(), text, arg);
			if (text2 == null)
			{
				return;
			}
			IosLogsHandler.LogIos(text2);
		}

		// Token: 0x06003B7A RID: 15226 RVA: 0x00135330 File Offset: 0x00133530
		public void LogException(Exception exception, UnityEngine.Object context)
		{
			if (exception == null)
			{
				return;
			}
			try
			{
				this.LogFormat(LogType.Exception, context, exception.ToString(), new object[0]);
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x06003B7B RID: 15227 RVA: 0x00135384 File Offset: 0x00133584
		public static void LogIos(string message)
		{
		}
	}
}
