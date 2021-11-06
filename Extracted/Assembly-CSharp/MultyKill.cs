using System;
using UnityEngine;

// Token: 0x0200030F RID: 783
public class MultyKill : MonoBehaviour
{
	// Token: 0x06001B64 RID: 7012 RVA: 0x0007052C File Offset: 0x0006E72C
	private void OnEnable()
	{
		if (Defs.isSoundFX)
		{
			this.multikillSound.Play();
		}
		this.multikillTween.Play(true);
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x00070550 File Offset: 0x0006E750
	public void PlayTween()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
		base.transform.GetChild(0).gameObject.SetActive(true);
		if (Defs.isSoundFX)
		{
			this.multikillSound.Stop();
			this.multikillSound.Play();
		}
		this.multikillTween.Play(true);
	}

	// Token: 0x04001086 RID: 4230
	public AudioSource multikillSound;

	// Token: 0x04001087 RID: 4231
	public UIPlayTween multikillTween;

	// Token: 0x04001088 RID: 4232
	public UISprite scorePict;

	// Token: 0x04001089 RID: 4233
	private string scorePictTempName;
}
