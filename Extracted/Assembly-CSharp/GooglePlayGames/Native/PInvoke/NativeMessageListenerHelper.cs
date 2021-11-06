using System;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200024E RID: 590
	internal class NativeMessageListenerHelper : BaseReferenceHolder
	{
		// Token: 0x060012BD RID: 4797 RVA: 0x0004E4E4 File Offset: 0x0004C6E4
		internal NativeMessageListenerHelper() : base(MessageListenerHelper.MessageListenerHelper_Construct())
		{
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x0004E4F4 File Offset: 0x0004C6F4
		protected override void CallDispose(HandleRef selfPointer)
		{
			MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0004E4FC File Offset: 0x0004C6FC
		internal void SetOnMessageReceivedCallback(NativeMessageListenerHelper.OnMessageReceived callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(base.SelfPtr(), new MessageListenerHelper.OnMessageReceivedCallback(NativeMessageListenerHelper.InternalOnMessageReceivedCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0004E51C File Offset: 0x0004C71C
		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnMessageReceivedCallback))]
		private static void InternalOnMessageReceivedCallback(long id, string name, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
		{
			NativeMessageListenerHelper.OnMessageReceived onMessageReceived = Callbacks.IntPtrToPermanentCallback<NativeMessageListenerHelper.OnMessageReceived>(userData);
			if (onMessageReceived == null)
			{
				return;
			}
			try
			{
				onMessageReceived(id, name, Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0004E580 File Offset: 0x0004C780
		internal void SetOnDisconnectedCallback(Action<long, string> callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(base.SelfPtr(), new MessageListenerHelper.OnDisconnectedCallback(NativeMessageListenerHelper.InternalOnDisconnectedCallback), Callbacks.ToIntPtr(callback));
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0004E5A0 File Offset: 0x0004C7A0
		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnDisconnectedCallback))]
		private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, IntPtr userData)
		{
			Action<long, string> action = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
			if (action == null)
			{
				return;
			}
			try
			{
				action(id, lostEndpointId);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x020008E1 RID: 2273
		// (Invoke) Token: 0x06005024 RID: 20516
		internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);
	}
}
