using System;
using UnityEngine;

// Token: 0x02000381 RID: 897
[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour
{
	// Token: 0x06001F99 RID: 8089 RVA: 0x00091E64 File Offset: 0x00090064
	private void OnEnable()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.LateUpdate();
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x00091E78 File Offset: 0x00090078
	private void LateUpdate()
	{
		this.mWidget.color = this.color;
	}

	// Token: 0x04001406 RID: 5126
	public Color color = Color.white;

	// Token: 0x04001407 RID: 5127
	private UIWidget mWidget;
}
