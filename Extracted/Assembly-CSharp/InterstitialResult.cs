using System;

// Token: 0x02000621 RID: 1569
internal sealed class InterstitialResult
{
	// Token: 0x06003658 RID: 13912 RVA: 0x00118C04 File Offset: 0x00116E04
	private InterstitialResult(string closeReason, string errorMessage)
	{
		this._closeReason = (closeReason ?? string.Empty);
		this._errorMessage = (errorMessage ?? string.Empty);
	}

	// Token: 0x170008EC RID: 2284
	// (get) Token: 0x06003659 RID: 13913 RVA: 0x00118C40 File Offset: 0x00116E40
	public string CloseReason
	{
		get
		{
			return this._closeReason;
		}
	}

	// Token: 0x170008ED RID: 2285
	// (get) Token: 0x0600365A RID: 13914 RVA: 0x00118C48 File Offset: 0x00116E48
	public string ErrorMessage
	{
		get
		{
			return this._errorMessage;
		}
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x00118C50 File Offset: 0x00116E50
	public static InterstitialResult FromCloseReason(string closeReason)
	{
		return new InterstitialResult(closeReason, string.Empty);
	}

	// Token: 0x0600365C RID: 13916 RVA: 0x00118C60 File Offset: 0x00116E60
	public static InterstitialResult FromErrorMessage(string errorMessage)
	{
		return new InterstitialResult(string.Empty, errorMessage);
	}

	// Token: 0x040027E1 RID: 10209
	private readonly string _closeReason;

	// Token: 0x040027E2 RID: 10210
	private readonly string _errorMessage;
}
