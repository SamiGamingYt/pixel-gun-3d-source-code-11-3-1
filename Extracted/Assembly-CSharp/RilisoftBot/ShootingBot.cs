using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x0200059A RID: 1434
	public class ShootingBot : BaseShootingBot
	{
		// Token: 0x060031BD RID: 12733 RVA: 0x00102CAC File Offset: 0x00100EAC
		protected override void Initialize()
		{
			base.Initialize();
			float length = this.animations[this.animationsName.Attack].length;
			this._normalBulletSpeed = (this.attackDistance + this.rangeShootingDistance) / length;
		}

		// Token: 0x060031BE RID: 12734 RVA: 0x00102CF0 File Offset: 0x00100EF0
		protected override void InitializeShotsPool(int sizePool)
		{
			base.InitializeShotsPool(sizePool);
			float lifeTime = (this.attackDistance + this.rangeShootingDistance) / this.GetBulletSpeed();
			for (int i = 0; i < this.bulletsEffectPool.Length; i++)
			{
				BulletForBot component = this.bulletsEffectPool[i].GetComponent<BulletForBot>();
				if (component != null)
				{
					component.lifeTime = lifeTime;
					component.OnBulletDamage += this.MakeDamageTarget;
					component.needDestroyByStop = false;
				}
			}
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x00102D6C File Offset: 0x00100F6C
		private BulletForBot GetShotFromPool()
		{
			GameObject shotEffectFromPool = base.GetShotEffectFromPool();
			return shotEffectFromPool.GetComponent<BulletForBot>();
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x00102D88 File Offset: 0x00100F88
		private float GetBulletSpeed()
		{
			if (this.isCalculateSpeedBullet)
			{
				this.speedBullet = this._normalBulletSpeed * this.speedAnimationAttack;
			}
			return this.speedBullet;
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x00102DBC File Offset: 0x00100FBC
		private void MakeDamageTarget(GameObject target, Vector3 positionDamage)
		{
			if (this.isProjectileExplosion)
			{
				UnityEngine.Object.Instantiate(this.effectExplosion, positionDamage, Quaternion.identity);
				Collider[] array = Physics.OverlapSphere(positionDamage, this.radiusExplosion, Tools.AllAvailabelBotRaycastMask);
				if (array.Length == 0)
				{
					return;
				}
				float num = this.radiusExplosion * this.radiusExplosion;
				this.damagedTargets.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					if (!(array[i].gameObject == null))
					{
						Transform root = array[i].transform.root;
						if (!(root.gameObject == null) && !(base.transform.gameObject == null) && !root.Equals(base.transform))
						{
							float sqrMagnitude = (root.position - positionDamage).sqrMagnitude;
							if (sqrMagnitude <= num)
							{
								if (this.isFriendlyFire || !root.CompareTag("Enemy"))
								{
									if (!this.damagedTargets.Contains(array[i].transform.root.transform))
									{
										float num2 = this.damagePerHitMin + (this.damagePerHit - this.damagePerHitMin) * ((num - sqrMagnitude) / num);
										base.MakeDamage(array[i].transform.root.transform, (float)((int)num2));
										this.damagedTargets.Add(array[i].transform.root.transform);
									}
								}
							}
						}
					}
				}
			}
			else if (target != null)
			{
				base.MakeDamage(target.transform);
			}
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x00102F70 File Offset: 0x00101170
		protected override void Fire(Transform pointFire, GameObject target)
		{
			Vector3 position = target.transform.position;
			position.y += 0.5f;
			this.FireBullet(pointFire.position, position, true);
			if (Defs.isCOOP)
			{
				base.FireByRPC(pointFire.position, position);
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x00102FC4 File Offset: 0x001011C4
		private void FireBullet(Vector3 pointFire, Vector3 positionToFire, bool doDamage)
		{
			BulletForBot shotFromPool = this.GetShotFromPool();
			if (this.isCalculateSpeedBullet)
			{
				this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			}
			if (this.isMoveByPhysics)
			{
				Quaternion rotation = Quaternion.AngleAxis(this.angle, base.transform.right);
				Vector3 forceVector = rotation * base.transform.forward;
				shotFromPool.ApplyForceFroBullet(pointFire, positionToFire, this.isFriendlyFire, this.force, forceVector, doDamage);
			}
			else
			{
				shotFromPool.StartBullet(pointFire, positionToFire, this.GetBulletSpeed(), this.isFriendlyFire, doDamage);
			}
			base.TryPlayAudioClip(this.shootingSound);
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x00103074 File Offset: 0x00101274
		[RPC]
		[PunRPC]
		private void FireBulletRPC(Vector3 pointFire, Vector3 positionToFire)
		{
			this.FireBullet(pointFire, positionToFire, false);
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x00103080 File Offset: 0x00101280
		protected override void OnBotDestroyEvent()
		{
			for (int i = 0; i < this.bulletsEffectPool.Length; i++)
			{
				if (!(this.bulletsEffectPool[i].gameObject == null))
				{
					BulletForBot component = this.bulletsEffectPool[i].GetComponent<BulletForBot>();
					if (component != null)
					{
						component.OnBulletDamage -= this.MakeDamageTarget;
						if (component.IsUse)
						{
							component.needDestroyByStop = true;
						}
						else
						{
							UnityEngine.Object.Destroy(this.bulletsEffectPool[i]);
						}
					}
				}
			}
		}

		// Token: 0x040024A8 RID: 9384
		private const float offsetPointDamagePlayer = 0.5f;

		// Token: 0x040024A9 RID: 9385
		[Header("Explosion damage settings")]
		public bool isProjectileExplosion;

		// Token: 0x040024AA RID: 9386
		public float damagePerHitMin;

		// Token: 0x040024AB RID: 9387
		public GameObject effectExplosion;

		// Token: 0x040024AC RID: 9388
		public float radiusExplosion;

		// Token: 0x040024AD RID: 9389
		public float speedBullet = 10f;

		// Token: 0x040024AE RID: 9390
		[Header("Automatic bullet speed settings")]
		public bool isCalculateSpeedBullet;

		// Token: 0x040024AF RID: 9391
		[Header("Shooting sound settings")]
		public AudioClip shootingSound;

		// Token: 0x040024B0 RID: 9392
		private float _normalBulletSpeed;

		// Token: 0x040024B1 RID: 9393
		private List<Transform> damagedTargets = new List<Transform>();

		// Token: 0x040024B2 RID: 9394
		[Header("Physics shot settings")]
		public bool isMoveByPhysics;

		// Token: 0x040024B3 RID: 9395
		public float force = 14f;

		// Token: 0x040024B4 RID: 9396
		public float angle = -10f;
	}
}
