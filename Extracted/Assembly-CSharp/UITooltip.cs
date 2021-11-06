using System;
using UnityEngine;

// Token: 0x020003BC RID: 956
[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x0600224E RID: 8782 RVA: 0x000A5474 File Offset: 0x000A3674
	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000A54A8 File Offset: 0x000A36A8
	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000A54B0 File Offset: 0x000A36B0
	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000A54B8 File Offset: 0x000A36B8
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000A5520 File Offset: 0x000A3720
	protected virtual void Update()
	{
		if (this.mTooltip != UICamera.tooltipObject)
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 b = this.mSize * 0.25f;
				b.y = -b.y;
				Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 localPosition = Vector3.Lerp(this.mPos - b, this.mPos, this.mCurrent);
				this.mTrans.localPosition = localPosition;
				this.mTrans.localScale = localScale;
			}
		}
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000A5644 File Offset: 0x000A3844
	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uiwidget = this.mWidgets[i];
			Color color = uiwidget.color;
			color.a = val;
			uiwidget.color = color;
			i++;
		}
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000A568C File Offset: 0x000A388C
	protected virtual void SetText(string tooltipText)
	{
		if (this.text != null && !string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 1f;
			this.mTooltip = UICamera.tooltipObject;
			this.text.text = tooltipText;
			this.mPos = UICamera.lastEventPosition;
			Transform transform = this.text.transform;
			Vector3 localPosition = transform.localPosition;
			Vector3 localScale = transform.localScale;
			this.mSize = this.text.printedSize;
			this.mSize.x = this.mSize.x * localScale.x;
			this.mSize.y = this.mSize.y * localScale.y;
			if (this.background != null)
			{
				Vector4 border = this.background.border;
				this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
				this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera != null)
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
			else
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
			}
			if (this.tooltipRoot != null)
			{
				this.tooltipRoot.BroadcastMessage("UpdateAnchors");
			}
			else
			{
				this.text.BroadcastMessage("UpdateAnchors");
			}
		}
		else
		{
			this.mTooltip = null;
			this.mTarget = 0f;
		}
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000A5A8C File Offset: 0x000A3C8C
	[Obsolete("Use UITooltip.Show instead")]
	public static void ShowText(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000A5AAC File Offset: 0x000A3CAC
	public static void Show(string text)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(text);
		}
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000A5ACC File Offset: 0x000A3CCC
	public static void Hide()
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.mTooltip = null;
			UITooltip.mInstance.mTarget = 0f;
		}
	}

	// Token: 0x0400164E RID: 5710
	protected static UITooltip mInstance;

	// Token: 0x0400164F RID: 5711
	public Camera uiCamera;

	// Token: 0x04001650 RID: 5712
	public UILabel text;

	// Token: 0x04001651 RID: 5713
	public GameObject tooltipRoot;

	// Token: 0x04001652 RID: 5714
	public UISprite background;

	// Token: 0x04001653 RID: 5715
	public float appearSpeed = 10f;

	// Token: 0x04001654 RID: 5716
	public bool scalingTransitions = true;

	// Token: 0x04001655 RID: 5717
	protected GameObject mTooltip;

	// Token: 0x04001656 RID: 5718
	protected Transform mTrans;

	// Token: 0x04001657 RID: 5719
	protected float mTarget;

	// Token: 0x04001658 RID: 5720
	protected float mCurrent;

	// Token: 0x04001659 RID: 5721
	protected Vector3 mPos;

	// Token: 0x0400165A RID: 5722
	protected Vector3 mSize = Vector3.zero;

	// Token: 0x0400165B RID: 5723
	protected UIWidget[] mWidgets;
}
