using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x0200021B RID: 539
	internal class NativeEventClient : IEventsClient
	{
		// Token: 0x060010D2 RID: 4306 RVA: 0x00048B30 File Offset: 0x00046D30
		internal NativeEventClient(EventManager manager)
		{
			this.mEventManager = Misc.CheckNotNull<EventManager>(manager);
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00048B44 File Offset: 0x00046D44
		public void FetchAllEvents(DataSource source, Action<ResponseStatus, List<IEvent>> callback)
		{
			Misc.CheckNotNull<Action<ResponseStatus, List<IEvent>>>(callback);
			callback = CallbackUtils.ToOnGameThread<ResponseStatus, List<IEvent>>(callback);
			this.mEventManager.FetchAll(ConversionUtils.AsDataSource(source), delegate(EventManager.FetchAllResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, new List<IEvent>());
				}
				else
				{
					callback(arg, response.Data().Cast<IEvent>().ToList<IEvent>());
				}
			});
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00048B98 File Offset: 0x00046D98
		public void FetchEvent(DataSource source, string eventId, Action<ResponseStatus, IEvent> callback)
		{
			Misc.CheckNotNull<string>(eventId);
			Misc.CheckNotNull<Action<ResponseStatus, IEvent>>(callback);
			this.mEventManager.Fetch(ConversionUtils.AsDataSource(source), eventId, delegate(EventManager.FetchResponse response)
			{
				ResponseStatus arg = ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data());
				}
			});
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00048BE4 File Offset: 0x00046DE4
		public void IncrementEvent(string eventId, uint stepsToIncrement)
		{
			Misc.CheckNotNull<string>(eventId);
			this.mEventManager.Increment(eventId, stepsToIncrement);
		}

		// Token: 0x04000BB5 RID: 2997
		private readonly EventManager mEventManager;
	}
}
