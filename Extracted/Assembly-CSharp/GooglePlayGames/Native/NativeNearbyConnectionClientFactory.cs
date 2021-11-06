using System;
using GooglePlayGames.Android;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Native
{
	// Token: 0x0200021C RID: 540
	public class NativeNearbyConnectionClientFactory
	{
		// Token: 0x060010D7 RID: 4311 RVA: 0x00048C04 File Offset: 0x00046E04
		internal static NearbyConnectionsManager GetManager()
		{
			return NativeNearbyConnectionClientFactory.sManager;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00048C10 File Offset: 0x00046E10
		public static void Create(Action<INearbyConnectionClient> callback)
		{
			if (NativeNearbyConnectionClientFactory.sManager == null)
			{
				NativeNearbyConnectionClientFactory.sCreationCallback = callback;
				NativeNearbyConnectionClientFactory.InitializeFactory();
			}
			else
			{
				callback(new NativeNearbyConnectionsClient(NativeNearbyConnectionClientFactory.GetManager()));
			}
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00048C4C File Offset: 0x00046E4C
		internal static void InitializeFactory()
		{
			PlayGamesHelperObject.CreateObject();
			NearbyConnectionsManager.ReadServiceId();
			NearbyConnectionsManagerBuilder nearbyConnectionsManagerBuilder = new NearbyConnectionsManagerBuilder();
			nearbyConnectionsManagerBuilder.SetOnInitializationFinished(new Action<NearbyConnectionsStatus.InitializationStatus>(NativeNearbyConnectionClientFactory.OnManagerInitialized));
			PlatformConfiguration configuration = new AndroidClient().CreatePlatformConfiguration();
			Debug.Log("Building manager Now");
			NativeNearbyConnectionClientFactory.sManager = nearbyConnectionsManagerBuilder.Build(configuration);
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00048CA0 File Offset: 0x00046EA0
		internal static void OnManagerInitialized(NearbyConnectionsStatus.InitializationStatus status)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Nearby Init Complete: ",
				status,
				" sManager = ",
				NativeNearbyConnectionClientFactory.sManager
			}));
			if (status == NearbyConnectionsStatus.InitializationStatus.VALID)
			{
				if (NativeNearbyConnectionClientFactory.sCreationCallback != null)
				{
					NativeNearbyConnectionClientFactory.sCreationCallback(new NativeNearbyConnectionsClient(NativeNearbyConnectionClientFactory.GetManager()));
					NativeNearbyConnectionClientFactory.sCreationCallback = null;
				}
			}
			else
			{
				Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
			}
		}

		// Token: 0x04000BB6 RID: 2998
		private static volatile NearbyConnectionsManager sManager;

		// Token: 0x04000BB7 RID: 2999
		private static Action<INearbyConnectionClient> sCreationCallback;
	}
}
