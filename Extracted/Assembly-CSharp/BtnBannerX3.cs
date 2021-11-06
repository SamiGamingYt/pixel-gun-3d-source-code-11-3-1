using System;

// Token: 0x020005A1 RID: 1441
public class BtnBannerX3 : ButtonBannerBase
{
	// Token: 0x060031F1 RID: 12785 RVA: 0x001037D0 File Offset: 0x001019D0
	public override bool BannerIsActive()
	{
		return PromoActionsManager.sharedManager.IsEventX3Active;
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x001037DC File Offset: 0x001019DC
	public override void OnClickButton()
	{
		MainMenuController.sharedController.ShowBankWindow();
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x001037E8 File Offset: 0x001019E8
	public override void OnHide()
	{
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x001037EC File Offset: 0x001019EC
	public override void OnShow()
	{
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x001037F0 File Offset: 0x001019F0
	public override void OnChangeLocalize()
	{
	}
}
