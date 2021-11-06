using System;
using UnityEngine;

// Token: 0x020007AB RID: 1963
public class ShopCategoryButton : MonoBehaviour
{
	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x06004600 RID: 17920 RVA: 0x0017A670 File Offset: 0x00178870
	// (remove) Token: 0x06004601 RID: 17921 RVA: 0x0017A688 File Offset: 0x00178888
	public static event Action<ShopCategoryButton> CategoryButtonClicked;

	// Token: 0x06004602 RID: 17922 RVA: 0x0017A6A0 File Offset: 0x001788A0
	private void OnClick()
	{
		Action<ShopCategoryButton> categoryButtonClicked = ShopCategoryButton.CategoryButtonClicked;
		if (categoryButtonClicked != null)
		{
			categoryButtonClicked(this);
		}
	}

	// Token: 0x0400335C RID: 13148
	public UITexture icon;

	// Token: 0x0400335D RID: 13149
	public GameObject emptyIcon;

	// Token: 0x0400335E RID: 13150
	public Transform modelPoint;
}
