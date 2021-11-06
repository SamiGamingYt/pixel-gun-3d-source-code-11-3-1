using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200071A RID: 1818
public class DailyQuestItem : MonoBehaviour
{
	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06003F67 RID: 16231 RVA: 0x00153954 File Offset: 0x00151B54
	private AccumulativeQuestBase Quest
	{
		get
		{
			return this._questInfo.Map((QuestInfo qi) => qi.Quest as AccumulativeQuestBase);
		}
	}

	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06003F68 RID: 16232 RVA: 0x0015398C File Offset: 0x00151B8C
	public bool CanSkip
	{
		get
		{
			return this._questInfo != null && this._questInfo.CanSkip;
		}
	}

	// Token: 0x06003F69 RID: 16233 RVA: 0x001539A8 File Offset: 0x00151BA8
	public void Refresh()
	{
		if (this.itemInLobby)
		{
			this.FillData(-1);
			return;
		}
		if (this._questInfo == null)
		{
			return;
		}
		QuestBase quest = this._questInfo.Quest;
		if (quest != null)
		{
			this.FillData(this.Quest.Slot);
		}
	}

	// Token: 0x06003F6A RID: 16234 RVA: 0x001539F8 File Offset: 0x00151BF8
	private void OnEnable()
	{
		if (this.questSkipFrame != null)
		{
			this.questSkipFrame.SetActive(false);
		}
		if (this.rewardAnim != null)
		{
			this.rewardAnim.enabled = false;
		}
		if (this.skipAnimPosition != null)
		{
			this.skipAnimPosition.enabled = false;
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = this.skipAnimPosition.from;
		}
		if (this.itemInLobby)
		{
			this.FillData(-1);
		}
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x00153A94 File Offset: 0x00151C94
	private int GetQuestMode(AccumulativeQuestBase quest)
	{
		ModeAccumulativeQuest modeAccumulativeQuest = quest as ModeAccumulativeQuest;
		if (modeAccumulativeQuest != null)
		{
			return (int)modeAccumulativeQuest.Mode;
		}
		return 0;
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x00153AB8 File Offset: 0x00151CB8
	private string GetQuestMap(AccumulativeQuestBase quest)
	{
		MapAccumulativeQuest mapAccumulativeQuest = quest as MapAccumulativeQuest;
		if (mapAccumulativeQuest != null)
		{
			return mapAccumulativeQuest.Map;
		}
		return string.Empty;
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x00153AE0 File Offset: 0x00151CE0
	public void FillData(int slot)
	{
		if (!TrainingController.TrainingCompleted)
		{
			base.gameObject.SetActive(false);
			return;
		}
		QuestProgress questProgress = QuestSystem.Instance.QuestProgress;
		if (questProgress == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this._questInfo = ((slot != -1) ? questProgress.GetActiveQuestInfoBySlot(slot + 1) : questProgress.GetRandomQuestInfo());
		if (this.Quest == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (this.Quest.SetActive())
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Total",
					"Get"
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams);
			string value = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[]
			{
				this.Quest.Id,
				QuestConstants.GetDifficultyKey(this.Quest.Difficulty)
			});
			Dictionary<string, object> eventParams2 = new Dictionary<string, object>
			{
				{
					"Get",
					value
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams2);
		}
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		this.questDescription.text = QuestConstants.GetAccumulativeQuestDescriptionByType(this.Quest);
		this.progressLabel.text = string.Format("{0}/{1}", this.Quest.CurrentCount, this.Quest.RequiredCount);
		this.progressBar.fillAmount = (float)this.Quest.CalculateProgress();
		if (Defs.IsDeveloperBuild)
		{
			UISprite component = base.GetComponent<UISprite>();
			if (component != null)
			{
				this.oldQuest.SetActive(this.Quest.Day < questProgress.Day);
			}
		}
		if (this.itemInLobby)
		{
			if (this._questInfo.Quest != null)
			{
				bool flag = this._questInfo.Quest.CalculateProgress() >= 1m && !this._questInfo.Quest.Rewarded;
				if (this.questsButtonLabel != null && this.rewardButtonLabel != null)
				{
					this.questsButtonLabel.gameObject.SetActive(!flag);
					this.rewardButtonLabel.gameObject.SetActive(flag);
					if (flag)
					{
						DailyQuestsButton component2 = this.questsButtonLabel.parent.GetComponent<DailyQuestsButton>();
						if (component2 != null)
						{
							component2.SetUI();
						}
					}
				}
				if (this.modeColor != null)
				{
					this.modeColor.color = QuestImage.Instance.GetColor(this._questInfo.Quest);
				}
				if (this.modeIcon != null)
				{
					this.modeIcon.spriteName = QuestImage.Instance.GetSpriteName(this._questInfo.Quest);
				}
			}
		}
		else
		{
			if (this.Quest.CalculateProgress() >= 1m)
			{
				this.getRewardButton.SetActive(!this.Quest.Rewarded);
				this.completedObject.SetActive(this.Quest.Rewarded);
				this.toBattleButton.SetActive(false);
				this.questInProgress.SetActive(false);
				this.questSkipFrame.SetActive(false);
			}
			else
			{
				this.getRewardButton.SetActive(false);
				this.completedObject.SetActive(false);
				this.toBattleButton.SetActive(false);
				this.questInProgress.SetActive(true);
				this.questSkipFrame.SetActive(false);
			}
			if (SceneManager.GetActiveScene().name != Defs.MainMenuScene)
			{
				this.toBattleButton.SetActive(false);
			}
			if (this.modeColor != null)
			{
				this.modeColor.color = QuestImage.Instance.GetColor(this._questInfo.Quest);
			}
			if (this.modeIcon != null)
			{
				this.modeIcon.spriteName = QuestImage.Instance.GetSpriteName(this._questInfo.Quest);
			}
		}
		this.coinsCount.text = this.Quest.Reward.Coins.ToString();
		this.gemsCount.text = this.Quest.Reward.Gems.ToString();
		this.expCount.text = this.Quest.Reward.Experience.ToString();
		this.coinsObject.SetActive(this.Quest.Reward.Coins > 0);
		this.gemObject.SetActive(this.Quest.Reward.Gems > 0);
		this.expObject.SetActive(this.Quest.Reward.Experience > 0 && (ExperienceController.sharedController.currentLevel != 31 || (this.Quest.Reward.Coins == 0 && this.Quest.Reward.Gems == 0)));
		this.awardTable.repositionNow = true;
		if (this.skipButton != null)
		{
			this.skipButton.gameObject.SetActive(this._questInfo.CanSkip);
		}
	}

	// Token: 0x06003F6E RID: 16238 RVA: 0x00154088 File Offset: 0x00152288
	public void OnGetRewardButtonClick()
	{
		if (QuestSystem.Instance.QuestProgress == null)
		{
			return;
		}
		QuestSystem.Instance.QuestProgress.FilterFulfilledTutorialQuests();
		if (this.Quest.CalculateProgress() >= 1m && !this.Quest.Rewarded)
		{
			this.Quest.SetRewarded();
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Total",
					"Rewarded"
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams);
			string value = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[]
			{
				this.Quest.Id,
				QuestConstants.GetDifficultyKey(this.Quest.Difficulty)
			});
			Dictionary<string, object> eventParams2 = new Dictionary<string, object>
			{
				{
					"Quests",
					value
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams2);
			QuestSystem.Instance.QuestProgress.TryRemoveTutorialQuest(this.Quest.Id);
			QuestSystem.Instance.SaveQuestProgressIfDirty();
			this.getRewardButton.SetActive(false);
			this.completedObject.SetActive(true);
			this.rewardAnim.enabled = true;
			Reward reward = this.Quest.Reward;
			if (reward.Coins > 0)
			{
				BankController.AddCoins(reward.Coins, true, AnalyticsConstants.AccrualType.Earned);
			}
			if (reward.Gems > 0)
			{
				BankController.AddGems(reward.Gems, true, AnalyticsConstants.AccrualType.Earned);
			}
			if (reward.Experience > 0)
			{
				ExperienceController.sharedController.addExperience(reward.Experience);
			}
		}
		DailyQuestsBannerController.Instance.UpdateItems();
		MainMenuQuestSystemListener.Refresh();
	}

	// Token: 0x06003F6F RID: 16239 RVA: 0x00154224 File Offset: 0x00152424
	public void OnSkipInMainMenuClick()
	{
		this.HandleSkip();
	}

	// Token: 0x06003F70 RID: 16240 RVA: 0x0015422C File Offset: 0x0015242C
	public void OnSkipInGameClick()
	{
		this.HandleSkip();
	}

	// Token: 0x06003F71 RID: 16241 RVA: 0x00154234 File Offset: 0x00152434
	private void HandleSkip()
	{
		if (this._questInfo == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("QuestInfo is null.");
			}
			return;
		}
		if (this._questInfo.CanSkip)
		{
			this._questInfo.Skip();
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Total",
					"Skipped"
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams);
			QuestBase quest = this._questInfo.Quest;
			string value = string.Format(CultureInfo.InvariantCulture, "{0} / {1}", new object[]
			{
				quest.Id,
				QuestConstants.GetDifficultyKey(quest.Difficulty)
			});
			Dictionary<string, object> eventParams2 = new Dictionary<string, object>
			{
				{
					"Skip",
					value
				}
			};
			AnalyticsFacade.SendCustomEvent("Daily Quests", eventParams2);
			DailyQuestsBannerController.Instance.Do(delegate(DailyQuestsBannerController c)
			{
				c.UpdateItems();
			});
			if (this.questSkipFrame != null)
			{
				this.questSkipFrame.SetActive(true);
			}
			if (this.skipAnimPosition != null)
			{
				this.skipAnimPosition.enabled = true;
			}
		}
		else if (Defs.IsDeveloperBuild)
		{
			Debug.LogError("Cannot skip!");
		}
		MainMenuQuestSystemListener.Refresh();
	}

