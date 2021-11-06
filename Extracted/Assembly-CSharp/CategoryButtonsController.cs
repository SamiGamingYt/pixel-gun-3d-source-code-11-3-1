using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class CategoryButtonsController : MonoBehaviour
{
	// Token: 0x1700002B RID: 43
	// (get) Token: 0x0600024C RID: 588 RVA: 0x00014850 File Offset: 0x00012A50
	public List<Action<BtnCategory>> actions
	{
		get
		{
			if (this._actions == null)
			{
				this._actions = new List<Action<BtnCategory>>();
			}
			return this._actions;
		}
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00014870 File Offset: 0x00012A70
	private void Start()
	{
		this.buttonsTable.Reposition();
		foreach (BtnCategory btnCategory in this.buttons)
		{
			btnCategory.scaleMultypler = this.scaleMultypler;
			btnCategory.animTime = this.animTime;
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x000148C0 File Offset: 0x00012AC0
	public void BtnClicked(string btnName, bool instant = false)
	{
		this.buttons.ForEach(delegate(BtnCategory b)
		{
			b.isDefault = (b.btnName == btnName);
			b.isPressed = b.isDefault;
		});
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		for (int i = 0; i < this.buttons.Length; i++)
		{
			if (this.buttons[i].btnName == btnName)
			{
				this.buttons[i].isPressed = true;
				this.buttons[i].wasPressed = true;
				base.StartCoroutine(this.buttons[i].SetButtonPressed(true, instant));
				this.currentBtnName = btnName;
			}
			else
			{
				this.buttons[i].isPressed = false;
				base.StartCoroutine(this.buttons[i].SetButtonPressed(false, false));
			}
		}
		base.StartCoroutine(this.AnimateButtons());
	}

	// Token: 0x0600024F RID: 591 RVA: 0x000149AC File Offset: 0x00012BAC
	private IEnumerator AnimateButtons()
	{
		float animationTimer = this.animTime;
		while (animationTimer > 0f)
		{
			animationTimer -= Time.unscaledDeltaTime;
			this.buttonsTable.Reposition();
			yield return null;
		}
		if (this.currentBtnName != null)
		{
			BtnCategory currentButton = this.buttons.FirstOrDefault((BtnCategory btn) => btn.btnName == this.currentBtnName);
			while (currentButton != null && currentButton.IsAnimationPlayed)
			{
				yield return null;
				this.buttonsTable.Reposition();
			}
		}
		yield break;
	}

	// Token: 0x04000284 RID: 644
	public BtnCategory[] buttons;

	// Token: 0x04000285 RID: 645
	public float scaleMultypler = 1.1f;

	// Token: 0x04000286 RID: 646
	public float animTime = 0.7f;

	// Token: 0x04000287 RID: 647
	public UITable buttonsTable;

	// Token: 0x04000288 RID: 648
	public string currentBtnName;

	// Token: 0x04000289 RID: 649
	private List<Action<BtnCategory>> _actions;
}
