using System;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000473 RID: 1139
	public interface IChatClientListener
	{
		// Token: 0x060027C8 RID: 10184
		void DebugReturn(DebugLevel level, string message);

		// Token: 0x060027C9 RID: 10185
		void OnDisconnected();

		// Token: 0x060027CA RID: 10186
		void OnConnected();

		// Token: 0x060027CB RID: 10187
		void OnChatStateChange(ChatState state);

		// Token: 0x060027CC RID: 10188
		void OnGetMessages(string channelName, string[] senders, object[] messages);

		// Token: 0x060027CD RID: 10189
		void OnPrivateMessage(string sender, object message, string channelName);

		// Token: 0x060027CE RID: 10190
		void OnSubscribed(string[] channels, bool[] results);

		// Token: 0x060027CF RID: 10191
		void OnUnsubscribed(string[] channels);

		// Token: 0x060027D0 RID: 10192
		void OnStatusUpdate(string user, int status, bool gotMessage, object message);
	}
}
