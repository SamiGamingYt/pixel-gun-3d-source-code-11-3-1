using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006A3 RID: 1699
	public static class LogsManager
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06003B7D RID: 15229 RVA: 0x0013538C File Offset: 0x0013358C
		public static LogsManager.LoggingState State
		{
			get
			{
				return LogsManager.m_loggingState;
			}
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x00135394 File Offset: 0x00133594
		public static void Initialize()
		{
			bool flag = Storager.getInt("LogsManager.LoggingEnabledKey", false) == 1;
			LogsManager.m_loggingState = ((!flag) ? LogsManager.LoggingState.Default : LogsManager.LoggingState.EnabledByUserOrServer);
			LogsManager.SetLoggingEnabledCore(true);
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				try
				{
					Debug.logger.logHandler = new IosLogsHandler();
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in setting IosLogsHandler: {0}", new object[]
					{
						ex
					});
				}
			}
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x00135420 File Offset: 0x00133620
		public static void DisableLogsIfAllowed()
		{
			if (LogsManager.m_loggingState != LogsManager.LoggingState.EnabledByUserOrServer)
			{
				try
				{
					LogsManager.SetLoggingEnabledCore(false);
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in DisableLogsIfAllowed: {0}", new object[]
					{
						ex
					});
				}
			}
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0013547C File Offset: 0x0013367C
		public static void EnableLogsFromServer()
		{
			LogsManager.m_loggingState = LogsManager.LoggingState.EnabledByUserOrServer;
			try
			{
				LogsManager.SetLoggingEnabledCore(true);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in EnableLogsFromServer: {0}", new object[]
				{
					ex
				});
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x001354D0 File Offset: 0x001336D0
		public static void SetLoggingEnabled(bool enabled)
		{
			LogsManager.m_loggingState = ((!enabled) ? LogsManager.LoggingState.Default : LogsManager.LoggingState.EnabledByUserOrServer);
			try
			{
				LogsManager.SetLoggingEnabledCore(enabled);
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in SetLoggingEnabled: {0}", new object[]
				{
					ex
				});
			}
			Storager.setInt("LogsManager.LoggingEnabledKey", (!enabled) ? 0 : 1, false);
		}

		// Token: 0x06003B82 RID: 15234 RVA: 0x00135548 File Offset: 0x00133748
		private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (type != LogType.Exception)
			{
				return;
			}
			string text = string.Format("EXCEPTION: {0}\n{1}", condition ?? string.Empty, stackTrace ?? string.Empty);
			if (text == null)
			{
				return;
			}
			IosLogsHandler.LogIos(text);
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00135590 File Offset: 0x00133790
		private static void SetLoggingEnabledCore(bool enabled)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Application.logMessageReceived -= LogsManager.Application_logMessageReceived;
				if (enabled)
				{
					Application.logMessageReceived += LogsManager.Application_logMessageReceived;
				}
			}
			Debug.logger.logEnabled = (enabled || Application.isEditor);
		}

		// Token: 0x04002C00 RID: 11264
		public const string LoggingEnabledKey = "LogsManager.LoggingEnabledKey";

		// Token: 0x04002C01 RID: 11265
		private static LogsManager.LoggingState m_loggingState;

		// Token: 0x020006A4 RID: 1700
		public enum LoggingState
		{
			// Token: 0x04002C03 RID: 11267
			Default,
			// Token: 0x04002C04 RID: 11268
			EnabledByUserOrServer
		}
	}
}
