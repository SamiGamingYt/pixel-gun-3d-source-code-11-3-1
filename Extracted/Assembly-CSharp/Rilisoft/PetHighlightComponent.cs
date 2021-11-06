using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006F1 RID: 1777
	public class PetHighlightComponent : MonoBehaviour
	{
		// Token: 0x06003DC4 RID: 15812 RVA: 0x001410F8 File Offset: 0x0013F2F8
		private void Awake()
		{
			this._baseTexture = this._rend.material.mainTexture;
			if (this._rend.material.HasProperty(this._shaderColorProp))
			{
				this._baseColor = this._rend.material.GetColor(this._shaderColorProp);
			}
			else
			{
				Debug.LogError(string.Format("shader property '{0}' not found", this._shaderColorProp));
			}
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x0014116C File Offset: 0x0013F36C
		private void OnDisable()
		{
			this.ResetHit();
			this.ImmortalBlinkStop();
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x0014117C File Offset: 0x0013F37C
		public void Hit()
		{
			base.StopCoroutine(this.DamageCoroutine());
			base.StartCoroutine(this.DamageCoroutine());
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x001411A4 File Offset: 0x0013F3A4
		private void ResetHit()
		{
			this._damageCoroutineIsRunnig = false;
			this._rend.material.mainTexture = this._baseTexture;
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x001411C4 File Offset: 0x0013F3C4
		private IEnumerator DamageCoroutine()
		{
			if (this._damageCoroutineIsRunnig)
			{
				this.ResetHit();
				yield return null;
			}
			this._damageCoroutineIsRunnig = true;
			this._rend.material.mainTexture = this._damageTexture;
			yield return new WaitForSeconds(this._splashTime);
			this.ResetHit();
			yield break;
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x001411E0 File Offset: 0x0013F3E0
		public void ImmortalBlinkStart(float time)
		{
			if (!this._immortalBlinkStarted)
			{
				base.StartCoroutine("ImmortalBlinkCoroutine");
				this._immortalBlinkStarted = true;
			}
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x00141200 File Offset: 0x0013F400
		public void ImmortalBlinkStop()
		{
			base.StopCoroutine("ImmortalBlinkCoroutine");
			this._immortalBlinkStarted = false;
			this.SetColor(this._baseColor);
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x00141220 File Offset: 0x0013F420
		private IEnumerator ImmortalBlinkCoroutine()
		{
			float loopTime = 0.4f;
			int loopsCount = 1;
			float elapsedTime = 0f;
			for (;;)
			{
				elapsedTime += Time.deltaTime;
				float lv = (loopsCount % 2 == 0) ? (elapsedTime / loopTime * -1f) : (elapsedTime / loopTime);
				this.SetColor(Color.Lerp(this._baseColor, this._immortalColor, lv));
				if (elapsedTime > loopTime)
				{
					elapsedTime = 0f;
					loopsCount++;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x0014123C File Offset: 0x0013F43C
		private void SetColor(Color color)
		{
			if (this._rend.material.HasProperty(this._shaderColorProp))
			{
				this._rend.material.SetColor(this._shaderColorProp, color);
			}
			else
			{
				Debug.LogError(string.Format("shader property '{0}' not found", this._shaderColorProp));
			}
		}

		// Token: 0x04002D91 RID: 11665
		[SerializeField]
		private Texture2D _damageTexture;

		// Token: 0x04002D92 RID: 11666
		[SerializeField]
		private Renderer _rend;

		// Token: 0x04002D93 RID: 11667
		[SerializeField]
		private Color _immortalColor;

		// Token: 0x04002D94 RID: 11668
		[SerializeField]
		[Range(0f, 2f)]
		private float _splashTime = 0.3f;

		// Token: 0x04002D95 RID: 11669
		[SerializeField]
		[ReadOnly]
		private Texture _baseTexture;

		// Token: 0x04002D96 RID: 11670
		private Color _baseColor;

		// Token: 0x04002D97 RID: 11671
		[SerializeField]
		private string _shaderColorProp = "_ColorRili";

		// Token: 0x04002D98 RID: 11672
		private bool _damageCoroutineIsRunnig;

		// Token: 0x04002D99 RID: 11673
		private bool _immortalBlinkStarted;
	}
}
