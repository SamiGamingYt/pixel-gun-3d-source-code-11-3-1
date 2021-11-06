using System;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public class SetDaterIconInFastShop : MonoBehaviour
{
	// Token: 0x060045CB RID: 17867 RVA: 0x0017965C File Offset: 0x0017785C
	private void Awake()
	{
		if (Defs.isDaterRegim)
		{
			base.GetComponent<UISprite>().spriteName = this.daterIconName;
		}
	}

	// Token: 0x0400332C RID: 13100
	public string daterIconName;
}
