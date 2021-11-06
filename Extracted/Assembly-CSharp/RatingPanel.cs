using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class RatingPanel : MonoBehaviour
{
	// Token: 0x06002B36 RID: 11062 RVA: 0x000E3E90 File Offset: 0x000E2090
	private void OnEnable()
	{
		this.UpdateInfo();
		RatingSystem.OnRatingUpdate += this.UpdateInfo;
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000E3EAC File Offset: 0x000E20AC
	private void UpdateInfo()
	{
		if (!TrainingController.TrainingCompleted)
		{
			this.leaguePanel.SetActive(false);
			return;
		}
		this.leaguePanel.SetActive(true);
		this.cup.spriteName = RatingSystem.instance.currentLeague.ToString() + " " + (3 - RatingSystem.instance.currentDivision).ToString();
		if (RatingSystem.instance.currentLeague != RatingSystem.RatingLeague.Adamant)
		{
			this.leagueLabel.text = LocalizationStore.Get(RatingSystem.leagueLocalizations[(int)RatingSystem.instance.currentLeague]) + " " + RatingSystem.divisionByIndex[RatingSystem.instance.currentDivision];
		}
		else
		{
			this.leagueLabel.text = LocalizationStore.Get(RatingSystem.leagueLocalizations[(int)RatingSystem.instance.currentLeague]);
		}
		this.ratingLabel.text = RatingSystem.instance.currentRating.ToString();
		if (this._btnOpenProfile != null)
		{
			this._btnOpenProfile.Clicked += this.OnBtnOpenProfileClicked;
		}
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000E3FD0 File Offset: 0x000E21D0
	private void OnDisable()
	{
		if (this._btnOpenProfile != null)
		{
			this._btnOpenProfile.Clicked -= this.OnBtnOpenProfileClicked;
		}
		RatingSystem.OnRatingUpdate -= this.UpdateInfo;
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x000E400C File Offset: 0x000E220C
	private void OnBtnOpenProfileClicked(object sender, EventArgs e)
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.GoToProfile();
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(ProfileStatTabType.Leagues);
		}
	}

	// Token: 0x0400204C RID: 8268
	public GameObject leaguePanel;

	// Token: 0x0400204D RID: 8269
	public UISprite cup;

	// Token: 0x0400204E RID: 8270
	public UILabel leagueLabel;

	// Token: 0x0400204F RID: 8271
	public UILabel ratingLabel;

	// Token: 0x04002050 RID: 8272
	[SerializeField]
	private ButtonHandler _btnOpenProfile;
}
