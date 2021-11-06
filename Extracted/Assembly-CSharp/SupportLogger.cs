using System;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class SupportLogger : MonoBehaviour
{
	// Token: 0x06002740 RID: 10048 RVA: 0x000C4854 File Offset: 0x000C2A54
	public void Start()
	{
		GameObject gameObject = GameObject.Find("PunSupportLogger");
		if (gameObject == null)
		{
			gameObject = new GameObject("PunSupportLogger");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			SupportLogging supportLogging = gameObject.AddComponent<SupportLogging>();
			supportLogging.LogTrafficStats = this.LogTrafficStats;
		}
	}

	// Token: 0x04001B86 RID: 7046
	public bool LogTrafficStats = true;
}
