using System;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200054E RID: 1358
	internal sealed class AnalyticsFacade
	{
		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002F1C RID: 12060 RVA: 0x000F6064 File Offset: 0x000F4264
		private static Dictionary<string, string> RecyclingFlurryParameters
		{
			get
			{
				return AnalyticsFacade._recyclingFlurryParameters;
			}
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000F606C File Offset: 0x000F426C
		public static void Initialize()
		{
			if (AnalyticsFacade._initialized)
			{
				return;
			}
			if (MiscAppsMenu.Instance == null)
			{
				Debug.LogError("MiscAppsMenu.Instance == null");
				return;
			}
			if (MiscAppsMenu.Instance.misc == null)
			{
				Debug.LogError("MiscAppsMenu.Instance.misc == null");
				return;
			}
			try
			{
				HiddenSettings misc = MiscAppsMenu.Instance.misc;
				AnalyticsFacade.DuplicateToConsoleByDefault = Defs.IsDeveloperBuild;
				AnalyticsFacade.LoggingEnabled = Defs.IsDeveloperBuild;
				string text = string.Empty;
				string text2 = string.Empty;
				if (Defs.IsDeveloperBuild || Application.isEditor)
				{
					switch (BuildSettings.BuildTargetPlatform)
					{
					case RuntimePlatform.IPhonePlayer:
						text = "92002d69-82d8-067e-997d-88d1c5e804f7";
						text2 = "tQ4zhKGBvyFVObPUofaiHj7pSAcWn3Mw";
						break;
					case RuntimePlatform.Android:
						text = "8517441f-d330-04c5-b621-5d88e92f50e3";
						text2 = "xkjaPTLIgGQKs5MftquXrEHDW0y8OBAS";
						break;
					}
				}
				else
				{
					RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
					switch (buildTargetPlatform)
					{
					case RuntimePlatform.IPhonePlayer:
						text = "3c77b196-8042-0dab-a5dc-92eb4377aa8e";
						text2 = misc.devtodevSecretIos;
						break;
					default:
						if (buildTargetPlatform == RuntimePlatform.MetroPlayerX64)
						{
							text = "cd19ad66-971e-09b2-b449-ba84d3fb52d8";
							text2 = misc.devtodevSecretWsa;
						}
						break;
					case RuntimePlatform.Android:
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							text = "8d1482db-5181-0647-a80e-decf21db619f";
							text2 = misc.devtodevSecretGoogle;
						}
						else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
						{
							text = "531e6d54-b959-06c1-8a38-6dfdfbf309eb";
							text2 = misc.devtodevSecretAmazon;
						}
						break;
					}
				}
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Initializing DevtoDev {0}; appId: '*{1}', appSecret: '*{2}'...", new object[]
					{
						DevToDevFacade.Version,
						text.Substring(Math.Max(text.Length - 4, 0)),
						text2.Substring(Math.Max(text2.Length - 4, 0))
					});
				}
				AnalyticsFacade.InitializeDevToDev(text, text2);
				string text3 = string.Empty;
				string appsFlyerAppKey = misc.appsFlyerAppKey;
				if (!Defs.IsDeveloperBuild && !Application.isEditor)
				{
					switch (BuildSettings.BuildTargetPlatform)
					{
					case RuntimePlatform.IPhonePlayer:
						text3 = "ecd1e376-8e2f-45e4-a9dc-9e938f999d20";
						break;
					case RuntimePlatform.Android:
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							text3 = "com.pixel.gun3d";
						}
						else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
						{
							text3 = "com.PixelGun.a3D";
						}
						break;
					}
				}
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Initializing AppsFlyer; appsFlyerAppKey: '*{0}', appsFlyerAppId: '*{1}'...", new object[]
					{
						appsFlyerAppKey.Substring(Math.Max(appsFlyerAppKey.Length - 4, 0)),
						text3.Substring(Math.Max(text3.Length - 4, 0))
					});
				}
				AnalyticsFacade.InitializeAppsFlyer(appsFlyerAppKey, text3);
				AnalyticsFacade.InitializeFacebook();
				AnalyticsFacade.InitializeFlurry(AnalyticsFacade.GetFlurryApiKey(misc));
				AnalyticsFacade._initialized = true;
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in AnalyticsFacade.Initialize: " + arg);
			}
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000F6354 File Offset: 0x000F4554
		private static string GetFlurryApiKey(HiddenSettings settings)
		{
			bool flag = Defs.IsDeveloperBuild || Application.isEditor;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return (!flag) ? settings.flurryIosApiKey : settings.flurryIosDevApiKey;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
				if (androidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return (!flag) ? settings.flurryAmazonApiKey : settings.flurryAmazonDevApiKey;
				}
				if (androidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return (!flag) ? settings.flurryAndroidApiKey : settings.flurryAndroidDevApiKey;
				}
			}
			return string.Empty;
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002F1F RID: 12063 RVA: 0x000F63F4 File Offset: 0x000F45F4
		// (set) Token: 0x06002F20 RID: 12064 RVA: 0x000F63FC File Offset: 0x000F45FC
		public static bool DuplicateToConsoleByDefault { get; set; }

		// Token: 0x1700082B RID: 2091
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000F6404 File Offset: 0x000F4604
		public static bool LoggingEnabled
		{
			set
			{
				DevToDevFacade.LoggingEnabled = value;
				AppsFlyerFacade.LoggingEnabled = value;
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x000F6414 File Offset: 0x000F4614
		public static void SendCustomEvent(string eventName)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEvent(eventName, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x000F6428 File Offset: 0x000F4628
		public static void SendCustomEvent(string eventName, IDictionary<string, object> eventParams)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEvent(eventName, eventParams, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000F643C File Offset: 0x000F463C
		public static void SendCustomEvent(string eventName, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendCustomEvent(eventName);
			}
			if (AnalyticsFacade._flurryFacade != null)
			{
				AnalyticsFacade._flurryFacade.LogEvent(eventName);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._simpleEventFormat.Value, new object[]
				{
					eventName
				});
			}
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x000F64A8 File Offset: 0x000F46A8
		public static void SendCustomEvent(string eventName, IDictionary<string, object> eventParams, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendCustomEvent(eventName, eventParams);
			}
			if (AnalyticsFacade._flurryFacade != null)
			{
				AnalyticsFacade.RecyclingFlurryParameters.Clear();
				foreach (KeyValuePair<string, object> keyValuePair in eventParams)
				{
					AnalyticsFacade.RecyclingFlurryParameters[keyValuePair.Key] = keyValuePair.Value.ToString();
				}
				AnalyticsFacade._flurryFacade.LogEventWithParameters(eventName, AnalyticsFacade.RecyclingFlurryParameters);
				AnalyticsFacade.RecyclingFlurryParameters.Clear();
			}
			if (duplicateToConsole)
			{
				string text = Json.Serialize(eventParams);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					eventName,
					text
				});
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x000F65B4 File Offset: 0x000F47B4
		public static void SendCustomEventToAppsFlyer(string eventName, Dictionary<string, string> eventParams)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x000F65C8 File Offset: 0x000F47C8
		public static void SendCustomEventToAppsFlyer(string eventName, Dictionary<string, string> eventParams, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				AnalyticsFacade._appsFlyerFacade.TrackRichEvent(eventName, eventParams);
			}
			if (duplicateToConsole)
			{
				string text = Json.Serialize(eventParams);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					eventName,
					text
				});
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x000F663C File Offset: 0x000F483C
		public static void SendCustomEventToFlurry(string eventName, Dictionary<string, string> parameters)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEventToFlurry(eventName, parameters, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x000F6650 File Offset: 0x000F4850
		public static void SendCustomEventToFlurry(string eventName, Dictionary<string, string> parameters, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (AnalyticsFacade._flurryFacade != null)
			{
				AnalyticsFacade._flurryFacade.LogEventWithParameters(eventName, parameters);
			}
			if (duplicateToConsole)
			{
				string text = Json.Serialize(parameters);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					eventName,
					text
				});
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000F66C4 File Offset: 0x000F48C4
		public static void SendCustomEventToFacebook(string eventName, Dictionary<string, object> parameters)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendCustomEventToFacebook(eventName, null, parameters, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000F66EC File Offset: 0x000F48EC
		internal static void SendCustomEventToFacebook(string eventName, float? valueToSum, Dictionary<string, object> parameters, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._facebookFacade != null)
			{
				AnalyticsFacade._facebookFacade.LogAppEvent(eventName, valueToSum, parameters);
			}
			if (duplicateToConsole)
			{
				string text = (parameters == null) ? "{}" : Json.Serialize(parameters);
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					eventName,
					text
				});
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000F6750 File Offset: 0x000F4950
		public static void Flush()
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.SendBufferedEvents();
			}
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000F676C File Offset: 0x000F496C
		public static void Tutorial(AnalyticsConstants.TutorialState step)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.Tutorial(step, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x000F6780 File Offset: 0x000F4980
		public static void Tutorial(AnalyticsConstants.TutorialState step, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.Tutorial(step);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					"TUTORIAL_BUILTIN",
					Json.Serialize(new Dictionary<string, object>
					{
						{
							"step",
							step.ToString()
						}
					})
				});
			}
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x000F67F0 File Offset: 0x000F49F0
		public static void LevelUp(int level)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.LevelUp(level, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000F6804 File Offset: 0x000F4A04
		public static void LevelUp(int level, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("Coins", Storager.getInt("Coins", false));
			dictionary.Add("GemsCurrency", Storager.getInt("GemsCurrency", false));
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.LevelUp(level, dictionary);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					"LEVELUP_BUILTIN",
					Json.Serialize(new Dictionary<string, object>
					{
						{
							"level",
							level.ToString()
						},
						{
							"resources",
							dictionary
						}
					})
				});
			}
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000F68B0 File Offset: 0x000F4AB0
		public static void CurrencyAccrual(int amount, string currencyName, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.CurrencyAccrual(amount, currencyName, accrualType, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000F68C4 File Offset: 0x000F4AC4
		public static void CurrencyAccrual(int amount, string currencyName, AnalyticsConstants.AccrualType accrualType, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.CurrencyAccrual(amount, currencyName, accrualType);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					"CURRENCY_ACCRUAL_BUILTIN",
					Json.Serialize(new Dictionary<string, object>
					{
						{
							"amount",
							amount.ToString()
						},
						{
							"currencyName",
							currencyName
						},
						{
							"accrualType",
							accrualType.ToString()
						}
					})
				});
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000F6954 File Offset: 0x000F4B54
		public static void RealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode, string keyOfInappAction)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.RealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode, AnalyticsFacade.DuplicateToConsoleByDefault, keyOfInappAction);
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000F6978 File Offset: 0x000F4B78
		public static void RealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode, bool duplicateToConsole, string keyOfInappAction)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				try
				{
					AnalyticsFacade._devToDevFacade.RealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			if (AnalyticsFacade._flurryFacade != null)
			{
				AnalyticsFacade.RecyclingFlurryParameters.Clear();
				AnalyticsFacade.RecyclingFlurryParameters.Add("Total", inAppName);
				if (keyOfInappAction != null)
				{
					AnalyticsFacade.RecyclingFlurryParameters.Add("Special Offer", string.Format("{0}   {1}", inAppName, keyOfInappAction));
				}
				AnalyticsFacade._flurryFacade.LogEventWithParameters("RealPayment", AnalyticsFacade.RecyclingFlurryParameters);
				AnalyticsFacade.RecyclingFlurryParameters.Clear();
			}
			Lazy<Dictionary<string, string>> lazy = new Lazy<Dictionary<string, string>>(delegate()
			{
				string value = inAppPrice.ToString("0.00", CultureInfo.InvariantCulture);
				return new Dictionary<string, string>(4)
				{
					{
						"af_revenue",
						value
					},
					{
						"af_content_id",
						inAppName
					},
					{
						"af_currency",
						currencyIsoCode
					},
					{
						"af_receipt_id",
						paymentId
					}
				};
			});
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				try
				{
					AnalyticsFacade._appsFlyerFacade.TrackRichEvent("af_purchase", lazy.Value);
				}
				catch (Exception exception2)
				{
					Debug.LogException(exception2);
				}
			}
			if (AnalyticsFacade._facebookFacade != null)
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>
				{
					{
						"payment_id",
						paymentId
					},
					{
						"inapp_name",
						inAppName
					}
				};
				try
				{
					AnalyticsFacade._facebookFacade.LogPurchase(inAppPrice, currencyIsoCode, parameters);
				}
				catch (Exception exception3)
				{
					Debug.LogException(exception3);
				}
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					"REAL_PAYMENT_BUILTIN",
					Json.Serialize(lazy.Value)
				});
			}
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000F6B8C File Offset: 0x000F4D8C
		public static void SendFirstTimeRealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.SendFirstTimeRealPayment(paymentId, inAppPrice, inAppName, currencyIsoCode, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000F6BA4 File Offset: 0x000F4DA4
		public static void SendFirstTimeRealPayment(string paymentId, float inAppPrice, string inAppName, string currencyIsoCode, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			string priceAsString = inAppPrice.ToString("0.00", CultureInfo.InvariantCulture);
			Lazy<Dictionary<string, string>> lazy = new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>(4)
			{
				{
					"af_revenue",
					priceAsString
				},
				{
					"af_content_id",
					inAppName
				},
				{
					"af_currency",
					currencyIsoCode
				},
				{
					"af_receipt_id",
					paymentId
				}
			});
			if (AnalyticsFacade._appsFlyerFacade != null)
			{
				AnalyticsFacade._appsFlyerFacade.TrackRichEvent("first_buy", lazy.Value);
			}
			if (AnalyticsFacade._facebookFacade != null)
			{
				Dictionary<string, object> parameters = new Dictionary<string, object>
				{
					{
						"product",
						inAppName
					},
					{
						"revenue",
						priceAsString
					},
					{
						"currency",
						currencyIsoCode
					}
				};
				AnalyticsFacade._facebookFacade.LogAppEvent("first_buy", new float?(inAppPrice), parameters);
			}
			if (duplicateToConsole)
			{
				Debug.LogFormat(AnalyticsFacade._parametrizedEventFormat.Value, new object[]
				{
					"First time real payment",
					Json.Serialize(lazy.Value)
				});
			}
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x000F6CA8 File Offset: 0x000F4EA8
		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency)
		{
			AnalyticsFacade.Initialize();
			AnalyticsFacade.InAppPurchase(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchaseCurrency, AnalyticsFacade.DuplicateToConsoleByDefault);
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x000F6CCC File Offset: 0x000F4ECC
		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency, bool duplicateToConsole)
		{
			AnalyticsFacade.Initialize();
			if (AnalyticsFacade._devToDevFacade != null)
			{
				AnalyticsFacade._devToDevFacade.InAppPurchase(purchaseId, purchaseType, purchaseAmount, purchasePrice, purchaseCurrency);
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000F6CF0 File Offset: 0x000F4EF0
		private static void InitializeDevToDev(string appKey, string secretKey)
		{
			AnalyticsFacade._devToDevFacade = new DevToDevFacade(appKey, secretKey);
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000F6D00 File Offset: 0x000F4F00
		private static void InitializeAppsFlyer(string appKey, string appId)
		{
			AnalyticsFacade._appsFlyerFacade = new AppsFlyerFacade(appKey, appId);
			AnalyticsFacade._appsFlyerFacade.TrackAppLaunch();
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000F6D18 File Offset: 0x000F4F18
		private static void InitializeFacebook()
		{
			AnalyticsFacade._facebookFacade = new FacebookFacade();
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000F6D24 File Offset: 0x000F4F24
		private static void InitializeFlurry(string apiKey)
		{
			AnalyticsFacade._flurryFacade = new FlurryFacade(apiKey, Defs.IsDeveloperBuild);
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x000F6D38 File Offset: 0x000F4F38
		internal static DevToDevFacade DevToDevFacade
		{
			get
			{
				return AnalyticsFacade._devToDevFacade;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x000F6D40 File Offset: 0x000F4F40
		internal static AppsFlyerFacade AppsFlyerFacade
		{
			get
			{
				return AnalyticsFacade._appsFlyerFacade;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002F3F RID: 12095 RVA: 0x000F6D48 File Offset: 0x000F4F48
		internal static FacebookFacade FacebookFacade
		{
			get
			{
				return AnalyticsFacade._facebookFacade;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x000F6D50 File Offset: 0x000F4F50
		internal static FlurryFacade FlurryFacade
		{
			get
			{
				return AnalyticsFacade._flurryFacade;
			}
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000F6D58 File Offset: 0x000F4F58
		private static string InitializeSimpleEventFormat()
		{
			return (!Application.isEditor) ? "\"{0}\"" : "<color=magenta>\"{0}\"</color>";
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000F6D74 File Offset: 0x000F4F74
		private static string InitializeParametrizedEventFormat()
		{
			return (!Application.isEditor) ? "\"{0}\": {1}" : "<color=magenta>\"{0}\": {1}</color>";
		}

		// Token: 0x040022E1 RID: 8929
		private static bool _initialized = false;

		// Token: 0x040022E2 RID: 8930
		private static DevToDevFacade _devToDevFacade;

		// Token: 0x040022E3 RID: 8931
		private static AppsFlyerFacade _appsFlyerFacade;

		// Token: 0x040022E4 RID: 8932
		private static FacebookFacade _facebookFacade;

		// Token: 0x040022E5 RID: 8933
		private static FlurryFacade _flurryFacade;

		// Token: 0x040022E6 RID: 8934
		private static readonly Dictionary<string, string> _recyclingFlurryParameters = new Dictionary<string, string>();

		// Token: 0x040022E7 RID: 8935
		private static readonly Lazy<string> _simpleEventFormat = new Lazy<string>(new Func<string>(AnalyticsFacade.InitializeSimpleEventFormat));

		// Token: 0x040022E8 RID: 8936
		private static readonly Lazy<string> _parametrizedEventFormat = new Lazy<string>(new Func<string>(AnalyticsFacade.InitializeParametrizedEventFormat));
	}
}
