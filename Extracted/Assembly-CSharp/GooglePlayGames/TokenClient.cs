using System;
using GooglePlayGames.BasicApi;

namespace GooglePlayGames
{
	// Token: 0x02000288 RID: 648
	internal interface TokenClient
	{
		// Token: 0x060014C0 RID: 5312
		string GetEmail();

		// Token: 0x060014C1 RID: 5313
		void GetEmail(Action<CommonStatusCodes, string> callback);

		// Token: 0x060014C2 RID: 5314
		string GetAccessToken();

		// Token: 0x060014C3 RID: 5315
		void GetIdToken(string serverClientId, Action<string> idTokenCallback);

		// Token: 0x060014C4 RID: 5316
		void SetRationale(string rationale);
	}
}
