using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using com.amazon.device.iap.cpt;
using Holoville.HOTween;
using Prime31;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using RilisoftBot;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000484 RID: 1156
public sealed class Player_move_c : MonoBehaviour
{
	// Token: 0x06002829 RID: 10281 RVA: 0x000C9DDC File Offset: 0x000C7FDC
	public Player_move_c()
	{
		this.weKillForKillRate = new Dictionary<string, int>();
		this.weWereKilledForKillRate = new Dictionary<string, int>();
		base..ctor();
	}

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x0600282B RID: 10283 RVA: 0x000CA1D0 File Offset: 0x000C83D0
	// (remove) Token: 0x0600282C RID: 10284 RVA: 0x000CA1E8 File Offset: 0x000C83E8
	public static event Action StopBlinkShop;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x0600282D RID: 10285 RVA: 0x000CA200 File Offset: 0x000C8400
	// (remove) Token: 0x0600282E RID: 10286 RVA: 0x000CA21C File Offset: 0x000C841C
	public event Action<bool> OnMyImmortalyChanged;

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x0600282F RID: 10287 RVA: 0x000CA238 File Offset: 0x000C8438
	// (remove) Token: 0x06002830 RID: 10288 RVA: 0x000CA250 File Offset: 0x000C8450
	public static event Action OnMyPlayerMoveCCreated;

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06002831 RID: 10289 RVA: 0x000CA268 File Offset: 0x000C8468
	// (remove) Token: 0x06002832 RID: 10290 RVA: 0x000CA280 File Offset: 0x000C8480
	public static event Action<float> OnMyPlayerMoveCDestroyed;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06002833 RID: 10291 RVA: 0x000CA298 File Offset: 0x000C8498
	// (remove) Token: 0x06002834 RID: 10292 RVA: 0x000CA2B4 File Offset: 0x000C84B4
	public event Player_move_c.OnMessagesUpdate messageDelegate;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06002835 RID: 10293 RVA: 0x000CA2D0 File Offset: 0x000C84D0
	// (remove) Token: 0x06002836 RID: 10294 RVA: 0x000CA2EC File Offset: 0x000C84EC
	public event EventHandler<EventArgs> WeaponChanged;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x06002837 RID: 10295 RVA: 0x000CA308 File Offset: 0x000C8508
	// (remove) Token: 0x06002838 RID: 10296 RVA: 0x000CA324 File Offset: 0x000C8524
	public event Action<float> FreezerFired;

	// Token: 0x1400002E RID: 46
	// (add) Token: 0x06002839 RID: 10297 RVA: 0x000CA340 File Offset: 0x000C8540
	// (remove) Token: 0x0600283A RID: 10298 RVA: 0x000CA35C File Offset: 0x000C855C
	public event Action OnMyKillMechInDemon;

	// Token: 0x1400002F RID: 47
	// (add) Token: 0x0600283B RID: 10299 RVA: 0x000CA378 File Offset: 0x000C8578
	// (remove) Token: 0x0600283C RID: 10300 RVA: 0x000CA394 File Offset: 0x000C8594
	public event Action OnMyPlayerResurected;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600283D RID: 10301 RVA: 0x000CA3B0 File Offset: 0x000C85B0
	// (remove) Token: 0x0600283E RID: 10302 RVA: 0x000CA3C8 File Offset: 0x000C85C8
	public static event Action<bool> OnMyShootingStateSchanged;

	// Token: 0x0600283F RID: 10303 RVA: 0x000CA3E0 File Offset: 0x000C85E0
	private void SaveKillRate()
	{
		try
		{
			if (this.isMulti && !Defs.isHunger && !Defs.isCOOP && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && !Defs.IsSurvival)
			{
				Action<Dictionary<string, int>, Dictionary<string, Dictionary<int, int>>> action = delegate(Dictionary<string, int> battleDict, Dictionary<string, Dictionary<int, int>> dictToDisk)
				{
					foreach (KeyValuePair<string, int> keyValuePair in battleDict)
					{
						if (dictToDisk.ContainsKey(keyValuePair.Key))
						{
							Dictionary<int, int> dictionary = dictToDisk[keyValuePair.Key];
							if (dictionary.ContainsKey(this.tierForKilledRate))
							{
								Dictionary<int, int> dictionary3;
								Dictionary<int, int> dictionary2 = dictionary3 = dictionary;
								int num;
								int key = num = this.tierForKilledRate;
								num = dictionary3[num];
								dictionary2[key] = num + keyValuePair.Value;
							}
							else
							{
								dictionary.Add(this.tierForKilledRate, keyValuePair.Value);
							}
						}
						else
						{
							dictToDisk.Add(keyValuePair.Key, new Dictionary<int, int>
							{
								{
									this.tierForKilledRate,
									keyValuePair.Value
								}
							});
						}
					}
				};
				action(this.weKillForKillRate, KillRateStatisticsManager.WeKillOld);
				action(this.weWereKilledForKillRate, KillRateStatisticsManager.WeWereKilledOld);
				Dictionary<string, object> obj = new Dictionary<string, object>
				{
					{
						"version",
						GlobalGameController.AppVersion
					},
					{
						"wekill",
						KillRateStatisticsManager.WeKillOld
					},
					{
						"wewerekilled",
						KillRateStatisticsManager.WeWereKilledOld
					}
				};
				Storager.setString("KillRateKeyStatistics", Rilisoft.MiniJson.Json.Serialize(obj), false);
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in save kill rate statistics: " + arg);
		}
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x000CA4DC File Offset: 0x000C86DC
	private void AddWeKillStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			Debug.LogError("AddWeKillStatisctics string.IsNullOrEmpty (weaponName)");
			return;
		}
		if (this.weKillForKillRate.ContainsKey(weaponName))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.weKillForKillRate;
			int num = dictionary2[weaponName];
			dictionary[weaponName] = num + 1;
		}
		else
		{
			this.weKillForKillRate.Add(weaponName, 1);
		}
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x000CA540 File Offset: 0x000C8740
	private void AddWeWereKilledStatisctics(string weaponName)
	{
		if (string.IsNullOrEmpty(weaponName))
		{
			Debug.LogError("AddWeWereKilledStatisctics string.IsNullOrEmpty (weaponName)");
			return;
		}
		if (this.weWereKilledForKillRate.ContainsKey(weaponName))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.weWereKilledForKillRate;
			int num = dictionary2[weaponName];
			dictionary[weaponName] = num + 1;
		}
		else
		{
			this.weWereKilledForKillRate.Add(weaponName, 1);
		}
	}

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x06002842 RID: 10306 RVA: 0x000CA5A4 File Offset: 0x000C87A4
	// (set) Token: 0x06002843 RID: 10307 RVA: 0x000CA5AC File Offset: 0x000C87AC
	public bool isPlacemarker
	{
		get
		{
			return this._isPlacemarker;
		}
		set
		{
			this._isPlacemarker = value;
			this.placemarkerMark.SetActive(value);
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x06002844 RID: 10308 RVA: 0x000CA5C4 File Offset: 0x000C87C4
	public Animation mechGunAnimation
	{
		get
		{
			if (this.currentMech != null)
			{
				return this.currentMech.gunAnimation;
			}
			return null;
		}
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x06002845 RID: 10309 RVA: 0x000CA5E4 File Offset: 0x000C87E4
	// (set) Token: 0x06002846 RID: 10310 RVA: 0x000CA5EC File Offset: 0x000C87EC
	public int CurrentWeaponBeforeGrenade
	{
		get
		{
			return this.currentWeaponBeforeGrenade;
		}
		set
		{
			this.currentWeaponBeforeGrenade = value;
		}
	}

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x06002847 RID: 10311 RVA: 0x000CA5F8 File Offset: 0x000C87F8
	// (set) Token: 0x06002848 RID: 10312 RVA: 0x000CA600 File Offset: 0x000C8800
	public float koofDamageWeaponFromPotoins
	{
		get
		{
			return this._koofDamageWeaponFromPotoins;
		}
		set
		{
			this._koofDamageWeaponFromPotoins = value;
		}
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06002849 RID: 10313 RVA: 0x000CA60C File Offset: 0x000C880C
	private float maxTimerRegenerationArmor
	{
		get
		{
			return EffectsController.RegeneratingArmorTime;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x0600284A RID: 10314 RVA: 0x000CA614 File Offset: 0x000C8814
	public float[] byCatDamageModifs
	{
		get
		{
			return this._byCatDamageModifs;
		}
	}

	// Token: 0x1700070F RID: 1807
	// (get) Token: 0x0600284B RID: 10315 RVA: 0x000CA61C File Offset: 0x000C881C
	// (set) Token: 0x0600284C RID: 10316 RVA: 0x000CA624 File Offset: 0x000C8824
	public int myCommand
	{
		get
		{
			return this._myCommand;
		}
		set
		{
			this._myCommand = value;
			this.UpdateNickLabelColor();
		}
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000CA634 File Offset: 0x000C8834
	private void UpdateNickLabelColor()
	{
		if (ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
		{
			if (WeaponManager.sharedManager.myNetworkStartTable == null || WeaponManager.sharedManager.myNetworkStartTable.myCommand == 0)
			{
				if (this._nickColorInd != 0)
				{
					this.nickLabel.color = Color.white;
					this._nickColorInd = 0;
				}
			}
			else if (WeaponManager.sharedManager.myNetworkStartTable.myCommand == this.myCommand)
			{
				if (this._nickColorInd != 1)
				{
					this.nickLabel.color = Color.blue;
					this._nickColorInd = 1;
				}
			}
			else if (this._nickColorInd != 2)
			{
				this.nickLabel.color = Color.red;
				this._nickColorInd = 2;
			}
		}
		else if (Defs.isDaterRegim)
		{
			if (this._nickColorInd != 0)
			{
				this.nickLabel.color = Color.white;
				this._nickColorInd = 0;
			}
		}
		else if (Defs.isCOOP)
		{
			if (this._nickColorInd != 1)
			{
				this.nickLabel.color = Color.blue;
				this._nickColorInd = 1;
			}
		}
		else if (this._nickColorInd != 2)
		{
			this.nickLabel.color = Color.red;
			this._nickColorInd = 2;
		}
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x0600284E RID: 10318 RVA: 0x000CA7A4 File Offset: 0x000C89A4
	// (set) Token: 0x0600284F RID: 10319 RVA: 0x000CA7B4 File Offset: 0x000C89B4
	public int countKills
	{
		get
		{
			return this._killCount.Value;
		}
		set
		{
			this._killCount.Value = value;
		}
	}

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06002850 RID: 10320 RVA: 0x000CA7C4 File Offset: 0x000C89C4
	// (set) Token: 0x06002851 RID: 10321 RVA: 0x000CA7CC File Offset: 0x000C89CC
	public bool isImmortality
	{
		get
		{
			return this._isImmortalyVal;
		}
		set
		{
			if (this.isMine)
			{
				bool isImmortalyVal = this._isImmortalyVal;
				this._isImmortalyVal = value;
				if (isImmortalyVal != this._isImmortalyVal && this.OnMyImmortalyChanged != null)
				{
					this.OnMyImmortalyChanged(value);
				}
			}
			else
			{
				this._isImmortalyVal = value;
			}
		}
	}

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06002852 RID: 10322 RVA: 0x000CA824 File Offset: 0x000C8A24
	public KillerInfo killerInfo
	{
		get
		{
			return this._killerInfo;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06002853 RID: 10323 RVA: 0x000CA82C File Offset: 0x000C8A2C
	// (set) Token: 0x06002854 RID: 10324 RVA: 0x000CA834 File Offset: 0x000C8A34
	internal static bool NeedApply { get; set; }

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06002855 RID: 10325 RVA: 0x000CA83C File Offset: 0x000C8A3C
	// (set) Token: 0x06002856 RID: 10326 RVA: 0x000CA844 File Offset: 0x000C8A44
	internal static bool AnotherNeedApply { get; set; }

	// Token: 0x06002857 RID: 10327 RVA: 0x000CA84C File Offset: 0x000C8A4C
	public void IndicateDamage()
	{
		this.isDeadFrame = true;
		base.Invoke("setisDeadFrameFalse", 1f);
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06002858 RID: 10328 RVA: 0x000CA868 File Offset: 0x000C8A68
	private float maxBaseArmor
	{
		get
		{
			return 9f + EffectsController.ArmorBonus;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06002859 RID: 10329 RVA: 0x000CA878 File Offset: 0x000C8A78
	// (set) Token: 0x0600285A RID: 10330 RVA: 0x000CA880 File Offset: 0x000C8A80
	private float CurrentBaseArmor
	{
		get
		{
			return this._curBaseArmor;
		}
		set
		{
			this._curBaseArmor = value;
		}
	}

	// Token: 0x0600285B RID: 10331 RVA: 0x000CA88C File Offset: 0x000C8A8C
	private void AddArmor(float dt)
	{
		bool flag = this.WearedMaxArmor > 0f;
		if (flag)
		{
			float num = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN, false), this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
			float num2 = num - this.CurrentBodyArmor;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			if (dt <= num2)
			{
				this.CurrentBodyArmor += dt;
			}
			else
			{
				this.CurrentBodyArmor += num2;
				dt -= num2;
				float num3 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN, false), this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
				float num4 = num3 - this.CurrentHatArmor;
				if (num4 < 0f)
				{
					num4 = 0f;
				}
				this.CurrentHatArmor += Mathf.Min(num4, dt);
			}
		}
		else
		{
			float num5 = this.maxBaseArmor - this.CurrentBaseArmor;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			if (dt <= num5)
			{
				this.CurrentBaseArmor += dt;
			}
			else
			{
				this.CurrentBaseArmor += num5;
			}
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x0600285C RID: 10332 RVA: 0x000CA9F8 File Offset: 0x000C8BF8
	// (set) Token: 0x0600285D RID: 10333 RVA: 0x000CAA00 File Offset: 0x000C8C00
	public bool isInappWinOpen
	{
		get
		{
			return this._isInappWinOpen;
		}
		set
		{
			this._isInappWinOpen = value;
			ShopNGUIController.GuiActive = value;
			if (this.myCurrentWeaponSounds != null)
			{
				if (PauseGUIController.Instance != null && PauseGUIController.Instance.IsPaused)
				{
					this.myCurrentWeaponSounds.animationObject.SetActive(false);
				}
				else
				{
					this.myCurrentWeaponSounds.animationObject.SetActive(!value);
				}
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
				{
					if (value)
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
					}
					else
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
					}
				}
			}
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x0600285E RID: 10334 RVA: 0x000CAAC4 File Offset: 0x000C8CC4
	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x0600285F RID: 10335 RVA: 0x000CAAD8 File Offset: 0x000C8CD8
	// (set) Token: 0x06002860 RID: 10336 RVA: 0x000CAAE0 File Offset: 0x000C8CE0
	public WeaponManager _weaponManager
	{
		get
		{
			return this.___weaponManager;
		}
		set
		{
			this.___weaponManager = value;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06002861 RID: 10337 RVA: 0x000CAAEC File Offset: 0x000C8CEC
	// (set) Token: 0x06002862 RID: 10338 RVA: 0x000CAAFC File Offset: 0x000C8CFC
	public int countKillsCommandBlue
	{
		get
		{
			return this._countKillsCommandBlue.Value;
		}
		set
		{
			this._countKillsCommandBlue.Value = value;
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06002863 RID: 10339 RVA: 0x000CAB0C File Offset: 0x000C8D0C
	// (set) Token: 0x06002864 RID: 10340 RVA: 0x000CAB1C File Offset: 0x000C8D1C
	public int countKillsCommandRed
	{
		get
		{
			return this._countKillsCommandRed.Value;
		}
		set
		{
			this._countKillsCommandRed.Value = value;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06002865 RID: 10341 RVA: 0x000CAB2C File Offset: 0x000C8D2C
	// (set) Token: 0x06002866 RID: 10342 RVA: 0x000CAB34 File Offset: 0x000C8D34
	public NetworkView _networkView { get; set; }

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06002867 RID: 10343 RVA: 0x000CAB40 File Offset: 0x000C8D40
	private Material mainDamageMaterial
	{
		get
		{
			if (this.isMechActive)
			{
				this.curMainSelect = this._mechMaterial;
				return this._mechMaterial;
			}
			if (this.isBearActive)
			{
				this.curMainSelect = this._bearMaterial;
				return this._bearMaterial;
			}
			this.curMainSelect = this._bodyMaterial;
			return this._bodyMaterial;
		}
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x000CAB9C File Offset: 0x000C8D9C
	private void Awake()
	{
		this.isSurvival = Defs.IsSurvival;
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.isCompany = Defs.isCompany;
		this.isCOOP = Defs.isCOOP;
		this.isHunger = Defs.isHunger;
		if (this.isHunger)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("HungerGameController");
			if (gameObject == null)
			{
				Debug.LogError("hungerGameControllerObject == null");
			}
			else
			{
				this.hungerGameController = gameObject.GetComponent<HungerGameController>();
			}
		}
		this.myAudioSource = base.GetComponent<AudioSource>();
		this.myCamera.fieldOfView = this.stdFov;
		this.myDamageable = this.mySkinName.GetComponent<PlayerDamageable>();
		this.skinNamePixelView = this.mySkinName.GetComponent<PixelView>();
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06002869 RID: 10345 RVA: 0x000CAC68 File Offset: 0x000C8E68
	public bool IsPlayerFlying
	{
		get
		{
			return this.isPlayerFlying;
		}
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000CAC70 File Offset: 0x000C8E70
	public void ActivateJetpackGadget(bool isEnabled)
	{
		if (!Defs.isMulti || this.isMine)
		{
			this.SetJetpackEnabled(isEnabled);
		}
		else
		{
			this.SetJetpackEnabledRPC(isEnabled);
		}
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000CACA8 File Offset: 0x000C8EA8
	public void SetJetpackEnabled(bool _isEnabled)
	{
		Defs.isJetpackEnabled = _isEnabled;
		if (Defs.isSoundFX && _isEnabled)
		{
			AudioSource component = base.GetComponent<AudioSource>();
			if (component != null)
			{
				component.PlayOneShot(this.jetpackActivSound);
			}
		}
		if (Defs.isDaterRegim && Defs.isMulti)
		{
			if (Defs.isInet)
			{
				if (this.photonView != null)
				{
					this.photonView.RPC("SetJetpackEnabledRPC", PhotonTargets.Others, new object[]
					{
						_isEnabled
					});
				}
			}
			else if (this._networkView != null)
			{
				this._networkView.RPC("SetJetpackEnabledRPC", RPCMode.Others, new object[]
				{
					_isEnabled
				});
			}
		}
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000CAD70 File Offset: 0x000C8F70
	[PunRPC]
	[RPC]
	public void SetJetpackEnabledRPC(bool _isEnabled)
	{
		this.jetpackEnabled = _isEnabled;
		if (Defs.isSoundFX && _isEnabled)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.jetpackActivSound);
		}
		if (Defs.isDaterRegim)
		{
			this.wingsPoint.SetActive(_isEnabled);
			this.wingsPointBear.SetActive(_isEnabled);
		}
		else
		{
			this.jetPackPoint.SetActive(_isEnabled);
			if (this.isMechActive && this.currentMech != null)
			{
				this.currentMech.jetpackObject.SetActive(_isEnabled);
			}
		}
		if (!_isEnabled)
		{
			for (int i = 0; i < this.jetPackParticle.Length; i++)
			{
				this.jetPackParticle[i].enableEmission = _isEnabled;
			}
		}
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000CAE34 File Offset: 0x000C9034
	public void SetJetpackParticleEnabled(bool _isEnabled)
	{
		if (_isEnabled)
		{
			this.isPlayerFlying = true;
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(!this.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon));
			}
		}
		else
		{
			this.isPlayerFlying = false;
			if (!Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(false);
			}
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SetJetpackParticleEnabledRPC", PhotonTargets.Others, new object[]
				{
					_isEnabled
				});
			}
			else
			{
				this._networkView.RPC("SetJetpackParticleEnabledRPC", RPCMode.Others, new object[]
				{
					_isEnabled
				});
			}
		}
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000CAF04 File Offset: 0x000C9104
	[RPC]
	[PunRPC]
	public void SetJetpackParticleEnabledRPC(bool _isEnabled)
	{
		if (_isEnabled)
		{
			this.isPlayerFlying = true;
			if (ButtonClickSound.Instance != null && Defs.isSoundFX && !Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(!this.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon));
			}
		}
		else
		{
			this.isPlayerFlying = false;
			if (!Defs.isDaterRegim)
			{
				this.jetPackSound.SetActive(false);
			}
		}
		for (int i = 0; i < this.jetPackParticle.Length; i++)
		{
			this.jetPackParticle[i].enableEmission = _isEnabled;
		}
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000CAFA0 File Offset: 0x000C91A0
	[RPC]
	[PunRPC]
	private void SendChatMessageWithIcon(string text, bool _clanMode, string _clanLogo, string _ClanID, string _clanName, string _iconName)
	{
		if (_clanMode && !_ClanID.Equals(FriendsController.sharedController.ClanID))
		{
			return;
		}
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayerMoveC == null)
		{
			return;
		}
		if (!this.isInet)
		{
			this._weaponManager.myPlayerMoveC.AddMessage(text, Time.time, -1, this.myPlayerTransform.GetComponent<NetworkView>().viewID, 0, _clanLogo, _iconName);
		}
		else
		{
			this._weaponManager.myPlayerMoveC.AddMessage(text, Time.time, this.mySkinName.photonView.viewID, this.myPlayerTransform.GetComponent<NetworkView>().viewID, this.myCommand, _clanLogo, _iconName);
		}
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000CB070 File Offset: 0x000C9270
	[RPC]
	[PunRPC]
	private void SendChatMessage(string text, bool _clanMode, string _clanLogo, string _ClanID, string _clanName)
	{
		this.SendChatMessageWithIcon(text, _clanMode, _clanLogo, _ClanID, _clanName, string.Empty);
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000CB084 File Offset: 0x000C9284
	public void SendChat(string text, bool clanMode, string iconName)
	{
		if (text.Equals("-=ATTACK!=-"))
		{
			text = LocalizationStore.Get("Key_1086");
		}
		else if (text.Equals("-=HELP!=-"))
		{
			text = LocalizationStore.Get("Key_1087");
		}
		else if (text.Equals("-=OK!=-"))
		{
			text = LocalizationStore.Get("Key_1088");
		}
		else if (text.Equals("-=NO!=-"))
		{
			text = LocalizationStore.Get("Key_1089");
		}
		else
		{
			text = FilterBadWorld.FilterString(text);
		}
		if (!string.IsNullOrEmpty(text) || !string.IsNullOrEmpty(iconName))
		{
			if (!this.isInet)
			{
				this._networkView.RPC("SendChatMessageWithIcon", RPCMode.All, new object[]
				{
					"< " + this._weaponManager.myNetworkStartTable.NamePlayer + " > " + text,
					clanMode,
					FriendsController.sharedController.clanLogo,
					FriendsController.sharedController.ClanID,
					FriendsController.sharedController.clanName,
					iconName
				});
			}
			else
			{
				this.photonView.RPC("SendChatMessageWithIcon", PhotonTargets.All, new object[]
				{
					"< " + this._weaponManager.myNetworkStartTable.NamePlayer + " > " + text,
					clanMode,
					FriendsController.sharedController.clanLogo,
					FriendsController.sharedController.ClanID,
					FriendsController.sharedController.clanName,
					iconName
				});
			}
		}
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x000CB220 File Offset: 0x000C9420
	public void SendDaterChat(string nick1, string text, string nick2)
	{
		if (text != string.Empty)
		{
			if (!this.isInet)
			{
				this._networkView.RPC("SendDaterChatRPC", RPCMode.All, new object[]
				{
					nick1,
					text,
					nick2,
					false,
					FriendsController.sharedController.clanLogo,
					FriendsController.sharedController.ClanID,
					FriendsController.sharedController.clanName
				});
			}
			else
			{
				this.photonView.RPC("SendDaterChatRPC", PhotonTargets.All, new object[]
				{
					nick1,
					text,
					nick2,
					false,
					FriendsController.sharedController.clanLogo,
					FriendsController.sharedController.ClanID,
					FriendsController.sharedController.clanName
				});
			}
		}
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x000CB2F4 File Offset: 0x000C94F4
	[RPC]
	[PunRPC]
	public void SendDaterChatRPC(string nick1, string text, string nick2, bool _clanMode, string _clanLogo, string _ClanID, string _clanName)
	{
		text = string.Concat(new string[]
		{
			"< ",
			nick1,
			"[-] > ",
			LocalizationStore.Get(text),
			" < ",
			nick2,
			"[-] >"
		});
		this.SendChatMessage(text, _clanMode, _clanLogo, _ClanID, _clanName);
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000CB350 File Offset: 0x000C9550
	public void AddMessage(string text, float time, int ID, NetworkViewID IDLocal, int _command, string clanLogo, string iconName)
	{
		Player_move_c.MessageChat item = default(Player_move_c.MessageChat);
		item.text = text;
		item.iconName = iconName;
		item.time = time;
		item.ID = ID;
		item.IDLocal = IDLocal;
		item.command = _command;
		if (!string.IsNullOrEmpty(clanLogo))
		{
			byte[] data = Convert.FromBase64String(clanLogo);
			Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
			texture2D.LoadImage(data);
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			item.clanLogo = texture2D;
		}
		else
		{
			item.clanLogo = null;
		}
		this.messages.Add(item);
		if (this.messages.Count > 30)
		{
			this.messages.RemoveAt(0);
		}
		Player_move_c.OnMessagesUpdate onMessagesUpdate = this.messageDelegate;
		if (onMessagesUpdate != null)
		{
			onMessagesUpdate();
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000CB424 File Offset: 0x000C9624
	public void WalkAnimation()
	{
		if (!this._singleOrMultiMine() && (!Defs.isDaterRegim || !this.isBearActive))
		{
			return;
		}
		if (!Defs.isDaterRegim && this.isMechActive && !this.mechGunAnimation.IsPlaying("Shoot") && !this.mechGunAnimation.IsPlaying("Shoot1") && !this.mechGunAnimation.IsPlaying("Shoot2"))
		{
			this.mechGunAnimation.CrossFade("Walk");
		}
		if (this.___weaponManager.currentWeaponSounds.isCharging && this.chargeValue > 0f)
		{
			return;
		}
		if (this._weaponManager && this._weaponManager.currentWeaponSounds && this._weaponManager.currentWeaponSounds.animationObject != null)
		{
			if (this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Peg") != null && this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Peg"))
			{
				return;
			}
			this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Walk");
		}
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000CB58C File Offset: 0x000C978C
	public void IdleAnimation()
	{
		if (!this._singleOrMultiMine() && (!Defs.isDaterRegim || !this.isBearActive))
		{
			return;
		}
		if (!Defs.isDaterRegim && this.isMechActive && !this.mechGunAnimation.IsPlaying("Shoot") && !this.mechGunAnimation.IsPlaying("Shoot1") && !this.mechGunAnimation.IsPlaying("Shoot2"))
		{
			this.mechGunAnimation.CrossFade("Idle");
		}
		if (this.___weaponManager.currentWeaponSounds.isCharging && this.chargeValue > 0f)
		{
			return;
		}
		if (this.___weaponManager && this.___weaponManager.currentWeaponSounds && this.___weaponManager.currentWeaponSounds.animationObject != null)
		{
			if (this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Peg") != null && this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Peg"))
			{
				return;
			}
			this.___weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().CrossFade("Idle");
		}
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x000CB6F4 File Offset: 0x000C98F4
	public void ZoomPress()
	{
		if (WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			this.showZoomHint = false;
			HintController.instance.HideHintByName("use_zoom");
		}
		this.isZooming = !this.isZooming;
		if (this.isZooming)
		{
			if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.zoomIn != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.zoomIn);
			}
			this.myCamera.fieldOfView = this._weaponManager.currentWeaponSounds.fieldOfViewZomm;
			this.gunCamera.gameObject.SetActive(false);
			this.inGameGUI.SetScopeForWeapon(this._weaponManager.currentWeaponSounds.scopeNum.ToString());
			this.myTransform.localPosition = new Vector3(this.myTransform.localPosition.x, this.myTransform.localPosition.y, this.myTransform.localPosition.z);
		}
		else
		{
			if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.zoomOut != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.zoomOut);
			}
			this.myCamera.fieldOfView = this.stdFov;
			this.gunCamera.fieldOfView = 75f;
			this.gunCamera.gameObject.SetActive(true);
			if (this.inGameGUI != null)
			{
				this.inGameGUI.ResetScope();
			}
		}
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("SynhIsZoming", PhotonTargets.All, new object[]
			{
				this.isZooming
			});
		}
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x000CB900 File Offset: 0x000C9B00
	[PunRPC]
	[RPC]
	private void SynhIsZoming(bool _isZoomming)
	{
		this.isZooming = _isZoomming;
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x000CB90C File Offset: 0x000C9B0C
	public void hideGUI()
	{
		this.showGUI = false;
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x000CB918 File Offset: 0x000C9B18
	public void setMyTamble(GameObject _myTable)
	{
		if (this.myTable == null || _myTable == null)
		{
			return;
		}
		NetworkStartTable component = this.myTable.GetComponent<NetworkStartTable>();
		if (component == null)
		{
			return;
		}
		component.myPlayerMoveC = this;
		this.myTable = _myTable;
		this.myNetworkStartTable = this.myTable.GetComponent<NetworkStartTable>();
		if (this.myNetworkStartTable == null)
		{
			return;
		}
		this.CurHealth = this.MaxHealth;
		this.myCommand = this.myNetworkStartTable.myCommand;
		if (Initializer.redPlayers.Contains(this) && this.myCommand == 1)
		{
			Initializer.redPlayers.Remove(this);
		}
		if (Initializer.bluePlayers.Contains(this) && this.myCommand == 2)
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (this.myCommand == 1 && !Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Add(this);
		}
		if (this.myCommand == 2 && !Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Add(this);
		}
		this._skin = this.myNetworkStartTable.mySkin;
		this.SetTextureForBodyPlayer(this._skin);
		if (this.isMine)
		{
			if (ABTestController.useBuffSystem)
			{
				BuffSystem.instance.CheckForPlayerBuff();
			}
			else if (KillRateCheck.instance.buffEnabled)
			{
				this.SetupBuffParameters(KillRateCheck.instance.damageBuff, KillRateCheck.instance.healthBuff);
			}
			else
			{
				this.SetupBuffParameters(1f, 1f);
			}
		}
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x000CBAC8 File Offset: 0x000C9CC8
	public void SetupBuffParameters(float damage, float protection)
	{
		bool flag = this.damageBuff != damage || this.protectionBuff != protection;
		this.SetBuffParameters(damage, protection);
		if (flag && Defs.isMulti && Defs.isInet)
		{
			this.photonView.RPC("SendBuffParameters", PhotonTargets.Others, new object[]
			{
				this.damageBuff,
				this.protectionBuff
			});
		}
	}

	// Token: 0x0600287C RID: 10364 RVA: 0x000CBB48 File Offset: 0x000C9D48
	private void SetBuffParameters(float damage, float protection)
	{
		this.damageBuff = Mathf.Clamp(damage, 0.01f, 10f);
		this.protectionBuff = Mathf.Clamp(protection, 0.01f, 10f);
		Debug.Log(string.Format("<color=green>{0}Damage: {1}, Protection: {2}</color>", (!this.isMine) ? ("(" + this.mySkinName.NickName + ") ") : "(you) ", this.damageBuff, this.protectionBuff));
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000CBBD8 File Offset: 0x000C9DD8
	[PunRPC]
	private void SendBuffParameters(float damage, float protection)
	{
		if (!this.isMine)
		{
			this.SetBuffParameters(damage, protection);
		}
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000CBBF0 File Offset: 0x000C9DF0
	public void AddWeapon(GameObject weaponPrefab)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			int num = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList<Weapon>().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor == weaponPrefab.GetComponent<WeaponSounds>().categoryNabor);
			if (num >= 0)
			{
				this.ChangeWeapon(num, false);
			}
			return;
		}
		WeaponSounds component = weaponPrefab.GetComponent<WeaponSounds>();
		if (component != null && WeaponManager.sharedManager != null && !component.IsAvalibleFromFilter(WeaponManager.sharedManager.CurrentFilterMap))
		{
			return;
		}
		int num2;
		if (Defs.isHunger && component != null && int.TryParse(component.nameNoClone().Substring("Weapon".Length), out num2) && num2 != 9 && !ChestController.weaponForHungerGames.Contains(num2))
		{
			return;
		}
		int num3;
		bool flag = this._weaponManager.AddWeapon(weaponPrefab, out num3);
		if (flag)
		{
			if (this.indexWeapon < 1000)
			{
				this.ChangeWeapon(this._weaponManager.CurrentWeaponIndex, false);
			}
		}
		else if (ItemDb.IsWeaponCanDrop(ItemDb.GetByPrefabName(weaponPrefab.name.Replace("(Clone)", string.Empty)).Tag))
		{
			GlobalGameController.Score += num3;
			if (Defs.isSoundFX)
			{
				if (this.WeaponBonusClip != null)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.WeaponBonusClip);
				}
				else
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.ChangeWeaponClip);
				}
			}
		}
		else if (this.indexWeapon < 1000)
		{
			foreach (object obj in this._weaponManager.playerWeapons)
			{
				Weapon weapon = (Weapon)obj;
				if (weapon.weaponPrefab == weaponPrefab)
				{
					this.ChangeWeapon(this._weaponManager.playerWeapons.IndexOf(weapon), false);
					break;
				}
			}
		}
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000CBE64 File Offset: 0x000CA064
	[RPC]
	[PunRPC]
	public void StartFlashRPC()
	{
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject, false));
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x000CBE80 File Offset: 0x000CA080
	public void SendStartFlashMine()
	{
		if (!this.isInet)
		{
			this._networkView.RPC("StartFlashRPC", RPCMode.All, new object[0]);
		}
		else
		{
			this.photonView.RPC("StartFlashRPC", PhotonTargets.All, new object[0]);
		}
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x000CBECC File Offset: 0x000CA0CC
	public void StartFlash(GameObject _obj)
	{
		base.StartCoroutine(this.Flash(_obj, false));
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000CBEE0 File Offset: 0x000CA0E0
	public static void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		int childCount = obj.transform.childCount;
		Transform transform = obj.transform;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(null == child))
			{
				Player_move_c.SetLayerRecursively(child.gameObject, newLayer);
			}
		}
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000CBF4C File Offset: 0x000CA14C
	public static void PerformActionRecurs(GameObject obj, Action<Transform> act)
	{
		if (act == null || null == obj)
		{
			return;
		}
		act(obj.transform);
		int childCount = obj.transform.childCount;
		Transform transform = obj.transform;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(null == child))
			{
				Player_move_c.PerformActionRecurs(child.gameObject, act);
			}
		}
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000CBFC4 File Offset: 0x000CA1C4
	private Weapon GetWeaponByIndex(int index)
	{
		return this._weaponManager.playerWeapons[index] as Weapon;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000CBFDC File Offset: 0x000CA1DC
	public void ChangeWeapon(int index, bool shouldSetMaxAmmo = true)
	{
		if (index == 1001)
		{
			this.currentWeaponBeforeTurret = WeaponManager.sharedManager.CurrentWeaponIndex;
		}
		this.indexWeapon = index;
		this.shouldSetMaxAmmoWeapon = shouldSetMaxAmmo;
		base.StopCoroutine("ChangeWeaponCorutine");
		base.StopCoroutine(this.BazookaShoot());
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine("ChangeWeaponCorutine");
		}
		if (base.GetComponent<AudioSource>() != null && !this.isMechActive)
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000CC06C File Offset: 0x000CA26C
	public void ResetWeaponChange()
	{
		this._changingWeapon = false;
		this.deltaAngle = 0f;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000CC080 File Offset: 0x000CA280
	private IEnumerator ChangeWeaponCorutine()
	{
		this._changingWeapon = true;
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StopAllCircularIndicators();
		}
		this.photonView.synchronization = ViewSynchronization.Off;
		this._networkView.stateSynchronization = NetworkStateSynchronization.Off;
		if (!Defs.isTurretWeapon)
		{
			while (this.deltaAngle < 40f && !Defs.isTurretWeapon && !this.isMechActive)
			{
				this.deltaAngle += 300f * Time.deltaTime;
				yield return null;
			}
		}
		else
		{
			if (!this.isMechActive)
			{
				this.deltaAngle = 40f;
			}
			Defs.isTurretWeapon = false;
		}
		GameObject nw = null;
		GameObject _weaponPrefab;
		string _innerPath;
		if (this.indexWeapon == 1000)
		{
			_weaponPrefab = (Resources.Load("GadgetsContent/" + this.currentGrenadeGadget.GrenadeGadgetId) as GameObject);
			_innerPath = ResPath.Combine(Defs.GadgetContentFolder, this.currentGrenadeGadget.GrenadeGadgetId + Defs.InnerWeapons_Suffix);
		}
		else if (this.indexWeapon == 1001)
		{
			_weaponPrefab = this.turretPrefab;
			_innerPath = ResPath.Combine(Defs.GadgetContentFolder, _weaponPrefab.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		}
		else
		{
			_weaponPrefab = this.GetWeaponByIndex(this.indexWeapon).weaponPrefab;
			_innerPath = ResPath.Combine(Defs.InnerWeaponsFolder, _weaponPrefab.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		}
		LoadAsyncTool.ObjectRequest weaponRequest = LoadAsyncTool.Get(_innerPath, false);
		while (!weaponRequest.isDone)
		{
			yield return null;
		}
		nw = (GameObject)UnityEngine.Object.Instantiate(_weaponPrefab, Vector3.zero, Quaternion.identity);
		nw.GetComponent<WeaponSounds>().Initialize(weaponRequest.asset as GameObject);
		this.ChangeWeaponReal(_weaponPrefab, nw, this.indexWeapon, this.shouldSetMaxAmmoWeapon);
		if (this.indexWeapon != 1001 && !this.isMechActive)
		{
			while (this.deltaAngle > 0f)
			{
				this.deltaAngle -= 300f * Time.deltaTime;
				if (this.deltaAngle < 0f)
				{
					this.deltaAngle = -0.01f;
				}
				yield return null;
			}
		}
		if (this.indexWeapon == 1001)
		{
			this.deltaAngle = 0f;
		}
		this.photonView.synchronization = ViewSynchronization.Unreliable;
		this._networkView.stateSynchronization = NetworkStateSynchronization.Unreliable;
		this._changingWeapon = false;
		yield break;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000CC09C File Offset: 0x000CA29C
	public void ChangeWeaponReal(int index, bool shouldSetMaxAmmo = true)
	{
		GameObject gameObject;
		string a;
		if (index == 1000)
		{
			gameObject = (Resources.Load(Defs.GadgetContentFolder + "/" + this.currentGrenadeGadget.GrenadeGadgetId) as GameObject);
			a = Defs.GadgetContentFolder;
		}
		else if (index == 1001)
		{
			gameObject = this.turretPrefab;
			a = Defs.GadgetContentFolder;
		}
		else
		{
			gameObject = this.GetWeaponByIndex(index).weaponPrefab;
			a = Defs.InnerWeaponsFolder;
		}
		string path = ResPath.Combine(a, gameObject.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
		GameObject pref = LoadAsyncTool.Get(path, true).asset as GameObject;
		GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity);
		gameObject2.GetComponent<WeaponSounds>().Initialize(pref);
		this.ChangeWeaponReal(gameObject, gameObject2, index, shouldSetMaxAmmo);
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000CC17C File Offset: 0x000CA37C
	public void ChangeWeaponReal(GameObject _weaponPrefab, GameObject nw, int index, bool shouldSetMaxAmmo = true)
	{
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StopAllCircularIndicators();
		}
		EventHandler<EventArgs> weaponChanged = this.WeaponChanged;
		if (weaponChanged != null)
		{
			weaponChanged(this, EventArgs.Empty);
		}
		if (this.isZooming)
		{
			this.ZoomPress();
		}
		this.photonView = PhotonView.Get(this);
		this._networkView = base.GetComponent<NetworkView>();
		Quaternion rotation = Quaternion.identity;
		if (this._player)
		{
			rotation = this._player.transform.rotation;
		}
		this.ShotUnPressed(true);
		if (this._weaponManager.currentWeaponSounds)
		{
			rotation = this._weaponManager.currentWeaponSounds.gameObject.transform.rotation;
			this._SetGunFlashActive(false);
			this._weaponManager.currentWeaponSounds.gameObject.transform.parent = null;
			UnityEngine.Object.Destroy(this._weaponManager.currentWeaponSounds.gameObject);
			this._weaponManager.currentWeaponSounds = null;
		}
		this.ResetShootingBurst();
		this.myCurrentWeapon = nw;
		this.myCurrentWeaponSounds = this.myCurrentWeapon.GetComponent<WeaponSounds>();
		if (!ShopNGUIController.GuiActive && this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		if (!this.isMechActive)
		{
			if (this.myCurrentWeaponSounds.isDoubleShot)
			{
				this.gunCamera.transform.localPosition = Vector3.zero;
			}
			else
			{
				this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
		}
		nw.transform.parent = base.gameObject.transform;
		nw.transform.rotation = rotation;
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
		if (this.isMechActive)
		{
			this.myCurrentWeapon.SetActive(false);
		}
		if (Defs.isDaterRegim)
		{
			this.SetWeaponVisible(!this.isBearActive);
		}
		if (this.myCurrentWeaponSounds != null && PhotonNetwork.room != null)
		{
			Statistics.Instance.IncrementWeaponPopularity(LocalizationStore.GetByDefault(this.myCurrentWeaponSounds.localizeWeaponKey), false);
			this._weaponPopularityCacheIsDirty = true;
		}
		WeaponSkin skinForWeapon = WeaponSkinsManager.GetSkinForWeapon(_weaponPrefab.nameNoClone());
		if (skinForWeapon != null)
		{
			skinForWeapon.SetTo(this.myCurrentWeaponSounds.gameObject);
		}
		if (this.isMulti)
		{
			string sendingSkinId = (skinForWeapon == null) ? string.Empty : skinForWeapon.Id;
			this._sendingNameWeapon = _weaponPrefab.name;
			this._sendingAlternativeNameWeapon = _weaponPrefab.GetComponent<WeaponSounds>().alternativeName;
			this._sendingSkinId = sendingSkinId;
			if (this.isInet)
			{
				this.photonView.RPC("SetWeaponRPC", PhotonTargets.Others, new object[]
				{
					this._sendingNameWeapon,
					this._sendingAlternativeNameWeapon,
					this._sendingSkinId
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetWeaponRPC", RPCMode.Others, new object[]
				{
					this._sendingNameWeapon,
					this._sendingAlternativeNameWeapon,
					this._sendingSkinId
				});
			}
		}
		if (index == 1000)
		{
			WeaponSounds component = _weaponPrefab.GetComponent<WeaponSounds>();
			if (this.currentGrenadeGadget != null)
			{
				this.currentGrenadeGadget.CreateRocket(component);
			}
		}
		if (index == 1001)
		{
			Defs.isTurretWeapon = true;
			string text = ResPath.Combine(Defs.GadgetContentFolder, this.turretGadgetPrefabName);
			GameObject gameObject;
			if (this.isMulti)
			{
				if (!this.isInet)
				{
					UnityEngine.Object prefab = Resources.Load(text);
					gameObject = (GameObject)Network.Instantiate(prefab, new Vector3(-10000f, -10000f, -10000f), Quaternion.identity, 0);
				}
				else
				{
					gameObject = PhotonNetwork.Instantiate(text, Vector3.down * -10000f, Quaternion.identity, 0);
				}
			}
			else
			{
				GameObject original = Resources.Load(text) as GameObject;
				gameObject = (UnityEngine.Object.Instantiate(original, new Vector3(-10000f, -10000f, -10000f), base.transform.rotation) as GameObject);
			}
			if (gameObject != null)
			{
				TurretController component2 = gameObject.GetComponent<TurretController>();
				gameObject.GetComponent<Rigidbody>().useGravity = false;
				gameObject.GetComponent<Rigidbody>().isKinematic = true;
				if (Defs.isMulti && !Defs.isInet)
				{
					component2.SendNetworkViewMyPlayer(this.myPlayerTransform.GetComponent<NetworkView>().viewID);
				}
			}
			this.currentTurret = gameObject;
		}
		if (!this.myCurrentWeaponSounds.isMelee)
		{
			foreach (object obj in nw.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name.Equals("BulletSpawnPoint") && transform.childCount > 0)
				{
					GameObject gameObject2 = transform.GetChild(0).gameObject;
					WeaponManager.SetGunFlashActive(gameObject2, false);
					break;
				}
			}
		}
		this.SetTextureForBodyPlayer(this._skin);
		Player_move_c.SetLayerRecursively(nw, 9);
		this._weaponManager.currentWeaponSounds = this.myCurrentWeaponSounds;
		if (index < 1000)
		{
			this._weaponManager.CurrentWeaponIndex = index;
			this._weaponManager.SaveWeaponAsLastUsed(this._weaponManager.CurrentWeaponIndex);
			if (this.inGameGUI != null)
			{
				if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee && !this.isMechActive)
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
				else
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
			}
		}
		if (nw.transform.parent == null)
		{
			Debug.LogWarning("nw.transform.parent == null");
		}
		else if (this._weaponManager.currentWeaponSounds == null)
		{
			Debug.LogWarning("_weaponManager.currentWeaponSounds == null");
		}
		else
		{
			nw.transform.position = nw.transform.parent.TransformPoint(this._weaponManager.currentWeaponSounds.gunPosition);
		}
		TouchPadController rightJoystick = JoystickController.rightJoystick;
		if (index < 1000 && rightJoystick != null)
		{
			if (this.GetWeaponByIndex(index).currentAmmoInClip > 0 || (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee))
			{
				rightJoystick.HasAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(0);
				}
			}
			else if (this.GetWeaponByIndex(index).currentAmmoInBackpack <= 0)
			{
				rightJoystick.NoAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(1);
				}
			}
		}
		if (this._weaponManager.currentWeaponSounds.animationObject != null)
		{
			if (this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
			}
			if (!this._weaponManager.currentWeaponSounds.isDoubleShot)
			{
				if (this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
				{
					this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
				}
			}
			else
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
			}
		}
		if (!this._weaponManager.currentWeaponSounds.isMelee)
		{
			foreach (object obj2 in this._weaponManager.currentWeaponSounds.gameObject.transform)
			{
				Transform transform2 = (Transform)obj2;
				if (transform2.name.Equals("BulletSpawnPoint"))
				{
					this._bulletSpawnPoint = transform2.gameObject;
					break;
				}
			}
			this.GunFlash = this._bulletSpawnPoint.transform.GetChild(0);
		}
		if (Defs.isSoundFX && !Defs.isDaterRegim && !this.isMechActive && index != 1000)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.ChangeWeaponClip);
		}
		if (!Defs.isDaterRegim && this.isInvisible)
		{
			this.SetInVisibleShaders(this.isInvisible);
		}
		if (this.inGameGUI != null)
		{
			if (this.isMechActive)
			{
				this.inGameGUI.SetCrosshair(this.mechWeaponSounds);
			}
			else
			{
				this.inGameGUI.SetCrosshair(this._weaponManager.currentWeaponSounds);
			}
		}
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
		if (this.myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showZoomHint)
		{
			base.Invoke("TrainingShowZoomHint", 3f);
		}
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x000CCBE0 File Offset: 0x000CADE0
	private void TrainingShowZoomHint()
	{
		if (this.myCurrentWeaponSounds.isZooming && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showZoomHint)
		{
			HintController.instance.ShowHintByName("use_zoom", 0f);
		}
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x000CCC34 File Offset: 0x000CAE34
	[PunRPC]
	[RPC]
	private IEnumerator SetWeaponRPC(string _nameWeapon, string _alternativeNameWeapon, string skinId)
	{
		this.isWeaponSet = true;
		GameObject _weapon = null;
		string innerFolder = Defs.InnerWeaponsFolder;
		if (!_nameWeapon.Contains("Weapon"))
		{
			if (_nameWeapon.Equals("Like"))
			{
				_weapon = this.likePrefab;
				innerFolder = Defs.InnerWeaponsFolder;
			}
			else if (_nameWeapon.Equals("Turret"))
			{
				_weapon = this.turretPrefab;
				innerFolder = Defs.GadgetContentFolder;
			}
			else
			{
				_weapon = (Resources.Load("GadgetsContent/" + _nameWeapon) as GameObject);
				innerFolder = Defs.GadgetContentFolder;
			}
		}
		else
		{
			if (_nameWeapon != null && _alternativeNameWeapon != null && WeaponManager.Removed150615_PrefabNames.Contains(_nameWeapon))
			{
				_nameWeapon = _alternativeNameWeapon;
			}
			_weapon = (Resources.Load("Weapons/" + _nameWeapon) as GameObject);
			if (_weapon != null)
			{
				WeaponSounds ws = _weapon.GetComponent<WeaponSounds>();
				if (ws != null && ws.tier > 100)
				{
					_weapon = null;
				}
			}
			if (_weapon != null)
			{
				this.currentWeaponForKillCam = ItemDb.GetByPrefabName(_weapon.name.Replace("(Clone)", string.Empty)).Tag;
			}
		}
		this.playChargeLoopAnim = false;
		if (_weapon == null)
		{
			_weapon = (Resources.Load("Weapons/" + _alternativeNameWeapon) as GameObject);
			if (_weapon != null)
			{
				this.currentWeaponForKillCam = ItemDb.GetByPrefabName(_weapon.name.Replace("(Clone)", string.Empty)).Tag;
			}
		}
		if (_weapon != null)
		{
			GameObject nw = null;
			string innerPath = ResPath.Combine(innerFolder, _weapon.name.Replace("(Clone)", string.Empty) + Defs.InnerWeapons_Suffix);
			LoadAsyncTool.ObjectRequest weaponRequest = LoadAsyncTool.Get(innerPath, _nameWeapon.Equals("WeaponTurret"));
			while (!weaponRequest.isDone)
			{
				yield return null;
			}
			nw = (GameObject)UnityEngine.Object.Instantiate(_weapon, Vector3.zero, Quaternion.identity);
			nw.GetComponent<WeaponSounds>().Initialize(weaponRequest.asset as GameObject);
			if (this.isMechActive)
			{
				nw.SetActive(false);
			}
			this.myCurrentWeapon = nw;
			this.myCurrentWeaponSounds = this.myCurrentWeapon.GetComponent<WeaponSounds>();
			if (this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
			}
			if (Defs.isDaterRegim)
			{
				this.SetWeaponVisible(!this.isBearActive);
			}
			this.GunFlash = this.myCurrentWeaponSounds.gunFlash;
			Transform ap = this.mySkinName.armorPoint.transform;
			if (ap.childCount > 0)
			{
				ArmorRefs ar = ap.GetChild(0).GetChild(0).GetComponent<ArmorRefs>();
				ar.leftBone.GetComponent<SetPosInArmor>().target = this.myCurrentWeaponSounds.LeftArmorHand;
				ar.rightBone.GetComponent<SetPosInArmor>().target = this.myCurrentWeaponSounds.RightArmorHand;
			}
			foreach (object obj in base.transform)
			{
				Transform ch = (Transform)obj;
				UnityEngine.Object.Destroy(ch.gameObject);
			}
			nw.transform.parent = base.gameObject.transform;
			GameObject _gunFlashTemp = null;
			nw.transform.position = Vector3.zero;
			if (!this.myCurrentWeaponSounds.isMelee)
			{
				foreach (object obj2 in nw.transform)
				{
					Transform chaild = (Transform)obj2;
					if (chaild.gameObject.name.Equals("BulletSpawnPoint") && chaild.childCount > 0)
					{
						_gunFlashTemp = chaild.GetChild(0).gameObject;
						WeaponManager.SetGunFlashActive(_gunFlashTemp, false);
						break;
					}
				}
			}
			if (base.transform.FindChild("BulletSpawnPoint") != null)
			{
				this._bulletSpawnPoint = base.transform.FindChild("BulletSpawnPoint").gameObject;
			}
			base.transform.localPosition = new Vector3(0f, 0.4f, 0f);
			nw.transform.localPosition = new Vector3(0f, -1.4f, 0f);
			nw.transform.rotation = base.transform.rotation;
			this.SetTextureForBodyPlayer(this._skin);
		}
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
		if (!skinId.IsNullOrEmpty() && this.myCurrentWeapon != null)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skin != null)
			{
				skin.SetTo(this.myCurrentWeapon);
			}
		}
		yield break;
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x000CCC7C File Offset: 0x000CAE7C
	[Obfuscation(Exclude = true)]
	public void SetStealthModifier()
	{
		if (this._player != null)
		{
		}
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x000CCC90 File Offset: 0x000CAE90
	public bool NeedAmmo()
	{
		if (this._weaponManager == null)
		{
			return false;
		}
		int currentWeaponIndex = this._weaponManager.CurrentWeaponIndex;
		Weapon weaponByIndex = this.GetWeaponByIndex(currentWeaponIndex);
		return weaponByIndex.currentAmmoInBackpack < this._weaponManager.currentWeaponSounds.MaxAmmoWithEffectApplied;
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x000CCCDC File Offset: 0x000CAEDC
	private void SwitchPause()
	{
		if (this.CurHealth > 0f)
		{
			this.SetPause(true);
		}
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x000CCCF8 File Offset: 0x000CAEF8
	private void ShopPressed()
	{
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (TrainingController.stepTrainingList.ContainsKey("InterTheShop"))
			{
				TrainingController.isNextStep = TrainingState.EnterTheShop;
				if (Player_move_c.StopBlinkShop != null)
				{
					Player_move_c.StopBlinkShop();
				}
			}
			else
			{
				TrainingController.isNextStep = TrainingState.TapToShoot;
			}
		}
		if (this.CurHealth > 0f)
		{
			this.SetInApp();
			this.SetPause(false);
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.clickShop);
			}
		}
	}

	// Token: 0x06002890 RID: 10384 RVA: 0x000CCDB0 File Offset: 0x000CAFB0
	public void PlayPortalSound()
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("PlayPortalSoundRPC", PhotonTargets.All, new object[0]);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("PlayPortalSoundRPC", RPCMode.All, new object[0]);
			}
		}
		else
		{
			this.PlayPortalSoundRPC();
		}
	}

	// Token: 0x06002891 RID: 10385 RVA: 0x000CCE10 File Offset: 0x000CB010
	[PunRPC]
	[RPC]
	public void PlayPortalSoundRPC()
	{
		if (Defs.isSoundFX && this.portalSound != null)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.portalSound);
		}
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000CCE4C File Offset: 0x000CB04C
	public void AddButtonHandlers()
	{
		PauseTapReceiver.PauseClicked += this.SwitchPause;
		ShopTapReceiver.ShopClicked += this.ShopPressed;
		RanksTapReceiver.RanksClicked += this.RanksPressed;
		TopPanelsTapReceiver.OnClicked += this.RanksPressed;
		ChatTapReceiver.ChatClicked += this.ShowChat;
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(true);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(true);
		}
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000CCEE4 File Offset: 0x000CB0E4
	public void RemoveButtonHandelrs()
	{
		PauseTapReceiver.PauseClicked -= this.SwitchPause;
		ShopTapReceiver.ShopClicked -= this.ShopPressed;
		RanksTapReceiver.RanksClicked -= this.RanksPressed;
		TopPanelsTapReceiver.OnClicked -= this.RanksPressed;
		ChatTapReceiver.ChatClicked -= this.ShowChat;
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.SetJoystickActive(false);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(false);
		}
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000CCF7C File Offset: 0x000CB17C
	public void RanksPressed()
	{
		if (this.mySkinName.playerMoveC.isKilled)
		{
			return;
		}
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		this.RemoveButtonHandelrs();
		this.showRanks = true;
		this.networkStartTableNGUIController.winnerPanelCom1.SetActive(false);
		this.networkStartTableNGUIController.winnerPanelCom2.SetActive(false);
		this.networkStartTableNGUIController.ShowRanksTable();
		this.inGameGUI.gameObject.SetActive(false);
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000CD014 File Offset: 0x000CB214
	public void BackRanksPressed()
	{
		this.AddButtonHandlers();
		this.showRanks = false;
		if (this.inGameGUI != null && this.inGameGUI.interfacePanel != null)
		{
			this.inGameGUI.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000CD068 File Offset: 0x000CB268
	private void OnDisable()
	{
		if (this._backSubscription != null)
		{
			this._backSubscription.Dispose();
			this._backSubscription = null;
		}
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000CD088 File Offset: 0x000CB288
	[RPC]
	[PunRPC]
	private void setIp(string _ip)
	{
		this.myIp = _ip;
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000CD094 File Offset: 0x000CB294
	private void CheckTimeCondition()
	{
		CampaignLevel campaignLevel = null;
		foreach (LevelBox levelBox in LevelBox.campaignBoxes)
		{
			if (levelBox.name.Equals(CurrentCampaignGame.boXName))
			{
				foreach (CampaignLevel campaignLevel2 in levelBox.levels)
				{
					if (campaignLevel2.sceneName.Equals(CurrentCampaignGame.levelSceneName))
					{
						campaignLevel = campaignLevel2;
						break;
					}
				}
				break;
			}
		}
		float timeToComplete = campaignLevel.timeToComplete;
		if (this.inGameTime >= timeToComplete)
		{
			CurrentCampaignGame.completeInTime = false;
		}
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000CD198 File Offset: 0x000CB398
	private IEnumerator GetHardwareKeysInput()
	{
		for (;;)
		{
			bool androidBackPressed = false;
			bool backButtonSupported = true;
			if (backButtonSupported)
			{
				if (this._escapePressed)
				{
					if (Application.isEditor)
					{
						Debug.Log("[Escape] presed in PlayerMoveC");
					}
					this._escapePressed = false;
					this._backWasPressed = true;
				}
				else
				{
					if (this._backWasPressed)
					{
						androidBackPressed = true;
					}
					this._backWasPressed = false;
				}
			}
			if (androidBackPressed && !this.isInappWinOpen)
			{
				androidBackPressed = false;
				if (this.inGameGUI == null || this.inGameGUI.pausePanel == null)
				{
					yield return null;
					continue;
				}
				if (this.inGameGUI.blockedCollider.activeSelf)
				{
					yield return null;
					continue;
				}
				this.SwitchPause();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000CD1B4 File Offset: 0x000CB3B4
	private void InitiailizeIcnreaseArmorEffectFlags()
	{
		this.BonusEffectForArmorWorksInThisMatch = (EffectsController.IcnreaseEquippedArmorPercentage > 1f);
		this.ArmorBonusGiven = (EffectsController.ArmorBonus > 0f);
	}

	// Token: 0x1700071F RID: 1823
	// (get) Token: 0x0600289C RID: 10396 RVA: 0x000CD1F4 File Offset: 0x000CB3F4
	// (set) Token: 0x0600289B RID: 10395 RVA: 0x000CD1E8 File Offset: 0x000CB3E8
	public bool isNeedTakePremiumAccountRewards { get; private set; }

	// Token: 0x0600289D RID: 10397 RVA: 0x000CD1FC File Offset: 0x000CB3FC
	private IEnumerator Start()
	{
		string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.Start()", new object[]
		{
			base.GetType().Name
		});
		IEnumerator startSteps = this.StartSteps();
		int i = 0;
		for (;;)
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "Step {0}", new object[]
			{
				i
			});
			using (new ScopeLogger(thisMethod, callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				bool completed = !startSteps.MoveNext();
				if (completed)
				{
					yield break;
				}
			}
			yield return startSteps.Current;
			i++;
		}
		yield break;
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x000CD218 File Offset: 0x000CB418
	private IEnumerator StartSteps()
	{
		string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.StartSteps()", new object[]
		{
			base.GetType().Name
		});
		this._bodyMaterial = this.playerBodyRenderer.material;
		this.playerBodyRenderer.sharedMaterial = this._bodyMaterial;
		this._bearMaterial = new Material(this.mechBearBodyRenderer.material);
		this.mechBearBodyRenderer.sharedMaterial = this._bearMaterial;
		this.mechBearHandRenderer.sharedMaterial = this._bearMaterial;
		this.SetMaterialForArms();
		try
		{
			this.tierForKilledRate = ExpController.OurTierForAnyPlace() + 1;
			this.weKillForKillRate.Clear();
			this.weWereKilledForKillRate.Clear();
		}
		catch (Exception ex)
		{
			Exception e = ex;
			Debug.LogError("Exception in cleaning kill rate stats Player_move_c.Start(): " + e);
			if (this.weKillForKillRate != null)
			{
				this.weKillForKillRate.Clear();
			}
			if (this.weWereKilledForKillRate != null)
			{
				this.weWereKilledForKillRate.Clear();
			}
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this.isImmortality = false;
			this.timerImmortality = 0f;
		}
		this.isDaterRegim = Defs.isDaterRegim;
		this._killerInfo.Reset();
		this.isNeedTakePremiumAccountRewards = PremiumAccountController.Instance.isAccountActive;
		this.InitiailizeIcnreaseArmorEffectFlags();
		Initializer.players.Add(this);
		Initializer.playersObj.Add(this.myPlayerTransform.gameObject);
		if (!Defs.isMulti)
		{
			WeaponManager.sharedManager.myPlayerMoveC = this;
			WeaponManager.sharedManager.myPlayer = this.myPlayerTransform.gameObject;
		}
		this.AmmoBox.fontSize = Mathf.RoundToInt(18f * (float)Screen.width / 1024f);
		this.ScoreBox.fontSize = Mathf.RoundToInt((float)Screen.height * 0.035f);
		if (Defs.isFlag)
		{
			this.flag1 = Initializer.flag1;
			this.flag2 = Initializer.flag2;
		}
		this.timerRegenerationLiveZel = this.maxTimerRegenerationLiveZel;
		this.timerRegenerationLiveCape = this.maxTimerRegenerationLiveCape;
		this.timerRegenerationArmor = this.maxTimerRegenerationArmor;
		this.photonView = PhotonView.Get(this);
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				this.isMine = base.GetComponent<NetworkView>().isMine;
			}
			else if (this.photonView == null)
			{
				Debug.Log("Player_move_c.Start():    photonView == null");
			}
			else
			{
				this.isMine = this.photonView.isMine;
			}
		}
		if (this.drumLoopSound != null && this.drumLoopSound.transform.GetChild(0) != null)
		{
			this.drumLoopSound.transform.GetChild(0).gameObject.SetActive(this.isMulti && !this.isMine);
		}
		if (!this.isMulti || this.isMine)
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
			}
			this._backSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Player Move C");
		}
		if (!this.isMulti || this.isMine)
		{
			this.UpdatePet();
			ShopNGUIController.EquippedPet += this.EquippedPet;
			ShopNGUIController.UnequippedPet += this.UnequipPet;
		}
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		if (!this.isMulti || this.isMine)
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				if (!Defs.isDaterRegim && Storager.getInt("GrenadeID", false) <= 0)
				{
					Storager.setInt("GrenadeID", 1, false);
				}
				if (Defs.isDaterRegim && Storager.getInt("LikeID", false) <= 0)
				{
					Storager.setInt("LikeID", 1, false);
				}
			}
			EffectsController.SlowdownCoeff = 1f;
			ScopeLogger workingLogger = new ScopeLogger(thisMethod, "Resources.Load('InGameGui')", Defs.IsDeveloperBuild && !Application.isEditor);
			UnityEngine.Object inGameGuiPrefab = Resources.Load("InGameGUI");
			workingLogger.Dispose();
			workingLogger = new ScopeLogger(thisMethod, "Instantiate(inGameGuiPrefab)", Defs.IsDeveloperBuild && !Application.isEditor);
			GameObject inGameGuiInstance = (GameObject)UnityEngine.Object.Instantiate(inGameGuiPrefab, Vector3.up * 10000f, Quaternion.identity);
			workingLogger.Dispose();
			this.inGameGUI = inGameGuiInstance.GetComponent<InGameGUI>();
			this.SetGrenateFireEnabled();
			Defs.isJetpackEnabled = false;
			Defs.isTurretWeapon = false;
			this.oldKilledPlayerCharactersCount = ((!Storager.hasKey("KilledPlayerCharactersCount")) ? 0 : Storager.getInt("KilledPlayerCharactersCount", false));
		}
		if (!this.isMulti)
		{
			this._skin = SkinsController.currentSkinForPers;
			this._skin.filterMode = FilterMode.Point;
			ShopNGUIController.sharedShop.onEquipSkinAction = delegate(string id)
			{
				this.UpdateSkin();
			};
		}
		if (!Defs.isMulti)
		{
			GameObject trainigControllerGo = GameObject.FindGameObjectWithTag("TrainingController");
			if (trainigControllerGo != null)
			{
				this.trainigController = trainigControllerGo.GetComponent<TrainingController>();
			}
		}
		this.expController = ExperienceController.sharedController;
		if (this.isMulti && this.isInet)
		{
			GameObject[] tables = GameObject.FindGameObjectsWithTag("NetworkTable");
			for (int i = 0; i < tables.Length; i++)
			{
				if (tables[i].GetComponent<PhotonView>().owner == base.transform.GetComponent<PhotonView>().owner)
				{
					this.myTable = tables[i];
					this.setMyTamble(this.myTable);
					break;
				}
			}
		}
		if (this.isMulti)
		{
			if (this.isInet)
			{
				this.myPlayerID = this.myPlayerTransform.GetComponent<PhotonView>().viewID;
			}
			else
			{
				this.myPlayerIDLocal = this.myPlayerTransform.GetComponent<NetworkView>().viewID;
			}
		}
		if (this.isMulti && !this.isMine)
		{
			base.transform.localPosition = new Vector3(0f, 0.4f, 0f);
		}
		if (!this.isMulti)
		{
			CurrentCampaignGame.ResetConditionParameters();
			CurrentCampaignGame._levelStartedAtTime = Time.time;
			ZombieCreator.BossKilled += this.CheckTimeCondition;
		}
		if (this.isMulti && this.isCompany && this.isMine)
		{
			this.countKillsCommandBlue = GlobalGameController.countKillsBlue;
			this.countKillsCommandRed = GlobalGameController.countKillsRed;
		}
		if (this.isMulti && this.isCOOP)
		{
			this.zombiManager = ZombiManager.sharedManager;
		}
		if (this.isMulti && this.isMine)
		{
			this.networkStartTableNGUIController = NetworkStartTableNGUIController.sharedController;
		}
		if (!this.isMulti || this.isMine)
		{
			this.InitPurchaseActions();
			ActivityIndicator.IsActiveIndicator = false;
		}
		if (!Defs.isMulti || this.isMine)
		{
			this._inAppGameObject = GameObject.FindGameObjectWithTag("InAppGameObject");
			this._listener = this._inAppGameObject.GetComponent<StoreKitEventListener>();
		}
		if (!this.isMulti)
		{
			this.fpsPlayerBody.SetActive(false);
		}
		HOTween.Init(true, true, true);
		HOTween.EnableOverwriteManager(true);
		if (this.isMulti)
		{
			this.showGUI = this.isMine;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AmazonIapV2Impl.Instance.AddPurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessful));
		}
		else
		{
			GoogleIABManager.purchaseSucceededEvent += this.purchaseSuccessful;
		}
		if (!this.isMulti || this.isMine)
		{
			this._player = this.myPlayerTransform.gameObject;
		}
		else
		{
			this._player = null;
		}
		this._weaponManager = WeaponManager.sharedManager;
		if (Defs.isMulti && ((!Defs.isInet && base.GetComponent<NetworkView>().isMine) || (Defs.isInet && this.photonView.isMine && !NetworkStartTable.StartAfterDisconnect)))
		{
			foreach (object obj in this._weaponManager.allAvailablePlayerWeapons)
			{
				Weapon _w = (Weapon)obj;
				_w.currentAmmoInClip = _w.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
				_w.currentAmmoInBackpack = _w.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			GameObject tmpDamage = Resources.Load("Damage") as GameObject;
			this.damage = UnityEngine.Object.Instantiate<GameObject>(tmpDamage);
			Color rgba = this.damage.GetComponent<GUITexture>().color;
			rgba.a = 0f;
			this.damage.GetComponent<GUITexture>().color = rgba;
		}
		if (!this.isMulti || this.isMine)
		{
			GameObject pauseGo = GameObject.FindGameObjectWithTag("GameController");
			if (pauseGo != null)
			{
				this._pauser = pauseGo.GetComponent<Pauser>();
			}
			if (this._pauser == null)
			{
				Debug.LogWarning("Start(): _pauser is null.");
			}
		}
		if (this._singleOrMultiMine())
		{
			this.numberOfGrenadesOnStart.Value = ((!Defs.isHunger) ? ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage != TrainingController.NewTrainingCompletedStage.None) ? Storager.getInt((!Defs.isDaterRegim) ? "GrenadeID" : "LikeID", false) : 0) : 0);
			this.numberOfGrenades.Value = this.numberOfGrenadesOnStart.Value;
			if (!this.isMulti)
			{
				this.indexWeapon = this._weaponManager.CurrentWeaponIndex;
				this.ChangeWeaponReal(this._weaponManager.CurrentWeaponIndex, false);
			}
			else
			{
				this.ChangeWeaponReal(this._weaponManager.CurrentIndexOfLastUsedWeaponInPlayerWeapons(), false);
			}
			this._weaponManager.myGun = base.gameObject;
			if (this._weaponManager.currentWeaponSounds != null)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Stop();
			}
		}
		if (this.isMulti && this.isMine)
		{
			string _nameFilter = FilterBadWorld.FilterString(ProfileController.GetPlayerNameOrDefault());
			if (this.isInet)
			{
				this.photonView.RPC("SetNickName", PhotonTargets.AllBuffered, new object[]
				{
					_nameFilter
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetNickName", RPCMode.AllBuffered, new object[]
				{
					_nameFilter
				});
			}
		}
		this.CurrentBaseArmor = EffectsController.ArmorBonus;
		this.CurHealth = this.MaxHealth;
		if (!this.isMulti || this.isMine)
		{
			Wear.RenewCurArmor(this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
			string armorEquipped = Storager.getString(Defs.ArmorEquppedSN, false);
			if (this._actionsForPurchasedItems.ContainsKey(armorEquipped))
			{
				this._actionsForPurchasedItems[armorEquipped](armorEquipped);
				Storager.setString(Defs.ArmorEquppedSN, Defs.ArmorNoneEqupped, false);
			}
			if (Storager.getInt(Defs.AmmoBoughtSN, false) == 1)
			{
				if (this._actionsForPurchasedItems.ContainsKey("bigammopack"))
				{
					this._actionsForPurchasedItems["bigammopack"]("bigammopack");
				}
				Storager.setInt(Defs.AmmoBoughtSN, 0, false);
			}
		}
		if (this._singleOrMultiMine())
		{
			base.StartCoroutine(this.GetHardwareKeysInput());
			this.inGameGUI.health = (() => Mathf.Clamp((!this.isMechActive) ? this.CurHealth : this.liveMech, 0f, float.MaxValue));
			this.inGameGUI.armor = (() => this.curArmor);
			this.inGameGUI.killsToMaxKills = (() => this.myScoreController.currentScore.ToString());
			this.inGameGUI.timeLeft = delegate()
			{
				float num;
				if (this.isHunger)
				{
					if (this.hungerGameController == null)
					{
						this.hungerGameController = HungerGameController.Instance;
					}
					if (this.hungerGameController != null)
					{
						num = this.hungerGameController.gameTimer;
					}
					else
					{
						num = 0f;
					}
				}
				else if (Defs.isDuel)
				{
					num = DuelController.instance.timeLeft;
				}
				else
				{
					num = (float)TimeGameController.sharedController.timerToEndMatch;
				}
				if (num < 0f)
				{
					num = 0f;
				}
				return Player_move_c.FormatTime(num);
			};
			this.AddButtonHandlers();
			ShopNGUIController.sharedShop.buyAction = new Action<string>(this.PurchaseSuccessful);
			ShopNGUIController.sharedShop.equipAction = delegate(string id)
			{
				this.ChangeWeaponReal(this._weaponManager.CurrentWeaponIndex, false);
				if (WeaponManager.sharedManager != null)
				{
					WeaponManager.sharedManager.ReloadWeaponFromSet(WeaponManager.sharedManager.CurrentWeaponIndex);
				}
			};
			ShopNGUIController.sharedShop.activatePotionAction = delegate(string potion)
			{
				Storager.setInt(potion, Storager.getInt(potion, false) - 1, false);
				PotionsController.sharedController.ActivatePotion(potion, this, new Dictionary<string, object>(), false);
			};
			ShopNGUIController.sharedShop.resumeAction = delegate()
			{
				if (base.gameObject != null)
				{
					this.SetInApp();
					if (this.inAppOpenedFromPause)
					{
						this.inAppOpenedFromPause = false;
						if (this.inGameGUI != null && this.inGameGUI.pausePanel != null)
						{
							this.inGameGUI.pausePanel.SetActive(true);
							PauseNGUIController component = this.inGameGUI.pausePanel.GetComponent<PauseNGUIController>();
							if (component != null && component.settingsPanel != null)
							{
								component.settingsPanel.SetActive(true);
							}
						}
						ExperienceController.sharedController.isShowRanks = true;
					}
					else
					{
						this.SetPause(true);
					}
				}
				else
				{
					ShopNGUIController.GuiActive = false;
				}
			};
			ShopNGUIController.sharedShop.wearEquipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem, string equippedItem)
			{
				if (!this.BonusEffectForArmorWorksInThisMatch)
				{
					float num = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty, this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier)) * (EffectsController.IcnreaseEquippedArmorPercentage - 1f);
					float num2 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty, this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier)) * (EffectsController.IcnreaseEquippedArmorPercentage - 1f);
					this.BonusEffectForArmorWorksInThisMatch = ((double)(num + num2) > 0.001);
					this.AddArmor(num + num2);
				}
				if (!this.ArmorBonusGiven)
				{
					this.ArmorBonusGiven = ((double)EffectsController.ArmorBonus > 0.001);
					this.CurrentBaseArmor += EffectsController.ArmorBonus;
				}
				this.mySkinName.SetWearVisible(null);
				if (category == ShopNGUIController.CategoryNames.CapesCategory)
				{
					this.mySkinName.SetCape(null);
				}
				if (category == ShopNGUIController.CategoryNames.MaskCategory)
				{
					this.mySkinName.SetMask(null);
				}
				if (category == ShopNGUIController.CategoryNames.HatsCategory)
				{
					this.mySkinName.SetHat(null);
					if (equippedItem != null && unequippedItem != null && (!Wear.NonArmorHat(equippedItem) || !Wear.NonArmorHat(unequippedItem)))
					{
						this.CurrentBaseArmor = 0f;
					}
				}
				if (category == ShopNGUIController.CategoryNames.BootsCategory)
				{
					this.mySkinName.SetBoots(null);
				}
				if (category == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					this.mySkinName.SetArmor(null);
					this.respawnedForGUI = true;
					this.CurrentBaseArmor = 0f;
				}
			};
			ShopNGUIController.sharedShop.wearUnequipAction = delegate(ShopNGUIController.CategoryNames category, string unequippedItem)
			{
				this.mySkinName.SetWearVisible(null);
				if (category == ShopNGUIController.CategoryNames.CapesCategory)
				{
					this.mySkinName.SetCape(null);
				}
				if (category == ShopNGUIController.CategoryNames.MaskCategory)
				{
					this.mySkinName.SetMask(null);
				}
				if (category == ShopNGUIController.CategoryNames.HatsCategory)
				{
					this.mySkinName.SetHat(null);
					if (!Wear.NonArmorHat(unequippedItem))
					{
						this.CurrentBaseArmor = 0f;
					}
				}
				if (category == ShopNGUIController.CategoryNames.BootsCategory)
				{
					this.mySkinName.SetBoots(null);
				}
				if (category == ShopNGUIController.CategoryNames.ArmorCategory)
				{
					this.mySkinName.SetArmor(null);
					this.CurrentBaseArmor = 0f;
				}
			};
			ShopNGUIController.ShowArmorChanged += this.HandleShowArmorChanged;
			ShopNGUIController.ShowWearChanged += this.HandleShowWearChanged;
		}
		if (NetworkStartTable.StartAfterDisconnect && Defs.isMulti && Defs.isInet && this.photonView.isMine)
		{
			this.countKills = GlobalGameController.CountKills;
			this.myScoreController.currentScore = Mathf.Max(0, GlobalGameController.Score);
			if (this.countKills < 0)
			{
				this.countKills = 0;
			}
			if (GlobalGameController.healthMyPlayer > 0f || Defs.isHunger)
			{
				this.CurHealth = GlobalGameController.healthMyPlayer;
				this.myPlayerTransform.position = GlobalGameController.posMyPlayer;
				this.myPlayerTransform.rotation = GlobalGameController.rotMyPlayer;
				this.curArmor = GlobalGameController.armorMyPlayer;
			}
			RatingSystem.instance.BackupLastRatingTake();
			NetworkStartTable.StartAfterDisconnect = false;
		}
		yield return null;
		if (this._singleOrMultiMine())
		{
			PotionsController.sharedController.ReactivatePotions(this, new Dictionary<string, object>());
			string curHat = Storager.getString(Defs.HatEquppedSN, false);
			if (!curHat.Equals(Defs.HatNoneEqupped) && Wear.hatsMethods.ContainsKey(curHat))
			{
				Wear.hatsMethods[curHat].Key(this, new Dictionary<string, object>());
			}
			string curCape = Storager.getString(Defs.CapeEquppedSN, false);
			if (!curCape.Equals(Defs.CapeNoneEqupped) && Wear.capesMethods.ContainsKey(curCape))
			{
				Wear.capesMethods[curCape].Key(this, new Dictionary<string, object>());
			}
			string curBoots = Storager.getString(Defs.BootsEquppedSN, false);
			if (!curBoots.Equals(Defs.BootsNoneEqupped) && Wear.bootsMethods.ContainsKey(curBoots))
			{
				Wear.bootsMethods[curBoots].Key(this, new Dictionary<string, object>());
			}
			string curArmor_ = Storager.getString(Defs.ArmorNewEquppedSN, false);
			if (!curArmor_.Equals(Defs.ArmorNewNoneEqupped) && Wear.armorMethods.ContainsKey(curArmor_))
			{
				Wear.armorMethods[curArmor_].Key(this, new Dictionary<string, object>());
			}
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.SetJoystickActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.MakeActive();
			}
			if (JoystickController.leftTouchPad != null)
			{
				JoystickController.leftTouchPad.SetJoystickActive(true);
			}
		}
		if (this.isMulti && this.myTable != null)
		{
			this._skin = this.myNetworkStartTable.mySkin;
			if (this._skin != null)
			{
				this.SetTextureForBodyPlayer(this._skin);
			}
		}
		if (!this.isMine || !TrainingController.TrainingCompleted)
		{
		}
		for (int j = 0; j < Initializer.players.Count; j++)
		{
			Initializer.players[j].SetNicklabelVisible();
		}
		if (this.isMine && Player_move_c.OnMyPlayerMoveCCreated != null)
		{
			Player_move_c.OnMyPlayerMoveCCreated();
		}
		yield break;
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x000CD234 File Offset: 0x000CB434
	public static string FormatTime(float timeDown)
	{
		int num = Mathf.FloorToInt(timeDown);
		if (num != Player_move_c._countdownMemo.Key)
		{
			int value = num / 60;
			int num2 = num % 60;
			Player_move_c._sharedStringBuilder.Length = 0;
			string value2 = (num2 >= 10) ? ":" : ":0";
			Player_move_c._sharedStringBuilder.Append(value).Append(value2).Append(num2);
			string value3 = Player_move_c._sharedStringBuilder.ToString();
			Player_move_c._sharedStringBuilder.Length = 0;
			Player_move_c._countdownMemo = new KeyValuePair<int, string>(num, value3);
		}
		return Player_move_c._countdownMemo.Value;
	}

	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x060028A0 RID: 10400 RVA: 0x000CD2CC File Offset: 0x000CB4CC
	// (set) Token: 0x060028A1 RID: 10401 RVA: 0x000CD2DC File Offset: 0x000CB4DC
	public int GrenadeCount
	{
		get
		{
			return this.numberOfGrenades.Value;
		}
		set
		{
			this.numberOfGrenades.Value = value;
		}
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000CD2EC File Offset: 0x000CB4EC
	private void ActualizeNumberOfGrenades()
	{
		if (!Defs.isHunger && !SceneLoader.ActiveSceneName.Equals(Defs.TrainingSceneName))
		{
			if (this.numberOfGrenades.Value != this.numberOfGrenadesOnStart.Value)
			{
				Storager.setInt((!Defs.isDaterRegim) ? "GrenadeID" : "LikeID", this.numberOfGrenades.Value, false);
				this.numberOfGrenadesOnStart.Value = this.numberOfGrenades.Value;
			}
		}
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x000CD378 File Offset: 0x000CB578
	public void OnApplicationPause(bool pause)
	{
		if (!this._singleOrMultiMine())
		{
			return;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		if (pause)
		{
			this.ActualizeNumberOfGrenades();
			if ((!Defs.isCOOP && !Defs.isCompany && !Defs.isFlag && !Defs.isCapturePoints) || this.liveTime > 90f)
			{
				this.pausedRating = (this.myNetworkStartTable.CalculateMatchRating(true).addRating < 0);
			}
		}
		else if (this.pausedRating && PhotonNetwork.connected && PhotonNetwork.inRoom)
		{
			this.pausedRating = false;
			RatingSystem.instance.BackupLastRatingTake();
		}
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x000CD434 File Offset: 0x000CB634
	private void HandleShowArmorChanged()
	{
		this.mySkinName.SetArmor(null);
		this.mySkinName.SetHat(null);
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x000CD450 File Offset: 0x000CB650
	private void HandleShowWearChanged()
	{
		this.mySkinName.SetWearVisible(null);
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x060028A6 RID: 10406 RVA: 0x000CD460 File Offset: 0x000CB660
	private float WearedCurrentArmor
	{
		get
		{
			return this.CurrentBodyArmor + this.CurrentHatArmor;
		}
	}

	// Token: 0x17000722 RID: 1826
	// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000CD470 File Offset: 0x000CB670
	// (set) Token: 0x060028A8 RID: 10408 RVA: 0x000CD4B4 File Offset: 0x000CB6B4
	private float CurrentBodyArmor
	{
		get
		{
			SaltedFloat saltedFloat;
			if (Wear.curArmor.TryGetValue(Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty, out saltedFloat))
			{
				return saltedFloat.value;
			}
			return 0f;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty))
			{
				Wear.curArmor[Storager.getString(Defs.ArmorNewEquppedSN, false) ?? string.Empty].value = value;
			}
		}
	}

	// Token: 0x17000723 RID: 1827
	// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000CD510 File Offset: 0x000CB710
	// (set) Token: 0x060028AA RID: 10410 RVA: 0x000CD554 File Offset: 0x000CB754
	private float CurrentHatArmor
	{
		get
		{
			SaltedFloat saltedFloat;
			if (Wear.curArmor.TryGetValue(Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty, out saltedFloat))
			{
				return saltedFloat.value;
			}
			return 0f;
		}
		set
		{
			if (Wear.curArmor.ContainsKey(Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty))
			{
				Wear.curArmor[Storager.getString(Defs.HatEquppedSN, false) ?? string.Empty].value = value;
			}
		}
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x000CD5B0 File Offset: 0x000CB7B0
	public void UpdateSkin()
	{
		if (!this.isMulti)
		{
			this._skin = SkinsController.currentSkinForPers;
			this._skin.filterMode = FilterMode.Point;
			this.SetTextureForBodyPlayer(this._skin);
		}
	}

	// Token: 0x060028AC RID: 10412 RVA: 0x000CD5EC File Offset: 0x000CB7EC
	public void SetIDMyTable(string _id)
	{
		this.myTableId = _id;
		base.Invoke("SetIDMyTableInvoke", 0.1f);
	}

	// Token: 0x060028AD RID: 10413 RVA: 0x000CD608 File Offset: 0x000CB808
	[Obfuscation(Exclude = true)]
	private void SetIDMyTableInvoke()
	{
		base.GetComponent<NetworkView>().RPC("SetIDMyTableRPC", RPCMode.AllBuffered, new object[]
		{
			this.myTableId
		});
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x000CD638 File Offset: 0x000CB838
	[RPC]
	[PunRPC]
	private void SetIDMyTableRPC(string _id)
	{
		this.myTableId = _id;
		GameObject[] array = GameObject.FindGameObjectsWithTag("NetworkTable");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetComponent<NetworkView>().viewID.ToString().Equals(_id))
			{
				this.myTable = gameObject;
				this.setMyTamble(this.myTable);
			}
		}
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x000CD6A4 File Offset: 0x000CB8A4
	[RPC]
	[PunRPC]
	public void SetNickName(string _nickName)
	{
		this.photonView = PhotonView.Get(this);
		this.mySkinName.NickName = _nickName;
		if (!this.isMine)
		{
			this.nickLabel.gameObject.SetActive(true);
			this.nickLabel.text = _nickName;
		}
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
	public bool _singleOrMultiMine()
	{
		return !this.isMulti || this.isMine;
	}

	// Token: 0x060028B1 RID: 10417 RVA: 0x000CD70C File Offset: 0x000CB90C
	private void OnDestroy()
	{
		this.DestroyEffects();
		if (this.isMine && Player_move_c.OnMyPlayerMoveCDestroyed != null)
		{
			Player_move_c.OnMyPlayerMoveCDestroyed(this.liveTime);
		}
		if (this.isMine && Defs.isMulti && Defs.isInet && ABTestController.useBuffSystem)
		{
			BuffSystem.instance.PlayerLeaved();
		}
		if (!this.isMulti || this.isMine)
		{
			ShopNGUIController.EquippedPet -= this.EquippedPet;
			ShopNGUIController.UnequippedPet -= this.UnequipPet;
			if (this.myPetEngine != null)
			{
				this.myPetEngine.Destroy();
				this.myPet = null;
			}
		}
		this._bodyMaterial = null;
		this._mechMaterial = null;
		this._bearMaterial = null;
		Initializer.players.Remove(this);
		Initializer.playersObj.Remove(this.myPlayerTransform.gameObject);
		if (Initializer.bluePlayers.Contains(this))
		{
			Initializer.bluePlayers.Remove(this);
		}
		if (Initializer.redPlayers.Contains(this))
		{
			Initializer.redPlayers.Remove(this);
		}
		if (Defs.isCapturePoints && CapturePointController.sharedController != null)
		{
			for (int i = 0; i < CapturePointController.sharedController.basePointControllers.Length; i++)
			{
				if (CapturePointController.sharedController.basePointControllers[i].capturePlayers.Contains(this))
				{
					CapturePointController.sharedController.basePointControllers[i].capturePlayers.Remove(this);
				}
			}
		}
		if (this._weaponPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveWeaponPopularity();
			this._weaponPopularityCacheIsDirty = false;
		}
		if (!this.isMulti)
		{
			ShopNGUIController.sharedShop.onEquipSkinAction = null;
		}
		if (this._singleOrMultiMine())
		{
			this.ActualizeNumberOfGrenades();
			this.SaveKillRate();
			if (this.networkStartTableNGUIController != null)
			{
				this.networkStartTableNGUIController.ranksInterface.SetActive(false);
			}
			if (ShopNGUIController.sharedShop != null)
			{
				ShopNGUIController.sharedShop.resumeAction = null;
			}
			if (this.inGameGUI && this.inGameGUI.gameObject)
			{
				if (!this.isHunger && !Defs.isRegimVidosDebug)
				{
					UnityEngine.Object.Destroy(this.inGameGUI.gameObject);
				}
				else
				{
					this.inGameGUI.topAnchor.SetActive(false);
					this.inGameGUI.leftAnchor.SetActive(false);
					this.inGameGUI.rightAnchor.SetActive(false);
					this.inGameGUI.joystickContainer.SetActive(false);
					this.inGameGUI.bottomAnchor.SetActive(false);
					this.inGameGUI.fastShopPanel.SetActive(false);
					this.inGameGUI.swipeWeaponPanel.gameObject.SetActive(false);
					this.inGameGUI.turretPanel.SetActive(false);
					for (int j = 0; j < 3; j++)
					{
						if (this.inGameGUI.messageAddScore[j].gameObject.activeSelf)
						{
							this.inGameGUI.messageAddScore[j].gameObject.SetActive(false);
						}
					}
				}
			}
			if (ChatViewrController.sharedController != null)
			{
				ChatViewrController.sharedController.CloseChat(true);
			}
			if (coinsShop.thisScript != null && coinsShop.thisScript.enabled)
			{
				coinsShop.ExitFromShop(false);
			}
			coinsPlashka.hidePlashka();
		}
		if (this.isMulti && this.isMine && CameraSceneController.sharedController != null)
		{
			CameraSceneController.sharedController.SetTargetKillCam(null);
		}
		if (!this.isMulti || this.isMine)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AmazonIapV2Impl.Instance.RemovePurchaseResponseListener(new PurchaseResponseDelegate(this.HandlePurchaseSuccessful));
			}
			else
			{
				GoogleIABManager.purchaseSucceededEvent -= this.purchaseSuccessful;
			}
			if (Defs.isTurretWeapon && this.currentTurret != null)
			{
				if (Defs.isMulti)
				{
					if (Defs.isInet)
					{
						PhotonNetwork.Destroy(this.currentTurret);
					}
					else
					{
						Network.RemoveRPCs(this.currentTurret.GetComponent<NetworkView>().viewID);
						Network.Destroy(this.currentTurret);
					}
				}
				else
				{
					UnityEngine.Object.Destroy(this.currentTurret);
				}
			}
		}
		if (this._singleOrMultiMine() || (this._weaponManager != null && this._weaponManager.myPlayer == this.myPlayerTransform.gameObject))
		{
			if (this._pauser != null && this._pauser && this._pauser.paused)
			{
				this._pauser.paused = !this._pauser.paused;
				Time.timeScale = 1f;
				this.AddButtonHandlers();
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("DamageFrame");
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			this.RemoveButtonHandelrs();
			ShopNGUIController.sharedShop.buyAction = null;
			ShopNGUIController.sharedShop.equipAction = null;
			ShopNGUIController.sharedShop.activatePotionAction = null;
			ShopNGUIController.sharedShop.resumeAction = null;
			ShopNGUIController.sharedShop.wearEquipAction = null;
			ShopNGUIController.sharedShop.wearUnequipAction = null;
			ZombieCreator.BossKilled -= this.CheckTimeCondition;
			ShopNGUIController.ShowArmorChanged -= this.HandleShowArmorChanged;
			ShopNGUIController.ShowArmorChanged -= this.HandleShowWearChanged;
		}
		if (this.isMulti && this.isMine)
		{
			ProfileController.ResaveStatisticToKeychain();
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		if (Defs.isMulti && Defs.isCOOP)
		{
			int @int = Storager.getInt(Defs.COOPScore, false);
			int num = (this.myNetworkStartTable.score != -1) ? this.myNetworkStartTable.score : this.myNetworkStartTable.scoreOld;
			if (num > @int)
			{
				Storager.setInt(Defs.COOPScore, num, false);
			}
		}
	}

	// Token: 0x060028B2 RID: 10418 RVA: 0x000CDD40 File Offset: 0x000CBF40
	public bool HasFreezerFireSubscr()
	{
		return this.FreezerFired != null;
	}

	// Token: 0x060028B3 RID: 10419 RVA: 0x000CDD50 File Offset: 0x000CBF50
	private void _SetGunFlashActive(bool state)
	{
		WeaponSounds weaponSounds = (!this.isMechActive) ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds;
		if (weaponSounds.isDoubleShot && !weaponSounds.isMelee)
		{
			weaponSounds.gunFlashDouble[this.numShootInDoubleShot - 1].GetChild(0).gameObject.SetActive(state);
			if (state)
			{
				return;
			}
		}
		if (this.GunFlash != null && !weaponSounds.isMelee && (!this.isZooming || (this.isZooming && !state)))
		{
			WeaponManager.SetGunFlashActive(this.GunFlash.gameObject, state);
		}
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000CDE08 File Offset: 0x000CC008
	public void setInString(string nick)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_0995")));
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000CDE64 File Offset: 0x000CC064
	public void setOutString(string nick)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_0996")));
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x000CDEC0 File Offset: 0x000CC0C0
	public void AddSystemMessage(string _nick1, string _message2, string _nick2, string _message = null)
	{
		this.AddSystemMessage(_nick1, _message2, _nick2, Color.white, _message);
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x000CDED4 File Offset: 0x000CC0D4
	public void AddSystemMessage(string _nick1, string _message2, string _nick2, Color color, string _message = null)
	{
		this.killedSpisok[2] = this.killedSpisok[1];
		this.killedSpisok[1] = this.killedSpisok[0];
		this.killedSpisok[0] = new Player_move_c.SystemMessage(_nick1, _message2, _nick2, _message, color);
		this.timerShow[2] = this.timerShow[1];
		this.timerShow[1] = this.timerShow[0];
		this.timerShow[0] = 3f;
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x000CDF70 File Offset: 0x000CC170
	public void AddSystemMessage(string nick1, int _typeKills, Color color)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], string.Empty, color, null);
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x000CDF88 File Offset: 0x000CC188
	public void AddSystemMessage(string nick1, int _typeKills)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], string.Empty, null);
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000CDFA0 File Offset: 0x000CC1A0
	public void AddSystemMessage(string nick1, int _typeKills, string nick2, Color color, string iconWeapon = null)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], nick2, color, iconWeapon);
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x000CDFB8 File Offset: 0x000CC1B8
	public void AddSystemMessage(string nick1, int _typeKills, string nick2, string iconWeapon = null)
	{
		this.AddSystemMessage(nick1, this.iconShotName[_typeKills], nick2, iconWeapon);
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x000CDFCC File Offset: 0x000CC1CC
	public void AddSystemMessage(string _message)
	{
		this.AddSystemMessage(_message, string.Empty, string.Empty, null);
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x000CDFE0 File Offset: 0x000CC1E0
	public void AddSystemMessage(string _message, Color color)
	{
		this.AddSystemMessage(_message, string.Empty, string.Empty, color, null);
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x000CDFF8 File Offset: 0x000CC1F8
	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagDroppedRPC(bool isBlueFlag, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1) || (!isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1798")));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1799")));
			}
		}
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x000CE0A0 File Offset: 0x000CC2A0
	public void SendSystemMessegeFromFlagReturned(bool isBlueFlag)
	{
		this.photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, new object[]
		{
			isBlueFlag
		});
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x000CE0D0 File Offset: 0x000CC2D0
	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagReturnedRPC(bool isBlueFlag)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if ((isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1) || (!isBlueFlag && WeaponManager.sharedManager.myPlayerMoveC.myCommand == 2))
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1800"));
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1801"));
			}
		}
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000CE160 File Offset: 0x000CC360
	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagCaptureRPC(bool isBlueFlag, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			bool flag = WeaponManager.sharedManager.myPlayerMoveC.myCommand == 1;
			if (flag == isBlueFlag)
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(string.Format("{0} {1}", nick, LocalizationStore.Get("Key_1001")));
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.flagLostClip);
				}
			}
			else
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(LocalizationStore.Get("Key_1002"));
				if (Defs.isSoundFX)
				{
					base.GetComponent<AudioSource>().PlayOneShot(this.flagGetClip);
				}
			}
		}
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x000CE214 File Offset: 0x000CC414
	[PunRPC]
	[RPC]
	public void SendSystemMessegeFromFlagAddScoreRPC(bool isCommandBlue, string nick)
	{
		if (WeaponManager.sharedManager.myPlayer != null)
		{
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot((isCommandBlue != (this._weaponManager.myPlayerMoveC.myCommand == 1)) ? this.flagScoreEnemyClip : this.flagScoreMyCommandClip);
			}
			this.isCaptureFlag = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(nick, 5);
		}
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000CE290 File Offset: 0x000CC490
	public void SendHouseKeeperEvent()
	{
		this.countHouseKeeperEvent++;
		if (this.countHouseKeeperEvent == 1)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.houseKeeperPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 3)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.defenderPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 5)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.guardianPoint, 1f);
		}
		if (this.countHouseKeeperEvent == 10)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.oneManArmyPoint, 1f);
		}
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000CE324 File Offset: 0x000CC524
	private void ResetHouseKeeperEvent()
	{
		this.countHouseKeeperEvent = 0;
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x000CE330 File Offset: 0x000CC530
	public void ShowBonuseParticle(Player_move_c.TypeBonuses _type)
	{
		if (!Defs.isMulti)
		{
			return;
		}
		if (Defs.isInet)
		{
			this.photonView.RPC("ShowBonuseParticleRPC", PhotonTargets.Others, new object[]
			{
				(int)_type
			});
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("ShowBonuseParticleRPC", RPCMode.Others, new object[]
			{
				(int)_type
			});
		}
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x000CE398 File Offset: 0x000CC598
	[PunRPC]
	[RPC]
	public void ShowBonuseParticleRPC(int _type)
	{
		if (this.bonusesParticles.Length >= _type)
		{
			this.bonusesParticles[_type].ShowParticle();
		}
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x000CE3B8 File Offset: 0x000CC5B8
	public void SetTextureForBodyPlayer(Texture needTx)
	{
		this.SetMaterialForArms();
		if (this._bodyMaterial != null)
		{
			this._bodyMaterial.mainTexture = needTx;
		}
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x000CE3E0 File Offset: 0x000CC5E0
	public void SetTextureForActiveMesh(Texture needTx)
	{
		this.SetMaterialForArms();
		if (this.mainDamageMaterial != null)
		{
			this.mainDamageMaterial.mainTexture = needTx;
		}
	}

	// Token: 0x060028C9 RID: 10441 RVA: 0x000CE410 File Offset: 0x000CC610
	private void SetMaterialForArms()
	{
		if (this.myCurrentWeaponSounds != null)
		{
			if (!this.isBearActive)
			{
				this.myCurrentWeaponSounds._innerPars.SetMaterialForArms(this._bodyMaterial);
			}
		}
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x000CE44C File Offset: 0x000CC64C
	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs)
	{
		Transform transform = obj.transform;
		int childCount = obj.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			bool flag = false;
			foreach (GameObject o in stopObjs)
			{
				if (child.gameObject.Equals(o))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (child.gameObject.GetComponent<Renderer>() && child.gameObject.GetComponent<Renderer>().material)
				{
					child.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
				flag = false;
				foreach (GameObject o2 in stopObjs)
				{
					if (child.gameObject.Equals(o2))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Player_move_c.SetTextureRecursivelyFrom(child.gameObject, txt, stopObjs);
				}
			}
		}
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x000CE564 File Offset: 0x000CC764
	private IEnumerator Flash(GameObject _obj, bool poison = false)
	{
		if (this.isDaterRegim)
		{
			yield break;
		}
		this.SetTextureForBodyPlayer((!poison) ? SkinsController.damageHitTexture : SkinsController.poisonHitTexture);
		if (this.isMechActive && this.currentMech != null)
		{
			this.currentMech.ShowHitMaterial(true, poison);
		}
		yield return new WaitForSeconds(0.125f);
		this.SetTextureForBodyPlayer(this._skin);
		if (this.isMechActive && this.currentMech != null)
		{
			this.currentMech.ShowHitMaterial(false, false);
		}
		yield break;
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x000CE590 File Offset: 0x000CC790
	public static GameObject[] GetStopObjFromPlayer(GameObject _obj)
	{
		List<GameObject> list = new List<GameObject>();
		Transform transform = _obj.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.name.Equals("GameObject") && child.transform.childCount > 0)
			{
				for (int j = 0; j < child.transform.childCount; j++)
				{
					GameObject gameObject = null;
					WeaponSounds component = child.transform.GetChild(j).gameObject.GetComponent<WeaponSounds>();
					GameObject bonusPrefab = component.bonusPrefab;
					if (!component.isMelee)
					{
						gameObject = child.transform.GetChild(j).Find("BulletSpawnPoint").gameObject;
					}
					if (component.noFillObjects != null && component.noFillObjects.Length > 0)
					{
						for (int k = 0; k < component.noFillObjects.Length; k++)
						{
							list.Add(component.noFillObjects[k]);
						}
					}
					if (bonusPrefab != null)
					{
						list.Add(bonusPrefab);
					}
					if (gameObject != null)
					{
						list.Add(gameObject);
					}
					if (component.LeftArmorHand != null)
					{
						list.Add(component.LeftArmorHand.gameObject);
					}
					if (component.RightArmorHand != null)
					{
						list.Add(component.RightArmorHand.gameObject);
					}
					if (component.grenatePoint != null)
					{
						list.Add(component.grenatePoint.gameObject);
					}
					if (component.animationObject != null && component.animationObject.GetComponent<InnerWeaponPars>() != null && component.animationObject.GetComponent<InnerWeaponPars>().particlePoint != null)
					{
						list.Add(component.animationObject.GetComponent<InnerWeaponPars>().particlePoint);
					}
					List<GameObject> listWeaponAnimEffects = component.GetListWeaponAnimEffects();
					if (listWeaponAnimEffects != null)
					{
						list.AddRange(listWeaponAnimEffects);
					}
				}
				break;
			}
		}
		if (_obj != null && _obj.GetComponent<SkinName>() != null)
		{
			SkinName component2 = _obj.GetComponent<SkinName>();
			list.Add(component2.capesPoint);
			list.Add(component2.hatsPoint);
			list.Add(component2.maskPoint);
			list.Add(component2.bootsPoint);
			list.Add(component2.armorPoint);
			list.Add(component2.onGroundEffectsPoint.gameObject);
			if (component2.playerMoveC != null)
			{
				list.Add(component2.playerMoveC.flagPoint);
				list.Add(component2.playerMoveC.invisibleParticle);
				list.Add(component2.playerMoveC.jetPackPoint);
				list.Add(component2.playerMoveC.wingsPoint);
				list.Add(component2.playerMoveC.wingsPointBear);
				list.Add(component2.playerMoveC.turretPoint);
				list.Add(component2.playerMoveC.currentMech.body);
				list.Add(component2.playerMoveC.mechBearPoint);
				list.Add(component2.playerMoveC.mechExplossion);
				list.Add(component2.playerMoveC.bearExplosion);
				if (Defs.isDaterRegim && component2.playerMoveC.myCurrentWeaponSounds != null)
				{
					list.Add(component2.playerMoveC.myCurrentWeaponSounds.BearWeaponObject);
				}
				list.Add(component2.playerMoveC.particleBonusesPoint);
				component2.playerMoveC.arrowToPortalPoint.Do(new Action<GameObject>(list.Add));
			}
		}
		else
		{
			Debug.Log("Condition failed: “_obj != null && _obj.GetComponent<SkinName>() != null”");
		}
		return list.ToArray();
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000CE980 File Offset: 0x000CCB80
	private IEnumerator RunOnGroundEffectCoroutine(string name, float tm)
	{
		yield return new WaitForSeconds(tm);
		this.RunOnGroundEffect(name);
		yield break;
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000CE9B8 File Offset: 0x000CCBB8
	private void FixedUpdate()
	{
		if (this.rocketToLaunch != null && this.rocketToLaunch.GetComponent<Rocket>().currentRocketSettings != null)
		{
			Rocket component = this.rocketToLaunch.GetComponent<Rocket>();
			this.rocketToLaunch.GetComponent<Rigidbody>().AddForce(component.currentRocketSettings.startForce * this.rocketToLaunch.transform.forward);
			this.rocketToLaunch = null;
		}
		if (!this.isMulti || this.isMine)
		{
			ShopNGUIController.sharedShop.SetInGame(true);
			if (((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled) != this.isJumpPresedOld && (Defs.isJetpackEnabled || this.isJumpPresedOld))
			{
				this.SetJetpackParticleEnabled((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled);
				this.isJumpPresedOld = ((JoystickController.rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed) && Defs.isJetpackEnabled);
			}
		}
		if (!this.isMulti || !this.isMine)
		{
			return;
		}
		if (Camera.main == null)
		{
			return;
		}
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x060028CF RID: 10447 RVA: 0x000CEB1C File Offset: 0x000CCD1C
	public static int _ShootRaycastLayerMask
	{
		get
		{
			return -2053 & ~(1 << LayerMask.NameToLayer("DamageCollider")) & ~(1 << LayerMask.NameToLayer("TransparentFX")) & ~(1 << LayerMask.NameToLayer("IgnoreRocketsAndBullets"));
		}
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x000CEB64 File Offset: 0x000CCD64
	public static int TierOfCurrentRoom()
	{
		if (PhotonNetwork.room != null && PhotonNetwork.room.customProperties.ContainsKey("tier"))
		{
			return (int)PhotonNetwork.room.customProperties["tier"];
		}
		return ExpController.Instance.OurTier;
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000CEBB8 File Offset: 0x000CCDB8
	public int TierOrRoomTier(int tier)
	{
		if (!this.roomTierInitialized)
		{
			this.roomTierInitialized = true;
			this.roomTier = Player_move_c.TierOfCurrentRoom();
		}
		return Math.Min(tier, this.roomTier);
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000CEBE4 File Offset: 0x000CCDE4
	private IEnumerator Fade(float start, float end, float length, GameObject currentObject)
	{
		if (currentObject == null)
		{
			Debug.LogWarningFormat("{0}: currentObject == null", new object[]
			{
				base.GetType().Name
			});
			yield break;
		}
		GUITexture texture = currentObject.GetComponent<GUITexture>();
		for (float i = 0f; i < 1f; i += Time.deltaTime / length)
		{
			if (texture == null)
			{
				Debug.LogWarningFormat("{0}: texture == null", new object[]
				{
					base.GetType().Name
				});
				yield break;
			}
			Color rgba = texture.color;
			rgba.a = Mathf.Lerp(start, end, i);
			texture.color = rgba;
			yield return 0;
			if (texture == null)
			{
				Debug.LogWarningFormat("{0}: texture == null", new object[]
				{
					base.GetType().Name
				});
				yield break;
			}
			Color rgba_ = texture.color;
			rgba_.a = end;
			texture.color = rgba_;
		}
		yield break;
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000CEC3C File Offset: 0x000CCE3C
	private IEnumerator SetCanReceiveSwipes()
	{
		yield return new WaitForSeconds(0.1f);
		this.canReceiveSwipes = true;
		yield break;
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x000CEC58 File Offset: 0x000CCE58
	[Obfuscation(Exclude = true)]
	private void setisDeadFrameFalse()
	{
		this.isDeadFrame = false;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000CEC64 File Offset: 0x000CCE64
	private void UpdateImmortalityAlpColor(float _alpha)
	{
		if (Mathf.Abs(_alpha - this.oldAlphaImmortality) < 0.001f)
		{
			return;
		}
		this.oldAlphaImmortality = _alpha;
		if (this.myCurrentWeaponSounds != null)
		{
			this.playerBodyRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			Shader shader = Shader.Find("Mobile/Diffuse-Color");
			if (shader != null && this.myCurrentWeaponSounds.bonusPrefab != null && this.myCurrentWeaponSounds.bonusPrefab.transform.parent != null)
			{
				this.myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.shader = shader;
				this.myCurrentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material.SetColor("_ColorRili", new Color(1f, 1f, 1f, _alpha));
			}
		}
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x000CED7C File Offset: 0x000CCF7C
	private void Update()
	{
		this.liveTime += Time.deltaTime;
		if (this.isMulti && this.isMine && !this.isDetectCh && this.CurHealth > 50f)
		{
			this.isDetectCh = true;
			Switcher.AppendAbuseMethod(AbuseMetod.health);
		}
		if (this._timerDelayInShootingBurst > 0f)
		{
			this._timerDelayInShootingBurst -= Time.deltaTime;
		}
		this.UpdateHealth();
		this.UpdateNickLabelColor();
		this.GadgetsUpdate();
		this.UpdateEffects();
		if (this.timerUpdatePointAutoAi > 0f)
		{
			this.timerUpdatePointAutoAi -= Time.deltaTime;
		}
		if ((!this.isMulti || this.isMine) && this._timeOfSlowdown > 0f)
		{
			this._timeOfSlowdown -= Time.deltaTime;
			if (this._timeOfSlowdown <= 0f)
			{
				EffectsController.SlowdownCoeff = 1f;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			Defs.isZooming = this.isZooming;
		}
		if (!this.isKilled && this.timerImmortality > 0f)
		{
			this.timerImmortality -= Time.deltaTime;
			if (this.timerImmortality <= 0f)
			{
				this.isImmortality = false;
			}
		}
		if (!this.isInvisible)
		{
			if (this.isImmortality)
			{
				float num = 1f;
				this.timerImmortalityForAlpha += Time.deltaTime;
				float num2 = 2f * (this.timerImmortalityForAlpha - Mathf.Floor(this.timerImmortalityForAlpha / num) * num) / num;
				if (num2 > 1f)
				{
					num2 = 2f - num2;
				}
				this.UpdateImmortalityAlpColor(0.5f + num2 * 0.4f);
			}
			else
			{
				this.UpdateImmortalityAlpColor(1f);
			}
		}
		if (this.isMulti && this.isMine)
		{
			if ((this.isCompany || Defs.isFlag) && this.myCommand == 0 && this.myTable != null)
			{
				this.myCommand = this.myNetworkStartTable.myCommand;
			}
			if (Defs.isFlag && this.myBaza == null && this.myCommand != 0)
			{
				if (this.myCommand == 1)
				{
					this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand1");
				}
				else
				{
					this.myBaza = GameObject.FindGameObjectWithTag("BazaZoneCommand2");
				}
			}
			if (Defs.isFlag && (this.myFlag == null || this.enemyFlag == null) && this.myCommand != 0)
			{
				this.myFlag = ((this.myCommand != 1) ? this.flag2 : this.flag1);
				this.enemyFlag = ((this.myCommand != 1) ? this.flag1 : this.flag2);
			}
			if (Defs.isFlag && this.myFlag != null && this.enemyFlag != null)
			{
				if (!this.myFlag.isCapture && !this.myFlag.isBaza && Vector3.SqrMagnitude(this.myPlayerTransform.position - this.myFlag.transform.position) < 2.25f)
				{
					this.photonView.RPC("SendSystemMessegeFromFlagReturnedRPC", PhotonTargets.All, new object[]
					{
						this.myFlag.isBlue
					});
					this.myFlag.GoBaza();
				}
				if (!this.enemyFlag.isCapture && !this.isKilled && this.enemyFlag.GetComponent<FlagController>().flagModel.activeSelf && Vector3.SqrMagnitude(this.myPlayerTransform.position - this.enemyFlag.transform.position) < 2.25f)
				{
					this.enemyFlag.SetCapture(this.photonView.ownerId);
					this.isCaptureFlag = true;
					this.photonView.RPC("SendSystemMessegeFromFlagCaptureRPC", PhotonTargets.All, new object[]
					{
						this.enemyFlag.isBlue,
						this.mySkinName.NickName
					});
				}
			}
			if (this.isCaptureFlag && Vector3.SqrMagnitude(this.myPlayerTransform.position - this.myBaza.transform.position) < 2.25f)
			{
				if (this.myFlag.isBaza)
				{
					if (Defs.isSoundFX)
					{
						base.GetComponent<AudioSource>().PlayOneShot(this.flagScoreMyCommandClip);
					}
					if (this.myTable != null)
					{
						this.myNetworkStartTable.AddScore();
					}
					this.countMultyFlag++;
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyCapture(ConnectSceneNGUIController.RegimGame.FlagCapture);
					}
					this.myScoreController.AddScoreOnEvent((this.countMultyFlag != 3) ? ((this.countMultyFlag != 2) ? PlayerEventScoreController.ScoreEvent.flagTouchDown : PlayerEventScoreController.ScoreEvent.flagTouchDouble) : PlayerEventScoreController.ScoreEvent.flagTouchDownTriple, 1f);
					this.isCaptureFlag = false;
					this.photonView.RPC("SendSystemMessegeFromFlagAddScoreRPC", PhotonTargets.Others, new object[]
					{
						!this.enemyFlag.isBlue,
						this.mySkinName.NickName
					});
					this.AddSystemMessage(LocalizationStore.Get("Key_1003"));
					this.enemyFlag.GoBaza();
				}
				else if (!this.inGameGUI.message_returnFlag.activeSelf)
				{
					this.inGameGUI.message_returnFlag.SetActive(true);
				}
			}
			else if (this.inGameGUI.message_returnFlag.activeSelf)
			{
				this.inGameGUI.message_returnFlag.SetActive(false);
			}
			if (Defs.isFlag && this.inGameGUI != null)
			{
				if (this.isCaptureFlag)
				{
					if (!this.inGameGUI.flagRedCaptureTexture.activeSelf)
					{
						this.inGameGUI.flagRedCaptureTexture.SetActive(true);
					}
				}
				else if (this.inGameGUI.flagRedCaptureTexture.activeSelf)
				{
					this.inGameGUI.flagRedCaptureTexture.SetActive(false);
				}
			}
		}
		if (!this.isMulti || this.isMine)
		{
			if (this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).currentAmmoInClip == 0 && !this._changingWeapon && this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).currentAmmoInBackpack > 0 && (!this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot") || (this._weaponManager.currentWeaponSounds.countInSeriaBazooka <= 1 && this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).currentAmmoInClip == 0 && this.lastShotTime + 0.2f <= Time.time)) && !this.isReloading)
			{
				this.ReloadPressed();
			}
			if (!this.isHunger || this.hungerGameController.isGo)
			{
				PotionsController.sharedController.Step(Time.deltaTime, this);
			}
		}
		if (this.isHunger && this.isMine)
		{
			this.timeHingerGame += Time.deltaTime;
			bool flag = InGameGUI.sharedInGameGUI != null && InGameGUI.sharedInGameGUI.pausePanel.activeSelf;
			if (Initializer.players.Count == 1 && this.hungerGameController.isGo && this.timeHingerGame > 10f && !this.isZachetWin && !flag)
			{
				this.isZachetWin = true;
				int val = Storager.getInt(Defs.RatingHunger, false) + 1;
				Storager.setInt(Defs.RatingHunger, val, false);
				val = Storager.getInt("Rating", false) + 1;
				Storager.setInt("Rating", val, false);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.TryIncrementWinCountTimestamp();
				}
				this.myNetworkStartTable.WinInHunger();
			}
		}
		if (!this.isMulti)
		{
			this.inGameTime += Time.deltaTime;
		}
		if ((this.isCompany || Defs.isFlag) && this.myCommand == 0 && this.myTable != null)
		{
			this.myCommand = this.myNetworkStartTable.myCommand;
		}
		if (this.isMulti && this.isMine && this._weaponManager.myPlayer != null)
		{
			GlobalGameController.posMyPlayer = this._weaponManager.myPlayer.transform.position;
			GlobalGameController.rotMyPlayer = this._weaponManager.myPlayer.transform.rotation;
			GlobalGameController.healthMyPlayer = this.CurHealth;
			GlobalGameController.armorMyPlayer = this.curArmor;
		}
		if (!this.isMulti || this.isMine)
		{
			if (this.timerShow[0] > 0f)
			{
				this.timerShow[0] -= Time.deltaTime;
			}
			if (this.timerShow[1] > 0f)
			{
				this.timerShow[1] -= Time.deltaTime;
			}
			if (this.timerShow[2] > 0f)
			{
				this.timerShow[2] -= Time.deltaTime;
			}
		}
		if (!this.isMulti || this.isMine)
		{
			Func<bool> func = () => this._pauser != null && this._pauser.paused;
			if (func() || !this.canReceiveSwipes || !this.isInappWinOpen)
			{
			}
		}
		if (this.GunFlashLifetime > 0f)
		{
			this.GunFlashLifetime -= Time.deltaTime;
			if (this.GunFlashLifetime <= 0f)
			{
				this.GunFlashLifetime = 0f;
				this._SetGunFlashActive(false);
			}
		}
		else if (this.GunFlashLifetime == -1f && JoystickController.IsButtonFireUp())
		{
			this.GunFlashLifetime = 0f;
			this._SetGunFlashActive(false);
		}
		if (Defs.isDaterRegim && this.isPlayerFlying)
		{
			if (!this.isMine)
			{
				if (!this.wingsAnimation.isPlaying)
				{
					this.wingsAnimation.Play();
				}
				if (!this.wingsBearAnimation.isPlaying)
				{
					this.wingsBearAnimation.Play();
				}
			}
			if (Defs.isSoundFX && !this.wingsSound.isPlaying)
			{
				this.wingsSound.Play();
			}
		}
		if (!this.isMulti || this.isMine)
		{
			this.ShootUpdate();
		}
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000CF8A8 File Offset: 0x000CDAA8
	[RPC]
	[PunRPC]
	public void SetWeaponSkinRPC(string skinId, string weaponName)
	{
		if (this.myCurrentWeapon.nameNoClone() == weaponName)
		{
			WeaponSkin skin = WeaponSkinsManager.GetSkin(skinId);
			if (skinId != null)
			{
				skin.SetTo(this.myCurrentWeapon);
			}
		}
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x000CF8E8 File Offset: 0x000CDAE8
	private void HandleEscape()
	{
		if (this.trainigController != null)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Ignoring [Escape] in training scene.");
			}
			return;
		}
		if (this.isMulti && !this.isMine)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Ignoring [Escape]; isMulti: {0}, isMine: {1}", new object[]
				{
					this.isMulti,
					this.isMine
				});
			}
			return;
		}
		if (!Cursor.visible)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Handling [Escape]. Cursor locked.");
			}
			this._escapePressed = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			return;
		}
		if (this.showRanks)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Ignoring [Escape]; showRanks: {0}", new object[]
				{
					this.showRanks
				});
			}
			return;
		}
		if (RespawnWindow.Instance != null && RespawnWindow.Instance.isShown)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Handling [Escape] in Respawn Window.");
			}
			RespawnWindow.Instance.OnBtnGoBattleClick();
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("ChatViewer");
		if (gameObject == null)
		{
			if (!this.isInappWinOpen && Cursor.lockState != CursorLockMode.Locked)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Handling [Escape]; isInappWinOpen: {0}, lockState: '{1}'", new object[]
					{
						this.isInappWinOpen,
						Cursor.lockState
					});
				}
				this._escapePressed = true;
			}
			return;
		}
		if (!gameObject.GetComponent<ChatViewrController>().buySmileBannerPrefab.activeSelf)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Handling [Escape]. Closing chat");
			}
			gameObject.GetComponent<ChatViewrController>().CloseChat(false);
		}
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x000CFAAC File Offset: 0x000CDCAC
	public void GoToShopFromPause()
	{
		this.SetInApp();
		this.inAppOpenedFromPause = true;
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000CFABC File Offset: 0x000CDCBC
	public void QuitGame()
	{
		Time.timeScale = 1f;
		Time.timeScale = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = Defs.MainMenuScene;
			SceneManager.LoadScene("LevelToCompleteProm");
		}
		else if (this.isMulti)
		{
			if (!this.isInet)
			{
				if (PlayerPrefs.GetString("TypeGame").Equals("server"))
				{
					Network.Disconnect(200);
					GameObject.FindGameObjectWithTag("NetworkTable").GetComponent<LANBroadcastService>().StopBroadCasting();
				}
				else if (Network.connections.Length == 1)
				{
					Network.CloseConnection(Network.connections[0], true);
				}
				ActivityIndicator.IsActiveIndicator = false;
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				ConnectSceneNGUIController.Local();
			}
			else
			{
				coinsShop.hideCoinsShop();
				coinsPlashka.hidePlashka();
				Defs.typeDisconnectGame = Defs.DisconectGameType.Exit;
				PhotonNetwork.LeaveRoom();
			}
		}
		else if (Defs.IsSurvival)
		{
			if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
			{
				GlobalGameController.HasSurvivalRecord = true;
				PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
				PlayerPrefs.Save();
				FriendsController.sharedController.survivalScore = GlobalGameController.Score;
				FriendsController.sharedController.SendOurData(false);
			}
			if (Storager.getInt("SendFirstResaltArena", false) != 1)
			{
				Storager.setInt("SendFirstResaltArena", 1, false);
				AnalyticsStuff.LogArenaFirst(true, false);
			}
			Debug.Log("Player_move_c.QuitGame(): Trying to report survival score: " + GlobalGameController.Score);
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				AGSLeaderboardsClient.SubmitScore("best_survival_scores", (long)GlobalGameController.Score, 0);
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite && Social.localUser.authenticated)
			{
				Social.ReportScore((long)GlobalGameController.Score, "CgkIr8rGkPIJEAIQCg", delegate(bool success)
				{
					Debug.Log("Player_move_c.QuitGame(): " + ((!success) ? "Failed to report score." : "Reported score successfully."));
				});
			}
			LevelCompleteScript.LastGameResult = GameResult.Quit;
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			SceneManager.LoadScene("LevelToCompleteProm");
		}
		else
		{
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "ChooseLevel";
			bool flag = !this.isMulti;
			if (flag)
			{
				string reasonToDismissInterstitialCampaign = LevelCompleteInterstitialRunner.GetReasonToDismissInterstitialCampaign(false);
				if (string.IsNullOrEmpty(reasonToDismissInterstitialCampaign))
				{
					if (Application.isEditor)
					{
						Debug.Log("<color=magenta>QuitGame()</color>");
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
			}
			SceneManager.LoadScene((!flag) ? Defs.MainMenuScene : "LevelToCompleteProm");
		}
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000CFD74 File Offset: 0x000CDF74
	public void SetPause(bool showGUI = true)
	{
		this.ShotUnPressed(true);
		JoystickController.rightJoystick.jumpPressed = false;
		JoystickController.leftTouchPad.isJumpPressed = false;
		JoystickController.rightJoystick.Reset();
		if (this._pauser == null)
		{
			Debug.LogWarning("SetPause(): _pauser is null.");
			return;
		}
		this._pauser.paused = !this._pauser.paused;
		if (this.myCurrentWeaponSounds != null)
		{
			this.myCurrentWeaponSounds.animationObject.SetActive(!this._pauser.paused);
		}
		if (this._pauser.paused)
		{
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(false);
		}
		else
		{
			InGameGUI.sharedInGameGUI.turretPanel.SetActive(InGameGUI.sharedInGameGUI.isTurretInterfaceActive);
		}
		if (showGUI && this.inGameGUI != null && this.inGameGUI.pausePanel != null)
		{
			this.inGameGUI.pausePanel.SetActive(this._pauser.paused);
			this.inGameGUI.fastShopPanel.SetActive(!this._pauser.paused);
			if (ExperienceController.sharedController != null && ExpController.Instance != null)
			{
				ExperienceController.sharedController.isShowRanks = this._pauser.paused;
				ExpController.Instance.InterfaceEnabled = this._pauser.paused;
			}
		}
		if (this._pauser.paused)
		{
			if (!this.isMulti)
			{
				Time.timeScale = 0f;
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
				{
					TrainingController.isPause = true;
				}
			}
		}
		else
		{
			Time.timeScale = 1f;
			TrainingController.isPause = false;
		}
		if (this._pauser.paused)
		{
			this.RemoveButtonHandelrs();
		}
		else
		{
			this.AddButtonHandlers();
		}
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000CFF70 File Offset: 0x000CE170
	public void WinFromTimer()
	{
		if (!base.enabled)
		{
			return;
		}
		base.enabled = false;
		InGameGUI.sharedInGameGUI.gameObject.SetActive(false);
		if (Defs.isCompany)
		{
			int commandWin = 0;
			if (this.countKillsCommandBlue > this.countKillsCommandRed)
			{
				commandWin = 1;
			}
			if (this.countKillsCommandRed > this.countKillsCommandBlue)
			{
				commandWin = 2;
			}
			if (WeaponManager.sharedManager.myTable != null)
			{
				WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, commandWin, this.countKillsCommandBlue, this.countKillsCommandRed);
			}
		}
		else if (Defs.isCOOP)
		{
			ZombiManager.sharedManager.EndMatch();
		}
		else if (WeaponManager.sharedManager.myTable != null)
		{
			WeaponManager.sharedManager.myNetworkStartTable.win(string.Empty, 0, 0, 0);
		}
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000D0054 File Offset: 0x000CE254
	private void SetInApp()
	{
		this.isInappWinOpen = !this.isInappWinOpen;
		if (this.isInappWinOpen)
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!this.isMulti)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			if (InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.activeSelf)
			{
				InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(false);
				InGameGUI.sharedInGameGUI.shopPanelForSwipe.gameObject.SetActive(TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None);
			}
			if (InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.activeSelf)
			{
				InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(false);
				InGameGUI.sharedInGameGUI.shopPanelForTap.gameObject.SetActive(true);
			}
			ActivityIndicator.IsActiveIndicator = false;
			if (this._pauser == null)
			{
				Debug.LogWarning("SetInApp(): _pauser is null.");
			}
			else if (!this._pauser.paused)
			{
				Time.timeScale = 1f;
			}
		}
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000D0180 File Offset: 0x000CE380
	private void providePotion(string inShopId)
	{
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x000D0184 File Offset: 0x000CE384
	private void ProvideAmmo(string inShopId)
	{
		this._listener.ProvideContent();
		this._weaponManager.SetMaxAmmoFrAllWeapons();
		if (JoystickController.rightJoystick != null)
		{
			if (this.inGameGUI != null)
			{
				this.inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			Debug.Log("JoystickController.rightJoystick = null");
		}
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000D01F0 File Offset: 0x000CE3F0
	public void PurchaseSuccessful(string id)
	{
		if (this._actionsForPurchasedItems.ContainsKey(id))
		{
			this._actionsForPurchasedItems[id](id);
		}
		this._timeWhenPurchShown = Time.realtimeSinceStartup;
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000D022C File Offset: 0x000CE42C
	private void purchaseSuccessful(GooglePurchase purchase)
	{
		try
		{
			if (purchase == null)
			{
				throw new ArgumentNullException("purchase");
			}
			this.PurchaseSuccessful(purchase.productId);
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	// Token: 0x060028E2 RID: 10466 RVA: 0x000D0284 File Offset: 0x000CE484
	private void HandlePurchaseSuccessful(PurchaseResponse response)
	{
		if (!"SUCCESSFUL".Equals(response.Status, StringComparison.OrdinalIgnoreCase))
		{
			Debug.LogWarning("Amazon PurchaseResponse (Player_move_c): " + response.Status);
			return;
		}
		Debug.Log("Amazon PurchaseResponse (Player_move_c): " + response.PurchaseReceipt.ToJson());
		this.PurchaseSuccessful(response.PurchaseReceipt.Sku);
	}

	// Token: 0x060028E3 RID: 10467 RVA: 0x000D02E8 File Offset: 0x000CE4E8
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (this.isMine)
		{
			this._networkView.RPC("SetInvisibleRPC", player, new object[]
			{
				(!Defs.isDaterRegim) ? this.isInvisible : this.isBigHead,
				this.isInvisByWeapon
			});
			this._networkView.RPC("CountKillsCommandSynch", player, new object[]
			{
				this.countKillsCommandBlue,
				this.countKillsCommandRed
			});
			this._networkView.RPC("SetWeaponRPC", player, new object[]
			{
				this._sendingNameWeapon,
				this._sendingAlternativeNameWeapon,
				this._sendingSkinId
			});
			this.SendSynhHealth(true, null);
			if (Defs.isDaterRegim && Defs.isJetpackEnabled)
			{
				this._networkView.RPC("SetJetpackEnabledRPC", player, new object[]
				{
					Defs.isJetpackEnabled
				});
			}
			this.GadgetsOnPlayerConnected();
			if (this.isBearActive)
			{
				this._networkView.RPC("ActivateMechRPC", player, new object[0]);
			}
			this._networkView.RPC("SynhIsZoming", player, new object[]
			{
				this.isZooming
			});
		}
	}

	// Token: 0x060028E4 RID: 10468 RVA: 0x000D043C File Offset: 0x000CE63C
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			this.photonView.RPC("CountKillsCommandSynch", player, new object[]
			{
				this.countKillsCommandBlue,
				this.countKillsCommandRed
			});
			if ((!Defs.isDaterRegim) ? this.isInvisible : this.isBigHead)
			{
				this.photonView.RPC("SetInvisibleRPC", player, new object[]
				{
					(!Defs.isDaterRegim) ? this.isInvisible : this.isBigHead,
					this.isInvisByWeapon
				});
			}
			this.photonView.RPC("SetWeaponRPC", player, new object[]
			{
				this._sendingNameWeapon,
				this._sendingAlternativeNameWeapon,
				this._sendingSkinId
			});
			this.SendSynhHealth(true, player);
			if (Defs.isDaterRegim && Defs.isJetpackEnabled)
			{
				this.photonView.RPC("SetJetpackEnabledRPC", player, new object[]
				{
					Defs.isJetpackEnabled
				});
			}
			this.GadgetsOnPlayerConnected();
			if (this.isBearActive)
			{
				this.photonView.RPC("ActivateMechRPC", player, new object[0]);
			}
			this.photonView.RPC("SynhIsZoming", player, new object[]
			{
				this.isZooming
			});
			if (ABTestController.useBuffSystem || KillRateCheck.instance.buffEnabled)
			{
				this.photonView.RPC("SendBuffParameters", player, new object[]
				{
					this.damageBuff,
					this.protectionBuff
				});
			}
		}
	}

	// Token: 0x060028E5 RID: 10469 RVA: 0x000D0610 File Offset: 0x000CE810
	public void ShowChat()
	{
		if (this.isKilled)
		{
			return;
		}
		this.ShotUnPressed(true);
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.jumpPressed = false;
			JoystickController.leftTouchPad.isJumpPressed = false;
			JoystickController.rightJoystick.Reset();
		}
		this.RemoveButtonHandelrs();
		this.showChat = true;
		if (this.inGameGUI.gameObject != null)
		{
			this.inGameGUI.gameObject.SetActive(false);
		}
		this._weaponManager.currentWeaponSounds.gameObject.SetActive(false);
		if (this.isMechActive)
		{
			if (Defs.isDaterRegim)
			{
				this.mechBearPoint.SetActive(false);
			}
			else if (this.currentMech != null)
			{
				this.currentMech.point.SetActive(false);
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chatViewer);
	}

	// Token: 0x060028E6 RID: 10470 RVA: 0x000D0700 File Offset: 0x000CE900
	public void SetInvisible(bool _isInvisible, bool byWeapon = false)
	{
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				base.GetComponent<NetworkView>().RPC("SetInvisibleRPC", RPCMode.All, new object[]
				{
					_isInvisible,
					byWeapon
				});
			}
			else if (this.photonView != null)
			{
				this.photonView.RPC("SetInvisibleRPC", PhotonTargets.All, new object[]
				{
					_isInvisible,
					byWeapon
				});
			}
		}
		else
		{
			this.SetInvisibleRPC(_isInvisible, byWeapon);
		}
	}

	// Token: 0x060028E7 RID: 10471 RVA: 0x000D0798 File Offset: 0x000CE998
	public void SetNicklabelVisible()
	{
		if (this.isMine)
		{
			return;
		}
		this.nickLabel.gameObject.SetActive(!this.isInvisible || ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture) && WeaponManager.sharedManager.myPlayerMoveC != null && this.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand));
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x000D0820 File Offset: 0x000CEA20
	[RPC]
	[PunRPC]
	private void SetInvisibleRPC(bool _isInvisible, bool _isInvisByWeapon = false)
	{
		if (!Defs.isDaterRegim)
		{
			if (_isInvisByWeapon)
			{
				this.isInvisByWeapon = _isInvisible;
			}
			else
			{
				this.isInvisByGadget = _isInvisible;
			}
			bool flag = this.isInvisByGadget || this.isInvisByWeapon;
			if (flag == this.isInvisible)
			{
				return;
			}
			this.isInvisible = flag;
			if (!this.isMulti || this.isMine)
			{
				this.SetInVisibleShaders(flag);
			}
			else
			{
				this.SetNicklabelVisible();
				if (!flag)
				{
					this.invisibleParticle.SetActive(false);
					if (this.isMechActive)
					{
						if (Defs.isDaterRegim)
						{
							this.mechBearPoint.SetActive(true);
						}
						else if (this.currentMech != null)
						{
							this.currentMech.point.SetActive(true);
						}
					}
					else
					{
						this.mySkinName.FPSplayerObject.SetActive(true);
					}
				}
				else
				{
					this.invisibleParticle.SetActive(true);
					this.mySkinName.FPSplayerObject.SetActive(false);
					if (Defs.isDaterRegim)
					{
						this.mechBearPoint.SetActive(false);
					}
					else if (this.currentMech != null)
					{
						this.currentMech.point.SetActive(false);
					}
				}
				if (this.myCurrentWeaponSounds != null)
				{
					this.myCurrentWeaponSounds.CheckForInvisible();
				}
			}
		}
		else
		{
			if (this.isBigHead == _isInvisible)
			{
				return;
			}
			this.isBigHead = _isInvisible;
			if (Defs.isSoundFX && _isInvisible)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.potionSound);
			}
			if (this.isMulti && !this.isMine)
			{
				if (_isInvisible)
				{
					this.MechHeadTransform.localScale = Vector3.one * 2f;
					this.PlayerHeadTransform.localScale = Vector3.one * 2f;
					if (this.isBearActive)
					{
						this.nickLabel.transform.localPosition = 2.549f * Vector3.up;
					}
					else
					{
						this.nickLabel.transform.localPosition = 1.678f * Vector3.up;
					}
				}
				else
				{
					this.MechHeadTransform.localScale = Vector3.one;
					this.PlayerHeadTransform.localScale = Vector3.one;
					if (this.isBearActive)
					{
						this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
					}
					else
					{
						this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
					}
				}
			}
		}
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000D0AD8 File Offset: 0x000CECD8
	private void SetInVisibleShaders(bool _isInvisible)
	{
		WeaponSounds currentWeaponSounds = WeaponManager.sharedManager.currentWeaponSounds;
		if (_isInvisible)
		{
			if (!this.isGrenadePress)
			{
				Shader shader = Shader.Find("Mobile/Diffuse-Color");
				this.oldWeaponHandMaterial = currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial;
				Material material = UnityEngine.Object.Instantiate<Material>(currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().material);
				currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial = material;
				material.shader = shader;
				Color color2;
				if (this.oldWeaponHandMaterial.HasProperty("_Color"))
				{
					Color color = this.oldWeaponHandMaterial.GetColor("_Color");
					color2 = new Color(color.r, color.g, color.b, 0.5f);
				}
				else
				{
					color2 = new Color(1f, 1f, 1f, 0.5f);
				}
				material.SetColor("_ColorRili", color2);
				if (currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
				{
					this.oldWeaponMaterials = currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().sharedMaterials;
					for (int i = 0; i < currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials.Length; i++)
					{
						currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].shader = shader;
						if (this.oldWeaponMaterials[i].HasProperty("_Color"))
						{
							Color color3 = this.oldWeaponMaterials[i].GetColor("_Color");
							color2 = new Color(color3.r, color3.g, color3.b, 0.5f);
						}
						else
						{
							color2 = new Color(1f, 1f, 1f, 0.5f);
						}
						currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().materials[i].SetColor("_ColorRili", color2);
					}
				}
			}
			if (this.isMechActive && this.currentMech != null)
			{
				this.currentMech.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
				this.currentMech.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			}
			this._bodyMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
		}
		else
		{
			if (!this.isGrenadePress)
			{
				currentWeaponSounds.bonusPrefab.transform.parent.GetComponent<Renderer>().sharedMaterial = this.oldWeaponHandMaterial;
				if (currentWeaponSounds.bonusPrefab.GetComponent<Renderer>() != null)
				{
					currentWeaponSounds.bonusPrefab.GetComponent<Renderer>().sharedMaterials = this.oldWeaponMaterials;
				}
			}
			if (this.isMechActive && this.currentMech != null)
			{
				this.currentMech.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
				this.currentMech.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
			}
			this._bodyMaterial.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		}
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000D0E8C File Offset: 0x000CF08C
	[RPC]
	[PunRPC]
	public void ActivateMechRPC(int num)
	{
		this.ActivateMech(string.Empty);
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x000D0E9C File Offset: 0x000CF09C
	[PunRPC]
	[RPC]
	public void ActivateMechRPC()
	{
		this.ActivateMech(string.Empty);
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000D0EAC File Offset: 0x000CF0AC
	[RPC]
	[PunRPC]
	public void DeactivateMechRPC()
	{
		this.DeactivateMech();
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x000D0EB4 File Offset: 0x000CF0B4
	private void SetWeaponVisible(bool visible)
	{
		Transform transform = (!(this.myCurrentWeaponSounds.grenatePoint != null) || this.myCurrentWeaponSounds.grenatePoint.transform.childCount <= 0) ? null : this.myCurrentWeaponSounds.grenatePoint.transform.GetChild(0);
		if (transform != null)
		{
			transform.parent = null;
		}
		this.myCurrentWeaponSounds.SetDaterBearHandsAnim(!visible);
		if (transform != null)
		{
			transform.parent = this.myCurrentWeaponSounds.grenatePoint;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x000D0F64 File Offset: 0x000CF164
	public void ActivateBear()
	{
		if (this.isBearActive)
		{
			return;
		}
		float num = -1f;
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			num = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		this.mechExplossionSound = this.mechBearExplosionSound;
		this.mechExplossion = this.bearExplosion;
		if ((!Defs.isMulti || this.isMine) && this.isZooming)
		{
			this.ZoomPress();
		}
		this.deltaAngle = 0f;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.mechBearActivSound);
		}
		this.isBearActive = true;
		this.fpsPlayerBody.SetActive(false);
		if (this.myCurrentWeapon != null)
		{
			this.SetWeaponVisible(false);
		}
		if (this.isMine || (!this.isMine && !this.isInvisible) || !this.isMulti)
		{
			this.mechBearPoint.SetActive(true);
		}
		this.mechBearPoint.GetComponent<DisableObjectFromTimer>().timer = -1f;
		if (!this.isMulti || this.isMine)
		{
			base.transform.localPosition = this.myCamera.transform.localPosition;
			this.mechBearBody.SetActive(false);
			this.mechBearSyncRot.enabled = true;
			this.mechBearPoint.transform.localPosition = Vector3.zero;
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (this.myCurrentWeaponSounds.animationObject != null)
			{
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (!this.myCurrentWeaponSounds.isDoubleShot)
				{
					if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
					}
				}
				else
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
			}
		}
		else
		{
			this.bodyCollayder.height = 2.07f;
			this.bodyCollayder.center = new Vector3(0f, 0.19f, 0f);
			this.headCollayder.center = new Vector3(0f, 0.54f, 0f);
			if (this.isBigHead)
			{
				this.nickLabel.transform.localPosition = 2.549f * Vector3.up;
			}
			else
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
		}
		this.liveMech = this.liveMechByTier[0];
		if (this.isMulti && this.isMine)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("ActivateMechRPC", PhotonTargets.Others, new object[0]);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("ActivateMechRPC", RPCMode.Others, new object[0]);
			}
		}
		if (num != -1f)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = num;
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000D137C File Offset: 0x000CF57C
	public void DeactivateBear()
	{
		if (!this.isBearActive)
		{
			return;
		}
		this.isBearActive = false;
		float num = -1f;
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			num = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time;
		}
		if (this.myCurrentWeapon != null)
		{
			this.SetWeaponVisible(true);
		}
		this.myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
		if (Defs.isSoundFX)
		{
			this.mechExplossionSound.Play();
		}
		if (this.isMulti && !this.isMine)
		{
			if (!this.isInvisible)
			{
				this.fpsPlayerBody.SetActive(true);
			}
			this.bodyCollayder.height = 1.51f;
			this.bodyCollayder.center = Vector3.zero;
			this.headCollayder.center = Vector3.zero;
			this.mechExplossion.SetActive(true);
			this.mechExplossion.GetComponent<DisableObjectFromTimer>().timer = 1f;
			this.mechBearBodyAnimation.Play("Dead");
			this.mechBearPoint.GetComponent<DisableObjectFromTimer>().timer = 0.46f;
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().cullingType = AnimationCullingType.AlwaysAnimate;
			if (this.myCurrentWeaponSounds.animationObject != null)
			{
				if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload") != null)
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].layer = 1;
				}
				if (!this.myCurrentWeaponSounds.isDoubleShot)
				{
					if (this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Shoot") != null)
					{
						this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].layer = 1;
					}
				}
				else
				{
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot1"].layer = 1;
					this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot2"].layer = 1;
				}
			}
			if (this.isBigHead)
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.54f;
			}
			else
			{
				this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
			}
		}
		else
		{
			this.mechBearPoint.SetActive(false);
			this.gunCamera.fieldOfView = 75f;
			base.transform.localPosition = this.myCamera.transform.localPosition;
			this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
		}
		if (!this.isMulti || this.isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Mech, this, true);
		}
		if (this.isMulti && this.isMine)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("DeactivateMechRPC", PhotonTargets.Others, new object[0]);
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("DeactivateMechRPC", RPCMode.Others, new object[0]);
			}
		}
		if (num != -1f)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].time = Mathf.Min(num, this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].length);
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000D17A0 File Offset: 0x000CF9A0
	public void ActivateMech(string id = "")
	{
		if (this.isMechActive)
		{
			return;
		}
		if (Defs.isDaterRegim)
		{
			this.ActivateBear();
			return;
		}
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000D17C0 File Offset: 0x000CF9C0
	public void DeactivateMech()
	{
		if (Defs.isDaterRegim)
		{
			this.DeactivateBear();
			return;
		}
		if (!this.isMechActive)
		{
			return;
		}
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000D17E0 File Offset: 0x000CF9E0
	public void UpdateEffectsForCurrentWeapon(string currentCape, string currentMask, string currentHat)
	{
		if (this.myCurrentWeaponSounds == null)
		{
			return;
		}
		if (!this.isMine)
		{
			this._chanceToIgnoreHeadshot = EffectsController.GetChanceToIgnoreHeadshot(this.myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
		}
		this._currentReloadAnimationSpeed = EffectsController.GetReloadAnimationSpeed(this.myCurrentWeaponSounds.categoryNabor, currentCape, currentMask, currentHat);
		this._protectionShieldValue = 1f;
		bool flag = !this.isMechActive && this.myCurrentWeaponSounds.specialEffect == WeaponSounds.SpecialEffects.PlayerShield;
		this._protectionShieldValue = ((!flag) ? 1f : this.myCurrentWeaponSounds.protectionEffectValue);
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000D1888 File Offset: 0x000CFA88
	public void BlockPlayerInEnd()
	{
		this.mySkinName.BlockFirstPersonController();
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().enabled = false;
		if (this.GunFlash != null)
		{
			this.GunFlash.gameObject.SetActive(false);
		}
		this.mySkinName.character.enabled = false;
		base.enabled = false;
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000D18F0 File Offset: 0x000CFAF0
	public Transform GetPointForFlyingPet()
	{
		for (int i = 0; i < this.petPointsFlying.Length; i++)
		{
			if (!this.petPointsFlying[i].isCollision)
			{
				return this.petPointsFlying[i].thisTransform;
			}
		}
		return null;
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000D1938 File Offset: 0x000CFB38
	public Transform GetPointForGroundPet()
	{
		for (int i = 0; i < this.petPointsGround.Length; i++)
		{
			if (!this.petPointsGround[i].isCollision)
			{
				return this.petPointsGround[i].thisTransform;
			}
		}
		return null;
	}

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000D1980 File Offset: 0x000CFB80
	// (set) Token: 0x060028F7 RID: 10487 RVA: 0x000D1988 File Offset: 0x000CFB88
	public PetEngine myPetEngine { get; private set; }

	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x060028F8 RID: 10488 RVA: 0x000D1994 File Offset: 0x000CFB94
	// (set) Token: 0x060028F9 RID: 10489 RVA: 0x000D199C File Offset: 0x000CFB9C
	private GameObject myPet
	{
		get
		{
			return this.myPetValue;
		}
		set
		{
			this.myPetValue = value;
			this.myPetEngine = ((!(this.myPetValue != null)) ? null : this.myPet.GetComponent<PetEngine>());
		}
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000D19D8 File Offset: 0x000CFBD8
	public void EquippedPet(string newPet, string oldPet)
	{
		this.UpdatePet();
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x000D19E0 File Offset: 0x000CFBE0
	public void UnequipPet(string oldPet)
	{
		this.UpdatePet();
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000D19E8 File Offset: 0x000CFBE8
	private void UpdatePet()
	{
		if (Defs.isHunger)
		{
			return;
		}
		string eqipedPetId = Singleton<PetsManager>.Instance.GetEqipedPetId();
		this.mySkinName.SetPet(null);
		if (this.myPet != null && this.myPet.name.Replace("(Clone)", string.Empty) != eqipedPetId)
		{
			this.myPetEngine.Destroy();
			this.myPet = null;
		}
		if (string.IsNullOrEmpty(eqipedPetId) || this.myPet != null)
		{
			return;
		}
		string relativePrefabPath = Singleton<PetsManager>.Instance.GetRelativePrefabPath(eqipedPetId);
		PetInfo info = Singleton<PetsManager>.Instance.GetInfo(eqipedPetId);
		Vector3 position;
		if (info == null)
		{
			position = this.GetPointForFlyingPet().position;
		}
		else
		{
			position = ((info.Behaviour != PetInfo.BehaviourType.Ground) ? this.GetPointForFlyingPet().position : this.GetPointForGroundPet().position);
		}
		if (this.isMulti)
		{
			if (Defs.isInet)
			{
				this.myPet = PhotonNetwork.Instantiate(relativePrefabPath, position, this.myPlayerTransform.rotation, 0);
			}
			else
			{
				GameObject prefab = Resources.Load(relativePrefabPath) as GameObject;
				this.myPet = (Network.Instantiate(prefab, position, this.myPlayerTransform.rotation, 0) as GameObject);
				this.myPet.GetComponent<PetEngine>().SendOwnerLocalRPC(this.myPlayerIDLocal);
			}
		}
		else
		{
			GameObject original = Resources.Load(relativePrefabPath) as GameObject;
			this.myPet = (UnityEngine.Object.Instantiate(original, position, this.myPlayerTransform.rotation) as GameObject);
		}
		this.myPet.GetComponent<PetEngine>().SetInfo(eqipedPetId);
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x000D1B88 File Offset: 0x000CFD88
	public void SendPlayerEffect(int effectIndex, float effectTime = 0f)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("PlayerEffectRPC", PhotonTargets.Others, new object[]
				{
					effectIndex,
					effectTime
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("PlayerEffectRPC", RPCMode.Others, new object[]
				{
					effectIndex,
					effectTime
				});
			}
		}
		this.PlayerEffectRPC(effectIndex, effectTime);
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x000D1C08 File Offset: 0x000CFE08
	public void SendPlayerEffectToPlayer(PhotonPlayer photonPlayer, NetworkPlayer localPlayer, int effectIndex, float effectTime = 0f)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("PlayerEffectRPC", photonPlayer, new object[]
				{
					effectIndex,
					effectTime
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("PlayerEffectRPC", localPlayer, new object[]
				{
					effectIndex,
					effectTime
				});
			}
		}
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000D1C84 File Offset: 0x000CFE84
	[RPC]
	[PunRPC]
	public void PlayerEffectRPC(int effectIndex, float effectTime)
	{
		if (effectTime == 0f)
		{
			this.ActivatePlayerEffect((Player_move_c.PlayerEffect)effectIndex, effectTime);
			return;
		}
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].effect == (Player_move_c.PlayerEffect)effectIndex)
			{
				this.playerEffects[i] = this.playerEffects[i].UpdateTime(effectTime);
				return;
			}
		}
		this.playerEffects.Add(new Player_move_c.ActivePlayerEffect((Player_move_c.PlayerEffect)effectIndex, effectTime));
		this.ActivatePlayerEffect((Player_move_c.PlayerEffect)effectIndex, effectTime);
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000D1D1C File Offset: 0x000CFF1C
	public bool IsPlayerEffectActive(Player_move_c.PlayerEffect effect)
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000D1D64 File Offset: 0x000CFF64
	private void ActivatePlayerEffect(Player_move_c.PlayerEffect effect, float time)
	{
		bool flag = !Defs.isMulti || this.isMine;
		switch (effect)
		{
		case Player_move_c.PlayerEffect.burning:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.burningEffect.Play(time);
				}
			}
			else
			{
				this.burningEffect = ParticleStacks.instance.fireStack.GetParticle();
				if (this.burningEffect != null)
				{
					this.burningEffect.transform.SetParent(this.myPlayerTransform, false);
					this.burningEffect.transform.localPosition = Vector3.zero;
				}
			}
			break;
		case Player_move_c.PlayerEffect.fireMushroom:
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.mushroomShotSound);
			}
			this.RunOnGroundEffect("Weapon278");
			break;
		case Player_move_c.PlayerEffect.disabler:
			if (flag)
			{
				this.gadgetsDisabled = true;
				if (Defs.isSoundFX)
				{
					this.myAudioSource.PlayOneShot(this.disablerEffectSound);
				}
			}
			break;
		case Player_move_c.PlayerEffect.blackMark:
			if (!flag)
			{
				this.blackMark.SetActive(true);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.blackMarkEffect.Play(time);
			}
			break;
		case Player_move_c.PlayerEffect.dragon:
		{
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.dragonWhistleSound);
			}
			GameObject dragonPrefab = ParticleStacks.instance.dragonPrefab;
			GameObject gameObject = UnityEngine.Object.Instantiate(dragonPrefab, this.myPlayerTransform.position, this.myPlayerTransform.rotation) as GameObject;
			gameObject.SetActive(true);
			break;
		}
		case Player_move_c.PlayerEffect.lightningEnemies:
		{
			Initializer.TargetsList targetsList = new Initializer.TargetsList(this, false, true);
			foreach (Transform transform in targetsList)
			{
				GameObject particle = ParticleStacks.instance.lightningStack.GetParticle();
				particle.transform.position = transform.position;
			}
			break;
		}
		case Player_move_c.PlayerEffect.disablerEffect:
			if (flag && this.inGameGUI != null)
			{
				this.inGameGUI.disablerEffect.Play();
			}
			this.RunOnGroundEffect("gadget_disabler");
			break;
		case Player_move_c.PlayerEffect.resurrection:
			if (!flag)
			{
				this.resurrectionEffect.SetActive(true);
				this.resurrectionEffect.GetComponent<DisableObjectFromTimer>().timer = 2f;
			}
			break;
		case Player_move_c.PlayerEffect.timeTravel:
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.timeWatchSound);
			}
			if (!flag)
			{
				this.timeTravelEffect.SetActive(true);
				this.timeTravelEffect.GetComponent<DisableObjectFromTimer>().timer = 2f;
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.timeTravelEffect.Play();
			}
			break;
		case Player_move_c.PlayerEffect.lightningSelf:
		{
			GameObject particle2 = ParticleStacks.instance.lightningStack.GetParticle();
			particle2.transform.position = this.myPlayerTransform.position;
			break;
		}
		case Player_move_c.PlayerEffect.clearPoisons:
			if (WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				for (int i = 0; i < WeaponManager.sharedManager.myPlayerMoveC.poisonTargets.Count; i++)
				{
					if (WeaponManager.sharedManager.myPlayerMoveC.poisonTargets[i].target.Equals(this.myDamageable))
					{
						WeaponManager.sharedManager.myPlayerMoveC.poisonTargets[i].hitCount = 0;
					}
				}
			}
			this.RemoveEffect(Player_move_c.PlayerEffect.burning);
			break;
		}
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000D2144 File Offset: 0x000D0344
	private void DeactivatePlayerEffect(Player_move_c.PlayerEffect effect)
	{
		bool flag = !Defs.isMulti || this.isMine;
		switch (effect)
		{
		case Player_move_c.PlayerEffect.burning:
			if (flag)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.burningEffect.Stop();
				}
			}
			else if (this.burningEffect != null && ParticleStacks.instance != null)
			{
				ParticleStacks.instance.fireStack.ReturnParticle(this.burningEffect);
				this.burningEffect = null;
			}
			break;
		case Player_move_c.PlayerEffect.disabler:
			if (flag)
			{
				this.gadgetsDisabled = false;
			}
			break;
		case Player_move_c.PlayerEffect.blackMark:
			if (!flag)
			{
				this.blackMark.SetActive(false);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.blackMarkEffect.Stop();
			}
			break;
		}
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x000D2240 File Offset: 0x000D0440
	private void UpdateEffects()
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].lifeTime < Time.time)
			{
				this.DeactivatePlayerEffect(this.playerEffects[i].effect);
				this.playerEffects.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x000D22B4 File Offset: 0x000D04B4
	private void DestroyEffects()
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].effect == Player_move_c.PlayerEffect.burning)
			{
				this.DeactivatePlayerEffect(this.playerEffects[i].effect);
				this.playerEffects.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x000D2324 File Offset: 0x000D0524
	public void RemoveEffect(Player_move_c.PlayerEffect effect)
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].effect == effect)
			{
				this.DeactivatePlayerEffect(effect);
				this.playerEffects.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x17000727 RID: 1831
	// (get) Token: 0x06002906 RID: 10502 RVA: 0x000D237C File Offset: 0x000D057C
	public WeaponSounds mechWeaponSounds
	{
		get
		{
			if (this.currentMech != null)
			{
				return this.currentMech.weapon;
			}
			return null;
		}
	}

	// Token: 0x17000728 RID: 1832
	// (get) Token: 0x06002907 RID: 10503 RVA: 0x000D239C File Offset: 0x000D059C
	public bool canUseGadgets
	{
		get
		{
			return this.showGadgetsPanel && Defs.isGrenateFireEnable && !this.gadgetsDisabled && !this.isMechActive && !this.isKilled;
		}
	}

	// Token: 0x17000729 RID: 1833
	// (get) Token: 0x06002908 RID: 10504 RVA: 0x000D23D8 File Offset: 0x000D05D8
	public bool showGadgetsPanel
	{
		get
		{
			return this.gadgetsPanelEnabled && !Defs.isZooming && !Defs.isTurretWeapon && InGameGUI.sharedInGameGUI != null && !InGameGUI.sharedInGameGUI.isTurretInterfaceActive;
		}
	}

	// Token: 0x1700072A RID: 1834
	// (get) Token: 0x06002909 RID: 10505 RVA: 0x000D2424 File Offset: 0x000D0624
	public bool gadgetsPanelEnabled
	{
		get
		{
			return !Defs.isHunger && !Defs.isDaterRegim && WeaponManager.sharedManager._currentFilterMap != 1 && WeaponManager.sharedManager._currentFilterMap != 2 && (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None);
		}
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000D2480 File Offset: 0x000D0680
	private void GadgetsUpdate()
	{
		if (!Defs.isMulti || this.isMine)
		{
			this.AddTimeWatchPositions();
			if (this.gadgetsPanelEnabled)
			{
				Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
				while (enumerator.MoveNext())
				{
					KeyValuePair<GadgetInfo.GadgetCategory, Gadget> keyValuePair = enumerator.Current;
					Gadget value = keyValuePair.Value;
					value.Step(Time.deltaTime);
				}
				enumerator.Dispose();
			}
			bool flag = this.drumActive;
			this.drumActive = false;
			this.drumDamageMultiplier = 1f;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				Player_move_c player_move_c = Initializer.players[i];
				if (((!Defs.isCOOP && !player_move_c.myDamageable.IsEnemyTo(this)) || player_move_c.Equals(this)) && (player_move_c.myPlayerTransform.position - this.myPlayerTransform.position).sqrMagnitude < this.drumSupportRadius * this.drumSupportRadius)
				{
					for (int j = 0; j < player_move_c.activatedGadgetEffects.Count; j++)
					{
						if (player_move_c.activatedGadgetEffects[j].effect == Player_move_c.GadgetEffect.drumSupport)
						{
							this.drumActive = true;
							this.drumDamageMultiplier = Mathf.Max(this.drumDamageMultiplier, 1f + 0.01f * GadgetsInfo.info[player_move_c.activatedGadgetEffects[j].gadgetID].Amplification);
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x000D2624 File Offset: 0x000D0824
	public void GadgetsOnMatchEnd()
	{
		if (!Defs.isMulti || this.isMine)
		{
			Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<GadgetInfo.GadgetCategory, Gadget> keyValuePair = enumerator.Current;
				Gadget value = keyValuePair.Value;
				value.OnMatchEnd();
			}
			enumerator.Dispose();
		}
	}

	// Token: 0x0600290C RID: 10508 RVA: 0x000D2680 File Offset: 0x000D0880
	private void GadgetsOnPlayerConnected()
	{
		for (int i = 0; i < this.activatedGadgetEffects.Count; i++)
		{
			this.SetGadgetEffectActivation(this.activatedGadgetEffects[i].effect, true, this.activatedGadgetEffects[i].gadgetID);
		}
	}

	// Token: 0x0600290D RID: 10509 RVA: 0x000D26D8 File Offset: 0x000D08D8
	public Gadget CurrentDefaultGadget()
	{
		Gadget result = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x000D2700 File Offset: 0x000D0900
	public void CurrentGadgetPreUse()
	{
		if (!this.canUseGadgets)
		{
			return;
		}
		Gadget gadget = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget) && gadget.CanUse)
		{
			gadget.PreUse();
			this.gadgetWasPreused = true;
		}
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x000D274C File Offset: 0x000D094C
	public void CurrentGadgetUse()
	{
		if (!this.canUseGadgets)
		{
			return;
		}
		if (!this.gadgetWasPreused)
		{
			return;
		}
		this.gadgetWasPreused = false;
		Gadget gadget = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget) && gadget.CanUse)
		{
			gadget.Use();
		}
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x000D27A4 File Offset: 0x000D09A4
	public void GadgetOnPlayerDeath(bool inDeathCollider = false)
	{
		if (this.gadgetsDisabled || !this.gadgetsPanelEnabled)
		{
			return;
		}
		Dictionary<GadgetInfo.GadgetCategory, Gadget>.Enumerator enumerator = InGameGadgetSet.CurrentSet.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<GadgetInfo.GadgetCategory, Gadget> keyValuePair = enumerator.Current;
			Gadget value = keyValuePair.Value;
			if (value.CanUse)
			{
				value.OnKill(inDeathCollider);
			}
		}
		enumerator.Dispose();
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000D2810 File Offset: 0x000D0A10
	public void SetGadgetEffectActivation(Player_move_c.GadgetEffect effect, bool acitve, string gadgetID = "")
	{
		this.SetGadgetEffectActiveRPC((int)effect, acitve, gadgetID);
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SetGadgetEffectActiveRPC", PhotonTargets.Others, new object[]
				{
					(int)effect,
					acitve,
					gadgetID
				});
			}
			else
			{
				this._networkView.RPC("SetGadgetEffectActiveRPC", RPCMode.Others, new object[]
				{
					(int)effect,
					acitve,
					gadgetID
				});
			}
		}
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x000D289C File Offset: 0x000D0A9C
	public bool IsGadgetSelected(string name)
	{
		Gadget gadget = null;
		return InGameGadgetSet.CurrentSet.TryGetValue(GadgetsInfo.DefaultGadget, out gadget) && gadget.CanUse && gadget.Info.Id.Equals(name);
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000D28E0 File Offset: 0x000D0AE0
	public bool IsGadgetEffectActive(Player_move_c.GadgetEffect effect)
	{
		for (int i = 0; i < this.activatedGadgetEffects.Count; i++)
		{
			if (this.activatedGadgetEffects[i].effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000D2928 File Offset: 0x000D0B28
	[RPC]
	[PunRPC]
	private void SetGadgetEffectActiveRPC(int effectIndex, bool active, string gadgetId = "")
	{
		if (active == this.IsGadgetEffectActive((Player_move_c.GadgetEffect)effectIndex))
		{
			return;
		}
		if (active)
		{
			this.activatedGadgetEffects.Add(new Player_move_c.GadgetEffectParams((Player_move_c.GadgetEffect)effectIndex, gadgetId));
			this.ActivateGadgetEffect((Player_move_c.GadgetEffect)effectIndex, gadgetId);
		}
		else
		{
			for (int i = 0; i < this.activatedGadgetEffects.Count; i++)
			{
				if (this.activatedGadgetEffects[i].effect == (Player_move_c.GadgetEffect)effectIndex)
				{
					this.activatedGadgetEffects.RemoveAt(i);
				}
			}
			this.DeactivateGadgetEffect((Player_move_c.GadgetEffect)effectIndex);
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000D29B4 File Offset: 0x000D0BB4
	private void ActivateGadgetEffect(Player_move_c.GadgetEffect effect, string gadgetID)
	{
		switch (effect)
		{
		case Player_move_c.GadgetEffect.reflector:
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.reflectorOnSound);
				this.reflectorPulseSound.SetActive(true);
			}
			if (Defs.isMulti && !this.isMine)
			{
				this.reflectorParticles.SetActive(true);
			}
			else if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.shieldEffect.Play(GadgetsInfo.info[gadgetID].Duration);
			}
			break;
		case Player_move_c.GadgetEffect.mech:
		case Player_move_c.GadgetEffect.demon:
			this.ActivateBody(effect, GadgetsInfo.info[gadgetID]);
			break;
		case Player_move_c.GadgetEffect.invisible:
			this.SetInvisibleRPC(true, false);
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.invisibleActivSound);
			}
			break;
		case Player_move_c.GadgetEffect.jetpack:
			this.ActivateJetpackGadget(true);
			break;
		case Player_move_c.GadgetEffect.drumSupport:
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.drumActiveSound);
			}
			if (this.isMine)
			{
				InGameGUI.sharedInGameGUI.drumEffect.Play(true);
			}
			if (this.drumLoopSound != null)
			{
				this.drumLoopSound.SetActive(true);
			}
			break;
		case Player_move_c.GadgetEffect.petAdrenaline:
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.petBoosterActiveSound);
			}
			break;
		}
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000D2B34 File Offset: 0x000D0D34
	private void DeactivateGadgetEffect(Player_move_c.GadgetEffect effect)
	{
		switch (effect)
		{
		case Player_move_c.GadgetEffect.reflector:
			if (Defs.isSoundFX)
			{
				this.myAudioSource.PlayOneShot(this.reflectorOffSound);
			}
			this.reflectorPulseSound.SetActive(false);
			if (Defs.isMulti && !this.isMine)
			{
				this.reflectorParticles.SetActive(false);
			}
			break;
		case Player_move_c.GadgetEffect.mech:
		case Player_move_c.GadgetEffect.demon:
			this.DeactivateBody(effect);
			break;
		case Player_move_c.GadgetEffect.invisible:
			this.SetInvisibleRPC(false, false);
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.invisibleDeactivSound);
			}
			break;
		case Player_move_c.GadgetEffect.jetpack:
			this.ActivateJetpackGadget(false);
			break;
		case Player_move_c.GadgetEffect.drumSupport:
			if (this.isMine)
			{
				InGameGUI.sharedInGameGUI.drumEffect.Play(false);
			}
			if (this.drumLoopSound != null)
			{
				this.drumLoopSound.SetActive(false);
			}
			break;
		}
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000D2C34 File Offset: 0x000D0E34
	public bool ApplyMedkit(GadgetInfo _info)
	{
		if (this.CurHealth >= this.MaxHealth && (this.myPetEngine == null || !this.myPetEngine.IsAlive || this.myPetEngine.CurrentHealth >= this.myPetEngine.Info.HP))
		{
			return false;
		}
		this.SendPlayerEffect(13, 0f);
		float num = this.CurHealth + _info.Heal;
		if (num >= this.MaxHealth)
		{
			this.CurHealth = this.MaxHealth;
		}
		else
		{
			this.CurHealth = num;
		}
		if (Defs.isMulti)
		{
			this.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
		}
		if (Defs.isSoundFX)
		{
			this.myAudioSource.PlayOneShot(this.medkitSound);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.healEffect.Play();
		}
		if (this.myPetEngine != null)
		{
			this.myPetEngine.UpdateCurrentHealth(this.myPetEngine.CurrentHealth + _info.Heal);
		}
		return true;
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000D2D50 File Offset: 0x000D0F50
	public void ApplyResurrection(bool inDeathCollider = false)
	{
		if (!this.wasResurrected)
		{
			this.resurrectionPosition = this.myPlayerTransform.position;
		}
		this.wasResurrected = true;
		this.deadInCollider = inDeathCollider;
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000D2D88 File Offset: 0x000D0F88
	public void ResetWatchPositions()
	{
		this.firstPositionsReached = false;
		this.currentTimeIndex = 0;
		this.nextTimeAdd = 0f;
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x0600291A RID: 10522 RVA: 0x000D2DA4 File Offset: 0x000D0FA4
	public bool wasTimeJump
	{
		get
		{
			if (this._isTimeJump)
			{
				this._isTimeJump = false;
				return true;
			}
			return false;
		}
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000D2DBC File Offset: 0x000D0FBC
	private void AddTimeWatchPositions()
	{
		if (this.isKilled || this.CurHealth <= 0f || this.myPlayerTransform.position.y < -5000f)
		{
			this.ResetWatchPositions();
			return;
		}
		if (this.nextTimeAdd < Time.time)
		{
			this.timeRotations[this.currentTimeIndex] = this.myPlayerTransform.rotation;
			this.timeGunRotations[this.currentTimeIndex] = this.myCamera.transform.rotation;
			this.timePositions[this.currentTimeIndex++] = this.myPlayerTransform.position;
			if (this.currentTimeIndex >= this.timePositions.Length)
			{
				this.firstPositionsReached = true;
				this.currentTimeIndex = 0;
			}
			this.nextTimeAdd = Time.time + 0.3f;
		}
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000D2EC0 File Offset: 0x000D10C0
	public void ApplyTimeWatch()
	{
		this._isTimeJump = true;
		Vector3 position = this.timePositions[(!this.firstPositionsReached) ? 0 : this.currentTimeIndex];
		Quaternion rotation = this.timeRotations[(!this.firstPositionsReached) ? 0 : this.currentTimeIndex];
		Quaternion rotation2 = this.timeGunRotations[(!this.firstPositionsReached) ? 0 : this.currentTimeIndex];
		this.myPlayerTransform.position = position;
		this.myPlayerTransform.rotation = rotation;
		this.myCamera.transform.rotation = rotation2;
		base.StartCoroutine(this.StartTimeWatchEffect());
		this.ResetWatchPositions();
		this.SendPlayerEffect(10, 0f);
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000D2F98 File Offset: 0x000D1198
	private IEnumerator StartTimeWatchEffect()
	{
		this.myCamera.fieldOfView = 1f;
		while (this.myCamera.fieldOfView < this.stdFov)
		{
			this.myCamera.fieldOfView = Mathf.MoveTowards(this.myCamera.fieldOfView, this.stdFov, Time.deltaTime * (10f + 10f * (this.stdFov - this.myCamera.fieldOfView)));
			yield return null;
		}
		this.myCamera.fieldOfView = this.stdFov;
		yield break;
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000D2FB4 File Offset: 0x000D11B4
	public void ResurrectionEvent()
	{
		if (Defs.isMulti)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.resurrection, 1f);
		}
		this.wasResurrected = false;
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000D2FDC File Offset: 0x000D11DC
	private void ActivateBody(Player_move_c.GadgetEffect type, GadgetInfo param)
	{
		if (this.isMechActive || this.currentMech != null)
		{
			return;
		}
		this.SendSynhHealth(true, null);
		this.isMechActive = true;
		bool flag = !Defs.isMulti || this.isMine;
		if (flag)
		{
			if (this.isZooming)
			{
				this.ZoomPress();
			}
			InGameGUI.sharedInGameGUI.StopAllCircularIndicators();
			this.ShotUnPressed(true);
		}
		this.deltaAngle = 0f;
		this.fpsPlayerBody.SetActive(false);
		if (this.myCurrentWeapon != null)
		{
			this.myCurrentWeapon.SetActive(false);
		}
		PlayerMechBody playerMechBody = null;
		switch (type)
		{
		case Player_move_c.GadgetEffect.mech:
			playerMechBody = this.mechBodyScript;
			break;
		case Player_move_c.GadgetEffect.demon:
			playerMechBody = this.demonBodyScript;
			break;
		}
		if (playerMechBody == null)
		{
			return;
		}
		this.currentMech = playerMechBody;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(playerMechBody.activationSound);
		}
		if (flag || !this.isInvisible)
		{
			playerMechBody.point.SetActive(true);
		}
		if (param != null)
		{
			this.liveMech = param.Durability;
			playerMechBody.weapon.damage = param.SurvivalDamage;
			for (int i = 0; i < playerMechBody.weapon.damageByTier.Length; i++)
			{
				playerMechBody.weapon.damageByTier[i] = param.Damage;
			}
		}
		if (flag)
		{
			Player_move_c.SetLayerRecursively(playerMechBody.gun, 9);
			playerMechBody.body.SetActive(false);
			this.myCamera.transform.localPosition = playerMechBody.cameraPosition;
			playerMechBody.transform.localPosition = new Vector3(0f, -0.3f, 0f);
			this.gunCamera.fieldOfView = playerMechBody.gunCameraFieldOfView;
			this.gunCamera.transform.localPosition = playerMechBody.gunCameraPosition;
			if (this.inGameGUI != null)
			{
				this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
				this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
			}
			if (this.isInvisible)
			{
				playerMechBody.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
				playerMechBody.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 0.5f));
			}
			else
			{
				playerMechBody.handsRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
				playerMechBody.gunRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
			}
			if (this.inGameGUI != null)
			{
				this.inGameGUI.SetCrosshair(playerMechBody.weapon);
			}
		}
		else
		{
			playerMechBody.ShowActivationEffect();
			playerMechBody.gun.SetActive(!playerMechBody.dontShowHandsInThirdPerson);
			this.bodyCollayder.height = playerMechBody.bodyColliderHeight;
			this.bodyCollayder.center = playerMechBody.bodyColliderCenter;
			this.headCollayder.center = playerMechBody.headColliderCenter;
			this.nickLabel.transform.localPosition = Vector3.up * playerMechBody.nickLabelYPoision;
			this.currentMech.jetpackObject.SetActive(this.jetpackEnabled);
		}
		if (!playerMechBody.weapon.isMelee)
		{
			for (int j = 0; j < playerMechBody.weapon.gunFlashDouble.Length; j++)
			{
				playerMechBody.weapon.gunFlashDouble[j].GetChild(0).gameObject.SetActive(false);
			}
		}
		playerMechBody.ShowHitMaterial(false, false);
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000D3420 File Offset: 0x000D1620
	private void DeactivateBody(Player_move_c.GadgetEffect type)
	{
		PlayerMechBody x = null;
		switch (type)
		{
		case Player_move_c.GadgetEffect.mech:
			x = this.mechBodyScript;
			break;
		case Player_move_c.GadgetEffect.demon:
			x = this.demonBodyScript;
			break;
		}
		if (x == null || x != this.currentMech)
		{
			return;
		}
		this.DeactivateCurrentBody();
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000D348C File Offset: 0x000D168C
	private void DeactivateCurrentBody()
	{
		if (!this.isMechActive || this.currentMech == null)
		{
			return;
		}
		this.isMechActive = false;
		bool flag = !Defs.isMulti || this.isMine;
		if (flag && this.GadgetsOnMechKill != null)
		{
			this.GadgetsOnMechKill();
		}
		if (this.myCurrentWeapon != null)
		{
			this.myCurrentWeapon.SetActive(true);
		}
		PlayerMechBody playerMechBody = this.currentMech;
		this.currentMech = null;
		if (Defs.isSoundFX)
		{
			playerMechBody.explosionSound.Play();
		}
		if (flag)
		{
			this.myCamera.transform.localPosition = new Vector3(0f, 0.7f, 0f);
			playerMechBody.point.SetActive(false);
			this.gunCamera.fieldOfView = 75f;
			if (this.myCurrentWeaponSounds.isDoubleShot)
			{
				this.gunCamera.transform.localPosition = Vector3.zero;
			}
			else
			{
				this.gunCamera.transform.localPosition = new Vector3(-0.1f, 0f, 0f);
			}
			if (this.inGameGUI != null)
			{
				if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee)
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_strike";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_strike";
				}
				else
				{
					this.inGameGUI.fireButtonSprite.spriteName = "controls_fire";
					this.inGameGUI.fireButtonSprite2.spriteName = "controls_fire";
				}
			}
			if (this.inGameGUI != null)
			{
				this.inGameGUI.SetCrosshair(this._weaponManager.currentWeaponSounds);
			}
		}
		else
		{
			if (!this.isInvisible)
			{
				this.fpsPlayerBody.SetActive(true);
			}
			this.bodyCollayder.height = 1.51f;
			this.bodyCollayder.center = Vector3.zero;
			this.headCollayder.center = Vector3.zero;
			playerMechBody.ShowExplosionEffect();
			playerMechBody.bodyAnimation.Play("Dead");
			playerMechBody.gunAnimation.Play("Dead");
			this.nickLabel.transform.localPosition = Vector3.up * 1.08f;
		}
		this.mySkinName.SetAnim(this.mySkinName.currentAnim, EffectsController.WeAreStealth);
		this.UpdateEffectsForCurrentWeapon(this.mySkinName.currentCape, this.mySkinName.currentMask, this.mySkinName.currentHat);
	}

	// Token: 0x06002922 RID: 10530 RVA: 0x000D3754 File Offset: 0x000D1954
	public void ActivateFireMushroom()
	{
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.mushroomActivationSound);
		}
	}

	// Token: 0x06002923 RID: 10531 RVA: 0x000D3774 File Offset: 0x000D1974
	public void FireMushroomShot(GadgetInfo info)
	{
		this.SendPlayerEffect(2, 0f);
		float num = this.mushroomRadius * this.mushroomRadius;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			float sqrMagnitude = (transform.position - this._player.transform.position).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				this.DamageTarget(transform.gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, Player_move_c.TypeKills.none);
				this.PoisonShotWithEffect(transform.gameObject, new Player_move_c.PoisonParameters(Player_move_c.PoisonType.Burn, this.mushroomBurnDamage, info.Damage, this.mushroomBurnTime, this.mushroomBurnCount, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion));
			}
		}
	}

	// Token: 0x06002924 RID: 10532 RVA: 0x000D3870 File Offset: 0x000D1A70
	public void DisablerGadget(GadgetInfo info)
	{
		if (Defs.isSoundFX)
		{
			this.myAudioSource.PlayOneShot(this.disablerActiveSound);
		}
		this.SendPlayerEffect(7, 0f);
		if (Defs.isCOOP)
		{
			return;
		}
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (!Initializer.players[i].Equals(this) && (!ConnectSceneNGUIController.isTeamRegim || this.myCommand != Initializer.players[i].myCommand) && (Initializer.players[i].myPlayerTransform.position - this.myPlayerTransform.position).sqrMagnitude < this.disablerRadius * this.disablerRadius)
			{
				Initializer.players[i].SendPlayerEffect(3, info.Duration);
			}
		}
	}

	// Token: 0x06002925 RID: 10533 RVA: 0x000D395C File Offset: 0x000D1B5C
	public void BlackMarkPlayer(Player_move_c player, GadgetInfo info)
	{
		player.SendPlayerEffect(4, info.Duration);
	}

	// Token: 0x06002926 RID: 10534 RVA: 0x000D396C File Offset: 0x000D1B6C
	public void UseDragonWhistle(GadgetInfo info)
	{
		this.SendPlayerEffect(5, 0f);
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		float num = 23f;
		float num2 = 2f;
		Vector3 center = this.myPlayerTransform.position + this.myPlayerTransform.rotation * new Vector3(0f, 0f, num * 0.6f);
		Vector3 halfExtents = new Vector3(num2, num2, num);
		Collider[] array = Physics.OverlapBox(center, halfExtents, this.myPlayerTransform.rotation);
		for (int i = 0; i < array.Length; i++)
		{
			GameObject gameObject = array[i].transform.root.gameObject;
			if (Initializer.IsEnemyTarget(gameObject.transform, null))
			{
				if (!gameObject.Equals(this.myPlayerTransform.gameObject) && !hashSet.Contains(gameObject))
				{
					this.DamageTarget(gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, Player_move_c.TypeKills.none);
					this.PoisonShotWithEffect(gameObject, new Player_move_c.PoisonParameters(Player_move_c.PoisonType.Burn, 0.02f, info.Damage, 1f, 6, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion));
					hashSet.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x06002927 RID: 10535 RVA: 0x000D3AA8 File Offset: 0x000D1CA8
	public void UsePandoraBox(GadgetInfo info, bool pandoraSuccess)
	{
		if (pandoraSuccess)
		{
			this.SendPlayerEffect(6, 0f);
			Initializer.TargetsList targetsList = new Initializer.TargetsList();
			foreach (Transform transform in targetsList)
			{
				this.DamageTarget(transform.gameObject, info.Damage, GadgetsInfo.BaseName(info.Id), WeaponSounds.TypeDead.explosion, Player_move_c.TypeKills.none);
			}
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.pandoraSuccess, 1f);
		}
		else
		{
			this.SendPlayerEffect(11, 0f);
			this.CurHealth = 1f;
			this.SendSynhHealth(false, null);
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.pandoraFail, 1f);
		}
	}

	// Token: 0x06002928 RID: 10536 RVA: 0x000D3B80 File Offset: 0x000D1D80
	public float DamageMultiplierByGadgets()
	{
		float num = 1f;
		if (this.drumActive)
		{
			num *= this.drumDamageMultiplier;
		}
		return num;
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06002929 RID: 10537 RVA: 0x000D3BA8 File Offset: 0x000D1DA8
	public static int MaxPlayerGUIHealth
	{
		get
		{
			return 9;
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x0600292A RID: 10538 RVA: 0x000D3BAC File Offset: 0x000D1DAC
	// (set) Token: 0x0600292B RID: 10539 RVA: 0x000D3BBC File Offset: 0x000D1DBC
	private float _curHealth
	{
		get
		{
			return this._curHealthSalt.value;
		}
		set
		{
			this._curHealthSalt.value = value;
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600292C RID: 10540 RVA: 0x000D3BCC File Offset: 0x000D1DCC
	// (set) Token: 0x0600292D RID: 10541 RVA: 0x000D3BD4 File Offset: 0x000D1DD4
	public float CurHealth
	{
		get
		{
			return this._curHealth;
		}
		set
		{
			float num = this._curHealth - value;
			this._curHealth -= num;
		}
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x0600292E RID: 10542 RVA: 0x000D3BF8 File Offset: 0x000D1DF8
	public float MaxHealth
	{
		get
		{
			return (!Defs.isMulti || !Defs.isHunger) ? ExperienceController.HealthByLevel[(!Defs.isMulti || !(this.myNetworkStartTable != null)) ? ExperienceController.sharedController.currentLevel : this.myNetworkStartTable.myRanks] : ExperienceController.HealthByLevel[1];
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x0600292F RID: 10543 RVA: 0x000D3C60 File Offset: 0x000D1E60
	// (set) Token: 0x06002930 RID: 10544 RVA: 0x000D3C84 File Offset: 0x000D1E84
	public float curArmor
	{
		get
		{
			return this.CurrentBaseArmor + this.CurrentBodyArmor + this.CurrentHatArmor;
		}
		set
		{
			float num = this.curArmor - value;
			if (num >= 0f)
			{
				if (this.CurrentHatArmor >= num)
				{
					this.CurrentHatArmor -= num;
				}
				else
				{
					num -= this.CurrentHatArmor;
					this.CurrentHatArmor = 0f;
					if (this.CurrentBodyArmor >= num)
					{
						this.CurrentBodyArmor -= num;
					}
					else
					{
						num -= this.CurrentBodyArmor;
						this.CurrentBodyArmor = 0f;
						this.CurrentBaseArmor -= num;
					}
				}
			}
			else if (num < 0f)
			{
				num *= -1f;
				bool flag = this.WearedMaxArmor > 0f;
				if (flag)
				{
					if (this.WearedMaxArmor > 5f)
					{
						num = Mathf.Min(this.WearedMaxArmor - this.WearedCurrentArmor, this.WearedMaxArmor * 0.5f);
					}
					else
					{
						num = this.WearedMaxArmor - this.WearedCurrentArmor;
					}
				}
				else
				{
					num = 1f;
				}
				this.AddArmor(num);
			}
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06002931 RID: 10545 RVA: 0x000D3D9C File Offset: 0x000D1F9C
	public float MaxArmor
	{
		get
		{
			return this.maxBaseArmor + this.WearedMaxArmor;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06002932 RID: 10546 RVA: 0x000D3DAC File Offset: 0x000D1FAC
	public float WearedMaxArmor
	{
		get
		{
			float num = Wear.MaxArmorForItem(Storager.getString(Defs.ArmorNewEquppedSN, false), this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
			float num2 = Wear.MaxArmorForItem(Storager.getString(Defs.HatEquppedSN, false), this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
			return num + num2;
		}
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x000D3E3C File Offset: 0x000D203C
	public bool AddHealth(float heal)
	{
		if (this.CurHealth >= this.MaxHealth)
		{
			return false;
		}
		this.CurHealth = Mathf.Min(this.MaxHealth, this.CurHealth + heal);
		this.SendSynhDeltaHealth(heal);
		return true;
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000D3E80 File Offset: 0x000D2080
	public bool MinusMechHealth(float _minus)
	{
		this.liveMech -= _minus;
		if (this.liveMech <= 0f)
		{
			this.DeactivateCurrentBody();
			return true;
		}
		return false;
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x000D3EAC File Offset: 0x000D20AC
	public void SuicidePenalty()
	{
		if (!Defs.isCOOP && WeaponManager.sharedManager.myNetworkStartTable.score > 0)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.suicide, 1f);
		}
		if (Defs.isDuel)
		{
			this.countKills = Mathf.Max(0, this.countKills - 1);
			this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
			this._weaponManager.myNetworkStartTable.SynhCountKills(null);
		}
		if (this.countKills >= 0)
		{
			GlobalGameController.CountKills = this.countKills;
		}
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x000D3F48 File Offset: 0x000D2148
	public void ImSuicide()
	{
		this.isSuicided = true;
		this.respawnedForGUI = true;
		if (!this.wasResurrected)
		{
			base.Invoke("SuicidePenalty", 1.5f);
		}
		if (Defs.isFlag && this.isCaptureFlag)
		{
			this.enemyFlag.GoBaza();
			this.isCaptureFlag = false;
			this.SendSystemMessegeFromFlagReturned(this.enemyFlag.isBlue);
		}
		if (this.countKills >= 0)
		{
			GlobalGameController.CountKills = this.countKills;
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
		if (!this.wasResurrected)
		{
			this.sendImDeath(this.mySkinName.NickName);
		}
		else
		{
			this._killerInfo.isSuicide = true;
		}
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x000D4024 File Offset: 0x000D2224
	private void UpdateHealth()
	{
		if (this.isMulti && this.isMine && this.CurHealth + this.curArmor - this.synhHealth > 0.1f)
		{
			this.SendSynhHealth(true, null);
		}
		if (!this.isMulti || this.isMine)
		{
			if (!this.isRegenerationLiveCape)
			{
				this.timerRegenerationLiveCape = this.maxTimerRegenerationLiveCape;
			}
			if (this.isRegenerationLiveCape)
			{
				if (this.timerRegenerationLiveCape > 0f)
				{
					this.timerRegenerationLiveCape -= Time.deltaTime;
				}
				else
				{
					this.timerRegenerationLiveCape = this.maxTimerRegenerationLiveCape;
					if (this.CurHealth < this.MaxHealth)
					{
						this.CurHealth += 1f;
					}
				}
			}
			if (!EffectsController.IsRegeneratingArmor)
			{
				this.timeSettedAfterRegenerationSwitchedOn = false;
			}
			if (EffectsController.IsRegeneratingArmor)
			{
				if (!this.timeSettedAfterRegenerationSwitchedOn)
				{
					this.timeSettedAfterRegenerationSwitchedOn = true;
					this.timerRegenerationArmor = this.maxTimerRegenerationArmor;
				}
				if (this.timerRegenerationArmor > 0f)
				{
					this.timerRegenerationArmor -= Time.deltaTime;
				}
				else
				{
					this.timerRegenerationArmor = this.maxTimerRegenerationArmor;
					if (this.curArmor < this.MaxArmor && Storager.getString(Defs.ArmorNewEquppedSN, false) != Defs.ArmorNewNoneEqupped)
					{
						this.AddArmor(1f);
					}
				}
			}
			if (!this.isRegenerationLiveZel)
			{
				this.timerRegenerationLiveZel = this.maxTimerRegenerationLiveZel;
			}
			if (this.isRegenerationLiveZel)
			{
				if (this.timerRegenerationLiveZel > 0f)
				{
					this.timerRegenerationLiveZel -= Time.deltaTime;
				}
				else
				{
					this.timerRegenerationLiveZel = this.maxTimerRegenerationLiveZel;
					if (this.CurHealth < this.MaxHealth)
					{
						this.CurHealth += 1f;
					}
				}
			}
			if (this.timerShowUp > 0f)
			{
				this.timerShowUp -= Time.deltaTime;
			}
			if (this.timerShowDown > 0f)
			{
				this.timerShowDown -= Time.deltaTime;
			}
			if (this.timerShowLeft > 0f)
			{
				this.timerShowLeft -= Time.deltaTime;
			}
			if (this.timerShowRight > 0f)
			{
				this.timerShowRight -= Time.deltaTime;
			}
		}
		if ((!this.isMulti || this.isMine) && this.CurHealth <= 0f && !this.isKilled)
		{
			bool flag = this.showRanks || this.showChat || ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy || (this._pauser != null && this._pauser.paused);
			this.GadgetOnPlayerDeath(false);
			this.DestroyEffects();
			if (!this.wasResurrected || this.deadInCollider)
			{
				this.countMultyFlag = 0;
				this.ResetMySpotEvent();
				this.ResetHouseKeeperEvent();
			}
			if (this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Stop();
			}
			if (!this.wasResurrected && Defs.isCOOP)
			{
				this.SendImKilled();
				this.SendSynhHealth(false, null);
			}
			this.inGameGUI.ResetDamageTaken();
			if (InGameGUI.sharedInGameGUI.isTurretInterfaceActive || Defs.isTurretWeapon)
			{
				this.CancelTurret();
				InGameGUI.sharedInGameGUI.HideTurretInterface();
				Defs.isTurretWeapon = false;
			}
			if (this.isGrenadePress)
			{
				this.ReturnWeaponAfterGrenade();
				this.isGrenadePress = false;
			}
			if (this.isZooming)
			{
				this.ZoomPress();
			}
			this.inGameGUI.StopAllCircularIndicators();
			if (this.isMulti)
			{
				if ((!this.isMulti || this.isMine) && this._player != null && this._player)
				{
					ImpactReceiverTrampoline component = this._player.GetComponent<ImpactReceiverTrampoline>();
					if (component != null)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
				if (Defs.isFlag && this.isCaptureFlag)
				{
					this.isCaptureFlag = false;
					this.photonView.RPC("SendSystemMessegeFromFlagDroppedRPC", PhotonTargets.All, new object[]
					{
						this.enemyFlag.isBlue,
						this.mySkinName.NickName
					});
					this.enemyFlag.SetNOCapture(this.flagPoint.transform.position, this.flagPoint.transform.rotation);
				}
				this.resetMultyKill();
				this.isKilled = true;
				if (!this.wasResurrected || this.deadInCollider)
				{
					if (Defs.isCOOP && !this.isSuicided)
					{
						this.killedInMatch = true;
					}
					if (Defs.isMulti && this.isMine && !Defs.isHunger && !this.isSuicided && UnityEngine.Random.Range(0, 100) < 50)
					{
						BonusController.sharedController.AddBonusAfterKillPlayer(new Vector3(this.myPlayerTransform.position.x, this.myPlayerTransform.position.y - 1f, this.myPlayerTransform.position.z));
					}
				}
				this.isSuicided = false;
				if ((!this.wasResurrected || this.deadInCollider) && this.isHunger && this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).weaponPrefab.name.Replace("(Clone)", string.Empty) != WeaponManager.KnifeWN)
				{
					BonusController.sharedController.AddWeaponAfterKillPlayer(this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).weaponPrefab.name, this.myPlayerTransform.position);
				}
				if (!this.wasResurrected && Defs.isSoundFX)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.deadPlayerSound);
				}
				if (!this.wasResurrected && this.isCOOP)
				{
					this._weaponManager.myNetworkStartTable.score -= 1000;
					if (this._weaponManager.myNetworkStartTable.score < 0)
					{
						this._weaponManager.myNetworkStartTable.score = 0;
					}
					GlobalGameController.Score = this._weaponManager.myNetworkStartTable.score;
					this._weaponManager.myNetworkStartTable.SynhScore();
				}
				this.isDeadFrame = true;
				bool NeedShowWindow = this.isNeedShowRespawnWindow && !flag;
				if (this.wasResurrected)
				{
					AutoFade.fadeKilled(0.5f, 1.5f, 0.1f, Color.white);
				}
				else
				{
					AutoFade.fadeKilled(0.5f, (!NeedShowWindow || Defs.inRespawnWindow) ? 1.5f : 0.5f, 0.5f, Color.white);
				}
				base.Invoke("setisDeadFrameFalse", 1f);
				base.StartCoroutine(this.FlashWhenDead());
				this.SetJoysticksActive(false);
				if (Defs.inRespawnWindow)
				{
					Defs.inRespawnWindow = false;
					this.RespawnPlayer();
				}
				else
				{
					Vector3 localPosition = this.myPlayerTransform.localPosition;
					this.inGameGUI.blockedCollider.SetActive(true);
					TweenParms p_parms = new TweenParms().Prop("localPosition", new Vector3(localPosition.x, localPosition.y + 100f, localPosition.z)).Ease(EaseType.EaseInCubic).OnComplete(delegate()
					{
						this.myPlayerTransform.localPosition = new Vector3(0f, -1000f, 0f);
						if (this.wasResurrected)
						{
							this.ResurrectPlayer();
						}
						else if (NeedShowWindow && !Defs.inRespawnWindow && this._killerInfo.killerTransform != null)
						{
							this.SetMapCameraActive(true);
							this.KillCam();
						}
						else
						{
							Defs.inRespawnWindow = false;
							this.RespawnPlayer();
						}
						this.inGameGUI.blockedCollider.SetActive(false);
					});
					HOTween.To(this.myPlayerTransform, (!NeedShowWindow) ? 2f : 0.75f, p_parms);
				}
			}
			else if (!this.wasResurrected)
			{
				if (Defs.IsSurvival)
				{
					if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0))
					{
						GlobalGameController.HasSurvivalRecord = true;
						PlayerPrefs.SetInt(Defs.SurvivalScoreSett, GlobalGameController.Score);
						PlayerPrefs.Save();
						FriendsController.sharedController.survivalScore = GlobalGameController.Score;
						FriendsController.sharedController.SendOurData(false);
					}
					if (ZombieCreator.sharedCreator != null)
					{
						if (Storager.getInt("SendFirstResaltArena", false) != 1)
						{
							Storager.setInt("SendFirstResaltArena", 1, false);
							AnalyticsStuff.LogArenaFirst(false, ZombieCreator.sharedCreator.currentWave > 0);
						}
						AnalyticsStuff.LogArenaWavesPassed(ZombieCreator.sharedCreator.currentWave);
					}
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						AGSLeaderboardsClient.SubmitScore("best_survival_scores", (long)GlobalGameController.Score, 0);
					}
					else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite && Social.localUser.authenticated)
					{
						Social.ReportScore((long)GlobalGameController.Score, "CgkIr8rGkPIJEAIQCg", delegate(bool success)
						{
							Debug.Log("Player_move_c.Update(): " + ((!success) ? "Failed to report score." : "Reported score successfully."));
						});
					}
				}
				else if (GlobalGameController.Score > PlayerPrefs.GetInt(Defs.BestScoreSett, 0))
				{
					PlayerPrefs.SetInt(Defs.BestScoreSett, GlobalGameController.Score);
					PlayerPrefs.Save();
				}
				LevelCompleteScript.LastGameResult = GameResult.Death;
				LevelCompleteLoader.action = null;
				LevelCompleteLoader.sceneName = "LevelComplete";
				Singleton<SceneLoader>.Instance.LoadScene("LevelToCompleteProm", LoadSceneMode.Single);
			}
			else
			{
				this.isKilled = true;
				this.isSuicided = false;
				this.isDeadFrame = true;
				AutoFade.fadeKilled(0.5f, 1.5f, 0.1f, Color.white);
				base.Invoke("setisDeadFrameFalse", 1f);
				base.StartCoroutine(this.FlashWhenDead());
				this.SetJoysticksActive(false);
				Vector3 localPosition2 = this.myPlayerTransform.localPosition;
				TweenParms p_parms2 = new TweenParms().Prop("localPosition", new Vector3(localPosition2.x, localPosition2.y + 100f, localPosition2.z)).Ease(EaseType.EaseInCubic).OnComplete(delegate()
				{
					this.ResurrectPlayer();
				});
				HOTween.To(this.myPlayerTransform, 2f, p_parms2);
			}
		}
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x000D4AA4 File Offset: 0x000D2CA4
	private void SetJoysticksActive(bool active)
	{
		if (JoystickController.leftJoystick != null)
		{
			if (!active)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
			}
			JoystickController.leftJoystick.SetJoystickActive(active);
		}
		if (JoystickController.leftTouchPad != null)
		{
			JoystickController.leftTouchPad.SetJoystickActive(active);
		}
		if (JoystickController.rightJoystick != null)
		{
			if (!active)
			{
				JoystickController.rightJoystick.gameObject.SetActive(false);
				JoystickController.rightJoystick.MakeInactive();
			}
			else
			{
				JoystickController.rightJoystick.MakeActive();
			}
		}
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x000D4B48 File Offset: 0x000D2D48
	public void KillSelf()
	{
		if ((this.isMulti && !this.isMine) || this.isKilled || this.CurHealth <= 0f)
		{
			return;
		}
		this.curArmor = 0f;
		this.CurHealth = 0f;
		this.GadgetOnPlayerDeath(true);
		if (Defs.isMulti)
		{
			this.ImSuicide();
			if (!Defs.isCOOP)
			{
				this.SendImKilled();
			}
		}
		else
		{
			this.StartFlash(this.mySkinName.gameObject);
		}
	}

	// Token: 0x0600293A RID: 10554 RVA: 0x000D4BDC File Offset: 0x000D2DDC
	private bool GetDamageForSync(float damage)
	{
		if (!Defs.isMulti || this.isMine)
		{
			return false;
		}
		this.synhHealth -= damage;
		if (this.synhHealth < 0f)
		{
			this.synhHealth = 0f;
		}
		if (this.armorSynch > damage)
		{
			this.armorSynch -= damage;
		}
		else
		{
			this.armorSynch = 0f;
		}
		return this.synhHealth <= 0f;
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x000D4C64 File Offset: 0x000D2E64
	private void GetDamageInternal(float damage, Player_move_c.TypeKills _typeKills, WeaponSounds.TypeDead typeDead, Vector3 posKiller, string weaponName, int idKiller)
	{
		this.MinusLiveRPCEffects(_typeKills);
		if ((!Defs.isMulti || this.isMine) && !this.isKilled && !this.isImmortality)
		{
			if (_typeKills != Player_move_c.TypeKills.mob && (idKiller == this.skinNamePixelView.viewID || idKiller == 0))
			{
				_typeKills = Player_move_c.TypeKills.himself;
			}
			float num = 0f;
			if (this.isMechActive)
			{
				this.MinusMechHealth(damage);
			}
			else
			{
				num = damage - this.curArmor;
				if (num < 0f)
				{
					this.curArmor -= damage;
					num = 0f;
				}
				else
				{
					this.curArmor = 0f;
					if (!Defs.isMulti)
					{
						CurrentCampaignGame.withoutHits = false;
					}
				}
			}
			if (this.CurHealth > 0f)
			{
				this.CurHealth -= num;
				if (Defs.isMulti && this.CurHealth <= 0f)
				{
					this.GadgetOnPlayerDeath(false);
					if (_typeKills != Player_move_c.TypeKills.mob)
					{
						if (_typeKills == Player_move_c.TypeKills.himself)
						{
							this.ImSuicide();
							if (!Defs.isCOOP)
							{
								this.SendImKilled();
							}
						}
						else if (!this.wasResurrected)
						{
							try
							{
								if (!WeaponManager.sharedManager.currentWeaponSounds.isGrenadeWeapon)
								{
									WeaponManager.sharedManager.myPlayerMoveC.AddWeWereKilledStatisctics((WeaponManager.sharedManager.currentWeaponSounds.name ?? string.Empty).Replace("(Clone)", string.Empty));
								}
							}
							catch (Exception arg)
							{
								Debug.LogError("Exception we were killed AddWeWereKilledStatisctics: " + arg);
							}
							if (this.placemarkerMoveC != null)
							{
								this.placemarkerMoveC.isPlacemarker = false;
							}
							if (Defs.isInet)
							{
								this.photonView.RPC("KilledPhoton", PhotonTargets.All, new object[]
								{
									idKiller,
									(int)_typeKills,
									weaponName,
									(int)typeDead
								});
							}
							else
							{
								base.GetComponent<NetworkView>().RPC("KilledPhoton", RPCMode.All, new object[]
								{
									idKiller,
									(int)_typeKills,
									weaponName,
									(int)typeDead
								});
							}
						}
						else
						{
							for (int i = 0; i < Initializer.players.Count; i++)
							{
								if (Initializer.players[i].mySkinName.pixelView != null && Initializer.players[i].mySkinName.pixelView.viewID == idKiller)
								{
									this.isResurrectionKill = true;
									this.resurrectionMoveC = Initializer.players[i];
									break;
								}
							}
						}
					}
				}
				this.SynhHealthRPC(this.CurHealth + this.curArmor, this.curArmor, false);
			}
			if (posKiller != Vector3.zero && _typeKills != Player_move_c.TypeKills.burning && _typeKills != Player_move_c.TypeKills.poison && _typeKills != Player_move_c.TypeKills.bleeding)
			{
				this.ShowDamageDirection(posKiller);
				if (this.myPetEngine != null)
				{
					this.myPetEngine.OwnerAttacked(_typeKills, idKiller);
				}
			}
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x000D4FAC File Offset: 0x000D31AC
	private void KillMechInDemon()
	{
		if (this.OnMyKillMechInDemon != null)
		{
			this.OnMyKillMechInDemon();
		}
	}

	// Token: 0x0600293D RID: 10557 RVA: 0x000D4FC4 File Offset: 0x000D31C4
	public void GetDamage(float damage, Player_move_c.TypeKills _typeKills, WeaponSounds.TypeDead _typeDead = WeaponSounds.TypeDead.angel, [Optional] Vector3 posKiller, string weaponName = "", int idKiller = 0)
	{
		if (Defs.isDaterRegim || this.isImmortality)
		{
			return;
		}
		if (Defs.isMulti && !this.isMine && !Defs.isCOOP && _typeKills == Player_move_c.TypeKills.burning && _typeKills == Player_move_c.TypeKills.poison)
		{
			ProfileController.OnGameHit();
		}
		damage *= this._protectionShieldValue;
		if (!ABTestController.useBuffSystem || !BuffSystem.instance.haveBuffForWeapon(weaponName))
		{
			damage *= WeaponManager.sharedManager.myPlayerMoveC.damageBuff;
		}
		else
		{
			damage *= BuffSystem.instance.weaponBuffValue;
		}
		damage /= this.protectionBuff;
		if (WeaponManager.sharedManager.myPlayerMoveC.IsPlayerEffectActive(Player_move_c.PlayerEffect.blackMark))
		{
			damage *= 0.5f;
		}
		if (Defs.isMulti && !this.isMine)
		{
			if (this.isMechActive)
			{
				PlayerEventScoreController.ScoreEvent @event = (!(this.currentMech != null)) ? PlayerEventScoreController.ScoreEvent.deadMech : this.currentMech.scoreEventOnKill;
				bool flag = this.IsGadgetEffectActive(Player_move_c.GadgetEffect.mech);
				if (this.MinusMechHealth(damage))
				{
					WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(@event, 1f);
					damage = 1000f;
					if (flag && WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon))
					{
						WeaponManager.sharedManager.myPlayerMoveC.KillMechInDemon();
					}
					if (_typeKills != Player_move_c.TypeKills.mech && _typeKills != Player_move_c.TypeKills.turret)
					{
						try
						{
							WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? string.Empty).Replace("(Clone)", string.Empty));
						}
						catch (Exception arg)
						{
							Debug.LogError("Exception we were killed AddWeKillStatisctics: " + arg);
						}
					}
				}
			}
			else if (this.synhHealth > 0f)
			{
				this.getLocalHurt = true;
				if (this.GetDamageForSync(damage))
				{
					if (_typeKills != Player_move_c.TypeKills.mech && _typeKills != Player_move_c.TypeKills.turret)
					{
						try
						{
							WeaponManager.sharedManager.myPlayerMoveC.AddWeKillStatisctics((weaponName ?? string.Empty).Replace("(Clone)", string.Empty));
						}
						catch (Exception arg2)
						{
							Debug.LogError("Exception we were killed AddWeKillStatisctics: " + arg2);
						}
					}
					damage = 10000f;
					if (this.isCaptureFlag)
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deadWithFlag, 1f);
						if (!NetworkStartTable.LocalOrPasswordRoom())
						{
							QuestMediator.NotifyKillOtherPlayerWithFlag();
						}
					}
					if (Defs.isCapturePoints && WeaponManager.sharedManager.myPlayerMoveC != null)
					{
						for (int i = 0; i < CapturePointController.sharedController.basePointControllers.Length; i++)
						{
							if (CapturePointController.sharedController.basePointControllers[i].captureConmmand == (BasePointController.TypeCapture)WeaponManager.sharedManager.myPlayerMoveC.myCommand && CapturePointController.sharedController.basePointControllers[i].capturePlayers.Contains(this))
							{
								this.isRaiderMyPoint = true;
								break;
							}
						}
					}
					if (this.getLocalHurt)
					{
						this.getLocalHurt = false;
					}
					this.ImKilled(this.myPlayerTransform.position, this.myPlayerTransform.rotation, (int)_typeDead);
					this.myPersonNetwork.StartAngel();
					if (Defs.isFlag && this.isCaptureFlag)
					{
						FlagController flagController = null;
						if (this.flag1.targetTrasform == this.flagPoint.transform)
						{
							flagController = this.flag1;
						}
						if (this.flag2.targetTrasform == this.flagPoint.transform)
						{
							flagController = this.flag2;
						}
						if (flagController != null)
						{
							flagController.SetNOCaptureRPC(this.myPlayerTransform.position, this.myPlayerTransform.rotation);
						}
					}
				}
			}
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				if (idKiller == 0)
				{
					if (posKiller == Vector3.zero)
					{
						this.photonView.RPC("GetDamageNoKillerRPC", PhotonTargets.Others, new object[]
						{
							damage,
							(int)_typeKills
						});
					}
					else
					{
						this.photonView.RPC("GetDamageNoKillerRPC", PhotonTargets.Others, new object[]
						{
							damage,
							(int)_typeKills,
							posKiller
						});
					}
				}
				else
				{
					this.photonView.RPC("GetDamageRPC", PhotonTargets.Others, new object[]
					{
						damage,
						(int)_typeKills,
						(int)_typeDead,
						posKiller,
						idKiller,
						weaponName
					});
				}
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("GetDamageRPC", RPCMode.Others, new object[]
				{
					damage,
					(int)_typeKills,
					(int)_typeDead,
					posKiller,
					idKiller,
					weaponName
				});
			}
		}
		this.GetDamageInternal(damage, _typeKills, _typeDead, posKiller, weaponName, idKiller);
	}

	// Token: 0x0600293E RID: 10558 RVA: 0x000D550C File Offset: 0x000D370C
	[PunRPC]
	public void GetDamageNoKillerRPC(float minus, int _typeKills)
	{
		if (Defs.isMulti && !this.isMine)
		{
			this.GetDamageForSync(minus);
		}
		this.GetDamageInternal(minus, (Player_move_c.TypeKills)_typeKills, WeaponSounds.TypeDead.angel, Vector3.zero, string.Empty, 0);
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x000D554C File Offset: 0x000D374C
	[PunRPC]
	public void GetDamageNoKillerRPC(float minus, int _typeKills, Vector3 enemyPos)
	{
		if (Defs.isMulti && !this.isMine)
		{
			this.GetDamageForSync(minus);
		}
		this.GetDamageInternal(minus, (Player_move_c.TypeKills)_typeKills, WeaponSounds.TypeDead.angel, enemyPos, string.Empty, 0);
	}

	// Token: 0x06002940 RID: 10560 RVA: 0x000D5588 File Offset: 0x000D3788
	[RPC]
	[PunRPC]
	public void GetDamageRPC(float minus, int _typeKills, int _typeWeapon, Vector3 enemyPos, int idKiller, string weaponName)
	{
		if (Defs.isMulti && !this.isMine)
		{
			this.GetDamageForSync(minus);
		}
		this.GetDamageInternal(minus, (Player_move_c.TypeKills)_typeKills, (WeaponSounds.TypeDead)_typeWeapon, enemyPos, weaponName, idKiller);
	}

	// Token: 0x06002941 RID: 10561 RVA: 0x000D55C4 File Offset: 0x000D37C4
	private void MinusLiveRPCEffects(Player_move_c.TypeKills _typeKills)
	{
		if (!Device.isPixelGunLow && Defs.isMulti && !this.isDaterRegim && !this.isMine)
		{
			if (_typeKills == Player_move_c.TypeKills.headshot)
			{
				HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
			else if (_typeKills == Player_move_c.TypeKills.poison)
			{
				HitParticle currentParticle2 = ParticleStacks.instance.poisonHitStack.GetCurrentParticle(false);
				if (currentParticle2 != null)
				{
					currentParticle2.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
			else if (_typeKills == Player_move_c.TypeKills.critical)
			{
				HitParticle currentParticle3 = ParticleStacks.instance.criticalHitStack.GetCurrentParticle(false);
				if (currentParticle3 != null)
				{
					currentParticle3.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
			else if (_typeKills == Player_move_c.TypeKills.bleeding)
			{
				HitParticle currentParticle4 = ParticleStacks.instance.bleedHitStack.GetCurrentParticle(false);
				if (currentParticle4 != null)
				{
					currentParticle4.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
			else
			{
				HitParticle currentParticle5 = ParticleStacks.instance.hitStack.GetCurrentParticle(false);
				if (currentParticle5 != null)
				{
					currentParticle5.StartShowParticle(this.myPlayerTransform.position, this.myPlayerTransform.rotation, false);
				}
			}
		}
		if (!Defs.isMulti || this.isMine)
		{
			if (_typeKills == Player_move_c.TypeKills.poison)
			{
				if (InGameGUI.sharedInGameGUI != null)
				{
					InGameGUI.sharedInGameGUI.poisonEffect.Play();
				}
			}
			else if (_typeKills == Player_move_c.TypeKills.bleeding && InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.bleedEffect.Play();
			}
		}
		if (Defs.isSoundFX)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot((this.curArmor <= 0f && !this.isMechActive) ? ((_typeKills != Player_move_c.TypeKills.headshot) ? this.damagePlayerSound : this.headShotSound) : this.damageArmorPlayerSound);
		}
		base.StartCoroutine(this.Flash(this.myPlayerTransform.gameObject, _typeKills == Player_move_c.TypeKills.poison));
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x000D582C File Offset: 0x000D3A2C
	public void SendSynhHealth(bool isUp, PhotonPlayer player = null)
	{
		if (this.isMulti && this.isMine)
		{
			if (Defs.isInet)
			{
				if (player == null)
				{
					this.photonView.RPC("SynhHealthRPC", PhotonTargets.All, new object[]
					{
						this.CurHealth + this.curArmor,
						this.curArmor,
						isUp
					});
				}
				else
				{
					this.photonView.RPC("SynhHealthRPC", player, new object[]
					{
						this.CurHealth + this.curArmor,
						this.curArmor,
						isUp
					});
				}
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SynhHealthRPC", RPCMode.All, new object[]
				{
					this.CurHealth + this.curArmor,
					this.curArmor,
					isUp
				});
			}
		}
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x000D5934 File Offset: 0x000D3B34
	public void SendSynhDeltaHealth(float deltaHealth)
	{
		if (this.isMulti && this.isMine)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SynhDeltaHealthRPC", PhotonTargets.All, new object[]
				{
					deltaHealth
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SynhDeltaHealthRPC", RPCMode.All, new object[]
				{
					deltaHealth
				});
			}
		}
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x000D59A8 File Offset: 0x000D3BA8
	[PunRPC]
	[RPC]
	private void SynhDeltaHealthRPC(float healthDelta)
	{
		this.SynhHealthRPC(Mathf.Min(this.synhHealth + healthDelta, this.MaxHealth), this.armorSynch, healthDelta > 0f);
	}

	// Token: 0x06002945 RID: 10565 RVA: 0x000D59DC File Offset: 0x000D3BDC
	[RPC]
	[PunRPC]
	private void SynhHealthRPC(float _synhHealth, float _synchArmor, bool isUp)
	{
		if (this.isMine)
		{
			this.synhHealth = _synhHealth;
		}
		else if (!isUp)
		{
			if (_synhHealth < this.synhHealth)
			{
				this.synhHealth = _synhHealth;
			}
			if (_synchArmor < this.armorSynch)
			{
				this.armorSynch = _synchArmor;
			}
		}
		else
		{
			this.synhHealth = _synhHealth;
			this.armorSynch = _synchArmor;
			this.isRaiderMyPoint = false;
		}
		if (this.synhHealth > 0f)
		{
			this.isStartAngel = false;
			this.myPersonNetwork.isStartAngel = false;
		}
	}

	// Token: 0x06002946 RID: 10566 RVA: 0x000D5A6C File Offset: 0x000D3C6C
	private void ShowDamageDirection(Vector3 posDamage)
	{
		if (this.isDaterRegim)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		Vector3 lhs = posDamage - this.myPlayerTransform.position;
		if (lhs == Vector3.zero)
		{
			return;
		}
		float num = Mathf.Atan(lhs.z / lhs.x);
		num = num * 180f / 3.1415927f;
		if (lhs.x > 0f)
		{
			num = 90f - num;
		}
		if (lhs.x < 0f)
		{
			num = 270f - num;
		}
		float y = this.myPlayerTransform.rotation.eulerAngles.y;
		float num2 = num - y;
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		if (num2 < -180f)
		{
			num2 += 360f;
		}
		if (this.inGameGUI != null)
		{
			this.inGameGUI.AddDamageTaken(num);
		}
		if (num2 > -45f && num2 <= 45f)
		{
			flag3 = true;
		}
		if (num2 < -45f && num2 >= -135f)
		{
			flag = true;
		}
		if (num2 > 45f && num2 <= 135f)
		{
			flag2 = true;
		}
		if (num2 < -135f || num2 >= 135f)
		{
			flag4 = true;
		}
		if (flag3)
		{
			this.timerShowUp = this.maxTimeSetTimerShow;
		}
		if (flag4)
		{
			this.timerShowDown = this.maxTimeSetTimerShow;
		}
		if (flag)
		{
			this.timerShowLeft = this.maxTimeSetTimerShow;
		}
		if (flag2)
		{
			this.timerShowRight = this.maxTimeSetTimerShow;
		}
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x000D5C30 File Offset: 0x000D3E30
	private void UpdateKillerInfo(Player_move_c killerPlayerMoveC, int killType)
	{
		SkinName skinName = killerPlayerMoveC.mySkinName;
		this._killerInfo.nickname = skinName.NickName;
		if (killerPlayerMoveC.myTable != null)
		{
			NetworkStartTable component = killerPlayerMoveC.myTable.GetComponent<NetworkStartTable>();
			int myRanks = component.myRanks;
			if (myRanks > 0 && myRanks < this.expController.marks.Length)
			{
				this._killerInfo.rankTex = ExperienceController.sharedController.marks[myRanks];
				this._killerInfo.rank = myRanks;
			}
			if (component.myClanTexture != null)
			{
				this._killerInfo.clanLogoTex = component.myClanTexture;
			}
			this._killerInfo.clanName = component.myClanName;
		}
		this._killerInfo.weapon = killerPlayerMoveC.currentWeaponForKillCam;
		this._killerInfo.skinTex = killerPlayerMoveC._skin;
		this._killerInfo.hat = skinName.currentHat;
		this._killerInfo.mask = skinName.currentMask;
		this._killerInfo.armor = skinName.currentArmor;
		this._killerInfo.cape = skinName.currentCape;
		this._killerInfo.capeTex = skinName.currentCapeTex;
		this._killerInfo.boots = skinName.currentBoots;
		this._killerInfo.pet = skinName.currentPet;
		this._killerInfo.gadgetSupport = skinName.currentGadgetSupport;
		this._killerInfo.gadgetTools = skinName.currentGadgetTools;
		this._killerInfo.gadgetTrowing = skinName.currentGadgetThrowing;
		this._killerInfo.turretUpgrade = killerPlayerMoveC.turretUpgrade;
		this._killerInfo.killerTransform = killerPlayerMoveC.myPlayerTransform;
		this._killerInfo.healthValue = Mathf.CeilToInt((killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch <= 0f) ? 0f : (killerPlayerMoveC.synhHealth - killerPlayerMoveC.armorSynch));
		this._killerInfo.armorValue = Mathf.CeilToInt(killerPlayerMoveC.armorSynch);
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x000D5E2C File Offset: 0x000D402C
	[PunRPC]
	[RPC]
	public void imDeath(string _name)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		this._weaponManager.myPlayerMoveC.AddSystemMessage(_name, 1, Color.white);
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x000D5E7C File Offset: 0x000D407C
	public void sendImDeath(string _name)
	{
		if (!this.isInet)
		{
			base.GetComponent<NetworkView>().RPC("imDeath", RPCMode.All, new object[]
			{
				_name
			});
		}
		else
		{
			this.photonView.RPC("imDeath", PhotonTargets.All, new object[]
			{
				_name
			});
		}
		this._killerInfo.isSuicide = true;
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x000D5EDC File Offset: 0x000D40DC
	[RPC]
	[PunRPC]
	public void KilledPhoton(int idKiller, int _typekill, string weaponName, int _typeWeapon)
	{
		if (this._weaponManager == null)
		{
			return;
		}
		if (this._weaponManager.myPlayer == null)
		{
			return;
		}
		string nick = string.Empty;
		string nickName = this.mySkinName.NickName;
		this.resurrectionMoveC = null;
		this.isResurrectionKill = false;
		if (weaponName.Contains("pet_"))
		{
			_typekill = 15;
		}
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (Initializer.players[i].mySkinName.pixelView != null && Initializer.players[i].mySkinName.pixelView.viewID == idKiller)
			{
				SkinName skinName = Initializer.players[i].mySkinName;
				Player_move_c player_move_c = Initializer.players[i];
				nick = skinName.NickName;
				if (this.isMine && Defs.isJetpackEnabled && !this.mySkinName.character.isGrounded)
				{
					player_move_c.AddScoreDuckHunt();
				}
				if (this._weaponManager != null && Initializer.players[i] == this._weaponManager.myPlayerMoveC)
				{
					ProfileController.OnGameTotalKills();
					if (ABTestController.useBuffSystem)
					{
						BuffSystem.instance.KillInteraction();
					}
					else
					{
						KillRateCheck.instance.IncrementKills();
					}
					WeaponManager.sharedManager.myNetworkStartTable.IncrementKills();
					if (this.isRaiderMyPoint)
					{
						WeaponManager.sharedManager.myPlayerMoveC.SendHouseKeeperEvent();
						this.isRaiderMyPoint = false;
					}
					if (Defs.isJetpackEnabled && !this._weaponManager.myPlayerMoveC.mySkinName.character.isGrounded && _typekill != 8)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.deathFromAbove, 1f);
					}
					if (player_move_c.isRocketJump && _typekill != 8)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.rocketJumpKill, 1f);
					}
					if (this.IsPlayerEffectActive(Player_move_c.PlayerEffect.blackMark))
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.blackMarked, 1f);
					}
					if (_typekill == 13)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.ricochet, 1f);
					}
					if (_typekill == 15)
					{
						this.AddPetCountSerials(player_move_c);
					}
					if (weaponName.Contains("Weapon"))
					{
						if (_typekill != 8 && _typekill != 10)
						{
							GameObject gameObject = Resources.Load("Weapons/" + weaponName) as GameObject;
							if (gameObject != null && gameObject.GetComponent<WeaponSounds>() != null)
							{
								this.AddCountSerials(gameObject.GetComponent<WeaponSounds>().categoryNabor - 1, player_move_c);
							}
						}
					}
					else if (GadgetsInfo.info.ContainsKey(weaponName))
					{
						GadgetInfo gadgetInfo = GadgetsInfo.info[weaponName];
						if (gadgetInfo.KillStreakType != PlayerEventScoreController.ScoreEvent.none)
						{
							player_move_c.myScoreController.AddScoreOnEvent(gadgetInfo.KillStreakType, 1f);
						}
						this.AddCountSerials(6, player_move_c);
					}
					if (this.multiKill > 1)
					{
						if (!NetworkStartTable.LocalOrPasswordRoom())
						{
							QuestMediator.NotifyBreakSeries();
						}
						if (this.multiKill == 2)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill2, 1f);
						}
						else if (this.multiKill == 3)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill3, 1f);
						}
						else if (this.multiKill == 4)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill4, 1f);
						}
						else if (this.multiKill == 5)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill5, 1f);
						}
						else if (this.multiKill < 10)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill6, 1f);
						}
						else if (this.multiKill < 20)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill10, 1f);
						}
						else if (this.multiKill < 50)
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill20, 1f);
						}
						else
						{
							player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killMultyKill50, 1f);
						}
						this.multiKill = 0;
					}
					if (!Defs.isFlag)
					{
						player_move_c.ImKill(idKiller, _typekill);
					}
					ShopNGUIController.CategoryNames weaponSlot = (ShopNGUIController.CategoryNames)(-1);
					ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
					if (byPrefabName != null)
					{
						int itemCategory = ItemDb.GetItemCategory(byPrefabName.Tag);
						weaponSlot = (ShopNGUIController.CategoryNames)itemCategory;
					}
					Player_move_c.TypeKills typeKills = (Player_move_c.TypeKills)_typekill;
					if (!NetworkStartTable.LocalOrPasswordRoom())
					{
						QuestMediator.NotifyKillOtherPlayer(ConnectSceneNGUIController.regim, weaponSlot, typeKills == Player_move_c.TypeKills.headshot, false, this.isPlacemarker, this.isInvisible, false);
						QuestMediator.NotifyKillOtherPlayerOnFly(this.IsPlayerFlying && Defs.isJump, player_move_c.IsPlayerFlying);
					}
					player_move_c.myScoreController.AddScoreOnEvent((_typekill != 9) ? ((_typekill != 2) ? ((_typekill != 3) ? ((_typekill != 15) ? PlayerEventScoreController.ScoreEvent.dead : PlayerEventScoreController.ScoreEvent.killPet) : PlayerEventScoreController.ScoreEvent.deadExplosion) : PlayerEventScoreController.ScoreEvent.deadHeadShot) : PlayerEventScoreController.ScoreEvent.deadTurret, 1f);
					if (this.isInvisible)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.invisibleKill, 1f);
					}
					if (this.isPlacemarker)
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.revenge, 1f);
					}
					if (this.Equals(this._weaponManager.myPlayerMoveC.placemarkerMoveC))
					{
						this._weaponManager.myPlayerMoveC.placemarkerMoveC = null;
						this.isPlacemarker = false;
					}
					if (this._weaponManager.myPlayerMoveC.isResurrectionKill && this.Equals(this._weaponManager.myPlayerMoveC.resurrectionMoveC))
					{
						player_move_c.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.hellraiser, 1f);
						this._weaponManager.myPlayerMoveC.resurrectionMoveC = null;
						this._weaponManager.myPlayerMoveC.isResurrectionKill = false;
					}
					if (this.getLocalHurt)
					{
						this.getLocalHurt = false;
					}
				}
				if (this.isMine)
				{
					player_move_c.isPlacemarker = true;
					this.placemarkerMoveC = player_move_c;
				}
				this.UpdateKillerInfo(Initializer.players[i], _typekill);
				break;
			}
		}
		this.RemoveEffect(Player_move_c.PlayerEffect.blackMark);
		this.ImKilled(this.myPlayerTransform.position, this.myPlayerTransform.rotation, _typeWeapon);
		if (this._weaponManager && this._weaponManager.myPlayerMoveC != null)
		{
			this._weaponManager.myPlayerMoveC.AddSystemMessage(nick, _typekill, nickName, Color.white, weaponName);
		}
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000D6570 File Offset: 0x000D4770
	public void PetKilled()
	{
		if (!Defs.isMulti || this.isMine)
		{
			this.counterPetSerial = 0;
			if (this.GadgetsOnPetKill != null)
			{
				this.GadgetsOnPetKill();
			}
		}
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000D65B0 File Offset: 0x000D47B0
	public void AddPetCountSerials(Player_move_c killerPlayerMoveC)
	{
		killerPlayerMoveC.counterPetSerial++;
		switch (killerPlayerMoveC.counterPetSerial)
		{
		case 2:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.tamer, 1f);
			break;
		case 3:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.packLeader, 1f);
			break;
		case 5:
			killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.kingOfBeasts, 1f);
			break;
		}
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000D6634 File Offset: 0x000D4834
	private void AddCountSerials(int categoryNabor, Player_move_c killerPlayerMoveC)
	{
		killerPlayerMoveC.counterSerials[categoryNabor]++;
		switch (killerPlayerMoveC.counterSerials[categoryNabor])
		{
		case 1:
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee, 1f);
			}
			break;
		case 2:
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee2, 1f);
			}
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.hunter, 1f);
			}
			break;
		case 3:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary1, 1f);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup1, 1f);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee3, 1f);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special1, 1f);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper1, 1f);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium1, 1f);
			}
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.gadgetMaster, 1f);
			}
			break;
		case 5:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary2, 1f);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup2, 1f);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee5, 1f);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special2, 1f);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper2, 1f);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium2, 1f);
			}
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.gadgetManiac, 1f);
			}
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.poacher, 1f);
			}
			break;
		case 7:
			if (categoryNabor == 0)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.primary3, 1f);
			}
			if (categoryNabor == 1)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.backup3, 1f);
			}
			if (categoryNabor == 2)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.melee7, 1f);
			}
			if (categoryNabor == 3)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.special3, 1f);
			}
			if (categoryNabor == 4)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.sniper3, 1f);
			}
			if (categoryNabor == 5)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.premium3, 1f);
			}
			break;
		case 9:
			if (categoryNabor == 6)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.mechanist, 1f);
			}
			break;
		case 10:
			if (categoryNabor == 7)
			{
				killerPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.animalsFear, 1f);
			}
			break;
		}
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000D693C File Offset: 0x000D4B3C
	public void SendImKilled()
	{
		if (this.wasResurrected)
		{
			return;
		}
		if (Defs.isInet)
		{
			this.photonView.RPC("ImKilled", PhotonTargets.All, new object[]
			{
				this.myPlayerTransform.position,
				this.myPlayerTransform.rotation,
				0
			});
			this.SendSynhHealth(false, null);
		}
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000D69B0 File Offset: 0x000D4BB0
	[PunRPC]
	[RPC]
	private void ImKilled(Vector3 pos, Quaternion rot)
	{
		this.ImKilled(pos, rot, 0);
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000D69BC File Offset: 0x000D4BBC
	[PunRPC]
	[RPC]
	private void ImKilled(Vector3 pos, Quaternion rot, int _typeDead = 0)
	{
		if (Device.isPixelGunLow)
		{
			_typeDead = 0;
		}
		if (!this.isStartAngel || Defs.isCOOP)
		{
			this.isStartAngel = true;
			if (Defs.inComingMessagesCounter < 15)
			{
				PlayerDeadController currentParticle = PlayerDeadStackController.sharedController.GetCurrentParticle(false);
				if (currentParticle != null)
				{
					currentParticle.StartShow(pos, rot, _typeDead, false, this._skin);
				}
				if (Defs.isSoundFX)
				{
					base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.deadPlayerSound);
				}
			}
		}
		if (!this.isMine && this.getLocalHurt)
		{
			WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.killAssist, 1f);
			this.getLocalHurt = false;
		}
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000D6A80 File Offset: 0x000D4C80
	private IEnumerator FlashWhenHit()
	{
		this.damageShown = true;
		Color rgba = this.damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		this.damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return base.StartCoroutine(this.Fade(0f, 1f, danageTime, this.damage));
		yield return new WaitForSeconds(0.01f);
		yield return base.StartCoroutine(this.Fade(1f, 0f, danageTime, this.damage));
		this.damageShown = false;
		yield break;
	}

	// Token: 0x06002952 RID: 10578 RVA: 0x000D6A9C File Offset: 0x000D4C9C
	private IEnumerator FlashWhenDead()
	{
		this.damageShown = true;
		Color rgba = this.damage.GetComponent<GUITexture>().color;
		rgba.a = 0f;
		this.damage.GetComponent<GUITexture>().color = rgba;
		float danageTime = 0.15f;
		yield return base.StartCoroutine(this.Fade(0f, 1f, danageTime, this.damage));
		while (this.isDeadFrame)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.Fade(1f, 0f, danageTime / 3f, this.damage));
		this.damageShown = false;
		yield break;
	}

	// Token: 0x06002953 RID: 10579 RVA: 0x000D6AB8 File Offset: 0x000D4CB8
	private void KillCam()
	{
		ProfileController.OnGameDeath();
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.DeathInteraction();
		}
		else
		{
			KillRateCheck.instance.IncrementDeath();
		}
		this.myNetworkStartTable.IncrementDeath();
		this.killedInMatch = true;
		base.StartCoroutine(InGameGUI.sharedInGameGUI.ShowRespawnWindow(this._killerInfo, 15f));
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06002954 RID: 10580 RVA: 0x000D6B1C File Offset: 0x000D4D1C
	private bool isNeedShowRespawnWindow
	{
		get
		{
			return !this.isHunger && !Defs.isRegimVidosDebug && !this._killerInfo.isSuicide && Defs.isMulti && !Defs.isCOOP && !this.wasResurrected;
		}
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x000D6B70 File Offset: 0x000D4D70
	private void SetMapCameraActive(bool active)
	{
		InGameGUI.sharedInGameGUI.SetInterfaceVisible(!active);
		Camera component = Initializer.Instance.tc.GetComponent<Camera>();
		Camera camera = this.myCamera;
		component.gameObject.SetActive(active);
		camera.gameObject.SetActive(!active);
		Camera currentCamera = (!active) ? camera : component;
		NickLabelController.currentCamera = currentCamera;
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x000D6BD4 File Offset: 0x000D4DD4
	[Obfuscation(Exclude = true)]
	private void SetNoKilled()
	{
		this.isKilled = false;
		this.resetMultyKill();
	}

	// Token: 0x06002957 RID: 10583 RVA: 0x000D6BE4 File Offset: 0x000D4DE4
	[Obfuscation(Exclude = true)]
	private void ChangePositionAfterRespawn()
	{
		this.myPlayerTransform.position += Vector3.forward * 0.01f;
	}

	// Token: 0x06002958 RID: 10584 RVA: 0x000D6C0C File Offset: 0x000D4E0C
	public void RespawnPlayer()
	{
		Defs.inRespawnWindow = false;
		this.respawnedForGUI = true;
		this.SetMapCameraActive(false);
		this._killerInfo.Reset();
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StopAllCircularIndicators();
		}
		if (this.myCurrentWeaponSounds && this.myCurrentWeaponSounds.animationObject != null && this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Reload"))
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Stop("Reload");
		}
		Func<bool> func = () => this._pauser != null && this._pauser.paused;
		if (base.transform.parent == null)
		{
			Debug.Log("transform.parent == null");
			return;
		}
		this.myPlayerTransform.localScale = new Vector3(1f, 1f, 1f);
		this.myTransform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		if (this.isHunger || Defs.isRegimVidosDebug)
		{
			this.myTable.GetComponent<NetworkStartTable>().ImDeadInHungerGames();
			PhotonNetwork.Destroy(this.myPlayerTransform.gameObject);
			return;
		}
		this.InitiailizeIcnreaseArmorEffectFlags();
		this.isDeadFrame = false;
		this.isImmortality = true;
		this.timerImmortality = this.maxTimerImmortality;
		this.SetNoKilled();
		if (this._weaponManager.myPlayer == null)
		{
			Debug.Log("_weaponManager.myPlayer == null");
			return;
		}
		this._weaponManager.myPlayerMoveC.mySkinName.camPlayer.transform.parent = this._weaponManager.myPlayer.transform;
		if (!func())
		{
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.gameObject.SetActive(true);
				JoystickController.rightJoystick._isFirstFrame = false;
			}
		}
		this.SetJoysticksActive(true);
		if (JoystickController.rightJoystick != null)
		{
			if (this.inGameGUI != null)
			{
				this.inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			Debug.Log("JoystickController.rightJoystick = null");
		}
		this.CurHealth = this.MaxHealth;
		Wear.RenewCurArmor(this.TierOrRoomTier((!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier));
		this.CurrentBaseArmor = EffectsController.ArmorBonus;
		this.zoneCreatePlayer = GameObject.FindGameObjectsWithTag((!this.isCOOP) ? ((!this.isCompany) ? ((!Defs.isFlag) ? ((!Defs.isCapturePoints) ? "MultyPlayerCreateZone" : ("MultyPlayerCreateZonePointZone" + this.myCommand)) : ("MultyPlayerCreateZoneFlagCommand" + this.myCommand)) : ("MultyPlayerCreateZoneCommand" + this.myCommand)) : "MultyPlayerCreateZoneCOOP");
		GameObject gameObject = this.zoneCreatePlayer[UnityEngine.Random.Range(0, this.zoneCreatePlayer.Length)];
		BoxCollider component = gameObject.GetComponent<BoxCollider>();
		Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
		Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
		Vector3 position = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
		Quaternion rotation = gameObject.transform.rotation;
		this.myPlayerTransform.position = position;
		this.myPlayerTransform.rotation = rotation;
		if (Storager.getInt("GrenadeID", false) <= 0)
		{
			Storager.setInt("GrenadeID", 1, false);
		}
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		Vector3 eulerAngles = this.myCamera.transform.rotation.eulerAngles;
		this.myCamera.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z);
		base.Invoke("ChangePositionAfterRespawn", 0.01f);
		foreach (object obj in this._weaponManager.allAvailablePlayerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			weapon.currentAmmoInClip = weapon.weaponPrefab.GetComponent<WeaponSounds>().ammoInClip;
			weapon.currentAmmoInBackpack = weapon.weaponPrefab.GetComponent<WeaponSounds>().InitialAmmoWithEffectsApplied;
		}
		if (WeaponManager.sharedManager != null)
		{
			for (int i = 0; i < WeaponManager.sharedManager.playerWeapons.Count; i++)
			{
				WeaponSounds component2 = (WeaponManager.sharedManager.playerWeapons[i] as Weapon).weaponPrefab.GetComponent<WeaponSounds>();
				if (component2 != null && (!component2.isMelee || component2.isShotMelee))
				{
					WeaponManager.sharedManager.ReloadWeaponFromSet(i);
				}
			}
		}
		EffectsController.SlowdownCoeff = 1f;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showGrenadeHint && ++this.respawnCountForTraining == 2)
		{
			try
			{
				if (InGameGadgetSet.CurrentSet.Count == 1)
				{
					HintController.instance.ShowHintByName("use_grenade", 5f);
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in showing grenade hint: {0}", new object[]
				{
					ex
				});
			}
			this.respawnCountForTraining = 0;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			HintController.instance.ShowHintByName("change_weapon", 5f);
		}
	}

	// Token: 0x06002959 RID: 10585 RVA: 0x000D7340 File Offset: 0x000D5540
	public void ResurrectPlayer()
	{
		this.ResurrectionEvent();
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.reviveEffect.Play();
		}
		if (Defs.isSoundFX)
		{
			this.myAudioSource.PlayOneShot(this.resurrectionSound);
		}
		this.SendPlayerEffect(8, 0f);
		Defs.inRespawnWindow = false;
		this.respawnedForGUI = true;
		Func<bool> func = () => this._pauser != null && this._pauser.paused;
		if (base.transform.parent == null)
		{
			Debug.Log("transform.parent == null");
			return;
		}
		this.myPlayerTransform.localScale = Vector3.one;
		if (this.deadInCollider)
		{
			this.myTransform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
		}
		this.InitiailizeIcnreaseArmorEffectFlags();
		this.isDeadFrame = false;
		this.isImmortality = true;
		this.timerImmortality = this.maxTimerImmortality;
		this.SetNoKilled();
		if (this.OnMyPlayerResurected != null)
		{
			this.OnMyPlayerResurected();
		}
		if (this._weaponManager.myPlayer == null)
		{
			Debug.Log("_weaponManager.myPlayer == null");
			return;
		}
		this._weaponManager.myPlayerMoveC.mySkinName.camPlayer.transform.parent = this._weaponManager.myPlayer.transform;
		if (!func())
		{
			if (JoystickController.leftJoystick != null)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(true);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.gameObject.SetActive(true);
				JoystickController.rightJoystick._isFirstFrame = false;
			}
		}
		this.SetJoysticksActive(true);
		if (JoystickController.rightJoystick != null)
		{
			if (this.inGameGUI != null)
			{
				this.inGameGUI.BlinkNoAmmo(0);
			}
			JoystickController.rightJoystick.HasAmmo();
		}
		else
		{
			Debug.Log("JoystickController.rightJoystick = null");
		}
		this.CurHealth = this.MaxHealth;
		if (!this.deadInCollider)
		{
			this.myPlayerTransform.position = this.resurrectionPosition;
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			if (Defs.isMulti)
			{
				this.zoneCreatePlayer = GameObject.FindGameObjectsWithTag((!this.isCOOP) ? ((!this.isCompany) ? ((!Defs.isFlag) ? ((!Defs.isCapturePoints) ? "MultyPlayerCreateZone" : ("MultyPlayerCreateZonePointZone" + this.myCommand)) : ("MultyPlayerCreateZoneFlagCommand" + this.myCommand)) : ("MultyPlayerCreateZoneCommand" + this.myCommand)) : "MultyPlayerCreateZoneCOOP");
				GameObject gameObject = this.zoneCreatePlayer[UnityEngine.Random.Range(0, this.zoneCreatePlayer.Length - 1)];
				BoxCollider component = gameObject.GetComponent<BoxCollider>();
				Vector2 vector = new Vector2(component.size.x * gameObject.transform.localScale.x, component.size.z * gameObject.transform.localScale.z);
				Rect rect = new Rect(gameObject.transform.position.x - vector.x / 2f, gameObject.transform.position.z - vector.y / 2f, vector.x, vector.y);
				zero = new Vector3(rect.x + UnityEngine.Random.Range(0f, rect.width), gameObject.transform.position.y, rect.y + UnityEngine.Random.Range(0f, rect.height));
				rotation = gameObject.transform.rotation;
				this.myPlayerTransform.position = zero;
				this.myPlayerTransform.rotation = rotation;
			}
			else
			{
				Initializer.Instance.SetPlayerInStartPoint(this.myPlayerTransform.gameObject);
			}
		}
		if (this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>() != null)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
		if (this.deadInCollider)
		{
			Vector3 eulerAngles = this.myCamera.transform.rotation.eulerAngles;
			this.myCamera.transform.rotation = Quaternion.Euler(0f, eulerAngles.y, eulerAngles.z);
			base.Invoke("ChangePositionAfterRespawn", 0.01f);
		}
		EffectsController.SlowdownCoeff = 1f;
		this.deadInCollider = false;
	}

	// Token: 0x0600295A RID: 10586 RVA: 0x000D7828 File Offset: 0x000D5A28
	public void HideChangeWeaponTrainingHint()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			this.showChangeWeaponHint = false;
			HintController.instance.HideHintByName("change_weapon");
		}
	}

	// Token: 0x0600295B RID: 10587 RVA: 0x000D786C File Offset: 0x000D5A6C
	[RPC]
	[PunRPC]
	private void CountKillsCommandSynch(int _blue, int _red)
	{
		GlobalGameController.countKillsBlue = _blue;
		GlobalGameController.countKillsRed = _red;
	}

	// Token: 0x0600295C RID: 10588 RVA: 0x000D787C File Offset: 0x000D5A7C
	[RPC]
	[PunRPC]
	private void plusCountKillsCommand(int _command)
	{
		Debug.Log("plusCountKillsCommand: " + _command);
		if (_command == 1)
		{
			if (this._weaponManager && this._weaponManager.myPlayer)
			{
				this._weaponManager.myPlayerMoveC.countKillsCommandBlue++;
			}
			else
			{
				GlobalGameController.countKillsBlue++;
			}
		}
		if (_command == 2)
		{
			if (this._weaponManager && this._weaponManager.myPlayer)
			{
				this._weaponManager.myPlayerMoveC.countKillsCommandRed++;
			}
			else
			{
				GlobalGameController.countKillsRed++;
			}
		}
	}

	// Token: 0x0600295D RID: 10589 RVA: 0x000D7948 File Offset: 0x000D5B48
	public void addMultyKill()
	{
		this.multiKill++;
		if (this.multiKill > 1)
		{
			if (this.multiKill > 1 && !NetworkStartTable.LocalOrPasswordRoom())
			{
				QuestMediator.NotifyMakeSeries();
			}
			int num = this.multiKill;
			switch (num)
			{
			case 2:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill2;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			case 3:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill3;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			case 4:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill4;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			case 5:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill5;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			case 6:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill6;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			default:
				if (num != 20)
				{
					if (num == 50)
					{
						PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill50;
						this.myScoreController.AddScoreOnEvent(@event, 1f);
					}
				}
				else
				{
					PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill20;
					this.myScoreController.AddScoreOnEvent(@event, 1f);
				}
				break;
			case 10:
			{
				PlayerEventScoreController.ScoreEvent @event = PlayerEventScoreController.ScoreEvent.multyKill10;
				this.myScoreController.AddScoreOnEvent(@event, 1f);
				break;
			}
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					this.photonView.RPC("ShowMultyKillRPC", PhotonTargets.Others, new object[]
					{
						this.multiKill
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("ShowMultyKillRPC", RPCMode.Others, new object[]
					{
						this.multiKill
					});
				}
			}
		}
	}

	// Token: 0x0600295E RID: 10590 RVA: 0x000D7B00 File Offset: 0x000D5D00
	[PunRPC]
	[RPC]
	public void ShowMultyKillRPC(int countMulty)
	{
		this.multiKill = countMulty;
		if (this.multiKill > 1)
		{
			PlayerEventScoreController.ScoreEvent scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
			int num = this.multiKill;
			switch (num)
			{
			case 2:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill2;
				break;
			case 3:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill3;
				break;
			case 4:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill4;
				break;
			case 5:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill5;
				break;
			case 6:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill6;
				break;
			default:
				if (num != 20)
				{
					if (num == 50)
					{
						scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill50;
					}
				}
				else
				{
					scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill20;
				}
				break;
			case 10:
				scoreEvent = PlayerEventScoreController.ScoreEvent.multyKill10;
				break;
			}
			string spriteName = PlayerEventScoreController.pictureNameOnEvent[scoreEvent.ToString()];
			this.killStreakParticles.GetStreak(spriteName);
		}
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x000D7BD0 File Offset: 0x000D5DD0
	public void resetMultyKill()
	{
		this.multiKill = 0;
		for (int i = 0; i < this.counterSerials.Length; i++)
		{
			this.counterSerials[i] = 0;
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x000D7C08 File Offset: 0x000D5E08
	public void ImKill(NetworkViewID idKiller, int _typeKill)
	{
		this.countKills++;
		GlobalGameController.CountKills = this.countKills;
		this.CheckRookieKillerAchievement();
		this.addMultyKill();
		if (this.isCompany)
		{
			if (this.myCommand == 1)
			{
				this.countKillsCommandBlue++;
				if (this.isInet)
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[]
					{
						1
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[]
					{
						1
					});
				}
			}
			if (this.myCommand == 2)
			{
				this.countKillsCommandRed++;
				if (this.isInet)
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[]
					{
						2
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[]
					{
						2
					});
				}
			}
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x000D7D40 File Offset: 0x000D5F40
	public void ImKill(int idKiller, int _typeKill)
	{
		if (_typeKill == 8)
		{
			QuestMediator.NotifyTurretKill();
		}
		this.countKills++;
		GlobalGameController.CountKills = this.countKills;
		this.CheckRookieKillerAchievement();
		if (_typeKill != 15)
		{
			this.addMultyKill();
		}
		if (this.isCompany)
		{
			if (this.myCommand == 1)
			{
				this.countKillsCommandBlue++;
				if (this.isInet)
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[]
					{
						1
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[]
					{
						1
					});
				}
			}
			if (this.myCommand == 2)
			{
				this.countKillsCommandRed++;
				if (this.isInet)
				{
					this.photonView.RPC("plusCountKillsCommand", PhotonTargets.Others, new object[]
					{
						2
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("plusCountKillsCommand", RPCMode.Others, new object[]
					{
						2
					});
				}
			}
		}
		this._weaponManager.myNetworkStartTable.CountKills = this.countKills;
		this._weaponManager.myNetworkStartTable.SynhCountKills(null);
		if (this.isHunger && Initializer.players.Count == 1)
		{
			if (Defs.isHunger)
			{
				int val = Storager.getInt(Defs.RatingHunger, false) + 1;
				Storager.setInt(Defs.RatingHunger, val, false);
			}
			this.photonView.RPC("pobedaPhoton", PhotonTargets.All, new object[]
			{
				idKiller,
				this.myCommand
			});
			int val2 = Storager.getInt("Rating", false) + 1;
			Storager.setInt("Rating", val2, false);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.TryIncrementWinCountTimestamp();
			}
			this._weaponManager.myNetworkStartTable.isIwin = true;
		}
	}

	// Token: 0x06002962 RID: 10594 RVA: 0x000D7F40 File Offset: 0x000D6140
	private void CheckRookieKillerAchievement()
	{
		int num = this.oldKilledPlayerCharactersCount + 1;
		if (num <= 15)
		{
			Storager.setInt("KilledPlayerCharactersCount", num, false);
		}
		this.oldKilledPlayerCharactersCount = num;
		if (Social.localUser.authenticated && !Storager.hasKey("RookieKillerAchievmentCompleted") && num >= 15)
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQBw", 1, delegate(bool success)
				{
					Debug.Log("Achievement Rookie Killer incremented: " + success);
				});
			}
			Storager.setInt("RookieKillerAchievmentCompleted", 1, false);
		}
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x000D7FE8 File Offset: 0x000D61E8
	public void AddScoreDuckHunt()
	{
		if (Defs.isInet)
		{
			this.photonView.RPC("AddScoreDuckHuntRPC", PhotonTargets.All, new object[0]);
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("AddScoreDuckHuntRPC", RPCMode.All, new object[0]);
		}
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x000D8034 File Offset: 0x000D6234
	[RPC]
	[PunRPC]
	public void AddScoreDuckHuntRPC()
	{
		if (this.isMine)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.duckHunt, 1f);
		}
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x000D8054 File Offset: 0x000D6254
	[PunRPC]
	[RPC]
	public void pobeda(NetworkViewID idKiller)
	{
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (idKiller.Equals(player_move_c.mySkinName.GetComponent<NetworkView>().viewID))
			{
				this.nickPobeditel = player_move_c.mySkinName.NickName;
			}
		}
		if (this._weaponManager && this._weaponManager.myTable)
		{
			this._weaponManager.myNetworkStartTable.win(this.nickPobeditel, 0, 0, 0);
		}
	}

	// Token: 0x06002966 RID: 10598 RVA: 0x000D8124 File Offset: 0x000D6324
	[RPC]
	[PunRPC]
	public void pobedaPhoton(int idKiller, int _command)
	{
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (idKiller == player_move_c.mySkinName.pixelView.viewID)
			{
				this.nickPobeditel = player_move_c.mySkinName.NickName;
			}
		}
		if (this._weaponManager != null && this._weaponManager.myTable != null)
		{
			this._weaponManager.myNetworkStartTable.win(this.nickPobeditel, _command, 0, 0);
		}
		else
		{
			Debug.Log("_weaponManager.myTable==null");
		}
	}

	// Token: 0x06002967 RID: 10599 RVA: 0x000D81F8 File Offset: 0x000D63F8
	public void SendMySpotEvent()
	{
		this.countMySpotEvent++;
		if (this.countMySpotEvent == 1)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.mySpotPoint, 1f);
		}
		if (this.countMySpotEvent == 2)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.unstoppablePoint, 1f);
		}
		if (this.countMySpotEvent >= 3)
		{
			this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.monopolyPoint, 1f);
		}
	}

	// Token: 0x06002968 RID: 10600 RVA: 0x000D8270 File Offset: 0x000D6470
	private void ResetMySpotEvent()
	{
		this.countMySpotEvent = 0;
	}

	// Token: 0x06002969 RID: 10601 RVA: 0x000D827C File Offset: 0x000D647C
	private void ProvideHealth(string inShopId)
	{
		this.CurHealth = this.MaxHealth;
		CurrentCampaignGame.withoutHits = true;
	}

	// Token: 0x0600296A RID: 10602 RVA: 0x000D8290 File Offset: 0x000D6490
	public Vector3 GetPointAutoAim(Vector3 _posTo)
	{
		if (this.timerUpdatePointAutoAi < 0f)
		{
			this.rayAutoAim = this.myCamera.ScreenPointToRay(new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(this.rayAutoAim, out raycastHit, 300f, Tools.AllWithoutDamageCollidersMaskAndWithoutRocket))
			{
				if (raycastHit.collider.gameObject.name.Equals("Rocket(Clone)"))
				{
					Debug.Log("Rocket(Clone)");
				}
				this.pointAutoAim = raycastHit.point;
			}
			else
			{
				this.pointAutoAim = Vector3.down * 10000f;
			}
			this.timerUpdatePointAutoAi = 0.2f;
		}
		if (this.pointAutoAim.y < -1000f)
		{
			return this.rayAutoAim.GetPoint(Vector3.Magnitude(this.myCamera.transform.position - _posTo) * 1.1f);
		}
		return this.pointAutoAim;
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x0600296B RID: 10603 RVA: 0x000D83A0 File Offset: 0x000D65A0
	// (set) Token: 0x0600296C RID: 10604 RVA: 0x000D83A8 File Offset: 0x000D65A8
	private bool isShooting
	{
		get
		{
			return this._isShootingVal;
		}
		set
		{
			if (this._isShootingVal == value)
			{
				return;
			}
			this._isShootingVal = value;
			if (((this.isMulti && this.isMine) || !this.isMulti) && Player_move_c.OnMyShootingStateSchanged != null)
			{
				Player_move_c.OnMyShootingStateSchanged(this._isShootingVal);
			}
		}
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x000D8404 File Offset: 0x000D6604
	private void ShootUpdate()
	{
		for (int i = 0; i < this.poisonTargets.Count; i++)
		{
			if (this.poisonTargets[i].target.Equals(null) || this.poisonTargets[i].target.IsDead() || this.poisonTargets[i].hitCount <= 0)
			{
				this.poisonTargets.RemoveAt(i);
				i--;
			}
			else if (this.poisonTargets[i].nextHitTime < Time.time)
			{
				this.poisonTargets[i].hitCount--;
				this.ApplyDamageToTarget(this.poisonTargets[i].target, this.poisonTargets[i].param.multiplayerDamage, this.poisonTargets[i].param.weaponName, (this.poisonTargets[i].param.poisonType != Player_move_c.PoisonType.Burn) ? this.poisonTargets[i].param.typeDead : WeaponSounds.TypeDead.explosion, (this.poisonTargets[i].param.poisonType != Player_move_c.PoisonType.Toxic) ? ((this.poisonTargets[i].param.poisonType != Player_move_c.PoisonType.Burn) ? ((this.poisonTargets[i].param.poisonType != Player_move_c.PoisonType.Bleeding) ? Player_move_c.TypeKills.none : Player_move_c.TypeKills.bleeding) : Player_move_c.TypeKills.burning) : Player_move_c.TypeKills.poison);
				this.poisonTargets[i].nextHitTime = Time.time + this.poisonTargets[i].param.poisonTime;
			}
		}
		bool isShooting = this.isShooting;
		this.isShooting = (JoystickController.rightJoystick.isShooting || JoystickController.rightJoystick.isShootingPressure || JoystickController.leftTouchPad.isShooting);
		bool flag = !this.isShooting && isShooting;
		bool flag2 = TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None || TrainingController.FireButtonEnabled;
		if (this.isShooting)
		{
			if (flag2 && (!this.isHunger || this.hungerGameController.isGo) && !this.myCurrentWeaponSounds.isGrenadeWeapon)
			{
				this.ShotPressed();
			}
		}
		else
		{
			if (flag)
			{
				this.ShotUnPressed(false);
			}
			this.ResetShootingBurst();
		}
		if (this.myCurrentWeaponSounds.isFrostSword)
		{
			this.FrostSwordUpdate(this.myCurrentWeaponSounds);
		}
	}

	// Token: 0x0600296E RID: 10606 RVA: 0x000D86B4 File Offset: 0x000D68B4
	public void ShotUnPressed(bool weaponChanged = false)
	{
		if (this._weaponManager.currentWeaponSounds.isLoopShoot && this.isShootingLoop)
		{
			this.StopLoopShot();
		}
		if (this._weaponManager.currentWeaponSounds.isCharging)
		{
			this.UnchargeGun(weaponChanged);
		}
	}

	// Token: 0x0600296F RID: 10607 RVA: 0x000D8704 File Offset: 0x000D6904
	private void UnchargeGun(bool weaponChanged)
	{
		this.fullyCharged = false;
		base.GetComponent<AudioSource>().Stop();
		this.inGameGUI.ChargeValue.gameObject.SetActive(false);
		if (this._weaponManager.currentWeaponSounds.invisWhenCharged)
		{
			this.SetInvisible(false, true);
		}
		if (this.chargeValue > 0f)
		{
			this.lastChargeValue = this.chargeValue;
			Weapon weapon = (Weapon)this._weaponManager.playerWeapons[this.lastChargeWeaponIndex];
			if (!weaponChanged)
			{
				this._Shot();
				if (!this._weaponManager.currentWeaponSounds.isShotMelee && !this._weaponManager.currentWeaponSounds.isMelee)
				{
					this._SetGunFlashActive(true);
					this.GunFlashLifetime = this._weaponManager.currentWeaponSounds.gameObject.GetComponent<FlashFire>().timeFireAction;
				}
			}
			else
			{
				if (this._weaponManager.currentWeaponSounds.isMelee)
				{
					weapon.currentAmmoInClip = this.ammoInClipBeforeCharge;
				}
				Animation animation = (!this.isMechActive) ? this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>() : this.mechGunAnimation;
				if (animation != null && animation.IsPlaying("Charge"))
				{
					animation.Stop();
					animation.Play("Idle");
				}
				base.GetComponent<AudioSource>().clip = null;
			}
			if (this.isMulti && this.isMine)
			{
				if (this.isInet)
				{
					this.photonView.RPC("ChargeGunAnimation", PhotonTargets.Others, new object[]
					{
						false
					});
				}
				else
				{
					this._networkView.RPC("ChargeGunAnimation", RPCMode.Others, new object[]
					{
						false
					});
				}
			}
			Debug.Log("Charge release: " + this.chargeValue);
			this.chargeValue = 0f;
		}
	}

	// Token: 0x06002970 RID: 10608 RVA: 0x000D8904 File Offset: 0x000D6B04
	public void StartLoopShot()
	{
		if (this.isMulti && this.isMine)
		{
			if (this.isInet)
			{
				this.photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, new object[]
				{
					true
				});
			}
			else
			{
				this._networkView.RPC("StartShootLoopRPC", RPCMode.Others, new object[]
				{
					true
				});
			}
		}
		this.isShootingLoop = true;
		Animation component = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		float lengthAnimShootDown = 0f;
		if (component.GetClip("Shoot_start") != null)
		{
			if (component.IsPlaying("Shoot_end"))
			{
				lengthAnimShootDown = component["Shoot_end"].length - component["Shoot_end"].time;
			}
			else
			{
				component.Stop();
				lengthAnimShootDown = component["Shoot_start"].length;
				component.Play("Shoot_start");
			}
		}
		else
		{
			component.Stop();
		}
		this.ctsShootLoop.Cancel();
		this.ctsShootLoop = new CancellationTokenSource();
		base.StartCoroutine(this.ShootLoop(this.ctsShootLoop.Token, lengthAnimShootDown));
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x000D8A40 File Offset: 0x000D6C40
	private void StopLoopShot()
	{
		if (this.isMulti && this.isMine)
		{
			if (this.isInet)
			{
				this.photonView.RPC("StartShootLoopRPC", PhotonTargets.Others, new object[]
				{
					false
				});
			}
			else
			{
				this._networkView.RPC("StartShootLoopRPC", RPCMode.Others, new object[]
				{
					false
				});
			}
		}
		this.isShootingLoop = false;
		this.ctsShootLoop.Cancel();
		Animation component = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		if (component.IsPlaying("Shoot"))
		{
			component.Stop();
		}
		else if (component.IsPlaying("Shoot_start"))
		{
			float num = component["Shoot_start"].length - component["Shoot_start"].time;
			component.Stop();
			component["Shoot_end"].time = component["Shoot_start"].length - component["Shoot_start"].time;
		}
		if (component["Shoot_end"] != null)
		{
			component.Play("Shoot_end");
		}
		if (Defs.isSoundFX)
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().clip = this.myCurrentWeaponSounds.idle;
			this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
		}
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x000D8BBC File Offset: 0x000D6DBC
	private IEnumerator ShootLoop(CancellationToken token, float _lengthAnimShootDown)
	{
		yield return new WaitForSeconds(_lengthAnimShootDown);
		Animation currentWeaponAnimation = this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>();
		float lengthAnimShoot = currentWeaponAnimation["Shoot"].length;
		if (!token.IsCancellationRequested)
		{
			currentWeaponAnimation.Play("Shoot");
			if (Defs.isSoundFX)
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().clip = this.myCurrentWeaponSounds.shoot;
				this.myCurrentWeaponSounds.animationObject.GetComponent<AudioSource>().Play();
			}
		}
		if (Defs.isMulti && !this.isMine)
		{
			yield break;
		}
		while (!token.IsCancellationRequested)
		{
			this.shootS();
			yield return new WaitForSeconds(lengthAnimShoot);
		}
		yield break;
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x000D8BF4 File Offset: 0x000D6DF4
	[RPC]
	[PunRPC]
	private void StartShootLoopRPC(bool isStart)
	{
		if (isStart && !this.isShootingLoop)
		{
			this.StartLoopShot();
		}
		if (!isStart && this.isShootingLoop)
		{
			this.StopLoopShot();
		}
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000D8C30 File Offset: 0x000D6E30
	public void ResetShootingBurst()
	{
		this._countShootInBurst = 0;
		this._timerDelayInShootingBurst = -1f;
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x000D8C44 File Offset: 0x000D6E44
	public void ShotPressed()
	{
		if (this.deltaAngle > 10f)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToShoot)
		{
			TrainingController.isNextStep = TrainingState.TapToShoot;
		}
		if ((this.isMulti && this.isInet && this.photonView && !this.photonView.isMine) || this._weaponManager == null || this._weaponManager.currentWeaponSounds == null || this._weaponManager.currentWeaponSounds.animationObject == null)
		{
			return;
		}
		if (this._weaponManager.currentWeaponSounds.name.Contains("WeaponGrenade"))
		{
			return;
		}
		if (Defs.isTurretWeapon)
		{
			return;
		}
		if (!this.isMechActive && this._weaponManager.currentWeaponSounds.isLoopShoot)
		{
			if (!this.isShootingLoop)
			{
				this.StartLoopShot();
			}
			return;
		}
		Animation animation = (!this.isMechActive) ? this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>() : this.mechGunAnimation;
		Weapon weaponByIndex = this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex);
		if (animation.IsPlaying("Shoot1") || animation.IsPlaying("Shoot2") || animation.IsPlaying("Shoot"))
		{
			return;
		}
		if (animation.IsPlaying("Reload") || this.isReloading)
		{
			return;
		}
		if (animation.IsPlaying("Empty"))
		{
			return;
		}
		if (this._timerDelayInShootingBurst > 0f)
		{
			return;
		}
		this.lastShotTime = Time.time;
		if (!this.isMechActive && this._weaponManager.currentWeaponSounds.isCharging)
		{
			if ((weaponByIndex.currentAmmoInClip > 0 || this._weaponManager.currentWeaponSounds.isMelee) && this.chargeValue < 1f)
			{
				if (this.chargeValue == 0f)
				{
					this.ammoInClipBeforeCharge = weaponByIndex.currentAmmoInClip;
					this.lastChargeWeaponIndex = this._weaponManager.CurrentWeaponIndex;
					this.firstChargePlay = false;
				}
				if (this.nextChargeConsumeTime < Time.time)
				{
					this.nextChargeConsumeTime = Time.time + this._weaponManager.currentWeaponSounds.chargeTime / (float)this._weaponManager.currentWeaponSounds.chargeMax;
					this.chargeValue = Math.Min(1f, this.chargeValue + 1f / (float)this._weaponManager.currentWeaponSounds.chargeMax);
					animation["Charge"].speed = 1f;
					if (!this._weaponManager.currentWeaponSounds.isMelee)
					{
						weaponByIndex.currentAmmoInClip--;
					}
					if (this.inGameGUI != null)
					{
						this.inGameGUI.ChargeValue.gameObject.SetActive(true);
						this.inGameGUI.ChargeValue.fillAmount = this.chargeValue;
						this.inGameGUI.ChargeValue.color = new Color(1f, 1f - this.chargeValue, 0f);
					}
					if (!this.fullyCharged && this.chargeValue == 1f)
					{
						this.fullyCharged = true;
						if (this._weaponManager.currentWeaponSounds.invisWhenCharged)
						{
							this.SetInvisible(true, true);
						}
					}
				}
			}
			else
			{
				if (!this._weaponManager.currentWeaponSounds.chargeLoop)
				{
					animation["Charge"].speed = 0f;
				}
				if (this.chargeValue == 0f)
				{
					this.ShowNoAmmo();
				}
			}
			if (this.chargeValue > 0f)
			{
				if (!animation.IsPlaying("Charge") && animation.GetClip("Charge") != null)
				{
					animation.Stop();
					animation.Play("Charge");
					if (!this.firstChargePlay)
					{
						this.firstChargePlay = true;
						if (this.isMulti && this.isMine)
						{
							if (this.isInet)
							{
								this.photonView.RPC("ChargeGunAnimation", PhotonTargets.Others, new object[]
								{
									true
								});
							}
							else
							{
								this._networkView.RPC("ChargeGunAnimation", RPCMode.Others, new object[]
								{
									true
								});
							}
						}
					}
				}
				if (Defs.isSoundFX && this._weaponManager.currentWeaponSounds.charge != null && (!base.GetComponent<AudioSource>().isPlaying || base.GetComponent<AudioSource>().clip != this._weaponManager.currentWeaponSounds.charge))
				{
					base.GetComponent<AudioSource>().clip = this._weaponManager.currentWeaponSounds.charge;
					base.GetComponent<AudioSource>().Play();
				}
			}
			return;
		}
		animation.Stop();
		if (this._weaponManager.currentWeaponSounds.isBurstShooting)
		{
			this._countShootInBurst++;
			if (this._countShootInBurst >= this._weaponManager.currentWeaponSounds.countShootInBurst)
			{
				this._timerDelayInShootingBurst = this._weaponManager.currentWeaponSounds.delayInBurstShooting;
				this._countShootInBurst = 0;
			}
		}
		if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee && !this.isMechActive)
		{
			this._Shot();
			return;
		}
		if (weaponByIndex.currentAmmoInClip > 0 || this.isMechActive)
		{
			if (!this.isMechActive)
			{
				weaponByIndex.currentAmmoInClip--;
				if (weaponByIndex.currentAmmoInClip == 0)
				{
					if (weaponByIndex.currentAmmoInBackpack > 0)
					{
						if (this._weaponManager.currentWeaponSounds.isShotMelee)
						{
							this.Reload();
						}
					}
					else
					{
						TouchPadController rightJoystick = JoystickController.rightJoystick;
						if (rightJoystick)
						{
							rightJoystick.NoAmmo();
						}
						if (this.inGameGUI != null)
						{
							this.inGameGUI.BlinkNoAmmo(3);
							this.inGameGUI.PlayLowResourceBeep(3);
						}
					}
				}
			}
			this._Shot();
			if (!this._weaponManager.currentWeaponSounds.isShotMelee)
			{
				this._SetGunFlashActive(true);
				if (this.isMechActive)
				{
					this.GunFlashLifetime = 0.15f;
				}
				else
				{
					this.GunFlashLifetime = this._weaponManager.currentWeaponSounds.gameObject.GetComponent<FlashFire>().timeFireAction;
				}
			}
		}
		else
		{
			this.ShowNoAmmo();
		}
	}

	// Token: 0x06002976 RID: 10614 RVA: 0x000D931C File Offset: 0x000D751C
	private void ShowNoAmmo()
	{
		Weapon weaponByIndex = this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex);
		if (this.inGameGUI != null)
		{
			this.inGameGUI.BlinkNoAmmo(1);
			if (weaponByIndex.currentAmmoInBackpack == 0)
			{
				this.inGameGUI.PlayLowResourceBeepIfNotPlaying(1);
			}
		}
		if (this._weaponManager.currentWeaponSounds.isMelee)
		{
			return;
		}
		if (!this.isMechActive && weaponByIndex.currentAmmoInBackpack <= 0 && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && this.showChangeWeaponHint)
		{
			HintController.instance.ShowHintByName("change_weapon", 2f);
		}
		this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Empty");
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.empty);
		}
	}

	// Token: 0x06002977 RID: 10615 RVA: 0x000D9418 File Offset: 0x000D7618
	private void _Shot()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowFire = 1000f;
			HintController.instance.HideHintByName("press_fire");
		}
		if (this.isGrenadePress || this.showChat)
		{
			return;
		}
		if (Defs.isMulti && !Defs.isCOOP)
		{
			ProfileController.OnGameShoot();
		}
		float length;
		if (this.isMechActive)
		{
			if (!this.mechWeaponSounds.isDoubleShot)
			{
				this.mechGunAnimation.Play("Shoot");
				length = this.mechGunAnimation["Shoot"].length;
			}
			else
			{
				int numShootInDouble = this.GetNumShootInDouble();
				this.mechGunAnimation.Play("Shoot" + numShootInDouble);
				length = this.mechGunAnimation["Shoot" + numShootInDouble].length;
			}
			if (Defs.isSoundFX && this.currentMech != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.currentMech.shootSound);
			}
		}
		else
		{
			if (!this._weaponManager.currentWeaponSounds.isDoubleShot)
			{
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
				length = this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot"].length;
			}
			else
			{
				int numShootInDouble2 = this.GetNumShootInDouble();
				this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot" + numShootInDouble2);
				length = this._weaponManager.currentWeaponSounds.animationObject.GetComponent<Animation>()["Shoot" + numShootInDouble2].length;
			}
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.shoot);
			}
		}
		if (this.inGameGUI != null)
		{
			this.inGameGUI.StartFireCircularIndicators(length);
		}
		this.shootS();
	}

	// Token: 0x06002978 RID: 10616 RVA: 0x000D9648 File Offset: 0x000D7848
	public Player_move_c.RayHitsInfo GetHitsFromRay(Ray ray, bool getAll = true)
	{
		Player_move_c.RayHitsInfo result = default(Player_move_c.RayHitsInfo);
		result.obstacleFound = false;
		result.lenRay = 150f;
		RaycastHit[] array = Physics.RaycastAll(ray, 150f, Player_move_c._ShootRaycastLayerMask);
		if (array == null || array.Length == 0)
		{
			array = new RaycastHit[0];
		}
		if (!getAll)
		{
			Array.Sort<RaycastHit>(array, delegate(RaycastHit hit1, RaycastHit hit2)
			{
				float num = (hit1.point - this.GunFlash.position).sqrMagnitude - (hit2.point - this.GunFlash.position).sqrMagnitude;
				return (num <= 0f) ? ((num != 0f) ? -1 : 0) : 1;
			});
			RaycastHit raycastHit = default(RaycastHit);
			List<RaycastHit> list = new List<RaycastHit>();
			foreach (RaycastHit item in array)
			{
				bool flag = false;
				if (this.isHunger && item.collider.gameObject != null && item.collider.gameObject.CompareTag("Chest"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.parent != null && item.collider.gameObject.transform.parent.CompareTag("Enemy"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.parent != null && item.collider.gameObject.transform.parent.CompareTag("Player"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject.transform.root != null && item.collider.gameObject.transform.root.CompareTag("Pet"))
				{
					list.Add(item);
				}
				else if (item.collider.gameObject != null && item.collider.gameObject.CompareTag("Turret"))
				{
					list.Add(item);
					if (item.collider.gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
					{
						flag = true;
					}
				}
				else if (item.collider.gameObject != null && item.collider.gameObject.CompareTag("DamagedExplosion"))
				{
					list.Add(item);
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					result.obstacleFound = true;
					Vector3 point = item.point;
					Vector3 a = point - ray.origin;
					result.lenRay = Vector3.Magnitude(a);
					result.rayReflect = new Ray(point, Vector3.Reflect(ray.direction, item.normal));
					break;
				}
			}
			result.hits = list.ToArray();
		}
		else
		{
			result.hits = array;
		}
		return result;
	}

	// Token: 0x06002979 RID: 10617 RVA: 0x000D9958 File Offset: 0x000D7B58
	private IEnumerator ShowRayWithDelay(Vector3 _origin, Vector3 _direction, string _railName, float _len, float _delay)
	{
		yield return new WaitForSeconds(_delay);
		WeaponManager.AddRay(_origin, _direction, _railName, _len);
		yield break;
	}

	// Token: 0x0600297A RID: 10618 RVA: 0x000D99B8 File Offset: 0x000D7BB8
	private void FrostSwordUpdate(WeaponSounds weapon)
	{
		if (this.nextFrostHitTime < Time.time)
		{
			this.nextFrostHitTime = Time.time + weapon.slowdownTime;
			float num = weapon.frostRadius * weapon.frostRadius;
			Initializer.TargetsList targetsList = new Initializer.TargetsList();
			foreach (Transform transform in targetsList)
			{
				if ((transform.position - this._player.transform.position).sqrMagnitude < num)
				{
					this.DamageTargetWithWeapon(weapon, transform.gameObject, false, 0f, weapon.frostDamageMultiplier);
				}
			}
		}
	}

	// Token: 0x0600297B RID: 10619 RVA: 0x000D9A88 File Offset: 0x000D7C88
	private void SnowStormShot(WeaponSounds weapon)
	{
		this._FireFlash(true, 0);
		GameObject gameObject = null;
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, weapon.range, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null)
		{
			if (raycastHit.transform.parent && (raycastHit.transform.parent.CompareTag("Enemy") || raycastHit.transform.parent.CompareTag("Player")))
			{
				gameObject = raycastHit.transform.parent.gameObject;
			}
			else
			{
				gameObject = raycastHit.transform.gameObject;
			}
			this.DamageTargetWithWeapon(weapon, gameObject, false, raycastHit.distance * raycastHit.distance, 1f);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if (!(gameObject == transform))
			{
				Vector3 to = transform.position - this._player.transform.position;
				float sqrMagnitude = to.sqrMagnitude;
				if (sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					this.DamageTargetWithWeapon(weapon, transform.gameObject, false, sqrMagnitude, 1f);
				}
			}
		}
	}

	// Token: 0x0600297C RID: 10620 RVA: 0x000D9C84 File Offset: 0x000D7E84
	private void FlamethrowerShot(WeaponSounds weapon)
	{
		this._FireFlash(true, 0);
		GameObject gameObject = null;
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, weapon.range, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null)
		{
			if (raycastHit.transform.parent && (raycastHit.transform.parent.CompareTag("Enemy") || raycastHit.transform.parent.CompareTag("Player")))
			{
				gameObject = raycastHit.transform.parent.gameObject;
			}
			else
			{
				gameObject = raycastHit.transform.gameObject;
			}
			this.DamageTargetWithWeapon(weapon, gameObject, false, 0f, 1f);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if (!(gameObject == transform))
			{
				Vector3 to = transform.position - this._player.transform.position;
				if (to.sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					this.DamageTargetWithWeapon(weapon, transform.gameObject, false, 0f, 1f);
				}
			}
		}
	}

	// Token: 0x0600297D RID: 10621 RVA: 0x000D9E74 File Offset: 0x000D8074
	private void ShockerShot(WeaponSounds weapon)
	{
		this._FireFlash(true, 0);
		GameObject gameObject = null;
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, weapon.range, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null)
		{
			if (raycastHit.transform.parent && (raycastHit.transform.parent.CompareTag("Enemy") || raycastHit.transform.parent.CompareTag("Player") || raycastHit.transform.parent.CompareTag("Pet")))
			{
				gameObject = raycastHit.transform.root.gameObject;
			}
			else
			{
				gameObject = raycastHit.transform.gameObject;
			}
			this.DamageTargetWithWeapon(weapon, gameObject, false, 0f, 1f);
		}
		if (gameObject != null && gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
		{
			return;
		}
		float num = weapon.range * weapon.range;
		float num2 = weapon.shockerRange * weapon.shockerRange;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if (!(gameObject == transform))
			{
				Vector3 to = transform.position - this._player.transform.position;
				if (to.sqrMagnitude < num && Vector3.Angle(base.gameObject.transform.forward, to) < weapon.meleeAngle)
				{
					this.DamageTargetWithWeapon(weapon, transform.gameObject, false, 0f, 1f);
				}
				if (to.sqrMagnitude < num2)
				{
					this.DamageTargetWithWeapon(weapon, transform.gameObject, false, 0f, weapon.shockerDamageMultiplier);
				}
			}
		}
	}

	// Token: 0x0600297E RID: 10622 RVA: 0x000DA0B8 File Offset: 0x000D82B8
	private IEnumerator MeleeShot(WeaponSounds weapon)
	{
		this._FireFlash(false, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		yield return new WaitForSeconds(this.TimeOfMeleeAttack(weapon));
		if (weapon == null)
		{
			yield break;
		}
		GameObject raycastedObj = null;
		GameObject closestTargetObj = null;
		float closestTarget = float.MaxValue;
		bool isHeadshot = false;
		RaycastHit _hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, weapon.range, Player_move_c._ShootRaycastLayerMask) && _hit.collider.gameObject != null)
		{
			if (_hit.transform.parent)
			{
				raycastedObj = _hit.transform.gameObject;
				if (_hit.transform.parent.CompareTag("Enemy"))
				{
					raycastedObj = _hit.transform.parent.gameObject;
					isHeadshot = (_hit.collider.GetType() == typeof(SphereCollider));
				}
				if (_hit.transform.parent.CompareTag("Player") || _hit.transform.parent.CompareTag("Dummy"))
				{
					raycastedObj = _hit.transform.parent.gameObject;
					isHeadshot = (_hit.transform.name == "HeadCollider");
				}
			}
			else
			{
				raycastedObj = _hit.transform.gameObject;
				isHeadshot = (_hit.transform.name == "HeadCollider");
			}
			closestTargetObj = raycastedObj;
			closestTarget = 0f;
		}
		float weaponRangeSqr = weapon.range * weapon.range;
		Initializer.TargetsList targets = new Initializer.TargetsList();
		foreach (Transform target in targets)
		{
			if (!(raycastedObj == target))
			{
				Vector3 enemyDelta = target.position - this._player.transform.position;
				float targetDistance = enemyDelta.sqrMagnitude;
				if (targetDistance < closestTarget && targetDistance < weaponRangeSqr && Vector3.Angle(base.gameObject.transform.forward, enemyDelta) < weapon.meleeAngle)
				{
					closestTarget = targetDistance;
					closestTargetObj = target.gameObject;
				}
			}
		}
		if (closestTargetObj != null)
		{
			this.DamageTargetWithWeapon(weapon, closestTargetObj, isHeadshot, 0f, 1f);
		}
		yield break;
	}

	// Token: 0x0600297F RID: 10623 RVA: 0x000DA0E4 File Offset: 0x000D82E4
	private void RailgunShot(WeaponSounds weapon)
	{
		weapon.fire();
		this._FireFlash(true, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		float num = weapon.tekKoof * Defs.Coef;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * num) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * num)), ((float)Screen.height - weapon.startZone.y * num) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * num)), 0f));
		if (weapon.freezer)
		{
			Player_move_c.RayHitsInfo hitsFromRay = this.GetHitsFromRay(ray, false);
			foreach (RaycastHit hit in hitsFromRay.hits)
			{
				this._DoHit(weapon, hit, true);
			}
			this.AddFreezerRayWithLength(hitsFromRay.lenRay);
			if (this.isMulti)
			{
				if (this.isInet)
				{
					this.photonView.RPC("AddFreezerRayWithLength", PhotonTargets.Others, new object[]
					{
						hitsFromRay.lenRay
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("AddFreezerRayWithLength", RPCMode.Others, new object[]
					{
						hitsFromRay.lenRay
					});
				}
			}
		}
		else
		{
			bool flag = false;
			int num2 = 0;
			do
			{
				Player_move_c.RayHitsInfo hitsFromRay2 = this.GetHitsFromRay(ray, weapon.countReflectionRay == 1);
				foreach (RaycastHit hit2 in hitsFromRay2.hits)
				{
					this._DoHit(weapon, hit2, false);
				}
				bool flag2 = num2 == 0 && (weapon.countReflectionRay == 1 || !hitsFromRay2.obstacleFound);
				Vector3 origin = (num2 != 0) ? ray.origin : this.GunFlash.gameObject.transform.parent.position;
				Vector3 direction = (!flag2) ? ((num2 != 0) ? ray.direction : (hitsFromRay2.rayReflect.origin - this.GunFlash.gameObject.transform.parent.position)) : this.GunFlash.gameObject.transform.parent.parent.forward;
				float len = (!flag2) ? ((num2 != 0) ? hitsFromRay2.lenRay : (hitsFromRay2.rayReflect.origin - this.GunFlash.gameObject.transform.parent.position).magnitude) : 150f;
				base.StartCoroutine(this.ShowRayWithDelay(origin, direction, weapon.railName, len, (float)num2 * 0.05f));
				if (hitsFromRay2.obstacleFound)
				{
					ray = hitsFromRay2.rayReflect;
					flag = true;
				}
				num2++;
			}
			while (flag && num2 < weapon.countReflectionRay);
		}
	}

	// Token: 0x06002980 RID: 10624 RVA: 0x000DA430 File Offset: 0x000D8630
	private void BulletShot(WeaponSounds weapon)
	{
		int num = (!weapon.isShotGun) ? 1 : weapon.countShots;
		float maxDistance = (!weapon.isShotGun) ? 100f : 30f;
		Vector3[] array = null;
		Quaternion[] array2 = null;
		bool[] array3 = null;
		int num2 = Mathf.Min(7, num);
		bool flag = false;
		bool flag2 = false;
		Vector3 vector = Vector3.zero;
		Quaternion quaternion = Quaternion.identity;
		if (weapon.bulletExplode)
		{
			maxDistance = 250f;
		}
		for (int i = 0; i < num; i++)
		{
			float num3 = weapon.tekKoof * Defs.Coef;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(((float)Screen.width - weapon.startZone.x * num3) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.x * num3)), ((float)Screen.height - weapon.startZone.y * num3) * 0.5f + (float)UnityEngine.Random.Range(0, Mathf.RoundToInt(weapon.startZone.y * num3)), 0f));
			Transform x = (!weapon.isDoubleShot) ? this.GunFlash : weapon.gunFlashDouble[this.numShootInDoubleShot - 1];
			if (x != null && !Defs.isDaterRegim)
			{
				GameObject currentBullet = BulletStackController.sharedController.GetCurrentBullet((int)weapon.typeTracer);
				if (currentBullet != null)
				{
					currentBullet.transform.rotation = this.myTransform.rotation;
					Bullet component = currentBullet.GetComponent<Bullet>();
					component.endPos = ray.GetPoint(200f);
					component.startPos = ((!weapon.isDoubleShot) ? this.GunFlash.position : weapon.gunFlashDouble[this.numShootInDoubleShot - 1].position);
					component.StartBullet();
				}
				weapon.fire();
			}
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, maxDistance, Player_move_c._ShootRaycastLayerMask))
			{
				if (!weapon.bulletExplode)
				{
					if (!hit.collider.gameObject.transform.CompareTag("DamagedExplosion") && !hit.collider.gameObject.name.Equals("StopCollider"))
					{
						vector = hit.point + hit.normal * 0.001f;
						quaternion = Quaternion.FromToRotation(Vector3.up, hit.normal);
						flag2 = true;
						flag = (hit.collider.gameObject.transform.root.CompareTag("Enemy") || hit.collider.gameObject.transform.root.CompareTag("Player") || hit.collider.gameObject.transform.root.CompareTag("Pet"));
						this.HoleRPC(flag, vector, quaternion);
						if (this.isMulti)
						{
							if (!this.isInet)
							{
								this._networkView.RPC("HoleRPC", RPCMode.Others, new object[]
								{
									flag,
									vector,
									quaternion
								});
							}
							else if (num2 > 1 && i < num2)
							{
								if (array == null)
								{
									array = new Vector3[num2];
									array2 = new Quaternion[num2];
									array3 = new bool[num2];
								}
								array[i] = vector;
								array2[i] = quaternion;
								array3[i] = flag;
							}
						}
					}
					this._DoHit(weapon, hit, false);
				}
				else
				{
					Rocket rocket = Player_move_c.CreateRocket(this.myCurrentWeaponSounds, hit.point, Quaternion.identity, 1f);
					rocket.StartCoroutine(rocket.KillRocket(hit.collider));
				}
			}
		}
		if (!flag2 || !this.isInet)
		{
			this._FireFlash(true, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		}
		else if (num2 > 1)
		{
			this._FireFlashWithManyHoles(array3, array, array2, true, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		}
		else
		{
			this._FireFlashWithHole(flag, vector, quaternion, true, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		}
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000DA8AC File Offset: 0x000D8AAC
	public void PoisonShotWithEffect(GameObject target, float damageMultiplayer, WeaponSounds weapon)
	{
		this.PoisonShotWithEffect(target, new Player_move_c.PoisonParameters(damageMultiplayer, weapon));
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x000DA8BC File Offset: 0x000D8ABC
	public void PoisonShotWithEffect(GameObject target, Player_move_c.PoisonParameters param)
	{
		if (Defs.isDaterRegim)
		{
			return;
		}
		string tag = target.tag;
		switch (tag)
		{
		case "Enemy":
			if (param.poisonType == Player_move_c.PoisonType.Burn)
			{
				AdvancedEffects component = target.GetComponent<AdvancedEffects>();
				if (component != null)
				{
					component.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
				}
			}
			this.PoisonShot(target, param);
			break;
		case "Player":
			if (param.poisonType == Player_move_c.PoisonType.Burn)
			{
				Player_move_c playerMoveC = target.GetComponent<SkinName>().playerMoveC;
				playerMoveC.SendPlayerEffect(1, param.poisonTime * (float)param.poisonCount);
			}
			this.PoisonShot(target, param);
			break;
		case "Turret":
		{
			TurretController component2 = target.GetComponent<TurretController>();
			if (component2 != null && ((param.poisonType == Player_move_c.PoisonType.Burn && component2.isEnemyTurret) || component2 is VoodooSnowman) && component2.isRun)
			{
				if (param.poisonType == Player_move_c.PoisonType.Burn)
				{
					AdvancedEffects component3 = target.GetComponent<AdvancedEffects>();
					if (component3 != null)
					{
						component3.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
					}
				}
				this.PoisonShot(target, param);
			}
			break;
		}
		case "DamagedExplosion":
			if (param.poisonType == Player_move_c.PoisonType.Burn)
			{
				this.PoisonShot(target, param);
			}
			break;
		case "Pet":
			if (param.poisonType == Player_move_c.PoisonType.Burn)
			{
				AdvancedEffects component4 = target.GetComponent<AdvancedEffects>();
				if (component4 != null)
				{
					component4.SendAdvancedEffect(1, param.poisonTime * (float)param.poisonCount);
				}
			}
			this.PoisonShot(target, param);
			break;
		}
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x000DAAD8 File Offset: 0x000D8CD8
	public void PoisonShot(GameObject target, Player_move_c.PoisonParameters poison)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(this))
		{
			return;
		}
		for (int i = 0; i < this.poisonTargets.Count; i++)
		{
			if (this.poisonTargets[i].target.Equals(component) && this.poisonTargets[i].param.poisonType == poison.poisonType)
			{
				this.poisonTargets[i].UpdatePoison(poison);
				return;
			}
		}
		Player_move_c.PoisonTarget item = new Player_move_c.PoisonTarget(component, poison);
		this.poisonTargets.Add(item);
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x000DAB80 File Offset: 0x000D8D80
	public void shootS()
	{
		if (this.isGrenadePress)
		{
			return;
		}
		WeaponSounds weaponSounds = (!this.isMechActive) ? this._weaponManager.currentWeaponSounds : this.mechWeaponSounds;
		if (weaponSounds.bazooka)
		{
			base.StartCoroutine(this.BazookaShoot());
			return;
		}
		if (weaponSounds.railgun || weaponSounds.freezer)
		{
			this.RailgunShot(weaponSounds);
			return;
		}
		if (weaponSounds.flamethrower)
		{
			this.FlamethrowerShot(weaponSounds);
			return;
		}
		if (weaponSounds.snowStorm)
		{
			this.SnowStormShot(weaponSounds);
			return;
		}
		if (weaponSounds.shocker)
		{
			this.ShockerShot(weaponSounds);
			return;
		}
		if (weaponSounds.isRoundMelee)
		{
			base.StartCoroutine(this.HitRoundMelee(weaponSounds));
			return;
		}
		if (weaponSounds.isMelee)
		{
			base.StartCoroutine(this.MeleeShot(weaponSounds));
			return;
		}
		this.BulletShot(weaponSounds);
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x000DAC68 File Offset: 0x000D8E68
	public void GrenadePress(ThrowGadget gadget)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted)
		{
			this.showGrenadeHint = false;
			HintController.instance.HideHintByName("use_grenade");
		}
		if (this.indexWeapon == 1001)
		{
			return;
		}
		this.currentGrenadeGadget = gadget;
		this.isGrenadePress = true;
		this.currentWeaponBeforeGrenade = WeaponManager.sharedManager.CurrentWeaponIndex;
		this.ChangeWeapon(1000, false);
		this.timeGrenadePress = Time.realtimeSinceStartup;
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider != null)
		{
			this.inGameGUI.blockedCollider.SetActive(true);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider2 != null)
		{
			this.inGameGUI.blockedCollider2.SetActive(true);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedColliderDater != null)
		{
			this.inGameGUI.blockedColliderDater.SetActive(true);
		}
		if (this.inGameGUI != null)
		{
			for (int i = 0; i < this.inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				this.inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = false;
			}
			for (int j = 0; j < this.inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				this.inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = false;
			}
		}
	}

	// Token: 0x06002986 RID: 10630 RVA: 0x000DAE0C File Offset: 0x000D900C
	public void GrenadeFire()
	{
		if (!this.isGrenadePress)
		{
			return;
		}
		float num = Time.realtimeSinceStartup - this.timeGrenadePress;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToThrowGrenade)
		{
			TrainingController.isNextStep = TrainingState.TapToThrowGrenade;
		}
		Defs.isGrenateFireEnable = false;
		float num2 = 0.4f;
		if (this.indexWeapon == 1000)
		{
			GameObject gameObject = Resources.Load("GadgetsContent/" + this.currentGrenadeGadget.GrenadeGadgetId) as GameObject;
			num2 = gameObject.GetComponent<WeaponSounds>().grenadeUseTime;
		}
		if (num - num2 > 0f)
		{
			this.GrenadeStartFire();
		}
		else
		{
			base.Invoke("GrenadeStartFire", num2 - num);
		}
		if (this.currentGrenadeGadget != null)
		{
			this.currentGrenadeGadget.ShowThrowingEffect(num2 - num);
		}
	}

	// Token: 0x06002987 RID: 10631 RVA: 0x000DAEE0 File Offset: 0x000D90E0
	[Obfuscation(Exclude = true)]
	public void GrenadeStartFire()
	{
		if (this.isMulti)
		{
			if (!this.isInet)
			{
				base.GetComponent<NetworkView>().RPC("fireFlash", RPCMode.All, new object[]
				{
					false,
					0
				});
			}
			else
			{
				this.photonView.RPC("fireFlash", PhotonTargets.All, new object[]
				{
					false,
					0
				});
			}
		}
		else
		{
			this.fireFlash(false, 0);
		}
		this.GrenadeCount--;
		float time = (!(this.myCurrentWeaponSounds != null)) ? 0.2667f : this.myCurrentWeaponSounds.grenadeThrowTime;
		base.Invoke("RunGrenade", time);
		base.Invoke("SetGrenateFireEnabled", 1f);
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x000DAFB8 File Offset: 0x000D91B8
	[Obfuscation(Exclude = true)]
	private void SetGrenateFireEnabled()
	{
		Defs.isGrenateFireEnable = true;
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x000DAFC0 File Offset: 0x000D91C0
	[Obfuscation(Exclude = true)]
	private void RunGrenade()
	{
		if (this.currentGrenadeGadget != null)
		{
			this.currentGrenadeGadget.ThrowGrenade();
		}
		base.Invoke("ReturnWeaponAfterGrenade", 0.5f);
		this.isGrenadePress = false;
	}

	// Token: 0x0600298A RID: 10634 RVA: 0x000DAFF0 File Offset: 0x000D91F0
	[Obfuscation(Exclude = true)]
	private void ReturnWeaponAfterGrenade()
	{
		if (this.currentWeaponBeforeGrenade != -1)
		{
			this.ChangeWeapon(this.currentWeaponBeforeGrenade, false);
		}
		this.currentWeaponBeforeGrenade = -1;
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider != null)
		{
			this.inGameGUI.blockedCollider.SetActive(false);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedCollider2 != null)
		{
			this.inGameGUI.blockedCollider2.SetActive(false);
		}
		if (this.inGameGUI != null && this.inGameGUI.blockedColliderDater != null)
		{
			this.inGameGUI.blockedColliderDater.SetActive(false);
		}
		if (this.inGameGUI != null)
		{
			for (int i = 0; i < this.inGameGUI.upButtonsInShopPanel.Length; i++)
			{
				this.inGameGUI.upButtonsInShopPanel[i].GetComponent<ButtonHandler>().isEnable = true;
			}
			for (int j = 0; j < this.inGameGUI.upButtonsInShopPanelSwipeRegim.Length; j++)
			{
				this.inGameGUI.upButtonsInShopPanelSwipeRegim[j].GetComponent<ButtonHandler>().isEnable = true;
			}
		}
	}

	// Token: 0x0600298B RID: 10635 RVA: 0x000DB144 File Offset: 0x000D9344
	public static Rocket CreateRocket(WeaponSounds _currentWeaponSounds, Vector3 pos, Quaternion rot, float _chargePower = 1f)
	{
		GameObject rocket = RocketStack.sharedController.GetRocket();
		rocket.transform.position = pos;
		rocket.transform.rotation = rot;
		Rocket component = rocket.GetComponent<Rocket>();
		string weaponName = _currentWeaponSounds.gameObject.name.Replace("(Clone)", string.Empty);
		float radiusImpulse = _currentWeaponSounds.bazookaImpulseRadius * (1f + EffectsController.ExplosionImpulseRadiusIncreaseCoef);
		component.SendSetRocketActive(weaponName, radiusImpulse, _chargePower);
		return component;
	}

	// Token: 0x0600298C RID: 10636 RVA: 0x000DB1B8 File Offset: 0x000D93B8
	private IEnumerator BazookaShoot()
	{
		for (int i = 0; i < this._weaponManager.currentWeaponSounds.countInSeriaBazooka; i++)
		{
			this._weaponManager.currentWeaponSounds.fire();
			this._FireFlash(true, 0);
			float rangeFromUs = 0.2f;
			Rocket rocketScript = Player_move_c.CreateRocket(this.myCurrentWeaponSounds, (!(this._weaponManager.currentWeaponSounds.gunFlash != null)) ? (this.myTransform.position + this.myTransform.forward * rangeFromUs) : this._weaponManager.currentWeaponSounds.gunFlash.position, this.myTransform.rotation, (!this._weaponManager.currentWeaponSounds.isCharging) ? 1f : this.lastChargeValue);
			this.rocketToLaunch = rocketScript.gameObject;
			if (i != this._weaponManager.currentWeaponSounds.countInSeriaBazooka - 1)
			{
				yield return new WaitForSeconds(this._weaponManager.currentWeaponSounds.stepTimeInSeriaBazooka);
			}
		}
		yield break;
	}

	// Token: 0x0600298D RID: 10637 RVA: 0x000DB1D4 File Offset: 0x000D93D4
	private IEnumerator RunShockerEffect()
	{
		this.myCurrentWeaponSounds._innerPars.shockerEffect.SetActive(true);
		yield return new WaitForSeconds(1f);
		this.myCurrentWeaponSounds._innerPars.shockerEffect.SetActive(false);
		yield break;
	}

	// Token: 0x0600298E RID: 10638 RVA: 0x000DB1F0 File Offset: 0x000D93F0
	private void RunOnGroundEffect(string name)
	{
		if (name == null || this.mySkinName == null)
		{
			return;
		}
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName("OnGroundWeaponEffects/" + name + "_OnGroundEffect");
		if (objectFromName == null)
		{
			return;
		}
		Player_move_c.PerformActionRecurs(objectFromName, delegate(Transform t)
		{
			t.gameObject.SetActive(false);
		});
		objectFromName.transform.parent = this.mySkinName.onGroundEffectsPoint;
		objectFromName.transform.localPosition = Vector3.zero;
		objectFromName.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
		Player_move_c.PerformActionRecurs(objectFromName, delegate(Transform t)
		{
			t.gameObject.SetActive(true);
		});
		ParticleSystem component = objectFromName.GetComponent<ParticleSystem>();
		if (component != null)
		{
			component.Play();
		}
	}

	// Token: 0x0600298F RID: 10639 RVA: 0x000DB2E4 File Offset: 0x000D94E4
	private IEnumerator HitRoundMelee(WeaponSounds weapon)
	{
		this._FireFlash(false, (!weapon.isDoubleShot) ? 0 : this.numShootInDoubleShot);
		yield return new WaitForSeconds(this.TimeOfMeleeAttack(weapon));
		if (weapon == null)
		{
			yield break;
		}
		this.RunOnGroundEffect(weapon.gameObject.name.Replace("(Clone)", string.Empty));
		float weaponRangeSqr = weapon.radiusRoundMelee * weapon.radiusRoundMelee;
		Initializer.TargetsList targets = new Initializer.TargetsList();
		foreach (Transform target in targets)
		{
			float targetDistance = (target.position - this._player.transform.position).sqrMagnitude;
			if (targetDistance < weaponRangeSqr)
			{
				this.DamageTargetWithWeapon(weapon, target.gameObject, false, targetDistance, 1f);
			}
		}
		yield break;
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x000DB310 File Offset: 0x000D9510
	private void _DoHit(WeaponSounds weapon, RaycastHit _hit, bool slowdown = false)
	{
		bool isHeadshot = _hit.collider.name == "HeadCollider" || _hit.collider is SphereCollider;
		if (_hit.transform.root && _hit.transform.root.GetComponent<IDamageable>() != null)
		{
			this.DamageTargetWithWeapon(weapon, _hit.transform.root.gameObject, isHeadshot, 0f, 1f);
			return;
		}
		this.DamageTargetWithWeapon(weapon, _hit.transform.gameObject, isHeadshot, 0f, 1f);
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x000DB3BC File Offset: 0x000D95BC
	private float TimeOfMeleeAttack(WeaponSounds ws)
	{
		if (this.isMechActive)
		{
			return this.mechGunAnimation[(!ws.isDoubleShot) ? "Shoot" : "Shoot1"].length * ws.meleeAttackTimeModifier;
		}
		return ws.animationObject.GetComponent<Animation>()[(!ws.isDoubleShot) ? "Shoot" : "Shoot1"].length * ws.meleeAttackTimeModifier;
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000DB43C File Offset: 0x000D963C
	private void _FireFlash(bool isFlash = true, int numFlash = 0)
	{
		if (this.myCurrentWeaponSounds.isLoopShoot)
		{
			return;
		}
		if (this.isMulti)
		{
			if (this.isInet)
			{
				this.photonView.RPC("fireFlash", PhotonTargets.Others, new object[]
				{
					isFlash,
					numFlash
				});
			}
			else
			{
				this._networkView.RPC("fireFlash", RPCMode.Others, new object[]
				{
					isFlash,
					numFlash
				});
			}
		}
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x000DB4C8 File Offset: 0x000D96C8
	private void _FireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash = true, int numFlash = 0)
	{
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("fireFlashWithHole", PhotonTargets.Others, new object[]
			{
				_isBloodParticle,
				_pos,
				_rot,
				isFlash,
				numFlash
			});
		}
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x000DB534 File Offset: 0x000D9734
	private void _FireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash = true, int numFlash = 0)
	{
		if (this.isMulti && this.isInet)
		{
			this.photonView.RPC("fireFlashWithManyHoles", PhotonTargets.Others, new object[]
			{
				_isBloodParticle,
				_pos,
				_rot,
				isFlash,
				numFlash
			});
		}
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x000DB590 File Offset: 0x000D9790
	[RPC]
	[PunRPC]
	private void fireFlashWithManyHoles(bool[] _isBloodParticle, Vector3[] _pos, Quaternion[] _rot, bool isFlash, int numFlash)
	{
		this.fireFlash(isFlash, numFlash);
		if (_isBloodParticle != null)
		{
			for (int i = 0; i < _isBloodParticle.Length; i++)
			{
				this.HoleRPC(_isBloodParticle[i], _pos[i], _rot[i]);
			}
		}
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x000DB5E4 File Offset: 0x000D97E4
	[PunRPC]
	[RPC]
	private void fireFlashWithHole(bool _isBloodParticle, Vector3 _pos, Quaternion _rot, bool isFlash, int numFlash)
	{
		this.fireFlash(isFlash, numFlash);
		this.HoleRPC(_isBloodParticle, _pos, _rot);
	}

	// Token: 0x06002997 RID: 10647 RVA: 0x000DB5FC File Offset: 0x000D97FC
	[RPC]
	[PunRPC]
	private void fireFlash(bool isFlash, int numFlash)
	{
		WeaponSounds weaponSounds = (!this.isMechActive) ? this.myCurrentWeaponSounds : this.mechWeaponSounds;
		if (weaponSounds == null)
		{
			return;
		}
		if (isFlash)
		{
			if (numFlash == 0)
			{
				FlashFire component = weaponSounds.GetComponent<FlashFire>();
				if (component != null)
				{
					component.fire(this);
				}
			}
			else if (weaponSounds.gunFlashDouble.Length > numFlash - 1)
			{
				if (Application.isEditor)
				{
					Debug.Log(weaponSounds + " " + numFlash.ToString());
				}
				weaponSounds.gunFlashDouble[numFlash - 1].GetComponent<FlashFire>().fire(this);
			}
		}
		if (weaponSounds.isRoundMelee)
		{
			float tm = this.TimeOfMeleeAttack(weaponSounds);
			base.StartCoroutine(this.RunOnGroundEffectCoroutine(weaponSounds.gameObject.name.Replace("(Clone)", string.Empty), tm));
		}
		string animation;
		if (!weaponSounds.isDoubleShot)
		{
			animation = "Shoot";
		}
		else
		{
			animation = "Shoot" + numFlash.ToString();
		}
		if (this.isMechActive)
		{
			this.mechGunAnimation.Play(animation);
			if (Defs.isSoundFX && this.currentMech != null)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.currentMech.shootSound);
			}
		}
		else
		{
			weaponSounds.animationObject.GetComponent<Animation>().Play(animation);
		}
		if (Defs.isSoundFX && !this.isMechActive)
		{
			base.GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().PlayOneShot(weaponSounds.shoot);
		}
		this.playChargeLoopAnim = false;
	}

	// Token: 0x06002998 RID: 10648 RVA: 0x000DB7A0 File Offset: 0x000D99A0
	[RPC]
	[PunRPC]
	public void ChargeGunAnimation(bool active)
	{
		if (this.myCurrentWeaponSounds == null || !this.myCurrentWeaponSounds.isCharging)
		{
			return;
		}
		if (!active)
		{
			this.playChargeLoopAnim = false;
			if (!this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Shoot"))
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Shoot");
			}
			if (Defs.isSoundFX && !this.isMechActive)
			{
				base.GetComponent<AudioSource>().Stop();
			}
			return;
		}
		if (this.myCurrentWeaponSounds.chargeLoop)
		{
			this.playChargeLoopAnim = true;
			base.StartCoroutine(this.PlayChargeLoopAnim());
		}
		else
		{
			this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Charge");
			if (Defs.isSoundFX && !this.isMechActive)
			{
				base.GetComponent<AudioSource>().clip = this.myCurrentWeaponSounds.charge;
				base.GetComponent<AudioSource>().Play();
			}
		}
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x000DB8B4 File Offset: 0x000D9AB4
	private IEnumerator PlayChargeLoopAnim()
	{
		while (this.playChargeLoopAnim && this.myCurrentWeaponSounds != null && this.myCurrentWeaponSounds.isCharging && this.myCurrentWeaponSounds.chargeLoop)
		{
			if (!this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().IsPlaying("Charge"))
			{
				this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Charge");
				if (Defs.isSoundFX && !this.isMechActive)
				{
					base.GetComponent<AudioSource>().clip = this.myCurrentWeaponSounds.charge;
					base.GetComponent<AudioSource>().Play();
				}
			}
			yield return null;
		}
		this.playChargeLoopAnim = false;
		yield break;
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x000DB8D0 File Offset: 0x000D9AD0
	[RPC]
	[PunRPC]
	public void HoleRPC(bool _isBloodParticle, Vector3 _pos, Quaternion _rot)
	{
		if (Device.isPixelGunLow)
		{
			return;
		}
		if (!_isBloodParticle)
		{
			HoleScript currentHole = HoleBulletStackController.sharedController.GetCurrentHole(false);
			if (currentHole != null)
			{
				currentHole.StartShowHole(_pos, _rot, false);
			}
		}
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x000DB914 File Offset: 0x000D9B14
	private float GetRawWeaponDamage(WeaponSounds weapon)
	{
		float num;
		if (weapon.isMechWeapon)
		{
			num = weapon.DamageByTier[0];
		}
		else
		{
			num = ((!(ExpController.Instance != null) || ExpController.Instance.OurTier >= weapon.DamageByTier.Length) ? ((weapon.DamageByTier.Length <= 0) ? 0f : weapon.DamageByTier[0]) : weapon.DamageByTier[this.TierOrRoomTier(ExpController.Instance.OurTier)]);
			num *= 1f + this.koofDamageWeaponFromPotoins + EffectsController.DamageModifsByCats(weapon.categoryNabor - 1);
		}
		return num;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x000DB9BC File Offset: 0x000D9BBC
	private float GetWeaponDamage(WeaponSounds weapon, float sqrDistance, bool isHeadshot)
	{
		float num = this.GetRawWeaponDamage(weapon);
		if (weapon.isRoundMelee)
		{
			float num2 = num * 0.7f;
			float num3 = num;
			num = num2 + (num3 - num2) * (1f - sqrDistance / (weapon.radiusRoundMelee * weapon.radiusRoundMelee));
		}
		else if (weapon.snowStorm)
		{
			float num4 = num;
			num = num4 * (1f - sqrDistance / (weapon.range * weapon.range));
			if (sqrDistance < weapon.snowStormBonusRange)
			{
				num += num4 * weapon.snowStormBonusMultiplier;
			}
		}
		else if (isHeadshot)
		{
			num *= ((!weapon.isMechWeapon) ? (2f + EffectsController.AddingForHeadshot(weapon.categoryNabor - 1)) : 2f);
		}
		if (weapon.isCharging)
		{
			num *= this.lastChargeValue;
		}
		return num;
	}

	// Token: 0x0600299D RID: 10653 RVA: 0x000DBA90 File Offset: 0x000D9C90
	public void DamageTargetWithWeapon(WeaponSounds weapon, GameObject target, bool isHeadshot = false, float sqrDistance = 0f, float damageMultiplier = 1f)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(this))
		{
			return;
		}
		if (Defs.isDaterRegim && weapon.isDaterWeapon && component is PlayerDamageable)
		{
			Player_move_c myPlayer = (component as PlayerDamageable).myPlayer;
			myPlayer.SendDaterChat(this.mySkinName.NickName, weapon.daterMessage, myPlayer.mySkinName.NickName);
			return;
		}
		if (component is PlayerDamageable)
		{
			Player_move_c myPlayer2 = (component as PlayerDamageable).myPlayer;
			if (isHeadshot)
			{
				float num = UnityEngine.Random.Range(0f, 1f);
				isHeadshot = (num >= myPlayer2._chanceToIgnoreHeadshot);
			}
			if (weapon.snowStorm)
			{
				this.SendPlayerEffectToPlayer(myPlayer2.photonView.owner, myPlayer2.GetComponent<NetworkView>().owner, 9, 0.5f);
			}
		}
		if (weapon.isSlowdown)
		{
			this.SlowdownTarget(component, weapon.slowdownTime, weapon.slowdownCoeff);
		}
		float num2 = this.GetWeaponDamage(weapon, sqrDistance, isHeadshot) * damageMultiplier;
		num2 *= this.DamageMultiplierByGadgets();
		bool flag = weapon.criticalHitChance >= UnityEngine.Random.Range(0, 100);
		if (flag)
		{
			num2 *= weapon.criticalHitCoef;
		}
		string weaponName = (!this.isMechActive) ? weapon.gameObject.name.Replace("(Clone)", string.Empty) : ((!(this.currentMech != null)) ? "Chat_Mech" : this.currentMech.killChatIcon);
		Player_move_c.TypeKills typeKill = (!this.isMechActive) ? ((!flag) ? ((!isHeadshot) ? ((!this.isZooming) ? Player_move_c.TypeKills.none : Player_move_c.TypeKills.zoomingshot) : Player_move_c.TypeKills.headshot) : Player_move_c.TypeKills.critical) : Player_move_c.TypeKills.mech;
		this.ApplyDamageToTarget(component, num2, weaponName, weapon.typeDead, typeKill);
		if (weapon.isPoisoning)
		{
			this.PoisonShotWithEffect(target, num2, weapon);
		}
	}

	// Token: 0x0600299E RID: 10654 RVA: 0x000DBC88 File Offset: 0x000D9E88
	public void DamageTarget(GameObject target, float damage, string weaponName, WeaponSounds.TypeDead typeDead, Player_move_c.TypeKills typeKill)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null || !component.IsEnemyTo(this))
		{
			return;
		}
		this.ApplyDamageToTarget(component, damage, weaponName, typeDead, typeKill);
	}

	// Token: 0x0600299F RID: 10655 RVA: 0x000DBCBC File Offset: 0x000D9EBC
	private void ApplyDamageToTarget(IDamageable damageable, float damage, string weaponName, WeaponSounds.TypeDead typeDead, Player_move_c.TypeKills typeKill)
	{
		damageable.ApplyDamage(damage, this.myDamageable, typeKill, typeDead, weaponName, this.skinNamePixelView.viewID);
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x000DBCE8 File Offset: 0x000D9EE8
	public void SlowdownTarget(GameObject target, float slowdownTime, float slowdownCoeff)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null)
		{
			return;
		}
		this.SlowdownTarget(component, slowdownTime, slowdownCoeff);
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x000DBD0C File Offset: 0x000D9F0C
	public void SlowdownTarget(IDamageable damageable, float slowdownTime, float slowdownCoeff)
	{
		if (damageable is BaseBot)
		{
			BaseBot baseBot = damageable as BaseBot;
			baseBot.ApplyDebuffByMode(BotDebuffType.DecreaserSpeed, slowdownTime, slowdownCoeff);
		}
		if (damageable is PlayerDamageable)
		{
			Player_move_c myPlayer = (damageable as PlayerDamageable).myPlayer;
			myPlayer.SlowdownPlayer(slowdownCoeff, slowdownTime);
		}
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x000DBD58 File Offset: 0x000D9F58
	public void SlowdownPlayer(float coef, float time)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SlowdownRPC", this.photonView.owner, new object[]
				{
					coef,
					time
				});
			}
			else
			{
				this._networkView.RPC("SlowdownRPC", this._networkView.owner, new object[]
				{
					coef,
					time
				});
			}
		}
		else
		{
			this.SlowdownRPC(coef, time);
		}
	}

	// Token: 0x060029A3 RID: 10659 RVA: 0x000DBDF4 File Offset: 0x000D9FF4
	[RPC]
	[PunRPC]
	public void SlowdownRPC(float coef, float time)
	{
		if (!Defs.isMulti || this.isMine)
		{
			EffectsController.SlowdownCoeff = coef;
			this._timeOfSlowdown = time;
		}
	}

	// Token: 0x060029A4 RID: 10660 RVA: 0x000DBE24 File Offset: 0x000DA024
	[RPC]
	[PunRPC]
	private void ReloadGun()
	{
		if (this.myCurrentWeaponSounds == null)
		{
			return;
		}
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>().Play("Reload");
		this.myCurrentWeaponSounds.animationObject.GetComponent<Animation>()["Reload"].speed = this._currentReloadAnimationSpeed;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.myCurrentWeaponSounds.reload);
		}
	}

	// Token: 0x060029A5 RID: 10661 RVA: 0x000DBEA4 File Offset: 0x000DA0A4
	private bool Reload()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.currentWeaponSounds != null && this.inGameGUI != null)
		{
			if (WeaponManager.sharedManager.currentWeaponSounds.ammoInClip > 1 || !WeaponManager.sharedManager.currentWeaponSounds.isShotMelee)
			{
				AnimationClip clip = WeaponManager.sharedManager.currentWeaponSounds.animationObject.GetComponent<Animation>().GetClip("Reload");
				if (!(clip != null))
				{
					return false;
				}
				this.inGameGUI.ShowCircularIndicatorOnReload(clip.length / this._currentReloadAnimationSpeed);
			}
			else
			{
				WeaponManager.sharedManager.ReloadAmmo();
			}
		}
		WeaponManager.sharedManager.Reload();
		return true;
	}

	// Token: 0x060029A6 RID: 10662 RVA: 0x000DBF74 File Offset: 0x000DA174
	[Obfuscation(Exclude = true)]
	public void ReloadPressed()
	{
		if (this.myCurrentWeaponSounds.isCharging && this.chargeValue > 0f)
		{
			return;
		}
		if (this.isGrenadePress || this.isReloading)
		{
			return;
		}
		if (this.isMechActive)
		{
			return;
		}
		if (this._weaponManager.currentWeaponSounds.isMelee && !this._weaponManager.currentWeaponSounds.isShotMelee)
		{
			return;
		}
		if (this.isZooming)
		{
			this.ZoomPress();
		}
		if (this._weaponManager.CurrentWeaponIndex < 0 || this._weaponManager.CurrentWeaponIndex >= this._weaponManager.playerWeapons.Count)
		{
			return;
		}
		if (this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).currentAmmoInBackpack > 0 && this.GetWeaponByIndex(this._weaponManager.CurrentWeaponIndex).currentAmmoInClip != this._weaponManager.currentWeaponSounds.ammoInClip)
		{
			bool flag = this.Reload();
			if (!flag || this._weaponManager.currentWeaponSounds.isShotMelee)
			{
				return;
			}
			if (this.isMulti)
			{
				if (!this.isInet)
				{
					base.GetComponent<NetworkView>().RPC("ReloadGun", RPCMode.Others, new object[0]);
				}
				else
				{
					this.photonView.RPC("ReloadGun", PhotonTargets.Others, new object[0]);
				}
			}
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._weaponManager.currentWeaponSounds.reload);
			}
			if (JoystickController.rightJoystick != null)
			{
				JoystickController.rightJoystick.HasAmmo();
				if (this.inGameGUI != null)
				{
					this.inGameGUI.BlinkNoAmmo(0);
				}
			}
			else
			{
				Debug.Log("JoystickController.rightJoystick = null");
			}
		}
	}

	// Token: 0x060029A7 RID: 10663 RVA: 0x000DC154 File Offset: 0x000DA354
	public void RemoveRay(FreezerRay ray)
	{
		for (int i = 0; i < this.freezeRays.Length; i++)
		{
			this.freezeRays[i] = ray;
			if (ray)
			{
				this.freezeRays[i] = null;
			}
		}
	}

	// Token: 0x060029A8 RID: 10664 RVA: 0x000DC19C File Offset: 0x000DA39C
	public void AddScoreKillPet()
	{
		this.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.petKnockout, 1f);
		this.AddCountSerials(7, this);
	}

	// Token: 0x060029A9 RID: 10665 RVA: 0x000DC1B8 File Offset: 0x000DA3B8
	private void AddFreezerRayForGunFlash(int freezeInd, Transform gf, float len)
	{
		if (gf == null)
		{
			return;
		}
		if (this.freezeRays[freezeInd] != null)
		{
			this.freezeRays[freezeInd].UpdatePosition(len);
		}
		else
		{
			GameObject gameObject = WeaponManager.AddRay(gf.gameObject.transform.parent.position, gf.gameObject.transform.parent.parent.forward, gf.gameObject.transform.parent.parent.GetComponent<WeaponSounds>().railName, len);
			if (gameObject != null)
			{
				this.freezeRays[freezeInd] = gameObject.GetComponent<FreezerRay>();
				if (this.freezeRays[freezeInd] != null)
				{
					this.freezeRays[freezeInd].Activate(this, gf);
				}
			}
		}
	}

	// Token: 0x060029AA RID: 10666 RVA: 0x000DC28C File Offset: 0x000DA48C
	[RPC]
	[PunRPC]
	public void AddFreezerRayWithLength(float len)
	{
		if (this.myCurrentWeaponSounds == null)
		{
			return;
		}
		if (this.myCurrentWeaponSounds.isDoubleShot)
		{
			for (int i = 0; i < this.myCurrentWeaponSounds.gunFlashDouble.Length; i++)
			{
				Transform gf = this.myCurrentWeaponSounds.gunFlashDouble[i];
				this.AddFreezerRayForGunFlash(i, gf, len);
			}
		}
		else
		{
			Transform transform = this.GunFlash;
			if (transform == null && this.myTransform.childCount > 0)
			{
				Transform child = this.myTransform.GetChild(0);
				FlashFire component = child.GetComponent<FlashFire>();
				if (component != null && component.gunFlashObj != null)
				{
					transform = component.gunFlashObj.transform;
				}
			}
			this.AddFreezerRayForGunFlash(0, transform, len);
		}
	}

	// Token: 0x060029AB RID: 10667 RVA: 0x000DC364 File Offset: 0x000DA564
	public void RunTurret()
	{
		if (Defs.isTurretWeapon)
		{
			if (Defs.isDaterRegim)
			{
				string key = (!Defs.isDaterRegim) ? GearManager.Turret : GearManager.MusicBox;
				Storager.setInt(key, Storager.getInt(key, false) - 1, false);
				PotionsController.sharedController.ActivatePotion(GearManager.Turret, this, new Dictionary<string, object>(), false);
			}
			this.currentTurret.transform.parent = null;
			this.currentTurret.GetComponent<TurretController>().StartTurret();
			this.ChangeWeapon(this.currentWeaponBeforeTurret, false);
			this.currentWeaponBeforeTurret = -1;
		}
	}

	// Token: 0x060029AC RID: 10668 RVA: 0x000DC3FC File Offset: 0x000DA5FC
	public void CancelTurret()
	{
		this.ChangeWeapon(this.currentWeaponBeforeTurret, false);
		this.currentWeaponBeforeTurret = -1;
		if (this.currentTurret != null)
		{
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					PhotonNetwork.Destroy(this.currentTurret);
				}
				else
				{
					Network.RemoveRPCs(this.currentTurret.GetComponent<NetworkView>().viewID);
					Network.Destroy(this.currentTurret);
				}
			}
			else
			{
				UnityEngine.Object.Destroy(this.currentTurret);
			}
		}
	}

	// Token: 0x060029AD RID: 10669 RVA: 0x000DC484 File Offset: 0x000DA684
	public void SendLike(Player_move_c whomMoveC)
	{
		if (whomMoveC != null)
		{
			whomMoveC.SendDaterChat(this.mySkinName.NickName, "Key_1803", whomMoveC.mySkinName.NickName);
		}
		if (Defs.isInet)
		{
			this.photonView.RPC("LikeRPC", PhotonTargets.All, new object[]
			{
				this.photonView.ownerId,
				whomMoveC.photonView.ownerId
			});
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("LikeRPCLocal", RPCMode.All, new object[]
			{
				base.GetComponent<NetworkView>().viewID,
				whomMoveC.GetComponent<NetworkView>().viewID
			});
		}
	}

	// Token: 0x060029AE RID: 10670 RVA: 0x000DC548 File Offset: 0x000DA748
	[PunRPC]
	[RPC]
	private void LikeRPC(int idWho, int idWhom)
	{
		Player_move_c player_move_c = null;
		Player_move_c player_move_c2 = null;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c player_move_c3 = Initializer.players[i];
			if (idWho == player_move_c3.photonView.ownerId)
			{
				player_move_c = player_move_c3;
			}
			if (idWhom == player_move_c3.photonView.ownerId)
			{
				player_move_c2 = player_move_c3;
			}
		}
		if (player_move_c != null && player_move_c2 != null)
		{
			this.Like(player_move_c, player_move_c2);
		}
	}

	// Token: 0x060029AF RID: 10671 RVA: 0x000DC5C8 File Offset: 0x000DA7C8
	[PunRPC]
	[RPC]
	private void LikeRPCLocal(NetworkViewID idWho, NetworkViewID idWhom)
	{
		Player_move_c player_move_c = null;
		Player_move_c player_move_c2 = null;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			Player_move_c player_move_c3 = Initializer.players[i];
			if (idWho.Equals(player_move_c3.GetComponent<NetworkView>().viewID))
			{
				player_move_c = player_move_c3;
			}
			if (idWhom.Equals(player_move_c3.GetComponent<NetworkView>().viewID))
			{
				player_move_c2 = player_move_c3;
			}
		}
		if (player_move_c != null && player_move_c2 != null)
		{
			this.Like(player_move_c, player_move_c2);
		}
	}

	// Token: 0x060029B0 RID: 10672 RVA: 0x000DC65C File Offset: 0x000DA85C
	private void Like(Player_move_c whoMoveC, Player_move_c whomMoveC)
	{
		if (whomMoveC.Equals(WeaponManager.sharedManager.myPlayerMoveC))
		{
			this.countKills++;
			GlobalGameController.CountKills = this.countKills;
			WeaponManager.sharedManager.myNetworkStartTable.CountKills = this.countKills;
			WeaponManager.sharedManager.myNetworkStartTable.SynhCountKills(null);
			ProfileController.OnGetLike();
		}
	}

	// Token: 0x060029B1 RID: 10673 RVA: 0x000DC6C4 File Offset: 0x000DA8C4
	private int GetNumShootInDouble()
	{
		this.numShootInDoubleShot++;
		if (this.numShootInDoubleShot == 3)
		{
			this.numShootInDoubleShot = 1;
		}
		return this.numShootInDoubleShot;
	}

	// Token: 0x060029B2 RID: 10674 RVA: 0x000DC6F0 File Offset: 0x000DA8F0
	[RPC]
	[PunRPC]
	private void SyncTurretUpgrade(int turretUpgrade)
	{
		this.turretUpgrade = turretUpgrade;
	}

	// Token: 0x060029B3 RID: 10675 RVA: 0x000DC6FC File Offset: 0x000DA8FC
	private void InitPurchaseActions()
	{
		this._actionsForPurchasedItems.Add("bigammopack", new Action<string>(this.ProvideAmmo));
		this._actionsForPurchasedItems.Add("Fullhealth", new Action<string>(this.ProvideHealth));
		this._actionsForPurchasedItems.Add(StoreKitEventListener.elixirID, delegate(string inShopId)
		{
			Defs.NumberOfElixirs++;
		});
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor, delegate(string inShopId)
		{
		});
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor2, delegate(string inShopId)
		{
		});
		this._actionsForPurchasedItems.Add(StoreKitEventListener.armor3, delegate(string inShopId)
		{
		});
		foreach (string key in PotionsController.potions)
		{
			this._actionsForPurchasedItems.Add(key, new Action<string>(this.providePotion));
		}
		string[] canBuyWeaponTags = ItemDb.GetCanBuyWeaponTags(true);
		for (int j = 0; j < canBuyWeaponTags.Length; j++)
		{
			string shopIdByTag = ItemDb.GetShopIdByTag(canBuyWeaponTags[j]);
			this._actionsForPurchasedItems.Add(shopIdByTag, new Action<string>(this.AddWeaponToInv));
		}
	}

	// Token: 0x060029B4 RID: 10676 RVA: 0x000DC870 File Offset: 0x000DAA70
	private void AddWeaponToInv(string shopId)
	{
		string tagByShopId = ItemDb.GetTagByShopId(shopId);
		ItemRecord byTag = ItemDb.GetByTag(tagByShopId);
		if ((TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None) && byTag != null && !byTag.TemporaryGun)
		{
			Player_move_c.SaveWeaponInPrefs(tagByShopId, 0);
		}
		GameObject prefabByTag = this._weaponManager.GetPrefabByTag(tagByShopId);
		this.AddWeapon(prefabByTag);
	}

	// Token: 0x060029B5 RID: 10677 RVA: 0x000DC8CC File Offset: 0x000DAACC
	public static void SaveWeaponInPrefs(string weaponTag, int timeForRentIndex = 0)
	{
		string storageIdByTag = ItemDb.GetStorageIdByTag(weaponTag);
		if (storageIdByTag == null)
		{
			int tm = TempItemsController.RentTimeForIndex(timeForRentIndex);
			TempItemsController.sharedController.AddTemporaryItem(weaponTag, tm);
			return;
		}
		Storager.setInt(storageIdByTag, 1, true);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x04001CF2 RID: 7410
	public const int GADGET_SERIAL = 6;

	// Token: 0x04001CF3 RID: 7411
	public const int PETKILL_SERIAL = 7;

	// Token: 0x04001CF4 RID: 7412
	private const string keyKilledPlayerCharactersCount = "KilledPlayerCharactersCount";

	// Token: 0x04001CF5 RID: 7413
	private const string startShootAnimName = "Shoot_start";

	// Token: 0x04001CF6 RID: 7414
	private const string endShootAnimName = "Shoot_end";

	// Token: 0x04001CF7 RID: 7415
	private const float slowdownCoefConst = 0.75f;

	// Token: 0x04001CF8 RID: 7416
	private int tierForKilledRate;

	// Token: 0x04001CF9 RID: 7417
	private readonly Dictionary<string, int> weKillForKillRate;

	// Token: 0x04001CFA RID: 7418
	private readonly Dictionary<string, int> weWereKilledForKillRate;

	// Token: 0x04001CFB RID: 7419
	public ColliderCollisions[] petPointsFlying;

	// Token: 0x04001CFC RID: 7420
	public ColliderCollisions[] petPointsGround;

	// Token: 0x04001CFD RID: 7421
	public TextMesh nickLabel;

	// Token: 0x04001CFE RID: 7422
	public AudioClip mechBearActivSound;

	// Token: 0x04001CFF RID: 7423
	public AudioClip potionSound;

	// Token: 0x04001D00 RID: 7424
	public AudioClip invisibleActivSound;

	// Token: 0x04001D01 RID: 7425
	public AudioClip invisibleDeactivSound;

	// Token: 0x04001D02 RID: 7426
	public AudioClip jetpackActivSound;

	// Token: 0x04001D03 RID: 7427
	public AudioClip portalSound;

	// Token: 0x04001D04 RID: 7428
	public PlayerScoreController myScoreController;

	// Token: 0x04001D05 RID: 7429
	public bool isRocketJump;

	// Token: 0x04001D06 RID: 7430
	public float armorSynch;

	// Token: 0x04001D07 RID: 7431
	public bool isReloading;

	// Token: 0x04001D08 RID: 7432
	public bool _isPlacemarker;

	// Token: 0x04001D09 RID: 7433
	public GameObject placemarkerMark;

	// Token: 0x04001D0A RID: 7434
	public GameObject blackMark;

	// Token: 0x04001D0B RID: 7435
	public GameObject redMark;

	// Token: 0x04001D0C RID: 7436
	public GameObject blueMark;

	// Token: 0x04001D0D RID: 7437
	public Player_move_c placemarkerMoveC;

	// Token: 0x04001D0E RID: 7438
	public bool isResurrectionKill;

	// Token: 0x04001D0F RID: 7439
	public Player_move_c resurrectionMoveC;

	// Token: 0x04001D10 RID: 7440
	[Header("0-Ammo; 1-Health; 2-Armor; 3-Grenade")]
	public ParticleBonuse[] bonusesParticles;

	// Token: 0x04001D11 RID: 7441
	public KillStreakMapper killStreakParticles;

	// Token: 0x04001D12 RID: 7442
	public GameObject particleBonusesPoint;

	// Token: 0x04001D13 RID: 7443
	public Transform myTransform;

	// Token: 0x04001D14 RID: 7444
	public Transform myPlayerTransform;

	// Token: 0x04001D15 RID: 7445
	public int myPlayerID;

	// Token: 0x04001D16 RID: 7446
	public NetworkViewID myPlayerIDLocal;

	// Token: 0x04001D17 RID: 7447
	public SkinName mySkinName;

	// Token: 0x04001D18 RID: 7448
	public GameObject mechBearPoint;

	// Token: 0x04001D19 RID: 7449
	public GameObject mechBearBody;

	// Token: 0x04001D1A RID: 7450
	public GameObject mechBearHands;

	// Token: 0x04001D1B RID: 7451
	public Animation mechBearBodyAnimation;

	// Token: 0x04001D1C RID: 7452
	public GameObject fpsPlayerBody;

	// Token: 0x04001D1D RID: 7453
	public GameObject myCurrentWeapon;

	// Token: 0x04001D1E RID: 7454
	public WeaponSounds myCurrentWeaponSounds;

	// Token: 0x04001D1F RID: 7455
	public GameObject mechExplossion;

	// Token: 0x04001D20 RID: 7456
	public GameObject bearExplosion;

	// Token: 0x04001D21 RID: 7457
	public AudioSource mechExplossionSound;

	// Token: 0x04001D22 RID: 7458
	public AudioSource mechBearExplosionSound;

	// Token: 0x04001D23 RID: 7459
	public SkinnedMeshRenderer playerBodyRenderer;

	// Token: 0x04001D24 RID: 7460
	public SkinnedMeshRenderer mechBearBodyRenderer;

	// Token: 0x04001D25 RID: 7461
	public SkinnedMeshRenderer mechBearHandRenderer;

	// Token: 0x04001D26 RID: 7462
	public SynhRotationWithGameObject mechBearSyncRot;

	// Token: 0x04001D27 RID: 7463
	public Transform PlayerHeadTransform;

	// Token: 0x04001D28 RID: 7464
	public Transform MechHeadTransform;

	// Token: 0x04001D29 RID: 7465
	public CapsuleCollider bodyCollayder;

	// Token: 0x04001D2A RID: 7466
	public CapsuleCollider headCollayder;

	// Token: 0x04001D2B RID: 7467
	private int numShootInDoubleShot = 1;

	// Token: 0x04001D2C RID: 7468
	public bool isMechActive;

	// Token: 0x04001D2D RID: 7469
	public bool isBearActive;

	// Token: 0x04001D2E RID: 7470
	public AudioClip flagGetClip;

	// Token: 0x04001D2F RID: 7471
	public AudioClip flagLostClip;

	// Token: 0x04001D30 RID: 7472
	public AudioClip flagScoreEnemyClip;

	// Token: 0x04001D31 RID: 7473
	public AudioClip flagScoreMyCommandClip;

	// Token: 0x04001D32 RID: 7474
	public float deltaAngle;

	// Token: 0x04001D33 RID: 7475
	public GameObject playerDeadPrefab;

	// Token: 0x04001D34 RID: 7476
	public ThirdPersonNetwork1 myPersonNetwork;

	// Token: 0x04001D35 RID: 7477
	public GameObject grenadePrefab;

	// Token: 0x04001D36 RID: 7478
	public GameObject likePrefab;

	// Token: 0x04001D37 RID: 7479
	public GameObject turretPrefab;

	// Token: 0x04001D38 RID: 7480
	public GameObject turretPoint;

	// Token: 0x04001D39 RID: 7481
	public GameObject currentTurret;

	// Token: 0x04001D3A RID: 7482
	public float liveMech = 9f;

	// Token: 0x04001D3B RID: 7483
	public float[] liveMechByTier;

	// Token: 0x04001D3C RID: 7484
	private ThrowGadget currentGrenadeGadget;

	// Token: 0x04001D3D RID: 7485
	public int currentWeaponBeforeTurret = -1;

	// Token: 0x04001D3E RID: 7486
	private int currentWeaponBeforeGrenade = -1;

	// Token: 0x04001D3F RID: 7487
	private float stdFov = 60f;

	// Token: 0x04001D40 RID: 7488
	private AudioSource myAudioSource;

	// Token: 0x04001D41 RID: 7489
	private int countMultyFlag;

	// Token: 0x04001D42 RID: 7490
	private string[] iconShotName = new string[]
	{
		string.Empty,
		"Chat_Death",
		"Chat_HeadShot",
		"Chat_Explode",
		"Chat_Sniper",
		"Chat_Flag",
		"Chat_grenade",
		"Chat_grenade_hell",
		string.Empty,
		"Chat_Turret_Explode",
		string.Empty,
		"Smile_1_15",
		"Chat_Poison",
		"Chat_Reflection",
		"Chat_Burning",
		string.Empty,
		string.Empty,
		"Chat_bleeding",
		string.Empty
	};

	// Token: 0x04001D43 RID: 7491
	public bool isImVisible;

	// Token: 0x04001D44 RID: 7492
	public bool isWeaponSet;

	// Token: 0x04001D45 RID: 7493
	public NetworkStartTableNGUIController networkStartTableNGUIController;

	// Token: 0x04001D46 RID: 7494
	public GameObject invisibleParticle;

	// Token: 0x04001D47 RID: 7495
	public bool isInvisible;

	// Token: 0x04001D48 RID: 7496
	private bool isInvisByWeapon;

	// Token: 0x04001D49 RID: 7497
	private bool isInvisByGadget;

	// Token: 0x04001D4A RID: 7498
	public bool isBigHead;

	// Token: 0x04001D4B RID: 7499
	public float maxTimeSetTimerShow = 0.5f;

	// Token: 0x04001D4C RID: 7500
	private float _koofDamageWeaponFromPotoins;

	// Token: 0x04001D4D RID: 7501
	public bool isRegenerationLiveZel;

	// Token: 0x04001D4E RID: 7502
	private float maxTimerRegenerationLiveZel = 5f;

	// Token: 0x04001D4F RID: 7503
	public bool isRegenerationLiveCape;

	// Token: 0x04001D50 RID: 7504
	private float maxTimerRegenerationLiveCape = 15f;

	// Token: 0x04001D51 RID: 7505
	private float timerRegenerationLiveZel;

	// Token: 0x04001D52 RID: 7506
	private float timerRegenerationLiveCape;

	// Token: 0x04001D53 RID: 7507
	private float timerRegenerationArmor;

	// Token: 0x04001D54 RID: 7508
	private Shader[] oldShadersInInvisible;

	// Token: 0x04001D55 RID: 7509
	private Color[] oldColorInInvisible;

	// Token: 0x04001D56 RID: 7510
	public bool isCaptureFlag;

	// Token: 0x04001D57 RID: 7511
	public GameObject myBaza;

	// Token: 0x04001D58 RID: 7512
	public Camera myCamera;

	// Token: 0x04001D59 RID: 7513
	public Camera gunCamera;

	// Token: 0x04001D5A RID: 7514
	public GameObject hatsPoint;

	// Token: 0x04001D5B RID: 7515
	public GameObject capesPoint;

	// Token: 0x04001D5C RID: 7516
	public GameObject flagPoint;

	// Token: 0x04001D5D RID: 7517
	public GameObject bootsPoint;

	// Token: 0x04001D5E RID: 7518
	public GameObject armorPoint;

	// Token: 0x04001D5F RID: 7519
	public GameObject arrowToPortalPoint;

	// Token: 0x04001D60 RID: 7520
	public bool isZooming;

	// Token: 0x04001D61 RID: 7521
	public AudioClip headShotSound;

	// Token: 0x04001D62 RID: 7522
	public AudioClip flagCaptureClip;

	// Token: 0x04001D63 RID: 7523
	public AudioClip flagPointClip;

	// Token: 0x04001D64 RID: 7524
	public GameObject headShotParticle;

	// Token: 0x04001D65 RID: 7525
	public GameObject healthParticle;

	// Token: 0x04001D66 RID: 7526
	public GameObject chatViewer;

	// Token: 0x04001D67 RID: 7527
	public GUISkin MySkin;

	// Token: 0x04001D68 RID: 7528
	public GameObject myTable;

	// Token: 0x04001D69 RID: 7529
	public NetworkStartTable myNetworkStartTable;

	// Token: 0x04001D6A RID: 7530
	private float[] _byCatDamageModifs = new float[6];

	// Token: 0x04001D6B RID: 7531
	public int AimTextureWidth = 50;

	// Token: 0x04001D6C RID: 7532
	public int AimTextureHeight = 50;

	// Token: 0x04001D6D RID: 7533
	public Transform GunFlash;

	// Token: 0x04001D6E RID: 7534
	private bool isZachetWin;

	// Token: 0x04001D6F RID: 7535
	public bool showGUIUnlockFullVersion;

	// Token: 0x04001D70 RID: 7536
	public float timeHingerGame;

	// Token: 0x04001D71 RID: 7537
	public int BulletForce = 5000;

	// Token: 0x04001D72 RID: 7538
	public bool killed;

	// Token: 0x04001D73 RID: 7539
	public ZombiManager zombiManager;

	// Token: 0x04001D74 RID: 7540
	public NickLabelController myNickLabelController;

	// Token: 0x04001D75 RID: 7541
	public visibleObjPhoton visibleObj;

	// Token: 0x04001D76 RID: 7542
	public string textChat;

	// Token: 0x04001D77 RID: 7543
	public bool showGUI = true;

	// Token: 0x04001D78 RID: 7544
	public bool showRanks;

	// Token: 0x04001D79 RID: 7545
	public Player_move_c.SystemMessage[] killedSpisok = new Player_move_c.SystemMessage[3];

	// Token: 0x04001D7A RID: 7546
	public GUIStyle combatRifleStyle;

	// Token: 0x04001D7B RID: 7547
	public GUIStyle goldenEagleInappStyle;

	// Token: 0x04001D7C RID: 7548
	public GUIStyle magicBowInappStyle;

	// Token: 0x04001D7D RID: 7549
	public GUIStyle spasStyle;

	// Token: 0x04001D7E RID: 7550
	public GUIStyle axeStyle;

	// Token: 0x04001D7F RID: 7551
	public GUIStyle famasStyle;

	// Token: 0x04001D80 RID: 7552
	public GUIStyle glockStyle;

	// Token: 0x04001D81 RID: 7553
	public GUIStyle chainsawStyle;

	// Token: 0x04001D82 RID: 7554
	public GUIStyle scytheStyle;

	// Token: 0x04001D83 RID: 7555
	public GUIStyle shovelStyle;

	// Token: 0x04001D84 RID: 7556
	private Vector3 camPosition;

	// Token: 0x04001D85 RID: 7557
	private Quaternion camRotaion;

	// Token: 0x04001D86 RID: 7558
	public bool showChat;

	// Token: 0x04001D87 RID: 7559
	public bool showChatOld;

	// Token: 0x04001D88 RID: 7560
	public bool showRanksOld;

	// Token: 0x04001D89 RID: 7561
	private bool isDeadFrame;

	// Token: 0x04001D8A RID: 7562
	private int _myCommand;

	// Token: 0x04001D8B RID: 7563
	public bool respawnedForGUI = true;

	// Token: 0x04001D8C RID: 7564
	public PixelView skinNamePixelView;

	// Token: 0x04001D8D RID: 7565
	public PlayerDamageable myDamageable;

	// Token: 0x04001D8E RID: 7566
	private int _nickColorInd;

	// Token: 0x04001D8F RID: 7567
	public float liveTime;

	// Token: 0x04001D90 RID: 7568
	public float timerShowUp;

	// Token: 0x04001D91 RID: 7569
	public float timerShowLeft;

	// Token: 0x04001D92 RID: 7570
	public float timerShowDown;

	// Token: 0x04001D93 RID: 7571
	public float timerShowRight;

	// Token: 0x04001D94 RID: 7572
	public string myIp = string.Empty;

	// Token: 0x04001D95 RID: 7573
	public TrainingController trainigController;

	// Token: 0x04001D96 RID: 7574
	public bool isKilled;

	// Token: 0x04001D97 RID: 7575
	public bool theEnd;

	// Token: 0x04001D98 RID: 7576
	public string nickPobeditel;

	// Token: 0x04001D99 RID: 7577
	public Texture hitTexture;

	// Token: 0x04001D9A RID: 7578
	public Texture poisonTexture;

	// Token: 0x04001D9B RID: 7579
	public Texture _skin;

	// Token: 0x04001D9C RID: 7580
	public float showNoInetTimer = 5f;

	// Token: 0x04001D9D RID: 7581
	private SaltedInt _killCount = new SaltedInt(428452539);

	// Token: 0x04001D9E RID: 7582
	private float _timeWhenPurchShown;

	// Token: 0x04001D9F RID: 7583
	private bool inAppOpenedFromPause;

	// Token: 0x04001DA0 RID: 7584
	public bool isMulti;

	// Token: 0x04001DA1 RID: 7585
	public bool isInet;

	// Token: 0x04001DA2 RID: 7586
	public bool isMine;

	// Token: 0x04001DA3 RID: 7587
	public bool isCompany;

	// Token: 0x04001DA4 RID: 7588
	public bool isCOOP;

	// Token: 0x04001DA5 RID: 7589
	private ExperienceController expController;

	// Token: 0x04001DA6 RID: 7590
	private float inGameTime;

	// Token: 0x04001DA7 RID: 7591
	public int multiKill;

	// Token: 0x04001DA8 RID: 7592
	private HungerGameController hungerGameController;

	// Token: 0x04001DA9 RID: 7593
	private bool isHunger;

	// Token: 0x04001DAA RID: 7594
	public static float maxTimerShowMultyKill = 3f;

	// Token: 0x04001DAB RID: 7595
	public FlagController flag1;

	// Token: 0x04001DAC RID: 7596
	public FlagController flag2;

	// Token: 0x04001DAD RID: 7597
	public FlagController myFlag;

	// Token: 0x04001DAE RID: 7598
	public FlagController enemyFlag;

	// Token: 0x04001DAF RID: 7599
	private GameObject rocketToLaunch;

	// Token: 0x04001DB0 RID: 7600
	public bool isStartAngel;

	// Token: 0x04001DB1 RID: 7601
	public float maxTimerImmortality = 3f;

	// Token: 0x04001DB2 RID: 7602
	private bool _isImmortalyVal = true;

	// Token: 0x04001DB3 RID: 7603
	private float timerImmortality = 3f;

	// Token: 0x04001DB4 RID: 7604
	private float timerImmortalityForAlpha = 3f;

	// Token: 0x04001DB5 RID: 7605
	private readonly KillerInfo _killerInfo = new KillerInfo();

	// Token: 0x04001DB6 RID: 7606
	private readonly List<NetworkViewID> myKillAssistsLocal = new List<NetworkViewID>();

	// Token: 0x04001DB7 RID: 7607
	private bool showGrenadeHint = true;

	// Token: 0x04001DB8 RID: 7608
	private bool showZoomHint = true;

	// Token: 0x04001DB9 RID: 7609
	private bool showChangeWeaponHint = true;

	// Token: 0x04001DBA RID: 7610
	private int respawnCountForTraining;

	// Token: 0x04001DBB RID: 7611
	[NonSerialized]
	public string currentWeaponForKillCam;

	// Token: 0x04001DBC RID: 7612
	[NonSerialized]
	public int turretUpgrade;

	// Token: 0x04001DBD RID: 7613
	private bool _weaponPopularityCacheIsDirty;

	// Token: 0x04001DBE RID: 7614
	private int counterPetSerial;

	// Token: 0x04001DBF RID: 7615
	private int[] counterSerials = new int[8];

	// Token: 0x04001DC0 RID: 7616
	private float _curBaseArmor;

	// Token: 0x04001DC1 RID: 7617
	public int AmmoBoxWidth = 100;

	// Token: 0x04001DC2 RID: 7618
	public int AmmoBoxHeight = 100;

	// Token: 0x04001DC3 RID: 7619
	public int AmmoBoxOffset = 10;

	// Token: 0x04001DC4 RID: 7620
	public int ScoreBoxWidth = 100;

	// Token: 0x04001DC5 RID: 7621
	public int ScoreBoxHeight = 100;

	// Token: 0x04001DC6 RID: 7622
	public int ScoreBoxOffset = 10;

	// Token: 0x04001DC7 RID: 7623
	public float[] timerShow = new float[]
	{
		-1f,
		-1f,
		-1f
	};

	// Token: 0x04001DC8 RID: 7624
	public AudioClip deadPlayerSound;

	// Token: 0x04001DC9 RID: 7625
	public AudioClip damagePlayerSound;

	// Token: 0x04001DCA RID: 7626
	public AudioClip damageArmorPlayerSound;

	// Token: 0x04001DCB RID: 7627
	private float GunFlashLifetime;

	// Token: 0x04001DCC RID: 7628
	public GameObject[] zoneCreatePlayer;

	// Token: 0x04001DCD RID: 7629
	public GUIStyle ScoreBox;

	// Token: 0x04001DCE RID: 7630
	public GUIStyle AmmoBox;

	// Token: 0x04001DCF RID: 7631
	private float mySens;

	// Token: 0x04001DD0 RID: 7632
	public GUIStyle sliderSensStyle;

	// Token: 0x04001DD1 RID: 7633
	public GUIStyle thumbSensStyle;

	// Token: 0x04001DD2 RID: 7634
	private GameObject damage;

	// Token: 0x04001DD3 RID: 7635
	private bool damageShown;

	// Token: 0x04001DD4 RID: 7636
	private Pauser _pauser;

	// Token: 0x04001DD5 RID: 7637
	private bool _backWasPressed;

	// Token: 0x04001DD6 RID: 7638
	public GameObject _player;

	// Token: 0x04001DD7 RID: 7639
	public GameObject bulletPrefab;

	// Token: 0x04001DD8 RID: 7640
	public GameObject bulletPrefabRed;

	// Token: 0x04001DD9 RID: 7641
	public GameObject bulletPrefabFor252;

	// Token: 0x04001DDA RID: 7642
	public GameObject _bulletSpawnPoint;

	// Token: 0x04001DDB RID: 7643
	private GameObject _inAppGameObject;

	// Token: 0x04001DDC RID: 7644
	public StoreKitEventListener _listener;

	// Token: 0x04001DDD RID: 7645
	public GUIStyle puliInApp;

	// Token: 0x04001DDE RID: 7646
	public InGameGUI inGameGUI;

	// Token: 0x04001DDF RID: 7647
	private Dictionary<string, Action<string>> _actionsForPurchasedItems = new Dictionary<string, Action<string>>();

	// Token: 0x04001DE0 RID: 7648
	public GUIStyle crystalSwordInapp;

	// Token: 0x04001DE1 RID: 7649
	public GUIStyle elixirInapp;

	// Token: 0x04001DE2 RID: 7650
	public GUIStyle pulemetInApp;

	// Token: 0x04001DE3 RID: 7651
	public bool _isInappWinOpen;

	// Token: 0x04001DE4 RID: 7652
	private WeaponManager ___weaponManager;

	// Token: 0x04001DE5 RID: 7653
	public GUIStyle armorStyle;

	// Token: 0x04001DE6 RID: 7654
	private SaltedInt _countKillsCommandBlue = new SaltedInt(180068360);

	// Token: 0x04001DE7 RID: 7655
	private SaltedInt _countKillsCommandRed = new SaltedInt(180068361);

	// Token: 0x04001DE8 RID: 7656
	private bool canReceiveSwipes = true;

	// Token: 0x04001DE9 RID: 7657
	public float slideMagnitudeX;

	// Token: 0x04001DEA RID: 7658
	public float slideMagnitudeY;

	// Token: 0x04001DEB RID: 7659
	public AudioClip ChangeWeaponClip;

	// Token: 0x04001DEC RID: 7660
	public AudioClip ChangeGrenadeClip;

	// Token: 0x04001DED RID: 7661
	public AudioClip WeaponBonusClip;

	// Token: 0x04001DEE RID: 7662
	public PhotonView photonView;

	// Token: 0x04001DEF RID: 7663
	public AudioClip clickShop;

	// Token: 0x04001DF0 RID: 7664
	public List<Player_move_c.MessageChat> messages = new List<Player_move_c.MessageChat>();

	// Token: 0x04001DF1 RID: 7665
	public bool isSurvival;

	// Token: 0x04001DF2 RID: 7666
	public string myTableId;

	// Token: 0x04001DF3 RID: 7667
	private int oldKilledPlayerCharactersCount;

	// Token: 0x04001DF4 RID: 7668
	public Material _bodyMaterial;

	// Token: 0x04001DF5 RID: 7669
	public Material _mechMaterial;

	// Token: 0x04001DF6 RID: 7670
	public Material _bearMaterial;

	// Token: 0x04001DF7 RID: 7671
	public Material curMainSelect;

	// Token: 0x04001DF8 RID: 7672
	public GameObject jetPackPoint;

	// Token: 0x04001DF9 RID: 7673
	public GameObject wingsPoint;

	// Token: 0x04001DFA RID: 7674
	public GameObject wingsPointBear;

	// Token: 0x04001DFB RID: 7675
	public Animation wingsAnimation;

	// Token: 0x04001DFC RID: 7676
	public Animation wingsBearAnimation;

	// Token: 0x04001DFD RID: 7677
	private bool isPlayerFlying;

	// Token: 0x04001DFE RID: 7678
	public ParticleSystem[] jetPackParticle;

	// Token: 0x04001DFF RID: 7679
	public GameObject jetPackSound;

	// Token: 0x04001E00 RID: 7680
	public AudioSource wingsSound;

	// Token: 0x04001E01 RID: 7681
	private bool jetpackEnabled;

	// Token: 0x04001E02 RID: 7682
	public string turretGadgetPrefabName;

	// Token: 0x04001E03 RID: 7683
	private int indexWeapon;

	// Token: 0x04001E04 RID: 7684
	private bool shouldSetMaxAmmoWeapon;

	// Token: 0x04001E05 RID: 7685
	private bool _changingWeapon;

	// Token: 0x04001E06 RID: 7686
	private string _sendingNameWeapon;

	// Token: 0x04001E07 RID: 7687
	private string _sendingAlternativeNameWeapon;

	// Token: 0x04001E08 RID: 7688
	private string _sendingSkinId;

	// Token: 0x04001E09 RID: 7689
	private IDisposable _backSubscription;

	// Token: 0x04001E0A RID: 7690
	private bool BonusEffectForArmorWorksInThisMatch;

	// Token: 0x04001E0B RID: 7691
	private bool ArmorBonusGiven;

	// Token: 0x04001E0C RID: 7692
	private bool isDaterRegim;

	// Token: 0x04001E0D RID: 7693
	private static KeyValuePair<int, string> _countdownMemo = new KeyValuePair<int, string>(0, "0:00");

	// Token: 0x04001E0E RID: 7694
	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();

	// Token: 0x04001E0F RID: 7695
	private bool pausedRating;

	// Token: 0x04001E10 RID: 7696
	public float _currentReloadAnimationSpeed = 1f;

	// Token: 0x04001E11 RID: 7697
	private int countHouseKeeperEvent;

	// Token: 0x04001E12 RID: 7698
	private bool isJumpPresedOld;

	// Token: 0x04001E13 RID: 7699
	private int countFixedUpdateForResetLabel;

	// Token: 0x04001E14 RID: 7700
	private float _chanceToIgnoreHeadshot;

	// Token: 0x04001E15 RID: 7701
	private float _protectionShieldValue = 1f;

	// Token: 0x04001E16 RID: 7702
	private bool isShieldActivated;

	// Token: 0x04001E17 RID: 7703
	private bool roomTierInitialized;

	// Token: 0x04001E18 RID: 7704
	private int roomTier;

	// Token: 0x04001E19 RID: 7705
	private bool _escapePressed;

	// Token: 0x04001E1A RID: 7706
	private float oldAlphaImmortality = -1f;

	// Token: 0x04001E1B RID: 7707
	private float _timeOfSlowdown;

	// Token: 0x04001E1C RID: 7708
	private bool isDetectCh;

	// Token: 0x04001E1D RID: 7709
	private Material oldWeaponHandMaterial;

	// Token: 0x04001E1E RID: 7710
	private Material[] oldWeaponMaterials;

	// Token: 0x04001E1F RID: 7711
	private SaltedInt numberOfGrenadesOnStart = new SaltedInt(45853678);

	// Token: 0x04001E20 RID: 7712
	private SaltedInt numberOfGrenades = new SaltedInt(29076718);

	// Token: 0x04001E21 RID: 7713
	private GameObject myPetValue;

	// Token: 0x04001E22 RID: 7714
	private List<Player_move_c.ActivePlayerEffect> playerEffects = new List<Player_move_c.ActivePlayerEffect>(3);

	// Token: 0x04001E23 RID: 7715
	private GameObject burningEffect;

	// Token: 0x04001E24 RID: 7716
	private GameObject bleedingEffect;

	// Token: 0x04001E25 RID: 7717
	private List<Player_move_c.GadgetEffectParams> activatedGadgetEffects = new List<Player_move_c.GadgetEffectParams>(3);

	// Token: 0x04001E26 RID: 7718
	private Vector3[] timePositions = new Vector3[10];

	// Token: 0x04001E27 RID: 7719
	private Quaternion[] timeRotations = new Quaternion[10];

	// Token: 0x04001E28 RID: 7720
	private Quaternion[] timeGunRotations = new Quaternion[10];

	// Token: 0x04001E29 RID: 7721
	private bool firstPositionsReached;

	// Token: 0x04001E2A RID: 7722
	private int currentTimeIndex;

	// Token: 0x04001E2B RID: 7723
	private float nextTimeAdd;

	// Token: 0x04001E2C RID: 7724
	private float disablerRadius = 35f;

	// Token: 0x04001E2D RID: 7725
	private float mushroomRadius = 6f;

	// Token: 0x04001E2E RID: 7726
	private float mushroomBurnTime = 0.5f;

	// Token: 0x04001E2F RID: 7727
	private int mushroomBurnCount = 6;

	// Token: 0x04001E30 RID: 7728
	private float mushroomBurnDamage = 0.04f;

	// Token: 0x04001E31 RID: 7729
	public AudioClip mushroomActivationSound;

	// Token: 0x04001E32 RID: 7730
	public AudioClip mushroomShotSound;

	// Token: 0x04001E33 RID: 7731
	private float drumSupportRadius = 10f;

	// Token: 0x04001E34 RID: 7732
	private bool drumActive;

	// Token: 0x04001E35 RID: 7733
	private float drumDamageMultiplier;

	// Token: 0x04001E36 RID: 7734
	public AudioClip timeWatchSound;

	// Token: 0x04001E37 RID: 7735
	public AudioClip medkitSound;

	// Token: 0x04001E38 RID: 7736
	public AudioClip reflectorOnSound;

	// Token: 0x04001E39 RID: 7737
	public AudioClip reflectorOffSound;

	// Token: 0x04001E3A RID: 7738
	public AudioClip resurrectionSound;

	// Token: 0x04001E3B RID: 7739
	public GameObject reflectorPulseSound;

	// Token: 0x04001E3C RID: 7740
	public AudioClip dragonWhistleSound;

	// Token: 0x04001E3D RID: 7741
	public AudioClip disablerActiveSound;

	// Token: 0x04001E3E RID: 7742
	public AudioClip disablerEffectSound;

	// Token: 0x04001E3F RID: 7743
	public AudioClip drumActiveSound;

	// Token: 0x04001E40 RID: 7744
	public GameObject drumLoopSound;

	// Token: 0x04001E41 RID: 7745
	public AudioClip petBoosterActiveSound;

	// Token: 0x04001E42 RID: 7746
	public GameObject reflectorParticles;

	// Token: 0x04001E43 RID: 7747
	public GameObject resurrectionEffect;

	// Token: 0x04001E44 RID: 7748
	public GameObject timeTravelEffect;

	// Token: 0x04001E45 RID: 7749
	public bool wasResurrected;

	// Token: 0x04001E46 RID: 7750
	public bool deadInCollider;

	// Token: 0x04001E47 RID: 7751
	public Vector3 resurrectionPosition;

	// Token: 0x04001E48 RID: 7752
	public PlayerMechBody mechBodyScript;

	// Token: 0x04001E49 RID: 7753
	public PlayerMechBody demonBodyScript;

	// Token: 0x04001E4A RID: 7754
	[HideInInspector]
	public PlayerMechBody currentMech;

	// Token: 0x04001E4B RID: 7755
	[HideInInspector]
	public Gadget daterLikeGadget = new DaterLikeGadget();

	// Token: 0x04001E4C RID: 7756
	public bool gadgetsDisabled;

	// Token: 0x04001E4D RID: 7757
	public Action GadgetsOnMechKill;

	// Token: 0x04001E4E RID: 7758
	public Action GadgetsOnPetKill;

	// Token: 0x04001E4F RID: 7759
	public bool gadgetWasPreused;

	// Token: 0x04001E50 RID: 7760
	private bool _isTimeJump;

	// Token: 0x04001E51 RID: 7761
	public float timeBuyHealth = -10000f;

	// Token: 0x04001E52 RID: 7762
	private SaltedFloat _curHealthSalt = new SaltedFloat();

	// Token: 0x04001E53 RID: 7763
	public float synhHealth = -10000000f;

	// Token: 0x04001E54 RID: 7764
	public double synchTimeHealth;

	// Token: 0x04001E55 RID: 7765
	public bool isSuicided;

	// Token: 0x04001E56 RID: 7766
	public bool killedInMatch;

	// Token: 0x04001E57 RID: 7767
	private float damageBuff = 1f;

	// Token: 0x04001E58 RID: 7768
	private float protectionBuff = 1f;

	// Token: 0x04001E59 RID: 7769
	private bool getLocalHurt;

	// Token: 0x04001E5A RID: 7770
	private bool timeSettedAfterRegenerationSwitchedOn;

	// Token: 0x04001E5B RID: 7771
	private bool isRaiderMyPoint;

	// Token: 0x04001E5C RID: 7772
	private int countMySpotEvent;

	// Token: 0x04001E5D RID: 7773
	public Vector3 pointAutoAim;

	// Token: 0x04001E5E RID: 7774
	private float timerUpdatePointAutoAi = -1f;

	// Token: 0x04001E5F RID: 7775
	private Ray rayAutoAim;

	// Token: 0x04001E60 RID: 7776
	private List<Player_move_c.PoisonTarget> poisonTargets = new List<Player_move_c.PoisonTarget>();

	// Token: 0x04001E61 RID: 7777
	private bool _isShootingVal;

	// Token: 0x04001E62 RID: 7778
	public bool isShootingLoop;

	// Token: 0x04001E63 RID: 7779
	public float chargeValue;

	// Token: 0x04001E64 RID: 7780
	private float lastChargeValue;

	// Token: 0x04001E65 RID: 7781
	private CancellationTokenSource ctsShootLoop = new CancellationTokenSource();

	// Token: 0x04001E66 RID: 7782
	private int _countShootInBurst;

	// Token: 0x04001E67 RID: 7783
	private float _timerDelayInShootingBurst = -1f;

	// Token: 0x04001E68 RID: 7784
	private int ammoInClipBeforeCharge;

	// Token: 0x04001E69 RID: 7785
	private int lastChargeWeaponIndex;

	// Token: 0x04001E6A RID: 7786
	private float nextChargeConsumeTime = -1f;

	// Token: 0x04001E6B RID: 7787
	private bool firstChargePlay;

	// Token: 0x04001E6C RID: 7788
	private bool fullyCharged;

	// Token: 0x04001E6D RID: 7789
	private float lastShotTime;

	// Token: 0x04001E6E RID: 7790
	private float nextFrostHitTime;

	// Token: 0x04001E6F RID: 7791
	private float timeGrenadePress;

	// Token: 0x04001E70 RID: 7792
	public bool isGrenadePress;

	// Token: 0x04001E71 RID: 7793
	private bool playChargeLoopAnim;

	// Token: 0x04001E72 RID: 7794
	private FreezerRay[] freezeRays = new FreezerRay[2];

	// Token: 0x02000485 RID: 1157
	public enum TypeBonuses
	{
		// Token: 0x04001E8D RID: 7821
		Ammo,
		// Token: 0x04001E8E RID: 7822
		Health,
		// Token: 0x04001E8F RID: 7823
		Armor,
		// Token: 0x04001E90 RID: 7824
		Grenade
	}

	// Token: 0x02000486 RID: 1158
	public enum TypeKills
	{
		// Token: 0x04001E92 RID: 7826
		none,
		// Token: 0x04001E93 RID: 7827
		himself,
		// Token: 0x04001E94 RID: 7828
		headshot,
		// Token: 0x04001E95 RID: 7829
		explosion,
		// Token: 0x04001E96 RID: 7830
		zoomingshot,
		// Token: 0x04001E97 RID: 7831
		flag,
		// Token: 0x04001E98 RID: 7832
		grenade,
		// Token: 0x04001E99 RID: 7833
		grenade_hell,
		// Token: 0x04001E9A RID: 7834
		turret,
		// Token: 0x04001E9B RID: 7835
		killTurret,
		// Token: 0x04001E9C RID: 7836
		mech,
		// Token: 0x04001E9D RID: 7837
		like,
		// Token: 0x04001E9E RID: 7838
		poison,
		// Token: 0x04001E9F RID: 7839
		reflector,
		// Token: 0x04001EA0 RID: 7840
		burning,
		// Token: 0x04001EA1 RID: 7841
		pet,
		// Token: 0x04001EA2 RID: 7842
		mob,
		// Token: 0x04001EA3 RID: 7843
		bleeding,
		// Token: 0x04001EA4 RID: 7844
		critical
	}

	// Token: 0x02000487 RID: 1159
	public struct SystemMessage
	{
		// Token: 0x060029C5 RID: 10693 RVA: 0x000DCBAC File Offset: 0x000DADAC
		public SystemMessage(string nick1, string message2, string nick2, string message, Color textColor)
		{
			this.nick1 = nick1;
			this.message2 = message2;
			this.nick2 = nick2;
			this.message = message;
			this.textColor = textColor;
		}

		// Token: 0x04001EA5 RID: 7845
		public string nick1;

		// Token: 0x04001EA6 RID: 7846
		public string message2;

		// Token: 0x04001EA7 RID: 7847
		public string nick2;

		// Token: 0x04001EA8 RID: 7848
		public string message;

		// Token: 0x04001EA9 RID: 7849
		public Color textColor;
	}

	// Token: 0x02000488 RID: 1160
	public struct MessageChat
	{
		// Token: 0x04001EAA RID: 7850
		public string text;

		// Token: 0x04001EAB RID: 7851
		public float time;

		// Token: 0x04001EAC RID: 7852
		public int ID;

		// Token: 0x04001EAD RID: 7853
		public int command;

		// Token: 0x04001EAE RID: 7854
		public bool isClanMessage;

		// Token: 0x04001EAF RID: 7855
		public Texture clanLogo;

		// Token: 0x04001EB0 RID: 7856
		public string clanID;

		// Token: 0x04001EB1 RID: 7857
		public string clanName;

		// Token: 0x04001EB2 RID: 7858
		public NetworkViewID IDLocal;

		// Token: 0x04001EB3 RID: 7859
		public string iconName;
	}

	// Token: 0x02000489 RID: 1161
	public enum PlayerEffect
	{
		// Token: 0x04001EB5 RID: 7861
		none,
		// Token: 0x04001EB6 RID: 7862
		burning,
		// Token: 0x04001EB7 RID: 7863
		fireMushroom,
		// Token: 0x04001EB8 RID: 7864
		disabler,
		// Token: 0x04001EB9 RID: 7865
		blackMark,
		// Token: 0x04001EBA RID: 7866
		dragon,
		// Token: 0x04001EBB RID: 7867
		lightningEnemies,
		// Token: 0x04001EBC RID: 7868
		disablerEffect,
		// Token: 0x04001EBD RID: 7869
		resurrection,
		// Token: 0x04001EBE RID: 7870
		attrackPlayer,
		// Token: 0x04001EBF RID: 7871
		timeTravel,
		// Token: 0x04001EC0 RID: 7872
		lightningSelf,
		// Token: 0x04001EC1 RID: 7873
		rocketFly,
		// Token: 0x04001EC2 RID: 7874
		clearPoisons
	}

	// Token: 0x0200048A RID: 1162
	public struct ActivePlayerEffect
	{
		// Token: 0x060029C6 RID: 10694 RVA: 0x000DCBD4 File Offset: 0x000DADD4
		public ActivePlayerEffect(Player_move_c.PlayerEffect effect, float time)
		{
			this.effect = effect;
			this.lifeTime = Time.time + time;
		}

		// Token: 0x060029C7 RID: 10695 RVA: 0x000DCBEC File Offset: 0x000DADEC
		public Player_move_c.ActivePlayerEffect UpdateTime(float time)
		{
			return new Player_move_c.ActivePlayerEffect(this.effect, time);
		}

		// Token: 0x04001EC3 RID: 7875
		public Player_move_c.PlayerEffect effect;

		// Token: 0x04001EC4 RID: 7876
		public float lifeTime;
	}

	// Token: 0x0200048B RID: 1163
	public enum GadgetEffect
	{
		// Token: 0x04001EC6 RID: 7878
		reflector,
		// Token: 0x04001EC7 RID: 7879
		mech,
		// Token: 0x04001EC8 RID: 7880
		invisible,
		// Token: 0x04001EC9 RID: 7881
		jetpack,
		// Token: 0x04001ECA RID: 7882
		demon,
		// Token: 0x04001ECB RID: 7883
		disabler,
		// Token: 0x04001ECC RID: 7884
		drumSupport,
		// Token: 0x04001ECD RID: 7885
		petAdrenaline
	}

	// Token: 0x0200048C RID: 1164
	public struct GadgetEffectParams
	{
		// Token: 0x060029C8 RID: 10696 RVA: 0x000DCBFC File Offset: 0x000DADFC
		public GadgetEffectParams(Player_move_c.GadgetEffect effect, string gadgetID)
		{
			this.effect = effect;
			this.gadgetID = gadgetID;
		}

		// Token: 0x04001ECE RID: 7886
		public Player_move_c.GadgetEffect effect;

		// Token: 0x04001ECF RID: 7887
		public string gadgetID;
	}

	// Token: 0x0200048D RID: 1165
	public enum PoisonType
	{
		// Token: 0x04001ED1 RID: 7889
		None,
		// Token: 0x04001ED2 RID: 7890
		Toxic,
		// Token: 0x04001ED3 RID: 7891
		Burn,
		// Token: 0x04001ED4 RID: 7892
		Bleeding
	}

	// Token: 0x0200048E RID: 1166
	public struct PoisonParameters
	{
		// Token: 0x060029C9 RID: 10697 RVA: 0x000DCC0C File Offset: 0x000DAE0C
		public PoisonParameters(Player_move_c.PoisonType poisonType, float dmPercent, float damageMultiplayer, float poisonTime, int poisonCount, string weaponName, WeaponSounds.TypeDead typeDead)
		{
			this.poisonType = poisonType;
			this.multiplayerDamage = damageMultiplayer * dmPercent;
			this.poisonTime = poisonTime;
			this.poisonCount = poisonCount;
			this.weaponName = weaponName;
			this.typeDead = typeDead;
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x000DCC4C File Offset: 0x000DAE4C
		public PoisonParameters(float damageMultiplayer, WeaponSounds weapon)
		{
			this.poisonType = weapon.poisonType;
			this.multiplayerDamage = damageMultiplayer * weapon.poisonDamageMultiplier;
			this.poisonTime = weapon.poisonTime;
			this.poisonCount = weapon.poisonCount;
			this.weaponName = weapon.gameObject.name.Replace("(Clone)", string.Empty);
			this.typeDead = weapon.typeDead;
		}

		// Token: 0x04001ED5 RID: 7893
		public Player_move_c.PoisonType poisonType;

		// Token: 0x04001ED6 RID: 7894
		public float multiplayerDamage;

		// Token: 0x04001ED7 RID: 7895
		public float poisonTime;

		// Token: 0x04001ED8 RID: 7896
		public int poisonCount;

		// Token: 0x04001ED9 RID: 7897
		public string weaponName;

		// Token: 0x04001EDA RID: 7898
		public WeaponSounds.TypeDead typeDead;
	}

	// Token: 0x0200048F RID: 1167
	private class PoisonTarget
	{
		// Token: 0x060029CB RID: 10699 RVA: 0x000DCCB8 File Offset: 0x000DAEB8
		public PoisonTarget(IDamageable target, Player_move_c.PoisonParameters poison)
		{
			this.target = target;
			this.hitCount = poison.poisonCount;
			this.param = poison;
			this.nextHitTime = Time.time;
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000DCCF4 File Offset: 0x000DAEF4
		public void UpdatePoison(Player_move_c.PoisonParameters poison)
		{
			this.hitCount = poison.poisonCount;
			this.param = poison;
		}

		// Token: 0x04001EDB RID: 7899
		public IDamageable target;

		// Token: 0x04001EDC RID: 7900
		public int hitCount;

		// Token: 0x04001EDD RID: 7901
		public float nextHitTime;

		// Token: 0x04001EDE RID: 7902
		public Player_move_c.PoisonParameters param;
	}

	// Token: 0x02000490 RID: 1168
	public struct RayHitsInfo
	{
		// Token: 0x04001EDF RID: 7903
		public RaycastHit[] hits;

		// Token: 0x04001EE0 RID: 7904
		public bool obstacleFound;

		// Token: 0x04001EE1 RID: 7905
		public float lenRay;

		// Token: 0x04001EE2 RID: 7906
		public Ray rayReflect;
	}

	// Token: 0x02000917 RID: 2327
	// (Invoke) Token: 0x060050FC RID: 20732
	public delegate void OnMessagesUpdate();
}
