using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class SorryWeaponAndArmorBanner : BannerWindow
{
	// Token: 0x06003049 RID: 12361 RVA: 0x000FBEC0 File Offset: 0x000FA0C0
	public override void Show()
	{
		if (this.sorryArmorHatRemoved)
		{
			this._compensationGemsCount.Value = 0;
		}
		else if (this.sorryGearRemoved)
		{
			this._compensationGoldCount.Value = 0;
		}
		for (int i = 0; i < this.labelGoldCompensations.Length; i++)
		{
			this.labelGoldCompensations[i].text = this._compensationGoldCount.Value.ToString();
		}
		for (int j = 0; j < this.labelGemsCompensations.Length; j++)
		{
			this.labelGemsCompensations[j].text = this._compensationGemsCount.Value.ToString();
		}
		this.AligmentCompensationContainer();
		base.Show();
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x000FBF84 File Offset: 0x000FA184
	private void AligmentCompensationContainer()
	{
		if (this._compensationGoldCount.Value > 0 && this._compensationGemsCount.Value == 0)
		{
			this.goldContainer.gameObject.SetActive(true);
			this.gemsContainer.gameObject.SetActive(false);
		}
		else if (this._compensationGoldCount.Value == 0 && this._compensationGemsCount.Value > 0)
		{
			this.goldContainer.gameObject.SetActive(false);
			this.gemsContainer.gameObject.SetActive(true);
		}
		else if (this._compensationGoldCount.Value > 0 && this._compensationGemsCount.Value > 0)
		{
			Vector3 localPosition = this.goldContainer.transform.localPosition;
			this.goldContainer.transform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)(this.goldContainer.height / 2), localPosition.z);
			localPosition = this.gemsContainer.transform.localPosition;
			this.gemsContainer.transform.localPosition = new Vector3(localPosition.x, localPosition.y + (float)(this.gemsContainer.height / 2), localPosition.z);
		}
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000FC0D8 File Offset: 0x000FA2D8
	public void SorryWeaponAndArmorExitClick()
	{
		if (this._compensationGoldCount.Value > 0)
		{
			BankController.AddCoins(this._compensationGoldCount.Value, true, AnalyticsConstants.AccrualType.Earned);
		}
		if (this._compensationGemsCount.Value > 0)
		{
			BankController.AddGems(this._compensationGemsCount.Value, true, AnalyticsConstants.AccrualType.Earned);
		}
		if (!this.sorryArmorHatRemoved)
		{
			if (this.sorryGearRemoved)
			{
			}
		}
		Storager.setInt(Defs.ShowSorryWeaponAndArmor, 1, false);
		AudioClip audioClip = Resources.Load("coin_get") as AudioClip;
		if (Defs.isSoundFX && audioClip != null)
		{
			NGUITools.PlaySound(audioClip);
		}
		if (BannerWindowController.SharedController != null)
		{
			BannerWindowController.SharedController.HideBannerWindow();
		}
	}

	// Token: 0x04002377 RID: 9079
	public bool sorryGearRemoved;

	// Token: 0x04002378 RID: 9080
	public bool sorryArmorHatRemoved;

	// Token: 0x04002379 RID: 9081
	public UILabel[] labelGoldCompensations;

	// Token: 0x0400237A RID: 9082
	public UILabel[] labelGemsCompensations;

	// Token: 0x0400237B RID: 9083
	public UIWidget goldContainer;

	// Token: 0x0400237C RID: 9084
	public UIWidget gemsContainer;

	// Token: 0x0400237D RID: 9085
	private SaltedInt _compensationGoldCount;

	// Token: 0x0400237E RID: 9086
	private SaltedInt _compensationGemsCount;
}
