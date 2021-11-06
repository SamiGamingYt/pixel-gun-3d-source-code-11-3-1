using System;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class ButtonEnabledLabel : MonoBehaviour
{
	// Token: 0x0600020C RID: 524 RVA: 0x00013550 File Offset: 0x00011750
	private void Start()
	{
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00013554 File Offset: 0x00011754
	private void Update()
	{
		if (this.myButton.isEnabled && !this.enabledLabel.activeSelf)
		{
			this.enabledLabel.SetActive(true);
			this.disableLabel.SetActive(false);
		}
		if (!this.myButton.isEnabled && this.enabledLabel.activeSelf)
		{
			this.enabledLabel.SetActive(false);
			this.disableLabel.SetActive(true);
		}
	}

	// Token: 0x0400023B RID: 571
	public UIButton myButton;

	// Token: 0x0400023C RID: 572
	public GameObject enabledLabel;

	// Token: 0x0400023D RID: 573
	public GameObject disableLabel;
}
