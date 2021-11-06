using System;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class DamageInRadiusEffect : MonoBehaviour
{
	// Token: 0x06000417 RID: 1047 RVA: 0x0002370C File Offset: 0x0002190C
	public void ActivateEffect(float damageSurvival, float damageMultiplayer, float radiusDetectTarget, float time, string weaponName, WeaponSounds.TypeDead typeDead, Player_move_c.TypeKills typeKills)
	{
		this.active = true;
		this.damageSurvival = damageSurvival;
		this.damageMultiplayer = damageMultiplayer;
		this.weaponName = weaponName;
		this.typeDead = typeDead;
		this.typeKilsIconChat = typeKills;
		this.raduisDetectTarget = radiusDetectTarget;
		this.hitTime = time;
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x00023758 File Offset: 0x00021958
	public void ActivatePoisonEffect(float damageSurvival, float damageMultiplayer, float radiusDetectTarget, float time, WeaponSounds weapon, Player_move_c.PoisonType type)
	{
		this.active = true;
		this.isPoison = true;
		this.damageSurvival = damageSurvival;
		this.damageMultiplayer = damageMultiplayer;
		this.weaponSounds = weapon;
		this.raduisDetectTarget = radiusDetectTarget;
		this.hitTime = time;
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00023790 File Offset: 0x00021990
	public void ActivateSlowdonEffect(WeaponSounds weapon)
	{
		this.active = true;
		this.isSlowdown = true;
		this.weaponSounds = weapon;
		this.slowdownCoef = weapon.slowdownCoeff;
		this.hitTime = weapon.slowdownTime;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x000237C0 File Offset: 0x000219C0
	private void OnDisable()
	{
		this.active = false;
		this.isPoison = false;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x000237D0 File Offset: 0x000219D0
	private void Update()
	{
		if (!this.active)
		{
			return;
		}
		if (this.nextHitTime < Time.time)
		{
			this.nextHitTime = Time.time + this.hitTime;
			this.HitTargets();
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00023814 File Offset: 0x00021A14
	private void HitTargets()
	{
		Initializer.TargetsList targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, Defs.isMulti && !this.isSlowdown, true);
		foreach (Transform transform in targetsList)
		{
			if ((transform.position - base.transform.position).sqrMagnitude < this.raduisDetectTarget * this.raduisDetectTarget)
			{
				if (this.isPoison)
				{
					WeaponManager.sharedManager.myPlayerMoveC.PoisonShotWithEffect(transform.gameObject, this.damageMultiplayer, this.weaponSounds);
				}
				else if (this.isSlowdown)
				{
					WeaponManager.sharedManager.myPlayerMoveC.SlowdownTarget(transform.gameObject, this.hitTime, this.slowdownCoef);
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.DamageTarget(transform.gameObject, this.damageMultiplayer, this.weaponName, this.typeDead, this.typeKilsIconChat);
				}
			}
		}
	}

	// Token: 0x040004A4 RID: 1188
	private WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.explosion;

	// Token: 0x040004A5 RID: 1189
	private Player_move_c.TypeKills typeKilsIconChat = Player_move_c.TypeKills.explosion;

	// Token: 0x040004A6 RID: 1190
	private WeaponSounds weaponSounds;

	// Token: 0x040004A7 RID: 1191
	private float raduisDetectTarget = 10f;

	// Token: 0x040004A8 RID: 1192
	private float hitTime = 1.2f;

	// Token: 0x040004A9 RID: 1193
	private float damageSurvival = 1f;

	// Token: 0x040004AA RID: 1194
	private float damageMultiplayer = 1f;

	// Token: 0x040004AB RID: 1195
	private float slowdownCoef = 1f;

	// Token: 0x040004AC RID: 1196
	private string weaponName;

	// Token: 0x040004AD RID: 1197
	private float nextHitTime;

	// Token: 0x040004AE RID: 1198
	private bool active;

	// Token: 0x040004AF RID: 1199
	private bool isPoison;

	// Token: 0x040004B0 RID: 1200
	private bool isSlowdown;
}
