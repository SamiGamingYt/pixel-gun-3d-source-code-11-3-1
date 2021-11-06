using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005A4 RID: 1444
	public sealed class ButtonHandler : MonoBehaviour
	{
		// Token: 0x14000048 RID: 72
		// (add) Token: 0x06003218 RID: 12824 RVA: 0x00103F5C File Offset: 0x0010215C
		// (remove) Token: 0x06003219 RID: 12825 RVA: 0x00103F78 File Offset: 0x00102178
		public event EventHandler Clicked;

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x0600321A RID: 12826 RVA: 0x00103F94 File Offset: 0x00102194
		public bool HasClickedHandlers
		{
			get
			{
				return this.Clicked != null;
			}
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x00103FA4 File Offset: 0x001021A4
		private void OnClick()
		{
			if (!this.isEnable)
			{
				return;
			}
			if (ButtonClickSound.Instance != null && !this.noSound)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			EventHandler clicked = this.Clicked;
			if (clicked != null)
			{
				clicked(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x00103FFC File Offset: 0x001021FC
		public void DoClick()
		{
			if (!this.isEnable)
			{
				return;
			}
			this.OnClick();
		}

		// Token: 0x040024DB RID: 9435
		public bool noSound;

		// Token: 0x040024DC RID: 9436
		[NonSerialized]
		public bool isEnable = true;
	}
}
