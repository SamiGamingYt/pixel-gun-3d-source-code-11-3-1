using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x02000696 RID: 1686
	[DisallowMultipleComponent]
	internal sealed class LevelCompleteScript : MonoBehaviour
	{
		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x00130F50 File Offset: 0x0012F150
		public static bool IsInterfaceBusy
		{
			get
			{
				return !(LevelCompleteScript.sharedScript == null) && (LevelCompleteScript.IsShowRewardWindow() || LevelCompleteScript.sharedScript.DisplayLevelResultIsRunning || LevelCompleteScript.sharedScript.DisplaySurvivalResultIsRunning);
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06003AF4 RID: 15092 RVA: 0x00130F98 File Offset: 0x0012F198
		// (set) Token: 0x06003AF5 RID: 15093 RVA: 0x00130FA0 File Offset: 0x0012F1A0
		public bool DisplayLevelResultIsRunning { get; set; }

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x06003AF6 RID: 15094 RVA: 0x00130FAC File Offset: 0x0012F1AC
		// (set) Token: 0x06003AF7 RID: 15095 RVA: 0x00130FB4 File Offset: 0x0012F1B4
		public bool DisplaySurvivalResultIsRunning { get; set; }

		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003AF8 RID: 15096 RVA: 0x00130FC0 File Offset: 0x0012F1C0
		// (set) Token: 0x06003AF9 RID: 15097 RVA: 0x00130FC8 File Offset: 0x0012F1C8
		internal static GameResult LastGameResult { private get; set; }

		// Token: 0x06003AFA RID: 15098 RVA: 0x00130FD0 File Offset: 0x0012F1D0
		private bool AllStarsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt("AllStarsForBoxRewardWindowIsShown_" + boXName, 0) == 1;
		}

		// Token: 0x06003AFB RID: 15099 RVA: 0x00130FE8 File Offset: 0x0012F1E8
		private bool AllSecretsForBoxRewardWindowIsShown(string boXName)
		{
			return PlayerPrefs.GetInt("AllSecretsForBoxRewardWindowIsShown_" + boXName, 0) == 1;
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x00131000 File Offset: 0x0012F200
		private void Awake()
		{
			RewardWindowBase.Shown += this.HandleRewardWindowShown;
			LevelCompleteScript.sharedScript = this;
			EventDelegate.Add(this.rewardWindow.hideButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.continueButton.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.collect.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindow.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindow"));
			EventDelegate.Add(this.rewardWindowSurvival.hideButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.continueButton.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.collect.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.continueAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			EventDelegate.Add(this.rewardWindowSurvival.collectAndShare.onClick, new EventDelegate(this, "HideRewardWindowSurvival"));
			FacebookController.StoryPriority priority = FacebookController.StoryPriority.Red;
			this.rewardWindowSurvival.priority = priority;
			this.rewardWindowSurvival.twitterPriority = FacebookController.StoryPriority.ArenaLimit;
			this.rewardWindowSurvival.shareAction = delegate()
			{
				FacebookController.PostOpenGraphStory("complete", "fight", priority, new Dictionary<string, string>
				{
					{
						"map",
						Defs.SurvivalMaps[Defs.CurrentSurvMapIndex]
					}
				});
			};
			this.rewardWindowSurvival.HasReward = false;
			this.rewardWindowSurvival.twitterStatus = (() => "I've beaten ARENA score in @PixelGun3D! Can you beat my record? #pixelgun3d #pixelgun #pg3d #mobile #fps #shooter http://goo.gl/8fzL9u");
			this.rewardWindowSurvival.EventTitle = "Arena Survival";
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x001311EC File Offset: 0x0012F3EC
		private void HandleRewardWindowShown()
		{
			this._numOfRewardWindowsShown++;
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x001311FC File Offset: 0x0012F3FC
		private static bool IsBox1Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("School");
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x00131210 File Offset: 0x0012F410
		private static bool IsBox2Completed()
		{
			return CurrentCampaignGame.levelSceneName.StartsWith("Gluk");
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x00131224 File Offset: 0x0012F424
		private static void PostBoxCompletedAchievement()
		{
			string text = string.Empty;
			string achievementName = string.Empty;
			bool flag = LevelCompleteScript.IsBox1Completed();
			bool flag2 = LevelCompleteScript.IsBox2Completed();
			if (flag)
			{
				text = ((BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer) ? "CgkIr8rGkPIJEAIQCA" : "block_world_id");
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					text = "Block_Survivor_id";
				}
				achievementName = "Block World Survivor";
			}
			else if (flag2)
			{
				text = "CgkIr8rGkPIJEAIQCQ";
				achievementName = "Dragon Slayer";
			}
			if (string.IsNullOrEmpty(text))
			{
				Debug.LogWarning("Achievement Box Completed: id is null. Scene: " + CurrentCampaignGame.levelSceneName);
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AGSAchievementsClient.UpdateAchievementProgress(text, 100f, 0);
				Debug.LogFormat("Achievement {0} completed.", new object[]
				{
					achievementName
				});
			}
			else
			{
				Social.ReportProgress(text, 100.0, delegate(bool success)
				{
					Debug.LogFormat("Achievement {0} completed: {1}", new object[]
					{
						achievementName,
						success
					});
				});
			}
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x00131324 File Offset: 0x0012F524
		private IEnumerator PlayGetCoinsClip()
		{
			float delay = (!(ExperienceController.sharedController != null)) ? 0.5f : ExperienceController.sharedController.exp_1.length;
			yield return new WaitForSeconds(delay);
			if (this.awardClip != null && Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.awardClip);
			}
			yield break;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x00131340 File Offset: 0x0012F540
		private static string EnglishNameForCompletedLevel(out CampaignLevel campaignLevel)
		{
			campaignLevel = LevelBox.GetLevelBySceneName(CurrentCampaignGame.levelSceneName);
			if (LevelCompleteScript.IsBox3Completed())
			{
				return "???";
			}
			if (campaignLevel == null || campaignLevel.localizeKeyForLevelMap == null)
			{
				return "FARM";
			}
			return (LocalizationStore.GetByDefault(campaignLevel.localizeKeyForLevelMap) ?? "FARM").Replace("\n", " ");
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x001313AC File Offset: 0x0012F5AC
		private void GiveAwardForCampaign()
		{
			int num = 0;
			int num2 = 0;
			if (this._awardConferred || this._hasAwardForMission)
			{
				num = Mathf.Min(LevelCompleteScript.InitializeCoinIndexBound(), this._starCount);
				num *= ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
				num2 = 0;
				if (this._awardConferred)
				{
					num2 = this.GemsToAddForBox();
					int num3 = this.CoinsToAddForBox();
					num += num3;
					LevelCompleteScript.PostBoxCompletedAchievement();
				}
				if (num > 0)
				{
					BankController.AddCoins(num, true, AnalyticsConstants.AccrualType.Earned);
				}
				if (num2 > 0)
				{
					BankController.AddGems(num2, true, AnalyticsConstants.AccrualType.Earned);
				}
			}
			int num4 = 0;
			if (this._starCount == 3 && this._oldStarCount < 3 && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 31)
			{
				num4 += 5;
			}
			if (this._boxCompletionExperienceAward != null && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 31)
			{
				num4 += this._boxCompletionExperienceAward.Value;
			}
			num4 *= ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff);
			if (num4 != 0)
			{
				this._experienceController.addExperience(num4);
			}
			if (num > 0 || num2 > 0)
			{
				base.StartCoroutine(this.PlayGetCoinsClip());
				if (num2 > 0)
				{
					this._shouldBlinkGemsIndicatorAfterRewardWindow = true;
				}
				if (num > 0)
				{
					this._shouldBlinkCoinsIndicatorAfterRewardWindow = true;
				}
			}
			bool flag = this._awardConferred && LevelCompleteScript.IsBox3Completed();
			CampaignLevel campaignLevel = null;
			string arg = LevelCompleteScript.EnglishNameForCompletedLevel(out campaignLevel);
			string twitterStatus = string.Format("All enemies {0} {1} are defeated in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", campaignLevel.predlog, arg);
			string eventTitle = "Level Complete";
			if (this._isLastLevel)
			{
				if (LevelCompleteScript.IsBox1Completed())
				{
					twitterStatus = "I’ve defeated the RIDER in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 1 Complete";
				}
				else if (LevelCompleteScript.IsBox2Completed())
				{
					twitterStatus = "I’ve defeated the DRAGON in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 2 Complete";
				}
				else if (LevelCompleteScript.IsBox3Completed())
				{
					twitterStatus = "I’ve defeated the EVIL BUG in @PixelGun3D! Join my adventures now! #pixelgun3d #pixelgun #3d #pg3d #mobile #fps http://goo.gl/8fzL9u";
					eventTitle = "Box 3 Complete";
				}
			}
			FacebookController.StoryPriority storyPriority = (!this._isLastLevel) ? FacebookController.StoryPriority.Red : FacebookController.StoryPriority.Green;
			this.rewardWindow.priority = storyPriority;
			this.rewardWindow.twitterStatus = (() => twitterStatus);
			this.rewardWindow.EventTitle = eventTitle;
			this.rewardWindow.HasReward = true;
			this.rewardWindow.shareAction = delegate()
			{
				FacebookController.PostOpenGraphStory((!this._isLastLevel) ? "complete" : "finish", (!this._isLastLevel) ? "mission" : "chapter", storyPriority, (!this._isLastLevel) ? new Dictionary<string, string>
				{
					{
						"mission",
						CurrentCampaignGame.levelSceneName
					}
				} : new Dictionary<string, string>
				{
					{
						"chapter",
						CurrentCampaignGame.boXName
					}
				});
			};
			this.rewardSettings.normalBackground.SetActive(PremiumAccountController.Instance == null || !PremiumAccountController.Instance.isAccountActive);
			this.rewardSettings.premiumBackground.SetActive(PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive);
			foreach (UILabel uilabel in this.rewardSettings.bossDefeatedHeader)
			{
				uilabel.gameObject.SetActive(this._awardConferred);
				if (this._awardConferred)
				{
					if (LevelCompleteScript.IsBox1Completed())
					{
						uilabel.text = LocalizationStore.Get("Key_1546");
					}
					else if (LevelCompleteScript.IsBox2Completed())
					{
						uilabel.text = LocalizationStore.Get("Key_1547");
					}
					else if (LevelCompleteScript.IsBox3Completed())
					{
						uilabel.text = LocalizationStore.Get("Key_1548");
					}
				}
			}
			foreach (UILabel uilabel2 in this.rewardSettings.boxCompletedLabels)
			{
				uilabel2.gameObject.SetActive(this._awardConferred);
				if (this._awardConferred)
				{
					if (LevelCompleteScript.IsBox1Completed())
					{
						uilabel2.text = LocalizationStore.Get("Key_1549");
					}
					else if (LevelCompleteScript.IsBox2Completed())
					{
						uilabel2.text = LocalizationStore.Get("Key_1550");
					}
					else if (LevelCompleteScript.IsBox3Completed())
					{
						uilabel2.text = LocalizationStore.Get("Key_1551");
					}
				}
			}
			foreach (UILabel uilabel3 in this.rewardSettings.missionHeader)
			{
				uilabel3.gameObject.SetActive(!this._awardConferred);
			}
			float num5 = (!flag) ? 1f : 0.8f;
			this.rewardSettings.coinsReward.gameObject.SetActive(num > 0);
			this.rewardSettings.coinsReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel uilabel4 in this.rewardSettings.coinsRewardLabels)
			{
				uilabel4.text = "+" + num.ToString() + " " + LocalizationStore.Get("Key_0275");
			}
			this.rewardSettings.coinsMultiplierContainer.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1 && !this._awardConferred);
			foreach (UILabel uilabel5 in this.rewardSettings.coinsMultiplierLabels)
			{
				uilabel5.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff).ToString();
			}
			this.rewardSettings.gemsReward.gameObject.SetActive(num2 > 0);
			this.rewardSettings.gemsReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel uilabel6 in this.rewardSettings.gemsRewrdLabels)
			{
				uilabel6.text = "+" + num2.ToString() + " " + LocalizationStore.Get("Key_0951");
			}
			this.rewardSettings.gemsMultyplierContainer.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1 && !this._awardConferred);
			foreach (UILabel uilabel7 in this.rewardSettings.gemsMultiplierLabels)
			{
				uilabel7.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff).ToString();
			}
			this.rewardSettings.experienceReward.gameObject.SetActive(num4 > 0);
			this.rewardSettings.experienceReward.localScale = new Vector3(num5, num5, num5);
			foreach (UILabel uilabel8 in this.rewardSettings.experienceRewardLabels)
			{
				uilabel8.text = "+" + num4.ToString() + " " + LocalizationStore.Get("Key_0204");
			}
			this.rewardSettings.expMultiplier.SetActive(((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff) > 1);
			foreach (UILabel uilabel9 in this.rewardSettings.expMultiplierLabels)
			{
				uilabel9.text = "x" + ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff).ToString();
			}
			this.rewardSettings.badcode.gameObject.SetActive(flag);
			this.rewardSettings.badcode.localScale = new Vector3(num5, num5, num5);
			this.rewardSettings.grid.Reposition();
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x00131DA0 File Offset: 0x0012FFA0
		private void Start()
		{
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			QuestSystem.Instance.SaveQuestProgressIfDirty();
			if (Defs.IsSurvival)
			{
				this.backgroundSurvivalTexture.SetActive(true);
			}
			else
			{
				this.backgroundTexture.SetActive(true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			if (LevelCompleteScript.LastGameResult == GameResult.Death)
			{
				this._gameOver = true;
				LevelCompleteScript.LastGameResult = GameResult.None;
			}
			if (!this._gameOver && !Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Completed";
				StoreKitEventListener.State.Parameters["Level"] = CurrentCampaignGame.levelSceneName + " Level Completed";
			}
			else if (this._gameOver && !Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Level Failed";
				StoreKitEventListener.State.Parameters["Level"] = CurrentCampaignGame.levelSceneName + " Level Failed";
			}
			else if (!this._gameOver && Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Player quit";
				StoreKitEventListener.State.Parameters["Waves"] = StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")) + " Player quit";
			}
			else if (this._gameOver && Defs.IsSurvival)
			{
				StoreKitEventListener.State.PurchaseKey = "Game over";
				StoreKitEventListener.State.Parameters["Waves"] = StoreKitEventListener.State.Parameters["Waves"].Substring(0, StoreKitEventListener.State.Parameters["Waves"].IndexOf(" In game")) + " Game over";
			}
			this._shouldShowFacebookButton = FacebookController.FacebookSupported;
			this._shouldShowTwitterButton = TwitterController.TwitterSupported;
			this._experienceController = LevelCompleteScript.InitializeExperienceController();
			LevelCompleteScript.BindButtonHandler(this.menuButton, new EventHandler(this.HandleMenuButton));
			LevelCompleteScript.BindButtonHandler(this.retryButton, new EventHandler(this.HandleRetryButton));
			LevelCompleteScript.BindButtonHandler(this.nextButton, new EventHandler(this.HandleNextButton));
			LevelCompleteScript.BindButtonHandler(this.shopButton, new EventHandler(this.HandleShopButton));
			LevelCompleteScript.BindButtonHandler(this.quitButton, new EventHandler(this.HandleQuitButton));
			LevelCompleteScript.BindButtonHandler(this.facebookButton, new EventHandler(this.HandleFacebookButton));
			LevelCompleteScript.BindButtonHandler(this.twitterButton, new EventHandler(this.HandleTwitterButton));
			if (!Defs.IsSurvival)
			{
				int num = -1;
				LevelBox levelBox = null;
				foreach (LevelBox levelBox2 in LevelBox.campaignBoxes)
				{
					if (levelBox2.name.Equals(CurrentCampaignGame.boXName))
					{
						levelBox = levelBox2;
						for (int num2 = 0; num2 != levelBox2.levels.Count; num2++)
						{
							CampaignLevel campaignLevel = levelBox2.levels[num2];
							if (campaignLevel.sceneName.Equals(CurrentCampaignGame.levelSceneName))
							{
								num = num2;
								break;
							}
						}
						break;
					}
				}
				if (levelBox != null)
				{
					this._isLastLevel = (num >= levelBox.levels.Count - 1);
					this._nextSceneName = levelBox.levels[(!this._isLastLevel) ? (num + 1) : num].sceneName;
				}
				else
				{
					Debug.LogError("Current box not found in the list of boxes!");
					this._isLastLevel = true;
					this._nextSceneName = SceneManager.GetActiveScene().name;
				}
				this._oldStarCount = 0;
				this._starCount = LevelCompleteScript.InitializeStarCount();
				if (!this._gameOver)
				{
					if (WeaponManager.sharedManager != null)
					{
						WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
					}
					Dictionary<string, int> dictionary = CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName];
					if (!dictionary.ContainsKey(CurrentCampaignGame.levelSceneName))
					{
						this.completedFirstTime = true;
						if (this._isLastLevel)
						{
							this._boxCompletionExperienceAward = new int?(levelBox.CompletionExperienceAward);
						}
						dictionary.Add(CurrentCampaignGame.levelSceneName, this._starCount);
						CampaignProgress.SaveCampaignProgress();
						try
						{
							string text = null;
							if (this._isLastLevel)
							{
								if (LevelCompleteScript.IsBox1Completed())
								{
									text = "Box 1";
								}
								else if (LevelCompleteScript.IsBox2Completed())
								{
									text = "Box 2";
								}
								else
								{
									text = "Box 3";
								}
							}
							CampaignLevel campaignLevel2;
							string text2 = LevelCompleteScript.EnglishNameForCompletedLevel(out campaignLevel2);
							AnalyticsStuff.LogCampaign(text2, text);
							AnalyticsFacade.SendCustomEventToFacebook("campaign_level_reached", new Dictionary<string, object>
							{
								{
									"level",
									text2
								},
								{
									"box_level",
									string.Format("{0}: {1}", text, text2)
								}
							});
						}
						catch (Exception arg)
						{
							Debug.LogError("Exception in LogCampaign(LevelCompleteScript): " + arg);
						}
					}
					else
					{
						this._oldStarCount = dictionary[CurrentCampaignGame.levelSceneName];
						dictionary[CurrentCampaignGame.levelSceneName] = Math.Max(this._oldStarCount, this._starCount);
						CampaignProgress.SaveCampaignProgress();
					}
					CampaignProgress.OpenNewBoxIfPossible();
					Dictionary<string, <>__AnonType1<int, int>> rememberedAmmo = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().ToDictionary((Weapon w) => w.weaponPrefab.nameNoClone(), (Weapon w) => new
					{
						AmmoInClip = w.currentAmmoInClip,
						AmmoInBackpack = w.currentAmmoInBackpack
					});
					Action returnRememberedAmmp = delegate()
					{
						IEnumerable<Weapon> source2 = WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>();
						KeyValuePair<string, <>__AnonType1<int, int>> kvp;
						foreach (var kvp2 in rememberedAmmo)
						{
							kvp = kvp2;
							Weapon weapon = source2.FirstOrDefault((Weapon w) => w.weaponPrefab.nameNoClone() == kvp.Key);
							if (weapon != null)
							{
								weapon.currentAmmoInClip = kvp.Value.AmmoInClip;
								weapon.currentAmmoInBackpack = kvp.Value.AmmoInBackpack;
							}
						}
					};
					if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
					{
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							ProgressSynchronizer.Instance.AuthenticateAndSynchronize(delegate
							{
								WeaponManager.sharedManager.Reset(WeaponManager.sharedManager.CurrentFilterMap);
								returnRememberedAmmp();
							}, true);
							CampaignProgressSynchronizer.Instance.Sync();
						}
						else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
						{
							ProgressSynchronizer.Instance.SynchronizeAmazonProgress();
							WeaponManager.sharedManager.Reset(0);
							returnRememberedAmmp();
						}
					}
					if (Application.platform == RuntimePlatform.IPhonePlayer)
					{
						ProgressSynchronizer.Instance.SynchronizeIosProgress();
						WeaponManager.sharedManager.Reset(0);
						CampaignProgressSynchronizer.Instance.Sync();
						returnRememberedAmmp();
						AchievementSynchronizer.Instance.Sync();
					}
					try
					{
						if (!this.AllStarsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							int num3 = dictionary.Values.ToList<int>().Sum();
							this._shouldShowAllStarsCollectedRewardWindow = (num3 == LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels.Count * 3);
						}
						if (!this.AllSecretsForBoxRewardWindowIsShown(CurrentCampaignGame.boXName))
						{
							List<string> source = (from level in LevelBox.campaignBoxes.Find((LevelBox lb) => lb.name == CurrentCampaignGame.boXName).levels
							select level.sceneName).ToList<string>();
							HashSet<string> levelsWhereGotCoins = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Coin));
							HashSet<string> levelsWhereGotGems = new HashSet<string>(CoinBonus.GetLevelsWhereGotBonus(VirtualCurrencyBonusType.Gem));
							this._shouldShowAllSecretsCollectedRewardWindow = source.All((string l) => levelsWhereGotCoins.Contains(l) && levelsWhereGotGems.Contains(l));
						}
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					this._hasAwardForMission = (this._starCount > this._oldStarCount && LevelCompleteScript.InitializeCoinIndexBound() > this._oldStarCount);
				}
				this._awardConferred = this.InitializeAwardConferred();
			}
			this.survivalResults.SetActive(false);
			this.quitButton.SetActive(false);
			if (!this._gameOver)
			{
				bool active = PremiumAccountController.Instance != null && PremiumAccountController.Instance.isAccountActive;
				this.premium.SetActive(active);
				if (!Defs.IsSurvival)
				{
					this.award1coinSprite.SetActive(true);
				}
				foreach (GameObject gameObject in this.statisticLabels)
				{
					gameObject.SetActive(Defs.IsSurvival);
				}
				if (this._starCount > this._oldStarCount)
				{
					CoinsMessage.FireCoinsAddedEvent(false, 2);
				}
			}
			else
			{
				this.award1coinSprite.SetActive(false);
				this.nextButton.SetActive(false);
				this.checkboxSpritePrototype.SetActive(false);
				if (!Defs.IsSurvival && this.gameOverSprite != null)
				{
					this.gameOverSprite.SetActive(true);
				}
				foreach (GameObject gameObject2 in this.statisticLabels)
				{
					gameObject2.SetActive(Defs.IsSurvival);
				}
				if (!Defs.IsSurvival)
				{
					float x = (this.retryButton.transform.position.x - this.menuButton.transform.position.x) / 2f;
					Vector3 b = new Vector3(x, 0f, 0f);
					this.menuButton.transform.position = this.retryButton.transform.position - b;
					this.retryButton.transform.position += b;
				}
				this.menuButton.SetActive(!Defs.IsSurvival);
				if (!Defs.IsSurvival)
				{
					base.StartCoroutine(this.TryToShowExpiredBanner());
				}
			}
			if (Defs.IsSurvival)
			{
				if (WeaponManager.sharedManager != null && WavesSurvivedStat.SurvivedWaveCount > 0)
				{
					WeaponManager.sharedManager.DecreaseTryGunsMatchesCount();
				}
				WeaponManager sharedManager = WeaponManager.sharedManager;
				sharedManager.Reset(0);
			}
			LevelCompleteScript._instance = this;
			if ((!Defs.IsSurvival && (this._awardConferred || this._hasAwardForMission)) || (this._starCount == 3 && this._oldStarCount < 3 && !this._gameOver && ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel < 31))
			{
				this.mainInterface.SetActive(false);
				this.rewardWindow.gameObject.SetActive(true);
				this.GiveAwardForCampaign();
			}
			else if (!Defs.IsSurvival)
			{
				this.mainInterface.SetActive(true);
				this.rewardWindow.gameObject.SetActive(false);
				if (!this._gameOver && this.brightStarPrototypeSprite != null && this.darkStarPrototypeSprite != null)
				{
					base.StartCoroutine(this.DisplayLevelResult());
				}
			}
			else if (Defs.IsSurvival)
			{
				int num4 = LevelCompleteScript.CalculateExperienceAward(GlobalGameController.Score);
				if (num4 > 0)
				{
					this._experienceController.addExperience(num4 * ((!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff));
				}
				if (GlobalGameController.HasSurvivalRecord)
				{
					GlobalGameController.HasSurvivalRecord = false;
					if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && !Device.isPixelGunLow)
					{
						this.mainInterface.SetActive(false);
						this.rewardWindowSurvival.gameObject.SetActive(true);
						foreach (UILabel uilabel in this.survivalRewardWindowSettings.scoreLabels)
						{
							uilabel.text = string.Format(LocalizationStore.Get("Key_1553"), GlobalGameController.Score);
						}
					}
					else
					{
						this.DisplaySurvivalResult();
					}
				}
				else
				{
					this.DisplaySurvivalResult();
				}
			}
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00132A10 File Offset: 0x00130C10
		public void HideRewardWindow()
		{
			ButtonClickSound.TryPlayClick();
			this.mainInterface.SetActive(true);
			this.rewardWindow.gameObject.SetActive(false);
			if (!Defs.IsSurvival && this.brightStarPrototypeSprite != null && this.darkStarPrototypeSprite != null)
			{
				base.StartCoroutine(this.DisplayLevelResult());
			}
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x00132A78 File Offset: 0x00130C78
		public void HideRewardWindowSurvival()
		{
			ButtonClickSound.TryPlayClick();
			this.mainInterface.SetActive(true);
			this.rewardWindowSurvival.gameObject.SetActive(false);
			this.DisplaySurvivalResult();
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x00132AB0 File Offset: 0x00130CB0
		private void OnDestroy()
		{
			LevelCompleteScript._instance = null;
			if (this._experienceController != null)
			{
				this._experienceController.isShowRanks = false;
			}
			PlayerPrefs.Save();
			RewardWindowBase.Shown -= this.HandleRewardWindowShown;
			LevelCompleteScript.sharedScript = null;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x00132AFC File Offset: 0x00130CFC
		public static bool IsShowRewardWindow()
		{
			if (LevelCompleteScript.sharedScript == null)
			{
				return false;
			}
			bool flag = LevelCompleteScript.sharedScript.rewardWindowSurvival != null && LevelCompleteScript.sharedScript.rewardWindowSurvival.gameObject != null && LevelCompleteScript.sharedScript.rewardWindowSurvival.gameObject.activeInHierarchy;
			bool flag2 = LevelCompleteScript.sharedScript.rewardWindow != null && LevelCompleteScript.sharedScript.rewardWindow.gameObject != null && LevelCompleteScript.sharedScript.rewardWindow.gameObject.activeInHierarchy;
			return flag || flag2;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00132BB4 File Offset: 0x00130DB4
		private void Update()
		{
			if (this._experienceController != null && BankController.Instance != null && !BankController.Instance.InterfaceEnabled && !ShopNGUIController.GuiActive)
			{
				this._experienceController.isShowRanks = (this.RentWindowPoint.childCount == 0 && !this.loadingPanel.activeSelf && (!(LevelCompleteScript.sharedScript != null) || !LevelCompleteScript.IsShowRewardWindow()));
			}
			bool active = FacebookController.FacebookSupported && this._shouldShowFacebookButton && FacebookController.FacebookPost_Old_Supported && FB.IsLoggedIn;
			this.facebookButton.SetActive(active);
			this.twitterButton.SetActive(TwitterController.TwitterSupported && this._shouldShowTwitterButton && TwitterController.TwitterSupported_OldPosts && TwitterController.IsLoggedIn);
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x00132CAC File Offset: 0x00130EAC
		private void OnEnable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
			}
			this._backSubscription = BackSystem.Instance.Register(delegate
			{
				this.HandleMenuButton(this, EventArgs.Empty);
			}, "Level Complete");
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x00132CE8 File Offset: 0x00130EE8
		private void OnDisable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x00132D08 File Offset: 0x00130F08
		private static void BindButtonHandler(GameObject button, EventHandler handler)
		{
			if (button != null)
			{
				ButtonHandler component = button.GetComponent<ButtonHandler>();
				if (component != null)
				{
					component.Clicked += handler;
				}
			}
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x00132D3C File Offset: 0x00130F3C
		private static int CalculateExperienceAward(int score)
		{
			if (ExperienceController.sharedController != null && ExperienceController.sharedController.currentLevel == 31)
			{
				return 0;
			}
			int num = (!Application.isEditor) ? 1 : 100;
			if (score < 15000 / num)
			{
				return 0;
			}
			if (score < 50000 / num)
			{
				return 10;
			}
			if (score < 100000 / num)
			{
				return 35;
			}
			if (score < 150000 / num)
			{
				return 50;
			}
			return 75;
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x00132DC0 File Offset: 0x00130FC0
		private void DisplaySurvivalResult()
		{
			try
			{
				this.DisplaySurvivalResultIsRunning = true;
				this.menuButton.SetActive(false);
				this.retryButton.SetActive(false);
				this.nextButton.SetActive(false);
				this.shopButton.SetActive(false);
				this.quitButton.SetActive(false);
				this.survivalResults.SetActive(true);
				this.retryButton.SetActive(true);
				this.shopButton.SetActive(true);
				this.quitButton.SetActive(true);
				base.StartCoroutine(this.TryToShowExpiredBanner());
			}
			finally
			{
				this.DisplaySurvivalResultIsRunning = false;
			}
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x00132E78 File Offset: 0x00131078
		private static int InitializeCoinIndexBound()
		{
			int diffGame = Defs.diffGame;
			return diffGame + 1;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x00132E90 File Offset: 0x00131090
		private static bool IsBox3Completed()
		{
			return CurrentCampaignGame.levelSceneName.Equals("Code_campaign3");
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x00132EA4 File Offset: 0x001310A4
		private int GemsToAddForBox()
		{
			int result = 0;
			if (LevelCompleteScript.IsBox1Completed())
			{
				result = LevelBox.campaignBoxes[0].gems;
			}
			else if (LevelCompleteScript.IsBox2Completed())
			{
				result = LevelBox.campaignBoxes[1].gems;
			}
			else if (LevelCompleteScript.IsBox3Completed())
			{
				result = LevelBox.campaignBoxes[2].gems;
			}
			return result;
		}

		// Token: 0x06003B12 RID: 15122 RVA: 0x00132F10 File Offset: 0x00131110
		private int CoinsToAddForBox()
		{
			int result = 0;
			if (LevelCompleteScript.IsBox1Completed())
			{
				result = LevelBox.campaignBoxes[0].coins;
			}
			else if (LevelCompleteScript.IsBox2Completed())
			{
				result = LevelBox.campaignBoxes[1].coins;
			}
			else if (LevelCompleteScript.IsBox3Completed())
			{
				result = LevelBox.campaignBoxes[2].coins;
			}
			return result;
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x00132F7C File Offset: 0x0013117C
		private IEnumerator DisplayLevelResult()
		{
			try
			{
				this.DisplayLevelResultIsRunning = true;
				this.menuButton.SetActive(false);
				this.retryButton.SetActive(false);
				this.nextButton.SetActive(false);
				this.shopButton.SetActive(false);
				this._shouldShowFacebookButton = false;
				this._shouldShowTwitterButton = false;
				int coinIndexBound = LevelCompleteScript.InitializeCoinIndexBound();
				List<GameObject> stars = new List<GameObject>(3);
				for (int i = 0; i != 3; i++)
				{
					float x = -140f + (float)i * 140f;
					GameObject star = UnityEngine.Object.Instantiate<GameObject>(this.darkStarPrototypeSprite);
					star.transform.parent = this.darkStarPrototypeSprite.transform.parent;
					star.transform.localPosition = new Vector3(x, this.darkStarPrototypeSprite.transform.localPosition.y, 0f);
					star.transform.localScale = this.darkStarPrototypeSprite.transform.localScale;
					star.SetActive(true);
					stars.Add(star);
				}
				int currentStarIndex = 0;
				for (int checkboxIndex = 0; checkboxIndex < 3; checkboxIndex++)
				{
					if (checkboxIndex != 1 || CurrentCampaignGame.completeInTime)
					{
						if (checkboxIndex != 2 || CurrentCampaignGame.withoutHits)
						{
							yield return new WaitForSeconds(0.7f);
							GameObject star2 = UnityEngine.Object.Instantiate<GameObject>(this.brightStarPrototypeSprite);
							star2.transform.parent = this.brightStarPrototypeSprite.transform.parent;
							star2.transform.localPosition = stars[currentStarIndex].transform.localPosition;
							star2.transform.localScale = stars[currentStarIndex].transform.localScale;
							star2.SetActive(true);
							UnityEngine.Object.Destroy(stars[currentStarIndex]);
							GameObject checkbox = UnityEngine.Object.Instantiate<GameObject>(this.checkboxSpritePrototype);
							checkbox.transform.parent = this.checkboxSpritePrototype.transform.parent;
							checkbox.transform.localPosition = new Vector3(this.checkboxSpritePrototype.transform.localPosition.x, this.checkboxSpritePrototype.transform.localPosition.y - 45f * (float)checkboxIndex, this.checkboxSpritePrototype.transform.localPosition.z);
							checkbox.transform.localScale = this.checkboxSpritePrototype.transform.localScale;
							checkbox.SetActive(true);
							if (this.starClips != null && currentStarIndex < this.starClips.Length && this.starClips[currentStarIndex] != null && Defs.isSoundFX)
							{
								NGUITools.PlaySound(this.starClips[currentStarIndex]);
							}
							currentStarIndex++;
						}
					}
				}
				UnityEngine.Object.Destroy(this.brightStarPrototypeSprite);
				UnityEngine.Object.Destroy(this.darkStarPrototypeSprite);
				this.menuButton.SetActive(true);
				this.retryButton.SetActive(true);
				this.nextButton.SetActive(true);
				this.shopButton.SetActive(true);
				this._shouldShowFacebookButton = FacebookController.FacebookSupported;
				this._shouldShowTwitterButton = TwitterController.TwitterSupported;
				if (this._shouldBlinkCoinsIndicatorAfterRewardWindow)
				{
					CoinsMessage.FireCoinsAddedEvent(false, 2);
				}
				if (this._shouldBlinkGemsIndicatorAfterRewardWindow)
				{
					CoinsMessage.FireCoinsAddedEvent(true, 2);
				}
				yield return new WaitForSeconds(0.7f);
				base.StartCoroutine(this.TryToShowExpiredBanner());
			}
			finally
			{
				this.DisplayLevelResultIsRunning = false;
			}
			yield break;
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x00132F9C File Offset: 0x0013119C
		private static string BoxNameForTwitter()
		{
			return LevelCompleteScript.boxNamesTwitter[CurrentCampaignGame.boXName];
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x00132FB0 File Offset: 0x001311B0
		private IEnumerator TryToShowExpiredBanner()
		{
			while (FriendsController.sharedController == null || TempItemsController.sharedController == null)
			{
				yield return null;
			}
			for (;;)
			{
				yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
				try
				{
					if (!ShopNGUIController.GuiActive && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && (!(ExpController.Instance != null) || !ExpController.Instance.WaitingForLevelUpView) && (!(ExpController.Instance != null) || !ExpController.Instance.IsLevelUpShown) && !this.loadingPanel.activeInHierarchy)
					{
						if (this.RentWindowPoint.childCount == 0)
						{
							if (this._shouldShowAllStarsCollectedRewardWindow && this._numOfRewardWindowsShown < 2)
							{
								this._shouldShowAllStarsCollectedRewardWindow = false;
								PlayerPrefs.SetInt("AllStarsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
								if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && !Device.isPixelGunLow)
								{
									GameObject window = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/AllStarsNGUI"));
									RewardWindowBase rwb = window.GetComponent<RewardWindowBase>();
									FacebookController.StoryPriority priority = FacebookController.StoryPriority.Green;
									rwb.priority = priority;
									rwb.shareAction = delegate()
									{
										FacebookController.PostOpenGraphStory("get", "star", priority, new Dictionary<string, string>
										{
											{
												"chapter",
												CurrentCampaignGame.boXName
											}
										});
									};
									rwb.HasReward = false;
									rwb.twitterStatus = (() => string.Format("I've got all the stars in {0} in @PixelGun3D! Play now and try to get them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", LevelCompleteScript.BoxNameForTwitter()));
									rwb.EventTitle = "All Stars";
									AllStarsRewardSettings asrs = window.GetComponent<AllStarsRewardSettings>();
									foreach (UILabel lab in asrs.headerLabels)
									{
										if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
										{
											lab.text = LocalizationStore.Get("Key_1543");
										}
										else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
										{
											lab.text = LocalizationStore.Get("Key_1544");
										}
										else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
										{
											lab.text = LocalizationStore.Get("Key_1545");
										}
									}
									window.transform.parent = this.RentWindowPoint;
									Player_move_c.SetLayerRecursively(window, LayerMask.NameToLayer("Default"));
									window.transform.localPosition = new Vector3(0f, 0f, -130f);
									window.transform.localRotation = Quaternion.identity;
									window.transform.localScale = new Vector3(1f, 1f, 1f);
								}
							}
							else if (this._shouldShowAllSecretsCollectedRewardWindow && this._numOfRewardWindowsShown < 2)
							{
								this._shouldShowAllSecretsCollectedRewardWindow = false;
								PlayerPrefs.SetInt("AllSecretsForBoxRewardWindowIsShown_" + CurrentCampaignGame.boXName, 1);
								if ((FacebookController.FacebookSupported || TwitterController.TwitterSupported) && !Device.isPixelGunLow)
								{
									GameObject window2 = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("NguiWindows/AllSecretsNGUI"));
									RewardWindowBase rwb2 = window2.GetComponent<RewardWindowBase>();
									FacebookController.StoryPriority priority2 = FacebookController.StoryPriority.Green;
									rwb2.priority = priority2;
									rwb2.shareAction = delegate()
									{
										FacebookController.PostOpenGraphStory("find", "secret", priority2, new Dictionary<string, string>
										{
											{
												"chapter",
												CurrentCampaignGame.boXName
											}
										});
									};
									rwb2.HasReward = false;
									rwb2.twitterStatus = (() => string.Format("I've found all coins in {0} in @PixelGun3D! Play now and try to find them! #pixelgun3d #pixelgun #pg3d http://goo.gl/8fzL9u", LevelCompleteScript.BoxNameForTwitter()));
									rwb2.EventTitle = "All Coins";
									AllSecretsRewardSettings asrs2 = window2.GetComponent<AllSecretsRewardSettings>();
									foreach (UILabel lab2 in asrs2.headerLabels)
									{
										if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[0].name)
										{
											lab2.text = LocalizationStore.Get("Key_1540");
										}
										else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[1].name)
										{
											lab2.text = LocalizationStore.Get("Key_1541");
										}
										else if (CurrentCampaignGame.boXName == LevelBox.campaignBoxes[2].name)
										{
											lab2.text = LocalizationStore.Get("Key_1542");
										}
									}
									window2.transform.parent = this.RentWindowPoint;
									Player_move_c.SetLayerRecursively(window2, LayerMask.NameToLayer("Default"));
									window2.transform.localPosition = new Vector3(0f, 0f, -130f);
									window2.transform.localRotation = Quaternion.identity;
									window2.transform.localScale = new Vector3(1f, 1f, 1f);
								}
							}
							else
							{
								bool flag = Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1 && ShopNGUIController.ShowPremimAccountExpiredIfPossible(this.RentWindowPoint, "Default", string.Empty, true);
							}
						}
					}
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogWarning("exception in LevelComplete  TryToShowExpiredBanner: " + e);
				}
			}
			yield break;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x00132FCC File Offset: 0x001311CC
		private void HandleMenuButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			bool afterDeath = LevelCompleteScript.LastGameResult == GameResult.Death;
			string reasonToDismissInterstitialCampaign = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialCampaign(afterDeath);
			if (string.IsNullOrEmpty(reasonToDismissInterstitialCampaign))
			{
				if (Application.isEditor)
				{
					Debug.Log("<color=magenta>HandleMenuButton()</color>");
				}
				LevelCompleteInterstitialRunner levelCompleteInterstitialRunner = new LevelCompleteInterstitialRunner();
				levelCompleteInterstitialRunner.Run();
			}
			else
			{
				string format = (!Application.isEditor) ? "Dismissing interstitial. {0}" : "<color=magenta>Dismissing interstitial. {0}</color>";
				Debug.LogFormat(format, new object[]
				{
					reasonToDismissInterstitialCampaign
				});
			}
			string sceneName = (!Defs.IsSurvival) ? "ChooseLevel" : Defs.MainMenuScene;
			Singleton<SceneLoader>.Instance.LoadScene(sceneName, LoadSceneMode.Single);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x00133080 File Offset: 0x00131280
		private void HandleQuitButton(object sender, EventArgs args)
		{
			bool afterDeath = LevelCompleteScript.LastGameResult == GameResult.Death;
			string reasonToDismissInterstitialSurvivalArena = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialSurvivalArena(afterDeath);
			if (string.IsNullOrEmpty(reasonToDismissInterstitialSurvivalArena))
			{
				if (Application.isEditor)
				{
					Debug.Log("<color=magenta>HandleQuitButton()</color>");
				}
				LevelCompleteInterstitialRunner levelCompleteInterstitialRunner = new LevelCompleteInterstitialRunner();
				levelCompleteInterstitialRunner.Run();
			}
			else
			{
				string format = (!Application.isEditor) ? "Dismissing interstitial. {0}" : "<color=magenta>Dismissing interstitial. {0}</color>";
				Debug.LogFormat(format, new object[]
				{
					reasonToDismissInterstitialSurvivalArena
				});
			}
			ActivityIndicator.IsActiveIndicator = true;
			this.loadingPanel.SetActive(true);
			this.mainPanel.SetActive(false);
			ExperienceController.sharedController.isShowRanks = false;
			base.Invoke("QuitLevel", 0.1f);
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x00133130 File Offset: 0x00131330
		[Obfuscation(Exclude = true)]
		private void QuitLevel()
		{
			Singleton<SceneLoader>.Instance.LoadSceneAsync(Defs.MainMenuScene, LoadSceneMode.Single);
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x00133144 File Offset: 0x00131344
		private static void SetInitialAmmoForAllGuns()
		{
			foreach (object obj in WeaponManager.sharedManager.allAvailablePlayerWeapons)
			{
				Weapon weapon = (Weapon)obj;
				WeaponSounds component = weapon.weaponPrefab.GetComponent<WeaponSounds>();
				if (weapon.currentAmmoInClip + weapon.currentAmmoInBackpack < component.InitialAmmoWithEffectsApplied + component.ammoInClip)
				{
					weapon.currentAmmoInClip = component.ammoInClip;
					weapon.currentAmmoInBackpack = component.InitialAmmoWithEffectsApplied;
				}
				else if (weapon.currentAmmoInClip < component.ammoInClip)
				{
					int num = Mathf.Min(component.ammoInClip - weapon.currentAmmoInClip, weapon.currentAmmoInBackpack);
					weapon.currentAmmoInClip += num;
					weapon.currentAmmoInBackpack -= num;
				}
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x00133244 File Offset: 0x00131444
		private void HandleRetryButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			if (!Defs.IsSurvival)
			{
				LevelCompleteScript.SetInitialAmmoForAllGuns();
			}
			else
			{
				WeaponManager.sharedManager.Reset(0);
			}
			GlobalGameController.Score = 0;
			if (Defs.IsSurvival)
			{
				Defs.CurrentSurvMapIndex = UnityEngine.Random.Range(0, Defs.SurvivalMaps.Length);
			}
			Singleton<SceneLoader>.Instance.LoadScene("CampaignLoading", LoadSceneMode.Single);
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x001332B4 File Offset: 0x001314B4
		private void HandleFacebookButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			FacebookController.ShowPostDialog();
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x001332D0 File Offset: 0x001314D0
		private void HandleTwitterButton(object sender, EventArgs args)
		{
			if (Application.isEditor)
			{
				Debug.Log("Send Twitter: " + this._SocialMessage());
				return;
			}
			if (this._shopInstance != null)
			{
				return;
			}
			if (TwitterController.Instance != null)
			{
				TwitterController.Instance.PostStatusUpdate(this._SocialMessage(), null);
			}
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x00133330 File Offset: 0x00131530
		private void HandleNextButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			if (!this._isLastLevel)
			{
				CurrentCampaignGame.levelSceneName = this._nextSceneName;
				LevelCompleteScript.SetInitialAmmoForAllGuns();
				LevelArt.endOfBox = false;
				Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts) ? "CampaignLoading" : "LevelArt", LoadSceneMode.Single);
			}
			else
			{
				LevelArt.endOfBox = true;
				Singleton<SceneLoader>.Instance.LoadScene((!LevelArt.ShouldShowArts) ? "ChooseLevel" : "LevelArt", LoadSceneMode.Single);
			}
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x001333C4 File Offset: 0x001315C4
		[Obfuscation(Exclude = true)]
		private void GoToChooseLevel()
		{
			Singleton<SceneLoader>.Instance.LoadScene("ChooseLevel", LoadSceneMode.Single);
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x001333D8 File Offset: 0x001315D8
		private void HandleShopButton(object sender, EventArgs args)
		{
			if (this._shopInstance != null)
			{
				return;
			}
			this._shopInstance = ShopNGUIController.sharedShop;
			if (this._shopInstance != null)
			{
				this._shopInstance.SetInGame(false);
				ShopNGUIController.GuiActive = true;
				if (this.shopButtonSound != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(this.shopButtonSound);
				}
				if (Defs.IsSurvival)
				{
					this.backgroundSurvivalTexture.SetActive(false);
				}
				else
				{
					this.backgroundTexture.SetActive(false);
				}
				this._shopInstance.resumeAction = new Action(this.HandleResumeFromShop);
			}
			this.quitButton.SetActive(false);
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00133498 File Offset: 0x00131698
		private void HandleResumeFromShop()
		{
			if (this._shopInstance == null)
			{
				return;
			}
			ShopNGUIController.GuiActive = false;
			this._shopInstance.resumeAction = delegate()
			{
			};
			this._shopInstance = null;
			if (coinsPlashka.thisScript != null)
			{
				coinsPlashka.thisScript.enabled = false;
			}
			this.quitButton.SetActive(Defs.IsSurvival);
			if (this._experienceController != null)
			{
				this._experienceController.isShowRanks = true;
			}
			if (Defs.IsSurvival)
			{
				this.backgroundSurvivalTexture.SetActive(true);
			}
			else
			{
				this.backgroundTexture.SetActive(true);
			}
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x0013355C File Offset: 0x0013175C
		private static ExperienceController InitializeExperienceController()
		{
			ExperienceController experienceController = null;
			GameObject gameObject = GameObject.FindGameObjectWithTag("ExperienceController");
			if (gameObject != null)
			{
				experienceController = gameObject.GetComponent<ExperienceController>();
			}
			if (experienceController == null)
			{
				Debug.LogError("Cannot find experience controller.");
			}
			else
			{
				experienceController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
				experienceController.isShowRanks = true;
				if (ExpController.Instance != null)
				{
					ExpController.Instance.InterfaceEnabled = true;
				}
			}
			return experienceController;
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x001335E8 File Offset: 0x001317E8
		private static int InitializeStarCount()
		{
			int num = 1;
			if (CurrentCampaignGame.completeInTime)
			{
				num++;
			}
			if (CurrentCampaignGame.withoutHits)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06003B23 RID: 15139 RVA: 0x00133614 File Offset: 0x00131814
		private bool InitializeAwardConferred()
		{
			return this._isLastLevel && this.completedFirstTime;
		}

		// Token: 0x06003B24 RID: 15140 RVA: 0x0013362C File Offset: 0x0013182C
		private string _SocialMessage()
		{
			if (Defs.IsSurvival)
			{
				string text = string.Format(CultureInfo.InvariantCulture, LocalizationStore.GetByDefault("Key_1382"), new object[]
				{
					WavesSurvivedStat.SurvivedWaveCount,
					PlayerPrefs.GetInt(Defs.KilledZombiesSett, 0),
					GlobalGameController.Score
				});
				Debug.Log(text);
				return text;
			}
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(CurrentCampaignGame.levelSceneName);
			if (!(infoScene != null))
			{
				return "error map";
			}
			if (!this._gameOver)
			{
				string text2 = string.Format(LocalizationStore.GetByDefault("Key_1382"), infoScene.TranslateName, this._starCount);
				Debug.Log(text2);
				return text2;
			}
			string text3 = string.Format(LocalizationStore.GetByDefault("Key_1380"), infoScene.TranslateName);
			Debug.Log(text3);
			return text3;
		}

		// Token: 0x06003B25 RID: 15141 RVA: 0x00133708 File Offset: 0x00131908
		public static void SetInputEnabled(bool enabled)
		{
			if (LevelCompleteScript._instance != null)
			{
				LevelCompleteScript._instance.uiCamera.enabled = enabled;
			}
		}

		// Token: 0x04002B84 RID: 11140
		private const string AllStarsForBoxRewardWindowIsShownNameBase = "AllStarsForBoxRewardWindowIsShown_";

		// Token: 0x04002B85 RID: 11141
		private const string AllSecretsForBoxRewardWindowIsShownNameBase = "AllSecretsForBoxRewardWindowIsShown_";

		// Token: 0x04002B86 RID: 11142
		public static LevelCompleteScript sharedScript;

		// Token: 0x04002B87 RID: 11143
		public RewardWindowBase rewardWindow;

		// Token: 0x04002B88 RID: 11144
		public RewardWindowBase rewardWindowSurvival;

		// Token: 0x04002B89 RID: 11145
		public CampaignLevelCompleteRewardSettings rewardSettings;

		// Token: 0x04002B8A RID: 11146
		public ArenaRewardWindowSettings survivalRewardWindowSettings;

		// Token: 0x04002B8B RID: 11147
		public GameObject mainInterface;

		// Token: 0x04002B8C RID: 11148
		public GameObject premium;

		// Token: 0x04002B8D RID: 11149
		public Transform RentWindowPoint;

		// Token: 0x04002B8E RID: 11150
		public GameObject mainPanel;

		// Token: 0x04002B8F RID: 11151
		public GameObject loadingPanel;

		// Token: 0x04002B90 RID: 11152
		public GameObject quitButton;

		// Token: 0x04002B91 RID: 11153
		public GameObject menuButton;

		// Token: 0x04002B92 RID: 11154
		public GameObject retryButton;

		// Token: 0x04002B93 RID: 11155
		public GameObject nextButton;

		// Token: 0x04002B94 RID: 11156
		public GameObject shopButton;

		// Token: 0x04002B95 RID: 11157
		public GameObject brightStarPrototypeSprite;

		// Token: 0x04002B96 RID: 11158
		public GameObject darkStarPrototypeSprite;

		// Token: 0x04002B97 RID: 11159
		public GameObject award1coinSprite;

		// Token: 0x04002B98 RID: 11160
		public GameObject checkboxSpritePrototype;

		// Token: 0x04002B99 RID: 11161
		public AudioClip[] starClips;

		// Token: 0x04002B9A RID: 11162
		public AudioClip shopButtonSound;

		// Token: 0x04002B9B RID: 11163
		public AudioClip awardClip;

		// Token: 0x04002B9C RID: 11164
		public GameObject survivalResults;

		// Token: 0x04002B9D RID: 11165
		public GameObject facebookButton;

		// Token: 0x04002B9E RID: 11166
		public GameObject twitterButton;

		// Token: 0x04002B9F RID: 11167
		public GameObject backgroundTexture;

		// Token: 0x04002BA0 RID: 11168
		public GameObject backgroundSurvivalTexture;

		// Token: 0x04002BA1 RID: 11169
		public GameObject[] statisticLabels;

		// Token: 0x04002BA2 RID: 11170
		public GameObject gameOverSprite;

		// Token: 0x04002BA3 RID: 11171
		public UICamera uiCamera;

		// Token: 0x04002BA4 RID: 11172
		private static LevelCompleteScript _instance = null;

		// Token: 0x04002BA5 RID: 11173
		private int _numOfRewardWindowsShown;

		// Token: 0x04002BA6 RID: 11174
		private bool _hasAwardForMission;

		// Token: 0x04002BA7 RID: 11175
		private bool _shouldBlinkCoinsIndicatorAfterRewardWindow;

		// Token: 0x04002BA8 RID: 11176
		private bool _shouldBlinkGemsIndicatorAfterRewardWindow;

		// Token: 0x04002BA9 RID: 11177
		private bool _shouldShowAllStarsCollectedRewardWindow;

		// Token: 0x04002BAA RID: 11178
		private bool _shouldShowAllSecretsCollectedRewardWindow;

		// Token: 0x04002BAB RID: 11179
		private IDisposable _backSubscription;

		// Token: 0x04002BAC RID: 11180
		private static Dictionary<string, string> boxNamesTwitter = new Dictionary<string, string>
		{
			{
				"Real",
				"PIXELATED WORLD"
			},
			{
				"minecraft",
				"BLOCK WORLD"
			},
			{
				"Crossed",
				"CROSSED WORLDS"
			}
		};

		// Token: 0x04002BAD RID: 11181
		private bool _awardConferred;

		// Token: 0x04002BAE RID: 11182
		private AudioSource _awardAudioSource;

		// Token: 0x04002BAF RID: 11183
		private ExperienceController _experienceController;

		// Token: 0x04002BB0 RID: 11184
		private int _oldStarCount;

		// Token: 0x04002BB1 RID: 11185
		private int _starCount;

		// Token: 0x04002BB2 RID: 11186
		private ShopNGUIController _shopInstance;

		// Token: 0x04002BB3 RID: 11187
		private string _nextSceneName = string.Empty;

		// Token: 0x04002BB4 RID: 11188
		private bool _isLastLevel;

		// Token: 0x04002BB5 RID: 11189
		private int? _boxCompletionExperienceAward;

		// Token: 0x04002BB6 RID: 11190
		private bool completedFirstTime;

		// Token: 0x04002BB7 RID: 11191
		private bool _gameOver;

		// Token: 0x04002BB8 RID: 11192
		private bool _shouldShowFacebookButton;

		// Token: 0x04002BB9 RID: 11193
		private bool _shouldShowTwitterButton;
	}
}
