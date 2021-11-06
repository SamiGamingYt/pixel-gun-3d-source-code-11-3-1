using System;
using UnityEngine;

namespace Photon
{
	// Token: 0x0200040D RID: 1037
	public class MonoBehaviour : MonoBehaviour
	{
		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060024E0 RID: 9440 RVA: 0x000BA1C0 File Offset: 0x000B83C0
		public PhotonView photonView
		{
			get
			{
				if (this.pvCache == null)
				{
					this.pvCache = PhotonView.Get(this);
				}
				return this.pvCache;
			}
		}

		// Token: 0x040019F3 RID: 6643
		private PhotonView pvCache;
	}
}
