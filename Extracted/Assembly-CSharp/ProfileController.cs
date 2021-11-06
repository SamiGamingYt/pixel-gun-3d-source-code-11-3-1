using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using Rilisoft;
using UnityEngine;

// Token: 0x0200070B RID: 1803
internal sealed class ProfileController : MonoBehaviour
{
	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06003EA0 RID: 16032 RVA: 0x0014FC18 File Offset: 0x0014DE18
	// (remove) Token: 0x06003EA1 RID: 16033 RVA: 0x0014FC38 File Offset: 0x0014DE38
	public event EventHandler<ProfileView.InputEventArgs> NicknameInput
	{
		add
		{
			if (this.profileView != null)
			{
				this.profileView.NicknameInput += value;
			}
		}
		remove
		{
			if (this.profileView != null)
			{
				this.profileView.NicknameInput -= value;
			}
		}
	}

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06003EA2 RID: 16034 RVA: 0x0014FC58 File Offset: 0x0014DE58
	// (remove) Token: 0x06003EA3 RID: 16035 RVA: 0x0014FC9C File Offset: 0x0014DE9C
	public event EventHandler BackRequested
	{
		add
		{
			if (this.profileView != null)
			{
				this.profileView.BackButtonPressed += value;
			}
			this.EscapePressed = (EventHandler)Delegate.Combine(this.EscapePressed, value);
		}
		remove
		{
			if (this.profileView != null)
			{
				this.profileView.BackButtonPressed -= value;
			}
			this.EscapePressed = (EventHandler)Delegate.Remove(this.EscapePressed, value);
		}
	}

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06003EA4 RID: 16036 RVA: 0x0014FCE0 File Offset: 0x0014DEE0
	// (remove) Token: 0x06003EA5 RID: 16037 RVA: 0x0014FCFC File Offset: 0x0014DEFC
	private event EventHandler EscapePressed;

	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x06003EA6 RID: 16038 RVA: 0x0014FD18 File Offset: 0x0014DF18
	public Camera Camera3D
	{
		get
		{
			return this._camera3D;
		}
	}

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x06003EA7 RID: 16039 RVA: 0x0014FD20 File Offset: 0x0014DF20
	public static ProfileController Instance
	{
		get
		{
			return ProfileController._instance;
		}
	}

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x06003EA8 RID: 16040 RVA: 0x0014FD28 File Offset: 0x0014DF28
	// (set) Token: 0x06003EA9 RID: 16041 RVA: 0x0014FD30 File Offset: 0x0014DF30
	public string DesiredWeaponTag { get; set; }

