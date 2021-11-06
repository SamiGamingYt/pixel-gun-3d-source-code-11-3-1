using System;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Facebook.Unity.Canvas;
using Facebook.Unity.Editor.Dialogs;
using Facebook.Unity.Mobile;

namespace Facebook.Unity.Editor
{
	// Token: 0x020000EC RID: 236
	internal class EditorFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler, IMobileFacebook, IMobileFacebookCallbackHandler, IMobileFacebookImplementation
	{
		// Token: 0x06000733 RID: 1843 RVA: 0x0002E784 File Offset: 0x0002C984
		public EditorFacebook() : base(new CallbackManager())
		{
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0002E794 File Offset: 0x0002C994
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0002E79C File Offset: 0x0002C99C
		public override bool LimitEventUsage { get; set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0002E7A8 File Offset: 0x0002C9A8
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
		public ShareDialogMode ShareDialogMode { get; set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public override string SDKName
		{
			get
			{
				return "FBUnityEditorSDK";
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0002E7C4 File Offset: 0x0002C9C4
		public override string SDKVersion
		{
			get
			{
				return FacebookSdkVersion.Build;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0002E7CC File Offset: 0x0002C9CC
		private IFacebookCallbackHandler EditorGameObject
		{
			get
			{
				return ComponentFactory.GetComponent<EditorFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002E7D4 File Offset: 0x0002C9D4
		public override void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			FacebookLogger.Warn("You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
			base.Init(hideUnityDelegate, onInitComplete);
			this.EditorGameObject.OnInitComplete(string.Empty);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002E804 File Offset: 0x0002CA04
		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.LogInWithPublishPermissions(permissions, callback);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002E810 File Offset: 0x0002CA10
		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			MockLoginDialog component = ComponentFactory.GetComponent<MockLoginDialog>(ComponentFactory.IfNotExist.AddNew);
			component.Callback = new EditorFacebookMockDialog.OnComplete(this.EditorGameObject.OnLoginComplete);
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002E850 File Offset: 0x0002CA50
		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			this.ShowEmptyMockDialog<IAppRequestResult>(new EditorFacebookMockDialog.OnComplete(this.OnAppRequestsComplete), callback, "Mock App Request");
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002E86C File Offset: 0x0002CA6C
		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			this.ShowMockShareDialog("ShareLink", callback);
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0002E87C File Offset: 0x0002CA7C
		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			this.ShowMockShareDialog("FeedShare", callback);
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0002E88C File Offset: 0x0002CA8C
		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			this.ShowEmptyMockDialog<IGroupCreateResult>(new EditorFacebookMockDialog.OnComplete(this.OnGroupCreateComplete), callback, "Mock Group Create");
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0002E8A8 File Offset: 0x0002CAA8
		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			this.ShowEmptyMockDialog<IGroupJoinResult>(new EditorFacebookMockDialog.OnComplete(this.OnGroupJoinComplete), callback, "Mock Group Join");
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0002E8C4 File Offset: 0x0002CAC4
		public override void ActivateApp(string appId)
		{
			FacebookLogger.Info("This only needs to be called for iOS or Android.");
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0002E8D0 File Offset: 0x0002CAD0
		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
			this.OnGetAppLinkComplete(Json.Serialize(dictionary));
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0002E918 File Offset: 0x0002CB18
		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002E924 File Offset: 0x0002CB24
		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002E930 File Offset: 0x0002CB30
		public void AppInvite(Uri appLinkUrl, Uri previewImageUrl, FacebookDelegate<IAppInviteResult> callback)
		{
			this.ShowEmptyMockDialog<IAppInviteResult>(new EditorFacebookMockDialog.OnComplete(this.OnAppInviteComplete), callback, "Mock App Invite");
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002E94C File Offset: 0x0002CB4C
		public void FetchDeferredAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["ref"] = "mock ref";
			dictionary["extras"] = new Dictionary<string, object>
			{
				{
					"mock extra key",
					"mock extra value"
				}
			};
			dictionary["target_url"] = "mocktargeturl://mocktarget.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate<IAppLinkResult>(callback);
			this.OnFetchDeferredAppLinkComplete(Json.Serialize(dictionary));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002E9D4 File Offset: 0x0002CBD4
		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			this.ShowEmptyMockDialog<IPayResult>(new EditorFacebookMockDialog.OnComplete(this.OnPayComplete), callback, "Mock Pay Dialog");
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
		public void RefreshCurrentAccessToken(FacebookDelegate<IAccessTokenRefreshResult> callback)
		{
			if (callback == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"callback_id",
					base.CallbackManager.AddFacebookDelegate<IAccessTokenRefreshResult>(callback)
				}
			};
			if (AccessToken.CurrentAccessToken == null)
			{
				dictionary["error"] = "No current access token";
			}
			else
			{
				IDictionary<string, object> source = (IDictionary<string, object>)Json.Deserialize(AccessToken.CurrentAccessToken.ToJson());
				dictionary.AddAllKVPFrom(source);
			}
			this.OnRefreshCurrentAccessTokenComplete(dictionary.ToJson());
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0002EA6C File Offset: 0x0002CC6C
		public override void OnAppRequestsComplete(string message)
		{
			AppRequestResult result = new AppRequestResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0002EA8C File Offset: 0x0002CC8C
		public override void OnGetAppLinkComplete(string message)
		{
			AppLinkResult result = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0002EAAC File Offset: 0x0002CCAC
		public override void OnGroupCreateComplete(string message)
		{
			GroupCreateResult result = new GroupCreateResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002EACC File Offset: 0x0002CCCC
		public override void OnGroupJoinComplete(string message)
		{
			GroupJoinResult result = new GroupJoinResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0002EAEC File Offset: 0x0002CCEC
		public override void OnLoginComplete(string message)
		{
			LoginResult result = new LoginResult(message);
			base.OnAuthResponse(result);
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002EB08 File Offset: 0x0002CD08
		public override void OnShareLinkComplete(string message)
		{
			ShareResult result = new ShareResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0002EB28 File Offset: 0x0002CD28
		public void OnAppInviteComplete(string message)
		{
			AppInviteResult result = new AppInviteResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0002EB48 File Offset: 0x0002CD48
		public void OnFetchDeferredAppLinkComplete(string message)
		{
			AppLinkResult result = new AppLinkResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002EB68 File Offset: 0x0002CD68
		public void OnPayComplete(string message)
		{
			PayResult result = new PayResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0002EB88 File Offset: 0x0002CD88
		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			AccessTokenRefreshResult result = new AccessTokenRefreshResult(message);
			base.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0002EBA8 File Offset: 0x0002CDA8
		public void OnFacebookAuthResponseChange(string message)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0002EBB0 File Offset: 0x0002CDB0
		public void OnUrlResponse(string message)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002EBB8 File Offset: 0x0002CDB8
		private void ShowEmptyMockDialog<T>(EditorFacebookMockDialog.OnComplete callback, FacebookDelegate<T> userCallback, string title) where T : IResult
		{
			EmptyMockDialog component = ComponentFactory.GetComponent<EmptyMockDialog>(ComponentFactory.IfNotExist.AddNew);
			component.Callback = callback;
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<T>(userCallback);
			component.EmptyDialogTitle = title;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0002EBEC File Offset: 0x0002CDEC
		private void ShowMockShareDialog(string subTitle, FacebookDelegate<IShareResult> userCallback)
		{
			MockShareDialog component = ComponentFactory.GetComponent<MockShareDialog>(ComponentFactory.IfNotExist.AddNew);
			component.SubTitle = subTitle;
			component.Callback = new EditorFacebookMockDialog.OnComplete(this.EditorGameObject.OnShareLinkComplete);
			component.CallbackID = base.CallbackManager.AddFacebookDelegate<IShareResult>(userCallback);
		}

		// Token: 0x04000661 RID: 1633
		private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

		// Token: 0x04000662 RID: 1634
		private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";
	}
}
