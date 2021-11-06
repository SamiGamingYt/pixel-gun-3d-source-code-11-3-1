using System;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000598 RID: 1432
	public class ShockerBotEffect : MonoBehaviour
	{
		// Token: 0x060031B8 RID: 12728 RVA: 0x00102B2C File Offset: 0x00100D2C
		private void Awake()
		{
			this.myBot = base.GetComponent<BaseBot>();
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x00102B3C File Offset: 0x00100D3C
		private void Update()
		{
			if (WeaponManager.sharedManager.myPlayer == null)
			{
				return;
			}
			if (this.nextHitTime < Time.time)
			{
				this.nextHitTime = Time.time + this.timeToRadiusDamage;
				Initializer.TargetsList targetsList = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, true, true);
				foreach (Transform transform in targetsList)
				{
					if ((base.transform.position + Vector3.up - transform.transform.position).sqrMagnitude <= this.damageInRadius * this.damageInRadius)
					{
						IDamageable component = transform.GetComponent<IDamageable>();
						if (component != null && !component.Equals(this.myBot))
						{
							component.ApplyDamage(this.damageValue, this.myBot, Player_move_c.TypeKills.mob);
						}
					}
				}
			}
		}

		// Token: 0x040024A3 RID: 9379
		[Header("Shocker damage settings")]
		public float timeToRadiusDamage = 0.5f;

		// Token: 0x040024A4 RID: 9380
		public float damageInRadius = 4f;

		// Token: 0x040024A5 RID: 9381
		public float damageValue = 0.5f;

		// Token: 0x040024A6 RID: 9382
		private float nextHitTime;

		// Token: 0x040024A7 RID: 9383
		private BaseBot myBot;
	}
}
