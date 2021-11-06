using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000774 RID: 1908
internal sealed class TrainingFingerSwipe : MonoBehaviour
{
	// Token: 0x06004328 RID: 17192 RVA: 0x00166C50 File Offset: 0x00164E50
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.initialAnchoredPosition = this.rectTransform.anchoredPosition;
	}

	// Token: 0x06004329 RID: 17193 RVA: 0x00166C80 File Offset: 0x00164E80
	private void OnEnable()
	{
		this.rectTransform.anchoredPosition = this.initialAnchoredPosition;
		if (this.tweener != null)
		{
			this.tweener.Kill();
		}
		this.tweener = HOTween.To(this.rectTransform, 1f, new TweenParms().Prop("anchoredPosition", this.arrowDdelta, true).Ease(EaseType.EaseInQuad).Loops(-1, LoopType.Restart));
	}

	// Token: 0x0600432A RID: 17194 RVA: 0x00166CF8 File Offset: 0x00164EF8
	private void Update()
	{
		int completedLoops = this.tweener.completedLoops;
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x00166D14 File Offset: 0x00164F14
	private void OnDisable()
	{
		if (this.tweener != null)
		{
			this.tweener.Kill();
			this.tweener = null;
		}
	}

	// Token: 0x04003129 RID: 12585
	public Vector2 arrowDdelta = new Vector3(300f, 0f);

	// Token: 0x0400312A RID: 12586
	private Vector3 initialAnchoredPosition;

	// Token: 0x0400312B RID: 12587
	private RectTransform rectTransform;

	// Token: 0x0400312C RID: 12588
	private Tweener tweener;
}
