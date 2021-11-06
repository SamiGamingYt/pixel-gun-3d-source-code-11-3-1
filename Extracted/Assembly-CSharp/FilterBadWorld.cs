using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public class FilterBadWorld
{
	// Token: 0x06004A08 RID: 18952 RVA: 0x0019AE7C File Offset: 0x0019907C
	public static void InitBadLists()
	{
		List<object> list = new List<object>();
		List<object> list2 = new List<object>();
		if (PlayerPrefs.HasKey("PixelFilterWordsKey"))
		{
			list = (Json.Deserialize(PlayerPrefs.GetString("PixelFilterWordsKey")) as List<object>);
		}
		if (PlayerPrefs.HasKey("PixelFilterSymbolsKey"))
		{
			list2 = (Json.Deserialize(PlayerPrefs.GetString("PixelFilterSymbolsKey")) as List<object>);
		}
		if (list != null)
		{
			FilterBadWorld.badWords = new string[FilterBadWorld.badWordsConst.Length + list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				FilterBadWorld.badWords[FilterBadWorld.badWordsConst.Length + i] = (string)list[i];
			}
		}
		else
		{
			FilterBadWorld.badWords = new string[FilterBadWorld.badWordsConst.Length];
		}
		FilterBadWorld.badWordsConst.CopyTo(FilterBadWorld.badWords, 0);
		if (list2 != null)
		{
			FilterBadWorld.badSymbols = new string[FilterBadWorld.badSymbolsConst.Length + list2.Count];
			for (int j = 0; j < list2.Count; j++)
			{
				FilterBadWorld.badSymbols[FilterBadWorld.badSymbolsConst.Length + j] = (string)list2[j];
			}
		}
		else
		{
			FilterBadWorld.badSymbols = new string[FilterBadWorld.badSymbolsConst.Length];
		}
		FilterBadWorld.badSymbolsConst.CopyTo(FilterBadWorld.badSymbols, 0);
		FilterBadWorld.badArraysInit = true;
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x0019AFD0 File Offset: 0x001991D0
	public static string FilterString(string inputStr)
	{
		if (!FilterBadWorld.badArraysInit)
		{
			FilterBadWorld.InitBadLists();
		}
		inputStr = NGUIText.StripSymbols(inputStr);
		string[] array = new string[]
		{
			".",
			",",
			"%",
			"!",
			"@",
			"#",
			"$",
			"*",
			"&",
			";",
			":",
			"?",
			"/",
			"<",
			">",
			"|",
			"-",
			"_",
			"\""
		};
		string text = inputStr;
		string text2 = string.Empty;
		for (int i = 0; i < array.Length; i++)
		{
			text = text.Replace(array[i], " ");
		}
		text = text.ToLower();
		int num = 0;
		int num2 = text.IndexOf(" ", num);
		while (num2 != -1)
		{
			if (FilterBadWorld.scanMatInWold(text.Substring(num, num2 - num)))
			{
				text2 = text2 + "***" + inputStr.Substring(num2, 1);
			}
			else
			{
				text2 += inputStr.Substring(num, num2 - num + 1);
			}
			num = num2 + 1;
			if (num <= text.Length - 1)
			{
				num2 = text.IndexOf(" ", num);
			}
			else
			{
				num2 = -1;
			}
		}
		if (num < text.Length)
		{
			if (FilterBadWorld.scanMatInWold(text.Substring(num, text.Length - num)))
			{
				text2 += "***";
			}
			else
			{
				text2 += inputStr.Substring(num, text.Length - num);
			}
		}
		for (int j = 0; j < FilterBadWorld.badSymbols.Length; j++)
		{
			text2 = text2.Replace(FilterBadWorld.badSymbols[j], "*");
		}
		return text2;
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x0019B1E4 File Offset: 0x001993E4
	private static bool scanMatInWold(string str)
	{
		if (str.Length < 3)
		{
			return false;
		}
		foreach (string text in FilterBadWorld.badWords)
		{
			if (text.Equals(str))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040036D5 RID: 14037
	private const string CensoredText = "***";

	// Token: 0x040036D6 RID: 14038
	private const string PatternTemplate = "\\b({0})(s?)\\b";

	// Token: 0x040036D7 RID: 14039
	private const RegexOptions Options = RegexOptions.IgnoreCase;

	// Token: 0x040036D8 RID: 14040
	private static string[] badWordsConst = new string[]
	{
		"drugs",
		"drugz",
		"alcohol",
		"penis",
		"vagina",
		"sexx",
		"sexxy",
		"boobs",
		"cumshot",
		"facial",
		"masturbate",
		"nipples",
		"orgasm",
		"slut",
		"porn",
		"porno",
		"pornography",
		"ass",
		"arse",
		"assbag",
		"assbandit",
		"assbanger",
		"assbite",
		"asscock",
		"assfuck",
		"asshead",
		"asshole",
		"asshopper",
		"asslicker",
		"assshole",
		"asswipe",
		"bampot",
		"bastard",
		"beaner",
		"bitch",
		"blow job",
		"blowjob",
		"boner",
		"brotherfucker",
		"bullshit",
		"butt plug",
		"butt-pirate",
		"buttfucker",
		"camel toe",
		"carpetmuncher",
		"chink",
		"choad",
		"chode",
		"clit",
		"cock",
		"cockbite",
		"cockface",
		"cockfucker",
		"cockmaster",
		"cockmongruel",
		"cockmuncher",
		"cocksmoker",
		"cocksucker",
		"coon",
		"cooter",
		"cracker",
		"cum",
		"cumtart",
		"cunnilingus",
		"cunt",
		"cunthole",
		"damn",
		"deggo",
		"dick",
		"dickbag",
		"dickhead",
		"dickhole",
		"dicks",
		"dickweed",
		"dickwod",
		"dildo",
		"dipshit",
		"dookie",
		"douche",
		"douchebag",
		"douchewaffle",
		"dumass",
		"dumb ass",
		"dumbass",
		"dumbfuck",
		"dumbshit",
		"dyke",
		"fag",
		"fagbag",
		"fagfucker",
		"faggit",
		"faggot",
		"fagtard",
		"fatass",
		"fellatio",
		"fuck",
		"fuckass",
		"fucked",
		"fucker",
		"fuckface",
		"fuckhead",
		"fuckhole",
		"fuckin",
		"fucking",
		"fucknut",
		"fucks",
		"fuckstick",
		"fucktard",
		"fuckup",
		"fuckwad",
		"fuckwit",
		"fudgepacker",
		"gay",
		"gaydo",
		"gaytard",
		"gaywad",
		"goddamn",
		"goddamnit",
		"gooch",
		"gook",
		"gringo",
		"guido",
		"hard on",
		"heeb",
		"hell",
		"ho",
		"homo",
		"homodumbshit",
		"honkey",
		"humping",
		"jackass",
		"jap",
		"jerk off",
		"jigaboo",
		"jizz",
		"jungle bunny",
		"kike",
		"kooch",
		"kootch",
		"kyke",
		"lesbian",
		"lesbo",
		"lezzie",
		"mcfagget",
		"mick",
		"minge",
		"mothafucka",
		"motherfucker",
		"motherfucking",
		"muff",
		"negro",
		"nigga",
		"nigger",
		"niglet",
		"nut sack",
		"nutsack",
		"paki",
		"panooch",
		"pecker",
		"peckerhead",
		"penis",
		"piss",
		"pissed",
		"pissed off",
		"pollock",
		"poon",
		"poonani",
		"poonany",
		"porch monkey",
		"porchmonkey",
		"prick",
		"punta",
		"pussy",
		"pussylicking",
		"puto",
		"queef",
		"queer",
		"queerbait",
		"renob",
		"rimjob",
		"sand nigger",
		"sandnigger",
		"schlong",
		"scrote",
		"shit",
		"shitcunt",
		"shitdick",
		"shitface",
		"shitfaced",
		"shithead",
		"shitter",
		"shittiest",
		"shitting",
		"shitty",
		"skank",
		"skeet",
		"slut",
		"slutbag",
		"snatch",
		"spic",
		"spick",
		"splooge",
		"tard",
		"testicle",
		"thundercunt",
		"tit",
		"titfuck",
		"tits",
		"twat",
		"twatlips",
		"twats",
		"twatwaffle",
		"va-j-j",
		"vag",
		"vjayjay",
		"wank",
		"wetback",
		"whore",
		"whorebag",
		"wop",
		"sex",
		"sexy"
	};

	// Token: 0x040036D9 RID: 14041
	private static string[] badSymbolsConst = new string[]
	{
		"卐",
		"卍"
	};

	// Token: 0x040036DA RID: 14042
	private static string[] badWords;

	// Token: 0x040036DB RID: 14043
	private static string[] badSymbols;

	// Token: 0x040036DC RID: 14044
	private static bool badArraysInit = false;
}
