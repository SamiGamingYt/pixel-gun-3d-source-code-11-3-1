using System;

// Token: 0x020003F7 RID: 1015
public class OperationCode
{
	// Token: 0x0400192C RID: 6444
	[Obsolete("Exchanging encrpytion keys is done internally in the lib now. Don't expect this operation-result.")]
	public const byte ExchangeKeysForEncryption = 250;

	// Token: 0x0400192D RID: 6445
	public const byte Join = 255;

	// Token: 0x0400192E RID: 6446
	public const byte AuthenticateOnce = 231;

	// Token: 0x0400192F RID: 6447
	public const byte Authenticate = 230;

	// Token: 0x04001930 RID: 6448
	public const byte JoinLobby = 229;

	// Token: 0x04001931 RID: 6449
	public const byte LeaveLobby = 228;

	// Token: 0x04001932 RID: 6450
	public const byte CreateGame = 227;

	// Token: 0x04001933 RID: 6451
	public const byte JoinGame = 226;

	// Token: 0x04001934 RID: 6452
	public const byte JoinRandomGame = 225;

	// Token: 0x04001935 RID: 6453
	public const byte Leave = 254;

	// Token: 0x04001936 RID: 6454
	public const byte RaiseEvent = 253;

	// Token: 0x04001937 RID: 6455
	public const byte SetProperties = 252;

	// Token: 0x04001938 RID: 6456
	public const byte GetProperties = 251;

	// Token: 0x04001939 RID: 6457
	public const byte ChangeGroups = 248;

	// Token: 0x0400193A RID: 6458
	public const byte FindFriends = 222;

	// Token: 0x0400193B RID: 6459
	public const byte GetLobbyStats = 221;

	// Token: 0x0400193C RID: 6460
	public const byte GetRegions = 220;

	// Token: 0x0400193D RID: 6461
	public const byte WebRpc = 219;

	// Token: 0x0400193E RID: 6462
	public const byte ServerSettings = 218;
}