	// Token: 0x06003F72 RID: 16242 RVA: 0x00154380 File Offset: 0x00152580
	public void OnViewAllButtonClick()
	{
		BannerWindowController sharedController = BannerWindowController.SharedController;
		if (sharedController == null)
		{
			return;
		}
		sharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003F73 RID: 16243 RVA: 0x001543B4 File Offset: 0x001525B4
	private void OpenConnectScene(int mode)
	{
		PlayerPrefs.SetInt("RegimMulty", mode);
		ConnectSceneNGUIController.directedFromQuests = true;
		MainMenuController.sharedController.OnClickMultiplyerButton();
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x001543D4 File Offset: 0x001525D4
	public void OnToBattleButtonClick()
	{
		if (this.Quest == null)
		{
			Debug.LogError("Quest is null.");
			return;
		}
		string id = this.Quest.Id;
		switch (id)
		{
		case "winInMode":
		case "killInMode":
			this.OpenConnectScene(this.GetQuestMode(this.Quest));
			break;
		case "killFlagCarriers":
		case "captureFlags":
			this.OpenConnectScene(4);
			break;
		case "capturePoints":
			this.OpenConnectScene(5);
			break;
		case "killNpcWithWeapon":
			this.OpenConnectScene(1);
			break;
		case "winInMap":
			ConnectSceneNGUIController.selectedMap = this.GetQuestMap(this.Quest);
			MainMenuController.sharedController.OnClickMultiplyerButton();
			break;
		case "killWithWeapon":
		case "killViaHeadshot":
		case "killWithGrenade":
		case "revenge":
		case "breakSeries":
		case "makeSeries":
			MainMenuController.sharedController.OnClickMultiplyerButton();
			break;
		case "surviveWavesInArena":
			MainMenuController.sharedController.StartSurvivalButton();
			break;
		case "killInCampaign":
			MainMenuController.sharedController.StartCampaingButton();
			break;
		}
	}

	// Token: 0x04002EA1 RID: 11937
	public UISprite progressBar;

	// Token: 0x04002EA2 RID: 11938
	public GameObject coinsObject;

	// Token: 0x04002EA3 RID: 11939
	public GameObject gemObject;

	// Token: 0x04002EA4 RID: 11940
	public GameObject expObject;

	// Token: 0x04002EA5 RID: 11941
	public UILabel coinsCount;

	// Token: 0x04002EA6 RID: 11942
	public UILabel gemsCount;

	// Token: 0x04002EA7 RID: 11943
	public UILabel expCount;

	// Token: 0x04002EA8 RID: 11944
	public UILabel progressLabel;

	// Token: 0x04002EA9 RID: 11945
	public UILabel questDescription;

	// Token: 0x04002EAA RID: 11946
	public UITable awardTable;

	// Token: 0x04002EAB RID: 11947
	public GameObject getRewardButton;

	// Token: 0x04002EAC RID: 11948
	public GameObject toBattleButton;

	// Token: 0x04002EAD RID: 11949
	public GameObject completedObject;

	// Token: 0x04002EAE RID: 11950
	public GameObject questInProgress;

	// Token: 0x04002EAF RID: 11951
	public UILabel viewAllLabel;

	// Token: 0x04002EB0 RID: 11952
	public bool itemInLobby;

	// Token: 0x04002EB1 RID: 11953
	public UIButton skipButton;

	// Token: 0x04002EB2 RID: 11954
	public UILabel questsButtonLabel;

	// Token: 0x04002EB3 RID: 11955
	public UILabel rewardButtonLabel;

	// Token: 0x04002EB4 RID: 11956
	[Header("Animations for quest frame")]
	public GameObject questSkipFrame;

	// Token: 0x04002EB5 RID: 11957
	public GameObject oldQuest;

	// Token: 0x04002EB6 RID: 11958
	public TweenPosition skipAnimPosition;

	// Token: 0x04002EB7 RID: 11959
	public TweenScale rewardAnim;

	// Token: 0x04002EB8 RID: 11960
	public UISprite modeColor;

	// Token: 0x04002EB9 RID: 11961
	public UISprite modeIcon;

	// Token: 0x04002EBA RID: 11962
	private QuestInfo _questInfo;
}
