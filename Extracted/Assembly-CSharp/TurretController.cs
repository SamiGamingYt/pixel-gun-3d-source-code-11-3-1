using System;
using System.Collections;
using System.Reflection;
using Rilisoft;
using UnityEngine;

// Token: 0x0200086C RID: 2156
public class TurretController : MonoBehaviour, IDamageable
{
	// Token: 0x06004DDC RID: 19932 RVA: 0x001C3474 File Offset: 0x001C1674
	private void Awake()
	{
		this.originalMaterials = new Material[this.turretRenderer.materials.Length];
		for (int i = 0; i < this.turretRenderer.sharedMaterials.Length; i++)
		{
			this.originalMaterials[i] = this.turretRenderer.sharedMaterials[i];
		}
		this.photonView = PhotonView.Get(this);
		this.myCollider = base.GetComponent<BoxCollider>();
		this._networkView = base.GetComponent<NetworkView>();
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.maxRadiusScanTargetSQR = this.maxRadiusScanTarget * this.maxRadiusScanTarget;
		this.rigidBody = base.transform.GetComponent<Rigidbody>();
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001C3540 File Offset: 0x001C1740
	private void OnCollisionEnter(Collision col)
	{
		if (this.isMine && col.gameObject.name == "DeadCollider")
		{
			this.MinusLive(1000f, 0);
			this.rigidBody.isKinematic = true;
		}
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001C358C File Offset: 0x001C178C
	private void Start()
	{
		this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		this.UpdateMaterial();
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				this.isMine = this._networkView.isMine;
			}
			else
			{
				this.isMine = this.photonView.isMine;
			}
		}
		if (!Defs.isMulti || this.isMine)
		{
			this.numUpdate = 0;
			if (!Defs.isDaterRegim)
			{
				string text = base.gameObject.nameNoClone();
				string text2 = GadgetsInfo.LastBoughtFor(text);
				if (text2 != null)
				{
					this.numUpdate = GadgetsInfo.Upgrades[text].IndexOf(text2);
				}
				Player_move_c.SetLayerRecursively(base.gameObject, 9);
			}
			if (Defs.isMulti)
			{
				if (!Defs.isInet)
				{
					this._networkView.RPC("SynchNumUpdateRPC", RPCMode.AllBuffered, new object[]
					{
						this.numUpdate
					});
				}
				else
				{
					this.photonView.RPC("SynchNumUpdateRPC", PhotonTargets.AllBuffered, new object[]
					{
						this.numUpdate
					});
				}
			}
			else
			{
				this.SynchNumUpdateRPC(this.numUpdate);
			}
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				for (int i = 0; i < Initializer.players.Count; i++)
				{
					if (this.photonView.ownerId == Initializer.players[i].mySkinName.photonView.ownerId)
					{
						this.myPlayer = Initializer.players[i].mySkinName.gameObject;
						this.myPlayerMoveC = this.myPlayer.GetComponent<SkinName>().playerMoveC;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < Initializer.players.Count; j++)
				{
					if (this._networkView.owner == Initializer.players[j].mySkinName.GetComponent<NetworkView>().owner)
					{
						this.myPlayer = Initializer.players[j].mySkinName.gameObject;
						this.myPlayerMoveC = this.myPlayer.GetComponent<SkinName>().playerMoveC;
						break;
					}
				}
			}
		}
		else
		{
			this.myPlayer = WeaponManager.sharedManager.myPlayer;
			this.myPlayerMoveC = this.myPlayer.GetComponent<SkinName>().playerMoveC;
		}
		this.SetPlayerParent();
		if (this is VoodooSnowman)
		{
			if (!Defs.isMulti || this.isMine)
			{
				Initializer.damageableObjects.Add(base.gameObject);
			}
		}
		else
		{
			Initializer.turretsObj.Add(base.gameObject);
		}
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001C386C File Offset: 0x001C1A6C
	protected void SetPlayerParent()
	{
		if (!this.isRun && this.myPlayerMoveC != null && !this.wasStickedToPlayer)
		{
			this.wasStickedToPlayer = true;
			base.transform.parent = this.myPlayerMoveC.turretPoint.transform;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001C38E4 File Offset: 0x001C1AE4
	protected virtual IEnumerator ScanTarget()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06004DE1 RID: 19937 RVA: 0x001C38F8 File Offset: 0x001C1AF8
	private void UpdateNickLabelColor()
	{
		if (this.nickLabel == null)
		{
			return;
		}
		if (ConnectSceneNGUIController.isTeamRegim)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC == null || this.myPlayerMoveC == null)
			{
				if (this._nickColorInd != 0)
				{
					this.nickLabel.color = Color.white;
					this._nickColorInd = 0;
				}
			}
			else if (WeaponManager.sharedManager.myPlayerMoveC.myCommand == this.myPlayerMoveC.myCommand)
			{
				if (this._nickColorInd != 1)
				{
					this.nickLabel.color = Color.blue;
					this._nickColorInd = 1;
				}
			}
			else if (this._nickColorInd != 2)
			{
				this.nickLabel.color = Color.red;
				this._nickColorInd = 2;
			}
		}
		else if (Defs.isDaterRegim)
		{
			if (this._nickColorInd != 0)
			{
				this.nickLabel.color = Color.white;
				this._nickColorInd = 0;
			}
		}
		else if (Defs.isCOOP || (this.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC == this.myPlayerMoveC))
		{
			if (this._nickColorInd != 1)
			{
				this.nickLabel.color = Color.blue;
				this._nickColorInd = 1;
			}
		}
		else if (this._nickColorInd != 2)
		{
			this.nickLabel.color = Color.red;
			this._nickColorInd = 2;
		}
	}

	// Token: 0x06004DE2 RID: 19938 RVA: 0x001C3A90 File Offset: 0x001C1C90
	protected virtual void OnKill()
	{
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001C3A94 File Offset: 0x001C1C94
	protected virtual void SearchTarget()
	{
		this.timerScanTargetIdle -= Time.deltaTime;
		if (this.timerScanTargetIdle < 0f)
		{
			this.timerScanTargetIdle = this.maxTimerScanTargetIdle;
			if (!this.inScaning)
			{
				base.StartCoroutine(this.ScanTarget());
			}
		}
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001C3AE8 File Offset: 0x001C1CE8
	protected virtual void TargetUpdate()
	{
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001C3AEC File Offset: 0x001C1CEC
	private void Update()
	{
		if (!this.isRun && !this.wasStickedToPlayer)
		{
			this.SetPlayerParent();
		}
		if (this.setMaterialsForEnemy && this.isEnemyTurret != this.isSetAsEnemy)
		{
			this.UpdateMaterial();
			this.isSetAsEnemy = this.isEnemyTurret;
		}
		this.UpdateTurret();
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x001C3B4C File Offset: 0x001C1D4C
	protected virtual void UpdateTurret()
	{
		if (Defs.isMulti && this.myPlayerMoveC != null && !this._isSetNickLabelText)
		{
			this._isSetNickLabelText = true;
			if (this.nickLabel != null)
			{
				this.nickLabel.text = FilterBadWorld.FilterString(this.myPlayerMoveC.mySkinName.NickName);
			}
		}
		this.UpdateNickLabelColor();
		if (this.isRun && this.healthBar != null)
		{
			this.healthBar.localScale = new Vector3((this.health <= 0f) ? 0f : (this.health / this.maxHealth), 1f, 1f);
		}
		this.SetStateIsEnemyTurret();
		if (this.isEnemySprite != null && this.isEnemySprite.activeSelf != this.isEnemyTurret)
		{
			this.isEnemySprite.SetActive(this.isEnemyTurret);
		}
		this.isReady = (this.isRun && !this.isKilled && (this.animation == null || !this.animation.IsPlaying("turret_start")));
		if (!Defs.isMulti || this.isMine)
		{
			if (Defs.isMulti && WeaponManager.sharedManager.myPlayer == null)
			{
				this.DestroyTurrel();
				return;
			}
			if (!this.isRun)
			{
				Ray ray = new Ray(this.rayGroundPoint.position, Vector3.down);
				RaycastHit raycastHit;
				bool flag = Physics.Raycast(ray, out raycastHit, 1f, Tools.AllWithoutDamageCollidersMask) && raycastHit.distance > 0.05f && raycastHit.distance < 0.7f;
				bool flag2;
				if (this.validatePlaceWithSphere)
				{
					flag2 = Physics.CheckSphere(this.spherePoint.position, this.spherePoint.GetComponent<SphereCollider>().radius, Tools.AllWithoutMyPlayerMask);
				}
				else
				{
					RaycastHit raycastHit2;
					flag2 = (Physics.Linecast(this.myPlayerMoveC.myTransform.position, this.spherePoint.position, out raycastHit2, Tools.AllWithoutMyPlayerMask) && raycastHit2.collider.transform.root != base.transform);
				}
				if (flag && !flag2)
				{
					this.turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 1f, 1f, 0.08f));
					if (InGameGUI.sharedInGameGUI != null)
					{
						InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = true;
					}
				}
				else
				{
					this.turretRenderer.materials[0].SetColor("_TintColor", new Color(1f, 0f, 0f, 0.08f));
					if (InGameGUI.sharedInGameGUI != null)
					{
						InGameGUI.sharedInGameGUI.runTurrelButton.GetComponent<UIButton>().isEnabled = false;
					}
				}
				return;
			}
			if (this.isKilled)
			{
				this.OnKill();
				return;
			}
			if (!this.isReady)
			{
				return;
			}
			if (this.target != null && (this.target.position.y < -500f || (!Defs.isDaterRegim && this.target.CompareTag("Player") && this.target.GetComponent<SkinName>().playerMoveC.isInvisible)))
			{
				this.target = null;
			}
			if (this.target == null)
			{
				this.SearchTarget();
				return;
			}
			this.TargetUpdate();
			this.timerScanTargetIdle -= Time.deltaTime;
			if (this.timerScanTargetIdle < 0f)
			{
				this.timerScanTargetIdle = this.maxTimerScanTargetIdle;
				if (!this.inScaning)
				{
					base.StartCoroutine(this.ScanTarget());
				}
			}
			if (!this.rigidBody.isKinematic)
			{
				if (base.transform.position.y < this.turretMinPos.y)
				{
					this.turretMinPos = base.transform.position;
				}
				else
				{
					base.transform.position = this.turretMinPos;
				}
			}
		}
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001C3FC4 File Offset: 0x001C21C4
	[RPC]
	[PunRPC]
	public void SynchNumUpdateRPC(int _numUpdate)
	{
		if (Defs.isDaterRegim)
		{
			return;
		}
		this.numUpdate = _numUpdate;
		string key = base.gameObject.name.Replace("(Clone)", string.Empty) + ((_numUpdate <= 0) ? string.Empty : ("_up" + _numUpdate));
		if (!GadgetsInfo.info.ContainsKey(key))
		{
			return;
		}
		this.SetParametersFromGadgets(GadgetsInfo.info[key]);
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001C4048 File Offset: 0x001C2248
	protected virtual void SetParametersFromGadgets(GadgetInfo info)
	{
		if (Defs.isMulti && !Defs.isCOOP)
		{
			this.maxHealth = info.Durability;
			this.health = this.maxHealth;
			this.damageMulty = info.Damage;
		}
		else
		{
			this.damageSurvival = (float)info.SurvivalDamage;
			this.maxHealth = this.healthSurvival;
			this.health = this.maxHealth;
		}
		if (!Defs.isMulti || this.isMine)
		{
			this.gadgetName = GadgetsInfo.BaseName(info.Id);
		}
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x001C40E0 File Offset: 0x001C22E0
	private void SetStateIsEnemyTurret()
	{
		bool flag = this.isEnemyTurret;
		this.isEnemyTurret = false;
		if (ConnectSceneNGUIController.isTeamRegim)
		{
			if (this.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.myPlayerMoveC.myCommand != WeaponManager.sharedManager.myPlayerMoveC.myCommand)
			{
				this.isEnemyTurret = true;
			}
		}
		else if (Defs.isMulti && !this.isMine)
		{
			this.isEnemyTurret = true;
		}
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x001C4174 File Offset: 0x001C2374
	[RPC]
	[PunRPC]
	protected void ShotRPC()
	{
		this.Shot();
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x001C417C File Offset: 0x001C237C
	protected virtual void Shot()
	{
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x001C4180 File Offset: 0x001C2380
	protected bool HitIDestructible(GameObject _obj)
	{
		IDamageable component = _obj.GetComponent<IDamageable>();
		if (component == null && _obj.transform.parent != null)
		{
			component = _obj.transform.parent.GetComponent<IDamageable>();
		}
		if (component != null && WeaponManager.sharedManager.myPlayer != null && component.IsEnemyTo(WeaponManager.sharedManager.myPlayerMoveC))
		{
			component.ApplyDamage(this.damageMulty, this, Player_move_c.TypeKills.turret, WeaponSounds.TypeDead.angel, this.gadgetName, WeaponManager.sharedManager.myPlayer.GetComponent<PixelView>().viewID);
			return true;
		}
		return false;
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x001C4220 File Offset: 0x001C2420
	public virtual void StartTurret()
	{
		if (Defs.isMulti && this.isMine)
		{
			if (!Defs.isInet)
			{
				this._networkView.RPC("StartTurretRPC", RPCMode.AllBuffered, new object[0]);
			}
			else
			{
				this.photonView.RPC("StartTurretRPC", PhotonTargets.AllBuffered, new object[0]);
			}
		}
		else if (!Defs.isMulti)
		{
			this.StartTurretRPC();
		}
		this.myCollider.enabled = true;
		this.rigidBody.isKinematic = false;
		this.rigidBody.useGravity = true;
		base.Invoke("SetNoUseGravityInvoke", 5f);
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x001C42C8 File Offset: 0x001C24C8
	[Obfuscation(Exclude = true)]
	private void SetNoUseGravityInvoke()
	{
		this.rigidBody.useGravity = false;
		this.rigidBody.isKinematic = true;
		this.myCollider.isTrigger = !this.solidWithEnemiesOnly;
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x001C4304 File Offset: 0x001C2504
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (this.isMine)
		{
			this.PlayerConnectedLocal(player);
		}
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x001C4318 File Offset: 0x001C2518
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.isMine)
		{
			this.PlayerConnectedPhoton(player);
		}
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x001C432C File Offset: 0x001C252C
	protected virtual void PlayerConnectedLocal(NetworkPlayer player)
	{
		this._networkView.RPC("SynchHealth", player, new object[]
		{
			this.health
		});
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x001C4354 File Offset: 0x001C2554
	protected virtual void PlayerConnectedPhoton(PhotonPlayer player)
	{
		this.photonView.RPC("SynchHealth", player, new object[]
		{
			this.health
		});
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x001C437C File Offset: 0x001C257C
	[RPC]
	[PunRPC]
	public void SynchHealth(float _health)
	{
		if (this.health > _health)
		{
			this.health = _health;
		}
	}

	// Token: 0x06004DF4 RID: 19956 RVA: 0x001C4394 File Offset: 0x001C2594
	protected IEnumerator FlashRed()
	{
		this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		yield return null;
		this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 0f, 0f, 1f));
		yield return new WaitForSeconds(0.1f);
		this.turretRenderer.material.SetColor("_ColorRili", new Color(1f, 1f, 1f, 1f));
		yield break;
	}

	// Token: 0x06004DF5 RID: 19957 RVA: 0x001C43B0 File Offset: 0x001C25B0
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		this.ApplyDamage(damage, damageFrom, Player_move_c.TypeKills.none, WeaponSounds.TypeDead.angel, string.Empty, 0);
	}

	// Token: 0x06004DF6 RID: 19958 RVA: 0x001C43C4 File Offset: 0x001C25C4
	public virtual void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
	{
		this.MinusLive(damage, killerId);
	}

	// Token: 0x06004DF7 RID: 19959 RVA: 0x001C43D0 File Offset: 0x001C25D0
	public virtual bool IsEnemyTo(Player_move_c player)
	{
		return Defs.isMulti && !Defs.isCOOP && !this.myPlayerMoveC.Equals(player) && (!ConnectSceneNGUIController.isTeamRegim || this.myPlayerMoveC.myCommand != player.myCommand);
	}

	// Token: 0x06004DF8 RID: 19960 RVA: 0x001C4428 File Offset: 0x001C2628
	public bool IsDead()
	{
		return this.isKilled;
	}

	// Token: 0x17000CBB RID: 3259
	// (get) Token: 0x06004DF9 RID: 19961 RVA: 0x001C4430 File Offset: 0x001C2630
	public bool isLivingTarget
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004DFA RID: 19962 RVA: 0x001C4434 File Offset: 0x001C2634
	public void MinusLive(float dm, int idKiller = 0)
	{
		this.MinusLive(dm, true, idKiller);
	}

	// Token: 0x06004DFB RID: 19963 RVA: 0x001C4440 File Offset: 0x001C2640
	public void MinusLive(float dm, bool isExplosion, int idKiller = 0)
	{
		if (Defs.isDaterRegim || !this.isRun)
		{
			return;
		}
		isExplosion = true;
		if (Defs.isMulti)
		{
			this.health -= dm;
			if (this.health < 0f)
			{
				this.ImKilledRPCWithExplosion(isExplosion);
				dm = 10000f;
			}
			if (!Defs.isInet)
			{
				this._networkView.RPC("MinusLiveRPC", RPCMode.All, new object[]
				{
					dm,
					isExplosion,
					idKiller
				});
			}
			else
			{
				this.photonView.RPC("MinusLiveRPC", PhotonTargets.All, new object[]
				{
					dm,
					isExplosion,
					idKiller
				});
			}
		}
		else
		{
			this.MinusLiveReal(dm, isExplosion, 0);
		}
	}

	// Token: 0x06004DFC RID: 19964 RVA: 0x001C4520 File Offset: 0x001C2720
	[RPC]
	[PunRPC]
	public void MinusLiveRPC(float dm, int idKiller)
	{
		this.MinusLiveReal(dm, true, idKiller);
	}

	// Token: 0x06004DFD RID: 19965 RVA: 0x001C452C File Offset: 0x001C272C
	[RPC]
	[PunRPC]
	public void MinusLiveRPC(float dm, bool isExplosion, int idKiller)
	{
		this.MinusLiveReal(dm, isExplosion, idKiller);
	}

	// Token: 0x06004DFE RID: 19966 RVA: 0x001C4538 File Offset: 0x001C2738
	public void MinusLiveReal(float dm, bool isExplosion, int idKiller = 0)
	{
		base.StopCoroutine(this.FlashRed());
		base.StartCoroutine(this.FlashRed());
		if (Defs.isSoundFX && isExplosion)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.hitSound);
		}
		if (this.isKilled || (Defs.isMulti && !this.isMine))
		{
			return;
		}
		this.health -= dm;
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				this._networkView.RPC("SynchHealth", RPCMode.Others, new object[]
				{
					this.health
				});
			}
			else
			{
				this.photonView.RPC("SynchHealth", PhotonTargets.Others, new object[]
				{
					this.health
				});
			}
		}
		if (this.health < 0f)
		{
			this.health = 0f;
			if (Defs.isMulti)
			{
				if (!Defs.isInet)
				{
					this._networkView.RPC("ImKilledRPCWithExplosion", RPCMode.AllBuffered, new object[]
					{
						isExplosion
					});
					this._networkView.RPC("MeKillRPC", RPCMode.All, new object[]
					{
						idKiller
					});
				}
				else
				{
					this.photonView.RPC("ImKilledRPCWithExplosion", PhotonTargets.AllBuffered, new object[]
					{
						isExplosion
					});
					this.photonView.RPC("MeKillRPC", PhotonTargets.All, new object[]
					{
						idKiller
					});
				}
			}
			else
			{
				this.ImKilledRPCWithExplosion(isExplosion);
			}
		}
	}

	// Token: 0x06004DFF RID: 19967 RVA: 0x001C46D4 File Offset: 0x001C28D4
	[RPC]
	[PunRPC]
	public void MeKillRPC(int idKiller)
	{
		string nick = string.Empty;
		foreach (Player_move_c player_move_c in Initializer.players)
		{
			if (player_move_c.mySkinName.pixelView != null && player_move_c.mySkinName.pixelView.viewID == idKiller)
			{
				nick = player_move_c.mySkinName.NickName;
				if (player_move_c.Equals(WeaponManager.sharedManager.myPlayerMoveC))
				{
					WeaponManager.sharedManager.myPlayerMoveC.ImKill(idKiller, 9);
					if (this.turretType == TurretController.TurretType.ShieldWall)
					{
						WeaponManager.sharedManager.myPlayerMoveC.myScoreController.AddScoreOnEvent(PlayerEventScoreController.ScoreEvent.barrierBreaker, 1f);
					}
				}
				break;
			}
		}
		this.MeKill(nick);
	}

	// Token: 0x06004E00 RID: 19968 RVA: 0x001C47CC File Offset: 0x001C29CC
	public void MeKill(string _nick)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null && this.myPlayer != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.AddSystemMessage(_nick, 9, this.myPlayer.GetComponent<SkinName>().NickName, Color.white, null);
		}
	}

	// Token: 0x06004E01 RID: 19969 RVA: 0x001C4828 File Offset: 0x001C2A28
	public void SendImKilledRPC()
	{
		if (Defs.isMulti)
		{
			if (!Defs.isInet)
			{
				this._networkView.RPC("ImKilledRPC", RPCMode.AllBuffered, new object[0]);
			}
			else
			{
				this.photonView.RPC("ImKilledRPC", PhotonTargets.AllBuffered, new object[0]);
			}
		}
		else
		{
			this.ImKilledRPC();
		}
	}

	// Token: 0x06004E02 RID: 19970 RVA: 0x001C4888 File Offset: 0x001C2A88
	[PunRPC]
	[RPC]
	public void ImKilledRPC()
	{
		this.ImKilledRPCWithExplosion(false);
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001C4894 File Offset: 0x001C2A94
	[RPC]
	[PunRPC]
	public void ImKilledRPCWithExplosion(bool isExplosion)
	{
		if (this.isKilled)
		{
			return;
		}
		this.isKilled = true;
		if (this.nickLabel != null)
		{
			this.nickLabel.gameObject.SetActive(false);
		}
		this.myCollider.enabled = false;
		this.rigidBody.isKinematic = true;
		if (Defs.isSoundFX)
		{
			if (isExplosion)
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.turretDeadSound);
			}
			else
			{
				base.GetComponent<AudioSource>().PlayOneShot(this.turretDeactivationSound);
			}
		}
		if (isExplosion)
		{
			if (this.explosionAnimObj != null)
			{
				this.explosionAnimObj.SetActive(true);
			}
			if (this.animation != null)
			{
				this.animation.Play("turret_dead");
			}
			if (this.GadgetOnKill != null)
			{
				this.GadgetOnKill();
			}
		}
		else if (this.animation != null)
		{
			this.animation.Play("turret_stop");
		}
		if (this.workingSound != null)
		{
			this.workingSound.SetActive(false);
		}
		if (this.workingParticle != null)
		{
			this.workingParticle.SetActive(false);
		}
		base.Invoke("DestroyTurrel", this.destroyTurretTime);
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x001C49F4 File Offset: 0x001C2BF4
	[Obfuscation(Exclude = true)]
	private void DestroyTurrel()
	{
		if (Defs.isMulti)
		{
			if (this.isMine)
			{
				if (!Defs.isInet)
				{
					Network.RemoveRPCs(base.GetComponent<NetworkView>().viewID);
					Network.Destroy(base.gameObject);
				}
				else
				{
					PhotonNetwork.Destroy(base.gameObject);
				}
			}
			else
			{
				base.transform.position = new Vector3(-1000f, -1000f, -1000f);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x001C4A80 File Offset: 0x001C2C80
	[PunRPC]
	[RPC]
	public void StartTurretRPC()
	{
		if (this.nickLabel != null)
		{
			this.nickLabel.gameObject.SetActive(true);
		}
		this.myCollider.enabled = true;
		base.transform.parent = null;
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.turretActivSound);
		}
		Player_move_c.SetLayerRecursively(base.gameObject, LayerMask.NameToLayer("Default"));
		if (Defs.isInet)
		{
			this.photonView.synchronization = ViewSynchronization.UnreliableOnChange;
		}
		else
		{
			this._networkView.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
		}
		this.isRun = true;
		if (Defs.isSoundFX && this.workingSound != null)
		{
			this.workingSound.SetActive(true);
		}
		if (this.workingParticle != null)
		{
			this.workingParticle.SetActive(true);
		}
		if (this.animation != null)
		{
			this.animation.Play("turret_start");
		}
		this.UpdateMaterial();
		if (this.solidWithEnemiesOnly && (!Defs.isMulti || this.isMine || Defs.isCOOP || (ConnectSceneNGUIController.isTeamRegim && this.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC != null && this.myPlayerMoveC.myCommand == WeaponManager.sharedManager.myPlayerMoveC.myCommand)))
		{
			base.gameObject.layer = LayerMask.NameToLayer("IgnoreRocketsAndBullets");
		}
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001C4C20 File Offset: 0x001C2E20
	private void UpdateMaterial()
	{
		if (this.isRun)
		{
			this.turretRenderer.sharedMaterials = ((!this.setMaterialsForEnemy || !this.isEnemyTurret) ? this.originalMaterials : this.enemyMaterials);
		}
		else
		{
			Material[] array = new Material[this.originalMaterials.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.inactiveMaterial;
			}
			this.turretRenderer.sharedMaterials = array;
		}
	}

	// Token: 0x06004E07 RID: 19975 RVA: 0x001C4CA8 File Offset: 0x001C2EA8
	private void OnDestroy()
	{
		if (!Defs.isMulti || this.isMine)
		{
			PotionsController.sharedController.DeActivePotion(GearManager.Turret, null, false);
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
		if (this is VoodooSnowman)
		{
			if (!Defs.isMulti || this.isMine)
			{
				Initializer.damageableObjects.Remove(base.gameObject);
			}
		}
		else
		{
			Initializer.turretsObj.Remove(base.gameObject);
		}
	}

	// Token: 0x06004E08 RID: 19976 RVA: 0x001C4D30 File Offset: 0x001C2F30
	public Vector3 GetHeadPoint()
	{
		Vector3 position = base.transform.position;
		position.y += this.myCollider.size.y - 0.5f;
		return position;
	}

	// Token: 0x06004E09 RID: 19977 RVA: 0x001C4D74 File Offset: 0x001C2F74
	public void SendNetworkViewMyPlayer(NetworkViewID myId)
	{
		this._networkView.RPC("SendNetworkViewMyPlayerRPC", RPCMode.AllBuffered, new object[]
		{
			myId
		});
	}

	// Token: 0x06004E0A RID: 19978 RVA: 0x001C4DA4 File Offset: 0x001C2FA4
	[RPC]
	[PunRPC]
	public void SendNetworkViewMyPlayerRPC(NetworkViewID myId)
	{
		for (int i = 0; i < Initializer.players.Count; i++)
		{
			if (myId.Equals(Initializer.players[i].mySkinName.GetComponent<NetworkView>().viewID))
			{
				this.myPlayer = Initializer.players[i].mySkinName.gameObject;
				this.myPlayerMoveC = this.myPlayer.GetComponent<SkinName>().playerMoveC;
				break;
			}
		}
	}

	// Token: 0x04003C8D RID: 15501
	[Header("Damage settings")]
	public float damageMulty = 1f;

	// Token: 0x04003C8E RID: 15502
	public float damageSurvival = 30f;

	// Token: 0x04003C8F RID: 15503
	public float healthSurvival = 18f;

	// Token: 0x04003C90 RID: 15504
	public float maxRadiusScanTarget = 30f;

	// Token: 0x04003C91 RID: 15505
	[Header("Main sound settings")]
	public AudioClip turretActivSound;

	// Token: 0x04003C92 RID: 15506
	public AudioClip turretDeadSound;

	// Token: 0x04003C93 RID: 15507
	public AudioClip turretDeactivationSound;

	// Token: 0x04003C94 RID: 15508
	public AudioClip hitSound;

	// Token: 0x04003C95 RID: 15509
	[Header("Other")]
	public TurretController.TurretType turretType;

	// Token: 0x04003C96 RID: 15510
	public Renderer turretRenderer;

	// Token: 0x04003C97 RID: 15511
	public GameObject explosionAnimObj;

	// Token: 0x04003C98 RID: 15512
	public GameObject isEnemySprite;

	// Token: 0x04003C99 RID: 15513
	public Transform healthBar;

	// Token: 0x04003C9A RID: 15514
	public Transform spherePoint;

	// Token: 0x04003C9B RID: 15515
	public Transform rayGroundPoint;

	// Token: 0x04003C9C RID: 15516
	public TextMesh nickLabel;

	// Token: 0x04003C9D RID: 15517
	public Material inactiveMaterial;

	// Token: 0x04003C9E RID: 15518
	public Animation animation;

	// Token: 0x04003C9F RID: 15519
	public bool solidWithEnemiesOnly;

	// Token: 0x04003CA0 RID: 15520
	public bool setMaterialsForEnemy;

	// Token: 0x04003CA1 RID: 15521
	public Material[] enemyMaterials;

	// Token: 0x04003CA2 RID: 15522
	public GameObject workingSound;

	// Token: 0x04003CA3 RID: 15523
	public GameObject workingParticle;

	// Token: 0x04003CA4 RID: 15524
	[HideInInspector]
	public bool isRun;

	// Token: 0x04003CA5 RID: 15525
	protected bool isReady;

	// Token: 0x04003CA6 RID: 15526
	[HideInInspector]
	public bool isKilled;

	// Token: 0x04003CA7 RID: 15527
	[HideInInspector]
	public int numUpdate;

	// Token: 0x04003CA8 RID: 15528
	[HideInInspector]
	public float health = 10000000f;

	// Token: 0x04003CA9 RID: 15529
	[HideInInspector]
	public float maxHealth = 10000000f;

	// Token: 0x04003CAA RID: 15530
	[HideInInspector]
	public GameObject myPlayer;

	// Token: 0x04003CAB RID: 15531
	[HideInInspector]
	public Player_move_c myPlayerMoveC;

	// Token: 0x04003CAC RID: 15532
	[HideInInspector]
	public PhotonView photonView;

	// Token: 0x04003CAD RID: 15533
	[HideInInspector]
	public NetworkView _networkView;

	// Token: 0x04003CAE RID: 15534
	[HideInInspector]
	public bool isEnemyTurret;

	// Token: 0x04003CAF RID: 15535
	protected float maxRadiusScanTargetSQR;

	// Token: 0x04003CB0 RID: 15536
	protected bool isMine;

	// Token: 0x04003CB1 RID: 15537
	protected bool inScaning;

	// Token: 0x04003CB2 RID: 15538
	protected float maxTimerShot = 0.1f;

	// Token: 0x04003CB3 RID: 15539
	protected Transform target;

	// Token: 0x04003CB4 RID: 15540
	private float timerScanTargetIdle = -1f;

	// Token: 0x04003CB5 RID: 15541
	private float maxTimerScanTargetIdle = 0.5f;

	// Token: 0x04003CB6 RID: 15542
	private Rigidbody rigidBody;

	// Token: 0x04003CB7 RID: 15543
	private Vector3 turretMinPos = new Vector3(0f, float.MaxValue, 0f);

	// Token: 0x04003CB8 RID: 15544
	private int _nickColorInd;

	// Token: 0x04003CB9 RID: 15545
	private bool _isSetNickLabelText;

	// Token: 0x04003CBA RID: 15546
	private bool wasStickedToPlayer;

	// Token: 0x04003CBB RID: 15547
	private BoxCollider myCollider;

	// Token: 0x04003CBC RID: 15548
	private Material[] originalMaterials;

	// Token: 0x04003CBD RID: 15549
	private bool isSetAsEnemy;

	// Token: 0x04003CBE RID: 15550
	public Action GadgetOnKill;

	// Token: 0x04003CBF RID: 15551
	public bool validatePlaceWithSphere = true;

	// Token: 0x04003CC0 RID: 15552
	public float destroyTurretTime = 2f;

	// Token: 0x04003CC1 RID: 15553
	protected string gadgetName;

	// Token: 0x0200086D RID: 2157
	public enum TurretType
	{
		// Token: 0x04003CC3 RID: 15555
		AttackTurret,
		// Token: 0x04003CC4 RID: 15556
		ShieldWall
	}
}
