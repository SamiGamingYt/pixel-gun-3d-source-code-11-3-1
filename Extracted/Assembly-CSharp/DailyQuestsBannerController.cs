using System;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200071B RID: 1819
internal sealed class DailyQuestsBannerController : BannerWindow
{
	// Token: 0x06003F78 RID: 16248 RVA: 0x001545C4 File Offset: 0x001527C4
	private void Awake()
	{
		QuestSystem.Instance.Updated += this.HandleQuestSystemUpdate;
		DailyQuestsBannerController.Instance = this;
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x001545E4 File Offset: 0x001527E4
	private void OnDestroy()
	{
		QuestSystem.Instance.Updated -= this.HandleQuestSystemUpdate;
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x001545FC File Offset: 0x001527FC
	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Refreshing after quest system update.");
		}
		this.UpdateItems();
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00154618 File Offset: 0x00152818
	public new void Show()
	{
		if (this.inBannerSystem && BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.RegisterWindow(this, BannerWindowType.DailyQuests);
			BannerWindowController.SharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		}
		else
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x0015466C File Offset: 0x0015286C
	public new void Hide()
	{
		if (this.inBannerSystem)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x001546A0 File Offset: 0x001528A0
	private void OnEnable()
	{
		ExpController.LevelUpShown += this.HandleLevelUpShown;
		this.UpdateItems();
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.Hide), "Quest Banner");
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x001546FC File Offset: 0x001528FC
	private void OnDisable()
	{
		ExpController.LevelUpShown -= this.HandleLevelUpShown;
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x00154738 File Offset: 0x00152938
	private void Update()
	{
		if (this.blockingFon != null)
		{
			this.blockingFon.depth = ((!(ExpController.Instance != null) || !ExpController.Instance.WaitingForLevelUpView) ? 100 : 100000);
		}
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x0015478C File Offset: 0x0015298C
	public void UpdateItems()
	{
		QuestProgress questProgress = QuestSystem.Instance.QuestProgress;
		bool flag;
		if (TrainingController.TrainingCompleted && questProgress != null)
		{
			flag = (questProgress.GetActiveQuests().Values.Count((QuestBase q) => q != null && !q.Rewarded) > 0);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3 = false;
		for (int i = 0; i < this.DailyQuests.Length; i++)
		{
			DailyQuestItem dailyQuestItem = this.DailyQuests[i];
			if (flag2)
			{
				if (!dailyQuestItem.gameObject.activeSelf)
				{
					dailyQuestItem.gameObject.SetActive(true);
				}
				dailyQuestItem.FillData(i);
			}
			else if (dailyQuestItem.gameObject.activeSelf)
			{
				dailyQuestItem.gameObject.SetActive(false);
			}
			flag3 = (flag3 || dailyQuestItem.CanSkip);
		}
		if (this.skipHint != null)
		{
			this.skipHint.gameObject.SetActive(flag3);
		}
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		if (flag2)
		{
			this.noQuestsLabel.SetActive(false);
			this.questsTable.Reposition();
		}
		else
		{
			this.noQuestsLabel.SetActive(true);
		}
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x001548C8 File Offset: 0x00152AC8
	private void HandleLevelUpShown()
	{
		if (this.inBannerSystem)
		{
			if (BannerWindowController.SharedController != null)
			{
				BannerWindowController.SharedController.HideBannerWindowNoShowNext();
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04002EBE RID: 11966
	public UISprite blockingFon;

	// Token: 0x04002EBF RID: 11967
	public DailyQuestItem[] DailyQuests;

	// Token: 0x04002EC0 RID: 11968
	public GameObject noQuestsLabel;

	// Token: 0x04002EC1 RID: 11969
	public UITable questsTable;

	// Token: 0x04002EC2 RID: 11970
	public UILabel skipHint;

	// Token: 0x04002EC3 RID: 11971
	public bool inBannerSystem = true;

	// Token: 0x04002EC4 RID: 11972
	public static DailyQuestsBannerController Instance;

	// Token: 0x04002EC5 RID: 11973
	private IDisposable _backSubscription;
}
