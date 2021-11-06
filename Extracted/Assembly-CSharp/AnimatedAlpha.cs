using System;
using UnityEngine;

// Token: 0x02000380 RID: 896
[ExecuteInEditMode]
public class AnimatedAlpha : MonoBehaviour
{
	// Token: 0x06001F96 RID: 8086 RVA: 0x00091DDC File Offset: 0x0008FFDC
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.LateUpdate();
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x00091DFC File Offset: 0x0008FFFC
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	// Token: 0x04001403 RID: 5123
	[Range(0f, 1f)]
	public float alpha = 1f;

	// Token: 0x04001404 RID: 5124
	private UIWidget mWidget;

	// Token: 0x04001405 RID: 5125
	private UIPanel mPanel;
}
