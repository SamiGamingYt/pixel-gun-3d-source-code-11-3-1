using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
[Serializable]
public class MaterialToChange
{
	// Token: 0x040004DA RID: 1242
	public string description = "description";

	// Token: 0x040004DB RID: 1243
	public Color[] cicleColors;

	// Token: 0x040004DC RID: 1244
	public Material[] materials;

	// Token: 0x040004DD RID: 1245
	public float[] cicleLerp;

	// Token: 0x040004DE RID: 1246
	public bool changecolor;

	// Token: 0x040004DF RID: 1247
	[HideInInspector]
	public Color currentColor;

	// Token: 0x040004E0 RID: 1248
	[HideInInspector]
	public float currentLerp;
}
