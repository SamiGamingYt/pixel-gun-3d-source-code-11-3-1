using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F3 RID: 1523
	[DisallowMultipleComponent]
	public sealed class NestHighlighter : MonoBehaviour
	{
		// Token: 0x0600342B RID: 13355 RVA: 0x0010E10C File Offset: 0x0010C30C
		private void Awake()
		{
			this._mat = base.GetComponent<Renderer>().sharedMaterial;
			this._baseColor = this._mat.GetColor("_OutlineColor");
			this._colorStart = new Color(this._baseColor.r, this._baseColor.g, this._baseColor.b, 0f);
			this._colorEnd = new Color(this._baseColor.r, this._baseColor.g, this._baseColor.b, 1f);
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x0010E1A4 File Offset: 0x0010C3A4
		private void Update()
		{
			if (Nest.Instance == null || !Nest.Instance.EggIsReady)
			{
				this._mat.SetColor("_OutlineColor", this._colorEnd);
				return;
			}
			this._elapsed += Time.deltaTime;
			if (this._elapsed % 2f < 1f)
			{
				this._mat.SetColor("_OutlineColor", Color.Lerp(this._colorStart, this._colorEnd, this._elapsed % 2f));
			}
			else
			{
				this._mat.SetColor("_OutlineColor", Color.Lerp(this._colorEnd, this._colorStart, this._elapsed % 2f - 1f));
			}
		}

		// Token: 0x0400265F RID: 9823
		private const string COLOR_PORP_NAME = "_OutlineColor";

		// Token: 0x04002660 RID: 9824
		private Material _mat;

		// Token: 0x04002661 RID: 9825
		private Color _baseColor;

		// Token: 0x04002662 RID: 9826
		private Color _colorStart;

		// Token: 0x04002663 RID: 9827
		private Color _colorEnd;

		// Token: 0x04002664 RID: 9828
		private float _elapsed;

		// Token: 0x04002665 RID: 9829
		private bool _forvard = true;
	}
}
