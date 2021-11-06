using System;
using UnityEngine;

// Token: 0x02000378 RID: 888
public class UIGeometry
{
	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06001F13 RID: 7955 RVA: 0x0008EAB8 File Offset: 0x0008CCB8
	public bool hasVertices
	{
		get
		{
			return this.verts.size > 0;
		}
	}

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0008EAC8 File Offset: 0x0008CCC8
	public bool hasTransformed
	{
		get
		{
			return this.mRtpVerts != null && this.mRtpVerts.size > 0 && this.mRtpVerts.size == this.verts.size;
		}
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x0008EB04 File Offset: 0x0008CD04
	public void Clear()
	{
		this.verts.Clear();
		this.uvs.Clear();
		this.cols.Clear();
		this.mRtpVerts.Clear();
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x0008EB40 File Offset: 0x0008CD40
	public void ApplyTransform(Matrix4x4 widgetToPanel, bool generateNormals = true)
	{
		if (this.verts.size > 0)
		{
			this.mRtpVerts.Clear();
			int i = 0;
			int size = this.verts.size;
			while (i < size)
			{
				this.mRtpVerts.Add(widgetToPanel.MultiplyPoint3x4(this.verts[i]));
				i++;
			}
			if (generateNormals)
			{
				this.mRtpNormal = widgetToPanel.MultiplyVector(Vector3.back).normalized;
				Vector3 normalized = widgetToPanel.MultiplyVector(Vector3.right).normalized;
				this.mRtpTan = new Vector4(normalized.x, normalized.y, normalized.z, -1f);
			}
		}
		else
		{
			this.mRtpVerts.Clear();
		}
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x0008EC10 File Offset: 0x0008CE10
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		if (this.mRtpVerts != null && this.mRtpVerts.size > 0)
		{
			if (n == null)
			{
				for (int i = 0; i < this.mRtpVerts.size; i++)
				{
					v.Add(this.mRtpVerts.buffer[i]);
					u.Add(this.uvs.buffer[i]);
					c.Add(this.cols.buffer[i]);
				}
			}
			else
			{
				for (int j = 0; j < this.mRtpVerts.size; j++)
				{
					v.Add(this.mRtpVerts.buffer[j]);
					u.Add(this.uvs.buffer[j]);
					c.Add(this.cols.buffer[j]);
					n.Add(this.mRtpNormal);
					t.Add(this.mRtpTan);
				}
			}
		}
	}

	// Token: 0x040013AE RID: 5038
	public BetterList<Vector3> verts = new BetterList<Vector3>();

	// Token: 0x040013AF RID: 5039
	public BetterList<Vector2> uvs = new BetterList<Vector2>();

	// Token: 0x040013B0 RID: 5040
	public BetterList<Color32> cols = new BetterList<Color32>();

	// Token: 0x040013B1 RID: 5041
	private BetterList<Vector3> mRtpVerts = new BetterList<Vector3>();

	// Token: 0x040013B2 RID: 5042
	private Vector3 mRtpNormal;

	// Token: 0x040013B3 RID: 5043
	private Vector4 mRtpTan;
}
