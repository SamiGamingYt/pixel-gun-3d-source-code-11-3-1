using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class BtnCategory : MonoBehaviour
{
	// Token: 0x14000005 RID: 5
	// (add) Token: 0x060001BA RID: 442 RVA: 0x00011080 File Offset: 0x0000F280
	// (remove) Token: 0x060001BB RID: 443 RVA: 0x0001109C File Offset: 0x0000F29C
	public event EventHandler Clicked;

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x060001BC RID: 444 RVA: 0x000110B8 File Offset: 0x0000F2B8
	// (set) Token: 0x060001BD RID: 445 RVA: 0x000110C0 File Offset: 0x0000F2C0
	public bool isEnable
	{
		get
		{
			return this._isEnable;
		}
		set
		{
			this._isEnable = value;
			if (this.lockSprite != null)
			{
				this.lockSprite.SetActive(!this._isEnable);
			}
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x060001BE RID: 446 RVA: 0x000110FC File Offset: 0x0000F2FC
	public bool IsAnimationPlayed
	{
		get
		{
			return this.isAnimationPlayed;
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x00011104 File Offset: 0x0000F304
	private void OnEnable()
	{
		this.ResetButton();
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0001110C File Offset: 0x0000F30C
	private void OnDisable()
	{
		this.ResetButton();
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x00011114 File Offset: 0x0000F314
	private void ResetButton()
	{
		this.isAnimationPlayed = false;
		if (this.isDefault)
		{
			this.btnController.currentBtnName = this.btnName;
			this.isPressed = true;
			this.wasPressed = true;
		}
		else
		{
			this.isPressed = false;
		}
		this.alphaColor = new Color(1f, 1f, 1f, 0.04f);
		this.normalColor = new Color(1f, 1f, 1f, 1f);
		this.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
		if (this.isPressed)
		{
			base.transform.localScale = Vector3.one * this.scaleMultypler;
			this.normalState.color = this.alphaColor;
			this.pressedState.color = this.normalColor;
		}
		else
		{
			base.transform.localScale = Vector3.one;
			this.normalState.color = this.normalColor;
			this.pressedState.color = this.alphaColor;
		}
		this.btnController.buttonsTable.Reposition();
		this.SetDependentsState(this.isPressed);
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00011258 File Offset: 0x0000F458
	public IEnumerator SetButtonPressed(bool isButtonPressed, bool instant = false)
	{
		while (this.isAnimationPlayed)
		{
			yield return null;
		}
		if (isButtonPressed != this.isPressed)
		{
			yield break;
		}
		this.SetDependentsState(isButtonPressed);
		if (isButtonPressed)
		{
			if (instant)
			{
				this.normalState.color = Color.Lerp(this.alphaColor, this.normalColor, 0f);
				this.pressedState.color = Color.Lerp(this.normalColor, this.alphaColor, 0f);
				base.transform.localScale = Vector3.Lerp(Vector3.one * this.scaleMultypler, Vector3.one, 0f);
				this.isAnimationPlayed = false;
			}
			else
			{
				this.isAnimationPlayed = true;
				float animationTimer = this.animTime;
				while (animationTimer > 0f && this.isPressed)
				{
					animationTimer -= Time.unscaledDeltaTime;
					float lerpFactor = animationTimer / this.animTime;
					this.normalState.color = Color.Lerp(this.alphaColor, this.normalColor, lerpFactor);
					this.pressedState.color = Color.Lerp(this.normalColor, this.alphaColor, lerpFactor);
					base.transform.localScale = Vector3.Lerp(Vector3.one * this.scaleMultypler, Vector3.one, lerpFactor);
					yield return null;
				}
				this.isAnimationPlayed = false;
			}
		}
		else if (this.wasPressed)
		{
			this.isAnimationPlayed = true;
			float animationTimer2 = 0f;
			while (animationTimer2 < this.animTime)
			{
				animationTimer2 += Time.unscaledDeltaTime;
				float lerpFactor2 = animationTimer2 / this.animTime;
				this.normalState.color = Color.Lerp(this.alphaColor, this.normalColor, lerpFactor2);
				this.pressedState.color = Color.Lerp(this.normalColor, this.alphaColor, lerpFactor2);
				base.transform.localScale = Vector3.Lerp(Vector3.one * this.scaleMultypler, Vector3.one, lerpFactor2);
				yield return null;
			}
			this.wasPressed = false;
			this.isAnimationPlayed = false;
		}
		else
		{
			this.normalState.color = this.normalColor;
			this.pressedState.color = this.alphaColor;
			base.transform.localScale = Vector3.one;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00011290 File Offset: 0x0000F490
	private void OnClick()
	{
		if (!this.isEnable)
		{
			return;
		}
		EventHandler clicked = this.Clicked;
		if (clicked != null)
		{
			clicked(this, EventArgs.Empty);
		}
		if (!this.isPressed && !this.isAnimationPlayed)
		{
			if (ButtonClickSound.Instance != null)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			this.btnController.BtnClicked(this.btnName, false);
			if (this.btnAnimation != null)
			{
				this.btnAnimation.ResetToBeginning();
				this.btnAnimation.PlayForward();
			}
			try
			{
				if (this.btnController.actions != null && this.btnController.buttons != null)
				{
					int num = this.btnController.buttons.ToList<BtnCategory>().IndexOf(this);
					if (num != -1 && this.btnController.actions.Count > num && this.btnController.actions[num] != null)
					{
						this.btnController.actions[num](this);
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in invoking action in BtnCategory: " + arg);
			}
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x000113E4 File Offset: 0x0000F5E4
	private void OnPress(bool pressed)
	{
		if (!this.isEnable)
		{
			return;
		}
		if (!this.isPressed)
		{
			if (pressed)
			{
				this.normalState.color = this.pressedColor;
			}
			else
			{
				this.normalState.color = this.normalColor;
			}
		}
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00011438 File Offset: 0x0000F638
	private void SetDependentsState(bool buttonSelected)
	{
		foreach (BtnCategoryDependent btnCategoryDependent in this._dependents)
		{
			if (btnCategoryDependent != null && !(btnCategoryDependent.Dependent == null))
			{
				bool flag = (!btnCategoryDependent.InvertVisible) ? buttonSelected : (!buttonSelected);
				if (btnCategoryDependent.Dependent.activeSelf != flag)
				{
					btnCategoryDependent.Dependent.SetActive(flag);
				}
			}
		}
	}

	// Token: 0x040001C2 RID: 450
	private bool _isEnable = true;

	// Token: 0x040001C3 RID: 451
	public GameObject lockSprite;

	// Token: 0x040001C4 RID: 452
	public UISprite normalState;

	// Token: 0x040001C5 RID: 453
	public UISprite pressedState;

	// Token: 0x040001C6 RID: 454
	public UITweener btnAnimation;

	// Token: 0x040001C7 RID: 455
	public CategoryButtonsController btnController;

	// Token: 0x040001C8 RID: 456
	public bool isPressed;

	// Token: 0x040001C9 RID: 457
	public bool isDefault;

	// Token: 0x040001CA RID: 458
	private Color alphaColor;

	// Token: 0x040001CB RID: 459
	private Color normalColor;

	// Token: 0x040001CC RID: 460
	private Color pressedColor;

	// Token: 0x040001CD RID: 461
	[HideInInspector]
	public float scaleMultypler = 1.1f;

	// Token: 0x040001CE RID: 462
	[HideInInspector]
	public float animTime = 0.7f;

	// Token: 0x040001CF RID: 463
	public string btnName;

	// Token: 0x040001D0 RID: 464
	[HideInInspector]
	public bool wasPressed;

	// Token: 0x040001D1 RID: 465
	private bool isAnimationPlayed;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	public List<BtnCategoryDependent> _dependents;
}
