using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class ColorChangerVertex : MonoBehaviour
{
	// Token: 0x06000B72 RID: 2930 RVA: 0x00040970 File Offset: 0x0003EB70
	private void Start()
	{
		this.mesh = base.GetComponent<MeshFilter>().mesh;
		this.meshColors = new Color[this.mesh.vertices.Length];
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000409A8 File Offset: 0x0003EBA8
	private void Update()
	{
		for (int i = 0; i < this.meshColors.Length; i++)
		{
			float magnitude = this.mesh.vertices[i].magnitude;
			float r = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad + magnitude));
			float g = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 0.45f + magnitude));
			float b = Mathf.Abs(Mathf.Sin(Time.timeSinceLevelLoad * 1.2f + magnitude));
			Color color = new Color(r, g, b);
			this.meshColors[i] = color;
		}
		this.mesh.colors = this.meshColors;
	}

	// Token: 0x04000916 RID: 2326
	private Mesh mesh;

	// Token: 0x04000917 RID: 2327
	private Color[] meshColors;
}
