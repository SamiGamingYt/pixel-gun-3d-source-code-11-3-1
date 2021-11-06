using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public sealed class CustomCapePicker : MonoBehaviour
{
	// Token: 0x0600040B RID: 1035 RVA: 0x00023504 File Offset: 0x00021704
	private void Start()
	{
		if (this.shouldLoadTexture)
		{
			Texture capeUserTexture = SkinsController.capeUserTexture;
			capeUserTexture.filterMode = FilterMode.Point;
			Player_move_c.SetTextureRecursivelyFrom(base.gameObject, capeUserTexture, new GameObject[0]);
		}
	}

	// Token: 0x0400049D RID: 1181
	public bool shouldLoadTexture = true;
}
