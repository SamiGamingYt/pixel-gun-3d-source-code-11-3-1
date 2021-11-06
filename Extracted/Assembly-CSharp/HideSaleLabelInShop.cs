using System;
using UnityEngine;

// Token: 0x0200029F RID: 671
public sealed class HideSaleLabelInShop : MonoBehaviour
{
	// Token: 0x06001535 RID: 5429 RVA: 0x00054248 File Offset: 0x00052448
	private void Update()
	{
		if (this.needTier.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000C61 RID: 3169
	public GameObject needTier;
}
