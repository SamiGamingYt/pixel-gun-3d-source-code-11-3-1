using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000585 RID: 1413
	public class BotAiController : MonoBehaviour
	{
		// Token: 0x06003141 RID: 12609 RVA: 0x001006C4 File Offset: 0x000FE8C4
		private void Start()
		{
			this._photonView = base.GetComponent<PhotonView>();
			this._isMultiplayerCoopMode = (Defs.isCOOP && this._photonView != null);
			this._currentState = BotAiController.AiState.None;
			this._botController = base.GetComponent<BaseBot>();
			this._typeBot = this.GetCurrentTypeBot();
			this._naveMeshAgent = base.GetComponent<NavMeshAgent>();
			this._modelCollider = base.GetComponentInChildren<BoxCollider>();
			this.InitializePatrolModule();
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
			}
			this._timeLastTeleport = this.timeToNextTeleport;
			this.InitTeleportData();
			this._naveMeshAgent.Warp(base.transform.position + this._naveMeshAgent.baseOffset * Vector3.up);
		}

		// Token: 0x06003142 RID: 12610 RVA: 0x00100794 File Offset: 0x000FE994
		private BotAiController.TypeBot GetCurrentTypeBot()
		{
			if (this._botController == null)
			{
				return BotAiController.TypeBot.None;
			}
			if (this._botController as MeleeBot != null || this._botController as MeleeBossBot != null)
			{
				return BotAiController.TypeBot.Melee;
			}
			if (this._botController as MeleeShootBot != null)
			{
				return BotAiController.TypeBot.ShootAndMelee;
			}
			return BotAiController.TypeBot.Shooting;
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x001007FC File Offset: 0x000FE9FC
		private void UpdateCurrentAiState()
		{
			if (this.IsCanMove)
			{
				this._currentState = BotAiController.AiState.Patrol;
			}
			else if (!this._botController.IsDeath && this.currentTarget != null)
			{
				this._currentState = this.CheckActiveAttackState();
			}
			else
			{
				this._currentState = BotAiController.AiState.None;
			}
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x0010085C File Offset: 0x000FEA5C
		private bool CheckApplyMultiplayerLogic()
		{
			if (!this._isMultiplayerCoopMode)
			{
				return false;
			}
			if (ZombiManager.sharedManager == null)
			{
				return true;
			}
			if (!ZombiManager.sharedManager.startGame)
			{
				if (PhotonNetwork.isMasterClient)
				{
					this._botController.DestroyByNetworkType();
				}
				return true;
			}
			return !this._photonView.isMine;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x001008C4 File Offset: 0x000FEAC4
		private void Update()
		{
			if (Defs.isMulti && this._naveMeshAgent.enabled != PhotonNetwork.isMasterClient)
			{
				this._naveMeshAgent.enabled = PhotonNetwork.isMasterClient;
			}
			if (this.CheckApplyMultiplayerLogic())
			{
				return;
			}
			this.UpdateTargetsForBot();
			this.UpdateCurrentAiState();
			if (this._currentState == BotAiController.AiState.Patrol)
			{
				this.UpdatePatrolState();
			}
			else if (this._currentState == BotAiController.AiState.MoveToTarget)
			{
				this.UpdateMoveToTargetState();
			}
			else if (this._currentState == BotAiController.AiState.Damage)
			{
				this.UpdateDamagedTargetState();
			}
			else if (this._currentState == BotAiController.AiState.Waiting)
			{
				this.IsWaitingState = true;
			}
			else if (this._currentState == BotAiController.AiState.Teleportation)
			{
				base.StartCoroutine(this.TeleportFromRandomPoint());
			}
			if (this._botController.IsDeath)
			{
				if (this._botController.IsFalling)
				{
					this._botController.SetPositionForFallState();
				}
				if (!this._isDeaded)
				{
					this._naveMeshAgent.enabled = false;
					this._isDeaded = true;
				}
			}
			for (int i = 0; i < Initializer.singularities.Count; i++)
			{
				SingularityHole singularityHole = Initializer.singularities[i];
				Vector3 vector = singularityHole.transform.position - base.transform.position;
				float force = singularityHole.GetForce(vector.sqrMagnitude);
				if (force != 0f)
				{
					base.transform.position += vector.normalized * force * Time.deltaTime;
				}
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x00100A60 File Offset: 0x000FEC60
		// (set) Token: 0x06003147 RID: 12615 RVA: 0x00100A68 File Offset: 0x000FEC68
		public bool IsCanMove
		{
			get
			{
				return this._isCanMove;
			}
			set
			{
				if (this._isCanMove == value)
				{
					return;
				}
				this._isCanMove = value;
				if (this._isCanMove)
				{
					this._lastTimeMoving = -1f;
					this._botController.PlayAnimZombieWalkByMode();
				}
			}
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x00100AA0 File Offset: 0x000FECA0
		private void InitializePatrolModule()
		{
			this._lastTimeMoving = -1f;
			if (this._isMultiplayerCoopMode && !this._photonView.isMine)
			{
				this._botController.PlayAnimZombieWalkByMode();
				this.IsCanMove = false;
			}
			else
			{
				this.IsCanMove = !this.isStationary;
			}
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x00100AFC File Offset: 0x000FECFC
		private void UpdatePatrolState()
		{
			if (!this.IsCanMove)
			{
				return;
			}
			if (this._lastTimeMoving <= Time.time)
			{
				this.ResetNavigationPathIfNeed();
				Vector3 position = base.transform.position;
				this._targetPoint = new Vector3(position.x + UnityEngine.Random.Range(-this.minLenghtMove, this.minLenghtMove), position.y, position.z + UnityEngine.Random.Range(-this.minLenghtMove, this.minLenghtMove));
				this._lastTimeMoving = Time.time + Vector3.Distance(base.transform.position, this._targetPoint) / this._botController.GetWalkSpeed();
				if (!this._naveMeshAgent.SetDestination(this._targetPoint))
				{
					this._lastTimeMoving = 0f;
					return;
				}
				this._botController.OrientToTarget(this._targetPoint);
				this._naveMeshAgent.speed = this._botController.GetWalkSpeed();
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x00100C04 File Offset: 0x000FEE04
		// (set) Token: 0x0600314A RID: 12618 RVA: 0x00100BF8 File Offset: 0x000FEDF8
		public Transform currentTarget { get; private set; }

		// Token: 0x0600314C RID: 12620 RVA: 0x00100C0C File Offset: 0x000FEE0C
		private float GetTimeToTakeDamageMeleeBot()
		{
			if (this._typeBot != BotAiController.TypeBot.Melee)
			{
				return 0f;
			}
			MeleeBot meleeBot = this._botController as MeleeBot;
			return meleeBot.CheckTimeToTakeDamage();
		}

		// Token: 0x0600314D RID: 12621 RVA: 0x00100C3C File Offset: 0x000FEE3C
		private void CheckTargetAvailabelForShot()
		{
			this._timeToCheckAvailabelShot -= Time.deltaTime;
			if (this._timeToCheckAvailabelShot > 0f)
			{
				return;
			}
			this._timeToCheckAvailabelShot = 1f;
			this._isTargetAvalabelShot = this.IsTargetAvailabelForShot();
		}

		// Token: 0x0600314E RID: 12622 RVA: 0x00100C84 File Offset: 0x000FEE84
		private string GetTargetTagAndPointToShot(out Vector3 pointToShot)
		{
			BotAiController.TargetType targetType = this.GetTargetType(this.currentTarget);
			if (targetType == BotAiController.TargetType.Player)
			{
				SkinName component = this.currentTarget.GetComponent<SkinName>();
				if (component == null)
				{
					pointToShot = Vector3.zero;
					return null;
				}
				Transform transform = component.headObj.transform;
				pointToShot = transform.position;
				return "Player";
			}
			else
			{
				if (targetType == BotAiController.TargetType.Turret)
				{
					pointToShot = this.currentTarget.GetComponent<TurretController>().GetHeadPoint();
					return "Turret";
				}
				if (targetType == BotAiController.TargetType.Bot)
				{
					pointToShot = this.currentTarget.GetComponent<BaseBot>().GetHeadPoint();
					return "Enemy";
				}
				if (targetType == BotAiController.TargetType.Pet)
				{
					pointToShot = this.currentTarget.position;
					return "Pet";
				}
				pointToShot = Vector3.zero;
				return null;
			}
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x00100D58 File Offset: 0x000FEF58
		private bool IsTargetAvailabelForShot()
		{
			Vector3 headPoint = this._botController.GetHeadPoint();
			Vector3 a;
			string targetTagAndPointToShot = this.GetTargetTagAndPointToShot(out a);
			float maxAttackDistance = this._botController.GetMaxAttackDistance();
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(headPoint, a - headPoint, out raycastHit, maxAttackDistance, Tools.AllAvailabelBotRaycastMask);
			if (flag)
			{
				Transform transform = raycastHit.collider.transform;
				return transform.root.CompareTag(targetTagAndPointToShot);
			}
			return false;
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x00100DC4 File Offset: 0x000FEFC4
		private void UpdateMoveToTargetState()
		{
			this.IsWaitingState = false;
			if (this._botController.isFlyingSpeedLimit && this._naveMeshAgent.isOnOffMeshLink)
			{
				this._naveMeshAgent.speed = this._botController.maxFlyingSpeed;
			}
			else
			{
				this._naveMeshAgent.speed = this._botController.GetAttackSpeedByCompleteLevel();
			}
			this._naveMeshAgent.SetDestination(this.currentTarget.position);
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
			}
			this._botController.PlayAnimZombieWalkByMode();
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x00100E64 File Offset: 0x000FF064
		private BotAiController.AiState CheckActiveAttackState()
		{
			if (this._botController.IsDeath || this.currentTarget == null)
			{
				return BotAiController.AiState.None;
			}
			if (this.CheckMoveFromTeleport())
			{
				return BotAiController.AiState.Teleportation;
			}
			float distanceToEnemy = Vector3.SqrMagnitude(this.currentTarget.position - (base.transform.position + Vector3.up));
			if (!this._botController.CheckEnemyInAttackZone(distanceToEnemy))
			{
				return (!this.isStationary) ? BotAiController.AiState.MoveToTarget : BotAiController.AiState.Waiting;
			}
			if (this._typeBot != BotAiController.TypeBot.Shooting && this._typeBot != BotAiController.TypeBot.ShootAndMelee)
			{
				return BotAiController.AiState.Damage;
			}
			this.CheckTargetAvailabelForShot();
			BotAiController.AiState result = (!this.isStationary) ? BotAiController.AiState.MoveToTarget : BotAiController.AiState.Waiting;
			if (this._isTargetAvalabelShot)
			{
				return BotAiController.AiState.Damage;
			}
			return result;
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x00100F34 File Offset: 0x000FF134
		private void InitTeleportData()
		{
			if (!this.isTeleportationMove)
			{
				return;
			}
			this._effectObject = UnityEngine.Object.Instantiate<GameObject>(this.effectTeleport);
			this._effectObject.transform.parent = base.transform;
			this._effectObject.transform.localPosition = Vector3.zero;
			this._effectObject.transform.rotation = Quaternion.identity;
			this._effectObject.SetActive(false);
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x00100FAC File Offset: 0x000FF1AC
		private IEnumerator ShowEffectTeleport(float seconds)
		{
			this._effectObject.SetActive(true);
			yield return new WaitForSeconds(seconds);
			this._effectObject.SetActive(false);
			yield break;
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x00100FD8 File Offset: 0x000FF1D8
		private IEnumerator TeleportFromRandomPoint()
		{
			bool isWarpComplete = false;
			Vector3 positionFromTeleport = Vector3.zero;
			this.isStationary = true;
			base.StartCoroutine(this.ShowEffectTeleport(0.2f));
			this._botController.TryPlayAudioClip(this.teleportStart);
			yield return new WaitForSeconds(0.2f);
			for (int i = 0; i < 5; i++)
			{
				positionFromTeleport = this.GetPositionFromTeleport();
				isWarpComplete = this._naveMeshAgent.Warp(positionFromTeleport);
				if (isWarpComplete)
				{
					break;
				}
			}
			if (!isWarpComplete)
			{
				this._naveMeshAgent.Warp(Vector3.zero);
			}
			this._botController.DelayShootAfterEvent(4f);
			base.StartCoroutine(this.ShowEffectTeleport(0.2f));
			this._botController.TryPlayAudioClip(this.teleportEnd);
			yield return new WaitForSeconds(0.2f);
			this.isStationary = false;
			yield break;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x00100FF4 File Offset: 0x000FF1F4
		private Vector3 GetPositionFromTeleport()
		{
			Vector3 result = Vector3.zero;
			float min = this._botController.attackDistance + this.DeltaTeleportAttackDistance[0];
			float max = this._botController.attackDistance + this.DeltaTeleportAttackDistance[1];
			float d = UnityEngine.Random.Range(min, max);
			float value = UnityEngine.Random.value;
			if (value >= 0f && value < 0.4f)
			{
				Quaternion rotation = Quaternion.Euler(0f, this.angleByPlayerLook, 0f);
				result = this.currentTarget.position + rotation * (this.currentTarget.forward * d);
			}
			else if (value >= 0.4f && value < 0.5f)
			{
				Quaternion rotation2 = Quaternion.Euler(0f, this.angleByPlayerLook, 0f);
				Vector3 forward = this.currentTarget.forward;
				forward.z = -forward.z;
				result = this.currentTarget.position + rotation2 * (forward * d);
			}
			else if (value >= 0.5f && value < 0.6f)
			{
				Vector3 forward2 = this.currentTarget.forward;
				forward2.z = -forward2.z;
				Quaternion rotation3 = Quaternion.Euler(0f, -this.angleByPlayerLook, 0f);
				result = this.currentTarget.position + rotation3 * (forward2 * d);
			}
			else
			{
				Quaternion rotation4 = Quaternion.Euler(0f, -this.angleByPlayerLook, 0f);
				result = this.currentTarget.position + rotation4 * (this.currentTarget.forward * d);
			}
			return result;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x001011C0 File Offset: 0x000FF3C0
		private bool CheckMoveFromTeleport()
		{
			if (!this.isTeleportationMove)
			{
				return false;
			}
			if (this._timeLastTeleport > 0f)
			{
				this._timeLastTeleport -= Time.deltaTime;
				return false;
			}
			this._timeLastTeleport = this.timeToNextTeleport;
			return true;
		}

		// Token: 0x06003157 RID: 12631 RVA: 0x0010120C File Offset: 0x000FF40C
		public void SetTargetToMove(Transform target)
		{
			if (!this.isDetectPlayer)
			{
				return;
			}
			if (target != null && this.currentTarget != target)
			{
				this.ResetNavigationPathIfNeed();
				this._botController.PlayAnimZombieWalkByMode();
			}
			else if (target == null && this.currentTarget != target)
			{
				this.ResetNavigationPathIfNeed();
				this._botController.PlayAnimationWalk();
			}
			this.currentTarget = target;
			this.IsCanMove = (target == null && !this.isStationary);
		}

		// Token: 0x06003158 RID: 12632 RVA: 0x001012AC File Offset: 0x000FF4AC
		private void ResetNavigationPathIfNeed()
		{
			if (this._naveMeshAgent.path == null)
			{
				return;
			}
			if (this._naveMeshAgent.isOnOffMeshLink)
			{
				return;
			}
			this._naveMeshAgent.ResetPath();
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x001012DC File Offset: 0x000FF4DC
		private void UpdateDamagedTargetState()
		{
			this.IsWaitingState = false;
			this.ResetNavigationPathIfNeed();
			Vector3 position = this.currentTarget.position;
			position.y = base.transform.position.y;
			this._botController.OrientToTarget(position);
			this._botController.PlayAnimZombieAttackOrStopByMode();
			if (this._typeBot == BotAiController.TypeBot.Melee)
			{
				this._timeToTakeDamage -= Time.deltaTime;
				if (this._timeToTakeDamage <= 0f)
				{
					this._botController.MakeDamage(this.currentTarget);
					this._timeToTakeDamage = this.GetTimeToTakeDamageMeleeBot();
				}
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x0600315A RID: 12634 RVA: 0x00101380 File Offset: 0x000FF580
		// (set) Token: 0x0600315B RID: 12635 RVA: 0x00101388 File Offset: 0x000FF588
		private bool IsWaitingState
		{
			get
			{
				return this._isWaiting;
			}
			set
			{
				if (this._isWaiting == value)
				{
					return;
				}
				this._isWaiting = value;
				if (this._isWaiting)
				{
					this._botController.PlayAnimationIdle();
				}
			}
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x001013C0 File Offset: 0x000FF5C0
		private BotAiController.TargetType GetTargetType(Transform target)
		{
			if (target.CompareTag("Player"))
			{
				return BotAiController.TargetType.Player;
			}
			if (target.CompareTag("Turret"))
			{
				return BotAiController.TargetType.Turret;
			}
			if (target.CompareTag("Enemy"))
			{
				return BotAiController.TargetType.Bot;
			}
			if (target.CompareTag("Pet"))
			{
				return BotAiController.TargetType.Pet;
			}
			return BotAiController.TargetType.None;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x00101418 File Offset: 0x000FF618
		private void UpdateTargetsForBot()
		{
			if (!this.isDetectPlayer)
			{
				return;
			}
			if (this._isMultiplayerCoopMode)
			{
				this._timeToUpdateMultiplayerTargets -= Time.deltaTime;
				if (this._timeToUpdateMultiplayerTargets <= 0f)
				{
					this._timeToUpdateMultiplayerTargets = 3f;
					this.CheckTargetForMultiplayerMode();
				}
			}
			else
			{
				this._timeToUpdateLocalTargets -= Time.deltaTime;
				if (this._timeToUpdateLocalTargets <= 0f)
				{
					this.CheckTargetForLocalMode();
				}
			}
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x0010149C File Offset: 0x000FF69C
		private bool CheckForcedTarget()
		{
			if (!this._isTargetCaptureForce)
			{
				return false;
			}
			if (this.IsCurrentTargetLost())
			{
				this.SetTargetToMove(null);
				this._isTargetCaptureForce = false;
			}
			return true;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x001014C8 File Offset: 0x000FF6C8
		private void CheckTargetForMultiplayerMode()
		{
			if (this.CheckForcedTarget())
			{
				return;
			}
			bool flag;
			GameObject nearestTargetForMultiplayer = this.GetNearestTargetForMultiplayer(out flag);
			if (nearestTargetForMultiplayer == null)
			{
				this.SetTargetToMove(null);
				return;
			}
			float num;
			if (flag)
			{
				num = this.GetDistanceToPlayer(nearestTargetForMultiplayer);
			}
			else
			{
				num = this.GetDistanceToTurret(nearestTargetForMultiplayer);
			}
			if (num != -1f && this._botController.detectRadius >= num && !this.IsTargetLost(nearestTargetForMultiplayer.transform))
			{
				this.SetTargetToMove(nearestTargetForMultiplayer.transform);
			}
			else
			{
				this.SetTargetToMove(null);
			}
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x00101564 File Offset: 0x000FF764
		private void CheckTargetForLocalMode()
		{
			if (this.CheckForcedTarget())
			{
				return;
			}
			if (!this._isEntered)
			{
				GameObject gameObject = (Initializer.turretsObj != null && Initializer.turretsObj.Count != 0) ? Initializer.turretsObj[0] : null;
				float distanceToTurret = this.GetDistanceToTurret(gameObject);
				bool flag = distanceToTurret != -1f && this._botController.detectRadius >= distanceToTurret;
				GameObject gameObject2 = GameObject.FindGameObjectWithTag("Player");
				float distanceToPlayer = this.GetDistanceToPlayer(gameObject2);
				bool flag2 = distanceToPlayer != -1f && this._botController.detectRadius >= distanceToPlayer;
				GameObject gameObject3 = GameObject.FindGameObjectWithTag("Pet");
				float distanceToTurret2 = this.GetDistanceToTurret(gameObject3);
				bool flag3 = distanceToTurret2 != -1f && this._botController.detectRadius >= distanceToTurret2;
				Transform transform = null;
				if (flag2 && flag)
				{
					if (distanceToPlayer < distanceToTurret)
					{
						transform = gameObject2.transform;
					}
					else
					{
						transform = gameObject.transform;
					}
				}
				else if (flag2)
				{
					transform = gameObject2.transform;
				}
				else if (flag)
				{
					transform = gameObject.transform;
				}
				else if (flag3)
				{
					transform = gameObject3.transform;
				}
				if (transform != null)
				{
					this.SetTargetToMove(transform);
					this._isEntered = true;
				}
			}
			else if (this.IsCurrentTargetLost())
			{
				this.SetTargetToMove(null);
				this._isEntered = false;
			}
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x001016F8 File Offset: 0x000FF8F8
		private float GetDistanceToPlayer(GameObject playerObj)
		{
			if (playerObj == null)
			{
				return -1f;
			}
			if (this.IsTargetNotAvailabel(playerObj.transform, BotAiController.TargetType.Player))
			{
				return -1f;
			}
			return Vector3.Distance(base.transform.position, playerObj.transform.position);
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x0010174C File Offset: 0x000FF94C
		private float GetDistanceToTurret(GameObject turretObj)
		{
			if (turretObj == null)
			{
				return -1f;
			}
			if (this.IsTargetNotAvailabel(turretObj.transform, BotAiController.TargetType.Turret) && this.IsTargetNotAvailabel(turretObj.transform, BotAiController.TargetType.Pet))
			{
				return -1f;
			}
			return Vector3.Distance(base.transform.position, turretObj.transform.position);
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x001017B0 File Offset: 0x000FF9B0
		private GameObject GetNearestTargetForMultiplayer(out bool isTargetPlayer)
		{
			isTargetPlayer = false;
			GameObject result = null;
			if (Initializer.players.Count > 0)
			{
				float num = float.MaxValue;
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (!this.IsTargetNotAvailabel(Initializer.players[i]))
					{
						float num2 = Vector3.SqrMagnitude(base.transform.position - Initializer.players[i].myPlayerTransform.position);
						if (num2 < num)
						{
							num = num2;
							result = Initializer.players[i].mySkinName.gameObject;
							isTargetPlayer = true;
						}
					}
				}
				for (int j = 0; j < Initializer.turretsObj.Count; j++)
				{
					if (!this.IsTargetNotAvailabel(Initializer.turretsObj[j].transform, BotAiController.TargetType.Turret))
					{
						float num3 = Vector3.SqrMagnitude(base.transform.position - Initializer.turretsObj[j].transform.position);
						if (num3 < num)
						{
							num = num3;
							result = Initializer.turretsObj[j];
							isTargetPlayer = false;
						}
					}
				}
				for (int k = 0; k < Initializer.petsObj.Count; k++)
				{
					if (!this.IsTargetNotAvailabel(Initializer.petsObj[k].transform, BotAiController.TargetType.Pet))
					{
						float num4 = Vector3.SqrMagnitude(base.transform.position - Initializer.petsObj[k].transform.position);
						if (num4 < num)
						{
							num = num4;
							result = Initializer.petsObj[k];
							isTargetPlayer = false;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06003164 RID: 12644 RVA: 0x00101970 File Offset: 0x000FFB70
		private bool IsCurrentTargetLost()
		{
			return this.IsTargetLost(this.currentTarget);
		}

		// Token: 0x06003165 RID: 12645 RVA: 0x00101980 File Offset: 0x000FFB80
		private bool IsTargetLost(Transform target)
		{
			if (target == null)
			{
				return true;
			}
			if (this._isTargetCaptureForce && target.gameObject == null)
			{
				return true;
			}
			BotAiController.TargetType targetType = this.GetTargetType(target);
			if (targetType == BotAiController.TargetType.Player)
			{
				if (this.IsTargetNotAvailabel(target, BotAiController.TargetType.Player))
				{
					return true;
				}
				if (!this._isTargetCaptureForce && Vector3.SqrMagnitude(base.transform.position - target.transform.position) > this._botController.GetSquareDetectRadius())
				{
					return true;
				}
			}
			return (targetType == BotAiController.TargetType.Turret && this.IsTargetNotAvailabel(target, BotAiController.TargetType.Turret)) || (targetType == BotAiController.TargetType.Pet && this.IsTargetNotAvailabel(target, BotAiController.TargetType.Pet));
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x00101A40 File Offset: 0x000FFC40
		private bool IsTargetNotAvailabel(Transform target, BotAiController.TargetType targetType)
		{
			if (targetType == BotAiController.TargetType.Player)
			{
				SkinName component = target.GetComponent<SkinName>();
				if (component != null && component.playerMoveC.isInvisible)
				{
					return true;
				}
			}
			if (targetType == BotAiController.TargetType.Turret)
			{
				TurretController component2 = target.GetComponent<TurretController>();
				if (component2 == null || component2.isKilled || !component2.isRun)
				{
					return true;
				}
			}
			if (targetType == BotAiController.TargetType.Pet)
			{
				PetEngine component3 = target.GetComponent<PetEngine>();
				if (component3 == null || !component3.IsAlive)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x00101AD4 File Offset: 0x000FFCD4
		private bool IsTargetNotAvailabel(Player_move_c target)
		{
			return target != null && target.isInvisible;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x00101AF0 File Offset: 0x000FFCF0
		public void SetTargetForced(Transform target)
		{
			this.SetTargetToMove(target);
			this._isTargetCaptureForce = true;
		}

		// Token: 0x04002436 RID: 9270
		private const int MaxAttempTeleportation = 5;

		// Token: 0x04002437 RID: 9271
		private const float TimeDelayedTeleport = 0.2f;

		// Token: 0x04002438 RID: 9272
		private const float TimeOutUpdateMultiplayerTargets = 3f;

		// Token: 0x04002439 RID: 9273
		private const float TimeOutUpdateLocalTargets = 1f;

		// Token: 0x0400243A RID: 9274
		private BaseBot _botController;

		// Token: 0x0400243B RID: 9275
		private BotAiController.TypeBot _typeBot;

		// Token: 0x0400243C RID: 9276
		private BotAiController.AiState _currentState;

		// Token: 0x0400243D RID: 9277
		private bool _isMultiplayerCoopMode;

		// Token: 0x0400243E RID: 9278
		private PhotonView _photonView;

		// Token: 0x0400243F RID: 9279
		private bool _isDeaded;

		// Token: 0x04002440 RID: 9280
		[Header("Patrol module settings")]
		public float minLenghtMove = 9f;

		// Token: 0x04002441 RID: 9281
		private bool _isCanMove;

		// Token: 0x04002442 RID: 9282
		private float _lastTimeMoving;

		// Token: 0x04002443 RID: 9283
		private Vector3 _targetPoint;

		// Token: 0x04002444 RID: 9284
		private NavMeshAgent _naveMeshAgent;

		// Token: 0x04002445 RID: 9285
		[Header("Movement module settings")]
		public bool isStationary;

		// Token: 0x04002446 RID: 9286
		public bool isTeleportationMove;

		// Token: 0x04002447 RID: 9287
		public static bool deathAudioPlaying;

		// Token: 0x04002448 RID: 9288
		private float _timeToTakeDamage;

		// Token: 0x04002449 RID: 9289
		private bool _isFalling;

		// Token: 0x0400244A RID: 9290
		private BoxCollider _modelCollider;

		// Token: 0x0400244B RID: 9291
		private float _timeToCheckAvailabelShot;

		// Token: 0x0400244C RID: 9292
		private bool _isTargetAvalabelShot;

		// Token: 0x0400244D RID: 9293
		[Header("Teleport movement setting")]
		public float timeToNextTeleport = 2f;

		// Token: 0x0400244E RID: 9294
		public float[] DeltaTeleportAttackDistance = new float[]
		{
			1f,
			2f
		};

		// Token: 0x0400244F RID: 9295
		public GameObject effectTeleport;

		// Token: 0x04002450 RID: 9296
		public float angleByPlayerLook = 30f;

		// Token: 0x04002451 RID: 9297
		public AudioClip teleportStart;

		// Token: 0x04002452 RID: 9298
		public AudioClip teleportEnd;

		// Token: 0x04002453 RID: 9299
		private float _timeLastTeleport;

		// Token: 0x04002454 RID: 9300
		private GameObject _effectObject;

		// Token: 0x04002455 RID: 9301
		private bool _isWaiting;

		// Token: 0x04002456 RID: 9302
		[NonSerialized]
		public bool isDetectPlayer = true;

		// Token: 0x04002457 RID: 9303
		private bool _isEntered;

		// Token: 0x04002458 RID: 9304
		private float _timeToUpdateMultiplayerTargets = 3f;

		// Token: 0x04002459 RID: 9305
		private float _timeToUpdateLocalTargets = 1f;

		// Token: 0x0400245A RID: 9306
		private bool _isTargetCaptureForce;

		// Token: 0x02000586 RID: 1414
		private enum AiState
		{
			// Token: 0x0400245D RID: 9309
			Patrol,
			// Token: 0x0400245E RID: 9310
			MoveToTarget,
			// Token: 0x0400245F RID: 9311
			Damage,
			// Token: 0x04002460 RID: 9312
			Waiting,
			// Token: 0x04002461 RID: 9313
			Teleportation,
			// Token: 0x04002462 RID: 9314
			None
		}

		// Token: 0x02000587 RID: 1415
		private enum TypeBot
		{
			// Token: 0x04002464 RID: 9316
			Melee,
			// Token: 0x04002465 RID: 9317
			Shooting,
			// Token: 0x04002466 RID: 9318
			ShootAndMelee,
			// Token: 0x04002467 RID: 9319
			None
		}

		// Token: 0x02000588 RID: 1416
		private enum TargetType
		{
			// Token: 0x04002469 RID: 9321
			Player,
			// Token: 0x0400246A RID: 9322
			Turret,
			// Token: 0x0400246B RID: 9323
			Bot,
			// Token: 0x0400246C RID: 9324
			None,
			// Token: 0x0400246D RID: 9325
			Pet
		}
	}
}
