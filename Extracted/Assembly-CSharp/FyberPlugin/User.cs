using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using FyberPlugin.LitJson;
using UnityEngine;

namespace FyberPlugin
{
	// Token: 0x02000132 RID: 306
	public class User
	{
		// Token: 0x06000995 RID: 2453 RVA: 0x00039DBC File Offset: 0x00037FBC
		protected static void NativePut(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("put", new object[]
				{
					json
				});
			}
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00039E20 File Offset: 0x00038020
		protected static string GetJsonMessage(string key)
		{
			string result;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper", new object[0]))
			{
				result = androidJavaObject.CallStatic<string>("get", new object[]
				{
					key
				});
			}
			return result;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00039E88 File Offset: 0x00038088
		public static void SetAge(int age)
		{
			User.Put("age", age);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00039E9C File Offset: 0x0003809C
		public static DateTime? GetBirthdate()
		{
			string s = User.Get<string>("birthdate");
			DateTime value;
			if (DateTime.TryParseExact(s, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out value))
			{
				return new DateTime?(value);
			}
			return null;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00039EDC File Offset: 0x000380DC
		public static void SetBirthdate(DateTime birthdate)
		{
			User.Put("birthdate", birthdate);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00039EF0 File Offset: 0x000380F0
		public static void SetGender(UserGender gender)
		{
			User.Put("gender", gender);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00039F04 File Offset: 0x00038104
		public static void SetSexualOrientation(UserSexualOrientation sexualOrientation)
		{
			User.Put("sexual_orientation", sexualOrientation);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00039F18 File Offset: 0x00038118
		public static void SetEthnicity(UserEthnicity ethnicity)
		{
			User.Put("ethnicity", ethnicity);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00039F2C File Offset: 0x0003812C
		public static Location GetLocation()
		{
			return User.Get<Location>("fyberlocation");
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00039F38 File Offset: 0x00038138
		public static void SetLocation(Location location)
		{
			User.Put("fyberlocation", location);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00039F48 File Offset: 0x00038148
		public static void SetMaritalStatus(UserMaritalStatus maritalStatus)
		{
			User.Put("marital_status", maritalStatus);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00039F5C File Offset: 0x0003815C
		public static void SetNumberOfChildrens(int numberOfChildrens)
		{
			User.Put("children", numberOfChildrens);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00039F70 File Offset: 0x00038170
		public static void SetAnnualHouseholdIncome(int annualHouseholdIncome)
		{
			User.Put("annual_household_income", annualHouseholdIncome);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00039F84 File Offset: 0x00038184
		public static void SetEducation(UserEducation education)
		{
			User.Put("education", education);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00039F98 File Offset: 0x00038198
		public static string GetZipcode()
		{
			return User.Get<string>("zipcode");
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00039FA4 File Offset: 0x000381A4
		public static void SetZipcode(string zipcode)
		{
			User.Put("zipcode", zipcode);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00039FB4 File Offset: 0x000381B4
		public static string[] GetInterests()
		{
			return User.Get<string[]>("interests");
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00039FC0 File Offset: 0x000381C0
		public static void SetInterests(string[] interests)
		{
			User.Put("interests", interests);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00039FD0 File Offset: 0x000381D0
		public static void SetIap(bool iap)
		{
			User.Put("iap", iap);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00039FE4 File Offset: 0x000381E4
		public static void SetIapAmount(float iap_amount)
		{
			User.Put("iap_amount", (double)iap_amount);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00039FF8 File Offset: 0x000381F8
		public static void SetNumberOfSessions(int numberOfSessions)
		{
			User.Put("number_of_sessions", numberOfSessions);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003A00C File Offset: 0x0003820C
		public static void SetPsTime(long ps_time)
		{
			User.Put("ps_time", ps_time);
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0003A020 File Offset: 0x00038220
		public static void SetLastSession(long lastSession)
		{
			User.Put("last_session", lastSession);
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0003A034 File Offset: 0x00038234
		public static void SetConnection(UserConnection connection)
		{
			User.Put("connection", connection);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003A048 File Offset: 0x00038248
		public static string GetDevice()
		{
			return User.Get<string>("device");
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0003A054 File Offset: 0x00038254
		public static void SetDevice(string device)
		{
			User.Put("device", device);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003A064 File Offset: 0x00038264
		public static string GetAppVersion()
		{
			return User.Get<string>("app_version");
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003A070 File Offset: 0x00038270
		public static void SetAppVersion(string appVersion)
		{
			User.Put("app_version", appVersion);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003A080 File Offset: 0x00038280
		public static void PutCustomValue(string key, string value)
		{
			User.Put(key, value);
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0003A08C File Offset: 0x0003828C
		public static string GetCustomValue(string key)
		{
			return User.Get<string>(key);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003A094 File Offset: 0x00038294
		private static void Put(string key, object value)
		{
			string json = User.GeneratePutJsonString(key, value);
			User.NativePut(json);
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003A0B0 File Offset: 0x000382B0
		protected static T Get<T>(string key)
		{
			string jsonMessage = User.GetJsonMessage(key);
			User.JsonResponse<T> jsonResponse = JsonMapper.ToObject<User.JsonResponse<T>>(jsonMessage);
			if (jsonResponse.success)
			{
				return jsonResponse.value;
			}
			Debug.Log(jsonResponse.error);
			return default(T);
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0003A0F4 File Offset: 0x000382F4
		private static string GeneratePutJsonString(string key, object value)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("action", "put");
			dictionary.Add("key", key);
			dictionary.Add("type", value.GetType().ToString());
			if (value is DateTime)
			{
				dictionary.Add("value", ((DateTime)value).ToString("yyyy/MM/dd"));
			}
			else
			{
				dictionary.Add("value", value);
			}
			return JsonMapper.ToJson(dictionary);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003A17C File Offset: 0x0003837C
		protected static string GenerateGetJsonString(string key)
		{
			return JsonMapper.ToJson(new Dictionary<string, string>
			{
				{
					"action",
					"get"
				},
				{
					"key",
					key
				}
			});
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003A1B4 File Offset: 0x000383B4
		public static int? GetAge()
		{
			return User.Get<int?>("age");
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0003A1C0 File Offset: 0x000383C0
		public static UserGender? GetGender()
		{
			return User.Get<UserGender?>("gender");
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0003A1CC File Offset: 0x000383CC
		public static UserSexualOrientation? GetSexualOrientation()
		{
			return User.Get<UserSexualOrientation?>("sexual_orientation");
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003A1D8 File Offset: 0x000383D8
		public static UserEthnicity? GetEthnicity()
		{
			return User.Get<UserEthnicity?>("ethnicity");
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003A1E4 File Offset: 0x000383E4
		public static UserMaritalStatus? GetMaritalStatus()
		{
			return User.Get<UserMaritalStatus?>("marital_status");
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0003A1F0 File Offset: 0x000383F0
		public static int? GetNumberOfChildrens()
		{
			return User.Get<int?>("children");
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0003A1FC File Offset: 0x000383FC
		public static int? GetAnnualHouseholdIncome()
		{
			return User.Get<int?>("annual_household_income");
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003A208 File Offset: 0x00038408
		public static UserEducation? GetEducation()
		{
			return User.Get<UserEducation?>("education");
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0003A214 File Offset: 0x00038414
		public static bool? GetIap()
		{
			return User.Get<bool?>("iap");
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0003A220 File Offset: 0x00038420
		public static float? GetIapAmount()
		{
			double? num = User.Get<double?>("iap_amount");
			return (num == null) ? null : new float?((float)num.Value);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003A260 File Offset: 0x00038460
		public static int? GetNumberOfSessions()
		{
			return User.Get<int?>("number_of_sessions");
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0003A26C File Offset: 0x0003846C
		public static long? GetPsTime()
		{
			return User.Get<long?>("ps_time");
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0003A278 File Offset: 0x00038478
		public static long? GetLastSession()
		{
			return User.Get<long?>("last_session");
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0003A284 File Offset: 0x00038484
		public static UserConnection? GetConnection()
		{
			return User.Get<UserConnection?>("connection");
		}

		// Token: 0x040007CF RID: 1999
		protected const string AGE = "age";

		// Token: 0x040007D0 RID: 2000
		protected const string BIRTHDATE = "birthdate";

		// Token: 0x040007D1 RID: 2001
		protected const string GENDER = "gender";

		// Token: 0x040007D2 RID: 2002
		protected const string SEXUAL_ORIENTATION = "sexual_orientation";

		// Token: 0x040007D3 RID: 2003
		protected const string ETHNICITY = "ethnicity";

		// Token: 0x040007D4 RID: 2004
		protected const string MARITAL_STATUS = "marital_status";

		// Token: 0x040007D5 RID: 2005
		protected const string NUMBER_OF_CHILDRENS = "children";

		// Token: 0x040007D6 RID: 2006
		protected const string ANNUAL_HOUSEHOLD_INCOME = "annual_household_income";

		// Token: 0x040007D7 RID: 2007
		protected const string EDUCATION = "education";

		// Token: 0x040007D8 RID: 2008
		protected const string ZIPCODE = "zipcode";

		// Token: 0x040007D9 RID: 2009
		protected const string INTERESTS = "interests";

		// Token: 0x040007DA RID: 2010
		protected const string IAP = "iap";

		// Token: 0x040007DB RID: 2011
		protected const string IAP_AMOUNT = "iap_amount";

		// Token: 0x040007DC RID: 2012
		protected const string NUMBER_OF_SESSIONS = "number_of_sessions";

		// Token: 0x040007DD RID: 2013
		protected const string PS_TIME = "ps_time";

		// Token: 0x040007DE RID: 2014
		protected const string LAST_SESSION = "last_session";

		// Token: 0x040007DF RID: 2015
		protected const string CONNECTION = "connection";

		// Token: 0x040007E0 RID: 2016
		protected const string DEVICE = "device";

		// Token: 0x040007E1 RID: 2017
		protected const string APP_VERSION = "app_version";

		// Token: 0x040007E2 RID: 2018
		protected const string LOCATION = "fyberlocation";

		// Token: 0x02000133 RID: 307
		[Obfuscation(Exclude = true)]
		private class JsonResponse<T>
		{
			// Token: 0x170000FF RID: 255
			// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0003A298 File Offset: 0x00038498
			// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0003A2A0 File Offset: 0x000384A0
			public bool success { get; set; }

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0003A2AC File Offset: 0x000384AC
			// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0003A2B4 File Offset: 0x000384B4
			public string key { get; set; }

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x060009CA RID: 2506 RVA: 0x0003A2C0 File Offset: 0x000384C0
			// (set) Token: 0x060009CB RID: 2507 RVA: 0x0003A2C8 File Offset: 0x000384C8
			public T value { get; set; }

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x060009CC RID: 2508 RVA: 0x0003A2D4 File Offset: 0x000384D4
			// (set) Token: 0x060009CD RID: 2509 RVA: 0x0003A2DC File Offset: 0x000384DC
			public string error { get; set; }
		}
	}
}
