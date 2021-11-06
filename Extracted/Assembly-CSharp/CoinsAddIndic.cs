using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000073 RID: 115
public class CoinsAddIndic : MonoBehaviour
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x06000343 RID: 835 RVA: 0x0001BD8C File Offset: 0x00019F8C
	private UISprite ind
	{
		get
		{
			if (this._ind == null)
			{
				this._ind = base.GetComponent<UISprite>();
			}
			return this._ind;
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x0001BDB4 File Offset: 0x00019FB4
	private void Start()
	{
		this.isSurvival = Defs.IsSurvival;
		if (this.remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= this.BackgroundEventAdd;
			CoinsMessage.CoinsLabelDisappeared += this.BackgroundEventAdd;
		}
	}

	// Token: 0x06000345 RID: 837 RVA: 0x0001BDFC File Offset: 0x00019FFC
	private void OnEnable()
	{
		CoinsMessage.CoinsLabelDisappeared += this.IndicateCoinsAdd;
		if (this.ind != null)
		{
			this.ind.color = this.NormalColor();
		}
		if (this.backgroundAdd > 0)
		{
			this.blinking = false;
			this.IndicateCoinsAdd(this.backgroundAdd == 1, 2);
			this.backgroundAdd = 0;
		}
		if (this.blinking && !this.stopBlinkingOnEnable)
		{
			base.StartCoroutine(this.blink());
		}
		else if (this.stopBlinkingOnEnable)
		{
			this.blinking = false;
		}
	}

	// Token: 0x06000346 RID: 838 RVA: 0x0001BEAC File Offset: 0x0001A0AC
	private void OnDisable()
	{
		CoinsMessage.CoinsLabelDisappeared -= this.IndicateCoinsAdd;
	}

	// Token: 0x06000347 RID: 839 RVA: 0x0001BEC0 File Offset: 0x0001A0C0
	private void OnDestroy()
	{
		if (this.remembeState)
		{
			CoinsMessage.CoinsLabelDisappeared -= this.BackgroundEventAdd;
		}
	}

	// Token: 0x06000348 RID: 840 RVA: 0x0001BEE0 File Offset: 0x0001A0E0
	private void IndicateCoinsAdd(bool gems, int count)
	{
		if (this.isGems == gems && !this.blinking)
		{
			base.StartCoroutine(this.blink());
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0001BF14 File Offset: 0x0001A114
	private Color BlinkColor
	{
		get
		{
			return (!this.isGems) ? new Color(1f, 1f, 0f, 0.4509804f) : new Color(0f, 0f, 1f, 0.4509804f);
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x0001BF64 File Offset: 0x0001A164
	private Color NormalColor()
	{
		return (!this.isX3) ? new Color(0f, 0f, 0f, 0.4509804f) : new Color(1f, 0f, 0f, 0.5882353f);
	}

	// Token: 0x0600034B RID: 843 RVA: 0x0001BFB4 File Offset: 0x0001A1B4
	private IEnumerator blink()
	{
		if (this.ind == null)
		{
			Debug.LogWarning("Indicator sprite is null.");
			yield return null;
		}
		this.blinking = true;
		try
		{
			for (int i = 0; i < 15; i++)
			{
				this.ind.color = this.BlinkColor;
				yield return null;
				yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
				this.ind.color = this.NormalColor();
				yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(0.1f));
			}
			this.ind.color = this.NormalColor();
		}
		finally
		{
			this.blinking = false;
		}
		yield break;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0001BFD0 File Offset: 0x0001A1D0
	private void BackgroundEventAdd(bool gems, int count)
	{
		if (this.isGems != gems)
		{
			return;
		}
		if ((BankController.Instance == null || !BankController.Instance.InterfaceEnabled) && GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
		{
			if (gems && this.isGems)
			{
				this.backgroundAdd = 1;
			}
			if (!gems && !this.isGems)
			{
				this.backgroundAdd = 2;
			}
		}
	}

	// Token: 0x04000383 RID: 899
	private const float blinkR = 255f;

	// Token: 0x04000384 RID: 900
	private const float blinkG = 255f;

	// Token: 0x04000385 RID: 901
	private const float blinkB = 0f;

	// Token: 0x04000386 RID: 902
	private const float blinkA = 115f;

	// Token: 0x04000387 RID: 903
	private const float blinkGemsR = 0f;

	// Token: 0x04000388 RID: 904
	private const float blinkGemsG = 0f;

	// Token: 0x04000389 RID: 905
	private const float blinkGemsB = 255f;

	// Token: 0x0400038A RID: 906
	private const float blinkGemsA = 115f;

	// Token: 0x0400038B RID: 907
	public bool stopBlinkingOnEnable;

	// Token: 0x0400038C RID: 908
	public bool isGems;

	// Token: 0x0400038D RID: 909
	public bool isX3;

	// Token: 0x0400038E RID: 910
	private UISprite _ind;

	// Token: 0x0400038F RID: 911
	private bool blinking;

	// Token: 0x04000390 RID: 912
	public bool remembeState;

	// Token: 0x04000391 RID: 913
	private int backgroundAdd;

	// Token: 0x04000392 RID: 914
	private bool isSurvival;
}
