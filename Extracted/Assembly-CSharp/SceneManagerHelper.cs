using System;
using UnityEngine.SceneManagement;

// Token: 0x02000412 RID: 1042
public class SceneManagerHelper
{
	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x06002516 RID: 9494 RVA: 0x000BA84C File Offset: 0x000B8A4C
	public static string ActiveSceneName
	{
		get
		{
			return SceneManager.GetActiveScene().name;
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x06002517 RID: 9495 RVA: 0x000BA868 File Offset: 0x000B8A68
	public static int ActiveSceneBuildIndex
	{
		get
		{
			return SceneManager.GetActiveScene().buildIndex;
		}
	}
}
