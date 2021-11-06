using System;
using Google.Developers;

namespace Com.Google.Android.Gms.Common.Api
{
	// Token: 0x020001B0 RID: 432
	public class PendingResult<R> : JavaObjWrapper where R : Result
	{
		// Token: 0x06000E0D RID: 3597 RVA: 0x00046270 File Offset: 0x00044470
		public PendingResult(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0004627C File Offset: 0x0004447C
		public PendingResult() : base("com.google.android.gms.common.api.PendingResult")
		{
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0004628C File Offset: 0x0004448C
		public R await(long arg_long_1, object arg_object_2)
		{
			return base.InvokeCall<R>("await", "(JLjava/util/concurrent/TimeUnit;)Lcom/google/android/gms/common/api/Result;", new object[]
			{
				arg_long_1,
				arg_object_2
			});
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x000462B4 File Offset: 0x000444B4
		public R await()
		{
			return base.InvokeCall<R>("await", "()Lcom/google/android/gms/common/api/Result;", new object[0]);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x000462CC File Offset: 0x000444CC
		public bool isCanceled()
		{
			return base.InvokeCall<bool>("isCanceled", "()Z", new object[0]);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000462E4 File Offset: 0x000444E4
		public void cancel()
		{
			base.InvokeCallVoid("cancel", "()V", new object[0]);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000462FC File Offset: 0x000444FC
		public void setResultCallback(ResultCallback<R> arg_ResultCallback_1)
		{
			base.InvokeCallVoid("setResultCallback", "(Lcom/google/android/gms/common/api/ResultCallback;)V", new object[]
			{
				arg_ResultCallback_1
			});
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00046318 File Offset: 0x00044518
		public void setResultCallback(ResultCallback<R> arg_ResultCallback_1, long arg_long_2, object arg_object_3)
		{
			base.InvokeCallVoid("setResultCallback", "(Lcom/google/android/gms/common/api/ResultCallback;JLjava/util/concurrent/TimeUnit;)V", new object[]
			{
				arg_ResultCallback_1,
				arg_long_2,
				arg_object_3
			});
		}

		// Token: 0x04000AA7 RID: 2727
		private const string CLASS_NAME = "com/google/android/gms/common/api/PendingResult";
	}
}
