using System;

// Token: 0x020006EC RID: 1772
public interface IDamageable
{
	// Token: 0x06003DA2 RID: 15778
	void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill);

	// Token: 0x06003DA3 RID: 15779
	void ApplyDamage(float damage, IDamageable damageFrom, Player_move_c.TypeKills typeKill, WeaponSounds.TypeDead typeDead, string weaponName, int killerViewId = 0);

	// Token: 0x06003DA4 RID: 15780
	bool IsEnemyTo(Player_move_c player);

	// Token: 0x06003DA5 RID: 15781
	bool IsDead();

	// Token: 0x17000A42 RID: 2626
	// (get) Token: 0x06003DA6 RID: 15782
	bool isLivingTarget { get; }
}
