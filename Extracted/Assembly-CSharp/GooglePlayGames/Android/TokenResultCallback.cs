using System;
using Com.Google.Android.Gms.Common.Api;

namespace GooglePlayGames.Android
{
	// Token: 0x020001AC RID: 428
	internal class TokenResultCallback : ResultCallbackProxy<TokenResult>
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x00045788 File Offset: 0x00043988
		public TokenResultCallback(Action<int, string, string, string> callback)
		{
			this.callback = callback;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00045798 File Offset: 0x00043998
		public override void OnResult(TokenResult arg_Result_1)
		{
			if (this.callback != null)
			{
				this.callback(arg_Result_1.getStatusCode(), arg_Result_1.getAccessToken(), arg_Result_1.getIdToken(), arg_Result_1.getEmail());
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x000457D4 File Offset: 0x000439D4
		public string toString()
		{
			return this.ToString();
		}

		// Token: 0x04000AA3 RID: 2723
		private Action<int, string, string, string> callback;
	}
}
