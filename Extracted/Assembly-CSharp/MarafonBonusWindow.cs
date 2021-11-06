using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B9 RID: 1721
public class MarafonBonusWindow : BannerWindow
{
	// Token: 0x06003C07 RID: 15367 RVA: 0x00137D1C File Offset: 0x00135F1C
	private void FillBonusesForEveryday()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		BonusEverydayItem[] componentsInChildren = this.bonusScroll.GetComponentsInChildren<BonusEverydayItem>(true);
		bool flag = componentsInChildren.Length != 0;
		BonusEverydayItem bonusEverydayItem = null;
		GameObject gameObject = null;
		for (int i = 0; i < bonusItems.Count; i++)
		{
			if (!flag)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bonusEverydayItem);
				gameObject.name = string.Format("{0:00}", i);
			}
			bonusEverydayItem = ((!flag) ? gameObject.GetComponent<BonusEverydayItem>() : componentsInChildren[i]);
			if (bonusEverydayItem != null)
			{
				bool isBonusWeekly = (i + 1) % 7 == 0 || i == bonusItems.Count - 1;
				bonusEverydayItem.FillData(i, currentBonusIndex, isBonusWeekly);
			}
			if (!flag)
			{
				this.bonusScroll.AddChild(gameObject.transform);
				gameObject.gameObject.SetActive(true);
			}
			bonusEverydayItem.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
		}
		bonusEverydayItem.SetBackgroundForBonusWeek();
		this.bonusScroll.Reposition();
	}

	// Token: 0x06003C08 RID: 15368 RVA: 0x00137E48 File Offset: 0x00136048
	private void FillPrizesForEveryweek()
	{
		List<BonusMarafonItem> bonusItems = MarafonBonusController.Get.BonusItems;
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		int num = 0;
		for (int i = 6; i < bonusItems.Count; i += 7)
		{
			BonusEverydayItem bonusEverydayItem = this.superPrizes[num];
			num++;
			if (bonusEverydayItem != null)
			{
				bonusEverydayItem.FillData(i, currentBonusIndex, false);
			}
		}
		int num2 = this.superPrizes.Length - 1;
		int bonusIndex = bonusItems.Count - 1;
		this.superPrizes[num2].FillData(bonusIndex, currentBonusIndex, false);
	}

	// Token: 0x06003C09 RID: 15369 RVA: 0x00137ED4 File Offset: 0x001360D4
	private void OnEnable()
	{
		base.StartCoroutine(this.StartCentralizeBonusItem());
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x00137EE4 File Offset: 0x001360E4
	public override void Show()
	{
		MarafonBonusController.Get.InitializeBonusItems();
		this.FillBonusesForEveryday();
		this.FillPrizesForEveryweek();
		base.Show();
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00137F10 File Offset: 0x00136110
	public IEnumerator StartCentralizeBonusItem()
	{
		yield return null;
		this.CentralizeScrollByCurrentBonus();
		yield break;
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00137F2C File Offset: 0x0013612C
	private void ResetScrollPosition(GameObject centerElement)
	{
		this.bonusScroll.GetComponent<UICenterOnChild>().enabled = false;
		this.bonusScroll.Reposition();
	}

	// Token: 0x06003C0D RID: 15373 RVA: 0x00137F4C File Offset: 0x0013614C
	public void OnGetRewardClick()
	{
		ButtonClickSound.TryPlayClick();
		this.scrollView.ResetPosition();
		MarafonBonusController.Get.TakeMarafonBonus();
		BannerWindowController.SharedController.HideBannerWindow();
	}

	// Token: 0x06003C0E RID: 15374 RVA: 0x00137F80 File Offset: 0x00136180
	private void CentralizeScrollByCurrentBonus()
	{
		if (this.bonusScroll == null)
		{
			return;
		}
		int currentBonusIndex = MarafonBonusController.Get.GetCurrentBonusIndex();
		Transform child = this.bonusScroll.GetChild(currentBonusIndex);
		if (child != null)
		{
			if (currentBonusIndex > 2 && currentBonusIndex < 27)
			{
				this.bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				this.bonusScroll.GetComponent<UICenterOnChild>().CenterOn(child);
			}
			else if (currentBonusIndex >= 27)
			{
				this.bonusScroll.GetComponent<UICenterOnChild>().springStrength = 8f;
				Transform child2 = this.bonusScroll.GetChild(27);
				if (child2 != null)
				{
					this.bonusScroll.GetComponent<UICenterOnChild>().CenterOn(child2);
				}
			}
			child.localScale = Vector3.one;
		}
		this.bonusScroll.GetComponent<UICenterOnChild>().onCenter = new UICenterOnChild.OnCenterCallback(this.ResetScrollPosition);
	}

	// Token: 0x06003C0F RID: 15375 RVA: 0x00138070 File Offset: 0x00136270
	internal sealed override void Submit()
	{
		this.OnGetRewardClick();
	}

	// Token: 0x06003C10 RID: 15376 RVA: 0x00138078 File Offset: 0x00136278
	private void Update()
	{
		if (this.premiumInterface.activeSelf != (PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive))
		{
			this.premiumInterface.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
		}
	}

	// Token: 0x04002C65 RID: 11365
	public GameObject premiumInterface;

	// Token: 0x04002C66 RID: 11366
	public UIScrollView bonusScrollView;

	// Token: 0x04002C67 RID: 11367
	public UIGrid bonusScroll;

	// Token: 0x04002C68 RID: 11368
	public UILabel title;

	// Token: 0x04002C69 RID: 11369
	public GameObject bonusEverydayItem;

	// Token: 0x04002C6A RID: 11370
	public UIScrollView scrollView;

	// Token: 0x04002C6B RID: 11371
	public BonusEverydayItem[] superPrizes;
}
