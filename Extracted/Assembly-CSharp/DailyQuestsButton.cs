using System;
using System.Collections;
using Unity.Linq;
using UnityEngine;

// Token: 0x0200071C RID: 1820
internal sealed class DailyQuestsButton : MonoBehaviour
{
	// Token: 0x14000081 RID: 129
	// (add) Token: 0x06003F84 RID: 16260 RVA: 0x00154930 File Offset: 0x00152B30
	// (remove) Token: 0x06003F85 RID: 16261 RVA: 0x0015494C File Offset: 0x00152B4C
	public event Action OnClickAction;

	// Token: 0x06003F86 RID: 16262 RVA: 0x00154968 File Offset: 0x00152B68
	private void Awake()
	{
		if (this.inBannerSystem)
		{
			QuestSystem.Instance.Updated += this.HandleQuestSystemUpdate;
		}
		else if (Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
		if (QuestSystem.Instance.QuestProgress != null)
		{
			this.SetUI();
		}
	}

	// Token: 0x06003F87 RID: 16263 RVA: 0x001549C8 File Offset: 0x00152BC8
	private void OnEnable()
	{
		this.SetUI();
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.UpdateUI());
		}
	}

	// Token: 0x06003F88 RID: 16264 RVA: 0x001549F8 File Offset: 0x00152BF8
	private void OnDisable()
	{
		this.controller = null;
		if (this.dailyQuestsParent != null)
		{
			this.dailyQuestsParent.transform.DestroyChildren();
		}
	}

	// Token: 0x06003F89 RID: 16265 RVA: 0x00154A30 File Offset: 0x00152C30
	private void OnDestroy()
	{
		if (this.inBannerSystem)
		{
			QuestSystem.Instance.Updated -= this.HandleQuestSystemUpdate;
		}
	}

	// Token: 0x06003F8A RID: 16266 RVA: 0x00154A54 File Offset: 0x00152C54
	private void OnClick()
	{
		ButtonClickSound.TryPlayClick();
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (ShopNGUIController.GuiActive || ExperienceController.sharedController.isShowNextPlashka)
		{
			return;
		}
		if (this.OnClickAction != null)
		{
			this.OnClickAction();
			return;
		}
		if (this.inBannerSystem)
		{
			BannerWindowController sharedController = BannerWindowController.SharedController;
			if (sharedController == null)
			{
				return;
			}
			sharedController.ForceShowBanner(BannerWindowType.DailyQuests);
		}
		else
		{
			if (LoadingInAfterGame.isShowLoading)
			{
				return;
			}
			if ((this.controller == null || this.controller.gameObject == null) && this.dailyQuestsParent != null)
			{
				this.dailyQuestsParent.transform.DestroyChildren();
				DailyQuestsBannerController original = Resources.Load<DailyQuestsBannerController>("Windows/DailyQuests-Window");
				DailyQuestsBannerController dailyQuestsBannerController = UnityEngine.Object.Instantiate<DailyQuestsBannerController>(original);
				if (dailyQuestsBannerController != null)
				{
					dailyQuestsBannerController.transform.parent = this.dailyQuestsParent.transform;
					dailyQuestsBannerController.transform.localPosition = Vector3.zero;
					dailyQuestsBannerController.transform.localRotation = Quaternion.identity;
					dailyQuestsBannerController.transform.localScale = Vector3.one;
					int layer = base.gameObject.layer;
					dailyQuestsBannerController.gameObject.layer = layer;
					foreach (GameObject gameObject in dailyQuestsBannerController.gameObject.Descendants())
					{
						gameObject.layer = layer;
					}
					dailyQuestsBannerController.inBannerSystem = this.inBannerSystem;
				}
				this.controller = dailyQuestsBannerController;
			}
			if (this.controller != null)
			{
				this.controller.Show();
			}
		}
	}

	// Token: 0x06003F8B RID: 16267 RVA: 0x00154C64 File Offset: 0x00152E64
	private void CheckUnrewardedEvent(object sender, EventArgs e)
	{
		this.SetUI();
	}

	// Token: 0x06003F8C RID: 16268 RVA: 0x00154C6C File Offset: 0x00152E6C
	public void SetUI()
	{
		bool flag = QuestSystem.Instance.QuestProgress != null && QuestSystem.Instance.AnyActiveQuest;
		if (this.rewardIndicator != null && QuestSystem.Instance.QuestProgress != null)
		{
			bool active = QuestSystem.Instance.QuestProgress.HasUnrewaredAccumQuests();
			this.rewardIndicator.SetActive(active);
		}
	}

	// Token: 0x06003F8D RID: 16269 RVA: 0x00154CD4 File Offset: 0x00152ED4
	private IEnumerator UpdateUI()
	{
		WaitForSeconds delay = new WaitForSeconds(0.5f);
		for (;;)
		{
			yield return delay;
			this.SetUI();
		}
		yield break;
	}

	// Token: 0x06003F8E RID: 16270 RVA: 0x00154CF0 File Offset: 0x00152EF0
	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("Refreshing after quest system update.");
		}
		this.SetUI();
	}

	// Token: 0x04002EC7 RID: 11975
	public bool inBannerSystem = true;

	// Token: 0x04002EC8 RID: 11976
	[SerializeField]
	private DailyQuestsBannerController controller;

	// Token: 0x04002EC9 RID: 11977
	public GameObject rewardIndicator;

	// Token: 0x04002ECA RID: 11978
	[SerializeField]
	private GameObject dailyQuestsParent;
}
