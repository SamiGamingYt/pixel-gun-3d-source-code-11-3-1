using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class PlusMinusController : MonoBehaviour
{
	// Token: 0x060029DD RID: 10717 RVA: 0x000DCE5C File Offset: 0x000DB05C
	private void Awake()
	{
		this.minValue.Value = 4;
		this.maxValue.Value = 8;
		this.value.Value = 4;
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x000DCE90 File Offset: 0x000DB090
	private void Start()
	{
		if (this.plusButton != null)
		{
			ButtonHandler component = this.plusButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandlePlusButtonClicked;
			}
		}
		if (this.minusButton != null)
		{
			ButtonHandler component2 = this.minusButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleMinusButtonClicked;
			}
		}
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000DCF14 File Offset: 0x000DB114
	private void HandlePlusButtonClicked(object sender, EventArgs e)
	{
		this.value.Value = this.value.Value + this.stepValue;
		if (this.value.Value > this.maxValue.Value)
		{
			this.value.Value = this.maxValue.Value;
		}
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x000DCF6C File Offset: 0x000DB16C
	private void HandleMinusButtonClicked(object sender, EventArgs e)
	{
		this.value.Value = this.value.Value - this.stepValue;
		if (this.value.Value < this.minValue.Value)
		{
			this.value.Value = this.minValue.Value;
		}
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000DCFC4 File Offset: 0x000DB1C4
	private void Update()
	{
		if (this.countLabel != null)
		{
			this.countLabel.text = this.value.Value.ToString();
		}
	}

	// Token: 0x04001EF5 RID: 7925
	public int stepValue = 1;

	// Token: 0x04001EF6 RID: 7926
	public SaltedInt minValue = default(SaltedInt);

	// Token: 0x04001EF7 RID: 7927
	public SaltedInt maxValue = default(SaltedInt);

	// Token: 0x04001EF8 RID: 7928
	public SaltedInt value = default(SaltedInt);

	// Token: 0x04001EF9 RID: 7929
	public GameObject plusButton;

	// Token: 0x04001EFA RID: 7930
	public GameObject minusButton;

	// Token: 0x04001EFB RID: 7931
	public UILabel countLabel;

	// Token: 0x04001EFC RID: 7932
	public UILabel headLabel;
}
