using System;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000596 RID: 1430
	public class MeleeShootBot : ShootingBot
	{
		// Token: 0x060031B0 RID: 12720 RVA: 0x00102800 File Offset: 0x00100A00
		private string GameNameMeleeAnimation()
		{
			if (this.modelCollider == null)
			{
				return string.Empty;
			}
			string name = this.modelCollider.gameObject.name;
			return string.Format("{0}_shooting", name);
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x00102840 File Offset: 0x00100A40
		protected override void Initialize()
		{
			base.Initialize();
			this.animationNameMelee = this.GameNameMeleeAnimation();
			this.nextShootTime = Time.time + UnityEngine.Random.Range(this.minTimeToShoot, this.maxTimeToShoot);
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x0010287C File Offset: 0x00100A7C
		public override void DelayShootAfterEvent(float seconds)
		{
			if (this.nextShootTime < Time.time + seconds)
			{
				this.nextShootTime = Time.time + seconds;
			}
		}

		// Token: 0x060031B3 RID: 12723 RVA: 0x001028A0 File Offset: 0x00100AA0
		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			float num = base.GetSquareAttackDistance();
			this.isEnemyInMeleeZone = false;
			if (distanceToEnemy < num)
			{
				if (distanceToEnemy < this.meleeAttackDetect * this.meleeAttackDetect)
				{
					this.isEnemyInMeleeZone = true;
					return distanceToEnemy < this.meleeAttackDistance * this.meleeAttackDistance;
				}
				if (this.attackType == MeleeShootBot.AttackType.MeleeAndShootAtTime && this.nextShootTime < Time.time)
				{
					return true;
				}
				if (this.attackType != MeleeShootBot.AttackType.MeleeAndShootAtTime)
				{
					this.isEnemyEnterInAttackZone = true;
					return true;
				}
			}
			if (this.isEnemyEnterInAttackZone)
			{
				num += this.rangeShootingDistance * this.rangeShootingDistance;
				if (distanceToEnemy < num)
				{
					return true;
				}
			}
			this.isEnemyEnterInAttackZone = false;
			return false;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x00102954 File Offset: 0x00100B54
		protected override void MakeShot(GameObject target)
		{
			if (this.wasMeleeShot)
			{
				this.Melee(target);
			}
			else
			{
				if (this.attackType == MeleeShootBot.AttackType.MeleeAndShootAtTime)
				{
					this.nextShootTime = Time.time + UnityEngine.Random.Range(this.minTimeToShoot, this.maxTimeToShoot);
				}
				base.MakeShot(target);
			}
		}

		// Token: 0x060031B5 RID: 12725 RVA: 0x001029A8 File Offset: 0x00100BA8
		protected override void PlayAnimationZombieAttackOrStop()
		{
			if (this.isEnemyInMeleeZone)
			{
				this.wasMeleeShot = true;
				if (this.animations[this.animationNameMelee])
				{
					this.animations.CrossFade(this.animationNameMelee);
				}
				else if (this.animations[this.animationsName.Stop])
				{
					this.animations.CrossFade(this.animationsName.Stop);
				}
			}
			else
			{
				this.wasMeleeShot = false;
				if (this.animations[this.animationsName.Attack])
				{
					this.animations.CrossFade(this.animationsName.Attack);
				}
				else if (this.animations[this.animationsName.Stop])
				{
					this.animations.CrossFade(this.animationsName.Stop);
				}
			}
		}

		// Token: 0x060031B6 RID: 12726 RVA: 0x00102AAC File Offset: 0x00100CAC
		private void Melee(GameObject target)
		{
			float num = Vector3.SqrMagnitude(target.transform.position - base.transform.position);
			if (num < this.meleeAttackDistance * this.meleeAttackDistance)
			{
				base.MakeDamage(target.transform, this.meleeDamagePerHit);
			}
		}

		// Token: 0x04002495 RID: 9365
		[Header("Melee damage settings")]
		public float meleeAttackDetect = 6f;

		// Token: 0x04002496 RID: 9366
		public float meleeAttackDistance = 3f;

		// Token: 0x04002497 RID: 9367
		public float meleeDamagePerHit = 5f;

		// Token: 0x04002498 RID: 9368
		[Header("Attack settings")]
		public MeleeShootBot.AttackType attackType;

		// Token: 0x04002499 RID: 9369
		public float minTimeToShoot = 30f;

		// Token: 0x0400249A RID: 9370
		public float maxTimeToShoot = 40f;

		// Token: 0x0400249B RID: 9371
		private bool isEnemyInMeleeZone;

		// Token: 0x0400249C RID: 9372
		private bool isEnemyEnterInAttackZone;

		// Token: 0x0400249D RID: 9373
		private string animationNameMelee;

		// Token: 0x0400249E RID: 9374
		private bool wasMeleeShot;

		// Token: 0x0400249F RID: 9375
		private float nextShootTime;

		// Token: 0x02000597 RID: 1431
		public enum AttackType
		{
			// Token: 0x040024A1 RID: 9377
			MeleeAndShoot,
			// Token: 0x040024A2 RID: 9378
			MeleeAndShootAtTime
		}
	}
}
