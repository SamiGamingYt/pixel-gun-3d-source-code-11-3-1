using System;
using UnityEngine;

// Token: 0x02000822 RID: 2082
public class StartUp : MonoBehaviour
{
	// Token: 0x06004BB9 RID: 19385 RVA: 0x001B3FE0 File Offset: 0x001B21E0
	private void Start()
	{
		if (Application.isEditor)
		{
			return;
		}
		AppsFlyer.setAppsFlyerKey("Cja8TmYiYqwrDDFHJykmjD");
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			AppsFlyer.setAppID("com.pixel.gun3d");
		}
		else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			AppsFlyer.setAppID("com.PixelGun.a3D");
		}
		AppsFlyer.setIsDebug(Defs.IsDeveloperBuild);
		AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks", "didReceiveConversionData", "didReceiveConversionDataWithError");
		AppsFlyer.trackAppLaunch();
	}
}
