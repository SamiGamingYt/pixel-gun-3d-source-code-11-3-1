using System;
using Com.Google.Android.Gms.Common.Api;
using Google.Developers;

namespace GooglePlayGames.Android
{
	// Token: 0x020001AB RID: 427
	internal class TokenResult : JavaObjWrapper, Result
	{
		// Token: 0x06000DDB RID: 3547 RVA: 0x000456F0 File Offset: 0x000438F0
		public TokenResult(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x000456FC File Offset: 0x000438FC
		public Status getStatus()
		{
			IntPtr ptr = base.InvokeCall<IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
			return new Status(ptr);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00045728 File Offset: 0x00043928
		public int getStatusCode()
		{
			return base.InvokeCall<int>("getStatusCode", "()I", new object[0]);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00045740 File Offset: 0x00043940
		public string getAccessToken()
		{
			return base.InvokeCall<string>("getAccessToken", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00045758 File Offset: 0x00043958
		public string getEmail()
		{
			return base.InvokeCall<string>("getEmail", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00045770 File Offset: 0x00043970
		public string getIdToken()
		{
			return base.InvokeCall<string>("getIdToken", "()Ljava/lang/String;", new object[0]);
		}
	}
}
