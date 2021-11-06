using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000771 RID: 1905
internal sealed class TrainingArrow : MonoBehaviour
{
	// Token: 0x06004317 RID: 17175 RVA: 0x001667A8 File Offset: 0x001649A8
	private void Init()
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.initialPosition = this.rectTransform.anchoredPosition;
		}
	}

	// Token: 0x06004318 RID: 17176 RVA: 0x001667E4 File Offset: 0x001649E4
	public void SetAnchoredPosition(Vector3 position)
	{
		this.Init();
		if (this.rectTransform != null)
		{
			this.rectTransform.anchoredPosition = position;
			this.initialPosition = this.rectTransform.anchoredPosition;
			if (this.tweener != null)
			{
				this.tweener.Kill();
			}
			this.tweener = HOTween.To(this.rectTransform, 0.5f, new TweenParms().Prop("anchoredPosition", this.arrowDelta, true).Loops(-1, LoopType.YoyoInverse));
		}
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x00166878 File Offset: 0x00164A78
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0600431A RID: 17178 RVA: 0x00166880 File Offset: 0x00164A80
	private void OnEnable()
	{
		this.rectTransform.anchoredPosition = this.initialPosition;
		if (this.tweener != null)
		{
			this.tweener.Kill();
		}
		this.tweener = HOTween.To(this.rectTransform, 0.5f, new TweenParms().Prop("anchoredPosition", this.arrowDelta, true).Loops(-1, LoopType.YoyoInverse));
	}

	// Token: 0x0600431B RID: 17179 RVA: 0x001668EC File Offset: 0x00164AEC
	private void OnDisable()
	{
		if (this.tweener != null)
		{
			this.tweener.Kill();
			this.tweener = null;
		}
	}

	// Token: 0x04003120 RID: 12576
	public Vector2 arrowDelta = Vector2.zero;

	// Token: 0x04003121 RID: 12577
	private Vector2 initialPosition;

	// Token: 0x04003122 RID: 12578
	private RectTransform rectTransform;

	// Token: 0x04003123 RID: 12579
	private Tweener tweener;
}
