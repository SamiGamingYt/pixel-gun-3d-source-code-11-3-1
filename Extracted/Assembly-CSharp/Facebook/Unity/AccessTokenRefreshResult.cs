using System;

namespace Facebook.Unity
{
	// Token: 0x020000F3 RID: 243
	internal class AccessTokenRefreshResult : ResultBase, IAccessTokenRefreshResult, IResult
	{
		// Token: 0x0600077C RID: 1916 RVA: 0x0002F28C File Offset: 0x0002D48C
		public AccessTokenRefreshResult(string result) : base(result)
		{
			if (this.ResultDictionary != null && this.ResultDictionary.ContainsKey(LoginResult.AccessTokenKey))
			{
				this.AccessToken = Utilities.ParseAccessTokenFromResult(this.ResultDictionary);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0002F2D4 File Offset: 0x0002D4D4
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0002F2DC File Offset: 0x0002D4DC
		public AccessToken AccessToken { get; private set; }
	}
}
