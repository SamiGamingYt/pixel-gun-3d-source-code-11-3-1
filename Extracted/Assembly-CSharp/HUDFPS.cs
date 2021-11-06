using System;
using UnityEngine;

// Token: 0x02000652 RID: 1618
[AddComponentMenu("Utilities/HUDFPS")]
public class HUDFPS : MonoBehaviour
{
	// Token: 0x0400291E RID: 10526
	private Rect startRect = new Rect((float)Screen.width * 0.2f, (float)Screen.height - 55f * Defs.Coef, 150f * Defs.Coef, 55f * Defs.Coef);

	// Token: 0x0400291F RID: 10527
	public bool updateColor = true;

	// Token: 0x04002920 RID: 10528
	public bool allowDrag = true;

	// Token: 0x04002921 RID: 10529
	public float frequency = 0.5f;

	// Token: 0x04002922 RID: 10530
	public int nbDecimal = 1;

	// Token: 0x04002923 RID: 10531
	private float accum;

	// Token: 0x04002924 RID: 10532
	private int frames;

	// Token: 0x04002925 RID: 10533
	private Color color = Color.white;

	// Token: 0x04002926 RID: 10534
	private string sFPS = string.Empty;

	// Token: 0x04002927 RID: 10535
	private GUIStyle style;

	// Token: 0x04002928 RID: 10536
	private string maxFPS = "0.0";

	// Token: 0x04002929 RID: 10537
	private string minFPS = "300.0";

	// Token: 0x0400292A RID: 10538
	private string middleFPS = "0.0";

	// Token: 0x0400292B RID: 10539
	private float updateTime = 5f;

	// Token: 0x0400292C RID: 10540
	private float sumFps;

	// Token: 0x0400292D RID: 10541
	private int countFps;
}
