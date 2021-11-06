using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000879 RID: 2169
public class UnlockedItemsArmoryIndicatorController : MonoBehaviour
{
	// Token: 0x06004E61 RID: 20065 RVA: 0x001C684C File Offset: 0x001C4A4C
	private void Start()
	{
		this.UpdateIndicator();
	}

	// Token: 0x06004E62 RID: 20066 RVA: 0x001C6854 File Offset: 0x001C4A54
	private void Update()
	{
		try
		{
			if (Time.realtimeSinceStartup - 0.5f >= this.m_lastUpdateTime)
			{
				this.UpdateIndicator();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UnlockedItemsArmoryIndicatorController.Update: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x06004E63 RID: 20067 RVA: 0x001C68C0 File Offset: 0x001C4AC0
	private void UpdateIndicator()
	{
		int num = ShopNGUIController.CurrentNumberOfUnlockedItems();
		bool flag = num > 0;
		this.label.gameObject.SetActiveSafeSelf(flag);
		if (flag)
		{
			this.label.text = num.ToString();
		}
		this.m_lastUpdateTime = Time.realtimeSinceStartup;
	}

	// Token: 0x04003CF9 RID: 15609
	public UILabel label;

	// Token: 0x04003CFA RID: 15610
	private float m_lastUpdateTime = float.MinValue;
}
