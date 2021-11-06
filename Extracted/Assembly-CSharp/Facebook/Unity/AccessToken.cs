using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.MiniJSON;

namespace Facebook.Unity
{
	// Token: 0x020000B7 RID: 183
	public class AccessToken
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0002B5E8 File Offset: 0x000297E8
		internal AccessToken(string tokenString, string userId, DateTime expirationTime, IEnumerable<string> permissions, DateTime? lastRefresh)
		{
			if (string.IsNullOrEmpty(tokenString))
			{
				throw new ArgumentNullException("tokenString");
			}
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentNullException("userId");
			}
			if (expirationTime == DateTime.MinValue)
			{
				throw new ArgumentException("Expiration time is unassigned");
			}
			if (permissions == null)
			{
				throw new ArgumentNullException("permissions");
			}
			this.TokenString = tokenString;
			this.ExpirationTime = expirationTime;
			this.Permissions = permissions;
			this.UserId = userId;
			this.LastRefresh = lastRefresh;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0002B67C File Offset: 0x0002987C
		// (set) Token: 0x0600056B RID: 1387 RVA: 0x0002B684 File Offset: 0x00029884
		public static AccessToken CurrentAccessToken { get; internal set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0002B68C File Offset: 0x0002988C
		// (set) Token: 0x0600056D RID: 1389 RVA: 0x0002B694 File Offset: 0x00029894
		public string TokenString { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x0002B6A0 File Offset: 0x000298A0
		// (set) Token: 0x0600056F RID: 1391 RVA: 0x0002B6A8 File Offset: 0x000298A8
		public DateTime ExpirationTime { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0002B6B4 File Offset: 0x000298B4
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0002B6BC File Offset: 0x000298BC
		public IEnumerable<string> Permissions { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x0002B6C8 File Offset: 0x000298C8
		// (set) Token: 0x06000573 RID: 1395 RVA: 0x0002B6D0 File Offset: 0x000298D0
		public string UserId { get; private set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0002B6DC File Offset: 0x000298DC
		// (set) Token: 0x06000575 RID: 1397 RVA: 0x0002B6E4 File Offset: 0x000298E4
		public DateTime? LastRefresh { get; private set; }

		// Token: 0x06000576 RID: 1398 RVA: 0x0002B6F0 File Offset: 0x000298F0
		internal string ToJson()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[LoginResult.PermissionsKey] = string.Join(",", this.Permissions.ToArray<string>());
			dictionary[LoginResult.ExpirationTimestampKey] = this.ExpirationTime.TotalSeconds().ToString();
			dictionary[LoginResult.AccessTokenKey] = this.TokenString;
			dictionary[LoginResult.UserIdKey] = this.UserId;
			if (this.LastRefresh != null)
			{
				dictionary["last_refresh"] = this.LastRefresh.Value.TotalSeconds().ToString();
			}
			return Json.Serialize(dictionary);
		}
	}
}
