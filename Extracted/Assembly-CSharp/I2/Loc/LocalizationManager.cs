using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002BE RID: 702
	public static class LocalizationManager
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600161F RID: 5663 RVA: 0x00059394 File Offset: 0x00057594
		// (remove) Token: 0x06001620 RID: 5664 RVA: 0x000593AC File Offset: 0x000575AC
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001621 RID: 5665 RVA: 0x000593C4 File Offset: 0x000575C4
		// (set) Token: 0x06001622 RID: 5666 RVA: 0x000593EC File Offset: 0x000575EC
		public static string CurrentLanguage
		{
			get
			{
				if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage))
				{
					LocalizationManager.RegisterSceneSources();
					LocalizationManager.RegisterSourceInResources();
					LocalizationManager.SelectStartupLanguage();
				}
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value);
				if (LocalizationManager.mCurrentLanguage != value && !string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.mCurrentLanguage = supportedLanguage;
					LocalizationManager.CurrentLanguageCode = LocalizationManager.GetLanguageCode(supportedLanguage);
					LocalizationManager.LocalizeAll();
				}
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001623 RID: 5667 RVA: 0x00059434 File Offset: 0x00057634
		// (set) Token: 0x06001624 RID: 5668 RVA: 0x0005943C File Offset: 0x0005763C
		public static string CurrentLanguageCode
		{
			get
			{
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.mLanguageCode = value;
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			}
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x00059454 File Offset: 0x00057654
		private static void SelectStartupLanguage()
		{
			string @string = PlayerPrefs.GetString(Defs.CurrentLanguage, string.Empty);
			string language = Application.systemLanguage.ToString();
			if (LocalizationManager.HasLanguage(@string, true))
			{
				LocalizationManager.CurrentLanguage = @string;
				return;
			}
			string supportedLanguage = LocalizationManager.GetSupportedLanguage(language);
			if (!string.IsNullOrEmpty(supportedLanguage))
			{
				LocalizationManager.CurrentLanguage = supportedLanguage;
				return;
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					LocalizationManager.CurrentLanguage = LocalizationManager.Sources[i].mLanguages[0].Name;
					return;
				}
				i++;
			}
		}

		// Token: 0x06001626 RID: 5670 RVA: 0x00059508 File Offset: 0x00057708
		public static string GetTermTranslation(string Term)
		{
			return LocalizationManager.GetTranslation(Term);
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x00059510 File Offset: 0x00057710
		public static string GetTermTranslationByDefault(string term)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.RegisterSourceInResources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term);
				if (termData == null)
				{
					return string.Empty;
				}
				if (termData.Languages.Length != 0)
				{
					return termData.Languages[0];
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x00059588 File Offset: 0x00057788
		public static string GetTranslation(string term)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term);
				if (termData != null)
				{
					int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
					if (languageIndex != -1)
					{
						string text = termData.Languages[languageIndex];
						if (!string.IsNullOrEmpty(text))
						{
							return text;
						}
					}
					if (termData.Languages.Length != 0)
					{
						return termData.Languages[0];
					}
				}
				i++;
			}
			return term;
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x00059620 File Offset: 0x00057820
		internal static void LocalizeAll()
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				Localize localize = array[i];
				localize.OnLocalize();
				i++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
			ResourceManager.pInstance.CleanResourceCache();
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x00059680 File Offset: 0x00057880
		private static void RegisterSceneSources()
		{
			LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				if (!LocalizationManager.Sources.Contains(array[i]))
				{
					LocalizationManager.AddSource(array[i]);
				}
				i++;
			}
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x000596D4 File Offset: 0x000578D4
		private static void RegisterSourceInResources()
		{
			GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>("I2Languages");
			LanguageSource languageSource = (!asset) ? null : asset.GetComponent<LanguageSource>();
			if (languageSource && !LocalizationManager.Sources.Contains(languageSource))
			{
				LocalizationManager.AddSource(languageSource);
			}
		}

		// Token: 0x0600162C RID: 5676 RVA: 0x0005972C File Offset: 0x0005792C
		internal static void AddSource(LanguageSource Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			Source.Import_Google();
		}

		// Token: 0x0600162D RID: 5677 RVA: 0x0005975C File Offset: 0x0005795C
		internal static void RemoveSource(LanguageSource Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x0600162E RID: 5678 RVA: 0x0005976C File Offset: 0x0005796C
		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false) >= 0)
				{
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true) >= 0)
					{
						return true;
					}
					j++;
				}
			}
			return false;
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x000597F0 File Offset: 0x000579F0
		public static string GetSupportedLanguage(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, false);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Name;
				}
				i++;
			}
			int j = 0;
			int count2 = LocalizationManager.Sources.Count;
			while (j < count2)
			{
				int languageIndex2 = LocalizationManager.Sources[j].GetLanguageIndex(Language, true);
				if (languageIndex2 >= 0)
				{
					return LocalizationManager.Sources[j].mLanguages[languageIndex2].Name;
				}
				j++;
			}
			return string.Empty;
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x000598B0 File Offset: 0x00057AB0
		public static string GetLanguageCode(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00059918 File Offset: 0x00057B18
		public static List<string> GetAllLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources[i].mLanguages.Count;
				while (j < count2)
				{
					if (!list.Contains(LocalizationManager.Sources[i].mLanguages[j].Name))
					{
						list.Add(LocalizationManager.Sources[i].mLanguages[j].Name);
					}
					j++;
				}
				i++;
			}
			return list;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000599BC File Offset: 0x00057BBC
		public static UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
				{
					return @object;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00059A08 File Offset: 0x00057C08
		private static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x04000D00 RID: 3328
		private static string mCurrentLanguage;

		// Token: 0x04000D01 RID: 3329
		private static string mLanguageCode;

		// Token: 0x04000D02 RID: 3330
		public static bool IsRight2Left = false;

		// Token: 0x04000D03 RID: 3331
		public static List<LanguageSource> Sources = new List<LanguageSource>();

		// Token: 0x04000D04 RID: 3332
		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ",
			"ar",
			"ar-BH",
			"ar-EG",
			"ar-IQ",
			"ar-JO",
			"ar-KW",
			"ar-LB",
			"ar-LY",
			"ar-MA",
			"ar-OM",
			"ar-QA",
			"ar-SA",
			"ar-SY",
			"ar-TN",
			"ar-AE",
			"ar-YE",
			"he",
			"ur",
			"ji"
		};

		// Token: 0x020008E5 RID: 2277
		// (Invoke) Token: 0x06005034 RID: 20532
		public delegate void OnLocalizeCallback();
	}
}
