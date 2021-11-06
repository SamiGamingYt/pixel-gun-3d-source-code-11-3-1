using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020005F9 RID: 1529
public class BaseExplosionObject : MonoBehaviour
{
	// Token: 0x06003481 RID: 13441 RVA: 0x0010F800 File Offset: 0x0010DA00
	private void Start()
	{
		this.InitializeData();
		Initializer.damageableObjects.Add(base.gameObject);
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x0010F818 File Offset: 0x0010DA18
	private void OnDestroy()
	{
		Initializer.damageableObjects.Remove(base.gameObject);
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0010F82C File Offset: 0x0010DA2C
	protected virtual void InitializeData()
	{
		this.isMultiplayerMode = Defs.isMulti;
		this.photonView = PhotonView.Get(this);
		this.InitializeRespawnPoint();
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0010F84C File Offset: 0x0010DA4C
	private void InitializeRespawnPoint()
	{
		if (this.isMultiplayerMode && !PhotonNetwork.isMasterClient)
		{
			GameObject gameObject = null;
			float num = float.MaxValue;
			for (int i = 0; i < ExplosionObjectRespawnController.respawnList.Count; i++)
			{
				if (ExplosionObjectRespawnController.respawnList[i] != null)
				{
					float sqrMagnitude = (ExplosionObjectRespawnController.respawnList[i].transform.position - base.transform.position).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						gameObject = ExplosionObjectRespawnController.respawnList[i];
					}
				}
			}
			if (gameObject != null)
			{
				base.transform.parent = gameObject.transform;
				this._respawnController = gameObject.GetComponent<ExplosionObjectRespawnController>();
			}
			else
			{
				this._respawnController = null;
			}
			return;
		}
		this._respawnController = base.transform.parent.GetComponent<ExplosionObjectRespawnController>();
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x0010F938 File Offset: 0x0010DB38
	private void PlayDestroyEffect()
	{
		UnityEngine.Object.Instantiate(this.explosionEffect, base.transform.position, Quaternion.identity);
		base.GetComponent<Animation>().Play("Broken");
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x0010F974 File Offset: 0x0010DB74
	protected bool IsTargetAvailable(Transform targetTransform)
	{
		return !targetTransform.Equals(base.transform) && (targetTransform.CompareTag("Player") || targetTransform.CompareTag("Enemy") || targetTransform.CompareTag("Turret") || (targetTransform.childCount > 0 && targetTransform.GetChild(0).CompareTag("DamagedExplosion")));
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0010F9E8 File Offset: 0x0010DBE8
	private void CheckTakeDamage()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, this.radiusExplosion, Tools.AllWithoutDamageCollidersMask);
		if (array.Length == 0)
		{
			return;
		}
		List<Transform> list = new List<Transform>();
		float num = this.radiusExplosion * this.radiusExplosion;
		float diameterMaxExplosion = this.radiusMaxExplosion * this.radiusMaxExplosion;
		for (int i = 0; i < array.Length; i++)
		{
			if (!(array[i].gameObject == null))
			{
				Transform root = array[i].transform.root;
				if (!(root.gameObject == null) && !(base.transform.gameObject == null))
				{
					if (!list.Contains(root))
					{
						if (this.IsTargetAvailable(root))
						{
							float sqrMagnitude = (root.position - base.transform.position).sqrMagnitude;
							if (sqrMagnitude <= num)
							{
								this.ApplyDamage(root, sqrMagnitude, num, diameterMaxExplosion);
								list.Add(root);
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x0010FB14 File Offset: 0x0010DD14
	private void ApplyDamage(Transform target, float distanceToTarget, float diameterExplosion, float diameterMaxExplosion)
	{
		IDamageable component = target.GetComponent<IDamageable>();
		if (component == null)
		{
			return;
		}
		int num = ExpController.OurTierForAnyPlace();
		if (component is PlayerDamageable)
		{
			Player_move_c myPlayer = (component as PlayerDamageable).myPlayer;
			num = ((!(myPlayer.myTable != null)) ? 0 : ExpController.TierForLevel(myPlayer.myTable.GetComponent<NetworkStartTable>().myRanks));
		}
		float damage;
		if (distanceToTarget > diameterMaxExplosion)
		{
			damage = this.damageByTier[num] * ((diameterExplosion - (distanceToTarget - diameterMaxExplosion)) / diameterExplosion);
		}
		else
		{
			damage = this.damageByTier[num];
		}
		IDamageable damageFrom = null;
		if (this is IDamageable)
		{
			damageFrom = (this as IDamageable);
		}
		component.ApplyDamage(damage, damageFrom, Player_move_c.TypeKills.none);
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x0010FBC8 File Offset: 0x0010DDC8
	private void RecreateObject()
	{
		if (this.isMultiplayerMode)
		{
			this.DestroyObjectByNetworkRpc();
			this.photonView.RPC("DestroyObjectByNetworkRpc", PhotonTargets.Others, new object[0]);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (this.isMultiplayerMode)
		{
			this.StartNewRespanObjectRpc();
			this.photonView.RPC("StartNewRespanObjectRpc", PhotonTargets.Others, new object[0]);
		}
		else if (this._respawnController != null)
		{
			this._respawnController.StartProcessNewRespawn();
		}
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x0010FC58 File Offset: 0x0010DE58
	public void RunExplosion()
	{
		if (this.isMultiplayerMode)
		{
			this.PlayDestroyEffect();
			this.photonView.RPC("PlayDestroyEffectRpc", PhotonTargets.Others, new object[0]);
		}
		else
		{
			this.PlayDestroyEffect();
		}
		this.CheckTakeDamage();
		this.RecreateObject();
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x0010FCA4 File Offset: 0x0010DEA4
	[RPC]
	[PunRPC]
	public void DestroyObjectByNetworkRpc()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			this.explosionObject.SetActive(false);
		}
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0010FCD8 File Offset: 0x0010DED8
	[RPC]
	[PunRPC]
	public void StartNewRespanObjectRpc()
	{
		if (this._respawnController != null)
		{
			this._respawnController.StartProcessNewRespawn();
		}
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x0010FCF8 File Offset: 0x0010DEF8
	[PunRPC]
	[RPC]
	public void PlayDestroyEffectRpc()
	{
		this.PlayDestroyEffect();
	}

	// Token: 0x0400268E RID: 9870
	private const string ExplosionAnimationName = "Broken";

	// Token: 0x0400268F RID: 9871
	[Header("Common Settings")]
	public GameObject explosionObject;

	// Token: 0x04002690 RID: 9872
	[Header("Common Damage settings")]
	public float radiusExplosion;

	// Token: 0x04002691 RID: 9873
	[Header("Common Damage settings")]
	public float radiusMaxExplosion;

	// Token: 0x04002692 RID: 9874
	public float damageZombie = 2f;

	// Token: 0x04002693 RID: 9875
	public float[] damageByTier = new float[ExpController.LevelsForTiers.Length];

	// Token: 0x04002694 RID: 9876
	[Header("Common Effect settings")]
	public GameObject explosionEffect;

	// Token: 0x04002695 RID: 9877
	protected bool isMultiplayerMode;

	// Token: 0x04002696 RID: 9878
	protected PhotonView photonView;

	// Token: 0x04002697 RID: 9879
	private ExplosionObjectRespawnController _respawnController;
}
