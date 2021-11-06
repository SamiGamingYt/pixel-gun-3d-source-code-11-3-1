using System;
using UnityEngine;

// Token: 0x020005BB RID: 1467
public class ChestBonusView : MonoBehaviour
{
	// Token: 0x060032AD RID: 12973 RVA: 0x001069FC File Offset: 0x00104BFC
	private void SetTitleText(string text)
	{
		for (int i = 0; i < this.title.Length; i++)
		{
			this.title[i].text = text;
		}
	}

	// Token: 0x060032AE RID: 12974 RVA: 0x00106A30 File Offset: 0x00104C30
	public void OnButtonOkClick()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060032AF RID: 12975 RVA: 0x00106A40 File Offset: 0x00104C40
	public void Show(ChestBonusData bonus)
	{
		if (bonus.items == null || bonus.items.Count == 0)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.SetTitleText(LocalizationStore.Get("Key_1057"));
		this.description.text = LocalizationStore.Get("Key_1058");
		this.CreateBonusesItemsAndAlign(bonus);
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x00106AA4 File Offset: 0x00104CA4
	private void CreateBonusesItemsAndAlign(ChestBonusData bonus)
	{
		int num = 0;
		for (int i = 0; i < this.bonusItems.Length; i++)
		{
			if (i >= bonus.items.Count)
			{
				this.bonusItems[i].SetVisible(false);
				num++;
			}
			else
			{
				this.bonusItems[i].SetVisible(true);
				this.bonusItems[i].SetData(bonus.items[i]);
			}
		}
		this.CenterItems(num);
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x00106B24 File Offset: 0x00104D24
	private void CenterItems(int countHideElements)
	{
		float num = (float)countHideElements / 2f;
		float num2 = this.cellWidth * num;
		int num3 = this.bonusItems.Length - countHideElements;
		for (int i = 0; i < num3; i++)
		{
			Vector3 localPosition = this.bonusItems[i].transform.localPosition;
			float x = this.startXPos + num2 + this.cellWidth * (float)i;
			this.bonusItems[i].transform.localPosition = new Vector3(x, localPosition.y, localPosition.z);
		}
	}

	// Token: 0x04002537 RID: 9527
	public UILabel[] title;

	// Token: 0x04002538 RID: 9528
	public UILabel description;

	// Token: 0x04002539 RID: 9529
	public ChestBonusItem[] bonusItems;

	// Token: 0x0400253A RID: 9530
	public float cellWidth;

	// Token: 0x0400253B RID: 9531
	public float startXPos;
}
