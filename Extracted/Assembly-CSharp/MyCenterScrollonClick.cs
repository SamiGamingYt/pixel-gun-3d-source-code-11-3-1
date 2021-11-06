using System;

// Token: 0x02000310 RID: 784
public class MyCenterScrollonClick : UIDragScrollView
{
	// Token: 0x06001B67 RID: 7015 RVA: 0x000705C0 File Offset: 0x0006E7C0
	private void Awake()
	{
		if (this.center == null)
		{
			this.center = NGUITools.FindInParents<MyCenterOnChild>(base.gameObject);
		}
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x000705F0 File Offset: 0x0006E7F0
	private void OnClick()
	{
		this.center.CenterOn(base.transform);
	}

	// Token: 0x0400108A RID: 4234
	private MyCenterOnChild center;
}
