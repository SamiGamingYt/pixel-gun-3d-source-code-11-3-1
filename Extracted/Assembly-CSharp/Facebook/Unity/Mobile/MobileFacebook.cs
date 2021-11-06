using System;
using System.Collections.Generic;
using Facebook.MiniJSON;

namespace Facebook.Unity.Mobile
{
	// Token: 0x020000E8 RID: 232
	internal abstract class MobileFacebook : FacebookBase, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
	{
		// Token: 0x0600071A RID: 1818 RVA: 0x0002E564 File Offset: 0x0002C764
		protected MobileFacebook(CallbackManager callbackManager) : base(callbackManager)
		{
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0002E570 File Offset: 0x0002C770
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0002E578 File Offset: 0x0002C778
		public ShareDialogMode ShareDialogMode
		{
			get
			{
				return this.shareDialogMode;
			}
			set
			{
				this.shareDialogMode = value;
				this.SetShareDialogMode(this.shareDialogMode);
			}
		}

		// Token: 0x0600071D RID: 1821
		public abstract void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback);

		// Token: 0x0600071E RID: 1822
		public abstract void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback);

		// Token: 0x0600071F RID: 1823
		public abstract void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback);

		// Token: 0x06000720 RID: 1824 RVA: 0x0002E590 File Offset: 0x0002C790
		public override void OnLoginComplete(string message)
		{
			LoginResult result = new LoginResult(message);
			base.OnAuthResponse(result);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002E5AC File Offset: 0x0002C7AC
		public override void OnGetAppLinkComplete(string message)
		{
			AppLinkResult result = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002E5CC File Offset: 0x0002C7CC
		public override void OnGroupCreateComplete(string message)
		{
			GroupCreateResult result = new GroupCreateResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002E5EC File Offset: 0x0002C7EC
		public override void OnGroupJoinComplete(string message)
		{
			GroupJoinResult result = new GroupJoinResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0002E60C File Offset: 0x0002C80C
		public override void OnAppRequestsComplete(string message)
		{
			AppRequestResult result = new AppRequestResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0002E62C File Offset: 0x0002C82C
		public void OnAppInviteComplete(string message)
		{
			AppInviteResult result = new AppInviteResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0002E64C File Offset: 0x0002C84C
		public void OnFetchDeferredAppLinkComplete(string message)
		{
			AppLinkResult result = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0002E66C File Offset: 0x0002C86C
		public override void OnShareLinkComplete(string message)
		{
			ShareResult result = new ShareResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002E68C File Offset: 0x0002C88C
		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			AccessTokenRefreshResult accessTokenRefreshResult = new AccessTokenRefreshResult(message);
			if (accessTokenRefreshResult.AccessToken != null)
			{
				AccessToken.CurrentAccessToken = accessTokenRefreshResult.AccessToken;
			}
			base.CallbackManager.OnFacebookResponse(accessTokenRefreshResult);
		}

		// Token: 0x06000729 RID: 1833
		protected abstract void SetShareDialogMode(ShareDialogMode mode);

		// Token: 0x0600072A RID: 1834 RVA: 0x0002E6C4 File Offset: 0x0002C8C4
		private static IDictionary<string, object> DeserializeMessage(string message)
		{
			return (Dictionary<string, object>)Json.Deserialize(message);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002E6D4 File Offset: 0x0002C8D4
		private static string SerializeDictionary(IDictionary<string, object> dict)
		{
			return Json.Serialize(dict);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0002E6DC File Offset: 0x0002C8DC
		private static bool TryGetCallbackId(IDictionary<string, object> result, out string callbackId)
		{
			callbackId = null;
			object obj;
			if (result.TryGetValue("callback_id", out obj))
			{
				callbackId = (obj as string);
				return true;
			}
			return false;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0002E70C File Offset: 0x0002C90C
		private static bool TryGetError(IDictionary<string, object> result, out string errorMessage)
		{
			errorMessage = null;
			object obj;
			if (result.TryGetValue("error", out obj))
			{
				errorMessage = (obj as string);
				return true;
			}
			return false;
		}

		// Token: 0x04000656 RID: 1622
		private const string CallbackIdKey = "callback_id";

		// Token: 0x04000657 RID: 1623
		private ShareDialogMode shareDialogMode;
	}
}
