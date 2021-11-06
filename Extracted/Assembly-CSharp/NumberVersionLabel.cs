using System;
using UnityEngine;

// Token: 0x020003D1 RID: 977
public class NumberVersionLabel : MonoBehaviour
{
	// Token: 0x0600236A RID: 9066 RVA: 0x000B0698 File Offset: 0x000AE898
	private void Start()
	{
		UILabel component = base.GetComponent<UILabel>();
		if (component != null)
		{
			component.text = this.CurrentVersion.ToString();
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x0600236B RID: 9067 RVA: 0x000B06CC File Offset: 0x000AE8CC
	private Version CurrentVersion
	{
		get
		{
			return base.GetType().Assembly.GetName().Version;
		}
	}
}
