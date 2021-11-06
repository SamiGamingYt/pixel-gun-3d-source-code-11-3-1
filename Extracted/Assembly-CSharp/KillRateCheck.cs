using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class KillRateCheck
{
	// Token: 0x060019A0 RID: 6560 RVA: 0x00066590 File Offset: 0x00064790
	public KillRateCheck()
	{
		this.LoadParameters();
		string @string = Storager.getString("killRateValues", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("roundCount"))
			{
				this.roundCount = Convert.ToInt32(dictionary["roundCount"]);
			}
			if (dictionary.ContainsKey("allRoundsCount"))
			{
				this.allRoundsCount = Convert.ToInt32(dictionary["allRoundsCount"]);
			}
			if (dictionary.ContainsKey("isFirstRounds"))
			{
				this.isFirstRounds = (Convert.ToInt32(dictionary["isFirstRounds"]) == 1);
			}
			if (dictionary.ContainsKey("status"))
			{
				this.status = (KillRateCheck.CheckStatus)Convert.ToInt32(dictionary["status"]);
			}
			if (dictionary.ContainsKey("killRateVal"))
			{
				this.killRateVal = Convert.ToSingle(dictionary["killRateVal"]);
			}
			if (dictionary.ContainsKey("nextBuffCheck"))
			{
				this.nextBuffCheck = Convert.ToSingle(dictionary["nextBuffCheck"]);
			}
			if (dictionary.ContainsKey("StarterBuff"))
			{
				this.starterBuff = Convert.ToInt32(dictionary["StarterBuff"]);
			}
			this.CheckForBuff();
		}
		if (this.status == KillRateCheck.CheckStatus.None)
		{
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) != 1)
			{
				this.status = KillRateCheck.CheckStatus.StarterBuff;
				this.starterBuff = 2;
				this.isFirstRounds = true;
			}
			else
			{
				this.status = KillRateCheck.CheckStatus.Starter;
			}
			this.SaveValues();
			this.CheckForBuff();
		}
		string string2 = Storager.getString("LastKillRates", false);
		List<object> list = Json.Deserialize(string2) as List<object>;
		if (list != null && list.Count == 2)
		{
			this.kills = (from o in list[0] as List<object>
			select Convert.ToInt32(o)).ToArray<int>();
			this.deaths = (from o in list[1] as List<object>
			select Convert.ToInt32(o)).ToArray<int>();
		}
	}

	// Token: 0x170004A6 RID: 1190
	// (get) Token: 0x060019A1 RID: 6561 RVA: 0x00066880 File Offset: 0x00064A80
	public static KillRateCheck instance
	{
		get
		{
			if (KillRateCheck._instance == null)
			{
				KillRateCheck._instance = new KillRateCheck();
			}
			if (!KillRateCheck._instance.configLoaded && Time.time > KillRateCheck.lastConfigCheck)
			{
				KillRateCheck.lastConfigCheck = Time.time + 20f;
				KillRateCheck._instance.LoadParameters();
				Debug.LogWarning("KillRateCheck config not loaded: try loading");
			}
			return KillRateCheck._instance;
		}
	}

	// Token: 0x170004A7 RID: 1191
	// (get) Token: 0x060019A2 RID: 6562 RVA: 0x000668E8 File Offset: 0x00064AE8
	private KillRateCheck.ParamByTier tierParam
	{
		get
		{
			return this.paramsByTier[ExpController.Instance.OurTier];
		}
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000668FC File Offset: 0x00064AFC
	public void SetActive(bool isAcitve, bool roundMore30Sec)
	{
		this.active = (isAcitve && this.activeFromServer && roundMore30Sec && this.configLoaded);
		this.writeKill = (isAcitve && roundMore30Sec);
		this.calcbuff = (roundMore30Sec && this.configLoaded);
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x00066954 File Offset: 0x00064B54
	private void WriteDefaultParameters()
	{
		this.active = false;
		if (this.paramsByTier == null)
		{
			this.paramsByTier = new KillRateCheck.ParamByTier[6];
			for (int i = 0; i < this.paramsByTier.Length; i++)
			{
				this.paramsByTier[i] = new KillRateCheck.ParamByTier();
			}
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000669A8 File Offset: 0x00064BA8
	public void OnGetProgress()
	{
		if (this.status == KillRateCheck.CheckStatus.StarterBuff)
		{
			this.status = KillRateCheck.CheckStatus.Starter;
			this.buffEnabled = false;
			this.damageBuff = 1f;
			this.healthBuff = 1f;
		}
		this.isFirstRounds = false;
		this.SaveValues();
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000669E8 File Offset: 0x00064BE8
	private void LoadParameters()
	{
		if (!Storager.hasKey("BuffsParam"))
		{
			this.WriteDefaultParameters();
			return;
		}
		string @string = Storager.getString("BuffsParam", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("killRate"))
		{
			this.WriteDefaultParameters();
			return;
		}
		try
		{
			Dictionary<string, object> dictionary2 = dictionary["killRate"] as Dictionary<string, object>;
			this.activeFromServer = (Convert.ToInt32(dictionary2["active"]) == 1);
			this.startRounds = Convert.ToInt32(dictionary2["startRounds"]);
			this.roundsForCheckL1 = Convert.ToInt32(dictionary2["roundsForCheckL1"]);
			this.roundsForCheckL2 = Convert.ToInt32(dictionary2["roundsForCheckL2"]);
			this.roundsForL2Buff = Convert.ToInt32(dictionary2["roundsForL2Buff"]);
			this.starterHighKillrate = Convert.ToSingle(dictionary2["starterHighKillrate"]);
			this.roundsForGunLow = Convert.ToInt32(dictionary2["roundsForGunLow"]);
			this.roundsForGunMiddle = Convert.ToInt32(dictionary2["roundsForGunMiddle"]);
			this.roundsForGunHigh = Convert.ToInt32(dictionary2["roundsForGunHigh"]);
			this.killRateLength = Convert.ToInt32(dictionary2["killRateLength"]);
			this.timeForDiscount = Convert.ToSingle(dictionary2["timeForDiscount"]);
			this.discountValue = Convert.ToInt32(dictionary2["discountValue"]);
			this.debuffL1 = Convert.ToSingle(dictionary2["debuffL1"]);
			this.debuffL2 = Convert.ToSingle(dictionary2["debuffL2"]);
			this.debuffRoundsL1 = Convert.ToInt32(dictionary2["debuffRoundsL1"]);
			this.debuffRoundsL2 = Convert.ToInt32(dictionary2["debuffRoundsL2"]);
			this.debuffVal = Convert.ToSingle(dictionary2["killrateForDebuff"]);
			this.debuffRoundsAfterGun = Convert.ToInt32(dictionary2["debuffRoundsAfterGun"]);
			this.debuffAfterGun = Convert.ToSingle(dictionary2["debuffAfterGun"]);
			List<object> list = dictionary2["tierParams"] as List<object>;
			if (list == null)
			{
				Debug.LogWarning("Error Deserialize JSON: tierParams");
				return;
			}
			this.paramsByTier = (from e in list
			select new KillRateCheck.ParamByTier(e as Dictionary<string, object>)).ToArray<KillRateCheck.ParamByTier>();
		}
		catch (Exception arg)
		{
			Debug.LogWarning("Error Deserialize JSON: BuffsParam: " + arg);
			this.WriteDefaultParameters();
			return;
		}
		this.configLoaded = true;
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00066C9C File Offset: 0x00064E9C
	public void IncrementKills()
	{
		if (!this.active)
		{
			return;
		}
		this.killCount++;
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x00066CB8 File Offset: 0x00064EB8
	public void IncrementDeath()
	{
		if (!this.active)
		{
			return;
		}
		this.deathCount++;
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x00066CD4 File Offset: 0x00064ED4
	private void WriteKillRate()
	{
		for (int i = this.kills.Length - 2; i >= 0; i--)
		{
			this.kills[i + 1] = this.kills[i];
		}
		this.kills[0] = this.killCount;
		for (int j = this.deaths.Length - 2; j >= 0; j--)
		{
			this.deaths[j + 1] = this.deaths[j];
		}
		this.deaths[0] = this.deathCount;
		this.killCount = 0;
		this.deathCount = 0;
		this.SaveKillRates();
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x00066D6C File Offset: 0x00064F6C
	public float GetKillRate()
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < Mathf.Min(this.killRateLength, this.kills.Length); i++)
		{
			num += this.kills[i];
		}
		for (int j = 0; j < Mathf.Min(this.killRateLength, this.deaths.Length); j++)
		{
			num2 += this.deaths[j];
		}
		if (num2 != 0)
		{
			return (float)num / (float)num2;
		}
		return (float)num;
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x00066DEC File Offset: 0x00064FEC
	private void SaveKillRates()
	{
		Storager.setString("LastKillRates", Json.Serialize(new int[][]
		{
			this.kills,
			this.deaths
		}), false);
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x00066E24 File Offset: 0x00065024
	private void CheckForBuff()
	{
		this.damageBuff = 1f;
		this.healthBuff = 1f;
		this.buffEnabled = false;
		if (this.status == KillRateCheck.CheckStatus.StarterBuff || this.status == KillRateCheck.CheckStatus.StarterBuff2 || this.status == KillRateCheck.CheckStatus.HardPlayer)
		{
			int num = this.starterBuff;
			if (num != 1)
			{
				if (num == 2)
				{
					this.buffEnabled = true;
					this.damageBuff = this.tierParam.buffL1;
					this.healthBuff = this.tierParam.buffL1;
				}
			}
			else
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL2;
				this.healthBuff = this.tierParam.buffL2;
			}
		}
		if (this.active)
		{
			if (this.status == KillRateCheck.CheckStatus.HardDebuff)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffL1;
				this.healthBuff = this.debuffL1;
			}
			else if (this.status == KillRateCheck.CheckStatus.HardDebuff2)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffL2;
				this.healthBuff = this.debuffL2;
			}
			else if (this.status == KillRateCheck.CheckStatus.DebuffAfterGun)
			{
				this.buffEnabled = true;
				this.damageBuff = this.debuffAfterGun;
				this.healthBuff = this.debuffAfterGun;
			}
			else if (this.killRateVal < this.tierParam.lowKillRate)
			{
				if (this.status == KillRateCheck.CheckStatus.GetGun || (this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL1))
				{
					this.buffEnabled = true;
					this.damageBuff = this.tierParam.buffL1;
					this.healthBuff = this.tierParam.buffL1;
				}
				else if (this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL1 + this.tierParam.buffRoundsL2)
				{
					this.buffEnabled = true;
					this.damageBuff = this.tierParam.buffL2;
					this.healthBuff = this.tierParam.buffL2;
				}
			}
			else if (this.killRateVal < this.tierParam.highKillRate && (this.status == KillRateCheck.CheckStatus.GetGun || (this.status == KillRateCheck.CheckStatus.BuyedGun && this.roundCount < this.tierParam.buffRoundsL2)))
			{
				this.buffEnabled = true;
				this.damageBuff = this.tierParam.buffL2;
				this.healthBuff = this.tierParam.buffL2;
			}
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetupBuffParameters(this.damageBuff, this.healthBuff);
		}
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x00067104 File Offset: 0x00065304
	private void GiveWeaponByKillRate(float rateValue)
	{
		if (!this.active)
		{
			return;
		}
		this.giveWeapon = true;
		this.roundCount = 0;
		this.status = KillRateCheck.CheckStatus.WaitGetGun;
		this.killRateVal = rateValue;
		this.SaveValues();
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00067140 File Offset: 0x00065340
	public int GetRoundsForGun()
	{
		if (this.killRateVal < this.tierParam.lowKillRate)
		{
			return this.roundsForGunLow;
		}
		if (this.killRateVal < this.tierParam.highKillRate)
		{
			return this.roundsForGunMiddle;
		}
		return this.roundsForGunHigh;
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x00067190 File Offset: 0x00065390
	private int GetCooldownForGun(float value)
	{
		if (value < this.tierParam.lowKillRate)
		{
			return this.tierParam.cooldownRoundsLow;
		}
		if (value < this.tierParam.highKillRate)
		{
			return this.tierParam.cooldownRoundsMiddle;
		}
		return this.tierParam.cooldownRoundsHigh;
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000671E4 File Offset: 0x000653E4
	public void LogFirstBattlesResult(bool isWinner)
	{
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesResult(this.allRoundsCount, isWinner);
		}
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00067218 File Offset: 0x00065418
	public void CheckKillRate()
	{
		if (this.writeKill)
		{
			this.WriteKillRate();
		}
		float killRate = this.GetKillRate();
		Debug.Log(killRate);
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesKillRate(this.allRoundsCount, killRate);
		}
		this.allRoundsCount++;
		if (this.allRoundsCount == 3)
		{
			AnalyticsStuff.TrySendOnceToAppsFlyer("third_round_complete");
		}
		if (this.allRoundsCount > 9)
		{
			this.isFirstRounds = false;
		}
		switch (this.status)
		{
		case KillRateCheck.CheckStatus.Starter:
			if (!this.calcbuff)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.startRounds)
			{
				if (killRate < this.tierParam.lowKillRate)
				{
					this.GiveWeaponByKillRate(killRate);
				}
				else
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
			}
			break;
		case KillRateCheck.CheckStatus.StarterBuff:
			if (!this.calcbuff)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.roundsForCheckL1 && killRate >= this.tierParam.highKillRate)
			{
				this.roundCount = 0;
				this.starterBuff = 1;
				this.status = KillRateCheck.CheckStatus.HardPlayer;
			}
			else if (this.roundCount >= this.startRounds)
			{
				if (killRate < this.tierParam.lowKillRate)
				{
					this.GiveWeaponByKillRate(killRate);
				}
				else if (killRate < this.tierParam.highKillRate && this.roundCount == this.startRounds)
				{
					this.roundCount = 0;
					this.starterBuff = 1;
					this.status = KillRateCheck.CheckStatus.StarterBuff2;
				}
				else
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
			}
			break;
		case KillRateCheck.CheckStatus.WaitGetGun:
			if (!this.active)
			{
				return;
			}
			this.giveWeapon = true;
			break;
		case KillRateCheck.CheckStatus.BuyedGun:
			if (!this.calcbuff)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.tierParam.buffRoundsL1 + this.tierParam.buffRoundsL2)
			{
				this.roundCount = 0;
				this.status = KillRateCheck.CheckStatus.CoolDown;
			}
			break;
		case KillRateCheck.CheckStatus.CoolDown:
			if (!this.active)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.GetCooldownForGun(killRate))
			{
				if (killRate > this.debuffVal)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.HardDebuff;
				}
				else
				{
					this.GiveWeaponByKillRate(killRate);
				}
			}
			break;
		case KillRateCheck.CheckStatus.StarterBuff2:
			if (!this.calcbuff)
			{
				return;
			}
			this.roundCount++;
			if (killRate < this.tierParam.lowKillRate)
			{
				this.GiveWeaponByKillRate(killRate);
			}
			else if (this.roundCount >= this.roundsForL2Buff)
			{
				this.roundCount = 0;
				this.status = KillRateCheck.CheckStatus.CoolDown;
			}
			break;
		case KillRateCheck.CheckStatus.HardPlayer:
			if (!this.calcbuff)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount == this.roundsForCheckL2)
			{
				if (killRate >= this.tierParam.highKillRate)
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.CoolDown;
				}
				else
				{
					this.roundCount = 0;
					this.status = KillRateCheck.CheckStatus.StarterBuff2;
				}
			}
			break;
		case KillRateCheck.CheckStatus.HardDebuff:
			if (!this.active)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.debuffRoundsL1)
			{
				this.roundCount = 0;
				this.status = KillRateCheck.CheckStatus.HardDebuff2;
			}
			break;
		case KillRateCheck.CheckStatus.HardDebuff2:
			if (!this.active)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.debuffRoundsL2)
			{
				this.roundCount = 0;
				this.GiveWeaponByKillRate(killRate);
			}
			break;
		case KillRateCheck.CheckStatus.DebuffAfterGun:
			if (!this.active)
			{
				return;
			}
			this.roundCount++;
			if (this.roundCount >= this.debuffRoundsAfterGun)
			{
				this.roundCount = 0;
				this.status = KillRateCheck.CheckStatus.CoolDown;
			}
			break;
		}
		this.SaveValues();
		this.CheckForBuff();
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x00067658 File Offset: 0x00065858
	public void SetGetWeapon()
	{
		this.giveWeapon = false;
		if (!this.active)
		{
			return;
		}
		this.roundCount = 0;
		this.status = KillRateCheck.CheckStatus.GetGun;
		this.SaveValues();
		this.CheckForBuff();
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x00067688 File Offset: 0x00065888
	private void SaveValues()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (this.roundCount > 0)
		{
			dictionary["roundCount"] = this.roundCount;
		}
		if (this.allRoundsCount > 0)
		{
			dictionary["allRoundsCount"] = this.allRoundsCount;
		}
		if (this.isFirstRounds)
		{
			dictionary["isFirstRounds"] = 1;
		}
		if (this.killRateVal > 0f)
		{
			dictionary["killRateVal"] = this.killRateVal;
		}
		if (this.nextBuffCheck > 0f)
		{
			dictionary["nextBuffCheck"] = this.nextBuffCheck;
		}
		if (this.status == KillRateCheck.CheckStatus.StarterBuff && this.starterBuff > 0)
		{
			dictionary["StarterBuff"] = this.starterBuff;
		}
		dictionary["status"] = (int)this.status;
		Storager.setString("killRateValues", Json.Serialize(dictionary), false);
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x0006779C File Offset: 0x0006599C
	public static void OnTryGunBuyed()
	{
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.BuyedGun, 0);
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000677AC File Offset: 0x000659AC
	public static void OnGunTakeOff()
	{
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.DebuffAfterGun, -1);
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000677BC File Offset: 0x000659BC
	public static void RemoveGunBuff()
	{
		KillRateCheck.instance.MakeRemoveGunBuff();
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000677C8 File Offset: 0x000659C8
	public void MakeRemoveGunBuff()
	{
		if (this.status != KillRateCheck.CheckStatus.GetGun)
		{
			return;
		}
		KillRateCheck.instance.WriteStatusAndResetCounter(KillRateCheck.CheckStatus.CoolDown, 0);
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000677E4 File Offset: 0x000659E4
	private void WriteStatusAndResetCounter(KillRateCheck.CheckStatus stat, int round)
	{
		this.roundCount = round;
		this.status = stat;
		this.SaveValues();
		this.CheckForBuff();
	}

	// Token: 0x04000EBB RID: 3771
	public const int DiscountTryGun = 50;

	// Token: 0x04000EBC RID: 3772
	public const int TryGunPromoDuration = 3600;

	// Token: 0x04000EBD RID: 3773
	private static KillRateCheck _instance;

	// Token: 0x04000EBE RID: 3774
	private static float lastConfigCheck;

	// Token: 0x04000EBF RID: 3775
	private bool activeFromServer;

	// Token: 0x04000EC0 RID: 3776
	private KillRateCheck.ParamByTier[] paramsByTier;

	// Token: 0x04000EC1 RID: 3777
	private int startRounds = 5;

	// Token: 0x04000EC2 RID: 3778
	private int roundsForCheckL1 = 3;

	// Token: 0x04000EC3 RID: 3779
	private int roundsForCheckL2 = 2;

	// Token: 0x04000EC4 RID: 3780
	private int roundsForL2Buff = 3;

	// Token: 0x04000EC5 RID: 3781
	private float starterHighKillrate = 1.2f;

	// Token: 0x04000EC6 RID: 3782
	private int roundsForGunLow = 3;

	// Token: 0x04000EC7 RID: 3783
	private int roundsForGunMiddle = 2;

	// Token: 0x04000EC8 RID: 3784
	private int roundsForGunHigh = 2;

	// Token: 0x04000EC9 RID: 3785
	private float debuffL1 = 0.85f;

	// Token: 0x04000ECA RID: 3786
	private float debuffL2 = 0.7f;

	// Token: 0x04000ECB RID: 3787
	private int debuffRoundsL1 = 2;

	// Token: 0x04000ECC RID: 3788
	private int debuffRoundsL2 = 2;

	// Token: 0x04000ECD RID: 3789
	private float debuffVal = 1.2f;

	// Token: 0x04000ECE RID: 3790
	private int debuffRoundsAfterGun = 1;

	// Token: 0x04000ECF RID: 3791
	private float debuffAfterGun = 0.75f;

	// Token: 0x04000ED0 RID: 3792
	public float timeForDiscount = 3600f;

	// Token: 0x04000ED1 RID: 3793
	public int discountValue = 50;

	// Token: 0x04000ED2 RID: 3794
	private KillRateCheck.CheckStatus status;

	// Token: 0x04000ED3 RID: 3795
	private bool writeKill;

	// Token: 0x04000ED4 RID: 3796
	private int[] kills = new int[30];

	// Token: 0x04000ED5 RID: 3797
	private int[] deaths = new int[30];

	// Token: 0x04000ED6 RID: 3798
	private int roundCount;

	// Token: 0x04000ED7 RID: 3799
	private int allRoundsCount;

	// Token: 0x04000ED8 RID: 3800
	private float killRateVal;

	// Token: 0x04000ED9 RID: 3801
	private int starterBuff;

	// Token: 0x04000EDA RID: 3802
	private float nextBuffCheck;

	// Token: 0x04000EDB RID: 3803
	private int killRateLength;

	// Token: 0x04000EDC RID: 3804
	private int killCount;

	// Token: 0x04000EDD RID: 3805
	private int deathCount;

	// Token: 0x04000EDE RID: 3806
	public bool buffEnabled;

	// Token: 0x04000EDF RID: 3807
	public float damageBuff = 1f;

	// Token: 0x04000EE0 RID: 3808
	public float healthBuff = 1f;

	// Token: 0x04000EE1 RID: 3809
	public bool giveWeapon;

	// Token: 0x04000EE2 RID: 3810
	public bool active;

	// Token: 0x04000EE3 RID: 3811
	private bool calcbuff;

	// Token: 0x04000EE4 RID: 3812
	private bool isFirstRounds;

	// Token: 0x04000EE5 RID: 3813
	private bool configLoaded;

	// Token: 0x020002DE RID: 734
	private enum CheckStatus
	{
		// Token: 0x04000EEA RID: 3818
		None,
		// Token: 0x04000EEB RID: 3819
		Starter,
		// Token: 0x04000EEC RID: 3820
		StarterBuff,
		// Token: 0x04000EED RID: 3821
		WaitGetGun,
		// Token: 0x04000EEE RID: 3822
		GetGun,
		// Token: 0x04000EEF RID: 3823
		BuyedGun,
		// Token: 0x04000EF0 RID: 3824
		CoolDown,
		// Token: 0x04000EF1 RID: 3825
		StarterBuff2,
		// Token: 0x04000EF2 RID: 3826
		HardPlayer,
		// Token: 0x04000EF3 RID: 3827
		HardDebuff,
		// Token: 0x04000EF4 RID: 3828
		HardDebuff2,
		// Token: 0x04000EF5 RID: 3829
		DebuffAfterGun
	}

	// Token: 0x020002DF RID: 735
	public class ParamByTier
	{
		// Token: 0x060019BC RID: 6588 RVA: 0x00067820 File Offset: 0x00065A20
		public ParamByTier()
		{
			this.cooldownRoundsLow = 10;
			this.cooldownRoundsMiddle = 15;
			this.cooldownRoundsHigh = 20;
			this.lowKillRate = 0.5f;
			this.highKillRate = 1.2f;
			this.buffL1 = 1.4f;
			this.buffL2 = 1.2f;
			this.buffRoundsL1 = 2;
			this.buffRoundsL2 = 2;
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x000678AC File Offset: 0x00065AAC
		public ParamByTier(Dictionary<string, object> dictionary)
		{
			this.cooldownRoundsLow = Convert.ToInt32(dictionary["cooldownRoundsLow"]);
			this.cooldownRoundsMiddle = Convert.ToInt32(dictionary["cooldownRoundsMiddle"]);
			this.cooldownRoundsHigh = Convert.ToInt32(dictionary["cooldownRoundsHigh"]);
			this.lowKillRate = Convert.ToSingle(dictionary["lowKillRate"]);
			this.highKillRate = Convert.ToSingle(dictionary["highKillRate"]);
			this.buffL1 = Convert.ToSingle(dictionary["buffL1"]);
			this.buffL2 = Convert.ToSingle(dictionary["buffL2"]);
			this.buffRoundsL1 = Convert.ToInt32(dictionary["buffRoundsL1"]);
			this.buffRoundsL2 = Convert.ToInt32(dictionary["buffRoundsL2"]);
		}

		// Token: 0x04000EF6 RID: 3830
		public int cooldownRoundsLow;

		// Token: 0x04000EF7 RID: 3831
		public int cooldownRoundsMiddle;

		// Token: 0x04000EF8 RID: 3832
		public int cooldownRoundsHigh;

		// Token: 0x04000EF9 RID: 3833
		public float lowKillRate;

		// Token: 0x04000EFA RID: 3834
		public float highKillRate;

		// Token: 0x04000EFB RID: 3835
		public float buffL1 = 1f;

		// Token: 0x04000EFC RID: 3836
		public float buffL2 = 1f;

		// Token: 0x04000EFD RID: 3837
		public int buffRoundsL1 = 2;

		// Token: 0x04000EFE RID: 3838
		public int buffRoundsL2 = 1;
	}
}
