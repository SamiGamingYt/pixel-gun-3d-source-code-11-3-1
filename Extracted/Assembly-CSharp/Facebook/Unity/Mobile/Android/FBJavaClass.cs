using System;
using UnityEngine;

namespace Facebook.Unity.Mobile.Android
{
	// Token: 0x020000DC RID: 220
	internal class FBJavaClass : IAndroidJavaClass
	{
		// Token: 0x060006B0 RID: 1712 RVA: 0x0002DD24 File Offset: 0x0002BF24
		public T CallStatic<T>(string methodName)
		{
			return this.facebookJavaClass.CallStatic<T>(methodName, new object[0]);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002DD38 File Offset: 0x0002BF38
		public void CallStatic(string methodName, params object[] args)
		{
			this.facebookJavaClass.CallStatic(methodName, args);
		}

		// Token: 0x0400064B RID: 1611
		private const string FacebookJavaClassName = "com.facebook.unity.FB";

		// Token: 0x0400064C RID: 1612
		private AndroidJavaClass facebookJavaClass = new AndroidJavaClass("com.facebook.unity.FB");
	}
}
