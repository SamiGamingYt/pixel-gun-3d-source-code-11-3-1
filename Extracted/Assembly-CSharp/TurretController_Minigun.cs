using System;
using System.Reflection;
using Rilisoft;
using UnityEngine;
using ZeichenKraftwerk;

// Token: 0x02000870 RID: 2160
public sealed class TurretController_Minigun : TurretController_Tower
{
	// Token: 0x06004E13 RID: 19987 RVA: 0x001C52A8 File Offset: 0x001C34A8
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (this.rotator != null && this.rotator.eulersPerSecond.z < -200f)
		{
			this.rotator.eulersPerSecond = new Vector3(0f, this.rotator.eulersPerSecond.z + this.downSpeedRotator * Time.deltaTime, 0f);
		}
	}

	// Token: 0x06004E14 RID: 19988 RVA: 0x001C5320 File Offset: 0x001C3520
	[Obfuscation(Exclude = true)]
	private void StopGunFlash()
	{
		this.gunFlash.enableEmission = false;
	}

	// Token: 0x06004E15 RID: 19989 RVA: 0x001C5330 File Offset: 0x001C3530
	protected override void Shot()
	{
		if (this.shotPoint2 == null || this.shotPoint == null)
		{
			return;
		}
		if (this.rotator != null)
		{
			this.rotator.eulersPerSecond = new Vector3(0f, this.maxSpeedRotator, 0f);
		}
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.shotClip);
		}
		if (this.gunFlash != null)
		{
			this.gunFlash.enableEmission = true;
			this.gunFlash.Play();
		}
		base.CancelInvoke("StopGunFlash");
		base.Invoke("StopGunFlash", this.maxTimerShot * 1.1f);
		if (Defs.isMulti && !this.isMine)
		{
			return;
		}
		Vector3 vector = new Vector3(this.shotPoint2.position.x - this.shotPoint.position.x + UnityEngine.Random.Range(-this.dissipation, this.dissipation), this.shotPoint2.position.y - this.shotPoint.position.y + UnityEngine.Random.Range(-this.dissipation, this.dissipation), this.shotPoint2.position.z - this.shotPoint.position.z + UnityEngine.Random.Range(-this.dissipation, this.dissipation));
		Ray ray = new Ray(this.shotPoint.position, vector);
		Debug.DrawRay(this.shotPoint.position, vector * 100f, Color.green, 1f);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, Tools.AllWithoutDamageCollidersMask) && (!Defs.isMulti || WeaponManager.sharedManager.myPlayer != null))
		{
			bool flag = raycastHit.collider.transform.root != null && raycastHit.collider.transform.root.gameObject.Equals(WeaponManager.sharedManager.myPlayer);
			bool flag2 = false;
			if (flag || base.HitIDestructible(raycastHit.collider.gameObject))
			{
				flag2 = true;
			}
			if (Defs.isMulti)
			{
				if (!Defs.isInet)
				{
					WeaponManager.sharedManager.myPlayerMoveC.GetComponent<NetworkView>().RPC("HoleRPC", RPCMode.All, new object[]
					{
						flag2,
						raycastHit.point + raycastHit.normal * 0.001f,
						Quaternion.FromToRotation(Vector3.up, raycastHit.normal)
					});
				}
				else
				{
					WeaponManager.sharedManager.myPlayerMoveC.photonView.RPC("HoleRPC", PhotonTargets.All, new object[]
					{
						flag2,
						raycastHit.point + raycastHit.normal * 0.001f,
						Quaternion.FromToRotation(Vector3.up, raycastHit.normal)
					});
				}
			}
		}
	}

	// Token: 0x04003CD3 RID: 15571
	[Header("Minigun settings")]
	public ParticleSystem gunFlash;

	// Token: 0x04003CD4 RID: 15572
	public Rotator rotator;

	// Token: 0x04003CD5 RID: 15573
	public AudioClip shotClip;

	// Token: 0x04003CD6 RID: 15574
	public Transform shotPoint;

	// Token: 0x04003CD7 RID: 15575
	public Transform shotPoint2;

	// Token: 0x04003CD8 RID: 15576
	private float maxSpeedRotator = -1000f;

	// Token: 0x04003CD9 RID: 15577
	private float downSpeedRotator = 500f;

	// Token: 0x04003CDA RID: 15578
	private float dissipation = 0.015f;
}
