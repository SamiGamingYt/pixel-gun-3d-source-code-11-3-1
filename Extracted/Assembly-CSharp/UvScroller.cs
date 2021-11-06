using System;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class UvScroller : MonoBehaviour
{
	// Token: 0x06004E8A RID: 20106 RVA: 0x001C75D8 File Offset: 0x001C57D8
	private void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			this.rendererGlossnes = base.GetComponent<Renderer>();
		}
	}

	// Token: 0x06004E8B RID: 20107 RVA: 0x001C75F8 File Offset: 0x001C57F8
	private void Update()
	{
		this.ScrollX += Time.unscaledDeltaTime * this.scrollSpeed;
		if (Mathf.Abs(this.ScrollX) >= 1f)
		{
			this.ScrollX = 0f;
		}
		if (this.rendererGlossnes.material.HasProperty("_GlossTex"))
		{
			this.rendererGlossnes.material.SetTextureOffset("_GlossTex", new Vector2(this.ScrollX, 0f));
		}
		else if (this.rendererGlossnes.materials.Length > 1 && this.rendererGlossnes.materials[1].HasProperty("_GlossTex"))
		{
			this.rendererGlossnes.materials[1].SetTextureOffset("_GlossTex", new Vector2(this.ScrollX, 0f));
		}
	}

	// Token: 0x04003D1E RID: 15646
	public float scrollSpeed = -0.5f;

	// Token: 0x04003D1F RID: 15647
	private float ScrollX;

	// Token: 0x04003D20 RID: 15648
	private Renderer rendererGlossnes;
}
