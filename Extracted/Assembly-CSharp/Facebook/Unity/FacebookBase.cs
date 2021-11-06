using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000CD RID: 205
	internal abstract class FacebookBase : IFacebook, IFacebookCallbackHandler, IFacebookImplementation
	{
		// Token: 0x06000606 RID: 1542 RVA: 0x0002CC98 File Offset: 0x0002AE98
		protected FacebookBase(CallbackManager callbackManager)
		{
			this.CallbackManager = callbackManager;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000607 RID: 1543
		// (set) Token: 0x06000608 RID: 1544
		public abstract bool LimitEventUsage { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000609 RID: 1545
		public abstract string SDKName { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600060A RID: 1546
		public abstract string SDKVersion { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0002CCA8 File Offset: 0x0002AEA8
		public virtual string SDKUserAgent
		{
			get
			{
				return Utilities.GetUserAgent(this.SDKName, this.SDKVersion);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0002CCBC File Offset: 0x0002AEBC
		public bool LoggedIn
		{
			get
			{
				return AccessToken.CurrentAccessToken != null;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0002CCCC File Offset: 0x0002AECC
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x0002CCD4 File Offset: 0x0002AED4
		public bool Initialized { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0002CCE0 File Offset: 0x0002AEE0
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x0002CCE8 File Offset: 0x0002AEE8
		private protected CallbackManager CallbackManager { protected get; private set; }

		// Token: 0x06000611 RID: 1553 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
		public virtual void Init(HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			this.onHideUnityDelegate = hideUnityDelegate;
			this.onInitCompleteDelegate = onInitComplete;
		}

		// Token: 0x06000612 RID: 1554
		public abstract void LogInWithPublishPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);

		// Token: 0x06000613 RID: 1555
		public abstract void LogInWithReadPermissions(IEnumerable<string> scope, FacebookDelegate<ILoginResult> callback);

		// Token: 0x06000614 RID: 1556 RVA: 0x0002CD04 File Offset: 0x0002AF04
		public virtual void LogOut()
		{
			AccessToken.CurrentAccessToken = null;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002CD0C File Offset: 0x0002AF0C
		public void AppRequest(string message, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			this.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		// Token: 0x06000616 RID: 1558
		public abstract void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback);

		// Token: 0x06000617 RID: 1559
		public abstract void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback);

		// Token: 0x06000618 RID: 1560
		public abstract void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback);

		// Token: 0x06000619 RID: 1561 RVA: 0x0002CD38 File Offset: 0x0002AF38
		public void API(string query, HttpMethod method, IDictionary<string, string> formData, FacebookDelegate<IGraphResult> callback)
		{
			IDictionary<string, string> dictionary2;
			if (formData != null)
			{
				IDictionary<string, string> dictionary = this.CopyByValue(formData);
				dictionary2 = dictionary;
			}
			else
			{
				dictionary2 = new Dictionary<string, string>();
			}
			IDictionary<string, string> dictionary3 = dictionary2;
			if (!dictionary3.ContainsKey("access_token") && !query.Contains("access_token="))
			{
				dictionary3["access_token"] = ((!FB.IsLoggedIn) ? string.Empty : AccessToken.CurrentAccessToken.TokenString);
			}
			AsyncRequestString.Request(this.GetGraphUrl(query), method, dictionary3, callback);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0002CDB8 File Offset: 0x0002AFB8
		public void API(string query, HttpMethod method, WWWForm formData, FacebookDelegate<IGraphResult> callback)
		{
			if (formData == null)
			{
				formData = new WWWForm();
			}
			string value = (AccessToken.CurrentAccessToken == null) ? string.Empty : AccessToken.CurrentAccessToken.TokenString;
			formData.AddField("access_token", value);
			AsyncRequestString.Request(this.GetGraphUrl(query), method, formData, callback);
		}

		// Token: 0x0600061B RID: 1563
		public abstract void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback);

		// Token: 0x0600061C RID: 1564
		public abstract void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback);

		// Token: 0x0600061D RID: 1565
		public abstract void ActivateApp(string appId = null);

		// Token: 0x0600061E RID: 1566
		public abstract void GetAppLink(FacebookDelegate<IAppLinkResult> callback);

		// Token: 0x0600061F RID: 1567
		public abstract void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters);

		// Token: 0x06000620 RID: 1568
		public abstract void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters);

		// Token: 0x06000621 RID: 1569 RVA: 0x0002CE10 File Offset: 0x0002B010
		public virtual void OnHideUnity(bool isGameShown)
		{
			if (this.onHideUnityDelegate != null)
			{
				this.onHideUnityDelegate(isGameShown);
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0002CE2C File Offset: 0x0002B02C
		public virtual void OnInitComplete(string message)
		{
			this.Initialized = true;
			this.OnLoginComplete(message);
			if (this.onInitCompleteDelegate != null)
			{
				this.onInitCompleteDelegate();
			}
		}

		// Token: 0x06000623 RID: 1571
		public abstract void OnLoginComplete(string message);

		// Token: 0x06000624 RID: 1572 RVA: 0x0002CE60 File Offset: 0x0002B060
		public void OnLogoutComplete(string message)
		{
			AccessToken.CurrentAccessToken = null;
		}

		// Token: 0x06000625 RID: 1573
		public abstract void OnGetAppLinkComplete(string message);

		// Token: 0x06000626 RID: 1574
		public abstract void OnGroupCreateComplete(string message);

		// Token: 0x06000627 RID: 1575
		public abstract void OnGroupJoinComplete(string message);

		// Token: 0x06000628 RID: 1576
		public abstract void OnAppRequestsComplete(string message);

		// Token: 0x06000629 RID: 1577
		public abstract void OnShareLinkComplete(string message);

		// Token: 0x0600062A RID: 1578 RVA: 0x0002CE68 File Offset: 0x0002B068
		protected void ValidateAppRequestArgs(string message, OGActionType? actionType, string objectId, IEnumerable<string> to = null, IEnumerable<object> filters = null, IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", FacebookDelegate<IAppRequestResult> callback = null)
		{
			if (string.IsNullOrEmpty(message))
			{
				throw new ArgumentNullException("message", "message cannot be null or empty!");
			}
			if (!string.IsNullOrEmpty(objectId) && !(actionType == OGActionType.ASKFOR) && !(actionType == OGActionType.SEND))
			{
				throw new ArgumentNullException("objectId", "Object ID must be set if and only if action type is SEND or ASKFOR");
			}
			if (actionType == null && !string.IsNullOrEmpty(objectId))
			{
				throw new ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0002CF08 File Offset: 0x0002B108
		protected void OnAuthResponse(LoginResult result)
		{
			if (result.AccessToken != null)
			{
				AccessToken.CurrentAccessToken = result.AccessToken;
			}
			this.CallbackManager.OnFacebookResponse(result);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002CF38 File Offset: 0x0002B138
		private IDictionary<string, string> CopyByValue(IDictionary<string, string> data)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(data.Count);
			foreach (KeyValuePair<string, string> keyValuePair in data)
			{
				dictionary[keyValuePair.Key] = ((keyValuePair.Value == null) ? null : new string(keyValuePair.Value.ToCharArray()));
			}
			return dictionary;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002CFCC File Offset: 0x0002B1CC
		private Uri GetGraphUrl(string query)
		{
			if (!string.IsNullOrEmpty(query) && query.StartsWith("/"))
			{
				query = query.Substring(1);
			}
			return new Uri(Constants.GraphUrl, query);
		}

		// Token: 0x04000627 RID: 1575
		private InitDelegate onInitCompleteDelegate;

		// Token: 0x04000628 RID: 1576
		private HideUnityDelegate onHideUnityDelegate;
	}
}
