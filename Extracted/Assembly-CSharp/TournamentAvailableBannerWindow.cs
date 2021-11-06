using System;
using Rilisoft;

// Token: 0x02000575 RID: 1397
public class TournamentAvailableBannerWindow : BannerWindow
{
	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x0600306E RID: 12398 RVA: 0x000FC820 File Offset: 0x000FAA20
	// (set) Token: 0x0600306F RID: 12399 RVA: 0x000FC82C File Offset: 0x000FAA2C
	public static bool CanShow
	{
		get
		{
			return TournamentAvailableBannerWindow._canShow.Value;
		}
		set
		{
			TournamentAvailableBannerWindow._canShow.Value = value;
		}
	}

	// Token: 0x06003070 RID: 12400 RVA: 0x000FC83C File Offset: 0x000FAA3C
	public void HideButtonAction()
	{
		TournamentAvailableBannerWindow.CanShow = false;
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.TournamentAvailable);
			BannerWindowController.SharedController.HideBannerWindow();
		}
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowLeaderboards(new LeaderboardsView.State?(LeaderboardsView.State.Tournament));
		}
	}

	// Token: 0x04002397 RID: 9111
	private static readonly PrefsBoolCachedProperty _canShow = new PrefsBoolCachedProperty("TournamentAwailableBannerWindow_needShow");
}
