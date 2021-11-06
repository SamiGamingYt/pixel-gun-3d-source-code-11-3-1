using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x0200046E RID: 1134
	public class AuthenticationValues
	{
		// Token: 0x060027B6 RID: 10166 RVA: 0x000C6454 File Offset: 0x000C4654
		public AuthenticationValues()
		{
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x000C6468 File Offset: 0x000C4668
		public AuthenticationValues(string userId)
		{
			this.UserId = userId;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x000C6484 File Offset: 0x000C4684
		// (set) Token: 0x060027B9 RID: 10169 RVA: 0x000C648C File Offset: 0x000C468C
		public CustomAuthenticationType AuthType
		{
			get
			{
				return this.authType;
			}
			set
			{
				this.authType = value;
			}
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060027BA RID: 10170 RVA: 0x000C6498 File Offset: 0x000C4698
		// (set) Token: 0x060027BB RID: 10171 RVA: 0x000C64A0 File Offset: 0x000C46A0
		public string AuthGetParameters { get; set; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x000C64AC File Offset: 0x000C46AC
		// (set) Token: 0x060027BD RID: 10173 RVA: 0x000C64B4 File Offset: 0x000C46B4
		public object AuthPostData { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060027BE RID: 10174 RVA: 0x000C64C0 File Offset: 0x000C46C0
		// (set) Token: 0x060027BF RID: 10175 RVA: 0x000C64C8 File Offset: 0x000C46C8
		public string Token { get; set; }

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060027C0 RID: 10176 RVA: 0x000C64D4 File Offset: 0x000C46D4
		// (set) Token: 0x060027C1 RID: 10177 RVA: 0x000C64DC File Offset: 0x000C46DC
		public string UserId { get; set; }

		// Token: 0x060027C2 RID: 10178 RVA: 0x000C64E8 File Offset: 0x000C46E8
		public virtual void SetAuthPostData(string stringData)
		{
			this.AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000C6504 File Offset: 0x000C4704
		public virtual void SetAuthPostData(byte[] byteData)
		{
			this.AuthPostData = byteData;
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000C6510 File Offset: 0x000C4710
		public virtual void AddAuthParameter(string key, string value)
		{
			string text = (!string.IsNullOrEmpty(this.AuthGetParameters)) ? "&" : string.Empty;
			this.AuthGetParameters = string.Format("{0}{1}{2}={3}", new object[]
			{
				this.AuthGetParameters,
				text,
				Uri.EscapeDataString(key),
				Uri.EscapeDataString(value)
			});
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000C6574 File Offset: 0x000C4774
		public override string ToString()
		{
			return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
		}

		// Token: 0x04001BE6 RID: 7142
		private CustomAuthenticationType authType = CustomAuthenticationType.None;
	}
}
