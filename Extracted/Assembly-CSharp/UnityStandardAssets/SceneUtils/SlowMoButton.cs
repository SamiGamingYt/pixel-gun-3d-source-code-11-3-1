using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.SceneUtils
{
	// Token: 0x020004E6 RID: 1254
	public class SlowMoButton : MonoBehaviour
	{
		// Token: 0x06002C74 RID: 11380 RVA: 0x000EBBE0 File Offset: 0x000E9DE0
		private void Start()
		{
			this.m_SlowMo = false;
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000EBBEC File Offset: 0x000E9DEC
		private void OnDestroy()
		{
			Time.timeScale = 1f;
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000EBBF8 File Offset: 0x000E9DF8
		public void ChangeSpeed()
		{
			this.m_SlowMo = !this.m_SlowMo;
			Image image = this.button.targetGraphic as Image;
			if (image != null)
			{
				image.sprite = ((!this.m_SlowMo) ? this.FullSpeedTex : this.SlowSpeedTex);
			}
			this.button.targetGraphic = image;
			Time.timeScale = ((!this.m_SlowMo) ? this.fullSpeed : this.slowSpeed);
		}

		// Token: 0x04002185 RID: 8581
		public Sprite FullSpeedTex;

		// Token: 0x04002186 RID: 8582
		public Sprite SlowSpeedTex;

		// Token: 0x04002187 RID: 8583
		public float fullSpeed = 1f;

		// Token: 0x04002188 RID: 8584
		public float slowSpeed = 0.3f;

		// Token: 0x04002189 RID: 8585
		public Button button;

		// Token: 0x0400218A RID: 8586
		private bool m_SlowMo;
	}
}
