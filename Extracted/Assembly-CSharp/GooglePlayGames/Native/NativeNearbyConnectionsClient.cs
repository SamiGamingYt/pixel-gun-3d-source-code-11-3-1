using System;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x0200021D RID: 541
	internal class NativeNearbyConnectionsClient : INearbyConnectionClient
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x00048D28 File Offset: 0x00046F28
		internal NativeNearbyConnectionsClient(NearbyConnectionsManager manager)
		{
			this.mManager = Misc.CheckNotNull<NearbyConnectionsManager>(manager);
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00048D3C File Offset: 0x00046F3C
		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00048D44 File Offset: 0x00046F44
		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00048D4C File Offset: 0x00046F4C
		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			this.InternalSend(recipientEndpointIds, payload, true);
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00048D58 File Offset: 0x00046F58
		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			this.InternalSend(recipientEndpointIds, payload, false);
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00048D64 File Offset: 0x00046F64
		private void InternalSend(List<string> recipientEndpointIds, byte[] payload, bool isReliable)
		{
			if (recipientEndpointIds == null)
			{
				throw new ArgumentNullException("recipientEndpointIds");
			}
			if (payload == null)
			{
				throw new ArgumentNullException("payload");
			}
			if (recipientEndpointIds.Contains(null))
			{
				throw new InvalidOperationException("Cannot send a message to a null recipient");
			}
			if (recipientEndpointIds.Count == 0)
			{
				Logger.w("Attempted to send a reliable message with no recipients");
				return;
			}
			if (isReliable)
			{
				if (payload.Length > this.MaxReliableMessagePayloadLength())
				{
					throw new InvalidOperationException("cannot send more than " + this.MaxReliableMessagePayloadLength() + " bytes");
				}
			}
			else if (payload.Length > this.MaxUnreliableMessagePayloadLength())
			{
				throw new InvalidOperationException("cannot send more than " + this.MaxUnreliableMessagePayloadLength() + " bytes");
			}
			foreach (string remoteEndpointId in recipientEndpointIds)
			{
				if (isReliable)
				{
					this.mManager.SendReliable(remoteEndpointId, payload);
				}
				else
				{
					this.mManager.SendUnreliable(remoteEndpointId, payload);
				}
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00048E9C File Offset: 0x0004709C
		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> requestCallback)
		{
			Misc.CheckNotNull<List<string>>(appIdentifiers, "appIdentifiers");
			Misc.CheckNotNull<Action<AdvertisingResult>>(resultCallback, "resultCallback");
			Misc.CheckNotNull<Action<ConnectionRequest>>(requestCallback, "connectionRequestCallback");
			if (advertisingDuration != null && advertisingDuration.Value.Ticks < 0L)
			{
				throw new InvalidOperationException("advertisingDuration must be positive");
			}
			resultCallback = Callbacks.AsOnGameThreadCallback<AdvertisingResult>(resultCallback);
			requestCallback = Callbacks.AsOnGameThreadCallback<ConnectionRequest>(requestCallback);
			this.mManager.StartAdvertising(name, appIdentifiers.Select(new Func<string, NativeAppIdentifier>(NativeAppIdentifier.FromString)).ToList<NativeAppIdentifier>(), NativeNearbyConnectionsClient.ToTimeoutMillis(advertisingDuration), delegate(long localClientId, NativeStartAdvertisingResult result)
			{
				resultCallback(result.AsResult());
			}, delegate(long localClientId, NativeConnectionRequest request)
			{
				requestCallback(request.AsRequest());
			});
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00048F80 File Offset: 0x00047180
		private static long ToTimeoutMillis(TimeSpan? span)
		{
			return (span == null) ? 0L : PInvokeUtilities.ToMilliseconds(span.Value);
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00048FA4 File Offset: 0x000471A4
		public void StopAdvertising()
		{
			this.mManager.StopAdvertising();
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x00048FB4 File Offset: 0x000471B4
		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
			Misc.CheckNotNull<byte[]>(payload, "payload");
			Misc.CheckNotNull<Action<ConnectionResponse>>(responseCallback, "responseCallback");
			Misc.CheckNotNull<IMessageListener>(listener, "listener");
			responseCallback = Callbacks.AsOnGameThreadCallback<ConnectionResponse>(responseCallback);
			using (NativeMessageListenerHelper nativeMessageListenerHelper = NativeNearbyConnectionsClient.ToMessageListener(listener))
			{
				this.mManager.SendConnectionRequest(name, remoteEndpointId, payload, delegate(long localClientId, NativeConnectionResponse response)
				{
					responseCallback(response.AsResponse(localClientId));
				}, nativeMessageListenerHelper);
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00049068 File Offset: 0x00047268
		private static NativeMessageListenerHelper ToMessageListener(IMessageListener listener)
		{
			listener = new NativeNearbyConnectionsClient.OnGameThreadMessageListener(listener);
			NativeMessageListenerHelper nativeMessageListenerHelper = new NativeMessageListenerHelper();
			nativeMessageListenerHelper.SetOnMessageReceivedCallback(delegate(long localClientId, string endpointId, byte[] data, bool isReliable)
			{
				listener.OnMessageReceived(endpointId, data, isReliable);
			});
			nativeMessageListenerHelper.SetOnDisconnectedCallback(delegate(long localClientId, string endpointId)
			{
				listener.OnRemoteEndpointDisconnected(endpointId);
			});
			return nativeMessageListenerHelper;
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x000490C0 File Offset: 0x000472C0
		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
			Misc.CheckNotNull<byte[]>(payload, "payload");
			Misc.CheckNotNull<IMessageListener>(listener, "listener");
			Logger.d("Calling AcceptConncectionRequest");
			this.mManager.AcceptConnectionRequest(remoteEndpointId, payload, NativeNearbyConnectionsClient.ToMessageListener(listener));
			Logger.d("Called!");
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00049118 File Offset: 0x00047318
		public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
		{
			Misc.CheckNotNull<string>(serviceId, "serviceId");
			Misc.CheckNotNull<IDiscoveryListener>(listener, "listener");
			using (NativeEndpointDiscoveryListenerHelper nativeEndpointDiscoveryListenerHelper = NativeNearbyConnectionsClient.ToDiscoveryListener(listener))
			{
				this.mManager.StartDiscovery(serviceId, NativeNearbyConnectionsClient.ToTimeoutMillis(advertisingTimeout), nativeEndpointDiscoveryListenerHelper);
			}
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00049188 File Offset: 0x00047388
		private static NativeEndpointDiscoveryListenerHelper ToDiscoveryListener(IDiscoveryListener listener)
		{
			listener = new NativeNearbyConnectionsClient.OnGameThreadDiscoveryListener(listener);
			NativeEndpointDiscoveryListenerHelper nativeEndpointDiscoveryListenerHelper = new NativeEndpointDiscoveryListenerHelper();
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointFound(delegate(long localClientId, NativeEndpointDetails endpoint)
			{
				listener.OnEndpointFound(endpoint.ToDetails());
			});
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointLostCallback(delegate(long localClientId, string lostEndpointId)
			{
				listener.OnEndpointLost(lostEndpointId);
			});
			return nativeEndpointDiscoveryListenerHelper;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x000491E0 File Offset: 0x000473E0
		public void StopDiscovery(string serviceId)
		{
			Misc.CheckNotNull<string>(serviceId, "serviceId");
			this.mManager.StopDiscovery(serviceId);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x000491FC File Offset: 0x000473FC
		public void RejectConnectionRequest(string requestingEndpointId)
		{
			Misc.CheckNotNull<string>(requestingEndpointId, "requestingEndpointId");
			this.mManager.RejectConnectionRequest(requestingEndpointId);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00049218 File Offset: 0x00047418
		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			this.mManager.DisconnectFromEndpoint(remoteEndpointId);
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00049228 File Offset: 0x00047428
		public void StopAllConnections()
		{
			this.mManager.StopAllConnections();
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00049238 File Offset: 0x00047438
		public string LocalEndpointId()
		{
			return this.mManager.LocalEndpointId();
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00049248 File Offset: 0x00047448
		public string LocalDeviceId()
		{
			return this.mManager.LocalDeviceId();
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x00049258 File Offset: 0x00047458
		public string GetAppBundleId()
		{
			return this.mManager.AppBundleId;
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00049268 File Offset: 0x00047468
		public string GetServiceId()
		{
			return NearbyConnectionsManager.ServiceId;
		}

		// Token: 0x04000BB8 RID: 3000
		private readonly NearbyConnectionsManager mManager;

		// Token: 0x0200021E RID: 542
		protected class OnGameThreadMessageListener : IMessageListener
		{
			// Token: 0x060010F1 RID: 4337 RVA: 0x00049270 File Offset: 0x00047470
			public OnGameThreadMessageListener(IMessageListener listener)
			{
				this.mListener = Misc.CheckNotNull<IMessageListener>(listener);
			}

			// Token: 0x060010F2 RID: 4338 RVA: 0x00049284 File Offset: 0x00047484
			public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnMessageReceived(remoteEndpointId, data, isReliableMessage);
				});
			}

			// Token: 0x060010F3 RID: 4339 RVA: 0x000492C4 File Offset: 0x000474C4
			public void OnRemoteEndpointDisconnected(string remoteEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnRemoteEndpointDisconnected(remoteEndpointId);
				});
			}

			// Token: 0x04000BB9 RID: 3001
			private readonly IMessageListener mListener;
		}

		// Token: 0x0200021F RID: 543
		protected class OnGameThreadDiscoveryListener : IDiscoveryListener
		{
			// Token: 0x060010F4 RID: 4340 RVA: 0x000492F8 File Offset: 0x000474F8
			public OnGameThreadDiscoveryListener(IDiscoveryListener listener)
			{
				this.mListener = Misc.CheckNotNull<IDiscoveryListener>(listener);
			}

			// Token: 0x060010F5 RID: 4341 RVA: 0x0004930C File Offset: 0x0004750C
			public void OnEndpointFound(EndpointDetails discoveredEndpoint)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnEndpointFound(discoveredEndpoint);
				});
			}

			// Token: 0x060010F6 RID: 4342 RVA: 0x00049340 File Offset: 0x00047540
			public void OnEndpointLost(string lostEndpointId)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					this.mListener.OnEndpointLost(lostEndpointId);
				});
			}

			// Token: 0x04000BBA RID: 3002
			private readonly IDiscoveryListener mListener;
		}
	}
}
