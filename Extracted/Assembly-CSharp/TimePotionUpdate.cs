using System;
using UnityEngine;

// Token: 0x02000861 RID: 2145
public class TimePotionUpdate : MonoBehaviour
{
	// Token: 0x06004D85 RID: 19845 RVA: 0x001C0758 File Offset: 0x001BE958
	private void Start()
	{
	}

	// Token: 0x06004D86 RID: 19846 RVA: 0x001C075C File Offset: 0x001BE95C
	private void Update()
	{
		if (this.myLabel.enabled)
		{
			this.timerUpdate -= Time.deltaTime;
			if (this.timerUpdate < 0f)
			{
				this.timerUpdate = 0.25f;
				this.SetTimeForLabel();
			}
		}
	}

	// Token: 0x06004D87 RID: 19847 RVA: 0x001C07AC File Offset: 0x001BE9AC
	private void SetTimeForLabel()
	{
		if (!PotionsController.sharedController.PotionIsActive(this.myPotionName))
		{
			if (this.mySpriteObj != null && this.mySpriteObj.activeSelf)
			{
				this.mySpriteObj.SetActive(false);
				this.myLabel.text = string.Empty;
			}
			return;
		}
		if (this.mySpriteObj != null && !this.mySpriteObj.activeSelf)
		{
			this.mySpriteObj.SetActive(true);
		}
		float num = PotionsController.sharedController.RemainDuratioForPotion(this.myPotionName);
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)num);
		this.myLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		if (num <= 5f)
		{
			this.myLabel.color = new Color(1f, 0f, 0f);
		}
		else
		{
			this.myLabel.color = new Color(1f, 1f, 1f);
		}
	}

	// Token: 0x06004D88 RID: 19848 RVA: 0x001C08D0 File Offset: 0x001BEAD0
	public void UpdateTime()
	{
		float num = PotionsController.sharedController.RemainDuratioForPotion(this.myPotionName);
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)num);
		this.myLabel.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		if (num <= 5f)
		{
			this.myLabel.color = new Color(1f, 0f, 0f);
		}
		else
		{
			this.myLabel.color = new Color(1f, 1f, 1f);
		}
	}

	// Token: 0x04003BFC RID: 15356
	public UILabel myLabel;

	// Token: 0x04003BFD RID: 15357
	public GameObject mySpriteObj;

	// Token: 0x04003BFE RID: 15358
	public string myPotionName;

	// Token: 0x04003BFF RID: 15359
	[NonSerialized]
	public float timerUpdate = -1f;
}
