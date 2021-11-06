using System;
using System.Reflection;
using GooglePlayGames.OurUtils;
using UnityEngine;

namespace GooglePlayGames.Native
{
	// Token: 0x02000218 RID: 536
	internal static class JavaUtils
	{
		// Token: 0x0600109A RID: 4250 RVA: 0x00047158 File Offset: 0x00045358
		internal static AndroidJavaObject JavaObjectFromPointer(IntPtr jobject)
		{
			if (jobject == IntPtr.Zero)
			{
				return null;
			}
			return (AndroidJavaObject)JavaUtils.IntPtrConstructor.Invoke(new object[]
			{
				jobject
			});
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00047198 File Offset: 0x00045398
		internal static AndroidJavaObject NullSafeCall(this AndroidJavaObject target, string methodName, params object[] args)
		{
			AndroidJavaObject result;
			try
			{
				result = target.Call<AndroidJavaObject>(methodName, args);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("null"))
				{
					result = null;
				}
				else
				{
					GooglePlayGames.OurUtils.Logger.w("CallObjectMethod exception: " + ex);
					result = null;
				}
			}
			return result;
		}

		// Token: 0x04000B96 RID: 2966
		private static ConstructorInfo IntPtrConstructor = typeof(AndroidJavaObject).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[]
		{
			typeof(IntPtr)
		}, null);
	}
}
