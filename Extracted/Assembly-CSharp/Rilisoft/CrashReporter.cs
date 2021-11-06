using System;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005CE RID: 1486
	internal sealed class CrashReporter : MonoBehaviour
	{
		// Token: 0x06003337 RID: 13111 RVA: 0x00108F50 File Offset: 0x00107150
		internal void OnGUI()
		{
			float num = (Screen.dpi != 0f) ? Screen.dpi : 160f;
			if (GUILayout.Button("Simulate exception", new GUILayoutOption[]
			{
				GUILayout.Width(1f * num)
			}))
			{
				throw new InvalidOperationException(DateTime.Now.ToString("s"));
			}
			GUILayout.Label("Report path: " + Application.persistentDataPath, new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(this._reportText))
			{
				this._showReport = GUILayout.Toggle(this._showReport, "Show: " + this._reportTime, new GUILayoutOption[0]);
				if (this._showReport)
				{
					GUILayout.Label(this._reportText, new GUILayoutOption[0]);
				}
			}
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x00109028 File Offset: 0x00107228
		internal void Start()
		{
			if (Debug.isDebugBuild)
			{
				AppDomain.CurrentDomain.UnhandledException += CrashReporter.HandleException;
				string[] files = Directory.GetFiles(Application.persistentDataPath, "Report_*.txt", SearchOption.TopDirectoryOnly);
				if (files.Length > 0)
				{
					string path = files[files.Length - 1];
					this._reportTime = Path.GetFileNameWithoutExtension(path);
					this._reportText = File.ReadAllText(path);
				}
			}
			else
			{
				base.enabled = false;
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x0010909C File Offset: 0x0010729C
		private static void HandleException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				string path = string.Format("Report_{0:s}.txt", DateTime.Now).Replace(':', '-');
				string path2 = Path.Combine(Application.persistentDataPath, path);
				File.WriteAllText(path2, ex.ToString());
			}
		}

		// Token: 0x0400259E RID: 9630
		private string _reportText = string.Empty;

		// Token: 0x0400259F RID: 9631
		private string _reportTime = string.Empty;

		// Token: 0x040025A0 RID: 9632
		private bool _showReport;
	}
}
