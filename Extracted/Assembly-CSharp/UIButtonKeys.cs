using System;
using UnityEngine;

// Token: 0x0200031D RID: 797
[AddComponentMenu("NGUI/Interaction/Button Keys (Legacy)")]
[ExecuteInEditMode]
public class UIButtonKeys : UIKeyNavigation
{
	// Token: 0x06001BAF RID: 7087 RVA: 0x00071DA4 File Offset: 0x0006FFA4
	protected override void OnEnable()
	{
		this.Upgrade();
		base.OnEnable();
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x00071DB4 File Offset: 0x0006FFB4
	public void Upgrade()
	{
		if (this.onClick == null && this.selectOnClick != null)
		{
			this.onClick = this.selectOnClick.gameObject;
			this.selectOnClick = null;
			NGUITools.SetDirty(this);
		}
		if (this.onLeft == null && this.selectOnLeft != null)
		{
			this.onLeft = this.selectOnLeft.gameObject;
			this.selectOnLeft = null;
			NGUITools.SetDirty(this);
		}
		if (this.onRight == null && this.selectOnRight != null)
		{
			this.onRight = this.selectOnRight.gameObject;
			this.selectOnRight = null;
			NGUITools.SetDirty(this);
		}
		if (this.onUp == null && this.selectOnUp != null)
		{
			this.onUp = this.selectOnUp.gameObject;
			this.selectOnUp = null;
			NGUITools.SetDirty(this);
		}
		if (this.onDown == null && this.selectOnDown != null)
		{
			this.onDown = this.selectOnDown.gameObject;
			this.selectOnDown = null;
			NGUITools.SetDirty(this);
		}
	}

	// Token: 0x040010C6 RID: 4294
	public UIButtonKeys selectOnClick;

	// Token: 0x040010C7 RID: 4295
	public UIButtonKeys selectOnUp;

	// Token: 0x040010C8 RID: 4296
	public UIButtonKeys selectOnDown;

	// Token: 0x040010C9 RID: 4297
	public UIButtonKeys selectOnLeft;

	// Token: 0x040010CA RID: 4298
	public UIButtonKeys selectOnRight;
}
