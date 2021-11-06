using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006BE RID: 1726
	[RequireComponent(typeof(Renderer))]
	public class MaterialColorSwitcher : MonoBehaviour
	{
		// Token: 0x06003C31 RID: 15409 RVA: 0x00138390 File Offset: 0x00136590
		private void Awake()
		{
			this._mat = base.GetComponent<Renderer>().material;
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x001383A4 File Offset: 0x001365A4
		private void OnEnable()
		{
			base.StopAllCoroutines();
			this._changed = true;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x001383B4 File Offset: 0x001365B4
		private void Update()
		{
			if (this._changed)
			{
				this._changed = false;
				this._colorIdx = ((this.Colors.Count - 1 <= this._colorIdx) ? 0 : (this._colorIdx + 1));
				base.StartCoroutine(this.ChangeColor(this._colorIdx, this.ToColorTime));
			}
		}

		// Token: 0x06003C34 RID: 15412 RVA: 0x00138418 File Offset: 0x00136618
		private IEnumerator ChangeColor(int toIdx, float time)
		{
			Color startColor = this._mat.color;
			Color color = this.Colors[toIdx];
			float elapsed = 0f;
			while (elapsed < time)
			{
				elapsed += Time.deltaTime;
				this._mat.color = Color.Lerp(startColor, color, elapsed / time);
				yield return null;
			}
			this._changed = true;
			yield break;
		}

		// Token: 0x04002C6E RID: 11374
		public List<Color> Colors = new List<Color>();

		// Token: 0x04002C6F RID: 11375
		public float ToColorTime = 1f;

		// Token: 0x04002C70 RID: 11376
		private Material _mat;

		// Token: 0x04002C71 RID: 11377
		private int _colorIdx = -1;

		// Token: 0x04002C72 RID: 11378
		private bool _changed = true;
	}
}
