using System;

namespace Facebook.Unity.Mobile.Android
{
	// Token: 0x020000DD RID: 221
	internal interface IAndroidJavaClass
	{
		// Token: 0x060006B2 RID: 1714
		T CallStatic<T>(string methodName);

		// Token: 0x060006B3 RID: 1715
		void CallStatic(string methodName, params object[] args);
	}
}
