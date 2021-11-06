using System;

// Token: 0x0200077A RID: 1914
public class UIInputRilisoft : UIInput
{
	// Token: 0x06004339 RID: 17209 RVA: 0x00167040 File Offset: 0x00165240
	protected override void OnSelect(bool isSelected)
	{
		base.OnSelect(isSelected);
		if (isSelected && this.onFocus != null)
		{
			this.onFocus();
		}
		else if (!isSelected && this.onFocusLost != null)
		{
			this.onFocusLost();
		}
	}

	// Token: 0x0400313F RID: 12607
	public UIInputRilisoft.OnFocus onFocus;

	// Token: 0x04003140 RID: 12608
	public UIInputRilisoft.OnFocusLost onFocusLost;

	// Token: 0x02000926 RID: 2342
	// (Invoke) Token: 0x06005138 RID: 20792
	public delegate void OnFocus();

	// Token: 0x02000927 RID: 2343
	// (Invoke) Token: 0x0600513C RID: 20796
	public delegate void OnFocusLost();
}
