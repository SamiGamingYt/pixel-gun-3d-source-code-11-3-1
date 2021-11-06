using System;
using UnityEngine;

// Token: 0x02000315 RID: 789
[ExecuteInEditMode]
public class UITiledSprite : UISlicedSprite
{
	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06001B7D RID: 7037 RVA: 0x000709B8 File Offset: 0x0006EBB8
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Tiled;
		}
	}
}
