using System;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class PlayerMechBody : MonoBehaviour
{
	// Token: 0x06002801 RID: 10241 RVA: 0x000C7F64 File Offset: 0x000C6164
	public void ShowActivationEffect()
	{
		this.activationEffect.SetActive(true);
		this.activationEffect.GetComponent<DisableObjectFromTimer>().timer = this.activationEffectTime;
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x000C7F94 File Offset: 0x000C6194
	public void ShowExplosionEffect()
	{
		this.dieTimer = Time.time + this.dieTime;
		this.explosionEffectTimer = Time.time + this.explosionEffectTime;
		this.explosionEffect.SetActive(true);
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x000C7FD4 File Offset: 0x000C61D4
	private void Awake()
	{
		this.defaultBodyTexture = this.bodyRenderer.sharedMaterial.mainTexture;
		this.defaultHandsTexture = this.handsRenderer.sharedMaterial.mainTexture;
	}

	// Token: 0x06002804 RID: 10244 RVA: 0x000C8010 File Offset: 0x000C6210
	public void ShowHitMaterial(bool hit, bool poison = false)
	{
		if (hit)
		{
			this.bodyRenderer.material.mainTexture = ((!poison) ? SkinsController.damageHitTexture : SkinsController.poisonHitTexture);
			this.handsRenderer.material.mainTexture = ((!poison) ? SkinsController.damageHitTexture : SkinsController.poisonHitTexture);
		}
		else
		{
			this.bodyRenderer.material.mainTexture = this.defaultBodyTexture;
			this.handsRenderer.material.mainTexture = this.defaultHandsTexture;
		}
	}

	// Token: 0x06002805 RID: 10245 RVA: 0x000C80A0 File Offset: 0x000C62A0
	private void Update()
	{
		if (this.dieTimer != -1f && this.dieTimer < Time.time)
		{
			this.point.SetActive(false);
			this.dieTimer = -1f;
		}
		if (this.explosionEffectTimer != -1f && this.explosionEffectTimer < Time.time)
		{
			this.explosionEffect.SetActive(false);
			this.explosionEffectTimer = -1f;
		}
	}

	// Token: 0x04001C4A RID: 7242
	public AudioClip activationSound;

	// Token: 0x04001C4B RID: 7243
	public AudioClip stepSound;

	// Token: 0x04001C4C RID: 7244
	public AudioClip shootSound;

	// Token: 0x04001C4D RID: 7245
	public Vector3 cameraPosition = new Vector3(0.12f, 0.7f, -0.3f);

	// Token: 0x04001C4E RID: 7246
	public Vector3 gunCameraPosition = new Vector3(-0.1f, 0f, 0f);

	// Token: 0x04001C4F RID: 7247
	public float gunCameraFieldOfView = 45f;

	// Token: 0x04001C50 RID: 7248
	public float bodyColliderHeight = 2.07f;

	// Token: 0x04001C51 RID: 7249
	public Vector3 bodyColliderCenter = new Vector3(0f, 0.19f, 0f);

	// Token: 0x04001C52 RID: 7250
	public Vector3 headColliderCenter = new Vector3(0f, 0.54f, 0f);

	// Token: 0x04001C53 RID: 7251
	public float nickLabelYPoision = 1.72f;

	// Token: 0x04001C54 RID: 7252
	public AudioSource explosionSound;

	// Token: 0x04001C55 RID: 7253
	public GameObject activationEffect;

	// Token: 0x04001C56 RID: 7254
	public GameObject explosionEffect;

	// Token: 0x04001C57 RID: 7255
	public GameObject jetpackObject;

	// Token: 0x04001C58 RID: 7256
	public GameObject body;

	// Token: 0x04001C59 RID: 7257
	public GameObject gun;

	// Token: 0x04001C5A RID: 7258
	public GameObject point;

	// Token: 0x04001C5B RID: 7259
	public Renderer bodyRenderer;

	// Token: 0x04001C5C RID: 7260
	public Renderer handsRenderer;

	// Token: 0x04001C5D RID: 7261
	public Renderer gunRenderer;

	// Token: 0x04001C5E RID: 7262
	public Animation bodyAnimation;

	// Token: 0x04001C5F RID: 7263
	public Animation gunAnimation;

	// Token: 0x04001C60 RID: 7264
	public WeaponSounds weapon;

	// Token: 0x04001C61 RID: 7265
	public string killChatIcon = "Chat_Mech";

	// Token: 0x04001C62 RID: 7266
	public PlayerEventScoreController.ScoreEvent scoreEventOnKill = PlayerEventScoreController.ScoreEvent.deadMech;

	// Token: 0x04001C63 RID: 7267
	public float dieTime = 0.46f;

	// Token: 0x04001C64 RID: 7268
	public float explosionEffectTime = 1f;

	// Token: 0x04001C65 RID: 7269
	public float activationEffectTime = 2f;

	// Token: 0x04001C66 RID: 7270
	public string[] healthSpriteName = new string[]
	{
		"mech_armor1",
		"mech_armor2",
		"mech_armor3",
		"mech_armor4",
		"mech_armor5",
		"mech_armor6"
	};

	// Token: 0x04001C67 RID: 7271
	public bool dontShowHandsInThirdPerson;

	// Token: 0x04001C68 RID: 7272
	private float dieTimer = -1f;

	// Token: 0x04001C69 RID: 7273
	private float explosionEffectTimer = -1f;

	// Token: 0x04001C6A RID: 7274
	private Texture defaultBodyTexture;

	// Token: 0x04001C6B RID: 7275
	private Texture defaultHandsTexture;
}
