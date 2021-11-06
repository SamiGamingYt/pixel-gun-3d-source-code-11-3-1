using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

// Token: 0x0200063F RID: 1599
public class GameCenterSingleton
{
	// Token: 0x06003727 RID: 14119 RVA: 0x0011CECC File Offset: 0x0011B0CC
	private GameCenterSingleton()
	{
	}

	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06003729 RID: 14121 RVA: 0x0011CF18 File Offset: 0x0011B118
	public static GameCenterSingleton Instance
	{
		get
		{
			if (GameCenterSingleton.instance == null)
			{
				GameCenterSingleton.instance = new GameCenterSingleton();
				GameCenterSingleton.instance.Initialize();
			}
			return GameCenterSingleton.instance;
		}
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x0011CF40 File Offset: 0x0011B140
	public void Initialize()
	{
		if (!this.IsUserAuthenticated())
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}
	}

	// Token: 0x0600372B RID: 14123 RVA: 0x0011CF64 File Offset: 0x0011B164
	public bool IsUserAuthenticated()
	{
		if (Social.localUser.authenticated)
		{
			return true;
		}
		Debug.Log("User not Authenticated");
		return false;
	}

	// Token: 0x0600372C RID: 14124 RVA: 0x0011CF84 File Offset: 0x0011B184
	public void ShowAchievementUI()
	{
		if (this.IsUserAuthenticated())
		{
			Social.ShowAchievementsUI();
		}
	}

	// Token: 0x0600372D RID: 14125 RVA: 0x0011CF98 File Offset: 0x0011B198
	public void ShowLeaderboardUI()
	{
		if (this.IsUserAuthenticated())
		{
			Social.ShowLeaderboardUI();
		}
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x0011CFAC File Offset: 0x0011B1AC
	public bool AddAchievementProgress(string achievementID, float percentageToAdd)
	{
		IAchievement achievement = this.GetAchievement(achievementID);
		if (achievement != null)
		{
			return this.ReportAchievementProgress(achievementID, (float)achievement.percentCompleted + percentageToAdd);
		}
		return this.ReportAchievementProgress(achievementID, percentageToAdd);
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x0011CFE0 File Offset: 0x0011B1E0
	public void ReportScore(long score, string tableName = null)
	{
		if (tableName == null)
		{
			tableName = GameCenterSingleton._leaderboardID;
		}
		Debug.Log(string.Concat(new object[]
		{
			"Reporting score ",
			score,
			" on leaderboard ",
			tableName
		}));
		Social.ReportScore(score, tableName, delegate(bool success)
		{
			Debug.Log((!success) ? "Failed to report score" : "Reported score successfully");
		});
	}

	// Token: 0x06003730 RID: 14128 RVA: 0x0011D04C File Offset: 0x0011B24C
	public void GetScore()
	{
		Social.LoadScores(GameCenterSingleton._leaderboardID, delegate(IScore[] scores)
		{
			if (scores.Length > 0)
			{
				Debug.Log("Got " + scores.Length + " scores");
				if (scores.Length > 0)
				{
					this.bestScore = scores[0].formattedValue;
					if (this.bestScore == null || this.bestScore.Equals(string.Empty))
					{
						this.bestScore = "0";
					}
				}
			}
			else
			{
				Debug.Log("No scores loaded");
			}
			this.bestScore = "0";
		});
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x0011D064 File Offset: 0x0011B264
	public bool ReportAchievementProgress(string achievementID, float progressCompleted)
	{
		if (!Social.localUser.authenticated)
		{
			Debug.Log("ERROR: GameCenter user not authenticated");
			return false;
		}
		if (!this.IsAchievementComplete(achievementID))
		{
			bool success = false;
			Social.ReportProgress(achievementID, (double)progressCompleted, delegate(bool result)
			{
				if (result)
				{
					success = true;
					this.LoadAchievements();
					Debug.Log("Successfully reported progress");
				}
				else
				{
					success = false;
					Debug.Log("Failed to report progress");
				}
			});
			return success;
		}
		return true;
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x0011D0C8 File Offset: 0x0011B2C8
	public void ResetAchievements()
	{
		GameCenterPlatform.ResetAllAchievements(new Action<bool>(this.ResetAchievementsHandler));
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x0011D0DC File Offset: 0x0011B2DC
	private void LoadAchievements()
	{
		Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x0011D0F0 File Offset: 0x0011B2F0
	private void ProcessAuthentication(bool success)
	{
		if (success)
		{
			Debug.Log("Authenticated, checking achievements");
			this.GetScore();
		}
		else
		{
			Debug.Log("Failed to authenticate");
		}
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x0011D118 File Offset: 0x0011B318
	private void ProcessLoadedAchievements(IAchievement[] achievements)
	{
		if (this.achievements != null)
		{
			this.achievements = null;
		}
		if (achievements.Length == 0)
		{
			Debug.Log("Error: no achievements found");
		}
		else
		{
			Debug.Log("Got " + achievements.Length + " achievements");
			this.achievements = achievements;
		}
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x0011D174 File Offset: 0x0011B374
	private bool IsAchievementComplete(string achievementID)
	{
		if (this.achievements != null)
		{
			foreach (IAchievement achievement in this.achievements)
			{
				if (achievement.id == achievementID && achievement.completed)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x0011D1CC File Offset: 0x0011B3CC
	private IAchievement GetAchievement(string achievementID)
	{
		if (this.achievements != null)
		{
			foreach (IAchievement achievement in this.achievements)
			{
				if (achievement.id == achievementID)
				{
					return achievement;
				}
			}
		}
		return null;
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x0011D218 File Offset: 0x0011B418
	private void ResetAchievementsHandler(bool status)
	{
		if (status)
		{
			if (this.achievements != null)
			{
				this.achievements = null;
			}
			this.LoadAchievements();
			Debug.Log("Achievements successfully resetted!");
		}
		else
		{
			Debug.Log("Achievements reset failure!");
		}
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x0011D254 File Offset: 0x0011B454
	public void updateGameCenter()
	{
		GameCenterSingleton.instance = new GameCenterSingleton();
		GameCenterSingleton.instance.Initialize();
	}

	// Token: 0x04002831 RID: 10289
	private static GameCenterSingleton instance;

	// Token: 0x04002832 RID: 10290
	private static string _leaderboardID = (!GlobalGameController.isFullVersion) ? "zombieslayerslite" : "zombieslayers";

	// Token: 0x04002833 RID: 10291
	public static string SurvivalTableID = "arena_heroes";

	// Token: 0x04002834 RID: 10292
	public string bestScore = "0";

	// Token: 0x04002835 RID: 10293
	private IAchievement[] achievements;
}
