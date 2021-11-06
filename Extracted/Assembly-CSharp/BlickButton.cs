using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class BlickButton : MonoBehaviour
{
	// Token: 0x0600016B RID: 363 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
	private void Start()
	{
		this.blickSprite.gameObject.SetActive(false);
		base.StartCoroutine(this.Blink());
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000E9E4 File Offset: 0x0000CBE4
	private IEnumerator Blink()
	{
		yield return base.StartCoroutine(this.MyWaitForSeconds(this.firstSdvig));
		for (;;)
		{
			while (this.baseButton.state == UIButtonColor.State.Disabled)
			{
				yield return null;
			}
			yield return base.StartCoroutine(this.MyWaitForSeconds(this.blickPeriod));
			this.blickSprite.gameObject.SetActive(true);
			for (int i = 0; i < this.countFrame; i++)
			{
				this.blickSprite.spriteName = this.baseNameSprite + i;
				yield return base.StartCoroutine(this.MyWaitForSeconds(this.blickSpeed));
			}
			this.blickSprite.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000EA00 File Offset: 0x0000CC00
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000EA24 File Offset: 0x0000CC24
	private void Update()
	{
	}

	// Token: 0x04000155 RID: 341
	public float firstSdvig;

	// Token: 0x04000156 RID: 342
	public float blickPeriod = 3f;

	// Token: 0x04000157 RID: 343
	public float blickSpeed = 0.3f;

	// Token: 0x04000158 RID: 344
	public UISprite blickSprite;

	// Token: 0x04000159 RID: 345
	public UIButton baseButton;

	// Token: 0x0400015A RID: 346
	public string baseNameSprite;

	// Token: 0x0400015B RID: 347
	public int countFrame;
}
