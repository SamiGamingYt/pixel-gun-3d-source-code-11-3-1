using System;
using UnityEngine;

// Token: 0x02000035 RID: 53
internal sealed class BlinkReloadButton : MonoBehaviour
{
	// Token: 0x06000175 RID: 373 RVA: 0x0000ED84 File Offset: 0x0000CF84
	private void Start()
	{
		BlinkReloadButton.isBlink = false;
		BlinkReloadButton.isBlinkState = false;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000ED94 File Offset: 0x0000CF94
	private void Update()
	{
		this.isBlinkTemp = BlinkReloadButton.isBlink;
		if (this.isBlinkOld != BlinkReloadButton.isBlink)
		{
			this.timerBlink = this.maxTimerBlink;
		}
		if (BlinkReloadButton.isBlink)
		{
			this.timerBlink -= Time.deltaTime;
			if (this.timerBlink < 0f)
			{
				this.timerBlink = this.maxTimerBlink;
				BlinkReloadButton.isBlinkState = !BlinkReloadButton.isBlinkState;
				for (int i = 0; i < this.blinkObjs.Length; i++)
				{
					this.blinkObjs[i].color = ((!BlinkReloadButton.isBlinkState) ? this.unBlinkColor : this.blinkColor);
				}
			}
		}
		if (!BlinkReloadButton.isBlink && BlinkReloadButton.isBlinkState)
		{
			BlinkReloadButton.isBlinkState = !BlinkReloadButton.isBlinkState;
			for (int j = 0; j < this.blinkObjs.Length; j++)
			{
				this.blinkObjs[j].color = ((!BlinkReloadButton.isBlinkState) ? this.unBlinkColor : this.blinkColor);
			}
		}
		this.isBlinkOld = BlinkReloadButton.isBlink;
	}

	// Token: 0x0400016C RID: 364
	public static bool isBlink;

	// Token: 0x0400016D RID: 365
	private bool isBlinkOld;

	// Token: 0x0400016E RID: 366
	private float timerBlink;

	// Token: 0x0400016F RID: 367
	public float maxTimerBlink = 0.5f;

	// Token: 0x04000170 RID: 368
	public Color blinkColor = new Color(1f, 0f, 0f);

	// Token: 0x04000171 RID: 369
	public Color unBlinkColor = new Color(1f, 1f, 1f);

	// Token: 0x04000172 RID: 370
	public static bool isBlinkState;

	// Token: 0x04000173 RID: 371
	public UISprite[] blinkObjs;

	// Token: 0x04000174 RID: 372
	public bool isBlinkTemp;
}
