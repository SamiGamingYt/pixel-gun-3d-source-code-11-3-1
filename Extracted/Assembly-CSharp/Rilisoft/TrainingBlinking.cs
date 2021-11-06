using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000772 RID: 1906
	internal sealed class TrainingBlinking : MonoBehaviour
	{
		// Token: 0x0600431D RID: 17181 RVA: 0x00166920 File Offset: 0x00164B20
		public void SetSprites(IList<UISprite> sprites)
		{
			this.RestoreColorTints();
			this._sprites.Clear();
			if (sprites == null)
			{
				return;
			}
			foreach (UISprite uisprite in sprites)
			{
				if (!(uisprite == null))
				{
					this._sprites.Add(new KeyValuePair<UISprite, Color>(uisprite, uisprite.color));
				}
			}
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x001669B8 File Offset: 0x00164BB8
		private void OnDisable()
		{
			this.RestoreColorTints();
			this._sprites.Clear();
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x001669D4 File Offset: 0x00164BD4
		private void OnDestroy()
		{
			this.RestoreColorTints();
			this._sprites.Clear();
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x001669E8 File Offset: 0x00164BE8
		private void Update()
		{
			float interpolationCoefficient = TrainingBlinking.GetInterpolationCoefficient(Time.time);
			foreach (KeyValuePair<UISprite, Color> keyValuePair in this._sprites)
			{
				keyValuePair.Key.color = Color.Lerp(keyValuePair.Value, Color.green, interpolationCoefficient);
			}
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x00166A70 File Offset: 0x00164C70
		private static float GetInterpolationCoefficient(float time)
		{
			float num = time - Mathf.Floor(time);
			return 1f - num * num;
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x00166A94 File Offset: 0x00164C94
		private void RestoreColorTints()
		{
			foreach (KeyValuePair<UISprite, Color> keyValuePair in this._sprites)
			{
				keyValuePair.Key.color = keyValuePair.Value;
			}
		}

		// Token: 0x04003124 RID: 12580
		private readonly List<KeyValuePair<UISprite, Color>> _sprites = new List<KeyValuePair<UISprite, Color>>(5);
	}
}
