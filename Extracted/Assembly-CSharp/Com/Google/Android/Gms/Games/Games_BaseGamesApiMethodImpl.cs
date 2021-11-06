using System;
using Com.Google.Android.Gms.Common.Api;
using Google.Developers;

namespace Com.Google.Android.Gms.Games
{
	// Token: 0x020001B7 RID: 439
	public class Games_BaseGamesApiMethodImpl<R> : JavaObjWrapper where R : Result
	{
		// Token: 0x06000E71 RID: 3697 RVA: 0x00046CBC File Offset: 0x00044EBC
		public Games_BaseGamesApiMethodImpl(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00046CC8 File Offset: 0x00044EC8
		public Games_BaseGamesApiMethodImpl(GoogleApiClient arg_GoogleApiClient_1)
		{
			base.CreateInstance("com/google/android/gms/games/Games$BaseGamesApiMethodImpl", new object[]
			{
				arg_GoogleApiClient_1
			});
		}

		// Token: 0x04000AAC RID: 2732
		private const string CLASS_NAME = "com/google/android/gms/games/Games$BaseGamesApiMethodImpl";
	}
}
