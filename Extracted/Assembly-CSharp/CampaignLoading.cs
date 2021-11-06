using System;
using System.Reflection;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000059 RID: 89
public sealed class CampaignLoading : MonoBehaviour
{
	// Token: 0x0600023B RID: 571 RVA: 0x00013DD0 File Offset: 0x00011FD0
	private void Start()
	{
		ActivityIndicator.IsActiveIndicator = true;
		string b;
		if (!Defs.IsSurvival)
		{
			if (TrainingController.TrainingCompleted)
			{
				int num = 0;
				LevelBox levelBox = null;
				foreach (LevelBox levelBox2 in LevelBox.campaignBoxes)
				{
					if (levelBox2.name.Equals(CurrentCampaignGame.boXName))
					{
						levelBox = levelBox2;
						foreach (CampaignLevel campaignLevel in levelBox2.levels)
						{
							if (campaignLevel.sceneName.Equals(CurrentCampaignGame.levelSceneName))
							{
								num = levelBox2.levels.IndexOf(campaignLevel);
								break;
							}
						}
					}
				}
				bool flag = num >= levelBox.levels.Count - 1;
				bool flag2 = false;
				if (!CampaignProgress.boxesLevelsAndStars[CurrentCampaignGame.boXName].ContainsKey(CurrentCampaignGame.levelSceneName))
				{
					flag2 = true;
				}
				bool flag3 = flag2 && flag;
				b = ((!flag3) ? "gey_1" : "gey_15");
				if (this.ordinaryAwardLabel != null)
				{
					this.ordinaryAwardLabel.SetActive(!flag3);
				}
				if (this.stackOfCoinsLabel != null)
				{
					this.stackOfCoinsLabel.SetActive(flag3);
				}
				if (this.campaignNotesOverlay != null)
				{
					this.campaignNotesOverlay.SetActive(true);
				}
			}
			else
			{
				b = "Restore";
				if (this.trainingNotesOverlay != null)
				{
					this.trainingNotesOverlay.SetActive(true);
				}
			}
		}
		else
		{
			b = "gey_surv";
			if (this.survivalNotesOverlay != null)
			{
				this.survivalNotesOverlay.SetActive(true);
			}
		}
		this.plashkaCoins = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", b));
		float num2 = (float)((!TrainingController.TrainingCompleted) ? 484 : 500) * Defs.Coef;
		float num3 = (float)((!TrainingController.TrainingCompleted) ? 279 : 244) * Defs.Coef;
		this.plashkaCoinsRect = new Rect(((float)Screen.width - num2) / 2f, (float)Screen.height * 0.8f - num3 / 2f, num2, num3);
		string str = TrainingController.TrainingCompleted ? ((!Defs.IsSurvival) ? CurrentCampaignGame.levelSceneName : Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length]) : "Training";
		string b2 = "Loading_" + str;
		this.fonToDraw = Resources.Load<Texture>(ResPath.Combine(Switcher.LoadingInResourcesPath + ((!Device.isRetinaAndStrong) ? string.Empty : "/Hi"), b2));
		if (this.backgroundUiTexture != null)
		{
			this.backgroundUiTexture.mainTexture = this.fonToDraw;
		}
		base.Invoke("Load", 2f);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0001412C File Offset: 0x0001232C
	[Obfuscation(Exclude = true)]
	private void Load()
	{
		if (Defs.IsSurvival)
		{
			Singleton<SceneLoader>.Instance.LoadScene(Defs.SurvivalMaps[Defs.CurrentSurvMapIndex % Defs.SurvivalMaps.Length], LoadSceneMode.Single);
		}
		else
		{
			Singleton<SceneLoader>.Instance.LoadScene(TrainingController.TrainingCompleted ? CurrentCampaignGame.levelSceneName : "Training", LoadSceneMode.Single);
		}
	}

	// Token: 0x04000269 RID: 617
	public static readonly string DesignersTestMap = "Coliseum";

	// Token: 0x0400026A RID: 618
	public UITexture backgroundUiTexture;

	// Token: 0x0400026B RID: 619
	public GameObject survivalNotesOverlay;

	// Token: 0x0400026C RID: 620
	public GameObject campaignNotesOverlay;

	// Token: 0x0400026D RID: 621
	public GameObject trainingNotesOverlay;

	// Token: 0x0400026E RID: 622
	public GameObject ordinaryAwardLabel;

	// Token: 0x0400026F RID: 623
	public GameObject stackOfCoinsLabel;

	// Token: 0x04000270 RID: 624
	public Texture loadingNote;

	// Token: 0x04000271 RID: 625
	private Texture fonToDraw;

	// Token: 0x04000272 RID: 626
	private Texture plashkaCoins;

	// Token: 0x04000273 RID: 627
	private Rect plashkaCoinsRect;
}
