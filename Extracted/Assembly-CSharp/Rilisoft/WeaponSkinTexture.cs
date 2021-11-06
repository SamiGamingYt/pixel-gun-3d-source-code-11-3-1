using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000887 RID: 2183
	[Serializable]
	public class WeaponSkinTexture
	{
		// Token: 0x06004E92 RID: 20114 RVA: 0x001C7B84 File Offset: 0x001C5D84
		public WeaponSkinTexture(string raw, int w, int h, string[] toObjects = null)
		{
			this.Raw = raw;
			this.W = w;
			this.H = h;
			this.ToObjects = toObjects;
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004E93 RID: 20115 RVA: 0x001C7BCC File Offset: 0x001C5DCC
		public Texture2D Texture
		{
			get
			{
				if (this._texture == null)
				{
					byte[] data = Convert.FromBase64String(this.Raw);
					this._texture = new Texture2D(this.W, this.H, TextureFormat.RGBA32, false);
					this._texture.LoadImage(data);
					this._texture.filterMode = this.FilterMode;
					this._texture.Apply();
				}
				return this._texture;
			}
		}

		// Token: 0x04003D2A RID: 15658
		[StringTexture]
		public string Raw;

		// Token: 0x04003D2B RID: 15659
		public int W;

		// Token: 0x04003D2C RID: 15660
		public int H;

		// Token: 0x04003D2D RID: 15661
		public FilterMode FilterMode;

		// Token: 0x04003D2E RID: 15662
		public string[] ToObjects = new string[0];

		// Token: 0x04003D2F RID: 15663
		public string ShaderPropertyName = "_MainTex";

		// Token: 0x04003D30 RID: 15664
		private Texture2D _texture;
	}
}
