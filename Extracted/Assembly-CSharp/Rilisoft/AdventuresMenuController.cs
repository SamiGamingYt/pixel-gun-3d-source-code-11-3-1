using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000546 RID: 1350
	internal sealed class AdventuresMenuController : MonoBehaviour
	{
		// Token: 0x06002F00 RID: 12032 RVA: 0x000F5814 File Offset: 0x000F3A14
		private void Awake()
		{
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000F5818 File Offset: 0x000F3A18
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x000F5820 File Offset: 0x000F3A20
		private void Refresh()
		{
			this.sandboxButton.gameObject.SetActive(this.IsSandboxEnabled());
			Transform parent = this.sandboxButton.transform.parent;
			float x = (!this.IsSandboxEnabled()) ? (0.5f * this.period) : 0f;
			parent.localPosition = new Vector3(x, parent.localPosition.y, parent.localPosition.z);
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x000F58A0 File Offset: 0x000F3AA0
		private bool IsSandboxEnabled()
		{
			return true;
		}

		// Token: 0x040022AF RID: 8879
		[SerializeField]
		private UIButton sandboxButton;

		// Token: 0x040022B0 RID: 8880
		[SerializeField]
		private float period = 334f;

		// Token: 0x040022B1 RID: 8881
		private float _distance;
	}
}
