using System;
using UnityEngine;

// Token: 0x020003EB RID: 1003
public static class GameObjectExtensions
{
	// Token: 0x060023DF RID: 9183 RVA: 0x000B2B74 File Offset: 0x000B0D74
	public static bool GetActive(this GameObject target)
	{
		return target.activeInHierarchy;
	}
}
