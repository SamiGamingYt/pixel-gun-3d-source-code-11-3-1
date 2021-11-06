using System;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

// Token: 0x02000719 RID: 1817
public class DailyQuestBannerHandler : MonoBehaviour
{
	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x06003F5F RID: 16223 RVA: 0x0015379C File Offset: 0x0015199C
	// (set) Token: 0x06003F60 RID: 16224 RVA: 0x001537A4 File Offset: 0x001519A4
	public static DailyQuestBannerHandler Instance { get; private set; }

	// Token: 0x06003F61 RID: 16225 RVA: 0x001537AC File Offset: 0x001519AC
	private void Awake()
	{
		DailyQuestBannerHandler.Instance = this;
		this._controller = new LazyObject<DailyQuestsBannerController>(this._prefab.ResourcePath, this._windowRoot);
		ExpController.LevelUpShown += this.HandleLevelUpShown;
		if (this.questsButton != null)
		{
			this.questsButton.OnClickAction += this.ShowUI;
		}
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x00153814 File Offset: 0x00151A14
	private void OnDetroy()
	{
		ExpController.LevelUpShown -= this.HandleLevelUpShown;
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x00153828 File Offset: 0x00151A28
	private void HandleLevelUpShown()
	{
		if (this._controller.ObjectIsLoaded)
		{
			this._controller.Value.Hide();
		}
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x00153858 File Offset: 0x00151A58
	public void ShowUI()
	{
		if (!this._controller.ObjectIsLoaded)
		{
			DailyQuestsBannerController value = this._controller.Value;
			int layer = this._windowRoot.layer;
			value.gameObject.layer = layer;
			foreach (GameObject gameObject in value.gameObject.Descendants())
			{
				gameObject.layer = layer;
			}
			this._controller.Value.inBannerSystem = this.inBannerSystem;
		}
		this._controller.Value.Show();
		if (this.questsButton != null)
		{
			this.questsButton.SetUI();
		}
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x00153938 File Offset: 0x00151B38
	public void HideUI()
	{
		this._controller.Value.Hide();
	}

	// Token: 0x04002E9B RID: 11931
	public bool inBannerSystem = true;

	// Token: 0x04002E9C RID: 11932
	[SerializeField]
	private DailyQuestsButton questsButton;

	// Token: 0x04002E9D RID: 11933
	[SerializeField]
	private GameObject _windowRoot;

	// Token: 0x04002E9E RID: 11934
	[SerializeField]
	private PrefabHandler _prefab;

	// Token: 0x04002E9F RID: 11935
	[SerializeField]
	private LazyObject<DailyQuestsBannerController> _controller;
}
