using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
public class HideInLocal : MonoBehaviour
{
	// Token: 0x06001531 RID: 5425 RVA: 0x000541B0 File Offset: 0x000523B0
	private void Start()
	{
		if (!Defs.isInet || Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
	}
}
