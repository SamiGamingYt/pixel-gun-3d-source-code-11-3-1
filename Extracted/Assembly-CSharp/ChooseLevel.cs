using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000068 RID: 104
internal sealed class ChooseLevel : MonoBehaviour
{
	// Token: 0x060002D8 RID: 728 RVA: 0x0001893C File Offset: 0x00016B3C
	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = false;
		WeaponManager.RefreshExpControllers();
		QuestSystem.Instance.SaveQuestProgressIfDirty();
		StoreKitEventListener.State.PurchaseKey = "In Map";
		StoreKitEventListener.State.Parameters.Clear();
		ChooseLevel.sharedChooseLevel = this;
		this._timeStarted = Time.realtimeSinceStartup;
		bool draggableLayout = false;
		this._boxIndex = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == CurrentCampaignGame.boXName);
		if (this._boxIndex == -1)
		{
			Debug.LogWarning("Box not found in list!");
			throw new InvalidOperationException("Box not found in list!");
		}
		bool flag = true;
		IList<ChooseLevel.LevelInfo> levelInfos;
		if (flag)
		{
			IList<ChooseLevel.LevelInfo> list = ChooseLevel.InitializeLevelInfos(draggableLayout);
			levelInfos = list;
		}
		else
		{
			levelInfos = ChooseLevel.InitializeLevelInfosWithTestData(draggableLayout);
		}
		this._levelInfos = levelInfos;
		this._gainedStarCount = ChooseLevel.InitializeGainStarCount(this._levelInfos);
		if (CurrentCampaignGame.boXName == "Real")
		{
			this._boxLevelButtons = this.boxOneLevelButtons;
			this.backgroundHolder.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName == "minecraft")
		{
			this._boxLevelButtons = this.boxTwoLevelButtons;
			this.backgroundHolder_2.SetActive(true);
		}
		else if (CurrentCampaignGame.boXName == "Crossed")
		{
			this._boxLevelButtons = this.boxThreeLevelButtons;
			this.backgroundHolder_3.SetActive(true);
			string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
			{
				'#'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					array[i] = string.Empty;
				}
			}
			this.BonusGun3Box.SetActive(array == null || !array.Contains(WeaponManager.BugGunWN));
		}
		else
		{
			Debug.LogWarning("Unknown box: " + CurrentCampaignGame.boXName);
		}
		this.InitializeLevelButtons();
		this.InitializeFixedDisplay();
		this.InitializeNextButton(this._levelInfos, this.nextButton);
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x00018B54 File Offset: 0x00016D54
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			if (this._shopInstance == null)
			{
				this.HandleBackButton(this, EventArgs.Empty);
			}
		}, "Choose Level");
	}

	// Token: 0x060002DA RID: 730 RVA: 0x00018B90 File Offset: 0x00016D90
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00018BB0 File Offset: 0x00016DB0
	private void InitializeNextButton(IList<ChooseLevel.LevelInfo> levels, ButtonHandler nextButton)
	{
		if (levels == null)
		{
			throw new ArgumentNullException("levels");
		}
		if (nextButton == null)
		{
			throw new ArgumentNullException("nextButton");
		}
		ChooseLevel.LevelInfo level = levels.LastOrDefault((ChooseLevel.LevelInfo l) => l.Enabled && l.StarGainedCount == 0);
		nextButton.gameObject.SetActive(level != null);
		if (level != null)
		{
			nextButton.Clicked += delegate(object sender, EventArgs e)
			{
				this.HandleLevelButton(level.Name);
			};
		}
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00018C50 File Offset: 0x00016E50
	private void InitializeFixedDisplay()
	{
		if (this.backButton != null)
		{
			this.backButton.GetComponent<ButtonHandler>().Clicked += this.HandleBackButton;
		}
		if (this.shopButton != null)
		{
			this.shopButton.GetComponent<ButtonHandler>().Clicked += this.HandleShopButton;
		}
		if (this.gainedStarCountLabel != null)
		{
			this.gainedStarCountLabel.GetComponent<UILabel>().text = this._gainedStarCount;
		}
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00018CE0 File Offset: 0x00016EE0
	private void HandleBackButton(object sender, EventArgs args)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (Time.time - this._timeWhenShopWasClosed < 1f)
		{
			return;
		}
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "CampaignChooseBox";
		LoadConnectScene.noteToShow = null;
		Application.LoadLevel(Defs.PromSceneName);
	}

	// Token: 0x060002DE RID: 734 RVA: 0x00018D3C File Offset: 0x00016F3C
	private void HandleShopButton(object sender, EventArgs args)
	{
		if (this._shopInstance == null)
		{
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				if (this.shopButtonSound != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(this.shopButtonSound);
				}
				this.panel.gameObject.SetActive(false);
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
		}
	}

	// Token: 0x060002DF RID: 735 RVA: 0x00018DD8 File Offset: 0x00016FD8
	private void HandleResumeFromShop()
	{
		this.panel.gameObject.SetActive(true);
		if (this._shopInstance != null)
		{
			ShopNGUIController.GuiActive = false;
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = false;
				ExpController.Instance.InterfaceEnabled = false;
			}
			this._shopInstance.resumeAction = delegate()
			{
			};
			this._shopInstance = null;
			this._timeWhenShopWasClosed = Time.time;
		}
	}

	// Token: 0x060002E0 RID: 736 RVA: 0x00018E80 File Offset: 0x00017080
	private void InitializeLevelButtons()
	{
		if (this.starEnabledPrototypes != null)
		{
			foreach (GameObject gameObject in this.starEnabledPrototypes)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
		if (this.starDisabledPrototypes != null)
		{
			foreach (GameObject gameObject2 in this.starDisabledPrototypes)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
		}
		if (this.boxContents == null)
		{
			throw new InvalidOperationException("boxContents == 0");
		}
		for (int num = 0; num != this.boxContents.Length; num++)
		{
			this.boxContents[num].SetActive(num == this._boxIndex);
		}
		if (this._boxLevelButtons == null)
		{
			throw new InvalidOperationException("Box level buttons are null.");
		}
		foreach (GameObject gameObject3 in this._boxLevelButtons)
		{
			if (gameObject3 != null)
			{
				UIButton component = gameObject3.GetComponent<UIButton>();
				if (component != null)
				{
					component.isEnabled = false;
				}
			}
		}
		int num2 = Math.Min(this._levelInfos.Count, this._boxLevelButtons.Length);
		for (int num3 = 0; num3 != num2; num3++)
		{
			ChooseLevel.LevelInfo levelInfo = this._levelInfos[num3];
			GameObject gameObject4 = this._boxLevelButtons[num3];
			gameObject4.transform.parent = gameObject4.transform.parent;
			gameObject4.GetComponent<UIButton>().isEnabled = levelInfo.Enabled;
			UISprite componentInChildren = gameObject4.GetComponentInChildren<UISprite>();
			if (componentInChildren == null)
			{
				Debug.LogWarning("Could not find background of level button.");
			}
			else
			{
				UILabel componentInChildren2 = componentInChildren.GetComponentInChildren<UILabel>();
				if (componentInChildren2 == null)
				{
					Debug.LogWarning("Could not find caption of level button.");
				}
				else
				{
					componentInChildren2.applyGradient = levelInfo.Enabled;
				}
			}
			gameObject4.AddComponent<ButtonHandler>();
			string levelName = levelInfo.Name;
			gameObject4.GetComponent<ButtonHandler>().Clicked += delegate(object sender, EventArgs e)
			{
				this.HandleLevelButton(levelName);
			};
			gameObject4.SetActive(true);
			for (int num4 = 0; num4 != this.starEnabledPrototypes.Length; num4++)
			{
				if (levelInfo.Enabled)
				{
					GameObject gameObject5 = this.starEnabledPrototypes[num4];
					if (!(gameObject5 == null))
					{
						GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(gameObject5);
						gameObject6.transform.parent = gameObject4.transform;
						gameObject6.GetComponent<UIToggle>().value = (num4 < levelInfo.StarGainedCount);
						gameObject6.transform.localPosition = gameObject5.transform.localPosition;
						gameObject6.transform.localScale = gameObject5.transform.localScale;
						gameObject6.SetActive(true);
					}
				}
			}
		}
		foreach (GameObject gameObject7 in this.starEnabledPrototypes)
		{
			if (gameObject7 != null)
			{
				UnityEngine.Object.Destroy(gameObject7);
			}
		}
		foreach (GameObject gameObject8 in this.starDisabledPrototypes)
		{
			if (gameObject8 != null)
			{
				UnityEngine.Object.Destroy(gameObject8);
			}
		}
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x00019208 File Offset: 0x00017408
	private void HandleLevelButton(string levelName)
	{
		if (this._shopInstance != null)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._timeStarted < 0.15f)
		{
			return;
		}
		CurrentCampaignGame.levelSceneName = levelName;
		WeaponManager.sharedManager.Reset(0);
		LevelArt.endOfBox = false;
		Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts) ? "CampaignLoading" : "LevelArt", LoadSceneMode.Single);
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x0001927C File Offset: 0x0001747C
	private static IList<ChooseLevel.LevelInfo> InitializeLevelInfosWithTestData(bool draggableLayout = false)
	{
		return new List<ChooseLevel.LevelInfo>
		{
			new ChooseLevel.LevelInfo
			{
				Enabled = true,
				Name = "Cementery",
				StarGainedCount = 1
			},
			new ChooseLevel.LevelInfo
			{
				Enabled = true,
				Name = "City",
				StarGainedCount = 3
			},
			new ChooseLevel.LevelInfo
			{
				Enabled = false,
				Name = "Hospital"
			}
		};
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x00019300 File Offset: 0x00017500
	private static IList<ChooseLevel.LevelInfo> InitializeLevelInfos(bool draggableLayout = false)
	{
		List<ChooseLevel.LevelInfo> list = new List<ChooseLevel.LevelInfo>();
		string boxName = CurrentCampaignGame.boXName;
		int num = LevelBox.campaignBoxes.FindIndex((LevelBox b) => b.name == boxName);
		if (num == -1)
		{
			Debug.LogWarning("Box not found in list!");
			return list;
		}
		LevelBox levelBox = LevelBox.campaignBoxes[num];
		List<CampaignLevel> levels = levelBox.levels;
		Dictionary<string, int> dictionary;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(boxName, out dictionary))
		{
			Debug.LogWarning("Box not found in dictionary: " + boxName);
			dictionary = new Dictionary<string, int>();
		}
		for (int num2 = 0; num2 != levels.Count; num2++)
		{
			string sceneName = levels[num2].sceneName;
			int starGainedCount = 0;
			dictionary.TryGetValue(sceneName, out starGainedCount);
			ChooseLevel.LevelInfo item = new ChooseLevel.LevelInfo
			{
				Enabled = (num2 <= dictionary.Count),
				Name = sceneName,
				StarGainedCount = starGainedCount
			};
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x00019410 File Offset: 0x00017610
	private static string InitializeGainStarCount(IList<ChooseLevel.LevelInfo> levelInfos)
	{
		int num = 3 * levelInfos.Count;
		int num2 = 0;
		foreach (ChooseLevel.LevelInfo levelInfo in levelInfos)
		{
			num2 += levelInfo.StarGainedCount;
		}
		return string.Format("{0}/{1}", num2, num);
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00019494 File Offset: 0x00017694
	public static string GetGainStarsString()
	{
		IList<ChooseLevel.LevelInfo> levelInfos = ChooseLevel.InitializeLevelInfos(false);
		return ChooseLevel.InitializeGainStarCount(levelInfos);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000194B0 File Offset: 0x000176B0
	private void OnDestroy()
	{
		ChooseLevel.sharedChooseLevel = null;
	}

	// Token: 0x04000308 RID: 776
	public GameObject BonusGun3Box;

	// Token: 0x04000309 RID: 777
	public GameObject panel;

	// Token: 0x0400030A RID: 778
	public GameObject[] starEnabledPrototypes;

	// Token: 0x0400030B RID: 779
	public GameObject[] starDisabledPrototypes;

	// Token: 0x0400030C RID: 780
	public GameObject gainedStarCountLabel;

	// Token: 0x0400030D RID: 781
	public GameObject backButton;

	// Token: 0x0400030E RID: 782
	public GameObject shopButton;

	// Token: 0x0400030F RID: 783
	public ButtonHandler nextButton;

	// Token: 0x04000310 RID: 784
	public GameObject[] boxOneLevelButtons;

	// Token: 0x04000311 RID: 785
	public GameObject[] boxTwoLevelButtons;

	// Token: 0x04000312 RID: 786
	public GameObject[] boxThreeLevelButtons;

	// Token: 0x04000313 RID: 787
	public AudioClip shopButtonSound;

	// Token: 0x04000314 RID: 788
	public GameObject backgroundHolder;

	// Token: 0x04000315 RID: 789
	public GameObject backgroundHolder_2;

	// Token: 0x04000316 RID: 790
	public GameObject backgroundHolder_3;

	// Token: 0x04000317 RID: 791
	public GameObject[] boxContents;

	// Token: 0x04000318 RID: 792
	public static ChooseLevel sharedChooseLevel;

	// Token: 0x04000319 RID: 793
	private float _timeStarted;

	// Token: 0x0400031A RID: 794
	private IDisposable _backSubscription;

	// Token: 0x0400031B RID: 795
	private int _boxIndex;

	// Token: 0x0400031C RID: 796
	private GameObject[] _boxLevelButtons;

	// Token: 0x0400031D RID: 797
	private string _gainedStarCount = string.Empty;

	// Token: 0x0400031E RID: 798
	private IList<ChooseLevel.LevelInfo> _levelInfos = new List<ChooseLevel.LevelInfo>();

	// Token: 0x0400031F RID: 799
	public ShopNGUIController _shopInstance;

	// Token: 0x04000320 RID: 800
	private float _timeWhenShopWasClosed;

	// Token: 0x02000069 RID: 105
	private sealed class LevelInfo
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00019514 File Offset: 0x00017714
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0001951C File Offset: 0x0001771C
		public bool Enabled { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00019528 File Offset: 0x00017728
		// (set) Token: 0x060002EF RID: 751 RVA: 0x00019530 File Offset: 0x00017730
		public string Name { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0001953C File Offset: 0x0001773C
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x00019544 File Offset: 0x00017744
		public int StarGainedCount { get; set; }
	}
}
