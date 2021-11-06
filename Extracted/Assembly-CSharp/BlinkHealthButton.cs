using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
public sealed class BlinkHealthButton : MonoBehaviour
{
	// Token: 0x06000171 RID: 369 RVA: 0x0000EA80 File Offset: 0x0000CC80
	private void Start()
	{
		BlinkHealthButton.isBlink = false;
		this.isBlinkState = false;
		this._blinkColorNoAlpha = new Color(this.blinkColor.r, this.blinkColor.g, this.blinkColor.b, 0f);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000EACC File Offset: 0x0000CCCC
	private void Update()
	{
		if (this.player_move_c == null)
		{
			if (Defs.isMulti)
			{
				this.player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
			}
			else
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if (gameObject != null)
				{
					this.player_move_c = gameObject.GetComponent<SkinName>().playerMoveC;
				}
			}
		}
		if (this.player_move_c == null)
		{
			return;
		}
		if (this.typeButton == BlinkHealthButton.RegimButton.Health)
		{
			if (this.player_move_c.CurHealth + this.player_move_c.curArmor < 3f && !this.player_move_c.isMechActive)
			{
				BlinkHealthButton.isBlink = true;
			}
			else
			{
				BlinkHealthButton.isBlink = false;
			}
		}
		if (this.typeButton == BlinkHealthButton.RegimButton.Ammo)
		{
			Weapon weapon = (Weapon)WeaponManager.sharedManager.playerWeapons[WeaponManager.sharedManager.CurrentWeaponIndex];
			if (weapon.currentAmmoInClip == 0 && weapon.currentAmmoInBackpack == 0 && (!weapon.weaponPrefab.GetComponent<WeaponSounds>().isMelee || weapon.weaponPrefab.GetComponent<WeaponSounds>().isShotMelee) && !this.player_move_c.isMechActive)
			{
				BlinkHealthButton.isBlink = true;
			}
			else
			{
				BlinkHealthButton.isBlink = false;
			}
		}
		this.isBlinkTemp = BlinkHealthButton.isBlink;
		if (this.isBlinkOld != BlinkHealthButton.isBlink)
		{
			this.timerBlink = this.maxTimerBlink;
		}
		if (BlinkHealthButton.isBlink)
		{
			this.timerBlink -= Time.deltaTime;
			if (this.timerBlink < 0f)
			{
				this.timerBlink = this.maxTimerBlink;
				this.isBlinkState = !this.isBlinkState;
				if (this.shine != null)
				{
					this.shine.color = ((!this.isBlinkState) ? this._blinkColorNoAlpha : this.blinkColor);
				}
			}
		}
		if (!BlinkHealthButton.isBlink && this.isBlinkState)
		{
			this.isBlinkState = !this.isBlinkState;
			if (this.shine != null)
			{
				this.shine.color = ((!this.isBlinkState) ? this._blinkColorNoAlpha : this.blinkColor);
			}
		}
		this.isBlinkOld = BlinkHealthButton.isBlink;
	}

	// Token: 0x0400015C RID: 348
	public BlinkHealthButton.RegimButton typeButton;

	// Token: 0x0400015D RID: 349
	public static bool isBlink;

	// Token: 0x0400015E RID: 350
	private bool isBlinkOld;

	// Token: 0x0400015F RID: 351
	public float timerBlink;

	// Token: 0x04000160 RID: 352
	public float maxTimerBlink = 0.5f;

	// Token: 0x04000161 RID: 353
	public Color blinkColor = new Color(1f, 0f, 0f);

	// Token: 0x04000162 RID: 354
	public Color unBlinkColor = new Color(1f, 1f, 1f);

	// Token: 0x04000163 RID: 355
	public bool isBlinkState;

	// Token: 0x04000164 RID: 356
	public UISprite[] blinkObjs;

	// Token: 0x04000165 RID: 357
	public bool isBlinkTemp;

	// Token: 0x04000166 RID: 358
	public UISprite shine;

	// Token: 0x04000167 RID: 359
	private Player_move_c player_move_c;

	// Token: 0x04000168 RID: 360
	private Color _blinkColorNoAlpha;

	// Token: 0x02000034 RID: 52
	public enum RegimButton
	{
		// Token: 0x0400016A RID: 362
		Health,
		// Token: 0x0400016B RID: 363
		Ammo
	}
}
