using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x02000186 RID: 390
	public interface IDiscoveryListener
	{
		// Token: 0x06000CA2 RID: 3234
		void OnEndpointFound(EndpointDetails discoveredEndpoint);

		// Token: 0x06000CA3 RID: 3235
		void OnEndpointLost(string lostEndpointId);
	}
}
