using System;
using Photon;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class ChestController : Photon.MonoBehaviour, IDamageable
{
	// Token: 0x060002B1 RID: 689 RVA: 0x00017BD8 File Offset: 0x00015DD8
	private void Start()
	{
		Initializer.damageableObjects.Add(base.gameObject);
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00017BEC File Offset: 0x00015DEC
	private void OnDestroy()
	{
		Initializer.damageableObjects.Remove(base.gameObject);
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00017C00 File Offset: 0x00015E00
	private void Update()
	{
		if (!this.oldIsMaster && PhotonNetwork.isMasterClient && this.isKilled)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		this.oldIsMaster = PhotonNetwork.isMasterClient;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00017C44 File Offset: 0x00015E44
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00017C58 File Offset: 0x00015E58
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerViewId = 0)
	{
		this.MinusLive(damage);
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00017C64 File Offset: 0x00015E64
	public bool IsEnemyTo(Player_move_c player)
	{
		return true;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x00017C68 File Offset: 0x00015E68
	public bool IsDead()
	{
		return this.isKilled;
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002B8 RID: 696 RVA: 0x00017C70 File Offset: 0x00015E70
	public bool isLivingTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00017C74 File Offset: 0x00015E74
	public void MinusLive(float _minus)
	{
		base.photonView.RPC("KilledChest", PhotonTargets.All, new object[0]);
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00017C90 File Offset: 0x00015E90
	[PunRPC]
	[RPC]
	public void MinusLiveRPC(float _minus)
	{
		if (this.isKilled)
		{
			return;
		}
		this.live -= _minus;
		base.photonView.RPC("SynchLiveRPC", PhotonTargets.AllBuffered, new object[]
		{
			this.live
		});
		if (this.live <= 0f)
		{
			base.photonView.RPC("KilledChest", PhotonTargets.AllBuffered, new object[0]);
		}
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00017D04 File Offset: 0x00015F04
	[PunRPC]
	[RPC]
	public void SynchLiveRPC(float _live)
	{
		this.live = _live;
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00017D10 File Offset: 0x00015F10
	[RPC]
	[PunRPC]
	public void KilledChest()
	{
		if (this.isKilled)
		{
			return;
		}
		this.isKilled = true;
		if (PhotonNetwork.isMasterClient)
		{
			int num = UnityEngine.Random.Range(0, ChestController.weaponForHungerGames.Length);
			if (this.isChestBonus)
			{
				if (UnityEngine.Random.Range(0, 11) < 7)
				{
					BonusController.sharedController.AddBonusForHunger(base.transform.position, this.TypeBonus(), base.transform.GetComponent<SettingBonus>().numberSpawnZone);
				}
				else
				{
					PhotonNetwork.InstantiateSceneObject("Weapon_Bonuses/Weapon" + ChestController.weaponForHungerGames[num] + "_Bonus", base.transform.position, base.transform.rotation, 0, null);
				}
			}
			else
			{
				PhotonNetwork.InstantiateSceneObject("Weapon_Bonuses/Weapon" + ChestController.weaponForHungerGames[num] + "_Bonus", base.transform.position, base.transform.rotation, 0, null);
			}
		}
		if (Defs.isSoundFX)
		{
			base.gameObject.GetComponent<AudioSource>().PlayOneShot(this.brokenAudio);
		}
		base.GetComponent<Animation>().Stop();
		base.GetComponent<Animation>().Play("Broken");
		base.Invoke("DestroyChestRPC", 0.5f);
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00017E54 File Offset: 0x00016054
	private int TypeBonus()
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (num < 70)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00017E78 File Offset: 0x00016078
	private void DestroyChest()
	{
		base.photonView.RPC("DestroyChestRPC", PhotonTargets.AllBuffered, new object[0]);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00017E94 File Offset: 0x00016094
	[PunRPC]
	[RPC]
	private void DestroyChestRPC()
	{
		Debug.Log("DestroyChestRPC");
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.transform.position = new Vector3(0f, -10000f, 0f);
		}
	}

	// Token: 0x040002E8 RID: 744
	public bool isStartChest = true;

	// Token: 0x040002E9 RID: 745
	public int currentSpawnZone;

	// Token: 0x040002EA RID: 746
	public float live = 5f;

	// Token: 0x040002EB RID: 747
	public bool isKilled;

	// Token: 0x040002EC RID: 748
	public bool isChestBonus;

	// Token: 0x040002ED RID: 749
	public static readonly int[] weaponForHungerGames = new int[]
	{
		1,
		2,
		3,
		8,
		53,
		5,
		52,
		51,
		66,
		67,
		162,
		333
	};

	// Token: 0x040002EE RID: 750
	public AudioClip brokenAudio;

	// Token: 0x040002EF RID: 751
	private bool oldIsMaster;
}
