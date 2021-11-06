using System;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public class PlayerPrefsX
{
	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x06004BAA RID: 19370 RVA: 0x001B3B70 File Offset: 0x001B1D70
	public static string SndSetting
	{
		get
		{
			return "SoundMusicSetting";
		}
	}

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x06004BAB RID: 19371 RVA: 0x001B3B78 File Offset: 0x001B1D78
	public static string SoundMusicSetting
	{
		get
		{
			return "SoundMusicSetting";
		}
	}

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x06004BAC RID: 19372 RVA: 0x001B3B80 File Offset: 0x001B1D80
	public static string SoundFXSetting
	{
		get
		{
			return "SoundFXSetting";
		}
	}

	// Token: 0x06004BAD RID: 19373 RVA: 0x001B3B88 File Offset: 0x001B1D88
	public static void SetBool(string name, bool booleanValue)
	{
		PlayerPrefs.SetInt(name, (!booleanValue) ? 0 : 1);
	}

	// Token: 0x06004BAE RID: 19374 RVA: 0x001B3BA0 File Offset: 0x001B1DA0
	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1;
	}

	// Token: 0x06004BAF RID: 19375 RVA: 0x001B3BB8 File Offset: 0x001B1DB8
	public static bool GetBool(string name, bool defaultValue)
	{
		if (PlayerPrefs.HasKey(name))
		{
			return PlayerPrefsX.GetBool(name);
		}
		return defaultValue;
	}
}
