using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004C7 RID: 1223
	internal sealed class ResourceProfiler
	{
		// Token: 0x06002BB1 RID: 11185 RVA: 0x000E6030 File Offset: 0x000E4230
		public void Update()
		{
			Renderer[] array = UnityEngine.Object.FindObjectsOfType<Renderer>();
			foreach (Renderer renderer in array)
			{
				foreach (Material key in renderer.sharedMaterials)
				{
					MaterialInfo materialInfo = null;
					if (!this._materials.TryGetValue(key, out materialInfo))
					{
						materialInfo = new MaterialInfo();
						this._materials.Add(key, materialInfo);
					}
					materialInfo.AddRenderer(renderer);
				}
			}
			MeshFilter[] array3 = UnityEngine.Object.FindObjectsOfType<MeshFilter>();
			foreach (MeshFilter meshFilter in array3)
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				if (sharedMesh != null)
				{
					MeshInfo meshInfo = null;
					if (!this._meshes.TryGetValue(sharedMesh, out meshInfo))
					{
						meshInfo = new MeshInfo();
						this._meshes.Add(sharedMesh, meshInfo);
					}
					meshInfo.AddMeshFilter(meshFilter);
				}
			}
		}

		// Token: 0x040020A8 RID: 8360
		private readonly IDictionary<Material, MaterialInfo> _materials = new Dictionary<Material, MaterialInfo>();

		// Token: 0x040020A9 RID: 8361
		private readonly IDictionary<Mesh, MeshInfo> _meshes = new Dictionary<Mesh, MeshInfo>();
	}
}
