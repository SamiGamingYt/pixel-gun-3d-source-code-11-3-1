using System;
using UnityEngine;

// Token: 0x02000521 RID: 1313
internal sealed class AdjustSpriteToLabel : MonoBehaviour
{
	// Token: 0x06002DBD RID: 11709 RVA: 0x000F08AC File Offset: 0x000EEAAC
	private void Start()
	{
		this._sprite = base.GetComponent<UISprite>();
		if (this.label == null)
		{
			this.label = base.transform.parent.GetComponent<UILabel>();
		}
		if (this._sprite == null)
		{
			Debug.LogWarning("sprite == null");
		}
		if (this.label == null)
		{
			Debug.LogWarning("label == null");
		}
	}

	// Token: 0x06002DBE RID: 11710 RVA: 0x000F0924 File Offset: 0x000EEB24
	private void Update()
	{
		if (this._sprite == null)
		{
			return;
		}
		if (this.label == null)
		{
			return;
		}
		this._sprite.transform.localPosition = new Vector3(this.padding + 0.5f * (float)this.label.width, 0f, 0f);
	}

	// Token: 0x0400222A RID: 8746
	public UILabel label;

	// Token: 0x0400222B RID: 8747
	[Range(0f, 100f)]
	public float padding = 30f;

	// Token: 0x0400222C RID: 8748
	private UISprite _sprite;
}
