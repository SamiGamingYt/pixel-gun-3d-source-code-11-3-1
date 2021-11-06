using System;
using System.Collections;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200059B RID: 1435
	internal sealed class TrainingEnemy : MonoBehaviour, IDamageable
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060031C7 RID: 12743 RVA: 0x00103130 File Offset: 0x00101330
		private Collider HeadCollider
		{
			get
			{
				if (this._headCol != null)
				{
					return this._headCol;
				}
				GameObject gameObject = base.gameObject.Descendants("HeadCollider").FirstOrDefault<GameObject>();
				if (gameObject != null)
				{
					this._headCol = gameObject.GetComponent<Collider>();
				}
				return this._headCol;
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x0010318C File Offset: 0x0010138C
		public void WakeUp(float delaySeconds = 0f)
		{
			base.StartCoroutine(this.AwakeCoroutine(delaySeconds));
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x0010319C File Offset: 0x0010139C
		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
		{
			this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x001031B0 File Offset: 0x001013B0
		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
		{
			this.ApplyDamage(damage, typeKill == Player_move_c.TypeKills.headshot);
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x001031C0 File Offset: 0x001013C0
		public bool IsEnemyTo(Player_move_c player)
		{
			return true;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x001031C4 File Offset: 0x001013C4
		public bool IsDead()
		{
			return this._currentState == TrainingEnemy.State.Dead;
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060031CD RID: 12749 RVA: 0x001031D0 File Offset: 0x001013D0
		public bool isLivingTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x001031D4 File Offset: 0x001013D4
		public void ApplyDamage(float damage, bool isHeadShot)
		{
			if (this._currentState != TrainingEnemy.State.Awakened)
			{
				return;
			}
			base.StartCoroutine(this.HighlightHitCoroutine());
			if (isHeadShot)
			{
				this.ShowHeadShotEffect();
			}
			else
			{
				this.ShowHitEffect();
			}
			this.hitPoints--;
			if (this._animation != null)
			{
				this._animation.Play("Dummy_Damage", PlayMode.StopSameLayer);
			}
			if (this.hitPoints <= 0)
			{
				this._currentState = TrainingEnemy.State.Dead;
				if (this.aimTarget != null)
				{
					UnityEngine.Object.Destroy(this.aimTarget);
					this.aimTarget = null;
				}
				base.StartCoroutine(this.DieCoroutine());
			}
		}

		// Token: 0x060031CF RID: 12751 RVA: 0x00103288 File Offset: 0x00101488
		private void Awake()
		{
			this.baseHitPoints = this.hitPoints;
			if (this.aimTarget != null)
			{
				this.aimTarget.SetActive(false);
			}
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x001032B4 File Offset: 0x001014B4
		private void Start()
		{
			this._audioSource = base.GetComponent<AudioSource>();
			this._animation = base.GetComponent<Animation>();
			if (this._animation != null)
			{
				this._animation.Play("Dummy_Idle", PlayMode.StopSameLayer);
			}
			this.meshRender = base.GetComponentInChildren<SkinnedMeshRenderer>();
			if (this.meshRender)
			{
				this.meshRender.sharedMaterial = new Material(this.meshRender.sharedMaterial);
				this.skinTexture = this.meshRender.sharedMaterial.mainTexture;
			}
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0010334C File Offset: 0x0010154C
		private IEnumerator HighlightHitCoroutine()
		{
			this.SetTexture(this.hitTexture);
			yield return new WaitForSeconds(0.125f);
			this.SetTexture(this.skinTexture);
			yield break;
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x00103368 File Offset: 0x00101568
		public void SetTexture(Texture needTx)
		{
			if (this.meshRender != null)
			{
				this.meshRender.sharedMaterial.mainTexture = needTx;
			}
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x00103398 File Offset: 0x00101598
		private IEnumerator AwakeCoroutine(float delaySeconds = 0f)
		{
			if (delaySeconds > 0f)
			{
				yield return new WaitForSeconds(delaySeconds);
			}
			if (this._animation != null)
			{
				if (this._audioSource != null && this.wakeUpAudioClip != null)
				{
					this._audioSource.PlayOneShot(this.wakeUpAudioClip);
				}
				this._animation.Play("Dummy_Up", PlayMode.StopSameLayer);
				while (this._animation.isPlaying)
				{
					yield return null;
				}
				if (this.aimTarget != null)
				{
					this.aimTarget.SetActive(true);
				}
			}
			this._currentState = TrainingEnemy.State.Awakened;
			yield break;
		}

		// Token: 0x060031D4 RID: 12756 RVA: 0x001033C4 File Offset: 0x001015C4
		private IEnumerator DieCoroutine()
		{
			if (this._animation != null)
			{
				while (this._animation.IsPlaying("Dummy_Damage"))
				{
					yield return null;
				}
				if (this._audioSource != null && this.dieAudioClip != null)
				{
					this._audioSource.PlayOneShot(this.dieAudioClip);
				}
				this._animation.Play("Dead", PlayMode.StopSameLayer);
				while (this._animation.isPlaying)
				{
					yield return null;
				}
			}
			if (ZombieCreator.sharedCreator != null)
			{
				ZombieCreator.sharedCreator.NumOfDeadZombies++;
			}
			yield break;
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x001033E0 File Offset: 0x001015E0
		private void ShowHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = ParticleStacks.instance.hitStack.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * this.heightFlyOutHitEffect);
			}
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x00103454 File Offset: 0x00101654
		private void ShowHeadShotEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null && this.HeadCollider != null)
			{
				Vector3 position = (!(this.HeadCollider is BoxCollider)) ? ((SphereCollider)this.HeadCollider).center : ((BoxCollider)this.HeadCollider).center;
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.HeadCollider.transform.TransformPoint(position));
			}
		}

		// Token: 0x040024B5 RID: 9397
		public AudioClip wakeUpAudioClip;

		// Token: 0x040024B6 RID: 9398
		public AudioClip dieAudioClip;

		// Token: 0x040024B7 RID: 9399
		public GameObject aimTarget;

		// Token: 0x040024B8 RID: 9400
		public int hitPoints = 3;

		// Token: 0x040024B9 RID: 9401
		private SkinnedMeshRenderer meshRender;

		// Token: 0x040024BA RID: 9402
		public Texture hitTexture;

		// Token: 0x040024BB RID: 9403
		private Texture skinTexture;

		// Token: 0x040024BC RID: 9404
		[ReadOnly]
		public int baseHitPoints;

		// Token: 0x040024BD RID: 9405
		public float heightFlyOutHitEffect = 1.75f;

		// Token: 0x040024BE RID: 9406
		private Collider _headCol;

		// Token: 0x040024BF RID: 9407
		private AudioSource _audioSource;

		// Token: 0x040024C0 RID: 9408
		private Animation _animation;

		// Token: 0x040024C1 RID: 9409
		private TrainingEnemy.State _currentState;

		// Token: 0x0200059C RID: 1436
		private enum State
		{
			// Token: 0x040024C3 RID: 9411
			None,
			// Token: 0x040024C4 RID: 9412
			Awakened,
			// Token: 0x040024C5 RID: 9413
			Dead
		}
	}
}
