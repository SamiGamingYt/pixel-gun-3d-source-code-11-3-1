using System;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000595 RID: 1429
	public class MeleeBot : BaseBot
	{
		// Token: 0x060031AC RID: 12716 RVA: 0x00102710 File Offset: 0x00100910
		protected override void Initialize()
		{
			base.Initialize();
			this._animationAttackLength = this.animations[this.animationsName.Attack].length;
		}

		// Token: 0x060031AD RID: 12717 RVA: 0x0010273C File Offset: 0x0010093C
		public float CheckTimeToTakeDamage()
		{
			if (!this.isAutomaticAnimationEnable)
			{
				return this.timeToTakeDamage * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
			}
			this.animations[this.animationsName.Attack].speed = this.speedAnimationAttack;
			float num = this._animationAttackLength / this.speedAnimationAttack;
			return num * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x001027B0 File Offset: 0x001009B0
		public override bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			return base.GetSquareAttackDistance() >= distanceToEnemy;
		}

		// Token: 0x04002493 RID: 9363
		[Header("Melee damage settings")]
		public float timeToTakeDamage = 2f;

		// Token: 0x04002494 RID: 9364
		private float _animationAttackLength;
	}
}
