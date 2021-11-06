using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x02000740 RID: 1856
internal sealed class RewardedLikeButton : MonoBehaviour
{
	// Token: 0x0600414D RID: 16717 RVA: 0x0015C034 File Offset: 0x0015A234
	private IEnumerator Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.RefreshRewardedLikeButton));
		this.RefreshRewardedLikeButton();
		while (MainMenuController.sharedController == null)
		{
			yield return null;
		}
		this.Refresh();
		if (FB.IsLoggedIn)
		{
			yield break;
		}
		while (!FB.IsLoggedIn)
		{
			yield return new WaitForSeconds(1f);
		}
		this.Refresh();
		yield break;
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x0015C050 File Offset: 0x0015A250
	private void OnEnable()
	{
		this.Refresh();
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x0015C058 File Offset: 0x0015A258
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.RefreshRewardedLikeButton));
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x0015C06C File Offset: 0x0015A26C
	public void OnClick()
	{
		Application.OpenURL("https://www.facebook.com/PixelGun3DOfficial");
		try
		{
			TutorialQuestManager.Instance.AddFulfilledQuest("likeFacebook");
			QuestMediator.NotifySocialInteraction("likeFacebook");
			if (Storager.getInt("RewardForLikeGained", true) <= 0)
			{
				Storager.setInt("RewardForLikeGained", 1, true);
				int @int = Storager.getInt("GemsCurrency", false);
				Storager.setInt("GemsCurrency", @int + 10, false);
				AnalyticsFacade.CurrencyAccrual(10, "GemsCurrency", AnalyticsConstants.AccrualType.Earned);
				AnalyticsFacade.SendCustomEvent("Virality", new Dictionary<string, object>
				{
					{
						"Like Facebook Page",
						"Likes"
					}
				});
				CoinsMessage.FireCoinsAddedEvent(true, 2);
			}
		}
		finally
		{
			this.Refresh();
		}
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x0015C138 File Offset: 0x0015A338
	private void RefreshRewardedLikeButton()
	{
		if (this.rewardedLikeCaption == null)
		{
			Debug.LogError("rewardedLikeCaption == null");
			return;
		}
		try
		{
			string format = LocalizationStore.Get("Key_1653");
			this.rewardedLikeCaption.text = string.Format(format, 10);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x0015C1B4 File Offset: 0x0015A3B4
	internal void Refresh()
	{
		if (!FacebookController.FacebookSupported)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (this.rewardedLikeButton == null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (Storager.hasKey("RewardForLikeGained") && Storager.getInt("RewardForLikeGained", true) > 0)
		{
			UnityEngine.Object.Destroy(this.rewardedLikeButton.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (!FB.IsLoggedIn)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (!Storager.hasKey(Defs.IsFacebookLoginRewardaGained) || Storager.getInt(Defs.IsFacebookLoginRewardaGained, true) == 0)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (ExpController.LobbyLevel <= 1)
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		if (MainMenuController.SavedShwonLobbyLevelIsLessThanActual())
		{
			this.rewardedLikeButton.gameObject.SetActive(false);
			return;
		}
		this.RefreshRewardedLikeButton();
		this.rewardedLikeButton.gameObject.SetActive(true);
	}

	// Token: 0x04002FBA RID: 12218
	private const int RewardGemsCount = 10;

	// Token: 0x04002FBB RID: 12219
	internal const string RewardKey = "RewardForLikeGained";

	// Token: 0x04002FBC RID: 12220
	public UIButton rewardedLikeButton;

	// Token: 0x04002FBD RID: 12221
	public UILabel rewardedLikeCaption;
}
