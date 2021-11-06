using System;

// Token: 0x020003FB RID: 1019
public enum EventCaching : byte
{
	// Token: 0x0400194D RID: 6477
	DoNotCache,
	// Token: 0x0400194E RID: 6478
	[Obsolete]
	MergeCache,
	// Token: 0x0400194F RID: 6479
	[Obsolete]
	ReplaceCache,
	// Token: 0x04001950 RID: 6480
	[Obsolete]
	RemoveCache,
	// Token: 0x04001951 RID: 6481
	AddToRoomCache,
	// Token: 0x04001952 RID: 6482
	AddToRoomCacheGlobal,
	// Token: 0x04001953 RID: 6483
	RemoveFromRoomCache,
	// Token: 0x04001954 RID: 6484
	RemoveFromRoomCacheForActorsLeft,
	// Token: 0x04001955 RID: 6485
	SliceIncreaseIndex = 10,
	// Token: 0x04001956 RID: 6486
	SliceSetIndex,
	// Token: 0x04001957 RID: 6487
	SlicePurgeIndex,
	// Token: 0x04001958 RID: 6488
	SlicePurgeUpToIndex
}
