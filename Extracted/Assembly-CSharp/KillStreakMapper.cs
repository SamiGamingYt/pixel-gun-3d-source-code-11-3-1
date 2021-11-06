using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
public class KillStreakMapper : MonoBehaviour
{
	// Token: 0x060019C8 RID: 6600 RVA: 0x00067C78 File Offset: 0x00065E78
	private void Start()
	{
		this.atlasSize = new Vector2((float)this.killstreakAtlas.texture.width, (float)this.killstreakAtlas.texture.height);
	}

	// Token: 0x060019C9 RID: 6601 RVA: 0x00067CB4 File Offset: 0x00065EB4
	public void GetStreak(string spriteName)
	{
		int x = this.killstreakAtlas.GetSprite(spriteName).x;
		int y = this.killstreakAtlas.GetSprite(spriteName).y;
		int height = this.killstreakAtlas.GetSprite(spriteName).height;
		int width = this.killstreakAtlas.GetSprite(spriteName).width;
		float y2 = (float)height / (float)width;
		this.killstreakRenderer.transform.localScale = new Vector3(1f, y2, 1f);
		this.killstreakRenderer.material.mainTextureScale = new Vector2((float)width / this.atlasSize.x, (float)height / this.atlasSize.y);
		this.killstreakRenderer.material.mainTextureOffset = new Vector2((float)x / this.atlasSize.x, (this.atlasSize.y - (float)(y + height)) / this.atlasSize.y);
		if (this.killstreakAnimator != null)
		{
			this.killstreakAnimator.SetTrigger("play");
		}
	}

	// Token: 0x04000F05 RID: 3845
	public Renderer killstreakRenderer;

	// Token: 0x04000F06 RID: 3846
	public UIAtlas killstreakAtlas;

	// Token: 0x04000F07 RID: 3847
	private Vector2 atlasSize;

	// Token: 0x04000F08 RID: 3848
	public Animator killstreakAnimator;
}
