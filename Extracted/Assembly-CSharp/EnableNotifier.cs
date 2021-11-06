using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class EnableNotifier : MonoBehaviour
{
	// Token: 0x06000525 RID: 1317 RVA: 0x00029E68 File Offset: 0x00028068
	private void OnEnable()
	{
		if (!this.isSoundFX)
		{
			EventDelegate.Execute(this.onEnable);
		}
		else if (Defs.isSoundFX)
		{
			EventDelegate.Execute(this.onEnable);
		}
	}

	// Token: 0x0400059B RID: 1435
	public List<EventDelegate> onEnable = new List<EventDelegate>();

	// Token: 0x0400059C RID: 1436
	public bool isSoundFX;
}
