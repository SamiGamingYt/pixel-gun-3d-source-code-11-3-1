using System;
using UnityEngine;

// Token: 0x0200005B RID: 91
public class CapePrerenderer : MonoBehaviour
{
	// Token: 0x06000240 RID: 576 RVA: 0x000141F8 File Offset: 0x000123F8
	private void Awake()
	{
		this._rt = new RenderTexture(512, 512, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00014248 File Offset: 0x00012448
	public Texture Render_()
	{
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		return this._rt;
	}

	// Token: 0x04000274 RID: 628
	public Camera activeCamera;

	// Token: 0x04000275 RID: 629
	private RenderTexture _rt;

	// Token: 0x04000276 RID: 630
	public bool FinishPrerendering;

	// Token: 0x04000277 RID: 631
	private GameObject _enemiesToRender;
}
