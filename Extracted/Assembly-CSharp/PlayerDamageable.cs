using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class PlayerDamageable : MonoBehaviour, IDamageable
{
	// Token: 0x060027F1 RID: 10225 RVA: 0x000C7910 File Offset: 0x000C5B10
	private void Awake()
	{
		this.myPlayer = base.GetComponent<SkinName>().playerMoveC;
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000C7924 File Offset: 0x000C5B24
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill)
	{
		this.ApplyDamage(damage, damageFrom, typeKill, WeaponSounds.TypeDead.angel, string.Empty, 0);
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x000C7938 File Offset: 0x000C5B38
	public void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerViewId = 0)
	{
		if (Defs.isDaterRegim)
		{
			return;
		}
		Vector3 posKiller = Vector3.zero;
		if (damageFrom != null)
		{
			posKiller = (damageFrom as MonoBehaviour).transform.position;
		}
		if (damageFrom != null && typeKill != Player_move_c.TypeKills.reflector && this.myPlayer.IsGadgetEffectActive(Player_move_c.GadgetEffect.reflector))
		{
			damage /= 2f;
			damageFrom.ApplyDamage(damage, this, Player_move_c.TypeKills.reflector, typeDead, weaponName, this.myPlayer.skinNamePixelView.viewID);
		}
		this.myPlayer.GetDamage(damage, typeKill, typeDead, posKiller, weaponName, killerViewId);
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x000C79C8 File Offset: 0x000C5BC8
	public bool IsEnemyTo(Player_move_c player)
	{
		return Defs.isMulti && (player.Equals(this.myPlayer) || (!Defs.isCOOP && (!ConnectSceneNGUIController.isTeamRegim || this.myPlayer.myCommand != player.myCommand)));
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000C7A20 File Offset: 0x000C5C20
	public bool IsDead()
	{
		return this.myPlayer.isKilled || this.myPlayer.isImmortality;
	}

	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000C7A40 File Offset: 0x000C5C40
	public bool isLivingTarget
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04001C3C RID: 7228
	[NonSerialized]
	public Player_move_c myPlayer;
}
