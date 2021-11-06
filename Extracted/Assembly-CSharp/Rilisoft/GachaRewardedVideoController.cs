using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000625 RID: 1573
	[DisallowMultipleComponent]
	internal sealed class GachaRewardedVideoController : MonoBehaviour
	{
		// Token: 0x06003660 RID: 13920 RVA: 0x00118C94 File Offset: 0x00116E94
		private GachaRewardedVideoController()
		{
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x00118C9C File Offset: 0x00116E9C
		public void OnGachaRewardedVideoButton()
		{
			if (this.GachaRewardedVideo != null)
			{
				this.GachaRewardedVideo.OnWatchButtonClicked();
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x00118CBC File Offset: 0x00116EBC
		public static GachaRewardedVideoController Instance
		{
			get
			{
				return GachaRewardedVideoController.s_instance;
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00118CC4 File Offset: 0x00116EC4
		public void RefreshGui(bool forceButton)
		{
			this.gachaRewardedVideoButton.SetActive(forceButton);
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00118CD4 File Offset: 0x00116ED4
		public void RefreshGui()
		{
			bool forceButton = this.GachaRewardedVideoButtonIsEnabled();
			this.RefreshGui(forceButton);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00118CF0 File Offset: 0x00116EF0
		private void Awake()
		{
			GachaRewardedVideoController.s_instance = this;
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00118CF8 File Offset: 0x00116EF8
		private void Start()
		{
			this.RefreshGui();
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00118D00 File Offset: 0x00116F00
		private void OnDestroy()
		{
			if (this._gachaRewardedVideo != null)
			{
				this._gachaRewardedVideo.EnterIdle -= this.OnEnterIdle;
				this._gachaRewardedVideo.ExitIdle -= this.OnExitIdle;
				this._gachaRewardedVideo.AdWatchedSuccessfully -= this.OnAdWatchedSuccessfully;
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00118D64 File Offset: 0x00116F64
		private static string GetReasonToDismissVideoFreeSpin()
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				return "Ads config is `null`.";
			}
			if (lastLoadedConfig.Exception != null)
			{
				return lastLoadedConfig.Exception.Message;
			}
			string videoDisabledReason = AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig);
			if (!string.IsNullOrEmpty(videoDisabledReason))
			{
				return videoDisabledReason;
			}
			FreeSpinPointMemento freeSpin = lastLoadedConfig.AdPointsConfig.FreeSpin;
			if (freeSpin == null)
			{
				return string.Format("`{0}` config is `null`", freeSpin.Id);
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = freeSpin.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			double finalTimeoutBetweenShowInMinutes = freeSpin.GetFinalTimeoutBetweenShowInMinutes(playerCategory);
			double timeSpanSinceLastShowInMinutes = GachaRewardedVideoController.GetTimeSpanSinceLastShowInMinutes();
			if (timeSpanSinceLastShowInMinutes < finalTimeoutBetweenShowInMinutes)
			{
				return string.Format(CultureInfo.InvariantCulture, "`{0}`: {1:f2} < `timeoutInMinutes: {2:f2}`", new object[]
				{
					freeSpin.Id,
					timeSpanSinceLastShowInMinutes,
					finalTimeoutBetweenShowInMinutes
				});
			}
			return string.Empty;
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00118E48 File Offset: 0x00117048
		private static double GetTimeSpanSinceLastShowInMinutes()
		{
			DateTime? timeSinceLastShow = GachaRewardedVideoController.GetTimeSinceLastShow();
			if (timeSinceLastShow == null)
			{
				return 3.4028234663852886E+38;
			}
			return DateTime.UtcNow.Subtract(timeSinceLastShow.Value).TotalMinutes;
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00118E90 File Offset: 0x00117090
		private static DateTime? GetTimeSinceLastShow()
		{
			string @string = PlayerPrefs.GetString("Ads.LastTimeShown", string.Empty);
			if (string.IsNullOrEmpty(@string))
			{
				return null;
			}
			DateTime value;
			if (!DateTime.TryParse(@string, out value))
			{
				return null;
			}
			return new DateTime?(value);
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00118EE0 File Offset: 0x001170E0
		public static bool VideoViewedToday()
		{
			DateTime? timeSinceLastShow = GachaRewardedVideoController.GetTimeSinceLastShow();
			if (timeSinceLastShow == null)
			{
				return false;
			}
			DateTime t = timeSinceLastShow.Value.AddDays(1.0);
			DateTime utcNow = DateTime.UtcNow;
			return t >= utcNow;
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x00118F28 File Offset: 0x00117128
		private bool GachaRewardedVideoButtonIsEnabled()
		{
			bool canGetGift = GiftController.Instance.CanGetGift;
			if (canGetGift)
			{
				return false;
			}
			string reasonToDismissVideoFreeSpin = GachaRewardedVideoController.GetReasonToDismissVideoFreeSpin();
			if (string.IsNullOrEmpty(reasonToDismissVideoFreeSpin))
			{
				return true;
			}
			string format = (!Application.isEditor) ? "GachaRewardedVideoButtonIsEnabled(): false. {0}" : "<color=magenta>GachaRewardedVideoButtonIsEnabled(): false. {0}</color>";
			Debug.LogFormat(format, new object[]
			{
				reasonToDismissVideoFreeSpin
			});
			return false;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x00118F88 File Offset: 0x00117188
		private void OnEnterIdle(object sender, FinishedEventArgs e)
		{
			bool succeeded = e.Succeeded;
			if (Application.isEditor)
			{
				Debug.LogFormat("<color=magenta>OnEnterIdle: {0}</color>", new object[]
				{
					succeeded
				});
			}
			if (this.skinCamera != null)
			{
				this.skinCamera.SetActive(true);
			}
			this.gachaRewardedVideoButton.SetActive(this.GachaRewardedVideoButtonIsEnabled());
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x00118FF0 File Offset: 0x001171F0
		private void OnExitIdle(object sender, EventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=magenta>OnExitIdle</color>");
			}
			if (this.skinCamera != null)
			{
				this.skinCamera.SetActive(false);
			}
			this.gachaRewardedVideoButton.SetActive(this.GachaRewardedVideoButtonIsEnabled());
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00119040 File Offset: 0x00117240
		private void OnAdWatchedSuccessfully(object sender, EventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=magenta>OnAdWatchedSuccessfully</color>");
			}
			GiftController.Instance.IncrementFreeSpins();
			GiftController.Instance.ReCreateSlots();
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003670 RID: 13936 RVA: 0x0011906C File Offset: 0x0011726C
		private GachaRewardedVideo GachaRewardedVideo
		{
			get
			{
				if (this._gachaRewardedVideo == null)
				{
					GachaRewardedVideo gachaRewardedVideo = Resources.Load<GachaRewardedVideo>("GachaRewardedVideo");
					if (gachaRewardedVideo == null)
					{
						Debug.LogWarning("gachaRewardedVideoPrefab is null.");
						return null;
					}
					this._gachaRewardedVideo = UnityEngine.Object.Instantiate<GachaRewardedVideo>(gachaRewardedVideo);
					if (this._gachaRewardedVideo == null)
					{
						Debug.LogWarning("gachaRewardedVideo is null.");
						return null;
					}
					this._gachaRewardedVideo.transform.SetParent(this.gachaRewardedVideoParent.transform);
					this._gachaRewardedVideo.transform.localPosition = Vector3.zero;
					this._gachaRewardedVideo.transform.localScale = Vector3.one;
					this._gachaRewardedVideo.EnterIdle += this.OnEnterIdle;
					this._gachaRewardedVideo.ExitIdle += this.OnExitIdle;
					this._gachaRewardedVideo.AdWatchedSuccessfully += this.OnAdWatchedSuccessfully;
				}
				return this._gachaRewardedVideo;
			}
		}

		// Token: 0x040027E8 RID: 10216
		[SerializeField]
		private GameObject gachaRewardedVideoParent;

		// Token: 0x040027E9 RID: 10217
		[SerializeField]
		private GameObject gachaRewardedVideoButton;

		// Token: 0x040027EA RID: 10218
		[SerializeField]
		private GameObject skinCamera;

		// Token: 0x040027EB RID: 10219
		private GachaRewardedVideo _gachaRewardedVideo;

		// Token: 0x040027EC RID: 10220
		private static GachaRewardedVideoController s_instance;
	}
}
