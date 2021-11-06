using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200025E RID: 606
	internal class NearbyConnectionsManager : BaseReferenceHolder
	{
		// Token: 0x06001368 RID: 4968 RVA: 0x0004F7B8 File Offset: 0x0004D9B8
		internal NearbyConnectionsManager(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0004F7D0 File Offset: 0x0004D9D0
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnections.NearbyConnections_Dispose(selfPointer);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0004F7D8 File Offset: 0x0004D9D8
		internal void SendUnreliable(string remoteEndpointId, byte[] payload)
		{
			NearbyConnections.NearbyConnections_SendUnreliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((long)payload.Length)));
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x0004F7F0 File Offset: 0x0004D9F0
		internal void SendReliable(string remoteEndpointId, byte[] payload)
		{
			NearbyConnections.NearbyConnections_SendReliableMessage(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((long)payload.Length)));
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0004F808 File Offset: 0x0004DA08
		internal void StartAdvertising(string name, List<NativeAppIdentifier> appIds, long advertisingDuration, Action<long, NativeStartAdvertisingResult> advertisingCallback, Action<long, NativeConnectionRequest> connectionRequestCallback)
		{
			NearbyConnections.NearbyConnections_StartAdvertising(base.SelfPtr(), name, (from id in appIds
			select id.AsPointer()).ToArray<IntPtr>(), new UIntPtr((ulong)((long)appIds.Count)), advertisingDuration, new NearbyConnectionTypes.StartAdvertisingCallback(NearbyConnectionsManager.InternalStartAdvertisingCallback), Callbacks.ToIntPtr<long, NativeStartAdvertisingResult>(advertisingCallback, new Func<IntPtr, NativeStartAdvertisingResult>(NativeStartAdvertisingResult.FromPointer)), new NearbyConnectionTypes.ConnectionRequestCallback(NearbyConnectionsManager.InternalConnectionRequestCallback), Callbacks.ToIntPtr<long, NativeConnectionRequest>(connectionRequestCallback, new Func<IntPtr, NativeConnectionRequest>(NativeConnectionRequest.FromPointer)));
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0004F894 File Offset: 0x0004DA94
		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.StartAdvertisingCallback))]
		private static void InternalStartAdvertisingCallback(long id, IntPtr result, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalStartAdvertisingCallback", Callbacks.Type.Permanent, id, result, userData);
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x0004F8A4 File Offset: 0x0004DAA4
		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionRequestCallback))]
		private static void InternalConnectionRequestCallback(long id, IntPtr result, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionsManager#InternalConnectionRequestCallback", Callbacks.Type.Permanent, id, result, userData);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x0004F8B4 File Offset: 0x0004DAB4
		internal void StopAdvertising()
		{
			NearbyConnections.NearbyConnections_StopAdvertising(base.SelfPtr());
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x0004F8C4 File Offset: 0x0004DAC4
		internal void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<long, NativeConnectionResponse> callback, NativeMessageListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_SendConnectionRequest(base.SelfPtr(), name, remoteEndpointId, payload, new UIntPtr((ulong)((long)payload.Length)), new NearbyConnectionTypes.ConnectionResponseCallback(NearbyConnectionsManager.InternalConnectResponseCallback), Callbacks.ToIntPtr<long, NativeConnectionResponse>(callback, new Func<IntPtr, NativeConnectionResponse>(NativeConnectionResponse.FromPointer)), listener.AsPointer());
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x0004F910 File Offset: 0x0004DB10
		[MonoPInvokeCallback(typeof(NearbyConnectionTypes.ConnectionResponseCallback))]
		private static void InternalConnectResponseCallback(long localClientId, IntPtr response, IntPtr userData)
		{
			Callbacks.PerformInternalCallback<long>("NearbyConnectionManager#InternalConnectResponseCallback", Callbacks.Type.Temporary, localClientId, response, userData);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x0004F920 File Offset: 0x0004DB20
		internal void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, NativeMessageListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_AcceptConnectionRequest(base.SelfPtr(), remoteEndpointId, payload, new UIntPtr((ulong)((long)payload.Length)), listener.AsPointer());
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0004F94C File Offset: 0x0004DB4C
		internal void DisconnectFromEndpoint(string remoteEndpointId)
		{
			NearbyConnections.NearbyConnections_Disconnect(base.SelfPtr(), remoteEndpointId);
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0004F95C File Offset: 0x0004DB5C
		internal void StopAllConnections()
		{
			NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0004F96C File Offset: 0x0004DB6C
		internal void StartDiscovery(string serviceId, long duration, NativeEndpointDiscoveryListenerHelper listener)
		{
			NearbyConnections.NearbyConnections_StartDiscovery(base.SelfPtr(), serviceId, duration, listener.AsPointer());
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0004F98C File Offset: 0x0004DB8C
		internal void StopDiscovery(string serviceId)
		{
			NearbyConnections.NearbyConnections_StopDiscovery(base.SelfPtr(), serviceId);
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0004F99C File Offset: 0x0004DB9C
		internal void RejectConnectionRequest(string remoteEndpointId)
		{
			NearbyConnections.NearbyConnections_RejectConnectionRequest(base.SelfPtr(), remoteEndpointId);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0004F9AC File Offset: 0x0004DBAC
		internal void Shutdown()
		{
			NearbyConnections.NearbyConnections_Stop(base.SelfPtr());
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0004F9BC File Offset: 0x0004DBBC
		internal string LocalEndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnections.NearbyConnections_GetLocalEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x0004F9D0 File Offset: 0x0004DBD0
		internal string LocalDeviceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnections.NearbyConnections_GetLocalDeviceId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x0004F9E4 File Offset: 0x0004DBE4
		public string AppBundleId
		{
			get
			{
				string result;
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					result = @static.Call<string>("getPackageName", new object[0]);
				}
				return result;
			}
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x0004FA50 File Offset: 0x0004DC50
		internal static string ReadServiceId()
		{
			Debug.Log("Initializing ServiceId property!!!!");
			string result;
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity"))
				{
					string text = @static.Call<string>("getPackageName", new object[0]);
					AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
					AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getApplicationInfo", new object[]
					{
						text,
						128
					});
					AndroidJavaObject androidJavaObject3 = androidJavaObject2.Get<AndroidJavaObject>("metaData");
					string text2 = androidJavaObject3.Call<string>("getString", new object[]
					{
						"com.google.android.gms.nearby.connection.SERVICE_ID"
					});
					Debug.Log("SystemId from Manifest: " + text2);
					result = text2;
				}
			}
			return result;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x0004FB64 File Offset: 0x0004DD64
		public static string ServiceId
		{
			get
			{
				return NearbyConnectionsManager.sServiceId;
			}
		}

		// Token: 0x04000C06 RID: 3078
		private static readonly string sServiceId = NearbyConnectionsManager.ReadServiceId();
	}
}
