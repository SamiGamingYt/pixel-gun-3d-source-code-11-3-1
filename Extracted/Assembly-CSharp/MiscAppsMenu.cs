using System;
using UnityEngine;

// Token: 0x02000307 RID: 775
internal sealed class MiscAppsMenu : MonoBehaviour
{
	// Token: 0x06001B44 RID: 6980 RVA: 0x0007007C File Offset: 0x0006E27C
	public void UnloadMisc()
	{
		this.misc = null;
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x00070088 File Offset: 0x0006E288
	private void Awake()
	{
		MiscAppsMenu.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04001075 RID: 4213
	public static MiscAppsMenu Instance;

	// Token: 0x04001076 RID: 4214
	public HiddenSettings misc;
}
