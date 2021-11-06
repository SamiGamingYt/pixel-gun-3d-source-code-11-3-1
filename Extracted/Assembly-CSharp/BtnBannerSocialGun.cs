using System;

// Token: 0x020005A0 RID: 1440
public class BtnBannerSocialGun : ButtonBannerBase
{
	// Token: 0x060031EB RID: 12779 RVA: 0x001037A4 File Offset: 0x001019A4
	public override bool BannerIsActive()
	{
		return FacebookController.sharedController.SocialGunEventActive;
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x001037B0 File Offset: 0x001019B0
	public override void OnClickButton()
	{
		MainMenuController.sharedController.OnSocialGunEventButtonClick();
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x001037BC File Offset: 0x001019BC
	public override void OnHide()
	{
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x001037C0 File Offset: 0x001019C0
	public override void OnShow()
	{
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x001037C4 File Offset: 0x001019C4
	public override void OnChangeLocalize()
	{
	}
}
