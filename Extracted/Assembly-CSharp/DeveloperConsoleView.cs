using System;
using System.Globalization;
using UnityEngine;

// Token: 0x020007ED RID: 2029
internal sealed class DeveloperConsoleView : MonoBehaviour
{
	// Token: 0x06004994 RID: 18836 RVA: 0x001975B4 File Offset: 0x001957B4
	private void Awake()
	{
		DeveloperConsoleView.instance = this;
		this.diagonalInfo.text = "Диагональ: " + Defs.ScreenDiagonal;
	}

	// Token: 0x06004995 RID: 18837 RVA: 0x001975DC File Offset: 0x001957DC
	private void OnDestroy()
	{
		DeveloperConsoleView.instance = null;
	}

	// Token: 0x17000BF9 RID: 3065
	// (set) Token: 0x06004996 RID: 18838 RVA: 0x001975E4 File Offset: 0x001957E4
	public string LevelLabel
	{
		set
		{
			if (this.levelLabel == null)
			{
				return;
			}
			this.levelLabel.text = value;
		}
	}

	// Token: 0x17000BFA RID: 3066
	// (set) Token: 0x06004997 RID: 18839 RVA: 0x00197604 File Offset: 0x00195804
	public string ExperienceLabel
	{
		set
		{
			if (this.experienceLabel == null)
			{
				return;
			}
			this.experienceLabel.text = value;
		}
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x06004998 RID: 18840 RVA: 0x00197624 File Offset: 0x00195824
	// (set) Token: 0x06004999 RID: 18841 RVA: 0x00197658 File Offset: 0x00195858
	public float ExperiencePercentage
	{
		get
		{
			return (!(this.experienceSlider != null)) ? 0f : this.experienceSlider.value;
		}
		set
		{
			if (this.experienceSlider == null)
			{
				return;
			}
			this.experienceSlider.value = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x0600499A RID: 18842 RVA: 0x00197680 File Offset: 0x00195880
	// (set) Token: 0x0600499B RID: 18843 RVA: 0x001976B4 File Offset: 0x001958B4
	public float LevelPercentage
	{
		get
		{
			return (!(this.levelSlider != null)) ? 0f : this.levelSlider.value;
		}
		set
		{
			if (this.levelSlider == null)
			{
				return;
			}
			this.levelSlider.value = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000BFD RID: 3069
	// (set) Token: 0x0600499C RID: 18844 RVA: 0x001976DC File Offset: 0x001958DC
	public string RatingLabel
	{
		set
		{
			if (this.ratingLabel == null)
			{
				return;
			}
			this.ratingLabel.text = value;
		}
	}

	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x0600499D RID: 18845 RVA: 0x001976FC File Offset: 0x001958FC
	// (set) Token: 0x0600499E RID: 18846 RVA: 0x00197730 File Offset: 0x00195930
	public float RatingPercentage
	{
		get
		{
			return (!(this.ratingSlider != null)) ? 0f : this.ratingSlider.value;
		}
		set
		{
			if (this.ratingSlider == null)
			{
				return;
			}
			this.ratingSlider.value = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000BFF RID: 3071
	// (set) Token: 0x0600499F RID: 18847 RVA: 0x00197758 File Offset: 0x00195958
	public int CoinsInput
	{
		set
		{
			if (this.coinsInput != null)
			{
				this.coinsInput.value = value.ToString();
			}
		}
	}

	// Token: 0x17000C00 RID: 3072
	// (set) Token: 0x060049A0 RID: 18848 RVA: 0x00197780 File Offset: 0x00195980
	public int GemsInput
	{
		set
		{
			if (this.gemsInput != null)
			{
				this.gemsInput.value = value.ToString();
			}
		}
	}

	// Token: 0x17000C01 RID: 3073
	// (set) Token: 0x060049A1 RID: 18849 RVA: 0x001977A8 File Offset: 0x001959A8
	public int EnemiesInSurvivalWaveInput
	{
		set
		{
			if (this.enemyCountInSurvivalWave != null)
			{
				this.enemyCountInSurvivalWave.value = value.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (set) Token: 0x060049A2 RID: 18850 RVA: 0x001977E0 File Offset: 0x001959E0
	public int EnemiesInCampaignInput
	{
		set
		{
			if (this.enemiesInCampaignInput != null)
			{
				this.enemiesInCampaignInput.value = value.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (set) Token: 0x060049A3 RID: 18851 RVA: 0x00197818 File Offset: 0x00195A18
	public int Days
	{
		set
		{
			if (this.daysInput != null)
			{
				this.daysInput.value = value.ToString(CultureInfo.InvariantCulture);
			}
		}
	}

	// Token: 0x17000C04 RID: 3076
	// (set) Token: 0x060049A4 RID: 18852 RVA: 0x00197850 File Offset: 0x00195A50
	public bool StrongDevice
	{
		set
		{
			if (this.strongDeivceCheckbox != null)
			{
				this.strongDeivceCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C05 RID: 3077
	// (set) Token: 0x060049A5 RID: 18853 RVA: 0x00197870 File Offset: 0x00195A70
	public bool TrainingCompleted
	{
		set
		{
			if (this.trainingCheckbox != null)
			{
				this.trainingCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C06 RID: 3078
	// (set) Token: 0x060049A6 RID: 18854 RVA: 0x00197890 File Offset: 0x00195A90
	public bool TempGunActive
	{
		set
		{
			if (this.tempGunCheckbox != null)
			{
				this.tempGunCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C07 RID: 3079
	// (set) Token: 0x060049A7 RID: 18855 RVA: 0x001978B0 File Offset: 0x00195AB0
	public bool Set60FPSActive
	{
		set
		{
			if (this.set60FpsCheckbox != null)
			{
				this.set60FpsCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C08 RID: 3080
	// (set) Token: 0x060049A8 RID: 18856 RVA: 0x001978D0 File Offset: 0x00195AD0
	public bool SetMouseControll
	{
		set
		{
			if (this.mouseCOntrollCheckbox != null)
			{
				this.mouseCOntrollCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C09 RID: 3081
	// (set) Token: 0x060049A9 RID: 18857 RVA: 0x001978F0 File Offset: 0x00195AF0
	public bool SetSpectatorMode
	{
		set
		{
			if (this.spectatorModeCheckbox != null)
			{
				this.spectatorModeCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C0A RID: 3082
	// (set) Token: 0x060049AA RID: 18858 RVA: 0x00197910 File Offset: 0x00195B10
	public bool SetFBReward
	{
		set
		{
			if (this.fbRewardCheckbox != null)
			{
				this.fbRewardCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C0B RID: 3083
	// (set) Token: 0x060049AB RID: 18859 RVA: 0x00197930 File Offset: 0x00195B30
	public bool IsPayingUser
	{
		set
		{
			if (this.isPayingCheckbox != null)
			{
				this.isPayingCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C0C RID: 3084
	// (set) Token: 0x060049AC RID: 18860 RVA: 0x00197950 File Offset: 0x00195B50
	public int MarathonDayInput
	{
		set
		{
			if (this.marathonCurrentDay != null)
			{
				this.marathonCurrentDay.value = value.ToString();
			}
		}
	}

	// Token: 0x17000C0D RID: 3085
	// (set) Token: 0x060049AD RID: 18861 RVA: 0x00197978 File Offset: 0x00195B78
	public bool MarathonTestMode
	{
		set
		{
			if (this.marathonTestMode != null)
			{
				this.marathonTestMode.value = value;
			}
		}
	}

	// Token: 0x17000C0E RID: 3086
	// (set) Token: 0x060049AE RID: 18862 RVA: 0x00197998 File Offset: 0x00195B98
	public bool GameGUIOffMode
	{
		set
		{
			if (this.gameGUIOffMode != null)
			{
				this.gameGUIOffMode.value = value;
			}
		}
	}

	// Token: 0x17000C0F RID: 3087
	// (set) Token: 0x060049AF RID: 18863 RVA: 0x001979B8 File Offset: 0x00195BB8
	public string DevicePushTokenInput
	{
		set
		{
			if (this.devicePushTokenInput != null)
			{
				this.devicePushTokenInput.value = value;
			}
		}
	}

	// Token: 0x17000C10 RID: 3088
	// (set) Token: 0x060049B0 RID: 18864 RVA: 0x001979D8 File Offset: 0x00195BD8
	public string PlayerIdInput
	{
		set
		{
			if (this.playerIdInput != null)
			{
				this.playerIdInput.value = value;
			}
		}
	}

	// Token: 0x17000C11 RID: 3089
	// (set) Token: 0x060049B1 RID: 18865 RVA: 0x001979F8 File Offset: 0x00195BF8
	public string SocialUserName
	{
		set
		{
			if (this.socialUsername != null)
			{
				this.socialUsername.text = value;
			}
		}
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x060049B2 RID: 18866 RVA: 0x00197A18 File Offset: 0x00195C18
	// (set) Token: 0x060049B3 RID: 18867 RVA: 0x00197A38 File Offset: 0x00195C38
	public bool MemoryInfoActive
	{
		get
		{
			return this.memoryCheckbox && this.memoryCheckbox.value;
		}
		set
		{
			if (this.memoryCheckbox)
			{
				this.memoryCheckbox.value = value;
			}
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x060049B4 RID: 18868 RVA: 0x00197A58 File Offset: 0x00195C58
	// (set) Token: 0x060049B5 RID: 18869 RVA: 0x00197A78 File Offset: 0x00195C78
	public bool ReviewActive
	{
		get
		{
			return this.reviewCheckbox && this.reviewCheckbox.value;
		}
		set
		{
			if (this.reviewCheckbox)
			{
				this.reviewCheckbox.value = value;
			}
		}
	}

	// Token: 0x0400367E RID: 13950
	public static DeveloperConsoleView instance;

	// Token: 0x0400367F RID: 13951
	public UIToggle strongDeivceCheckbox;

	// Token: 0x04003680 RID: 13952
	public UIInput gemsInput;

	// Token: 0x04003681 RID: 13953
	public UIToggle set60FpsCheckbox;

	// Token: 0x04003682 RID: 13954
	public UIToggle mouseCOntrollCheckbox;

	// Token: 0x04003683 RID: 13955
	public UIToggle spectatorModeCheckbox;

	// Token: 0x04003684 RID: 13956
	public UIToggle fbRewardCheckbox;

	// Token: 0x04003685 RID: 13957
	public UIToggle tempGunCheckbox;

	// Token: 0x04003686 RID: 13958
	public UILabel levelLabel;

	// Token: 0x04003687 RID: 13959
	public UILabel experienceLabel;

	// Token: 0x04003688 RID: 13960
	public UISlider experienceSlider;

	// Token: 0x04003689 RID: 13961
	public UISlider levelSlider;

	// Token: 0x0400368A RID: 13962
	public UILabel ratingLabel;

	// Token: 0x0400368B RID: 13963
	public UISlider ratingSlider;

	// Token: 0x0400368C RID: 13964
	public UIInput coinsInput;

	// Token: 0x0400368D RID: 13965
	public UIInput enemyCountInSurvivalWave;

	// Token: 0x0400368E RID: 13966
	public UIInput enemiesInCampaignInput;

	// Token: 0x0400368F RID: 13967
	public UIInput daysInput;

	// Token: 0x04003690 RID: 13968
	public UIToggle trainingCheckbox;

	// Token: 0x04003691 RID: 13969
	public UIToggle downgradeResolutionCheckbox;

	// Token: 0x04003692 RID: 13970
	public UIToggle isPayingCheckbox;

	// Token: 0x04003693 RID: 13971
	public UIToggle serverTimeCheckbox;

	// Token: 0x04003694 RID: 13972
	public UIToggle areLogsEnabledCheckbox;

	// Token: 0x04003695 RID: 13973
	public UIToggle isDebugGuiVisibleCheckbox;

	// Token: 0x04003696 RID: 13974
	public UIToggle isEventX3ForcedCheckbox;

	// Token: 0x04003697 RID: 13975
	public UIToggle adIdCheckbox;

	// Token: 0x04003698 RID: 13976
	public UIInput marathonCurrentDay;

	// Token: 0x04003699 RID: 13977
	public UIToggle marathonTestMode;

	// Token: 0x0400369A RID: 13978
	public UIToggle gameGUIOffMode;

	// Token: 0x0400369B RID: 13979
	public UILabel deviceInfo;

	// Token: 0x0400369C RID: 13980
	public UILabel diagonalInfo;

	// Token: 0x0400369D RID: 13981
	public UIInput devicePushTokenInput;

	// Token: 0x0400369E RID: 13982
	public UIInput playerIdInput;

	// Token: 0x0400369F RID: 13983
	public UILabel starterPackLive;

	// Token: 0x040036A0 RID: 13984
	public UILabel starterPackCooldown;

	// Token: 0x040036A1 RID: 13985
	public UILabel socialUsername;

	// Token: 0x040036A2 RID: 13986
	public UIInput oneDayPreminAccount;

	// Token: 0x040036A3 RID: 13987
	public UIInput threeDayPreminAccount;

	// Token: 0x040036A4 RID: 13988
	public UIInput sevenDayPreminAccount;

	// Token: 0x040036A5 RID: 13989
	public UIInput monthDayPreminAccount;

	// Token: 0x040036A6 RID: 13990
	public UIToggle memoryCheckbox;

	// Token: 0x040036A7 RID: 13991
	public UIToggle isPixelGunLowCheckbox;

	// Token: 0x040036A8 RID: 13992
	public UIToggle reviewCheckbox;
}
