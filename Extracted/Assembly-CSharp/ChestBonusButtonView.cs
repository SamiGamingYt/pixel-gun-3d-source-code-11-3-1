using System;
using I2.Loc;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
public class ChestBonusButtonView : MonoBehaviour
{
	// Token: 0x06003282 RID: 12930 RVA: 0x001060F8 File Offset: 0x001042F8
	private void Awake()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x0010610C File Offset: 0x0010430C
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x00106120 File Offset: 0x00104320
	private void HandleLocalizationChanged()
	{
		this.CheckBonusButtonUpdate();
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x00106128 File Offset: 0x00104328
	public void Initialize()
	{
		ChestBonusController.OnChestBonusChange += this.CheckBonusButtonUpdate;
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x0010613C File Offset: 0x0010433C
	public void UpdateState(PurchaseEventArgs purchaseInfo)
	{
		this._purchaseInfo = purchaseInfo;
		this.CheckBonusButtonUpdate();
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x0010614C File Offset: 0x0010434C
	public void Deinitialize()
	{
		ChestBonusController.OnChestBonusChange -= this.CheckBonusButtonUpdate;
		this._purchaseInfo = null;
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x00106168 File Offset: 0x00104368
	private void CheckBonusButtonUpdate()
	{
		bool flag = this._purchaseInfo != null && ChestBonusController.Get.IsBonusActiveForItem(this._purchaseInfo);
		base.gameObject.SetActive(flag);
		if (flag)
		{
			this.SetViewData(this._purchaseInfo);
		}
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x001061B8 File Offset: 0x001043B8
	private void SetViewData(PurchaseEventArgs purchaseInfo)
	{
		ChestBonusData bonusData = ChestBonusController.Get.GetBonusData(purchaseInfo);
		this.timeOrCountLabel.text = bonusData.GetItemCountOrTime();
		this.itemTexture.mainTexture = bonusData.GetImage();
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x001061F4 File Offset: 0x001043F4
	public void OnButtonClick()
	{
		ChestBonusController.Get.ShowBonusWindowForItem(this._purchaseInfo);
	}

	// Token: 0x0400251A RID: 9498
	public UILabel timeOrCountLabel;

	// Token: 0x0400251B RID: 9499
	public UITexture itemTexture;

	// Token: 0x0400251C RID: 9500
	private PurchaseEventArgs _purchaseInfo;
}
