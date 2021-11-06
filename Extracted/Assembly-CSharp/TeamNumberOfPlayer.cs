using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000856 RID: 2134
public class TeamNumberOfPlayer : MonoBehaviour
{
	// Token: 0x06004D4B RID: 19787 RVA: 0x001BE5DC File Offset: 0x001BC7DC
	private void Start()
	{
		if (this.button2x2 != null)
		{
			ButtonHandler component = this.button2x2.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleButton2x2Clicked;
			}
		}
		if (this.button3x3 != null)
		{
			ButtonHandler component2 = this.button3x3.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleButton3x3Clicked;
			}
		}
		if (this.button4x4 != null)
		{
			ButtonHandler component3 = this.button4x4.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleButton4x4Clicked;
			}
		}
		if (this.button5x5 != null)
		{
			ButtonHandler component4 = this.button5x5.GetComponent<ButtonHandler>();
			if (component4 != null)
			{
				component4.Clicked += this.HandleButton5x5Clicked;
			}
		}
		this.value = 10;
		this.button2x2.GetComponent<UIButton>().isEnabled = true;
		this.button3x3.GetComponent<UIButton>().isEnabled = true;
		this.button4x4.GetComponent<UIButton>().isEnabled = true;
		this.button5x5.GetComponent<UIButton>().isEnabled = false;
	}

	// Token: 0x06004D4C RID: 19788 RVA: 0x001BE724 File Offset: 0x001BC924
	public void SetValue(int _value)
	{
		this.value = _value;
		this.button2x2.GetComponent<UIButton>().isEnabled = (this.value != 4);
		this.button3x3.GetComponent<UIButton>().isEnabled = (this.value != 6);
		this.button4x4.GetComponent<UIButton>().isEnabled = (this.value != 8);
		this.button5x5.GetComponent<UIButton>().isEnabled = (this.value != 10);
	}

	// Token: 0x06004D4D RID: 19789 RVA: 0x001BE7AC File Offset: 0x001BC9AC
	private void HandleButton2x2Clicked(object sender, EventArgs e)
	{
		this.SetValue(4);
	}

	// Token: 0x06004D4E RID: 19790 RVA: 0x001BE7B8 File Offset: 0x001BC9B8
	private void HandleButton3x3Clicked(object sender, EventArgs e)
	{
		this.SetValue(6);
	}

	// Token: 0x06004D4F RID: 19791 RVA: 0x001BE7C4 File Offset: 0x001BC9C4
	private void HandleButton4x4Clicked(object sender, EventArgs e)
	{
		this.SetValue(8);
	}

	// Token: 0x06004D50 RID: 19792 RVA: 0x001BE7D0 File Offset: 0x001BC9D0
	private void HandleButton5x5Clicked(object sender, EventArgs e)
	{
		this.SetValue(10);
	}

	// Token: 0x04003BB5 RID: 15285
	public int value;

	// Token: 0x04003BB6 RID: 15286
	public GameObject button2x2;

	// Token: 0x04003BB7 RID: 15287
	public GameObject button3x3;

	// Token: 0x04003BB8 RID: 15288
	public GameObject button4x4;

	// Token: 0x04003BB9 RID: 15289
	public GameObject button5x5;

	// Token: 0x04003BBA RID: 15290
	private int oldValue = 8;
}
