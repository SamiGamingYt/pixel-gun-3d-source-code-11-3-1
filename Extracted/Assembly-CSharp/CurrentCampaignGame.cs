using System;

// Token: 0x020005DA RID: 1498
public sealed class CurrentCampaignGame
{
	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x0600336F RID: 13167 RVA: 0x0010A658 File Offset: 0x00108858
	public static int currentLevel
	{
		get
		{
			if (Switcher.sceneNameToGameNum.ContainsKey(CurrentCampaignGame.levelSceneName))
			{
				return Switcher.sceneNameToGameNum[CurrentCampaignGame.levelSceneName];
			}
			return 0;
		}
	}

	// Token: 0x06003370 RID: 13168 RVA: 0x0010A680 File Offset: 0x00108880
	public static void ResetConditionParameters()
	{
		CurrentCampaignGame.withoutHits = true;
		CurrentCampaignGame.completeInTime = true;
	}

	// Token: 0x040025C3 RID: 9667
	public static string boXName = string.Empty;

	// Token: 0x040025C4 RID: 9668
	public static string levelSceneName = string.Empty;

	// Token: 0x040025C5 RID: 9669
	public static float _levelStartedAtTime;

	// Token: 0x040025C6 RID: 9670
	public static bool withoutHits;

	// Token: 0x040025C7 RID: 9671
	public static bool completeInTime;
}