	// Token: 0x06003EAA RID: 16042 RVA: 0x0014FD3C File Offset: 0x0014DF3C
	public void HandleBankButton()
	{
		if (BankController.Instance != null)
		{
			EventHandler handleBackFromBank = null;
			handleBackFromBank = delegate(object sender, EventArgs e)
			{
				BankController.Instance.BackRequested -= handleBackFromBank;
				BankController.Instance.InterfaceEnabled = false;
				this.InterfaceEnabled = true;
			};
			BankController.Instance.BackRequested += handleBackFromBank;
			BankController.Instance.InterfaceEnabled = true;
			this.InterfaceEnabled = false;
		}
		else
		{
			Debug.LogWarning("BankController.Instance == null");
		}
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0014FDB0 File Offset: 0x0014DFB0
	public void HandleAchievementsButton()
	{
		if (Application.isEditor)
		{
			Debug.Log("[Achievements] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (!Application.isEditor)
			{
				if (Social.localUser.authenticated)
				{
					Social.ShowAchievementsUI();
				}
				else
				{
					GameCenterSingleton.Instance.updateGameCenter();
				}
			}
			break;
		case RuntimePlatform.Android:
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Social.ShowAchievementsUI();
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AGSAchievementsClient.ShowAchievementsOverlay();
			}
			break;
		}
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0014FE58 File Offset: 0x0014E058
	public void HandleLeaderboardsButton()
	{
		if (Application.isEditor)
		{
			Debug.Log("[Leaderboards] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (!Application.isEditor)
			{
				if (Social.localUser.authenticated)
				{
					Social.ShowLeaderboardUI();
				}
				else
				{
					GameCenterSingleton.Instance.updateGameCenter();
				}
			}
			break;
		case RuntimePlatform.Android:
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Social.ShowLeaderboardUI();
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AGSLeaderboardsClient.ShowLeaderboardsOverlay();
			}
			break;
		}
	}

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x06003EAD RID: 16045 RVA: 0x0014FF00 File Offset: 0x0014E100
	public static int CurOrderCup
	{
		get
		{
			int currentLevel = ExperienceController.sharedController.currentLevel;
			for (int i = 0; i < ExpController.LevelsForTiers.Length; i++)
			{
				if (currentLevel >= ProfileController.MinLevelTir(i) && currentLevel <= ProfileController.MaxLevelTir(i))
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0014FF4C File Offset: 0x0014E14C
	public static int MinLevelTir(int curTir)
	{
		if (curTir >= 0 && curTir < ExpController.LevelsForTiers.Length)
		{
			return ExpController.LevelsForTiers[curTir];
		}
		return -1;
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x0014FF6C File Offset: 0x0014E16C
	public static int MaxLevelTir(int curTir)
	{
		if (curTir < 0 || curTir >= ExpController.LevelsForTiers.Length)
		{
			return -1;
		}
		if (curTir == ExpController.LevelsForTiers.Length - 1)
		{
			return 31;
		}
		return ExpController.LevelsForTiers[curTir + 1];
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x0014FFA0 File Offset: 0x0014E1A0
	public static float GetPerFillProgress(int order, int lev)
	{
		float result;
		if (order < ExpController.LevelsForTiers.Length)
		{
			int num = ProfileController.MinLevelTir(order);
			int num2 = ProfileController.MaxLevelTir(order);
			float num3 = (float)(lev - num);
			float num4 = (float)(num2 - num);
			if (num3 > 0f)
			{
				result = num3 / num4;
			}
			else
			{
				result = 0f;
			}
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06003EB1 RID: 16049 RVA: 0x00150008 File Offset: 0x0014E208
	// (set) Token: 0x06003EB2 RID: 16050 RVA: 0x00150054 File Offset: 0x0014E254
	public bool InterfaceEnabled
	{
		get
		{
			return this.profileView != null && this.profileView.interfaceHolder != null && this.profileView.interfaceHolder.gameObject.activeInHierarchy;
		}
		private set
		{
			if (this.profileView != null && this.profileView.interfaceHolder != null)
			{
				this.profileView.interfaceHolder.gameObject.SetActive(value);
				if (value)
				{
					this.Refresh(true);
					if (ExperienceController.sharedController != null && ExpController.Instance != null)
					{
						ExperienceController.sharedController.isShowRanks = true;
						ExpController.Instance.InterfaceEnabled = true;
					}
					if (this._backSubscription != null)
					{
						this._backSubscription.Dispose();
					}
					this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Profile Controller");
				}
				else
				{
					this.DesiredWeaponTag = string.Empty;
					if (this._backSubscription != null)
					{
						this._backSubscription.Dispose();
						this._backSubscription = null;
					}
				}
				FreeAwardShowHandler.CheckShowChest(value);
			}
		}
	}

	// Token: 0x06003EB3 RID: 16051 RVA: 0x0015014C File Offset: 0x0014E34C
	public void ShowInterface(params Action[] exitCallbacks)
	{
		FriendsController.sharedController.GetOurWins();
		this.InterfaceEnabled = true;
		this._exitCallbacks = (exitCallbacks ?? new Action[0]);
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x00150174 File Offset: 0x0014E374
	public void SetStaticticTab(ProfileStatTabType tabType)
	{
		this._statsTabButtonsController.BtnClicked(tabType.ToString(), false);
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x00150190 File Offset: 0x0014E390
	private void Awake()
	{
		ProfileController._instance = this;
	}

	// Token: 0x06003EB6 RID: 16054 RVA: 0x00150198 File Offset: 0x0014E398
	private void Start()
	{
		this.BackRequested += this.HandleBackRequest;
		if (this.profileView != null)
		{
			this.profileView.nicknameInput.defaultText = ProfileController.defaultPlayerName;
			UIInputRilisoft nicknameInput = this.profileView.nicknameInput;
			nicknameInput.onFocus = (UIInputRilisoft.OnFocus)Delegate.Combine(nicknameInput.onFocus, new UIInputRilisoft.OnFocus(this.OnFocusNickname));
			UIInputRilisoft nicknameInput2 = this.profileView.nicknameInput;
			nicknameInput2.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Combine(nicknameInput2.onFocusLost, new UIInputRilisoft.OnFocusLost(this.onFocusLostNickname));
		}
		if (this.profileView != null)
		{
			this.profileView.Nickname = ProfileController.GetPlayerNameOrDefault();
			this.profileView.NicknameInput += this.HandleNicknameInput;
			this._initialLocalRotation = this.profileView.characterView.character.localRotation;
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.IPhonePlayer:
				this.UpdateButton(this.profileView.achievementsButton, "gamecntr");
				this.UpdateButton(this.profileView.leaderboardsButton, "gamecntr");
				goto IL_1E5;
			case RuntimePlatform.Android:
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					this.UpdateButton(this.profileView.achievementsButton, "google");
					this.UpdateButton(this.profileView.leaderboardsButton, "google");
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					this.UpdateButton(this.profileView.achievementsButton, "amazon");
					this.UpdateButton(this.profileView.leaderboardsButton, "amazon");
				}
				else
				{
					this.profileView.achievementsButton.gameObject.SetActive(false);
				}
				goto IL_1E5;
			}
			this.profileView.achievementsButton.gameObject.SetActive(false);
		}
		IL_1E5:
		this.InterfaceEnabled = false;
		FriendsController.OurInfoUpdated += this.HandleOurInfoUpdated;
	}

	// Token: 0x06003EB7 RID: 16055 RVA: 0x001503A4 File Offset: 0x0014E5A4
	private void HandleOurInfoUpdated()
	{
		if (this.InterfaceEnabled)
		{
			this.Refresh(false);
		}
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x001503B8 File Offset: 0x0014E5B8
	private void UpdateButton(UIButton button, string spriteName)
	{
		if (button == null)
		{
			return;
		}
		button.normalSprite = spriteName;
		button.pressedSprite = spriteName + "_n";
		button.hoverSprite = spriteName;
		button.disabledSprite = spriteName;
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x001503F0 File Offset: 0x0014E5F0
	private void OnDestroy()
	{
		FriendsController.OurInfoUpdated -= this.HandleOurInfoUpdated;
		if (this.profileView != null)
		{
			this.profileView.NicknameInput -= this.HandleNicknameInput;
			UIInputRilisoft nicknameInput = this.profileView.nicknameInput;
			nicknameInput.onFocus = (UIInputRilisoft.OnFocus)Delegate.Remove(nicknameInput.onFocus, new UIInputRilisoft.OnFocus(this.OnFocusNickname));
			UIInputRilisoft nicknameInput2 = this.profileView.nicknameInput;
			nicknameInput2.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Remove(nicknameInput2.onFocusLost, new UIInputRilisoft.OnFocusLost(this.onFocusLostNickname));
		}
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x00150490 File Offset: 0x0014E690
	private void Refresh(bool updateWeapon = true)
	{
		if (this.profileView != null)
		{
			this.profileView.Nickname = ProfileController.GetPlayerNameOrDefault();
			Dictionary<string, object> dictionary = (!(FriendsController.sharedController != null) || FriendsController.sharedController.ourInfo == null || !FriendsController.sharedController.ourInfo.ContainsKey("wincount") || FriendsController.sharedController.ourInfo["wincount"] == null) ? null : (FriendsController.sharedController.ourInfo["wincount"] as Dictionary<string, object>);
			this.profileView.CheckBtnCopy();
			this.profileView.DeathmatchWinCount = Storager.getInt(Defs.RatingDeathmatch, false).ToString();
			this.profileView.TeamBattleWinCount = Storager.getInt(Defs.RatingTeamBattle, false).ToString();
			this.profileView.DeadlyGamesWinCount = Storager.getInt(Defs.RatingHunger, false).ToString();
			this.profileView.FlagCaptureWinCount = Storager.getInt(Defs.RatingFlag, false).ToString();
			this.profileView.CapturePointWinCount = Storager.getInt(Defs.RatingCapturePoint, false).ToString();
			this.profileView.DuelWinCount = Storager.getInt(Defs.RatingDuel, false).ToString();
			this.profileView.TotalWinCount = (Storager.getInt(Defs.RatingDeathmatch, false) + Storager.getInt(Defs.RatingTeamBattle, false) + Storager.getInt(Defs.RatingHunger, false) + Storager.getInt(Defs.RatingFlag, false) + Storager.getInt(Defs.RatingCapturePoint, false) + Storager.getInt(Defs.RatingDuel, false)).ToString();
			this.profileView.PixelgunFriendsID = ((!(FriendsController.sharedController != null) || FriendsController.sharedController.id == null) ? string.Empty : FriendsController.sharedController.id);
			object obj;
			this.profileView.TotalWeeklyWinCount = ((dictionary == null || !dictionary.TryGetValue("weekly", out obj)) ? 0L : ((long)obj)).ToString();
			this.profileView.CoopTimeSurvivalPointCount = Storager.getInt(Defs.COOPScore, false).ToString();
			this.profileView.GameTotalKills = ProfileController.countGameTotalKills.Value.ToString();
			float num;
			if (ProfileController.countGameTotalDeaths.Value == 0)
			{
				num = (float)ProfileController.countGameTotalKills.Value;
			}
			else
			{
				num = (float)ProfileController.countGameTotalKills.Value / (1f * (float)ProfileController.countGameTotalDeaths.Value);
			}
			num = (float)Math.Round((double)num, 2);
			this.profileView.GameKillrate = num.ToString();
			float num2 = 0f;
			if (ProfileController.countGameTotalHit.Value != 0)
			{
				num2 = (float)(100 * ProfileController.countGameTotalHit.Value) / (1f * (float)ProfileController.countGameTotalShoot.Value);
			}
			num2 = (float)Math.Round((double)num2, 2);
			this.profileView.GameAccuracy = num2.ToString();
			this.profileView.GameLikes = ProfileController.countLikes.Value.ToString();
			this.profileView.WaveCountLabel = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0).ToString();
			this.profileView.KilledCountLabel = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0).ToString();
			this.profileView.SurvivalScoreLabel = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0).ToString();
			this.profileView.Box1StarsLabel = this.InitializeStarCountLabelForBox(0);
			this.profileView.Box2StarsLabel = this.InitializeStarCountLabelForBox(1);
			this.profileView.Box3StarsLabel = this.InitializeStarCountLabelForBox(2);
			this.profileView.SecretCoinsLabel = this.InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Coin);
			this.profileView.SecretGemsLabel = this.InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Gem);
			if (updateWeapon && WeaponManager.sharedManager != null)
			{
				Weapon[] array = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToArray<Weapon>();
				if (array.Length > 0)
				{
					string desiredPrefabName = null;
					if (!string.IsNullOrEmpty(this.DesiredWeaponTag))
					{
						ItemRecord byTag = ItemDb.GetByTag(this.DesiredWeaponTag);
						if (byTag != null)
						{
							desiredPrefabName = byTag.PrefabName;
						}
					}
					if (!string.IsNullOrEmpty(desiredPrefabName) && array.Any((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", string.Empty) == desiredPrefabName))
					{
						this.profileView.SetWeaponAndSkin(this.DesiredWeaponTag, false);
					}
					else
					{
						System.Random random = new System.Random(Time.frameCount);
						int num3 = random.Next(array.Length);
						Weapon weapon = array[num3];
						this.profileView.SetWeaponAndSkin(ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag, false);
					}
				}
				else
				{
					this.profileView.SetWeaponAndSkin("Knife", false);
				}
			}
			if (Storager.getString(Defs.HatEquppedSN, false) != Defs.HatNoneEqupped)
			{
				this.profileView.UpdateHat(Storager.getString(Defs.HatEquppedSN, false));
			}
			else
			{
				this.profileView.RemoveHat();
			}
			if (Storager.getString("MaskEquippedSN", false) != "MaskNoneEquipped")
			{
				this.profileView.UpdateMask(Storager.getString("MaskEquippedSN", false));
			}
			else
			{
				this.profileView.RemoveMask();
			}
			if (Storager.getString(Defs.BootsEquppedSN, false) != Defs.BootsNoneEqupped)
			{
				this.profileView.UpdateBoots(Storager.getString(Defs.BootsEquppedSN, false));
			}
			else
			{
				this.profileView.RemoveBoots();
			}
			if (Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNoneEqupped)
			{
				this.profileView.UpdateArmor(Storager.getString(Defs.ArmorNewEquppedSN, false));
			}
			else
			{
				this.profileView.RemoveArmor();
			}
			if (Storager.getString(Defs.CapeEquppedSN, false) != Defs.CapeNoneEqupped)
			{
				this.profileView.UpdateCape(Storager.getString(Defs.CapeEquppedSN, false));
			}
			else
			{
				this.profileView.RemoveCape();
			}
			if (FriendsController.sharedController != null)
			{
				this.profileView.SetClanLogo(FriendsController.sharedController.clanLogo ?? string.Empty);
			}
			else
			{
				this.profileView.SetClanLogo(string.Empty);
			}
		}
		this._idleTimeStart = Time.realtimeSinceStartup;
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x00150B3C File Offset: 0x0014ED3C
	private void OnEnable()
	{
		this.Refresh(true);
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x00150B48 File Offset: 0x0014ED48
	private void Update()
	{
		EventHandler escapePressed = this.EscapePressed;
		if (this._escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			this._escapePressed = false;
		}
		if (Time.realtimeSinceStartup - this._idleTimeStart > ShopNGUIController.IdleTimeoutPers)
		{
			this.ReturnCharacterToInitialState();
		}
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x00150B9C File Offset: 0x0014ED9C
	private void LateUpdate()
	{
		if (this.profileView != null && this.InterfaceEnabled && !HOTween.IsTweening(this.profileView.characterView.character))
		{
			float rotationRateForCharacterInMenues = RilisoftRotator.RotationRateForCharacterInMenues;
			if (this._touchZone == null)
			{
				this._touchZone = new Rect?(new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height));
			}
			RilisoftRotator.RotateCharacter(this.profileView.characterView.character, rotationRateForCharacterInMenues, this._touchZone.Value, ref this._idleTimeStart, ref this._lastTime, null);
		}
	}

	// Token: 0x06003EBE RID: 16062 RVA: 0x00150C5C File Offset: 0x0014EE5C
	private void HandleEscape()
	{
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (InfoWindowController.IsActive)
		{
			InfoWindowController.HideCurrentWindow();
			return;
		}
		this._escapePressed = true;
	}

	// Token: 0x06003EBF RID: 16063 RVA: 0x00150CA0 File Offset: 0x0014EEA0
	private void ReturnCharacterToInitialState()
	{
		if (this.profileView == null)
		{
			Debug.LogWarning("profileView == null");
			return;
		}
		int num = HOTween.Kill(this.profileView.characterView.character);
		if (num > 0 && Application.isEditor)
		{
			Debug.LogWarning("Tweens killed: " + num);
		}
		this._idleTimeStart = Time.realtimeSinceStartup;
		HOTween.To(this.profileView.characterView.character, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(this._initialLocalRotation)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(delegate()
		{
			this._idleTimeStart = Time.realtimeSinceStartup;
		}));
	}

	// Token: 0x06003EC0 RID: 16064 RVA: 0x00150D64 File Offset: 0x0014EF64
	private string InitializeStarCountLabelForBox(int boxIndex)
	{
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			Debug.LogWarning("Box index is out of range:    " + boxIndex);
			return string.Empty;
		}
		LevelBox levelBox = LevelBox.campaignBoxes[boxIndex];
		List<CampaignLevel> levels = levelBox.levels;
		Dictionary<string, int> dictionary;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out dictionary))
		{
			Debug.LogWarning("ProfileController: Box not found in dictionary: " + levelBox.name);
			dictionary = new Dictionary<string, int>();
		}
		int num = 0;
		for (int num2 = 0; num2 != levels.Count; num2++)
		{
			string sceneName = levels[num2].sceneName;
			int num3 = 0;
			dictionary.TryGetValue(sceneName, out num3);
			num += num3;
		}
		return num + '/' + levels.Count * 3;
	}

	// Token: 0x06003EC1 RID: 16065 RVA: 0x00150E40 File Offset: 0x0014F040
	private string InitializeSecretBonusCountLabel(VirtualCurrencyBonusType bonusType)
	{
		List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(bonusType);
		int num = Math.Min(20, levelsWhereGotBonus.Count);
		return num + '/' + 20;
	}

	// Token: 0x06003EC2 RID: 16066 RVA: 0x00150E7C File Offset: 0x0014F07C
	private void HandleNicknameInput(object sender, ProfileView.InputEventArgs e)
	{
		this.SaveNamePlayer(e.Input);
	}

	// Token: 0x06003EC3 RID: 16067 RVA: 0x00150E8C File Offset: 0x0014F08C
	public void SaveNamePlayer(string namePlayer)
	{
		namePlayer = FilterBadWorld.FilterString(namePlayer);
		if (string.IsNullOrEmpty(namePlayer) || namePlayer.Trim() == string.Empty)
		{
			namePlayer = ProfileController.defaultPlayerName;
			this.profileView.nicknameInput.label.text = namePlayer;
		}
		if (Application.isEditor)
		{
			Debug.Log("Saving new name:    " + namePlayer);
		}
		PlayerPrefs.SetString("NamePlayer", namePlayer);
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
		{
			NetworkStartTable component = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>();
			if (component != null)
			{
				component.SetNewNick();
			}
		}
		this._dirty = true;
		this._isNicknameSubmit = true;
	}

	// Token: 0x06003EC4 RID: 16068 RVA: 0x00150F58 File Offset: 0x0014F158
	private void HandleBackRequest(object sender, EventArgs e)
	{
		if (this._dirty && FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendOurData(false);
			this._dirty = false;
		}
		Action action = this._exitCallbacks.FirstOrDefault<Action>();
		if (action != null)
		{
			action();
		}
		base.StartCoroutine(this.ExitCallbacksCoroutine());
	}

	// Token: 0x06003EC5 RID: 16069 RVA: 0x00150FB8 File Offset: 0x0014F1B8
	private IEnumerator ExitCallbacksCoroutine()
	{
		for (int i = 1; i < this._exitCallbacks.Length; i++)
		{
			Action exitCallback = this._exitCallbacks[i];
			exitCallback();
			yield return null;
		}
		this._exitCallbacks = new Action[0];
		this.InterfaceEnabled = false;
		yield break;
	}

	// Token: 0x06003EC6 RID: 16070 RVA: 0x00150FD4 File Offset: 0x0014F1D4
	private void OnFocusNickname()
	{
		this._isNicknameSubmit = false;
	}

	// Token: 0x06003EC7 RID: 16071 RVA: 0x00150FE0 File Offset: 0x0014F1E0
	private void onFocusLostNickname()
	{
		if (!this._isNicknameSubmit && this.profileView != null)
		{
			this.profileView.nicknameInput.value = ProfileController.GetPlayerNameOrDefault();
		}
	}

	// Token: 0x06003EC8 RID: 16072 RVA: 0x00151014 File Offset: 0x0014F214
	public static void ResaveStatisticToKeychain()
	{
		Storager.setInt("keyGameTotalKills", ProfileController.countGameTotalKills.Value, false);
		Storager.setInt("keyGameDeath", ProfileController.countGameTotalDeaths.Value, false);
		Storager.setInt("keyGameShoot", ProfileController.countGameTotalShoot.Value, false);
		Storager.setInt("keyGameHit", ProfileController.countGameTotalHit.Value, false);
		Storager.setInt("keyCountLikes", ProfileController.countLikes.Value, false);
	}

	// Token: 0x06003EC9 RID: 16073 RVA: 0x0015108C File Offset: 0x0014F28C
	public static void LoadStatisticFromKeychain()
	{
		if (!Storager.hasKey("keyGameTotalKills"))
		{
			Storager.setInt("keyGameTotalKills", 0, false);
		}
		if (!Storager.hasKey("keyGameDeath"))
		{
			Storager.setInt("keyGameDeath", 0, false);
		}
		if (!Storager.hasKey("keyGameShoot"))
		{
			Storager.setInt("keyGameShoot", 0, false);
		}
		if (!Storager.hasKey("keyGameHit"))
		{
			Storager.setInt("keyGameHit", 0, false);
		}
		ProfileController.countGameTotalKills.Value = Storager.getInt("keyGameTotalKills", false);
		ProfileController.countGameTotalDeaths.Value = Storager.getInt("keyGameDeath", false);
		ProfileController.countGameTotalShoot.Value = Storager.getInt("keyGameShoot", false);
		ProfileController.countGameTotalHit.Value = Storager.getInt("keyGameHit", false);
		ProfileController.countLikes.Value = Storager.getInt("keyCountLikes", false);
	}

	// Token: 0x06003ECA RID: 16074 RVA: 0x00151170 File Offset: 0x0014F370
	public static void OnGameTotalKills()
	{
		ProfileController.countGameTotalKills.Value = ProfileController.countGameTotalKills.Value + 1;
	}

	// Token: 0x06003ECB RID: 16075 RVA: 0x00151184 File Offset: 0x0014F384
	public static void OnGameDeath()
	{
		ProfileController.countGameTotalDeaths.Value = ProfileController.countGameTotalDeaths.Value + 1;
	}

	// Token: 0x06003ECC RID: 16076 RVA: 0x00151198 File Offset: 0x0014F398
	public static void OnGameShoot()
	{
		ProfileController.countGameTotalShoot.Value = ProfileController.countGameTotalShoot.Value + 1;
	}

	// Token: 0x06003ECD RID: 16077 RVA: 0x001511AC File Offset: 0x0014F3AC
	public static void OnGameHit()
	{
		ProfileController.countGameTotalHit.Value = ProfileController.countGameTotalHit.Value + 1;
	}

	// Token: 0x06003ECE RID: 16078 RVA: 0x001511C0 File Offset: 0x0014F3C0
	public static void OnGetLike()
	{
		ProfileController.countLikes.Value = ProfileController.countLikes.Value + 1;
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06003ECF RID: 16079 RVA: 0x001511D4 File Offset: 0x0014F3D4
	public static string defaultPlayerName
	{
		get
		{
			if (!PlayerPrefs.HasKey("keyChooseDefaultName"))
			{
				ProfileController._defaultPlayerName = ProfileController.GetRandomName();
				PlayerPrefs.SetString("keyChooseDefaultName", ProfileController._defaultPlayerName);
			}
			if (ProfileController._defaultPlayerName == null)
			{
				ProfileController._defaultPlayerName = PlayerPrefs.GetString("keyChooseDefaultName");
			}
			return ProfileController._defaultPlayerName;
		}
	}

	// Token: 0x06003ED0 RID: 16080 RVA: 0x00151228 File Offset: 0x0014F428
	private static string GetRandomName()
	{
		if (ProfileController.DefaultKeyNames != null && ProfileController.DefaultKeyNames.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, ProfileController.DefaultKeyNames.Length);
			return LocalizationStore.Get(ProfileController.DefaultKeyNames[num]);
		}
		return "Player";
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0015126C File Offset: 0x0014F46C
	public static string GetPlayerNameOrDefault()
	{
		if (PlayerPrefs.HasKey("NamePlayer"))
		{
			string text = PlayerPrefs.GetString("NamePlayer");
			if (text != null)
			{
				if (text.Length > 20)
				{
					text = text.Substring(0, 20);
				}
				return text;
			}
		}
		string text2 = PlayerPrefs.GetString("SocialName", string.Empty);
		if (Social.localUser != null && Social.localUser.authenticated && !string.IsNullOrEmpty(Social.localUser.userName))
		{
			if (!text2.Equals(Social.localUser.userName))
			{
				text2 = Social.localUser.userName;
				PlayerPrefs.SetString("SocialName", text2);
			}
			return text2;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (GameCircleSocial.Instance.localUser.authenticated)
			{
				if (!string.IsNullOrEmpty(GameCircleSocial.Instance.localUser.userName))
				{
					if (!text2.Equals(GameCircleSocial.Instance.localUser.userName))
					{
						text2 = GameCircleSocial.Instance.localUser.userName;
						PlayerPrefs.SetString("SocialName", text2);
					}
					return text2;
				}
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			return text2;
		}
		return ProfileController.defaultPlayerName;
	}

	// Token: 0x04002E44 RID: 11844
	public const string keyChooseDefaultName = "keyChooseDefaultName";

	// Token: 0x04002E45 RID: 11845
	private const string NicknameKey = "NamePlayer";

	// Token: 0x04002E46 RID: 11846
	public static SaltedInt countGameTotalKills = new SaltedInt(640178770);

	// Token: 0x04002E47 RID: 11847
	public static SaltedInt countGameTotalDeaths = new SaltedInt(371743314);

	// Token: 0x04002E48 RID: 11848
	public static SaltedInt countGameTotalShoot = new SaltedInt(623401554);

	// Token: 0x04002E49 RID: 11849
	public static SaltedInt countGameTotalHit = new SaltedInt(606624338);

	// Token: 0x04002E4A RID: 11850
	public static SaltedInt countLikes = new SaltedInt(606624338);

	// Token: 0x04002E4B RID: 11851
	public ProfileView profileView;

	// Token: 0x04002E4C RID: 11852
	[SerializeField]
	private Camera _camera3D;

	// Token: 0x04002E4D RID: 11853
	public static string[] DefaultKeyNames = new string[]
	{
		"Key_2020",
		"Key_2021",
		"Key_2022",
		"Key_2023",
		"Key_2024",
		"Key_2025",
		"Key_2026",
		"Key_2027",
		"Key_2028",
		"Key_2029",
		"Key_2030",
		"Key_2031",
		"Key_2032",
		"Key_2033",
		"Key_2034",
		"Key_2035",
		"Key_2036",
		"Key_2037",
		"Key_2038"
	};

	// Token: 0x04002E4E RID: 11854
	private static string _defaultPlayerName = null;

	// Token: 0x04002E4F RID: 11855
	private static ProfileController _instance;

	// Token: 0x04002E50 RID: 11856
	[SerializeField]
	private CategoryButtonsController _statsTabButtonsController;

	// Token: 0x04002E51 RID: 11857
	private IDisposable _backSubscription;

	// Token: 0x04002E52 RID: 11858
	private bool _dirty;

	// Token: 0x04002E53 RID: 11859
	private bool _escapePressed;

	// Token: 0x04002E54 RID: 11860
	private Action[] _exitCallbacks = new Action[0];

	// Token: 0x04002E55 RID: 11861
	private float _idleTimeStart;

	// Token: 0x04002E56 RID: 11862
	private Quaternion _initialLocalRotation;

	// Token: 0x04002E57 RID: 11863
	private float _lastTime;

	// Token: 0x04002E58 RID: 11864
	private Rect? _touchZone;

	// Token: 0x04002E59 RID: 11865
	private Color? _storedAmbientLight;

	// Token: 0x04002E5A RID: 11866
	private bool _isNicknameSubmit;
}
