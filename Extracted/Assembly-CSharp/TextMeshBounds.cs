using System;
using UnityEngine;

// Token: 0x02000859 RID: 2137
[ExecuteInEditMode]
public class TextMeshBounds : MonoBehaviour
{
	// Token: 0x06004D58 RID: 19800 RVA: 0x001BEBB8 File Offset: 0x001BCDB8
	private void Start()
	{
		this.text = base.GetComponent<TextMesh>();
	}

	// Token: 0x06004D59 RID: 19801 RVA: 0x001BEBC8 File Offset: 0x001BCDC8
	private void Update()
	{
		if (!this.text.text.Equals(this.oldText) && this.text.text.Length > 1)
		{
			Quaternion rotation = base.transform.rotation;
			base.transform.rotation = new Quaternion
			{
				eulerAngles = Vector3.zero
			};
			this.ResizeTextByWidth();
			base.transform.rotation = rotation;
		}
		this.oldText = this.text.text;
	}

	// Token: 0x06004D5A RID: 19802 RVA: 0x001BEC58 File Offset: 0x001BCE58
	private void ResizeTextByWidth()
	{
		float x = base.transform.lossyScale.x;
		Vector3 size = this.text.GetComponent<Renderer>().bounds.size;
		while ((size.x > this.textBound.x * x || size.y > this.textBound.y * x) && (double)this.text.characterSize > 0.01)
		{
			this.text.characterSize -= 0.003f;
			size = this.text.GetComponent<Renderer>().bounds.size;
		}
		while (size.x < this.textBound.x * x && size.y < this.textBound.y * x)
		{
			this.text.characterSize += 0.003f;
			size = this.text.GetComponent<Renderer>().bounds.size;
		}
	}

	// Token: 0x06004D5B RID: 19803 RVA: 0x001BED7C File Offset: 0x001BCF7C
	private void OnDrawGizmos()
	{
		Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.transform.rotation, base.transform.lossyScale);
		Gizmos.matrix = matrix;
		Gizmos.DrawWireCube(Vector3.zero, this.boundSize);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, this.textBound);
	}

	// Token: 0x04003BBD RID: 15293
	private GameObject[] outlines = new GameObject[4];

	// Token: 0x04003BBE RID: 15294
	private Vector3 boundSize;

	// Token: 0x04003BBF RID: 15295
	public Vector3 textBound = new Vector3(1f, 0.4f, 0f);

	// Token: 0x04003BC0 RID: 15296
	private TextMesh text;

	// Token: 0x04003BC1 RID: 15297
	private string oldText = string.Empty;

	// Token: 0x04003BC2 RID: 15298
	private bool outlined;
}
