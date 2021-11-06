using System;
using UnityEngine;

// Token: 0x020002A7 RID: 679
[ExecuteInEditMode]
public class HintedObject : MonoBehaviour
{
	// Token: 0x06001555 RID: 5461 RVA: 0x00054FFC File Offset: 0x000531FC
	private void OnPress(bool pressed)
	{
		this.timer = this.timeToShowHint;
		this.press = pressed;
		if (!pressed && this.hintObj.isActiveAndEnabled)
		{
			this.CloseHint();
		}
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x00055030 File Offset: 0x00053230
	public void ShowHint()
	{
		this.isShowing = true;
		this.hintObj.gameObject.SetActive(true);
		this.hintObj.body.transform.parent = base.transform;
		this.hintObj.botRightArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botRight);
		this.hintObj.botCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botCenter);
		this.hintObj.botLeftArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.botLeft);
		this.hintObj.leftBotArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftBot);
		this.hintObj.leftCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftCenter);
		this.hintObj.leftTopArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.leftTop);
		this.hintObj.rightTopArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightTop);
		this.hintObj.rightCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightCenter);
		this.hintObj.rightBotArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.rightBot);
		this.hintObj.topLeftArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topLeft);
		this.hintObj.topCenterArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topCenter);
		this.hintObj.topRightArrow.SetActive(this.arrowPos == HintedObject.ArrowPos.topRight);
		this.hintObj.label.text = LocalizationStore.Get(this.term);
		this.hintObj.label.fontSize = this.fontSize;
		this.hintObj.label.transform.localPosition = new Vector3(0f, 0f);
		this.hintObj.body.transform.localPosition = this.position;
		if (Application.isPlaying)
		{
			this.hintObj.tween.PlayForward();
		}
		if (this.arrowPos == HintedObject.ArrowPos.leftTop || this.arrowPos == HintedObject.ArrowPos.rightTop || this.arrowPos == HintedObject.ArrowPos.topCenter || this.arrowPos == HintedObject.ArrowPos.topLeft || this.arrowPos == HintedObject.ArrowPos.topRight)
		{
			this.hintObj.label.pivot = UIWidget.Pivot.TopRight;
		}
		else if (this.arrowPos == HintedObject.ArrowPos.leftCenter || this.arrowPos == HintedObject.ArrowPos.rightCenter)
		{
			this.hintObj.label.pivot = UIWidget.Pivot.Right;
		}
		else
		{
			this.hintObj.label.pivot = UIWidget.Pivot.BottomRight;
		}
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000552C0 File Offset: 0x000534C0
	public void CloseHint()
	{
		this.isShowing = false;
		this.hintObj.gameObject.SetActive(false);
		if (Application.isPlaying)
		{
			this.hintObj.tween.ResetToBeginning();
		}
		this.timer = this.timeToShowHint;
		this.hintObj.body.transform.parent = this.hintObj.transform;
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x0005532C File Offset: 0x0005352C
	private void Update()
	{
		if (this.press && this.showOnPress)
		{
			this.timer -= Time.deltaTime;
			if (this.timer < 0f)
			{
				this.ShowHint();
			}
		}
		if (this.isShowing && this.showOnPress && !this.press)
		{
			this.CloseHint();
		}
		if (!Application.isPlaying)
		{
			if (this.preview)
			{
				this.ShowHint();
			}
			if (this.isShowing && !this.preview)
			{
				this.CloseHint();
			}
		}
	}

	// Token: 0x04000C96 RID: 3222
	public int fontSize = 20;

	// Token: 0x04000C97 RID: 3223
	public string term = "hint";

	// Token: 0x04000C98 RID: 3224
	public float timeToShowHint = 0.2f;

	// Token: 0x04000C99 RID: 3225
	public MenuHintObject hintObj;

	// Token: 0x04000C9A RID: 3226
	public Vector3 position;

	// Token: 0x04000C9B RID: 3227
	public HintedObject.ArrowPos arrowPos;

	// Token: 0x04000C9C RID: 3228
	public bool showOnPress;

	// Token: 0x04000C9D RID: 3229
	public bool preview;

	// Token: 0x04000C9E RID: 3230
	private float timer;

	// Token: 0x04000C9F RID: 3231
	private bool press;

	// Token: 0x04000CA0 RID: 3232
	private Transform tempTransform;

	// Token: 0x04000CA1 RID: 3233
	private bool isShowing;

	// Token: 0x020002A8 RID: 680
	public enum ArrowPos
	{
		// Token: 0x04000CA3 RID: 3235
		botRight,
		// Token: 0x04000CA4 RID: 3236
		botCenter,
		// Token: 0x04000CA5 RID: 3237
		botLeft,
		// Token: 0x04000CA6 RID: 3238
		leftBot,
		// Token: 0x04000CA7 RID: 3239
		leftCenter,
		// Token: 0x04000CA8 RID: 3240
		leftTop,
		// Token: 0x04000CA9 RID: 3241
		rightTop,
		// Token: 0x04000CAA RID: 3242
		rightCenter,
		// Token: 0x04000CAB RID: 3243
		rightBot,
		// Token: 0x04000CAC RID: 3244
		topLeft,
		// Token: 0x04000CAD RID: 3245
		topCenter,
		// Token: 0x04000CAE RID: 3246
		topRight
	}
}
