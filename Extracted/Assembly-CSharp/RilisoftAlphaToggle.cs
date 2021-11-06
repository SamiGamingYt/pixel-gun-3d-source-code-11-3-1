using System;
using UnityEngine;

// Token: 0x02000741 RID: 1857
[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(UIToggle))]
public class RilisoftAlphaToggle : MonoBehaviour
{
	// Token: 0x06004154 RID: 16724 RVA: 0x0015C2C8 File Offset: 0x0015A4C8
	private void Start()
	{
		this._toggle = base.GetComponent<UIToggle>();
		this._toggledSprite = this._toggle.GetComponent<UISprite>();
		if (this._toggle != null && this._toggledSprite != null)
		{
			this.OnAlphaChange();
			EventDelegate.Add(this._toggle.onChange, new EventDelegate.Callback(this.OnAlphaChange));
		}
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x0015C338 File Offset: 0x0015A538
	public void OnAlphaChange()
	{
		if (this._toggle.value)
		{
			this._toggledSprite.alpha = this.alphaOnState;
		}
		else
		{
			this._toggledSprite.alpha = this.alphaOffState;
		}
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x0015C374 File Offset: 0x0015A574
	private void OnDestroy()
	{
		if (this._toggle != null)
		{
			EventDelegate.Remove(this._toggle.onChange, new EventDelegate.Callback(this.OnAlphaChange));
		}
	}

	// Token: 0x04002FBE RID: 12222
	[Range(0f, 1f)]
	public float alphaOnState;

	// Token: 0x04002FBF RID: 12223
	[Range(0f, 1f)]
	public float alphaOffState;

	// Token: 0x04002FC0 RID: 12224
	private UIToggle _toggle;

	// Token: 0x04002FC1 RID: 12225
	private UISprite _toggledSprite;
}
