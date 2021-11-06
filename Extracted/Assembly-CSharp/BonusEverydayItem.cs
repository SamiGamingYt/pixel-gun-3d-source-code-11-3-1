using System;
using I2.Loc;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
public class BonusEverydayItem : MonoBehaviour
{
	// Token: 0x06003BDB RID: 15323 RVA: 0x001368BC File Offset: 0x00134ABC
	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x001368D0 File Offset: 0x00134AD0
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x001368E4 File Offset: 0x00134AE4
	private void HandleLocalizationChanged()
	{
		this.SetTitleItem();
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x001368EC File Offset: 0x00134AEC
	public void SetCheckForTakedReward()
	{
		this.checkTakedReward.gameObject.SetActive(true);
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x00136900 File Offset: 0x00134B00
	public void SetImageForReward(Texture2D image)
	{
		this.imageReward.mainTexture = image;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00136910 File Offset: 0x00134B10
	public void SetDescriptionItem(string text)
	{
		this.descriptionReward.text = text;
		if (this.descriptionReward1 != null)
		{
			this.descriptionReward1.text = text;
		}
		if (this.descriptionReward2 != null)
		{
			this.descriptionReward2.text = text;
		}
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x00136964 File Offset: 0x00134B64
	private void SetTitleItem(string text)
	{
		this.titleDayTakeReward.text = text;
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x00136974 File Offset: 0x00134B74
	// (set) Token: 0x06003BE3 RID: 15331 RVA: 0x0013697C File Offset: 0x00134B7C
	protected int BonusIndex { get; set; }

	// Token: 0x06003BE4 RID: 15332 RVA: 0x00136988 File Offset: 0x00134B88
	private void SetTitleItem()
	{
		this.SetTitleItem(string.Format("{0} {1}", LocalizationStore.Get("Key_0469"), this.BonusIndex + 1));
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x001369BC File Offset: 0x00134BBC
	public void FillData(int bonusIndex, int currentBonusIndex, bool isBonusWeekly)
	{
		this.BonusIndex = bonusIndex;
		this.SetTitleItem();
		if (bonusIndex < currentBonusIndex)
		{
			this.SetCheckForTakedReward();
		}
		bool flag = bonusIndex == currentBonusIndex;
		if (this.hightlightBonus != null && !isBonusWeekly)
		{
			this.hightlightBonus.alpha = ((!flag) ? 0f : 1f);
		}
		if (this._bonusData != null)
		{
			this.SetDescriptionItem(this._bonusData.GetShortDescription());
			this.SetImageForReward(this._bonusData.GetIcon());
		}
		if (isBonusWeekly && this.hightlightWeeklyBonus != null)
		{
			this.SetBackgroundForBonusWeek();
			this.hightlightWeeklyBonus.alpha = ((!flag) ? 0f : 1f);
		}
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x00136A8C File Offset: 0x00134C8C
	public void SetBackgroundForBonusWeek()
	{
		this.background.gameObject.SetActive(false);
		if (this.backgroundWeekly != null)
		{
			this.backgroundWeekly.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x00136ACC File Offset: 0x00134CCC
	private void OnClickDetailInfo()
	{
		if (this._bonusData == null)
		{
			return;
		}
		string shortDescription = this._bonusData.GetShortDescription();
		string longDescription = this._bonusData.GetLongDescription();
		Texture2D icon = this._bonusData.GetIcon();
		this.windowDetail.SetTitle(shortDescription);
		this.windowDetail.SetDescription(longDescription);
		this.windowDetail.SetImage(icon);
		this.windowDetail.Show();
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x00136B38 File Offset: 0x00134D38
	private void OnClick()
	{
		this.OnClickDetailInfo();
	}

	// Token: 0x04002C39 RID: 11321
	public UISprite checkTakedReward;

	// Token: 0x04002C3A RID: 11322
	public UITexture imageReward;

	// Token: 0x04002C3B RID: 11323
	public UILabel descriptionReward;

	// Token: 0x04002C3C RID: 11324
	public UILabel descriptionReward1;

	// Token: 0x04002C3D RID: 11325
	public UILabel descriptionReward2;

	// Token: 0x04002C3E RID: 11326
	public UILabel titleDayTakeReward;

	// Token: 0x04002C3F RID: 11327
	public UITexture background;

	// Token: 0x04002C40 RID: 11328
	public UITexture backgroundWeekly;

	// Token: 0x04002C41 RID: 11329
	public UIWidget hightlightWeeklyBonus;

	// Token: 0x04002C42 RID: 11330
	public BonusItemDetailInfo windowDetail;

	// Token: 0x04002C43 RID: 11331
	public UIWidget hightlightBonus;

	// Token: 0x04002C44 RID: 11332
	private BonusMarafonItem _bonusData;
}
