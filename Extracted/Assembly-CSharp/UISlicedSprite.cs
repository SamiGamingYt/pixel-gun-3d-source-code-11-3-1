using System;
using UnityEngine;

// Token: 0x02000314 RID: 788
[ExecuteInEditMode]
public class UISlicedSprite : UISprite
{
	// Token: 0x170004C0 RID: 1216
	// (get) Token: 0x06001B7B RID: 7035 RVA: 0x000709AC File Offset: 0x0006EBAC
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Sliced;
		}
	}
}
