using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft;
using Rilisoft.NullExtensions;
using RilisoftBot;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public sealed class ZombieCreator : MonoBehaviour
{
	// Token: 0x06004558 RID: 17752 RVA: 0x001761AC File Offset: 0x001743AC
	static ZombieCreator()
	{
		List<string> list = new List<string>();
		list.Add(WeaponManager.PistolWN);
		list.Add(WeaponManager.ShotgunWN);
		list.Add(WeaponManager.MP5WN);
		List<string> list2 = new List<string>();
		list2.Add(WeaponManager.AK47WN);
		list2.Add(WeaponManager.RevolverWN);
		List<string> list3 = new List<string>();
		list3.Add(WeaponManager.M16_2WN);
		list3.Add(WeaponManager.ObrezWN);
		List<string> list4 = new List<string>();
		list4.Add(WeaponManager.MachinegunWN);
		List<string> list5 = new List<string>();
		list5.Add(WeaponManager.AlienGunWN);
		ZombieCreator._WeaponsAddedInWaves.Add(list);
		ZombieCreator._WeaponsAddedInWaves.Add(list2);
		ZombieCreator._WeaponsAddedInWaves.Add(list3);
		ZombieCreator._WeaponsAddedInWaves.Add(list4);
		ZombieCreator._WeaponsAddedInWaves.Add(list5);
	}

	// Token: 0x140000A0 RID: 160
	// (add) Token: 0x06004559 RID: 17753 RVA: 0x001762B4 File Offset: 0x001744B4
	// (remove) Token: 0x0600455A RID: 17754 RVA: 0x001762CC File Offset: 0x001744CC
	public static event Action LastEnemy;

	// Token: 0x140000A1 RID: 161
	// (add) Token: 0x0600455B RID: 17755 RVA: 0x001762E4 File Offset: 0x001744E4
	// (remove) Token: 0x0600455C RID: 17756 RVA: 0x001762FC File Offset: 0x001744FC
	public static event Action BossKilled;

	// Token: 0x17000BB1 RID: 2993
	// (get) Token: 0x0600455E RID: 17758 RVA: 0x00176320 File Offset: 0x00174520
	// (set) Token: 0x0600455D RID: 17757 RVA: 0x00176314 File Offset: 0x00174514
	public GameObject[] bossGuads { get; private set; }

	// Token: 0x17000BB2 RID: 2994
	// (get) Token: 0x0600455F RID: 17759 RVA: 0x00176328 File Offset: 0x00174528
	// (set) Token: 0x06004560 RID: 17760 RVA: 0x00176350 File Offset: 0x00174550
	public static int EnemyCountInSurvivalWave
	{
		get
		{
			return (ZombieCreator._enemyCountInSurvivalWave == null) ? ZombieCreator.DefaultEnemyCountInSurvivalWave : ZombieCreator._enemyCountInSurvivalWave.Value;
		}
		set
		{
			ZombieCreator._enemyCountInSurvivalWave = new int?(value);
		}
	}

	// Token: 0x17000BB3 RID: 2995
	// (get) Token: 0x06004561 RID: 17761 RVA: 0x00176360 File Offset: 0x00174560
	public static int DefaultEnemyCountInSurvivalWave
	{
		get
		{
			return ZombieCreator._ZombiesInWave;
		}
	}

	// Token: 0x06004562 RID: 17762 RVA: 0x00176368 File Offset: 0x00174568
	private IEnumerator _DrawFirstMessage()
	{
		if (!Defs.IsSurvival)
		{
			yield break;
		}
		while (InGameGUI.sharedInGameGUI == null)
		{
			yield return null;
		}
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
		{
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = string.Format("{0} 1", LocalizationStore.Key_0349);
			yield return new WaitForSeconds(2f);
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06004563 RID: 17763 RVA: 0x0017637C File Offset: 0x0017457C
	private IEnumerator _DrawWaveMessage(Action act)
	{
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(true);
		}
		yield return new WaitForSeconds(5f);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.waveDone.gameObject.SetActive(false);
		}
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.Wave1_And_Counter != null)
		{
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(true);
			for (int i = 5; i > 0; i--)
			{
				InGameGUI.sharedInGameGUI.Wave1_And_Counter.text = i.ToString();
				yield return new WaitForSeconds(1f);
			}
			InGameGUI.sharedInGameGUI.Wave1_And_Counter.gameObject.SetActive(false);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(true);
		}
		act();
		yield return new WaitForSeconds(1f);
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.newWave.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06004564 RID: 17764 RVA: 0x001763A0 File Offset: 0x001745A0
	private void OnDestroy()
	{
		ZombieCreator.sharedCreator = null;
		if (Defs.IsSurvival)
		{
			PlayerPrefs.SetInt(Defs.KilledZombiesSett, this.totalNumOfKilledEnemies);
			int @int = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0);
			if (this.totalNumOfKilledEnemies > @int)
			{
				PlayerPrefs.SetInt(Defs.KilledZombiesMaxSett, this.totalNumOfKilledEnemies);
			}
			WavesSurvivedStat.SurvivedWaveCount = this.currentWave;
			int int2 = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0);
			if (this.currentWave > int2)
			{
				PlayerPrefs.SetInt(Defs.WavesSurvivedMaxS, this.currentWave);
			}
		}
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x00176428 File Offset: 0x00174628
	private void _UpdateIntervalStructures()
	{
		this._genWithThisTimeInterval = 0;
		this._indexInTimesArray = 0;
		this.curInterval = (float)this._intervalArr[this._indexInTimesArray];
	}

	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06004566 RID: 17766 RVA: 0x00176458 File Offset: 0x00174658
	// (set) Token: 0x06004567 RID: 17767 RVA: 0x00176468 File Offset: 0x00174668
	public int NumOfLiveZombies
	{
		get
		{
			return this._numOfLiveZombies.Value;
		}
		set
		{
			this._numOfLiveZombies = value;
		}
	}

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06004568 RID: 17768 RVA: 0x00176478 File Offset: 0x00174678
	public bool IsLasTMonsRemains
	{
		get
		{
			return this.NumOfDeadZombies + 1 == ZombieCreator.NumOfEnemisesToKill && !this.bossShowm;
		}
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x00176498 File Offset: 0x00174698
	private void SetWaveNumberInGUI()
	{
		if (InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.SurvivalWaveNumber != null)
		{
			InGameGUI.sharedInGameGUI.SurvivalWaveNumber.text = string.Format("{0} {1}", LocalizationStore.Get("Key_0349"), this.currentWave + 1);
		}
	}

	// Token: 0x0600456A RID: 17770 RVA: 0x001764FC File Offset: 0x001746FC
	public void NextWave()
	{
		this.currentWave++;
		QuestMediator.NotifySurviveWaveInArena(this.currentWave);
		StoreKitEventListener.State.Parameters.Clear();
		StoreKitEventListener.State.Parameters.Add("Waves", ((this.currentWave >= 10) ? string.Concat(new object[]
		{
			string.Empty,
			this.currentWave / 10 * 10,
			"-",
			(this.currentWave / 10 + 1) * 10 - 1
		}) : (string.Empty + (this.currentWave + 1))) + " In game");
		base.StartCoroutine(this._DrawWaveMessage(delegate
		{
			this._UpdateIntervalStructures();
			this._numOfDeadZombies = 0;
			this._numOfDeadZombsSinceLastFast = 0;
			this._SetZombiePrefabs();
			this._UpdateAvailableWeapons();
			this._generatingZombiesIsStopped = false;
			this.stopGeneratingBonuses = false;
			this.SetWaveNumberInGUI();
		}));
		Vector3 position;
		if (SceneLoader.ActiveSceneName.Equals("Pizza"))
		{
			position = new Vector3(-7.83f, 0.46f, -2.44f);
		}
		else
		{
			position = new Vector3(0f, 1f, 0f);
		}
		GameObject gameObject = Initializer.CreateBonusAtPosition(position, VirtualCurrencyBonusType.Coin);
		if (gameObject == null)
		{
			return;
		}
		CoinBonus component = gameObject.GetComponent<CoinBonus>();
		if (component == null)
		{
			Debug.LogErrorFormat("Cannot find component '{0}'", new object[]
			{
				component.GetType().Name
			});
			return;
		}
		component.SetPlayer();
	}

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x0600456B RID: 17771 RVA: 0x0017666C File Offset: 0x0017486C
	// (set) Token: 0x0600456C RID: 17772 RVA: 0x0017667C File Offset: 0x0017487C
	public int NumOfDeadZombies
	{
		get
		{
			return this._numOfDeadZombies.Value;
		}
		set
		{
			if (!this.bossShowm)
			{
				int num = value - this._numOfDeadZombies.Value;
				this._numOfDeadZombies = value;
				this.totalNumOfKilledEnemies += num;
				this.NumOfLiveZombies -= num;
				if (!Defs.IsSurvival && ZombieCreator.NumOfEnemisesToKill - this._numOfDeadZombies.Value <= 5 && Initializer.enemiesObj.Count > 0)
				{
					PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
					Transform transform = null;
					float num2 = float.MaxValue;
					for (int i = 0; i < Initializer.enemiesObj.Count; i++)
					{
						if (Initializer.enemiesObj[i].GetComponent<BaseBot>().health > 0f)
						{
							float sqrMagnitude = (WeaponManager.sharedManager.myPlayer.transform.position - Initializer.enemiesObj[i].transform.position).sqrMagnitude;
							if (sqrMagnitude < num2)
							{
								transform = Initializer.enemiesObj[i].transform;
								num2 = sqrMagnitude;
							}
						}
					}
					component.RemovePointOfInterest();
					if (transform != null)
					{
						component.RemovePointOfInterest();
						component.SetPointOfInterest(transform);
					}
				}
				if (!Defs.IsSurvival)
				{
					this._numOfDeadZombsSinceLastFast = this._numOfDeadZombsSinceLastFast.Value + num;
					if (this._numOfDeadZombsSinceLastFast.Value == 12)
					{
						if (this.curInterval > 5f)
						{
							this.curInterval -= 5f;
						}
						this._numOfDeadZombsSinceLastFast = 0;
					}
					if (this.IsLasTMonsRemains && ZombieCreator.LastEnemy != null)
					{
						ZombieCreator.LastEnemy();
					}
				}
				if (Defs.IsSurvival && this.NumOfDeadZombies == ZombieCreator.NumOfEnemisesToKill - 1)
				{
					this.stopGeneratingBonuses = true;
				}
				if (this._numOfDeadZombies.Value >= ZombieCreator.NumOfEnemisesToKill)
				{
					if (Defs.IsSurvival)
					{
						if (ZombieCreator.bossesSurvival.ContainsKey(this.currentWave))
						{
							this.CreateBoss();
						}
						else
						{
							this.NextWave();
						}
					}
					else if (CurrentCampaignGame.currentLevel == 0)
					{
						foreach (GameObject gameObject in this._teleports)
						{
							if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
							{
								TrainingController.isNextStep = TrainingState.KillZombie;
							}
							gameObject.SetActive(true);
						}
					}
					else
					{
						this.CreateBoss();
						if (this.bossMus != null)
						{
							GameObject gameObject2 = GameObject.FindGameObjectWithTag("BackgroundMusic");
							if (gameObject2 != null && gameObject2.GetComponent<AudioSource>())
							{
								gameObject2.GetComponent<AudioSource>().Stop();
								gameObject2.GetComponent<AudioSource>().clip = this.bossMus;
								if (Defs.isSoundMusic)
								{
									gameObject2.GetComponent<AudioSource>().Play();
								}
							}
						}
					}
				}
				return;
			}
			this.bossShowm = false;
			if (!Defs.IsSurvival)
			{
				if (ZombieCreator.BossKilled != null)
				{
					ZombieCreator.BossKilled();
				}
				if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
				{
					foreach (GameObject gameObject3 in this._teleports)
					{
						gameObject3.SetActive(true);
					}
					GameObject gameObject4 = this._teleports.Map((GameObject[] ts) => ts.FirstOrDefault<GameObject>());
					if (gameObject4 != null)
					{
						PlayerArrowToPortalController component2 = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
						if (component2 != null)
						{
							component2.RemovePointOfInterest();
							component2.SetPointOfInterest(gameObject4.transform);
						}
					}
				}
				else
				{
					GameObject gameObject5 = (from g in UnityEngine.Object.FindObjectsOfType<GotToNextLevel>()
					select g.gameObject).FirstOrDefault<GameObject>();
					if (gameObject5 != null)
					{
						PlayerArrowToPortalController component3 = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
						if (component3 != null)
						{
							component3.RemovePointOfInterest();
							component3.SetPointOfInterest(gameObject5.transform);
						}
					}
				}
				return;
			}
			this.totalNumOfKilledEnemies++;
			this.NumOfLiveZombies--;
			this.NextWave();
		}
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x00176AF0 File Offset: 0x00174CF0
	public static void SetLayerRecursively(GameObject go, int layerNumber)
	{
		if (go == null)
		{
			return;
		}
		foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
		{
			transform.gameObject.layer = layerNumber;
		}
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x00176B38 File Offset: 0x00174D38
	private IEnumerator _PrerenderBoss()
	{
		GameObject prer = UnityEngine.Object.Instantiate(Resources.Load("ObjectPrerenderer") as GameObject, new Vector3(0f, 0f, -10000f), Quaternion.identity) as GameObject;
		ObjectPrerenderer op = prer.GetComponentInChildren<ObjectPrerenderer>();
		if (op)
		{
			GameObject bc = UnityEngine.Object.Instantiate<GameObject>(this.boss.transform.GetChild(0).gameObject);
			bc.transform.parent = op.transform;
			bc.transform.localPosition = new Vector3(0f, 0f, 2.7f);
			bc.layer = op.gameObject.layer;
			ZombieCreator.SetLayerRecursively(bc, bc.layer);
			if (this.weaponBonus != null)
			{
				GameObject w = BonusCreator._CreateBonusFromPrefab(this.weaponBonus, Vector3.zero);
				w.transform.parent = op.transform;
				w.transform.localPosition = new Vector3(1.5f, 0f, 3f);
				w.layer = op.gameObject.layer;
				ZombieCreator.SetLayerRecursively(w, w.layer);
			}
			yield return null;
			op.Render_();
			UnityEngine.Object.Destroy(prer);
		}
		yield break;
	}

	// Token: 0x0600456F RID: 17775 RVA: 0x00176B54 File Offset: 0x00174D54
	private void TryCreateBossGuard(GameObject bossObj)
	{
		this.bossGuads = null;
		BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(bossObj.transform);
		if (botScriptForObject == null)
		{
			return;
		}
		int num = botScriptForObject.guards.Length;
		if (num == 0)
		{
			return;
		}
		this.bossGuads = new GameObject[num];
		for (int i = 0; i < num; i++)
		{
			GameObject original = botScriptForObject.guards[i];
			this.bossGuads[i] = UnityEngine.Object.Instantiate<GameObject>(original);
			this.bossGuads[i].name = string.Format("{0}{1}", "BossGuard", i + 1);
			this.bossGuads[i].SetActive(false);
		}
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x00176BF8 File Offset: 0x00174DF8
	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[]
		{
			base.GetType().Name
		});
		using (new ScopeLogger(callee, false))
		{
			if (!Defs.isMulti)
			{
				if (Defs.IsSurvival)
				{
					ZombieCreator._enemiesInWaves.Clear();
					if (SceneLoader.ActiveSceneName.Equals("Pizza"))
					{
						List<string> list = new List<string>();
						List<string> list2 = new List<string>();
						List<string> list3 = new List<string>();
						List<string> list4 = new List<string>();
						List<string> list5 = new List<string>();
						List<string> list6 = new List<string>();
						list.Add("88");
						list.Add("85");
						list.Add("86");
						list2.Add("85");
						list2.Add("87");
						list2.Add("82");
						list2.Add("81");
						list2.Add("88");
						list3.Add("86");
						list3.Add("82");
						list3.Add("84");
						list3.Add("81");
						list3.Add("88");
						list3.Add("87");
						list4.Add("81");
						list4.Add("82");
						list4.Add("86");
						list4.Add("80");
						list4.Add("83");
						list4.Add("87");
						list4.Add("84");
						list5.Add("81");
						list5.Add("86");
						list5.Add("88");
						list5.Add("80");
						list5.Add("83");
						list5.Add("82");
						list5.Add("84");
						list5.Add("87");
						list6.Add("81");
						list6.Add("80");
						list6.Add("83");
						list6.Add("82");
						list6.Add("84");
						list6.Add("87");
						ZombieCreator._enemiesInWaves.Add(list);
						ZombieCreator._enemiesInWaves.Add(list2);
						ZombieCreator._enemiesInWaves.Add(list3);
						ZombieCreator._enemiesInWaves.Add(list4);
						ZombieCreator._enemiesInWaves.Add(list5);
						ZombieCreator._enemiesInWaves.Add(list6);
					}
					else
					{
						List<string> list7 = new List<string>();
						List<string> list8 = new List<string>();
						List<string> list9 = new List<string>();
						List<string> list10 = new List<string>();
						List<string> list11 = new List<string>();
						List<string> list12 = new List<string>();
						List<string> list13 = new List<string>();
						List<string> list14 = new List<string>();
						list7.Add("1");
						list7.Add("2");
						list7.Add("15");
						list8.Add("1");
						list8.Add("2");
						list8.Add("15");
						list8.Add("77");
						list8.Add("12");
						list9.Add("3");
						list9.Add("9");
						list9.Add("10");
						list9.Add("11");
						list9.Add("12");
						list9.Add("57");
						list10.Add("49");
						list10.Add("9");
						list10.Add("24");
						list10.Add("29");
						list10.Add("38");
						list10.Add("74");
						list10.Add("48");
						list10.Add("10");
						list11.Add("80");
						list11.Add("81");
						list11.Add("82");
						list11.Add("83");
						list11.Add("84");
						list11.Add("85");
						list11.Add("86");
						list11.Add("87");
						list11.Add("88");
						list12.Add("37");
						list12.Add("46");
						list12.Add("47");
						list12.Add("57");
						list12.Add("24");
						list12.Add("74");
						list12.Add("50");
						list12.Add("20");
						list12.Add("51");
						list13.Add("74");
						list13.Add("57");
						list13.Add("20");
						list13.Add("66");
						list13.Add("60");
						list13.Add("50");
						list13.Add("53");
						list13.Add("33");
						list13.Add("24");
						list13.Add("46");
						list14.Add("74");
						list14.Add("57");
						list14.Add("49");
						list14.Add("66");
						list14.Add("60");
						list14.Add("50");
						list14.Add("53");
						list14.Add("59");
						list14.Add("24");
						list14.Add("46");
						ZombieCreator._enemiesInWaves.Add(list7);
						ZombieCreator._enemiesInWaves.Add(list8);
						ZombieCreator._enemiesInWaves.Add(list9);
						ZombieCreator._enemiesInWaves.Add(list10);
						ZombieCreator._enemiesInWaves.Add(list11);
						ZombieCreator._enemiesInWaves.Add(list12);
						ZombieCreator._enemiesInWaves.Add(list13);
						ZombieCreator._enemiesInWaves.Add(list14);
					}
					foreach (List<string> list15 in ZombieCreator._enemiesInWaves)
					{
						foreach (string item in list15)
						{
							ZombieCreator._allEnemiesSurvival.Add(item);
						}
					}
				}
				this.stopGeneratingBonuses = false;
				ZombieCreator.sharedCreator = this;
				if (!Defs.IsSurvival && CurrentCampaignGame.currentLevel != 0)
				{
					string b = "Boss" + CurrentCampaignGame.currentLevel;
					this.boss = (UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", b))) as GameObject);
					this.TryCreateBossGuard(this.boss);
					this.boss.SetActive(false);
					if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
					{
						string weaponName = LevelBox.weaponsFromBosses[Application.loadedLevelName];
						this.weaponBonus = BonusCreator._CreateBonusPrefab(weaponName);
					}
					base.StartCoroutine(this._PrerenderBoss());
					this.bossMus = (Resources.Load("Snd/boss_campaign") as AudioClip);
				}
				GlobalGameController.curThr = GlobalGameController.thrStep;
				this._enemies.Add(new string[]
				{
					"1",
					"2",
					"1",
					"11",
					"12",
					"13"
				});
				this._enemies.Add(new string[]
				{
					"30",
					"31",
					"32",
					"33",
					"34",
					"77"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"2",
					"3",
					"9",
					"10",
					"12",
					"14",
					"15",
					"78"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"2",
					"4",
					"11",
					"9",
					"16",
					"78"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"2",
					"4",
					"9",
					"11",
					"10",
					"12"
				});
				this._enemies.Add(new string[]
				{
					"43",
					"44",
					"45",
					"46",
					"47",
					"73"
				});
				this._enemies.Add(new string[]
				{
					"6",
					"7",
					"7"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"2",
					"8",
					"10",
					"11",
					"12",
					"76"
				});
				this._enemies.Add(new string[]
				{
					"18",
					"19",
					"20"
				});
				this._enemies.Add(new string[]
				{
					"21",
					"22",
					"23",
					"24",
					"25",
					"75"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"15"
				});
				this._enemies.Add(new string[]
				{
					"1",
					"3",
					"9",
					"10",
					"14",
					"15",
					"16",
					"78"
				});
				this._enemies.Add(new string[]
				{
					"8",
					"21",
					"22",
					"79"
				});
				this._enemies.Add(new string[]
				{
					"26",
					"27",
					"28",
					"29",
					"57"
				});
				this._enemies.Add(new string[]
				{
					"35",
					"36",
					"37",
					"38",
					"48",
					"57"
				});
				this._enemies.Add(new string[]
				{
					"39",
					"40",
					"41",
					"42",
					"74"
				});
				this._enemies.Add(new string[]
				{
					"53",
					"55",
					"57",
					"61"
				});
				this._enemies.Add(new string[]
				{
					"59",
					"56",
					"54",
					"60"
				});
				this._enemies.Add(new string[]
				{
					"67",
					"68",
					"66",
					"69"
				});
				this._enemies.Add(new string[]
				{
					"70",
					"71",
					"72"
				});
				this._enemies.Add(new string[]
				{
					"58",
					"63",
					"64",
					"65"
				});
				this.UpdateBotPrefabs();
				if (Defs.IsSurvival)
				{
					this._SetZombiePrefabs();
				}
				ZombieCreator.survivalAvailableWeapons.Clear();
				this._UpdateAvailableWeapons();
				this._UpdateIntervalStructures();
				base.StartCoroutine(this._DrawFirstMessage());
			}
		}
	}

	// Token: 0x06004571 RID: 17777 RVA: 0x00177888 File Offset: 0x00175A88
	private void _SetZombiePrefabs()
	{
		this.waveZombiePrefabs.Clear();
		int index = (this.currentWave < ZombieCreator._enemiesInWaves.Count) ? this.currentWave : (ZombieCreator._enemiesInWaves.Count - 1);
		foreach (GameObject gameObject in this.zombiePrefabs)
		{
			string item = gameObject.name.Substring("Enemy".Length).Substring(0, gameObject.name.Substring("Enemy".Length).IndexOf("_"));
			if (ZombieCreator._enemiesInWaves[index].Contains(item))
			{
				this.waveZombiePrefabs.Add(gameObject);
			}
		}
	}

	// Token: 0x06004572 RID: 17778 RVA: 0x0017797C File Offset: 0x00175B7C
	private void _UpdateAvailableWeapons()
	{
		if (this.currentWave < ZombieCreator._WeaponsAddedInWaves.Count)
		{
			foreach (string item in ZombieCreator._WeaponsAddedInWaves[this.currentWave])
			{
				ZombieCreator.survivalAvailableWeapons.Add(item);
			}
		}
	}

	// Token: 0x06004573 RID: 17779 RVA: 0x00177A08 File Offset: 0x00175C08
	private void UpdateBotPrefabs()
	{
		this.zombiePrefabs.Clear();
		string[] array;
		if (Defs.IsSurvival)
		{
			array = ZombieCreator._allEnemiesSurvival.ToArray<string>();
		}
		else if (CurrentCampaignGame.currentLevel == 0)
		{
			array = new string[]
			{
				"1"
			};
		}
		else
		{
			array = this._enemies[CurrentCampaignGame.currentLevel - 1];
		}
		foreach (string str in array)
		{
			GameObject item = Resources.Load("Enemies/Enemy" + str + "_go") as GameObject;
			this.zombiePrefabs.Add(item);
		}
	}

	// Token: 0x06004574 RID: 17780 RVA: 0x00177AB0 File Offset: 0x00175CB0
	private void Start()
	{
		if (Defs.IsSurvival)
		{
			StoreKitEventListener.State.Parameters.Clear();
			StoreKitEventListener.State.Parameters.Add("Waves", this.currentWave + 1 + " In game");
		}
		this.labelStyle.fontSize = Mathf.RoundToInt(50f * Defs.Coef);
		this.labelStyle.font = LocalizationStore.GetFontByLocalize("Key_04B_03");
		if (Defs.isMulti)
		{
			this._isMultiplayer = true;
		}
		else
		{
			this._isMultiplayer = false;
		}
		this._teleports = GameObject.FindGameObjectsWithTag("Portal");
		foreach (GameObject gameObject in this._teleports)
		{
			gameObject.SetActive(false);
		}
		if (!this._isMultiplayer)
		{
			this._enemyCreationZones = GameObject.FindGameObjectsWithTag("EnemyCreationZone");
			if (!Defs.IsSurvival)
			{
				this._ResetInterval();
			}
		}
	}

	// Token: 0x06004575 RID: 17781 RVA: 0x00177BB0 File Offset: 0x00175DB0
	private void _ResetInterval()
	{
		this.curInterval = Mathf.Max(1f, this.curInterval);
	}

	// Token: 0x06004576 RID: 17782 RVA: 0x00177BC8 File Offset: 0x00175DC8
	public void BeganCreateEnemies()
	{
		if (Application.isEditor && Defs.IsSurvival && !SceneLoader.ActiveSceneName.Equals(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length]))
		{
			return;
		}
		if (Defs.IsSurvival)
		{
			this.SetWaveNumberInGUI();
		}
		base.StartCoroutine(this.AddZombies());
	}

	// Token: 0x06004577 RID: 17783 RVA: 0x00177C2C File Offset: 0x00175E2C
	internal static int GetEnemiesToKill(string sceneName)
	{
		if (Defs.IsSurvival)
		{
			return ZombieCreator.EnemyCountInSurvivalWave;
		}
		return GlobalGameController.GetEnemiesToKill(sceneName);
	}

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06004578 RID: 17784 RVA: 0x00177C44 File Offset: 0x00175E44
	public static int NumOfEnemisesToKill
	{
		get
		{
			return (!Defs.IsSurvival) ? GlobalGameController.EnemiesToKill : ZombieCreator.EnemyCountInSurvivalWave;
		}
	}

	// Token: 0x06004579 RID: 17785 RVA: 0x00177C60 File Offset: 0x00175E60
	public static int GetCountMobsForLevel()
	{
		Dictionary<string, int> counCreateMobsInLevel = Switcher.counCreateMobsInLevel;
		string levelSceneName = CurrentCampaignGame.levelSceneName;
		if (counCreateMobsInLevel.ContainsKey(levelSceneName))
		{
			return counCreateMobsInLevel[levelSceneName];
		}
		return GlobalGameController.ZombiesInWave;
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x00177C94 File Offset: 0x00175E94
	private IEnumerator AddZombies()
	{
		do
		{
			int numOfZombsToAdd = GlobalGameController.ZombiesInWave;
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, GlobalGameController.SimultaneousEnemiesOnLevelConstraint - this.NumOfLiveZombies);
			numOfZombsToAdd = Mathf.Min(numOfZombsToAdd, ZombieCreator.NumOfEnemisesToKill - (this.NumOfDeadZombies + this.NumOfLiveZombies));
			string[] enemyNumbers = null;
			if (Defs.IsSurvival)
			{
				int ind = (this.currentWave < ZombieCreator._enemiesInWaves.Count) ? this.currentWave : (ZombieCreator._enemiesInWaves.Count - 1);
				enemyNumbers = ZombieCreator._enemiesInWaves[ind].ToArray();
			}
			else if (CurrentCampaignGame.currentLevel == 0)
			{
				enemyNumbers = new string[]
				{
					"1"
				};
			}
			else
			{
				enemyNumbers = this._enemies[CurrentCampaignGame.currentLevel - 1];
			}
			for (int i = 0; i < numOfZombsToAdd; i++)
			{
				int typeOfZomb = UnityEngine.Random.Range(0, enemyNumbers.Length);
				GameObject spawnZone = (!Defs.IsSurvival) ? this._enemyCreationZones[UnityEngine.Random.Range(0, this._enemyCreationZones.Length)] : this._enemyCreationZones[i % this._enemyCreationZones.Length];
				Vector3 pos = this._createPos(spawnZone);
				UnityEngine.Object.Instantiate((!Defs.IsSurvival) ? this.zombiePrefabs[typeOfZomb] : this.waveZombiePrefabs[typeOfZomb], pos, Quaternion.identity);
			}
			if (Defs.IsSurvival && this.NumOfDeadZombies + this.NumOfLiveZombies >= ZombieCreator.NumOfEnemisesToKill)
			{
				this._generatingZombiesIsStopped = true;
				do
				{
					yield return new WaitForEndOfFrame();
				}
				while (this._generatingZombiesIsStopped);
			}
			yield return new WaitForSeconds(this.curInterval);
			if (Defs.IsSurvival)
			{
				this._genWithThisTimeInterval++;
				if (this._genWithThisTimeInterval == 3 && this._indexInTimesArray < this._intervalArr.Length - 1)
				{
					this._indexInTimesArray++;
				}
				this.curInterval = (float)this._intervalArr[this._indexInTimesArray];
			}
		}
		while (this.NumOfDeadZombies + this.NumOfLiveZombies < ZombieCreator.NumOfEnemisesToKill || Defs.IsSurvival);
		yield break;
	}

	// Token: 0x0600457B RID: 17787 RVA: 0x00177CB0 File Offset: 0x00175EB0
	private Vector3 _createPos(GameObject spawnZone)
	{
		BoxCollider component = spawnZone.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * spawnZone.transform.localScale.x, component.size.z * spawnZone.transform.localScale.z);
		Rect rect = new Rect(spawnZone.transform.position.x - vector.x / 2f, spawnZone.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 result = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), spawnZone.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		return result;
	}

	// Token: 0x0600457C RID: 17788 RVA: 0x00177DBC File Offset: 0x00175FBC
	private void ShowGuards(Vector3 bossPosition)
	{
		if (this.bossGuads == null)
		{
			return;
		}
		for (int i = 0; i < this.bossGuads.Length; i++)
		{
			this.bossGuads[i].transform.position = BaseBot.GetPositionSpawnGuard(bossPosition);
			this.bossGuads[i].transform.rotation = Quaternion.identity;
			this.bossGuads[i].SetActive(true);
		}
	}

	// Token: 0x0600457D RID: 17789 RVA: 0x00177E2C File Offset: 0x0017602C
	private void CreateBoss()
	{
		GameObject gameObject = null;
		float num = float.PositiveInfinity;
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
		if (!gameObject2)
		{
			return;
		}
		foreach (GameObject gameObject3 in this._enemyCreationZones)
		{
			float num2 = Vector3.SqrMagnitude(gameObject2.transform.position - gameObject3.transform.position);
			float num3 = Mathf.Abs(gameObject2.transform.position.y - gameObject3.transform.position.y);
			if (num2 > 225f && num2 < num && num3 < 2.5f)
			{
				num = num2;
				gameObject = gameObject3;
			}
		}
		if (!gameObject)
		{
			gameObject = this._enemyCreationZones[0];
		}
		Vector3 vector = this._createPos(gameObject);
		if (this.boss != null)
		{
			GameObject gameObject4 = GameObject.FindGameObjectWithTag("BossRespawnPoint");
			if (gameObject4 != null)
			{
				vector = gameObject4.transform.position;
			}
			this.boss.transform.position = vector;
			this.boss.transform.rotation = Quaternion.identity;
			this.boss.SetActive(true);
			this.ShowGuards(vector);
		}
		else if (Defs.IsSurvival && ZombieCreator.bossesSurvival.ContainsKey(this.currentWave))
		{
			string b = "Boss" + ZombieCreator.bossesSurvival[this.currentWave];
			this.boss = (UnityEngine.Object.Instantiate(Resources.Load(ResPath.Combine("Bosses", b))) as GameObject);
			this.boss.transform.position = vector;
			this.boss.transform.rotation = Quaternion.identity;
		}
		if (this.boss != null && !Defs.IsSurvival)
		{
			PlayerArrowToPortalController component = WeaponManager.sharedManager.myPlayer.GetComponent<PlayerArrowToPortalController>();
			if (component != null)
			{
				component.RemovePointOfInterest();
				component.SetPointOfInterest(this.boss.transform, Color.red);
			}
		}
		this.boss = null;
		this.bossShowm = true;
	}

	// Token: 0x040032DC RID: 13020
	private GameObject boss;

	// Token: 0x040032DD RID: 13021
	public GameObject weaponBonus;

	// Token: 0x040032DE RID: 13022
	public static ZombieCreator sharedCreator = null;

	// Token: 0x040032DF RID: 13023
	private static int _ZombiesInWave = 45;

	// Token: 0x040032E0 RID: 13024
	public int currentWave;

	// Token: 0x040032E1 RID: 13025
	private static List<List<string>> _enemiesInWaves = new List<List<string>>();

	// Token: 0x040032E2 RID: 13026
	private static readonly HashSet<string> _allEnemiesSurvival = new HashSet<string>();

	// Token: 0x040032E3 RID: 13027
	public List<GameObject> waveZombiePrefabs = new List<GameObject>();

	// Token: 0x040032E4 RID: 13028
	public static Dictionary<int, int> bossesSurvival = new Dictionary<int, int>();

	// Token: 0x040032E5 RID: 13029
	private static List<List<string>> _WeaponsAddedInWaves = new List<List<string>>();

	// Token: 0x040032E6 RID: 13030
	public static List<string> survivalAvailableWeapons = new List<string>();

	// Token: 0x040032E7 RID: 13031
	private bool _generatingZombiesIsStopped;

	// Token: 0x040032E8 RID: 13032
	private int totalNumOfKilledEnemies;

	// Token: 0x040032E9 RID: 13033
	private AudioClip bossMus;

	// Token: 0x040032EA RID: 13034
	private static int? _enemyCountInSurvivalWave;

	// Token: 0x040032EB RID: 13035
	public GUIStyle labelStyle;

	// Token: 0x040032EC RID: 13036
	private int[] _intervalArr = new int[]
	{
		6,
		4,
		3
	};

	// Token: 0x040032ED RID: 13037
	private int _genWithThisTimeInterval;

	// Token: 0x040032EE RID: 13038
	private int _indexInTimesArray;

	// Token: 0x040032EF RID: 13039
	private string _msg = string.Empty;

	// Token: 0x040032F0 RID: 13040
	private GameObject[] _teleports;

	// Token: 0x040032F1 RID: 13041
	public bool bossShowm;

	// Token: 0x040032F2 RID: 13042
	public bool stopGeneratingBonuses;

	// Token: 0x040032F3 RID: 13043
	public List<GameObject> zombiePrefabs = new List<GameObject>();

	// Token: 0x040032F4 RID: 13044
	private bool _isMultiplayer;

	// Token: 0x040032F5 RID: 13045
	private SaltedInt _numOfLiveZombies = 0;

	// Token: 0x040032F6 RID: 13046
	private SaltedInt _numOfDeadZombies = 0;

	// Token: 0x040032F7 RID: 13047
	private SaltedInt _numOfDeadZombsSinceLastFast = 0;

	// Token: 0x040032F8 RID: 13048
	public float curInterval = 10f;

	// Token: 0x040032F9 RID: 13049
	private GameObject[] _enemyCreationZones;

	// Token: 0x040032FA RID: 13050
	private List<string[]> _enemies = new List<string[]>();
}
