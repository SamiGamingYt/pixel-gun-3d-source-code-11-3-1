using System;
using I2.Loc;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class PromoActionPreview : MonoBehaviour
{
	// Token: 0x1700073D RID: 1853
	// (get) Token: 0x06002A53 RID: 10835 RVA: 0x000DFFD4 File Offset: 0x000DE1D4
	// (set) Token: 0x06002A54 RID: 10836 RVA: 0x000DFFDC File Offset: 0x000DE1DC
	public int Discount { get; set; }

	// Token: 0x06002A55 RID: 10837 RVA: 0x000DFFE8 File Offset: 0x000DE1E8
	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000DFFFC File Offset: 0x000DE1FC
	private void SetSaleText()
	{
		if (this.Discount > 0 && this.sale != null)
		{
			this.sale.text = string.Format("{0}\n{1}%", LocalizationStore.Key_0419, this.Discount);
		}
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000E004C File Offset: 0x000DE24C
	private void OnEnable()
	{
		foreach (UIButton uibutton in base.GetComponentsInChildren<UIButton>(true))
		{
			uibutton.isEnabled = TrainingController.TrainingCompleted;
		}
		this.SetSaleText();
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000E008C File Offset: 0x000DE28C
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000E00A0 File Offset: 0x000DE2A0
	private void HandleLocalizationChanged()
	{
		this.SetSaleText();
	}

	// Token: 0x04001F55 RID: 8021
	public UIButton button;

	// Token: 0x04001F56 RID: 8022
	public UITexture stickerTexture;

	// Token: 0x04001F57 RID: 8023
	public GameObject stickersLabel;

	// Token: 0x04001F58 RID: 8024
	public UISprite currencyImage;

	// Token: 0x04001F59 RID: 8025
	public string tg;

	// Token: 0x04001F5A RID: 8026
	public UITexture icon;

	// Token: 0x04001F5B RID: 8027
	public UILabel topSeller;

	// Token: 0x04001F5C RID: 8028
	public UILabel newItem;

	// Token: 0x04001F5D RID: 8029
	public UILabel sale;

	// Token: 0x04001F5E RID: 8030
	public UILabel coins;

	// Token: 0x04001F5F RID: 8031
	public Texture unpressed;

	// Token: 0x04001F60 RID: 8032
	public Texture pressed;
}
