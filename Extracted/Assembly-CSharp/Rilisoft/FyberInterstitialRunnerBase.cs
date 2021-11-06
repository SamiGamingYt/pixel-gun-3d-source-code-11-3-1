using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using FyberPlugin;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000534 RID: 1332
	internal abstract class FyberInterstitialRunnerBase : IDisposable, AdCallback
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x000F301C File Offset: 0x000F121C
		public void Run()
		{
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.Run" : "<color=magenta>{0}.Run</color>";
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name
				});
			}
			FyberCallback.AdAvailable += this.OnAdAvailable;
			FyberCallback.AdNotAvailable += this.OnAdNotAvailable;
			FyberCallback.RequestFail += this.OnRequestFail;
			if (Application.isEditor || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				CoroutineRunner.Instance.StartCoroutine(this.SimulateRequestCoroutine());
			}
			else
			{
				InterstitialRequester.Create().Request();
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06002E6D RID: 11885 RVA: 0x000F30D0 File Offset: 0x000F12D0
		public Task<AdResult> AdFinishedFuture
		{
			get
			{
				return this._adFinishedPromise.Task;
			}
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x000F30E0 File Offset: 0x000F12E0
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this.Unsubscribe();
			this.Dispose(true);
			this._disposed = true;
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x000F3110 File Offset: 0x000F1310
		public void OnAdFinished(AdResult result)
		{
			if (this._disposed)
			{
				return;
			}
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.OnAdFinished: {1}" : "<color=magenta>{0}.OnAdFinished: {1}</color>";
				string text = string.Format(CultureInfo.InvariantCulture, "{{ status: {0}, message: '{1}' }}", new object[]
				{
					result.Status,
					result.Message
				});
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name,
					text
				});
			}
			this._adFinishedPromise.TrySetResult(result);
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x000F31A8 File Offset: 0x000F13A8
		public void OnAdStarted(Ad ad)
		{
			if (this._disposed)
			{
				return;
			}
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.OnAdStarted: {1}" : "<color=magenta>{0}.OnAdStarted: {1}</color>";
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name,
					ad
				});
			}
		}

		// Token: 0x06002E71 RID: 11889
		protected abstract string GetReasonToSkip();

		// Token: 0x06002E72 RID: 11890 RVA: 0x000F3204 File Offset: 0x000F1404
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x000F3208 File Offset: 0x000F1408
		private void OnAdAvailable(Ad ad)
		{
			if (this._disposed)
			{
				return;
			}
			if (ad.AdFormat != AdFormat.INTERSTITIAL)
			{
				return;
			}
			this.Unsubscribe();
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.OnAdAvailable: {1}" : "<color=magenta>{0}.OnAdAvailable: {1}</color>";
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name,
					ad
				});
			}
			string reasonToSkip = this.GetReasonToSkip();
			if (!string.IsNullOrEmpty(reasonToSkip))
			{
				Debug.LogFormat("Skipping showing interstitial: {0}", new object[]
				{
					reasonToSkip
				});
				return;
			}
			Debug.Log("Trying to show interstitial.");
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, DateTime.UtcNow.ToString("s"));
			FyberFacade.Instance.IncrementCurrentDailyInterstitialCount();
			if (Application.isEditor || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				CoroutineRunner.Instance.StartCoroutine(this.SimulateStartCoroutine());
			}
			else
			{
				ad.WithCallback(this).Start();
			}
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x000F3308 File Offset: 0x000F1508
		private void OnAdNotAvailable(AdFormat adFormat)
		{
			if (this._disposed)
			{
				return;
			}
			if (adFormat != AdFormat.INTERSTITIAL)
			{
				return;
			}
			this.Unsubscribe();
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.OnAdNotAvailable: {1}" : "<color=magenta>{0}.OnAdNotAvailable: {1}</color>";
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name,
					adFormat
				});
			}
			string message = "Interstitial is not available.";
			this._adFinishedPromise.TrySetException(new InvalidOperationException(message));
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x000F3390 File Offset: 0x000F1590
		private void OnRequestFail(RequestError requestError)
		{
			if (this._disposed)
			{
				return;
			}
			this.Unsubscribe();
			if (Defs.IsDeveloperBuild)
			{
				string format = (!Application.isEditor) ? "{0}.OnRequestFail: {1}" : "<color=magenta>{0}.OnRequestFail: {1}</color>";
				Debug.LogFormat(format, new object[]
				{
					base.GetType().Name,
					requestError.Description
				});
			}
			this._adFinishedPromise.TrySetException(new InvalidOperationException(requestError.Description));
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x000F3410 File Offset: 0x000F1610
		private void Unsubscribe()
		{
			FyberCallback.AdAvailable -= this.OnAdAvailable;
			FyberCallback.AdNotAvailable -= this.OnAdNotAvailable;
			FyberCallback.RequestFail -= this.OnRequestFail;
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000F3448 File Offset: 0x000F1648
		private IEnumerator SimulateRequestCoroutine()
		{
			yield return new WaitForSeconds(1f);
			this.OnAdNotAvailable(AdFormat.INTERSTITIAL);
			yield break;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x000F3464 File Offset: 0x000F1664
		private IEnumerator SimulateStartCoroutine()
		{
			yield return new WaitForSeconds(1f);
			this._adFinishedPromise.TrySetException(new NotSupportedException());
			yield break;
		}

		// Token: 0x0400226B RID: 8811
		private readonly TaskCompletionSource<AdResult> _adFinishedPromise = new TaskCompletionSource<AdResult>();

		// Token: 0x0400226C RID: 8812
		private bool _disposed;
	}
}
