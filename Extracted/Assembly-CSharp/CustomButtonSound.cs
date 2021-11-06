using System;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class CustomButtonSound : MonoBehaviour
{
	// Token: 0x06000409 RID: 1033 RVA: 0x000234C4 File Offset: 0x000216C4
	private void OnClick()
	{
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.clickSound, 1f, 1f);
		}
	}

	// Token: 0x0400049C RID: 1180
	public AudioClip clickSound;
}
