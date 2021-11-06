using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000871 RID: 2161
public sealed class TurretController_MusicBox : TurretController
{
	// Token: 0x06004E17 RID: 19991 RVA: 0x001C5694 File Offset: 0x001C3894
	protected override void SearchTarget()
	{
		base.SearchTarget();
		if (this.isPlayMusicDater)
		{
			this.PlayMusic(false);
			if (!Defs.isInet)
			{
				this._networkView.RPC("PlayMusic", RPCMode.Others, new object[]
				{
					false
				});
			}
			else
			{
				this.photonView.RPC("PlayMusic", PhotonTargets.Others, new object[]
				{
					false
				});
			}
		}
	}

	// Token: 0x06004E18 RID: 19992 RVA: 0x001C5708 File Offset: 0x001C3908
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
			if (targetDistance < closestTarget && targetDistance < this.maxRadiusScanTargetSQR)
			{
				Vector3 popravochka = Vector3.zero;
				BoxCollider _collider = enemy.GetComponent<BoxCollider>();
				if (_collider != null)
				{
					popravochka = _collider.center;
				}
				Ray ray = new Ray(this.tower.position, enemy.position + popravochka - this.tower.position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, this.maxRadiusScanTarget, Tools.AllWithoutDamageCollidersMask) && (hit.collider.gameObject == enemy.gameObject || (hit.collider.gameObject.transform.parent != null && (hit.collider.gameObject.transform.parent.Equals(enemy) || hit.collider.gameObject.transform.parent.Equals(enemy.parent)))))
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

	// Token: 0x06004E19 RID: 19993 RVA: 0x001C5724 File Offset: 0x001C3924
	protected override void TargetUpdate()
	{
		base.TargetUpdate();
		if (!this.isPlayMusicDater)
		{
			this.PlayMusic(true);
			if (!Defs.isInet)
			{
				this._networkView.RPC("PlayMusic", RPCMode.Others, new object[]
				{
					true
				});
			}
			else
			{
				this.photonView.RPC("PlayMusic", PhotonTargets.Others, new object[]
				{
					true
				});
			}
		}
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001C5798 File Offset: 0x001C3998
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (this.isPlayMusicDater)
		{
			this.tower.Rotate(new Vector3(0f, 0f, 180f * Time.deltaTime));
			this.gun.Rotate(new Vector3(180f * Time.deltaTime, 0f, 0f));
		}
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001C5800 File Offset: 0x001C3A00
	[RPC]
	[PunRPC]
	private void PlayMusic(bool isPlay)
	{
		if (this.isPlayMusicDater == isPlay)
		{
			return;
		}
		this.isPlayMusicDater = isPlay;
		if (isPlay)
		{
			if (Defs.isSoundFX)
			{
				base.GetComponent<AudioSource>().loop = true;
				base.GetComponent<AudioSource>().clip = this.musicDater;
				base.GetComponent<AudioSource>().Play();
			}
		}
		else
		{
			base.GetComponent<AudioSource>().Stop();
		}
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x001C586C File Offset: 0x001C3A6C
	protected override void PlayerConnectedLocal(NetworkPlayer player)
	{
		base.PlayerConnectedLocal(player);
		this._networkView.RPC("PlayMusic", player, new object[]
		{
			this.isPlayMusicDater
		});
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x001C58A8 File Offset: 0x001C3AA8
	protected override void PlayerConnectedPhoton(PhotonPlayer player)
	{
		base.PlayerConnectedPhoton(player);
		this.photonView.RPC("PlayMusic", player, new object[]
		{
			this.isPlayMusicDater
		});
	}

	// Token: 0x04003CDB RID: 15579
	[Header("MusicBox settings")]
	public AudioClip musicDater;

	// Token: 0x04003CDC RID: 15580
	public Transform tower;

	// Token: 0x04003CDD RID: 15581
	public Transform gun;

	// Token: 0x04003CDE RID: 15582
	private bool isPlayMusicDater;
}
