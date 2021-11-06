using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020007EC RID: 2028
internal sealed class DeveloperConsoleController : MonoBehaviour
{
	// Token: 0x0600494B RID: 18763 RVA: 0x00197378 File Offset: 0x00195578
	public void HandleInvalidateQuestConfig(UILabel label)
	{
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x0019737C File Offset: 0x0019557C
	public void HandleFacebookLoginReward(UIToggle toggle)
	{
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x00197380 File Offset: 0x00195580
	public void HandleBackButton()
	{
		this._backRequested = true;
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x0019738C File Offset: 0x0019558C
	public void HandleClearKeychainAndPlayerPrefs()
	{
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x00197390 File Offset: 0x00195590
	public void HandleLevelMinusButton()
	{
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x00197394 File Offset: 0x00195594
	public void HandleTipsShownButton()
	{
	}

	// Token: 0x06004951 RID: 18769 RVA: 0x00197398 File Offset: 0x00195598
	public void HandleAddGemsButton()
	{
	}

	// Token: 0x06004952 RID: 18770 RVA: 0x0019739C File Offset: 0x0019559C
	public void HandleAddCoinsButton()
	{
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x001973A0 File Offset: 0x001955A0
	public void HandleLevelPlusButton()
	{
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x001973A4 File Offset: 0x001955A4
	public void HandleLevelChanged()
	{
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x001973A8 File Offset: 0x001955A8
	public void HandleLevelSliderChanged()
	{
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x001973AC File Offset: 0x001955AC
	public void HandleCoinsInputSubmit(UIInput input)
	{
		if (!input.isActiveAndEnabled)
		{
			return;
		}
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x001973BC File Offset: 0x001955BC
	public void HandleEnemyCountInSurvivalWaveInput(UIInput input)
	{
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x001973C0 File Offset: 0x001955C0
	public void HandleEnemiesInCampaignChange()
	{
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x001973C4 File Offset: 0x001955C4
	public void HandleDayChange(UIInput input)
	{
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x001973C8 File Offset: 0x001955C8
	public void HandleDaySubmit(UIInput input)
	{
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x001973CC File Offset: 0x001955CC
	public void HandleEnemiesInCampaignInput(UIInput input)
	{
	}

	// Token: 0x0600495C RID: 18780 RVA: 0x001973D0 File Offset: 0x001955D0
	public void HandleTrainingCompleteChanged(UIToggle toggle)
	{
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x001973D4 File Offset: 0x001955D4
	public void HandleStrongDeviceChanged(UIToggle toggle)
	{
	}

	// Token: 0x0600495E RID: 18782 RVA: 0x001973D8 File Offset: 0x001955D8
	public void HandleSet60FpsChanged(UIToggle toggle)
	{
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x001973DC File Offset: 0x001955DC
	public void HandleMouseControlChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004960 RID: 18784 RVA: 0x001973E0 File Offset: 0x001955E0
	public void HandleSpectatorMode(UIToggle toggle)
	{
	}

	// Token: 0x06004961 RID: 18785 RVA: 0x001973E4 File Offset: 0x001955E4
	public void HandleTempGunChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004962 RID: 18786 RVA: 0x001973E8 File Offset: 0x001955E8
	public void HandleIpadMiniRetinaChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004963 RID: 18787 RVA: 0x001973EC File Offset: 0x001955EC
	public void HandleIsPayingChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004964 RID: 18788 RVA: 0x001973F0 File Offset: 0x001955F0
	public void HandleIsDebugGuiVisibleChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x001973F4 File Offset: 0x001955F4
	public void HandleServerTimeChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004966 RID: 18790 RVA: 0x001973F8 File Offset: 0x001955F8
	public void HandleAreLogsEnabledChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x001973FC File Offset: 0x001955FC
	public void HandleIsPixelGunLowChanged(UIToggle toggle)
	{
	}

	// Token: 0x06004968 RID: 18792 RVA: 0x00197400 File Offset: 0x00195600
	public void HandleForcedEventX3Changed(UIToggle toggle)
	{
	}

	// Token: 0x06004969 RID: 18793 RVA: 0x00197404 File Offset: 0x00195604
	public void HandleAdIdCanged(UIToggle toggle)
	{
	}

	// Token: 0x0600496A RID: 18794 RVA: 0x00197408 File Offset: 0x00195608
	private static void SetItemsBought(bool bought, bool onlyGuns = true)
	{
	}

	// Token: 0x0600496B RID: 18795 RVA: 0x0019740C File Offset: 0x0019560C
	public void HandleAllPets()
	{
		foreach (string petId in PetsInfo.info.Keys)
		{
			for (int i = 0; i < 15; i++)
			{
				Singleton<PetsManager>.Instance.AddOrUpdatePet(petId);
			}
		}
	}

	// Token: 0x0600496C RID: 18796 RVA: 0x00197490 File Offset: 0x00195690
	public void HandleAllAchievements()
	{
	}

	// Token: 0x0600496D RID: 18797 RVA: 0x00197494 File Offset: 0x00195694
	public void HandleFillGunsButton()
	{
	}

	// Token: 0x0600496E RID: 18798 RVA: 0x00197498 File Offset: 0x00195698
	public void HandleClearPurchasesButton()
	{
	}

	// Token: 0x0600496F RID: 18799 RVA: 0x0019749C File Offset: 0x0019569C
	public void HandleClearProgressButton()
	{
	}

	// Token: 0x06004970 RID: 18800 RVA: 0x001974A0 File Offset: 0x001956A0
	public void HandleFillProgressButton()
	{
	}

	// Token: 0x06004971 RID: 18801 RVA: 0x001974A4 File Offset: 0x001956A4
	public void HandleClearCloud()
	{
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x001974A8 File Offset: 0x001956A8
	public void HandleUnbanUs(UIButton butt)
	{
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x001974AC File Offset: 0x001956AC
	public void HandleClearX3()
	{
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x001974B0 File Offset: 0x001956B0
	private void RefreshRating(bool current)
	{
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x001974B4 File Offset: 0x001956B4
	private void RefreshExperience()
	{
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x001974B8 File Offset: 0x001956B8
	private void RefreshLevel()
	{
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x001974BC File Offset: 0x001956BC
	private void RefreshLevelSlider()
	{
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x001974C0 File Offset: 0x001956C0
	public void HandleExperienceSliderChanged()
	{
	}

	// Token: 0x06004979 RID: 18809 RVA: 0x001974C4 File Offset: 0x001956C4
	public void HandleRatingSliderChanged()
	{
	}

	// Token: 0x0600497A RID: 18810 RVA: 0x001974C8 File Offset: 0x001956C8
	public void HandleSignInOuButton(UILabel socialUsernameLabel)
	{
	}

	// Token: 0x0600497B RID: 18811 RVA: 0x001974CC File Offset: 0x001956CC
	public void SetMarathonTestMode(UIToggle toggle)
	{
	}

	// Token: 0x0600497C RID: 18812 RVA: 0x001974D0 File Offset: 0x001956D0
	public void SetMarathonCurrentDay(UIInput input)
	{
	}

	// Token: 0x0600497D RID: 18813 RVA: 0x001974D4 File Offset: 0x001956D4
	public void SetOffGameGUIMode(UIToggle toggle)
	{
	}

	// Token: 0x0600497E RID: 18814 RVA: 0x001974D8 File Offset: 0x001956D8
	public void ClearStarterPackData()
	{
	}

	// Token: 0x0600497F RID: 18815 RVA: 0x001974DC File Offset: 0x001956DC
	private void Refresh()
	{
	}

	// Token: 0x06004980 RID: 18816 RVA: 0x001974E0 File Offset: 0x001956E0
	private void Awake()
	{
		DeveloperConsoleController.instance = this;
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x001974E8 File Offset: 0x001956E8
	private void OnDestroy()
	{
		DeveloperConsoleController.instance = null;
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x001974F0 File Offset: 0x001956F0
	private IEnumerator Start()
	{
		yield break;
	}

	// Token: 0x06004983 RID: 18819 RVA: 0x00197504 File Offset: 0x00195704
	public void ChangePremiumAccountLiveTime(UIInput input)
	{
	}

	// Token: 0x06004984 RID: 18820 RVA: 0x00197508 File Offset: 0x00195708
	public void ClearAllPremiumAccounts()
	{
	}

	// Token: 0x06004985 RID: 18821 RVA: 0x0019750C File Offset: 0x0019570C
	public void ClearCurrentPremiumAccont()
	{
	}

	// Token: 0x06004986 RID: 18822 RVA: 0x00197510 File Offset: 0x00195710
	private void HandleGemsInputSubmit(UIInput input)
	{
		if (!input.isActiveAndEnabled)
		{
			return;
		}
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x00197520 File Offset: 0x00195720
	private void Update()
	{
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x00197524 File Offset: 0x00195724
	private void OnEnable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
		}
		this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "DevConsole");
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x00197560 File Offset: 0x00195760
	private void OnDisable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
			this._escapeSubscription = null;
		}
	}

	// Token: 0x0600498A RID: 18826 RVA: 0x00197580 File Offset: 0x00195780
	private void HandleEscape()
	{
		this._backRequested = true;
	}

	// Token: 0x0600498B RID: 18827 RVA: 0x0019758C File Offset: 0x0019578C
	public void OnChangeStarterPackLive(UIInput inputField)
	{
	}

	// Token: 0x0600498C RID: 18828 RVA: 0x00197590 File Offset: 0x00195790
	public void OnChangeStarterPackCooldown(UIInput inputField)
	{
	}

	// Token: 0x0600498D RID: 18829 RVA: 0x00197594 File Offset: 0x00195794
	public void UpdateStateActiveMemoryInfo()
	{
	}

	// Token: 0x0600498E RID: 18830 RVA: 0x00197598 File Offset: 0x00195798
	public void OnChangeStateMemoryInfo()
	{
	}

	// Token: 0x0600498F RID: 18831 RVA: 0x0019759C File Offset: 0x0019579C
	public void OnChangeReviewActive()
	{
	}

	// Token: 0x06004990 RID: 18832 RVA: 0x001975A0 File Offset: 0x001957A0
	public void OnClickSystemBuff()
	{
	}

	// Token: 0x06004991 RID: 18833 RVA: 0x001975A4 File Offset: 0x001957A4
	public void OnClickRating()
	{
	}

	// Token: 0x06004992 RID: 18834 RVA: 0x001975A8 File Offset: 0x001957A8
	public void FillAll()
	{
	}

	// Token: 0x04003672 RID: 13938
	public static DeveloperConsoleController instance;

	// Token: 0x04003673 RID: 13939
	public DeveloperConsoleView view;

	// Token: 0x04003674 RID: 13940
	public static bool isDebugGuiVisible;

	// Token: 0x04003675 RID: 13941
	public bool isMiniConsole;

	// Token: 0x04003676 RID: 13942
	private int sliderLevel;

	// Token: 0x04003677 RID: 13943
	private IDisposable _escapeSubscription;

	// Token: 0x04003678 RID: 13944
	public UIToggle buffToogle;

	// Token: 0x04003679 RID: 13945
	public UIToggle ratingToogle;

	// Token: 0x0400367A RID: 13946
	private bool? _enemiesInCampaignDirty;

	// Token: 0x0400367B RID: 13947
	private bool _backRequested;

	// Token: 0x0400367C RID: 13948
	private bool _initialized;

	// Token: 0x0400367D RID: 13949
	private bool _needsRestart;
}
