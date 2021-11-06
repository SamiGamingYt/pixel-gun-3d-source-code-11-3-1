using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004C9 RID: 1225
	internal sealed class MeshInfo
	{
		// Token: 0x06002BB6 RID: 11190 RVA: 0x000E617C File Offset: 0x000E437C
		public bool AddMeshFilter(MeshFilter meshFilter)
		{
			return this._meshFilters.Add(meshFilter);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000E618C File Offset: 0x000E438C
		public IList<MeshFilter> GetRenderers()
		{
			return this._meshFilters.ToList<MeshFilter>();
		}

		// Token: 0x040020AB RID: 8363
		private readonly HashSet<MeshFilter> _meshFilters = new HashSet<MeshFilter>();
	}
}
