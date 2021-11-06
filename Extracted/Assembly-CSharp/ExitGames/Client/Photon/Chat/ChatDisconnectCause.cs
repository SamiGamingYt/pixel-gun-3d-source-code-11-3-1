using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000468 RID: 1128
	public enum ChatDisconnectCause
	{
		// Token: 0x04001BB0 RID: 7088
		None,
		// Token: 0x04001BB1 RID: 7089
		DisconnectByServerUserLimit,
		// Token: 0x04001BB2 RID: 7090
		ExceptionOnConnect,
		// Token: 0x04001BB3 RID: 7091
		DisconnectByServer,
		// Token: 0x04001BB4 RID: 7092
		TimeoutDisconnect,
		// Token: 0x04001BB5 RID: 7093
		Exception,
		// Token: 0x04001BB6 RID: 7094
		InvalidAuthentication,
		// Token: 0x04001BB7 RID: 7095
		MaxCcuReached,
		// Token: 0x04001BB8 RID: 7096
		InvalidRegion,
		// Token: 0x04001BB9 RID: 7097
		OperationNotAllowedInCurrentState,
		// Token: 0x04001BBA RID: 7098
		CustomAuthenticationFailed
	}
}
