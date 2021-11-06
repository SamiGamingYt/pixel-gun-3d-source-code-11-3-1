using System;

// Token: 0x02000407 RID: 1031
public enum DisconnectCause
{
	// Token: 0x040019A7 RID: 6567
	DisconnectByServerUserLimit = 1042,
	// Token: 0x040019A8 RID: 6568
	ExceptionOnConnect = 1023,
	// Token: 0x040019A9 RID: 6569
	DisconnectByServerTimeout = 1041,
	// Token: 0x040019AA RID: 6570
	DisconnectByServerLogic = 1043,
	// Token: 0x040019AB RID: 6571
	Exception = 1026,
	// Token: 0x040019AC RID: 6572
	InvalidAuthentication = 32767,
	// Token: 0x040019AD RID: 6573
	MaxCcuReached = 32757,
	// Token: 0x040019AE RID: 6574
	InvalidRegion = 32756,
	// Token: 0x040019AF RID: 6575
	SecurityExceptionOnConnect = 1022,
	// Token: 0x040019B0 RID: 6576
	DisconnectByClientTimeout = 1040,
	// Token: 0x040019B1 RID: 6577
	InternalReceiveException = 1039,
	// Token: 0x040019B2 RID: 6578
	AuthenticationTicketExpired = 32753
}
