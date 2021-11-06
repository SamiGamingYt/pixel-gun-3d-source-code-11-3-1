using System;
using UnityEngine;
using ZeichenKraftwerk;

// Token: 0x020004D0 RID: 1232
public class RocketSettings : MonoBehaviour
{
	// Token: 0x04002109 RID: 8457
	[Header("General settings")]
	public RocketSettings.TypeFlyRocket typeFly;

	// Token: 0x0400210A RID: 8458
	public WeaponSounds.TypeDead typeDead = WeaponSounds.TypeDead.explosion;

	// Token: 0x0400210B RID: 8459
	public Player_move_c.TypeKills typeKilsIconChat = Player_move_c.TypeKills.explosion;

	// Token: 0x0400210C RID: 8460
	public float lifeTime = 7f;

	// Token: 0x0400210D RID: 8461
	public float startForce = 190f;

	// Token: 0x0400210E RID: 8462
	[Header("Particles")]
	public GameObject flyParticle;

	// Token: 0x0400210F RID: 8463
	public TrailRenderer trail;

	// Token: 0x04002110 RID: 8464
	public Rotator droneRotator;

	// Token: 0x04002111 RID: 8465
	public GameObject droneParticle;

	// Token: 0x04002112 RID: 8466
	[Header("Size detect collider")]
	public Vector3 sizeBoxCollider = new Vector3(0.15f, 0.15f, 0.75f);

	// Token: 0x04002113 RID: 8467
	public Vector3 centerBoxCollider = new Vector3(0f, 0f, 0f);

	// Token: 0x04002114 RID: 8468
	[Header("For AutoTarget, Autoaim")]
	public float autoRocketForce = 15f;

	// Token: 0x04002115 RID: 8469
	[Header("For AutoTarget, StickyBomb, ToxicBomb")]
	public float raduisDetectTarget = 5f;

	// Token: 0x04002116 RID: 8470
	[Header("For AutoTarget, StickyBomb, ToxicBomb")]
	public float toxicHitTime = 1f;

	// Token: 0x04002117 RID: 8471
	public float toxicDamageMultiplier = 0.2f;

	// Token: 0x04002118 RID: 8472
	[Header("For StickyBomb, ToxicBomb")]
	public GameObject stickedParticle;

	// Token: 0x04002119 RID: 8473
	[Header("For Lightning")]
	public int countJumpLightning = 2;

	// Token: 0x0400211A RID: 8474
	public float raduisDetectTargetLightning = 5f;

	// Token: 0x0400211B RID: 8475
	[Header("For charge weapon")]
	public float chargeScaleMin = 0.7f;

	// Token: 0x0400211C RID: 8476
	public float chargeScaleMax = 1.2f;

	// Token: 0x020004D1 RID: 1233
	public enum TypeFlyRocket
	{
		// Token: 0x0400211E RID: 8478
		Rocket,
		// Token: 0x0400211F RID: 8479
		Grenade,
		// Token: 0x04002120 RID: 8480
		Bullet,
		// Token: 0x04002121 RID: 8481
		MegaBullet,
		// Token: 0x04002122 RID: 8482
		Autoaim,
		// Token: 0x04002123 RID: 8483
		Bomb,
		// Token: 0x04002124 RID: 8484
		AutoaimBullet,
		// Token: 0x04002125 RID: 8485
		Ball,
		// Token: 0x04002126 RID: 8486
		GravityRocket,
		// Token: 0x04002127 RID: 8487
		Lightning,
		// Token: 0x04002128 RID: 8488
		AutoTarget,
		// Token: 0x04002129 RID: 8489
		StickyBomb,
		// Token: 0x0400212A RID: 8490
		Ghost,
		// Token: 0x0400212B RID: 8491
		ChargeRocket,
		// Token: 0x0400212C RID: 8492
		ToxicBomb,
		// Token: 0x0400212D RID: 8493
		GrenadeBouncing,
		// Token: 0x0400212E RID: 8494
		SingularityGrenade,
		// Token: 0x0400212F RID: 8495
		NuclearGrenade,
		// Token: 0x04002130 RID: 8496
		StickyMine,
		// Token: 0x04002131 RID: 8497
		Molotov,
		// Token: 0x04002132 RID: 8498
		Drone,
		// Token: 0x04002133 RID: 8499
		FakeBonus,
		// Token: 0x04002134 RID: 8500
		BlackMark,
		// Token: 0x04002135 RID: 8501
		Firework,
		// Token: 0x04002136 RID: 8502
		HomingGrenade,
		// Token: 0x04002137 RID: 8503
		SlowdownGrenade
	}
}
