using System;

namespace Facebook.Unity
{
	// Token: 0x020000D4 RID: 212
	internal interface IFacebookCallbackHandler
	{
		// Token: 0x06000671 RID: 1649
		void OnInitComplete(string message);

		// Token: 0x06000672 RID: 1650
		void OnLoginComplete(string message);

		// Token: 0x06000673 RID: 1651
		void OnLogoutComplete(string message);

		// Token: 0x06000674 RID: 1652
		void OnGetAppLinkComplete(string message);

		// Token: 0x06000675 RID: 1653
		void OnGroupCreateComplete(string message);

		// Token: 0x06000676 RID: 1654
		void OnGroupJoinComplete(string message);

		// Token: 0x06000677 RID: 1655
		void OnAppRequestsComplete(string message);

		// Token: 0x06000678 RID: 1656
		void OnShareLinkComplete(string message);
	}
}
