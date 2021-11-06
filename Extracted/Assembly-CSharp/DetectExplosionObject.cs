using System;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class DetectExplosionObject : BaseExplosionObject
{
	// Token: 0x0600349A RID: 13466 RVA: 0x0010FEAC File Offset: 0x0010E0AC
	private void SetEnableDetectCollider(bool enable)
	{
		if (base.GetComponent<Collider>() == null)
		{
			return;
		}
		base.GetComponent<Collider>().enabled = enable;
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x0010FECC File Offset: 0x0010E0CC
	private void Awake()
	{
		this.SetEnableDetectCollider(false);
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x0010FED8 File Offset: 0x0010E0D8
	private void OnTriggerEnter(Collider collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x0010FEE8 File Offset: 0x0010E0E8
	private void OnCollisionEnter(Collision collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x0010FEF8 File Offset: 0x0010E0F8
	private void CollisionEvent(GameObject collisionObj)
	{
		if (!base.IsTargetAvailable(collisionObj.transform.root))
		{
			return;
		}
		if (this._isEnter)
		{
			return;
		}
		this._isEnter = true;
		if (this.durationBeforeExplosion != 0f)
		{
			base.Invoke("RunExplosion", this.durationBeforeExplosion);
		}
		else
		{
			base.RunExplosion();
		}
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x0010FF5C File Offset: 0x0010E15C
	protected override void InitializeData()
	{
		base.InitializeData();
		this.SetEnableDetectCollider(true);
	}

	// Token: 0x0400269E RID: 9886
	[Header("Detect settings")]
	public float durationBeforeExplosion;

	// Token: 0x0400269F RID: 9887
	private bool _isEnter;
}
