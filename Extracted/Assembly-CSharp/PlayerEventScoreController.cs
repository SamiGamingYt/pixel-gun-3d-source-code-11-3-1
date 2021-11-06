using System;
using System.Collections.Generic;
using I2.Loc;

// Token: 0x02000480 RID: 1152
public class PlayerEventScoreController
{
	// Token: 0x0600281F RID: 10271 RVA: 0x000C881C File Offset: 0x000C6A1C
	static PlayerEventScoreController()
	{
		PlayerEventScoreController.SetScoreEventInfo();
		PlayerEventScoreController.SetLocalizeForScoreEvent();
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(PlayerEventScoreController.SetLocalizeForScoreEvent));
	}

	// Token: 0x06002820 RID: 10272 RVA: 0x000C8878 File Offset: 0x000C6A78
	public static void SetScoreEventInfo()
	{
		PlayerEventScoreController._eventsScoreInfo.Clear();
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageBody.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageBody.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageHead.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageHead.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageMechBody.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageMechBody.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageMechHead.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageMechHead.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageTurret.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageTurret.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageExplosion.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageExplosion.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.damageGrenade.ToString(), 0, PlayerEventScoreController.ScoreEvent.damageGrenade.ToString(), string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadMech.ToString(), 40, "Key_1129", "MechKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadTurret.ToString(), 40, "Key_1130", "TurretKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.dead.ToString(), 15, "Key_1127", "Kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killPet.ToString(), 15, "Key_1127", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadHeadShot.ToString(), 30, "Key_1128", "Headshot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadLongShot.ToString(), 45, "Key_1133", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.invisibleKill.ToString(), 30, "Key_1131", "InvisibleKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.doubleHeadShot.ToString(), 40, "Key_1132", "DoubleHeadshot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadWithFlag.ToString(), 30, "Key_1144", "FlagKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deathFromAbove.ToString(), 20, "Key_1215", "DeathFromAboue"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.duckHunt.ToString(), 20, "Key_1214", "DuckHunt"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.rocketJumpKill.ToString(), 20, "Key_1216", "RocketJumpKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee.ToString(), 30, "Key_1270", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee2.ToString(), 30, "Key_1209", "Butcer"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee3.ToString(), 45, "Key_1210", "MadButcer"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee5.ToString(), 75, "Key_1211", "Slaughter"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.melee7.ToString(), 150, "Key_1212", "Massacre"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup1.ToString(), 45, "Key_2085", "Backup_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup2.ToString(), 75, "Key_2086", "Backup_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.backup3.ToString(), 150, "Key_2087", "Backup_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium1.ToString(), 45, "Key_2091", "Premium_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium2.ToString(), 75, "Key_2092", "Premium_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.premium3.ToString(), 150, "Key_2093", "Premium_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary1.ToString(), 45, "Key_2088", "Primary_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary2.ToString(), 75, "Key_2089", "Primary_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.primary3.ToString(), 150, "Key_2090", "Primary_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper1.ToString(), 45, "Key_2097", "Sniper_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper2.ToString(), 75, "Key_2098", "Sniper_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.sniper3.ToString(), 150, "Key_2099", "Sniper_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special1.ToString(), 45, "Key_2094", "Special_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special2.ToString(), 75, "Key_2095", "Special_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.special3.ToString(), 150, "Key_2096", "Special_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killAssist.ToString(), 5, "Key_1143", "KillAssist"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.teamKill.ToString(), 10, "Key_1147", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.revenge.ToString(), 30, "Key_1206", "Revenge"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDown.ToString(), 100, "Key_1145", "TouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDouble.ToString(), 300, "Key_1195", "DoubleTouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.flagTouchDownTriple.ToString(), 500, "Key_1146", "TripleTouchDown"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill2.ToString(), 20, "Key_1135", "kill_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill3.ToString(), 30, "Key_1136", "kill_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill4.ToString(), 40, "Key_1137", "kill_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill5.ToString(), 50, "Key_1138", "kill_4"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill6.ToString(), 60, "Key_1139", "kill_5"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill10.ToString(), 100, "Key_1140", "kill_9"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill20.ToString(), 350, "Key_1141", "kill_19"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.multyKill50.ToString(), 1000, "Key_1142", "kill_49"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill2.ToString(), 10, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill3.ToString(), 15, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill4.ToString(), 20, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill5.ToString(), 25, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill6.ToString(), 30, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill10.ToString(), 50, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill20.ToString(), 175, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.killMultyKill50.ToString(), 500, "Key_1213", "Nemesis"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadGrenade.ToString(), 50, "Key_1134", "GrenadeKill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadExplosion.ToString(), 15, "Key_1127", "Kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.teamCapturePoint.ToString(), 50, "Key_1271", "TeamCapture"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.mySpotPoint.ToString(), 100, "Key_1272", "MySpot"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.unstoppablePoint.ToString(), 300, "Key_1273", "Unstoppable"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.monopolyPoint.ToString(), 500, "Key_1274", "Monopoly"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.houseKeeperPoint.ToString(), 10, "Key_1275", "HouseKeeper"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.defenderPoint.ToString(), 30, "Key_1276", "Defender"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.guardianPoint.ToString(), 50, "Key_1277", "Guardian"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.oneManArmyPoint.ToString(), 100, "Key_1278", "OneManArmy"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.suicide.ToString(), -50, "Key_2441", "Penalty"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.resurrection.ToString(), 0, "Key_2495", "Resurrection"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.deadDemon.ToString(), 40, "Key_2626", "demon_kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.blackMarked.ToString(), 100, "Key_2630", "blackmark_kill"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.pandoraSuccess.ToString(), 0, "Key_2562", "wrath_of_fate"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.pandoraFail.ToString(), 0, "Key_2562", "revenge_of_fate"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.barrierBreaker.ToString(), 5, "Key_2618", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.hellraiser.ToString(), 35, "Key_2619", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.renegade.ToString(), 40, "Key_2620", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.ricochet.ToString(), 30, "Key_2621", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.nuker.ToString(), 40, "Key_2622", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.illusionist.ToString(), 50, "Key_2623", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.coldShower.ToString(), 40, "Key_2624", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.joker.ToString(), 40, "Key_2627", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.mushroomer.ToString(), 40, "Key_2628", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.dragonSpirit.ToString(), 40, "Key_2629", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.tamer.ToString(), 25, "Key_2603", "killer_pet_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.packLeader.ToString(), 35, "Key_2604", "killer_pet_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.kingOfBeasts.ToString(), 85, "Key_2605", "killer_pet_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.petKnockout.ToString(), 5, "Key_2692", string.Empty));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.hunter.ToString(), 20, "Key_2607", "pet_kill_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.poacher.ToString(), 50, "Key_2608", "pet_kill_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.animalsFear.ToString(), 100, "Key_2609", "pet_kill_3"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.gadgetMaster.ToString(), 25, "Key_2614", "gadget_use_1"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.gadgetManiac.ToString(), 50, "Key_2615", "gadget_use_2"));
		PlayerEventScoreController._eventsScoreInfo.Add(new PlayerEventScoreController.ScoreEventInfo(PlayerEventScoreController.ScoreEvent.mechanist.ToString(), 100, "Key_2616", "gadget_use_3"));
	}

	// Token: 0x06002821 RID: 10273 RVA: 0x000C9794 File Offset: 0x000C7994
	public static void SetLocalizeForScoreEvent()
	{
		PlayerEventScoreController.scoreOnEvent.Clear();
		PlayerEventScoreController.messageOnEvent.Clear();
		PlayerEventScoreController.pictureNameOnEvent.Clear();
		PlayerEventScoreController.audioClipNameOnEvent.Clear();
		foreach (PlayerEventScoreController.ScoreEventInfo scoreEventInfo in PlayerEventScoreController._eventsScoreInfo)
		{
			PlayerEventScoreController.scoreOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.eventScore);
			PlayerEventScoreController.messageOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.eventMessage);
			PlayerEventScoreController.pictureNameOnEvent.Add(scoreEventInfo.eventId, scoreEventInfo.pictName);
			if (!string.IsNullOrEmpty(scoreEventInfo.audioClipName) && !PlayerEventScoreController.audioClipNameOnEvent.ContainsKey(scoreEventInfo.pictName))
			{
				PlayerEventScoreController.audioClipNameOnEvent.Add(scoreEventInfo.pictName, scoreEventInfo.audioClipName);
			}
		}
	}

	// Token: 0x04001C7A RID: 7290
	private static List<PlayerEventScoreController.ScoreEventInfo> _eventsScoreInfo = new List<PlayerEventScoreController.ScoreEventInfo>();

	// Token: 0x04001C7B RID: 7291
	public static Dictionary<string, int> scoreOnEvent = new Dictionary<string, int>();

	// Token: 0x04001C7C RID: 7292
	public static Dictionary<string, string> messageOnEvent = new Dictionary<string, string>();

	// Token: 0x04001C7D RID: 7293
	public static Dictionary<string, string> pictureNameOnEvent = new Dictionary<string, string>();

	// Token: 0x04001C7E RID: 7294
	public static Dictionary<string, string> audioClipNameOnEvent = new Dictionary<string, string>();

	// Token: 0x02000481 RID: 1153
	private class ScoreEventInfo
	{
		// Token: 0x06002822 RID: 10274 RVA: 0x000C9898 File Offset: 0x000C7A98
		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName)
		{
			this.eventId = _eventId;
			this.eventScore = _eventScore;
			this.eventMessage = _eventMessage;
			this.pictName = _pictName;
			this.audioClipName = string.Empty;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000C98D4 File Offset: 0x000C7AD4
		public ScoreEventInfo(string _eventId, int _eventScore, string _eventMessage, string _pictName, string _audioClipName)
		{
			this.eventId = _eventId;
			this.eventScore = _eventScore;
			this.eventMessage = _eventMessage;
			this.pictName = _pictName;
			this.audioClipName = _audioClipName;
		}

		// Token: 0x04001C7F RID: 7295
		public string eventId;

		// Token: 0x04001C80 RID: 7296
		public int eventScore;

		// Token: 0x04001C81 RID: 7297
		public string eventMessage;

		// Token: 0x04001C82 RID: 7298
		public string pictName;

		// Token: 0x04001C83 RID: 7299
		public string audioClipName;
	}

	// Token: 0x02000482 RID: 1154
	public enum ScoreEvent
	{
		// Token: 0x04001C85 RID: 7301
		damageBody,
		// Token: 0x04001C86 RID: 7302
		damageHead,
		// Token: 0x04001C87 RID: 7303
		damageMechBody,
		// Token: 0x04001C88 RID: 7304
		damageMechHead,
		// Token: 0x04001C89 RID: 7305
		damageTurret,
		// Token: 0x04001C8A RID: 7306
		damageExplosion,
		// Token: 0x04001C8B RID: 7307
		damageGrenade,
		// Token: 0x04001C8C RID: 7308
		deadMech,
		// Token: 0x04001C8D RID: 7309
		deadTurret,
		// Token: 0x04001C8E RID: 7310
		dead,
		// Token: 0x04001C8F RID: 7311
		deadHeadShot,
		// Token: 0x04001C90 RID: 7312
		deadLongShot,
		// Token: 0x04001C91 RID: 7313
		invisibleKill,
		// Token: 0x04001C92 RID: 7314
		doubleHeadShot,
		// Token: 0x04001C93 RID: 7315
		deadWithFlag,
		// Token: 0x04001C94 RID: 7316
		killAssist,
		// Token: 0x04001C95 RID: 7317
		teamKill,
		// Token: 0x04001C96 RID: 7318
		revenge,
		// Token: 0x04001C97 RID: 7319
		deathFromAbove,
		// Token: 0x04001C98 RID: 7320
		duckHunt,
		// Token: 0x04001C99 RID: 7321
		rocketJumpKill,
		// Token: 0x04001C9A RID: 7322
		melee,
		// Token: 0x04001C9B RID: 7323
		melee2,
		// Token: 0x04001C9C RID: 7324
		melee3,
		// Token: 0x04001C9D RID: 7325
		melee5,
		// Token: 0x04001C9E RID: 7326
		melee7,
		// Token: 0x04001C9F RID: 7327
		primary1,
		// Token: 0x04001CA0 RID: 7328
		primary2,
		// Token: 0x04001CA1 RID: 7329
		primary3,
		// Token: 0x04001CA2 RID: 7330
		backup1,
		// Token: 0x04001CA3 RID: 7331
		backup2,
		// Token: 0x04001CA4 RID: 7332
		backup3,
		// Token: 0x04001CA5 RID: 7333
		special1,
		// Token: 0x04001CA6 RID: 7334
		special2,
		// Token: 0x04001CA7 RID: 7335
		special3,
		// Token: 0x04001CA8 RID: 7336
		sniper1,
		// Token: 0x04001CA9 RID: 7337
		sniper2,
		// Token: 0x04001CAA RID: 7338
		sniper3,
		// Token: 0x04001CAB RID: 7339
		premium1,
		// Token: 0x04001CAC RID: 7340
		premium2,
		// Token: 0x04001CAD RID: 7341
		premium3,
		// Token: 0x04001CAE RID: 7342
		flagTouchDown,
		// Token: 0x04001CAF RID: 7343
		flagTouchDouble,
		// Token: 0x04001CB0 RID: 7344
		flagTouchDownTriple,
		// Token: 0x04001CB1 RID: 7345
		multyKill2,
		// Token: 0x04001CB2 RID: 7346
		multyKill3,
		// Token: 0x04001CB3 RID: 7347
		multyKill4,
		// Token: 0x04001CB4 RID: 7348
		multyKill5,
		// Token: 0x04001CB5 RID: 7349
		multyKill6,
		// Token: 0x04001CB6 RID: 7350
		multyKill10,
		// Token: 0x04001CB7 RID: 7351
		multyKill20,
		// Token: 0x04001CB8 RID: 7352
		multyKill50,
		// Token: 0x04001CB9 RID: 7353
		killMultyKill2,
		// Token: 0x04001CBA RID: 7354
		killMultyKill3,
		// Token: 0x04001CBB RID: 7355
		killMultyKill4,
		// Token: 0x04001CBC RID: 7356
		killMultyKill5,
		// Token: 0x04001CBD RID: 7357
		killMultyKill6,
		// Token: 0x04001CBE RID: 7358
		killMultyKill10,
		// Token: 0x04001CBF RID: 7359
		killMultyKill20,
		// Token: 0x04001CC0 RID: 7360
		killMultyKill50,
		// Token: 0x04001CC1 RID: 7361
		deadGrenade,
		// Token: 0x04001CC2 RID: 7362
		deadExplosion,
		// Token: 0x04001CC3 RID: 7363
		teamCapturePoint,
		// Token: 0x04001CC4 RID: 7364
		mySpotPoint,
		// Token: 0x04001CC5 RID: 7365
		unstoppablePoint,
		// Token: 0x04001CC6 RID: 7366
		monopolyPoint,
		// Token: 0x04001CC7 RID: 7367
		houseKeeperPoint,
		// Token: 0x04001CC8 RID: 7368
		defenderPoint,
		// Token: 0x04001CC9 RID: 7369
		guardianPoint,
		// Token: 0x04001CCA RID: 7370
		oneManArmyPoint,
		// Token: 0x04001CCB RID: 7371
		suicide,
		// Token: 0x04001CCC RID: 7372
		resurrection,
		// Token: 0x04001CCD RID: 7373
		deadDemon,
		// Token: 0x04001CCE RID: 7374
		blackMarked,
		// Token: 0x04001CCF RID: 7375
		pandoraSuccess,
		// Token: 0x04001CD0 RID: 7376
		pandoraFail,
		// Token: 0x04001CD1 RID: 7377
		barrierBreaker,
		// Token: 0x04001CD2 RID: 7378
		hellraiser,
		// Token: 0x04001CD3 RID: 7379
		renegade,
		// Token: 0x04001CD4 RID: 7380
		ricochet,
		// Token: 0x04001CD5 RID: 7381
		nuker,
		// Token: 0x04001CD6 RID: 7382
		illusionist,
		// Token: 0x04001CD7 RID: 7383
		coldShower,
		// Token: 0x04001CD8 RID: 7384
		joker,
		// Token: 0x04001CD9 RID: 7385
		mushroomer,
		// Token: 0x04001CDA RID: 7386
		dragonSpirit,
		// Token: 0x04001CDB RID: 7387
		tamer,
		// Token: 0x04001CDC RID: 7388
		packLeader,
		// Token: 0x04001CDD RID: 7389
		kingOfBeasts,
		// Token: 0x04001CDE RID: 7390
		hunter,
		// Token: 0x04001CDF RID: 7391
		poacher,
		// Token: 0x04001CE0 RID: 7392
		animalsFear,
		// Token: 0x04001CE1 RID: 7393
		gadgetMaster,
		// Token: 0x04001CE2 RID: 7394
		gadgetManiac,
		// Token: 0x04001CE3 RID: 7395
		mechanist,
		// Token: 0x04001CE4 RID: 7396
		none,
		// Token: 0x04001CE5 RID: 7397
		killPet,
		// Token: 0x04001CE6 RID: 7398
		petKnockout
	}
}
