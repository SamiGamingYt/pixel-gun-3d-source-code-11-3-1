using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B2 RID: 690
	[AddComponentMenu("I2/Localization/Source")]
	public class LanguageSource : MonoBehaviour
	{
		// Token: 0x06001585 RID: 5509 RVA: 0x00056C3C File Offset: 0x00054E3C
		public string Export_CSV(string Category)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.Append("Key,Type,Desc");
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append(",");
				LanguageSource.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code));
			}
			stringBuilder.Append("\n");
			foreach (TermData termData in this.mTerms)
			{
				string text;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSource.EmptyCategory && termData.Term.IndexOfAny(LanguageSource.CategorySeparators) < 0))
				{
					text = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category) || !(Category != termData.Term))
					{
						continue;
					}
					text = termData.Term.Substring(Category.Length + 1);
				}
				LanguageSource.AppendString(stringBuilder, text);
				stringBuilder.AppendFormat(",{0}", termData.TermType.ToString());
				stringBuilder.Append(",");
				LanguageSource.AppendString(stringBuilder, termData.Description);
				for (int i = 0; i < Mathf.Min(count, termData.Languages.Length); i++)
				{
					stringBuilder.Append(",");
					LanguageSource.AppendString(stringBuilder, termData.Languages[i]);
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00056E50 File Offset: 0x00055050
		private static void AppendString(StringBuilder Builder, string Text)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny(",\n\"".ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
			}
			else
			{
				Builder.Append(Text);
			}
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00056EC0 File Offset: 0x000550C0
		public WWW Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string value = this.Export_Google_CreateData();
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("key", this.Google_SpreadsheetKey);
			wwwform.AddField("action", "SetLanguageSource");
			wwwform.AddField("data", value);
			wwwform.AddField("updateMode", UpdateMode.ToString());
			return new WWW(this.Google_WebServiceURL, wwwform);
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x00056F2C File Offset: 0x0005512C
		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in categories)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("<I2Loc>");
				}
				string value = this.Export_CSV(text);
				stringBuilder.Append(text);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00056FE0 File Offset: 0x000551E0
		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> list = LocalizationReader.ReadCSV(CSVstring);
			string[] array = list[0];
			if (UpdateMode == eSpreadsheetUpdateMode.Replace)
			{
				this.ClearAllData();
			}
			int num = Mathf.Max(array.Length - 3, 0);
			int[] array2 = new int[num];
			for (int i = 0; i < num; i++)
			{
				string text;
				string code;
				GoogleLanguages.UnPackCodeFromLanguageName(array[i + 3], out text, out code);
				int num2 = this.GetLanguageIndex(text, true);
				if (num2 < 0)
				{
					LanguageData languageData = new LanguageData();
					languageData.Name = text;
					languageData.Code = code;
					this.mLanguages.Add(languageData);
					num2 = this.mLanguages.Count - 1;
				}
				array2[i] = num2;
			}
			num = this.mLanguages.Count;
			int j = 0;
			int count = this.mTerms.Count;
			while (j < count)
			{
				TermData termData = this.mTerms[j];
				if (termData.Languages.Length < num)
				{
					Array.Resize<string>(ref termData.Languages, num);
				}
				j++;
			}
			int k = 1;
			int count2 = list.Count;
			while (k < count2)
			{
				array = list[k];
				string text2 = (!string.IsNullOrEmpty(Category)) ? (Category + "/" + array[0]) : array[0];
				LanguageSource.ValidateFullTerm(ref text2);
				TermData termData2 = this.GetTermData(text2);
				if (termData2 == null)
				{
					termData2 = new TermData();
					termData2.Term = text2;
					termData2.Languages = new string[this.mLanguages.Count];
					for (int l = 0; l < this.mLanguages.Count; l++)
					{
						termData2.Languages[l] = string.Empty;
					}
					this.mTerms.Add(termData2);
					this.mDictionary.Add(text2, termData2);
					goto IL_1D4;
				}
				if (UpdateMode != eSpreadsheetUpdateMode.AddNewTerms)
				{
					goto IL_1D4;
				}
				IL_233:
				k++;
				continue;
				IL_1D4:
				termData2.TermType = LanguageSource.GetTermType(array[1]);
				termData2.Description = array[2];
				int num3 = 0;
				while (num3 < array2.Length && num3 < array.Length - 3)
				{
					if (!string.IsNullOrEmpty(array[num3 + 3]))
					{
						termData2.Languages[array2[num3]] = array[num3 + 3];
					}
					num3++;
				}
				goto IL_233;
			}
			return string.Empty;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x00057234 File Offset: 0x00055434
		public static eTermType GetTermType(string type)
		{
			int i = 0;
			int num = 8;
			while (i <= num)
			{
				if (string.Equals(((eTermType)i).ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
				i++;
			}
			return eTermType.Text;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00057270 File Offset: 0x00055470
		public void Import_Google()
		{
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00057280 File Offset: 0x00055480
		private IEnumerator Import_Google_Coroutine()
		{
			WWW www = this.Import_Google_CreateWWWcall(false);
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(www.error) && www.text != "\"\"")
			{
				PlayerPrefs.SetString("I2Source_" + this.Google_SpreadsheetKey, www.text);
				PlayerPrefs.Save();
				this.Import_Google_Result(www.text, eSpreadsheetUpdateMode.Replace);
				LocalizationManager.LocalizeAll();
				Debug.Log("Done Google Sync '" + www.text + "'");
			}
			else
			{
				Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			}
			UnityEngine.Object.Destroy(this.mCoroutineManager.gameObject);
			this.mCoroutineManager = null;
			yield break;
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0005729C File Offset: 0x0005549C
		public WWW Import_Google_CreateWWWcall(bool ForceUpdate = false)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			string url = string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", this.Google_WebServiceURL, this.Google_SpreadsheetKey, (!ForceUpdate) ? this.Google_LastUpdatedVersion : "0");
			return new WWW(url);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000572EC File Offset: 0x000554EC
		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey);
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x00057310 File Offset: 0x00055510
		public void Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode)
		{
			if (JsonString == "\"\"")
			{
				Debug.Log("Language Source was up to date");
				return;
			}
			if (UpdateMode == eSpreadsheetUpdateMode.Replace)
			{
				this.ClearAllData();
			}
			this.Import_CSV(string.Empty, JsonString, UpdateMode);
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00057354 File Offset: 0x00055554
		public List<string> GetCategories(bool OnlyMainCategory = false)
		{
			List<string> list = new List<string>();
			foreach (TermData termData in this.mTerms)
			{
				string categoryFromFullTerm = LanguageSource.GetCategoryFromFullTerm(termData.Term, OnlyMainCategory);
				if (!list.Contains(categoryFromFullTerm))
				{
					list.Add(categoryFromFullTerm);
				}
			}
			list.Sort();
			return list;
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x000573E0 File Offset: 0x000555E0
		internal static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			return (num >= 0) ? FullTerm.Substring(num + 1) : FullTerm;
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00057428 File Offset: 0x00055628
		internal static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			return (num >= 0) ? FullTerm.Substring(0, num) : LanguageSource.EmptyCategory;
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00057470 File Offset: 0x00055670
		internal static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (!OnlyMainCategory) ? FullTerm.LastIndexOfAny(LanguageSource.CategorySeparators) : FullTerm.IndexOfAny(LanguageSource.CategorySeparators);
			if (num < 0)
			{
				Category = LanguageSource.EmptyCategory;
				Key = FullTerm;
			}
			else
			{
				Category = FullTerm.Substring(0, num);
				Key = FullTerm.Substring(num + 1);
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000574CC File Offset: 0x000556CC
		private void Awake()
		{
			if (this.NeverDestroy)
			{
				if (this.ManagerHasASimilarSource())
				{
					UnityEngine.Object.Destroy(this);
					return;
				}
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			LocalizationManager.AddSource(this);
			this.UpdateDictionary();
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00057510 File Offset: 0x00055710
		public void UpdateDictionary()
		{
			this.mDictionary.Clear();
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				LanguageSource.ValidateFullTerm(ref this.mTerms[i].Term);
				this.mDictionary[this.mTerms[i].Term] = this.mTerms[i];
				i++;
			}
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00057584 File Offset: 0x00055784
		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform parent = base.transform.parent;
			while (parent)
			{
				text = parent.name + "_" + text;
				parent = parent.parent;
			}
			return text;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000575D4 File Offset: 0x000557D4
		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (string.Compare(this.mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if (LanguageSource.AreTheSameLanguage(this.mLanguages[j].Name, language))
					{
						return j;
					}
					j++;
				}
			}
			return -1;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00057664 File Offset: 0x00055864
		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSource.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSource.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00057684 File Offset: 0x00055884
		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x000576B8 File Offset: 0x000558B8
		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, true) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			this.mLanguages.Add(languageData);
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				Array.Resize<string>(ref this.mTerms[i].Languages, count);
				i++;
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00057738 File Offset: 0x00055938
		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, true);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					this.mTerms[i].Languages[j - 1] = this.mTerms[i].Languages[j];
				}
				Array.Resize<string>(ref this.mTerms[i].Languages, count - 1);
				i++;
			}
			this.mLanguages.RemoveAt(languageIndex);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000577E8 File Offset: 0x000559E8
		public List<string> GetLanguages()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				list.Add(this.mLanguages[i].Name);
				i++;
			}
			return list;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00057834 File Offset: 0x00055A34
		public string GetTermTranslation(string term)
		{
			int languageIndex = this.GetLanguageIndex(LocalizationManager.CurrentLanguage, true);
			if (languageIndex < 0)
			{
				return string.Empty;
			}
			TermData termData = this.GetTermData(term);
			if (termData != null)
			{
				return termData.Languages[languageIndex];
			}
			return string.Empty;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00057878 File Offset: 0x00055A78
		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text);
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00057884 File Offset: 0x00055A84
		public TermData GetTermData(string term)
		{
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary();
			}
			TermData result;
			this.mDictionary.TryGetValue(term, out result);
			return result;
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x000578B8 File Offset: 0x00055AB8
		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term) != null;
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x000578C8 File Offset: 0x00055AC8
		public List<string> GetTermsList()
		{
			return new List<string>(this.mDictionary.Keys);
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x000578DC File Offset: 0x00055ADC
		public TermData AddTerm(string NewTerm, eTermType termType)
		{
			LanguageSource.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			TermData termData = this.GetTermData(NewTerm);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[this.mLanguages.Count];
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x0005794C File Offset: 0x00055B4C
		public void RemoveTerm(string term)
		{
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				if (this.mTerms[i].Term == term)
				{
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
				}
				i++;
			}
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x000579B0 File Offset: 0x00055BB0
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSource.EmptyCategory) && Term.Length > LanguageSource.EmptyCategory.Length && Term[LanguageSource.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSource.EmptyCategory.Length + 1);
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00057A28 File Offset: 0x00055C28
		public bool IsEqualTo(LanguageSource Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true) < 0)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00057A94 File Offset: 0x00055C94
		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LanguageSource languageSource = LocalizationManager.Sources[i];
				if (languageSource != null && languageSource.IsEqualTo(this) && languageSource != this)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x00057AF4 File Offset: 0x00055CF4
		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x00057B18 File Offset: 0x00055D18
		public UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00057B80 File Offset: 0x00055D80
		public bool HasAsset(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x04000CD0 RID: 3280
		public string Google_WebServiceURL;

		// Token: 0x04000CD1 RID: 3281
		public string Google_SpreadsheetKey;

		// Token: 0x04000CD2 RID: 3282
		public string Google_SpreadsheetName;

		// Token: 0x04000CD3 RID: 3283
		public string Google_LastUpdatedVersion;

		// Token: 0x04000CD4 RID: 3284
		private CoroutineManager mCoroutineManager;

		// Token: 0x04000CD5 RID: 3285
		public static string EmptyCategory = "Default";

		// Token: 0x04000CD6 RID: 3286
		public static char[] CategorySeparators = "/\\".ToCharArray();

		// Token: 0x04000CD7 RID: 3287
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04000CD8 RID: 3288
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x04000CD9 RID: 3289
		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>();

		// Token: 0x04000CDA RID: 3290
		public UnityEngine.Object[] Assets;

		// Token: 0x04000CDB RID: 3291
		public bool NeverDestroy = true;

		// Token: 0x04000CDC RID: 3292
		public bool UserAgreesToHaveItOnTheScene;
	}
}
