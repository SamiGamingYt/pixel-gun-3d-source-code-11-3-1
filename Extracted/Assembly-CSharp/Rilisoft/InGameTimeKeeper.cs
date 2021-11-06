using System;
using System.Collections.Generic;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200053D RID: 1341
	[DisallowMultipleComponent]
	internal sealed class InGameTimeKeeper : MonoBehaviour
	{
		// Token: 0x06002EAC RID: 11948 RVA: 0x000F4058 File Offset: 0x000F2258
		private InGameTimeKeeper()
		{
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002EAD RID: 11949 RVA: 0x000F4060 File Offset: 0x000F2260
		internal static InGameTimeKeeper Instance
		{
			get
			{
				if (InGameTimeKeeper.s_instance == null)
				{
					InGameTimeKeeper.s_instance = CoroutineRunner.Instance.GetOrAddComponent<InGameTimeKeeper>();
				}
				return InGameTimeKeeper.s_instance;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06002EAE RID: 11950 RVA: 0x000F4094 File Offset: 0x000F2294
		internal TimeSpan CurrentInGameTime
		{
			get
			{
				return this.GetInGameTime(DateTime.UtcNow);
			}
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x000F40A4 File Offset: 0x000F22A4
		internal void Initialize()
		{
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000F40A8 File Offset: 0x000F22A8
		internal void Save()
		{
			DateTime utcNow = DateTime.UtcNow;
			TimeSpan inGameTime = this.GetInGameTime(utcNow);
			Dictionary<string, double> obj = new Dictionary<string, double>
			{
				{
					utcNow.ToString("yyyy-MM-dd"),
					inGameTime.TotalSeconds
				}
			};
			string value = Json.Serialize(obj);
			PlayerPrefs.SetString("DailyInGameTime", value);
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000F40FC File Offset: 0x000F22FC
		internal TimeSpan GetInGameTime(DateTime dateTime)
		{
			return (!(this._start.Date < dateTime.Date)) ? (dateTime - this._start + TimeSpan.FromSeconds(this._accumulatedInGameTimeSeconds)) : dateTime.TimeOfDay;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000F4150 File Offset: 0x000F2350
		private void Awake()
		{
			InGameTimeKeeper.s_instance = this;
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x000F4158 File Offset: 0x000F2358
		private void Start()
		{
			DateTime utcNow = DateTime.UtcNow;
			this._start = utcNow;
			string @string = PlayerPrefs.GetString("DailyInGameTime", string.Empty);
			Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
			if (dictionary == null)
			{
				this._accumulatedInGameTimeSeconds = 0.0;
			}
			else
			{
				string key = utcNow.ToString("yyyy-MM-dd");
				double accumulatedInGameTimeSeconds;
				if (dictionary.TryGetValue(key, out accumulatedInGameTimeSeconds))
				{
					this._accumulatedInGameTimeSeconds = accumulatedInGameTimeSeconds;
				}
				else
				{
					this._accumulatedInGameTimeSeconds = 0.0;
				}
			}
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x000F41E0 File Offset: 0x000F23E0
		private void OnApplicationPause(bool pause)
		{
			DateTime utcNow = DateTime.UtcNow;
			if (pause)
			{
				if (this._start.Date < utcNow.Date)
				{
					this._start = utcNow.Date;
					this._accumulatedInGameTimeSeconds = 0.0;
				}
				double totalSeconds = (utcNow - this._start).TotalSeconds;
				this._accumulatedInGameTimeSeconds += totalSeconds;
				this._start = utcNow;
			}
			else
			{
				this._start = utcNow;
			}
		}

		// Token: 0x0400228D RID: 8845
		private const string DailyInGameTimeKey = "DailyInGameTime";

		// Token: 0x0400228E RID: 8846
		private static InGameTimeKeeper s_instance;

		// Token: 0x0400228F RID: 8847
		private double _accumulatedInGameTimeSeconds;

		// Token: 0x04002290 RID: 8848
		private DateTime _start;
	}
}
