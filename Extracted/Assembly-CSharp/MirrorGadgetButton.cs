using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class MirrorGadgetButton : MonoBehaviour
{
	// Token: 0x06001B42 RID: 6978 RVA: 0x0007002C File Offset: 0x0006E22C
	private void Update()
	{
		base.transform.localScale = ((this.targetUiAnchor.side != UIAnchor.Side.BottomRight) ? new Vector3(-1f, 1f, 1f) : Vector3.one);
	}

	// Token: 0x04001074 RID: 4212
	public UIAnchor targetUiAnchor;
}
