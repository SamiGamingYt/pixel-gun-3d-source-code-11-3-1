using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020004C8 RID: 1224
	internal sealed class MaterialInfo
	{
		// Token: 0x06002BB3 RID: 11187 RVA: 0x000E6148 File Offset: 0x000E4348
		public bool AddRenderer(Renderer renderer)
		{
			return this._renderers.Add(renderer);
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000E6158 File Offset: 0x000E4358
		public IList<Renderer> GetRenderers()
		{
			return this._renderers.ToList<Renderer>();
		}

		// Token: 0x040020AA RID: 8362
		private readonly HashSet<Renderer> _renderers = new HashSet<Renderer>();
	}
}
