using System;
using Rilisoft;

// Token: 0x02000770 RID: 1904
internal sealed class TrafficForwardingInfo : EventArgs
{
	// Token: 0x0600430E RID: 17166 RVA: 0x00166708 File Offset: 0x00164908
	public TrafficForwardingInfo(string url, int minLevel, int maxLevel)
	{
		this._url = url;
		this._minLevel = minLevel;
		this._maxLevel = maxLevel;
	}

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x06004310 RID: 17168 RVA: 0x00166754 File Offset: 0x00164954
	public static TrafficForwardingInfo DisabledInstance
	{
		get
		{
			return TrafficForwardingInfo._disabledInstance.Value;
		}
	}

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x06004311 RID: 17169 RVA: 0x00166760 File Offset: 0x00164960
	public bool Enabled
	{
		get
		{
			return !string.IsNullOrEmpty(this._url);
		}
	}

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x06004312 RID: 17170 RVA: 0x00166770 File Offset: 0x00164970
	public int MinLevel
	{
		get
		{
			return this._minLevel;
		}
	}

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x06004313 RID: 17171 RVA: 0x00166778 File Offset: 0x00164978
	public int MaxLevel
	{
		get
		{
			return this._maxLevel;
		}
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x06004314 RID: 17172 RVA: 0x00166780 File Offset: 0x00164980
	public string Url
	{
		get
		{
			return this._url;
		}
	}

	// Token: 0x0400311B RID: 12571
	private static readonly Lazy<TrafficForwardingInfo> _disabledInstance = new Lazy<TrafficForwardingInfo>(() => new TrafficForwardingInfo(null, 0, 31));

	// Token: 0x0400311C RID: 12572
	private readonly int _minLevel;

	// Token: 0x0400311D RID: 12573
	private readonly int _maxLevel;

	// Token: 0x0400311E RID: 12574
	private readonly string _url;
}
