using System;
using System.Linq;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200056E RID: 1390
public class EventX3Banner : BannerWindow
{
	// Token: 0x0600303E RID: 12350 RVA: 0x000FBC64 File Offset: 0x000F9E64
	private void OnEnable()
	{
		bool isAmazonEventX3Active = PromoActionsManager.sharedManager.IsAmazonEventX3Active;
		this.amazonEventObject.SetActive(isAmazonEventX3Active);
		PromoActionsManager.EventAmazonX3Updated += this.OnAmazonEventUpdated;
		this.RefreshAmazonBonus();
	}

	// Token: 0x0600303F RID: 12351 RVA: 0x000FBCA0 File Offset: 0x000F9EA0
	private void OnDisable()
	{
		PromoActionsManager.EventAmazonX3Updated -= this.OnAmazonEventUpdated;
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x000FBCB4 File Offset: 0x000F9EB4
	private void RefreshAmazonBonus()
	{
		UILabel[] componentsInChildren = this.amazonEventObject.GetComponentsInChildren<UILabel>();
		UILabel uilabel;
		if ((uilabel = this.amazonEventCaptionLabel) == null)
		{
			uilabel = componentsInChildren.FirstOrDefault((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
		}
		UILabel uilabel2 = uilabel;
		PromoActionsManager.AmazonEventInfo o = PromoActionsManager.sharedManager.Map((PromoActionsManager p) => p.AmazonEvent);
		if (uilabel2 != null)
		{
			uilabel2.text = (o.Map((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty);
		}
		UILabel uilabel3;
		if ((uilabel3 = this.amazonEventTitleLabel) == null)
		{
			uilabel3 = componentsInChildren.FirstOrDefault((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
		}
		UILabel o2 = uilabel3;
		UILabel[] array = o2.Map((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
		float num = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
		string text = LocalizationStore.Get("Key_1672");
		foreach (UILabel uilabel4 in array)
		{
			uilabel4.text = ("Key_1672".Equals(text, StringComparison.OrdinalIgnoreCase) ? string.Empty : string.Format(text, num));
		}
	}

	// Token: 0x06003041 RID: 12353 RVA: 0x000FBE50 File Offset: 0x000FA050
	private void OnAmazonEventUpdated()
	{
		this.amazonEventObject.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
		this.RefreshAmazonBonus();
	}

	// Token: 0x0400236E RID: 9070
	public GameObject amazonEventObject;

	// Token: 0x0400236F RID: 9071
	public UILabel amazonEventCaptionLabel;

	// Token: 0x04002370 RID: 9072
	public UILabel amazonEventTitleLabel;
}
