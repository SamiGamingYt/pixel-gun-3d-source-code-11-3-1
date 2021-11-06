using System;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class ServerTime : MonoBehaviour
{
	// Token: 0x06002734 RID: 10036 RVA: 0x000C4374 File Offset: 0x000C2574
	private void OnGUI()
	{
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 100), 0f, 200f, 30f));
		GUILayout.Label(string.Format("Time Offset: {0}", PhotonNetwork.ServerTimestamp - Environment.TickCount), new GUILayoutOption[0]);
		if (GUILayout.Button("fetch", new GUILayoutOption[0]))
		{
			PhotonNetwork.FetchServerTimestamp();
		}
		GUILayout.EndArea();
	}
}
