using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

// Token: 0x0200057D RID: 1405
public sealed class BotMovement : MonoBehaviour
{
	// Token: 0x060030AC RID: 12460 RVA: 0x000FD74C File Offset: 0x000FB94C
	private IEnumerator resetDeathAudio(float tm)
	{
		BotMovement._deathAudioPlaying = true;
		yield return new WaitForSeconds(tm);
		BotMovement._deathAudioPlaying = false;
		yield break;
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000FD770 File Offset: 0x000FB970
	public bool RequestPlayDeath(float tm)
	{
		if (BotMovement._deathAudioPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetDeathAudio(tm));
		return true;
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x000FD790 File Offset: 0x000FB990
	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
			return;
		}
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				this._modelChild = transform.gameObject;
			}
		}
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000FD81C File Offset: 0x000FBA1C
	private void Start()
	{
		this.myTransform = base.transform;
		this._nma = base.GetComponent<NavMeshAgent>();
		this._modelChildCollider = this._modelChild.GetComponent<BoxCollider>();
		this.shootAnim = this.offAnim;
		this.healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		this.player = GameObject.FindGameObjectWithTag("Player");
		this._gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		this._gameController.NumOfLiveZombies++;
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		this.timeToRemoveLive = this._soundClips.timeToHit * Mathf.Pow(0.95f, (float)GlobalGameController.AllLevelsCompleted);
		this.CurLifeTime = this.timeToRemoveLive;
		this.target = null;
		this._modelChild.GetComponent<Animation>().Stop();
		this.Walk();
		this._soundClips.attackingSpeed += UnityEngine.Random.Range(-this._soundClips.attackingSpeedRandomRange[0], this._soundClips.attackingSpeedRandomRange[1]);
		if (!Defs.IsSurvival)
		{
			this._soundClips.attackingSpeed *= Defs.DiffModif;
			this._soundClips.health *= Defs.DiffModif;
			this._soundClips.notAttackingSpeed *= Defs.DiffModif;
		}
		if (!Defs.IsSurvival && !base.gameObject.name.Contains("Boss"))
		{
			ZombieCreator.LastEnemy += this.IncreaseRange;
			if (this._gameController.IsLasTMonsRemains)
			{
				this.IncreaseRange();
			}
		}
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000FD9D4 File Offset: 0x000FBBD4
	private void IncreaseRange()
	{
		this._modelChild.GetComponent<Sounds>().attackingSpeed = Mathf.Max(this._modelChild.GetComponent<Sounds>().attackingSpeed, 3f);
		this._modelChild.GetComponent<Sounds>().detectRadius = 150f;
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000FDA20 File Offset: 0x000FBC20
	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000FDA44 File Offset: 0x000FBC44
	public void Slowdown(float coef)
	{
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x000FDA48 File Offset: 0x000FBC48
	private void Update()
	{
		if (!this.deaded)
		{
			if (this.target != null)
			{
				float num = Vector3.SqrMagnitude(this.target.position - this.myTransform.position);
				Vector3 vector = new Vector3(this.target.position.x, this.myTransform.position.y, this.target.position.z);
				if (num >= this._soundClips.attackDistance * this._soundClips.attackDistance)
				{
					this._nma.SetDestination(vector);
					this._nma.speed = this._soundClips.attackingSpeed * Mathf.Pow(1.05f, (float)GlobalGameController.AllLevelsCompleted);
					this.CurLifeTime = this.timeToRemoveLive;
					this.PlayZombieRun();
				}
				else
				{
					if (this._nma.path != null)
					{
						this._nma.ResetPath();
					}
					this.CurLifeTime -= Time.deltaTime;
					this.myTransform.LookAt(vector);
					if (this.CurLifeTime <= 0f)
					{
						if (this.target.CompareTag("Player"))
						{
						}
						if (this.target.CompareTag("Turret"))
						{
							this.target.GetComponent<TurretController>().MinusLive((float)this._soundClips.damagePerHit, 0);
						}
						this.CurLifeTime = this.timeToRemoveLive;
						if (Defs.isSoundFX)
						{
							base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.bite);
						}
					}
					if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
					{
						this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
					}
					else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
					{
						this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
					}
				}
			}
		}
		else if (this.falling)
		{
			float num2 = 7f;
			this.myTransform.position = new Vector3(this.myTransform.position.x, this.myTransform.position.y - num2 * Time.deltaTime, this.myTransform.position.z);
		}
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x000FDCD4 File Offset: 0x000FBED4
	public void PlayZombieRun()
	{
		if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000FDD18 File Offset: 0x000FBF18
	public void SetTarget(Transform _target, bool agression)
	{
		this.Agression = agression;
		if (_target && this.target != _target)
		{
			this._nma.ResetPath();
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.voice);
			}
			this.PlayZombieRun();
		}
		else if (!_target && this.target != _target)
		{
			this._nma.ResetPath();
			this.Walk();
		}
		this.target = _target;
		SpawnMonster component = base.GetComponent<SpawnMonster>();
		component.ShouldMove = (_target == null);
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x000FDDC8 File Offset: 0x000FBFC8
	private void Run()
	{
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x000FDDCC File Offset: 0x000FBFCC
	private void Stop()
	{
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x000FDDD0 File Offset: 0x000FBFD0
	private void Attack()
	{
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x000FDDD4 File Offset: 0x000FBFD4
	[Obfuscation(Exclude = true)]
	private void Death()
	{
		ZombieCreator.LastEnemy -= this.IncreaseRange;
		this._nma.enabled = false;
		if (this.RequestPlayDeath(this._soundClips.death.length) && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.death);
		}
		float num = this._soundClips.death.length;
		this._modelChild.GetComponent<Animation>().Stop();
		if (this._modelChild.GetComponent<Animation>()[this.deathAnim])
		{
			this._modelChild.GetComponent<Animation>().Play(this.deathAnim);
			num = Mathf.Max(num, this._modelChild.GetComponent<Animation>()[this.deathAnim].length);
			this.CodeAfterDelay(this._modelChild.GetComponent<Animation>()[this.deathAnim].length * 1.25f, new BotMovement.DelayedCallback(this.StartFall));
		}
		else
		{
			this.StartFall();
		}
		this.CodeAfterDelay(num, new BotMovement.DelayedCallback(this.DestroySelf));
		this._modelChild.GetComponent<BoxCollider>().enabled = false;
		this.deaded = true;
		SpawnMonster component = base.GetComponent<SpawnMonster>();
		component.ShouldMove = false;
		this._gameController.NumOfDeadZombies++;
		GlobalGameController.Score += this._soundClips.scorePerKill;
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x000FDF54 File Offset: 0x000FC154
	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x000FDF64 File Offset: 0x000FC164
	private void StartFall()
	{
		this.falling = true;
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x000FDF70 File Offset: 0x000FC170
	[Obfuscation(Exclude = true)]
	private void Walk()
	{
		this._modelChild.GetComponent<Animation>().Stop();
		if (this._modelChild.GetComponent<Animation>()[this.normWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.normWalkAnim);
		}
		else
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x000FDFE0 File Offset: 0x000FC1E0
	private void FixedUpdate()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().velocity = Vector3.zero;
			base.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x000FE020 File Offset: 0x000FC220
	public void CodeAfterDelay(float delay, BotMovement.DelayedCallback callback)
	{
		base.StartCoroutine(this.___DelayedCallback(delay, callback));
	}

	// Token: 0x060030BF RID: 12479 RVA: 0x000FE034 File Offset: 0x000FC234
	private IEnumerator ___DelayedCallback(float time, BotMovement.DelayedCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
		yield break;
	}

	// Token: 0x060030C0 RID: 12480 RVA: 0x000FE064 File Offset: 0x000FC264
	private void OnDestroy()
	{
		ZombieCreator.LastEnemy -= this.IncreaseRange;
	}

	// Token: 0x040023C1 RID: 9153
	public static bool _deathAudioPlaying;

	// Token: 0x040023C2 RID: 9154
	private Transform target;

	// Token: 0x040023C3 RID: 9155
	private float timeToRemoveLive;

	// Token: 0x040023C4 RID: 9156
	public ZombieCreator _gameController;

	// Token: 0x040023C5 RID: 9157
	private bool Agression;

	// Token: 0x040023C6 RID: 9158
	private bool deaded;

	// Token: 0x040023C7 RID: 9159
	private Player_move_c healthDown;

	// Token: 0x040023C8 RID: 9160
	private GameObject player;

	// Token: 0x040023C9 RID: 9161
	private float CurLifeTime;

	// Token: 0x040023CA RID: 9162
	private string idleAnim = "Idle";

	// Token: 0x040023CB RID: 9163
	private string normWalkAnim = "Norm_Walk";

	// Token: 0x040023CC RID: 9164
	private string zombieWalkAnim = "Zombie_Walk";

	// Token: 0x040023CD RID: 9165
	private string offAnim = "Zombie_Off";

	// Token: 0x040023CE RID: 9166
	private string deathAnim = "Zombie_Dead";

	// Token: 0x040023CF RID: 9167
	private string onAnim = "Zombie_On";

	// Token: 0x040023D0 RID: 9168
	private string attackAnim = "Zombie_Attack";

	// Token: 0x040023D1 RID: 9169
	private string shootAnim;

	// Token: 0x040023D2 RID: 9170
	private GameObject _modelChild;

	// Token: 0x040023D3 RID: 9171
	private Sounds _soundClips;

	// Token: 0x040023D4 RID: 9172
	private bool falling;

	// Token: 0x040023D5 RID: 9173
	private NavMeshAgent _nma;

	// Token: 0x040023D6 RID: 9174
	private BoxCollider _modelChildCollider;

	// Token: 0x040023D7 RID: 9175
	private Transform myTransform;

	// Token: 0x0200091A RID: 2330
	// (Invoke) Token: 0x06005108 RID: 20744
	public delegate void DelayedCallback();
}
