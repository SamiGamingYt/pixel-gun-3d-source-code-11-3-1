using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class GadgetsInfo
{
	// Token: 0x060036D4 RID: 14036 RVA: 0x0011A2C0 File Offset: 0x001184C0
	static GadgetsInfo()
	{
		GadgetsInfo.OnGetGadget = null;
		GadgetsInfo._info = new Dictionary<string, GadgetInfo>
		{
			{
				"gadget_black_label",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_black_label", "Key_2515", 2, null, "gadget_black_label_up1", "Key_2583", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>(), PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_black_label_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_black_label_up1", "Key_2659", 4, "gadget_black_label", "gadget_black_label_up2", "Key_2583", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>(), PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_black_label_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_black_label_up2", "Key_2660", 6, "gadget_black_label_up1", null, "Key_2583", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>(), PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_Blizzard_generator",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_Blizzard_generator", "Key_2512", 2, null, "gadget_Blizzard_generator_up1", "Key_2581", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.SlowTheTarget
				}, PlayerEventScoreController.ScoreEvent.coldShower)
			},
			{
				"gadget_Blizzard_generator_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_Blizzard_generator_up1", "Key_2657", 4, "gadget_Blizzard_generator", "gadget_Blizzard_generator_up2", "Key_2581", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.SlowTheTarget
				}, PlayerEventScoreController.ScoreEvent.coldShower)
			},
			{
				"gadget_Blizzard_generator_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_Blizzard_generator_up2", "Key_2658", 6, "gadget_Blizzard_generator_up1", null, "Key_2581", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.SlowTheTarget
				}, PlayerEventScoreController.ScoreEvent.coldShower)
			},
			{
				"gadget_christmastreeturret",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_christmastreeturret", "Key_2747", 2, null, "gadget_christmastreeturret_up1", "Key_2738", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_christmastreeturret_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_christmastreeturret_up1", "Key_2773", 4, "gadget_christmastreeturret", "gadget_christmastreeturret_up2", "Key_2738", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_christmastreeturret_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_christmastreeturret_up2", "Key_2774", 6, "gadget_christmastreeturret_up1", null, "Key_2738", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_demon_stone",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_demon_stone", "Key_2524", 2, null, "gadget_demon_stone_up1", "Key_2580", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying,
					WeaponSounds.Effects.Melee
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_demon_stone_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_demon_stone_up1", "Key_2661", 4, "gadget_demon_stone", "gadget_demon_stone_up2", "Key_2580", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying,
					WeaponSounds.Effects.Melee
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_demon_stone_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_demon_stone_up2", "Key_2662", 6, "gadget_demon_stone_up1", null, "Key_2580", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying,
					WeaponSounds.Effects.Melee
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_disabler",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_disabler", "Key_2526", 6, null, null, "Key_2582", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.GadgetBlocker,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_dragonwhistle",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_dragonwhistle", "Key_2525", 4, null, "gadget_dragonwhistle_up1", "Key_2585", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.dragonSpirit)
			},
			{
				"gadget_dragonwhistle_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_dragonwhistle_up1", "Key_2668", 6, "gadget_dragonwhistle", null, "Key_2585", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.dragonSpirit)
			},
			{
				"gadget_fakebonus",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_fakebonus", "Key_2527", 3, null, "gadget_fakebonus_up1", "Key_2567", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.joker)
			},
			{
				"gadget_fakebonus_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_fakebonus_up1", "Key_2665", 5, "gadget_fakebonus", null, "Key_2567", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.joker)
			},
			{
				"gadget_firemushroom",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_firemushroom", "Key_2514", 2, null, "gadget_firemushroom_up1", "Key_2586", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.mushroomer)
			},
			{
				"gadget_firemushroom_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_firemushroom_up1", "Key_2655", 4, "gadget_firemushroom", "gadget_firemushroom_up2", "Key_2586", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.mushroomer)
			},
			{
				"gadget_firemushroom_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_firemushroom_up2", "Key_2656", 6, "gadget_firemushroom_up1", null, "Key_2586", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.mushroomer)
			},
			{
				"gadget_firework",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_firework", "Key_2743", 1, null, "gadget_firework_up1", "Key_2736", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_firework_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_firework_up1", "Key_2767", 3, "gadget_firework", "gadget_firework_up2", "Key_2736", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_firework_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_firework_up2", "Key_2768", 5, "gadget_firework_up1", null, "Key_2736", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_fraggrenade",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_fraggrenade", "Key_2480", 1, null, "gadget_fraggrenade_up1", "Key_2538", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.deadGrenade)
			},
			{
				"gadget_fraggrenade_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_fraggrenade_up1", "Key_2641", 3, "gadget_fraggrenade", "gadget_fraggrenade_up2", "Key_2538", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.deadGrenade)
			},
			{
				"gadget_fraggrenade_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_fraggrenade_up2", "Key_2642", 5, "gadget_fraggrenade_up1", null, "Key_2538", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.deadGrenade)
			},
			{
				"gadget_jetpack",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_jetpack", "Key_0772", 1, null, "gadget_jetpack_up1", "Key_2572", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_jetpack_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_jetpack_up1", "Key_2645", 3, "gadget_jetpack", "gadget_jetpack_up2", "Key_2572", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_jetpack_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_jetpack_up2", "Key_2646", 5, "gadget_jetpack_up1", null, "Key_2572", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Flying
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_leaderdrum",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_leaderdrum", "Key_2744", 1, null, "gadget_leaderdrum_up1", "Key_2737", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_leaderdrum_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_leaderdrum_up1", "Key_2769", 3, "gadget_leaderdrum", "gadget_leaderdrum_up2", "Key_2737", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_leaderdrum_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_leaderdrum_up2", "Key_2770", 5, "gadget_leaderdrum_up1", null, "Key_2737", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_mech",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_mech", "Key_0774", 4, null, "gadget_mech_up1", "Key_2587", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_mech_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_mech_up1", "Key_2669", 6, "gadget_mech", null, "Key_2587", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_medicalstation",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_medicalstation", "Key_2513", 3, null, "gadget_medicalstation_up1", "Key_2540", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Durability
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Healing,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_medicalstation_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_medicalstation_up1", "Key_2666", 5, "gadget_medicalstation", null, "Key_2540", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Durability
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Healing,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_medkit",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_medkit", "Key_2475", 1, null, "gadget_medkit_up1", "Key_2576", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Healing
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Healing
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_medkit_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_medkit_up1", "Key_2643", 3, "gadget_medkit", "gadget_medkit_up2", "Key_2576", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Healing
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Healing
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_medkit_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_medkit_up2", "Key_2644", 5, "gadget_medkit_up1", null, "Key_2576", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Healing
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Healing
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_mine",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_mine", "Key_2485", 2, null, "gadget_mine_up1", "Key_2568", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.illusionist)
			},
			{
				"gadget_mine_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_mine_up1", "Key_2653", 4, "gadget_mine", "gadget_mine_up2", "Key_2568", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.illusionist)
			},
			{
				"gadget_mine_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_mine_up2", "Key_2654", 6, "gadget_mine_up1", null, "Key_2568", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Detonation,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.illusionist)
			},
			{
				"gadget_molotov",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_molotov", "Key_2484", 1, null, "gadget_molotov_up1", "Key_2569", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.renegade)
			},
			{
				"gadget_molotov_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_molotov_up1", "Key_2647", 3, "gadget_molotov", "gadget_molotov_up2", "Key_2569", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.renegade)
			},
			{
				"gadget_molotov_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_molotov_up2", "Key_2648", 5, "gadget_molotov_up1", null, "Key_2569", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Burning
				}, PlayerEventScoreController.ScoreEvent.renegade)
			},
			{
				"gadget_ninjashurickens",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_ninjashurickens", "Key_2878", 3, null, "gadget_ninjashurickens_up1", "Key_2895", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.ThroughEnemies,
					WeaponSounds.Effects.Bleeding
				}, PlayerEventScoreController.ScoreEvent.renegade)
			},
			{
				"gadget_ninjashurickens_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_ninjashurickens_up1", "Key_2878", 5, "gadget_ninjashurickens", null, "Key_2895", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.ThroughEnemies,
					WeaponSounds.Effects.Bleeding
				}, PlayerEventScoreController.ScoreEvent.renegade)
			},
			{
				"gadget_nucleargrenade",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_nucleargrenade", "Key_2481", 6, null, null, "Key_2570", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AreaDamage,
					WeaponSounds.Effects.Radiation
				}, PlayerEventScoreController.ScoreEvent.nuker)
			},
			{
				"gadget_nutcracker",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_nutcracker", "Key_2746", 4, null, "gadget_nutcracker_up1", "Key_2739", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AutoHoming,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.illusionist)
			},
			{
				"gadget_nutcracker_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_nutcracker_up1", "Key_2777", 6, "gadget_nutcracker", null, "Key_2739", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.AutoHoming,
					WeaponSounds.Effects.AreaDamage
				}, PlayerEventScoreController.ScoreEvent.illusionist)
			},
			{
				"gadget_pandorabox",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_pandorabox", "Key_2563", 3, null, "gadget_pandorabox_up1", "Key_2584", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>(), PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_pandorabox_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_pandorabox_up1", "Key_2667", 5, "gadget_pandorabox", null, "Key_2584", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Damage
				}, new List<WeaponSounds.Effects>(), PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_petbooster",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_petbooster", "Key_2745", 2, null, "gadget_petbooster_up1", "Key_2741", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_petbooster_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_petbooster_up1", "Key_2772", 4, "gadget_petbooster", "gadget_petbooster_up2", "Key_2741", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_petbooster_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_petbooster_up2", "Key_2773", 6, "gadget_petbooster_up1", null, "Key_2741", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageBoost
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_reflector",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_reflector", "Key_2482", 5, null, null, "Key_2577", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageReflection
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_resurrection",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_resurrection", "Key_2477", 6, null, null, "Key_2578", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Resurrection
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_shield",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_shield", "Key_2478", 1, null, "gadget_shield_up1", "Key_2573", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Durability
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageAbsorbtion
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_shield_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_shield_up1", "Key_2649", 3, "gadget_shield", "gadget_shield_up2", "Key_2573", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Durability
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageAbsorbtion
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_shield_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Support, "gadget_shield_up2", "Key_2650", 5, "gadget_shield_up1", null, "Key_2573", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Durability
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageAbsorbtion
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_singularity",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_singularity", "Key_2476", 5, null, null, "Key_2571", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.GravitationForce,
					WeaponSounds.Effects.AreaOfEffects
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_snowman",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_snowman", "Key_2749", 3, null, "gadget_snowman_up1", "Key_2740", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageTransfer
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_snowman_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_snowman_up1", "Key_2778", 5, "gadget_snowman", null, "Key_2740", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.DamageTransfer
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_stealth",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_stealth", "Key_2483", 2, null, "gadget_stealth_up1", "Key_2574", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Invisibility
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_stealth_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_stealth_up1", "Key_2663", 4, "gadget_stealth", "gadget_stealth_up2", "Key_2574", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Invisibility
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_stealth_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_stealth_up2", "Key_2664", 6, "gadget_stealth_up1", null, "Key_2574", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown,
					GadgetInfo.Parameter.Lifetime
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Invisibility
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_stickycandy",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_stickycandy", "Key_2748", 2, null, "gadget_stickycandy_up1", "Key_2742", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.SlowTheTarget,
					WeaponSounds.Effects.DisableJump
				}, PlayerEventScoreController.ScoreEvent.nuker)
			},
			{
				"gadget_stickycandy_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_stickycandy_up1", "Key_2775", 4, "gadget_stickycandy", "gadget_stickycandy_up2", "Key_2742", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.SlowTheTarget,
					WeaponSounds.Effects.DisableJump
				}, PlayerEventScoreController.ScoreEvent.nuker)
			},
			{
				"gadget_stickycandy_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Throwing, "gadget_stickycandy_up2", "Key_2776", 6, "gadget_stickycandy_up1", null, "Key_2742", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.SlowTheTarget,
					WeaponSounds.Effects.DisableJump
				}, PlayerEventScoreController.ScoreEvent.nuker)
			},
			{
				"gadget_timewatch",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_timewatch", "Key_2479", 5, null, null, "Key_2575", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.TimeShift
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_turret",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_turret", "Key_0773", 1, null, "gadget_turret_up1", "Key_2539", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_turret_up1",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_turret_up1", "Key_2651", 3, "gadget_turret", "gadget_turret_up2", "Key_2539", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic
				}, PlayerEventScoreController.ScoreEvent.none)
			},
			{
				"gadget_turret_up2",
				new GadgetInfo(GadgetInfo.GadgetCategory.Tools, "gadget_turret_up2", "Key_2652", 5, "gadget_turret_up1", null, "Key_2539", new List<GadgetInfo.Parameter>
				{
					GadgetInfo.Parameter.Damage,
					GadgetInfo.Parameter.Durability,
					GadgetInfo.Parameter.Lifetime,
					GadgetInfo.Parameter.Cooldown
				}, new List<WeaponSounds.Effects>
				{
					WeaponSounds.Effects.Automatic
				}, PlayerEventScoreController.ScoreEvent.none)
			}
		};
	}

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x060036D5 RID: 14037 RVA: 0x0011BCBC File Offset: 0x00119EBC
	// (remove) Token: 0x060036D6 RID: 14038 RVA: 0x0011BCD4 File Offset: 0x00119ED4
	public static event Action<string> OnGetGadget;

	// Token: 0x060036D7 RID: 14039 RVA: 0x0011BCEC File Offset: 0x00119EEC
	public static string BaseName(string id)
	{
		return id.Replace(GadgetsInfo.up1_suffix, string.Empty).Replace(GadgetsInfo.up2_suffix, string.Empty);
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x0011BD10 File Offset: 0x00119F10
	public static GameObject GetArmoryInfoPrefabFromName(string id)
	{
		string str = GadgetsInfo.BaseName(id);
		GameObject gameObject = Resources.Load<GameObject>("GadgetsContent/GadgetsArmoryInfoPreview/" + str);
		if (gameObject == null)
		{
			gameObject = Resources.Load<GameObject>("GadgetsContent/GadgetsArmoryInfoPreview/empty");
		}
		return gameObject;
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x0011BD50 File Offset: 0x00119F50
	public static Dictionary<GadgetInfo.GadgetCategory, List<string>> GetGadgetsByCategoriesFromItems(List<string> items)
	{
		try
		{
			IEnumerable<string> source = items.Intersect(GadgetsInfo.info.Keys);
			IEnumerable<GadgetInfo> source2 = from gadgetId in source
			select GadgetsInfo.info[gadgetId];
			IEnumerable<IGrouping<GadgetInfo.GadgetCategory, GadgetInfo>> source3 = from gadgetInfo in source2
			group gadgetInfo by gadgetInfo.Category;
			return source3.ToDictionary((IGrouping<GadgetInfo.GadgetCategory, GadgetInfo> grouping) => grouping.Key, (IGrouping<GadgetInfo.GadgetCategory, GadgetInfo> grouping) => (from gadgetInfo in grouping
			select gadgetInfo.Id).ToList<string>());
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GetGadgetsFromItems: {0}", new object[]
			{
				ex
			});
		}
		return new Dictionary<GadgetInfo.GadgetCategory, List<string>>();
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x0011BE44 File Offset: 0x0011A044
	public static List<string> GetNewGadgetsForTier(int tier)
	{
		try
		{
			IEnumerable<GadgetInfo> source = from chain in GadgetsInfo.UpgradeChains
			select GadgetsInfo.info[chain.First<string>()];
			return (from gadgetInfo in source
			where gadgetInfo.Tier == tier
			select gadgetInfo.Id).ToList<string>();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in GetNewGadgetsForTier: {0}", new object[]
			{
				ex
			});
		}
		return new List<string>();
	}

	// Token: 0x170008FA RID: 2298
	// (get) Token: 0x060036DB RID: 14043 RVA: 0x0011BF0C File Offset: 0x0011A10C
	public static Dictionary<string, GadgetInfo> info
	{
		get
		{
			return GadgetsInfo._info;
		}
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x0011BF14 File Offset: 0x0011A114
	public static Dictionary<string, GadgetInfo> GadgetsOfCategory(GadgetInfo.GadgetCategory category)
	{
		if (GadgetsInfo._infosByCategories == null)
		{
			GadgetsInfo._infosByCategories = new Dictionary<GadgetInfo.GadgetCategory, Dictionary<string, GadgetInfo>>(GadgetCategoryComparer.Instance);
			GadgetInfo.GadgetCategory gadgetCategory;
			foreach (GadgetInfo.GadgetCategory gadgetCategory2 in Enum.GetValues(typeof(GadgetInfo.GadgetCategory)).OfType<GadgetInfo.GadgetCategory>())
			{
				gadgetCategory = gadgetCategory2;
				GadgetsInfo._infosByCategories[gadgetCategory] = (from kvp in GadgetsInfo.info
				where kvp.Value.Category == gadgetCategory
				select kvp).ToDictionary((KeyValuePair<string, GadgetInfo> kvp) => kvp.Key, (KeyValuePair<string, GadgetInfo> kvp) => kvp.Value);
			}
		}
		Dictionary<string, GadgetInfo> result = null;
		if (GadgetsInfo._infosByCategories.TryGetValue(category, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x0011C020 File Offset: 0x0011A220
	public static IEnumerable<string> AvailableForBuyGadgets(int maximumTier)
	{
		try
		{
			return from chain in GadgetsInfo.UpgradeChains
			where !GadgetsInfo.IsBought(chain.Last<string>())
			select GadgetsInfo.FirstUnboughtOrForOurTier(chain.Last<string>()) into firstUnbought
			where GadgetsInfo.info[firstUnbought].Tier <= maximumTier
			select firstUnbought;
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in AvailableForBuyGadgets: {0}", new object[]
			{
				ex
			});
		}
		return new List<string>();
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x0011C0DC File Offset: 0x0011A2DC
	public static void ActualizeEquippedGadgets()
	{
		try
		{
			GadgetInfo.GadgetCategory[] array = new GadgetInfo.GadgetCategory[]
			{
				GadgetInfo.GadgetCategory.Throwing,
				GadgetInfo.GadgetCategory.Tools,
				GadgetInfo.GadgetCategory.Support
			};
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				GadgetInfo.GadgetCategory category = array[i];
				string text = GadgetsInfo.EquippedForCategory(category);
				if (!text.IsNullOrEmpty())
				{
					string text2 = GadgetsInfo.LastBoughtFor(text);
					if (!string.IsNullOrEmpty(text2) && text2 != text)
					{
						ShopNGUIController.EquipGadget(text2, category);
					}
				}
				i++;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in ActualizeEquippedGadgets: {0}", new object[]
			{
				ex
			});
		}
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x0011C1A0 File Offset: 0x0011A3A0
	public static void ProvideGadget(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("ProvideGadget gadgetId == null");
			return;
		}
		if (!GadgetsInfo.info.ContainsKey(gadgetId))
		{
			Debug.LogError("ProvideGadget !info.ContainsKey(gadgetId)");
			return;
		}
		List<string> list = GadgetsInfo.Upgrades[gadgetId];
		if (list == null)
		{
			Debug.LogError("ProvideGadget chain == null");
			return;
		}
		int index = 0;
		do
		{
			if (Storager.getInt(list[index], true) == 0)
			{
				Storager.setInt(list[index], 1, true);
				Action<string> onGetGadget = GadgetsInfo.OnGetGadget;
				if (onGetGadget != null)
				{
					onGetGadget(list[index]);
				}
			}
		}
		while (list[index++] != gadgetId);
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x0011C248 File Offset: 0x0011A448
	public static string EquippedForCategory(GadgetInfo.GadgetCategory category)
	{
		return Storager.getString(GadgetsInfo.SNForCategory(category), false);
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x0011C258 File Offset: 0x0011A458
	public static string SNForCategory(GadgetInfo.GadgetCategory category)
	{
		return "Equipped_" + category.ToString() + "_SN";
	}

	// Token: 0x170008FB RID: 2299
	// (get) Token: 0x060036E2 RID: 14050 RVA: 0x0011C274 File Offset: 0x0011A474
	// (set) Token: 0x060036E3 RID: 14051 RVA: 0x0011C27C File Offset: 0x0011A47C
	public static GadgetInfo.GadgetCategory DefaultGadget
	{
		get
		{
			return GadgetsInfo._defaultGadget;
		}
		set
		{
			GadgetsInfo._defaultGadget = value;
		}
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x0011C284 File Offset: 0x0011A484
	public static string LastBoughtFor(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("LastBoughtFor gadgetId == null");
			return null;
		}
		List<string> list = GadgetsInfo.UpgradesChainForGadget(gadgetId);
		if (list == null)
		{
			Debug.LogError("LastBoughtFor chain == null , gadgetId = " + gadgetId);
			return null;
		}
		string result = null;
		for (int i = 0; i < list.Count; i++)
		{
			if (!GadgetsInfo.IsBought(list[i]))
			{
				break;
			}
			result = list[i];
		}
		return result;
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x0011C300 File Offset: 0x0011A500
	public static List<string> UpgradesChainForGadget(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("UpgradesChainForGadget gadgetId = null");
			return null;
		}
		List<string> list = null;
		GadgetsInfo.Upgrades.TryGetValue(gadgetId, out list);
		if (list == null)
		{
			Debug.LogError("UpgradesChainForGadget chain = null, gadget = " + gadgetId);
		}
		return list;
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x0011C348 File Offset: 0x0011A548
	public static string FirstForOurTier(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("FirstForOurTier gadgetId == null");
			return null;
		}
		if (!GadgetsInfo._firstsForTiersInitialized)
		{
			GadgetsInfo.InitFirstForOurTierData();
			GadgetsInfo._firstsForTiersInitialized = true;
		}
		List<string> list = GadgetsInfo.UpgradesChainForGadget(gadgetId);
		if (list == null)
		{
			Debug.LogError("FirstForOurTier chain == null , gadgetId = " + gadgetId);
			return null;
		}
		if (list.Count > 0)
		{
			string text = null;
			GadgetsInfo.firstGadgetsForOurTier.TryGetValue(list[0], out text);
			if (text == null)
			{
				Debug.LogError("FirstForOurTier first == null , gadgetId = " + gadgetId);
			}
			return text;
		}
		Debug.LogError("FirstForOurTier chain.Count = 0 gadgetId = " + gadgetId);
		return null;
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x0011C3E8 File Offset: 0x0011A5E8
	public static string FirstUnboughtOrForOurTier(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("FirstUnboughtOrForOurTier gadgetId == null");
			return null;
		}
		List<string> list = GadgetsInfo.UpgradesChainForGadget(gadgetId);
		if (list == null)
		{
			Debug.LogError("FirstUnboughtOrForOurTier chain == null , gadgetId = " + gadgetId);
			return null;
		}
		string text = GadgetsInfo.FirstUnbought(gadgetId);
		if (text == null)
		{
			Debug.LogError("FirstUnboughtOrForOurTier firstUnobught == null , gadgetId = " + gadgetId);
			return null;
		}
		string text2 = GadgetsInfo.FirstForOurTier(gadgetId);
		if (text2 == null)
		{
			Debug.LogError("FirstUnboughtOrForOurTier forOurTier == null , gadgetId = " + gadgetId);
			return null;
		}
		return (list.IndexOf(text2) <= list.IndexOf(text)) ? text : text2;
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x0011C480 File Offset: 0x0011A680
	public static string FirstUnbought(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("FirstUnbought gadgetId == null");
			return null;
		}
		List<string> list = GadgetsInfo.UpgradesChainForGadget(gadgetId);
		if (list == null)
		{
			Debug.LogError("FirstUnbought chain == null , gadgetId = " + gadgetId);
			return null;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (!GadgetsInfo.IsBought(list[i]))
			{
				return list[i];
			}
		}
		return list[list.Count - 1];
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x0011C4FC File Offset: 0x0011A6FC
	public static bool IsBought(string gadgetId)
	{
		if (gadgetId == null)
		{
			Debug.LogError("IsBought gadgetId == null");
			return false;
		}
		return Storager.getInt(gadgetId, true) > 0;
	}

	// Token: 0x170008FC RID: 2300
	// (get) Token: 0x060036EA RID: 14058 RVA: 0x0011C51C File Offset: 0x0011A71C
	public static Dictionary<string, List<string>> Upgrades
	{
		get
		{
			GadgetsInfo.InitializeUpgrades();
			return GadgetsInfo._upgrades;
		}
	}

	// Token: 0x170008FD RID: 2301
	// (get) Token: 0x060036EB RID: 14059 RVA: 0x0011C528 File Offset: 0x0011A728
	public static IEnumerable<List<string>> UpgradeChains
	{
		get
		{
			GadgetsInfo.InitializeUpgrades();
			return GadgetsInfo._upgradeChains;
		}
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x0011C534 File Offset: 0x0011A734
	public static void FixFirstsForOurTier()
	{
		try
		{
			bool flag = false;
			foreach (List<string> list in GadgetsInfo.UpgradeChains)
			{
				if (list == null || list.Count != 0)
				{
					string text = list[0];
					string text2 = GadgetsInfo.LastBoughtFor(text);
					if (text2 != null)
					{
						string item = GadgetsInfo.FirstForOurTier(text);
						int num = list.IndexOf(text2);
						int num2 = list.IndexOf(item);
						if (num < num2)
						{
							flag = true;
							GadgetsInfo.firstGadgetsForOurTier[text] = text2;
						}
					}
				}
			}
			if (flag)
			{
				GadgetsInfo.SaveFirstsToDisc();
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in gadgets FixFirstsForOurTier: " + arg);
		}
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x0011C638 File Offset: 0x0011A838
	private static void InitFirstForOurTierData()
	{
		if (!Storager.hasKey("GadgetsInfo.FirstForOurTier"))
		{
			Storager.setString("GadgetsInfo.FirstForOurTier", "{}", false);
		}
		string @string = Storager.getString("GadgetsInfo.FirstForOurTier", false);
		try
		{
			Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				GadgetsInfo.firstGadgetsForOurTier.Add(keyValuePair.Key, (string)keyValuePair.Value);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return;
		}
		int ourTier = ExpController.GetOurTier();
		bool flag = false;
		foreach (List<string> list in GadgetsInfo.UpgradeChains)
		{
			if (list.Count == 0)
			{
				Debug.LogError("InitFirstTagsData upgrades.Count == 0");
			}
			else if (!GadgetsInfo.firstGadgetsForOurTier.ContainsKey(list[0]))
			{
				flag = true;
				List<GadgetInfo> list2 = (from gadgetId in list
				select GadgetsInfo.info[gadgetId]).ToList<GadgetInfo>();
				bool flag2 = false;
				for (int i = 0; i < list.Count; i++)
				{
					if (list2[i] != null && list2[i].Tier > ourTier)
					{
						GadgetsInfo.firstGadgetsForOurTier.Add(list[0], list[Math.Max(0, i - 1)]);
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					GadgetsInfo.firstGadgetsForOurTier.Add(list[0], list[list.Count - 1]);
				}
			}
		}
		if (flag)
		{
			GadgetsInfo.SaveFirstsToDisc();
		}
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x0011C874 File Offset: 0x0011AA74
	private static void SaveFirstsToDisc()
	{
		Storager.setString("GadgetsInfo.FirstForOurTier", Json.Serialize(GadgetsInfo.firstGadgetsForOurTier), false);
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x0011C88C File Offset: 0x0011AA8C
	private static void InitializeUpgrades()
	{
		if (GadgetsInfo._upgradesInitialized)
		{
			return;
		}
		GadgetsInfo._upgradesInitialized = true;
		IEnumerable<IGrouping<string, KeyValuePair<string, GadgetInfo>>> source = from kvp in GadgetsInfo.info
		group kvp by GadgetsInfo.BaseOfUpgardesChainFor(kvp.Value).Id;
		IEnumerable<List<string>> source2 = from grouping in source
		select (from kvp in grouping.OrderBy(delegate(KeyValuePair<string, GadgetInfo> kvp)
		{
			int num = 0;
			if (kvp.Value.PreviousUpgradeId != null)
			{
				num++;
			}
			if (kvp.Value.NextUpgradeId == null)
			{
				num += 10;
			}
			return num;
		})
		select kvp.Key).ToList<string>();
		Dictionary<string, List<string>> upgrades = source2.SelectMany((List<string> upgradesChain) => from upgrade in upgradesChain
		select new KeyValuePair<string, List<string>>(upgrade, upgradesChain)).ToDictionary((KeyValuePair<string, List<string>> kvp) => kvp.Key, (KeyValuePair<string, List<string>> kvp) => kvp.Value);
		GadgetsInfo._upgrades = upgrades;
		GadgetsInfo._upgradeChains = GadgetsInfo._upgrades.Values.Distinct<List<string>>();
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x0011C974 File Offset: 0x0011AB74
	private static GadgetInfo BaseOfUpgardesChainFor(GadgetInfo gadgetInfo)
	{
		if (gadgetInfo == null)
		{
			Debug.LogError("BaseOfUpgardesChainFor gadgetInfo == null");
			return null;
		}
		return (gadgetInfo.PreviousUpgradeId == null) ? gadgetInfo : GadgetsInfo.BaseOfUpgardesChainFor(GadgetsInfo.info[gadgetInfo.PreviousUpgradeId]);
	}

	// Token: 0x040027F9 RID: 10233
	private const string FirstForOurTierKey = "GadgetsInfo.FirstForOurTier";

	// Token: 0x040027FA RID: 10234
	private static string up1_suffix = "_up1";

	// Token: 0x040027FB RID: 10235
	private static string up2_suffix = "_up2";

	// Token: 0x040027FC RID: 10236
	private static Dictionary<string, string> firstGadgetsForOurTier = new Dictionary<string, string>();

	// Token: 0x040027FD RID: 10237
	private static bool _firstsForTiersInitialized = false;

	// Token: 0x040027FE RID: 10238
	private static Dictionary<string, List<string>> _upgrades = null;

	// Token: 0x040027FF RID: 10239
	private static IEnumerable<List<string>> _upgradeChains = null;

	// Token: 0x04002800 RID: 10240
	private static bool _upgradesInitialized = false;

	// Token: 0x04002801 RID: 10241
	private static GadgetInfo.GadgetCategory _defaultGadget = GadgetInfo.GadgetCategory.Throwing;

	// Token: 0x04002802 RID: 10242
	private static Dictionary<string, GadgetInfo> _info = new Dictionary<string, GadgetInfo>();

	// Token: 0x04002803 RID: 10243
	private static Dictionary<GadgetInfo.GadgetCategory, Dictionary<string, GadgetInfo>> _infosByCategories = null;
}
