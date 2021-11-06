using System;
using ExitGames.Client.Photon;

// Token: 0x02000453 RID: 1107
public static class ScoreExtensions
{
	// Token: 0x06002709 RID: 9993 RVA: 0x000C3BA4 File Offset: 0x000C1DA4
	public static void SetScore(this PhotonPlayer player, int newScore)
	{
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = newScore;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x000C3BD4 File Offset: 0x000C1DD4
	public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
	{
		int num = player.GetScore();
		num += scoreToAddToCurrent;
		Hashtable hashtable = new Hashtable();
		hashtable["score"] = num;
		player.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x000C3C0C File Offset: 0x000C1E0C
	public static int GetScore(this PhotonPlayer player)
	{
		object obj;
		if (player.customProperties.TryGetValue("score", out obj))
		{
			return (int)obj;
		}
		return 0;
	}
}
