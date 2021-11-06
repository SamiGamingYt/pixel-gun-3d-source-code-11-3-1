using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000850 RID: 2128
[AddComponentMenu("Tasharen/Merge Meshes")]
public class MergeMeshes : MonoBehaviour
{
	// Token: 0x06004D20 RID: 19744 RVA: 0x001BD1AC File Offset: 0x001BB3AC
	private void Start()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	// Token: 0x06004D21 RID: 19745 RVA: 0x001BD1C8 File Offset: 0x001BB3C8
	private void Update()
	{
		if (this.mMerge)
		{
			this.Merge(true);
		}
		base.enabled = false;
	}

	// Token: 0x06004D22 RID: 19746 RVA: 0x001BD1E4 File Offset: 0x001BB3E4
	public void Merge(bool immediate)
	{
		if (!immediate)
		{
			this.mMerge = true;
			base.enabled = true;
			return;
		}
		this.mMerge = false;
		this.mName = base.name;
		this.mFilter = base.GetComponent<MeshFilter>();
		this.mTrans = base.transform;
		this.Clear();
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>();
		if (componentsInChildren.Length == 0 || (this.mFilter != null && componentsInChildren.Length == 1))
		{
			return;
		}
		GameObject gameObject = base.gameObject;
		Matrix4x4 worldToLocalMatrix = gameObject.transform.worldToLocalMatrix;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			if (!(meshFilter == this.mFilter))
			{
				if (meshFilter.gameObject.isStatic)
				{
					Debug.LogError("MergeMeshes can't merge objects marked as static", meshFilter.gameObject);
				}
				else
				{
					Mesh sharedMesh = meshFilter.sharedMesh;
					if (this.material == null)
					{
						this.material = meshFilter.GetComponent<Renderer>().sharedMaterial;
					}
					num += sharedMesh.vertexCount;
					num2 += sharedMesh.triangles.Length;
					if (sharedMesh.normals != null)
					{
						num3 += sharedMesh.normals.Length;
					}
					if (sharedMesh.tangents != null)
					{
						num4 += sharedMesh.tangents.Length;
					}
					if (sharedMesh.colors != null)
					{
						num5 += sharedMesh.colors.Length;
					}
					if (sharedMesh.uv != null)
					{
						num6 += sharedMesh.uv.Length;
					}
					if (sharedMesh.uv2 != null)
					{
						num7 += sharedMesh.uv2.Length;
					}
				}
			}
		}
		if (num == 0)
		{
			Debug.LogWarning("Unable to find any non-static objects to merge", this);
			return;
		}
		Vector3[] array2 = new Vector3[num];
		int[] array3 = new int[num2];
		Vector2[] array4 = (num6 != num) ? null : new Vector2[num];
		Vector2[] array5 = (num7 != num) ? null : new Vector2[num];
		Vector3[] array6 = (num3 != num) ? null : new Vector3[num];
		Vector4[] array7 = (num4 != num) ? null : new Vector4[num];
		Color[] array8 = (num5 != num) ? null : new Color[num];
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		foreach (MeshFilter meshFilter2 in componentsInChildren)
		{
			if (!(meshFilter2 == this.mFilter) && !meshFilter2.gameObject.isStatic)
			{
				Mesh sharedMesh2 = meshFilter2.sharedMesh;
				if (sharedMesh2.vertexCount != 0)
				{
					Matrix4x4 localToWorldMatrix = meshFilter2.transform.localToWorldMatrix;
					Renderer component = meshFilter2.GetComponent<Renderer>();
					if (this.afterMerging != MergeMeshes.PostMerge.DestroyRenderers)
					{
						component.enabled = false;
						this.mDisabledRen.Add(component);
					}
					if (this.afterMerging == MergeMeshes.PostMerge.DisableGameObjects)
					{
						GameObject gameObject2 = meshFilter2.gameObject;
						Transform transform = gameObject2.transform;
						while (transform != this.mTrans)
						{
							if (transform.GetComponent<Rigidbody>() != null)
							{
								gameObject2 = transform.gameObject;
								break;
							}
							transform = transform.parent;
						}
						this.mDisabledGO.Add(gameObject2);
						TWTools.SetActive(gameObject2, false);
					}
					Vector3[] vertices = sharedMesh2.vertices;
					Vector3[] array10 = (array6 == null) ? null : sharedMesh2.normals;
					Vector4[] array11 = (array7 == null) ? null : sharedMesh2.tangents;
					Vector2[] array12 = (array4 == null) ? null : sharedMesh2.uv;
					Vector2[] array13 = (array5 == null) ? null : sharedMesh2.uv2;
					Color[] array14 = (array8 == null) ? null : sharedMesh2.colors;
					int[] triangles = sharedMesh2.triangles;
					int k = 0;
					int num11 = vertices.Length;
					while (k < num11)
					{
						array2[num10] = worldToLocalMatrix.MultiplyPoint3x4(localToWorldMatrix.MultiplyPoint3x4(vertices[k]));
						if (array6 != null)
						{
							array6[num10] = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(array10[k]));
						}
						if (array8 != null)
						{
							array8[num10] = array14[k];
						}
						if (array4 != null)
						{
							array4[num10] = array12[k];
						}
						if (array5 != null)
						{
							array5[num10] = array13[k];
						}
						if (array7 != null)
						{
							Vector4 vector = array11[k];
							Vector3 v = new Vector3(vector.x, vector.y, vector.z);
							v = worldToLocalMatrix.MultiplyVector(localToWorldMatrix.MultiplyVector(v));
							vector.x = v.x;
							vector.y = v.y;
							vector.z = v.z;
							array7[num10] = vector;
						}
						num10++;
						k++;
					}
					int l = 0;
					int num12 = triangles.Length;
					while (l < num12)
					{
						array3[num9++] = num8 + triangles[l];
						l++;
					}
					num8 = num10;
					if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
					{
						UnityEngine.Object.Destroy(meshFilter2.gameObject);
					}
					else if (this.afterMerging == MergeMeshes.PostMerge.DestroyRenderers)
					{
						UnityEngine.Object.Destroy(component);
					}
				}
			}
		}
		if (this.afterMerging == MergeMeshes.PostMerge.DestroyGameObjects)
		{
			this.mDisabledGO.Clear();
		}
		if (array2.Length > 0)
		{
			if (this.mMesh == null)
			{
				this.mMesh = new Mesh();
				this.mMesh.hideFlags = HideFlags.DontSave;
			}
			else
			{
				this.mMesh.Clear();
			}
			this.mMesh.name = this.mName;
			this.mMesh.vertices = array2;
			this.mMesh.normals = array6;
			this.mMesh.tangents = array7;
			this.mMesh.colors = array8;
			this.mMesh.uv = array4;
			this.mMesh.uv2 = array5;
			this.mMesh.triangles = array3;
			this.mMesh.RecalculateBounds();
			if (this.mFilter == null)
			{
				this.mFilter = gameObject.AddComponent<MeshFilter>();
				this.mFilter.mesh = this.mMesh;
			}
			if (this.mRen == null)
			{
				this.mRen = gameObject.AddComponent<MeshRenderer>();
			}
			this.mRen.sharedMaterial = this.material;
			this.mRen.enabled = true;
			gameObject.name = string.Concat(new object[]
			{
				this.mName,
				" (",
				array3.Length / 3,
				" tri)"
			});
		}
		else
		{
			this.Release();
		}
		base.enabled = false;
	}

	// Token: 0x06004D23 RID: 19747 RVA: 0x001BD928 File Offset: 0x001BBB28
	public void Clear()
	{
		int i = 0;
		int count = this.mDisabledGO.Count;
		while (i < count)
		{
			GameObject gameObject = this.mDisabledGO[i];
			if (gameObject)
			{
				TWTools.SetActive(gameObject, true);
			}
			i++;
		}
		int j = 0;
		int count2 = this.mDisabledRen.Count;
		while (j < count2)
		{
			Renderer renderer = this.mDisabledRen[j];
			if (renderer)
			{
				renderer.enabled = true;
			}
			j++;
		}
		this.mDisabledGO.Clear();
		this.mDisabledRen.Clear();
		if (this.mRen != null)
		{
			this.mRen.enabled = false;
		}
	}

	// Token: 0x06004D24 RID: 19748 RVA: 0x001BD9E8 File Offset: 0x001BBBE8
	public void Release()
	{
		this.Clear();
		TWTools.Destroy(this.mRen);
		TWTools.Destroy(this.mFilter);
		TWTools.Destroy(this.mMesh);
		this.mFilter = null;
		this.mMesh = null;
		this.mRen = null;
	}

	// Token: 0x04003B8E RID: 15246
	public Material material;

	// Token: 0x04003B8F RID: 15247
	public MergeMeshes.PostMerge afterMerging;

	// Token: 0x04003B90 RID: 15248
	private string mName;

	// Token: 0x04003B91 RID: 15249
	private Transform mTrans;

	// Token: 0x04003B92 RID: 15250
	private Mesh mMesh;

	// Token: 0x04003B93 RID: 15251
	private MeshFilter mFilter;

	// Token: 0x04003B94 RID: 15252
	private MeshRenderer mRen;

	// Token: 0x04003B95 RID: 15253
	private List<GameObject> mDisabledGO = new List<GameObject>();

	// Token: 0x04003B96 RID: 15254
	private List<Renderer> mDisabledRen = new List<Renderer>();

	// Token: 0x04003B97 RID: 15255
	private bool mMerge = true;

	// Token: 0x02000851 RID: 2129
	public enum PostMerge
	{
		// Token: 0x04003B99 RID: 15257
		DisableRenderers,
		// Token: 0x04003B9A RID: 15258
		DestroyRenderers,
		// Token: 0x04003B9B RID: 15259
		DisableGameObjects,
		// Token: 0x04003B9C RID: 15260
		DestroyGameObjects
	}
}
