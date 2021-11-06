using System;

// Token: 0x020003F2 RID: 1010
public class ErrorCode
{
	// Token: 0x040018B4 RID: 6324
	public const int Ok = 0;

	// Token: 0x040018B5 RID: 6325
	public const int OperationNotAllowedInCurrentState = -3;

	// Token: 0x040018B6 RID: 6326
	[Obsolete("Use InvalidOperation.")]
	public const int InvalidOperationCode = -2;

	// Token: 0x040018B7 RID: 6327
	public const int InvalidOperation = -2;

	// Token: 0x040018B8 RID: 6328
	public const int InternalServerError = -1;

	// Token: 0x040018B9 RID: 6329
	public const int InvalidAuthentication = 32767;

	// Token: 0x040018BA RID: 6330
	public const int GameIdAlreadyExists = 32766;

	// Token: 0x040018BB RID: 6331
	public const int GameFull = 32765;

	// Token: 0x040018BC RID: 6332
	public const int GameClosed = 32764;

	// Token: 0x040018BD RID: 6333
	[Obsolete("No longer used, cause random matchmaking is no longer a process.")]
	public const int AlreadyMatched = 32763;

	// Token: 0x040018BE RID: 6334
	public const int ServerFull = 32762;

	// Token: 0x040018BF RID: 6335
	public const int UserBlocked = 32761;

	// Token: 0x040018C0 RID: 6336
	public const int NoRandomMatchFound = 32760;

	// Token: 0x040018C1 RID: 6337
	public const int GameDoesNotExist = 32758;

	// Token: 0x040018C2 RID: 6338
	public const int MaxCcuReached = 32757;

	// Token: 0x040018C3 RID: 6339
	public const int InvalidRegion = 32756;

	// Token: 0x040018C4 RID: 6340
	public const int CustomAuthenticationFailed = 32755;

	// Token: 0x040018C5 RID: 6341
	public const int AuthenticationTicketExpired = 32753;

	// Token: 0x040018C6 RID: 6342
	public const int PluginReportedError = 32752;

	// Token: 0x040018C7 RID: 6343
	public const int PluginMismatch = 32751;

	// Token: 0x040018C8 RID: 6344
	public const int JoinFailedPeerAlreadyJoined = 32750;

	// Token: 0x040018C9 RID: 6345
	public const int JoinFailedFoundInactiveJoiner = 32749;

	// Token: 0x040018CA RID: 6346
	public const int JoinFailedWithRejoinerNotFound = 32748;

	// Token: 0x040018CB RID: 6347
	public const int JoinFailedFoundExcludedUserId = 32747;

	// Token: 0x040018CC RID: 6348
	public const int JoinFailedFoundActiveJoiner = 32746;

	// Token: 0x040018CD RID: 6349
	public const int HttpLimitReached = 32745;

	// Token: 0x040018CE RID: 6350
	public const int ExternalHttpCallFailed = 32744;

	// Token: 0x040018CF RID: 6351
	public const int SlotError = 32742;

	// Token: 0x040018D0 RID: 6352
	public const int InvalidEncryptionParameters = 32741;
}
