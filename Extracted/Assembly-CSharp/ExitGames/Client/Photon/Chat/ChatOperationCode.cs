using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x0200046A RID: 1130
	public class ChatOperationCode
	{
		// Token: 0x04001BC2 RID: 7106
		public const byte Authenticate = 230;

		// Token: 0x04001BC3 RID: 7107
		public const byte Subscribe = 0;

		// Token: 0x04001BC4 RID: 7108
		public const byte Unsubscribe = 1;

		// Token: 0x04001BC5 RID: 7109
		public const byte Publish = 2;

		// Token: 0x04001BC6 RID: 7110
		public const byte SendPrivate = 3;

		// Token: 0x04001BC7 RID: 7111
		public const byte ChannelHistory = 4;

		// Token: 0x04001BC8 RID: 7112
		public const byte UpdateStatus = 5;

		// Token: 0x04001BC9 RID: 7113
		public const byte AddFriends = 6;

		// Token: 0x04001BCA RID: 7114
		public const byte RemoveFriends = 7;
	}
}
