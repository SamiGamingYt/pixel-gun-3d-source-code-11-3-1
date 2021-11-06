using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000758 RID: 1880
public sealed class SettingsToggleButtons : MonoBehaviour
{
	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06004201 RID: 16897 RVA: 0x0015F324 File Offset: 0x0015D524
	// (remove) Token: 0x06004202 RID: 16898 RVA: 0x0015F340 File Offset: 0x0015D540
	public event EventHandler<ToggleButtonEventArgs> Clicked;

	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x06004203 RID: 16899 RVA: 0x0015F35C File Offset: 0x0015D55C
	// (set) Token: 0x06004204 RID: 16900 RVA: 0x0015F38C File Offset: 0x0015D58C
	public bool IsChecked
	{
		get
		{
			if (this._toggle != null)
			{
				return this._toggle.value;
			}
			return this._isChecked;
		}
		set
		{
			if (this._toggle != null)
			{
				this._toggle.value = value;
			}
			else
			{
				this._isChecked = value;
				if (this.offButton == null || this.onButton == null)
				{
					Debug.LogError(string.Format("toggle not setted, GO: '{0}'", base.gameObject.name));
					return;
				}
				this.offButton.isEnabled = this._isChecked;
				this.onButton.isEnabled = !this._isChecked;
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
	}

	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x06004205 RID: 16901 RVA: 0x0015F44C File Offset: 0x0015D64C
	private UIToggle _toggle
	{
		get
		{
			if (this._toggleVal == null)
			{
				this._toggleVal = base.gameObject.GetComponentInChildren<UIToggle>(true);
				if (this._toggleVal != null)
				{
					this._toggleVal.onChange.Add(new EventDelegate(new EventDelegate.Callback(this.OnValueChanged)));
				}
			}
			return this._toggleVal;
		}
	}

	// Token: 0x06004206 RID: 16902 RVA: 0x0015F4B4 File Offset: 0x0015D6B4
	private void OnValueChanged()
	{
		EventHandler<ToggleButtonEventArgs> clicked = this.Clicked;
		if (clicked != null)
		{
			clicked(this, new ToggleButtonEventArgs
			{
				IsChecked = this._toggle.value
			});
		}
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x0015F4F0 File Offset: 0x0015D6F0
	private void Start()
	{
		if (this._toggle == null)
		{
			this.onButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				this.IsChecked = true;
			};
			this.offButton.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				this.IsChecked = false;
			};
		}
	}

	// Token: 0x04003049 RID: 12361
	public UIButton offButton;

	// Token: 0x0400304A RID: 12362
	public UIButton onButton;

	// Token: 0x0400304B RID: 12363
	private bool _isChecked;

	// Token: 0x0400304C RID: 12364
	private UIToggle _toggleVal;
}
