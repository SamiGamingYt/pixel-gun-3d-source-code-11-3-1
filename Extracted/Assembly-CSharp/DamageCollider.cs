using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
public class DamageCollider : MonoBehaviour
{
	// Token: 0x0600040D RID: 1037 RVA: 0x00023544 File Offset: 0x00021744
	public void RegisterPlayer()
	{
		this._playerRegistered = true;
		this._remainsTimeToHit = this.frequency;
		this.CauseDamage();
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00023560 File Offset: 0x00021760
	public void UnregisterPlayer()
	{
		this._playerRegistered = false;
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x0002356C File Offset: 0x0002176C
	private void Start()
	{
		this.cachedTransform = base.transform;
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x0002357C File Offset: 0x0002177C
	private void CauseDamage()
	{
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.GetDamage(this.damage, Player_move_c.TypeKills.himself, WeaponSounds.TypeDead.angel, default(Vector3), string.Empty, 0);
		}
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x000235D4 File Offset: 0x000217D4
	private void Update()
	{
		if (this._playerRegistered)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC == null)
			{
				this._playerRegistered = false;
				return;
			}
			this._remainsTimeToHit -= Time.deltaTime;
			if (this._remainsTimeToHit <= 0f)
			{
				this._remainsTimeToHit = this.frequency;
				this.CauseDamage();
			}
		}
	}

	// Token: 0x0400049E RID: 1182
	public float damage;

	// Token: 0x0400049F RID: 1183
	public float frequency;

	// Token: 0x040004A0 RID: 1184
	private bool _playerRegistered;

	// Token: 0x040004A1 RID: 1185
	private float _remainsTimeToHit;

	// Token: 0x040004A2 RID: 1186
	private Transform cachedTransform;
}
