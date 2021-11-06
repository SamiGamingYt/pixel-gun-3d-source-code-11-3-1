using System;
using UnityEngine;

// Token: 0x02000312 RID: 786
[ExecuteInEditMode]
public class UIFilledSprite : UISprite
{
	// Token: 0x170004BF RID: 1215
	// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00070998 File Offset: 0x0006EB98
	public override UIBasicSprite.Type type
	{
		get
		{
			return UIBasicSprite.Type.Filled;
		}
	}
}
