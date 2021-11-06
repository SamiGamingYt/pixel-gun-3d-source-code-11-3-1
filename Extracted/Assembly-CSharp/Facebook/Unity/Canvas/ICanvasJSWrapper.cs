using System;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000C3 RID: 195
	internal interface ICanvasJSWrapper
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060005B4 RID: 1460
		string IntegrationMethodJs { get; }

		// Token: 0x060005B5 RID: 1461
		string GetSDKVersion();

		// Token: 0x060005B6 RID: 1462
		void ExternalCall(string functionName, params object[] args);

		// Token: 0x060005B7 RID: 1463
		void DisableFullScreen();

		// Token: 0x060005B8 RID: 1464
		void ExternalEval(string script);
	}
}
