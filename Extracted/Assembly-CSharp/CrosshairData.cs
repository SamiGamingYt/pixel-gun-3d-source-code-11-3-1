using System;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
[Serializable]
public class CrosshairData
{
	// Token: 0x040025A2 RID: 9634
	public int ID;

	// Token: 0x040025A3 RID: 9635
	public string Name;

	// Token: 0x040025A4 RID: 9636
	public Texture2D PreviewTexture;

	// Token: 0x040025A5 RID: 9637
	public CrosshairData.aimSprite center = new CrosshairData.aimSprite("aim_1", new Vector2(6f, 6f), new Vector2(0f, 0f));

	// Token: 0x040025A6 RID: 9638
	public CrosshairData.aimSprite up = new CrosshairData.aimSprite("aim_2", new Vector2(6f, 10f), new Vector2(0f, 5f));

	// Token: 0x040025A7 RID: 9639
	public CrosshairData.aimSprite leftUp = new CrosshairData.aimSprite(string.Empty, new Vector2(12f, 12f), new Vector2(8f, 8f));

	// Token: 0x040025A8 RID: 9640
	public CrosshairData.aimSprite left = new CrosshairData.aimSprite("aim_2", new Vector2(10f, 6f), new Vector2(5f, 0f));

	// Token: 0x040025A9 RID: 9641
	public CrosshairData.aimSprite leftDown = new CrosshairData.aimSprite(string.Empty, new Vector2(0f, 0f), new Vector2(0f, 0f));

	// Token: 0x040025AA RID: 9642
	public CrosshairData.aimSprite down = new CrosshairData.aimSprite("aim_3", new Vector2(6f, 10f), new Vector2(0f, 5f));

	// Token: 0x020005D1 RID: 1489
	[Serializable]
	public class aimSprite
	{
		// Token: 0x0600333C RID: 13116 RVA: 0x00109224 File Offset: 0x00107424
		public aimSprite(string name, Vector2 size, Vector2 pos)
		{
			this.spriteName = name;
			this.spriteSize = size;
			this.offset = pos;
		}

		// Token: 0x040025AB RID: 9643
		public string spriteName;

		// Token: 0x040025AC RID: 9644
		public Vector2 spriteSize;

		// Token: 0x040025AD RID: 9645
		public Vector2 offset;
	}
}
