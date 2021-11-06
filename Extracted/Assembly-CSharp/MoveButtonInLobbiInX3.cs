using System;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class MoveButtonInLobbiInX3 : MonoBehaviour
{
	// Token: 0x06001B4E RID: 6990 RVA: 0x00070100 File Offset: 0x0006E300
	private void Start()
	{
		this.myTransform = base.transform;
		this.yNotX3 = this.myTransform.localPosition.y;
		this.Move();
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x00070138 File Offset: 0x0006E338
	private void Move()
	{
		if (this.oldStateX3 != PromoActionsManager.sharedManager.IsEventX3Active)
		{
			this.oldStateX3 = PromoActionsManager.sharedManager.IsEventX3Active;
			this.myTransform.localPosition = new Vector3(this.myTransform.localPosition.x, (!this.oldStateX3) ? this.yNotX3 : this.yX3, this.myTransform.localPosition.z);
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000701BC File Offset: 0x0006E3BC
	private void Update()
	{
		this.Move();
	}

	// Token: 0x04001077 RID: 4215
	public float yX3;

	// Token: 0x04001078 RID: 4216
	private Transform myTransform;

	// Token: 0x04001079 RID: 4217
	private float yNotX3;

	// Token: 0x0400107A RID: 4218
	private bool oldStateX3;
}
