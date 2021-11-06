using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
public sealed class BonusItem : MonoBehaviour
{
	// Token: 0x060001AF RID: 431 RVA: 0x00010A38 File Offset: 0x0000EC38
	private void Awake()
	{
		this.isMulti = Defs.isMulti;
		this.isInet = Defs.isInet;
		this.isCOOP = Defs.isCOOP;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00010A5C File Offset: 0x0000EC5C
	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		if (!Defs.isMulti)
		{
			this.player = GameObject.FindGameObjectWithTag("Player");
			if (this.player != null)
			{
				this.playerMoveC = this.player.GetComponent<SkinName>().playerMoveC;
			}
			else
			{
				Debug.LogWarning("BonusItem.Start(): player == null");
			}
		}
		else
		{
			this.player = this._weaponManager.myPlayer;
			this.playerMoveC = this._weaponManager.myPlayerMoveC;
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00010AEC File Offset: 0x0000ECEC
	public void ActivateBonus(BonusController.TypeBonus type, Vector3 position, double expireTime, int zoneNumber, int index)
	{
		if (this.isActive)
		{
			return;
		}
		this.type = type;
		base.transform.position = position;
		this.SetModel(true);
		this.mySpawnNumber = zoneNumber;
		this.myIndex = index;
		if (expireTime != -1.0)
		{
			this.isTimeBonus = true;
			this.expireTime = expireTime;
		}
		else
		{
			this.isTimeBonus = false;
			this.expireTime = -1.0;
		}
		this.isActive = true;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00010B70 File Offset: 0x0000ED70
	private void SetModel(bool show = true)
	{
		this.itemMdls[(int)this.type].SetActive(show);
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00010B88 File Offset: 0x0000ED88
	public void DeactivateBonus()
	{
		this.isPickedUp = false;
		this.playerPicked = null;
		this.isActive = false;
		base.transform.position = Vector3.down * 100f;
		this.SetModel(false);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x00010BCC File Offset: 0x0000EDCC
	public void PickupBonus(PhotonPlayer player)
	{
		this.isPickedUp = true;
		this.playerPicked = player;
		base.transform.position = Vector3.down * 100f;
		this.SetModel(false);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00010C08 File Offset: 0x0000EE08
	private void Update()
	{
		if (!this.isActive)
		{
			return;
		}
		if (this.isMulti)
		{
			if (this.player == null)
			{
				this.player = this._weaponManager.myPlayer;
				this.playerMoveC = this._weaponManager.myPlayerMoveC;
			}
		}
		else if (this.player == null || this.playerMoveC == null || this.playerMoveC.isKilled || (this.playerMoveC.inGameGUI != null && (this.playerMoveC.inGameGUI.pausePanel.activeSelf || this.playerMoveC.inGameGUI.blockedCollider.activeSelf)) || ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.isTimeBonus && ((PhotonNetwork.isMasterClient && Defs.isInet && PhotonNetwork.time > this.expireTime) || (!Defs.isInet && Network.time > this.expireTime)))
		{
			BonusController.sharedController.RemoveBonus(this.myIndex);
			return;
		}
		if (this.player == null || this.playerMoveC == null || this.playerMoveC.isKilled)
		{
			return;
		}
		float num = Vector3.SqrMagnitude(base.transform.position - this.player.transform.position);
		if (num < 4f)
		{
			bool flag = false;
			switch (this.type)
			{
			case BonusController.TypeBonus.Ammo:
				if (!this._weaponManager.AddAmmoForAllGuns())
				{
					GlobalGameController.Score += Defs.ScoreForSurplusAmmo;
				}
				flag = true;
				if (Defs.isMulti)
				{
					this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Ammo);
				}
				break;
			case BonusController.TypeBonus.Health:
				if (this.playerMoveC.CurHealth == this.playerMoveC.MaxHealth)
				{
					if (!this.isMulti || this.isCOOP)
					{
						GlobalGameController.Score += 100;
					}
					flag = true;
				}
				else
				{
					this.playerMoveC.CurHealth += this.playerMoveC.MaxHealth / 2f;
					if (this.playerMoveC.CurHealth > this.playerMoveC.MaxHealth)
					{
						this.playerMoveC.CurHealth = this.playerMoveC.MaxHealth;
					}
					flag = true;
					if (Defs.isMulti)
					{
						this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
					}
				}
				break;
			case BonusController.TypeBonus.Armor:
				if (this.playerMoveC.curArmor + 1f > this.playerMoveC.MaxArmor)
				{
					if (!this.isMulti || this.isCOOP)
					{
						GlobalGameController.Score += 100;
					}
				}
				else
				{
					this.playerMoveC.curArmor += 1f;
				}
				flag = true;
				if (Defs.isMulti)
				{
					this.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Armor);
				}
				break;
			case BonusController.TypeBonus.Gem:
				flag = true;
				break;
			}
			if (flag)
			{
				if (Defs.isSoundFX)
				{
					this.playerMoveC.gameObject.GetComponent<AudioSource>().PlayOneShot(this.itemSounds[(int)this.type]);
				}
				if (this.type == BonusController.TypeBonus.Gem)
				{
					BonusController.sharedController.GetAndRemoveBonus(this.myIndex);
				}
				else
				{
					BonusController.sharedController.RemoveBonus(this.myIndex);
				}
			}
		}
	}

	// Token: 0x040001AE RID: 430
	private Player_move_c playerMoveC;

	// Token: 0x040001AF RID: 431
	private GameObject player;

	// Token: 0x040001B0 RID: 432
	public bool isActive = true;

	// Token: 0x040001B1 RID: 433
	public bool isPickedUp;

	// Token: 0x040001B2 RID: 434
	public PhotonPlayer playerPicked;

	// Token: 0x040001B3 RID: 435
	public AudioClip[] itemSounds = new AudioClip[Enum.GetValues(typeof(BonusController.TypeBonus)).Length];

	// Token: 0x040001B4 RID: 436
	public GameObject[] itemMdls = new GameObject[Enum.GetValues(typeof(BonusController.TypeBonus)).Length];

	// Token: 0x040001B5 RID: 437
	private bool isMulti;

	// Token: 0x040001B6 RID: 438
	private bool isInet;

	// Token: 0x040001B7 RID: 439
	private bool isCOOP;

	// Token: 0x040001B8 RID: 440
	private WeaponManager _weaponManager;

	// Token: 0x040001B9 RID: 441
	public BonusController.TypeBonus type;

	// Token: 0x040001BA RID: 442
	public double expireTime = -1.0;

	// Token: 0x040001BB RID: 443
	public bool isTimeBonus;

	// Token: 0x040001BC RID: 444
	public int mySpawnNumber = -1;

	// Token: 0x040001BD RID: 445
	public int myIndex;
}
