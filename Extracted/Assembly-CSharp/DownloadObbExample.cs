using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class DownloadObbExample : MonoBehaviour
{
	// Token: 0x06000465 RID: 1125 RVA: 0x0002514C File Offset: 0x0002334C
	private void OnGUI()
	{
		if (!GooglePlayDownloader.RunningOnAndroid())
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use GooglePlayDownloader only on Android device!");
			return;
		}
		string expansionFilePath = GooglePlayDownloader.GetExpansionFilePath();
		if (expansionFilePath == null)
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "External storage is not available!");
		}
		else
		{
			string mainOBBPath = GooglePlayDownloader.GetMainOBBPath(expansionFilePath);
			string patchOBBPath = GooglePlayDownloader.GetPatchOBBPath(expansionFilePath);
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Main = ..." + ((mainOBBPath != null) ? mainOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
			GUI.Label(new Rect(10f, 25f, (float)(Screen.width - 10), 20f), "Patch = ..." + ((patchOBBPath != null) ? patchOBBPath.Substring(expansionFilePath.Length) : " NOT AVAILABLE"));
			if ((mainOBBPath == null || patchOBBPath == null) && GUI.Button(new Rect(10f, 100f, 100f, 100f), "Fetch OBBs"))
			{
				GooglePlayDownloader.FetchOBB();
			}
		}
	}
}
