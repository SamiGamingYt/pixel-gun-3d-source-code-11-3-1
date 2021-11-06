using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Rilisoft;
using RilisoftBot;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public sealed class Rocket : MonoBehaviour
{
	// Token: 0x06002BF1 RID: 11249 RVA: 0x000E7320 File Offset: 0x000E5520
	public Rocket()
	{
		this.lightningCoefs = new float[]
		{
			1f,
			0.5f,
			0.25f,
			0.125f,
			0.0625f
		};
		base..ctor();
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x000E7394 File Offset: 0x000E5594
	private bool IsHitInDamageRadius(Vector3 targetPos, Vector3 selfPos, float radius)
	{
		return (targetPos - selfPos).sqrMagnitude < radius * radius;
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x000E73B8 File Offset: 0x000E55B8
	private float GetCoefDamageAtPoint(Vector3 pos)
	{
		float num = Vector3.SqrMagnitude(this.thisTransform.position - pos);
		return 1f - 0.3f * num / (this.radiusDamage * this.radiusDamage);
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x000E73F8 File Offset: 0x000E55F8
	public IEnumerator Hit(Collider hitCollider)
	{
		if (Defs.isMulti && (WeaponManager.sharedManager.myPlayer == null || !this.isMine))
		{
			yield break;
		}
		while (this.currentRocketSettings == null)
		{
			yield return null;
		}
		GameObject hitObject = null;
		Transform hitObjectRootTransform = null;
		if (hitCollider != null)
		{
			hitObject = hitCollider.gameObject;
			hitObjectRootTransform = hitCollider.gameObject.transform.root;
			GameObject hitObjectRoot = hitObjectRootTransform.gameObject;
		}
		if (this.currentRocketSettings.typeDead == WeaponSounds.TypeDead.like)
		{
			this.LikeHit(hitObject.transform);
			yield break;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.BlackMark)
		{
			this.BlackMarkHit(hitObject.transform);
			yield break;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
		{
			this.SendShowExplosion();
		}
		Vector3 point = this.thisTransform.position;
		Initializer.TargetsList targets = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, Defs.isMulti, true);
		foreach (Transform target in targets)
		{
			bool _isHit = false;
			bool _isHeadShot = false;
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
			{
				_isHit = target.Equals(this.targetDamageLightning);
			}
			else if (this.IsDamageByRadius())
			{
				if (this.IsHitInDamageRadius(target.position, point, this.radiusDamage))
				{
					_isHit = true;
				}
			}
			else if (target.Equals(hitObjectRootTransform))
			{
				_isHit = true;
				if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.ToxicBomb)
				{
					_isHeadShot = (hitObject.name == "HeadCollider" || hitCollider is SphereCollider);
				}
			}
			if (_isHit)
			{
				this.HitIDestructible(target, _isHeadShot);
			}
		}
		yield break;
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x000E7424 File Offset: 0x000E5624
	private void HitIDestructible(Transform _obj, bool _isHeadShot)
	{
		IDamageable component = _obj.GetComponent<IDamageable>();
		if (component == null)
		{
			return;
		}
		if (Defs.isDaterRegim)
		{
			if (this.currentWeaponSounds.isDaterWeapon && component is PlayerDamageable)
			{
				Player_move_c myPlayer = (component as PlayerDamageable).myPlayer;
				if (!myPlayer.Equals(WeaponManager.sharedManager.myPlayerMoveC) && !myPlayer.isMechActive)
				{
					myPlayer.SendDaterChat(WeaponManager.sharedManager.myPlayerMoveC.mySkinName.NickName, WeaponManager.sharedManager.currentWeaponSounds.daterMessage, myPlayer.mySkinName.NickName);
				}
			}
		}
		else
		{
			float num = 1f;
			if (_isHeadShot)
			{
				num = 2f + EffectsController.AddingForHeadshot(this.currentWeaponSounds.categoryNabor - 1);
			}
			if (this.IsDamageByRadius())
			{
				num *= this.GetCoefDamageAtPoint(_obj.position);
			}
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
			{
				num *= this.lightningCoefs[this.lightningHitCount];
				this.lightningHitCount++;
			}
			if (_obj.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
			{
				num *= EffectsController.SelfExplosionDamageDecreaseCoef;
			}
			num *= WeaponManager.sharedManager.myPlayerMoveC.DamageMultiplierByGadgets();
			float num2 = this.multiplayerDamage;
			num2 *= num;
			WeaponSounds.TypeDead typeDead = this.currentRocketSettings.typeDead;
			Player_move_c.TypeKills typeKill = (!_isHeadShot) ? this.currentRocketSettings.typeKilsIconChat : Player_move_c.TypeKills.headshot;
			string weaponName = this.weaponPrefabName;
			if (this.IsGrenadeWeaponName(this.weaponPrefabName))
			{
				weaponName = GadgetsInfo.BaseName(this.weaponPrefabName);
			}
			component.ApplyDamage(num2, this.myPlayerMoveC.myDamageable, typeKill, typeDead, weaponName, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
			if (this.currentWeaponSounds.isPoisoning)
			{
				WeaponManager.sharedManager.myPlayerMoveC.PoisonShotWithEffect(_obj.gameObject, new Player_move_c.PoisonParameters(this.multiplayerDamage, this.currentWeaponSounds));
			}
			if (this.currentWeaponSounds.isDamageHeal && component.isLivingTarget)
			{
				WeaponManager.sharedManager.myPlayerMoveC.AddHealth(this.multiplayerDamage * this.currentWeaponSounds.damageHealMultiplier);
			}
			if (this.isSlowdown)
			{
				WeaponManager.sharedManager.myPlayerMoveC.SlowdownTarget(component, this.slowdownTime, this.slowdownCoeff);
			}
		}
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x000E768C File Offset: 0x000E588C
	private void LikeHit(Transform go)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC == null)
		{
			return;
		}
		Player_move_c player_move_c = null;
		if (go != null && go.CompareTag("Player"))
		{
			player_move_c = go.GetComponent<SkinName>().playerMoveC;
		}
		else
		{
			float num = float.MaxValue;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				Player_move_c player_move_c2 = Initializer.players[i];
				if (!player_move_c2.Equals(WeaponManager.sharedManager.myPlayerMoveC))
				{
					float num2 = Vector3.SqrMagnitude(Initializer.players[i].myPlayerTransform.position - this.thisTransform.position);
					if (num2 < this.radiusDamage * this.radiusDamage && num2 < num)
					{
						num = num2;
						player_move_c = player_move_c2;
					}
				}
			}
		}
		if (player_move_c != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SendLike(player_move_c);
		}
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x000E778C File Offset: 0x000E598C
	private void BlackMarkHit(Transform go)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC == null)
		{
			return;
		}
		Player_move_c player_move_c = null;
		if (go != null && go.CompareTag("Player") && Initializer.IsEnemyTarget(go, null))
		{
			player_move_c = go.GetComponent<SkinName>().playerMoveC;
		}
		else
		{
			float num = float.MaxValue;
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (Initializer.IsEnemyTarget(Initializer.players[i].myPlayerTransform, null))
				{
					Player_move_c player_move_c2 = Initializer.players[i];
					if (!player_move_c2.Equals(WeaponManager.sharedManager.myPlayerMoveC))
					{
						float num2 = Vector3.SqrMagnitude(Initializer.players[i].myPlayerTransform.position - this.thisTransform.position);
						if (num2 < this.radiusDamage * this.radiusDamage && num2 < num)
						{
							num = num2;
							player_move_c = player_move_c2;
						}
					}
				}
			}
		}
		if (player_move_c != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.BlackMarkPlayer(player_move_c, GadgetsInfo.info[this.weaponPrefabName]);
		}
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x000E78C0 File Offset: 0x000E5AC0
	private void Update()
	{
		if (this.myPlayerMoveC == null)
		{
			this.SetMyPlayerMoveC();
		}
		if (Defs.isMulti && !this.isMine && this.isRun)
		{
			this.InterpolatePos();
			if (this.currentRocketSettings != null)
			{
				RocketSettings.TypeFlyRocket typeFly = this.currentRocketSettings.typeFly;
				if (typeFly == RocketSettings.TypeFlyRocket.Drone)
				{
					this.UpdateTargetDroneForOthers();
				}
			}
			return;
		}
		if (this.currentRocketSettings == null)
		{
			return;
		}
		if (!Defs.isMulti || this.isMine)
		{
			RocketSettings.TypeFlyRocket typeFly = this.currentRocketSettings.typeFly;
			switch (typeFly)
			{
			case RocketSettings.TypeFlyRocket.Autoaim:
			case RocketSettings.TypeFlyRocket.AutoaimBullet:
				this.UpdateForceForAutoaim();
				return;
			default:
				switch (typeFly)
				{
				case RocketSettings.TypeFlyRocket.StickyMine:
					goto IL_125;
				case RocketSettings.TypeFlyRocket.Molotov:
				case RocketSettings.TypeFlyRocket.BlackMark:
				case RocketSettings.TypeFlyRocket.Firework:
					return;
				case RocketSettings.TypeFlyRocket.Drone:
					this.UpdateTargetDrone();
					return;
				case RocketSettings.TypeFlyRocket.FakeBonus:
					this.UpdateFakeBonus();
					return;
				case RocketSettings.TypeFlyRocket.HomingGrenade:
					break;
				default:
					return;
				}
				break;
			case RocketSettings.TypeFlyRocket.Lightning:
				this.UpdateTargetForLightning();
				return;
			case RocketSettings.TypeFlyRocket.AutoTarget:
				break;
			case RocketSettings.TypeFlyRocket.StickyBomb:
				goto IL_125;
			case RocketSettings.TypeFlyRocket.ToxicBomb:
				this.UpdateTargetToxicBomb();
				return;
			}
			this.UpdateForceForAutoHoming();
			return;
			IL_125:
			this.UpdateTargetStickedBomb();
		}
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x000E7A20 File Offset: 0x000E5C20
	private void UpdateFakeBonus()
	{
		if (!this.isRun)
		{
			return;
		}
		if (this.isActivated)
		{
			Initializer.TargetsList targetsList = new Initializer.TargetsList();
			foreach (Transform transform in targetsList)
			{
				if ((transform.position - this.thisTransform.position).sqrMagnitude < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget)
				{
					this.KillRocket();
				}
			}
		}
		else
		{
			base.transform.rotation = Quaternion.identity;
			this.myRigidbody.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x000E7AFC File Offset: 0x000E5CFC
	private void UpdateForceForAutoaim()
	{
		if (this.isRun && WeaponManager.sharedManager.myPlayerMoveC != null && !WeaponManager.sharedManager.myPlayerMoveC.isKilled)
		{
			Vector3 pointAutoAim = WeaponManager.sharedManager.myPlayerMoveC.GetPointAutoAim(this.thisTransform.position);
			Vector3 normalized = (pointAutoAim - this.thisTransform.position).normalized;
			this.myRigidbody.AddForce(normalized * 27f);
			this.myRigidbody.velocity = this.myRigidbody.velocity.normalized * this.currentRocketSettings.autoRocketForce;
			this.thisTransform.rotation = Quaternion.LookRotation(this.myRigidbody.velocity);
		}
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x000E7BD4 File Offset: 0x000E5DD4
	private void UpdateForceForAutoHoming()
	{
		if (this.isRun)
		{
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.HomingGrenade)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 2f);
				this.myRigidbody.velocity = Vector3.Lerp(this.myRigidbody.velocity, new Vector3(this.myRigidbody.velocity.x, 0f, this.myRigidbody.velocity.z), Time.deltaTime * 2f);
			}
			if (this.targetAutoHoming == null || this.IsKilledTarget(this.targetAutoHoming) || (this.targetAutoHoming.position - this.thisTransform.position).sqrMagnitude > (this.currentRocketSettings.raduisDetectTarget + 1f) * (this.currentRocketSettings.raduisDetectTarget + 1f))
			{
				this.targetAutoHoming = this.FindNearestTarget(45f);
			}
			if (this.targetAutoHoming != null)
			{
				Vector3 b = Vector3.zero;
				if (this.targetAutoHoming.childCount > 0 && this.targetAutoHoming.GetChild(0).GetComponent<BoxCollider>() != null)
				{
					b = this.targetAutoHoming.GetChild(0).GetComponent<BoxCollider>().center;
				}
				Vector3 normalized = (this.targetAutoHoming.position + b - this.thisTransform.position).normalized;
				this.myRigidbody.AddForce(normalized * 9f);
				this.myRigidbody.velocity = this.myRigidbody.velocity.normalized * this.currentRocketSettings.autoRocketForce;
				this.thisTransform.rotation = Quaternion.LookRotation(this.myRigidbody.velocity);
			}
		}
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x000E7E0C File Offset: 0x000E600C
	private void UpdateTargetForLightning()
	{
		if (this.targetDamageLightning != null)
		{
			this.myRigidbody.isKinematic = true;
			this.targetLightning = this.FindLightningTarget();
			if (this.targetLightning == null)
			{
				this.thisTransform.position = this.targetDamageLightning.position;
				this.timerFromJumpLightning -= Time.deltaTime;
				if (this.timerFromJumpLightning <= 0f)
				{
					this.counterJumpLightning++;
					if (this.counterJumpLightning > this.currentRocketSettings.countJumpLightning)
					{
						this.KillRocket();
					}
					else
					{
						base.StartCoroutine(this.Hit(null));
					}
					this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
				}
			}
			else
			{
				this.targetDamageLightning = null;
				this.progressCaptureTargetLightning = 0f;
			}
		}
		if (this.targetLightning != null)
		{
			if (!this.IsKilledTarget(this.targetLightning))
			{
				this.thisTransform.position = Vector3.Lerp(this.thisTransform.position, this.targetLightning.position, this.progressCaptureTargetLightning + 5f * Time.deltaTime);
			}
			else
			{
				this.KillRocket();
			}
		}
		else if (this.isDetectFirstTargetLightning && (this.targetDamageLightning == null || this.IsKilledTarget(this.targetDamageLightning)))
		{
			this.KillRocket();
		}
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x000E7F88 File Offset: 0x000E6188
	private void UpdateTargetStickedBomb()
	{
		if (!this.isRun || !this.myRigidbody.isKinematic)
		{
			return;
		}
		if (this.stickedObject != null && this.stickedObjectPos != this.stickedObject.position)
		{
			this.KillRocket();
			return;
		}
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if ((transform.position - this.thisTransform.position).sqrMagnitude < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget)
			{
				this.KillRocket();
			}
		}
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x000E8074 File Offset: 0x000E6274
	private void UpdateTargetDrone()
	{
		if (!this.isRun || !this.isActivated)
		{
			return;
		}
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 2f);
		this.myRigidbody.angularVelocity = Vector3.MoveTowards(this.myRigidbody.angularVelocity, Vector3.zero, Time.time * 10f);
		Vector3 normalized = (this.dronePoint - base.transform.position).normalized;
		float sqrMagnitude = (this.dronePoint - base.transform.position).sqrMagnitude;
		this.myRigidbody.AddForce(normalized * Mathf.Min(8f, sqrMagnitude));
		this.myRigidbody.velocity = this.myRigidbody.velocity.normalized * Mathf.Clamp(sqrMagnitude, 0f, this.currentRocketSettings.autoRocketForce);
		if (this.lastToxicHit > Time.time)
		{
			return;
		}
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if ((transform.position - this.thisTransform.position).sqrMagnitude < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				this.lastToxicHit = Time.time + this.currentRocketSettings.toxicHitTime;
				WeaponManager.sharedManager.myPlayerMoveC.DamageTarget(transform.gameObject, this.multiplayerDamage, this.weaponPrefabName, this.currentRocketSettings.typeDead, this.currentRocketSettings.typeKilsIconChat);
			}
		}
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x000E82B0 File Offset: 0x000E64B0
	private void UpdateTargetDroneForOthers()
	{
		if (!this.isRun || !this.isActivated)
		{
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC == null || this.myPlayerMoveC == null)
		{
			return;
		}
		if (Defs.isCOOP || (ConnectSceneNGUIController.isTeamRegim && this.myPlayerMoveC.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand))
		{
			return;
		}
		if ((WeaponManager.sharedManager.myPlayerMoveC.transform.position - this.thisTransform.position).sqrMagnitude < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget)
		{
			this.lastToxicHit = Time.time + this.currentRocketSettings.toxicHitTime;
			WeaponManager.sharedManager.myPlayerMoveC.SlowdownRPC(0.5f, this.currentRocketSettings.toxicHitTime);
		}
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x000E83B0 File Offset: 0x000E65B0
	private void UpdateTargetToxicBomb()
	{
		if (!this.isRun || !this.myRigidbody.isKinematic)
		{
			return;
		}
		if (this.stickedObject != null && this.stickedObjectPos != this.stickedObject.position)
		{
			this.KillRocket();
			return;
		}
		if (this.lastToxicHit > Time.time)
		{
			return;
		}
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if ((transform.position - this.thisTransform.position).sqrMagnitude < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget && WeaponManager.sharedManager.myPlayerMoveC != null)
			{
				this.lastToxicHit = Time.time + this.currentRocketSettings.toxicHitTime;
				WeaponManager.sharedManager.myPlayerMoveC.DamageTarget(transform.gameObject, this.multiplayerDamage * this.currentRocketSettings.toxicDamageMultiplier, this.weaponPrefabName, this.currentRocketSettings.typeDead, Player_move_c.TypeKills.poison);
			}
		}
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x000E8510 File Offset: 0x000E6710
	private bool IsKilledTarget(Transform _target)
	{
		if (_target == null)
		{
			return true;
		}
		if (_target.GetComponent<SkinName>() != null)
		{
			return _target.GetComponent<SkinName>().playerMoveC.isKilled;
		}
		if (_target.GetComponent<TurretController>() != null)
		{
			return _target.GetComponent<TurretController>().isKilled;
		}
		if (_target.GetComponent<BaseBot>() != null)
		{
			return _target.GetComponent<BaseBot>().IsDeath;
		}
		return !(_target.GetComponent<BaseDummy>() != null) || _target.GetComponent<BaseDummy>().isDead;
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x000E85A8 File Offset: 0x000E67A8
	private Transform FindLightningTarget()
	{
		Transform result = null;
		float num = float.MaxValue;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if (!transform.Equals(this.targetDamageLightning))
			{
				float num2 = Vector3.SqrMagnitude(this.thisTransform.position - transform.position);
				RaycastHit raycastHit;
				if (num2 < this.currentRocketSettings.raduisDetectTargetLightning * this.currentRocketSettings.raduisDetectTargetLightning && num2 < num && Physics.Raycast(this.thisTransform.position, transform.position - this.thisTransform.position, out raycastHit, this.currentRocketSettings.raduisDetectTargetLightning, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null && raycastHit.collider.gameObject.transform.root.Equals(transform))
				{
					result = transform;
					num = num2;
				}
			}
		}
		return result;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x000E86E4 File Offset: 0x000E68E4
	private Transform FindNearestTarget(float searchAngle)
	{
		Transform result = null;
		float num = float.MaxValue;
		Initializer.TargetsList targetsList = new Initializer.TargetsList();
		foreach (Transform transform in targetsList)
		{
			if (!transform.Equals(this.targetAutoHoming))
			{
				Vector3 a = transform.position - this.thisTransform.position;
				if (Vector3.Angle(this.myRigidbody.velocity.normalized, a.normalized) <= searchAngle)
				{
					float num2 = Vector3.SqrMagnitude(a);
					RaycastHit raycastHit;
					if (num2 < this.currentRocketSettings.raduisDetectTarget * this.currentRocketSettings.raduisDetectTarget && num2 < num && Physics.Raycast(this.thisTransform.position, transform.position - this.thisTransform.position, out raycastHit, this.currentRocketSettings.raduisDetectTarget, Player_move_c._ShootRaycastLayerMask) && raycastHit.collider.gameObject != null && raycastHit.collider.gameObject.transform.root.Equals(transform))
					{
						result = transform;
						num = num2;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x000E884C File Offset: 0x000E6A4C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.isRun)
			{
				stream.SendNext(this.thisTransform.position);
				stream.SendNext(this.thisTransform.rotation);
			}
		}
		else
		{
			this.correctPos = (Vector3)stream.ReceiveNext();
			this.thisTransform.rotation = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x000E88C8 File Offset: 0x000E6AC8
	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.isRun)
			{
				Vector3 position = this.thisTransform.position;
				Quaternion rotation = this.thisTransform.rotation;
				stream.Serialize(ref position);
				stream.Serialize(ref rotation);
			}
		}
		else
		{
			Vector3 zero = Vector3.zero;
			Quaternion identity = Quaternion.identity;
			stream.Serialize(ref zero);
			stream.Serialize(ref identity);
			this.correctPos = zero;
			this.thisTransform.rotation = identity;
			if (this.isFirstPos)
			{
				this.isFirstPos = false;
				this.thisTransform.position = zero;
				this.thisTransform.rotation = identity;
			}
		}
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x000E8974 File Offset: 0x000E6B74
	private void InterpolatePos()
	{
		if (Vector3.SqrMagnitude(this.thisTransform.position - this.correctPos) > 3000f)
		{
			this.thisTransform.position = this.correctPos;
		}
		else
		{
			this.thisTransform.position = Vector3.Lerp(this.thisTransform.position, this.correctPos, Time.deltaTime * 5f);
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x06002C07 RID: 11271 RVA: 0x000E89E8 File Offset: 0x000E6BE8
	// (set) Token: 0x06002C08 RID: 11272 RVA: 0x000E89F0 File Offset: 0x000E6BF0
	public int rocketNum
	{
		get
		{
			return this._rocketNum;
		}
		set
		{
			this._rocketNum = value;
			base.StartCoroutine(this.SetCurrentRocket(this._rocketNum));
		}
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x000E8A0C File Offset: 0x000E6C0C
	private bool IsGrenadeWeaponName(string _weaponName)
	{
		return !_weaponName.Contains("Weapon");
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x000E8A1C File Offset: 0x000E6C1C
	private bool IsGravityRocket(RocketSettings _rs)
	{
		return _rs.typeFly == RocketSettings.TypeFlyRocket.ToxicBomb || _rs.typeFly == RocketSettings.TypeFlyRocket.StickyBomb || _rs.typeFly == RocketSettings.TypeFlyRocket.StickyMine || _rs.typeFly == RocketSettings.TypeFlyRocket.Firework || _rs.typeFly == RocketSettings.TypeFlyRocket.FakeBonus || _rs.typeFly == RocketSettings.TypeFlyRocket.Bomb || _rs.typeFly == RocketSettings.TypeFlyRocket.BlackMark || _rs.typeFly == RocketSettings.TypeFlyRocket.GravityRocket;
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x000E8A90 File Offset: 0x000E6C90
	private bool IsDamageByRadius()
	{
		return this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Grenade || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.GrenadeBouncing || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Bomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Rocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.ChargeRocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.GravityRocket || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.BlackMark || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Autoaim || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ball || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.AutoTarget || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyMine || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.NuclearGrenade || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.HomingGrenade;
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x000E8BB8 File Offset: 0x000E6DB8
	private void Awake()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.myRigidbody = base.GetComponent<Rigidbody>();
		this.thisTransform = base.transform;
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				this.isMine = base.GetComponent<NetworkView>().isMine;
			}
			else
			{
				this.isMine = this.photonView.isMine;
				if (this.isMine)
				{
					PhotonObjectCacher.AddObject(base.gameObject);
				}
			}
		}
		else
		{
			this.isMine = true;
		}
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x000E8C48 File Offset: 0x000E6E48
	private void Start()
	{
		this.SetMyPlayerMoveC();
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x000E8C50 File Offset: 0x000E6E50
	private void SetMyPlayerMoveC()
	{
		if (Defs.isMulti && !this.isMine)
		{
			this.myRigidbody.isKinematic = true;
			base.GetComponent<BoxCollider>().enabled = false;
			if (Defs.isInet)
			{
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (Initializer.players[i].mySkinName.photonView != null && this.photonView.ownerId == Initializer.players[i].mySkinName.photonView.ownerId)
					{
						this.myPlayerMoveC = Initializer.players[i];
						break;
					}
				}
			}
		}
		else
		{
			this.myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		}
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x000E8D24 File Offset: 0x000E6F24
	public void SendNetworkViewMyPlayer(NetworkViewID myId)
	{
		base.GetComponent<NetworkView>().RPC("SendNetworkViewMyPlayerRPC", RPCMode.AllBuffered, new object[]
		{
			myId
		});
	}

	// Token: 0x06002C10 RID: 11280 RVA: 0x000E8D54 File Offset: 0x000E6F54
	[RPC]
	[PunRPC]
	public void SendNetworkViewMyPlayerRPC(NetworkViewID myId)
	{
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (myId.Equals(Initializer.players[i].mySkinName.GetComponent<NetworkView>().viewID))
			{
				this.myPlayerMoveC = Initializer.players[i];
				break;
			}
		}
	}

	// Token: 0x06002C11 RID: 11281 RVA: 0x000E8DC0 File Offset: 0x000E6FC0
	public void SendSetRocketActive(string _weaponName, float _radiusImpulse, float _chargePower)
	{
		this.weaponPrefabName = _weaponName;
		this.radiusImpulse = _radiusImpulse;
		this.chargePower = _chargePower;
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				if (this.chargePower == 1f)
				{
					base.GetComponent<NetworkView>().RPC("SetRocketActive", RPCMode.All, new object[]
					{
						this.weaponPrefabName,
						this.radiusImpulse,
						base.transform.position
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("SetRocketActiveWithCharge", RPCMode.All, new object[]
					{
						this.weaponPrefabName,
						this.radiusImpulse,
						base.transform.position,
						this.chargePower
					});
				}
			}
			else if (this.chargePower == 1f)
			{
				this.photonView.RPC("SetRocketActive", PhotonTargets.All, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position
				});
			}
			else
			{
				this.photonView.RPC("SetRocketActiveWithCharge", PhotonTargets.All, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position,
					this.chargePower
				});
			}
		}
		else if (!Defs.isMulti)
		{
			this.SetRocketActiveWithCharge(this.weaponPrefabName, this.radiusImpulse, base.transform.position, this.chargePower);
		}
	}

	// Token: 0x06002C12 RID: 11282 RVA: 0x000E8F84 File Offset: 0x000E7184
	[PunRPC]
	[RPC]
	public void SetRocketActive(string weapon, float _radiusImpulse, Vector3 pos)
	{
		this.SetRocketActiveWithCharge(weapon, _radiusImpulse, pos, 1f);
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x000E8F94 File Offset: 0x000E7194
	[RPC]
	[PunRPC]
	public void SetRocketActiveWithCharge(string _weaponName, float _radiusImpulse, Vector3 pos, float _chargePower)
	{
		if (Defs.IsDeveloperBuild && _weaponName == "WeaponGrenade")
		{
			_weaponName = "gadget_fraggrenade";
		}
		if (Defs.IsDeveloperBuild && _weaponName == "WeaponLike")
		{
			_weaponName = "Like";
		}
		bool flag = this.IsGrenadeWeaponName(_weaponName);
		string str;
		if (flag)
		{
			str = "GadgetsContent";
		}
		else
		{
			str = "Weapons";
		}
		if (Application.isEditor)
		{
			Debug.Log(_weaponName);
		}
		this.currentWeaponSounds = (Resources.Load(str + "/" + _weaponName) as GameObject).GetComponent<WeaponSounds>();
		this.explosionName = this.currentWeaponSounds.bazookaExplosionName;
		this.impulseForce = this.currentWeaponSounds.impulseForce;
		this.rocketNum = this.currentWeaponSounds.rocketNum;
		this.myRigidbody.isKinematic = this.IsGrenadeWeaponName(_weaponName);
		if (this.isMine)
		{
			float num = 0f;
			if (!flag)
			{
				num = ((!(this.myPlayerMoveC != null)) ? 0f : this.myPlayerMoveC.koofDamageWeaponFromPotoins);
				int num2 = (!(ExpController.Instance != null)) ? (ExpController.LevelsForTiers.Length - 1) : ExpController.Instance.OurTier;
				int num3 = (!(this.myPlayerMoveC != null)) ? num2 : this.myPlayerMoveC.TierOrRoomTier(num2);
				this.damage = (float)this.currentWeaponSounds.survivalDamage * (1f + num + EffectsController.DamageModifsByCats(this.currentWeaponSounds.categoryNabor - 1));
				this.multiplayerDamage = ((!(ExpController.Instance != null) || ExpController.Instance.OurTier >= this.currentWeaponSounds.DamageByTier.Length) ? ((this.currentWeaponSounds.DamageByTier.Length <= 0) ? 0f : this.currentWeaponSounds.DamageByTier[0]) : this.currentWeaponSounds.DamageByTier[num3]);
			}
			this.damageRange = this.currentWeaponSounds.damageRange * (1f + num + EffectsController.DamageModifsByCats(this.currentWeaponSounds.categoryNabor - 1));
			this.radiusDamage = this.currentWeaponSounds.bazookaExplosionRadius;
			this.radiusDamageSelf = this.currentWeaponSounds.bazookaExplosionRadiusSelf;
			this.isSlowdown = this.currentWeaponSounds.isSlowdown;
			this.slowdownCoeff = this.currentWeaponSounds.slowdownCoeff;
			this.slowdownTime = this.currentWeaponSounds.slowdownTime;
			this.impulseForce = this.currentWeaponSounds.impulseForce;
			this.impulseForceSelf = this.currentWeaponSounds.impulseForceSelf;
			if (this.currentWeaponSounds.isCharging)
			{
				this.damage *= this.chargePower;
				this.multiplayerDamage *= this.chargePower;
				this.damageRange *= this.chargePower;
			}
			this.myRigidbody.useGravity = this.currentWeaponSounds.grenadeLauncher;
		}
		else
		{
			base.transform.position = pos;
			this.chargePower = _chargePower;
			this.weaponPrefabName = _weaponName;
			this.radiusImpulse = _radiusImpulse;
		}
		base.StartCoroutine(this.StartRocketCoroutine());
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x000E92E0 File Offset: 0x000E74E0
	private IEnumerator StartRocketCoroutine()
	{
		while (this.currentRocketSettings == null)
		{
			yield return null;
		}
		this.isKilled = false;
		if (this.isMine)
		{
			this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
			this.counterJumpLightning = 0;
			this.lightningHitCount = 0;
			this.isDetectFirstTargetLightning = false;
			this.targetDamageLightning = null;
			this.targetAutoHoming = null;
			this.targetLightning = null;
			this.megabulletLastTarget = null;
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus)
			{
				this.currentRocketSettings.GetComponent<BoxCollider>().enabled = true;
			}
			base.GetComponent<BoxCollider>().size = this.currentRocketSettings.sizeBoxCollider;
			base.GetComponent<BoxCollider>().center = this.currentRocketSettings.centerBoxCollider;
			if (this.IsGravityRocket(this.currentRocketSettings))
			{
				this.myRigidbody.useGravity = true;
			}
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.ChargeRocket)
		{
			this.currentRocketSettings.transform.localScale = Vector3.one * Mathf.Lerp(this.currentRocketSettings.chargeScaleMin, this.currentRocketSettings.chargeScaleMax, this.chargePower);
			base.GetComponent<BoxCollider>().size *= Mathf.Lerp(this.currentRocketSettings.chargeScaleMin, this.currentRocketSettings.chargeScaleMax, this.chargePower);
		}
		if (!this.IsGrenadeWeaponName(this.weaponPrefabName))
		{
			this.StartRocketRPC();
		}
		else
		{
			if (!Defs.isMulti || this.isMine)
			{
				Player_move_c.SetLayerRecursively(base.gameObject, 9);
			}
			while (this.myPlayerMoveC == null || this.myPlayerMoveC.myPlayerTransform.childCount == 0 || this.myPlayerMoveC.myCurrentWeaponSounds == null || this.myPlayerMoveC.myCurrentWeaponSounds.grenatePoint == null)
			{
				yield return null;
			}
			this.thisTransform.SetParent(this.myPlayerMoveC.myCurrentWeaponSounds.grenatePoint);
			this.thisTransform.localPosition = Vector3.zero;
			this.thisTransform.localRotation = Quaternion.identity;
			if (this.currentRocketSettings != null)
			{
				this.currentRocketSettings.transform.localPosition = Vector3.zero;
			}
		}
		yield break;
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x000E92FC File Offset: 0x000E74FC
	[PunRPC]
	[RPC]
	public void StartRocketRPC()
	{
		if (this.IsGrenadeWeaponName(this.weaponPrefabName) && this.myPlayerMoveC != null && this.myPlayerMoveC.myCurrentWeaponSounds != null && this.myPlayerMoveC.myCurrentWeaponSounds.fakeGrenade != null && base.transform.parent != null)
		{
			base.transform.parent.gameObject.SetActive(true);
			this.myPlayerMoveC.myCurrentWeaponSounds.fakeGrenade.SetActive(false);
		}
		base.transform.parent = null;
		this.isRun = true;
		base.StartCoroutine(this.StartRocketRPCCoroutine());
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x000E93C0 File Offset: 0x000E75C0
	private IEnumerator StartRocketRPCCoroutine()
	{
		while (this.currentRocketSettings == null)
		{
			yield return null;
		}
		if (this.currentRocketSettings.trail != null)
		{
			this.currentRocketSettings.trail.enabled = true;
		}
		if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Firework && this.currentRocketSettings.flyParticle != null)
		{
			this.currentRocketSettings.flyParticle.SetActive(true);
		}
		if (!Defs.isMulti || this.isMine)
		{
			Tools.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Rocket"));
			float _lifeTime = (!Defs.isDaterRegim || (this.rocketNum != 36 && this.rocketNum != 18)) ? this.currentRocketSettings.lifeTime : 1f;
			base.Invoke("KillRocket", _lifeTime);
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework)
			{
				base.Invoke("FlyFirework", Mathf.Max(0f, _lifeTime - 2f));
			}
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Drone)
		{
			base.Invoke("ActivateDrone", 1f);
			if (this.currentRocketSettings.GetComponent<Animation>() != null)
			{
				this.currentRocketSettings.GetComponent<Animation>().Play("throw");
			}
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus)
		{
			base.Invoke("SetFakeBonusRun", 2.5f);
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.HomingGrenade)
		{
			if (this.currentRocketSettings.GetComponent<Animation>() != null)
			{
				this.currentRocketSettings.GetComponent<Animation>().Play("Fly");
			}
			this.myRigidbody.useGravity = false;
		}
		yield break;
	}

	// Token: 0x06002C17 RID: 11287 RVA: 0x000E93DC File Offset: 0x000E75DC
	private void ActivateDrone()
	{
		if (this.currentRocketSettings != null && this.currentRocketSettings.droneRotator != null)
		{
			this.currentRocketSettings.droneRotator.enabled = true;
		}
		if (this.currentRocketSettings != null && this.currentRocketSettings.droneParticle != null)
		{
			this.currentRocketSettings.droneParticle.SetActive(true);
		}
		this.isActivated = true;
		if (!Defs.isMulti || this.isMine)
		{
			this.myRigidbody.useGravity = false;
			this.myRigidbody.velocity += Vector3.up;
			this.dronePoint = base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, 10000f, Player_move_c._ShootRaycastLayerMask) && raycastHit.distance < 4.5f)
			{
				this.dronePoint = raycastHit.point + Vector3.up * 2.5f;
			}
		}
	}

	// Token: 0x06002C18 RID: 11288 RVA: 0x000E9508 File Offset: 0x000E7708
	public IEnumerator SetCurrentRocket(int _rocketNum)
	{
		if (this.rocketsDict.ContainsKey(_rocketNum.ToString()))
		{
			this.currentRocketSettings = this.rocketsDict[_rocketNum.ToString()];
			this.currentRocketSettings.gameObject.SetActive(true);
		}
		else
		{
			ResourceRequest request = Resources.LoadAsync<GameObject>("Rockets/Rocket_" + _rocketNum.ToString());
			yield return request;
			if (request.isDone)
			{
				if (this.rocketsDict.ContainsKey(_rocketNum.ToString()))
				{
					this.currentRocketSettings = this.rocketsDict[_rocketNum.ToString()];
					this.currentRocketSettings.gameObject.SetActive(true);
				}
				else
				{
					GameObject _currentRocketPrefab = request.asset as GameObject;
					GameObject _currentRocket = UnityEngine.Object.Instantiate<GameObject>(_currentRocketPrefab);
					_currentRocket.transform.SetParent(base.transform);
					_currentRocket.transform.localPosition = Vector3.zero;
					_currentRocket.transform.localRotation = Quaternion.Euler(Vector3.zero);
					_currentRocket.transform.localScale = new Vector3(1f, 1f, 1f);
					this.currentRocketSettings = _currentRocket.GetComponent<RocketSettings>();
					this.rocketsDict.Add(_rocketNum.ToString(), this.currentRocketSettings);
					if (!this.isMine && _currentRocket.GetComponent<BoxCollider>() != null)
					{
						_currentRocket.GetComponent<BoxCollider>().enabled = false;
					}
				}
			}
		}
		yield break;
	}

	// Token: 0x06002C19 RID: 11289 RVA: 0x000E9534 File Offset: 0x000E7734
	public void RunGrenade()
	{
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("StartRocketRPC", RPCMode.All, new object[0]);
			}
			else
			{
				this.photonView.RPC("StartRocketRPC", PhotonTargets.All, new object[0]);
			}
		}
		else if (!Defs.isMulti)
		{
			this.StartRocketRPC();
		}
		this.myRigidbody.isKinematic = false;
	}

	// Token: 0x06002C1A RID: 11290 RVA: 0x000E95B4 File Offset: 0x000E77B4
	private void FlyFirework()
	{
		if (this.currentRocketSettings.flyParticle != null)
		{
			this.currentRocketSettings.flyParticle.SetActive(true);
		}
		this.myRigidbody.isKinematic = false;
		this.myRigidbody.AddForce(Vector3.up * 200f);
	}

	// Token: 0x06002C1B RID: 11291 RVA: 0x000E9610 File Offset: 0x000E7810
	public void SendSetRocketSticked()
	{
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("SetRocketStickedRPC", RPCMode.Others, new object[]
				{
					this.thisTransform.position
				});
			}
			else
			{
				this.photonView.RPC("SetRocketStickedRPC", PhotonTargets.Others, new object[]
				{
					this.thisTransform.position
				});
			}
		}
	}

	// Token: 0x06002C1C RID: 11292 RVA: 0x000E9698 File Offset: 0x000E7898
	[RPC]
	[PunRPC]
	public void SetRocketStickedRPC(Vector3 position)
	{
		base.transform.position = position;
		this.SetRocketSticked();
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x000E96AC File Offset: 0x000E78AC
	public void SetRocketStickedToPlayer(Player_move_c player)
	{
		base.CancelInvoke("KillRocket");
		base.CancelInvoke("FlyFirework");
		base.Invoke("KillRocket", 2f);
		if (this.currentRocketSettings.flyParticle != null)
		{
			this.currentRocketSettings.flyParticle.SetActive(true);
		}
		this.myRigidbody.isKinematic = true;
		this.isStickedToPlayer = true;
		base.transform.parent = player.myPlayerTransform;
		this.SetRocketSticked();
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("SetRocketStickedToPlayerRPC", RPCMode.Others, new object[]
				{
					player.skinNamePixelView.viewID,
					this.thisTransform.localPosition
				});
			}
			else
			{
				this.photonView.RPC("SetRocketStickedToPlayerRPC", PhotonTargets.Others, new object[]
				{
					player.skinNamePixelView.viewID,
					this.thisTransform.localPosition
				});
			}
		}
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x000E97D0 File Offset: 0x000E79D0
	[PunRPC]
	[RPC]
	public void SetRocketStickedToPlayerRPC(int pixelID, Vector3 relativePosition)
	{
		Player_move_c player_move_c = null;
		this.isRun = false;
		base.GetComponent<BoxCollider>().enabled = false;
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (Initializer.players[i].skinNamePixelView.viewID == pixelID)
			{
				player_move_c = Initializer.players[i];
				break;
			}
		}
		if (player_move_c == null)
		{
			return;
		}
		if (player_move_c.Equals(WeaponManager.sharedManager.myPlayerMoveC))
		{
			WeaponManager.sharedManager.myPlayerMoveC.PlayerEffectRPC(12, 1f);
		}
		base.transform.parent = player_move_c.myPlayerTransform;
		base.transform.localPosition = relativePosition;
		this.SetRocketSticked();
	}

	// Token: 0x06002C1F RID: 11295 RVA: 0x000E9894 File Offset: 0x000E7A94
	private void SetRocketSticked()
	{
		if (this.currentRocketSettings == null)
		{
			return;
		}
		if (this.currentRocketSettings.flyParticle != null)
		{
			this.currentRocketSettings.flyParticle.SetActive(false);
		}
		if (this.currentRocketSettings.stickedParticle != null)
		{
			this.currentRocketSettings.stickedParticle.SetActive(true);
		}
		if (this.currentRocketSettings.GetComponent<Animation>() != null)
		{
			this.currentRocketSettings.GetComponent<Animation>().Play("stick");
		}
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x000E9930 File Offset: 0x000E7B30
	private void OnCollisionEnter(Collision other)
	{
		if (this.IsSkipCollision(other.gameObject))
		{
			return;
		}
		if ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyMine || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.ToxicBomb) && !this.IsDamageableObject(other.transform.root.gameObject))
		{
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework && Vector3.Angle(other.contacts[0].normal, Vector3.up) > 30f)
			{
				return;
			}
			if (!this.isActivated)
			{
				this.isActivated = true;
				base.transform.position = other.contacts[0].point + other.contacts[0].normal * 0.035f;
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyMine)
				{
					base.transform.up = other.contacts[0].normal;
				}
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework)
				{
					base.transform.up = Vector3.back;
				}
				this.myRigidbody.isKinematic = true;
				this.stickedObject = other.transform;
				this.stickedObjectPos = this.stickedObject.position;
				this.SetRocketSticked();
				this.SendSetRocketSticked();
			}
			return;
		}
		else
		{
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus)
			{
				return;
			}
			this.CollisionWithCollide(other.collider);
			return;
		}
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x000E9AE0 File Offset: 0x000E7CE0
	private void SetFakeBonusRun()
	{
		if (!this.isRun)
		{
			return;
		}
		if (!Defs.isMulti || this.isMine)
		{
			this.myRigidbody.isKinematic = true;
			if (this.currentRocketSettings != null)
			{
				this.currentRocketSettings.GetComponent<BoxCollider>().enabled = false;
			}
		}
		this.isActivated = true;
		if (this.currentRocketSettings != null && this.currentRocketSettings.droneRotator != null)
		{
			this.currentRocketSettings.droneRotator.enabled = true;
		}
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x000E9B7C File Offset: 0x000E7D7C
	public void OnMegabulletTriggerEnter(Collider other)
	{
		if (other.gameObject.transform.root.Equals(this.megabulletLastTarget))
		{
			return;
		}
		if (this.IsSkipCollision(other.gameObject))
		{
			return;
		}
		if ((other.gameObject.transform.parent != null && other.gameObject.transform.parent.gameObject.CompareTag("Untagged")) || (other.gameObject.transform.parent == null && other.gameObject.CompareTag("Untagged")))
		{
			return;
		}
		if (this.IsDamageableObject(other.gameObject.transform.root.gameObject))
		{
			this.megabulletLastTarget = other.gameObject.transform.root;
			base.StartCoroutine(this.Hit(other));
		}
	}

	// Token: 0x06002C23 RID: 11299 RVA: 0x000E9C70 File Offset: 0x000E7E70
	private void OnTriggerEnter(Collider other)
	{
		if (this.IsSkipCollision(other.gameObject))
		{
			return;
		}
		if ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyMine || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.ToxicBomb) && !this.IsDamageableObject(other.gameObject.transform.root.gameObject))
		{
			return;
		}
		if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework && !this.isStickedToPlayer)
		{
			IDamageable component = other.transform.root.gameObject.GetComponent<IDamageable>();
			if (component != null && component.IsEnemyTo(WeaponManager.sharedManager.myPlayerMoveC) && component is PlayerDamageable)
			{
				this.SetRocketStickedToPlayer((component as PlayerDamageable).myPlayer);
			}
		}
		if (!this.isStickedToPlayer)
		{
			this.CollisionWithCollide(other);
		}
	}

	// Token: 0x06002C24 RID: 11300 RVA: 0x000E9D88 File Offset: 0x000E7F88
	private bool IsDamageableObject(GameObject go)
	{
		return go.GetComponent<IDamageable>() != null;
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x000E9D98 File Offset: 0x000E7F98
	private bool IsSkipCollision(GameObject go)
	{
		if (!this.isRun)
		{
			return true;
		}
		if (Defs.isMulti && !this.isMine)
		{
			return true;
		}
		if (go.name.Equals("DamageCollider") || go.name.Equals("StopCollider"))
		{
			return true;
		}
		if (go.CompareTag("Area") || go.CompareTag("CapturePoint"))
		{
			return true;
		}
		Transform root = go.transform.root;
		return (WeaponManager.sharedManager.myPlayer != null && (root.Equals(WeaponManager.sharedManager.myPlayer.transform) || (WeaponManager.sharedManager.myPlayerMoveC.myPetEngine != null && root.Equals(WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.ThisTransform)))) || (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Drone || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.GrenadeBouncing || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.SingularityGrenade || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.NuclearGrenade || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.SlowdownGrenade) || ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Bomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Ball) && !this.IsDamageableObject(root.gameObject));
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x000E9F20 File Offset: 0x000E8120
	private void CollisionWithCollide(Collider _collide)
	{
		GameObject gameObject = _collide.gameObject;
		if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Ghost && ((this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.MegaBullet && this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Lightning) || gameObject.transform.root.CompareTag("Untagged")))
		{
			if ((this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyBomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.StickyMine || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.ToxicBomb || this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus) && this.myRigidbody.isKinematic)
			{
				return;
			}
			base.StartCoroutine(this.KillRocket(_collide));
		}
		else if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Lightning)
		{
			if (Initializer.IsEnemyTarget(gameObject.transform.root, null))
			{
				this.targetDamageLightning = gameObject.transform.root;
				this.timerFromJumpLightning = this.maxTimerFromJumpLightning;
				this.counterJumpLightning++;
				if (gameObject.CompareTag("Turret") && gameObject.GetComponent<TurretController>().turretType == TurretController.TurretType.ShieldWall)
				{
					this.counterJumpLightning = this.currentRocketSettings.countJumpLightning + 1;
				}
				if (this.counterJumpLightning > this.currentRocketSettings.countJumpLightning)
				{
					this.KillRocket();
				}
				else
				{
					base.StartCoroutine(this.Hit(null));
				}
			}
		}
		else if (this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.MegaBullet)
		{
			base.StartCoroutine(this.Hit(_collide));
		}
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x000EA0CC File Offset: 0x000E82CC
	[Obfuscation(Exclude = true)]
	public void KillRocket()
	{
		base.StartCoroutine(this.KillRocket(null));
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x000EA0DC File Offset: 0x000E82DC
	public IEnumerator KillRocket(Collider _hitCollision)
	{
		if (this.isKilled)
		{
			yield break;
		}
		this.isKilled = true;
		if (this.OnExplode != null)
		{
			this.OnExplode();
			this.OnExplode = null;
		}
		yield return base.StartCoroutine(this.Hit(_hitCollision));
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("Collide", RPCMode.Others, new object[]
				{
					this.explosionName,
					this.thisTransform.position
				});
			}
			else
			{
				this.photonView.RPC("Collide", PhotonTargets.Others, new object[]
				{
					this.explosionName,
					this.thisTransform.position
				});
			}
		}
		this.Collide(this.explosionName, this.thisTransform.position);
		yield break;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000EA108 File Offset: 0x000E8308
	[RPC]
	[PunRPC]
	private void Collide(string _explosionName, Vector3 _pos)
	{
		base.transform.parent = null;
		this.thisTransform.position = _pos;
		if (Defs.inComingMessagesCounter <= 5 && this.currentRocketSettings != null && this.currentRocketSettings.typeFly != RocketSettings.TypeFlyRocket.Lightning)
		{
			this.BazookaExplosion(_explosionName);
		}
		this.DestroyRocket();
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000EA168 File Offset: 0x000E8368
	public void BazookaExplosion(string explosionName)
	{
		this.ShowExplosion(explosionName);
		GameObject myPlayer = WeaponManager.sharedManager.myPlayer;
		if (myPlayer == null || (WeaponManager.sharedManager.myPlayerMoveC.isImmortality && !this.isMine))
		{
			return;
		}
		Vector3 position = this.thisTransform.position;
		Vector3 dir = myPlayer.transform.position - position;
		float sqrMagnitude = dir.sqrMagnitude;
		float num = this.radiusImpulse * this.radiusImpulse;
		if (sqrMagnitude < num)
		{
			ImpactReceiver impactReceiver = myPlayer.GetComponent<ImpactReceiver>();
			if (impactReceiver == null)
			{
				impactReceiver = myPlayer.AddComponent<ImpactReceiver>();
			}
			float num2 = 100f;
			if (this.radiusImpulse != 0f)
			{
				num2 = Mathf.Sqrt(sqrMagnitude / num);
			}
			float num3 = Mathf.Max(0f, 1f - num2);
			float num4 = (Defs.isMulti && !this.isMine) ? this.impulseForce : this.impulseForceSelf;
			num4 *= num3;
			impactReceiver.AddImpact(dir, num4);
			if ((!Defs.isMulti || this.isMine) && num3 > 0.01f)
			{
				WeaponManager.sharedManager.myPlayerMoveC.isRocketJump = true;
			}
		}
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000EA2B0 File Offset: 0x000E84B0
	private void SendShowExplosion()
	{
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				base.GetComponent<NetworkView>().RPC("ShowExplosion", RPCMode.All, new object[]
				{
					this.explosionName
				});
			}
			else
			{
				this.photonView.RPC("ShowExplosion", PhotonTargets.All, new object[]
				{
					this.explosionName
				});
			}
		}
		else if (!Defs.isMulti)
		{
			this.ShowExplosion(this.explosionName);
		}
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000EA33C File Offset: 0x000E853C
	[PunRPC]
	[RPC]
	private void ShowExplosion(string explosionName)
	{
		if (this.currentRocketSettings == null)
		{
			return;
		}
		if (Defs.IsDeveloperBuild && explosionName == "WeaponLike")
		{
			explosionName = "Like";
		}
		Vector3 position = this.thisTransform.position;
		string name = ResPath.Combine("Explosions", explosionName);
		GameObject objectFromName = RayAndExplosionsStackController.sharedController.GetObjectFromName(name);
		if (objectFromName != null)
		{
			objectFromName.transform.position = position;
			bool flag = !Defs.isMulti || ((!Defs.isInet) ? base.GetComponent<NetworkView>().isMine : this.photonView.isMine);
			if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.SingularityGrenade)
			{
				objectFromName.GetComponent<SingularityHole>().owner = this.myPlayerMoveC;
			}
			if (flag)
			{
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.NuclearGrenade)
				{
					objectFromName.GetComponent<DamageInRadiusEffect>().ActivateEffect(this.damage * this.currentRocketSettings.toxicDamageMultiplier, this.multiplayerDamage * this.currentRocketSettings.toxicDamageMultiplier, this.currentRocketSettings.raduisDetectTarget, this.currentRocketSettings.toxicHitTime, this.weaponPrefabName, this.currentRocketSettings.typeDead, this.currentRocketSettings.typeKilsIconChat);
				}
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Molotov)
				{
					objectFromName.GetComponent<DamageInRadiusEffect>().ActivatePoisonEffect(this.damage, this.multiplayerDamage, this.currentRocketSettings.raduisDetectTarget, this.currentRocketSettings.toxicHitTime, this.currentWeaponSounds, Player_move_c.PoisonType.Burn);
				}
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.SlowdownGrenade)
				{
					objectFromName.GetComponent<DamageInRadiusEffect>().ActivateSlowdonEffect(this.currentWeaponSounds);
				}
			}
		}
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x000EA4F0 File Offset: 0x000E86F0
	[Obfuscation(Exclude = true)]
	private void DestroyRocket()
	{
		if (!Defs.isMulti || this.isMine)
		{
			base.CancelInvoke("KillRocket");
			if (this.currentRocketSettings != null && this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.Firework)
			{
				base.CancelInvoke("FlyFirework");
			}
			RocketStack.sharedController.ReturnRocket(base.gameObject);
		}
		this.SetRocketDeactive();
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x000EA564 File Offset: 0x000E8764
	[RPC]
	[PunRPC]
	public void SetRocketDeactive()
	{
		this.isRun = false;
		this.isKilled = false;
		this.isActivated = false;
		this.isStickedToPlayer = false;
		this.thisTransform.position = Vector3.down * 10000f;
		if (this.currentRocketSettings != null)
		{
			this.currentRocketSettings.gameObject.SetActive(false);
			if (this.currentRocketSettings.trail != null)
			{
				this.currentRocketSettings.trail.enabled = false;
			}
			if (this.currentRocketSettings.flyParticle != null)
			{
				this.currentRocketSettings.flyParticle.SetActive(false);
			}
			if (this.currentRocketSettings.stickedParticle != null)
			{
				this.currentRocketSettings.stickedParticle.SetActive(false);
			}
			if (this.currentRocketSettings.droneRotator != null)
			{
				this.currentRocketSettings.droneRotator.enabled = false;
				if (this.currentRocketSettings.typeFly == RocketSettings.TypeFlyRocket.FakeBonus)
				{
					this.currentRocketSettings.droneRotator.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
				}
			}
			if (this.currentRocketSettings.droneParticle != null)
			{
				this.currentRocketSettings.droneParticle.SetActive(false);
			}
		}
		this.currentRocketSettings = null;
	}

	// Token: 0x06002C2F RID: 11311 RVA: 0x000EA6D0 File Offset: 0x000E88D0
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.isMine && !this.isKilled)
		{
			if (this.chargePower == 1f)
			{
				this.photonView.RPC("SetRocketActive", player, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position
				});
			}
			else
			{
				this.photonView.RPC("SetRocketActiveWithCharge", player, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position,
					this.chargePower
				});
			}
			if (this.isRun && this.IsGrenadeWeaponName(this.weaponPrefabName))
			{
				this.photonView.RPC("StartRocketRPC", player, new object[0]);
			}
		}
	}

	// Token: 0x06002C30 RID: 11312 RVA: 0x000EA7CC File Offset: 0x000E89CC
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (this.isMine && !this.isKilled)
		{
			if (this.chargePower == 1f)
			{
				base.GetComponent<NetworkView>().RPC("SetRocketActive", player, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position
				});
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("SetRocketActiveWithCharge", player, new object[]
				{
					this.weaponPrefabName,
					this.radiusImpulse,
					base.transform.position,
					this.chargePower
				});
			}
			if (this.isRun && this.IsGrenadeWeaponName(this.weaponPrefabName))
			{
				base.GetComponent<NetworkView>().RPC("StartRocketRPC", player, new object[0]);
			}
		}
	}

	// Token: 0x040020DC RID: 8412
	private readonly float[] lightningCoefs;

	// Token: 0x040020DD RID: 8413
	[NonSerialized]
	public RocketSettings currentRocketSettings;

	// Token: 0x040020DE RID: 8414
	[NonSerialized]
	public bool isRun;

	// Token: 0x040020DF RID: 8415
	[NonSerialized]
	public string explosionName;

	// Token: 0x040020E0 RID: 8416
	[NonSerialized]
	public string weaponPrefabName;

	// Token: 0x040020E1 RID: 8417
	[NonSerialized]
	public float damage;

	// Token: 0x040020E2 RID: 8418
	[NonSerialized]
	public Vector2 damageRange;

	// Token: 0x040020E3 RID: 8419
	[NonSerialized]
	public float multiplayerDamage;

	// Token: 0x040020E4 RID: 8420
	[NonSerialized]
	public float radiusDamage;

	// Token: 0x040020E5 RID: 8421
	[NonSerialized]
	public float radiusDamageSelf;

	// Token: 0x040020E6 RID: 8422
	[NonSerialized]
	public float radiusImpulse;

	// Token: 0x040020E7 RID: 8423
	[NonSerialized]
	public float impulseForce;

	// Token: 0x040020E8 RID: 8424
	[NonSerialized]
	public float impulseForceSelf;

	// Token: 0x040020E9 RID: 8425
	[NonSerialized]
	public bool isSlowdown;

	// Token: 0x040020EA RID: 8426
	[NonSerialized]
	public float slowdownTime;

	// Token: 0x040020EB RID: 8427
	[NonSerialized]
	public float slowdownCoeff;

	// Token: 0x040020EC RID: 8428
	[NonSerialized]
	public float chargePower = 1f;

	// Token: 0x040020ED RID: 8429
	[NonSerialized]
	public bool isMine;

	// Token: 0x040020EE RID: 8430
	private WeaponSounds currentWeaponSounds;

	// Token: 0x040020EF RID: 8431
	private Dictionary<string, RocketSettings> rocketsDict = new Dictionary<string, RocketSettings>();

	// Token: 0x040020F0 RID: 8432
	private PhotonView photonView;

	// Token: 0x040020F1 RID: 8433
	private bool isKilled;

	// Token: 0x040020F2 RID: 8434
	private Player_move_c myPlayerMoveC;

	// Token: 0x040020F3 RID: 8435
	private Vector3 correctPos = Vector3.down * 1000f;

	// Token: 0x040020F4 RID: 8436
	private Transform thisTransform;

	// Token: 0x040020F5 RID: 8437
	private bool isFirstPos = true;

	// Token: 0x040020F6 RID: 8438
	private float lastToxicHit;

	// Token: 0x040020F7 RID: 8439
	private int counterJumpLightning;

	// Token: 0x040020F8 RID: 8440
	private int lightningHitCount;

	// Token: 0x040020F9 RID: 8441
	private float timerFromJumpLightning = 1f;

	// Token: 0x040020FA RID: 8442
	private Transform targetLightning;

	// Token: 0x040020FB RID: 8443
	private Transform targetDamageLightning;

	// Token: 0x040020FC RID: 8444
	private float maxTimerFromJumpLightning = 1f;

	// Token: 0x040020FD RID: 8445
	private float progressCaptureTargetLightning;

	// Token: 0x040020FE RID: 8446
	private bool isDetectFirstTargetLightning;

	// Token: 0x040020FF RID: 8447
	private Transform targetAutoHoming;

	// Token: 0x04002100 RID: 8448
	private Transform megabulletLastTarget;

	// Token: 0x04002101 RID: 8449
	private Transform stickedObject;

	// Token: 0x04002102 RID: 8450
	private Vector3 stickedObjectPos;

	// Token: 0x04002103 RID: 8451
	private Rigidbody myRigidbody;

	// Token: 0x04002104 RID: 8452
	private bool isActivated;

	// Token: 0x04002105 RID: 8453
	private int _rocketNum;

	// Token: 0x04002106 RID: 8454
	public Action OnExplode;

	// Token: 0x04002107 RID: 8455
	private Vector3 dronePoint;

	// Token: 0x04002108 RID: 8456
	private bool isStickedToPlayer;
}
