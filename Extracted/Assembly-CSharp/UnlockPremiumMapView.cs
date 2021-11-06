using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000616 RID: 1558
internal sealed class UnlockPremiumMapView : MonoBehaviour
{
	// Token: 0x1400005B RID: 91
	// (add) Token: 0x0600358F RID: 13711 RVA: 0x00114C24 File Offset: 0x00112E24
	// (remove) Token: 0x06003590 RID: 13712 RVA: 0x00114C40 File Offset: 0x00112E40
	public event EventHandler ClosePressed;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06003591 RID: 13713 RVA: 0x00114C5C File Offset: 0x00112E5C
	// (remove) Token: 0x06003592 RID: 13714 RVA: 0x00114C78 File Offset: 0x00112E78
	public event EventHandler UnlockPressed;

	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x06003593 RID: 13715 RVA: 0x00114C94 File Offset: 0x00112E94
	// (set) Token: 0x06003594 RID: 13716 RVA: 0x00114C9C File Offset: 0x00112E9C
	public int Price
	{
		get
		{
			return this._price;
		}
		set
		{
			this._price = value;
			if (this.priceSprite != null)
			{
				this.priceSprite.spriteName = string.Format("premium_baner_{0}", value);
			}
		}
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x00114CD4 File Offset: 0x00112ED4
	private void OnDestroy()
	{
		if (this.closeButton != null)
		{
			this.closeButton.Clicked -= this.RaiseClosePressed;
		}
		if (this.unlockButton != null)
		{
			this.unlockButton.Clicked -= this.RaiseUnlockPressed;
		}
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x00114D34 File Offset: 0x00112F34
	private void Start()
	{
		if (this.closeButton != null)
		{
			this.closeButton.Clicked += this.RaiseClosePressed;
		}
		if (this.unlockButton != null)
		{
			this.unlockButton.Clicked += this.RaiseUnlockPressed;
		}
		if (this.priceSprite != null)
		{
			this.priceSprite.spriteName = string.Format("premium_baner_{0}", this._price);
		}
		this.SetLabelPrice();
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x00114DC8 File Offset: 0x00112FC8
	private void SetLabelPrice()
	{
		if (this.priceLabel == null || this.priceLabel.Length == 0)
		{
			return;
		}
		string text = string.Format("{0} {1}", this._price, LocalizationStore.Get("Key_1041"));
		for (int i = 0; i < this.priceLabel.Length; i++)
		{
			this.priceLabel[i].text = text;
		}
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x00114E38 File Offset: 0x00113038
	private void RaiseClosePressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("Close event raised.");
		}
		EventHandler closePressed = this.ClosePressed;
		if (closePressed != null)
		{
			closePressed(sender, e);
		}
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x00114E70 File Offset: 0x00113070
	private void RaiseUnlockPressed(object sender, EventArgs e)
	{
		if (Application.isEditor)
		{
			Debug.Log("Unlock event raised.");
		}
		EventHandler unlockPressed = this.UnlockPressed;
		if (unlockPressed != null)
		{
			unlockPressed(sender, e);
		}
	}

	// Token: 0x04002757 RID: 10071
	public ButtonHandler closeButton;

	// Token: 0x04002758 RID: 10072
	public ButtonHandler unlockButton;

	// Token: 0x04002759 RID: 10073
	public UISprite priceSprite;

	// Token: 0x0400275A RID: 10074
	public UILabel[] priceLabel;

	// Token: 0x0400275B RID: 10075
	private int _price = 15;
}
