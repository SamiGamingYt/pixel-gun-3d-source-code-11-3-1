using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000550 RID: 1360
	public class AnalyticsStuff
	{
		// Token: 0x06002F53 RID: 12115 RVA: 0x000F7374 File Offset: 0x000F5574
		internal static void TrySendOnceToFacebook(string eventName, Lazy<Dictionary<string, object>> eventParams, Version excludeVersion)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (excludeVersion != null)
			{
				try
				{
					Version v = new Version(Switcher.InitialAppVersion);
					if (v <= excludeVersion)
					{
						return;
					}
				}
				catch
				{
				}
			}
			string key = "Analytics:" + eventName;
			if (Storager.hasKey(key) && !string.IsNullOrEmpty(Storager.getString(key, false)))
			{
				return;
			}
			Storager.setString(key, "True", false);
			Dictionary<string, object> parameters = (eventParams == null) ? null : eventParams.Value;
			AnalyticsFacade.SendCustomEventToFacebook(eventName, null, parameters, false);
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000F743C File Offset: 0x000F563C
		internal static void TrySendOnceToAppsFlyer(string eventName, Lazy<Dictionary<string, string>> eventParams, Version excludeVersion)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			if (eventParams == null)
			{
				throw new ArgumentNullException("eventParams");
			}
			if (excludeVersion == null)
			{
				throw new ArgumentNullException("excludeVersion");
			}
			try
			{
				Version v = new Version(Switcher.InitialAppVersion);
				if (v <= excludeVersion)
				{
					return;
				}
			}
			catch
			{
				return;
			}
			string key = "Analytics:" + eventName;
			if (Storager.hasKey(key) && !string.IsNullOrEmpty(Storager.getString(key, false)))
			{
				return;
			}
			Storager.setString(key, Json.Serialize(eventParams), false);
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, eventParams.Value);
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x000F750C File Offset: 0x000F570C
		public static void TrySendOnceToAppsFlyer(string eventName)
		{
			if (eventName == null)
			{
				throw new ArgumentNullException("eventName");
			}
			string key = "Analytics:" + eventName;
			if (Storager.hasKey(key) && !string.IsNullOrEmpty(Storager.getString(key, false)))
			{
				return;
			}
			Storager.setString(key, "{}", false);
			AnalyticsFacade.SendCustomEventToAppsFlyer(eventName, new Dictionary<string, string>());
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000F756C File Offset: 0x000F576C
		internal static void SendInGameDay(int newInGameDayIndex)
		{
			Version v = new Version(Switcher.InitialAppVersion);
			if (v <= new Version(11, 2, 3, 0))
			{
				return;
			}
			Dictionary<string, object> dictionary = ParametersCache.Acquire(1);
			dictionary.Add("Day", newInGameDayIndex.ToString(CultureInfo.InvariantCulture));
			AnalyticsFacade.SendCustomEvent("InGameDay", dictionary);
			ParametersCache.Release(dictionary);
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x000F75CC File Offset: 0x000F57CC
		public static void LogCampaign(string map, string boxName)
		{
			try
			{
				if (string.IsNullOrEmpty(map))
				{
					Debug.LogError("LogCampaign string.IsNullOrEmpty(map)");
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>
					{
						{
							"Maps",
							map
						}
					};
					if (boxName != null)
					{
						dictionary.Add("Boxes", boxName);
					}
					AnalyticsFacade.SendCustomEvent("Campaign", dictionary);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogCampaign: " + arg);
			}
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x000F765C File Offset: 0x000F585C
		public static void LogMultiplayer()
		{
			try
			{
				string text = ConnectSceneNGUIController.regim.ToString();
				if (Defs.isDaterRegim)
				{
					text = "Sandbox";
				}
				if (text == null)
				{
					Debug.LogError("LogMultiplayer modeName == null");
				}
				else
				{
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							"Game Modes",
							text
						},
						{
							text + " By Tier",
							ExpController.OurTierForAnyPlace() + 1
						}
					};
					AnalyticsFacade.SendCustomEvent("Multiplayer", eventParams);
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					try
					{
						int indexMap = Convert.ToInt32(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.mapProperty]);
						SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(indexMap);
						dictionary["Total"] = infoScene.NameScene;
						dictionary[text] = infoScene.NameScene;
					}
					catch (Exception ex)
					{
						dictionary["Error"] = ex.GetType().Name;
						Debug.LogException(ex);
					}
					AnalyticsFacade.SendCustomEvent("Maps", dictionary);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogMultiplayer: " + arg);
			}
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000F77B4 File Offset: 0x000F59B4
		public static void LogSandboxTimeGamePopularity(int timeGame, bool isStart)
		{
			try
			{
				string key = (timeGame != 5 && timeGame != 10 && timeGame != 15) ? "Other" : ("Time " + timeGame.ToString());
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						key,
						(!isStart) ? "End" : "Start"
					}
				};
				AnalyticsFacade.SendCustomEvent("Sandbox", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("Sandbox exception: " + arg);
			}
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000F785C File Offset: 0x000F5A5C
		public static void LogFirstBattlesKillRate(int battleIndex, float killRate)
		{
			try
			{
				string value = string.Empty;
				if (killRate < 0.4f)
				{
					value = "<0,4";
				}
				else if (killRate < 0.6f)
				{
					value = "0,4 - 0,6";
				}
				else if (killRate < 0.8f)
				{
					value = "0,6 - 0,8";
				}
				else if (killRate < 1f)
				{
					value = "0,8 - 1";
				}
				else if (killRate < 1.2f)
				{
					value = "1 - 1,2";
				}
				else if (killRate < 1.5f)
				{
					value = "1,2 - 1,5";
				}
				else if (killRate < 2f)
				{
					value = "1,5 - 2";
				}
				else if (killRate < 3f)
				{
					value = "2 - 3";
				}
				else
				{
					value = ">3";
				}
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						"Battle " + battleIndex.ToString(),
						value
					}
				};
				AnalyticsFacade.SendCustomEvent("First Battles KillRate", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogFirstBattlesKillRate: " + arg);
			}
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000F7988 File Offset: 0x000F5B88
		public static void LogFirstBattlesResult(int battleIndex, bool winner)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						"Battle " + battleIndex.ToString(),
						(!winner) ? "Lose" : "Win"
					}
				};
				AnalyticsFacade.SendCustomEvent("First Battles Result", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogFirstBattlesResult: " + arg);
			}
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x000F7A0C File Offset: 0x000F5C0C
		public static void LogABTest(string nameTest, string nameCohort, bool isStart = true)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						nameTest,
						(!isStart) ? ("Excluded " + nameCohort) : nameCohort
					}
				};
				AnalyticsFacade.SendCustomEvent("A/B Test", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("A/B Test exception: " + arg);
			}
		}

		// Token: 0x06002F5D RID: 12125 RVA: 0x000F7A84 File Offset: 0x000F5C84
		public static void LogArenaWavesPassed(int countWaveComplite)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						"Waves Passed",
						(countWaveComplite >= 9) ? ">=9" : countWaveComplite.ToString()
					}
				};
				AnalyticsFacade.SendCustomEvent("Arena", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("ArenaFirst  exception: " + arg);
			}
		}

		// Token: 0x06002F5E RID: 12126 RVA: 0x000F7B00 File Offset: 0x000F5D00
		public static void LogArenaFirst(bool isPause, bool isMoreOneWave)
		{
			try
			{
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						"First",
						(!isPause) ? ((!isMoreOneWave) ? "Fail" : "Complete") : "Quit"
					}
				};
				AnalyticsFacade.SendCustomEvent("Arena", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("ArenaFirst  exception: " + arg);
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000F7B88 File Offset: 0x000F5D88
		public static void Tutorial(AnalyticsConstants.TutorialState step, int count_battle = 0)
		{
			try
			{
				AnalyticsStuff.LoadTrainingStep();
				if (step > (AnalyticsConstants.TutorialState)AnalyticsStuff.trainingStep || ((step == AnalyticsConstants.TutorialState.Battle_Start || step == AnalyticsConstants.TutorialState.Battle_End) && AnalyticsStuff.trainingStep < 90))
				{
					int num = AnalyticsStuff.trainingStep;
					AnalyticsStuff.trainingStep = (int)step;
					string text = AnalyticsStuff.trainingStep.ToString() + "_" + step.ToString();
					if (step == AnalyticsConstants.TutorialState.Get_Progress)
					{
						text = string.Concat(new object[]
						{
							AnalyticsStuff.trainingStep.ToString(),
							"_",
							num,
							"_",
							step.ToString()
						});
					}
					if (step == AnalyticsConstants.TutorialState.Battle_Start || step == AnalyticsConstants.TutorialState.Battle_End)
					{
						if (step == AnalyticsConstants.TutorialState.Battle_Start)
						{
							int @int = PlayerPrefs.GetInt("SendingStartButtle", -1);
							if (count_battle <= @int)
							{
								return;
							}
							PlayerPrefs.SetInt("SendingStartButtle", count_battle);
						}
						text = string.Concat(new object[]
						{
							AnalyticsStuff.trainingStep.ToString(),
							"_",
							count_battle,
							"_",
							step.ToString()
						});
					}
					FriendsController.SendToturialEvent((int)step, text);
					AnalyticsFacade.Tutorial(step);
					AnalyticsFacade.SendCustomEvent("Tutorial", new Dictionary<string, object>
					{
						{
							"Progress",
							text
						}
					});
					if (step != AnalyticsConstants.TutorialState.Portal)
					{
						if (step != AnalyticsConstants.TutorialState.Connect_Scene)
						{
							if (step == AnalyticsConstants.TutorialState.Finished)
							{
								AnalyticsFacade.SendCustomEventToFacebook("training_completed", null);
							}
						}
						else
						{
							AnalyticsFacade.SendCustomEventToFacebook("training_armory", null);
						}
					}
					else
					{
						AnalyticsFacade.SendCustomEventToFacebook("training_controls", null);
					}
					if (step > AnalyticsConstants.TutorialState.Portal)
					{
						AnalyticsStuff.SaveTrainingStep();
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in Tutorial: " + arg);
			}
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000F7D7C File Offset: 0x000F5F7C
		public static void SaveTrainingStep()
		{
			if (AnalyticsStuff.trainingStepLoaded)
			{
				Storager.setInt(AnalyticsStuff.trainingProgressKey, AnalyticsStuff.trainingStep, false);
			}
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000F7D98 File Offset: 0x000F5F98
		public static void LogDailyGiftPurchases(string packId)
		{
			try
			{
				if (string.IsNullOrEmpty(packId))
				{
					Debug.LogError("LogDailyGiftPurchases: string.IsNullOrEmpty(packId)");
				}
				else
				{
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							"Purchases",
							AnalyticsStuff.ReadableNameForInApp(packId)
						}
					};
					AnalyticsFacade.SendCustomEvent("Daily Gift Total", eventParams);
					AnalyticsFacade.SendCustomEvent("Daily Gift" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogDailyGiftPurchases: " + arg);
			}
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000F7E30 File Offset: 0x000F6030
		public static void LogDailyGift(string giftId, GiftCategoryType giftCategoryType, int count, bool isForMoneyGift)
		{
			try
			{
				if (string.IsNullOrEmpty(giftId))
				{
					Debug.LogError("LogDailyGift: string.IsNullOrEmpty(giftId)");
				}
				else
				{
					if (SkinsController.shopKeyFromNameSkin.ContainsKey(giftId))
					{
						giftId = "Skin";
					}
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							"Chance",
							giftId
						},
						{
							"Spins",
							(!isForMoneyGift) ? "Free" : "Paid"
						}
					};
					AnalyticsFacade.SendCustomEvent("Daily Gift Total", eventParams);
					AnalyticsFacade.SendCustomEvent("Daily Gift" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogDailyGift: " + arg);
			}
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000F7EFC File Offset: 0x000F60FC
		public static void LogTrafficForwarding(AnalyticsStuff.LogTrafficForwardingMode mode)
		{
			try
			{
				string text = (mode != AnalyticsStuff.LogTrafficForwardingMode.Show) ? "Button Pressed" : "Button Show";
				Dictionary<string, object> eventParams = new Dictionary<string, object>
				{
					{
						"Conversion",
						text
					},
					{
						text + " Levels",
						(!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel
					},
					{
						text + " Tiers",
						ExpController.OurTierForAnyPlace() + 1
					},
					{
						text + " Paying",
						(!StoreKitEventListener.IsPayingUser()) ? "FALSE" : "TRUE"
					}
				};
				AnalyticsFacade.SendCustomEvent("Pereliv Button", eventParams);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogTrafficForwarding: " + arg);
			}
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000F7FF8 File Offset: 0x000F61F8
		public static void LogWEaponsSpecialOffers_MoneySpended(string packId)
		{
			try
			{
				if (string.IsNullOrEmpty(packId))
				{
					Debug.LogError("LogWEaponsSpecialOffers_MoneySpended: string.IsNullOrEmpty(packId)");
				}
				else
				{
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							"Money Spended",
							AnalyticsStuff.ReadableNameForInApp(packId)
						}
					};
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", eventParams);
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogWEaponsSpecialOffers_MoneySpended: " + arg);
			}
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000F8090 File Offset: 0x000F6290
		public static void LogWEaponsSpecialOffers_Conversion(bool show, string weaponId = null)
		{
			try
			{
				if (!show && string.IsNullOrEmpty(weaponId))
				{
					Debug.LogError("LogWEaponsSpecialOffers_Conversion: string.IsNullOrEmpty(weaponId)");
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>
					{
						{
							"Conversion",
							(!show) ? "Buy" : "Show"
						}
					};
					try
					{
						float num = (!ABTestController.useBuffSystem) ? KillRateCheck.instance.GetKillRate() : BuffSystem.instance.GetKillrateByInteractions();
						string arg = (num > 0.5f) ? ((num > 1.2f) ? "Strong" : "Normal") : "Weak";
						string key = string.Format("Conversion {0} Players", arg);
						if (!show)
						{
							dictionary.Add("Currency Spended", weaponId);
							dictionary.Add("Buy (Tier)", ExpController.OurTierForAnyPlace() + 1);
							dictionary.Add("Buy (Level)", (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
							dictionary.Add(key, "Buy");
						}
						else
						{
							dictionary.Add("Show (Tier)", ExpController.OurTierForAnyPlace() + 1);
							dictionary.Add("Show (Level)", (!(ExperienceController.sharedController != null)) ? 1 : ExperienceController.sharedController.currentLevel);
							dictionary.Add(key, "Show");
						}
					}
					catch (Exception arg2)
					{
						Debug.LogError("Exception in LogWEaponsSpecialOffers_Conversion adding paramters: " + arg2);
					}
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers Total", dictionary);
					AnalyticsFacade.SendCustomEvent("Weapons Special Offers" + AnalyticsStuff.GetPayingSuffixNo10(), dictionary);
				}
			}
			catch (Exception arg3)
			{
				Debug.LogError("Exception in LogWEaponsSpecialOffers_Conversion: " + arg3);
			}
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000F8294 File Offset: 0x000F6494
		public static void LogSpecialOffersPanel(string efficiencyPArameter, string efficiencyValue, string additionalParameter = null, string additionalValue = null)
		{
			try
			{
				if (string.IsNullOrEmpty(efficiencyPArameter) || string.IsNullOrEmpty(efficiencyValue))
				{
					Debug.LogError("LogSpecialOffersPanel:  string.IsNullOrEmpty(efficiencyPArameter) || string.IsNullOrEmpty(efficiencyValue)");
				}
				else
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>
					{
						{
							efficiencyPArameter,
							efficiencyValue
						}
					};
					if (additionalParameter != null && additionalValue != null)
					{
						dictionary.Add(additionalParameter, additionalValue);
					}
					AnalyticsFacade.SendCustomEvent("Special Offers Banner Total", dictionary);
					AnalyticsFacade.SendCustomEvent("Special Offers Banner" + AnalyticsStuff.GetPayingSuffixNo10(), dictionary);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogSpecialOffersPanel: " + arg);
			}
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000F8344 File Offset: 0x000F6544
		public static string AnalyticsReadableCategoryNameFromOldCategoryName(string categoryParameterName)
		{
			categoryParameterName = AnalyticsStuff.NewSkinCategoryToOldSkinCategory(categoryParameterName);
			if (categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueHatsCategory) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory))
			{
				categoryParameterName = "League";
			}
			return categoryParameterName;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x000F838C File Offset: 0x000F658C
		private static string NewSkinCategoryToOldSkinCategory(string categoryParameterName)
		{
			if (categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryMale) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryFemale) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategorySpecial) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryPremium) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.SkinsCategoryEditor) || categoryParameterName == AnalyticsConstants.GetSalesName(ShopNGUIController.CategoryNames.LeagueSkinsCategory))
			{
				categoryParameterName = "Skins";
			}
			return categoryParameterName;
		}

		// Token: 0x06002F69 RID: 12137 RVA: 0x000F8420 File Offset: 0x000F6620
		public static void LogEggDelivery(string eggId)
		{
			if (string.IsNullOrEmpty(eggId))
			{
				Debug.LogWarning("LogEggDelivery: egg id is empty.");
				return;
			}
			Dictionary<string, object> eventParams = new Dictionary<string, object>(1)
			{
				{
					"Eggs Delivery",
					eggId
				}
			};
			AnalyticsFacade.SendCustomEvent("Eggs Drop Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Eggs Drop" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
		}

		// Token: 0x06002F6A RID: 12138 RVA: 0x000F8478 File Offset: 0x000F6678
		public static void LogHatching(string petId, Egg egg)
		{
			if (egg == null)
			{
				Debug.LogWarning("LogHatching: egg is null.");
				return;
			}
			string key = AnalyticsStuff.DetermineHatchingParameterName(egg);
			string value = petId ?? string.Empty;
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					key,
					value
				}
			};
			AnalyticsFacade.SendCustomEvent("Eggs Drop Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Eggs Drop" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
		}

		// Token: 0x06002F6B RID: 12139 RVA: 0x000F84E0 File Offset: 0x000F66E0
		internal static void LogDailyVideoRewarded(int countWithinCurrentDay)
		{
			string value = countWithinCurrentDay.ToString(CultureInfo.InvariantCulture);
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Rewarded",
					value
				}
			};
			AnalyticsFacade.SendCustomEvent("Ads Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Ads" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000F8534 File Offset: 0x000F6734
		internal static void LogBattleInviteSent()
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Conversion",
					"Send Invite"
				}
			};
			AnalyticsFacade.SendCustomEvent("Push Notifications", eventParams);
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000F8568 File Offset: 0x000F6768
		internal static void LogBattleInviteAccepted()
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object>
			{
				{
					"Conversion",
					"Accept Invite"
				}
			};
			AnalyticsFacade.SendCustomEvent("Push Notifications", eventParams);
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000F859C File Offset: 0x000F679C
		private static string DetermineHatchingParameterName(Egg egg)
		{
			if (egg.HatchedType == EggHatchedType.Champion)
			{
				return "Drop Super Incubator";
			}
			if (egg.HatchedType == EggHatchedType.League)
			{
				return "Drop Champion";
			}
			string arg = egg.Data.Rare.ToString();
			string arg2 = egg.HatchedType.ToString();
			return string.Format("Drop {0} {1}", arg, arg2);
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x000F8604 File Offset: 0x000F6804
		public static void LogBestSales(string itemId, ShopNGUIController.CategoryNames category)
		{
			try
			{
				if (string.IsNullOrEmpty(itemId))
				{
					Debug.LogError("LogBestSales: string.IsNullOrEmpty(itemId)");
				}
				else
				{
					string key;
					if (category != ShopNGUIController.CategoryNames.BestWeapons)
					{
						if (category != ShopNGUIController.CategoryNames.BestWear)
						{
							if (category != ShopNGUIController.CategoryNames.BestGadgets)
							{
								Debug.LogErrorFormat("LogBestSales: incorrect category: {0}", new object[]
								{
									category
								});
								return;
							}
							key = "Gadgets";
						}
						else
						{
							key = "Wear";
						}
					}
					else
					{
						key = "Weapons";
					}
					string str = "Best Sales";
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							key,
							itemId
						}
					};
					AnalyticsFacade.SendCustomEvent(str + " Total", eventParams);
					AnalyticsFacade.SendCustomEvent(str + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogBestSales: " + arg);
			}
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x000F870C File Offset: 0x000F690C
		public static void LogSales(string itemId, string categoryParameterName, bool isDaterWeapon = false)
		{
			try
			{
				if (string.IsNullOrEmpty(itemId))
				{
					Debug.LogError("LogSales: string.IsNullOrEmpty(itemId)");
				}
				else if (string.IsNullOrEmpty(categoryParameterName))
				{
					Debug.LogError("LogSales: string.IsNullOrEmpty(categoryParameterName)");
				}
				else
				{
					categoryParameterName = AnalyticsStuff.AnalyticsReadableCategoryNameFromOldCategoryName(categoryParameterName);
					string salesCategory = SalesConstants.Instance.GetSalesCategory(categoryParameterName);
					Dictionary<string, object> dictionary = new Dictionary<string, object>
					{
						{
							categoryParameterName,
							itemId
						}
					};
					if (isDaterWeapon)
					{
						dictionary.Add("Dater Weapons", itemId);
					}
					AnalyticsFacade.SendCustomEvent(salesCategory + " Total", dictionary);
					AnalyticsFacade.SendCustomEvent(salesCategory + AnalyticsStuff.GetPayingSuffixNo10(), dictionary);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogSales: " + arg);
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x000F87DC File Offset: 0x000F69DC
		public static void RateUsFake(bool rate, int stars, bool sendNegativFeedback = false)
		{
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("Efficiency", (!rate) ? "Later" : "Rate");
				if (rate)
				{
					dictionary.Add("Rating (Stars)", stars);
				}
				if (stars > 0 && stars < 4)
				{
					dictionary.Add("Negative Feedback", (!sendNegativFeedback) ? "Not sended" : "Sended");
				}
				AnalyticsFacade.SendCustomEvent("Rate Us Fake", dictionary);
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in RateUsFake: " + arg);
			}
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000F8898 File Offset: 0x000F6A98
		public static string ReadableNameForInApp(string purchaseId)
		{
			return (!StoreKitEventListener.inAppsReadableNames.ContainsKey(purchaseId)) ? purchaseId : StoreKitEventListener.inAppsReadableNames[purchaseId];
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002F73 RID: 12147 RVA: 0x000F88BC File Offset: 0x000F6ABC
		public static int TrainingStep
		{
			get
			{
				AnalyticsStuff.LoadTrainingStep();
				return AnalyticsStuff.trainingStep;
			}
		}

		// Token: 0x06002F74 RID: 12148 RVA: 0x000F88C8 File Offset: 0x000F6AC8
		private static void LogGameDayCount()
		{
			string text = DateTime.UtcNow.ToString("yyyy-MM-dd");
			try
			{
				string @string = PlayerPrefs.GetString("Analytics.GameDayCount", string.Empty);
				Dictionary<string, object> dictionary = (Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				if (dictionary.Count == 0)
				{
					int num = 1;
					dictionary.Add(text, num);
					string value = Json.Serialize(dictionary);
					PlayerPrefs.SetString("Analytics.GameDayCount", value);
					AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object>
					{
						{
							"count",
							num
						}
					});
				}
				else
				{
					KeyValuePair<string, object> keyValuePair = dictionary.First<KeyValuePair<string, object>>();
					object value2 = keyValuePair.Value;
					if (text == keyValuePair.Key)
					{
						object value3 = value2;
						AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object>
						{
							{
								"count",
								value3
							}
						});
					}
					else
					{
						int num2 = Convert.ToInt32(value2) + 1;
						Dictionary<string, object> obj = new Dictionary<string, object>
						{
							{
								text,
								num2
							}
						};
						string value4 = Json.Serialize(obj);
						PlayerPrefs.SetString("Analytics.GameDayCount", value4);
						AnalyticsFacade.SendCustomEventToFacebook("game_days_count", new Dictionary<string, object>
						{
							{
								"count",
								num2
							}
						});
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000F8A58 File Offset: 0x000F6C58
		public static void LogAchievementEarned(int achievementId, int newStage)
		{
			try
			{
				if (newStage < 1 || newStage > 3)
				{
					Debug.LogError(string.Format("invalid achievement newStage : '{0}'", newStage));
				}
				else
				{
					Dictionary<string, object> eventParams = new Dictionary<string, object>
					{
						{
							"Earned",
							string.Format("{0}_{1}", achievementId, newStage)
						}
					};
					AnalyticsFacade.SendCustomEvent("Achievements", eventParams);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in LogAchievementEarned: " + arg);
			}
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000F8AF8 File Offset: 0x000F6CF8
		internal static IEnumerator WaitInitializationThenLogGameDayCountCoroutine()
		{
			yield return new WaitUntil(() => AnalyticsFacade.FacebookFacade != null);
			AnalyticsStuff.LogGameDayCount();
			yield break;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000F8B0C File Offset: 0x000F6D0C
		internal static void LogProgressInExperience(int levelBase1, int tierBase1)
		{
			Dictionary<string, object> eventParams = new Dictionary<string, object>(2)
			{
				{
					"Tier",
					tierBase1
				},
				{
					"Level",
					levelBase1
				}
			};
			AnalyticsFacade.SendCustomEvent("Active Users Progress Total", eventParams);
			AnalyticsFacade.SendCustomEvent("Active Users Progress" + AnalyticsStuff.GetPayingSuffixNo10(), eventParams);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000F8B64 File Offset: 0x000F6D64
		private static void LoadTrainingStep()
		{
			if (!AnalyticsStuff.trainingStepLoaded)
			{
				if (!Storager.hasKey(AnalyticsStuff.trainingProgressKey))
				{
					AnalyticsStuff.trainingStep = -1;
					Storager.setInt(AnalyticsStuff.trainingProgressKey, AnalyticsStuff.trainingStep, false);
				}
				else
				{
					AnalyticsStuff.trainingStep = Storager.getInt(AnalyticsStuff.trainingProgressKey, false);
				}
				AnalyticsStuff.trainingStepLoaded = true;
			}
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000F8BBC File Offset: 0x000F6DBC
		public static string GetPayingSuffixNo10()
		{
			if (!StoreKitEventListener.IsPayingUser())
			{
				return " (Non Paying)";
			}
			return " (Paying)";
		}

		// Token: 0x040022EC RID: 8940
		private const string dailyGiftEventNameBase = "Daily Gift";

		// Token: 0x040022ED RID: 8941
		private const string WeaponsSpecialOffersEvent = "Weapons Special Offers";

		// Token: 0x040022EE RID: 8942
		private static int trainingStep = -1;

		// Token: 0x040022EF RID: 8943
		private static bool trainingStepLoaded;

		// Token: 0x040022F0 RID: 8944
		private static string trainingProgressKey = "TrainingStepKeyAnalytics";

		// Token: 0x02000551 RID: 1361
		public enum LogTrafficForwardingMode
		{
			// Token: 0x040022F2 RID: 8946
			Show,
			// Token: 0x040022F3 RID: 8947
			Press
		}
	}
}
