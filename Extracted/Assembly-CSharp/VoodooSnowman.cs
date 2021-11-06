using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200087D RID: 2173
public class VoodooSnowman : TurretController
{
	// Token: 0x06004E6D RID: 20077 RVA: 0x001C6B00 File Offset: 0x001C4D00
	public override void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerId = 0)
	{
		if (!this.targetSelected || this.voodooTarget.Equals(null) || this.voodooTarget.IsDead())
		{
			return;
		}
		base.StopCoroutine(base.FlashRed());
		base.StartCoroutine(base.FlashRed());
		if (Defs.isSoundFX)
		{
			base.GetComponent<AudioSource>().PlayOneShot(this.hitSound);
		}
		this.voodooTarget.ApplyDamage(damage * 0.5f, damageFrom, typeKill, typeDead, this.gadgetName, killerId);
	}

	// Token: 0x06004E6E RID: 20078 RVA: 0x001C6B8C File Offset: 0x001C4D8C
	public override bool IsEnemyTo(Player_move_c player)
	{
		return this.myPlayerMoveC.Equals(player);
	}

	// Token: 0x06004E6F RID: 20079 RVA: 0x001C6BA4 File Offset: 0x001C4DA4
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (this.isReady && (!Defs.isMulti || this.isMine) && !this.targetSelected)
		{
			this.SelectVoodooTarget();
		}
		if (this.targetSelected && (this.voodooTarget.Equals(null) || this.voodooTarget.IsDead()))
		{
			if (this.GadgetOnKill != null)
			{
				this.GadgetOnKill();
			}
			base.SendImKilledRPC();
		}
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x001C6C30 File Offset: 0x001C4E30
	private void SelectVoodooTarget()
	{
		if (Defs.isMulti && !Defs.isCOOP)
		{
			List<Player_move_c> list = new List<Player_move_c>(9);
			for (int i = 0; i < Initializer.players.Count; i++)
			{
				if (!Initializer.players[i].Equals(this.myPlayerMoveC))
				{
					if (!Initializer.players[i].isKilled)
					{
						if (!Initializer.players[i].isImmortality)
						{
							if (Initializer.players[i].myDamageable.IsEnemyTo(this.myPlayerMoveC))
							{
								list.Add(Initializer.players[i]);
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				this.voodooTarget = list[UnityEngine.Random.Range(0, list.Count)].myDamageable;
			}
		}
		else if (Initializer.enemiesObj.Count > 0)
		{
			this.voodooTarget = Initializer.enemiesObj[UnityEngine.Random.Range(0, Initializer.enemiesObj.Count)].GetComponent<IDamageable>();
		}
		if (this.voodooTarget != null)
		{
			this.targetSelected = true;
		}
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001C6D70 File Offset: 0x001C4F70
	public override void StartTurret()
	{
		base.StartTurret();
	}

	// Token: 0x04003D04 RID: 15620
	private IDamageable voodooTarget;

	// Token: 0x04003D05 RID: 15621
	private bool targetSelected;
}
