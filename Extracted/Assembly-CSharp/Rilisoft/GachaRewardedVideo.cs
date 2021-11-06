using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FyberPlugin;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000535 RID: 1333
	[DisallowMultipleComponent]
	internal sealed class GachaRewardedVideo : MonoBehaviour
	{
		// Token: 0x06002E79 RID: 11897 RVA: 0x000F3480 File Offset: 0x000F1680
		private GachaRewardedVideo()
		{
			this._currentState = new GachaRewardedVideo.Initial(this);
		}

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06002E7A RID: 11898 RVA: 0x000F3494 File Offset: 0x000F1694
		// (remove) Token: 0x06002E7B RID: 11899 RVA: 0x000F34B0 File Offset: 0x000F16B0
		public event EventHandler<FinishedEventArgs> EnterIdle;

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002E7C RID: 11900 RVA: 0x000F34CC File Offset: 0x000F16CC
		// (remove) Token: 0x06002E7D RID: 11901 RVA: 0x000F34E8 File Offset: 0x000F16E8
		public event EventHandler ExitIdle;

		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06002E7E RID: 11902 RVA: 0x000F3504 File Offset: 0x000F1704
		// (remove) Token: 0x06002E7F RID: 11903 RVA: 0x000F3520 File Offset: 0x000F1720
		public event EventHandler AdWatchedSuccessfully;

		// Token: 0x06002E80 RID: 11904 RVA: 0x000F353C File Offset: 0x000F173C
		public void OnCloseFailurePanel()
		{
			this.Process(GachaRewardedVideo.Input.Close);
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x000F3548 File Offset: 0x000F1748
		public void OnCloseWatchingPanel()
		{
			this.Process(GachaRewardedVideo.Input.Close);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x000F3554 File Offset: 0x000F1754
		public void OnSimulateButtonClicked()
		{
			this.Process(GachaRewardedVideo.Input.Proceed);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x000F3560 File Offset: 0x000F1760
		public void OnWatchButtonClicked()
		{
			this.Process(GachaRewardedVideo.Input.Proceed);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x000F356C File Offset: 0x000F176C
		private void Awake()
		{
			this.Process(GachaRewardedVideo.Input.Start);
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x000F3578 File Offset: 0x000F1778
		private void OnEnable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
			}
			this._backSubscription = BackSystem.Instance.Register(new Action(this.OnBackRequested), base.GetType().Name);
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x000F35C4 File Offset: 0x000F17C4
		private void OnDisable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x000F35E4 File Offset: 0x000F17E4
		private void Update()
		{
			if (this.windowBlocker != null)
			{
				this.windowBlocker.SetActive(this.waitingPanel.activeInHierarchy || this.watchingPanel.activeInHierarchy || this.failurePanel.activeInHierarchy);
			}
			this.Process(GachaRewardedVideo.Input.Update);
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x000F3644 File Offset: 0x000F1844
		private void OnBackRequested()
		{
			ReactionBase<GachaRewardedVideo.Input> reactionBase = this._currentState.React(GachaRewardedVideo.Input.Close);
			StateBase<GachaRewardedVideo.Input> newState = reactionBase.GetNewState();
			if (newState != null)
			{
				this.OnCloseFailurePanel();
			}
			else
			{
				this._backSubscription = BackSystem.Instance.Register(new Action(this.OnCloseFailurePanel), base.GetType().Name);
			}
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x000F36A0 File Offset: 0x000F18A0
		private void RaiseEnterIdle(FinishedEventArgs e)
		{
			EventHandler<FinishedEventArgs> enterIdle = this.EnterIdle;
			if (enterIdle != null)
			{
				enterIdle(this, e);
			}
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x000F36C4 File Offset: 0x000F18C4
		private void RaiseExitIdle(EventArgs e)
		{
			EventHandler exitIdle = this.ExitIdle;
			if (exitIdle != null)
			{
				exitIdle(this, e);
			}
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x000F36E8 File Offset: 0x000F18E8
		private void RaiseAdWatchedSuccessfully(EventArgs e)
		{
			EventHandler adWatchedSuccessfully = this.AdWatchedSuccessfully;
			if (adWatchedSuccessfully != null)
			{
				adWatchedSuccessfully(this, e);
			}
		}

		// Token: 0x06002E8C RID: 11916 RVA: 0x000F370C File Offset: 0x000F190C
		private void Process(GachaRewardedVideo.Input input)
		{
			ReactionBase<GachaRewardedVideo.Input> reactionBase = this._currentState.React(input);
			StateBase<GachaRewardedVideo.Input> newState = reactionBase.GetNewState();
			if (newState == null)
			{
				return;
			}
			StateBase<GachaRewardedVideo.Input> currentState = this._currentState;
			currentState.Exit(newState, input);
			newState.Enter(currentState, input);
			this._currentState = newState;
			IDisposable disposable = currentState as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x06002E8D RID: 11917 RVA: 0x000F3768 File Offset: 0x000F1968
		private static void DebugLog(string message)
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			string format = (!Application.isEditor) ? "[{0}] {1}" : "<color=cyan>[{0}] {1}</color>";
			Debug.LogFormat(format, new object[]
			{
				typeof(GachaRewardedVideo).Name,
				message
			});
		}

		// Token: 0x0400226D RID: 8813
		internal const string LastTimeShownKey = "Ads.LastTimeShown";

		// Token: 0x0400226E RID: 8814
		[SerializeField]
		[Header("Internal")]
		private GameObject waitingPanel;

		// Token: 0x0400226F RID: 8815
		[SerializeField]
		private GameObject watchingPanel;

		// Token: 0x04002270 RID: 8816
		[SerializeField]
		private GameObject failurePanel;

		// Token: 0x04002271 RID: 8817
		[SerializeField]
		private GameObject simulateButton;

		// Token: 0x04002272 RID: 8818
		[SerializeField]
		private UITexture loadingSpinner;

		// Token: 0x04002273 RID: 8819
		public GameObject windowBlocker;

		// Token: 0x04002274 RID: 8820
		private IDisposable _backSubscription;

		// Token: 0x04002275 RID: 8821
		private StateBase<GachaRewardedVideo.Input> _currentState;

		// Token: 0x02000536 RID: 1334
		private enum Input
		{
			// Token: 0x0400227A RID: 8826
			None,
			// Token: 0x0400227B RID: 8827
			Start,
			// Token: 0x0400227C RID: 8828
			Update,
			// Token: 0x0400227D RID: 8829
			Proceed,
			// Token: 0x0400227E RID: 8830
			Close
		}

		// Token: 0x02000537 RID: 1335
		private sealed class Initial : StateBase<GachaRewardedVideo.Input>
		{
			// Token: 0x06002E8E RID: 11918 RVA: 0x000F37BC File Offset: 0x000F19BC
			public Initial(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				this._gachaRewardedVideo = gachaRewardedVideo;
			}

			// Token: 0x06002E8F RID: 11919 RVA: 0x000F37F0 File Offset: 0x000F19F0
			public override ReactionBase<GachaRewardedVideo.Input> React(GachaRewardedVideo.Input input)
			{
				return new TransitReaction<GachaRewardedVideo.Idle, GachaRewardedVideo.Input>(new GachaRewardedVideo.Idle(this._gachaRewardedVideo));
			}

			// Token: 0x0400227F RID: 8831
			private readonly GachaRewardedVideo _gachaRewardedVideo;
		}

		// Token: 0x02000538 RID: 1336
		private sealed class Idle : StateBase<GachaRewardedVideo.Input>
		{
			// Token: 0x06002E90 RID: 11920 RVA: 0x000F3804 File Offset: 0x000F1A04
			public Idle(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				this._gachaRewardedVideo = gachaRewardedVideo;
			}

			// Token: 0x06002E91 RID: 11921 RVA: 0x000F3838 File Offset: 0x000F1A38
			public override void Enter(StateBase<GachaRewardedVideo.Input> oldState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Idle.Enter");
				this._gachaRewardedVideo.RaiseEnterIdle(this.GetEnterEventArgs(oldState));
			}

			// Token: 0x06002E92 RID: 11922 RVA: 0x000F3858 File Offset: 0x000F1A58
			public override ReactionBase<GachaRewardedVideo.Input> React(GachaRewardedVideo.Input input)
			{
				if (input == GachaRewardedVideo.Input.Proceed)
				{
					return new TransitReaction<GachaRewardedVideo.Waiting, GachaRewardedVideo.Input>(new GachaRewardedVideo.Waiting(this._gachaRewardedVideo));
				}
				return DiscardReaction<GachaRewardedVideo.Input>.Default;
			}

			// Token: 0x06002E93 RID: 11923 RVA: 0x000F3878 File Offset: 0x000F1A78
			public override void Exit(StateBase<GachaRewardedVideo.Input> newState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Idle.Exit");
				this._gachaRewardedVideo.RaiseExitIdle(EventArgs.Empty);
			}

			// Token: 0x06002E94 RID: 11924 RVA: 0x000F3894 File Offset: 0x000F1A94
			private FinishedEventArgs GetEnterEventArgs(StateBase<GachaRewardedVideo.Input> oldState)
			{
				GachaRewardedVideo.Watching watching = oldState as GachaRewardedVideo.Watching;
				if (watching == null)
				{
					return FinishedEventArgs.Failure;
				}
				if (!watching.AdClosedFuture.IsCompleted)
				{
					return FinishedEventArgs.Failure;
				}
				if (watching.AdClosedFuture.IsFaulted)
				{
					return FinishedEventArgs.Failure;
				}
				return FinishedEventArgs.Success;
			}

			// Token: 0x04002280 RID: 8832
			private readonly GachaRewardedVideo _gachaRewardedVideo;
		}

		// Token: 0x02000539 RID: 1337
		private sealed class Waiting : StateBase<GachaRewardedVideo.Input>
		{
			// Token: 0x06002E95 RID: 11925 RVA: 0x000F38E8 File Offset: 0x000F1AE8
			public Waiting(GachaRewardedVideo gachaRewardedVideo)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				this._gachaRewardedVideo = gachaRewardedVideo;
			}

			// Token: 0x06002E96 RID: 11926 RVA: 0x000F3930 File Offset: 0x000F1B30
			public override void Enter(StateBase<GachaRewardedVideo.Input> oldState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Waiting.Enter");
				this._gachaRewardedVideo.waitingPanel.SetActive(true);
				if (this._gachaRewardedVideo.simulateButton != null)
				{
					this._gachaRewardedVideo.simulateButton.SetActive(Application.isEditor);
				}
				this._startTime = Time.realtimeSinceStartup;
				FyberCallback.AdAvailable += this.OnAdAvailable;
				FyberCallback.AdNotAvailable += this.OnAdNotAvailable;
				FyberCallback.RequestFail += this.OnRequestFail;
				if (!Application.isEditor)
				{
					RewardedVideoRequester.Create().NotifyUserOnCompletion(false).Request();
				}
			}

			// Token: 0x06002E97 RID: 11927 RVA: 0x000F39DC File Offset: 0x000F1BDC
			public override ReactionBase<GachaRewardedVideo.Input> React(GachaRewardedVideo.Input input)
			{
				if (this._gachaRewardedVideo.loadingSpinner != null)
				{
					float num = Time.realtimeSinceStartup - this._startTime;
					int num2 = Mathf.FloorToInt(num);
					bool flag = num2 % 2 == 0;
					this._gachaRewardedVideo.loadingSpinner.invert = flag;
					this._gachaRewardedVideo.loadingSpinner.fillAmount = ((!flag) ? (1f - num + (float)num2) : (num - (float)num2));
				}
				if (input == GachaRewardedVideo.Input.Proceed && Application.isEditor)
				{
					this._adPromise.TrySetResult(null);
					return DiscardReaction<GachaRewardedVideo.Input>.Default;
				}
				if (input != GachaRewardedVideo.Input.Update)
				{
					return DiscardReaction<GachaRewardedVideo.Input>.Default;
				}
				if (this.AdFuture.IsCompleted)
				{
					if (this.AdFuture.IsFaulted)
					{
						Exception ex = this.AdFuture.Exception.InnerExceptions.FirstOrDefault<Exception>();
						string reason = (ex == null) ? this.AdFuture.Exception.Message : ex.Message;
						return new TransitReaction<GachaRewardedVideo.Failure, GachaRewardedVideo.Input>(new GachaRewardedVideo.Failure(this._gachaRewardedVideo, reason));
					}
					if (this.AdFuture.Result != null)
					{
						return new TransitReaction<GachaRewardedVideo.Watching, GachaRewardedVideo.Input>(new GachaRewardedVideo.Watching(this._gachaRewardedVideo, this.AdFuture.Result));
					}
					if (Application.isEditor)
					{
						return new TransitReaction<GachaRewardedVideo.Watching, GachaRewardedVideo.Input>(new GachaRewardedVideo.Watching(this._gachaRewardedVideo, this.AdFuture.Result));
					}
					return new TransitReaction<GachaRewardedVideo.Failure, GachaRewardedVideo.Input>(new GachaRewardedVideo.Failure(this._gachaRewardedVideo, "Ad is not available."));
				}
				else
				{
					if (5f <= Time.realtimeSinceStartup - this._startTime)
					{
						string reason2 = string.Format(CultureInfo.InvariantCulture, "Timeout {0:f1} seconds.", new object[]
						{
							5f
						});
						return new TransitReaction<GachaRewardedVideo.Failure, GachaRewardedVideo.Input>(new GachaRewardedVideo.Failure(this._gachaRewardedVideo, reason2));
					}
					return DiscardReaction<GachaRewardedVideo.Input>.Default;
				}
			}

			// Token: 0x06002E98 RID: 11928 RVA: 0x000F3BAC File Offset: 0x000F1DAC
			public override void Exit(StateBase<GachaRewardedVideo.Input> newState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Waiting.Exit");
				FyberCallback.AdAvailable -= this.OnAdAvailable;
				FyberCallback.AdNotAvailable -= this.OnAdNotAvailable;
				FyberCallback.RequestFail -= this.OnRequestFail;
				if (this._gachaRewardedVideo.simulateButton != null)
				{
					this._gachaRewardedVideo.simulateButton.SetActive(false);
				}
				this._gachaRewardedVideo.waitingPanel.SetActive(false);
			}

			// Token: 0x06002E99 RID: 11929 RVA: 0x000F3C30 File Offset: 0x000F1E30
			private void OnAdAvailable(Ad ad)
			{
				this._adPromise.TrySetResult(ad);
			}

			// Token: 0x06002E9A RID: 11930 RVA: 0x000F3C40 File Offset: 0x000F1E40
			private void OnAdNotAvailable(AdFormat adFormat)
			{
				if (adFormat != AdFormat.REWARDED_VIDEO)
				{
					GachaRewardedVideo.DebugLog("Unexpected ad format: " + adFormat);
					return;
				}
				this._adPromise.TrySetResult(null);
			}

			// Token: 0x06002E9B RID: 11931 RVA: 0x000F3C78 File Offset: 0x000F1E78
			private void OnRequestFail(RequestError requestError)
			{
				this._adPromise.TrySetException(new InvalidOperationException(requestError.Description));
			}

			// Token: 0x17000805 RID: 2053
			// (get) Token: 0x06002E9C RID: 11932 RVA: 0x000F3C94 File Offset: 0x000F1E94
			private Task<Ad> AdFuture
			{
				get
				{
					return this._adPromise.Task;
				}
			}

			// Token: 0x04002281 RID: 8833
			private const float TimeoutInSeconds = 5f;

			// Token: 0x04002282 RID: 8834
			private readonly GachaRewardedVideo _gachaRewardedVideo;

			// Token: 0x04002283 RID: 8835
			private readonly TaskCompletionSource<Ad> _adPromise = new TaskCompletionSource<Ad>();

			// Token: 0x04002284 RID: 8836
			private float _startTime = Time.realtimeSinceStartup;
		}

		// Token: 0x0200053A RID: 1338
		private sealed class Failure : StateBase<GachaRewardedVideo.Input>
		{
			// Token: 0x06002E9D RID: 11933 RVA: 0x000F3CA4 File Offset: 0x000F1EA4
			public Failure(GachaRewardedVideo gachaRewardedVideo, string reason)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				this._gachaRewardedVideo = gachaRewardedVideo;
				this._reason = (reason ?? string.Empty);
			}

			// Token: 0x06002E9E RID: 11934 RVA: 0x000F3CE0 File Offset: 0x000F1EE0
			public override void Enter(StateBase<GachaRewardedVideo.Input> oldState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Failure.Enter: " + this._reason);
				this._gachaRewardedVideo.failurePanel.SetActive(true);
			}

			// Token: 0x06002E9F RID: 11935 RVA: 0x000F3D14 File Offset: 0x000F1F14
			public override ReactionBase<GachaRewardedVideo.Input> React(GachaRewardedVideo.Input input)
			{
				if (input == GachaRewardedVideo.Input.Close)
				{
					return new TransitReaction<GachaRewardedVideo.Idle, GachaRewardedVideo.Input>(new GachaRewardedVideo.Idle(this._gachaRewardedVideo));
				}
				return DiscardReaction<GachaRewardedVideo.Input>.Default;
			}

			// Token: 0x06002EA0 RID: 11936 RVA: 0x000F3D34 File Offset: 0x000F1F34
			public override void Exit(StateBase<GachaRewardedVideo.Input> newState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Failure.Exit: " + this._reason);
				this._gachaRewardedVideo.failurePanel.SetActive(false);
			}

			// Token: 0x04002285 RID: 8837
			private readonly GachaRewardedVideo _gachaRewardedVideo;

			// Token: 0x04002286 RID: 8838
			private readonly string _reason;
		}

		// Token: 0x0200053B RID: 1339
		private sealed class Watching : StateBase<GachaRewardedVideo.Input>
		{
			// Token: 0x06002EA1 RID: 11937 RVA: 0x000F3D68 File Offset: 0x000F1F68
			public Watching(GachaRewardedVideo gachaRewardedVideo, Ad ad)
			{
				if (gachaRewardedVideo == null)
				{
					throw new ArgumentNullException("gachaRewardedVideo");
				}
				if (!Application.isEditor && ad == null)
				{
					throw new ArgumentNullException("ad");
				}
				this._ad = ad;
				this._gachaRewardedVideo = gachaRewardedVideo;
			}

			// Token: 0x06002EA2 RID: 11938 RVA: 0x000F3DC8 File Offset: 0x000F1FC8
			public override void Enter(StateBase<GachaRewardedVideo.Input> oldState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Watching.Enter");
				this._gachaRewardedVideo.watchingPanel.SetActive(true);
				FyberCallback.AdFinished += this.OnAdFinished;
				CoroutineRunner.Instance.StartCoroutine(this.WaitFutureThenContinue());
				if (Application.isEditor)
				{
					CoroutineRunner.Instance.StartCoroutine(this.SimulateWatchingCoroutine());
				}
				else
				{
					this._ad.Start();
				}
			}

			// Token: 0x06002EA3 RID: 11939 RVA: 0x000F3E40 File Offset: 0x000F2040
			public override ReactionBase<GachaRewardedVideo.Input> React(GachaRewardedVideo.Input input)
			{
				if (input == GachaRewardedVideo.Input.Close)
				{
					string message = "Watching panel was closed manually.";
					this._adClosedPromise.TrySetException(new InvalidOperationException(message));
					return new TransitReaction<GachaRewardedVideo.Idle, GachaRewardedVideo.Input>(new GachaRewardedVideo.Idle(this._gachaRewardedVideo));
				}
				if (input != GachaRewardedVideo.Input.Update)
				{
					return DiscardReaction<GachaRewardedVideo.Input>.Default;
				}
				if (!this.AdClosedFuture.IsCompleted)
				{
					return DiscardReaction<GachaRewardedVideo.Input>.Default;
				}
				return new TransitReaction<GachaRewardedVideo.Idle, GachaRewardedVideo.Input>(new GachaRewardedVideo.Idle(this._gachaRewardedVideo));
			}

			// Token: 0x06002EA4 RID: 11940 RVA: 0x000F3EB0 File Offset: 0x000F20B0
			public override void Exit(StateBase<GachaRewardedVideo.Input> newState, GachaRewardedVideo.Input input)
			{
				GachaRewardedVideo.DebugLog("Watching.Exit");
				FyberCallback.AdFinished -= this.OnAdFinished;
				if (this.AdClosedFuture.IsFaulted)
				{
					Exception ex = this.AdClosedFuture.Exception.InnerExceptions.FirstOrDefault<Exception>();
					string message = (ex == null) ? this.AdClosedFuture.Exception.Message : ex.Message;
					Debug.LogWarning(message);
				}
				this._gachaRewardedVideo.watchingPanel.SetActive(false);
			}

			// Token: 0x06002EA5 RID: 11941 RVA: 0x000F3F38 File Offset: 0x000F2138
			private void OnAdFinished(AdResult adResult)
			{
				FyberCallback.AdFinished -= this.OnAdFinished;
				if (adResult.AdFormat != AdFormat.REWARDED_VIDEO)
				{
					GachaRewardedVideo.DebugLog("Unexpected ad format: " + adResult.AdFormat);
					return;
				}
				if (adResult.Status != AdStatus.OK)
				{
					string message = "Bad status: " + adResult.Status;
					this._adClosedPromise.TrySetException(new InvalidOperationException(message));
					return;
				}
				PlayerPrefs.SetString("Ads.LastTimeShown", DateTime.UtcNow.ToString("s"));
				this._adClosedPromise.TrySetResult(adResult.Message);
			}

			// Token: 0x06002EA6 RID: 11942 RVA: 0x000F3FE0 File Offset: 0x000F21E0
			private IEnumerator WaitFutureThenContinue()
			{
				Task<string> f = this.AdClosedFuture;
				yield return new WaitUntil(() => f.IsCompleted);
				if (f.IsFaulted || f.IsCanceled)
				{
					yield break;
				}
				if (f.Result == "CLOSE_FINISHED")
				{
					this._gachaRewardedVideo.RaiseAdWatchedSuccessfully(EventArgs.Empty);
				}
				yield break;
			}

			// Token: 0x06002EA7 RID: 11943 RVA: 0x000F3FFC File Offset: 0x000F21FC
			private IEnumerator SimulateWatchingCoroutine()
			{
				yield return new WaitForSeconds(5f);
				if (this.AdClosedFuture.IsCompleted)
				{
					yield break;
				}
				PlayerPrefs.SetString("Ads.LastTimeShown", DateTime.UtcNow.ToString("s"));
				this._adClosedPromise.TrySetResult("CLOSE_FINISHED");
				yield break;
			}

			// Token: 0x17000806 RID: 2054
			// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x000F4018 File Offset: 0x000F2218
			internal Task<string> AdClosedFuture
			{
				get
				{
					return this._adClosedPromise.Task;
				}
			}

			// Token: 0x04002287 RID: 8839
			private readonly Ad _ad;

			// Token: 0x04002288 RID: 8840
			private readonly TaskCompletionSource<string> _adClosedPromise = new TaskCompletionSource<string>();

			// Token: 0x04002289 RID: 8841
			private readonly GachaRewardedVideo _gachaRewardedVideo;
		}
	}
}
