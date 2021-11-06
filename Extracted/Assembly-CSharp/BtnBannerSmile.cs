using System;

// Token: 0x0200059F RID: 1439
public class BtnBannerSmile : ButtonBannerBase
{
	// Token: 0x060031E5 RID: 12773 RVA: 0x00103770 File Offset: 0x00101970
	public override bool BannerIsActive()
	{
		return !StickersController.IsBuyAllPack();
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x0010377C File Offset: 0x0010197C
	public override void OnClickButton()
	{
		MainMenuController.sharedController.HandlePromoActionClicked(this.tagForClick);
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x00103790 File Offset: 0x00101990
	public override void OnHide()
	{
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x00103794 File Offset: 0x00101994
	public override void OnShow()
	{
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x00103798 File Offset: 0x00101998
	public override void OnChangeLocalize()
	{
	}

	// Token: 0x040024CB RID: 9419
	public string tagForClick = string.Empty;
}
