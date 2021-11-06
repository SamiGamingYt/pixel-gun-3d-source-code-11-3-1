using System;
using UnityEngine;

// Token: 0x020006B4 RID: 1716
public class BonusItemDetailInfo : MonoBehaviour
{
	// Token: 0x06003BEA RID: 15338 RVA: 0x00136B48 File Offset: 0x00134D48
	public void SetTitle(string text)
	{
		this.title.text = text;
		this.title1.text = text;
		this.title2.text = text;
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x00136B7C File Offset: 0x00134D7C
	public void SetDescription(string text)
	{
		this.description.text = text;
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x00136B8C File Offset: 0x00134D8C
	public void SetImage(Texture2D image)
	{
		this.imageHolder.mainTexture = image;
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x00136B9C File Offset: 0x00134D9C
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x00136BAC File Offset: 0x00134DAC
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002C46 RID: 11334
	public UILabel title;

	// Token: 0x04002C47 RID: 11335
	public UILabel title1;

	// Token: 0x04002C48 RID: 11336
	public UILabel title2;

	// Token: 0x04002C49 RID: 11337
	public UILabel description;

	// Token: 0x04002C4A RID: 11338
	public UITexture imageHolder;
}
