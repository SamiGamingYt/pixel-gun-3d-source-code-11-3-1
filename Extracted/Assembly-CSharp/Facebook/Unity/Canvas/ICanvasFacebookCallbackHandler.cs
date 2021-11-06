using System;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000C1 RID: 193
	internal interface ICanvasFacebookCallbackHandler : IFacebookCallbackHandler
	{
		// Token: 0x060005B0 RID: 1456
		void OnPayComplete(string message);

		// Token: 0x060005B1 RID: 1457
		void OnFacebookAuthResponseChange(string message);

		// Token: 0x060005B2 RID: 1458
		void OnUrlResponse(string message);

		// Token: 0x060005B3 RID: 1459
		void OnHideUnity(bool hide);
	}
}
