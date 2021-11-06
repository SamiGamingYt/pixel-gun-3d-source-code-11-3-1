using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020005BA RID: 1466
internal sealed class ChestBonusModel
{
	// Token: 0x060032AB RID: 12971 RVA: 0x00106970 File Offset: 0x00104B70
	public static string GetUrlForDownloadBonusesData()
	{
		string arg = string.Empty;
		if (Defs.IsDeveloperBuild)
		{
			arg = "chest_bonus_test.json";
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				arg = "chest_bonus_amazon.json";
			}
			else
			{
				arg = "chest_bonus_android.json";
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			arg = "chest_bonus_wp8.json";
		}
		else
		{
			arg = "chest_bonus_ios.json";
		}
		return string.Format("{0}{1}", "https://secure.pixelgunserver.com/pixelgun3d-config/chestBonus/", arg);
	}

	// Token: 0x04002536 RID: 9526
	public const string pathToCommonBonusItems = "Textures/Bank/StarterPack_Weapon";
}
