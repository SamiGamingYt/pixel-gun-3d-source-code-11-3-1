using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x0200010A RID: 266
	internal class AsyncRequestString : MonoBehaviour
	{
		// Token: 0x060007C3 RID: 1987 RVA: 0x0002F9AC File Offset: 0x0002DBAC
		internal static void Post(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.POST, formData, callback);
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002F9B8 File Offset: 0x0002DBB8
		internal static void Get(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.GET, formData, callback);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
		internal static void Request(Uri url, HttpMethod method, WWWForm query = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetQuery(query).SetCallback(callback);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0002F9F0 File Offset: 0x0002DBF0
		internal static void Request(Uri url, HttpMethod method, IDictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetFormData(formData).SetCallback(callback);
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002FA1C File Offset: 0x0002DC1C
		internal IEnumerator Start()
		{
			WWW www;
			if (this.method == HttpMethod.GET)
			{
				string urlParams = (!this.url.AbsoluteUri.Contains("?")) ? "?" : "&";
				if (this.formData != null)
				{
					foreach (KeyValuePair<string, string> pair in this.formData)
					{
						urlParams += string.Format("{0}={1}&", Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(pair.Value));
					}
				}
				Dictionary<string, string> headers = new Dictionary<string, string>();
				headers["User-Agent"] = Constants.GraphApiUserAgent;
				www = new WWW(this.url + urlParams, null, headers);
			}
			else
			{
				if (this.query == null)
				{
					this.query = new WWWForm();
				}
				if (this.method == HttpMethod.DELETE)
				{
					this.query.AddField("method", "delete");
				}
				if (this.formData != null)
				{
					foreach (KeyValuePair<string, string> pair2 in this.formData)
					{
						this.query.AddField(pair2.Key, pair2.Value);
					}
				}
				this.query.headers["User-Agent"] = Constants.GraphApiUserAgent;
				www = new WWW(this.url.AbsoluteUri, this.query);
			}
			yield return www;
			if (this.callback != null)
			{
				this.callback(new GraphResult(www));
			}
			www.Dispose();
			UnityEngine.Object.Destroy(this);
			yield break;
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002FA38 File Offset: 0x0002DC38
		internal AsyncRequestString SetUrl(Uri url)
		{
			this.url = url;
			return this;
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002FA44 File Offset: 0x0002DC44
		internal AsyncRequestString SetMethod(HttpMethod method)
		{
			this.method = method;
			return this;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0002FA50 File Offset: 0x0002DC50
		internal AsyncRequestString SetFormData(IDictionary<string, string> formData)
		{
			this.formData = formData;
			return this;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0002FA5C File Offset: 0x0002DC5C
		internal AsyncRequestString SetQuery(WWWForm query)
		{
			this.query = query;
			return this;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0002FA68 File Offset: 0x0002DC68
		internal AsyncRequestString SetCallback(FacebookDelegate<IGraphResult> callback)
		{
			this.callback = callback;
			return this;
		}

		// Token: 0x04000685 RID: 1669
		private Uri url;

		// Token: 0x04000686 RID: 1670
		private HttpMethod method;

		// Token: 0x04000687 RID: 1671
		private IDictionary<string, string> formData;

		// Token: 0x04000688 RID: 1672
		private WWWForm query;

		// Token: 0x04000689 RID: 1673
		private FacebookDelegate<IGraphResult> callback;
	}
}
