using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class RefreshAndUpdateAnchors : MonoBehaviour
{
	// Token: 0x06002B7E RID: 11134 RVA: 0x000E5328 File Offset: 0x000E3528
	private void Start()
	{
		this.panel.ResetAndUpdateAnchors();
		this.panel.Refresh();
	}

	// Token: 0x04002087 RID: 8327
	public UIPanel panel;
}
