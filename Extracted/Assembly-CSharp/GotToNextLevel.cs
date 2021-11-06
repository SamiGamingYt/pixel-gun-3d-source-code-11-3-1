using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200028A RID: 650
internal sealed class GotToNextLevel : MonoBehaviour
{
	// Token: 0x060014C7 RID: 5319 RVA: 0x0005219C File Offset: 0x0005039C
	private void Awake()
	{
		this.OnPlayerAddedAct = delegate()
		{
			this._player = GameObject.FindGameObjectWithTag("Player");
			this._playerMoveC = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		};
		Initializer.PlayerAddedEvent += this.OnPlayerAddedAct;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x000521BC File Offset: 0x000503BC
	private void OnDestroy()
	{
		Initializer.PlayerAddedEvent -= this.OnPlayerAddedAct;
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x000521F0 File Offset: 0x000503F0
	private void Update()
	{
		if (this._player == null || this._playerMoveC == null)
		{
			return;
		}
		if (this.runLoading)
		{
			return;
		}
		if (Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) < 2.25f)
		{
			this.runLoading = true;
			this.GoToNextLevelInstance();
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
			}
		}
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x00052284 File Offset: 0x00050484
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x000522A4 File Offset: 0x000504A4
	private void HandleEscape()
	{
		if (Application.isEditor)
		{
			Debug.Log("Ignoring [Escape] after touching the portal.");
		}
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000522BC File Offset: 0x000504BC
	private void GoToNextLevelInstance()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
		}
		this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Touching the Portal");
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Portal, 0);
			base.Invoke("BannerTrainingCompleteInvoke", 2f);
			AutoFade.fadeKilled(2.05f, 0f, 0f, Color.white);
		}
		else
		{
			GotToNextLevel.GoToNextLevel();
		}
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x00052350 File Offset: 0x00050550
	private void GetRewardForTraining()
	{
		TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted;
		AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Rewards, 0);
		Storager.setInt("GrenadeID", 5, false);
		PlayerPrefs.Save();
		LevelCompleteLoader.sceneName = Defs.MainMenuScene;
		HashSet<RuntimePlatform> hashSet = new HashSet<RuntimePlatform>
		{
			RuntimePlatform.Android,
			RuntimePlatform.IPhonePlayer,
			RuntimePlatform.MetroPlayerX64
		};
		bool flag = hashSet.Contains(BuildSettings.BuildTargetPlatform);
		if (flag && !Storager.hasKey(Defs.GotCoinsForTraining))
		{
			if (!new Lazy<bool>(delegate()
			{
				string s;
				int num2;
				return Storager.UseSignedPreferences && Defs2.SignedPreferences.TryGetValue("Manterry", out s) && (!Defs2.SignedPreferences.Verify("Manterry") || !int.TryParse(s, out num2) || num2 % 2 == 1);
			}).Value)
			{
				int gemsForTraining = Defs.GemsForTraining;
				BankController.AddGems(gemsForTraining, false, AnalyticsConstants.AccrualType.Earned);
				int coinsForTraining = Defs.CoinsForTraining;
				BankController.AddCoins(coinsForTraining, false, AnalyticsConstants.AccrualType.Earned);
				AudioClip clip = Resources.Load<AudioClip>("coin_get");
				if (Defs.isSoundFX)
				{
					NGUITools.PlaySound(clip);
				}
				if (Storager.UseSignedPreferences)
				{
					int num = UnityEngine.Random.Range(255, 32767) << 2;
					Defs2.SignedPreferences.Add("Manterry", num.ToString(CultureInfo.InvariantCulture));
				}
			}
			else if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Skipping reward since it has been already claimed.");
			}
			if (ExperienceController.sharedController != null)
			{
				ExperienceController.sharedController.addExperience(Defs.ExpForTraining);
			}
		}
		MainMenuController.trainingCompleted = true;
		ShopNGUIController.GiveArmorArmy1OrNoviceArmor();
		try
		{
			Singleton<EggsManager>.Instance.AddEgg("egg_Training");
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in giving training egg: {0}", new object[]
			{
				ex
			});
		}
		AnalyticsFacade.SendCustomEventToAppsFlyer("Training complete", new Dictionary<string, string>());
		TrainingController.TrainingCompletedFlagForLogging = new bool?(true);
		PlayerPrefs.Save();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x00052520 File Offset: 0x00050720
	private void BannerTrainingCompleteInvoke()
	{
		GameObject.Find("Background_Training(Clone)").SetActive(false);
		coinsShop.hideCoinsShop();
		if (ABTestController.useBuffSystem)
		{
			Storager.setInt("Training.NoviceArmorUsedKey", 1, false);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/TrainigCompleteNGUI"));
		RewardWindowBase component = gameObject.GetComponent<RewardWindowBase>();
		FacebookController.StoryPriority priority = FacebookController.StoryPriority.Green;
		component.CollectOnlyNoShare = true;
		component.shareAction = delegate()
		{
			FacebookController.PostOpenGraphStory("complete", "tutorial", priority, null);
		};
		component.customHide = delegate()
		{
			this.GetRewardForTraining();
			ActivityIndicator.IsActiveIndicator = true;
			this.Invoke("LoadPromLevel", 0.4f);
		};
		component.priority = priority;
		component.twitterStatus = (() => "Training completed in @PixelGun3D! Come to play with me! \n#pixelgun3d #pixelgun #3d #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u");
		component.EventTitle = "Training Completed";
		component.HasReward = true;
		gameObject.transform.parent = InGameGUI.sharedInGameGUI.transform.GetChild(0);
		InGameGUI.sharedInGameGUI.joystikPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.interfacePanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.shopPanel.gameObject.SetActive(false);
		InGameGUI.sharedInGameGUI.bloodPanel.gameObject.SetActive(false);
		Player_move_c.SetLayerRecursively(gameObject, LayerMask.NameToLayer("NGUI"));
		gameObject.transform.localPosition = new Vector3(0f, 0f, -130f);
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		GameObject gameObject2 = new GameObject();
		UITexture uitexture = gameObject2.AddComponent<UITexture>();
		string path = ConnectSceneNGUIController.MainLoadingTexture();
		uitexture.mainTexture = Resources.Load<Texture>(path);
		uitexture.SetRect(0f, 0f, 1366f, 768f);
		uitexture.transform.SetParent(InGameGUI.sharedInGameGUI.transform.GetChild(0), false);
		uitexture.transform.localScale = Vector3.one;
		uitexture.transform.localPosition = Vector3.zero;
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayer.SetActive(false);
		}
	}

	// Token: 0x060014CF RID: 5327 RVA: 0x00052760 File Offset: 0x00050960
	private void LoadPromLevel()
	{
		Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm", LoadSceneMode.Single);
	}

	// Token: 0x060014D0 RID: 5328 RVA: 0x00052774 File Offset: 0x00050974
	public static void GoToNextLevel()
	{
		LevelCompleteLoader.action = null;
		LevelCompleteLoader.sceneName = "LevelComplete";
		AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
	}

	// Token: 0x04000C20 RID: 3104
	private Action OnPlayerAddedAct;

	// Token: 0x04000C21 RID: 3105
	private GameObject _player;

	// Token: 0x04000C22 RID: 3106
	private Player_move_c _playerMoveC;

	// Token: 0x04000C23 RID: 3107
	private bool runLoading;

	// Token: 0x04000C24 RID: 3108
	private IDisposable _backSubscription;
}
