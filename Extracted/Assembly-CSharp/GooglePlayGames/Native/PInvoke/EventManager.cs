using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000239 RID: 569
	internal class EventManager
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x0004CD00 File Offset: 0x0004AF00
		internal EventManager(GameServices services)
		{
			this.mServices = Misc.CheckNotNull<GameServices>(services);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0004CD14 File Offset: 0x0004AF14
		internal void FetchAll(Types.DataSource source, Action<EventManager.FetchAllResponse> callback)
		{
			EventManager.EventManager_FetchAll(this.mServices.AsHandle(), source, new EventManager.FetchAllCallback(EventManager.InternalFetchAllCallback), Callbacks.ToIntPtr<EventManager.FetchAllResponse>(callback, new Func<IntPtr, EventManager.FetchAllResponse>(EventManager.FetchAllResponse.FromPointer)));
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x0004CD50 File Offset: 0x0004AF50
		[MonoPInvokeCallback(typeof(EventManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0004CD60 File Offset: 0x0004AF60
		internal void Fetch(Types.DataSource source, string eventId, Action<EventManager.FetchResponse> callback)
		{
			EventManager.EventManager_Fetch(this.mServices.AsHandle(), source, eventId, new EventManager.FetchCallback(EventManager.InternalFetchCallback), Callbacks.ToIntPtr<EventManager.FetchResponse>(callback, new Func<IntPtr, EventManager.FetchResponse>(EventManager.FetchResponse.FromPointer)));
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0004CDA0 File Offset: 0x0004AFA0
		[MonoPInvokeCallback(typeof(EventManager.FetchCallback))]
		internal static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchCallback", Callbacks.Type.Temporary, response, data);
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0004CDB0 File Offset: 0x0004AFB0
		internal void Increment(string eventId, uint steps)
		{
			EventManager.EventManager_Increment(this.mServices.AsHandle(), eventId, steps);
		}

		// Token: 0x04000BFE RID: 3070
		private readonly GameServices mServices;

		// Token: 0x0200023A RID: 570
		internal class FetchResponse : BaseReferenceHolder
		{
			// Token: 0x06001202 RID: 4610 RVA: 0x0004CDC4 File Offset: 0x0004AFC4
			internal FetchResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001203 RID: 4611 RVA: 0x0004CDD0 File Offset: 0x0004AFD0
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return EventManager.EventManager_FetchResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x06001204 RID: 4612 RVA: 0x0004CDE0 File Offset: 0x0004AFE0
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x06001205 RID: 4613 RVA: 0x0004CDEC File Offset: 0x0004AFEC
			internal NativeEvent Data()
			{
				if (!this.RequestSucceeded())
				{
					return null;
				}
				return new NativeEvent(EventManager.EventManager_FetchResponse_GetData(base.SelfPtr()));
			}

			// Token: 0x06001206 RID: 4614 RVA: 0x0004CE0C File Offset: 0x0004B00C
			protected override void CallDispose(HandleRef selfPointer)
			{
				EventManager.EventManager_FetchResponse_Dispose(selfPointer);
			}

			// Token: 0x06001207 RID: 4615 RVA: 0x0004CE14 File Offset: 0x0004B014
			internal static EventManager.FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new EventManager.FetchResponse(pointer);
			}
		}

		// Token: 0x0200023B RID: 571
		internal class FetchAllResponse : BaseReferenceHolder
		{
			// Token: 0x06001208 RID: 4616 RVA: 0x0004CE34 File Offset: 0x0004B034
			internal FetchAllResponse(IntPtr selfPointer) : base(selfPointer)
			{
			}

			// Token: 0x06001209 RID: 4617 RVA: 0x0004CE40 File Offset: 0x0004B040
			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return EventManager.EventManager_FetchAllResponse_GetStatus(base.SelfPtr());
			}

			// Token: 0x0600120A RID: 4618 RVA: 0x0004CE50 File Offset: 0x0004B050
			internal List<NativeEvent> Data()
			{
				IntPtr[] source = PInvokeUtilities.OutParamsToArray<IntPtr>((IntPtr[] out_arg, UIntPtr out_size) => EventManager.EventManager_FetchAllResponse_GetData(base.SelfPtr(), out_arg, out_size));
				return (from ptr in source
				select new NativeEvent(ptr)).ToList<NativeEvent>();
			}

			// Token: 0x0600120B RID: 4619 RVA: 0x0004CE98 File Offset: 0x0004B098
			internal bool RequestSucceeded()
			{
				return this.ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			// Token: 0x0600120C RID: 4620 RVA: 0x0004CEA4 File Offset: 0x0004B0A4
			protected override void CallDispose(HandleRef selfPointer)
			{
				EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
			}

			// Token: 0x0600120D RID: 4621 RVA: 0x0004CEAC File Offset: 0x0004B0AC
			internal static EventManager.FetchAllResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new EventManager.FetchAllResponse(pointer);
			}
		}
	}
}
