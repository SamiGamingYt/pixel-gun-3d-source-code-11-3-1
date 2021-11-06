using System;

// Token: 0x0200055A RID: 1370
internal sealed class AdvertisementInfo
{
	// Token: 0x06002F9A RID: 12186 RVA: 0x000F91F4 File Offset: 0x000F73F4
	public AdvertisementInfo(int round, int slot, int unit = 0, string details = null)
	{
		this._round = round;
		this._slot = slot;
		this._unit = unit;
		this._details = (details ?? string.Empty);
	}

	// Token: 0x17000836 RID: 2102
	// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000F9238 File Offset: 0x000F7438
	public int Round
	{
		get
		{
			return this._round;
		}
	}

	// Token: 0x17000837 RID: 2103
	// (get) Token: 0x06002F9D RID: 12189 RVA: 0x000F9240 File Offset: 0x000F7440
	public int Slot
	{
		get
		{
			return this._slot;
		}
	}

	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000F9248 File Offset: 0x000F7448
	public int Unit
	{
		get
		{
			return this._unit;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06002F9F RID: 12191 RVA: 0x000F9250 File Offset: 0x000F7450
	public string Details
	{
		get
		{
			return this._details;
		}
	}

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06002FA0 RID: 12192 RVA: 0x000F9258 File Offset: 0x000F7458
	public static AdvertisementInfo Default
	{
		get
		{
			return AdvertisementInfo._default;
		}
	}

	// Token: 0x04002304 RID: 8964
	private static readonly AdvertisementInfo _default = new AdvertisementInfo(-1, -1, -1, null);

	// Token: 0x04002305 RID: 8965
	private readonly int _round;

	// Token: 0x04002306 RID: 8966
	private readonly int _slot;

	// Token: 0x04002307 RID: 8967
	private readonly int _unit;

	// Token: 0x04002308 RID: 8968
	private readonly string _details;
}
