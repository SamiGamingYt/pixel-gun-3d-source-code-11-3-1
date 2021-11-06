using System;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000CE RID: 206
	internal abstract class FacebookGameObject : MonoBehaviour, IFacebookCallbackHandler
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0002D010 File Offset: 0x0002B210
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x0002D018 File Offset: 0x0002B218
		public IFacebookImplementation Facebook { get; set; }

		// Token: 0x06000631 RID: 1585 RVA: 0x0002D024 File Offset: 0x0002B224
		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(this);
			AccessToken.CurrentAccessToken = null;
			this.OnAwake();
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0002D038 File Offset: 0x0002B238
		public void OnInitComplete(string message)
		{
			this.Facebook.OnInitComplete(message);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002D048 File Offset: 0x0002B248
		public void OnLoginComplete(string message)
		{
			this.Facebook.OnLoginComplete(message);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002D058 File Offset: 0x0002B258
		public void OnLogoutComplete(string message)
		{
			this.Facebook.OnLogoutComplete(message);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002D068 File Offset: 0x0002B268
		public void OnGetAppLinkComplete(string message)
		{
			this.Facebook.OnGetAppLinkComplete(message);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0002D078 File Offset: 0x0002B278
		public void OnGroupCreateComplete(string message)
		{
			this.Facebook.OnGroupCreateComplete(message);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002D088 File Offset: 0x0002B288
		public void OnGroupJoinComplete(string message)
		{
			this.Facebook.OnGroupJoinComplete(message);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002D098 File Offset: 0x0002B298
		public void OnAppRequestsComplete(string message)
		{
			this.Facebook.OnAppRequestsComplete(message);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002D0A8 File Offset: 0x0002B2A8
		public void OnShareLinkComplete(string message)
		{
			this.Facebook.OnShareLinkComplete(message);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
		protected virtual void OnAwake()
		{
		}
	}
}
