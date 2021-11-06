using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class ElixirSprite : MonoBehaviour
{
	// Token: 0x06000522 RID: 1314 RVA: 0x00029E0C File Offset: 0x0002800C
	private void Start()
	{
		bool flag = !Defs.isMulti;
		base.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		this.spr = base.GetComponent<UISprite>();
		this.spr.enabled = false;
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00029E50 File Offset: 0x00028050
	private void Update()
	{
	}

	// Token: 0x0400059A RID: 1434
	private UISprite spr;
}
