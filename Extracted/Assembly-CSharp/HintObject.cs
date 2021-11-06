using System;
using UnityEngine;

// Token: 0x020002A5 RID: 677
internal class HintObject : MonoBehaviour
{
	// Token: 0x0600154E RID: 5454 RVA: 0x00054C28 File Offset: 0x00052E28
	public void Show(HintController.HintItem hint)
	{
		base.gameObject.SetActive(!hint.showLabelByCode);
		base.transform.parent = hint.target.transform;
		base.transform.localPosition = hint.relativeHintPosition;
		this.label.text = LocalizationStore.Get(hint.hintText);
		if (hint.manualRotateArrow)
		{
			this.arrow.localRotation = Quaternion.Euler(hint.manualArrowRotation);
		}
		else
		{
			this.arrow.localRotation = Quaternion.identity;
		}
		this.label.transform.localPosition = hint.relativeLabelPosition;
		if (this.label.transform.localPosition.x > 0f)
		{
			this.arrow.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			this.arrow.localScale = Vector3.one;
		}
		this.myHint = hint;
		this.lastIndic = Time.time;
		if (hint.scaleTween)
		{
			base.transform.localScale = Vector3.one * 0.3f;
		}
		else
		{
			base.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00054D74 File Offset: 0x00052F74
	public void Hide()
	{
		if (this.myHint.indicateTarget)
		{
			if (this.myHint.targetSprites == null || this.myHint.targetSprites.Length == 0)
			{
				this.myHint.targetSprite.spriteName = this.myHint.defaultSpriteName;
			}
			else
			{
				for (int i = 0; i < this.myHint.targetSprites.Length; i++)
				{
					this.myHint.targetSprites[i].color = Color.white;
				}
			}
		}
		this.myHint = null;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x00054E1C File Offset: 0x0005301C
	private void Update()
	{
		if (this.myHint.indicateTarget && this.lastIndic < Time.time)
		{
			this.lastIndic = Time.time + 0.5f;
			this.indicOn = !this.indicOn;
			if (this.myHint.targetSprites == null || this.myHint.targetSprites.Length == 0)
			{
				this.myHint.targetSprite.spriteName = ((!this.indicOn) ? this.myHint.defaultSpriteName : this.myHint.indicatedSpriteName);
			}
			else
			{
				for (int i = 0; i < this.myHint.targetSprites.Length; i++)
				{
					this.myHint.targetSprites[i].color = ((!this.indicOn) ? Color.white : Color.green);
				}
			}
		}
		if (this.myHint.scaleTween)
		{
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), 3f * Time.unscaledDeltaTime);
		}
	}

	// Token: 0x04000C91 RID: 3217
	public UILabel label;

	// Token: 0x04000C92 RID: 3218
	public Transform arrow;

	// Token: 0x04000C93 RID: 3219
	private HintController.HintItem myHint;

	// Token: 0x04000C94 RID: 3220
	private bool indicOn;

	// Token: 0x04000C95 RID: 3221
	private float lastIndic;
}
