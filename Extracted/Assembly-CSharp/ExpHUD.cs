using System;
using UnityEngine;

// Token: 0x02000707 RID: 1799
public class ExpHUD : MonoBehaviour
{
	// Token: 0x06003E8B RID: 16011 RVA: 0x0014F390 File Offset: 0x0014D590
	private void OnEnable()
	{
		ExpController.Instance.experienceView.VisibleHUD = false;
		this.UpdateHUD();
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0014F3A8 File Offset: 0x0014D5A8
	private void OnDisable()
	{
		if (ExpController.Instance == null)
		{
			Debug.LogWarning("ExpController.Instance == null");
			return;
		}
		if (ExpController.Instance.experienceView == null)
		{
			Debug.LogWarning("experienceView == null");
			return;
		}
		ExpController.Instance.experienceView.VisibleHUD = true;
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0014F400 File Offset: 0x0014D600
	public void UpdateHUD()
	{
		this.lbCurLev.text = ExperienceController.sharedController.currentLevel.ToString();
		this.lbExp.text = ExpController.ExpToString();
		if (ExperienceController.sharedController.currentLevel == 31)
		{
			this.txExp.fillAmount = 1f;
		}
		else
		{
			this.txExp.fillAmount = ExpController.progressExpInPer();
		}
	}

	// Token: 0x04002E2E RID: 11822
	public UILabel lbCurLev;

	// Token: 0x04002E2F RID: 11823
	public UILabel lbExp;

	// Token: 0x04002E30 RID: 11824
	public UITexture txExp;
}
