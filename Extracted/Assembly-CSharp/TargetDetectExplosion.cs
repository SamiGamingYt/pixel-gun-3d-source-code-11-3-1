using System;
using UnityEngine;

// Token: 0x020005FD RID: 1533
public class TargetDetectExplosion : MonoBehaviour
{
	// Token: 0x060034A7 RID: 13479 RVA: 0x001100AC File Offset: 0x0010E2AC
	private void Awake()
	{
		if (this.explosionScript == null)
		{
			this.explosionScript = base.transform.parent.GetComponent<DamagedExplosionObject>();
		}
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x001100E0 File Offset: 0x0010E2E0
	private bool IsTargetAvailable(Transform targetTransform)
	{
		return !targetTransform.Equals(base.transform) && (targetTransform.CompareTag("Player") || targetTransform.CompareTag("Enemy") || targetTransform.CompareTag("Turret"));
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x00110130 File Offset: 0x0010E330
	private void OnTriggerEnter(Collider collisionObj)
	{
		if (!this.IsTargetAvailable(collisionObj.transform.root))
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
			base.Invoke("OnTargetEnter", this.durationBeforeExplosion);
		}
		else
		{
			this.OnTargetEnter();
		}
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x00110194 File Offset: 0x0010E394
	private void OnTargetEnter()
	{
		this.explosionScript.GetDamage(this.explosionScript.healthPoints);
	}

	// Token: 0x040026A5 RID: 9893
	[Header("Detect settings")]
	public float durationBeforeExplosion;

	// Token: 0x040026A6 RID: 9894
	public DamagedExplosionObject explosionScript;

	// Token: 0x040026A7 RID: 9895
	private bool _isEnter;
}
