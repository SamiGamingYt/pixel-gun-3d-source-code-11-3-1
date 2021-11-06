using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
public sealed class DaysOfValorLabelActive : MonoBehaviour
{
	// Token: 0x06000438 RID: 1080 RVA: 0x000241D8 File Offset: 0x000223D8
	private void Awake()
	{
		this.UpdateLabels();
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000241E0 File Offset: 0x000223E0
	private void UpdateLabels()
	{
		if (PromoActionsManager.sharedManager == null)
		{
			return;
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp > 1 && !this.expLabel.gameObject.activeSelf)
		{
			this.expLabel.gameObject.SetActive(true);
			this.expLabel.spriteName = PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp.ToString() + "x";
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp == 1 && this.expLabel.gameObject.activeSelf)
		{
			this.expLabel.gameObject.SetActive(false);
			this.coinsLabel.transform.localPosition = new Vector3(109f, this.coinsLabel.transform.localPosition.y, this.coinsLabel.transform.localPosition.z);
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney > 1 && !this.coinsLabel.gameObject.activeSelf)
		{
			this.coinsLabel.gameObject.SetActive(true);
			this.coinsLabel.spriteName = PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney.ToString() + "x";
			if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForExp > 1)
			{
				this.coinsLabel.transform.localPosition = new Vector3(28f, this.coinsLabel.transform.localPosition.y, this.coinsLabel.transform.localPosition.z);
			}
		}
		if (PromoActionsManager.sharedManager.DayOfValorMultiplyerForMoney == 1 && this.coinsLabel.gameObject.activeSelf)
		{
			this.coinsLabel.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x000243CC File Offset: 0x000225CC
	private void Update()
	{
		this.UpdateLabels();
	}

	// Token: 0x040004C7 RID: 1223
	public UISprite coinsLabel;

	// Token: 0x040004C8 RID: 1224
	public UISprite expLabel;
}
