using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A7 RID: 1447
public sealed class ZombiUpravlenie : MonoBehaviour
{
	// Token: 0x0600322D RID: 12845 RVA: 0x00104A58 File Offset: 0x00102C58
	private IEnumerator resetDeathAudio(float tm)
	{
		ZombiUpravlenie._deathAudioPlaying = true;
		yield return new WaitForSeconds(tm);
		ZombiUpravlenie._deathAudioPlaying = false;
		yield break;
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x00104A7C File Offset: 0x00102C7C
	public bool RequestPlayDeath(float tm)
	{
		if (ZombiUpravlenie._deathAudioPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetDeathAudio(tm));
		return true;
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x00104A9C File Offset: 0x00102C9C
	private void Awake()
	{
		try
		{
			this._modelChild = base.transform.GetChild(0).gameObject;
			this.health = this._modelChild.GetComponent<Sounds>().health;
			if (Defs.isMulti && !Defs.isCOOP)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!Defs.isCOOP)
			{
				base.enabled = false;
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x00104B40 File Offset: 0x00102D40
	[RPC]
	[PunRPC]
	private void setHealthRPC(float _health)
	{
		this.health = _health;
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x00104B4C File Offset: 0x00102D4C
	[PunRPC]
	[RPC]
	private void flashRPC()
	{
		base.StartCoroutine(this.Flash());
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x00104B5C File Offset: 0x00102D5C
	[PunRPC]
	[RPC]
	public void SlowdownRPC(float coef)
	{
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x00104B60 File Offset: 0x00102D60
	public void setHealth(float _health, bool isFlash)
	{
		this.photonView.RPC("setHealthRPC", PhotonTargets.All, new object[]
		{
			_health
		});
		if (isFlash && !this._flashing)
		{
			base.StartCoroutine(this.Flash());
			this.photonView.RPC("flashRPC", PhotonTargets.Others, new object[0]);
		}
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x00104BC4 File Offset: 0x00102DC4
	public static Texture SetSkinForObj(GameObject go)
	{
		if (!ZombiUpravlenie._skinsManager)
		{
			ZombiUpravlenie._skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		string text = ZombiUpravlenie.SkinNameForObj(go.name);
		Texture texture;
		if (!(texture = (ZombiUpravlenie._skinsManager.skins[text] as Texture)))
		{
			Debug.Log("No skin: " + text);
		}
		BotHealth.SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x00104C3C File Offset: 0x00102E3C
	public static string SkinNameForObj(string objName)
	{
		return objName;
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x00104C40 File Offset: 0x00102E40
	private IEnumerator Flash()
	{
		this._flashing = true;
		BotHealth.SetTextureRecursivelyFrom(this._modelChild, this.hitTexture);
		yield return new WaitForSeconds(0.125f);
		BotHealth.SetTextureRecursivelyFrom(this._modelChild, this._skin);
		this._flashing = false;
		yield break;
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x00104C5C File Offset: 0x00102E5C
	private void Start()
	{
		try
		{
			this._skin = ZombiUpravlenie.SetSkinForObj(this._modelChild);
			this._nma = base.GetComponent<NavMeshAgent>();
			this._modelChildCollider = this._modelChild.GetComponent<BoxCollider>();
			this.shootAnim = this.offAnim;
			this.player = GameObject.FindGameObjectWithTag("Player");
			this._gameController = GameObject.FindGameObjectWithTag("ZombiCreator").GetComponent<ZombiManager>();
			this._soundClips = this._modelChild.GetComponent<Sounds>();
			this.CurLifeTime = this._soundClips.timeToHit;
			this.target = null;
			this._modelChild.GetComponent<Animation>().Stop();
			this.Walk();
			this._soundClips.attackingSpeed += UnityEngine.Random.Range(-this._soundClips.attackingSpeedRandomRange[0], this._soundClips.attackingSpeedRandomRange[1]);
			this.photonView = PhotonView.Get(this);
			this._skin = ZombiUpravlenie.SetSkinForObj(this._modelChild);
			if (this.photonView.isMine)
			{
				this.photonView.RPC("setHealthRPC", PhotonTargets.All, new object[]
				{
					this._soundClips.health
				});
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x00104DCC File Offset: 0x00102FCC
	public void setId(int _id)
	{
		this.photonView.RPC("setIdRPC", PhotonTargets.All, new object[]
		{
			_id
		});
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x00104DFC File Offset: 0x00102FFC
	[PunRPC]
	[RPC]
	public void setIdRPC(int _id)
	{
		base.GetComponent<PhotonView>().viewID = _id;
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x00104E0C File Offset: 0x0010300C
	private void Update()
	{
		try
		{
			if (!ZombiManager.sharedManager.startGame)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (this.photonView.isMine)
			{
				if (!this.deaded)
				{
					if (this.target != null && this.target.CompareTag("Player") && this.target.GetComponent<SkinName>().playerMoveC.isInvisible)
					{
						this.target = null;
					}
					if (this.target != null && this.timeToUpdateTarget > 0f)
					{
						this.timeToUpdateTarget -= Time.deltaTime;
						float num = Vector3.SqrMagnitude(this.target.position - base.transform.position);
						Vector3 vector = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
						if (num >= this._soundClips.attackDistance * this._soundClips.attackDistance)
						{
							this.timeToUpdateNavMesh -= Time.deltaTime;
							if (this.timeToUpdateNavMesh < 0f)
							{
								this._nma.SetDestination(vector);
								this._nma.speed = this._soundClips.attackingSpeed * Mathf.Pow(1.05f, (float)GlobalGameController.AllLevelsCompleted);
								this.timeToUpdateNavMesh = 0.5f;
							}
							this.CurLifeTime = this._soundClips.timeToHit;
							this.PlayZombieRun();
						}
						else
						{
							if (this._nma.path != null)
							{
								this._nma.ResetPath();
							}
							this.CurLifeTime -= Time.deltaTime;
							base.transform.LookAt(vector);
							if (this.CurLifeTime <= 0f)
							{
								this.CurLifeTime = this._soundClips.timeToHit;
								if (Defs.isSoundFX)
								{
									base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.bite);
								}
								if (this.target.CompareTag("Player"))
								{
								}
								if (this.target.CompareTag("Turret"))
								{
									this.target.GetComponent<TurretController>().MinusLive((float)this._soundClips.damagePerHit, 0);
								}
							}
							this.PlayZombieAttack();
						}
					}
					else
					{
						this.timeToResetPath -= Time.deltaTime;
						if (this.timeToResetPath <= 0f)
						{
							this.timeToResetPath = 5f;
							this._nma.ResetPath();
							Vector3 vector2 = new Vector3((float)(-20 + UnityEngine.Random.Range(0, 40)), base.transform.position.y, (float)(-20 + UnityEngine.Random.Range(0, 40)));
							base.transform.LookAt(vector2);
							this._nma.SetDestination(vector2);
							this._nma.speed = this._soundClips.notAttackingSpeed;
						}
						GameObject[] array = GameObject.FindGameObjectsWithTag("Turret");
						if (Initializer.players.Count > 0)
						{
							this.timeToUpdateTarget = 5f;
							float num2 = Vector3.SqrMagnitude(base.transform.position - Initializer.players[0].myPlayerTransform.position);
							this.target = Initializer.players[0].myPlayerTransform;
							foreach (Player_move_c player_move_c in Initializer.players)
							{
								if (!player_move_c.isInvisible)
								{
									float num3 = Vector3.SqrMagnitude(base.transform.position - player_move_c.myPlayerTransform.position);
									if (num3 < num2)
									{
										num2 = num3;
										this.target = player_move_c.myPlayerTransform;
									}
								}
							}
							foreach (GameObject gameObject in array)
							{
								if (gameObject.GetComponent<TurretController>().isRun)
								{
									float num4 = Vector3.SqrMagnitude(base.transform.position - gameObject.transform.position);
									if (num4 < num2)
									{
										num2 = num4;
										this.target = gameObject.transform;
									}
								}
							}
						}
					}
					if (this.health <= 0f)
					{
						this.photonView.RPC("Death", PhotonTargets.All, new object[0]);
					}
				}
				else if (this.falling)
				{
					float num5 = 7f;
					base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - num5 * Time.deltaTime, base.transform.position.z);
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogError("Cooperative mode failure.");
			Debug.LogException(exception);
			throw;
		}
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x00105380 File Offset: 0x00103580
	[PunRPC]
	[RPC]
	private void Death()
	{
		if (!Defs.isCOOP)
		{
			return;
		}
		if (this._nma != null)
		{
			this._nma.enabled = false;
		}
		float num = 0.1f;
		if (Defs.isSoundFX && this._soundClips != null)
		{
			if (this.RequestPlayDeath(this._soundClips.death.length))
			{
				base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.death);
			}
			num = this._soundClips.death.length;
		}
		this._modelChild.GetComponent<Animation>().Stop();
		if (this._modelChild.GetComponent<Animation>()[this.deathAnim])
		{
			this._modelChild.GetComponent<Animation>().Play(this.deathAnim);
			num = Mathf.Max(num, this._modelChild.GetComponent<Animation>()[this.deathAnim].length);
			this.CodeAfterDelay(this._modelChild.GetComponent<Animation>()[this.deathAnim].length * 1.25f, new ZombiUpravlenie.DelayedCallback(this.StartFall));
		}
		else
		{
			this.StartFall();
		}
		this.CodeAfterDelay(num, new ZombiUpravlenie.DelayedCallback(this.DestroySelf));
		this._modelChild.GetComponent<BoxCollider>().enabled = false;
		this.deaded = true;
		SpawnMonster component = base.GetComponent<SpawnMonster>();
		component.ShouldMove = false;
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x001054F8 File Offset: 0x001036F8
	private void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x00105508 File Offset: 0x00103708
	private void StartFall()
	{
		this.falling = true;
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x00105514 File Offset: 0x00103714
	private void Walk()
	{
		this._modelChild.GetComponent<Animation>().Stop();
		if (this._modelChild.GetComponent<Animation>()[this.normWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.normWalkAnim);
		}
		else
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x00105584 File Offset: 0x00103784
	public void PlayZombieRun()
	{
		if (this.tekAnim != 1 && Defs.isCOOP)
		{
			if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
			}
			this.photonView.RPC("PlayZombieRunRPC", PhotonTargets.Others, new object[0]);
		}
		this.tekAnim = 1;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x001055FC File Offset: 0x001037FC
	public void PlayZombieAttack()
	{
		if (this.tekAnim != 2 && Defs.isCOOP)
		{
			if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
			}
			else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
			{
				this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
			}
			this.photonView.RPC("PlayZombieAttackRPC", PhotonTargets.Others, new object[0]);
		}
		this.tekAnim = 2;
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x001056B0 File Offset: 0x001038B0
	[PunRPC]
	[RPC]
	public void PlayZombieRunRPC()
	{
		if (this._modelChild.GetComponent<Animation>()[this.zombieWalkAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.zombieWalkAnim);
		}
		this.tekAnim = 1;
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x001056FC File Offset: 0x001038FC
	[RPC]
	[PunRPC]
	public void PlayZombieAttackRPC()
	{
		if (this._modelChild.GetComponent<Animation>()[this.attackAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.attackAnim);
		}
		else if (this._modelChild.GetComponent<Animation>()[this.shootAnim])
		{
			this._modelChild.GetComponent<Animation>().CrossFade(this.shootAnim);
		}
		this.tekAnim = 2;
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x00105784 File Offset: 0x00103984
	public void CodeAfterDelay(float delay, ZombiUpravlenie.DelayedCallback callback)
	{
		base.StartCoroutine(this.___DelayedCallback(delay, callback));
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x00105798 File Offset: 0x00103998
	private IEnumerator ___DelayedCallback(float time, ZombiUpravlenie.DelayedCallback callback)
	{
		yield return new WaitForSeconds(time);
		callback();
		yield break;
	}

	// Token: 0x040024ED RID: 9453
	public static bool _deathAudioPlaying;

	// Token: 0x040024EE RID: 9454
	public GameObject playerKill;

	// Token: 0x040024EF RID: 9455
	private Player_move_c healthDown;

	// Token: 0x040024F0 RID: 9456
	private GameObject player;

	// Token: 0x040024F1 RID: 9457
	private float CurLifeTime;

	// Token: 0x040024F2 RID: 9458
	private string idleAnim = "Idle";

	// Token: 0x040024F3 RID: 9459
	private string normWalkAnim = "Norm_Walk";

	// Token: 0x040024F4 RID: 9460
	private string zombieWalkAnim = "Zombie_Walk";

	// Token: 0x040024F5 RID: 9461
	private string offAnim = "Zombie_Off";

	// Token: 0x040024F6 RID: 9462
	private string deathAnim = "Zombie_Dead";

	// Token: 0x040024F7 RID: 9463
	private string onAnim = "Zombie_On";

	// Token: 0x040024F8 RID: 9464
	private string attackAnim = "Zombie_Attack";

	// Token: 0x040024F9 RID: 9465
	private string shootAnim;

	// Token: 0x040024FA RID: 9466
	private GameObject _modelChild;

	// Token: 0x040024FB RID: 9467
	private Sounds _soundClips;

	// Token: 0x040024FC RID: 9468
	private bool falling;

	// Token: 0x040024FD RID: 9469
	private NavMeshAgent _nma;

	// Token: 0x040024FE RID: 9470
	private BoxCollider _modelChildCollider;

	// Token: 0x040024FF RID: 9471
	private ZombiManager _gameController;

	// Token: 0x04002500 RID: 9472
	public bool deaded;

	// Token: 0x04002501 RID: 9473
	public Transform target;

	// Token: 0x04002502 RID: 9474
	public float health;

	// Token: 0x04002503 RID: 9475
	private PhotonView photonView;

	// Token: 0x04002504 RID: 9476
	public Texture hitTexture;

	// Token: 0x04002505 RID: 9477
	private Texture _skin;

	// Token: 0x04002506 RID: 9478
	private static SkinsManagerPixlGun _skinsManager;

	// Token: 0x04002507 RID: 9479
	private bool _flashing;

	// Token: 0x04002508 RID: 9480
	public int typeZombInMas;

	// Token: 0x04002509 RID: 9481
	private float timeToUpdateTarget = 5f;

	// Token: 0x0400250A RID: 9482
	private float timeToUpdateNavMesh;

	// Token: 0x0400250B RID: 9483
	public int tekAnim = -1;

	// Token: 0x0400250C RID: 9484
	private float timeToResetPath;

	// Token: 0x0200091F RID: 2335
	// (Invoke) Token: 0x0600511C RID: 20764
	public delegate void DelayedCallback();
}
