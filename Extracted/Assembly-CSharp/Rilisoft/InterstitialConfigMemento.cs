using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rilisoft
{
	// Token: 0x0200053E RID: 1342
	[Serializable]
	internal sealed class InterstitialConfigMemento
	{
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06002EB6 RID: 11958 RVA: 0x000F4288 File Offset: 0x000F2488
		// (set) Token: 0x06002EB7 RID: 11959 RVA: 0x000F4290 File Offset: 0x000F2490
		private bool Enabled { get; set; }

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06002EB8 RID: 11960 RVA: 0x000F429C File Offset: 0x000F249C
		private HashSet<string> DisabledDevices
		{
			get
			{
				return this._disabledDevices;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000F42A4 File Offset: 0x000F24A4
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000F42AC File Offset: 0x000F24AC
		private double TimeoutBetweenShowInMinutes { get; set; }

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000F42B8 File Offset: 0x000F24B8
		private Dictionary<string, InterstitialOverrideMemento> Overrides
		{
			get
			{
				return this._overrides;
			}
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x000F42C0 File Offset: 0x000F24C0
		public bool GetEnabled(string category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			InterstitialOverrideMemento interstitialOverrideMemento;
			if (this.Overrides.TryGetValue(category, out interstitialOverrideMemento) && interstitialOverrideMemento != null)
			{
				bool? enabled = interstitialOverrideMemento.Enabled;
				return (enabled == null) ? this.Enabled : enabled.Value;
			}
			return this.Enabled;
		}

		// Token: 0x06002EBD RID: 11965 RVA: 0x000F4324 File Offset: 0x000F2524
		internal double GetTimeoutBetweenShowInMinutes(string category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			InterstitialOverrideMemento interstitialOverrideMemento;
			if (this.Overrides.TryGetValue(category, out interstitialOverrideMemento) && interstitialOverrideMemento != null)
			{
				double? timeoutBetweenShowInMinutes = interstitialOverrideMemento.TimeoutBetweenShowInMinutes;
				return (timeoutBetweenShowInMinutes == null) ? this.TimeoutBetweenShowInMinutes : timeoutBetweenShowInMinutes.Value;
			}
			return this.TimeoutBetweenShowInMinutes;
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x000F4388 File Offset: 0x000F2588
		public int GetDisabledReasonCode(string category, string device, double timeSpanSinceLastShowInMinutes)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			InterstitialOverrideMemento interstitialOverrideMemento;
			this.Overrides.TryGetValue(category, out interstitialOverrideMemento);
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.Enabled != null)
			{
				if (!interstitialOverrideMemento.Enabled.Value)
				{
					return 1;
				}
			}
			else if (!this.Enabled)
			{
				return 2;
			}
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.DisabledDevices != null)
			{
				if (interstitialOverrideMemento.DisabledDevices.Contains(device))
				{
					return 3;
				}
			}
			else if (this.DisabledDevices.Contains(device))
			{
				return 4;
			}
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.TimeoutBetweenShowInMinutes != null)
			{
				if (timeSpanSinceLastShowInMinutes < interstitialOverrideMemento.TimeoutBetweenShowInMinutes.Value)
				{
					return 5;
				}
			}
			else if (timeSpanSinceLastShowInMinutes < this.TimeoutBetweenShowInMinutes)
			{
				return 6;
			}
			return 0;
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x000F4488 File Offset: 0x000F2688
		public string GetDisabledReason(string category, string device, double timeSpanSinceLastShowInMinutes)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			if (device == null)
			{
				throw new ArgumentNullException("device");
			}
			InterstitialOverrideMemento interstitialOverrideMemento;
			this.Overrides.TryGetValue(category, out interstitialOverrideMemento);
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.Enabled != null)
			{
				if (!interstitialOverrideMemento.Enabled.Value)
				{
					return string.Format("Explicitely disabled for category `{0}`.", category);
				}
			}
			else if (!this.Enabled)
			{
				return "Just disabled.";
			}
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.DisabledDevices != null)
			{
				if (interstitialOverrideMemento.DisabledDevices.Contains(device))
				{
					return string.Format(CultureInfo.InvariantCulture, "Explicitely disabled for category `{0}` and device `{1}`.", new object[]
					{
						category,
						device
					});
				}
			}
			else if (this.DisabledDevices.Contains(device))
			{
				return string.Format(CultureInfo.InvariantCulture, "Disabled for device `{0}`.", new object[]
				{
					device
				});
			}
			if (interstitialOverrideMemento != null && interstitialOverrideMemento.TimeoutBetweenShowInMinutes != null)
			{
				if (timeSpanSinceLastShowInMinutes < interstitialOverrideMemento.TimeoutBetweenShowInMinutes.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "Overridden timeout for category `{0}`: {1:f2} < {2:f2}.", new object[]
					{
						category,
						timeSpanSinceLastShowInMinutes,
						interstitialOverrideMemento.TimeoutBetweenShowInMinutes.Value
					});
				}
			}
			else if (timeSpanSinceLastShowInMinutes < this.TimeoutBetweenShowInMinutes)
			{
				return string.Format(CultureInfo.InvariantCulture, "Timeout: {0:f2} < {1:f2}.", new object[]
				{
					timeSpanSinceLastShowInMinutes,
					this.TimeoutBetweenShowInMinutes
				});
			}
			return string.Empty;
		}

		// Token: 0x06002EC0 RID: 11968 RVA: 0x000F4630 File Offset: 0x000F2830
		internal static InterstitialConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			InterstitialConfigMemento interstitialConfigMemento = new InterstitialConfigMemento();
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabled");
			if (boolean != null)
			{
				interstitialConfigMemento.Enabled = boolean.Value;
			}
			List<object> list = ParsingHelper.GetObject(dictionary, "disableDevices") as List<object>;
			if (list != null)
			{
				foreach (object obj in list)
				{
					string text = obj as string;
					if (!string.IsNullOrEmpty(text))
					{
						interstitialConfigMemento.DisabledDevices.Add(text);
					}
				}
			}
			double? @double = ParsingHelper.GetDouble(dictionary, "timeoutBetweenShowInMinutes");
			interstitialConfigMemento.TimeoutBetweenShowInMinutes = ((@double == null) ? 15.0 : @double.Value);
			Dictionary<string, object> dictionary2 = ParsingHelper.GetObject(dictionary, "overrides") as Dictionary<string, object>;
			if (dictionary2 != null)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
					if (dictionary3 != null)
					{
						InterstitialOverrideMemento value = InterstitialOverrideMemento.FromDictionary(dictionary3);
						interstitialConfigMemento.Overrides[keyValuePair.Key] = value;
					}
				}
			}
			return interstitialConfigMemento;
		}

		// Token: 0x04002291 RID: 8849
		private readonly HashSet<string> _disabledDevices = new HashSet<string>();

		// Token: 0x04002292 RID: 8850
		private readonly Dictionary<string, InterstitialOverrideMemento> _overrides = new Dictionary<string, InterstitialOverrideMemento>();
	}
}
