using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200026F RID: 623
	internal class RealTimeEventListenerHelper : BaseReferenceHolder
	{
		// Token: 0x060013EF RID: 5103 RVA: 0x0005086C File Offset: 0x0004EA6C
		internal RealTimeEventListenerHelper(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00050878 File Offset: 0x0004EA78
		protected override void CallDispose(HandleRef selfPointer)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_Dispose(selfPointer);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00050880 File Offset: 0x0004EA80
		internal RealTimeEventListenerHelper SetOnRoomStatusChangedCallback(Action<NativeRealTimeRoom> callback)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnRoomStatusChangedCallback(RealTimeEventListenerHelper.InternalOnRoomStatusChangedCallback), RealTimeEventListenerHelper.ToCallbackPointer(callback));
			return this;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x000508A0 File Offset: 0x0004EAA0
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnRoomStatusChangedCallback))]
		internal static void InternalOnRoomStatusChangedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomStatusChangedCallback", Callbacks.Type.Permanent, response, data);
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000508B0 File Offset: 0x0004EAB0
		internal RealTimeEventListenerHelper SetOnRoomConnectedSetChangedCallback(Action<NativeRealTimeRoom> callback)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback(RealTimeEventListenerHelper.InternalOnRoomConnectedSetChangedCallback), RealTimeEventListenerHelper.ToCallbackPointer(callback));
			return this;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000508D0 File Offset: 0x0004EAD0
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback))]
		internal static void InternalOnRoomConnectedSetChangedCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomConnectedSetChangedCallback", Callbacks.Type.Permanent, response, data);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000508E0 File Offset: 0x0004EAE0
		internal RealTimeEventListenerHelper SetOnP2PConnectedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PConnectedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnP2PConnectedCallback(RealTimeEventListenerHelper.InternalOnP2PConnectedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00050900 File Offset: 0x0004EB00
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnP2PConnectedCallback))]
		internal static void InternalOnP2PConnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnP2PConnectedCallback", room, participant, data);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00050910 File Offset: 0x0004EB10
		internal RealTimeEventListenerHelper SetOnP2PDisconnectedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnP2PDisconnectedCallback(RealTimeEventListenerHelper.InternalOnP2PDisconnectedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00050930 File Offset: 0x0004EB30
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnP2PDisconnectedCallback))]
		internal static void InternalOnP2PDisconnectedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnP2PDisconnectedCallback", room, participant, data);
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00050940 File Offset: 0x0004EB40
		internal RealTimeEventListenerHelper SetOnParticipantStatusChangedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant> callback)
		{
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnParticipantStatusChangedCallback(RealTimeEventListenerHelper.InternalOnParticipantStatusChangedCallback), Callbacks.ToIntPtr(callback));
			return this;
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x00050960 File Offset: 0x0004EB60
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnParticipantStatusChangedCallback))]
		internal static void InternalOnParticipantStatusChangedCallback(IntPtr room, IntPtr participant, IntPtr data)
		{
			RealTimeEventListenerHelper.PerformRoomAndParticipantCallback("InternalOnParticipantStatusChangedCallback", room, participant, data);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00050970 File Offset: 0x0004EB70
		internal static void PerformRoomAndParticipantCallback(string callbackName, IntPtr room, IntPtr participant, IntPtr data)
		{
			Logger.d("Entering " + callbackName);
			try
			{
				NativeRealTimeRoom arg = NativeRealTimeRoom.FromPointer(room);
				using (MultiplayerParticipant multiplayerParticipant = MultiplayerParticipant.FromPointer(participant))
				{
					Action<NativeRealTimeRoom, MultiplayerParticipant> action = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, MultiplayerParticipant>>(data);
					if (action != null)
					{
						action(arg, multiplayerParticipant);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.e(string.Concat(new object[]
				{
					"Error encountered executing ",
					callbackName,
					". Smothering to avoid passing exception into Native: ",
					ex
				}));
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00050A2C File Offset: 0x0004EC2C
		internal RealTimeEventListenerHelper SetOnDataReceivedCallback(Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool> callback)
		{
			IntPtr callback_arg = Callbacks.ToIntPtr(callback);
			Logger.d("OnData Callback has addr: " + callback_arg.ToInt64());
			RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnDataReceivedCallback(base.SelfPtr(), new RealTimeEventListenerHelper.OnDataReceivedCallback(RealTimeEventListenerHelper.InternalOnDataReceived), callback_arg);
			return this;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00050A74 File Offset: 0x0004EC74
		[MonoPInvokeCallback(typeof(RealTimeEventListenerHelper.OnDataReceivedCallback))]
		internal static void InternalOnDataReceived(IntPtr room, IntPtr participant, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
		{
			Logger.d("Entering InternalOnDataReceived: " + userData.ToInt64());
			Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool> action = Callbacks.IntPtrToPermanentCallback<Action<NativeRealTimeRoom, MultiplayerParticipant, byte[], bool>>(userData);
			using (NativeRealTimeRoom nativeRealTimeRoom = NativeRealTimeRoom.FromPointer(room))
			{
				using (MultiplayerParticipant multiplayerParticipant = MultiplayerParticipant.FromPointer(participant))
				{
					if (action != null)
					{
						byte[] array = null;
						if (dataLength.ToUInt64() != 0UL)
						{
							array = new byte[dataLength.ToUInt32()];
							Marshal.Copy(data, array, 0, (int)dataLength.ToUInt32());
						}
						try
						{
							action(nativeRealTimeRoom, multiplayerParticipant, array, isReliable);
						}
						catch (Exception arg)
						{
							Logger.e("Error encountered executing InternalOnDataReceived. Smothering to avoid passing exception into Native: " + arg);
						}
					}
				}
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00050B80 File Offset: 0x0004ED80
		private static IntPtr ToCallbackPointer(Action<NativeRealTimeRoom> callback)
		{
			Action<IntPtr> callback2 = delegate(IntPtr result)
			{
				NativeRealTimeRoom nativeRealTimeRoom = NativeRealTimeRoom.FromPointer(result);
				if (callback != null)
				{
					callback(nativeRealTimeRoom);
				}
				else if (nativeRealTimeRoom != null)
				{
					nativeRealTimeRoom.Dispose();
				}
			};
			return Callbacks.ToIntPtr(callback2);
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00050BB0 File Offset: 0x0004EDB0
		internal static RealTimeEventListenerHelper Create()
		{
			return new RealTimeEventListenerHelper(RealTimeEventListenerHelper.RealTimeEventListenerHelper_Construct());
		}
	}
}
