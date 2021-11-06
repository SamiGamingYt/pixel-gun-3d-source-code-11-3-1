using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Facebook.MiniJSON;

namespace Facebook.Unity
{
	// Token: 0x02000114 RID: 276
	internal static class Utilities
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x00030804 File Offset: 0x0002EA04
		public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
		{
			object obj;
			if (dictionary.TryGetValue(key, out obj) && obj is T)
			{
				value = (T)((object)obj);
				return true;
			}
			value = default(T);
			return false;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00030848 File Offset: 0x0002EA48
		public static long TotalSeconds(this DateTime dateTime)
		{
			return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00030874 File Offset: 0x0002EA74
		public static T GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, bool logWarning = true)
		{
			T result;
			if (!dictionary.TryGetValue(key, out result))
			{
				FacebookLogger.Warn("Did not find expected value '{0}' in dictionary", new string[]
				{
					key
				});
			}
			return result;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000308A4 File Offset: 0x0002EAA4
		public static string ToCommaSeparateList(this IEnumerable<string> list)
		{
			if (list == null)
			{
				return string.Empty;
			}
			return string.Join(",", list.ToArray<string>());
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x000308C4 File Offset: 0x0002EAC4
		public static string AbsoluteUrlOrEmptyString(this Uri uri)
		{
			if (uri == null)
			{
				return string.Empty;
			}
			return uri.AbsoluteUri;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x000308E0 File Offset: 0x0002EAE0
		public static string GetUserAgent(string productName, string productVersion)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", new object[]
			{
				productName,
				productVersion
			});
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00030900 File Offset: 0x0002EB00
		public static string ToJson(this IDictionary<string, object> dictionary)
		{
			return Json.Serialize(dictionary);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00030908 File Offset: 0x0002EB08
		public static void AddAllKVPFrom<T1, T2>(this IDictionary<T1, T2> dest, IDictionary<T1, T2> source)
		{
			foreach (T1 key in source.Keys)
			{
				dest[key] = source[key];
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00030974 File Offset: 0x0002EB74
		public static AccessToken ParseAccessTokenFromResult(IDictionary<string, object> resultDictionary)
		{
			string valueOrDefault = resultDictionary.GetValueOrDefault(LoginResult.UserIdKey, true);
			string valueOrDefault2 = resultDictionary.GetValueOrDefault(LoginResult.AccessTokenKey, true);
			DateTime expirationTime = Utilities.ParseExpirationDateFromResult(resultDictionary);
			ICollection<string> permissions = Utilities.ParsePermissionFromResult(resultDictionary);
			DateTime? lastRefresh = Utilities.ParseLastRefreshFromResult(resultDictionary);
			return new AccessToken(valueOrDefault2, valueOrDefault, expirationTime, permissions, lastRefresh);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x000309BC File Offset: 0x0002EBBC
		private static DateTime ParseExpirationDateFromResult(IDictionary<string, object> resultDictionary)
		{
			DateTime result;
			if (Constants.IsWeb)
			{
				result = DateTime.Now.AddSeconds((double)resultDictionary.GetValueOrDefault(LoginResult.ExpirationTimestampKey, true));
			}
			else
			{
				string valueOrDefault = resultDictionary.GetValueOrDefault(LoginResult.ExpirationTimestampKey, true);
				int num;
				if (int.TryParse(valueOrDefault, out num) && num > 0)
				{
					result = Utilities.FromTimestamp(num);
				}
				else
				{
					result = DateTime.MaxValue;
				}
			}
			return result;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00030A28 File Offset: 0x0002EC28
		private static DateTime? ParseLastRefreshFromResult(IDictionary<string, object> resultDictionary)
		{
			string valueOrDefault = resultDictionary.GetValueOrDefault(LoginResult.ExpirationTimestampKey, true);
			int num;
			if (int.TryParse(valueOrDefault, out num) && num > 0)
			{
				return new DateTime?(Utilities.FromTimestamp(num));
			}
			return null;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00030A6C File Offset: 0x0002EC6C
		private static ICollection<string> ParsePermissionFromResult(IDictionary<string, object> resultDictionary)
		{
			string text;
			IEnumerable<object> source;
			if (resultDictionary.TryGetValue(LoginResult.PermissionsKey, out text))
			{
				source = text.Split(new char[]
				{
					','
				});
			}
			else if (!resultDictionary.TryGetValue(LoginResult.PermissionsKey, out source))
			{
				source = new string[0];
				FacebookLogger.Warn("Failed to find parameter '{0}' in login result", new string[]
				{
					LoginResult.PermissionsKey
				});
			}
			return (from permission in source
			select permission.ToString()).ToList<string>();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00030AFC File Offset: 0x0002ECFC
		private static DateTime FromTimestamp(int timestamp)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return dateTime.AddSeconds((double)timestamp);
		}

		// Token: 0x040006A4 RID: 1700
		private const string WarningMissingParameter = "Did not find expected value '{0}' in dictionary";
	}
}
