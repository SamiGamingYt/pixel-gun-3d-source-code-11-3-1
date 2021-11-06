using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000692 RID: 1682
	public class LeprechauntLobbyView : MonoBehaviour
	{
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x06003AC8 RID: 15048 RVA: 0x0012FE18 File Offset: 0x0012E018
		private NickLabelController NickLabel
		{
			get
			{
				if (this._nickLabelValue == null)
				{
					if (NickLabelStack.sharedStack != null)
					{
						this._nickLabelValue = NickLabelStack.sharedStack.GetNextCurrentLabel();
					}
					if (this._nickLabelValue != null)
					{
						this._nickLabelValue.StartShow(NickLabelController.TypeNickLabel.Leprechaunt, this._model.transform);
					}
				}
				return this._nickLabelValue;
			}
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x0012FE88 File Offset: 0x0012E088
		public bool CanShow()
		{
			return (!(Singleton<LeprechauntManager>.Instance != null) || Singleton<LeprechauntManager>.Instance.CurrentTime != null) && TrainingController.TrainingCompleted && ExperienceController.sharedController.currentLevel >= 2 && (!(MainMenuController.sharedController != null) || (!MainMenuController.sharedController.SettingsJoysticksPanel.activeInHierarchy && !MainMenuController.sharedController.settingsPanel.activeInHierarchy && !MainMenuController.sharedController.FreePanelIsActive && !MainMenuController.sharedController.singleModePanel.activeInHierarchy)) && (!(FeedbackMenuController.Instance != null) || !FeedbackMenuController.Instance.gameObject.activeInHierarchy);
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x0012FF5C File Offset: 0x0012E15C
		private void Awake()
		{
			LeprechauntLobbyView.Instance = this;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x0012FF64 File Offset: 0x0012E164
		private void OnApplicationPause(bool pauseStatus)
		{
			this._waitEndRewardAnimation = false;
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x0012FF70 File Offset: 0x0012E170
		private void OnDestroy()
		{
			this._waitEndRewardAnimation = false;
			LeprechauntLobbyView.Instance = null;
		}

		// Token: 0x06003ACD RID: 15053 RVA: 0x0012FF80 File Offset: 0x0012E180
		private void OnEnable()
		{
			this._waitEndRewardAnimation = false;
			this._model.SetActive(Singleton<LeprechauntManager>.Instance.LeprechauntExists);
			base.StartCoroutine(this.WaitMainMenuAndBindBillboard());
			base.StartCoroutine(this.MainLoopCoroutine());
		}

		// Token: 0x06003ACE RID: 15054 RVA: 0x0012FFC4 File Offset: 0x0012E1C4
		private void Update()
		{
			if (this._animator.gameObject.activeInHierarchy && this._animator.GetBool("RewardAvailable") != Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
			{
				this._animator.SetBool("RewardAvailable", Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop);
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x00130020 File Offset: 0x0012E220
		private IEnumerator WaitMainMenuAndBindBillboard()
		{
			while (MainMenuController.sharedController == null)
			{
				yield return null;
			}
			MainMenuController.sharedController.LeprechauntBindedBillboard.BindTo(() => this._model.transform);
			yield break;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0013003C File Offset: 0x0012E23C
		private IEnumerator MainLoopCoroutine()
		{
			for (;;)
			{
				if (!this.CanShow())
				{
					this._model.SetActiveSafe(false);
					this.NickLabel.LeprechauntGO.SetActiveSafe(false);
				}
				else
				{
					this._model.SetActiveSafe(Singleton<LeprechauntManager>.Instance.LeprechauntExists);
					if (Singleton<LeprechauntManager>.Instance.LeprechauntExists && !this._waitEndRewardAnimation)
					{
						this.NickLabel.LeprechauntGO.SetActiveSafe(true);
						if (Singleton<LeprechauntManager>.Instance.LeprechauntLifeTimeLeft != null)
						{
							this.NickLabel.LeprechauntDaysLeft.gameObject.SetActiveSafe(true);
							int daysLeft = Singleton<LeprechauntManager>.Instance.LeprechauntLifeTimeLeft.Value / 3600 / 24 + 1;
							this.NickLabel.LeprechauntDaysLeft.text = string.Format(LocalizationStore.Get("Key_2913"), daysLeft);
						}
						if (Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
						{
							this.NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(true);
							this.NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(false);
							this.NickLabel.LeprechauntGemsRewardAvailable.text = string.Format(LocalizationStore.Get("Key_2914"), Singleton<LeprechauntManager>.Instance.RewardReadyToDrop);
							if (Singleton<LeprechauntManager>.Instance.RewardCurrency == "GemsCurrency")
							{
								this.NickLabel.LeprechauntGemsIcon.SetActiveSafe(true);
								this.NickLabel.LeprechauntCoinsIcon.SetActiveSafe(false);
							}
							else
							{
								this.NickLabel.LeprechauntGemsIcon.SetActiveSafe(false);
								this.NickLabel.LeprechauntCoinsIcon.SetActiveSafe(true);
							}
						}
						else
						{
							this.NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(false);
							this.NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(true);
							this.NickLabel.LeprechauntRewardTimeLeft.text = RiliExtensions.GetTimeString((long)Singleton<LeprechauntManager>.Instance.RewardDropSecsLeft.Value, ":");
						}
					}
					else
					{
						this.NickLabel.LeprechauntDaysLeft.gameObject.SetActiveSafe(false);
						this.NickLabel.LeprechauntRewardTimeLeft.gameObject.SetActiveSafe(false);
						this.NickLabel.LeprechauntGemsRewardAvailableGO.SetActiveSafe(false);
					}
				}
				yield return new WaitForRealSeconds(0.2f);
			}
			yield break;
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x00130058 File Offset: 0x0012E258
		public void Tap()
		{
			if (this._waitEndRewardAnimation)
			{
				return;
			}
			if (Singleton<LeprechauntManager>.Instance.RewardIsReadyToDrop)
			{
				this._waitEndRewardAnimation = true;
				base.StopCoroutine("MainLoopCoroutine");
				this.NickLabel.LeprechauntGO.SetActiveSafe(false);
				this._animator.SetTrigger("GetReward");
			}
			else
			{
				this._animator.SetTrigger("Tap");
			}
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x001300C8 File Offset: 0x0012E2C8
		public void OnAnimatorStateExit(string stateName)
		{
			if (stateName == "Close")
			{
				this._waitEndRewardAnimation = false;
				this._animator.SetBool("RewardAvailable", false);
				Singleton<LeprechauntManager>.Instance.DropReward();
				base.StartCoroutine(this.MainLoopCoroutine());
			}
		}

		// Token: 0x04002B66 RID: 11110
		private const string AnimBoolRewardAvailable = "RewardAvailable";

		// Token: 0x04002B67 RID: 11111
		private const string AnimTriggerGetReward = "GetReward";

		// Token: 0x04002B68 RID: 11112
		private const string AnimTriggerTap = "Tap";

		// Token: 0x04002B69 RID: 11113
		public static LeprechauntLobbyView Instance;

		// Token: 0x04002B6A RID: 11114
		[SerializeField]
		private GameObject _model;

		// Token: 0x04002B6B RID: 11115
		[SerializeField]
		private Animator _animator;

		// Token: 0x04002B6C RID: 11116
		private NickLabelController _nickLabelValue;

		// Token: 0x04002B6D RID: 11117
		private bool _waitEndRewardAnimation;
	}
}
