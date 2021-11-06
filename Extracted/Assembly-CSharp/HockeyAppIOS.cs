using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class HockeyAppIOS : MonoBehaviour
{
	// Token: 0x060029CE RID: 10702 RVA: 0x000DCD58 File Offset: 0x000DAF58
	private void Awake()
	{
	}

	// Token: 0x060029CF RID: 10703 RVA: 0x000DCD5C File Offset: 0x000DAF5C
	private void OnEnable()
	{
	}

	// Token: 0x060029D0 RID: 10704 RVA: 0x000DCD60 File Offset: 0x000DAF60
	private void OnDisable()
	{
	}

	// Token: 0x060029D1 RID: 10705 RVA: 0x000DCD64 File Offset: 0x000DAF64
	private void GameViewLoaded(string message)
	{
	}

	// Token: 0x060029D2 RID: 10706 RVA: 0x000DCD68 File Offset: 0x000DAF68
	protected virtual List<string> GetLogHeaders()
	{
		return new List<string>();
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x000DCD7C File Offset: 0x000DAF7C
	protected virtual WWWForm CreateForm(string log)
	{
		return new WWWForm();
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000DCD90 File Offset: 0x000DAF90
	protected virtual List<string> GetLogFiles()
	{
		return new List<string>();
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000DCDA4 File Offset: 0x000DAFA4
	protected virtual IEnumerator SendLogs(List<string> logs)
	{
		string crashPath = "api/2/apps/[APPID]/crashes/upload";
		string url = this.GetBaseURL() + crashPath.Replace("[APPID]", this.appID);
		foreach (string log in logs)
		{
			WWWForm postForm = this.CreateForm(log);
			string lContent = postForm.headers["Content-Type"].ToString();
			lContent = lContent.Replace("\"", string.Empty);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Content-Type", lContent);
			WWW www = new WWW(url, postForm.data, headers);
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				try
				{
					File.Delete(log);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					if (Debug.isDebugBuild)
					{
						Debug.Log("Failed to delete exception log: " + e);
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000DCDD0 File Offset: 0x000DAFD0
	protected virtual void WriteLogToDisk(string logString, string stackTrace)
	{
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x000DCDD4 File Offset: 0x000DAFD4
	protected virtual string GetBaseURL()
	{
		return string.Empty;
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000DCDE8 File Offset: 0x000DAFE8
	protected virtual string GetAuthenticatorTypeString()
	{
		return string.Empty;
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000DCDFC File Offset: 0x000DAFFC
	protected virtual bool IsConnected()
	{
		return false;
	}

	// Token: 0x060029DA RID: 10714 RVA: 0x000DCE0C File Offset: 0x000DB00C
	protected virtual void HandleException(string logString, string stackTrace)
	{
	}

	// Token: 0x060029DB RID: 10715 RVA: 0x000DCE10 File Offset: 0x000DB010
	public void OnHandleLogCallback(string logString, string stackTrace, LogType type)
	{
	}

	// Token: 0x04001EE3 RID: 7907
	protected const string HOCKEYAPP_BASEURL = "https://rink.hockeyapp.net/";

	// Token: 0x04001EE4 RID: 7908
	protected const string HOCKEYAPP_CRASHESPATH = "api/2/apps/[APPID]/crashes/upload";

	// Token: 0x04001EE5 RID: 7909
	protected const string LOG_FILE_DIR = "/logs/";

	// Token: 0x04001EE6 RID: 7910
	protected const int MAX_CHARS = 199800;

	// Token: 0x04001EE7 RID: 7911
	[Header("HockeyApp Setup")]
	public string appID = "your-hockey-app-id";

	// Token: 0x04001EE8 RID: 7912
	public string serverURL = "your-custom-server-url";

	// Token: 0x04001EE9 RID: 7913
	[Header("Authentication")]
	public HockeyAppIOS.AuthenticatorType authenticatorType;

	// Token: 0x04001EEA RID: 7914
	public string secret = "your-hockey-app-secret";

	// Token: 0x04001EEB RID: 7915
	[Header("Crashes & Exceptions")]
	public bool autoUploadCrashes;

	// Token: 0x04001EEC RID: 7916
	public bool exceptionLogging = true;

	// Token: 0x04001EED RID: 7917
	[Header("Metrics")]
	public bool userMetrics = true;

	// Token: 0x04001EEE RID: 7918
	[Header("Version Updates")]
	public bool updateAlert = true;

	// Token: 0x02000492 RID: 1170
	public enum AuthenticatorType
	{
		// Token: 0x04001EF0 RID: 7920
		Anonymous,
		// Token: 0x04001EF1 RID: 7921
		Device,
		// Token: 0x04001EF2 RID: 7922
		HockeyAppUser,
		// Token: 0x04001EF3 RID: 7923
		HockeyAppEmail,
		// Token: 0x04001EF4 RID: 7924
		WebAuth
	}
}
