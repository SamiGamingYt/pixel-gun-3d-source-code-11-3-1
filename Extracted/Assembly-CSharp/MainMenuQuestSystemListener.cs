using System;
using UnityEngine;

// Token: 0x0200071D RID: 1821
internal sealed class MainMenuQuestSystemListener : MonoBehaviour
{
	// Token: 0x06003F90 RID: 16272 RVA: 0x00154D14 File Offset: 0x00152F14
	public static void Refresh()
	{
		if (MainMenuQuestSystemListener._instance == null)
		{
			return;
		}
		if (MainMenuQuestSystemListener._instance.dailyQuestItem == null)
		{
			return;
		}
		MainMenuQuestSystemListener._instance.dailyQuestItem.Refresh();
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x00154D58 File Offset: 0x00152F58
	private void Awake()
	{
		MainMenuQuestSystemListener._instance = this;
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00154D60 File Offset: 0x00152F60
	private void Start()
	{
		QuestSystem.Instance.Updated += this.HandleQuestSystemUpdate;
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00154D78 File Offset: 0x00152F78
	private void OnDestroy()
	{
		QuestSystem.Instance.Updated -= this.HandleQuestSystemUpdate;
	}

	// Token: 0x06003F94 RID: 16276 RVA: 0x00154D90 File Offset: 0x00152F90
	private void HandleQuestSystemUpdate(object sender, EventArgs e)
	{
		if (this.dailyQuestItem != null)
		{
			this.dailyQuestItem.Refresh();
		}
	}

	// Token: 0x04002ECC RID: 11980
	public DailyQuestItem dailyQuestItem;

	// Token: 0x04002ECD RID: 11981
	private static MainMenuQuestSystemListener _instance;
}
