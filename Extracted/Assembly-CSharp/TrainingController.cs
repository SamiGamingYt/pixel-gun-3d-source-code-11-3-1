using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Rilisoft;
using RilisoftBot;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000868 RID: 2152
public sealed class TrainingController : MonoBehaviour
{
	// Token: 0x06004DAA RID: 19882 RVA: 0x001C1284 File Offset: 0x001BF484
	public TrainingController()
	{
		this._directionArrow = new Lazy<PlayerArrowToPortalController>(delegate()
		{
			GameObject gameObject = GameObject.FindWithTag("Player");
			if (gameObject == null)
			{
				return null;
			}
			return gameObject.GetComponent<PlayerArrowToPortalController>();
		});
	}

	// Token: 0x06004DAB RID: 19883 RVA: 0x001C1300 File Offset: 0x001BF500
	static TrainingController()
	{
		TrainingController.stepTrainingList.Add("TapToMove", TrainingState.TapToMove);
		TrainingController.stepTrainingList.Add("GetTheGun", TrainingState.GetTheGun);
		TrainingController.stepTrainingList.Add("WellDone", TrainingState.WellDone);
		TrainingController.stepTrainingList.Add("Shop", TrainingState.Shop);
		TrainingController.stepTrainingList.Add("TapToSelectWeapon", TrainingState.TapToSelectWeapon);
		TrainingController.stepTrainingList.Add("TapToShoot", TrainingState.TapToShoot);
		TrainingController.stepTrainingList.Add("TapToThrowGrenade", TrainingState.TapToThrowGrenade);
		TrainingController.stepTrainingList.Add("KillZombi", TrainingState.KillZombie);
		TrainingController.stepTrainingList.Add("GoToPortal", TrainingState.GoToPortal);
	}

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06004DAC RID: 19884 RVA: 0x001C142C File Offset: 0x001BF62C
	// (remove) Token: 0x06004DAD RID: 19885 RVA: 0x001C1444 File Offset: 0x001BF644
	public static event Action onChangeTraining;

