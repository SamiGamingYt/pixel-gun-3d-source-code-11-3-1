using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FyberPlugin;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000620 RID: 1568
internal sealed class FyberFacade
{
	// Token: 0x0600364A RID: 13898 RVA: 0x001186F8 File Offset: 0x001168F8
	private FyberFacade()
	{
	}

	// Token: 0x170008EA RID: 2282
	// (get) Token: 0x0600364C RID: 13900 RVA: 0x00118738 File Offset: 0x00116938
	public static FyberFacade Instance
	{
		get
		{
			return FyberFacade._instance.Value;
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x00118744 File Offset: 0x00116944
	public int GetCurrentDailyInterstitialCount()
	{
		string key = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
		string @string = PlayerPrefs.GetString("Ads.DailyInterstitialCount", string.Empty);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return 0;
		}
		object value;
		if (!dictionary.TryGetValue(key, out value))
		{
			return 0;
		}
		int result;
		try
		{
			result = Convert.ToInt32(value);
		}
		catch
		{
			result = 0;
		}
		return result;
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x001187E0 File Offset: 0x001169E0
	public void IncrementCurrentDailyInterstitialCount()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(1);
		string @string = PlayerPrefs.GetString("Ads.DailyInterstitialCount", string.Empty);
		Dictionary<string, object> dictionary2 = Json.Deserialize(@string) as Dictionary<string, object>;
		string key = DateTime.UtcNow.Date.ToString("yyyy-MM-dd");
		object value;
		if (dictionary2 == null)
		{
			dictionary.Add(key, 1);
		}
		else if (!dictionary2.TryGetValue(key, out value))
		{
			dictionary.Add(key, 1);
		}
		else
		{
			try
			{
				int value2 = Convert.ToInt32(value) + 1;
				dictionary.Add(key, value2);
			}
			catch
			{
				dictionary.Add(key, 1);
			}
		}
		string value3 = Json.Serialize(dictionary);
		PlayerPrefs.SetString("Ads.DailyInterstitialCount", value3);
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x001188B4 File Offset: 0x00116AB4
	public Task<Ad> RequestImageInterstitial(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		TaskCompletionSource<Ad> promise = new TaskCompletionSource<Ad>();
		return this.RequestImageInterstitialCore(promise, callerName);
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x001188DC File Offset: 0x00116ADC
	private Task<Ad> RequestImageInterstitialCore(TaskCompletionSource<Ad> promise, string callerName)
	{
		Action<Ad> onAdAvailable = null;
		Action<AdFormat> onAdNotAvailable = null;
		Action<RequestError> onRequestFail = null;
		onAdAvailable = delegate(Ad ad)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdAvailable: {{ format: {0}, placementId: '{1}' }}", new object[]
				{
					ad.AdFormat,
					ad.PlacementId
				});
			}
			promise.SetResult(ad);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onAdNotAvailable = delegate(AdFormat adFormat)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > AdNotAvailable: {{ format: {0} }}", new object[]
				{
					adFormat
				});
			}
			AdNotAwailableException exception = new AdNotAwailableException("Ad not available: " + adFormat);
			promise.SetException(exception);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onRequestFail = delegate(RequestError requestError)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("RequestImageInterstitialCore > RequestFail: {{ requestError: {0} }}", new object[]
				{
					requestError.Description
				});
			}
			AdRequestException exception = new AdRequestException(requestError.Description);
			promise.SetException(exception);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		FyberCallback.AdAvailable += onAdAvailable;
		FyberCallback.AdNotAvailable += onAdNotAvailable;
		FyberCallback.RequestFail += onRequestFail;
		FyberFacade.RequestInterstitialAds(callerName);
		if (Application.isEditor)
		{
			promise.SetException(new NotSupportedException("Ads are not supported in Editor."));
		}
		return promise.Task;
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x00118994 File Offset: 0x00116B94
	public Task<AdResult> ShowInterstitial(Dictionary<string, string> parameters, string callerName = null)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, string>();
		}
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Debug.LogFormat("[Rilisoft] ShowInterstitial('{0}')", new object[]
		{
			callerName
		});
		if (this.Requests.Count == 0)
		{
			Debug.LogWarning("[Rilisoft] No active requests.");
			TaskCompletionSource<AdResult> taskCompletionSource = new TaskCompletionSource<AdResult>();
			taskCompletionSource.SetException(new InvalidOperationException("No active requests."));
			return taskCompletionSource.Task;
		}
		Debug.Log("[Rilisoft] Active requests count: " + this.Requests.Count);
		LinkedListNode<Task<Ad>> requestNode = null;
		for (LinkedListNode<Task<Ad>> linkedListNode = this.Requests.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
		{
			if (!linkedListNode.Value.IsFaulted)
			{
				if (linkedListNode.Value.IsCompleted)
				{
					requestNode = linkedListNode;
					break;
				}
				if (requestNode == null)
				{
					requestNode = linkedListNode;
				}
			}
		}
		if (requestNode == null)
		{
			string text = "All requests are faulted: " + this.Requests.Count;
			Debug.LogWarning("[Rilisoft]" + text);
			TaskCompletionSource<AdResult> taskCompletionSource2 = new TaskCompletionSource<AdResult>();
			taskCompletionSource2.SetException(new InvalidOperationException(text));
			return taskCompletionSource2.Task;
		}
		TaskCompletionSource<AdResult> showPromise = new TaskCompletionSource<AdResult>();
		Action<Task<Ad>> action = delegate(Task<Ad> requestFuture)
		{
			if (requestFuture.IsFaulted)
			{
				string text2 = "Ad request failed: " + requestFuture.Exception.InnerException.Message;
				Debug.LogWarningFormat("[Rilisoft] {0}", new object[]
				{
					text2
				});
				showPromise.SetException(new AdRequestException(text2, requestFuture.Exception.InnerException));
				return;
			}
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("[Rilisoft] Ad request succeeded: {{ adFormat: {0}, placementId: '{1}' }}", new object[]
				{
					requestFuture.Result.AdFormat,
					requestFuture.Result.PlacementId
				});
			}
			Action<AdResult> adFinished = null;
			adFinished = delegate(AdResult adResult)
			{
				Lazy<string> lazy = new Lazy<string>(() => string.Format("[Rilisoft] Ad show finished: {{ format: {0}, status: {1}, message: '{2}' }}", adResult.AdFormat, adResult.Status, adResult.Message));
				if (adResult.Status == AdStatus.Error)
				{
					Debug.LogWarning(lazy.Value);
				}
				else if (Defs.IsDeveloperBuild)
				{
					Debug.Log(lazy.Value);
				}
				FyberCallback.AdFinished -= adFinished;
				showPromise.SetResult(adResult);
			};
			FyberCallback.AdFinished += adFinished;
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Start showing ad: {{ format: {0}, placementId: '{1}' }}", new object[]
				{
					requestFuture.Result.AdFormat,
					requestFuture.Result.PlacementId
				});
			}
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, DateTime.UtcNow.ToString("s"));
			this.IncrementCurrentDailyInterstitialCount();
			requestFuture.Result.Start();
			this.Requests.Remove(requestNode);
		};
		if (requestNode.Value.IsCompleted)
		{
			action(requestNode.Value);
		}
		else
		{
			requestNode.Value.ContinueWith(action);
		}
		return showPromise.Task;
	}

	// Token: 0x170008EB RID: 2283
	// (get) Token: 0x06003652 RID: 13906 RVA: 0x00118B64 File Offset: 0x00116D64
	public LinkedList<Task<Ad>> Requests
	{
		get
		{
			return this._requests;
		}
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x00118B6C File Offset: 0x00116D6C
	public void SetUserPaying(string payingBin)
	{
		if (string.IsNullOrEmpty(payingBin))
		{
			payingBin = "0";
		}
		this.SetUserPayingCore(payingBin);
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x00118B88 File Offset: 0x00116D88
	public void UpdateUserPaying()
	{
		string userPayingCore = Storager.getInt("PayingUser", true).ToString(CultureInfo.InvariantCulture);
		this.SetUserPayingCore(userPayingCore);
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x00118BB8 File Offset: 0x00116DB8
	private void SetUserPayingCore(string payingBin)
	{
		User.PutCustomValue("pg3d_paying", payingBin);
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x00118BC8 File Offset: 0x00116DC8
	private static void RequestInterstitialAds(string callerName)
	{
		InterstitialRequester.Create().Request();
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("[Rilisoft] RequestInterstitialAds('{0}')", callerName);
			Debug.Log(message);
		}
	}

	// Token: 0x040027DD RID: 10205
	private const string DailyInterstitialCountKey = "Ads.DailyInterstitialCount";

	// Token: 0x040027DE RID: 10206
	private readonly LinkedList<Task<Ad>> _requests = new LinkedList<Task<Ad>>();

	// Token: 0x040027DF RID: 10207
	private static readonly Lazy<FyberFacade> _instance = new Lazy<FyberFacade>(() => new FyberFacade());
}
