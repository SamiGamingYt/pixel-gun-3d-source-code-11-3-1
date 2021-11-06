using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000185 RID: 389
	public interface IMessageListener
	{
		// Token: 0x06000CA0 RID: 3232
		void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage);

		// Token: 0x06000CA1 RID: 3233
		void OnRemoteEndpointDisconnected(string remoteEndpointId);
	}
}
