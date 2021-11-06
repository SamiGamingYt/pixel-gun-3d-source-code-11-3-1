using System;
using System.Globalization;
using System.IO;
using System.Text;
using Rilisoft;
using UnityEngine;

// Token: 0x0200077B RID: 1915
internal sealed class URLs
{
	// Token: 0x0600433A RID: 17210 RVA: 0x00167094 File Offset: 0x00165294
	private URLs()
	{
	}

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x0600433C RID: 17212 RVA: 0x001670F4 File Offset: 0x001652F4
	public static string ABTestViewBankURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/test2505/currentView.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/android2505/currentView.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/amazon/currentView.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/wp/currentView.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestViewBank/ios/currentView.json";
		}
	}

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x0600433D RID: 17213 RVA: 0x00167150 File Offset: 0x00165350
	public static string ABTestBalansURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/test/abTest.php?key=0LjZB3GmACx15N7HJPYm";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/android/abTest.php?key=0LjZB3GmACx15N7HJPYm" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/amazon/abTest.php?key=0LjZB3GmACx15N7HJPYm";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/wp/abTest.php?key=0LjZB3GmACx15N7HJPYm";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/ios/abTest.php?key=0LjZB3GmACx15N7HJPYm";
		}
	}

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x0600433E RID: 17214 RVA: 0x001671AC File Offset: 0x001653AC
	public static string ABTestBalansFolderURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/test/";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/android/" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/amazon/";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/wp/";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/ios/";
		}
	}

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x0600433F RID: 17215 RVA: 0x00167208 File Offset: 0x00165408
	public static string ABTestSandBoxURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SandBox/abtestconfig_ios.json";
		}
	}

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x06004340 RID: 17216 RVA: 0x00167264 File Offset: 0x00165464
	public static string ABTestQuestSystemURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/QuestSystem/abtestconfig_ios.json";
		}
	}

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x06004341 RID: 17217 RVA: 0x001672C0 File Offset: 0x001654C0
	public static string ABTestSpecialOffersURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/SpecialOffers/abtestconfig_ios.json";
		}
	}

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06004342 RID: 17218 RVA: 0x0016731C File Offset: 0x0016551C
	public static string ABTestPolygonURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestPolygon/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestPolygon/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestPolygon/abtestconfig_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestPolygon/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestPolygon/abtestconfig_ios.json";
		}
	}

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06004343 RID: 17219 RVA: 0x00167378 File Offset: 0x00165578
	public static string ABTestAdvertURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestAdvert/abtestconfig_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestAdvert/abtestconfig_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestAdvert/abtestconfig_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestAdvert/abtestconfig_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestAdvert/abtestconfig_ios.json";
		}
	}

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06004344 RID: 17220 RVA: 0x001673D4 File Offset: 0x001655D4
	public static string RatingSystemConfigURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ratingConfig/rating_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ratingConfig/rating_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ratingConfig/rating_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ratingConfig/rating_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ratingConfig/rating_ios.json";
		}
	}

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06004345 RID: 17221 RVA: 0x00167430 File Offset: 0x00165630
	public static string ABTestBalansActualCohortNameURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/test/bCohortNameActual.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/android/bCohortNameActual.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/amazom/bCohortNameActual.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/wp/bCohortNameActual.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests/abTestBalans/ios/bCohortNameActual.json";
		}
	}

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06004346 RID: 17222 RVA: 0x0016748C File Offset: 0x0016568C
	public static string PromoActions
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_android.json" : "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_wp8.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
		}
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06004347 RID: 17223 RVA: 0x001674D8 File Offset: 0x001656D8
	public static string PromoActionsTest
	{
		get
		{
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_test.json";
		}
	}

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06004348 RID: 17224 RVA: 0x001674E0 File Offset: 0x001656E0
	public static string AmazonEvent
	{
		get
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event-test.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/amazonEvent/amazon-event.json";
		}
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x06004349 RID: 17225 RVA: 0x00167528 File Offset: 0x00165728
	public static string QuestConfig
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/questConfig/quest-config-wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x0600434A RID: 17226 RVA: 0x001675A4 File Offset: 0x001657A4
	public static string TutorialQuestConfig
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "test");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "ios");
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "android");
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "amazon");
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/tutorial-quests/tutorial-quests-{0}.json", "wp8");
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x0600434B RID: 17227 RVA: 0x00167650 File Offset: 0x00165850
	public static string EventX3
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/event_x3_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/event_x3_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/event_x3_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/event_x3_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/event_x3_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x0600434C RID: 17228 RVA: 0x001676CC File Offset: 0x001658CC
	public static string FilterBadWord
	{
		get
		{
			return "https://secure.pixelgunserver.com/pixelgun3d-config/FilterBadWord.json";
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x0600434D RID: 17229 RVA: 0x001676D4 File Offset: 0x001658D4
	internal static string TrafficForwardingConfigUrl
	{
		get
		{
			return URLs._trafficForwardingConfigUrl.Value;
		}
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x001676E0 File Offset: 0x001658E0
	private static string InitializeTrafficForwardingConfigUrl()
	{
		if (Defs.IsDeveloperBuild)
		{
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "test");
		}
		RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
		switch (buildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "ios");
		default:
			if (buildTargetPlatform != RuntimePlatform.MetroPlayerX64)
			{
				return string.Empty;
			}
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "wp8");
		case RuntimePlatform.Android:
		{
			Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
			if (androidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "amazon");
			}
			if (androidEdition != Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return string.Empty;
			}
			return string.Format("https://secure.pixelgunserver.com/pixelgun3d-config/trafficForwarding/traffic_forwarding_{0}.json", "android");
		}
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x0600434F RID: 17231 RVA: 0x00167798 File Offset: 0x00165998
	public static string PixelbookSettings
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_androd.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/PixelBookSettings/PixelBookSettings_wp.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06004350 RID: 17232 RVA: 0x00167814 File Offset: 0x00165A14
	public static string BuffSettings
	{
		get
		{
			string str = (!StoreKitEventListener.IsPayingUser()) ? ".json" : "_paying.json";
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_test" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_ios" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_android" + str;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_amazon" + str;
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings/BuffSettings_WP" + str;
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06004351 RID: 17233 RVA: 0x001678C8 File Offset: 0x00165AC8
	public static string BuffSettings1031
	{
		get
		{
			string str = (!StoreKitEventListener.IsPayingUser()) ? ".json" : "_paying.json";
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_test" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_ios" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_android" + str;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_amazon" + str;
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1031/BuffSettings_WP" + str;
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06004352 RID: 17234 RVA: 0x0016797C File Offset: 0x00165B7C
	public static string BuffSettings1050
	{
		get
		{
			string str = (!StoreKitEventListener.IsPayingUser()) ? ".json" : "_paying.json";
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_test" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_ios" + str;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_android" + str;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_amazon" + str;
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BuffSettings1050/BuffSettings_WP" + str;
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B20 RID: 2848
	// (get) Token: 0x06004353 RID: 17235 RVA: 0x00167A30 File Offset: 0x00165C30
	public static string ABTestBuffSettingsURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/abTestBuffSettings/wp.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B21 RID: 2849
	// (get) Token: 0x06004354 RID: 17236 RVA: 0x00167AAC File Offset: 0x00165CAC
	public static string LobbyNews
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_androd.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/lobbyNews/LobbyNews_wp.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B22 RID: 2850
	// (get) Token: 0x06004355 RID: 17237 RVA: 0x00167B28 File Offset: 0x00165D28
	public static string NewAdvertisingConfig
	{
		get
		{
			string empty = string.Empty;
			if (Defs.IsDeveloperBuild)
			{
				return URLs.FormatNewAdvertisingConfigUrl("advert-test", empty);
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return URLs.FormatNewAdvertisingConfigUrl("advert-ios", empty);
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return URLs.FormatNewAdvertisingConfigUrl("advert-amazon", empty);
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return URLs.FormatNewAdvertisingConfigUrl("advert-android", empty);
				}
			}
			return URLs.FormatNewAdvertisingConfigUrl("fallback", empty);
		}
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x06004356 RID: 17238 RVA: 0x00167BB0 File Offset: 0x00165DB0
	public static string NewPerelivConfig
	{
		get
		{
			return URLs._newPerelivConfigUrl.Value;
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x00167BBC File Offset: 0x00165DBC
	private static string GetNewPerelivConfigUrl()
	{
		if (Defs.IsDeveloperBuild)
		{
			return URLs.FormatNewPerelivConfigUrl("pereliv-test");
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			return URLs.FormatNewPerelivConfigUrl("pereliv-ios");
		}
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				return URLs.FormatNewPerelivConfigUrl("pereliv-amazon");
			}
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return URLs.FormatNewPerelivConfigUrl("pereliv-android");
			}
		}
		return URLs.FormatNewPerelivConfigUrl("pereliv-fallback");
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x00167C38 File Offset: 0x00165E38
	private static string FormatNewAdvertisingConfigUrl(string name, string suffix)
	{
		return string.Format(CultureInfo.InvariantCulture, "https://secure.pixelgunserver.com/pixelgun3d-config/advert-v2/{0}{1}.json", new object[]
		{
			name,
			suffix
		});
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x00167C64 File Offset: 0x00165E64
	private static string FormatNewPerelivConfigUrl(string name)
	{
		return "https://secure.pixelgunserver.com/pixelgun3d-config/pereliv/" + name + ".json";
	}

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x0600435A RID: 17242 RVA: 0x00167C84 File Offset: 0x00165E84
	public static string Advert
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_ios_TEST.json";
				}
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
				{
					return string.Empty;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_amazon_TEST.json";
				}
				return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_android_TEST.json";
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_ios.json";
				}
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
				{
					return string.Empty;
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_amazon.json";
				}
				return "https://secure.pixelgunserver.com/pixelgun3d-config/advert/advert_android.json";
			}
		}
	}

	// Token: 0x17000B25 RID: 2853
	// (get) Token: 0x0600435B RID: 17243 RVA: 0x00167D10 File Offset: 0x00165F10
	public static string BestBuy
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/bestBuy/best_buy_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B26 RID: 2854
	// (get) Token: 0x0600435C RID: 17244 RVA: 0x00167D8C File Offset: 0x00165F8C
	public static string DayOfValor
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/daysOfValor/days_of_valor_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x0600435D RID: 17245 RVA: 0x00167E08 File Offset: 0x00166008
	public static string PremiumAccount
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/premiumAccount/premium_account_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x0600435E RID: 17246 RVA: 0x00167E84 File Offset: 0x00166084
	public static string PopularityMapUrl
	{
		get
		{
			int num = ExpController.GetOurTier() + 1;
			return string.Concat(new string[]
			{
				"https://secure.pixelgunserver.com/mapstats/",
				GlobalGameController.MultiplayerProtocolVersion,
				"_",
				(ConnectSceneNGUIController.myPlatformConnect - ConnectSceneNGUIController.PlatformConnect.ios).ToString(),
				"_",
				num.ToString(),
				"_mapstat.json"
			});
		}
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00167EE8 File Offset: 0x001660E8
	internal static string Sanitize(WWW request)
	{
		if (request == null)
		{
			return string.Empty;
		}
		if (!request.isDone)
		{
			throw new InvalidOperationException("Request should be done.");
		}
		if (!string.IsNullOrEmpty(request.error))
		{
			return string.Empty;
		}
		UTF8Encoding utf8Encoding = new UTF8Encoding(false);
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64)
		{
			return utf8Encoding.GetString(request.bytes, 0, request.bytes.Length).Trim();
		}
		string result;
		using (StreamReader streamReader = new StreamReader(new MemoryStream(request.bytes), utf8Encoding))
		{
			string text = streamReader.ReadToEnd().Trim();
			result = text;
		}
		return result;
	}

	// Token: 0x04003141 RID: 12609
	public const string UrlForTwitterPost = "http://goo.gl/dQMf4n";

	// Token: 0x04003142 RID: 12610
	public static string BanURL = "https://secure.pixelgunserver.com/pixelgun3d-config/getBanList.php";

	// Token: 0x04003143 RID: 12611
	private static readonly Lazy<string> _trafficForwardingConfigUrl = new Lazy<string>(new Func<string>(URLs.InitializeTrafficForwardingConfigUrl));

	// Token: 0x04003144 RID: 12612
	private static readonly Lazy<string> _newPerelivConfigUrl = new Lazy<string>(new Func<string>(URLs.GetNewPerelivConfigUrl));

	// Token: 0x04003145 RID: 12613
	public static string Friends = "https://pixelgunserver.com/~rilisoft/action.php";

	// Token: 0x04003146 RID: 12614
	public static string TimeOnSecure = "https://secure.pixelgunserver.com/get_time.php";
}
