using System;
using System.Collections.Generic;
using System.Globalization;
using I2.Loc;
using Rilisoft;
using UnityEngine;

// Token: 0x0200060D RID: 1549
internal sealed class FreeAwardView : MonoBehaviour
{
	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x0600350C RID: 13580 RVA: 0x00112408 File Offset: 0x00110608
	// (set) Token: 0x0600350D RID: 13581 RVA: 0x00112410 File Offset: 0x00110610
	internal FreeAwardController.State CurrentState
	{
		private get
		{
			return this._currentState;
		}
		set
		{
			if (value != this._currentState)
			{
				FreeAwardController.WatchState watchState = value as FreeAwardController.WatchState;
				if (watchState != null)
				{
					TimeSpan estimatedTimeSpan = watchState.GetEstimatedTimeSpan();
					bool enabled = estimatedTimeSpan <= TimeSpan.FromMinutes(0.0);
					this.SetWatchButtonEnabled(enabled, estimatedTimeSpan);
				}
				else
				{
					this.SetWatchButtonEnabled(false);
				}
				this.RefreshAwardLabel(watchState != null);
			}
			if (this.backgroundPanel != null)
			{
				this.backgroundPanel.SetActive(!(value is FreeAwardController.IdleState));
			}
			bool flag = value is FreeAwardController.WaitingState;
			if (FreeAwardController.Instance.SimplifiedInterface)
			{
				if (this.waitingPanel != null)
				{
					this.waitingPanel.SetActive(false);
				}
				if ((this._currentState is FreeAwardController.WaitingState || value is FreeAwardController.WaitingState) && ActivityIndicator.IsActiveIndicator != flag)
				{
					ActivityIndicator.IsActiveIndicator = flag;
				}
			}
			else
			{
				if (this.waitingPanel != null)
				{
					this.waitingPanel.SetActive(flag);
				}
				if (this._currentState is FreeAwardController.WaitingState && !(value is FreeAwardController.WaitingState) && ActivityIndicator.IsActiveIndicator)
				{
					ActivityIndicator.IsActiveIndicator = false;
				}
			}
			if (this.connectionPanel != null)
			{
				this.connectionPanel.SetActive(value is FreeAwardController.ConnectionState);
			}
			if (this.closePanel != null)
			{
				this.closePanel.SetActive(value is FreeAwardController.CloseState);
			}
			if (value is FreeAwardController.WatchState)
			{
				if (FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")
				{
					this.watchForGemsPanel.gameObject.SetActive(true);
					this.watchForCoinsPanel.gameObject.SetActive(false);
				}
				else
				{
					this.watchForGemsPanel.gameObject.SetActive(false);
					this.watchForCoinsPanel.gameObject.SetActive(true);
				}
			}
			else
			{
				this.watchForGemsPanel.gameObject.SetActive(false);
				this.watchForCoinsPanel.gameObject.SetActive(false);
			}
			if (value is FreeAwardController.AwardState)
			{
				if (FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")
				{
					this.awardPanelGems.SetActive(true);
				}
				else
				{
					this.awardPanelCoins.SetActive(true);
				}
			}
			else
			{
				this.awardPanelCoins.SetActive(false);
				this.awardPanelGems.SetActive(false);
			}
			this._currentState = value;
		}
	}

	// Token: 0x0600350E RID: 13582 RVA: 0x00112690 File Offset: 0x00110890
	private void RefreshAwardLabel(bool visible)
	{
		if (!visible)
		{
			return;
		}
		string text = LocalizationStore.Get(ScriptLocalization.Key_0291);
		int countMoneyForAward = FreeAwardController.CountMoneyForAward;
		text += ((!(FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")) ? countMoneyForAward.ToString(CultureInfo.InvariantCulture) : string.Format(CultureInfo.InvariantCulture, " [c][50CEFFFF]{0}[-][/c]", new object[]
		{
			countMoneyForAward
		}));
		List<UILabel> list = new List<UILabel>();
		list.AddRange(this.awardOuterLabelCoins.gameObject.GetComponentsInChildren<UILabel>(true));
		list.AddRange(this.awardOuterLabelGems.gameObject.GetComponentsInChildren<UILabel>(true));
		foreach (UILabel uilabel in list)
		{
			uilabel.text = text;
		}
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x00112790 File Offset: 0x00110990
	private void Start()
	{
		if (this.devSkipButton != null)
		{
			this.devSkipButton.gameObject.SetActive(Application.isEditor || (Defs.IsDeveloperBuild && BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64));
		}
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x001127E4 File Offset: 0x001109E4
	private void Update()
	{
		FreeAwardController.WaitingState waitingState = this.CurrentState as FreeAwardController.WaitingState;
		if (waitingState != null)
		{
			if (Application.isEditor && Input.GetKeyUp(KeyCode.S))
			{
				FreeAwardController.Instance.HandleDeveloperSkip();
			}
			if (this.loadingSpinner != null)
			{
				float num = Time.realtimeSinceStartup - waitingState.StartTime;
				int num2 = Mathf.FloorToInt(num);
				this.loadingSpinner.invert = (num2 % 2 == 0);
				this.loadingSpinner.fillAmount = ((!this.loadingSpinner.invert) ? (1f - num + (float)num2) : (num - (float)num2));
			}
		}
		FreeAwardController.WatchState watchState = this.CurrentState as FreeAwardController.WatchState;
		if (watchState != null && Time.frameCount % 10 == 0)
		{
			TimeSpan estimatedTimeSpan = watchState.GetEstimatedTimeSpan();
			bool enabled = estimatedTimeSpan <= TimeSpan.FromMinutes(0.0);
			this.SetWatchButtonEnabled(enabled, estimatedTimeSpan);
		}
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x001128D0 File Offset: 0x00110AD0
	private void SetWatchButtonEnabled(bool enabled, TimeSpan nextTimeAwailable)
	{
		CurrencySpecificWatchPanel currencySpecificWatchPanel = (!(FreeAwardController.Instance.CurrencyForAward == "GemsCurrency")) ? this.watchForCoinsPanel : this.watchForGemsPanel;
		if (currencySpecificWatchPanel == null)
		{
			Debug.LogWarning("panel == null");
			return;
		}
		if (currencySpecificWatchPanel.WatchButton != null)
		{
			currencySpecificWatchPanel.WatchButton.isEnabled = enabled;
		}
		if (currencySpecificWatchPanel.WatchHeader != null)
		{
			currencySpecificWatchPanel.WatchHeader.gameObject.SetActive(enabled);
		}
		if (currencySpecificWatchPanel.WatchTimer != null)
		{
			currencySpecificWatchPanel.WatchTimer.transform.parent.gameObject.SetActive(!enabled);
			if (!enabled)
			{
				string text = (nextTimeAwailable.Hours <= 0) ? string.Format("{0}:{1:D2}", nextTimeAwailable.Minutes, nextTimeAwailable.Seconds) : string.Format("{0}:{1:D2}:{2:D2}", nextTimeAwailable.Hours, nextTimeAwailable.Minutes, nextTimeAwailable.Seconds);
				foreach (UILabel uilabel in this.GetWatchTimerLabels(currencySpecificWatchPanel.WatchTimer))
				{
					uilabel.text = text;
				}
			}
		}
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x00112A58 File Offset: 0x00110C58
	private void SetWatchButtonEnabled(bool enabled)
	{
		this.SetWatchButtonEnabled(enabled, default(TimeSpan));
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x00112A78 File Offset: 0x00110C78
	private List<UILabel> GetWatchTimerLabels(UILabel rootLabel)
	{
		List<UILabel> result = new List<UILabel>(3)
		{
			rootLabel
		};
		rootLabel.GetComponentsInChildren<UILabel>(true, result);
		return result;
	}

	// Token: 0x040026E7 RID: 9959
	public GameObject backgroundPanel;

	// Token: 0x040026E8 RID: 9960
	public GameObject waitingPanel;

	// Token: 0x040026E9 RID: 9961
	public CurrencySpecificWatchPanel watchForCoinsPanel;

	// Token: 0x040026EA RID: 9962
	public CurrencySpecificWatchPanel watchForGemsPanel;

	// Token: 0x040026EB RID: 9963
	public GameObject connectionPanel;

	// Token: 0x040026EC RID: 9964
	public GameObject awardPanelCoins;

	// Token: 0x040026ED RID: 9965
	public GameObject awardPanelGems;

	// Token: 0x040026EE RID: 9966
	public GameObject closePanel;

	// Token: 0x040026EF RID: 9967
	public UIButton devSkipButton;

	// Token: 0x040026F0 RID: 9968
	public UITexture loadingSpinner;

	// Token: 0x040026F1 RID: 9969
	public UILabel awardOuterLabelCoins;

	// Token: 0x040026F2 RID: 9970
	public UILabel awardOuterLabelGems;

	// Token: 0x040026F3 RID: 9971
	private FreeAwardController.State _currentState;
}
