using System;
using UnityEngine;

// Token: 0x0200086E RID: 2158
public class TurretController_ChristmasTree : TurretController
{
	// Token: 0x06004E0C RID: 19980 RVA: 0x001C4E70 File Offset: 0x001C3070
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		this.gunFlashes.SetActive(this.isReady);
		if (this.workSound != null)
		{
			this.workSound.SetActive(this.isReady && Defs.isSoundFX);
		}
		if (!this.isReady)
		{
			return;
		}
		float num = Time.deltaTime * this.towerRotationSpeed;
		this.tower.localRotation = Quaternion.Euler(new Vector3(0f, 0f, this.tower.localRotation.eulerAngles.z - num));
		if (!this.isRun || this.isKilled || (Defs.isMulti && !this.isMine))
		{
			return;
		}
		if (this.nextHitTime < Time.time)
		{
			this.ShotTargets();
			this.nextHitTime = Time.time + this.hitTime;
		}
	}

	// Token: 0x06004E0D RID: 19981 RVA: 0x001C4F6C File Offset: 0x001C316C
	private void ShotTargets()
	{
		Initializer.TargetsList targetsList = new Initializer.TargetsList(this.myPlayerMoveC, false, true);
		foreach (Transform transform in targetsList)
		{
			if (this.hitChance <= (float)UnityEngine.Random.Range(0, 100))
			{
				Vector3 vector = transform.transform.position - base.transform.position;
				if (vector.y >= -this.damageHeight && vector.y <= this.damageHeight && vector.x * vector.x + vector.z * vector.z <= this.damageRadius * this.damageRadius)
				{
					base.HitIDestructible(transform.gameObject);
				}
			}
		}
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
	}

	// Token: 0x06004E0E RID: 19982 RVA: 0x001C50C0 File Offset: 0x001C32C0
	protected override void Shot()
	{
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.shotClip);
		}
	}

	// Token: 0x04003CC5 RID: 15557
	[Header("ChristmasTree settings")]
	public Transform tower;

	// Token: 0x04003CC6 RID: 15558
	public float damageRadius = 7f;

	// Token: 0x04003CC7 RID: 15559
	public float damageHeight = 2f;

	// Token: 0x04003CC8 RID: 15560
	public float towerRotationSpeed = 900f;

	// Token: 0x04003CC9 RID: 15561
	public float hitTime = 0.1f;

	// Token: 0x04003CCA RID: 15562
	public float hitChance = 10f;

	// Token: 0x04003CCB RID: 15563
	public GameObject gunFlashes;

	// Token: 0x04003CCC RID: 15564
	public GameObject workSound;

	// Token: 0x04003CCD RID: 15565
	public AudioClip shotClip;

	// Token: 0x04003CCE RID: 15566
	private float nextHitTime;
}
