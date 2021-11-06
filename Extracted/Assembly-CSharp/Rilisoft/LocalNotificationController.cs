using System;
using System.Collections.Generic;
using System.Globalization;
using Prime31;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200069F RID: 1695
	[DisallowMultipleComponent]
	internal sealed class LocalNotificationController : MonoBehaviour
	{
		// Token: 0x06003B5A RID: 15194 RVA: 0x001345FC File Offset: 0x001327FC
		public LocalNotificationController()
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2239", "Key_2247"));
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2240", "Key_2248"));
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2239", "Key_2248"));
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2240", "Key_2247"));
			}
			else
			{
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2239", "Key_2284"));
				this._returnNotifications.Add(LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2240", "Key_2284"));
			}
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x001346DC File Offset: 0x001328DC
		private void Awake()
		{
			string callee = string.Format("{0}.Awake()", base.GetType().Name);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				this.CancelNotifications();
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0013474C File Offset: 0x0013294C
		private void Destroy()
		{
			string callee = string.Format("{0}.Destroy()", base.GetType().Name);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				this.ScheduleNotifications();
			}
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x001347BC File Offset: 0x001329BC
		private void OnApplicationQuit()
		{
			string callee = string.Format("{0}.OnApplicationQuit()", base.GetType().Name);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				this.ScheduleNotifications();
			}
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0013482C File Offset: 0x00132A2C
		private void OnApplicationPause(bool pauseStatus)
		{
			string callee = string.Format("{0}.OnApplicationPause({1})", base.GetType().Name, pauseStatus);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				if (pauseStatus)
				{
					this.ScheduleNotifications();
				}
				else
				{
					this.CancelNotifications();
				}
			}
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x001348B4 File Offset: 0x00132AB4
		private void ScheduleNotifications()
		{
			string text = string.Format("{0}.ScheduleNotifications()", base.GetType().Name);
			using (new ScopeLogger(text, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				this.ScheduleEggsNotifications();
				this.SchedulePetsNotifications(text);
				this.ScheduleGachaNotifications(text);
				this.ScheduleReturnNotifications();
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00134938 File Offset: 0x00132B38
		private void ScheduleEggsNotifications()
		{
			if (!this.EggsNotificationEnabled)
			{
				return;
			}
			if (Nest.Instance == null)
			{
				return;
			}
			if (Nest.Instance.EggIsReady)
			{
				return;
			}
			if (Nest.Instance.TimeLeft == null)
			{
				return;
			}
			long value = Nest.Instance.TimeLeft.Value;
			DateTime dateTime = DateTime.Now.AddSeconds((double)value);
			LocalNotificationController.LocalNotification localNotification = LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2801", "Key_2801");
			AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration(value, localNotification.Title, localNotification.Subtitle, localNotification.Ticker)
			{
				smallIcon = "small_icon",
				largeIcon = "large_icon",
				requestCode = 3000,
				cancelsNotificationId = 3000
			};
			int num = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration);
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Scheduled notification {0}: `{1}`, `{2}`, `{3}`", new object[]
				{
					num,
					androidNotificationConfiguration.title,
					androidNotificationConfiguration.subtitle,
					androidNotificationConfiguration.tickerText
				});
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00134A64 File Offset: 0x00132C64
		private void SchedulePetsNotifications(string caller)
		{
			if (!this.PetsNotificationEnabled)
			{
				return;
			}
			if (Singleton<EggsManager>.Instance == null)
			{
				return;
			}
			List<Egg> playerEggs = Singleton<EggsManager>.Instance.GetPlayerEggs();
			if (playerEggs == null || playerEggs.Count == 0)
			{
				return;
			}
			Egg egg = null;
			for (int num = 0; num != playerEggs.Count; num++)
			{
				Egg egg2 = playerEggs[num];
				if (egg2.HatchedType == EggHatchedType.Time)
				{
					if (egg2.IncubationTimeLeft != null)
					{
						if (!egg2.CheckReady())
						{
							if (egg == null)
							{
								egg = egg2;
							}
							else if (egg2.IncubationTimeLeft.Value < egg.IncubationTimeLeft.Value)
							{
								egg = egg2;
							}
						}
					}
				}
			}
			if (egg == null)
			{
				return;
			}
			long value = egg.IncubationTimeLeft.Value;
			string callee = string.Format(CultureInfo.InvariantCulture, "Scheduling pet notification in {0}", new object[]
			{
				value
			});
			using (new ScopeLogger(caller, callee, true))
			{
				LocalNotificationController.LocalNotification localNotification = LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2802", "Key_2802");
				AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration(value, localNotification.Title, localNotification.Subtitle, localNotification.Ticker)
				{
					smallIcon = "small_icon",
					largeIcon = "large_icon",
					requestCode = 4000,
					cancelsNotificationId = 4000
				};
				int num2 = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration);
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Scheduled notification {0}: `{1}`, `{2}`, `{3}`", new object[]
					{
						num2,
						androidNotificationConfiguration.title,
						androidNotificationConfiguration.subtitle,
						androidNotificationConfiguration.tickerText
					});
				}
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00134C58 File Offset: 0x00132E58
		private void ScheduleGachaNotifications(string caller)
		{
			if (!this.GachaNotificationEnabled)
			{
				return;
			}
			if (ExperienceController.GetCurrentLevel() < 2)
			{
				return;
			}
			if (GiftController.Instance == null)
			{
				return;
			}
			TimeSpan freeGachaAvailableIn = GiftController.Instance.FreeGachaAvailableIn;
			string callee = string.Format(CultureInfo.InvariantCulture, "Scheduling gacha notification in {0}", new object[]
			{
				freeGachaAvailableIn
			});
			using (new ScopeLogger(caller, callee, true))
			{
				int num = Convert.ToInt32(freeGachaAvailableIn.TotalSeconds);
				if (num > 0)
				{
					LocalNotificationController.LocalNotification localNotification = LocalNotificationController.LocalNotification.FromLocalizationKeys("Key_2225", "Key_2800", "Key_2800");
					AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration((long)num, localNotification.Title, localNotification.Subtitle, localNotification.Ticker)
					{
						smallIcon = "small_icon",
						largeIcon = "large_icon",
						requestCode = 1000,
						cancelsNotificationId = 1000
					};
					int num2 = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration);
					if (Defs.IsDeveloperBuild)
					{
						Debug.LogFormat("Scheduled notification {0}: `{1}`, `{2}`, `{3}`", new object[]
						{
							num2,
							androidNotificationConfiguration.title,
							androidNotificationConfiguration.subtitle,
							androidNotificationConfiguration.tickerText
						});
					}
				}
			}
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00134DB4 File Offset: 0x00132FB4
		private void ScheduleReturnNotifications()
		{
			if (!this.ReturnNotificationEnabled)
			{
				return;
			}
			DateTime now = DateTime.Now;
			List<DateTime> list = new List<DateTime>(3);
			if (Defs.IsDeveloperBuild)
			{
				list.Add(now.AddSeconds(40.0));
				list.Add(now.AddMinutes(1.5));
				list.Add(now.AddMinutes(5.0));
				list.Add(now.AddMinutes(10.0));
			}
			list.Add(this.ClampTimeOfTheDay(now.AddDays(3.0)));
			list.Add(this.ClampTimeOfTheDay(now.AddDays(7.0)));
			int num = UnityEngine.Random.Range(0, this.ReturnNotifications.Count);
			int count = list.Count;
			for (int num2 = 0; num2 != count; num2++)
			{
				DateTime d = list[num2];
				int num3 = Convert.ToInt32((d - now).TotalSeconds);
				int index = (num + num2) % this.ReturnNotifications.Count;
				LocalNotificationController.LocalNotification localNotification = this.ReturnNotifications[index];
				AndroidNotificationConfiguration androidNotificationConfiguration = new AndroidNotificationConfiguration((long)num3, localNotification.Title, localNotification.Subtitle, localNotification.Ticker)
				{
					groupKey = "Return",
					isGroupSummary = (num2 == 0),
					smallIcon = "small_icon",
					largeIcon = "large_icon",
					requestCode = 2000 + num2,
					cancelsNotificationId = 2000 + num2
				};
				if (Defs.IsDeveloperBuild)
				{
					androidNotificationConfiguration.title = string.Format("({0}) {1}", num3, androidNotificationConfiguration.title);
					androidNotificationConfiguration.tickerText = string.Format("({0}) {1}", num3, androidNotificationConfiguration.tickerText);
				}
				int num4 = EtceteraAndroid.scheduleNotification(androidNotificationConfiguration);
				if (Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Scheduled notification {0}: `{1}`, `{2}`, `{3}`", new object[]
					{
						num4,
						androidNotificationConfiguration.title,
						androidNotificationConfiguration.subtitle,
						androidNotificationConfiguration.tickerText
					});
				}
			}
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00134FEC File Offset: 0x001331EC
		private void CancelNotifications()
		{
			string callee = string.Format("{0}.CancelNotifications()", base.GetType().Name);
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild && !Application.isEditor))
			{
				EtceteraAndroid.cancelNotification(1000);
				for (int num = 0; num != 10; num++)
				{
					EtceteraAndroid.cancelNotification(2000 + num);
				}
				EtceteraAndroid.cancelNotification(3000);
				EtceteraAndroid.cancelNotification(4000);
				EtceteraAndroid.cancelAllNotifications();
			}
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x00135098 File Offset: 0x00133298
		private DateTime ClampTimeOfTheDay(DateTime rawDateTime)
		{
			TimeSpan t = new TimeSpan(16, 0, 0);
			TimeSpan t2 = TimeSpan.FromMinutes((double)UnityEngine.Random.Range(-30f, 30f));
			return rawDateTime.Date + t + t2;
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x001350DC File Offset: 0x001332DC
		private int SafeGetSdkLevel()
		{
			int result;
			try
			{
				result = AndroidSystem.GetSdkVersion();
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				result = 0;
			}
			return result;
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x0013512C File Offset: 0x0013332C
		private bool EggsNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003B68 RID: 15208 RVA: 0x00135130 File Offset: 0x00133330
		private bool GachaNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x06003B69 RID: 15209 RVA: 0x00135134 File Offset: 0x00133334
		private bool ReturnNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x06003B6A RID: 15210 RVA: 0x00135138 File Offset: 0x00133338
		private bool PetsNotificationEnabled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x0013513C File Offset: 0x0013333C
		private List<LocalNotificationController.LocalNotification> ReturnNotifications
		{
			get
			{
				return this._returnNotifications;
			}
		}

		// Token: 0x04002BF4 RID: 11252
		private const int GachaNotificationId = 1000;

		// Token: 0x04002BF5 RID: 11253
		private const int ReturnNotificationId = 2000;

		// Token: 0x04002BF6 RID: 11254
		private const int EggNotificationId = 3000;

		// Token: 0x04002BF7 RID: 11255
		private const int PetNotificationId = 4000;

		// Token: 0x04002BF8 RID: 11256
		private readonly List<LocalNotificationController.LocalNotification> _returnNotifications = new List<LocalNotificationController.LocalNotification>(4);

		// Token: 0x020006A0 RID: 1696
		private struct LocalNotification
		{
			// Token: 0x06003B6C RID: 15212 RVA: 0x00135144 File Offset: 0x00133344
			public LocalNotification(string title, string subtitle, string ticker)
			{
				this._title = (title ?? string.Empty);
				this._subtitle = (subtitle ?? string.Empty);
				this._ticker = (ticker ?? string.Empty);
			}

			// Token: 0x06003B6D RID: 15213 RVA: 0x00135180 File Offset: 0x00133380
			public static LocalNotificationController.LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey, string tickerKey)
			{
				string title = LocalizationStore.Get(titleKey ?? string.Empty);
				string subtitle = LocalizationStore.Get(subtitleKey ?? string.Empty);
				string ticker = LocalizationStore.Get(tickerKey ?? string.Empty);
				return new LocalNotificationController.LocalNotification(title, subtitle, ticker);
			}

			// Token: 0x06003B6E RID: 15214 RVA: 0x001351D0 File Offset: 0x001333D0
			public static LocalNotificationController.LocalNotification FromLocalizationKeys(string titleKey, string subtitleKey)
			{
				return LocalNotificationController.LocalNotification.FromLocalizationKeys(titleKey, subtitleKey, titleKey);
			}

			// Token: 0x170009CB RID: 2507
			// (get) Token: 0x06003B6F RID: 15215 RVA: 0x001351DC File Offset: 0x001333DC
			public string Title
			{
				get
				{
					return this._title ?? string.Empty;
				}
			}

			// Token: 0x170009CC RID: 2508
			// (get) Token: 0x06003B70 RID: 15216 RVA: 0x001351F0 File Offset: 0x001333F0
			public string Subtitle
			{
				get
				{
					return this._subtitle ?? string.Empty;
				}
			}

			// Token: 0x170009CD RID: 2509
			// (get) Token: 0x06003B71 RID: 15217 RVA: 0x00135204 File Offset: 0x00133404
			public string Ticker
			{
				get
				{
					return this._ticker ?? string.Empty;
				}
			}

			// Token: 0x04002BF9 RID: 11257
			private readonly string _title;

			// Token: 0x04002BFA RID: 11258
			private readonly string _subtitle;

			// Token: 0x04002BFB RID: 11259
			private readonly string _ticker;
		}
	}
}
