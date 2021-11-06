using System;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public class TrophiesPenaltyBannerWindow : BannerWindow
{
	// Token: 0x06003083 RID: 12419 RVA: 0x000FCB90 File Offset: 0x000FAD90
	public override void Show()
	{
		base.Show();
		int @int = PlayerPrefs.GetInt("leave_from_duel_penalty");
		this._penalty.text = @int.ToString();
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000FCBC0 File Offset: 0x000FADC0
	public void HideButtonAction()
	{
		PlayerPrefs.SetInt("leave_from_duel_penalty", 0);
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.ResetStateBannerShowed(BannerWindowType.TrophiesPenalty);
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	// Token: 0x040023A4 RID: 9124
	public const string KEY_LEAVE_FROM_DUEL = "leave_from_duel_penalty";

	// Token: 0x040023A5 RID: 9125
	[SerializeField]
	private UILabel _penalty;
}
