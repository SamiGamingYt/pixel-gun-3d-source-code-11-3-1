using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.Events
{
	// Token: 0x0200016E RID: 366
	public interface IEventsClient
	{
		// Token: 0x06000BD4 RID: 3028
		void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback);

		// Token: 0x06000BD5 RID: 3029
		void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback);

		// Token: 0x06000BD6 RID: 3030
		void IncrementEvent(string eventId, uint stepsToIncrement);
	}
}
