using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200087C RID: 2172
public class UpgradeWindow : MonoBehaviour
{
	// Token: 0x06004E6A RID: 20074 RVA: 0x001C6A20 File Offset: 0x001C4C20
	public void SetUpgrade(int num, int minBoughtIndex)
	{
		for (int i = 0; i < this.upgrades.Length; i++)
		{
			this.upgrades[i].gameObject.SetActive(i <= num);
			if (i <= minBoughtIndex)
			{
				this.upgrades[i].enabled = false;
				this.upgrades[i].GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				this.upgrades[i].enabled = true;
				base.StartCoroutine(this.ResetToBeginning(this.upgrades[i]));
				this.upgrades[i].ResetToBeginning();
			}
		}
	}

	// Token: 0x06004E6B RID: 20075 RVA: 0x001C6AD4 File Offset: 0x001C4CD4
	private IEnumerator ResetToBeginning(TweenColor tw)
	{
		yield return null;
		tw.ResetToBeginning();
		yield break;
	}

	// Token: 0x04003D03 RID: 15619
	public TweenColor[] upgrades;
}
