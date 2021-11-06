using System;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000577 RID: 1399
public class TournamentWinnerBannerWindow : BannerWindow
{
	// Token: 0x17000850 RID: 2128
	// (get) Token: 0x06003078 RID: 12408 RVA: 0x000FC928 File Offset: 0x000FAB28
	// (set) Token: 0x06003079 RID: 12409 RVA: 0x000FC934 File Offset: 0x000FAB34
	public static bool CanShow
	{
		get
		{
			return TournamentWinnerBannerWindow._canShow.Value;
		}
		set
		{
			TournamentWinnerBannerWindow._canShow.Value = value;
		}
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x000FC944 File Offset: 0x000FAB44
	public override void Show()
	{
		base.Show();
		if (BalanceController.competitionAward == null)
		{
			return;
		}
		if (BalanceController.competitionAward.Currency == "Coins")
		{
			this._RewardCoinsTextGroup.Do(delegate(TextGroup t)
			{
				t.Text = BalanceController.competitionAward.Price.ToString();
			});
			this._coinsIconObj.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(true);
			});
			this._gemsIconObj.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(false);
			});
		}
		else
		{
			this._RewardGemsTextGroup.Do(delegate(TextGroup t)
			{
				t.Text = BalanceController.competitionAward.Price.ToString();
			});
			this._coinsIconObj.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(false);
			});
			this._gemsIconObj.Do(delegate(GameObject o)
			{
				o.SetActiveSafe(true);
			});
		}
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x000FCA78 File Offset: 0x000FAC78
	public void HideButtonAction()
	{
		if (BalanceController.competitionAward != null)
		{
			if (BalanceController.competitionAward.Currency == "Coins")
			{
				BankController.AddCoins(BalanceController.competitionAward.Price, true, AnalyticsConstants.AccrualType.Earned);
			}
			else
			{
				BankController.AddGems(BalanceController.competitionAward.Price, true, AnalyticsConstants.AccrualType.Earned);
			}
		}
		base.StartCoroutine(BankController.WaitForIndicationGems(true));
		TournamentWinnerBannerWindow.CanShow = false;
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.TournamentWunner);
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	// Token: 0x04002399 RID: 9113
	private static readonly PrefsBoolCachedProperty _canShow = new PrefsBoolCachedProperty("TournamentWinnerBannerWindow_needShow");

	// Token: 0x0400239A RID: 9114
	[SerializeField]
	private GameObject _coinsIconObj;

	// Token: 0x0400239B RID: 9115
	[SerializeField]
	private GameObject _gemsIconObj;

	// Token: 0x0400239C RID: 9116
	[SerializeField]
	private TextGroup _RewardCoinsTextGroup;

	// Token: 0x0400239D RID: 9117
	[SerializeField]
	private TextGroup _RewardGemsTextGroup;
}
