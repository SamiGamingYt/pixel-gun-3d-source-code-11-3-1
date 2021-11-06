using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000065 RID: 101
internal sealed class ChooseBox : MonoBehaviour
{
	// Token: 0x060002C2 RID: 706 RVA: 0x00017F1C File Offset: 0x0001611C
	private void LoadBoxPreviews()
	{
		for (int i = 0; i < LevelBox.campaignBoxes.Count; i++)
		{
			Texture item = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme)) as Texture;
			this.boxPreviews.Add(item);
			Texture item2 = Resources.Load(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme + "_closed")) as Texture;
			this.closedBoxPreviews.Add(item2);
		}
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x00017FB0 File Offset: 0x000161B0
	private void UnloadBoxPreviews()
	{
		this.boxPreviews.Clear();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x00017FC4 File Offset: 0x000161C4
	private void Start()
	{
		ChooseBox.instance = this;
		if (this.nguiController.startButton != null)
		{
			ButtonHandler component = this.nguiController.startButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleStartClicked;
			}
		}
		if (this.nguiController.backButton != null)
		{
			ButtonHandler component2 = this.nguiController.backButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleBackClicked;
			}
		}
		StoreKitEventListener.State.Mode = "Campaign";
		StoreKitEventListener.State.Parameters.Clear();
		CampaignProgressSynchronizer.Instance.Sync();
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			ProgressSynchronizer.Instance.AuthenticateAndSynchronize(delegate
			{
				WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
			}, true);
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			ProgressSynchronizer.Instance.SynchronizeIosProgress();
			WeaponManager.sharedManager.Reset(0);
			AchievementSynchronizer.Instance.Sync();
		}
		this.pointMap = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
		int num = Math.Min(LevelBox.campaignBoxes.Count, this.gridTransform.childCount);
		for (int i = 0; i < num; i++)
		{
			bool flag = this.CalculateStarsLeftToOpenTheBox(i) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(i);
			Texture mainTexture = (!flag) ? (Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme + "_closed")) ?? Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme))) : Resources.Load<Texture>(ResPath.Combine("Boxes", LevelBox.campaignBoxes[i].PreviewNAme));
			Transform child = this.gridTransform.GetChild(i);
			child.GetComponent<UITexture>().mainTexture = mainTexture;
			Transform transform = child.FindChild("NeedMoreStarsLabel");
			if (transform != null)
			{
				if (!flag && i < LevelBox.campaignBoxes.Count - 1)
				{
					transform.gameObject.SetActive(true);
					int num2 = this.CalculateStarsLeftToOpenTheBox(i);
					string text = (this.IsCompliteAllLevelsToOpenTheBox(i) || num2 <= 0) ? ((num2 <= 0) ? LocalizationStore.Get("Key_1366") : string.Format(LocalizationStore.Get("Key_1367"), num2)) : string.Format(LocalizationStore.Get("Key_0241"), num2);
					transform.GetComponent<UILabel>().text = text;
				}
				else
				{
					transform.gameObject.SetActive(false);
				}
			}
			else
			{
				Debug.LogWarning("Could not find “NeedMoreStarsLabel”.");
			}
			Transform transform2 = child.FindChild("CaptionLabel");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(flag || i == LevelBox.campaignBoxes.Count - 1);
			}
			else
			{
				Debug.LogWarning("Could not find “CaptionLabel”.");
			}
		}
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x00018324 File Offset: 0x00016524
	private void HandleStartClicked(object sender, EventArgs e)
	{
		if (this.nguiController.selectIndexMap == 0 || (this.CalculateStarsLeftToOpenTheBox(this.nguiController.selectIndexMap) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(this.nguiController.selectIndexMap)))
		{
			this.StartNBox(this.nguiController.selectIndexMap);
		}
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00018380 File Offset: 0x00016580
	public void StartNameBox(string _nameBox)
	{
		if (_nameBox.Equals("Box_1"))
		{
			this.StartNBox(0);
			return;
		}
		if (_nameBox.Equals("Box_2"))
		{
			if (this.CalculateStarsLeftToOpenTheBox(1) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(1))
			{
				this.StartNBox(1);
			}
			return;
		}
		if (_nameBox.Equals("Box_3"))
		{
			if (this.CalculateStarsLeftToOpenTheBox(2) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(2))
			{
				this.StartNBox(2);
			}
			return;
		}
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00018408 File Offset: 0x00016608
	public void StartNBox(int n)
	{
		ButtonClickSound.Instance.PlayClick();
		CurrentCampaignGame.boXName = LevelBox.campaignBoxes[n].name;
		MenuBackgroundMusic.keepPlaying = true;
		LoadConnectScene.textureToShow = null;
		LoadConnectScene.sceneToLoad = "ChooseLevel";
		LoadConnectScene.noteToShow = null;
		SceneManager.LoadScene(Defs.PromSceneName);
	}

	// Token: 0x060002C8 RID: 712 RVA: 0x0001845C File Offset: 0x0001665C
	private void HandleBackClicked(object sender, EventArgs e)
	{
		this._escapePressed = true;
	}

	// Token: 0x060002C9 RID: 713 RVA: 0x00018468 File Offset: 0x00016668
	private void OnDestroy()
	{
		ChooseBox.instance = null;
		CampaignProgress.SaveCampaignProgress();
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		this.UnloadBoxPreviews();
	}

	// Token: 0x060002CA RID: 714 RVA: 0x0001848C File Offset: 0x0001668C
	private void Update()
	{
		if (this._escapePressed)
		{
			ButtonClickSound.Instance.PlayClick();
			Resources.UnloadUnusedAssets();
			MenuBackgroundMusic.keepPlaying = true;
			LoadConnectScene.textureToShow = null;
			LoadConnectScene.noteToShow = null;
			LoadConnectScene.sceneToLoad = Defs.MainMenuScene;
			SceneManager.LoadScene(Defs.PromSceneName);
			this._escapePressed = false;
		}
		if (this.nguiController.startButton != null)
		{
			this.nguiController.startButton.gameObject.SetActive(this.nguiController.selectIndexMap == 0 || (this.CalculateStarsLeftToOpenTheBox(this.nguiController.selectIndexMap) <= 0 && this.IsCompliteAllLevelsToOpenTheBox(this.nguiController.selectIndexMap)));
		}
	}

	// Token: 0x060002CB RID: 715 RVA: 0x0001854C File Offset: 0x0001674C
	private void OnEnable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(delegate
		{
			this._escapePressed = true;
		}, "Choose Box");
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00018588 File Offset: 0x00016788
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060002CD RID: 717 RVA: 0x000185A8 File Offset: 0x000167A8
	private bool IsCompliteAllLevelsToOpenTheBox(int boxIndex)
	{
		if (boxIndex == 0)
		{
			return true;
		}
		bool result = false;
		LevelBox levelBox = LevelBox.campaignBoxes[boxIndex - 1];
		Dictionary<string, int> dictionary;
		if (CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out dictionary))
		{
			if (boxIndex == 1 && dictionary.Count >= 9)
			{
				result = true;
			}
			if (boxIndex == 2 && dictionary.Count >= 6)
			{
				result = true;
			}
			if (boxIndex == 3 && dictionary.Count >= 5)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00018628 File Offset: 0x00016828
	private int CalculateStarsLeftToOpenTheBox(int boxIndex)
	{
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			throw new ArgumentOutOfRangeException("boxIndex");
		}
		int num = 0;
		for (int i = 0; i < boxIndex; i++)
		{
			LevelBox levelBox = LevelBox.campaignBoxes[i];
			Dictionary<string, int> dictionary;
			if (CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out dictionary))
			{
				foreach (CampaignLevel campaignLevel in levelBox.levels)
				{
					int num2 = 0;
					if (dictionary.TryGetValue(campaignLevel.sceneName, out num2))
					{
						num += num2;
					}
				}
			}
		}
		int starsToOpen = LevelBox.campaignBoxes[boxIndex].starsToOpen;
		return starsToOpen - num;
	}

	// Token: 0x040002F0 RID: 752
	public static ChooseBox instance;

	// Token: 0x040002F1 RID: 753
	private Vector2 pressPoint;

	// Token: 0x040002F2 RID: 754
	private Vector2 startPoint;

	// Token: 0x040002F3 RID: 755
	private Vector2 pointMap;

	// Token: 0x040002F4 RID: 756
	private bool isVozvratMap;

	// Token: 0x040002F5 RID: 757
	private Vector2 sizeMap = new Vector2(823f, 736f);

	// Token: 0x040002F6 RID: 758
	private bool isMoveMap;

	// Token: 0x040002F7 RID: 759
	private bool isSetMap;

	// Token: 0x040002F8 RID: 760
	private List<Texture> boxPreviews = new List<Texture>();

	// Token: 0x040002F9 RID: 761
	private List<Texture> closedBoxPreviews = new List<Texture>();

	// Token: 0x040002FA RID: 762
	public ChooseBoxNGUIController nguiController;

	// Token: 0x040002FB RID: 763
	public Transform gridTransform;

	// Token: 0x040002FC RID: 764
	private bool _escapePressed;

	// Token: 0x040002FD RID: 765
	private IDisposable _backSubscription;
}
