using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Google.Developers;
using UnityEngine;

namespace Com.Google.Android.Gms.Common.Api
{
	// Token: 0x020001B3 RID: 435
	public abstract class ResultCallbackProxy<R> : JavaInterfaceProxy, ResultCallback<R> where R : Result
	{
		// Token: 0x06000E17 RID: 3607 RVA: 0x00046344 File Offset: 0x00044544
		public ResultCallbackProxy() : base("com/google/android/gms/common/api/ResultCallback")
		{
		}

		// Token: 0x06000E18 RID: 3608
		public abstract void OnResult(R arg_Result_1);

		// Token: 0x06000E19 RID: 3609 RVA: 0x00046354 File Offset: 0x00044554
		public void onResult(R arg_Result_1)
		{
			this.OnResult(arg_Result_1);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00046360 File Offset: 0x00044560
		public void onResult(AndroidJavaObject arg_Result_1)
		{
			IntPtr rawObject = arg_Result_1.GetRawObject();
			ConstructorInfo constructor = typeof(R).GetConstructor(new Type[]
			{
				rawObject.GetType()
			});
			R r;
			if (constructor != null)
			{
				r = (R)((object)constructor.Invoke(new object[]
				{
					rawObject
				}));
			}
			else
			{
				ConstructorInfo constructor2 = typeof(R).GetConstructor(new Type[0]);
				r = (R)((object)constructor2.Invoke(new object[0]));
				Marshal.PtrToStructure(rawObject, r);
			}
			this.OnResult(r);
		}

		// Token: 0x04000AA8 RID: 2728
		private const string CLASS_NAME = "com/google/android/gms/common/api/ResultCallback";
	}
}
