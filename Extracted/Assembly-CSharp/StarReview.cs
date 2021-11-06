using System;
using UnityEngine;

// Token: 0x0200073E RID: 1854
public class StarReview : MonoBehaviour
{
	// Token: 0x06004146 RID: 16710 RVA: 0x0015BFCC File Offset: 0x0015A1CC
	public void SetActiveStar(bool val)
	{
		if (this.objActiveStar)
		{
			this.objActiveStar.SetActive(val);
		}
	}

	// Token: 0x06004147 RID: 16711 RVA: 0x0015BFEC File Offset: 0x0015A1EC
	private void OnPress(bool isDown)
	{
		if (isDown)
		{
			ReviewHUDWindow.Instance.SelectStar(this);
		}
		else
		{
			ReviewHUDWindow.Instance.SelectStar(null);
		}
	}

	// Token: 0x06004148 RID: 16712 RVA: 0x0015C010 File Offset: 0x0015A210
	private void OnClick()
	{
		ReviewHUDWindow.Instance.OnClickStarRating();
	}

	// Token: 0x04002FA8 RID: 12200
	[HideInInspector]
	public int numOrderStar;

	// Token: 0x04002FA9 RID: 12201
	public UILabel lbNumStar;

	// Token: 0x04002FAA RID: 12202
	public GameObject objFonStar;

	// Token: 0x04002FAB RID: 12203
	public GameObject objActiveStar;
}
