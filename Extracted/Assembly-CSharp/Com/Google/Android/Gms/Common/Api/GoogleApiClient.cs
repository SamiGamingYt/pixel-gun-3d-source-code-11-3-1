using System;
using Google.Developers;

namespace Com.Google.Android.Gms.Common.Api
{
	// Token: 0x020001AF RID: 431
	public class GoogleApiClient : JavaObjWrapper
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x00046014 File Offset: 0x00044214
		public GoogleApiClient(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00046020 File Offset: 0x00044220
		public GoogleApiClient() : base("com.google.android.gms.common.api.GoogleApiClient")
		{
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00046030 File Offset: 0x00044230
		public object getContext()
		{
			return base.InvokeCall<object>("getContext", "()Landroid/content/Context;", new object[0]);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00046048 File Offset: 0x00044248
		public void connect()
		{
			base.InvokeCallVoid("connect", "()V", new object[0]);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00046060 File Offset: 0x00044260
		public void disconnect()
		{
			base.InvokeCallVoid("disconnect", "()V", new object[0]);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00046078 File Offset: 0x00044278
		public void dump(string arg_string_1, object arg_object_2, object arg_object_3, string[] arg_string_4)
		{
			base.InvokeCallVoid("dump", "(Ljava/lang/String;Ljava/io/FileDescriptor;Ljava/io/PrintWriter;[Ljava/lang/String;)V", new object[]
			{
				arg_string_1,
				arg_object_2,
				arg_object_3,
				arg_string_4
			});
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000460A4 File Offset: 0x000442A4
		public ConnectionResult blockingConnect(long arg_long_1, object arg_object_2)
		{
			return base.InvokeCall<ConnectionResult>("blockingConnect", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/ConnectionResult;", new object[]
			{
				arg_long_1,
				arg_object_2
			});
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000460CC File Offset: 0x000442CC
		public ConnectionResult blockingConnect()
		{
			return base.InvokeCall<ConnectionResult>("blockingConnect", "()Lcom/google/android/gms/common/ConnectionResult;", new object[0]);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000460E4 File Offset: 0x000442E4
		public PendingResult<Status> clearDefaultAccountAndReconnect()
		{
			return base.InvokeCall<PendingResult<Status>>("clearDefaultAccountAndReconnect", "()Lcom/google/android/gms/common/api/PendingResult;", new object[0]);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000460FC File Offset: 0x000442FC
		public ConnectionResult getConnectionResult(object arg_object_1)
		{
			return base.InvokeCall<ConnectionResult>("getConnectionResult", "(Lcom/google/android/gms/common/api/Api;)Lcom/google/android/gms/common/ConnectionResult;", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00046118 File Offset: 0x00044318
		public int getSessionId()
		{
			return base.InvokeCall<int>("getSessionId", "()I", new object[0]);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00046130 File Offset: 0x00044330
		public bool isConnecting()
		{
			return base.InvokeCall<bool>("isConnecting", "()Z", new object[0]);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00046148 File Offset: 0x00044348
		public bool isConnectionCallbacksRegistered(object arg_object_1)
		{
			return base.InvokeCall<bool>("isConnectionCallbacksRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)Z", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00046164 File Offset: 0x00044364
		public bool isConnectionFailedListenerRegistered(object arg_object_1)
		{
			return base.InvokeCall<bool>("isConnectionFailedListenerRegistered", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)Z", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00046180 File Offset: 0x00044380
		public void reconnect()
		{
			base.InvokeCallVoid("reconnect", "()V", new object[0]);
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00046198 File Offset: 0x00044398
		public void registerConnectionCallbacks(object arg_object_1)
		{
			base.InvokeCallVoid("registerConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000461B4 File Offset: 0x000443B4
		public void registerConnectionFailedListener(object arg_object_1)
		{
			base.InvokeCallVoid("registerConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000461D0 File Offset: 0x000443D0
		public void stopAutoManage(object arg_object_1)
		{
			base.InvokeCallVoid("stopAutoManage", "(Landroid/support/v4/app/FragmentActivity;)V", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000461EC File Offset: 0x000443EC
		public void unregisterConnectionCallbacks(object arg_object_1)
		{
			base.InvokeCallVoid("unregisterConnectionCallbacks", "(Lcom/google/android/gms/common/api/GoogleApiClient$ConnectionCallbacks;)V", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00046208 File Offset: 0x00044408
		public void unregisterConnectionFailedListener(object arg_object_1)
		{
			base.InvokeCallVoid("unregisterConnectionFailedListener", "(Lcom/google/android/gms/common/api/GoogleApiClient$OnConnectionFailedListener;)V", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00046224 File Offset: 0x00044424
		public bool hasConnectedApi(object arg_object_1)
		{
			return base.InvokeCall<bool>("hasConnectedApi", "(Lcom/google/android/gms/common/api/Api;)Z", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00046240 File Offset: 0x00044440
		public object getLooper()
		{
			return base.InvokeCall<object>("getLooper", "()Landroid/os/Looper;", new object[0]);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00046258 File Offset: 0x00044458
		public bool isConnected()
		{
			return base.InvokeCall<bool>("isConnected", "()Z", new object[0]);
		}

		// Token: 0x04000AA6 RID: 2726
		private const string CLASS_NAME = "com/google/android/gms/common/api/GoogleApiClient";
	}
}
