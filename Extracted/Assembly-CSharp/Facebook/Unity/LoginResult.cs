using System;

namespace Facebook.Unity
{
	// Token: 0x02000106 RID: 262
	internal class LoginResult : ResultBase, ILoginResult, IResult
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x0002F61C File Offset: 0x0002D81C
		internal LoginResult(string response) : base(response)
		{
			if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
			{
				this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0002F6EC File Offset: 0x0002D8EC
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0002F6F4 File Offset: 0x0002D8F4
		public AccessToken AccessToken { get; private set; }

		// Token: 0x04000679 RID: 1657
		public const string LastRefreshKey = "last_refresh";

		// Token: 0x0400067A RID: 1658
		public static readonly string UserIdKey = (!Constants.IsWeb) ? "user_id" : "userID";

		// Token: 0x0400067B RID: 1659
		public static readonly string ExpirationTimestampKey = (!Constants.IsWeb) ? "expiration_timestamp" : "expiresIn";

		// Token: 0x0400067C RID: 1660
		public static readonly string PermissionsKey = (!Constants.IsWeb) ? "permissions" : "grantedScopes";

		// Token: 0x0400067D RID: 1661
		public static readonly string AccessTokenKey = (!Constants.IsWeb) ? "access_token" : "accessToken";
	}
}
