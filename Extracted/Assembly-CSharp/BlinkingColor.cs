using System;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x020006AA RID: 1706
internal sealed class BlinkingColor : MonoBehaviour
{
	// Token: 0x06003BAC RID: 15276 RVA: 0x00135B8C File Offset: 0x00133D8C
	private void Awake()
	{
		this._colorNameId = Shader.PropertyToID(this.nameColor);
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x00135BA0 File Offset: 0x00133DA0
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component)
		{
			this.mainMaterial = component.sharedMaterial;
			if (this.mainMaterial)
			{
				this.cashColor = this.mainMaterial.GetColor(this._colorNameId);
			}
		}
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x00135BF4 File Offset: 0x00133DF4
	private void OnDestroy()
	{
		this.ResetColor();
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x00135BFC File Offset: 0x00133DFC
	private void Update()
	{
		if (this.IsActive)
		{
			if (this.mainMaterial)
			{
				this.mainMaterial.SetColor(this._colorNameId, this.curColor);
			}
			if (!this.startBlink)
			{
				this.SetColorTwo();
			}
		}
		else if (this.startBlink)
		{
			this.ResetColor();
		}
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x00135C64 File Offset: 0x00133E64
	private void ResetColor()
	{
		if (this.mainMaterial)
		{
			this.mainMaterial.SetColor(this._colorNameId, this.cashColor);
		}
		this.startBlink = false;
		HOTween.Kill(this);
	}

	// Token: 0x06003BB1 RID: 15281 RVA: 0x00135C9C File Offset: 0x00133E9C
	private void SetColorOne()
	{
		this.startBlink = true;
		HOTween.To(this, this.speed, new TweenParms().Prop("curColor", this.normal).Ease(EaseType.Linear).OnComplete(new TweenDelegate.TweenCallback(this.SetColorTwo)));
	}

	// Token: 0x06003BB2 RID: 15282 RVA: 0x00135CF0 File Offset: 0x00133EF0
	private void SetColorTwo()
	{
		this.startBlink = true;
		HOTween.To(this, this.speed, new TweenParms().Prop("curColor", this.blink).Ease(EaseType.Linear).OnComplete(new TweenDelegate.TweenCallback(this.SetColorOne)));
	}

	// Token: 0x04002C19 RID: 11289
	private Material mainMaterial;

	// Token: 0x04002C1A RID: 11290
	public bool IsActive = true;

	// Token: 0x04002C1B RID: 11291
	public string nameColor = "_MainColor";

	// Token: 0x04002C1C RID: 11292
	public float speed = 1f;

	// Token: 0x04002C1D RID: 11293
	public Color normal;

	// Token: 0x04002C1E RID: 11294
	public Color blink;

	// Token: 0x04002C1F RID: 11295
	[HideInInspector]
	public Color curColor;

	// Token: 0x04002C20 RID: 11296
	private Color cashColor;

	// Token: 0x04002C21 RID: 11297
	private bool startBlink;

	// Token: 0x04002C22 RID: 11298
	private int _colorNameId;
}
