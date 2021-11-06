using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x020005F6 RID: 1526
public sealed class ExpController : MonoBehaviour
{
	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06003436 RID: 13366 RVA: 0x0010E334 File Offset: 0x0010C534
	// (remove) Token: 0x06003437 RID: 13367 RVA: 0x0010E34C File Offset: 0x0010C54C
	public static event Action LevelUpShown;

	// Token: 0x06003438 RID: 13368 RVA: 0x0010E364 File Offset: 0x0010C564
	public static int OurTierForAnyPlace()
	{
		return (!(ExpController.Instance != null)) ? ExpController.GetOurTier() : ExpController.Instance.OurTier;
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x06003439 RID: 13369 RVA: 0x0010E398 File Offset: 0x0010C598
	public static ExpController Instance
	{
		get
		{
			return ExpController._instance;
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x0600343A RID: 13370 RVA: 0x0010E3A0 File Offset: 0x0010C5A0
	public bool InAddingState
	{
		get
		{
			return this._inAddingState;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x0600343B RID: 13371 RVA: 0x0010E3A8 File Offset: 0x0010C5A8
	// (set) Token: 0x0600343C RID: 13372 RVA: 0x0010E3F8 File Offset: 0x0010C5F8
	public bool InterfaceEnabled
	{
		get
		{
			return this.experienceView != null && this.experienceView.interfaceHolder != null && this.experienceView.interfaceHolder.gameObject.activeInHierarchy;
		}
		set
		{
			this.SetInterfaceEnabled(value);
		}
	}

	// Token: 0x170008AD RID: 2221
	// (get) Token: 0x0600343D RID: 13373 RVA: 0x0010E404 File Offset: 0x0010C604
	public static int LobbyLevel
	{
		get
		{
			return 3;
		}
	}

	// Token: 0x170008AE RID: 2222
	// (get) Token: 0x0600343E RID: 13374 RVA: 0x0010E414 File Offset: 0x0010C614
	// (set) Token: 0x0600343F RID: 13375 RVA: 0x0010E41C File Offset: 0x0010C61C
	public bool IsLevelUpShown { get; private set; }

	// Token: 0x06003440 RID: 13376 RVA: 0x0010E428 File Offset: 0x0010C628
	private void SetInterfaceEnabled(bool value)
	{
		if (value && this.experienceView != null)
		{
			bool flag = Application.loadedLevelName != Defs.MainMenuScene || ShopNGUIController.GuiActive || (MainMenuController.sharedController != null && MainMenuController.sharedController.singleModePanel != null && MainMenuController.sharedController.singleModePanel.activeInHierarchy) || (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled);
			if (this.experienceView.rankIndicatorContainer.activeSelf != flag)
			{
				this.experienceView.rankIndicatorContainer.SetActive(flag);
			}
		}
		if (this.InterfaceEnabled == value)
		{
			return;
		}
		if (this.experienceView != null && this.experienceView.interfaceHolder != null)
		{
			if (!value)
			{
				this.experienceView.StopAnimation();
			}
			if (ExperienceController.sharedController != null)
			{
				this.Rank = ExperienceController.sharedController.currentLevel;
				this.Experience = ExperienceController.sharedController.CurrentExperience;
			}
			this.experienceView.interfaceHolder.gameObject.SetActive(value);
			if (value && this.experienceView.experienceCamera != null)
			{
				AudioListener component = this.experienceView.experienceCamera.GetComponent<AudioListener>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
		}
	}

	// Token: 0x06003441 RID: 13377 RVA: 0x0010E5B8 File Offset: 0x0010C7B8
	public void HandleContinueButton(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Application.loadedLevelName)) ? 0 : Defs.filterMaps[Application.loadedLevelName]);
		}
		if (!this.starterBannerShowed && ExperienceController.sharedController.currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			this.starterBannerShowed = true;
			this.experienceView.ToBonus(BalanceController.startCapitalGems, BalanceController.startCapitalCoins);
		}
		else
		{
			this.starterBannerShowed = false;
			ExpController.HideTierPanel(tierPanel);
		}
	}

	// Token: 0x06003442 RID: 13378 RVA: 0x0010E65C File Offset: 0x0010C85C
	public void HandleShopButtonFromTierPanel(GameObject tierPanel)
	{
		base.StartCoroutine(this.HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	// Token: 0x06003443 RID: 13379 RVA: 0x0010E66C File Offset: 0x0010C86C
	public void HandleNewAvailableItem(GameObject tierPanel, NewAvailableItemInShop itemInfo)
	{
		if (Defs.isHunger)
		{
			return;
		}
		if (ExpController.CurrentFilterMap() != 0)
		{
			int[] target = new int[0];
			bool flag = true;
			if (itemInfo != null && itemInfo._tag != null)
			{
				flag = (ItemDb.GetByTag(itemInfo._tag) != null);
				if (flag)
				{
					target = ItemDb.GetItemFilterMap(itemInfo._tag);
				}
			}
			if (flag && !target.Contains(ExpController.CurrentFilterMap()))
			{
				this.HandleShopButtonFromTierPanel(tierPanel);
				return;
			}
		}
		if (ShopNGUIController.sharedShop == null)
		{
			Debug.LogWarning("ShopNGUIController.sharedShop == null");
		}
		else
		{
			if (!(itemInfo == null))
			{
				string text = itemInfo._tag ?? string.Empty;
				Debug.Log(string.Concat(new object[]
				{
					"Available item:   ",
					text,
					"    ",
					itemInfo.category
				}));
				base.StartCoroutine(this.HandleShopButtonFromNewAvailableItemCoroutine(tierPanel, text, itemInfo.category));
				return;
			}
			Debug.LogWarning("itemInfo == null");
		}
		base.StartCoroutine(this.HandleShopButtonFromTierPanelCoroutine(tierPanel));
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x0010E794 File Offset: 0x0010C994
	public static int CurrentFilterMap()
	{
		return (!Defs.filterMaps.ContainsKey(Application.loadedLevelName)) ? 0 : Defs.filterMaps[Application.loadedLevelName];
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x0010E7C0 File Offset: 0x0010C9C0
	private IEnumerator HandleShopButtonFromTierPanelCoroutine(GameObject tierPanel)
	{
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset(ExpController.CurrentFilterMap());
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		ExpController.HideTierPanel(tierPanel);
		yield break;
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x0010E7E4 File Offset: 0x0010C9E4
	private IEnumerator HandleShopButtonFromNewAvailableItemCoroutine(GameObject tierPanel, string itemTag, ShopNGUIController.CategoryNames category)
	{
		if (ShopNGUIController.IsWeaponCategory(category))
		{
			itemTag = (WeaponManager.FirstUnboughtOrForOurTier(itemTag) ?? itemTag);
		}
		ShopNGUIController.sharedShop.SetItemToShow(new ShopNGUIController.ShopItem(itemTag, category));
		ShopNGUIController.sharedShop.CategoryToChoose = category;
		if (WeaponManager.sharedManager != null)
		{
			WeaponManager.sharedManager.Reset((!Defs.filterMaps.ContainsKey(Application.loadedLevelName)) ? 0 : Defs.filterMaps[Application.loadedLevelName]);
		}
		yield return null;
		ShopNGUIController.sharedShop.resumeAction = null;
		ShopNGUIController.GuiActive = true;
		yield return null;
		ExpController.HideTierPanel(tierPanel);
		ShopNGUIController.sharedShop.AdjustCategoryGridCells();
		yield break;
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x0010E824 File Offset: 0x0010CA24
	public void UpdateLabels()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x0010E864 File Offset: 0x0010CA64
	public void Refresh()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	// Token: 0x170008AF RID: 2223
	// (set) Token: 0x06003449 RID: 13385 RVA: 0x0010E8A4 File Offset: 0x0010CAA4
	public int Rank
	{
		set
		{
			if (this.experienceView == null)
			{
				return;
			}
			int rankSprite = Mathf.Clamp(value, 1, 31);
			this.experienceView.RankSprite = rankSprite;
		}
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x0010E8DC File Offset: 0x0010CADC
	public static int TierForLevel(int lev)
	{
		if (lev < ExpController.LevelsForTiers[1])
		{
			return 0;
		}
		if (lev < ExpController.LevelsForTiers[2])
		{
			return 1;
		}
		if (lev < ExpController.LevelsForTiers[3])
		{
			return 2;
		}
		if (lev < ExpController.LevelsForTiers[4])
		{
			return 3;
		}
		if (lev < ExpController.LevelsForTiers[5])
		{
			return 4;
		}
		return 5;
	}

	// Token: 0x170008B0 RID: 2224
	// (get) Token: 0x0600344B RID: 13387 RVA: 0x0010E938 File Offset: 0x0010CB38
	// (set) Token: 0x0600344C RID: 13388 RVA: 0x0010E940 File Offset: 0x0010CB40
	public bool WaitingForLevelUpView { get; private set; }

	// Token: 0x0600344D RID: 13389 RVA: 0x0010E94C File Offset: 0x0010CB4C
	public static int GetOurTier()
	{
		int currentLevelWithUpdateCorrection = ExperienceController.GetCurrentLevelWithUpdateCorrection();
		return ExpController.TierForLevel(currentLevelWithUpdateCorrection);
	}

	// Token: 0x170008B1 RID: 2225
	// (get) Token: 0x0600344E RID: 13390 RVA: 0x0010E968 File Offset: 0x0010CB68
	public int OurTier
	{
		get
		{
			if (ExperienceController.sharedController != null)
			{
				int currentLevel = ExperienceController.sharedController.currentLevel;
				return ExpController.TierForLevel(currentLevel);
			}
			return 0;
		}
	}

	// Token: 0x0600344F RID: 13391 RVA: 0x0010E998 File Offset: 0x0010CB98
	public void AddExperience(int oldLevel, int oldExperience, int addend, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
	{
		if (this.experienceView == null)
		{
			return;
		}
		if (ExperienceController.sharedController == null)
		{
			return;
		}
		int num = oldExperience + addend;
		int num2 = ExperienceController.MaxExpLevels[oldLevel];
		if (num < num2)
		{
			float percentage = ExpController.GetPercentage(num);
			this.experienceView.CurrentProgress = percentage;
			this.experienceView.StartBlinkingWithNewProgress();
			this.experienceView.WaitAndUpdateOldProgress(exp2);
			this.experienceView.ExperienceLabel = ExpController.FormatExperienceLabel(num, num2);
			if (this.experienceView.currentProgress != null && !this.experienceView.currentProgress.gameObject.activeInHierarchy)
			{
				this.experienceView.OldProgress = percentage;
			}
		}
		else
		{
			float num3 = 1f;
			this.experienceView.CurrentProgress = num3;
			AudioClip sound = levelup;
			if (tierup != null && Array.IndexOf<int>(ExpController.LevelsForTiers, oldLevel + 1) > 0)
			{
				sound = tierup;
			}
			if (oldLevel < 30)
			{
				this.experienceView.StartBlinkingWithNewProgress();
				int num4 = oldLevel + 1;
				int newExperience = num - num2;
				base.StartCoroutine(this.WaitAndUpdateExperience(num4, newExperience, ExperienceController.MaxExpLevels[num4], true, sound));
			}
			else if (oldLevel == 30)
			{
				this.experienceView.StartBlinkingWithNewProgress();
				int num5 = oldLevel + 1;
				int newExperience2 = ExperienceController.MaxExpLevels[num5];
				base.StartCoroutine(this.WaitAndUpdateExperience(num5, newExperience2, ExperienceController.MaxExpLevels[num5], true, sound));
			}
			else
			{
				if (ExperienceController.sharedController.currentLevel == 31)
				{
					num3 = 1f;
				}
				this.experienceView.OldProgress = num3;
				this.experienceView.StartBlinkingWithNewProgress();
				int num6 = 31;
				int newExperience3 = ExperienceController.MaxExpLevels[num6];
				base.StartCoroutine(this.WaitAndUpdateExperience(num6, newExperience3, ExperienceController.MaxExpLevels[num6], false, exp2));
			}
		}
		base.StartCoroutine(this.SetAddingState());
	}

	// Token: 0x06003450 RID: 13392 RVA: 0x0010EB80 File Offset: 0x0010CD80
	public bool IsRenderedWithCamera(Camera c)
	{
		return this.experienceView != null && this.experienceView.experienceCamera != null && this.experienceView.experienceCamera == c;
	}

	// Token: 0x170008B2 RID: 2226
	// (set) Token: 0x06003451 RID: 13393 RVA: 0x0010EBC0 File Offset: 0x0010CDC0
	private int Experience
	{
		set
		{
			if (this.experienceView == null)
			{
				return;
			}
			if (ExperienceController.sharedController == null)
			{
				return;
			}
			int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
			int num2 = Mathf.Clamp(value, 0, num);
			this.experienceView.ExperienceLabel = ExpController.FormatExperienceLabel(num2, num);
			float num3 = (float)num2 / (float)num;
			if (ExperienceController.sharedController.currentLevel == 31)
			{
				num3 = 1f;
			}
			this.experienceView.CurrentProgress = num3;
			this.experienceView.OldProgress = num3;
		}
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x0010EC54 File Offset: 0x0010CE54
	private string SubstituteTempGunIfReplaced(string constTg)
	{
		if (constTg == null)
		{
			return null;
		}
		KeyValuePair<string, string> keyValuePair = WeaponManager.replaceConstWithTemp.Find((KeyValuePair<string, string> kvp) => kvp.Key.Equals(constTg));
		if (keyValuePair.Key == null || keyValuePair.Value == null)
		{
			return constTg;
		}
		if (!TempItemsController.GunsMappingFromTempToConst.ContainsKey(keyValuePair.Value))
		{
			return keyValuePair.Value;
		}
		return constTg;
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x0010ECD8 File Offset: 0x0010CED8
	private IEnumerator WaitAndUpdateExperience(int newRank, int newExperience, int newBound, bool showLevelUpPanel, AudioClip sound)
	{
		this.experienceView.RankSprite = newRank;
		this.experienceView.ExperienceLabel = ExpController.FormatExperienceLabel(newExperience, newBound);
		this.WaitingForLevelUpView = showLevelUpPanel;
		int tier = Array.BinarySearch<int>(ExpController.LevelsForTiers, ExperienceController.sharedController.currentLevel);
		bool isTierLevelup = 0 <= tier && tier < ExpController.LevelsForTiers.Length;
		if (isTierLevelup)
		{
			if (PromoActionsManager.sharedManager != null)
			{
				try
				{
					List<string> newUnlockedItems = WeaponManager.GetNewWeaponsForTier(tier).Concat(GadgetsInfo.GetNewGadgetsForTier(tier)).ToList<string>();
					PromoActionsManager.sharedManager.ReplaceUnlockedItemsWith(newUnlockedItems);
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Debug.LogErrorFormat("Exception in WaitAndUpdateExperience ReplaceUnlockedItemsWith: {0}", new object[]
					{
						ex
					});
				}
			}
			else
			{
				Debug.LogErrorFormat("ShopNguiController.Update: PromoActionsManager.sharedManager == null", new object[0]);
			}
		}
		yield return new WaitForSeconds(1.2f);
		this.Experience = newExperience;
		if (showLevelUpPanel && ExperienceController.sharedController != null)
		{
			List<string> itemsToShow = new List<string>();
			if (isTierLevelup)
			{
				switch (tier)
				{
				case 1:
					itemsToShow = new List<string>
					{
						"Armor_Steel_1",
						this.SubstituteTempGunIfReplaced(WeaponTags.Antihero_Rifle_1_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.frank_sheepone_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.AcidCannon_Tag)
					};
					break;
				case 2:
					itemsToShow = new List<string>
					{
						"Armor_Royal_1",
						this.SubstituteTempGunIfReplaced(WeaponTags.DragonGun_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.charge_rifle_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.loud_piggy_Tag)
					};
					break;
				case 3:
					itemsToShow = new List<string>
					{
						"Armor_Almaz_1",
						this.SubstituteTempGunIfReplaced(WeaponTags.Dark_Matter_Generator_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.autoaim_bazooka_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.Photon_Pistol_Tag)
					};
					break;
				case 4:
					itemsToShow = new List<string>
					{
						this.SubstituteTempGunIfReplaced(WeaponTags.RailRevolverBuy_3_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.RayMinigun_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.Autoaim_RocketlauncherBuy_3_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.Impulse_Sniper_RifleBuy_3_Tag)
					};
					break;
				case 5:
					itemsToShow = new List<string>
					{
						this.SubstituteTempGunIfReplaced(WeaponTags.PX_3000_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.StormHammer_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.Sunrise_Tag),
						this.SubstituteTempGunIfReplaced(WeaponTags.Bastion_Tag)
					};
					break;
				}
				this.experienceView.LevelUpPanelOptions.ShowTierView = true;
			}
			else
			{
				this.experienceView.LevelUpPanelOptions.ShowTierView = false;
			}
			this.experienceView.LevelUpPanelOptions.ShareButtonEnabled = true;
			int oldRank = Math.Max(0, newRank - 1);
			int coinsReward = ExperienceController.addCoinsFromLevels[oldRank];
			int gemsReward = ExperienceController.addGemsFromLevels[oldRank];
			if (NetworkStartTableNGUIController.sharedController != null)
			{
				this._sameSceneIndicator = true;
				while (NetworkStartTableNGUIController.sharedController != null && NetworkStartTableNGUIController.sharedController.isRewardShow)
				{
					yield return null;
				}
				if (!this._sameSceneIndicator || NetworkStartTableNGUIController.sharedController == null)
				{
					this.WaitingForLevelUpView = false;
					yield break;
				}
			}
			else if (Application.loadedLevelName == "LevelComplete")
			{
				this._sameSceneIndicator = true;
				while (this._sameSceneIndicator && LevelCompleteScript.IsInterfaceBusy)
				{
					yield return null;
				}
				if (!this._sameSceneIndicator)
				{
					this.WaitingForLevelUpView = false;
					yield break;
				}
			}
			this.experienceView.LevelUpPanelOptions.NewItems = itemsToShow;
			this.experienceView.LevelUpPanelOptions.CurrentRank = newRank;
			this.experienceView.LevelUpPanelOptions.CoinsReward = coinsReward;
			this.experienceView.LevelUpPanelOptions.GemsReward = gemsReward;
			this.experienceView.ShowLevelUpPanel();
		}
		this.WaitingForLevelUpView = false;
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(sound);
		}
		yield break;
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x0010ED40 File Offset: 0x0010CF40
	private IEnumerator SetAddingState()
	{
		this._inAddingState = true;
		yield return new WaitForSeconds(1.2f);
		this._inAddingState = false;
		yield break;
	}

	// Token: 0x06003455 RID: 13397 RVA: 0x0010ED5C File Offset: 0x0010CF5C
	private static float GetPercentage(int experience)
	{
		if (ExperienceController.sharedController == null)
		{
			return 0f;
		}
		int num = ExperienceController.MaxExpLevels[ExperienceController.sharedController.currentLevel];
		int num2 = Mathf.Clamp(experience, 0, num);
		return (float)num2 / (float)num;
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x0010EDA0 File Offset: 0x0010CFA0
	public static float progressExpInPer()
	{
		return ExpController.GetPercentage(ExperienceController.sharedController.CurrentExperience);
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x0010EDB4 File Offset: 0x0010CFB4
	private static string FormatExperienceLabel(int xp, int bound)
	{
		string text = LocalizationStore.Get("Key_0928");
		string text2 = string.Format("{0} {1}/{2}", LocalizationStore.Get("Key_0204"), xp, bound);
		return (xp != bound && ExperienceController.sharedController.currentLevel != 31) ? text2 : text;
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x0010EE0C File Offset: 0x0010D00C
	public static string ExpToString()
	{
		int currentLevel = ExperienceController.sharedController.currentLevel;
		int currentExperience = ExperienceController.sharedController.CurrentExperience;
		return ExpController.FormatExperienceLabel(currentExperience, ExperienceController.MaxExpLevels[currentLevel]);
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x0010EE3C File Offset: 0x0010D03C
	private static string FormatLevelLabel(int level)
	{
		return string.Format("{0} {1}", LocalizationStore.Key_0226, level);
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x0010EE54 File Offset: 0x0010D054
	private void OnEnable()
	{
		if (ExperienceController.sharedController != null)
		{
			this.Rank = ExperienceController.sharedController.currentLevel;
			this.Experience = ExperienceController.sharedController.CurrentExperience;
		}
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x0010EE94 File Offset: 0x0010D094
	private void Awake()
	{
		if (!SceneLoader.ActiveSceneName.EndsWith("Workbench"))
		{
			this.InterfaceEnabled = false;
		}
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLabels));
		Singleton<SceneLoader>.Instance.OnSceneLoading += delegate(SceneLoadInfo sli)
		{
			if (this.experienceView != null)
			{
				LevelUpWithOffers currentVisiblePanel = this.experienceView.CurrentVisiblePanel;
				if (currentVisiblePanel != null)
				{
					ExpController.HideTierPanel(currentVisiblePanel.gameObject);
				}
			}
			this.IsLevelUpShown = false;
		};
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x0010EEE4 File Offset: 0x0010D0E4
	private void Start()
	{
		if (ExpController._instance != null)
		{
			Debug.LogWarning("ExpController is not null while starting.");
		}
		ExpController._instance = this;
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x0010EF14 File Offset: 0x0010D114
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.UpdateLabels));
		ExpController._instance = null;
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x0010EF30 File Offset: 0x0010D130
	private void Update()
	{
		if (ExperienceController.sharedController != null)
		{
			this.SetInterfaceEnabled(ExperienceController.sharedController.isShowRanks);
		}
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x0010EF60 File Offset: 0x0010D160
	public static void ShowTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(true);
			if (ExpController.Instance != null)
			{
				ExpController.Instance.IsLevelUpShown = true;
				Action levelUpShown = ExpController.LevelUpShown;
				if (levelUpShown != null)
				{
					levelUpShown();
				}
			}
			MainMenuController.SetInputEnabled(false);
			LevelCompleteScript.SetInputEnabled(false);
		}
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x0010EFBC File Offset: 0x0010D1BC
	public static void HideTierPanel(GameObject tierPanel)
	{
		if (tierPanel != null)
		{
			tierPanel.SetActive(false);
			if (ExpController.Instance != null)
			{
				ExpController.Instance.IsLevelUpShown = false;
			}
			MainMenuController.SetInputEnabled(true);
			LevelCompleteScript.SetInputEnabled(true);
		}
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x0010F004 File Offset: 0x0010D204
	private void OnLevelWasLoaded(int index)
	{
		this._sameSceneIndicator = false;
	}

	// Token: 0x0400266D RID: 9837
	public const int MaxLobbyLevel = 3;

	// Token: 0x0400266E RID: 9838
	public ExpView experienceView;

	// Token: 0x0400266F RID: 9839
	private bool starterBannerShowed;

	// Token: 0x04002670 RID: 9840
	public static readonly int[] LevelsForTiers = new int[]
	{
		1,
		7,
		12,
		17,
		22,
		27
	};

	// Token: 0x04002671 RID: 9841
	private static ExpController _instance;

	// Token: 0x04002672 RID: 9842
	private bool _sameSceneIndicator;

	// Token: 0x04002673 RID: 9843
	private bool _inAddingState;
}
