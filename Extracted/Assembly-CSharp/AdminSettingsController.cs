using System;

// Token: 0x0200000B RID: 11
public class AdminSettingsController
{
	// Token: 0x06000028 RID: 40 RVA: 0x00002F24 File Offset: 0x00001124
	// Note: this type is marked as 'beforefieldinit'.
	static AdminSettingsController()
	{
		int[][] array = new int[3][];
		int num = 0;
		int[] array2 = new int[10];
		array2[0] = 2;
		array2[1] = 1;
		array2[2] = 1;
		array[num] = array2;
		int num2 = 1;
		int[] array3 = new int[10];
		array3[0] = 4;
		array3[1] = 2;
		array3[2] = 1;
		array[num2] = array3;
		int num3 = 2;
		int[] array4 = new int[10];
		array4[0] = 6;
		array4[1] = 4;
		array4[2] = 2;
		array[num3] = array4;
		AdminSettingsController.coinAvardDeathMath = array;
		AdminSettingsController.expAvardDeathMath = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				2,
				1,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				3,
				2,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				4,
				2,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreTeamFight = 50;
		int[][] array5 = new int[3][];
		int num4 = 0;
		int[] array6 = new int[10];
		array6[0] = 2;
		array6[1] = 1;
		array6[2] = 1;
		array5[num4] = array6;
		int num5 = 1;
		int[] array7 = new int[10];
		array7[0] = 4;
		array7[1] = 3;
		array7[2] = 2;
		array5[num5] = array7;
		int num6 = 2;
		int[] array8 = new int[10];
		array8[0] = 6;
		array8[1] = 4;
		array8[2] = 3;
		array5[num6] = array8;
		AdminSettingsController.coinAvardTeamFight = array5;
		AdminSettingsController.expAvardTeamFight = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreFlagCapture = 50;
		int[][] array9 = new int[3][];
		int num7 = 0;
		int[] array10 = new int[10];
		array10[0] = 2;
		array10[1] = 1;
		array10[2] = 1;
		array9[num7] = array10;
		int num8 = 1;
		int[] array11 = new int[10];
		array11[0] = 4;
		array11[1] = 3;
		array11[2] = 2;
		array9[num8] = array11;
		int num9 = 2;
		int[] array12 = new int[10];
		array12[0] = 6;
		array12[1] = 4;
		array12[2] = 3;
		array9[num9] = array12;
		AdminSettingsController.coinAvardFlagCapture = array9;
		AdminSettingsController.expAvardFlagCapture = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreTimeBattle = 2000;
		AdminSettingsController.coinAvardTimeBattle = new int[]
		{
			3,
			2,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		};
		AdminSettingsController.expAvardTimeBattle = new int[]
		{
			20,
			15,
			10,
			5,
			5,
			5,
			5,
			5,
			5,
			5
		};
		AdminSettingsController.coinAvardDeadlyGames = new int[]
		{
			0,
			2,
			3,
			4,
			5,
			6,
			8,
			10
		};
		AdminSettingsController.expAvardDeadlyGames = new int[]
		{
			0,
			10,
			10,
			11,
			12,
			13,
			14,
			15
		};
		AdminSettingsController.minScoreCapturePoint = 50;
		int[][] array13 = new int[3][];
		int num10 = 0;
		int[] array14 = new int[10];
		array14[0] = 2;
		array14[1] = 1;
		array14[2] = 1;
		array13[num10] = array14;
		int num11 = 1;
		int[] array15 = new int[10];
		array15[0] = 4;
		array15[1] = 3;
		array15[2] = 2;
		array13[num11] = array15;
		int num12 = 2;
		int[] array16 = new int[10];
		array16[0] = 6;
		array16[1] = 4;
		array16[2] = 3;
		array13[num12] = array16;
		AdminSettingsController.coinAvardCapturePoint = array13;
		AdminSettingsController.expAvardCapturePoint = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreDuel = 50;
		int[] array17 = new int[2];
		array17[0] = 2;
		AdminSettingsController.coinAvardDuel = array17;
		int[] array18 = new int[2];
		array18[0] = 10;
		AdminSettingsController.expAvardDuel = array18;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003234 File Offset: 0x00001434
	public static void ResetAvardSettingsOnDefault()
	{
		AdminSettingsController.minScoreDeathMath = 50;
		int[][] array = new int[3][];
		int num = 0;
		int[] array2 = new int[10];
		array2[0] = 2;
		array2[1] = 1;
		array2[2] = 1;
		array[num] = array2;
		int num2 = 1;
		int[] array3 = new int[10];
		array3[0] = 4;
		array3[1] = 2;
		array3[2] = 1;
		array[num2] = array3;
		int num3 = 2;
		int[] array4 = new int[10];
		array4[0] = 6;
		array4[1] = 4;
		array4[2] = 2;
		array[num3] = array4;
		AdminSettingsController.coinAvardDeathMath = array;
		AdminSettingsController.expAvardDeathMath = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				2,
				1,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				3,
				2,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				4,
				2,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreTeamFight = 50;
		int[][] array5 = new int[3][];
		int num4 = 0;
		int[] array6 = new int[10];
		array6[0] = 2;
		array6[1] = 1;
		array6[2] = 1;
		array5[num4] = array6;
		int num5 = 1;
		int[] array7 = new int[10];
		array7[0] = 4;
		array7[1] = 3;
		array7[2] = 2;
		array5[num5] = array7;
		int num6 = 2;
		int[] array8 = new int[10];
		array8[0] = 6;
		array8[1] = 4;
		array8[2] = 3;
		array5[num6] = array8;
		AdminSettingsController.coinAvardTeamFight = array5;
		AdminSettingsController.expAvardTeamFight = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreFlagCapture = 50;
		int[][] array9 = new int[3][];
		int num7 = 0;
		int[] array10 = new int[10];
		array10[0] = 2;
		array10[1] = 1;
		array10[2] = 1;
		array9[num7] = array10;
		int num8 = 1;
		int[] array11 = new int[10];
		array11[0] = 4;
		array11[1] = 3;
		array11[2] = 2;
		array9[num8] = array11;
		int num9 = 2;
		int[] array12 = new int[10];
		array12[0] = 6;
		array12[1] = 4;
		array12[2] = 3;
		array9[num9] = array12;
		AdminSettingsController.coinAvardFlagCapture = array9;
		AdminSettingsController.expAvardFlagCapture = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
		AdminSettingsController.minScoreTimeBattle = 2000;
		AdminSettingsController.coinAvardTimeBattle = new int[]
		{
			3,
			2,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		};
		AdminSettingsController.expAvardTimeBattle = new int[]
		{
			20,
			15,
			10,
			5,
			5,
			5,
			5,
			5,
			5,
			5
		};
		AdminSettingsController.coinAvardDeadlyGames = new int[]
		{
			0,
			2,
			3,
			4,
			5,
			6,
			8,
			10
		};
		AdminSettingsController.expAvardDeadlyGames = new int[]
		{
			0,
			10,
			10,
			11,
			12,
			13,
			14,
			15
		};
		AdminSettingsController.minScoreCapturePoint = 50;
		int[][] array13 = new int[3][];
		int num10 = 0;
		int[] array14 = new int[10];
		array14[0] = 2;
		array14[1] = 1;
		array14[2] = 1;
		array13[num10] = array14;
		int num11 = 1;
		int[] array15 = new int[10];
		array15[0] = 4;
		array15[1] = 3;
		array15[2] = 2;
		array13[num11] = array15;
		int num12 = 2;
		int[] array16 = new int[10];
		array16[0] = 6;
		array16[1] = 4;
		array16[2] = 3;
		array13[num12] = array16;
		AdminSettingsController.coinAvardCapturePoint = array13;
		AdminSettingsController.expAvardCapturePoint = new int[][]
		{
			new int[]
			{
				10,
				8,
				5,
				3,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				20,
				10,
				6,
				4,
				0,
				0,
				0,
				0,
				0,
				0
			},
			new int[]
			{
				30,
				15,
				10,
				6,
				0,
				0,
				0,
				0,
				0,
				0
			}
		};
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003520 File Offset: 0x00001720
	public static AdminSettingsController.Avard GetAvardAfterMatch(ConnectSceneNGUIController.RegimGame regim, int timeGame, int place, int score, int countKills, bool isWin)
	{
		AdminSettingsController.Avard result = default(AdminSettingsController.Avard);
		result.coin = 0;
		result.expierense = 0;
		if (regim == ConnectSceneNGUIController.RegimGame.Deathmatch)
		{
			if (score < AdminSettingsController.minScoreDeathMath)
			{
				return result;
			}
			switch (timeGame)
			{
			case 4:
				result.coin = AdminSettingsController.coinAvardDeathMath[0][place];
				result.expierense = AdminSettingsController.expAvardDeathMath[0][place];
				goto IL_DE;
			case 5:
				result.coin = AdminSettingsController.coinAvardDeathMath[1][place];
				result.expierense = AdminSettingsController.expAvardDeathMath[1][place];
				goto IL_DE;
			case 7:
				result.coin = AdminSettingsController.coinAvardDeathMath[2][place];
				result.expierense = AdminSettingsController.expAvardDeathMath[2][place];
				goto IL_DE;
			}
			result.coin = AdminSettingsController.coinAvardDeathMath[0][place];
			result.expierense = AdminSettingsController.expAvardDeathMath[0][place];
			IL_DE:
			result.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
			result.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
			return result;
		}
		else
		{
			if (regim == ConnectSceneNGUIController.RegimGame.TeamFight)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
				{
					place += 5;
				}
				bool flag = ExperienceController.sharedController.currentLevel < 2;
				if (ABTestController.useBuffSystem)
				{
					if ((!flag) ? (score < AdminSettingsController.minScoreTeamFight) : (!BuffSystem.instance.haveFirstInteractons || score < 5))
					{
						if (flag && score >= 5)
						{
							result.expierense = 3;
						}
						return result;
					}
				}
				else if (score < ((!flag) ? AdminSettingsController.minScoreTeamFight : 5))
				{
					return result;
				}
				switch (timeGame)
				{
				case 4:
					result.coin = AdminSettingsController.coinAvardTeamFight[0][place];
					result.expierense = AdminSettingsController.expAvardTeamFight[0][(!flag) ? place : 0];
					goto IL_284;
				case 5:
					result.coin = AdminSettingsController.coinAvardTeamFight[1][place];
					result.expierense = AdminSettingsController.expAvardTeamFight[1][(!flag) ? place : 0];
					goto IL_284;
				case 7:
					result.coin = AdminSettingsController.coinAvardTeamFight[2][place];
					result.expierense = AdminSettingsController.expAvardTeamFight[2][(!flag) ? place : 0];
					goto IL_284;
				}
				result.coin = AdminSettingsController.coinAvardTeamFight[0][place];
				result.expierense = AdminSettingsController.expAvardTeamFight[0][(!flag) ? place : 0];
				IL_284:
				result.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
				result.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
				return result;
			}
			if (regim == ConnectSceneNGUIController.RegimGame.FlagCapture)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
				{
					place += 5;
				}
				if (score < AdminSettingsController.minScoreFlagCapture)
				{
					return result;
				}
				switch (timeGame)
				{
				case 4:
					result.coin = AdminSettingsController.coinAvardFlagCapture[0][place];
					result.expierense = AdminSettingsController.expAvardFlagCapture[0][place];
					goto IL_38C;
				case 5:
					result.coin = AdminSettingsController.coinAvardFlagCapture[1][place];
					result.expierense = AdminSettingsController.expAvardFlagCapture[1][place];
					goto IL_38C;
				case 7:
					result.coin = AdminSettingsController.coinAvardFlagCapture[2][place];
					result.expierense = AdminSettingsController.expAvardFlagCapture[2][place];
					goto IL_38C;
				}
				result.coin = AdminSettingsController.coinAvardFlagCapture[0][place];
				result.expierense = AdminSettingsController.expAvardFlagCapture[0][place];
				IL_38C:
				result.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
				result.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
				return result;
			}
			else if (regim == ConnectSceneNGUIController.RegimGame.TimeBattle)
			{
				if (score < AdminSettingsController.minScoreTimeBattle)
				{
					return result;
				}
				result.coin = AdminSettingsController.coinAvardTimeBattle[place] * PremiumAccountController.Instance.RewardCoeff;
				result.expierense = AdminSettingsController.expAvardTimeBattle[place] * PremiumAccountController.Instance.RewardCoeff;
				return result;
			}
			else if (regim == ConnectSceneNGUIController.RegimGame.DeadlyGames)
			{
				if (!isWin || countKills < 0)
				{
					return result;
				}
				result.coin = AdminSettingsController.coinAvardDeadlyGames[countKills] * PremiumAccountController.Instance.RewardCoeff;
				result.expierense = AdminSettingsController.expAvardDeadlyGames[countKills] * PremiumAccountController.Instance.RewardCoeff;
				return result;
			}
			else if (regim == ConnectSceneNGUIController.RegimGame.CapturePoints)
			{
				if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B && !isWin)
				{
					place += 5;
				}
				if (score < AdminSettingsController.minScoreCapturePoint)
				{
					return result;
				}
				switch (timeGame)
				{
				case 4:
					result.coin = AdminSettingsController.coinAvardCapturePoint[0][place];
					result.expierense = AdminSettingsController.expAvardCapturePoint[0][place];
					goto IL_52A;
				case 5:
					result.coin = AdminSettingsController.coinAvardCapturePoint[1][place];
					result.expierense = AdminSettingsController.expAvardCapturePoint[1][place];
					goto IL_52A;
				case 7:
					result.coin = AdminSettingsController.coinAvardCapturePoint[2][place];
					result.expierense = AdminSettingsController.expAvardCapturePoint[2][place];
					goto IL_52A;
				}
				result.coin = AdminSettingsController.coinAvardCapturePoint[0][place];
				result.expierense = AdminSettingsController.expAvardCapturePoint[0][place];
				IL_52A:
				result.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
				result.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
				return result;
			}
			else
			{
				if (regim != ConnectSceneNGUIController.RegimGame.Duel)
				{
					return result;
				}
				if (score < AdminSettingsController.minScoreDeathMath)
				{
					return result;
				}
				result.coin = AdminSettingsController.coinAvardDuel[place];
				result.expierense = AdminSettingsController.expAvardDuel[place];
				result.coin *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(true);
				result.expierense *= AdminSettingsController.GetMultiplyerRewardWithBoostEvent(false);
				return result;
			}
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003ADC File Offset: 0x00001CDC
	public static int GetMultiplyerRewardWithBoostEvent(bool isMoney)
	{
		int num = 1;
		PromoActionsManager sharedManager = PromoActionsManager.sharedManager;
		PremiumAccountController instance = PremiumAccountController.Instance;
		int num2 = (!isMoney) ? sharedManager.DayOfValorMultiplyerForExp : sharedManager.DayOfValorMultiplyerForMoney;
		if (sharedManager.IsDayOfValorEventActive && instance.IsActiveOrWasActiveBeforeStartMatch())
		{
			num = num2 + instance.GetRewardCoeffByActiveOrActiveBeforeMatch();
		}
		else if (sharedManager.IsDayOfValorEventActive)
		{
			num *= num2;
		}
		else if (instance.IsActiveOrWasActiveBeforeStartMatch())
		{
			num *= instance.GetRewardCoeffByActiveOrActiveBeforeMatch();
		}
		return num;
	}

	// Token: 0x04000023 RID: 35
	public static int minScoreDeathMath = 50;

	// Token: 0x04000024 RID: 36
	public static int[][] coinAvardDeathMath;

	// Token: 0x04000025 RID: 37
	public static int[][] expAvardDeathMath;

	// Token: 0x04000026 RID: 38
	public static int minScoreTeamFight;

	// Token: 0x04000027 RID: 39
	public static int[][] coinAvardTeamFight;

	// Token: 0x04000028 RID: 40
	public static int[][] expAvardTeamFight;

	// Token: 0x04000029 RID: 41
	public static int minScoreFlagCapture;

	// Token: 0x0400002A RID: 42
	public static int[][] coinAvardFlagCapture;

	// Token: 0x0400002B RID: 43
	public static int[][] expAvardFlagCapture;

	// Token: 0x0400002C RID: 44
	public static int minScoreTimeBattle;

	// Token: 0x0400002D RID: 45
	public static int[] coinAvardTimeBattle;

	// Token: 0x0400002E RID: 46
	public static int[] expAvardTimeBattle;

	// Token: 0x0400002F RID: 47
	public static int[] coinAvardDeadlyGames;

	// Token: 0x04000030 RID: 48
	public static int[] expAvardDeadlyGames;

	// Token: 0x04000031 RID: 49
	public static int minScoreCapturePoint;

	// Token: 0x04000032 RID: 50
	public static int[][] coinAvardCapturePoint;

	// Token: 0x04000033 RID: 51
	public static int[][] expAvardCapturePoint;

	// Token: 0x04000034 RID: 52
	public static int minScoreDuel;

	// Token: 0x04000035 RID: 53
	public static int[] coinAvardDuel;

	// Token: 0x04000036 RID: 54
	public static int[] expAvardDuel;

	// Token: 0x0200000C RID: 12
	public struct Avard
	{
		// Token: 0x04000037 RID: 55
		public int coin;

		// Token: 0x04000038 RID: 56
		public int expierense;
	}
}
