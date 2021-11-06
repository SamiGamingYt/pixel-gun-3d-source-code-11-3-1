using System;
using UnityEngine;

// Token: 0x0200031A RID: 794
[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
	// Token: 0x06001B9A RID: 7066 RVA: 0x00071760 File Offset: 0x0006F960
	private void OnClick()
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, this.state);
		}
	}

	// Token: 0x040010B5 RID: 4277
	public GameObject target;

	// Token: 0x040010B6 RID: 4278
	public bool state = true;
}
