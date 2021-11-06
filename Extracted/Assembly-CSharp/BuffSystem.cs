using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000041 RID: 65
internal sealed class BuffSystem : MonoBehaviour
{
	// Token: 0x1700001E RID: 30
	// (get) Token: 0x060001C7 RID: 455 RVA: 0x000115F8 File Offset: 0x0000F7F8
	public static BuffSystem instance
	{
		get
		{
			if (BuffSystem._instance == null)
			{
				GameObject gameObject = new GameObject("BuffSystem");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				BuffSystem._instance = gameObject.AddComponent<BuffSystem>();
			}
			return BuffSystem._instance;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x060001C8 RID: 456 RVA: 0x00011638 File Offset: 0x0000F838
	private BuffSystem.ParamByTier tierParam
	{
		get
		{
			return this.paramsByTier[ExpController.Instance.OurTier];
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x060001C9 RID: 457 RVA: 0x0001164C File Offset: 0x0000F84C
	public bool haveFirstInteractons
	{
		get
		{
			return this.interactionCounter >= 4;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060001CA RID: 458 RVA: 0x0001165C File Offset: 0x0000F85C
	public bool haveAllInteractons
	{
		get
		{
			return this.status != BuffSystem.CheckStatus.NewPlayer;
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0001166C File Offset: 0x0000F86C
	public void BuffsActive(bool value)
	{
		this.buffsActive = value;
		this.CheckExpiredBuffs();
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0001167C File Offset: 0x0000F87C
	public void CheckForPlayerBuff()
	{
		this.damageBuff = 1f;
		this.healthBuff = 1f;
		if (this.buffsActive)
		{
			float killrateByInteractions = this.GetKillrateByInteractions();
			this.currentBuff = null;
			this.weaponBuff = null;
			for (int i = 0; i < this.situationBuffs.Count; i++)
			{
				if (string.IsNullOrEmpty(this.situationBuffs[i].weapon))
				{
					if (this.currentBuff == null || this.currentBuff.param.priority < this.situationBuffs[i].param.priority)
					{
						this.currentBuff = this.situationBuffs[i];
					}
					if (this.currentBuff != null)
					{
						Debug.Log(string.Format("<color=green>Buff active: {0}</color>", this.currentBuff.param.type.ToString()));
					}
				}
				else
				{
					if (this.weaponBuff == null || this.weaponBuff.param.priority < this.situationBuffs[i].param.priority)
					{
						this.weaponBuff = this.situationBuffs[i];
					}
					if (this.weaponBuff != null)
					{
						Debug.Log(string.Format("<color=green>Weapon buff active: {0}</color>", this.weaponBuffValue));
					}
				}
			}
			switch (this.status)
			{
			case BuffSystem.CheckStatus.NewPlayer:
				if (this.interactionCounter < this.interactionBuffs.Length && this.interactionBuffs[this.interactionCounter])
				{
					this.healthBuff = ((ExperienceController.sharedController.currentLevel != 1 && !ShopNGUIController.NoviceArmorAvailable) ? this.firstBuffNoArmor : this.firstBuffArmor);
				}
				break;
			case BuffSystem.CheckStatus.Regular:
				if (this.currentBuff != null)
				{
					this.damageBuff = this.currentBuff.param.damageBuff;
					this.healthBuff = this.currentBuff.param.healthBuff;
				}
				else
				{
					float num = 1f + 0.01f * this.GetBuffPercentByKillRate(killrateByInteractions);
					this.damageBuff = num;
					this.healthBuff = num;
				}
				break;
			}
			if (this.status != BuffSystem.CheckStatus.NewPlayer)
			{
				this.damageBuff = Mathf.Clamp(this.damageBuff, (float)((killrateByInteractions >= 0.8f) ? 0 : 1), (float)((killrateByInteractions <= 2f) ? 2 : 1));
				this.healthBuff = Mathf.Clamp(this.healthBuff, (float)((killrateByInteractions >= 0.8f) ? 0 : 1), (float)((killrateByInteractions <= 2f) ? 2 : 1));
			}
		}
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SetupBuffParameters(this.damageBuff, this.healthBuff);
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0001197C File Offset: 0x0000FB7C
	private float GetBuffPercentByKillRate(float value)
	{
		float num = Mathf.Round(10f * value) / 10f;
		Debug.Log(string.Format("<color=green>Killrate: {0}</color>", num));
		if (this.tierParam.midbottom < num && num < this.tierParam.midtop)
		{
			return 0f;
		}
		return this.tierParam.b - Mathf.Clamp(num, this.tierParam.bottom, this.tierParam.top) * this.tierParam.a;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00011A10 File Offset: 0x0000FC10
	public void KillInteraction()
	{
		this.CheckAndWriteInteraction(BuffSystem.InteractionType.Kill);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00011A1C File Offset: 0x0000FC1C
	public void DeathInteraction()
	{
		this.CheckAndWriteInteraction(BuffSystem.InteractionType.Death);
		this.SaveInteractions();
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00011A2C File Offset: 0x0000FC2C
	private void CheckAndWriteInteraction(BuffSystem.InteractionType value)
	{
		if (!this.buffsActive)
		{
			return;
		}
		switch (this.status)
		{
		case BuffSystem.CheckStatus.NewPlayer:
			if (this.interactionCounter < this.interactionBuffs.Length && !this.interactionBuffs[this.interactionCounter])
			{
				this.WriteInteraction(value);
			}
			this.interactionCounter++;
			if (this.interactionCounter >= this.interactionBuffs.Length)
			{
				this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
				this.status = BuffSystem.CheckStatus.Regular;
			}
			break;
		case BuffSystem.CheckStatus.OldPlayer:
			this.WriteInteraction(value);
			this.interactionCounter++;
			if (this.interactionCounter >= this.interactionCountForOldPlayer)
			{
				this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
				this.status = BuffSystem.CheckStatus.Regular;
			}
			break;
		case BuffSystem.CheckStatus.Regular:
			this.WriteInteraction(value);
			this.interactionCounter++;
			break;
		}
		this.CheckForPlayerBuff();
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00011B30 File Offset: 0x0000FD30
	private void WriteInteraction(BuffSystem.InteractionType value)
	{
		this.interactionsChanged = true;
		this.killRateCached = -1f;
		for (int i = this.interactions.Length - 2; i >= 0; i--)
		{
			this.interactions[i + 1] = this.interactions[i];
		}
		this.interactions[0] = value;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00011B88 File Offset: 0x0000FD88
	public float GetKillrateByInteractions()
	{
		if (this.killRateCached != -1f)
		{
			return this.killRateCached;
		}
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < this.interactions.Length; i++)
		{
			BuffSystem.InteractionType interactionType = this.interactions[i];
			if (interactionType != BuffSystem.InteractionType.Kill)
			{
				if (interactionType == BuffSystem.InteractionType.Death)
				{
					num2++;
				}
			}
			else
			{
				num++;
			}
		}
		if (num2 != 0)
		{
			this.killRateCached = (float)num / (float)num2;
		}
		else
		{
			this.killRateCached = (float)num;
		}
		return this.killRateCached;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00011C1C File Offset: 0x0000FE1C
	public void OnGetProgress()
	{
		if (this.status == BuffSystem.CheckStatus.None || this.status == BuffSystem.CheckStatus.NewPlayer)
		{
			this.status = BuffSystem.CheckStatus.OldPlayer;
			this.damageBuff = 1f;
			this.healthBuff = 1f;
			this.isFirstRounds = false;
		}
		this.SaveValues();
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00011C6C File Offset: 0x0000FE6C
	private void WriteDefaultParameters()
	{
		this.configLoaded = false;
		if (this.paramsByTier == null)
		{
			this.paramsByTier = new BuffSystem.ParamByTier[6];
			for (int i = 0; i < this.paramsByTier.Length; i++)
			{
				this.paramsByTier[i] = new BuffSystem.ParamByTier();
			}
		}
		this.buffParamByType = new Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter>(new BuffSystem.SituationBuffComparer());
		this.buffParamByType.Add(BuffSystem.SituationBuffType.DebuffBeforeGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.DebuffBeforeGun, 1f, 0.5f, 540f, -1));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.DebuffAfterGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.DebuffAfterGun, 1f, 0.5f, 180f, -1));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.TierLvlUp, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.TierLvlUp, 1f, 0.7f, 240f, -1));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.TryGunBuff, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.TryGunBuff, 1f, 1.25f, 0f, 1));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.BuyedTryGun, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.BuyedTryGun, 1f, 1.25f, 600f, 2));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin1, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin1, 1f, 1.25f, 300f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin7, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin7, 1f, 1.25f, 400f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin2, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin2, 1f, 1.25f, 500f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin3, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin3, 1f, 1.25f, 600f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin4, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin4, 1f, 1.25f, 700f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin5, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin5, 1f, 1.25f, 800f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Coin8, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Coin8, 1f, 1.25f, 900f, 3));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem1, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem1, 1f, 1.25f, 300f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem2, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem2, 1f, 1.25f, 400f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem3, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem3, 1f, 1.25f, 500f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem4, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem4, 1f, 1.25f, 600f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem5, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem5, 1f, 1.25f, 700f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem6, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem6, 1f, 1.25f, 800f, 4));
		this.buffParamByType.Add(BuffSystem.SituationBuffType.Gem7, new BuffSystem.BuffParameter(BuffSystem.SituationBuffType.Gem7, 1f, 1.25f, 900f, 4));
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00011F68 File Offset: 0x00010168
	public void TryLoadConfig()
	{
		if (this.configLoaded)
		{
			return;
		}
		if (!Storager.hasKey("BuffsParam"))
		{
			this.WriteDefaultParameters();
			return;
		}
		string @string = Storager.getString("BuffsParam", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null || !dictionary.ContainsKey("buffSettings"))
		{
			this.WriteDefaultParameters();
			return;
		}
		Dictionary<string, object> dictionary2 = dictionary["buffSettings"] as Dictionary<string, object>;
		string text = string.Empty;
		if (dictionary2.ContainsKey("roundsForGunLow"))
		{
			this.roundsForGunLow = Convert.ToInt32(dictionary2["roundsForGunLow"]);
		}
		else
		{
			text = "get roundsForGunLow";
		}
		if (dictionary2.ContainsKey("roundsForGunMiddle"))
		{
			this.roundsForGunMiddle = Convert.ToInt32(dictionary2["roundsForGunMiddle"]);
		}
		else
		{
			text = "get roundsForGunMiddle";
		}
		if (dictionary2.ContainsKey("roundsForGunHigh"))
		{
			this.roundsForGunHigh = Convert.ToInt32(dictionary2["roundsForGunHigh"]);
		}
		else
		{
			text = "get roundsForGunHigh";
		}
		if (dictionary2.ContainsKey("timeForDiscount"))
		{
			this.timeForDiscount = Convert.ToSingle(dictionary2["timeForDiscount"]);
		}
		else
		{
			text = "get timeForDiscount";
		}
		if (dictionary2.ContainsKey("discountValue"))
		{
			this.discountValue = Convert.ToInt32(dictionary2["discountValue"]);
		}
		else
		{
			text = "get discountValue";
		}
		if (dictionary2.ContainsKey("debuffKillrateForGun"))
		{
			this.debuffKillrateForGun = Convert.ToSingle(dictionary2["debuffKillrateForGun"]);
		}
		else
		{
			text = "get debuffKillrateForGun";
		}
		if (dictionary2.ContainsKey("firstBuffArmor"))
		{
			this.firstBuffArmor = Convert.ToSingle(dictionary2["firstBuffArmor"]);
		}
		else
		{
			text = "get firstBuffArmor";
		}
		if (dictionary2.ContainsKey("firstBuffNoArmor"))
		{
			this.firstBuffNoArmor = Convert.ToSingle(dictionary2["firstBuffNoArmor"]);
		}
		else
		{
			text = "get firstBuffNoArmor";
		}
		if (dictionary2.ContainsKey("tierParams"))
		{
			List<object> list = dictionary2["tierParams"] as List<object>;
			if (list != null)
			{
				this.paramsByTier = (from e in list
				select new BuffSystem.ParamByTier(e as Dictionary<string, object>)).ToArray<BuffSystem.ParamByTier>();
			}
			else
			{
				text = "tierParams == null";
			}
		}
		else
		{
			text = "get tierParams";
		}
		if (dictionary2.ContainsKey("buffsParams"))
		{
			List<object> list2 = dictionary2["buffsParams"] as List<object>;
			this.buffParamByType = new Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter>();
			for (int i = 0; i < list2.Count; i++)
			{
				Dictionary<string, object> dictionary3 = list2[i] as Dictionary<string, object>;
				BuffSystem.SituationBuffType key = (BuffSystem.SituationBuffType)((int)Enum.Parse(typeof(BuffSystem.SituationBuffType), Convert.ToString(dictionary3["type"])));
				this.buffParamByType.Add(key, new BuffSystem.BuffParameter(dictionary3));
			}
		}
		else
		{
			text = "get buffsParams";
		}
		if (!string.IsNullOrEmpty(text))
		{
			Debug.LogError("Error Deserialize JSON: buffSettings - " + text);
			this.WriteDefaultParameters();
		}
		else
		{
			this.configLoaded = true;
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x000122A8 File Offset: 0x000104A8
	private void LoadValues()
	{
		this.loadValuesCalled = true;
		string @string = Storager.getString("buffsValues", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("interactionCount"))
			{
				this.interactionCounter = Convert.ToInt32(dictionary["interactionCount"]);
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
				this.status = (BuffSystem.CheckStatus)Convert.ToInt32(dictionary["status"]);
			}
			if (dictionary.ContainsKey("lastGiveGunTime"))
			{
				this.lastGiveGunTime = Convert.ToSingle(dictionary["lastGiveGunTime"]);
			}
			if (dictionary.ContainsKey("giveGun"))
			{
				this.readyToGiveGun = (Convert.ToInt32(dictionary["giveGun"]) == 1);
			}
			if (dictionary.ContainsKey("waitTime"))
			{
				this.waitingForPurchaseTime = Convert.ToSingle(dictionary["waitTime"]);
			}
			if (dictionary.ContainsKey("waitBuff"))
			{
				BuffSystem.SituationBuffType key = (BuffSystem.SituationBuffType)Convert.ToInt32(dictionary["waitBuff"]);
				if (this.buffParamByType.ContainsKey(key))
				{
					this.waitingForPurchaseBuff = this.buffParamByType[key];
				}
			}
			if (dictionary.ContainsKey("buffs"))
			{
				List<object> list = dictionary["buffs"] as List<object>;
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					BuffSystem.SituationBuffType key2 = (BuffSystem.SituationBuffType)Convert.ToInt32(dictionary2["type"]);
					string text = (!dictionary2.ContainsKey("weapon")) ? string.Empty : Convert.ToString(dictionary2["weapon"]);
					float savedTime = Convert.ToSingle(dictionary2["expire"]);
					BuffSystem.SituationBuff item = new BuffSystem.SituationBuff(this.buffParamByType[key2], text, savedTime);
					this.situationBuffs.Add(item);
				}
			}
		}
		if (this.status == BuffSystem.CheckStatus.None)
		{
			if (Storager.getInt(Defs.TrainingCompleted_4_4_Sett, false) != 1)
			{
				this.status = BuffSystem.CheckStatus.NewPlayer;
				this.isFirstRounds = true;
			}
			else
			{
				this.status = BuffSystem.CheckStatus.OldPlayer;
			}
			this.SaveValues();
		}
		string string2 = Storager.getString("buffsPlayerInteractions", false);
		List<object> list2 = Json.Deserialize(string2) as List<object>;
		if (list2 != null)
		{
			this.interactions = (from o in list2
			select (BuffSystem.InteractionType)Convert.ToInt32(o)).ToArray<BuffSystem.InteractionType>();
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00012580 File Offset: 0x00010780
	private void SaveValues()
	{
		if (!this.loadValuesCalled)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (this.interactionCounter > 0)
		{
			dictionary["interactionCount"] = this.interactionCounter;
		}
		if (this.isFirstRounds)
		{
			dictionary["isFirstRounds"] = 1;
		}
		if (this.readyToGiveGun)
		{
			dictionary["giveGun"] = 1;
		}
		if (this.allRoundsCount > 0)
		{
			dictionary["allRoundsCount"] = this.allRoundsCount;
		}
		if (this.situationBuffs != null && this.situationBuffs.Count > 0)
		{
			dictionary["buffs"] = (from o in this.situationBuffs
			select o.Serialize()).ToArray<Dictionary<string, object>>();
		}
		if (this.lastGiveGunTime > 0f)
		{
			dictionary["lastGiveGunTime"] = this.lastGiveGunTime;
		}
		if (this.waitingForPurchaseTime > 0f)
		{
			dictionary["waitTime"] = this.waitingForPurchaseTime;
		}
		if (this.waitingForPurchaseBuff != null)
		{
			dictionary["waitBuff"] = (int)this.waitingForPurchaseBuff.type;
		}
		dictionary["status"] = (int)this.status;
		Storager.setString("buffsValues", Json.Serialize(dictionary), false);
		this.SaveInteractions();
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00012710 File Offset: 0x00010910
	private void SaveInteractions()
	{
		if (this.interactionsChanged)
		{
			this.interactionsChanged = false;
			Storager.setString("buffsPlayerInteractions", Json.Serialize((from o in this.interactions
			select (int)o).ToArray<int>()), false);
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0001276C File Offset: 0x0001096C
	private void GiveTryGunToPlayer()
	{
		this.readyToGiveGun = true;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00012778 File Offset: 0x00010978
	private float GetTimeForGun()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		if (killrateByInteractions < this.tierParam.lowKillRate)
		{
			return this.tierParam.timeToGetGunLow;
		}
		if (killrateByInteractions < this.tierParam.highKillRate)
		{
			return this.tierParam.timeToGetGunMiddle;
		}
		return this.tierParam.timeToGetGunHigh;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x000127D4 File Offset: 0x000109D4
	private void CheckExpiredBuffs()
	{
		if (this.buffsActive)
		{
			for (int i = 0; i < this.situationBuffs.Count; i++)
			{
				if (this.situationBuffs[i].expired && this.situationBuffs[i].param.type != BuffSystem.SituationBuffType.TryGunBuff)
				{
					BuffSystem.SituationBuffType type = this.situationBuffs[i].param.type;
					if (type == BuffSystem.SituationBuffType.DebuffBeforeGun)
					{
						this.GiveTryGunToPlayer();
					}
					this.situationBuffs.RemoveAt(i);
					i--;
				}
			}
		}
	}

	// Token: 0x060001DC RID: 476 RVA: 0x00012878 File Offset: 0x00010A78
	public void PlayerLeaved()
	{
		this.CheckExpiredBuffs();
		this.SaveValues();
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00012888 File Offset: 0x00010A88
	public void EndRound()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		Debug.Log(killrateByInteractions);
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesKillRate(this.allRoundsCount, killrateByInteractions);
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
		if (this.buffsActive && this.configLoaded)
		{
			this.CheckExpiredBuffs();
			if (this.status != BuffSystem.CheckStatus.NewPlayer && this.status != BuffSystem.CheckStatus.OldPlayer)
			{
				if (this.lastGiveGunTime + this.GetTimeForGun() < NotificationController.instance.currentPlayTimeMatch)
				{
					this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
					if (this.GetKillrateByInteractions() > this.debuffKillrateForGun)
					{
						this.AddSituationBuff(BuffSystem.SituationBuffType.DebuffBeforeGun, string.Empty);
					}
					else
					{
						this.GiveTryGunToPlayer();
					}
				}
				if (this.readyToGiveGun && WeaponManager.sharedManager._currentFilterMap == 0)
				{
					this.giveTryGun = true;
				}
			}
		}
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	// Token: 0x060001DE RID: 478 RVA: 0x000129B8 File Offset: 0x00010BB8
	private void AddSituationBuff(BuffSystem.SituationBuffType type, string buffForWeapon = "")
	{
		this.situationBuffs.Add(new BuffSystem.SituationBuff(this.buffParamByType[type], buffForWeapon));
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	// Token: 0x060001DF RID: 479 RVA: 0x000129E4 File Offset: 0x00010BE4
	private void ClearDebuffs()
	{
		for (int i = 0; i < this.situationBuffs.Count; i++)
		{
			if (this.situationBuffs[i].isDebuff)
			{
				this.situationBuffs.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x00012A34 File Offset: 0x00010C34
	private void ClearBuffOfType(BuffSystem.SituationBuffType type)
	{
		for (int i = 0; i < this.situationBuffs.Count; i++)
		{
			if (this.situationBuffs[i].param.type == type)
			{
				this.situationBuffs.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00012A8C File Offset: 0x00010C8C
	public void SetGetTryGun(string weaponName)
	{
		this.giveTryGun = false;
		this.readyToGiveGun = false;
		this.ClearDebuffs();
		this.AddSituationBuff(BuffSystem.SituationBuffType.TryGunBuff, weaponName);
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00012AAC File Offset: 0x00010CAC
	public void OnTryGunBuyed(string weaponName)
	{
		this.ClearDebuffs();
		this.AddSituationBuff(BuffSystem.SituationBuffType.BuyedTryGun, weaponName);
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00012ABC File Offset: 0x00010CBC
	public void OnGunBuyed()
	{
		this.ClearDebuffs();
		this.lastGiveGunTime = NotificationController.instance.currentPlayTimeMatch;
		this.SaveValues();
		this.CheckForPlayerBuff();
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00012AEC File Offset: 0x00010CEC
	public void OnCurrencyBuyed(bool isGems, int index)
	{
		BuffSystem.SituationBuffType key;
		if (isGems)
		{
			if (index >= this.gemsBuffByIndex.Length)
			{
				return;
			}
			key = this.gemsBuffByIndex[index];
		}
		else
		{
			if (index >= this.coinsBuffByIndex.Length)
			{
				return;
			}
			key = this.coinsBuffByIndex[index];
		}
		if (!this.buffParamByType.ContainsKey(key))
		{
			return;
		}
		BuffSystem.BuffParameter buffParameter = this.buffParamByType[key];
		if (this.waitingForPurchaseBuff == null || this.waitingForPurchaseTime < buffParameter.timeForPurchase + NotificationController.instance.currentPlayTime)
		{
			this.waitingForPurchaseBuff = buffParameter;
			this.waitingForPurchaseTime = this.waitingForPurchaseBuff.timeForPurchase + NotificationController.instance.currentPlayTime;
		}
		this.SaveValues();
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00012BB0 File Offset: 0x00010DB0
	public void OnSomethingPurchased()
	{
		if (this.waitingForPurchaseBuff != null)
		{
			if (this.waitingForPurchaseTime > NotificationController.instance.currentPlayTime)
			{
				BuffSystem.SituationBuffType type = this.waitingForPurchaseBuff.type;
				this.waitingForPurchaseBuff = null;
				this.waitingForPurchaseTime = 0f;
				this.ClearDebuffs();
				this.AddSituationBuff(type, string.Empty);
			}
			else
			{
				this.waitingForPurchaseBuff = null;
				this.waitingForPurchaseTime = 0f;
				this.SaveValues();
			}
		}
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00012C2C File Offset: 0x00010E2C
	public void OnGunTakeOff()
	{
		this.ClearBuffOfType(BuffSystem.SituationBuffType.TryGunBuff);
		this.AddSituationBuff(BuffSystem.SituationBuffType.DebuffAfterGun, string.Empty);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00012C44 File Offset: 0x00010E44
	public void RemoveGunBuff()
	{
		this.ClearBuffOfType(BuffSystem.SituationBuffType.TryGunBuff);
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x00012C50 File Offset: 0x00010E50
	public void OnTierLvlUp()
	{
		this.AddSituationBuff(BuffSystem.SituationBuffType.TierLvlUp, string.Empty);
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x00012C60 File Offset: 0x00010E60
	public int GetRoundsForGun()
	{
		float killrateByInteractions = this.GetKillrateByInteractions();
		if (killrateByInteractions < this.tierParam.lowKillRate)
		{
			return this.roundsForGunLow;
		}
		if (killrateByInteractions < this.tierParam.highKillRate)
		{
			return this.roundsForGunMiddle;
		}
		return this.roundsForGunHigh;
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060001EA RID: 490 RVA: 0x00012CAC File Offset: 0x00010EAC
	public float weaponBuffValue
	{
		get
		{
			return (this.weaponBuff == null) ? 1f : Mathf.Clamp(this.weaponBuff.param.damageBuff, (float)((this.GetKillrateByInteractions() >= 0.8f) ? 0 : 1), (float)((this.GetKillrateByInteractions() <= 2f) ? 2 : 1));
		}
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00012D14 File Offset: 0x00010F14
	public bool haveBuffForWeapon(string weapon)
	{
		return this.weaponBuff != null && !string.IsNullOrEmpty(weapon) && this.weaponBuff.weapon == weapon;
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00012D4C File Offset: 0x00010F4C
	public void LogFirstBattlesResult(bool isWinner)
	{
		if (this.isFirstRounds && this.allRoundsCount < 10)
		{
			AnalyticsStuff.LogFirstBattlesResult(this.allRoundsCount, isWinner);
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00012D80 File Offset: 0x00010F80
	private void Awake()
	{
		this.TryLoadConfig();
		this.LoadValues();
		this.CheckForPlayerBuff();
		ShopNGUIController.GunOrArmorBought += this.OnGunBuyed;
	}

	// Token: 0x040001D4 RID: 468
	public const int DiscountTryGun = 50;

	// Token: 0x040001D5 RID: 469
	public const int TryGunPromoDuration = 3600;

	// Token: 0x040001D6 RID: 470
	private static BuffSystem _instance;

	// Token: 0x040001D7 RID: 471
	private bool[] interactionBuffs = new bool[]
	{
		true,
		true,
		false,
		true,
		false,
		true,
		false,
		false,
		true,
		false,
		false,
		true,
		false,
		false,
		true,
		false,
		false
	};

	// Token: 0x040001D8 RID: 472
	private BuffSystem.ParamByTier[] paramsByTier;

	// Token: 0x040001D9 RID: 473
	private Dictionary<BuffSystem.SituationBuffType, BuffSystem.BuffParameter> buffParamByType;

	// Token: 0x040001DA RID: 474
	private List<BuffSystem.SituationBuff> situationBuffs = new List<BuffSystem.SituationBuff>();

	// Token: 0x040001DB RID: 475
	private BuffSystem.SituationBuff currentBuff;

	// Token: 0x040001DC RID: 476
	private BuffSystem.SituationBuff weaponBuff;

	// Token: 0x040001DD RID: 477
	private bool configLoaded;

	// Token: 0x040001DE RID: 478
	private bool loadValuesCalled;

	// Token: 0x040001DF RID: 479
	private BuffSystem.CheckStatus status;

	// Token: 0x040001E0 RID: 480
	private BuffSystem.InteractionType[] interactions = new BuffSystem.InteractionType[30];

	// Token: 0x040001E1 RID: 481
	private bool interactionsChanged;

	// Token: 0x040001E2 RID: 482
	private bool buffsActive;

	// Token: 0x040001E3 RID: 483
	private int interactionCounter;

	// Token: 0x040001E4 RID: 484
	private BuffSystem.BuffParameter waitingForPurchaseBuff;

	// Token: 0x040001E5 RID: 485
	private float waitingForPurchaseTime;

	// Token: 0x040001E6 RID: 486
	private float lastGiveGunTime;

	// Token: 0x040001E7 RID: 487
	private bool readyToGiveGun;

	// Token: 0x040001E8 RID: 488
	public bool giveTryGun;

	// Token: 0x040001E9 RID: 489
	public float timeForDiscount = 3600f;

	// Token: 0x040001EA RID: 490
	public int discountValue = 50;

	// Token: 0x040001EB RID: 491
	private int roundsForGunLow = 3;

	// Token: 0x040001EC RID: 492
	private int roundsForGunMiddle = 2;

	// Token: 0x040001ED RID: 493
	private int roundsForGunHigh = 2;

	// Token: 0x040001EE RID: 494
	private float debuffKillrateForGun = 0.8f;

	// Token: 0x040001EF RID: 495
	private float firstBuffArmor = 8f;

	// Token: 0x040001F0 RID: 496
	private float firstBuffNoArmor = 2f;

	// Token: 0x040001F1 RID: 497
	private int interactionCountForOldPlayer = 10;

	// Token: 0x040001F2 RID: 498
	private bool isFirstRounds;

	// Token: 0x040001F3 RID: 499
	private int allRoundsCount;

	// Token: 0x040001F4 RID: 500
	private float damageBuff = 1f;

	// Token: 0x040001F5 RID: 501
	private float healthBuff = 1f;

	// Token: 0x040001F6 RID: 502
	private float killRateCached = -1f;

	// Token: 0x040001F7 RID: 503
	private readonly BuffSystem.SituationBuffType[] gemsBuffByIndex = new BuffSystem.SituationBuffType[]
	{
		BuffSystem.SituationBuffType.Gem1,
		BuffSystem.SituationBuffType.Gem2,
		BuffSystem.SituationBuffType.Gem3,
		BuffSystem.SituationBuffType.Gem4,
		BuffSystem.SituationBuffType.Gem5,
		BuffSystem.SituationBuffType.Gem6,
		BuffSystem.SituationBuffType.Gem7
	};

	// Token: 0x040001F8 RID: 504
	private readonly BuffSystem.SituationBuffType[] coinsBuffByIndex = new BuffSystem.SituationBuffType[]
	{
		BuffSystem.SituationBuffType.Coin1,
		BuffSystem.SituationBuffType.Coin7,
		BuffSystem.SituationBuffType.Coin2,
		BuffSystem.SituationBuffType.Coin3,
		BuffSystem.SituationBuffType.Coin4,
		BuffSystem.SituationBuffType.Coin5,
		BuffSystem.SituationBuffType.Coin8
	};

	// Token: 0x02000042 RID: 66
	private sealed class SituationBuffComparer : IEqualityComparer<BuffSystem.SituationBuffType>
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x00012DDC File Offset: 0x00010FDC
		public bool Equals(BuffSystem.SituationBuffType x, BuffSystem.SituationBuffType y)
		{
			return x == y;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00012DE4 File Offset: 0x00010FE4
		public int GetHashCode(BuffSystem.SituationBuffType obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000043 RID: 67
	public class ParamByTier
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00012DE8 File Offset: 0x00010FE8
		public ParamByTier()
		{
			this.timeToGetGunLow = 2400f;
			this.timeToGetGunMiddle = 3600f;
			this.timeToGetGunHigh = 4800f;
			this.lowKillRate = 0.5f;
			this.highKillRate = 1.2f;
			this.a = 50f;
			this.b = 50f;
			this.midbottom = 0.8f;
			this.midtop = 1.2f;
			this.top = 2f;
			this.bottom = 0f;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00012E74 File Offset: 0x00011074
		public ParamByTier(Dictionary<string, object> dictionary)
		{
			this.timeToGetGunLow = Convert.ToSingle(dictionary["timeGunLow"]);
			this.timeToGetGunMiddle = Convert.ToSingle(dictionary["timeGunMiddle"]);
			this.timeToGetGunHigh = Convert.ToSingle(dictionary["timeGunHigh"]);
			this.lowKillRate = Convert.ToSingle(dictionary["lowKillRate"]);
			this.highKillRate = Convert.ToSingle(dictionary["highKillRate"]);
			this.a = Convert.ToSingle(dictionary["form_a"]);
			this.b = Convert.ToSingle(dictionary["form_b"]);
			this.midbottom = Convert.ToSingle(dictionary["form_midbottom"]);
			this.midtop = Convert.ToSingle(dictionary["form_midtop"]);
			this.top = Convert.ToSingle(dictionary["form_top"]);
			if (dictionary.ContainsKey("form_bottom"))
			{
				this.bottom = Convert.ToSingle(dictionary["form_bottom"]);
			}
		}

		// Token: 0x040001FD RID: 509
		public float timeToGetGunLow;

		// Token: 0x040001FE RID: 510
		public float timeToGetGunMiddle;

		// Token: 0x040001FF RID: 511
		public float timeToGetGunHigh;

		// Token: 0x04000200 RID: 512
		public float lowKillRate;

		// Token: 0x04000201 RID: 513
		public float highKillRate;

		// Token: 0x04000202 RID: 514
		public float a;

		// Token: 0x04000203 RID: 515
		public float b;

		// Token: 0x04000204 RID: 516
		public float midbottom;

		// Token: 0x04000205 RID: 517
		public float midtop;

		// Token: 0x04000206 RID: 518
		public float top;

		// Token: 0x04000207 RID: 519
		public float bottom;
	}

	// Token: 0x02000044 RID: 68
	private class BuffParameter
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x00012F8C File Offset: 0x0001118C
		public BuffParameter(BuffSystem.SituationBuffType type, float healthBuff, float damageBuff, float time, int priority)
		{
			this.type = type;
			this.healthBuff = healthBuff;
			this.damageBuff = damageBuff;
			this.priority = priority;
			this.time = time;
			this.timeForPurchase = 1800f;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00012FD0 File Offset: 0x000111D0
		public BuffParameter(Dictionary<string, object> dictionary)
		{
			this.type = (BuffSystem.SituationBuffType)((int)Enum.Parse(typeof(BuffSystem.SituationBuffType), Convert.ToString(dictionary["type"])));
			if (dictionary.ContainsKey("health"))
			{
				this.healthBuff = Convert.ToSingle(dictionary["health"]);
			}
			else
			{
				this.healthBuff = 1f;
			}
			if (dictionary.ContainsKey("damage"))
			{
				this.damageBuff = Convert.ToSingle(dictionary["damage"]);
			}
			else
			{
				this.damageBuff = 1f;
			}
			if (dictionary.ContainsKey("timeToBuy"))
			{
				this.timeForPurchase = Convert.ToSingle(dictionary["timeToBuy"]);
			}
			else
			{
				this.timeForPurchase = 0f;
			}
			this.priority = Convert.ToInt32(dictionary["prior"]);
			this.time = Convert.ToSingle(dictionary["time"]);
		}

		// Token: 0x04000208 RID: 520
		public int priority;

		// Token: 0x04000209 RID: 521
		public BuffSystem.SituationBuffType type;

		// Token: 0x0400020A RID: 522
		public float healthBuff;

		// Token: 0x0400020B RID: 523
		public float damageBuff;

		// Token: 0x0400020C RID: 524
		public float time;

		// Token: 0x0400020D RID: 525
		public float timeForPurchase;
	}

	// Token: 0x02000045 RID: 69
	private class SituationBuff
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x000130DC File Offset: 0x000112DC
		public SituationBuff(BuffSystem.BuffParameter param, string weaponBuff)
		{
			this.param = param;
			this.weapon = weaponBuff;
			this.expireTime = NotificationController.instance.currentPlayTimeMatch + param.time;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0001310C File Offset: 0x0001130C
		public SituationBuff(BuffSystem.BuffParameter param, string weaponBuff, float savedTime)
		{
			this.param = param;
			this.weapon = weaponBuff;
			this.expireTime = savedTime;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0001312C File Offset: 0x0001132C
		public Dictionary<string, object> Serialize()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["type"] = (int)this.param.type;
			dictionary["expire"] = this.expireTime;
			if (!string.IsNullOrEmpty(this.weapon))
			{
				dictionary["weapon"] = this.weapon;
			}
			return dictionary;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00013194 File Offset: 0x00011394
		public bool isDebuff
		{
			get
			{
				return this.param.healthBuff < 1f || this.param.damageBuff < 1f;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000131CC File Offset: 0x000113CC
		public bool expired
		{
			get
			{
				return this.expireTime < NotificationController.instance.currentPlayTimeMatch;
			}
		}

		// Token: 0x0400020E RID: 526
		public BuffSystem.BuffParameter param;

		// Token: 0x0400020F RID: 527
		private float expireTime;

		// Token: 0x04000210 RID: 528
		public string weapon;
	}

	// Token: 0x02000046 RID: 70
	private enum CheckStatus
	{
		// Token: 0x04000212 RID: 530
		None,
		// Token: 0x04000213 RID: 531
		NewPlayer,
		// Token: 0x04000214 RID: 532
		OldPlayer,
		// Token: 0x04000215 RID: 533
		Regular
	}

	// Token: 0x02000047 RID: 71
	private enum InteractionType
	{
		// Token: 0x04000217 RID: 535
		None,
		// Token: 0x04000218 RID: 536
		Kill,
		// Token: 0x04000219 RID: 537
		Death
	}

	// Token: 0x02000048 RID: 72
	private enum SituationBuffType
	{
		// Token: 0x0400021B RID: 539
		DebuffBeforeGun,
		// Token: 0x0400021C RID: 540
		DebuffAfterGun,
		// Token: 0x0400021D RID: 541
		TierLvlUp,
		// Token: 0x0400021E RID: 542
		TryGunBuff,
		// Token: 0x0400021F RID: 543
		BuyedTryGun,
		// Token: 0x04000220 RID: 544
		Coin1,
		// Token: 0x04000221 RID: 545
		Coin7,
		// Token: 0x04000222 RID: 546
		Coin2,
		// Token: 0x04000223 RID: 547
		Coin3,
		// Token: 0x04000224 RID: 548
		Coin4,
		// Token: 0x04000225 RID: 549
		Coin5,
		// Token: 0x04000226 RID: 550
		Coin8,
		// Token: 0x04000227 RID: 551
		Gem1,
		// Token: 0x04000228 RID: 552
		Gem2,
		// Token: 0x04000229 RID: 553
		Gem3,
		// Token: 0x0400022A RID: 554
		Gem4,
		// Token: 0x0400022B RID: 555
		Gem5,
		// Token: 0x0400022C RID: 556
		Gem6,
		// Token: 0x0400022D RID: 557
		Gem7,
		// Token: 0x0400022E RID: 558
		Count
	}
}
