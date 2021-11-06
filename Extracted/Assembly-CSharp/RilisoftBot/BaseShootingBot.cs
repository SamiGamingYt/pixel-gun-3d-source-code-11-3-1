using System;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000584 RID: 1412
	public class BaseShootingBot : BaseBot
	{
		// Token: 0x06003135 RID: 12597 RVA: 0x0010036C File Offset: 0x000FE56C
		protected override void Initialize()
		{
			base.Initialize();
			this.animationsName.Attack = this.GameNameShootingAnimation();
			BotAnimationEventHandler componentInChildren = base.GetComponentInChildren<BotAnimationEventHandler>();
			if (componentInChildren != null)
			{
				componentInChildren.OnDamageEvent += this.OnShoot;
			}
			this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			this.InitializeShotsPool(4);
			this._isEnemyEnterInAttackZone = false;
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x001003E4 File Offset: 0x000FE5E4
		private string GameNameShootingAnimation()
		{
			if (this.modelCollider == null)
			{
				return string.Empty;
			}
			string name = this.modelCollider.gameObject.name;
			return string.Format("{0}_shooting", name);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x00100424 File Offset: 0x000FE624
		protected virtual void InitializeShotsPool(int sizePool)
		{
			int num = sizePool * this.firePoints.Length;
			this.bulletsEffectPool = new GameObject[num];
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab);
				this.bulletsEffectPool[i] = gameObject;
			}
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x00100470 File Offset: 0x000FE670
		protected virtual Transform GetFirePointForSequentialShot()
		{
			int nextFirePointIndex = this._nextFirePointIndex;
			this._nextFirePointIndex++;
			if (this._nextFirePointIndex >= this.firePoints.Length)
			{
				this._nextFirePointIndex = 0;
			}
			return this.firePoints[nextFirePointIndex];
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x001004B4 File Offset: 0x000FE6B4
		protected virtual void MakeShot(GameObject target)
		{
			if (this.firePoints.Length == 1)
			{
				this.Fire(this.firePoints[0], target);
				return;
			}
			if (this.isSequentialShooting)
			{
				Transform firePointForSequentialShot = this.GetFirePointForSequentialShot();
				this.Fire(firePointForSequentialShot, target);
				return;
			}
			for (int i = 0; i < this.firePoints.Length; i++)
			{
				this.Fire(this.firePoints[i], target);
			}
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x00100524 File Offset: 0x000FE724
		protected virtual void Fire(Transform pointFire, GameObject target)
		{
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x00100528 File Offset: 0x000FE728
		protected GameObject GetShotEffectFromPool()
		{
			int nextShootEffectIndex = this._nextShootEffectIndex;
			this._nextShootEffectIndex++;
			if (this._nextShootEffectIndex >= this.bulletsEffectPool.Length)
			{
				this._nextShootEffectIndex = 0;
			}
			return this.bulletsEffectPool[nextShootEffectIndex];
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x0010056C File Offset: 0x000FE76C
		private void OnShoot()
		{
			if (this.botAiController == null || this.botAiController.currentTarget == null)
			{
				return;
			}
			this.MakeShot(this.botAiController.currentTarget.gameObject);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x001005B8 File Offset: 0x000FE7B8
		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			float num = base.GetSquareAttackDistance();
			if (distanceToEnemy < num)
			{
				this._isEnemyEnterInAttackZone = true;
				return true;
			}
			if (this._isEnemyEnterInAttackZone)
			{
				num += this.rangeShootingDistance * this.rangeShootingDistance;
				if (distanceToEnemy < num)
				{
					return true;
				}
			}
			this._isEnemyEnterInAttackZone = false;
			return false;
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x00100608 File Offset: 0x000FE808
		public override float GetMaxAttackDistance()
		{
			float num = this.rangeShootingDistance * this.rangeShootingDistance;
			return base.GetSquareAttackDistance() + num;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0010062C File Offset: 0x000FE82C
		public override Vector3 GetHeadPoint()
		{
			if (this.headPoint != null)
			{
				return this.headPoint.position;
			}
			return base.GetHeadPoint();
		}

		// Token: 0x0400242B RID: 9259
		protected const int MaxCountShootEffectInPool = 4;

		// Token: 0x0400242C RID: 9260
		[Header("Shooting damage settings")]
		public GameObject bulletPrefab;

		// Token: 0x0400242D RID: 9261
		public bool isFriendlyFire;

		// Token: 0x0400242E RID: 9262
		public Transform[] firePoints;

		// Token: 0x0400242F RID: 9263
		public bool isSequentialShooting;

		// Token: 0x04002430 RID: 9264
		[Header("Detect shot settings")]
		public float rangeShootingDistance = 10f;

		// Token: 0x04002431 RID: 9265
		public Transform headPoint;

		// Token: 0x04002432 RID: 9266
		protected GameObject[] bulletsEffectPool;

		// Token: 0x04002433 RID: 9267
		private bool _isEnemyEnterInAttackZone;

		// Token: 0x04002434 RID: 9268
		private int _nextShootEffectIndex;

		// Token: 0x04002435 RID: 9269
		private int _nextFirePointIndex;
	}
}
