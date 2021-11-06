using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000532 RID: 1330
	[Serializable]
	internal sealed class FakeInterstitialConfigMemento
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002E59 RID: 11865 RVA: 0x000F28A4 File Offset: 0x000F0AA4
		public List<string> ImageUrls
		{
			get
			{
				return this._imageUrls;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002E5A RID: 11866 RVA: 0x000F28AC File Offset: 0x000F0AAC
		public List<string> RedirectUrls
		{
			get
			{
				return this._redirectUrls;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002E5B RID: 11867 RVA: 0x000F28B4 File Offset: 0x000F0AB4
		// (set) Token: 0x06002E5C RID: 11868 RVA: 0x000F28BC File Offset: 0x000F0ABC
		private bool Enabled { get; set; }

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002E5D RID: 11869 RVA: 0x000F28C8 File Offset: 0x000F0AC8
		// (set) Token: 0x06002E5E RID: 11870 RVA: 0x000F28D0 File Offset: 0x000F0AD0
		private int MinLevel { get; set; }

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002E5F RID: 11871 RVA: 0x000F28DC File Offset: 0x000F0ADC
		// (set) Token: 0x06002E60 RID: 11872 RVA: 0x000F28E4 File Offset: 0x000F0AE4
		private int MaxLevel { get; set; }

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002E61 RID: 11873 RVA: 0x000F28F0 File Offset: 0x000F0AF0
		// (set) Token: 0x06002E62 RID: 11874 RVA: 0x000F28F8 File Offset: 0x000F0AF8
		private int ShowFrequency { get; set; }

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002E63 RID: 11875 RVA: 0x000F2904 File Offset: 0x000F0B04
		// (set) Token: 0x06002E64 RID: 11876 RVA: 0x000F290C File Offset: 0x000F0B0C
		private int MaxShowCountDuringSession { get; set; }

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000F2918 File Offset: 0x000F0B18
		private Dictionary<string, Dictionary<string, object>> Overrides
		{
			get
			{
				return this._overrides;
			}
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000F2920 File Offset: 0x000F0B20
		internal static FakeInterstitialConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento = new FakeInterstitialConfigMemento();
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento2 = fakeInterstitialConfigMemento;
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabled");
			fakeInterstitialConfigMemento2.Enabled = (boolean != null && boolean.Value);
			List<object> list = ParsingHelper.GetObject(dictionary, "imageUrls") as List<object>;
			if (list != null)
			{
				foreach (object obj in list)
				{
					string text = obj as string;
					if (!string.IsNullOrEmpty(text))
					{
						fakeInterstitialConfigMemento.ImageUrls.Add(text);
					}
				}
			}
			List<object> list2 = ParsingHelper.GetObject(dictionary, "redirectUrls") as List<object>;
			if (list2 != null)
			{
				foreach (object obj2 in list2)
				{
					string text2 = obj2 as string;
					if (!string.IsNullOrEmpty(text2))
					{
						fakeInterstitialConfigMemento.RedirectUrls.Add(text2);
					}
				}
			}
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento3 = fakeInterstitialConfigMemento;
			int? @int = ParsingHelper.GetInt32(dictionary, "minLevel");
			fakeInterstitialConfigMemento3.MinLevel = ((@int == null) ? 1 : @int.Value);
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento4 = fakeInterstitialConfigMemento;
			int? int2 = ParsingHelper.GetInt32(dictionary, "maxLevel");
			fakeInterstitialConfigMemento4.MaxLevel = ((int2 == null) ? int.MaxValue : int2.Value);
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento5 = fakeInterstitialConfigMemento;
			int? int3 = ParsingHelper.GetInt32(dictionary, "showFrequency");
			fakeInterstitialConfigMemento5.ShowFrequency = ((int3 == null) ? 0 : int3.Value);
			FakeInterstitialConfigMemento fakeInterstitialConfigMemento6 = fakeInterstitialConfigMemento;
			int? int4 = ParsingHelper.GetInt32(dictionary, "maxShowCountDuringSession");
			fakeInterstitialConfigMemento6.MaxShowCountDuringSession = ((int4 == null) ? 0 : int4.Value);
			Dictionary<string, object> dictionary2 = ParsingHelper.GetObject(dictionary, "overrides") as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						fakeInterstitialConfigMemento.Overrides[keyValuePair.Key] = dictionary3;
					}
				}
			}
			return fakeInterstitialConfigMemento;
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x000F2BD0 File Offset: 0x000F0DD0
		internal string GetDisabledReason(string category, int level, int fakeInterstitialCount, int totalInterstitialCount, bool realInterstitialsEnabled)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			Dictionary<string, object> dictionary;
			if (!this.Overrides.TryGetValue(category, out dictionary))
			{
				dictionary = new Dictionary<string, object>();
			}
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabled");
			if (boolean != null)
			{
				if (!boolean.Value)
				{
					return string.Format("Explicitely disabled for category `{0}`.", category);
				}
			}
			else if (!this.Enabled)
			{
				return "Just disabled.";
			}
			if (this.ImageUrls.Count == 0)
			{
				return "Image URLs list is empty.";
			}
			if (this.RedirectUrls.Count == 0)
			{
				return "Redirect URLs list is empty";
			}
			int? @int = ParsingHelper.GetInt32(dictionary, "minLevel");
			if (@int != null)
			{
				if (level < @int.Value)
				{
					return string.Format("Level {0} < {1} for category `{2}`.", level, @int.Value, category);
				}
			}
			else if (level < this.MinLevel)
			{
				return string.Format("Level {0} < {1}.", level, @int.Value);
			}
			int? int2 = ParsingHelper.GetInt32(dictionary, "maxLevel");
			if (int2 != null)
			{
				if (level > int2.Value)
				{
					return string.Format("Level {0} > {1} for category `{2}`.", level, int2.Value, category);
				}
			}
			else if (level > this.MaxLevel)
			{
				return string.Format("Level {0} > {1}.", level, int2.Value);
			}
			int? int3 = ParsingHelper.GetInt32(dictionary, "showFrequency");
			if (realInterstitialsEnabled)
			{
				if (int3 != null)
				{
					if (int3.Value == 0)
					{
						return "showFrequencyOverride.Value == 0";
					}
					if (totalInterstitialCount % int3.Value != 0)
					{
						return string.Format("{0}: {1} % {2} != 0 for category `{3}`.", new object[]
						{
							"showFrequency",
							totalInterstitialCount,
							int3.Value,
							category
						});
					}
				}
				else
				{
					if (this.ShowFrequency == 0)
					{
						return "ShowFrequency == 0";
					}
					if (totalInterstitialCount % this.ShowFrequency != 0)
					{
						return string.Format("{0}: {1} % {2} != 0 ", "showFrequency", totalInterstitialCount, int3.Value);
					}
				}
			}
			int? int4 = ParsingHelper.GetInt32(dictionary, "maxShowCountDuringSession");
			if (int4 != null)
			{
				if (fakeInterstitialCount >= int4.Value)
				{
					return string.Format("{0}: {1} >= {2} for category `{3}`", new object[]
					{
						fakeInterstitialCount,
						int4.Value,
						"maxShowCountDuringSession",
						category
					});
				}
			}
			else if (fakeInterstitialCount >= this.MaxShowCountDuringSession)
			{
				return string.Format("{0}: {1} >= {2}", fakeInterstitialCount, int4.Value, "maxShowCountDuringSession");
			}
			return string.Empty;
		}

		// Token: 0x04002261 RID: 8801
		private const string MaxShowCountDuringSessionKey = "maxShowCountDuringSession";

		// Token: 0x04002262 RID: 8802
		private const string ShowFrequencyKey = "showFrequency";

		// Token: 0x04002263 RID: 8803
		private readonly List<string> _imageUrls = new List<string>();

		// Token: 0x04002264 RID: 8804
		private readonly List<string> _redirectUrls = new List<string>();

		// Token: 0x04002265 RID: 8805
		private readonly Dictionary<string, Dictionary<string, object>> _overrides = new Dictionary<string, Dictionary<string, object>>();
	}
}
