using System;
using UnityEngine;

// Token: 0x0200034A RID: 842
public class UIShowControlScheme : MonoBehaviour
{
	// Token: 0x06001D18 RID: 7448 RVA: 0x0007BEF0 File Offset: 0x0007A0F0
	private void OnEnable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Combine(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
		this.OnScheme();
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0007BF24 File Offset: 0x0007A124
	private void OnDisable()
	{
		UICamera.onSchemeChange = (UICamera.OnSchemeChange)Delegate.Remove(UICamera.onSchemeChange, new UICamera.OnSchemeChange(this.OnScheme));
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0007BF54 File Offset: 0x0007A154
	private void OnScheme()
	{
		if (this.target != null)
		{
			UICamera.ControlScheme currentScheme = UICamera.currentScheme;
			if (currentScheme == UICamera.ControlScheme.Mouse)
			{
				this.target.SetActive(this.mouse);
			}
			else if (currentScheme == UICamera.ControlScheme.Touch)
			{
				this.target.SetActive(this.touch);
			}
			else if (currentScheme == UICamera.ControlScheme.Controller)
			{
				this.target.SetActive(this.controller);
			}
		}
	}

	// Token: 0x0400124B RID: 4683
	public GameObject target;

	// Token: 0x0400124C RID: 4684
	public bool mouse;

	// Token: 0x0400124D RID: 4685
	public bool touch;

	// Token: 0x0400124E RID: 4686
	public bool controller = true;
}
