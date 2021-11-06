using System;

// Token: 0x0200059E RID: 1438
public class BtnBannerPereliv : ButtonBannerBase
{
	// Token: 0x060031DF RID: 12767 RVA: 0x0010373C File Offset: 0x0010193C
	public override bool BannerIsActive()
	{
		return MainMenuController.trafficForwardActive;
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x00103744 File Offset: 0x00101944
	public override void OnClickButton()
	{
		MainMenuController.sharedController.HandleTrafficForwardingClicked();
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x00103750 File Offset: 0x00101950
	public override void OnHide()
	{
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x00103754 File Offset: 0x00101954
	public override void OnShow()
	{
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x00103758 File Offset: 0x00101958
	public override void OnChangeLocalize()
	{
	}
}
