using System;
using System.Collections;
using LitJson;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class UnityDataConnector : MonoBehaviour
{
	// Token: 0x06000B3F RID: 2879 RVA: 0x0003FB08 File Offset: 0x0003DD08
	private void Start()
	{
		this.updating = false;
		this.currentStatus = "Offline";
		this.saveToGS = false;
		this.guiBoxRect = new Rect(10f, 10f, 310f, 140f);
		this.guiButtonRect = new Rect(30f, 40f, 270f, 30f);
		this.guiButtonRect2 = new Rect(30f, 75f, 270f, 30f);
		this.guiButtonRect3 = new Rect(30f, 110f, 270f, 30f);
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0003FBAC File Offset: 0x0003DDAC
	private void OnGUI()
	{
		GUI.Box(this.guiBoxRect, this.currentStatus);
		if (GUI.Button(this.guiButtonRect, "Update From Google Spreadsheet"))
		{
			this.Connect();
		}
		this.saveToGS = GUI.Toggle(this.guiButtonRect2, this.saveToGS, "Save Stats To Google Spreadsheet");
		if (GUI.Button(this.guiButtonRect3, "Reset Balls values"))
		{
			this.dataDestinationObject.SendMessage("ResetBalls");
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0003FC28 File Offset: 0x0003DE28
	private void Connect()
	{
		if (this.updating)
		{
			return;
		}
		this.updating = true;
		base.StartCoroutine(this.GetData());
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0003FC58 File Offset: 0x0003DE58
	private IEnumerator GetData()
	{
		string connectionString = string.Concat(new string[]
		{
			this.webServiceUrl,
			"?ssid=",
			this.spreadsheetId,
			"&sheet=",
			this.worksheetName,
			"&pass=",
			this.password,
			"&action=GetData"
		});
		if (this.debugMode)
		{
			Debug.Log("Connecting to webservice on " + connectionString);
		}
		WWW www = new WWW(connectionString);
		float elapsedTime = 0f;
		this.currentStatus = "Stablishing Connection... ";
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= this.maxWaitTime)
			{
				this.currentStatus = "Max wait time reached, connection aborted.";
				Debug.Log(this.currentStatus);
				this.updating = false;
				break;
			}
			yield return null;
		}
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			this.currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.LogError(this.currentStatus);
			this.updating = false;
			yield break;
		}
		string response = www.text;
		Debug.Log(elapsedTime + " : " + response);
		this.currentStatus = "Connection stablished, parsing data...";
		if (response == "\"Incorrect Password.\"")
		{
			this.currentStatus = "Connection error: Incorrect Password.";
			Debug.LogError(this.currentStatus);
			this.updating = false;
			yield break;
		}
		try
		{
			this.ssObjects = JsonMapper.ToObject<JsonData[]>(response);
		}
		catch
		{
			this.currentStatus = "Data error: could not parse retrieved data as json.";
			Debug.LogError(this.currentStatus);
			this.updating = false;
			yield break;
		}
		this.currentStatus = "Data Successfully Retrieved!";
		this.updating = false;
		this.dataDestinationObject.SendMessage("DoSomethingWithTheData", this.ssObjects);
		yield break;
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0003FC74 File Offset: 0x0003DE74
	public void SaveDataOnTheCloud(string ballName, float collisionMagnitude)
	{
		if (this.saveToGS)
		{
			base.StartCoroutine(this.SendData(ballName, collisionMagnitude));
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0003FC90 File Offset: 0x0003DE90
	private IEnumerator SendData(string ballName, float collisionMagnitude)
	{
		if (!this.saveToGS)
		{
			yield break;
		}
		string connectionString = string.Concat(new string[]
		{
			this.webServiceUrl,
			"?ssid=",
			this.spreadsheetId,
			"&sheet=",
			this.statisticsWorksheetName,
			"&pass=",
			this.password,
			"&val1=",
			ballName,
			"&val2=",
			collisionMagnitude.ToString(),
			"&action=SetData"
		});
		if (this.debugMode)
		{
			Debug.Log("Connection String: " + connectionString);
		}
		WWW www = new WWW(connectionString);
		float elapsedTime = 0f;
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= this.maxWaitTime)
			{
				break;
			}
			yield return null;
		}
		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			yield break;
		}
		string response = www.text;
		if (response.Contains("Incorrect Password"))
		{
			yield break;
		}
		if (response.Contains("RCVD OK"))
		{
			yield break;
		}
		yield break;
	}

	// Token: 0x040008BA RID: 2234
	public string webServiceUrl = string.Empty;

	// Token: 0x040008BB RID: 2235
	public string spreadsheetId = string.Empty;

	// Token: 0x040008BC RID: 2236
	public string worksheetName = string.Empty;

	// Token: 0x040008BD RID: 2237
	public string password = string.Empty;

	// Token: 0x040008BE RID: 2238
	public float maxWaitTime = 10f;

	// Token: 0x040008BF RID: 2239
	public GameObject dataDestinationObject;

	// Token: 0x040008C0 RID: 2240
	public string statisticsWorksheetName = "Statistics";

	// Token: 0x040008C1 RID: 2241
	public bool debugMode;

	// Token: 0x040008C2 RID: 2242
	private bool updating;

	// Token: 0x040008C3 RID: 2243
	private string currentStatus;

	// Token: 0x040008C4 RID: 2244
	private JsonData[] ssObjects;

	// Token: 0x040008C5 RID: 2245
	private bool saveToGS;

	// Token: 0x040008C6 RID: 2246
	private Rect guiBoxRect;

	// Token: 0x040008C7 RID: 2247
	private Rect guiButtonRect;

	// Token: 0x040008C8 RID: 2248
	private Rect guiButtonRect2;

	// Token: 0x040008C9 RID: 2249
	private Rect guiButtonRect3;
}
