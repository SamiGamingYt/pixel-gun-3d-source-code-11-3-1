using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.NullExtensions;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005EF RID: 1519
	public class Nest : MonoBehaviour
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060033FE RID: 13310 RVA: 0x0010D214 File Offset: 0x0010B414
		public static long CurrentTime
		{
			get
			{
				return FriendsController.ServerTime;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060033FF RID: 13311 RVA: 0x0010D21C File Offset: 0x0010B41C
		private long TimerInterval
		{
			get
			{
				return (Nest.timerIntervalDelays.Count <= this.DropCounter) ? Nest.timerIntervalDelays.Last<long>() : Nest.timerIntervalDelays[this.DropCounter];
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003400 RID: 13312 RVA: 0x0010D260 File Offset: 0x0010B460
		// (set) Token: 0x06003401 RID: 13313 RVA: 0x0010D2F8 File Offset: 0x0010B4F8
		private int DropCounter
		{
			get
			{
				if (this._dropCounter.Value < Nest.timerIntervalDelays.Count - 1 && PlayerPrefs.GetInt("nest_first_egg_getted", 0) > 0)
				{
					this._dropCounter = Nest.timerIntervalDelays.Count - 1;
					PlayerPrefs.SetInt("nest_dropped_eggs_counter", this._dropCounter.Value);
				}
				if (this._dropCounter.Value < 0)
				{
					this._dropCounter = PlayerPrefs.GetInt("nest_dropped_eggs_counter", 0);
				}
				return this._dropCounter.Value;
			}
			set
			{
				this._dropCounter = value;
				PlayerPrefs.SetInt("nest_dropped_eggs_counter", this._dropCounter.Value);
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003402 RID: 13314 RVA: 0x0010D31C File Offset: 0x0010B51C
		public bool BannerIsVisible
		{
			get
			{
				return this._banner != null && this._banner.HasValue && this._banner.Value.IsVisible;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x0010D358 File Offset: 0x0010B558
		internal bool EggIsReady
		{
			get
			{
				return this.DropCounter == 0 || (Nest.CurrentTime > 0L && this._startWaitTime.Value > 0L && this._startWaitTime.Value + this.TimerInterval <= Nest.CurrentTime);
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003404 RID: 13316 RVA: 0x0010D3B0 File Offset: 0x0010B5B0
		public long? TimeLeft
		{
			get
			{
				if (Nest.CurrentTime < 1L || this._startWaitTime.Value < 1L)
				{
					return null;
				}
				return new long?(this._startWaitTime.Value + this.TimerInterval - Nest.CurrentTime);
			}
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x0010D404 File Offset: 0x0010B604
		public bool NestCanShow()
		{
			return Nest.CurrentTime >= 1L && TrainingController.TrainingCompleted && ExperienceController.sharedController.currentLevel >= 2 && (!(MainMenuController.sharedController != null) || (!MainMenuController.sharedController.SettingsJoysticksPanel.activeInHierarchy && !MainMenuController.sharedController.settingsPanel.activeInHierarchy && !MainMenuController.sharedController.FreePanelIsActive && !MainMenuController.sharedController.singleModePanel.activeInHierarchy)) && (!(FeedbackMenuController.Instance != null) || !FeedbackMenuController.Instance.gameObject.activeInHierarchy);
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0010D4C0 File Offset: 0x0010B6C0
		private void Awake()
		{
			Nest.Instance = this;
			base.StartCoroutine(this.WaitMainMenu());
			this._startWaitTime.Value = (long)PlayerPrefs.GetFloat("nest_start_wait_at");
			this._animationHandler.OnAnimationEvent += this._animationHandler_OnAnimationEvent;
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x0010D510 File Offset: 0x0010B710
		private IEnumerator WaitMainMenu()
		{
			while (MainMenuController.sharedController == null)
			{
				yield return null;
			}
			this._banner = new LazyObject<NestBanner>(this._bannerPrefab, MainMenuController.sharedController.gameObject);
			MainMenuController.sharedController.EggChestBindedBillboard.BindTo(() => this.NestGO.transform);
			yield break;
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x0010D52C File Offset: 0x0010B72C
		private void Update()
		{
			if (!this.NestCanShow())
			{
				this.NestGO.SetActiveSafe(false);
				if (this._nickLabelController != null)
				{
					this._nickLabelController.gameObject.SetActiveSafe(false);
				}
				if (this._banner != null && this._banner.HasValue)
				{
					this._banner.Value.EnableTouchBlocker(false);
				}
				return;
			}
			this.NestGO.SetActiveSafe(true);
			if (this._nickLabelController != null)
			{
				this._nickLabelController.gameObject.SetActiveSafe(this._nickLabelControllerVisible);
			}
			if (this.DropCounter == 0)
			{
				this._animator.SetBool("IsEnabled", this.EggIsReady);
				this.ShowLobbyHeader(false, 0L);
			}
			else
			{
				if (this._startWaitTime.Value < 1L)
				{
					this.ResetTimer();
				}
				this._animator.SetBool("IsEnabled", this.EggIsReady);
				this.ShowLobbyHeader(true, this.TimeLeft.Value);
			}
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x0010D644 File Offset: 0x0010B844
		public void Click()
		{
			if (MainMenuController.sharedController != null && MainMenuController.sharedController.LeaderboardsIsOpening)
			{
				return;
			}
			this.GetEgg();
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x0010D678 File Offset: 0x0010B878
		private void GetEgg()
		{
			if (!this.EggIsReady || !this.NestGO.activeInHierarchy || this._getEggProcessed)
			{
				return;
			}
			this._getEggProcessed = true;
			this.SetMenuInteractionEnabled(false);
			this._animator.SetBool("IsOpen", true);
		}

		// Token: 0x0600340B RID: 13323 RVA: 0x0010D6CC File Offset: 0x0010B8CC
		private void _animationHandler_OnAnimationEvent(string animName, AnimationHandler.AnimState animState)
		{
			if (animName == "Open" && animState == AnimationHandler.AnimState.Finished && this._getEggProcessed)
			{
				this.DropEgg();
			}
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x0010D704 File Offset: 0x0010B904
		private void DropEgg()
		{
			if (!this.EggIsReady)
			{
				this.OnBannerClose();
				return;
			}
			Egg egg;
			if (this.DropCounter == 0)
			{
				EggData data = Singleton<EggsManager>.Instance.GetAllEggs().FirstOrDefault((EggData e) => e.Id == "egg_simple_rating");
				egg = Singleton<EggsManager>.Instance.AddEgg(data);
			}
			else
			{
				egg = Singleton<EggsManager>.Instance.AddRandomEgg();
			}
			if (egg != null && egg.Data != null)
			{
				AnalyticsStuff.LogEggDelivery(egg.Data.Id);
			}
			this.ResetTimer();
			this._banner.Value.OnClose += this.OnBannerClose;
			this._banner.Value.Show(egg);
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		private void OnBannerClose()
		{
			this._banner.Value.OnClose -= this.OnBannerClose;
			this._animator.SetBool("IsOpen", false);
			this._animator.SetBool("IsEnabled", false);
			this.SetMenuInteractionEnabled(true);
			this._getEggProcessed = false;
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x0010D82C File Offset: 0x0010BA2C
		private void ResetTimer()
		{
			if (Nest.CurrentTime < 1L)
			{
				return;
			}
			this._startWaitTime.Value = Nest.CurrentTime;
			PlayerPrefs.SetFloat("nest_start_wait_at", (float)this._startWaitTime.Value);
			this.DropCounter++;
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x0010D87C File Offset: 0x0010BA7C
		private void SetMenuInteractionEnabled(bool enabled)
		{
			if (!enabled)
			{
				this._banner.Value.EnableTouchBlocker(true);
				if (FreeAwardShowHandler.Instance != null)
				{
					FreeAwardShowHandler.Instance.IsInteractable = false;
				}
			}
			else
			{
				this._banner.Value.EnableTouchBlocker(false);
				if (FreeAwardShowHandler.Instance != null)
				{
					FreeAwardShowHandler.Instance.IsInteractable = true;
				}
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003410 RID: 13328 RVA: 0x0010D8EC File Offset: 0x0010BAEC
		private NickLabelController _nickLabelController
		{
			get
			{
				if (this._nickLabelControllerValue == null)
				{
					if (NickLabelStack.sharedStack != null)
					{
						this._nickLabelControllerValue = NickLabelStack.sharedStack.GetNextCurrentLabel();
					}
					if (this._nickLabelControllerValue != null)
					{
						this._nickLabelControllerValue.StartShow(NickLabelController.TypeNickLabel.Nest, this.NestGO.transform);
					}
				}
				return this._nickLabelControllerValue;
			}
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x0010D958 File Offset: 0x0010BB58
		private void ShowLobbyHeader(bool visible, long timeLeft)
		{
			if (this._nickLabelController == null)
			{
				return;
			}
			if (this._getEggProcessed)
			{
				visible = false;
			}
			this._nickLabelController.NestTimerLabel.gameObject.SetActiveSafe(visible && timeLeft > 0L);
			this._nickLabelController.NestGO.transform.localPosition = ((timeLeft > 0L) ? this._nickLabelController.NestLabelPos : this._nickLabelController.NestLabelPosWithoutTimer);
			if (this._nickLabelController.NestGO.transform.localPosition != this._prevPos)
			{
				this._nickLabelController.GetComponent<UIPanel>().Do(delegate(UIPanel p)
				{
					p.SetDirty();
					p.Refresh();
				});
			}
			this._prevPos = this._nickLabelController.NestGO.transform.localPosition;
			if (!visible)
			{
				return;
			}
			this._nickLabelController.NestTimerLabel.text = RiliExtensions.GetTimeString(timeLeft, ":");
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x0010DA70 File Offset: 0x0010BC70
		public void SetNickLabelVisible(bool isVisible)
		{
			if (this._nickLabelController == null)
			{
				return;
			}
			this._nickLabelControllerVisible = isVisible;
			this._nickLabelController.gameObject.SetActive(isVisible);
		}

		// Token: 0x06003413 RID: 13331 RVA: 0x0010DAA8 File Offset: 0x0010BCA8
		private void OnApplicationPause(bool pauseStatus)
		{
			this.SetMenuInteractionEnabled(true);
			this._getEggProcessed = false;
		}

		// Token: 0x04002639 RID: 9785
		private const string KEY_DROPPED_EGGS_COUNTER = "nest_dropped_eggs_counter";

		// Token: 0x0400263A RID: 9786
		private const string KEY_START_WAIT_AT = "nest_start_wait_at";

		// Token: 0x0400263B RID: 9787
		private const string AP_IS_ENABLED = "IsEnabled";

		// Token: 0x0400263C RID: 9788
		private const string AP_IS_OPEN = "IsOpen";

		// Token: 0x0400263D RID: 9789
		public static Nest Instance;

		// Token: 0x0400263E RID: 9790
		public static List<long> timerIntervalDelays = new List<long>
		{
			0L,
			900L,
			900L,
			1800L,
			1800L,
			3600L,
			3600L,
			7200L,
			7200L,
			14400L,
			14400L,
			21600L,
			21600L,
			43200L
		};

		// Token: 0x0400263F RID: 9791
		private SaltedLong _startWaitTime = new SaltedLong(187649984473770L);

		// Token: 0x04002640 RID: 9792
		private SaltedInt _dropCounter = new SaltedInt(178956970, -1);

		// Token: 0x04002641 RID: 9793
		[SerializeField]
		private GameObject NestGO;

		// Token: 0x04002642 RID: 9794
		[SerializeField]
		private Animator _animator;

		// Token: 0x04002643 RID: 9795
		[SerializeField]
		private AnimationHandler _animationHandler;

		// Token: 0x04002644 RID: 9796
		[SerializeField]
		private GameObject _bannerPrefab;

		// Token: 0x04002645 RID: 9797
		private LazyObject<NestBanner> _banner;

		// Token: 0x04002646 RID: 9798
		private bool _getEggProcessed;

		// Token: 0x04002647 RID: 9799
		private NickLabelController _nickLabelControllerValue;

		// Token: 0x04002648 RID: 9800
		private Vector3 _prevPos = Vector3.zero;

		// Token: 0x04002649 RID: 9801
		private bool _nickLabelControllerVisible = true;
	}
}
