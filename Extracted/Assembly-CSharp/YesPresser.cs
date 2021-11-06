using System;
using UnityEngine;

// Token: 0x0200088E RID: 2190
public sealed class YesPresser : SkipTrainingButton
{
	// Token: 0x06004EBE RID: 20158 RVA: 0x001C8B94 File Offset: 0x001C6D94
	protected override void OnClick()
	{
		if (this._clicked)
		{
			return;
		}
		this.noButton.GetComponent<UIButton>().enabled = false;
		base.enabled = false;
		GotToNextLevel.GoToNextLevel();
		this._clicked = true;
	}

	// Token: 0x04003D48 RID: 15688
	public GameObject noButton;

	// Token: 0x04003D49 RID: 15689
	private bool _clicked;
}
