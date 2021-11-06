using System;
using UnityEngine;

// Token: 0x02000382 RID: 898
[ExecuteInEditMode]
public class AnimatedWidget : MonoBehaviour
{
	// Token: 0x06001F9C RID: 8092 RVA: 0x00091EAC File Offset: 0x000900AC
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x00091EC0 File Offset: 0x000900C0
	private void LateUpdate()
	{
		if (this.mWidget != null)
		{
			this.mWidget.width = Mathf.RoundToInt(this.width);
			this.mWidget.height = Mathf.RoundToInt(this.height);
		}
	}

	// Token: 0x04001408 RID: 5128
	public float width = 1f;

	// Token: 0x04001409 RID: 5129
	public float height = 1f;

	// Token: 0x0400140A RID: 5130
	private UIWidget mWidget;
}
