using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200033E RID: 830
[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer
{
	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06001C9C RID: 7324 RVA: 0x000776D8 File Offset: 0x000758D8
	// (set) Token: 0x06001C9D RID: 7325 RVA: 0x0007771C File Offset: 0x0007591C
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06001C9E RID: 7326 RVA: 0x00077778 File Offset: 0x00075978
	// (set) Token: 0x06001C9F RID: 7327 RVA: 0x00077780 File Offset: 0x00075980
	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0007778C File Offset: 0x0007598C
	public static bool isOpen
	{
		get
		{
			return UIPopupList.current != null && (UIPopupList.mChild != null || UIPopupList.mFadeOutComplete > Time.unscaledTime);
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000777CC File Offset: 0x000759CC
	// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x000777D4 File Offset: 0x000759D4
	public virtual string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x00077808 File Offset: 0x00075A08
	public virtual object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			return (num <= -1 || num >= this.itemData.Count) ? null : this.itemData[num];
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x00077854 File Offset: 0x00075A54
	public bool isColliderEnabled
	{
		get
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x00077898 File Offset: 0x00075A98
	// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x000778A0 File Offset: 0x00075AA0
	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x000778AC File Offset: 0x00075AAC
	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x000778DC File Offset: 0x00075ADC
	private int activeFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? this.bitmapFont.defaultSize : this.fontSize;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x00077924 File Offset: 0x00075B24
	private float activeFontScale
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? ((float)this.fontSize / (float)this.bitmapFont.defaultSize) : 1f;
		}
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x00077974 File Offset: 0x00075B74
	public virtual void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x0007798C File Offset: 0x00075B8C
	public virtual void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000779A8 File Offset: 0x00075BA8
	public virtual void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000779C4 File Offset: 0x00075BC4
	public virtual void RemoveItem(string text)
	{
		int num = this.items.IndexOf(text);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x00077A00 File Offset: 0x00075C00
	public virtual void RemoveItemByData(object data)
	{
		int num = this.itemData.IndexOf(data);
		if (num != -1)
		{
			this.items.RemoveAt(num);
			this.itemData.RemoveAt(num);
		}
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x00077A3C File Offset: 0x00075C3C
	protected void TriggerCallbacks()
	{
		if (!this.mExecuting)
		{
			this.mExecuting = true;
			UIPopupList uipopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			UIPopupList.current = uipopupList;
			this.mExecuting = false;
		}
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x00077AEC File Offset: 0x00075CEC
	protected virtual void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((!(this.bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x00077C2C File Offset: 0x00075E2C
	protected virtual void OnValidate()
	{
		Font x = this.trueTypeFont;
		UIFont uifont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (x != null && (uifont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
		}
		else if (uifont != null)
		{
			if (uifont.isDynamic)
			{
				this.trueTypeFont = uifont.dynamicFont;
				this.fontStyle = uifont.dynamicFontStyle;
				this.fontSize = uifont.defaultSize;
				this.mUseDynamicFont = true;
			}
			else
			{
				this.bitmapFont = uifont;
				this.mUseDynamicFont = false;
			}
		}
		else
		{
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
		}
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00077CFC File Offset: 0x00075EFC
	protected virtual void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem) && this.items.Count > 0)
			{
				this.mSelectedItem = this.items[0];
			}
			if (!string.IsNullOrEmpty(this.mSelectedItem))
			{
				this.TriggerCallbacks();
			}
		}
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x00077D94 File Offset: 0x00075F94
	protected virtual void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x00077DA8 File Offset: 0x00075FA8
	protected virtual void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (!instant && this.isAnimated)
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
				}
			}
			else
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
		}
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x00077E48 File Offset: 0x00076048
	protected virtual Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.zero;
		}
		float pixelSize = this.atlas.pixelSize;
		float num = (float)atlasSprite.borderLeft * pixelSize;
		float y = (float)atlasSprite.borderTop * pixelSize;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num, y, 1f);
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x00077ED8 File Offset: 0x000760D8
	protected virtual IEnumerator UpdateTweenPosition()
	{
		if (this.mHighlight != null && this.mHighlightedLabel != null)
		{
			TweenPosition tp = this.mHighlight.GetComponent<TweenPosition>();
			while (tp != null && tp.enabled)
			{
				tp.to = this.GetHighlightPosition();
				yield return null;
			}
		}
		this.mTweening = false;
		yield break;
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x00077EF4 File Offset: 0x000760F4
	protected virtual void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x00077F18 File Offset: 0x00076118
	protected virtual void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
			UIEventListener component = go.GetComponent<UIEventListener>();
			this.value = (component.parameter as string);
			UIPlaySound[] components = base.GetComponents<UIPlaySound>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				UIPlaySound uiplaySound = components[i];
				if (uiplaySound.trigger == UIPlaySound.Trigger.OnClick)
				{
					NGUITools.PlaySound(uiplaySound.audioClip, uiplaySound.volume, 1f);
				}
				i++;
			}
			this.CloseSelf();
		}
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x00077FA0 File Offset: 0x000761A0
	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x00077FAC File Offset: 0x000761AC
	protected virtual void OnNavigate(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
				}
			}
			else if (key == KeyCode.DownArrow && num + 1 < this.mLabelList.Count)
			{
				this.Select(this.mLabelList[num + 1], false);
			}
		}
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x00078054 File Offset: 0x00076254
	protected virtual void OnKey(KeyCode key)
	{
		if (base.enabled && UIPopupList.current == this && (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1))
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000780A4 File Offset: 0x000762A4
	protected virtual void OnDisable()
	{
		this.CloseSelf();
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000780AC File Offset: 0x000762AC
	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.CloseSelf();
		}
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000780BC File Offset: 0x000762BC
	public static void Close()
	{
		if (UIPopupList.current != null)
		{
			UIPopupList.current.CloseSelf();
			UIPopupList.current = null;
		}
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x000780EC File Offset: 0x000762EC
	public virtual void CloseSelf()
	{
		if (UIPopupList.mChild != null && UIPopupList.current == this)
		{
			base.StopCoroutine("CloseIfUnselected");
			this.mSelection = null;
			this.mLabelList.Clear();
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = UIPopupList.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uiwidget = componentsInChildren[i];
					Color color = uiwidget.color;
					color.a = 0f;
					TweenColor.Begin(uiwidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = UIPopupList.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				UnityEngine.Object.Destroy(UIPopupList.mChild, 0.15f);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
			}
			else
			{
				UnityEngine.Object.Destroy(UIPopupList.mChild);
				UIPopupList.mFadeOutComplete = Time.unscaledTime + 0.1f;
			}
			this.mBackground = null;
			this.mHighlight = null;
			UIPopupList.mChild = null;
			UIPopupList.current = null;
		}
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00078228 File Offset: 0x00076428
	protected virtual void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x00078278 File Offset: 0x00076478
	protected virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = (!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
		widget.cachedTransform.localPosition = localPosition2;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x000782F0 File Offset: 0x000764F0
	protected virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x000783A4 File Offset: 0x000765A4
	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x000783B8 File Offset: 0x000765B8
	protected virtual void OnClick()
	{
		if (this.mOpenFrame == Time.frameCount)
		{
			return;
		}
		if (UIPopupList.mChild == null)
		{
			if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
			{
				return;
			}
			if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
			{
				return;
			}
			this.Show();
		}
		else if (this.mHighlightedLabel != null)
		{
			this.OnItemPress(this.mHighlightedLabel.gameObject, true);
		}
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00078448 File Offset: 0x00076648
	protected virtual void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x0007845C File Offset: 0x0007665C
	private IEnumerator CloseIfUnselected()
	{
		do
		{
			yield return null;
		}
		while (!(UICamera.selectedObject != this.mSelection));
		this.CloseSelf();
		yield break;
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x00078478 File Offset: 0x00076678
	public virtual void Show()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && UIPopupList.mChild == null && this.atlas != null && this.isValid && this.items.Count > 0)
		{
			this.mLabelList.Clear();
			base.StopCoroutine("CloseIfUnselected");
			UICamera.selectedObject = (UICamera.hoveredObject ?? base.gameObject);
			this.mSelection = UICamera.selectedObject;
			this.source = UICamera.selectedObject;
			if (this.source == null)
			{
				Debug.LogError("Popup list needs a source object...");
				return;
			}
			this.mOpenFrame = Time.frameCount;
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform);
				if (this.mPanel == null)
				{
					return;
				}
			}
			UIPopupList.mChild = new GameObject("Drop-down List");
			UIPopupList.mChild.layer = base.gameObject.layer;
			if (this.separatePanel)
			{
				if (base.GetComponent<Collider>() != null)
				{
					Rigidbody rigidbody = UIPopupList.mChild.AddComponent<Rigidbody>();
					rigidbody.isKinematic = true;
				}
				else if (base.GetComponent<Collider2D>() != null)
				{
					Rigidbody2D rigidbody2D = UIPopupList.mChild.AddComponent<Rigidbody2D>();
					rigidbody2D.isKinematic = true;
				}
				UIPopupList.mChild.AddComponent<UIPanel>().depth = 1000000;
			}
			UIPopupList.current = this;
			Transform transform = UIPopupList.mChild.transform;
			transform.parent = this.mPanel.cachedTransform;
			Vector3 localPosition;
			Vector3 vector;
			Vector3 v;
			if (this.openOn == UIPopupList.OpenOn.Manual && this.mSelection != base.gameObject)
			{
				localPosition = UICamera.lastEventPosition;
				vector = this.mPanel.cachedTransform.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(localPosition));
				v = vector;
				transform.localPosition = vector;
				localPosition = transform.position;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, base.transform, false, false);
				vector = bounds.min;
				v = bounds.max;
				transform.localPosition = vector;
				localPosition = transform.position;
			}
			base.StartCoroutine("CloseIfUnselected");
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.backgroundSprite, (!this.separatePanel) ? NGUITools.CalculateNextDepth(this.mPanel.gameObject) : 0);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.color = this.backgroundColor;
			Vector4 border = this.mBackground.border;
			this.mBgBorder = border.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
			this.mHighlight = NGUITools.AddSprite(UIPopupList.mChild, this.atlas, this.highlightSprite, this.mBackground.depth + 1);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float num = (float)atlasSprite.borderTop;
			float num2 = (float)this.activeFontSize;
			float activeFontScale = this.activeFontScale;
			float num3 = num2 * activeFontScale;
			float num4 = 0f;
			float num5 = -this.padding.y;
			List<UILabel> list = new List<UILabel>();
			if (!this.items.Contains(this.mSelectedItem))
			{
				this.mSelectedItem = null;
			}
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				string text = this.items[i];
				UILabel uilabel = NGUITools.AddWidget<UILabel>(UIPopupList.mChild, this.mBackground.depth + 2);
				uilabel.name = i.ToString();
				uilabel.pivot = UIWidget.Pivot.TopLeft;
				uilabel.bitmapFont = this.bitmapFont;
				uilabel.trueTypeFont = this.trueTypeFont;
				uilabel.fontSize = this.fontSize;
				uilabel.fontStyle = this.fontStyle;
				uilabel.text = ((!this.isLocalized) ? text : Localization.Get(text));
				uilabel.color = this.textColor;
				uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uilabel.pivotOffset.x, num5, -1f);
				uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uilabel.alignment = this.alignment;
				list.Add(uilabel);
				num5 -= num3;
				num5 -= this.padding.y;
				num4 = Mathf.Max(num4, uilabel.printedSize.x);
				UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
				uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				uieventListener.parameter = text;
				if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
				{
					this.Highlight(uilabel, true);
				}
				this.mLabelList.Add(uilabel);
				i++;
			}
			num4 = Mathf.Max(num4, v.x - vector.x - (border.x + this.padding.x) * 2f);
			float num6 = num4;
			Vector3 vector2 = new Vector3(num6 * 0.5f, -num3 * 0.5f, 0f);
			Vector3 vector3 = new Vector3(num6, num3 + this.padding.y, 1f);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				UILabel uilabel2 = list[j];
				NGUITools.AddWidgetCollider(uilabel2.gameObject);
				uilabel2.autoResizeBoxCollider = false;
				BoxCollider component = uilabel2.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector2.z = component.center.z;
					component.center = vector2;
					component.size = vector3;
				}
				else
				{
					BoxCollider2D component2 = uilabel2.GetComponent<BoxCollider2D>();
					component2.offset = vector2;
					component2.size = vector3;
				}
				j++;
			}
			int width = Mathf.RoundToInt(num4);
			num4 += (border.x + this.padding.x) * 2f;
			num5 -= border.y;
			this.mBackground.width = Mathf.RoundToInt(num4);
			this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
			int k = 0;
			int count3 = list.Count;
			while (k < count3)
			{
				UILabel uilabel3 = list[k];
				uilabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
				uilabel3.width = width;
				k++;
			}
			float num7 = 2f * this.atlas.pixelSize;
			float f = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
			float f2 = num3 + num * num7;
			this.mHighlight.width = Mathf.RoundToInt(f);
			this.mHighlight.height = Mathf.RoundToInt(f2);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uicamera = UICamera.FindCameraForLayer(this.mSelection.layer);
				if (uicamera != null)
				{
					flag = (uicamera.cachedCamera.WorldToViewportPoint(localPosition).y < 0.5f);
				}
			}
			if (this.isAnimated)
			{
				this.AnimateColor(this.mBackground);
				if (Time.timeScale == 0f || Time.timeScale >= 0.1f)
				{
					float bottom = num5 + num3;
					this.Animate(this.mHighlight, flag, bottom);
					int l = 0;
					int count4 = list.Count;
					while (l < count4)
					{
						this.Animate(list[l], flag, bottom);
						l++;
					}
					this.AnimateScale(this.mBackground, flag, bottom);
				}
			}
			if (flag)
			{
				vector.y = v.y - border.y;
				v.y = vector.y + (float)this.mBackground.height;
				v.x = vector.x + (float)this.mBackground.width;
				transform.localPosition = new Vector3(vector.x, v.y - border.y, vector.z);
			}
			else
			{
				v.y = vector.y + border.y;
				vector.y = v.y - (float)this.mBackground.height;
				v.x = vector.x + (float)this.mBackground.width;
			}
			Transform parent = this.mPanel.cachedTransform.parent;
			if (parent != null)
			{
				vector = this.mPanel.cachedTransform.TransformPoint(vector);
				v = this.mPanel.cachedTransform.TransformPoint(v);
				vector = parent.InverseTransformPoint(vector);
				v = parent.InverseTransformPoint(v);
			}
			Vector3 b = (!this.mPanel.hasClipping) ? this.mPanel.CalculateConstrainOffset(vector, v) : Vector3.zero;
			localPosition = transform.localPosition + b;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			transform.localPosition = localPosition;
		}
		else
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x040011C8 RID: 4552
	private const float animSpeed = 0.15f;

	// Token: 0x040011C9 RID: 4553
	public static UIPopupList current;

	// Token: 0x040011CA RID: 4554
	private static GameObject mChild;

	// Token: 0x040011CB RID: 4555
	private static float mFadeOutComplete;

	// Token: 0x040011CC RID: 4556
	public UIAtlas atlas;

	// Token: 0x040011CD RID: 4557
	public UIFont bitmapFont;

	// Token: 0x040011CE RID: 4558
	public Font trueTypeFont;

	// Token: 0x040011CF RID: 4559
	public int fontSize = 16;

	// Token: 0x040011D0 RID: 4560
	public FontStyle fontStyle;

	// Token: 0x040011D1 RID: 4561
	public string backgroundSprite;

	// Token: 0x040011D2 RID: 4562
	public string highlightSprite;

	// Token: 0x040011D3 RID: 4563
	public UIPopupList.Position position;

	// Token: 0x040011D4 RID: 4564
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x040011D5 RID: 4565
	public List<string> items = new List<string>();

	// Token: 0x040011D6 RID: 4566
	public List<object> itemData = new List<object>();

	// Token: 0x040011D7 RID: 4567
	public Vector2 padding = new Vector3(4f, 4f);

	// Token: 0x040011D8 RID: 4568
	public Color textColor = Color.white;

	// Token: 0x040011D9 RID: 4569
	public Color backgroundColor = Color.white;

	// Token: 0x040011DA RID: 4570
	public Color highlightColor = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040011DB RID: 4571
	public bool isAnimated = true;

	// Token: 0x040011DC RID: 4572
	public bool isLocalized;

	// Token: 0x040011DD RID: 4573
	public bool separatePanel = true;

	// Token: 0x040011DE RID: 4574
	public UIPopupList.OpenOn openOn;

	// Token: 0x040011DF RID: 4575
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x040011E0 RID: 4576
	[SerializeField]
	[HideInInspector]
	protected string mSelectedItem;

	// Token: 0x040011E1 RID: 4577
	[HideInInspector]
	[SerializeField]
	protected UIPanel mPanel;

	// Token: 0x040011E2 RID: 4578
	[SerializeField]
	[HideInInspector]
	protected UISprite mBackground;

	// Token: 0x040011E3 RID: 4579
	[SerializeField]
	[HideInInspector]
	protected UISprite mHighlight;

	// Token: 0x040011E4 RID: 4580
	[HideInInspector]
	[SerializeField]
	protected UILabel mHighlightedLabel;

	// Token: 0x040011E5 RID: 4581
	[SerializeField]
	[HideInInspector]
	protected List<UILabel> mLabelList = new List<UILabel>();

	// Token: 0x040011E6 RID: 4582
	[HideInInspector]
	[SerializeField]
	protected float mBgBorder;

	// Token: 0x040011E7 RID: 4583
	[NonSerialized]
	protected GameObject mSelection;

	// Token: 0x040011E8 RID: 4584
	[NonSerialized]
	protected int mOpenFrame;

	// Token: 0x040011E9 RID: 4585
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x040011EA RID: 4586
	[SerializeField]
	[HideInInspector]
	private string functionName = "OnSelectionChange";

	// Token: 0x040011EB RID: 4587
	[SerializeField]
	[HideInInspector]
	private float textScale;

	// Token: 0x040011EC RID: 4588
	[SerializeField]
	[HideInInspector]
	private UIFont font;

	// Token: 0x040011ED RID: 4589
	[SerializeField]
	[HideInInspector]
	private UILabel textLabel;

	// Token: 0x040011EE RID: 4590
	private UIPopupList.LegacyEvent mLegacyEvent;

	// Token: 0x040011EF RID: 4591
	[NonSerialized]
	protected bool mExecuting;

	// Token: 0x040011F0 RID: 4592
	protected bool mUseDynamicFont;

	// Token: 0x040011F1 RID: 4593
	protected bool mTweening;

	// Token: 0x040011F2 RID: 4594
	public GameObject source;

	// Token: 0x0200033F RID: 831
	public enum Position
	{
		// Token: 0x040011F4 RID: 4596
		Auto,
		// Token: 0x040011F5 RID: 4597
		Above,
		// Token: 0x040011F6 RID: 4598
		Below
	}

	// Token: 0x02000340 RID: 832
	public enum OpenOn
	{
		// Token: 0x040011F8 RID: 4600
		ClickOrTap,
		// Token: 0x040011F9 RID: 4601
		RightClick,
		// Token: 0x040011FA RID: 4602
		DoubleClick,
		// Token: 0x040011FB RID: 4603
		Manual
	}

	// Token: 0x020008F1 RID: 2289
	// (Invoke) Token: 0x06005064 RID: 20580
	public delegate void LegacyEvent(string val);
}
