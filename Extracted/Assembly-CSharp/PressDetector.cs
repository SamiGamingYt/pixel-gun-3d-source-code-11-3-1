using System;
using UnityEngine;

// Token: 0x02000706 RID: 1798
internal sealed class PressDetector : MonoBehaviour
{
	// Token: 0x06003E89 RID: 16009 RVA: 0x0014F35C File Offset: 0x0014D55C
	private void OnPress(bool isDown)
	{
		EventHandler<EventArgs> pressedDown = PressDetector.PressedDown;
		if (pressedDown != null)
		{
			pressedDown(base.gameObject, EventArgs.Empty);
		}
	}

	// Token: 0x04002E2D RID: 11821
	public static EventHandler<EventArgs> PressedDown;
}
