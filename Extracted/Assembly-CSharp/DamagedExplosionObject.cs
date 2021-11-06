using System;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class DamagedExplosionObject : BaseExplosionObject, IDamageable
{
	// Token: 0x0600348F RID: 13455 RVA: 0x0010FD2C File Offset: 0x0010DF2C
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0010FD40 File Offset: 0x0010DF40
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
	{
		this.GetDamage(damage);
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x0010FD4C File Offset: 0x0010DF4C
	public bool IsEnemyTo(Player_move_c player)
	{
		return true;
	}

	// Token: 0x06003492 RID: 13458 RVA: 0x0010FD50 File Offset: 0x0010DF50
	public bool IsDead()
	{
		return this.healthPoints <= 0f;
	}

	// Token: 0x170008BD RID: 2237
	// (get) Token: 0x06003493 RID: 13459 RVA: 0x0010FD64 File Offset: 0x0010DF64
	public bool isLivingTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003494 RID: 13460 RVA: 0x0010FD68 File Offset: 0x0010DF68
	public void GetDamage(float damage)
	{
		if (this.healthPoints <= 0f)
		{
			return;
		}
		float num = this.healthPoints / 100f * this.healthPoints;
		if (num <= this.percentHealthForFireEffect && !this.fireEffect.activeSelf)
		{
			this.SetVisibleFireEffect(true);
			base.Invoke("RunExplosion", this.timeToDestroyByFire);
		}
		this.healthPoints -= damage;
		if (this.healthPoints <= 0f)
		{
			this.healthPoints = 0f;
			base.RunExplosion();
		}
	}

	// Token: 0x06003495 RID: 13461 RVA: 0x0010FE00 File Offset: 0x0010E000
	protected override void InitializeData()
	{
		base.InitializeData();
		this._maxHealth = this.healthPoints;
		this.SetVisibleFireEffect(false);
	}

	// Token: 0x06003496 RID: 13462 RVA: 0x0010FE1C File Offset: 0x0010E01C
	private void SetVisibleFireEffect(bool visible)
	{
		if (this.isMultiplayerMode)
		{
			this.SetVisibleFireEffectRpc(visible);
			this.photonView.RPC("SetVisibleFireEffectRpc", PhotonTargets.Others, new object[]
			{
				visible
			});
		}
		else
		{
			this.fireEffect.SetActive(visible);
		}
	}

	// Token: 0x06003497 RID: 13463 RVA: 0x0010FE6C File Offset: 0x0010E06C
	public static void TryApplyDamageToObject(GameObject explosionObject, float damage)
	{
		DamagedExplosionObject component = explosionObject.GetComponent<DamagedExplosionObject>();
		if (component != null)
		{
			component.GetDamage(damage);
		}
	}

	// Token: 0x06003498 RID: 13464 RVA: 0x0010FE94 File Offset: 0x0010E094
	[PunRPC]
	[RPC]
	private void SetVisibleFireEffectRpc(bool visible)
	{
		this.fireEffect.SetActive(visible);
	}

	// Token: 0x04002698 RID: 9880
	public const string NameTag = "DamagedExplosion";

	// Token: 0x04002699 RID: 9881
	[Header("Damaged Health settings")]
	public float healthPoints = 100f;

	// Token: 0x0400269A RID: 9882
	[Range(1f, 100f)]
	public float percentHealthForFireEffect = 95f;

	// Token: 0x0400269B RID: 9883
	[Header("Damaged Effect settings")]
	public GameObject fireEffect;

	// Token: 0x0400269C RID: 9884
	public float timeToDestroyByFire = 5f;

	// Token: 0x0400269D RID: 9885
	private float _maxHealth;
}
