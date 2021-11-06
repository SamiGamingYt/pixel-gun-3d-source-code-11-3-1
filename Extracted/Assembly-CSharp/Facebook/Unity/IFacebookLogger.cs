using System;

namespace Facebook.Unity
{
	// Token: 0x0200010F RID: 271
	internal interface IFacebookLogger
	{
		// Token: 0x060007E2 RID: 2018
		void Log(string msg);

		// Token: 0x060007E3 RID: 2019
		void Info(string msg);

		// Token: 0x060007E4 RID: 2020
		void Warn(string msg);

		// Token: 0x060007E5 RID: 2021
		void Error(string msg);
	}
}
