using System;
using UnityEngine;

// Token: 0x0200004D RID: 77
public sealed class ButtonClickSound : MonoBehaviour
{
	// Token: 0x06000208 RID: 520 RVA: 0x000134E8 File Offset: 0x000116E8
	private void Start()
	{
		ButtonClickSound.Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x000134FC File Offset: 0x000116FC
	public void PlayClick()
	{
		if (this.Click != null && Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.Click);
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00013528 File Offset: 0x00011728
	public static void TryPlayClick()
	{
		if (ButtonClickSound.Instance == null)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
	}

	// Token: 0x04000239 RID: 569
	public static ButtonClickSound Instance;

	// Token: 0x0400023A RID: 570
	public AudioClip Click;
}
