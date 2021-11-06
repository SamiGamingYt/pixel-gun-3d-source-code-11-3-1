using System;
using UnityEngine;

// Token: 0x02000897 RID: 2199
public class setupGadgetPanel : MonoBehaviour
{
	// Token: 0x06004EFB RID: 20219 RVA: 0x001C9D14 File Offset: 0x001C7F14
	private void Awake()
	{
		this.timeLabel.value = TouchPadController.timeGadgetPanel.ToString();
		this.thresholdLabel.value = TouchPadController.thresholdGadgetPanel.ToString();
	}

	// Token: 0x06004EFC RID: 20220 RVA: 0x001C9D4C File Offset: 0x001C7F4C
	public void timeInputChanged(UIInputRilisoft input)
	{
		try
		{
			TouchPadController.timeGadgetPanel = float.Parse(this.timeLabel.value);
		}
		catch
		{
		}
	}

	// Token: 0x06004EFD RID: 20221 RVA: 0x001C9D98 File Offset: 0x001C7F98
	public void thresholdInputChanged(UIInputRilisoft input)
	{
		try
		{
			TouchPadController.thresholdGadgetPanel = int.Parse(this.thresholdLabel.value);
		}
		catch
		{
		}
	}

	// Token: 0x04003D57 RID: 15703
	public UIInputRilisoft timeLabel;

	// Token: 0x04003D58 RID: 15704
	public UIInputRilisoft thresholdLabel;
}
