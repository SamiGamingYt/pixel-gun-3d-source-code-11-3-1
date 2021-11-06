using System;
using System.Collections.Generic;

namespace Facebook.Unity
{
	// Token: 0x020000BA RID: 186
	internal class CallbackManager
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0002B7B8 File Offset: 0x000299B8
		public string AddFacebookDelegate<T>(FacebookDelegate<T> callback) where T : IResult
		{
			if (callback == null)
			{
				return null;
			}
			this.nextAsyncId++;
			this.facebookDelegates.Add(this.nextAsyncId.ToString(), callback);
			return this.nextAsyncId.ToString();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0002B800 File Offset: 0x00029A00
		public void OnFacebookResponse(IInternalResult result)
		{
			if (result == null || result.CallbackId == null)
			{
				return;
			}
			object callback;
			if (this.facebookDelegates.TryGetValue(result.CallbackId, out callback))
			{
				CallbackManager.CallCallback(callback, result);
				this.facebookDelegates.Remove(result.CallbackId);
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0002B850 File Offset: 0x00029A50
		private static void CallCallback(object callback, IResult result)
		{
			if (callback == null || result == null)
			{
				return;
			}
			if (CallbackManager.TryCallCallback<IAppRequestResult>(callback, result) || CallbackManager.TryCallCallback<IShareResult>(callback, result) || CallbackManager.TryCallCallback<IGroupCreateResult>(callback, result) || CallbackManager.TryCallCallback<IGroupJoinResult>(callback, result) || CallbackManager.TryCallCallback<IPayResult>(callback, result) || CallbackManager.TryCallCallback<IAppInviteResult>(callback, result) || CallbackManager.TryCallCallback<IAppLinkResult>(callback, result) || CallbackManager.TryCallCallback<ILoginResult>(callback, result) || CallbackManager.TryCallCallback<IAccessTokenRefreshResult>(callback, result))
			{
				return;
			}
			throw new NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0002B8F4 File Offset: 0x00029AF4
		private static bool TryCallCallback<T>(object callback, IResult result) where T : IResult
		{
			FacebookDelegate<T> facebookDelegate = callback as FacebookDelegate<T>;
			if (facebookDelegate != null)
			{
				facebookDelegate((T)((object)result));
				return true;
			}
			return false;
		}

		// Token: 0x040005F4 RID: 1524
		private IDictionary<string, object> facebookDelegates = new Dictionary<string, object>();

		// Token: 0x040005F5 RID: 1525
		private int nextAsyncId;
	}
}
