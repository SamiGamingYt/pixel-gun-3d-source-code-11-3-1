using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x020006DE RID: 1758
	public abstract class PetEngine : StateMachine<PetState>, IDamageable
	{
		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06003CF8 RID: 15608 RVA: 0x0013D094 File Offset: 0x0013B294
		// (set) Token: 0x06003CF9 RID: 15609 RVA: 0x0013D09C File Offset: 0x0013B29C
		public float CurrentHealth
		{
			get
			{
				return this._CurrentHealth;
			}
			set
			{
				this._CurrentHealth = value;
				this.SetCollidersEnabled(this._CurrentHealth > 0f);
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06003CFA RID: 15610 RVA: 0x0013D0B8 File Offset: 0x0013B2B8
		// (set) Token: 0x06003CFB RID: 15611 RVA: 0x0013D0C0 File Offset: 0x0013B2C0
		public Player_move_c Owner
		{
			get
			{
				return this._owner;
			}
			private set
			{
				this._owner = value;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06003CFC RID: 15612 RVA: 0x0013D0CC File Offset: 0x0013B2CC
		public PetInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003CFD RID: 15613 RVA: 0x0013D0D4 File Offset: 0x0013B2D4
		public GameObject Model
		{
			get
			{
				if (this._model == null)
				{
					this._model = base.gameObject.GetChildGameObject("Body", true);
					if (this._model == null)
					{
						Debug.LogErrorFormat("[PETS] model object not found for pet '{0}'. Please rename root model object to 'Body'", new object[]
						{
							base.gameObject.name
						});
					}
				}
				return this._model;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06003CFE RID: 15614 RVA: 0x0013D140 File Offset: 0x0013B340
		public Animation Animator
		{
			get
			{
				if (this._animator == null)
				{
					this._animator = this.Model.GetComponent<Animation>();
				}
				return this._animator;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06003CFF RID: 15615 RVA: 0x0013D178 File Offset: 0x0013B378
		public AnimationHandler AnimationHandler
		{
			get
			{
				if (this._animationHandler == null)
				{
					this._animationHandler = this.Model.GetComponent<AnimationHandler>();
				}
				return this._animationHandler;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003D00 RID: 15616 RVA: 0x0013D1B0 File Offset: 0x0013B3B0
		private PetHighlightComponent _highlightComponent
		{
			get
			{
				if (this._highlightComponentComponentValue == null)
				{
					this._highlightComponentComponentValue = base.gameObject.GetComponent<PetHighlightComponent>();
				}
				return this._highlightComponentComponentValue;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003D01 RID: 15617 RVA: 0x0013D1E8 File Offset: 0x0013B3E8
		// (set) Token: 0x06003D02 RID: 15618 RVA: 0x0013D1F0 File Offset: 0x0013B3F0
		public bool PetAlive { get; private set; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003D03 RID: 15619 RVA: 0x0013D1FC File Offset: 0x0013B3FC
		public bool IsAlive
		{
			get
			{
				return this.CurrentHealth > 0f;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003D04 RID: 15620 RVA: 0x0013D20C File Offset: 0x0013B40C
		protected virtual Vector3? MoveToTargetPosition
		{
			get
			{
				if (this.Target != null)
				{
					return new Vector3?(this.Target.position);
				}
				return null;
			}
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x0013D244 File Offset: 0x0013B444
		public PetAnimation GetAnimation(PetAnimationType type)
		{
			return this.Animations.FirstOrDefault((PetAnimation a) => a.Type == type);
		}

		// Token: 0x06003D06 RID: 15622 RVA: 0x0013D278 File Offset: 0x0013B478
		public string GetAnimationName(PetAnimationType type)
		{
			PetAnimation animation = this.GetAnimation(type);
			return (animation == null) ? string.Empty : animation.AnimationName;
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003D07 RID: 15623
		public abstract Vector3 MovePosition { get; }

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06003D08 RID: 15624 RVA: 0x0013D2A4 File Offset: 0x0013B4A4
		public virtual bool CanMoveToPlayer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06003D09 RID: 15625 RVA: 0x0013D2A8 File Offset: 0x0013B4A8
		// (set) Token: 0x06003D0A RID: 15626 RVA: 0x0013D2B0 File Offset: 0x0013B4B0
		public bool IsMine
		{
			get
			{
				return this._isMine;
			}
			set
			{
				this._isMine = value;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06003D0B RID: 15627 RVA: 0x0013D2BC File Offset: 0x0013B4BC
		public float RespawnTime
		{
			get
			{
				return this.Info.RespawnTime;
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06003D0C RID: 15628 RVA: 0x0013D2CC File Offset: 0x0013B4CC
		public float RespawnTimeLeft
		{
			get
			{
				if (this._dieTime < 0f)
				{
					return 0f;
				}
				return this._dieTime + this.Info.RespawnTime - Time.realtimeSinceStartup;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06003D0D RID: 15629 RVA: 0x0013D308 File Offset: 0x0013B508
		// (set) Token: 0x06003D0E RID: 15630 RVA: 0x0013D318 File Offset: 0x0013B518
		public bool IsImmortal
		{
			get
			{
				return this._immortalStartTime > 0f;
			}
			set
			{
				if (value)
				{
					this._immortalStartTime = Time.realtimeSinceStartup;
				}
				else
				{
					this._immortalStartTime = 0f;
				}
			}
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x0013D33C File Offset: 0x0013B53C
		public void OwnerAttacked(Player_move_c.TypeKills typeKill, int idKiller)
		{
			if (typeKill == Player_move_c.TypeKills.mob || !Defs.isMulti)
			{
				return;
			}
			if (this.Offender != null)
			{
				return;
			}
			if (!this.IsAlive || this.Owner == null || base.CurrentState.StateId == PetState.none || base.CurrentState.StateId == PetState.teleport || base.CurrentState.StateId == PetState.death || base.CurrentState.StateId == PetState.respawn)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.players[i].mySkinName.pixelView != null && Initializer.players[i].mySkinName.pixelView.viewID == idKiller)
				{
					num = i;
					break;
				}
			}
			if (num < 0 || Initializer.players[num] == this.Owner)
			{
				return;
			}
			if (Vector3.Distance(this.Owner.transform.position, Initializer.players[num].transform.position) <= this.Info.OffenderDetectRange)
			{
				this.Offender = Initializer.players[num];
			}
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0013D4A0 File Offset: 0x0013B6A0
		public bool IsEnemyTo(Player_move_c player)
		{
			return !(this.Owner != null) || (Defs.isMulti && !Defs.isCOOP && !this.Owner.Equals(player) && (!ConnectSceneNGUIController.isTeamRegim || this.Owner.myCommand != player.myCommand));
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0013D508 File Offset: 0x0013B708
		public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
		{
			this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0013D51C File Offset: 0x0013B71C
		public virtual void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeShoot, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
		{
			if (!this.IsAlive || Defs.isDaterRegim || this.IsImmortal)
			{
				return;
			}
			if (this.Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline))
			{
				damage *= 0.5f;
			}
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					this.photonView.RPC("ApplyDamageRPC", PhotonTargets.All, new object[]
					{
						damage,
						killerId
					});
				}
				else
				{
					this._networkView.RPC("ApplyDamageRPC", RPCMode.All, new object[]
					{
						damage,
						killerId
					});
				}
			}
			else
			{
				this.ApplyDamageFrom(damage, 0);
			}
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0013D5E0 File Offset: 0x0013B7E0
		public bool IsDead()
		{
			return this.CurrentHealth <= 0f;
		}

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06003D14 RID: 15636 RVA: 0x0013D5F4 File Offset: 0x0013B7F4
		public bool isLivingTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x0013D5F8 File Offset: 0x0013B7F8
		public void ApplyDamageFrom(float damage, int idKiller = 0)
		{
			this.CurrentHealth -= Mathf.Clamp(damage, 0f, float.MaxValue);
			if (this._highlightComponent != null)
			{
				this._highlightComponent.Hit();
			}
			if (Defs.isMulti && this.IsMine && this.CurrentHealth <= 0f && idKiller > 0)
			{
				if (Defs.isInet)
				{
					this.photonView.RPC("KilledByRPC", PhotonTargets.Others, new object[]
					{
						idKiller
					});
				}
				else
				{
					this._networkView.RPC("KilledByRPC", RPCMode.Others, new object[]
					{
						idKiller
					});
				}
			}
			if (Defs.isMulti && !this.IsMine && this.CurrentHealth <= 0f && !this.DeathEffectIsPlaying)
			{
				this.PlayDeathEffects();
			}
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x0013D6F0 File Offset: 0x0013B8F0
		[RPC]
		[PunRPC]
		public void ApplyDamageRPC(float damage, int idKiller)
		{
			this.ApplyDamageFrom(damage, idKiller);
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x0013D6FC File Offset: 0x0013B8FC
		[RPC]
		[PunRPC]
		public void KilledByRPC(int idKiller)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.skinNamePixelView.viewID == idKiller)
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddScoreKillPet();
			}
		}

		// Token: 0x06003D18 RID: 15640 RVA: 0x0013D748 File Offset: 0x0013B948
		protected bool IsMoveAnimation(PetAnimationType animationType)
		{
			return animationType == PetAnimationType.Walk || animationType == PetAnimationType.Run;
		}

		// Token: 0x06003D19 RID: 15641 RVA: 0x0013D758 File Offset: 0x0013B958
		public virtual void PlayAnimation(PetAnimationType animationType)
		{
			if (this._currAnimation != null && !string.IsNullOrEmpty(this._currAnimation.AnimationName))
			{
				this.Animator[this._currAnimation.AnimationName].speed = 1f;
				this._currAnimation = null;
			}
			PetAnimation animation = this.GetAnimation(animationType);
			if (animation == null)
			{
				if (animationType != PetAnimationType.Run)
				{
					return;
				}
				animationType = PetAnimationType.Walk;
				animation = this.GetAnimation(animationType);
				if (animation == null)
				{
					return;
				}
			}
			this.Animator.Play(animation.AnimationName);
			this._currAnimation = animation;
			if (this._currAnimation.Type == PetAnimationType.Attack)
			{
				this.PlaySound(this.ClipAttack);
			}
			else if (this._currAnimation.Type == PetAnimationType.Dead)
			{
				this.PlaySound(this.ClipDeath);
			}
			if (this.IsMine)
			{
				this.SetMovementAnimSpeed();
			}
			if (this.IsMine && Defs.isMulti && animationType != PetAnimationType.Run && animationType != PetAnimationType.Walk)
			{
				this.synchScript.currentAnimation = animationType;
			}
		}

		// Token: 0x06003D1A RID: 15642 RVA: 0x0013D874 File Offset: 0x0013BA74
		public void SetMovementAnimSpeed()
		{
			if (this.NOT_SCALE_ANIMS)
			{
				return;
			}
			if (this._currAnimation != null && (this._currAnimation.Type == PetAnimationType.Walk || this._currAnimation.Type == PetAnimationType.Run))
			{
				this.Animator[this._currAnimation.AnimationName].speed = this.Speed * this._currAnimation.SpeedModificator;
			}
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003D1B RID: 15643 RVA: 0x0013D8E8 File Offset: 0x0013BAE8
		protected virtual StateMachine<PetState>.State<PetState> IdleState
		{
			get
			{
				return new PetEngine.PetIdleState(this);
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06003D1C RID: 15644 RVA: 0x0013D8F0 File Offset: 0x0013BAF0
		protected virtual StateMachine<PetState>.State<PetState> AttackState
		{
			get
			{
				return new PetEngine.PetAttackState(this);
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06003D1D RID: 15645 RVA: 0x0013D8F8 File Offset: 0x0013BAF8
		protected virtual StateMachine<PetState>.State<PetState> DeathState
		{
			get
			{
				return new PetEngine.PetDeathState(this);
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06003D1E RID: 15646 RVA: 0x0013D900 File Offset: 0x0013BB00
		protected virtual StateMachine<PetState>.State<PetState> RespawnState
		{
			get
			{
				return new PetEngine.PetRespawnState(this);
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003D1F RID: 15647 RVA: 0x0013D908 File Offset: 0x0013BB08
		protected virtual StateMachine<PetState>.State<PetState> TeleportState
		{
			get
			{
				return new PetEngine.PetTeleportState(this);
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06003D20 RID: 15648
		protected abstract StateMachine<PetState>.State<PetState> MoveToOwnerState { get; }

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003D21 RID: 15649
		protected abstract StateMachine<PetState>.State<PetState> MoveToTargetState { get; }

		// Token: 0x06003D22 RID: 15650 RVA: 0x0013D910 File Offset: 0x0013BB10
		protected virtual void Awake()
		{
			base.gameObject.AddComponent<AdvancedEffects>();
			if (Defs.isMulti)
			{
				if (Defs.isInet && base.GetComponent<PhotonView>() != null)
				{
					this.photonView = base.GetComponent<PhotonView>();
					if (this.photonView.isMine)
					{
						this.IsMine = true;
					}
				}
				if (!Defs.isInet && base.GetComponent<NetworkView>() != null)
				{
					this._networkView = base.GetComponent<NetworkView>();
					if (this._networkView.isMine)
					{
						this.IsMine = true;
					}
				}
			}
			else
			{
				this.IsMine = true;
			}
			this.synchScript = base.GetComponent<NetworkPetEngineSynch>();
			this.ThisTransform = base.transform;
			this.Scanner = base.GetComponent<TargetScanner>();
			this.Scanner.LoopPoint = this._lookPoint;
			this.BodyCollider = base.gameObject.GetComponent<Collider>();
			this.HeadCollider = base.gameObject.Child("HeadCollider").GetComponent<Collider>();
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x0013DA1C File Offset: 0x0013BC1C
		public void SetInfo(string id)
		{
			PlayerPet playerPet = Singleton<PetsManager>.Instance.GetPlayerPet(id);
			if (playerPet != null)
			{
				this._info = playerPet.Info;
				this.Scanner.DetectRadius = this._info.TargetDetectRange;
			}
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x0013DA60 File Offset: 0x0013BC60
		private void Start()
		{
			Initializer.petsObj.Add(base.gameObject);
			this.SetOwner();
			base.Enabled = this.IsMine;
			if (this.IsMine)
			{
				this.SetName(Singleton<PetsManager>.Instance.GetPlayerPet(this.Info.Id).PetName);
				this.InitSM();
				this.SetInfo(base.gameObject.nameNoClone());
				if (this._info == null)
				{
					Debug.LogErrorFormat("[PETS] info for pet '{0}' not found", new object[]
					{
						base.gameObject.name
					});
					return;
				}
				this.CurrentHealth = this.Info.HP;
				if (Defs.isMulti)
				{
					this.SendSynhCurrentHealth();
				}
			}
			else
			{
				this.StopEngine();
			}
			if (Defs.isMulti && Defs.isInet && this.IsMine)
			{
				PhotonObjectCacher.AddObject(base.gameObject);
			}
			this.IsImmortal = true;
			if (this.EffectShow != null)
			{
				this.EffectShow.gameObject.transform.SetParent(null);
			}
			if (this.EffectHide != null)
			{
				this.EffectHide.gameObject.transform.SetParent(null);
			}
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x0013DBA8 File Offset: 0x0013BDA8
		protected virtual void InitSM()
		{
			base.Clean();
			base.Register(this.IdleState);
			base.Register(this.MoveToOwnerState);
			base.Register(this.MoveToTargetState);
			base.Register(this.AttackState);
			base.Register(this.DeathState);
			base.Register(this.RespawnState);
			base.Register(this.TeleportState);
			if (base.Enabled)
			{
				base.To(PetState.idle);
			}
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x0013DC28 File Offset: 0x0013BE28
		protected virtual void StopEngine()
		{
			this.Scanner.enabled = false;
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x0013DC38 File Offset: 0x0013BE38
		public bool CanShowPet()
		{
			return this.Owner.transform.position.y >= -5000f && !this.Owner.isKilled;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003D28 RID: 15656 RVA: 0x0013DC78 File Offset: 0x0013BE78
		// (set) Token: 0x06003D29 RID: 15657 RVA: 0x0013DC80 File Offset: 0x0013BE80
		public float StayTime
		{
			get
			{
				return this._stayTime;
			}
			private set
			{
				this._stayTime = value;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06003D2A RID: 15658 RVA: 0x0013DC8C File Offset: 0x0013BE8C
		// (set) Token: 0x06003D2B RID: 15659 RVA: 0x0013DC94 File Offset: 0x0013BE94
		public bool IsStay { get; private set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003D2C RID: 15660 RVA: 0x0013DCA0 File Offset: 0x0013BEA0
		// (set) Token: 0x06003D2D RID: 15661 RVA: 0x0013DCA8 File Offset: 0x0013BEA8
		public float Acceleration
		{
			get
			{
				return this._acceleration;
			}
			private set
			{
				this._acceleration = value;
			}
		}

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x0013DCB4 File Offset: 0x0013BEB4
		public bool IsMoving
		{
			get
			{
				return this.Acceleration > 0.01f;
			}
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003D2F RID: 15663 RVA: 0x0013DCC4 File Offset: 0x0013BEC4
		// (set) Token: 0x06003D30 RID: 15664 RVA: 0x0013DCCC File Offset: 0x0013BECC
		public float Speed
		{
			get
			{
				return this._speed;
			}
			private set
			{
				this._speed = value;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003D31 RID: 15665 RVA: 0x0013DCD8 File Offset: 0x0013BED8
		public float ToOwnerDistance
		{
			get
			{
				return Vector3.Distance(this.ThisTransform.position, this.Owner.transform.position);
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003D32 RID: 15666 RVA: 0x0013DD08 File Offset: 0x0013BF08
		public float ToTargetDistance
		{
			get
			{
				return Vector3.Distance(this.ThisTransform.position, this.Target.position);
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0013DD28 File Offset: 0x0013BF28
		public virtual float GetCalculatedSpeedMultyplier()
		{
			if (base.CurrentState.StateId == PetState.moveToOwner)
			{
				return (this.ToOwnerDistance >= this.Info.MaxToOwnerDistance * 0.3f) ? (1f + 0.05f * this.ToOwnerDistance) : 1f;
			}
			if (base.CurrentState.StateId == PetState.moveToTarget)
			{
				return (this.ToTargetDistance >= this.Info.AttackStopDistance) ? (1f + 0.1f * this.ToTargetDistance) : 1f;
			}
			return 1f;
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0013DDC8 File Offset: 0x0013BFC8
		protected virtual void Update()
		{
			if (this._prevPosition != null)
			{
				this.Acceleration = Mathf.Abs(Vector3.Distance(this._prevPosition.Value, this.ThisTransform.position));
				this.speedTempAcc += this.Acceleration;
				this.speedMonTime += Time.deltaTime;
				if (this.speedMonTime >= 0.1f)
				{
					this.Speed = this.speedTempAcc / this.speedMonTime;
					this.speedTempAcc = 0f;
					this.speedMonTime = 0f;
				}
			}
			this._prevPosition = new Vector3?(this.ThisTransform.position);
			this.StayTime = ((!this.IsMoving) ? (this.StayTime += Time.deltaTime) : 0f);
			this.IsStay = (this.StayTime >= this.PlayProfileAnimationAfterSecs);
			if (this.Owner != null)
			{
				if (this.Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline) && this.IsAlive)
				{
					if (this._petBuff != null)
					{
						this._petBuff.SetActiveSafe(true);
					}
					else if (!this._petBuffPrefabLoading)
					{
						base.StartCoroutine(this.LoadBuff());
					}
				}
				else if (this._petBuff != null)
				{
					this._petBuff.SetActiveSafe(false);
				}
			}
			if (!this.IsMine)
			{
				return;
			}
			if (this.Owner == null)
			{
				return;
			}
			if (this.Owner.isKilled)
			{
				this.Target = null;
				this.Offender = null;
				this.IsImmortal = true;
				if (base.CurrentState == null || (base.CurrentState.StateId != PetState.teleport && base.CurrentState.StateId != PetState.respawn))
				{
					base.To(PetState.teleport);
				}
				this.Tick();
				return;
			}
			if (!this.CanShowPet())
			{
				if (base.CurrentState == null || (base.CurrentState.StateId != PetState.teleport && base.CurrentState.StateId != PetState.respawn))
				{
					base.To(PetState.teleport);
				}
				this.Tick();
				return;
			}
			if (this.IsMine && this.PetName != Singleton<PetsManager>.Instance.GetPlayerPet(this.Info.Id).PetName)
			{
				this.SetName(Singleton<PetsManager>.Instance.GetPlayerPet(this.Info.Id).PetName);
			}
			if (this.Owner != null && this.IsMine && base.Enabled && !this.Owner.isKilled)
			{
				this.SetTarget();
				this.Tick();
			}
			if (base.CurrentState == null || (base.CurrentState.StateId != PetState.attack && base.CurrentState.StateId != PetState.death && base.CurrentState.StateId != PetState.respawn && base.CurrentState.StateId != PetState.teleport))
			{
				this.SetMovementAnimation();
			}
			if (this.IsImmortal)
			{
				this.BlinkImmortal();
			}
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x0013E114 File Offset: 0x0013C314
		private void SetTarget()
		{
			this.Target = null;
			if (this.Offender != null)
			{
				if (this.Offender.isKilled || Vector3.Distance(this.ThisTransform.position, this.Owner.transform.position) > this.Info.MaxToOwnerDistance)
				{
					this.Offender = null;
				}
				else if (this.Offender.gameObject.transform != null)
				{
					this.Target = this.Offender.gameObject.transform.root;
				}
			}
			if (this.Target == null)
			{
				this.Target = ((!(this.Scanner.Target != null)) ? null : this.Scanner.Target.transform);
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x0013E200 File Offset: 0x0013C400
		private IEnumerator LoadBuff()
		{
			this._petBuffPrefabLoading = true;
			ResourceRequest request = Resources.LoadAsync<GameObject>("Pets/pet_buf");
			yield return request;
			this._petBuffPrefabLoading = false;
			if (request.isDone)
			{
				this._petBuff = UnityEngine.Object.Instantiate<GameObject>(request.asset as GameObject);
				this._petBuff.transform.parent = this.ThisTransform;
				this._petBuff.transform.localPosition = Vector3.zero;
				this._petBuff.transform.localScale = Vector3.one;
				this._petBuff.transform.rotation = Quaternion.identity;
			}
			yield break;
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x0013E21C File Offset: 0x0013C41C
		public void BlinkImmortal()
		{
			this._highlightComponent.ImmortalBlinkStart(this.Owner.maxTimerImmortality);
			if (this._immortalStartTime + this.Owner.maxTimerImmortality <= Time.realtimeSinceStartup)
			{
				this.IsImmortal = false;
				this._highlightComponent.ImmortalBlinkStop();
			}
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x0013E270 File Offset: 0x0013C470
		public virtual void SetCollidersEnabled(bool enabled)
		{
			if (this.BodyCollider.enabled != enabled)
			{
				this.BodyCollider.enabled = enabled;
			}
			if (this.HeadCollider.enabled != enabled)
			{
				this.HeadCollider.enabled = enabled;
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x0013E2B8 File Offset: 0x0013C4B8
		public PetAnimationType SetMovementAnimation()
		{
			PetAnimationType petAnimationType;
			if (this.IsMoving)
			{
				petAnimationType = ((this.Speed >= this.Info.SpeedModif * Mathf.Clamp(this.ToRunSpeedPercent, 0f, 1f)) ? PetAnimationType.Run : PetAnimationType.Walk);
			}
			else
			{
				petAnimationType = ((!this.IsStay) ? PetAnimationType.Idle : PetAnimationType.Profile);
			}
			this.PlayAnimation(petAnimationType);
			this.PlaySound((petAnimationType != PetAnimationType.Walk && petAnimationType != PetAnimationType.Run) ? null : this.ClipWalk);
			return petAnimationType;
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x0013E344 File Offset: 0x0013C544
		public void Destroy()
		{
			base.Enabled = false;
			if (this.IsMine)
			{
				if (Defs.isMulti)
				{
					if (Defs.isInet)
					{
						PhotonNetwork.Destroy(base.gameObject);
					}
					else
					{
						Network.Destroy(base.gameObject);
					}
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0013E3A4 File Offset: 0x0013C5A4
		public void OnDestroy()
		{
			if (Initializer.petsObj.Contains(base.gameObject))
			{
				Initializer.petsObj.Remove(base.gameObject);
			}
			PhotonObjectCacher.RemoveObject(base.gameObject);
			if (this.EffectShow != null)
			{
				UnityEngine.Object.Destroy(this.EffectShow.gameObject);
			}
			if (this.EffectHide != null)
			{
				UnityEngine.Object.Destroy(this.EffectHide.gameObject);
			}
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x0013E424 File Offset: 0x0013C624
		public void PlayShowEffect()
		{
			if (this.EffectShow == null)
			{
				return;
			}
			this.EffectShow.gameObject.transform.position = this.ThisTransform.position;
			this.EffectShow.Play();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x0013E470 File Offset: 0x0013C670
		public void PlayHideEffect()
		{
			if (this.EffectHide == null)
			{
				return;
			}
			this.EffectHide.gameObject.transform.position = this.ThisTransform.position;
			this.EffectHide.Play();
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0013E4BC File Offset: 0x0013C6BC
		public void SetOwner()
		{
			if (Defs.isMulti && !this.IsMine)
			{
				if (Defs.isInet)
				{
					this.Owner = Initializer.GetPlayerMoveCWithPhotonOwnerID(this.photonView.ownerId);
				}
			}
			else
			{
				this.Owner = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0013E514 File Offset: 0x0013C714
		public void SendOwnerLocalRPC(NetworkViewID _ownerId)
		{
			this._networkView.RPC("SetOwnerLocalRPC", RPCMode.OthersBuffered, new object[]
			{
				_ownerId
			});
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0013E544 File Offset: 0x0013C744
		[RPC]
		public void SetOwnerLocalRPC(NetworkViewID _ownerId)
		{
			this.Owner = Initializer.GetPlayerMoveCWithLocalPlayerID(_ownerId);
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003D41 RID: 15681 RVA: 0x0013E554 File Offset: 0x0013C754
		public bool OwnerIsGrounded
		{
			get
			{
				return this.Owner.mySkinName.firstPersonControl.character.isGrounded;
			}
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x0013E570 File Offset: 0x0013C770
		public bool InRange(Vector3 first, Vector3 second, float range)
		{
			return (first - second).sqrMagnitude <= Mathf.Pow(range, 2f);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x0013E59C File Offset: 0x0013C79C
		public bool IsVisible(GameObject target, float maxCheckDistance = 200f)
		{
			Vector3 direction = target.transform.position - this._lookPoint.position;
			Ray ray = new Ray(this._lookPoint.position, direction);
			int layerMask = Tools.AllWithoutDamageCollidersMaskAndWithoutRocket & ~(1 << LayerMask.NameToLayer("Pets"));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, maxCheckDistance, layerMask))
			{
				GameObject gameObject = raycastHit.collider.transform.gameObject;
				return gameObject.Equals(target) || gameObject.layer == LayerMask.NameToLayer("MyPlayer") || gameObject.Ancestors().Any((GameObject a) => a.Equals(target));
			}
			return false;
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x0013E678 File Offset: 0x0013C878
		protected void AttackTarget()
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.Info.poisonEnabled)
			{
				this.Owner.PoisonShotWithEffect(this.Target.root.gameObject, new Player_move_c.PoisonParameters(this.Info.poisonType, this.Info.poisonDamagePercent, this.Info.Attack, this.Info.poisonTime, this.Info.poisonCount, Singleton<PetsManager>.Instance.GetFirstUpgrade(this.Info.Id).Id, WeaponSounds.TypeDead.angel));
			}
			IDamageable component = this.Target.GetComponent<IDamageable>();
			if (component == null)
			{
				return;
			}
			float num = this.Info.Attack;
			bool flag = this.Info.criticalHitChance >= UnityEngine.Random.Range(0, 100);
			if (flag)
			{
				num *= this.Info.criticalHitCoef;
			}
			if (this.Owner.IsGadgetEffectActive(Player_move_c.GadgetEffect.petAdrenaline))
			{
				num *= 2f;
			}
			component.ApplyDamage(num, this, (!flag) ? Player_move_c.TypeKills.pet : Player_move_c.TypeKills.critical, WeaponSounds.TypeDead.angel, Singleton<PetsManager>.Instance.GetFirstUpgrade(this.Info.Id).Id, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
		}

		// Token: 0x06003D45 RID: 15685 RVA: 0x0013E7C4 File Offset: 0x0013C9C4
		public virtual void WarpToOwner()
		{
			this.ThisTransform.position = this.MovePosition;
		}

		// Token: 0x06003D46 RID: 15686 RVA: 0x0013E7D8 File Offset: 0x0013C9D8
		public virtual void HideModel()
		{
			this.Model.SetActiveSafe(false);
			if (this.Model.activeInHierarchy)
			{
				this.PlayShowEffect();
				this.Model.SetActive(false);
			}
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x0013E814 File Offset: 0x0013CA14
		public virtual void ShowModel()
		{
			this.Model.SetActiveSafe(true);
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06003D48 RID: 15688 RVA: 0x0013E824 File Offset: 0x0013CA24
		// (set) Token: 0x06003D49 RID: 15689 RVA: 0x0013E82C File Offset: 0x0013CA2C
		public bool InAttackState { get; set; }

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06003D4A RID: 15690 RVA: 0x0013E838 File Offset: 0x0013CA38
		public bool DeathEffectIsPlaying
		{
			get
			{
				return this._deathEffectIsPlaying;
			}
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x0013E840 File Offset: 0x0013CA40
		public void PlayDeathEffects()
		{
			CoroutineRunner.Instance.StartCoroutine(this.PlayDeathEffectsCoroutine());
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x0013E854 File Offset: 0x0013CA54
		private IEnumerator PlayDeathEffectsCoroutine()
		{
			if (this._deathEffectIsPlaying)
			{
				yield break;
			}
			this._deathEffectIsPlaying = true;
			this.AnimationHandler.SubscribeTo(this.GetAnimationName(PetAnimationType.Dead), AnimationHandler.AnimState.Finished, true, delegate
			{
				bool animationPlaying = false;
			});
			this.PlayAnimation(PetAnimationType.Dead);
			yield return new WaitWhile(() => animationPlaying);
			this.PlayHideEffect();
			float timeLeft = this.EffectHide.WaitTime;
			while (timeLeft > 0f)
			{
				timeLeft -= Time.deltaTime;
				yield return null;
			}
			if (base.gameObject.Equals(null))
			{
				yield break;
			}
			base.transform.position = Vector3.down * 10000f;
			this._deathEffectIsPlaying = false;
			yield break;
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x0013E870 File Offset: 0x0013CA70
		public void UpdateCurrentHealth(float newValue)
		{
			float num = Mathf.Clamp(this.CurrentHealth + newValue, 0f, this.Info.HP);
			if (num != this.CurrentHealth)
			{
				this.CurrentHealth = num;
				if (Defs.isMulti && this.IsMine)
				{
					this.SendSynhCurrentHealth();
				}
			}
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x0013E8CC File Offset: 0x0013CACC
		public void SendSynhCurrentHealth()
		{
			if (Defs.isInet)
			{
				this.photonView.RPC("SynhCurrentHealthRPC", PhotonTargets.Others, new object[]
				{
					this.CurrentHealth
				});
			}
			else
			{
				this._networkView.RPC("SynhCurrentHealthRPC", RPCMode.Others, new object[]
				{
					this.CurrentHealth
				});
			}
		}

		// Token: 0x06003D4F RID: 15695 RVA: 0x0013E934 File Offset: 0x0013CB34
		[RPC]
		[PunRPC]
		public void SynhCurrentHealthRPC(float _health)
		{
			this.CurrentHealth = _health;
		}

		// Token: 0x06003D50 RID: 15696 RVA: 0x0013E940 File Offset: 0x0013CB40
		public void SetName(string _petName)
		{
			this.PetName = _petName;
			if (Defs.isMulti)
			{
				if (Defs.isInet)
				{
					this.photonView.RPC("SynhNameRPC", PhotonTargets.OthersBuffered, new object[]
					{
						this.PetName
					});
				}
				else
				{
					this._networkView.RPC("SynhNameRPC", RPCMode.OthersBuffered, new object[]
					{
						this.PetName
					});
				}
			}
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x0013E9B0 File Offset: 0x0013CBB0
		[PunRPC]
		[RPC]
		public void SynhNameRPC(string _petName)
		{
			this.PetName = _petName;
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x0013E9BC File Offset: 0x0013CBBC
		private void OnPlayerConnected(NetworkPlayer player)
		{
			if (this.IsMine)
			{
				this._networkView.RPC("SynhCurrentHealthRPC", player, new object[]
				{
					this.CurrentHealth
				});
			}
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0013E9FC File Offset: 0x0013CBFC
		public void OnPhotonPlayerConnected(PhotonPlayer player)
		{
			if (this.IsMine)
			{
				this.photonView.RPC("SynhCurrentHealthRPC", player, new object[]
				{
					this.CurrentHealth
				});
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x0013EA3C File Offset: 0x0013CC3C
		public AudioSource AudioSourceOne
		{
			get
			{
				AudioSource result;
				if ((result = this._audioSourceOneValue) == null)
				{
					result = (this._audioSourceOneValue = base.gameObject.GetComponentInChildren("Source1", true));
				}
				return result;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003D55 RID: 15701 RVA: 0x0013EA70 File Offset: 0x0013CC70
		public AudioSource AudioSourceTwo
		{
			get
			{
				AudioSource result;
				if ((result = this._audioSourceTwoValue) == null)
				{
					result = (this._audioSourceTwoValue = base.gameObject.GetComponentInChildren("Source2", true));
				}
				return result;
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x0013EAA4 File Offset: 0x0013CCA4
		public void PlaySound(AudioClip clip)
		{
			if (!Defs.isSoundFX)
			{
				return;
			}
			bool flag = clip == this.ClipAttack || this.ClipReceiveDamage;
			AudioSource audioSource = (!flag) ? this.AudioSourceOne : this.AudioSourceTwo;
			if (audioSource == null)
			{
				return;
			}
			if (clip == null)
			{
				audioSource.loop = false;
				return;
			}
			if (clip == audioSource.clip)
			{
				if (!audioSource.isPlaying)
				{
					audioSource.Play();
				}
				return;
			}
			audioSource.clip = clip;
			audioSource.Play();
			audioSource.loop = (clip == this.ClipWalk);
		}

		// Token: 0x04002D17 RID: 11543
		private const float speedMonInterval = 0.1f;

		// Token: 0x04002D18 RID: 11544
		private GameObject _petBuff;

		// Token: 0x04002D19 RID: 11545
		[ReadOnly]
		public float _CurrentHealth = 1f;

		// Token: 0x04002D1A RID: 11546
		public string PetName;

		// Token: 0x04002D1B RID: 11547
		[SerializeField]
		protected PetInfo _info;

		// Token: 0x04002D1C RID: 11548
		[SerializeField]
		public Transform _lookPoint;

		// Token: 0x04002D1D RID: 11549
		[SerializeField]
		public PetEffectHandler EffectShow;

		// Token: 0x04002D1E RID: 11550
		[SerializeField]
		public PetEffectHandler EffectHide;

		// Token: 0x04002D1F RID: 11551
		[SerializeField]
		protected float PlayProfileAnimationAfterSecs = 3f;

		// Token: 0x04002D20 RID: 11552
		[Range(0f, 1f)]
		[SerializeField]
		protected float ToRunSpeedPercent = 0.85f;

		// Token: 0x04002D21 RID: 11553
		public List<PetAnimation> Animations = new List<PetAnimation>
		{
			new PetAnimation
			{
				Type = PetAnimationType.Idle,
				AnimationName = "Idle"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Walk,
				AnimationName = "Walk"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Attack,
				AnimationName = "Attack"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Dead,
				AnimationName = "Dead"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Profile,
				AnimationName = "Profile"
			},
			new PetAnimation
			{
				Type = PetAnimationType.Walk,
				AnimationName = "Run"
			}
		};

		// Token: 0x04002D22 RID: 11554
		[Header("-== Sounds ==-")]
		public AudioClip ClipWalk;

		// Token: 0x04002D23 RID: 11555
		public AudioClip ClipAttack;

		// Token: 0x04002D24 RID: 11556
		public AudioClip ClipReceiveDamage;

		// Token: 0x04002D25 RID: 11557
		public AudioClip ClipDeath;

		// Token: 0x04002D26 RID: 11558
		public AudioClip ClipTap;

		// Token: 0x04002D27 RID: 11559
		[Header("-== from script setted ==-")]
		[SerializeField]
		[ReadOnly]
		private Player_move_c _owner;

		// Token: 0x04002D28 RID: 11560
		[ReadOnly]
		public Transform Target;

		// Token: 0x04002D29 RID: 11561
		[SerializeField]
		[ReadOnly]
		protected Player_move_c Offender;

		// Token: 0x04002D2A RID: 11562
		[ReadOnly]
		public TargetScanner Scanner;

		// Token: 0x04002D2B RID: 11563
		public Collider BodyCollider;

		// Token: 0x04002D2C RID: 11564
		public Collider HeadCollider;

		// Token: 0x04002D2D RID: 11565
		[NonSerialized]
		public Transform ThisTransform;

		// Token: 0x04002D2E RID: 11566
		[ReadOnly]
		public NetworkPetEngineSynch synchScript;

		// Token: 0x04002D2F RID: 11567
		[SerializeField]
		[ReadOnly]
		private GameObject _model;

		// Token: 0x04002D30 RID: 11568
		[ReadOnly]
		[SerializeField]
		private Animation _animator;

		// Token: 0x04002D31 RID: 11569
		[SerializeField]
		[ReadOnly]
		private AnimationHandler _animationHandler;

		// Token: 0x04002D32 RID: 11570
		[SerializeField]
		[ReadOnly]
		private PetHighlightComponent _highlightComponentComponentValue;

		// Token: 0x04002D33 RID: 11571
		[SerializeField]
		[ReadOnly]
		private bool _isMine;

		// Token: 0x04002D34 RID: 11572
		[NonSerialized]
		public PhotonView photonView;

		// Token: 0x04002D35 RID: 11573
		[NonSerialized]
		public NetworkView _networkView;

		// Token: 0x04002D36 RID: 11574
		private float _dieTime;

		// Token: 0x04002D37 RID: 11575
		[SerializeField]
		[ReadOnly]
		private float _immortalStartTime;

		// Token: 0x04002D38 RID: 11576
		[ReadOnly]
		[SerializeField]
		private PetAnimation _currAnimation;

		// Token: 0x04002D39 RID: 11577
		[SerializeField]
		private bool NOT_SCALE_ANIMS;

		// Token: 0x04002D3A RID: 11578
		[ReadOnly]
		[SerializeField]
		private float _stayTime;

		// Token: 0x04002D3B RID: 11579
		protected Vector3? _prevPosition;

		// Token: 0x04002D3C RID: 11580
		[ReadOnly]
		[SerializeField]
		private float _acceleration;

		// Token: 0x04002D3D RID: 11581
		[ReadOnly]
		[SerializeField]
		private float _speed;

		// Token: 0x04002D3E RID: 11582
		private float speedMonTime;

		// Token: 0x04002D3F RID: 11583
		private float speedTempAcc;

		// Token: 0x04002D40 RID: 11584
		private bool _petBuffPrefabLoading;

		// Token: 0x04002D41 RID: 11585
		private bool _deathEffectIsPlaying;

		// Token: 0x04002D42 RID: 11586
		private AudioSource _audioSourceOneValue;

		// Token: 0x04002D43 RID: 11587
		private AudioSource _audioSourceTwoValue;

		// Token: 0x020006DF RID: 1759
		protected class PetIdleState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003D57 RID: 15703 RVA: 0x0013EB58 File Offset: 0x0013CD58
			public PetIdleState(PetEngine context) : base(PetState.idle, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003D58 RID: 15704 RVA: 0x0013EB6C File Offset: 0x0013CD6C
			public override void In(PetState fromState)
			{
				this.ctx.ShowModel();
				this.ctx.PlaySound(null);
			}

			// Token: 0x06003D59 RID: 15705 RVA: 0x0013EB88 File Offset: 0x0013CD88
			public override void Update()
			{
				if (this.ctx.Owner.isKilled || this.ctx.Owner.transform.position.y < -5000f || !this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.MovePosition, this.ctx.Info.MaxToOwnerDistance + 1f))
				{
					base.To(PetState.teleport);
					return;
				}
				if (!this.ctx.IsAlive)
				{
					base.To(PetState.death);
					return;
				}
				if (this.ctx.Target != null && this.ctx.IsVisible(this.ctx.Target.gameObject, 200f))
				{
					if (!this.ctx.InRange(this.ctx.Target.position, this.ctx.ThisTransform.position, this.ctx.Info.AttackStopDistance))
					{
						base.To(PetState.moveToTarget);
						return;
					}
					if (!this.ctx.InAttackState)
					{
						base.To(PetState.attack);
					}
					return;
				}
				else
				{
					if (this.ctx.CanMoveToPlayer && !this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.MovePosition, this.ctx.Info.MinToOwnerDistance + 1f))
					{
						base.To(PetState.moveToOwner);
						return;
					}
					return;
				}
			}

			// Token: 0x04002D47 RID: 11591
			private PetEngine ctx;
		}

		// Token: 0x020006E0 RID: 1760
		protected class PetAttackState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003D5A RID: 15706 RVA: 0x0013ED28 File Offset: 0x0013CF28
			public PetAttackState(PetEngine context) : base(PetState.attack, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003D5B RID: 15707 RVA: 0x0013ED3C File Offset: 0x0013CF3C
			public override void In(PetState fromState)
			{
				base.In(fromState);
				this.ctx.InAttackState = true;
				if (this.ctx.Owner.isKilled)
				{
					base.To(PetState.teleport);
					return;
				}
				if (this.ctx.Target == null)
				{
					base.To(PetState.idle);
					return;
				}
				float sqrMagnitude = (this.ctx.MoveToTargetPosition.Value - this.ctx.ThisTransform.position).sqrMagnitude;
				if (sqrMagnitude > Mathf.Pow(this.ctx.Info.AttackStopDistance, 2f))
				{
					this.ctx.To(PetState.moveToTarget);
					return;
				}
				this._lockedTarget = this.ctx.Target;
				this.ctx.AnimationHandler.SubscribeTo(this.ctx.GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Custom1, true, delegate
				{
					if (this._lockedTarget == this.ctx.Target)
					{
						this.ctx.AttackTarget();
						this.ctx.AnimationHandler.SubscribeTo(this.ctx.GetAnimationName(PetAnimationType.Attack), AnimationHandler.AnimState.Finished, true, delegate
						{
							this.ctx.InAttackState = false;
							base.To(PetState.idle);
						});
					}
					else
					{
						this.ctx.InAttackState = false;
						base.To(PetState.idle);
					}
				});
				this.ctx.PlayAnimation(PetAnimationType.Attack);
			}

			// Token: 0x06003D5C RID: 15708 RVA: 0x0013EE3C File Offset: 0x0013D03C
			public override void Update()
			{
				base.Update();
				this._waitTime += Time.deltaTime;
				if (this._waitTime > 1.2f || this.ctx.Target == null)
				{
					this._waitTime = 0f;
					this.ctx.InAttackState = false;
					base.To(PetState.idle);
					return;
				}
				Vector3 worldPosition = new Vector3(this.ctx.Target.position.x, this.ctx.ThisTransform.position.y, this.ctx.Target.position.z);
				this.ctx.ThisTransform.LookAt(worldPosition);
			}

			// Token: 0x06003D5D RID: 15709 RVA: 0x0013EF08 File Offset: 0x0013D108
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this.ctx.InAttackState = false;
				this._waitTime = 0f;
				this._lockedTarget = null;
			}

			// Token: 0x04002D48 RID: 11592
			private PetEngine ctx;

			// Token: 0x04002D49 RID: 11593
			private float _waitTime;

			// Token: 0x04002D4A RID: 11594
			private Transform _lockedTarget;
		}

		// Token: 0x020006E1 RID: 1761
		protected class PetDeathState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003D60 RID: 15712 RVA: 0x0013EFC0 File Offset: 0x0013D1C0
			public PetDeathState(PetEngine context) : base(PetState.death, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003D61 RID: 15713 RVA: 0x0013EFD4 File Offset: 0x0013D1D4
			public override void In(PetState fromState)
			{
				base.In(fromState);
				this.ctx.AnimationHandler.SubscribeTo(this.ctx.GetAnimationName(PetAnimationType.Dead), AnimationHandler.AnimState.Finished, true, new Action(this.OnAnimationDeathEnded));
				this.ctx.PlayAnimation(PetAnimationType.Dead);
				this.ctx.PlaySound(this.ctx.ClipDeath);
				this.ctx.Owner.PetKilled();
			}

			// Token: 0x06003D62 RID: 15714 RVA: 0x0013F044 File Offset: 0x0013D244
			private void OnAnimationDeathEnded()
			{
				this.ctx.HideModel();
				this.ctx.EffectHide.OnEffectCompleted.AddListener(new UnityAction(this.OnAnimationHideEnded));
				this.ctx.PlayHideEffect();
			}

			// Token: 0x06003D63 RID: 15715 RVA: 0x0013F080 File Offset: 0x0013D280
			private void OnAnimationHideEnded()
			{
				this.ctx.EffectHide.OnEffectCompleted.RemoveListener(new UnityAction(this.OnAnimationHideEnded));
				this.ctx.ThisTransform.position = Vector3.down * 10000f;
				this.ctx.To(PetState.respawn);
			}

			// Token: 0x04002D4B RID: 11595
			private PetEngine ctx;
		}

		// Token: 0x020006E2 RID: 1762
		protected class PetRespawnState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003D64 RID: 15716 RVA: 0x0013F0DC File Offset: 0x0013D2DC
			public PetRespawnState(PetEngine context) : base(PetState.respawn, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003D65 RID: 15717 RVA: 0x0013F0F0 File Offset: 0x0013D2F0
			public override void In(PetState fromState)
			{
				base.In(fromState);
				this.ctx._dieTime = Time.realtimeSinceStartup;
			}

			// Token: 0x06003D66 RID: 15718 RVA: 0x0013F10C File Offset: 0x0013D30C
			public override void Out(PetState toState)
			{
				base.Out(toState);
			}

			// Token: 0x06003D67 RID: 15719 RVA: 0x0013F118 File Offset: 0x0013D318
			public override void Update()
			{
				base.Update();
				this.ctx.ThisTransform.position = Vector3.down * 10000f;
				if (this.ctx.RespawnTimeLeft > 0f)
				{
					return;
				}
				this.ctx._dieTime = -1f;
				this.ctx.CurrentHealth = this.ctx.Info.HP;
				if (Defs.isMulti && this.ctx.IsMine)
				{
					this.ctx.SendSynhCurrentHealth();
				}
				this.ctx.IsImmortal = true;
				base.To(PetState.teleport);
			}

			// Token: 0x04002D4C RID: 11596
			private PetEngine ctx;
		}

		// Token: 0x020006E3 RID: 1763
		protected class PetTeleportState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003D68 RID: 15720 RVA: 0x0013F1C4 File Offset: 0x0013D3C4
			public PetTeleportState(PetEngine context) : base(PetState.teleport, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003D69 RID: 15721 RVA: 0x0013F1D8 File Offset: 0x0013D3D8
			public override void In(PetState fromState)
			{
				base.In(fromState);
				this.ctx.HideModel();
				if (!this.ctx.IsAlive)
				{
					base.To(PetState.respawn);
					return;
				}
				this._teleportRunning = true;
			}

			// Token: 0x06003D6A RID: 15722 RVA: 0x0013F218 File Offset: 0x0013D418
			public override void Update()
			{
				if (this.ctx.CanShowPet() && this.ctx.OwnerIsGrounded)
				{
					this.ctx.ShowModel();
					this.ctx.WarpToOwner();
					this.ctx.PlayShowEffect();
					base.To(PetState.idle);
					this._teleportRunning = false;
				}
				else
				{
					if (!this._effectPlayed)
					{
						this._effectPlayed = true;
						this.ctx.PlayShowEffect();
					}
					this.ctx.ThisTransform.position = Vector3.down * 10000f;
				}
			}

			// Token: 0x06003D6B RID: 15723 RVA: 0x0013F2B8 File Offset: 0x0013D4B8
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this._effectPlayed = false;
			}

			// Token: 0x04002D4D RID: 11597
			private PetEngine ctx;

			// Token: 0x04002D4E RID: 11598
			private bool _teleportRunning;

			// Token: 0x04002D4F RID: 11599
			private bool _effectPlayed;
		}
	}
}
