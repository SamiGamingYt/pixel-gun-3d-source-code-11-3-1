using System;
using Rilisoft;

// Token: 0x02000576 RID: 1398
public class TournamentLooserBannerWindow : BannerWindow
{
	// Token: 0x1700084F RID: 2127
	// (get) Token: 0x06003073 RID: 12403 RVA: 0x000FC8B4 File Offset: 0x000FAAB4
	// (set) Token: 0x06003074 RID: 12404 RVA: 0x000FC8C0 File Offset: 0x000FAAC0
	public static bool CanShow
	{
		get
		{
			return TournamentLooserBannerWindow._canShow.Value;
		}
		set
		{
			TournamentLooserBannerWindow._canShow.Value = value;
		}
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000FC8D0 File Offset: 0x000FAAD0
	public void HideButtonAction()
	{
		TournamentLooserBannerWindow.CanShow = false;
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.TournamentLooser);
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	// Token: 0x04002398 RID: 9112
	private static readonly PrefsBoolCachedProperty _canShow = new PrefsBoolCachedProperty("TournamentLooserBannerWindow_needShow");
}
