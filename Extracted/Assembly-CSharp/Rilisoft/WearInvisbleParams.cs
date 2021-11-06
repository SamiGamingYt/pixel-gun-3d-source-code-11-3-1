using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200078F RID: 1935
	[RequireComponent(typeof(Renderer))]
	public class WearInvisbleParams : MonoBehaviour
	{
		// Token: 0x06004550 RID: 17744 RVA: 0x00175F94 File Offset: 0x00174194
		private void Awake()
		{
			this._rend = base.GetComponent<Renderer>();
			if (this._rend == null)
			{
				return;
			}
			this.BaseShader = this._rend.sharedMaterial.shader.name;
		}

		// Token: 0x040032D1 RID: 13009
		public bool SkipSetInvisible;

		// Token: 0x040032D2 RID: 13010
		public bool HideIsInvisible;

		// Token: 0x040032D3 RID: 13011
		public string InvisibleShader = "Mobile/Transparent-Shop";

		// Token: 0x040032D4 RID: 13012
		[ReadOnly]
		public string BaseShader;

		// Token: 0x040032D5 RID: 13013
		private Renderer _rend;
	}
}
