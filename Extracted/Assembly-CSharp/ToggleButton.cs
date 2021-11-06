using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000862 RID: 2146
public class ToggleButton : MonoBehaviour
{
	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x06004D8A RID: 19850 RVA: 0x001C0984 File Offset: 0x001BEB84
	// (remove) Token: 0x06004D8B RID: 19851 RVA: 0x001C09A0 File Offset: 0x001BEBA0
	public event EventHandler<ToggleButtonEventArgs> Clicked;

	// Token: 0x06004D8C RID: 19852 RVA: 0x001C09BC File Offset: 0x001BEBBC
	public void SetCheckedImage(bool c)
	{
		this.offButton.gameObject.SetActive(!c);
		this.onButton.gameObject.SetActive(c);
		if (this.useForMultipleToggle)
		{
			this.onButton.isEnabled = !this.onButton.gameObject.activeSelf;
		}
	}

	// Token: 0x17000CB2 RID: 3250
	// (get) Token: 0x06004D8D RID: 19853 RVA: 0x001C0A18 File Offset: 0x001BEC18
	// (set) Token: 0x06004D8E RID: 19854 RVA: 0x001C0A20 File Offset: 0x001BEC20
	public bool IsChecked
	{
		get
		{
			return this._isChecked;
		}
		set
		{
			this.SetCheckedWithoutEvent(value);
			EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new ToggleButtonEventArgs
				{
					IsChecked = this._isChecked
				});
			}
		}
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x001C0A5C File Offset: 0x001BEC5C
	public void SetCheckedWithoutEvent(bool val)
	{
		this._isChecked = val;
		this.offButton.gameObject.SetActive(!this._isChecked);
		this.onButton.gameObject.SetActive(this._isChecked);
		if (this.useForMultipleToggle)
		{
			this.onButton.isEnabled = !this.onButton.gameObject.activeSelf;
		}
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x001C0AC8 File Offset: 0x001BECC8
	private void Start()
	{
		this.onButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			this.IsChecked = false;
		};
		this.offButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
		{
			this.IsChecked = true;
		};
	}

	// Token: 0x04003C00 RID: 15360
	public UIButton offButton;

	// Token: 0x04003C01 RID: 15361
	public UIButton onButton;

	// Token: 0x04003C02 RID: 15362
	public bool useForMultipleToggle = true;

	// Token: 0x04003C03 RID: 15363
	private bool _isChecked;
}
