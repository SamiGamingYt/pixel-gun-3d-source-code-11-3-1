using System;

// Token: 0x020003F5 RID: 1013
public class EventCode
{
	// Token: 0x040018DD RID: 6365
	public const byte GameList = 230;

	// Token: 0x040018DE RID: 6366
	public const byte GameListUpdate = 229;

	// Token: 0x040018DF RID: 6367
	public const byte QueueState = 228;

	// Token: 0x040018E0 RID: 6368
	public const byte Match = 227;

	// Token: 0x040018E1 RID: 6369
	public const byte AppStats = 226;

	// Token: 0x040018E2 RID: 6370
	public const byte LobbyStats = 224;

	// Token: 0x040018E3 RID: 6371
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x040018E4 RID: 6372
	public const byte Join = 255;

	// Token: 0x040018E5 RID: 6373
	public const byte Leave = 254;

	// Token: 0x040018E6 RID: 6374
	public const byte PropertiesChanged = 253;

	// Token: 0x040018E7 RID: 6375
	[Obsolete("Use PropertiesChanged now.")]
	public const byte SetProperties = 253;

	// Token: 0x040018E8 RID: 6376
	public const byte ErrorInfo = 251;

	// Token: 0x040018E9 RID: 6377
	public const byte CacheSliceChanged = 250;
}
