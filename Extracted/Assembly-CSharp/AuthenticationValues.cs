using System;

// Token: 0x02000404 RID: 1028
public class AuthenticationValues
{
	// Token: 0x0600242D RID: 9261 RVA: 0x000B3D54 File Offset: 0x000B1F54
	public AuthenticationValues()
	{
	}

	// Token: 0x0600242E RID: 9262 RVA: 0x000B3D68 File Offset: 0x000B1F68
	public AuthenticationValues(string userId)
	{
		this.UserId = userId;
	}

	// Token: 0x1700065F RID: 1631
	// (get) Token: 0x0600242F RID: 9263 RVA: 0x000B3D84 File Offset: 0x000B1F84
	// (set) Token: 0x06002430 RID: 9264 RVA: 0x000B3D8C File Offset: 0x000B1F8C
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

	// Token: 0x17000660 RID: 1632
	// (get) Token: 0x06002431 RID: 9265 RVA: 0x000B3D98 File Offset: 0x000B1F98
	// (set) Token: 0x06002432 RID: 9266 RVA: 0x000B3DA0 File Offset: 0x000B1FA0
	public string AuthGetParameters { get; set; }

	// Token: 0x17000661 RID: 1633
	// (get) Token: 0x06002433 RID: 9267 RVA: 0x000B3DAC File Offset: 0x000B1FAC
	// (set) Token: 0x06002434 RID: 9268 RVA: 0x000B3DB4 File Offset: 0x000B1FB4
	public object AuthPostData { get; private set; }

	// Token: 0x17000662 RID: 1634
	// (get) Token: 0x06002435 RID: 9269 RVA: 0x000B3DC0 File Offset: 0x000B1FC0
	// (set) Token: 0x06002436 RID: 9270 RVA: 0x000B3DC8 File Offset: 0x000B1FC8
	public string Token { get; set; }

	// Token: 0x17000663 RID: 1635
	// (get) Token: 0x06002437 RID: 9271 RVA: 0x000B3DD4 File Offset: 0x000B1FD4
	// (set) Token: 0x06002438 RID: 9272 RVA: 0x000B3DDC File Offset: 0x000B1FDC
	public string UserId { get; set; }

	// Token: 0x06002439 RID: 9273 RVA: 0x000B3DE8 File Offset: 0x000B1FE8
	public virtual void SetAuthPostData(string stringData)
	{
		this.AuthPostData = ((!string.IsNullOrEmpty(stringData)) ? stringData : null);
	}

	// Token: 0x0600243A RID: 9274 RVA: 0x000B3E04 File Offset: 0x000B2004
	public virtual void SetAuthPostData(byte[] byteData)
	{
		this.AuthPostData = byteData;
	}

	// Token: 0x0600243B RID: 9275 RVA: 0x000B3E10 File Offset: 0x000B2010
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

	// Token: 0x0600243C RID: 9276 RVA: 0x000B3E74 File Offset: 0x000B2074
	public override string ToString()
	{
		return string.Format("AuthenticationValues UserId: {0}, GetParameters: {1} Token available: {2}", this.UserId, this.AuthGetParameters, this.Token != null);
	}

	// Token: 0x04001986 RID: 6534
	private CustomAuthenticationType authType = CustomAuthenticationType.None;
}
