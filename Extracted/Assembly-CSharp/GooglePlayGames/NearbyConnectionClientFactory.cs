using System;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames
{
	// Token: 0x02000286 RID: 646
	public static class NearbyConnectionClientFactory
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x000520CC File Offset: 0x000502CC
		public static void Create(Action<INearbyConnectionClient> callback)
		{
			if (Application.isEditor)
			{
				GooglePlayGames.OurUtils.Logger.d("Creating INearbyConnection in editor, using DummyClient.");
				callback(new DummyNearbyConnectionClient());
			}
			GooglePlayGames.OurUtils.Logger.d("Creating real INearbyConnectionClient");
			NativeNearbyConnectionClientFactory.Create(callback);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00052100 File Offset: 0x00050300
		private static InitializationStatus ToStatus(NearbyConnectionsStatus.InitializationStatus status)
		{
			switch (status + 4)
			{
			case (NearbyConnectionsStatus.InitializationStatus)0:
				return InitializationStatus.VersionUpdateRequired;
			case (NearbyConnectionsStatus.InitializationStatus)2:
				return InitializationStatus.InternalError;
			case (NearbyConnectionsStatus.InitializationStatus)5:
				return InitializationStatus.Success;
			}
			GooglePlayGames.OurUtils.Logger.w("Unknown initialization status: " + status);
			return InitializationStatus.InternalError;
		}
	}
}
