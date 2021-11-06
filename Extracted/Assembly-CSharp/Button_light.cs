using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public sealed class Button_light : MonoBehaviour
{
	// Token: 0x0600021B RID: 539 RVA: 0x000137B8 File Offset: 0x000119B8
	private void Start()
	{
		if (this.lightTexture != null)
		{
			this.lightTexture.enabled = false;
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x000137D8 File Offset: 0x000119D8
	private void OnPress(bool isDown)
	{
		if (this.lightTexture != null)
		{
			this.lightTexture.enabled = isDown;
		}
	}

	// Token: 0x04000246 RID: 582
	public UITexture lightTexture;
}
