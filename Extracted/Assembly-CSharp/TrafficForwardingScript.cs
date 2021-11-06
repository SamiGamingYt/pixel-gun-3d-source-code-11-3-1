using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200076F RID: 1903
internal sealed class TrafficForwardingScript : MonoBehaviour
{
	// Token: 0x0600430B RID: 17163 RVA: 0x001666C0 File Offset: 0x001648C0
	internal IEnumerator GetTrafficForwardingConfigLoopCoroutine()
	{
		yield return base.StartCoroutine(this.GetTrafficForwardingConfigCoroutine());
		float firstDelaySeconds = Math.Max(60f, Defs.timeUpdatePixelbookInfo - 60f);
		yield return new WaitForSeconds(firstDelaySeconds);
		yield return base.StartCoroutine(this.GetTrafficForwardingConfigCoroutine());
		float delaySeconds = Defs.timeUpdatePixelbookInfo;
		for (;;)
		{
			if (Time.realtimeSinceStartup - this._trafficForwardingConfigTimestamp < delaySeconds)
			{
				yield return null;
			}
			else
			{
				yield return base.StartCoroutine(this.GetTrafficForwardingConfigCoroutine());
			}
		}
		yield break;
	}

	// Token: 0x0600430C RID: 17164 RVA: 0x001666DC File Offset: 0x001648DC
	internal IEnumerator GetTrafficForwardingConfigCoroutine()
	{
		this._trafficForwardingConfigTimestamp = Time.realtimeSinceStartup;
		if (this._trafficForwardingPromise.Task.IsCompleted)
		{
			this._trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.TrafficForwardingConfigUrl);
		yield return response;
		if (response == null || !string.IsNullOrEmpty(response.error))
		{
			this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			string message = (response != null) ? response.error : "null";
			this._trafficForwardingPromise.TrySetException(new InvalidOperationException(message));
			yield break;
		}
		string responseText = URLs.Sanitize(response);
		Dictionary<string, object> responseDict = Json.Deserialize(responseText) as Dictionary<string, object>;
		if (responseDict == null)
		{
			this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			this._trafficForwardingPromise.TrySetException(new InvalidOperationException("Couldnot deserialize response: " + responseText));
			yield break;
		}
		object trafficForwardingObject;
		if (responseDict.TryGetValue("trafficForwarding_v_10.2.0", out trafficForwardingObject))
		{
			Dictionary<string, object> trafficForwarding = trafficForwardingObject as Dictionary<string, object>;
			if (trafficForwarding != null)
			{
				object urlObject;
				if (trafficForwarding.TryGetValue("url", out urlObject))
				{
					string url = Convert.ToString(urlObject);
					url = string.Concat(new string[]
					{
						url,
						"&uid=",
						FriendsController.sharedController.id,
						"&device=",
						SystemInfo.deviceUniqueIdentifier
					});
					int minLevel = 0;
					try
					{
						minLevel = Convert.ToInt32(trafficForwarding["minLevel"]);
					}
					catch (Exception ex3)
					{
						Exception ex = ex3;
						Debug.LogWarning(ex.ToString());
					}
					int maxLevel = 31;
					try
					{
						maxLevel = Convert.ToInt32(trafficForwarding["maxLevel"]);
					}
					catch (Exception ex4)
					{
						Exception ex2 = ex4;
						Debug.LogWarning(ex2.ToString());
					}
					TrafficForwardingInfo result = new TrafficForwardingInfo(url, minLevel, maxLevel);
					this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
					{
						handler(this, result);
					});
					this._trafficForwardingPromise.TrySetResult(result);
				}
				else
				{
					this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
					{
						handler(this, TrafficForwardingInfo.DisabledInstance);
					});
					this._trafficForwardingPromise.TrySetResult(TrafficForwardingInfo.DisabledInstance);
				}
			}
			else
			{
				this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
				{
					handler(this, TrafficForwardingInfo.DisabledInstance);
				});
				this._trafficForwardingPromise.TrySetException(new InvalidOperationException("Couldnot deserialize trafficForwarding node: " + Json.Serialize(trafficForwardingObject)));
			}
		}
		else
		{
			this.Updated.Do(delegate(EventHandler<TrafficForwardingInfo> handler)
			{
				handler(this, TrafficForwardingInfo.DisabledInstance);
			});
			this._trafficForwardingPromise.TrySetException(new InvalidOperationException("Response doesn't contain trafficForwarding node."));
		}
		yield break;
	}

	// Token: 0x0600430D RID: 17165 RVA: 0x001666F8 File Offset: 0x001648F8
	internal Task<TrafficForwardingInfo> GetTrafficForwardingInfo()
	{
		return this._trafficForwardingPromise.Task;
	}

	// Token: 0x04003118 RID: 12568
	public EventHandler<TrafficForwardingInfo> Updated;

	// Token: 0x04003119 RID: 12569
	private float _trafficForwardingConfigTimestamp;

	// Token: 0x0400311A RID: 12570
	private TaskCompletionSource<TrafficForwardingInfo> _trafficForwardingPromise = new TaskCompletionSource<TrafficForwardingInfo>();
}
