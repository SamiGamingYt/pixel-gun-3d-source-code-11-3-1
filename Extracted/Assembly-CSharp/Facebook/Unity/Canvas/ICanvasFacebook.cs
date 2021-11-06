using System;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000C0 RID: 192
	internal interface ICanvasFacebook : IFacebook
	{
		// Token: 0x060005AF RID: 1455
		void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback);
	}
}
