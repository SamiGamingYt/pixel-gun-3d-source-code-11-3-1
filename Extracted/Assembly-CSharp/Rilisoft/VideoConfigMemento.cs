using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Rilisoft
{
	// Token: 0x02000545 RID: 1349
	[Serializable]
	internal sealed class VideoConfigMemento
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002EF7 RID: 12023 RVA: 0x000F54F8 File Offset: 0x000F36F8
		// (set) Token: 0x06002EF8 RID: 12024 RVA: 0x000F5500 File Offset: 0x000F3700
		public double TimeoutWaitInSeconds { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x000F550C File Offset: 0x000F370C
		// (set) Token: 0x06002EFA RID: 12026 RVA: 0x000F5514 File Offset: 0x000F3714
		private bool Enabled { get; set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002EFB RID: 12027 RVA: 0x000F5520 File Offset: 0x000F3720
		private HashSet<string> DisabledDevices
		{
			get
			{
				return this._disabledDevices;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000F5528 File Offset: 0x000F3728
		private Dictionary<string, Dictionary<string, object>> Overrides
		{
			get
			{
				return this._overrides;
			}
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x000F5530 File Offset: 0x000F3730
		public string GetDisabledReason(string category, string device)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (device == null)
			{
				throw new ArgumentNullException("device");
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
			List<object> list = ParsingHelper.GetObject(dictionary, "disableDevices") as List<object>;
			if (list != null && list.OfType<string>().Any((string s) => device == s))
			{
				return string.Format(CultureInfo.InvariantCulture, "Explicitely disabled for category `{0}` and device `{1}`.", new object[]
				{
					category,
					device
				});
			}
			if (this.DisabledDevices.Contains(device))
			{
				return string.Format(CultureInfo.InvariantCulture, "Disabled for device `{0}`.", new object[]
				{
					device
				});
			}
			return string.Empty;
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000F5660 File Offset: 0x000F3860
		internal static VideoConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			VideoConfigMemento videoConfigMemento = new VideoConfigMemento();
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabled");
			if (boolean != null)
			{
				videoConfigMemento.Enabled = boolean.Value;
			}
			List<object> list = ParsingHelper.GetObject(dictionary, "disableDevices") as List<object>;
			if (list != null)
			{
				foreach (object obj in list)
				{
					string text = obj as string;
					if (!string.IsNullOrEmpty(text))
					{
						videoConfigMemento.DisabledDevices.Add(text);
					}
				}
			}
			double? @double = ParsingHelper.GetDouble(dictionary, "timeoutWaitSeconds");
			videoConfigMemento.TimeoutWaitInSeconds = ((@double == null) ? 7.0 : @double.Value);
			Dictionary<string, object> dictionary2 = ParsingHelper.GetObject(dictionary, "overrides") as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						videoConfigMemento.Overrides[keyValuePair.Key] = dictionary3;
					}
				}
			}
			return videoConfigMemento;
		}

		// Token: 0x040022AB RID: 8875
		private readonly HashSet<string> _disabledDevices = new HashSet<string>();

		// Token: 0x040022AC RID: 8876
		private readonly Dictionary<string, Dictionary<string, object>> _overrides = new Dictionary<string, Dictionary<string, object>>();
	}
}
