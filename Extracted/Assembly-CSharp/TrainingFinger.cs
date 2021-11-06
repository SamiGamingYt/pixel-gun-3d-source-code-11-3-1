using System;
using Holoville.HOTween;
using UnityEngine;

// Token: 0x02000773 RID: 1907
internal sealed class TrainingFinger : MonoBehaviour
{
	// Token: 0x06004324 RID: 17188 RVA: 0x00166B10 File Offset: 0x00164D10
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.initialPosition = this.rectTransform.localPosition;
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x00166B30 File Offset: 0x00164D30
	private void OnEnable()
	{
		this.rectTransform.localPosition = this.initialPosition;
		this.AngleX = 0f;
		HOTween.To(this, 4f, new TweenParms().Prop("AngleX", 6.2831855f, true).Ease(EaseType.Linear).Loops(-1, LoopType.Restart));
		this.AngleY = 0f;
		HOTween.To(this, 2f, new TweenParms().Prop("AngleY", 6.2831855f, true).Ease(EaseType.Linear).Loops(-1, LoopType.Restart));
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x00166BCC File Offset: 0x00164DCC
	private void Update()
	{
		Vector3 b = new Vector3(60f * Mathf.Sin(this.AngleX), 30f * Mathf.Sin(this.AngleY), 0f);
		base.transform.localPosition = this.initialPosition + b;
	}

	// Token: 0x04003125 RID: 12581
	public float AngleX;

	// Token: 0x04003126 RID: 12582
	public float AngleY;

	// Token: 0x04003127 RID: 12583
	private Vector3 initialPosition;

	// Token: 0x04003128 RID: 12584
	private RectTransform rectTransform;
}
