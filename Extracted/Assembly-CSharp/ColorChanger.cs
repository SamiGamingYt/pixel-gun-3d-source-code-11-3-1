using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class ColorChanger : MonoBehaviour
{
	// Token: 0x06000B6F RID: 2927 RVA: 0x00040878 File Offset: 0x0003EA78
	private void Start()
	{
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		this.meshColors = new Color[this.mesh.vertices.Length];
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x000408B0 File Offset: 0x0003EAB0
	private void Update()
	{
		float num = base.transform.position.magnitude / 3f;
		float r = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad + num));
		float g = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 0.45f + num));
		float b = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 1.2f + num));
		Color color = new Color(r, g, b);
		for (int i = 0; i < this.meshColors.Length; i++)
		{
			this.meshColors[i] = color;
		}
		this.mesh.colors = this.meshColors;
	}

	// Token: 0x04000914 RID: 2324
	private Mesh mesh;

	// Token: 0x04000915 RID: 2325
	private Color[] meshColors;
}
