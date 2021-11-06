using System;
using UnityEngine;

// Token: 0x020007E9 RID: 2025
public class DevMemoryController : MonoBehaviour
{
	// Token: 0x06004943 RID: 18755 RVA: 0x001971EC File Offset: 0x001953EC
	private void Awake()
	{
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0400366B RID: 13931
	public static string keyActiveMemoryInfo = "keyActiveMemoryInfo";

	// Token: 0x0400366C RID: 13932
	public static DevMemoryController instance;
}
