using System;

// Token: 0x02000410 RID: 1040
internal class PunEvent
{
	// Token: 0x040019F7 RID: 6647
	public const byte RPC = 200;

	// Token: 0x040019F8 RID: 6648
	public const byte SendSerialize = 201;

	// Token: 0x040019F9 RID: 6649
	public const byte Instantiation = 202;

	// Token: 0x040019FA RID: 6650
	public const byte CloseConnection = 203;

	// Token: 0x040019FB RID: 6651
	public const byte Destroy = 204;

	// Token: 0x040019FC RID: 6652
	public const byte RemoveCachedRPCs = 205;

	// Token: 0x040019FD RID: 6653
	public const byte SendSerializeReliable = 206;

	// Token: 0x040019FE RID: 6654
	public const byte DestroyPlayer = 207;

	// Token: 0x040019FF RID: 6655
	public const byte AssignMaster = 208;

	// Token: 0x04001A00 RID: 6656
	public const byte OwnershipRequest = 209;

	// Token: 0x04001A01 RID: 6657
	public const byte OwnershipTransfer = 210;

	// Token: 0x04001A02 RID: 6658
	public const byte VacantViewIds = 211;
}
