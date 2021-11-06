using System;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class ObjectPrerenderer : MonoBehaviour
{
	// Token: 0x06002371 RID: 9073 RVA: 0x000B0900 File Offset: 0x000AEB00
	private void Awake()
	{
		this._rt = new RenderTexture(32, 32, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000B0948 File Offset: 0x000AEB48
	public void Render_()
	{
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
	}

	// Token: 0x040017EB RID: 6123
	public Camera activeCamera;

	// Token: 0x040017EC RID: 6124
	private RenderTexture _rt;

	// Token: 0x040017ED RID: 6125
	public bool FinishPrerendering;

	// Token: 0x040017EE RID: 6126
	private GameObject _enemiesToRender;
}
