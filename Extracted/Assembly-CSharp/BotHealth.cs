using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public sealed class BotHealth : MonoBehaviour
{
	// Token: 0x0600309F RID: 12447 RVA: 0x000FD04C File Offset: 0x000FB24C
	private IEnumerator resetHurtAudio(float tm)
	{
		BotHealth._hurtAudioIsPlaying = true;
		yield return new WaitForSeconds(tm);
		BotHealth._hurtAudioIsPlaying = false;
		yield break;
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x000FD070 File Offset: 0x000FB270
	public bool RequestPlayHurt(float tm)
	{
		if (BotHealth._hurtAudioIsPlaying)
		{
			return false;
		}
		base.StartCoroutine(this.resetHurtAudio(tm));
		return true;
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000FD090 File Offset: 0x000FB290
	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000FD0A4 File Offset: 0x000FB2A4
	private void Start()
	{
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				this._modelChild = transform.gameObject;
			}
		}
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		if (Defs.IsSurvival && TrainingController.TrainingCompleted)
		{
			if (ZombieCreator.sharedCreator.currentWave == 0)
			{
				this._soundClips.notAttackingSpeed *= 0.75f;
				this._soundClips.attackingSpeed *= 0.75f;
				this._soundClips.health *= 0.7f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 1)
			{
				this._soundClips.notAttackingSpeed *= 0.85f;
				this._soundClips.attackingSpeed *= 0.85f;
				this._soundClips.health *= 0.8f;
			}
			if (ZombieCreator.sharedCreator.currentWave == 2)
			{
				this._soundClips.notAttackingSpeed *= 0.9f;
				this._soundClips.attackingSpeed *= 0.9f;
				this._soundClips.health *= 0.9f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 7)
			{
				this._soundClips.notAttackingSpeed *= 1.25f;
				this._soundClips.attackingSpeed *= 1.25f;
			}
			if (ZombieCreator.sharedCreator.currentWave >= 9)
			{
				this._soundClips.health *= 1.25f;
			}
		}
		this.ai = base.GetComponent<BotAI>();
		this.healthDown = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		if (base.gameObject.name.IndexOf("Boss") == -1)
		{
			this._skin = BotHealth.SetSkinForObj(this._modelChild);
		}
		else
		{
			Renderer componentInChildren = this._modelChild.GetComponentInChildren<Renderer>();
			this._skin = componentInChildren.material.mainTexture;
		}
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x000FD318 File Offset: 0x000FB518
	public static Texture SetSkinForObj(GameObject go)
	{
		if (!BotHealth._skinsManager)
		{
			BotHealth._skinsManager = GameObject.FindGameObjectWithTag("SkinsManager").GetComponent<SkinsManagerPixlGun>();
		}
		string text = BotHealth.SkinNameForObj(go.name);
		Texture texture;
		if (!(texture = (BotHealth._skinsManager.skins[text] as Texture)))
		{
			Debug.Log("No skin: " + text);
		}
		BotHealth.SetTextureRecursivelyFrom(go, texture);
		return texture;
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x000FD390 File Offset: 0x000FB590
	public static string SkinNameForObj(string objName)
	{
		return (!Defs.IsSurvival) ? (TrainingController.TrainingCompleted ? (objName + "_Level" + CurrentCampaignGame.currentLevel) : (objName + "_Level3")) : objName;
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x000FD3DC File Offset: 0x000FB5DC
	public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt)
	{
		foreach (object obj2 in obj.transform)
		{
			Transform transform = (Transform)obj2;
			if (!transform.name.Equals("Ears"))
			{
				if (transform.gameObject.GetComponent<Renderer>() && transform.gameObject.GetComponent<Renderer>().material)
				{
					transform.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
				}
				BotHealth.SetTextureRecursivelyFrom(transform.gameObject, txt);
			}
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000FD4B0 File Offset: 0x000FB6B0
	private IEnumerator Flash()
	{
		this._flashing = true;
		BotHealth.SetTextureRecursivelyFrom(this._modelChild, this.hitTexture);
		yield return new WaitForSeconds(0.125f);
		BotHealth.SetTextureRecursivelyFrom(this._modelChild, this._skin);
		this._flashing = false;
		yield break;
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x000FD4CC File Offset: 0x000FB6CC
	private void _CreateBonusWeapon()
	{
		if (LevelBox.weaponsFromBosses.ContainsKey(Application.loadedLevelName) && base.gameObject.name.Contains("Boss") && !this._weaponCreated)
		{
			string weaponName = LevelBox.weaponsFromBosses[Application.loadedLevelName];
			Vector3 pos = base.gameObject.transform.position + new Vector3(0f, 0.25f, 0f);
			GameObject gameObject = (!(base.GetComponent<BotMovement>()._gameController.weaponBonus != null)) ? BonusCreator._CreateBonus(weaponName, pos) : BonusCreator._CreateBonusFromPrefab(base.GetComponent<BotMovement>()._gameController.weaponBonus, pos);
			gameObject.AddComponent<GotToNextLevel>();
			base.GetComponent<BotMovement>()._gameController.weaponBonus = null;
			this._weaponCreated = true;
		}
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x000FD5AC File Offset: 0x000FB7AC
	public void adjustHealth(float _health, Transform target)
	{
		if (_health < 0f && !this._flashing)
		{
			base.StartCoroutine(this.Flash());
		}
		this._soundClips.health += _health;
		if (this._soundClips.health < 0f)
		{
			this._soundClips.health = 0f;
		}
		if (Debug.isDebugBuild)
		{
			this._CreateBonusWeapon();
			this.IsLife = false;
		}
		else if (this._soundClips.health == 0f)
		{
			this._CreateBonusWeapon();
			this.IsLife = false;
		}
		else
		{
			GlobalGameController.Score += 5;
		}
		if (this.RequestPlayHurt(this._soundClips.hurt.length) && Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this._soundClips.hurt);
		}
		if ((target.CompareTag("Player") && !target.GetComponent<SkinName>().playerMoveC.isInvisible) || target.CompareTag("Turret"))
		{
			this.ai.SetTarget(target, true);
		}
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x000FD6E0 File Offset: 0x000FB8E0
	public bool getIsLife()
	{
		return this.IsLife;
	}

	// Token: 0x040023B5 RID: 9141
	public static bool _hurtAudioIsPlaying;

	// Token: 0x040023B6 RID: 9142
	private static SkinsManagerPixlGun _skinsManager;

	// Token: 0x040023B7 RID: 9143
	public string myName = "Bot";

	// Token: 0x040023B8 RID: 9144
	private bool IsLife = true;

	// Token: 0x040023B9 RID: 9145
	public Texture hitTexture;

	// Token: 0x040023BA RID: 9146
	private BotAI ai;

	// Token: 0x040023BB RID: 9147
	private Player_move_c healthDown;

	// Token: 0x040023BC RID: 9148
	private bool _flashing;

	// Token: 0x040023BD RID: 9149
	private GameObject _modelChild;

	// Token: 0x040023BE RID: 9150
	private Sounds _soundClips;

	// Token: 0x040023BF RID: 9151
	private Texture _skin;

	// Token: 0x040023C0 RID: 9152
	private bool _weaponCreated;
}
