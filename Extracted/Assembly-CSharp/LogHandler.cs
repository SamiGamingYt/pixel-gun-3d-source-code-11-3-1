using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020006A1 RID: 1697
internal sealed class LogHandler : MonoBehaviour
{
	// Token: 0x06003B73 RID: 15219 RVA: 0x00135238 File Offset: 0x00133438
	private void Start()
	{
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003B74 RID: 15220 RVA: 0x00135264 File Offset: 0x00133464
	private void OnEnable()
	{
		base.StartCoroutine(this.RegisterLogCallbackCoroutine());
	}

	// Token: 0x06003B75 RID: 15221 RVA: 0x00135274 File Offset: 0x00133474
	private void OnDisable()
	{
		this._cancelled = true;
		if (this._registered)
		{
			Application.RegisterLogCallback(null);
		}
	}

	// Token: 0x06003B76 RID: 15222 RVA: 0x00135290 File Offset: 0x00133490
	private IEnumerator RegisterLogCallbackCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		if (!this._cancelled)
		{
			Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
			this._registered = true;
		}
		yield break;
	}

	// Token: 0x06003B77 RID: 15223 RVA: 0x001352AC File Offset: 0x001334AC
	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			this._logString = logString;
			this._stackTrace = stackTrace;
		}
	}

	// Token: 0x04002BFC RID: 11260
	private bool _cancelled;

	// Token: 0x04002BFD RID: 11261
	private bool _registered;

	// Token: 0x04002BFE RID: 11262
	private string _logString = string.Empty;

	// Token: 0x04002BFF RID: 11263
	private string _stackTrace = string.Empty;
}
