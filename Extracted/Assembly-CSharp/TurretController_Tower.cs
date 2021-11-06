using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000872 RID: 2162
public class TurretController_Tower : TurretController
{
	// Token: 0x06004E1F RID: 19999 RVA: 0x001C593C File Offset: 0x001C3B3C
	protected override void SearchTarget()
	{
		base.SearchTarget();
		if (Mathf.Abs(this.idleAlphaY) < 0.5f)
		{
			this.idleAlphaY = UnityEngine.Random.Range(-1f * this.maxDeltaRotateY / 2f, this.maxDeltaRotateY / 2f);
		}
		else
		{
			float num = Time.deltaTime * this.idleRotateSpeedY * Mathf.Abs(this.idleAlphaY) / this.idleAlphaY;
			this.idleAlphaY -= num;
			this.tower.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.tower.localRotation.eulerAngles.z + num));
		}
		if (Mathf.Abs(this.gun.localRotation.eulerAngles.x) > 1f)
		{
			this.gun.Rotate((float)((this.gun.localRotation.eulerAngles.x >= 180f) ? 1 : -1) * this.speedRotateX * Time.deltaTime, 0f, 0f);
		}
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001C5A78 File Offset: 0x001C3C78
	protected override IEnumerator ScanTarget()
	{
		this.inScaning = true;
		GameObject closestTargetObj = null;
		float closestTarget = float.MaxValue;
		Initializer.TargetsList targets = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
		foreach (Transform enemy in targets)
		{
			Vector3 enemyDelta = enemy.position - base.transform.position;
			Vector3 enemyForward = new Vector3(enemyDelta.x, 0f, enemyDelta.z);
			float targetDistance = enemyDelta.sqrMagnitude;
			if (targetDistance < closestTarget && targetDistance < this.maxRadiusScanTargetSQR && Vector3.Angle(enemyForward, enemyDelta) < this.maxRotateX)
			{
				Vector3 popravochka = Vector3.zero;
				BoxCollider _collider = enemy.GetComponent<BoxCollider>();
				if (_collider == null && enemy.CompareTag("Enemy"))
				{
					for (int i = 0; i < enemy.childCount; i++)
					{
						BoxCollider boxcollider = enemy.GetChild(i).GetComponent<BoxCollider>();
						if (boxcollider != null)
						{
							_collider = boxcollider;
							break;
						}
					}
				}
				if (_collider != null)
				{
					popravochka = _collider.transform.rotation * _collider.center;
				}
				Ray ray = new Ray(this.tower.position, enemy.transform.position + popravochka - this.tower.position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, this.maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && hit.collider.gameObject.transform.root.Equals(enemy.root))
				{
					closestTarget = targetDistance;
					closestTargetObj = enemy.gameObject;
				}
			}
			yield return null;
		}
		if (closestTargetObj != null)
		{
			this.target = closestTargetObj.transform;
		}
		else
		{
			this.target = null;
		}
		this.inScaning = false;
		yield break;
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001C5A94 File Offset: 0x001C3C94
	protected override void TargetUpdate()
	{
		base.TargetUpdate();
		bool flag = false;
		Vector2 to = new Vector2(this.target.position.x, this.target.position.z) - new Vector2(this.tower.position.x, this.tower.position.z);
		float deltaAngles = this.GetDeltaAngles(this.tower.rotation.eulerAngles.y, Mathf.Abs(to.x) / to.x * Vector2.Angle(Vector2.up, to));
		float num = -this.speedRotateY * Time.deltaTime * Mathf.Abs(deltaAngles) / deltaAngles;
		if (Mathf.Abs(deltaAngles) < 10f)
		{
			flag = true;
		}
		if (Mathf.Abs(num) > Mathf.Abs(deltaAngles))
		{
			num = -deltaAngles;
		}
		if (Mathf.Abs(num) > 0.001f)
		{
			this.tower.Rotate(0f, 0f, num);
		}
		Vector3 b = Vector3.zero;
		BoxCollider boxCollider = this.target.GetComponent<BoxCollider>();
		if (boxCollider == null && this.target.CompareTag("Enemy"))
		{
			for (int i = 0; i < this.target.childCount; i++)
			{
				BoxCollider component = this.target.GetChild(i).GetComponent<BoxCollider>();
				if (component != null)
				{
					boxCollider = component;
					break;
				}
			}
		}
		if (boxCollider != null)
		{
			b = boxCollider.transform.rotation * boxCollider.center;
		}
		float angle = -180f * Mathf.Atan((this.target.position.y + b.y - this.tower.position.y) / Vector3.Distance(this.target.position + b, base.transform.position)) / 3.1415927f;
		float deltaAngles2 = this.GetDeltaAngles(this.gun.localRotation.eulerAngles.x, angle);
		num = -this.speedRotateX * Time.deltaTime * Mathf.Abs(deltaAngles2) / deltaAngles2;
		if (Mathf.Abs(num) > Mathf.Abs(deltaAngles2))
		{
			num = -deltaAngles2;
		}
		if (Mathf.Abs(num) > 0.001f)
		{
			this.gun.Rotate(num, 0f, 0f);
		}
		if (flag)
		{
			this.timerShot -= Time.deltaTime;
			if (this.timerShot < 0f)
			{
				if (!Defs.isMulti)
				{
					base.ShotRPC();
				}
				else if (!Defs.isInet)
				{
					this._networkView.RPC("ShotRPC", RPCMode.All, new object[0]);
				}
				else
				{
					this.photonView.RPC("ShotRPC", PhotonTargets.All, new object[0]);
				}
				this.timerShot = this.maxTimerShot;
			}
		}
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001C5DC0 File Offset: 0x001C3FC0
	private float GetDeltaAngles(float angle1, float angle2)
	{
		if (angle1 < 0f)
		{
			angle1 += 360f;
		}
		if (angle2 < 0f)
		{
			angle2 += 360f;
		}
		float num = angle1 - angle2;
		if (Mathf.Abs(num) > 180f)
		{
			if (angle1 > angle2)
			{
				num -= 360f;
			}
			else
			{
				num += 360f;
			}
		}
		return num;
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001C5E28 File Offset: 0x001C4028
	protected override void OnKill()
	{
		if (this.gun.rotation.x > this.minRotateX)
		{
			this.gun.Rotate(this.speedRotateX * Time.deltaTime, 0f, 0f);
		}
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001C5E74 File Offset: 0x001C4074
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
	}

	// Token: 0x04003CDF RID: 15583
	[Header("Tower settings")]
	public Transform tower;

	// Token: 0x04003CE0 RID: 15584
	public Transform gun;

	// Token: 0x04003CE1 RID: 15585
	private float idleAlphaY;

	// Token: 0x04003CE2 RID: 15586
	private float idleRotateSpeedY = 20f;

	// Token: 0x04003CE3 RID: 15587
	private float maxDeltaRotateY = 60f;

	// Token: 0x04003CE4 RID: 15588
	private float maxRotateX = 75f;

	// Token: 0x04003CE5 RID: 15589
	private float minRotateX = -60f;

	// Token: 0x04003CE6 RID: 15590
	private float speedRotateY = 220f;

	// Token: 0x04003CE7 RID: 15591
	private float speedRotateX = 30f;

	// Token: 0x04003CE8 RID: 15592
	private float timerShot;
}
