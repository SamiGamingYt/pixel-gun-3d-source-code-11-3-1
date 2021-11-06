using System;
using System.Collections;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000593 RID: 1427
	public class FiringShotBot : BaseShootingBot
	{
		// Token: 0x060031A3 RID: 12707 RVA: 0x00102530 File Offset: 0x00100730
		protected override void InitializeShotsPool(int sizePool)
		{
			base.InitializeShotsPool(sizePool);
			bool flag = this.firePoints.Length == 1;
			Transform parent = (!flag) ? base.transform : this.firePoints[0];
			for (int i = 0; i < this.bulletsEffectPool.Length; i++)
			{
				this.bulletsEffectPool[i].transform.parent = parent;
				this.bulletsEffectPool[i].transform.localPosition = Vector3.zero;
				this.bulletsEffectPool[i].transform.rotation = Quaternion.identity;
				this.bulletsEffectPool[i].GetComponent<ParticleSystem>().Stop();
			}
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x001025D8 File Offset: 0x001007D8
		private ParticleSystem GetFireShotEffectFromPool()
		{
			GameObject shotEffectFromPool = base.GetShotEffectFromPool();
			return shotEffectFromPool.GetComponent<ParticleSystem>();
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x001025F4 File Offset: 0x001007F4
		private IEnumerator ShowFireEffect(GameObject effect)
		{
			if (this._isEffectFireShow)
			{
				yield break;
			}
			this._isEffectFireShow = true;
			effect.SetActive(true);
			yield return new WaitForSeconds(this.timeShowFireEffect);
			effect.SetActive(false);
			this._isEffectFireShow = false;
			yield break;
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x00102620 File Offset: 0x00100820
		private IEnumerator ShowFireEffect(ParticleSystem effect)
		{
			if (this._isEffectFireShow)
			{
				yield break;
			}
			this._isEffectFireShow = true;
			effect.Play();
			yield return new WaitForSeconds(this.timeShowFireEffect);
			effect.Stop();
			this._isEffectFireShow = false;
			yield break;
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x0010264C File Offset: 0x0010084C
		protected override void Fire(Transform pointFire, GameObject target)
		{
			ParticleSystem fireShotEffectFromPool = this.GetFireShotEffectFromPool();
			if (this.firePoints.Length == 1)
			{
				base.StartCoroutine(this.ShowFireEffect(fireShotEffectFromPool));
			}
			else
			{
				base.StartCoroutine(this.ShowFireEffect(pointFire, fireShotEffectFromPool));
			}
			if (this.chanceMakeDamage >= UnityEngine.Random.value)
			{
				base.MakeDamage(target.transform);
			}
		}

		// Token: 0x060031A8 RID: 12712 RVA: 0x001026AC File Offset: 0x001008AC
		private IEnumerator ShowFireEffect(Transform pointFire, ParticleSystem effect)
		{
			if (this._isEffectFireShow)
			{
				yield break;
			}
			this._isEffectFireShow = true;
			effect.transform.position = pointFire.position;
			effect.transform.rotation = pointFire.rotation;
			effect.Play();
			yield return new WaitForSeconds(this.timeShowFireEffect);
			effect.Stop();
			this._isEffectFireShow = false;
			yield break;
		}

		// Token: 0x04002490 RID: 9360
		[Range(0.1f, 1f)]
		[Header("Firing settings")]
		public float chanceMakeDamage = 1f;

		// Token: 0x04002491 RID: 9361
		public float timeShowFireEffect = 2f;

		// Token: 0x04002492 RID: 9362
		private bool _isEffectFireShow;
	}
}
