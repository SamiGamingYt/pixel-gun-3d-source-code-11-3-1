using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class MultipleToggleButton : MonoBehaviour
{
	// Token: 0x14000021 RID: 33
	// (add) Token: 0x06001B5D RID: 7005 RVA: 0x000703E4 File Offset: 0x0006E5E4
	// (remove) Token: 0x06001B5E RID: 7006 RVA: 0x00070400 File Offset: 0x0006E600
	public event EventHandler<MultipleToggleEventArgs> Clicked;

	// Token: 0x170004BE RID: 1214
	// (get) Token: 0x06001B5F RID: 7007 RVA: 0x0007041C File Offset: 0x0006E61C
	// (set) Token: 0x06001B60 RID: 7008 RVA: 0x00070424 File Offset: 0x0006E624
	public int SelectedIndex
	{
		get
		{
			return this._selectedIndex;
		}
		set
		{
			if (this.buttons == null || value == -1)
			{
				return;
			}
			this._selectedIndex = value;
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if (i != this._selectedIndex)
				{
					this.buttons[i].IsChecked = false;
				}
			}
			EventHandler<MultipleToggleEventArgs> clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, new MultipleToggleEventArgs
				{
					Num = this._selectedIndex
				});
			}
		}
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x000704A8 File Offset: 0x0006E6A8
	private void Start()
	{
		if (this.buttons != null)
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttons[i].Clicked += delegate(object sender, ToggleButtonEventArgs e)
				{
					if (e.IsChecked)
					{
						this.SelectedIndex = Array.IndexOf<ToggleButton>(this.buttons, sender as ToggleButton);
					}
				};
			}
		}
	}

	// Token: 0x04001083 RID: 4227
	public ToggleButton[] buttons;

	// Token: 0x04001084 RID: 4228
	private int _selectedIndex;
}
