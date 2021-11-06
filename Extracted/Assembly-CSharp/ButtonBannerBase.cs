using System;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
public class ButtonBannerBase : MonoBehaviour
{
	// Token: 0x060031F7 RID: 12791 RVA: 0x001037FC File Offset: 0x001019FC
	public virtual void OnShow()
	{
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x00103800 File Offset: 0x00101A00
	public virtual void OnHide()
	{
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x00103804 File Offset: 0x00101A04
	public virtual bool BannerIsActive()
	{
		return false;
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x00103808 File Offset: 0x00101A08
	public virtual void OnClickButton()
	{
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x0010380C File Offset: 0x00101A0C
	public virtual void OnChangeLocalize()
	{
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x00103810 File Offset: 0x00101A10
	public virtual void OnUpdateParameter()
	{
	}

	// Token: 0x060031FD RID: 12797 RVA: 0x00103814 File Offset: 0x00101A14
	private void OnClick()
	{
		this.OnClickButton();
	}

	// Token: 0x060031FE RID: 12798 RVA: 0x0010381C File Offset: 0x00101A1C
	private void OnPress(bool IsDown)
	{
		if (IsDown)
		{
			ButtonBannerHUD.instance.StopTimerNextBanner();
		}
		else
		{
			ButtonBannerHUD.instance.ResetTimerNextBanner();
		}
	}

	// Token: 0x040024CC RID: 9420
	[HideInInspector]
	public int indexBut;

	// Token: 0x040024CD RID: 9421
	public int priorityShow;
}
