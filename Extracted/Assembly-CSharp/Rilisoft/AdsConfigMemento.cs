using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft.MiniJson;

namespace Rilisoft
{
	// Token: 0x0200052E RID: 1326
	[Serializable]
	internal sealed class AdsConfigMemento
	{
		// Token: 0x06002E2F RID: 11823 RVA: 0x000F1D30 File Offset: 0x000EFF30
		private AdsConfigMemento(Exception exception)
		{
			this._exception = exception;
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000F1D4C File Offset: 0x000EFF4C
		internal static AdsConfigMemento FromJson(string json)
		{
			if (json == null)
			{
				return new AdsConfigMemento(new ArgumentNullException("json"));
			}
			if (json.Trim() == string.Empty)
			{
				return new AdsConfigMemento(new ArgumentException("Json is empty.", "json"));
			}
			object obj;
			try
			{
				obj = Json.Deserialize(json);
			}
			catch (Exception exception)
			{
				return new AdsConfigMemento(exception);
			}
			if (obj == null)
			{
				string message = string.Format(CultureInfo.InvariantCulture, "Failed to deserialize json: `{0}`", new object[]
				{
					json
				});
				return new AdsConfigMemento(new ArgumentException(message, "json"));
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				string message2 = string.Format(CultureInfo.InvariantCulture, "Failed to interpret json as dictionary: `{0}`", new object[]
				{
					json
				});
				return new AdsConfigMemento(new ArgumentException(message2, "json"));
			}
			AdsConfigMemento result;
			try
			{
				AdsConfigMemento adsConfigMemento = AdsConfigMemento.FromDictionary(dictionary);
				result = adsConfigMemento;
			}
			catch (Exception exception2)
			{
				result = new AdsConfigMemento(exception2);
			}
			return result;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x000F1E84 File Offset: 0x000F0084
		internal static AdsConfigMemento FromDictionary(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			AdsConfigMemento adsConfigMemento = new AdsConfigMemento(null);
			object obj;
			if (!dictionary.TryGetValue("cheater", out obj))
			{
				adsConfigMemento.CheaterConfig = new CheaterConfigMemento();
			}
			else
			{
				Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
				if (dictionary2 == null)
				{
					adsConfigMemento.CheaterConfig = new CheaterConfigMemento();
				}
				else
				{
					adsConfigMemento.CheaterConfig = CheaterConfigMemento.FromDictionary(dictionary2);
				}
			}
			object obj2;
			if (dictionary.TryGetValue("playerStates", out obj2))
			{
				Dictionary<string, object> dictionary3 = obj2 as Dictionary<string, object>;
				if (dictionary3 != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair in dictionary3)
					{
						Dictionary<string, object> dictionary4 = keyValuePair.Value as Dictionary<string, object>;
						if (dictionary4 != null)
						{
							adsConfigMemento.PlayerStates[keyValuePair.Key] = PlayerStateMemento.FromDictionary(keyValuePair.Key, dictionary4);
						}
					}
				}
			}
			object obj3;
			if (!dictionary.TryGetValue("interstitials", out obj3))
			{
				adsConfigMemento.InterstitialConfig = new InterstitialConfigMemento();
			}
			else
			{
				Dictionary<string, object> dictionary5 = obj3 as Dictionary<string, object>;
				if (dictionary5 == null)
				{
					adsConfigMemento.InterstitialConfig = new InterstitialConfigMemento();
				}
				else
				{
					adsConfigMemento.InterstitialConfig = InterstitialConfigMemento.FromDictionary(dictionary5);
				}
			}
			object obj4;
			if (!dictionary.TryGetValue("fakeInterstitials", out obj4))
			{
				adsConfigMemento.FakeInterstitialConfig = new FakeInterstitialConfigMemento();
			}
			else
			{
				Dictionary<string, object> dictionary6 = obj4 as Dictionary<string, object>;
				if (dictionary6 == null)
				{
					adsConfigMemento.FakeInterstitialConfig = new FakeInterstitialConfigMemento();
				}
				else
				{
					adsConfigMemento.FakeInterstitialConfig = FakeInterstitialConfigMemento.FromDictionary(dictionary6);
				}
			}
			object obj5;
			if (!dictionary.TryGetValue("video", out obj5))
			{
				adsConfigMemento.VideoConfig = new VideoConfigMemento();
			}
			else
			{
				Dictionary<string, object> dictionary7 = obj5 as Dictionary<string, object>;
				if (dictionary7 == null)
				{
					adsConfigMemento.VideoConfig = new VideoConfigMemento();
				}
				else
				{
					adsConfigMemento.VideoConfig = VideoConfigMemento.FromDictionary(dictionary7);
				}
			}
			object obj6;
			if (!dictionary.TryGetValue("points", out obj6))
			{
				adsConfigMemento.AdPointsConfig = new AdPointsConfigMemento();
			}
			else
			{
				Dictionary<string, object> dictionary8 = obj6 as Dictionary<string, object>;
				if (dictionary8 == null)
				{
					adsConfigMemento.AdPointsConfig = new AdPointsConfigMemento();
				}
				else
				{
					adsConfigMemento.AdPointsConfig = AdPointsConfigMemento.FromDictionary(dictionary8);
				}
			}
			return adsConfigMemento;
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000F20E8 File Offset: 0x000F02E8
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x000F20F0 File Offset: 0x000F02F0
		// (set) Token: 0x06002E34 RID: 11828 RVA: 0x000F20F8 File Offset: 0x000F02F8
		public CheaterConfigMemento CheaterConfig { get; private set; }

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002E35 RID: 11829 RVA: 0x000F2104 File Offset: 0x000F0304
		public Dictionary<string, PlayerStateMemento> PlayerStates
		{
			get
			{
				return this._playerStates;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06002E36 RID: 11830 RVA: 0x000F210C File Offset: 0x000F030C
		// (set) Token: 0x06002E37 RID: 11831 RVA: 0x000F2114 File Offset: 0x000F0314
		public InterstitialConfigMemento InterstitialConfig { get; private set; }

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06002E38 RID: 11832 RVA: 0x000F2120 File Offset: 0x000F0320
		// (set) Token: 0x06002E39 RID: 11833 RVA: 0x000F2128 File Offset: 0x000F0328
		public FakeInterstitialConfigMemento FakeInterstitialConfig { get; private set; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002E3A RID: 11834 RVA: 0x000F2134 File Offset: 0x000F0334
		// (set) Token: 0x06002E3B RID: 11835 RVA: 0x000F213C File Offset: 0x000F033C
		public VideoConfigMemento VideoConfig { get; private set; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000F2148 File Offset: 0x000F0348
		// (set) Token: 0x06002E3D RID: 11837 RVA: 0x000F2150 File Offset: 0x000F0350
		public AdPointsConfigMemento AdPointsConfig { get; private set; }

		// Token: 0x06002E3E RID: 11838 RVA: 0x000F215C File Offset: 0x000F035C
		private void TrySetException(Exception value)
		{
			if (this._exception != null)
			{
				return;
			}
			this._exception = value;
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000F2174 File Offset: 0x000F0374
		private static Exception CreateParsingException(string key)
		{
			string message = string.Format(CultureInfo.InvariantCulture, "Failed to interpret node as dictionary: `{0}`", new object[]
			{
				key
			});
			return new InvalidOperationException(message);
		}

		// Token: 0x04002251 RID: 8785
		private Exception _exception;

		// Token: 0x04002252 RID: 8786
		private readonly Dictionary<string, PlayerStateMemento> _playerStates = new Dictionary<string, PlayerStateMemento>();
	}
}