	// Token: 0x17000CB3 RID: 3251
	// (get) Token: 0x06004DAE RID: 19886 RVA: 0x001C145C File Offset: 0x001BF65C
	public static bool TrainingCompleted
	{
		get
		{
			if (TrainingController.cachedTrainingComplete)
			{
				return true;
			}
			if (!TrainingController._trainingCompletedInitialized)
			{
				if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) == 0 && PlayerPrefs.GetInt(Defs.TrainingCompleted_4_4_Sett, 0) == 1)
				{
					if (Defs.IsDeveloperBuild)
					{
						Debug.Log("Trying to set TrainingCompleted flag...");
					}
					TrainingController.OnGetProgress();
				}
				if (TrainingController.onChangeTraining != null)
				{
					TrainingController.onChangeTraining();
				}
				TrainingController._trainingCompletedInitialized = true;
			}
			TrainingController.cachedTrainingComplete = (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) > 0 || TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.FirstMatchCompleted);
			return TrainingController.cachedTrainingComplete;
		}
	}

	// Token: 0x17000CB4 RID: 3252
	// (get) Token: 0x06004DAF RID: 19887 RVA: 0x001C14FC File Offset: 0x001BF6FC
	// (set) Token: 0x06004DB0 RID: 19888 RVA: 0x001C1524 File Offset: 0x001BF724
	public static TrainingController.NewTrainingCompletedStage CompletedTrainingStage
	{
		get
		{
			if (!TrainingController._comletedTrainingStageInitialized)
			{
				TrainingController._comletedTrainingStageInitialized = true;
				TrainingController._completedTrainingStage = (TrainingController.NewTrainingCompletedStage)Storager.getInt("TrainingController.NewTrainingStageHolderKey", false);
			}
			return TrainingController._completedTrainingStage;
		}
		set
		{
			TrainingController._comletedTrainingStageInitialized = true;
			if (TrainingController._completedTrainingStage != value)
			{
				TrainingController._completedTrainingStage = value;
				Storager.setInt("TrainingController.NewTrainingStageHolderKey", (int)TrainingController._completedTrainingStage, false);
				if (TrainingController._completedTrainingStage == TrainingController.NewTrainingCompletedStage.FirstMatchCompleted)
				{
					Action action = TrainingController.onChangeTraining;
					if (action != null)
					{
						action();
					}
				}
			}
		}
	}

	// Token: 0x17000CB5 RID: 3253
	// (get) Token: 0x06004DB1 RID: 19889 RVA: 0x001C1578 File Offset: 0x001BF778
	public Vector3 PlayerDesiredPosition
	{
		get
		{
			return (!(this.playerTransform != null)) ? TrainingController._playerDefaultPosition : this.playerTransform.position;
		}
	}

	// Token: 0x17000CB6 RID: 3254
	// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x001C15AC File Offset: 0x001BF7AC
	public static Vector3 PlayerDefaultPosition
	{
		get
		{
			return TrainingController._playerDefaultPosition;
		}
	}

	// Token: 0x17000CB7 RID: 3255
	// (get) Token: 0x06004DB3 RID: 19891 RVA: 0x001C15B4 File Offset: 0x001BF7B4
	// (set) Token: 0x06004DB4 RID: 19892 RVA: 0x001C15BC File Offset: 0x001BF7BC
	public static bool? TrainingCompletedFlagForLogging
	{
		get
		{
			return TrainingController._trainingCompleted;
		}
		set
		{
			TrainingController._trainingCompleted = value;
		}
	}

	// Token: 0x06004DB5 RID: 19893 RVA: 0x001C15C4 File Offset: 0x001BF7C4
	public static void OnGetProgress()
	{
		PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(2.0)).ToString("s", CultureInfo.InvariantCulture));
		if (ShopNGUIController.NoviceArmorAvailable)
		{
			ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
			ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
		}
		Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
		AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Get_Progress, 0);
		Storager.setInt(Defs.TrainingCompleted_4_4_Sett, 1, false);
		if (!Defs.isABTestBalansNoneSkip)
		{
			FriendsController.ResetABTestsBalans();
		}
		FriendsController.ResetABTestAdvert();
		foreach (ABTestBase abtestBase in ABTestController.currentABTests)
		{
			abtestBase.ResetABTest();
		}
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.OnGetProgress();
		}
		else
		{
			KillRateCheck.instance.OnGetProgress();
		}
	}

	// Token: 0x17000CB8 RID: 3256
	// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x001C16DC File Offset: 0x001BF8DC
	public static bool FireButtonEnabled
	{
		get
		{
			return TrainingController.stepTraining >= TrainingState.KillZombie;
		}
	}

	// Token: 0x06004DB7 RID: 19895 RVA: 0x001C16EC File Offset: 0x001BF8EC
	public static void SkipTraining()
	{
		TrainingController.oldStepTraning = TrainingController.stepTraining;
		TrainingController.stepTraining = TrainingState.None;
		TrainingController.isPressSkip = true;
		TrainingController.isCanceled = true;
		TrainingController._trainingCompleted = new bool?(false);
		AnalyticsFacade.SendCustomEventToAppsFlyer("Training complete", new Dictionary<string, string>());
	}

	// Token: 0x06004DB8 RID: 19896 RVA: 0x001C1730 File Offset: 0x001BF930
	public static void CancelSkipTraining()
	{
		TrainingController.isCanceled = false;
		TrainingController.isPressSkip = false;
		TrainingController.stepTraining = TrainingController.oldStepTraning;
		TrainingController component = GameObject.FindGameObjectWithTag("TrainingController").GetComponent<TrainingController>();
		if (TrainingController.nextStepAfterSkipTraining)
		{
			TrainingController.nextStepAfterSkipTraining = false;
			component.StartNextStepTraning();
		}
		if (TrainingController.stepAnim == 0)
		{
			component.FirstStep();
		}
		else
		{
			component.NextStepAnim();
		}
	}

	// Token: 0x06004DB9 RID: 19897 RVA: 0x001C1794 File Offset: 0x001BF994
	private void AdjustShootReloadLabel()
	{
		bool flag = PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1;
		if (this.shootReloadOverlay != null && flag)
		{
			this.shootReloadOverlay.transform.localPosition = this.shootReloadOverlay.transform.localPosition - new Vector3(120f, 0f, 0f);
		}
	}

	// Token: 0x06004DBA RID: 19898 RVA: 0x001C1800 File Offset: 0x001BFA00
	private void AdjustJoystickAreaAndFinger()
	{
		float num = (float)((!GlobalGameController.LeftHanded) ? -1 : 1);
		Vector3 vector = new Vector3((float)Defs.JoyStickX * num, (float)Defs.JoyStickY, 0f);
		if (this.dragToMoveOverlay != null)
		{
			this.dragToMoveOverlay.transform.localPosition = vector + new Vector3(30f, 120f, 0f);
		}
		Vector3[] array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (array != null && array.Length > 4)
		{
			vector = array[4];
		}
		if (this.joystickShadowOverlay != null)
		{
			this.joystickShadowOverlay.GetComponent<RectTransform>().anchoredPosition = vector;
		}
		TrainingFinger trainingFinger = (!(this.joystickFingerOverlay == null)) ? this.joystickFingerOverlay.GetComponent<TrainingFinger>() : null;
		if (trainingFinger != null)
		{
			trainingFinger.GetComponent<RectTransform>().anchoredPosition = vector + new Vector3(20f, 20f, 0f);
		}
	}

	// Token: 0x06004DBB RID: 19899 RVA: 0x001C191C File Offset: 0x001BFB1C
	private void AdjustGrenadeLabelAndArrow()
	{
		Vector3 a = Vector3.zero;
		Vector3[] array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (array == null || array.Length < 6)
		{
			float num = (float)((!GlobalGameController.LeftHanded) ? -1 : 1);
			a = new Vector3((float)Defs.GrenadeX * num, (float)Defs.GrenadeY, 0f);
		}
		else
		{
			a = array[5];
		}
		TrainingArrow trainingArrow = (!(this.buyGrenadeArrowOverlay == null)) ? this.buyGrenadeArrowOverlay.GetComponent<TrainingArrow>() : null;
		if (trainingArrow != null)
		{
			trainingArrow.SetAnchoredPosition(a - new Vector3(64f, 0f, 0f));
		}
		TrainingArrow trainingArrow2 = (!(this.throwGrenadeArrowOverlay == null)) ? this.throwGrenadeArrowOverlay.GetComponent<TrainingArrow>() : null;
		if (trainingArrow2 != null)
		{
			trainingArrow2.SetAnchoredPosition(a - new Vector3(90f, -60f, 0f));
		}
		if (this.selectGrenadeOverlay != null)
		{
			this.selectGrenadeOverlay.transform.localPosition = a - new Vector3(120f, 0f, 0f);
		}
		if (this.throwGrenadeOverlay != null)
		{
			this.throwGrenadeOverlay.transform.localPosition = a - new Vector3(400f, -120f, 0f);
		}
	}

	// Token: 0x06004DBC RID: 19900 RVA: 0x001C1AA0 File Offset: 0x001BFCA0
	private IEnumerator Start()
	{
		TrainingController.sharedController = this;
		PlayerArrowToPortalController.PopulateArrowPoolIfEmpty();
		this._overlays = new GameObject[]
		{
			this.swipeToRotateOverlay,
			this.dragToMoveOverlay,
			this.pickupGunOverlay,
			this.wellDoneOverlay,
			this.getCoinOverlay,
			this.enterShopOverlay,
			this.shootReloadOverlay,
			this.selectGrenadeOverlay,
			this.throwGrenadeOverlay,
			this.killZombiesOverlay
		};
		TrainingController.isPause = false;
		this.animTextures = new Texture2D[3];
		TrainingController.stepTraining = TrainingState.None;
		TrainingController.isNextStep = TrainingState.None;
		TrainingController.setNextStepInd = TrainingState.None;
		this.StartNextStepTraning();
		this.coinsPrefab = GameObject.FindGameObjectWithTag("CoinBonus");
		if (this.coinsPrefab != null)
		{
			this.coinsPrefab.SetActive(false);
		}
		PlayerPrefs.SetInt("LogCountMatch", 1);
		while (GameObject.FindGameObjectWithTag("InGameGUI") == null)
		{
			yield return null;
		}
		this.shopButton = GameObject.FindGameObjectWithTag("InGameGUI").GetComponent<InGameGUI>().shopButton.GetComponent<UIButton>();
		InGameGUI.sharedInGameGUI.SetSwipeWeaponPanelVisibility(false);
		this._pauseButton = InGameGUI.sharedInGameGUI.pauseButton;
		if (this._pauseButton != null)
		{
			this._pauseButton.isEnabled = false;
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			if (InGameGUI.sharedInGameGUI.jumpButton != null && this.pickupGunOverlay != null)
			{
				List<UISprite> sprites = new List<UISprite>();
				InGameGUI.sharedInGameGUI.jumpButton.GetComponentsInChildren<UISprite>(true, sprites);
				TrainingBlinking tb = this.pickupGunOverlay.AddComponent<TrainingBlinking>();
				tb.SetSprites(sprites);
			}
			if (InGameGUI.sharedInGameGUI.fireButton != null)
			{
				List<UISprite> sprites2 = new List<UISprite>();
				InGameGUI.sharedInGameGUI.fireButton.GetComponentsInChildren<UISprite>(true, sprites2);
				if (this.killZombiesOverlay != null)
				{
					TrainingBlinking tb2 = this.killZombiesOverlay.AddComponent<TrainingBlinking>();
					tb2.SetSprites(sprites2);
				}
			}
		}
		yield break;
	}

	// Token: 0x06004DBD RID: 19901 RVA: 0x001C1ABC File Offset: 0x001BFCBC
	private void OnDestroy()
	{
		TrainingController.sharedController = null;
		if (this._pauseButton != null)
		{
			this._pauseButton.isEnabled = true;
		}
		this._weaponChangedSubscription.Dispose();
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001C1AF8 File Offset: 0x001BFCF8
	private void HandleWeaponChanged(object sender, EventArgs e)
	{
		if (this._weaponChangingCount > 0 && TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			TrainingController.isNextStep = TrainingState.TapToSelectWeapon;
		}
		this._weaponChangingCount++;
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001C1B28 File Offset: 0x001BFD28
	[Obfuscation(Exclude = true)]
	public void StartNextStepTraning()
	{
		if (TrainingController.isPressSkip)
		{
			TrainingController.nextStepAfterSkipTraining = true;
			return;
		}
		TrainingController.stepTraining++;
		Vector2 zero = Vector2.zero;
		if (TrainingController.stepTraining == TrainingState.SwipeToRotate)
		{
			this.AdjustJoystickAreaAndFinger();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			if (this.enemies != null && this.enemies.Length > 0)
			{
				foreach (GameObject gameObject in this.enemies)
				{
					TrainingEnemy item = gameObject.GetComponent<TrainingEnemy>() ?? gameObject.AddComponent<TrainingEnemy>();
					this._enemies.Add(item);
				}
			}
			else if (this.enemyPrototype != null)
			{
				Behaviour[] array2 = new Behaviour[]
				{
					this.enemyPrototype.GetComponent<BotAiController>(),
					this.enemyPrototype.GetComponent<MeleeBot>(),
					this.enemyPrototype.GetComponent<NavMeshAgent>()
				};
				foreach (Behaviour behaviour in array2)
				{
					if (behaviour != null)
					{
						behaviour.enabled = false;
						UnityEngine.Object.Destroy(behaviour);
					}
				}
				GameObject gameObject2 = new GameObject("DynamicEnemies");
				gameObject2.transform.localPosition = new Vector3(-2f, 0f, 15f);
				int enemiesToKill = GlobalGameController.EnemiesToKill;
				for (int k = 0; k < enemiesToKill; k++)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.enemyPrototype);
					gameObject3.transform.parent = gameObject2.transform;
					Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
					gameObject3.transform.localPosition = new Vector3((float)(3 * k) + insideUnitCircle.x, 0f, insideUnitCircle.y);
					gameObject3.transform.localRotation = Quaternion.AngleAxis(180f + UnityEngine.Random.Range(-60f, 60f), Vector3.up);
					TrainingEnemy item2 = gameObject3.GetComponent<TrainingEnemy>() ?? gameObject3.AddComponent<TrainingEnemy>();
					this._enemies.Add(item2);
				}
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToMove)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Overview, 0);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			for (int num = 0; num != this.animTextures.Length; num++)
			{
				this.animTextures[num] = null;
			}
			if (this.animTextures[0] != null)
			{
				zero = new Vector2(-10f * Defs.Coef, (float)Screen.height - ((float)this.animTextures[0].height - 51f) * Defs.Coef);
			}
		}
		if (TrainingController.stepTraining == TrainingState.GetTheGun)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Move, 0);
			HintController.instance.ShowHintByName("press_jump", 0f);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 0.2f;
			TrainingController.stepAnim = 0;
			Vector3 vector = (!(this.weaponTransform != null)) ? new Vector3(-1.6f, 1.75f, -2.6f) : this.weaponTransform.position;
			if (this.weaponTransform != null)
			{
				UnityEngine.Object.Destroy(this.weaponTransform.gameObject);
			}
			if (this.weapon == null)
			{
				this.weapon = BonusCreator._CreateBonus(WeaponManager.MP5WN, vector);
			}
			else
			{
				this.weapon.transform.position = vector;
			}
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
				this._directionArrow.Value.SetPointOfInterest(this.weapon.transform);
			}
		}
		if (TrainingController.stepTraining == TrainingState.WellDone || TrainingController.stepTraining == TrainingState.WellDoneCoin)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Controls_Jump, 0);
			HintController.instance.HideHintByName("press_jump");
			HintController.instance.ShowHintByName("press_fire", 0f);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 1;
			this.speedAnim = 1f;
			TrainingController.stepAnim = 0;
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
			}
		}
		if (TrainingController.stepTraining == TrainingState.GetTheCoin)
		{
			if (this.coinsPrefab != null)
			{
				this.coinsPrefab.SetActive(true);
				this.coinsPrefab.GetComponent<CoinBonus>().SetPlayer();
			}
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
		}
		bool flag = TrainingController.stepTraining == TrainingState.EnterTheShop;
		if (flag)
		{
			this.isAnimShop = false;
			this.AnimShop();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			InGameGUI.sharedInGameGUI.SetSwipeWeaponPanelVisibility(PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1);
			Player_move_c playerMove = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
			if (playerMove != null)
			{
				playerMove.WeaponChanged += this.HandleWeaponChanged;
				this._weaponChangedSubscription = new ActionDisposable(delegate()
				{
					playerMove.WeaponChanged -= this.HandleWeaponChanged;
					this._weaponChangingCount = 0;
				});
			}
		}
		else
		{
			this._weaponChangedSubscription.Dispose();
		}
		if (TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			this.AdjustShootReloadLabel();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		TrainingState trainingState;
		bool flag2 = TrainingController.stepTrainingList.TryGetValue("SwipeWeapon", out trainingState) && trainingState == TrainingController.stepTraining;
		if (flag2)
		{
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 13;
			this.speedAnim = 0.3f;
			TrainingController.stepAnim = 0;
			for (int num2 = 0; num2 != this.animTextures.Length; num2++)
			{
				this.animTextures[num2] = null;
			}
			if (this.animTextures[0] != null)
			{
				zero = new Vector2((float)Screen.width - (float)this.animTextures[0].width * Defs.Coef, 0f);
			}
		}
		if (TrainingController.stepTraining == TrainingState.KillZombie)
		{
			if (this._enemies.Count > 0)
			{
				foreach (TrainingEnemy trainingEnemy in this._enemies)
				{
					trainingEnemy.WakeUp(UnityEngine.Random.value);
				}
			}
			else
			{
				GameObject.FindGameObjectWithTag("GameController").transform.GetComponent<ZombieCreator>().BeganCreateEnemies();
			}
			InGameGUI.sharedInGameGUI.centerAnhor.SetActive(true);
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 2;
			this.speedAnim = 3f;
			TrainingController.stepAnim = 0;
			if (Application.isEditor)
			{
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
		if (TrainingController.stepTraining == TrainingState.GoToPortal)
		{
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Kill_Enemy, 0);
			if (this._directionArrow.Value != null)
			{
				this._directionArrow.Value.RemovePointOfInterest();
				this._directionArrow.Value.SetPointOfInterest(this.teleportTransform);
			}
			PlayerPrefs.SetInt("PendingGooglePlayGamesSync", 1);
		}
		if (TrainingController.stepTraining == TrainingState.TapToSelectWeapon)
		{
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			this.animTextures[0] = Resources.Load<Texture2D>("Training/ob_change_0");
			this.animTextures[1] = Resources.Load<Texture2D>("Training/ob_change_1");
			if (this.animTextures[0] != null)
			{
				zero = new Vector2((float)Screen.width * 0.5f - 164f * Defs.Coef - (float)this.animTextures[0].width * 0.5f * Defs.Coef, (float)Screen.height - (112f + (float)this.animTextures[0].height) * Defs.Coef);
			}
		}
		if (TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				WeaponManager.sharedManager.myPlayerMoveC.GrenadeCount = 10;
			}
			this.AdjustGrenadeLabelAndArrow();
			TrainingController.isCanceled = true;
			TrainingController.maxStepAnim = 19;
			this.speedAnim = 0.5f;
			TrainingController.stepAnim = 0;
			for (int num3 = 0; num3 != this.animTextures.Length; num3++)
			{
				this.animTextures[num3] = null;
			}
			Defs.InitCoordsIphone();
			if (this.animTextures[0] != null)
			{
				zero = new Vector2((float)Screen.width - ((float)(-(float)Defs.GrenadeX + this.animTextures[0].width) + 80f) * Defs.Coef, (float)Screen.height - ((float)(Defs.GrenadeY + this.animTextures[0].height) - 80f) * Defs.Coef);
			}
		}
		if (this.animTextures[0] != null)
		{
			this.animTextureRect = new Rect(zero.x, zero.y, (float)this.animTextures[0].width * Defs.Coef, (float)this.animTextures[0].height * Defs.Coef);
		}
		base.Invoke("FirstStep", 1f);
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001C2504 File Offset: 0x001C0704
	[Obfuscation(Exclude = true)]
	private void AnimShop()
	{
		this.isAnimShop = !this.isAnimShop;
		bool flag = TrainingController.stepTraining == TrainingState.EnterTheShop;
		string normalSprite = this.shopButton.normalSprite;
		string pressedSprite = this.shopButton.pressedSprite;
		this.shopButton.pressedSprite = normalSprite;
		this.shopButton.normalSprite = pressedSprite;
		if (flag)
		{
			base.Invoke("AnimShop", 0.3f);
		}
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001C2570 File Offset: 0x001C0770
	[Obfuscation(Exclude = true)]
	private void FirstStep()
	{
		TrainingController.isCanceled = false;
		TrainingController.stepAnim = 0;
		this.NextStepAnim();
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001C2584 File Offset: 0x001C0784
	[Obfuscation(Exclude = true)]
	private void NextStepAnim()
	{
		base.CancelInvoke("NextStepAnim");
		if (TrainingController.isCanceled)
		{
			return;
		}
		TrainingController.stepAnim++;
		if (TrainingController.stepTraining == TrainingState.WellDone && TrainingController.stepAnim >= TrainingController.maxStepAnim)
		{
			TrainingController.isNextStep = TrainingState.WellDone;
			return;
		}
		if (TrainingController.stepTraining == TrainingState.WellDoneCoin && TrainingController.stepAnim >= TrainingController.maxStepAnim)
		{
			TrainingController.isNextStep = TrainingState.WellDoneCoin;
			return;
		}
		base.Invoke("NextStepAnim", this.speedAnim);
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001C2608 File Offset: 0x001C0808
	private void Update()
	{
		if (this.coinsPrefab == null && TrainingController.stepTraining < TrainingState.GetTheCoin)
		{
			this.coinsPrefab = GameObject.FindGameObjectWithTag("CoinBonus");
			if (this.coinsPrefab != null)
			{
				this.coinsPrefab.SetActive(false);
			}
		}
		if (TrainingController.isNextStep > TrainingController.setNextStepInd)
		{
			TrainingController.setNextStepInd = TrainingController.isNextStep;
			if (TrainingController.stepTraining == TrainingState.SwipeToRotate || TrainingController.stepTraining == TrainingState.TapToMove)
			{
				base.Invoke("StartNextStepTraning", 1.5f);
			}
			else if (TrainingController.stepTraining == TrainingState.TapToShoot)
			{
				base.Invoke("StartNextStepTraning", 3f);
			}
			else
			{
				this.StartNextStepTraning();
			}
		}
		if (ShopNGUIController.GuiActive || TrainingController.isPause)
		{
			if (this.shopArrowOverlay != null)
			{
				this.shopArrowOverlay.SetActive(false);
			}
			if (this.buyGrenadeArrowOverlay != null)
			{
				this.buyGrenadeArrowOverlay.SetActive(false);
			}
			if (this.throwGrenadeArrowOverlay != null)
			{
				this.throwGrenadeArrowOverlay.SetActive(false);
			}
			if (this.joystickFingerOverlay != null)
			{
				this.joystickFingerOverlay.SetActive(false);
			}
			if (this.joystickShadowOverlay != null)
			{
				this.joystickShadowOverlay.SetActive(false);
			}
			if (this.touchpadOverlay != null)
			{
				this.touchpadOverlay.SetActive(false);
			}
			if (this.touchpadFingerOverlay != null)
			{
				this.touchpadFingerOverlay.SetActive(false);
			}
			if (this.swipeWeaponFingerOverlay != null)
			{
				this.swipeWeaponFingerOverlay.SetActive(false);
			}
			if (this.tapWeaponArrowOverlay != null)
			{
				this.tapWeaponArrowOverlay.SetActive(false);
			}
		}
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001C27E4 File Offset: 0x001C09E4
	private void LateUpdate()
	{
		this.RefreshOverlays();
	}

	// Token: 0x06004DC5 RID: 19909 RVA: 0x001C27EC File Offset: 0x001C09EC
	public void Hide3dTouchJump()
	{
		if (this.touch3dPressGun.activeSelf)
		{
			this.touch3dPressGun.SetActive(false);
		}
	}

	// Token: 0x06004DC6 RID: 19910 RVA: 0x001C280C File Offset: 0x001C0A0C
	public void Hide3dTouchFire()
	{
		if (this.touch3dPressFire.activeSelf)
		{
			this.touch3dPressFire.SetActive(false);
		}
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001C282C File Offset: 0x001C0A2C
	private void RefreshOverlays()
	{
		if (TrainingController.isPause)
		{
			return;
		}
		GameObject objA = null;
		if (TrainingController.stepTraining == TrainingState.SwipeToRotate)
		{
			objA = this.swipeToRotateOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToMove)
		{
			objA = this.dragToMoveOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.GetTheGun)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				TrainingController.timeShowJump += Time.deltaTime;
				if (this.touch3dPressGun.activeSelf)
				{
					TrainingController.timeShow3dTouchJump += Time.deltaTime;
					if (TrainingController.timeShow3dTouchJump > 5f)
					{
						this.Hide3dTouchJump();
					}
				}
				if (!this.isShow3dTouchJump && TrainingController.timeShowJump > 3f)
				{
					this.isShow3dTouchJump = true;
					HintController.instance.HideHintByName("press_jump");
					this.touch3dPressGun.SetActive(true);
				}
			}
			objA = this.pickupGunOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.WellDone || TrainingController.stepTraining == TrainingState.WellDoneCoin)
		{
			objA = this.wellDoneOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.GetTheCoin)
		{
			objA = this.getCoinOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.EnterTheShop)
		{
			objA = this.enterShopOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			objA = this.shootReloadOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			objA = this.throwGrenadeOverlay;
		}
		else if (TrainingController.stepTraining == TrainingState.KillZombie)
		{
			if (Defs.touchPressureSupported || Application.isEditor)
			{
				TrainingController.timeShowFire += Time.deltaTime;
				if (this.touch3dPressFire.activeSelf)
				{
					TrainingController.timeShow3dTouchFire += Time.deltaTime;
					if (TrainingController.timeShow3dTouchFire > 5f)
					{
						this.Hide3dTouchFire();
					}
				}
				if (!this.isShow3dTouchFire && TrainingController.timeShowFire > 3f)
				{
					this.isShow3dTouchFire = true;
					HintController.instance.HideHintByName("press_fire");
					this.touch3dPressFire.SetActive(true);
				}
			}
			objA = this.killZombiesOverlay;
		}
		foreach (GameObject gameObject in from o in this._overlays
		where null != o
		select o)
		{
			gameObject.SetActive(object.ReferenceEquals(objA, gameObject));
		}
		bool flag = PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1;
		if (this.swipeToChangeWeaponOverlay != null)
		{
			this.swipeToChangeWeaponOverlay.SetActive(TrainingController.stepTraining == TrainingState.TapToSelectWeapon && flag);
		}
		if (this.tapToChangeWeaponOverlay != null)
		{
			this.tapToChangeWeaponOverlay.SetActive(TrainingController.stepTraining == TrainingState.TapToSelectWeapon && !flag);
		}
		if (this.shopArrowOverlay != null)
		{
			this.shopArrowOverlay.SetActive(TrainingController.stepTraining == TrainingState.EnterTheShop);
		}
		if (this.throwGrenadeArrowOverlay != null)
		{
			this.throwGrenadeArrowOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToThrowGrenade"]);
		}
		if (this.joystickFingerOverlay != null)
		{
			this.joystickFingerOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToMove"]);
		}
		if (this.joystickShadowOverlay != null)
		{
			this.joystickShadowOverlay.SetActive(TrainingController.stepTraining == TrainingController.stepTrainingList["TapToMove"]);
		}
		if (this.touchpadOverlay != null)
		{
			this.touchpadOverlay.SetActive(TrainingController.stepTraining == TrainingState.SwipeToRotate);
		}
		if (this.touchpadFingerOverlay != null)
		{
			this.touchpadFingerOverlay.SetActive(TrainingController.stepTraining == TrainingState.SwipeToRotate);
		}
		if (this.swipeWeaponFingerOverlay != null)
		{
			this.swipeWeaponFingerOverlay.SetActive(TrainingController.stepTraining == TrainingState.TapToSelectWeapon && flag);
		}
		if (this.tapWeaponArrowOverlay != null)
		{
			this.tapWeaponArrowOverlay.SetActive(TrainingController.stepTraining == TrainingState.TapToSelectWeapon && !flag);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.CampaignContainer.SetActive(TrainingController.stepTraining == TrainingState.KillZombie);
			InGameGUI.sharedInGameGUI.leftAnchor.SetActive(false);
			InGameGUI.sharedInGameGUI.rightAnchor.SetActive(false);
		}
	}

	// Token: 0x06004DC8 RID: 19912 RVA: 0x001C2CD8 File Offset: 0x001C0ED8
	private void OnLevelWasLoaded(int unused)
	{
		if (SceneManager.GetActiveScene().name == Defs.TrainingSceneName)
		{
			GC.Collect();
			this.weapon = BonusCreator._CreateBonus(WeaponManager.MP5WN, new Vector3(0f, -10000f, 0f));
		}
	}

	// Token: 0x04003C25 RID: 15397
	public const string NewTrainingStageHolderKey = "TrainingController.NewTrainingStageHolderKey";

	// Token: 0x04003C26 RID: 15398
	public static TrainingController sharedController = null;

	// Token: 0x04003C27 RID: 15399
	private static bool _trainingCompletedInitialized = false;

	// Token: 0x04003C28 RID: 15400
	private static bool cachedTrainingComplete = false;

	// Token: 0x04003C29 RID: 15401
	private static bool _comletedTrainingStageInitialized = false;

	// Token: 0x04003C2A RID: 15402
	private static TrainingController.NewTrainingCompletedStage _completedTrainingStage = TrainingController.NewTrainingCompletedStage.None;

	// Token: 0x04003C2B RID: 15403
	public GameObject swipeToRotateOverlay;

	// Token: 0x04003C2C RID: 15404
	public GameObject dragToMoveOverlay;

	// Token: 0x04003C2D RID: 15405
	public GameObject pickupGunOverlay;

	// Token: 0x04003C2E RID: 15406
	public GameObject touch3dPressGun;

	// Token: 0x04003C2F RID: 15407
	public GameObject touch3dPressFire;

	// Token: 0x04003C30 RID: 15408
	public GameObject wellDoneOverlay;

	// Token: 0x04003C31 RID: 15409
	public GameObject getCoinOverlay;

	// Token: 0x04003C32 RID: 15410
	public GameObject enterShopOverlay;

	// Token: 0x04003C33 RID: 15411
	public GameObject shopArrowOverlay;

	// Token: 0x04003C34 RID: 15412
	public GameObject swipeToChangeWeaponOverlay;

	// Token: 0x04003C35 RID: 15413
	public GameObject tapToChangeWeaponOverlay;

	// Token: 0x04003C36 RID: 15414
	public GameObject shootReloadOverlay;

	// Token: 0x04003C37 RID: 15415
	public GameObject selectGrenadeOverlay;

	// Token: 0x04003C38 RID: 15416
	public GameObject buyGrenadeArrowOverlay;

	// Token: 0x04003C39 RID: 15417
	public GameObject throwGrenadeOverlay;

	// Token: 0x04003C3A RID: 15418
	public GameObject throwGrenadeArrowOverlay;

	// Token: 0x04003C3B RID: 15419
	public GameObject killZombiesOverlay;

	// Token: 0x04003C3C RID: 15420
	public GameObject overlaysRoot;

	// Token: 0x04003C3D RID: 15421
	public GameObject joystickFingerOverlay;

	// Token: 0x04003C3E RID: 15422
	public GameObject joystickShadowOverlay;

	// Token: 0x04003C3F RID: 15423
	public GameObject touchpadOverlay;

	// Token: 0x04003C40 RID: 15424
	public GameObject touchpadFingerOverlay;

	// Token: 0x04003C41 RID: 15425
	public GameObject swipeWeaponFingerOverlay;

	// Token: 0x04003C42 RID: 15426
	public GameObject tapWeaponArrowOverlay;

	// Token: 0x04003C43 RID: 15427
	public GameObject enemyPrototype;

	// Token: 0x04003C44 RID: 15428
	public GameObject[] enemies;

	// Token: 0x04003C45 RID: 15429
	public Transform teleportTransform;

	// Token: 0x04003C46 RID: 15430
	public Transform weaponTransform;

	// Token: 0x04003C47 RID: 15431
	public Transform playerTransform;

	// Token: 0x04003C48 RID: 15432
	private static readonly Vector3 _playerDefaultPosition = new Vector3(-0.72f, 1.75f, -13.23f);

	// Token: 0x04003C49 RID: 15433
	private GameObject[] _overlays = new GameObject[0];

	// Token: 0x04003C4A RID: 15434
	internal static TrainingState stepTraining = (TrainingState)(-1);

	// Token: 0x04003C4B RID: 15435
	internal static Dictionary<string, TrainingState> stepTrainingList = new Dictionary<string, TrainingState>(10);

	// Token: 0x04003C4C RID: 15436
	internal static TrainingState isNextStep = TrainingState.None;

	// Token: 0x04003C4D RID: 15437
	public static bool isPressSkip;

	// Token: 0x04003C4E RID: 15438
	public static bool isPause = false;

	// Token: 0x04003C4F RID: 15439
	private Rect animTextureRect;

	// Token: 0x04003C50 RID: 15440
	private static bool nextStepAfterSkipTraining = false;

	// Token: 0x04003C51 RID: 15441
	private GameObject coinsPrefab;

	// Token: 0x04003C52 RID: 15442
	private Texture2D[] animTextures;

	// Token: 0x04003C53 RID: 15443
	private static int stepAnim;

	// Token: 0x04003C54 RID: 15444
	private static int maxStepAnim;

	// Token: 0x04003C55 RID: 15445
	private static bool isCanceled;

	// Token: 0x04003C56 RID: 15446
	private float speedAnim;

	// Token: 0x04003C57 RID: 15447
	private static TrainingState setNextStepInd = TrainingState.None;

	// Token: 0x04003C58 RID: 15448
	private Texture2D shop;

	// Token: 0x04003C59 RID: 15449
	private Texture2D shop_n;

	// Token: 0x04003C5A RID: 15450
	private bool isAnimShop;

	// Token: 0x04003C5B RID: 15451
	private static TrainingState oldStepTraning;

	// Token: 0x04003C5C RID: 15452
	private UIButton shopButton;

	// Token: 0x04003C5D RID: 15453
	private static bool? _trainingCompleted;

	// Token: 0x04003C5E RID: 15454
	private ActionDisposable _weaponChangedSubscription = new ActionDisposable(delegate()
	{
	});

	// Token: 0x04003C5F RID: 15455
	private readonly List<TrainingEnemy> _enemies = new List<TrainingEnemy>(3);

	// Token: 0x04003C60 RID: 15456
	private UIButton _pauseButton;

	// Token: 0x04003C61 RID: 15457
	private int _weaponChangingCount;

	// Token: 0x04003C62 RID: 15458
	public static float timeShowJump = 0f;

	// Token: 0x04003C63 RID: 15459
	public static float timeShow3dTouchJump = 0f;

	// Token: 0x04003C64 RID: 15460
	private bool isShow3dTouchJump;

	// Token: 0x04003C65 RID: 15461
	public static float timeShowFire = 0f;

	// Token: 0x04003C66 RID: 15462
	public static float timeShow3dTouchFire = 0f;

	// Token: 0x04003C67 RID: 15463
	private bool isShow3dTouchFire;

	// Token: 0x04003C68 RID: 15464
	private GameObject weapon;

	// Token: 0x04003C69 RID: 15465
	private readonly Lazy<PlayerArrowToPortalController> _directionArrow;

	// Token: 0x02000869 RID: 2153
	public enum NewTrainingCompletedStage
	{
		// Token: 0x04003C6F RID: 15471
		None,
		// Token: 0x04003C70 RID: 15472
		ShootingRangeCompleted,
		// Token: 0x04003C71 RID: 15473
		ShopCompleted,
		// Token: 0x04003C72 RID: 15474
		FirstMatchCompleted
	}
}
