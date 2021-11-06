using System;

namespace Facebook.Unity.Mobile
{
	// Token: 0x020000E9 RID: 233
	internal abstract class MobileFacebookGameObject : FacebookGameObject, IFacebookCallbackHandler, IMobileFacebookCallbackHandler
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0002E744 File Offset: 0x0002C944
		private IMobileFacebookImplementation MobileFacebook
		{
			get
			{
				return (IMobileFacebookImplementation)base.Facebook;
			}
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0002E754 File Offset: 0x0002C954
		public void OnAppInviteComplete(string message)
		{
			this.MobileFacebook.OnAppInviteComplete(message);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0002E764 File Offset: 0x0002C964
		public void OnFetchDeferredAppLinkComplete(string message)
		{
			this.MobileFacebook.OnFetchDeferredAppLinkComplete(message);
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0002E774 File Offset: 0x0002C974
		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			this.MobileFacebook.OnRefreshCurrentAccessTokenComplete(message);
		}
	}
}
