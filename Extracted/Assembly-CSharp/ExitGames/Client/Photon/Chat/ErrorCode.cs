using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000470 RID: 1136
	public class ErrorCode
	{
		// Token: 0x04001BF4 RID: 7156
		public const int Ok = 0;

		// Token: 0x04001BF5 RID: 7157
		public const int OperationNotAllowedInCurrentState = -3;

		// Token: 0x04001BF6 RID: 7158
		public const int InvalidOperationCode = -2;

		// Token: 0x04001BF7 RID: 7159
		public const int InternalServerError = -1;

		// Token: 0x04001BF8 RID: 7160
		public const int InvalidAuthentication = 32767;

		// Token: 0x04001BF9 RID: 7161
		public const int GameIdAlreadyExists = 32766;

		// Token: 0x04001BFA RID: 7162
		public const int GameFull = 32765;

		// Token: 0x04001BFB RID: 7163
		public const int GameClosed = 32764;

		// Token: 0x04001BFC RID: 7164
		public const int ServerFull = 32762;

		// Token: 0x04001BFD RID: 7165
		public const int UserBlocked = 32761;

		// Token: 0x04001BFE RID: 7166
		public const int NoRandomMatchFound = 32760;

		// Token: 0x04001BFF RID: 7167
		public const int GameDoesNotExist = 32758;

		// Token: 0x04001C00 RID: 7168
		public const int MaxCcuReached = 32757;

		// Token: 0x04001C01 RID: 7169
		public const int InvalidRegion = 32756;

		// Token: 0x04001C02 RID: 7170
		public const int CustomAuthenticationFailed = 32755;
	}
}
