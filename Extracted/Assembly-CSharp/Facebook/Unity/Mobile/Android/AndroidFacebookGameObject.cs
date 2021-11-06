using System;
using UnityEngine;

namespace Facebook.Unity.Mobile.Android
{
	// Token: 0x020000DA RID: 218
	internal class AndroidFacebookGameObject : MobileFacebookGameObject
	{
		// Token: 0x060006AC RID: 1708 RVA: 0x0002DCCC File Offset: 0x0002BECC
		protected override void OnAwake()
		{
			AndroidJNIHelper.debug = Debug.isDebugBuild;
		}
	}
}
