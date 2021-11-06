using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Rilisoft;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x0200057F RID: 1407
	public class BaseBot : MonoBehaviour, IDamageable
	{
		// Token: 0x060030C5 RID: 12485 RVA: 0x000FE3F0 File Offset: 0x000FC5F0
		public BaseBot()
		{
			this.notAttackingSpeed = 1f;
			this.attackingSpeed = 1f;
			this.health = 100f;
			this.attackDistance = 3f;
			this.detectRadius = 17f;
			this.damagePerHit = 1f;
			this.scorePerKill = 50;
			this.attackingSpeedRandomRange = new float[]
			{
				-0.5f,
				0.5f
			};
			this.heightFlyOutHitEffect = 1.75f;
			this.coefHealthFromTier = new float[]
			{
				1f,
				1.25f,
				1.65f,
				2f,
				2.25f,
				2.65f
			};
			this.speedAnimationWalk = 1f;
			this.speedAnimationRun = 1f;
			this.speedAnimationAttack = 1f;
			base..ctor();
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000FE4D8 File Offset: 0x000FC6D8
		// (set) Token: 0x060030C6 RID: 12486 RVA: 0x000FE4CC File Offset: 0x000FC6CC
		public bool IsDeath { get; private set; }

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000FE4EC File Offset: 0x000FC6EC
		// (set) Token: 0x060030C8 RID: 12488 RVA: 0x000FE4E0 File Offset: 0x000FC6E0
		public bool IsFalling { get; private set; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000FE500 File Offset: 0x000FC700
		// (set) Token: 0x060030CA RID: 12490 RVA: 0x000FE4F4 File Offset: 0x000FC6F4
		public bool needDestroyByMasterClient { get; private set; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000FE508 File Offset: 0x000FC708
		// (set) Token: 0x060030CD RID: 12493 RVA: 0x000FE510 File Offset: 0x000FC710
		public float baseHealth { get; private set; }

		// Token: 0x060030CE RID: 12494 RVA: 0x000FE51C File Offset: 0x000FC71C
		private void Awake()
		{
			this.advancedEffects = base.gameObject.AddComponent<AdvancedEffects>();
			this._effectsManager = base.GetComponent<IEnemyEffectsManager>();
			this.audioSource = base.GetComponent<AudioSource>();
			this.isMobChampion = false;
			this.Initialize();
		}

		// Token: 0x060030CF RID: 12495 RVA: 0x000FE560 File Offset: 0x000FC760
		protected virtual void Initialize()
		{
			this.animationsName = new BaseBot.BotAnimationName();
			this.AntiHackForCreateMobInInvalidGameMode();
			this._photonView = base.GetComponent<PhotonView>();
			this._isMultiplayerMode = (this._photonView != null && Defs.isCOOP);
			this.animations = base.GetComponentInChildren<Animation>();
			this.animations.Stop();
			this.botAiController = base.GetComponent<BotAiController>();
			this.modelCollider = base.GetComponentInChildren<BoxCollider>();
			this.headCollider = base.GetComponentInChildren<SphereCollider>();
			UnityEngine.Random.seed = ((int)DateTime.Now.Ticks & 65535);
			this.InitializeRandomAttackSpeed();
			this.ModifyParametrsForLocalMode();
			this.needDestroyByMasterClient = false;
			int num = (PhotonNetwork.room == null) ? ((!(ExpController.Instance != null)) ? 0 : ExpController.Instance.OurTier) : Convert.ToInt32(PhotonNetwork.room.customProperties["tier"]);
			this.health *= this.coefHealthFromTier[num];
			this.baseHealth = this.health;
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x000FE67C File Offset: 0x000FC87C
		private void Start()
		{
			if (this._isMultiplayerMode && this._photonView.isMine)
			{
				this._photonView.RPC("SetBotHealthRPC", PhotonTargets.All, new object[]
				{
					this.health
				});
			}
			if (!this._isMultiplayerMode)
			{
				ZombieCreator.sharedCreator.NumOfLiveZombies++;
			}
			this.mobModel = this.modelCollider.transform.GetChild(0);
			this._botMaterials = base.GetComponentsInChildren<BotChangeDamageMaterial>();
			this.InitNetworkStateData();
			Initializer.enemiesObj.Add(base.gameObject);
			if (this._effectsManager == null)
			{
				this._effectsManager = base.gameObject.AddComponent<PortalEnemyEffectsManager>();
			}
			this._effectsManager.ShowSpawnEffect();
			this.PlayVoiceSound();
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000FE74C File Offset: 0x000FC94C
		public virtual void DelayShootAfterEvent(float seconds)
		{
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x000FE750 File Offset: 0x000FC950
		private Texture GetBotSkin()
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>(true);
			if (componentsInChildren.Length == 0)
			{
				return null;
			}
			return componentsInChildren[componentsInChildren.Length - 1].material.mainTexture;
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000FE780 File Offset: 0x000FC980
		private void AntiHackForCreateMobInInvalidGameMode()
		{
			if (Defs.isMulti && !Defs.isCOOP)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000FE7A4 File Offset: 0x000FC9A4
		public void OrientToTarget(Vector3 targetPos)
		{
			base.transform.LookAt(targetPos);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000FE7B4 File Offset: 0x000FC9B4
		[Obfuscation(Exclude = true)]
		public void PlayAnimationIdle()
		{
			this.animations.Stop();
			if (this.animations[this.animationsName.Idle])
			{
				this.animations.CrossFade(this.animationsName.Idle);
			}
			this.StopSteps();
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000FE808 File Offset: 0x000FCA08
		public void PlayAnimationWalk()
		{
			this.animations.Stop();
			if (this.animations[this.animationsName.Walk])
			{
				this.animations.CrossFade(this.animationsName.Walk);
			}
			else
			{
				this.animations.CrossFade(this.animationsName.Run);
			}
			this.PlayWalkStepSound();
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000FE878 File Offset: 0x000FCA78
		private void PlayAnimationZombieWalk()
		{
			if (this.animations[this.animationsName.Run])
			{
				this.animations.CrossFade(this.animationsName.Run);
			}
			this.PlayRunStepSound();
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000FE8C4 File Offset: 0x000FCAC4
		protected virtual void PlayAnimationZombieAttackOrStop()
		{
			if (this.animations[this.animationsName.Attack])
			{
				this.animations.CrossFade(this.animationsName.Attack);
			}
			else if (this.animations[this.animationsName.Stop])
			{
				this.animations.CrossFade(this.animationsName.Stop);
			}
			this.StopSteps();
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000FE948 File Offset: 0x000FCB48
		private void InitializeRandomAttackSpeed()
		{
			if (this.isAutomaticAnimationEnable)
			{
				float min = (this.attackingSpeed - this.attackingSpeedRandomRange[0]) / this.attackingSpeed;
				float max = (this.attackingSpeed + this.attackingSpeedRandomRange[1]) / this.attackingSpeed;
				this.speedAnimationRun = UnityEngine.Random.Range(min, max);
			}
			else
			{
				this.attackingSpeed += UnityEngine.Random.Range(-this.attackingSpeedRandomRange[0], this.attackingSpeedRandomRange[1]);
			}
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x000FE9C4 File Offset: 0x000FCBC4
		private void SetRangeParametrs()
		{
			if (this.isMobChampion || this.IsBotGuard())
			{
				return;
			}
			ZombieCreator.LastEnemy += this.IncreaseRange;
			if (ZombieCreator.sharedCreator.IsLasTMonsRemains)
			{
				this.IncreaseRange();
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000FEA04 File Offset: 0x000FCC04
		private void ModifyParametrsForLocalMode()
		{
			float num;
			float num2;
			if (this.isAutomaticAnimationEnable)
			{
				num = this.speedAnimationWalk;
				num2 = this.speedAnimationRun;
			}
			else
			{
				num = this.notAttackingSpeed;
				num2 = this.attackingSpeed;
			}
			if (!this._isMultiplayerMode)
			{
				if (!Defs.IsSurvival)
				{
					num2 *= Defs.DiffModif;
					this.health *= Defs.DiffModif;
					num *= Defs.DiffModif;
				}
				else if (Defs.IsSurvival && TrainingController.TrainingCompleted)
				{
					int currentWave = ZombieCreator.sharedCreator.currentWave;
					if (currentWave == 0)
					{
						num *= 0.75f;
						num2 *= 0.75f;
						this.health *= 0.7f;
					}
					else if (currentWave == 1)
					{
						num *= 0.85f;
						num2 *= 0.85f;
						this.health *= 0.8f;
					}
					else if (currentWave == 2)
					{
						num *= 0.9f;
						num2 *= 0.9f;
						this.health *= 0.9f;
					}
					else if (currentWave >= 7)
					{
						num *= 1.25f;
						num2 *= 1.25f;
					}
					else if (currentWave >= 9)
					{
						this.health *= 1.25f;
					}
				}
			}
			if (this._isMultiplayerMode || Defs.IsSurvival)
			{
				num *= 0.9f + (float)ExpController.OurTierForAnyPlace() * 0.1f;
				this.health *= 0.9f + (float)ExpController.OurTierForAnyPlace() * 0.1f;
			}
			if (this.isAutomaticAnimationEnable)
			{
				this.speedAnimationWalk = num;
				this.speedAnimationRun = num2;
			}
			else
			{
				this.notAttackingSpeed = num;
				this.attackingSpeed = num2;
			}
			if (!Defs.IsSurvival && !this._isMultiplayerMode)
			{
				this.SetRangeParametrs();
			}
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x000FEBF4 File Offset: 0x000FCDF4
		private void IncreaseRange()
		{
			if (this.isAutomaticAnimationEnable)
			{
				this.speedAnimationRun = Mathf.Max(this.speedAnimationRun, 1.5f);
			}
			else
			{
				this.attackingSpeed = Mathf.Max(this.attackingSpeed, 3f);
			}
			this.detectRadius = 150f;
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x000FEC48 File Offset: 0x000FCE48
		public float GetSquareAttackDistance()
		{
			return this.attackDistance * this.attackDistance;
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x000FEC58 File Offset: 0x000FCE58
		public float GetSquareDetectRadius()
		{
			return this.detectRadius * this.detectRadius;
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x000FEC68 File Offset: 0x000FCE68
		public void SetPositionForFallState()
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 7f * Time.deltaTime, base.transform.position.z);
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000FECCC File Offset: 0x000FCECC
		public void TryPlayAudioClip(AudioClip audioClip)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || audioClip == null)
			{
				return;
			}
			this.audioSource.PlayOneShot(audioClip);
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000FED04 File Offset: 0x000FCF04
		public void PlayVoiceSound()
		{
			this.TryPlayAudioClip(this.voiceMobSoud);
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x000FED14 File Offset: 0x000FCF14
		public void PlayWalkStepSound()
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.stepSound == null)
			{
				return;
			}
			if (this.audioSource.clip != this.stepSound)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.stepSound;
				this.audioSource.Play();
			}
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000FED84 File Offset: 0x000FCF84
		public void PlayRunStepSound()
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.runStepSound == null)
			{
				return;
			}
			if (this.audioSource.clip != this.runStepSound)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.runStepSound;
				this.audioSource.Play();
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x000FEDF4 File Offset: 0x000FCFF4
		public void StopSteps()
		{
			if (this.stepSound == null)
			{
				return;
			}
			if (this.audioSource.clip == this.stepSound || this.audioSource.clip == this.runStepSound)
			{
				this.audioSource.Pause();
				this.audioSource.clip = null;
			}
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x000FEE60 File Offset: 0x000FD060
		public void TryPlayDeathSound(float delay)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || !this.IsCanPlayDeathSound(delay))
			{
				return;
			}
			this.audioSource.PlayOneShot(this.deathSound);
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x000FEEA8 File Offset: 0x000FD0A8
		public void TryPlayDamageSound(float delay)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			if (this.audioSource == null || this._isPlayingDamageSound)
			{
				return;
			}
			base.StartCoroutine(this.CheckCanPlayDamageAudio(delay));
			this.audioSource.PlayOneShot(this.damageSound);
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x000FEEFC File Offset: 0x000FD0FC
		private IEnumerator CheckCanPlayDamageAudio(float timeOut)
		{
			this._isPlayingDamageSound = true;
			yield return new WaitForSeconds(timeOut);
			this._isPlayingDamageSound = false;
			yield break;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x000FEF28 File Offset: 0x000FD128
		private IEnumerator ResetDeathAudio(float timeOut)
		{
			this._isDeathAudioPlaying = true;
			yield return new WaitForSeconds(timeOut);
			this._isDeathAudioPlaying = false;
			yield break;
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x000FEF54 File Offset: 0x000FD154
		private bool IsCanPlayDeathSound(float timeOut)
		{
			if (this._isDeathAudioPlaying)
			{
				return false;
			}
			base.StartCoroutine(this.ResetDeathAudio(timeOut));
			return true;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x000FEF74 File Offset: 0x000FD174
		[Obfuscation(Exclude = true)]
		public void PrepareDeath(bool isOwnerDamage = true)
		{
			if (!this._isMultiplayerMode)
			{
				ZombieCreator.LastEnemy -= this.IncreaseRange;
			}
			this.botAiController.isDetectPlayer = false;
			this.botAiController.IsCanMove = false;
			this.IsDeath = true;
			float num = this.deathSound.length;
			this.TryPlayDeathSound(num);
			this.animations.Stop();
			if (this.animations[this.animationsName.Death])
			{
				this.animations.Play(this.animationsName.Death);
				num = Mathf.Max(num, this.animations[this.animationsName.Death].length);
				base.StartCoroutine(this.DelayedSetFallState(this.animations[this.animationsName.Death].length * 1.25f));
			}
			else
			{
				this.IsFalling = true;
			}
			base.StartCoroutine(this.DelayedDestroySelf(num));
			this.modelCollider.enabled = false;
			if (this.headCollider != null)
			{
				this.headCollider.enabled = false;
			}
			if (isOwnerDamage)
			{
				GlobalGameController.Score += this.scorePerKill;
			}
			this.CheckForceKillGuards();
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x000FF0C4 File Offset: 0x000FD2C4
		private IEnumerator DelayedSetFallState(float delay)
		{
			yield return new WaitForSeconds(delay);
			this.IsFalling = true;
			yield break;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000FF0F0 File Offset: 0x000FD2F0
		private IEnumerator DelayedDestroySelf(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (!this._isMultiplayerMode && !this.IsBotGuard())
			{
				ZombieCreator.sharedCreator.NumOfDeadZombies++;
			}
			this.DestroyByNetworkType();
			yield break;
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000FF11C File Offset: 0x000FD31C
		private void CheckForceKillGuards()
		{
			if (this.guards.Length == 0)
			{
				return;
			}
			ZombieCreator sharedCreator = ZombieCreator.sharedCreator;
			if (sharedCreator == null)
			{
				return;
			}
			for (int i = 0; i < sharedCreator.bossGuads.Length; i++)
			{
				GameObject gameObject = sharedCreator.bossGuads[i];
				if (!(gameObject.gameObject == null))
				{
					BaseBot botScriptForObject = BaseBot.GetBotScriptForObject(gameObject.transform);
					if (!botScriptForObject.IsDeath)
					{
						botScriptForObject.GetDamage(-2.1474836E+09f, null, false, Player_move_c.TypeKills.none);
					}
				}
			}
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x000FF1AC File Offset: 0x000FD3AC
		protected virtual void OnBotDestroyEvent()
		{
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x000FF1B0 File Offset: 0x000FD3B0
		private void OnDestroy()
		{
			this.OnBotDestroyEvent();
			if (!this._isMultiplayerMode)
			{
				ZombieCreator.LastEnemy -= this.IncreaseRange;
			}
			Initializer.enemiesObj.Remove(base.gameObject);
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x000FF1E8 File Offset: 0x000FD3E8
		public static BaseBot GetBotScriptForObject(Transform obj)
		{
			return obj.GetComponent<BaseBot>();
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x000FF1F0 File Offset: 0x000FD3F0
		public virtual bool CheckEnemyInAttackZone(float distanceToEnemy)
		{
			return false;
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x000FF1F4 File Offset: 0x000FD3F4
		public virtual Vector3 GetHeadPoint()
		{
			Vector3 position = base.transform.position;
			position.y += ((!(this.headCollider != null)) ? (this.modelCollider.size.y * 0.75f) : this.headCollider.center.y);
			return position;
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x000FF260 File Offset: 0x000FD460
		public virtual float GetMaxAttackDistance()
		{
			return this.GetMaxAttackDistance();
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x000FF268 File Offset: 0x000FD468
		public static void LogDebugData(string message)
		{
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x000FF26C File Offset: 0x000FD46C
		private float CheckAnimationSpeedWalkMoveForBot(float modSpeed)
		{
			float num = this.speedAnimationWalk * modSpeed;
			this.animations[this.animationsName.Walk].speed = num;
			return num * this.attackingSpeed;
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x000FF2A8 File Offset: 0x000FD4A8
		private float CheckAnimationSpeedRunMoveForBot(float modSpeed)
		{
			float num = this.speedAnimationRun * modSpeed;
			this.animations[this.animationsName.Run].speed = num;
			return num * this.notAttackingSpeed;
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x000FF2E4 File Offset: 0x000FD4E4
		public float GetWalkSpeed()
		{
			if (this.isAutomaticAnimationEnable)
			{
				return this.CheckAnimationSpeedWalkMoveForBot(this._modMoveSpeedByDebuff);
			}
			return this.notAttackingSpeed * this._modMoveSpeedByDebuff;
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x000FF30C File Offset: 0x000FD50C
		public float GetAttackSpeedByCompleteLevel()
		{
			if (this.isAutomaticAnimationEnable)
			{
				return this.CheckAnimationSpeedRunMoveForBot(this._modMoveSpeedByDebuff);
			}
			return this.attackingSpeed * this._modMoveSpeedByDebuff;
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x000FF334 File Offset: 0x000FD534
		public static Vector3 GetPositionSpawnGuard(Vector3 bossPosition)
		{
			float num = UnityEngine.Random.Range(0.5f, 1f);
			return bossPosition + new Vector3(num, num, num);
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x000FF364 File Offset: 0x000FD564
		private bool IsBotGuard()
		{
			return base.gameObject.name.Contains("BossGuard");
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x000FF37C File Offset: 0x000FD57C
		private void ShowDamageTexture(bool isEnable, bool isPoison = false)
		{
			if (this._botMaterials == null || this._botMaterials.Length == 0)
			{
				return;
			}
			for (int i = 0; i < this._botMaterials.Length; i++)
			{
				if (isEnable)
				{
					this._botMaterials[i].ShowDamageEffect(isPoison);
				}
				else
				{
					this._botMaterials[i].ResetMainMaterial();
				}
			}
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x000FF3E4 File Offset: 0x000FD5E4
		private IEnumerator ShowDamageEffect(bool poison)
		{
			this._isFlashing = true;
			this.ShowDamageTexture(true, poison);
			yield return new WaitForSeconds(0.125f);
			this.ShowDamageTexture(false, false);
			this._isFlashing = false;
			yield break;
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x000FF410 File Offset: 0x000FD610
		private void TakeBonusForKill()
		{
			if (!this.isMobChampion)
			{
				return;
			}
			if (this._isWeaponCreated)
			{
				return;
			}
			if (!LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName))
			{
				return;
			}
			string weaponName = LevelBox.weaponsFromBosses[Application.loadedLevelName];
			Vector3 vector = base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f);
			if (Application.loadedLevelName == "Sky_islands")
			{
				vector -= new Vector3(0f, 1.5f, 0f);
			}
			GameObject weaponBonus = ZombieCreator.sharedCreator.weaponBonus;
			GameObject gameObject = (!(weaponBonus != null)) ? BonusCreator._CreateBonus(weaponName, vector) : BonusCreator._CreateBonusFromPrefab(weaponBonus, vector);
			gameObject.AddComponent<GotToNextLevel>();
			ZombieCreator.sharedCreator.weaponBonus = null;
			this._isWeaponCreated = true;
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x000FF4F8 File Offset: 0x000FD6F8
		public void MakeDamage(Transform target, float damageValue)
		{
			IDamageable component = target.gameObject.GetComponent<IDamageable>();
			if (component != null)
			{
				component.ApplyDamage(damageValue, this, Player_move_c.TypeKills.mob);
			}
			this.TryPlayAudioClip(this.takeDamageSound);
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x000FF530 File Offset: 0x000FD730
		public void MakeDamage(Transform target)
		{
			this.MakeDamage(target, this.damagePerHit);
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x000FF540 File Offset: 0x000FD740
		private void ShowHeadShotEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = HeadShotStackController.sharedController.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
			}
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x000FF5A8 File Offset: 0x000FD7A8
		private void ShowPoisonHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = ParticleStacks.instance.poisonHitStack.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				if (this.headCollider != null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
				}
				else
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * this.heightFlyOutHitEffect);
				}
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x000FF668 File Offset: 0x000FD868
		private void ShowCriticalHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = ParticleStacks.instance.criticalHitStack.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				if (this.headCollider != null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
				}
				else
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * this.heightFlyOutHitEffect);
				}
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x000FF728 File Offset: 0x000FD928
		private void ShowBleedingHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = ParticleStacks.instance.bleedHitStack.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				if (this.headCollider != null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
				}
				else
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * this.heightFlyOutHitEffect);
				}
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x000FF7E8 File Offset: 0x000FD9E8
		private void ShowHitEffect()
		{
			if (Device.isPixelGunLow)
			{
				return;
			}
			HitParticle currentParticle = ParticleStacks.instance.hitStack.GetCurrentParticle(false);
			if (currentParticle != null)
			{
				if (this.headCollider != null)
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, this.headCollider.transform.TransformPoint(this.headCollider.center));
				}
				else
				{
					currentParticle.StartShowParticle(base.transform.position, base.transform.rotation, false, base.transform.position + Vector3.up * this.heightFlyOutHitEffect);
				}
			}
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000FF8A8 File Offset: 0x000FDAA8
		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
		{
			this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x000FF8BC File Offset: 0x000FDABC
		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
		{
			if (Defs.isMulti)
			{
				if (!this.IsDeath)
				{
					this.GetDamageForMultiplayer(-damage, weaponName, typeKill);
					WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>().score = GlobalGameController.Score;
					WeaponManager.sharedManager.myNetworkStartTable.SynhScore();
				}
			}
			else
			{
				Transform transform = null;
				if (damageFrom != null)
				{
					transform = (damageFrom as MonoBehaviour).transform;
				}
				this.GetDamage(-damage, transform, weaponName, transform, typeKill);
			}
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x000FF93C File Offset: 0x000FDB3C
		public bool IsEnemyTo(Player_move_c player)
		{
			return true;
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x000FF940 File Offset: 0x000FDB40
		public bool IsDead()
		{
			return this.IsDeath;
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003109 RID: 12553 RVA: 0x000FF948 File Offset: 0x000FDB48
		public bool isLivingTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x000FF94C File Offset: 0x000FDB4C
		public void GetDamage(float damage, Transform instigator, string weaponName, bool isOwnerDamage = true, Player_move_c.TypeKills typeKills = Player_move_c.TypeKills.none)
		{
			this.GetDamage(damage, instigator, isOwnerDamage, typeKills);
			if (this._killed)
			{
				if (Application.isEditor)
				{
					Debug.LogWarning("Bot is receiving damage after death.");
				}
				return;
			}
			if (this.health != 0f)
			{
				return;
			}
			if (!isOwnerDamage)
			{
				return;
			}
			this._killed = true;
			if (!TrainingController.TrainingCompleted)
			{
				return;
			}
			if (Defs.isMulti && NetworkStartTable.LocalOrPasswordRoom())
			{
				return;
			}
			if (string.IsNullOrEmpty(weaponName))
			{
				return;
			}
			ShopNGUIController.CategoryNames weaponSlot = (ShopNGUIController.CategoryNames)(-1);
			string prefabName = weaponName.Replace("(Clone)", string.Empty);
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(prefabName);
			if (byPrefabName != null)
			{
				int itemCategory = ItemDb.GetItemCategory(byPrefabName.Tag);
				weaponSlot = (ShopNGUIController.CategoryNames)itemCategory;
			}
			bool campaign = !Defs.isMulti && !Defs.IsSurvival;
			QuestMediator.NotifyKillMonster(weaponSlot, campaign);
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x000FFA20 File Offset: 0x000FDC20
		public void GetDamage(float damage, Transform instigator, bool isOwnerDamage = true, Player_move_c.TypeKills typeKills = Player_move_c.TypeKills.none)
		{
			if (this.IsDeath)
			{
				return;
			}
			if (damage < 0f && !this._isFlashing)
			{
				base.StartCoroutine(this.ShowDamageEffect(typeKills == Player_move_c.TypeKills.poison));
			}
			if (typeKills == Player_move_c.TypeKills.headshot)
			{
				this.ShowHeadShotEffect();
				damage *= 2f;
			}
			else if (typeKills == Player_move_c.TypeKills.poison)
			{
				this.ShowPoisonHitEffect();
			}
			else if (typeKills == Player_move_c.TypeKills.critical)
			{
				this.ShowCriticalHitEffect();
			}
			else if (typeKills == Player_move_c.TypeKills.bleeding)
			{
				this.ShowBleedingHitEffect();
			}
			else
			{
				this.ShowHitEffect();
			}
			this.health += damage;
			if (this.health < 0f)
			{
				this.health = 0f;
			}
			if (this.health == 0f)
			{
				this.PrepareDeath(isOwnerDamage);
				if (isOwnerDamage)
				{
					this.TakeBonusForKill();
				}
			}
			else if (isOwnerDamage)
			{
				GlobalGameController.Score += 5;
			}
			this.TryPlayDamageSound(this.damageSound.length);
			if (instigator != null && this.health > 0f)
			{
				this.botAiController.SetTargetForced(instigator);
			}
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x000FFB5C File Offset: 0x000FDD5C
		private BotDebuff GetDebuffByType(BotDebuffType type)
		{
			for (int i = 0; i < this._botDebufs.Count; i++)
			{
				if (this._botDebufs[i].type == type)
				{
					return this._botDebufs[i];
				}
			}
			return null;
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x000FFBAC File Offset: 0x000FDDAC
		private void RunDebuff(BotDebuff debuff)
		{
			if (debuff.type == BotDebuffType.DecreaserSpeed)
			{
				float floatParametr = debuff.GetFloatParametr();
				this._modMoveSpeedByDebuff = floatParametr;
			}
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x000FFBD4 File Offset: 0x000FDDD4
		private void StopDebuff(BotDebuff debuff)
		{
			if (debuff.type == BotDebuffType.DecreaserSpeed)
			{
				this._modMoveSpeedByDebuff = 1f;
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x000FFBEC File Offset: 0x000FDDEC
		public void ApplyDebuffByMode(BotDebuffType type, float timeLife, object parametrs)
		{
			if (!this._isMultiplayerMode)
			{
				this.ApplyDebuff(type, timeLife, parametrs);
				return;
			}
			this.ApplyDebufForMultiplayer(type, timeLife, parametrs);
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x000FFC0C File Offset: 0x000FDE0C
		private void ReplaceDebuff(BotDebuff oldDebuff, float newTimeLife, object newParametrs)
		{
			if (oldDebuff.type == BotDebuffType.DecreaserSpeed)
			{
				oldDebuff.ReplaceValues(newTimeLife, newParametrs);
				this.RunDebuff(oldDebuff);
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x000FFC28 File Offset: 0x000FDE28
		public void ApplyDebuff(BotDebuffType type, float timeLife, object parametrs)
		{
			BotDebuff debuffByType = this.GetDebuffByType(type);
			if (debuffByType == null)
			{
				BotDebuff botDebuff = new BotDebuff(type, timeLife, parametrs);
				botDebuff.OnRun += this.RunDebuff;
				botDebuff.OnStop += this.StopDebuff;
				this._botDebufs.Add(botDebuff);
			}
			else
			{
				this.ReplaceDebuff(debuffByType, timeLife, parametrs);
			}
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x000FFC8C File Offset: 0x000FDE8C
		public void UpdateDebuffState()
		{
			if (this._botDebufs.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._botDebufs.Count; i++)
			{
				if (!this._botDebufs[i].isRun)
				{
					this._botDebufs[i].Run();
				}
				else
				{
					this._botDebufs[i].timeLife -= Time.deltaTime;
					if (this._botDebufs[i].timeLife <= 0f)
					{
						this._botDebufs[i].Stop();
						this._botDebufs.Remove(this._botDebufs[i]);
					}
				}
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x000FFD54 File Offset: 0x000FDF54
		private void InitNetworkStateData()
		{
			this._botPosition = base.transform.position;
			this._botRotation = base.transform.rotation;
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x000FFD84 File Offset: 0x000FDF84
		private void DisableMobForDeleteMasterClient()
		{
			this.modelCollider.gameObject.SetActive(false);
			if (this.headCollider != null)
			{
				this.headCollider.gameObject.SetActive(false);
			}
			MonoBehaviour[] components = base.GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				bool flag = components[i] as PhotonView != null;
				bool flag2 = components[i] as BaseBot != null;
				if (!flag && !flag2)
				{
					components[i].enabled = false;
				}
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x000FFE18 File Offset: 0x000FE018
		public void DestroyByNetworkType()
		{
			if (!this._isMultiplayerMode)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			else
			{
				this.needDestroyByMasterClient = true;
				this.DisableMobForDeleteMasterClient();
			}
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x000FFE64 File Offset: 0x000FE064
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (!this._isMultiplayerMode)
			{
				return;
			}
			if (stream.isWriting)
			{
				stream.SendNext(base.transform.position);
				stream.SendNext(base.transform.rotation);
			}
			else
			{
				this._botPosition = (Vector3)stream.ReceiveNext();
				this._botRotation = (Quaternion)stream.ReceiveNext();
			}
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x000FFEDC File Offset: 0x000FE0DC
		private void Update()
		{
			this.UpdateDebuffState();
			if (base.GetComponent<AudioSource>().enabled == (Time.timeScale == 0f))
			{
				base.GetComponent<AudioSource>().enabled = !base.GetComponent<AudioSource>().enabled;
				if (base.GetComponent<AudioSource>().enabled)
				{
					base.GetComponent<AudioSource>().Play();
				}
			}
			if (!this._isMultiplayerMode)
			{
				return;
			}
			if (PhotonNetwork.isMasterClient && this.needDestroyByMasterClient)
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
			if (!this._photonView.isMine && !this.needDestroyByMasterClient)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this._botPosition, Time.deltaTime * 5f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this._botRotation, Time.deltaTime * 5f);
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000FFFE0 File Offset: 0x000FE1E0
		public void FireByRPC(Vector3 pointFire, Vector3 positionToFire)
		{
			this._photonView.RPC("FireBulletRPC", PhotonTargets.Others, new object[]
			{
				pointFire,
				positionToFire
			});
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x0010000C File Offset: 0x000FE20C
		[PunRPC]
		[RPC]
		public void SetBotHealthRPC(float botHealth)
		{
			this.health = botHealth;
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x00100018 File Offset: 0x000FE218
		[PunRPC]
		[RPC]
		public void PlayZombieRunRPC()
		{
			this.PlayAnimationZombieWalk();
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieWalk;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x00100028 File Offset: 0x000FE228
		[RPC]
		[PunRPC]
		public void PlayZombieAttackRPC()
		{
			this.PlayAnimationZombieAttackOrStop();
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieAttackOrStop;
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x00100038 File Offset: 0x000FE238
		[RPC]
		[PunRPC]
		public void GetDamageRPC(float damage, int _typeKills)
		{
			this.GetDamage(damage, null, false, (Player_move_c.TypeKills)_typeKills);
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x00100044 File Offset: 0x000FE244
		public void GetDamageForMultiplayer(float damage, string weaponName, Player_move_c.TypeKills typeKills)
		{
			this.GetDamage(damage, null, weaponName, true, typeKills);
			this._photonView.RPC("GetDamageRPC", PhotonTargets.Others, new object[]
			{
				damage,
				(int)typeKills
			});
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x00100088 File Offset: 0x000FE288
		[RPC]
		[PunRPC]
		public void ApplyDebuffRPC(int typeDebuff, float timeLife, float parametr)
		{
			this.ApplyDebuff((BotDebuffType)typeDebuff, timeLife, parametr);
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x00100098 File Offset: 0x000FE298
		public void ApplyDebufForMultiplayer(BotDebuffType type, float timeLife, object parametrs)
		{
			this.ApplyDebuff(type, timeLife, parametrs);
			if (type == BotDebuffType.DecreaserSpeed)
			{
				this._photonView.RPC("ApplyDebuffRPC", this._photonView.owner, new object[]
				{
					(int)type,
					timeLife,
					(float)parametrs
				});
			}
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x001000F8 File Offset: 0x000FE2F8
		public void PlayAnimZombieWalkByMode()
		{
			if (!this._isMultiplayerMode)
			{
				this.PlayAnimationZombieWalk();
				return;
			}
			if (this._currentRunNetworkAnimation != BaseBot.RunNetworkAnimationType.ZombieWalk)
			{
				this.PlayAnimationZombieWalk();
				this._photonView.RPC("PlayZombieRunRPC", PhotonTargets.Others, new object[0]);
			}
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieWalk;
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x00100148 File Offset: 0x000FE348
		public void PlayAnimZombieAttackOrStopByMode()
		{
			if (!this._isMultiplayerMode)
			{
				this.PlayAnimationZombieAttackOrStop();
				return;
			}
			if (this._currentRunNetworkAnimation != BaseBot.RunNetworkAnimationType.ZombieAttackOrStop)
			{
				this.PlayAnimationZombieAttackOrStop();
				this._photonView.RPC("PlayZombieAttackRPC", PhotonTargets.Others, new object[0]);
			}
			this._currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.ZombieAttackOrStop;
		}

		// Token: 0x040023E0 RID: 9184
		public const string BaseNameBotGuard = "BossGuard";

		// Token: 0x040023E1 RID: 9185
		private const int ScoreForDamage = 5;

		// Token: 0x040023E2 RID: 9186
		private const float HeadShotDamageModif = 2f;

		// Token: 0x040023E3 RID: 9187
		private const int MultiplySmoothMove = 5;

		// Token: 0x040023E4 RID: 9188
		public string nameBot;

		// Token: 0x040023E5 RID: 9189
		[Header("Sound settings")]
		public AudioClip damageSound;

		// Token: 0x040023E6 RID: 9190
		public AudioClip voiceMobSoud;

		// Token: 0x040023E7 RID: 9191
		public AudioClip takeDamageSound;

		// Token: 0x040023E8 RID: 9192
		public AudioClip deathSound;

		// Token: 0x040023E9 RID: 9193
		public AudioClip stepSound;

		// Token: 0x040023EA RID: 9194
		public AudioClip runStepSound;

		// Token: 0x040023EB RID: 9195
		[Header("Common damage settings")]
		public float notAttackingSpeed;

		// Token: 0x040023EC RID: 9196
		public float attackingSpeed;

		// Token: 0x040023ED RID: 9197
		public float health;

		// Token: 0x040023EE RID: 9198
		public float attackDistance;

		// Token: 0x040023EF RID: 9199
		public float detectRadius;

		// Token: 0x040023F0 RID: 9200
		public float damagePerHit;

		// Token: 0x040023F1 RID: 9201
		public int scorePerKill;

		// Token: 0x040023F2 RID: 9202
		public float[] attackingSpeedRandomRange;

		// Token: 0x040023F3 RID: 9203
		[Header("Effects settings")]
		public Texture flashDeadthTexture;

		// Token: 0x040023F4 RID: 9204
		public float heightFlyOutHitEffect;

		// Token: 0x040023F5 RID: 9205
		[NonSerialized]
		public int indexMobPrefabForCoop;

		// Token: 0x040023F6 RID: 9206
		protected BotAiController botAiController;

		// Token: 0x040023F7 RID: 9207
		protected Transform mobModel;

		// Token: 0x040023F8 RID: 9208
		protected Animation animations;

		// Token: 0x040023F9 RID: 9209
		protected bool isMobChampion;

		// Token: 0x040023FA RID: 9210
		protected BoxCollider modelCollider;

		// Token: 0x040023FB RID: 9211
		protected SphereCollider headCollider;

		// Token: 0x040023FC RID: 9212
		protected BaseBot.BotAnimationName animationsName;

		// Token: 0x040023FD RID: 9213
		protected AudioSource audioSource;

		// Token: 0x040023FE RID: 9214
		private bool _isFlashing;

		// Token: 0x040023FF RID: 9215
		private PhotonView _photonView;

		// Token: 0x04002400 RID: 9216
		private bool _isMultiplayerMode;

		// Token: 0x04002401 RID: 9217
		private BotChangeDamageMaterial[] _botMaterials;

		// Token: 0x04002402 RID: 9218
		private IEnemyEffectsManager _effectsManager;

		// Token: 0x04002403 RID: 9219
		private AdvancedEffects advancedEffects;

		// Token: 0x04002404 RID: 9220
		private float[] coefHealthFromTier;

		// Token: 0x04002405 RID: 9221
		private bool _isPlayingDamageSound;

		// Token: 0x04002406 RID: 9222
		private bool _isDeathAudioPlaying;

		// Token: 0x04002407 RID: 9223
		[Header("Automatic animation speed settings")]
		public bool isAutomaticAnimationEnable;

		// Token: 0x04002408 RID: 9224
		[Range(0.1f, 2f)]
		public float speedAnimationWalk;

		// Token: 0x04002409 RID: 9225
		[Range(0.1f, 2f)]
		public float speedAnimationRun;

		// Token: 0x0400240A RID: 9226
		[Range(0.1f, 2f)]
		public float speedAnimationAttack;

		// Token: 0x0400240B RID: 9227
		[Header("Flying settings")]
		public bool isFlyingSpeedLimit;

		// Token: 0x0400240C RID: 9228
		public float maxFlyingSpeed;

		// Token: 0x0400240D RID: 9229
		[Header("Guard settings")]
		public GameObject[] guards;

		// Token: 0x0400240E RID: 9230
		private bool _isWeaponCreated;

		// Token: 0x0400240F RID: 9231
		private bool _killed;

		// Token: 0x04002410 RID: 9232
		private float _modMoveSpeedByDebuff = 1f;

		// Token: 0x04002411 RID: 9233
		private List<BotDebuff> _botDebufs = new List<BotDebuff>();

		// Token: 0x04002412 RID: 9234
		private Vector3 _botPosition;

		// Token: 0x04002413 RID: 9235
		private Quaternion _botRotation;

		// Token: 0x04002414 RID: 9236
		private BaseBot.RunNetworkAnimationType _currentRunNetworkAnimation = BaseBot.RunNetworkAnimationType.None;

		// Token: 0x02000580 RID: 1408
		protected class BotAnimationName
		{
			// Token: 0x04002419 RID: 9241
			public string Walk = "Norm_Walk";

			// Token: 0x0400241A RID: 9242
			public string Run = "Zombie_Walk";

			// Token: 0x0400241B RID: 9243
			public string Stop = "Zombie_Off";

			// Token: 0x0400241C RID: 9244
			public string Death = "Zombie_Dead";

			// Token: 0x0400241D RID: 9245
			public string Attack = "Zombie_Attack";

			// Token: 0x0400241E RID: 9246
			public string Idle = "Idle";
		}

		// Token: 0x02000581 RID: 1409
		private enum RunNetworkAnimationType
		{
			// Token: 0x04002420 RID: 9248
			ZombieWalk,
			// Token: 0x04002421 RID: 9249
			ZombieAttackOrStop,
			// Token: 0x04002422 RID: 9250
			None
		}
	}
}
