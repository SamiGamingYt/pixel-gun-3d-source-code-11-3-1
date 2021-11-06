using System;
using System.Collections.Generic;

// Token: 0x02000816 RID: 2070
public static class WeaponUpgrades
{
	// Token: 0x06004B70 RID: 19312 RVA: 0x001B065C File Offset: 0x001AE85C
	static WeaponUpgrades()
	{
		List<string> item = new List<string>
		{
			WeaponTags.Fire_orb_Tag,
			WeaponTags.Fire_orb_2_Tag,
			WeaponTags.Fire_orb_3_Tag
		};
		WeaponUpgrades.upgrades.Add(item);
		List<string> item2 = new List<string>
		{
			WeaponTags.Hand_dragon_Tag,
			WeaponTags.Hand_dragon_2_Tag,
			WeaponTags.Hand_dragon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(item2);
		List<string> item3 = new List<string>
		{
			WeaponTags.Tesla_Cannon_Tag,
			WeaponTags.Tesla_Cannon_2_Tag,
			WeaponTags.Tesla_Cannon_3_Tag
		};
		WeaponUpgrades.upgrades.Add(item3);
		List<string> item4 = new List<string>
		{
			WeaponTags.Hydra_Tag,
			WeaponTags.Hydra_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item4);
		List<string> item5 = new List<string>
		{
			WeaponTags.Dark_Matter_Generator_Tag,
			WeaponTags.Dark_Matter_Generator_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item5);
		List<string> item6 = new List<string>
		{
			WeaponTags.Devostator_Tag,
			WeaponTags.Devostator_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item6);
		List<string> item7 = new List<string>
		{
			WeaponTags.TacticalBow_Tag,
			WeaponTags.TacticalBow_2_Tag,
			WeaponTags.TacticalBow_3_Tag
		};
		WeaponUpgrades.upgrades.Add(item7);
		List<string> item8 = new List<string>
		{
			WeaponTags.LaserDiscThower_Tag,
			WeaponTags.LaserDiscThower_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item8);
		List<string> item9 = new List<string>
		{
			WeaponTags.ElectroBlastRifle_Tag,
			WeaponTags.ElectroBlastRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item9);
		List<string> item10 = new List<string>
		{
			WeaponTags.Photon_Pistol_Tag,
			WeaponTags.Photon_Pistol_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item10);
		List<string> item11 = new List<string>
		{
			WeaponTags.RapidFireRifle_Tag,
			WeaponTags.RapidFireRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item11);
		List<string> item12 = new List<string>
		{
			WeaponTags.PlasmaShotgun_Tag,
			WeaponTags.PlasmaShotgun_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item12);
		List<string> item13 = new List<string>
		{
			WeaponTags.FutureRifle_Tag,
			WeaponTags.FutureRifle_2_Tag
		};
		WeaponUpgrades.upgrades.Add(item13);
		List<string> list = new List<string>();
		list.Add(WeaponTags.DragonGun_Tag);
		list.Add(WeaponTags.DragonGun_2_Tag);
		WeaponUpgrades.upgrades.Add(list);
		List<string> list2 = new List<string>();
		list2.Add(WeaponTags.Bazooka_2_3_Tag);
		list2.Add(WeaponTags.Bazooka_2_4tag);
		WeaponUpgrades.upgrades.Add(list2);
		List<string> list3 = new List<string>();
		list3.Add(WeaponTags.buddy_Tag);
		list3.Add(WeaponTags.bigbuddy_2tag);
		list3.Add(WeaponTags.bigbuddy_3tag);
		WeaponUpgrades.upgrades.Add(list3);
		List<string> list4 = new List<string>();
		list4.Add(WeaponTags.barret_3_Tag);
		list4.Add(WeaponTags.Barret_4tag);
		WeaponUpgrades.upgrades.Add(list4);
		List<string> list5 = new List<string>();
		list5.Add(WeaponTags.Flamethrower_3_Tag);
		list5.Add(WeaponTags.Flamethrower_4tag);
		list5.Add(WeaponTags.Flamethrower_5tag);
		WeaponUpgrades.upgrades.Add(list5);
		List<string> list6 = new List<string>();
		list6.Add(WeaponTags.katana_3_Tag);
		list6.Add(WeaponTags.katana_4tag);
		WeaponUpgrades.upgrades.Add(list6);
		List<string> list7 = new List<string>();
		list7.Add(WeaponTags.SparklyBlasterTag);
		list7.Add(WeaponTags.SparklyBlaster_2tag);
		list7.Add(WeaponTags.SparklyBlaster_3tag);
		WeaponUpgrades.upgrades.Add(list7);
		List<string> list8 = new List<string>();
		list8.Add(WeaponTags.Thompson_2_Tag);
		list8.Add(WeaponTags.StateDefender_2_tag);
		WeaponUpgrades.upgrades.Add(list8);
		List<string> list9 = new List<string>();
		list9.Add(WeaponTags.plazma_3_Tag);
		list9.Add(WeaponTags.PlasmaRifle_2_Tag);
		list9.Add(WeaponTags.PlasmaRifle_3_Tag);
		WeaponUpgrades.upgrades.Add(list9);
		List<string> list10 = new List<string>();
		list10.Add(WeaponTags._3_shotgun_3_Tag);
		list10.Add(WeaponTags.SteamPower_2_Tag);
		list10.Add(WeaponTags.SteamPower_3_Tag);
		WeaponUpgrades.upgrades.Add(list10);
		List<string> list11 = new List<string>();
		list11.Add(WeaponTags.MinersWeaponTag);
		list11.Add(WeaponTags.GoldenPickTag);
		list11.Add(WeaponTags.CrystalPickTag);
		WeaponUpgrades.upgrades.Add(list11);
		List<string> list12 = new List<string>();
		list12.Add(WeaponTags.Sword_2_3_Tag);
		list12.Add(WeaponTags.Sword_2_4_Tag);
		list12.Add(WeaponTags.Sword_2_5_Tag);
		WeaponUpgrades.upgrades.Add(list12);
		List<string> list13 = new List<string>();
		list13.Add(WeaponTags.RailgunTag);
		list13.Add(WeaponTags.railgun_2_Tag);
		list13.Add(WeaponTags.railgun_3_Tag);
		WeaponUpgrades.upgrades.Add(list13);
		List<string> list14 = new List<string>();
		list14.Add(WeaponTags.SteelAxeTag);
		list14.Add(WeaponTags.GoldenAxeTag);
		list14.Add(WeaponTags.CrystalAxeTag);
		WeaponUpgrades.upgrades.Add(list14);
		List<string> list15 = new List<string>();
		list15.Add(WeaponTags.IronSwordTag);
		list15.Add(WeaponTags.GoldenSwordTag);
		list15.Add(WeaponTags.CrystalSwordTag);
		WeaponUpgrades.upgrades.Add(list15);
		List<string> list16 = new List<string>();
		list16.Add(WeaponTags.Red_Stone_3_Tag);
		list16.Add(WeaponTags.Red_Stone_4_Tag);
		list16.Add(WeaponTags.Red_Stone_5tag);
		WeaponUpgrades.upgrades.Add(list16);
		List<string> list17 = new List<string>();
		list17.Add(WeaponTags.SPASTag);
		list17.Add(WeaponTags.GoldenSPASTag);
		list17.Add(WeaponTags.CrystalSPASTag);
		WeaponUpgrades.upgrades.Add(list17);
		List<string> list18 = new List<string>();
		list18.Add(WeaponTags.SteelCrossbowTag);
		list18.Add(WeaponTags.CrossbowTag);
		list18.Add(WeaponTags.CrystalCrossbowTag);
		WeaponUpgrades.upgrades.Add(list18);
		List<string> list19 = new List<string>();
		list19.Add(WeaponTags.minigun_3_Tag);
		list19.Add(WeaponTags.Minigun_4_Tag);
		list19.Add(WeaponTags.Minigun_5_Tag);
		WeaponUpgrades.upgrades.Add(list19);
		List<string> list20 = new List<string>();
		list20.Add(WeaponTags.LightSword_3_Tag);
		list20.Add(WeaponTags.LightSword_4_Tag);
		list20.Add(WeaponTags.LightSword_5tag);
		WeaponUpgrades.upgrades.Add(list20);
		List<string> list21 = new List<string>();
		list21.Add(WeaponTags.FAMASTag);
		list21.Add(WeaponTags.SandFamasTag);
		list21.Add(WeaponTags.NavyFamasTag);
		WeaponUpgrades.upgrades.Add(list21);
		List<string> list22 = new List<string>();
		list22.Add(WeaponTags.FreezeGunTag);
		list22.Add(WeaponTags.FreezeGun_2_Tag);
		list22.Add(WeaponTags.FreezeGun_3tag);
		WeaponUpgrades.upgrades.Add(list22);
		List<string> list23 = new List<string>();
		list23.Add(WeaponTags.BerettaTag);
		list23.Add(WeaponTags.WhiteBerettaTag);
		list23.Add(WeaponTags.BlackBerettaTag);
		WeaponUpgrades.upgrades.Add(list23);
		List<string> list24 = new List<string>();
		list24.Add(WeaponTags.EagleTag);
		list24.Add(WeaponTags.BlackEagleTag);
		list24.Add(WeaponTags.eagle_3Tag);
		WeaponUpgrades.upgrades.Add(list24);
		List<string> list25 = new List<string>();
		list25.Add(WeaponTags.GlockTag);
		list25.Add(WeaponTags.GoldenGlockTag);
		list25.Add(WeaponTags.CrystalGlockTag);
		WeaponUpgrades.upgrades.Add(list25);
		List<string> list26 = new List<string>();
		list26.Add(WeaponTags.svdTag);
		list26.Add(WeaponTags.svd_2Tag);
		list26.Add(WeaponTags.svd_3_Tag);
		WeaponUpgrades.upgrades.Add(list26);
		List<string> list27 = new List<string>();
		list27.Add(WeaponTags.m16Tag);
		list27.Add(WeaponTags.m16_3_Tag);
		list27.Add(WeaponTags.m16_4_Tag);
		WeaponUpgrades.upgrades.Add(list27);
		List<string> list28 = new List<string>();
		list28.Add(WeaponTags.TreeTag);
		list28.Add(WeaponTags.Tree_2_Tag);
		WeaponUpgrades.upgrades.Add(list28);
		List<string> list29 = new List<string>();
		list29.Add(WeaponTags.revolver_2_3_Tag);
		list29.Add(WeaponTags.Revolver5_Tag);
		list29.Add(WeaponTags.Revolver6_Tag);
		WeaponUpgrades.upgrades.Add(list29);
		List<string> list30 = new List<string>();
		list30.Add(WeaponTags.FreezeGun_0_Tag);
		list30.Add(WeaponTags.FreezeGun_0_2_Tag);
		WeaponUpgrades.upgrades.Add(list30);
		List<string> list31 = new List<string>();
		list31.Add(WeaponTags.TeslaTag);
		list31.Add(WeaponTags.Tesla_2Tag);
		list31.Add(WeaponTags.tesla_3_Tag);
		WeaponUpgrades.upgrades.Add(list31);
		List<string> list32 = new List<string>();
		list32.Add(WeaponTags.Easter_Bazooka_Tag);
		list32.Add(WeaponTags.Easter_Bazooka_2_Tag);
		list32.Add(WeaponTags.Easter_Bazooka_3_Tag);
		WeaponUpgrades.upgrades.Add(list32);
		List<string> list33 = new List<string>();
		list33.Add(WeaponTags.Bazooka_3Tag);
		list33.Add(WeaponTags.Bazooka_3_2_Tag);
		list33.Add(WeaponTags.Bazooka_3_3_Tag);
		WeaponUpgrades.upgrades.Add(list33);
		List<string> list34 = new List<string>();
		list34.Add(WeaponTags.GrenadeLuancher_2Tag);
		list34.Add(WeaponTags.grenade_launcher_3_Tag);
		list34.Add(WeaponTags.m32_1_2_Tag);
		WeaponUpgrades.upgrades.Add(list34);
		List<string> list35 = new List<string>();
		list35.Add(WeaponTags.BazookaTag);
		list35.Add(WeaponTags.Bazooka_2Tag);
		list35.Add(WeaponTags.Bazooka_1_3_Tag);
		WeaponUpgrades.upgrades.Add(list35);
		List<string> list36 = new List<string>();
		list36.Add(WeaponTags.AUGTag);
		list36.Add(WeaponTags.AUG_2Tag);
		list36.Add(WeaponTags.AUG_3tag);
		WeaponUpgrades.upgrades.Add(list36);
		List<string> list37 = new List<string>();
		list37.Add(WeaponTags.AK74Tag);
		list37.Add(WeaponTags.AK74_2_Tag);
		list37.Add(WeaponTags.AK74_3_Tag);
		WeaponUpgrades.upgrades.Add(list37);
		List<string> list38 = new List<string>();
		list38.Add(WeaponTags.GravigunTag);
		list38.Add(WeaponTags.gravity_2_Tag);
		list38.Add(WeaponTags.gravity_3_Tag);
		WeaponUpgrades.upgrades.Add(list38);
		List<string> list39 = new List<string>();
		list39.Add(WeaponTags.XM8_1_Tag);
		list39.Add(WeaponTags.XM8_2_Tag);
		list39.Add(WeaponTags.XM8_3_Tag);
		WeaponUpgrades.upgrades.Add(list39);
		List<string> list40 = new List<string>();
		list40.Add(WeaponTags.SnowballMachingun_Tag);
		list40.Add(WeaponTags.SnowballMachingun_2_Tag);
		list40.Add(WeaponTags.SnowballMachingun_3_Tag);
		WeaponUpgrades.upgrades.Add(list40);
		List<string> list41 = new List<string>();
		list41.Add(WeaponTags.SnowballGun_Tag);
		list41.Add(WeaponTags.SnowballGun_2_Tag);
		list41.Add(WeaponTags.SnowballGun_3_Tag);
		WeaponUpgrades.upgrades.Add(list41);
		List<string> list42 = new List<string>();
		list42.Add(WeaponTags.HeavyShotgun_Tag);
		list42.Add(WeaponTags.HeavyShotgun_2_Tag);
		list42.Add(WeaponTags.HeavyShotgun_3_Tag);
		WeaponUpgrades.upgrades.Add(list42);
		List<string> list43 = new List<string>();
		list43.Add(WeaponTags.TwoBolters_Tag);
		list43.Add(WeaponTags.TwoBolters_2_Tag);
		list43.Add(WeaponTags.TwoBolters_3_Tag);
		WeaponUpgrades.upgrades.Add(list43);
		List<string> list44 = new List<string>();
		list44.Add(WeaponTags.TwoRevolvers_Tag);
		list44.Add(WeaponTags.TwoRevolvers_2_Tag);
		list44.Add(WeaponTags.TwoRevolvers_3tag);
		WeaponUpgrades.upgrades.Add(list44);
		List<string> list45 = new List<string>();
		list45.Add(WeaponTags.AutoShotgun_Tag);
		list45.Add(WeaponTags.AutoShotgun_2_Tag);
		list45.Add(WeaponTags.AutoShotgun_3tag);
		WeaponUpgrades.upgrades.Add(list45);
		List<string> list46 = new List<string>();
		list46.Add(WeaponTags.Solar_Ray_Tag);
		list46.Add(WeaponTags.Solar_Ray_2_Tag);
		list46.Add(WeaponTags.Solar_Ray_3_Tag);
		WeaponUpgrades.upgrades.Add(list46);
		List<string> list47 = new List<string>();
		list47.Add(WeaponTags.Water_Pistol_Tag);
		list47.Add(WeaponTags.Water_Pistol_2_Tag);
		list47.Add(WeaponTags.Water_Pistol_3tag);
		WeaponUpgrades.upgrades.Add(list47);
		List<string> list48 = new List<string>();
		list48.Add(WeaponTags.Solar_Power_Cannon_Tag);
		list48.Add(WeaponTags.Solar_Power_Cannon_2_Tag);
		list48.Add(WeaponTags.Solar_Power_Cannon_3tag);
		WeaponUpgrades.upgrades.Add(list48);
		List<string> list49 = new List<string>();
		list49.Add(WeaponTags.Water_Rifle_Tag);
		list49.Add(WeaponTags.Water_Rifle_2_Tag);
		list49.Add(WeaponTags.Water_Rifle_3_Tag);
		WeaponUpgrades.upgrades.Add(list49);
		List<string> list50 = new List<string>();
		list50.Add(WeaponTags.Valentine_Shotgun_Tag);
		list50.Add(WeaponTags.Valentine_Shotgun_2_Tag);
		list50.Add(WeaponTags.Valentine_Shotgun_3_Tag);
		WeaponUpgrades.upgrades.Add(list50);
		List<string> list51 = new List<string>();
		list51.Add(WeaponTags.Needle_Throw_Tag);
		list51.Add(WeaponTags.Needle_Throw_2_Tag);
		list51.Add(WeaponTags.Needle_Throw_3_Tag);
		WeaponUpgrades.upgrades.Add(list51);
		List<string> list52 = new List<string>();
		list52.Add(WeaponTags.Carrot_Sword_Tag);
		list52.Add(WeaponTags.Carrot_Sword_2_Tag);
		list52.Add(WeaponTags.Carrot_Sword_3_Tag);
		WeaponUpgrades.upgrades.Add(list52);
		List<string> list53 = new List<string>();
		list53.Add(WeaponTags.RailRevolverBuy_Tag);
		list53.Add(WeaponTags.RailRevolverBuy_2_Tag);
		list53.Add(WeaponTags.RailRevolverBuy_3_Tag);
		WeaponUpgrades.upgrades.Add(list53);
		List<string> list54 = new List<string>();
		list54.Add(WeaponTags.Assault_Machine_GunBuy_Tag);
		list54.Add(WeaponTags.Assault_Machine_GunBuy_2_Tag);
		list54.Add(WeaponTags.Assault_Machine_GunBuy_3_Tag);
		WeaponUpgrades.upgrades.Add(list54);
		List<string> list55 = new List<string>();
		list55.Add(WeaponTags.Impulse_Sniper_RifleBuy_Tag);
		list55.Add(WeaponTags.Impulse_Sniper_RifleBuy_2_Tag);
		list55.Add(WeaponTags.Impulse_Sniper_RifleBuy_3_Tag);
		WeaponUpgrades.upgrades.Add(list55);
		List<string> list56 = new List<string>();
		list56.Add(WeaponTags.Autoaim_RocketlauncherBuy_Tag);
		list56.Add(WeaponTags.Autoaim_RocketlauncherBuy_2_Tag);
		list56.Add(WeaponTags.Autoaim_RocketlauncherBuy_3_Tag);
		WeaponUpgrades.upgrades.Add(list56);
		List<string> list57 = new List<string>();
		list57.Add(WeaponTags.DualUzi_Tag);
		list57.Add(WeaponTags.DualUzi_2_Tag);
		list57.Add(WeaponTags.DualUzi_3_Tag);
		WeaponUpgrades.upgrades.Add(list57);
		List<string> list58 = new List<string>();
		list58.Add(WeaponTags.Alligator_Tag);
		list58.Add(WeaponTags.Alligator_2_Tag);
		list58.Add(WeaponTags.Alligator_3_Tag);
		WeaponUpgrades.upgrades.Add(list58);
		List<string> list59 = new List<string>();
		list59.Add(WeaponTags.Hippo_Tag);
		list59.Add(WeaponTags.Hippo_2_Tag);
		list59.Add(WeaponTags.Hippo_3_Tag);
		WeaponUpgrades.upgrades.Add(list59);
		List<string> list60 = new List<string>();
		list60.Add(WeaponTags.Alien_Cannon_Tag);
		list60.Add(WeaponTags.Alien_Cannon_2_Tag);
		list60.Add(WeaponTags.Alien_Cannon_3_Tag);
		WeaponUpgrades.upgrades.Add(list60);
		List<string> list61 = new List<string>();
		list61.Add(WeaponTags.Alien_Laser_Pistol_Tag);
		list61.Add(WeaponTags.Alien_Laser_Pistol_2_Tag);
		list61.Add(WeaponTags.Alien_Laser_Pistol_3_Tag);
		WeaponUpgrades.upgrades.Add(list61);
		List<string> list62 = new List<string>();
		list62.Add(WeaponTags.Alien_rifle_Tag);
		list62.Add(WeaponTags.Alien_rifle_2_Tag);
		list62.Add(WeaponTags.Alien_rifle_3_Tag);
		WeaponUpgrades.upgrades.Add(list62);
		List<string> list63 = new List<string>();
		list63.Add(WeaponTags.Tiger_gun_Tag);
		list63.Add(WeaponTags.Tiger_gun_2_Tag);
		list63.Add(WeaponTags.Tiger_gun_3_Tag);
		WeaponUpgrades.upgrades.Add(list63);
		List<string> list64 = new List<string>();
		list64.Add(WeaponTags.Pit_Bull_Tag);
		list64.Add(WeaponTags.Pit_Bull_2_Tag);
		list64.Add(WeaponTags.Pit_Bull_3_Tag);
		WeaponUpgrades.upgrades.Add(list64);
		List<string> list65 = new List<string>();
		list65.Add(WeaponTags.Range_Rifle_Tag);
		list65.Add(WeaponTags.Range_Rifle_2_Tag);
		list65.Add(WeaponTags.Range_Rifle_3_Tag);
		WeaponUpgrades.upgrades.Add(list65);
		List<string> list66 = new List<string>();
		list66.Add(WeaponTags.Dater_Bow_Tag);
		list66.Add(WeaponTags.Dater_Bow_2_Tag);
		list66.Add(WeaponTags.Dater_Bow_3_Tag);
		WeaponUpgrades.upgrades.Add(list66);
		List<string> list67 = new List<string>();
		list67.Add(WeaponTags.Dater_DJ_Tag);
		list67.Add(WeaponTags.Dater_DJ_2_Tag);
		list67.Add(WeaponTags.Dater_DJ_3_Tag);
		WeaponUpgrades.upgrades.Add(list67);
		List<string> list68 = new List<string>();
		list68.Add(WeaponTags.Dater_Flowers_Tag);
		list68.Add(WeaponTags.Dater_Flowers_2_Tag);
		list68.Add(WeaponTags.Dater_Flowers_3_Tag);
		WeaponUpgrades.upgrades.Add(list68);
		List<string> list69 = new List<string>();
		list69.Add(WeaponTags.Balloon_Cannon_Tag);
		list69.Add(WeaponTags.Balloon_Cannon_2_Tag);
		list69.Add(WeaponTags.Balloon_Cannon_3_Tag);
		WeaponUpgrades.upgrades.Add(list69);
		List<string> list70 = new List<string>();
		list70.Add(WeaponTags.Fireworks_Launcher_Tag);
		list70.Add(WeaponTags.Fireworks_Launcher_2_Tag);
		list70.Add(WeaponTags.Fireworks_Launcher_3_Tag);
		WeaponUpgrades.upgrades.Add(list70);
		List<string> list71 = new List<string>();
		list71.Add(WeaponTags.PumpkinGun_1_Tag);
		list71.Add(WeaponTags.PumpkinGun_2_Tag);
		list71.Add(WeaponTags.PumpkinGun_5_Tag);
		WeaponUpgrades.upgrades.Add(list71);
		List<string> list72 = new List<string>();
		list72.Add(WeaponTags.Laser_Crossbow_Tag);
		list72.Add(WeaponTags.Laser_Crossbow2_Tag);
		list72.Add(WeaponTags.Laser_Crossbow3_Tag);
		WeaponUpgrades.upgrades.Add(list72);
		List<string> list73 = new List<string>();
		list73.Add(WeaponTags.SPACE_RIFLE_Tag);
		list73.Add(WeaponTags.SPACE_RIFLE_UP1_Tag);
		list73.Add(WeaponTags.SPACE_RIFLE_UP2_Tag);
		WeaponUpgrades.upgrades.Add(list73);
		List<string> list74 = new List<string>();
		list74.Add(WeaponTags.Nutcracker_Tag);
		list74.Add(WeaponTags.Nutcracker2_Tag);
		list74.Add(WeaponTags.Nutcracker3_Tag);
		WeaponUpgrades.upgrades.Add(list74);
		List<string> list75 = new List<string>();
		list75.Add(WeaponTags.Shuriken_Thrower_Tag);
		list75.Add(WeaponTags.Shuriken_Thrower2_Tag);
		list75.Add(WeaponTags.Shuriken_Thrower3_Tag);
		WeaponUpgrades.upgrades.Add(list75);
		List<string> list76 = new List<string>();
		list76.Add(WeaponTags.Icicle_Generator_Tag);
		list76.Add(WeaponTags.Icicle_Generator2_Tag);
		list76.Add(WeaponTags.Icicle_Generator3_Tag);
		WeaponUpgrades.upgrades.Add(list76);
		List<string> list77 = new List<string>();
		list77.Add(WeaponTags.Snowball_Tag);
		list77.Add(WeaponTags.Snowball2_Tag);
		list77.Add(WeaponTags.Snowball3_Tag);
		WeaponUpgrades.upgrades.Add(list77);
		List<string> list78 = new List<string>();
		list78.Add(WeaponTags.PORTABLE_DEATH_MOON_Tag);
		list78.Add(WeaponTags.PORTABLE_DEATH_MOON_UP1_Tag);
		list78.Add(WeaponTags.PORTABLE_DEATH_MOON_UP2_Tag);
		WeaponUpgrades.upgrades.Add(list78);
		List<string> list79 = new List<string>();
		list79.Add(WeaponTags.MysticOreEmitter_Tag);
		list79.Add(WeaponTags.MysticOreEmitter_UP1_Tag);
		list79.Add(WeaponTags.MysticOreEmitter_UP2_Tag);
		WeaponUpgrades.upgrades.Add(list79);
		List<string> list80 = new List<string>();
		list80.Add(WeaponTags.Hockey_stick_Tag);
		list80.Add(WeaponTags.Hockey_stick_UP1_Tag);
		list80.Add(WeaponTags.Hockey_stick_UP2_Tag);
		WeaponUpgrades.upgrades.Add(list80);
		List<string> list81 = new List<string>();
		list81.Add(WeaponTags.Space_blaster_Tag);
		list81.Add(WeaponTags.Space_blaster_UP1_Tag);
		list81.Add(WeaponTags.Space_blaster_UP2_Tag);
		WeaponUpgrades.upgrades.Add(list81);
		List<string> list82 = new List<string>();
		list82.Add(WeaponTags.Dynamite_Gun_1_Tag);
		list82.Add(WeaponTags.Dynamite_Gun_2_Tag);
		list82.Add(WeaponTags.Dynamite_Gun_3_Tag);
		WeaponUpgrades.upgrades.Add(list82);
		List<string> list83 = new List<string>();
		list83.Add(WeaponTags.Dual_shotguns_1_Tag);
		list83.Add(WeaponTags.Dual_shotguns_2_Tag);
		list83.Add(WeaponTags.Dual_shotguns_3_Tag);
		WeaponUpgrades.upgrades.Add(list83);
		List<string> list84 = new List<string>();
		list84.Add(WeaponTags.Antihero_Rifle_1_Tag);
		list84.Add(WeaponTags.Antihero_Rifle_2_Tag);
		list84.Add(WeaponTags.Antihero_Rifle_3_Tag);
		WeaponUpgrades.upgrades.Add(list84);
		List<string> list85 = new List<string>();
		list85.Add(WeaponTags.HarpoonGun_1_Tag);
		list85.Add(WeaponTags.HarpoonGun_2_Tag);
		list85.Add(WeaponTags.HarpoonGun_3_Tag);
		WeaponUpgrades.upgrades.Add(list85);
		List<string> list86 = new List<string>();
		list86.Add(WeaponTags.Red_twins_pistols_1_Tag);
		list86.Add(WeaponTags.Red_twins_pistols_2_Tag);
		list86.Add(WeaponTags.Red_twins_pistols_3_Tag);
		WeaponUpgrades.upgrades.Add(list86);
		List<string> list87 = new List<string>();
		list87.Add(WeaponTags.Toxic_sniper_rifle_1_Tag);
		list87.Add(WeaponTags.Toxic_sniper_rifle_2_Tag);
		WeaponUpgrades.upgrades.Add(list87);
		List<string> list88 = new List<string>();
		list88.Add(WeaponTags.NuclearRevolver_1_Tag);
		list88.Add(WeaponTags.NuclearRevolver_2_Tag);
		list88.Add(WeaponTags.NuclearRevolver_3_Tag);
		WeaponUpgrades.upgrades.Add(list88);
		List<string> list89 = new List<string>();
		list89.Add(WeaponTags.NAIL_MINIGUN_1_Tag);
		list89.Add(WeaponTags.NAIL_MINIGUN_2_Tag);
		list89.Add(WeaponTags.NAIL_MINIGUN_3_Tag);
		WeaponUpgrades.upgrades.Add(list89);
		List<string> list90 = new List<string>();
		list90.Add(WeaponTags.DUAL_MACHETE_1_Tag);
		list90.Add(WeaponTags.DUAL_MACHETE_2_Tag);
		list90.Add(WeaponTags.DUAL_MACHETE_3_Tag);
		WeaponUpgrades.upgrades.Add(list90);
		List<string> list91 = new List<string>();
		list91.Add(WeaponTags.Fighter_1_Tag);
		list91.Add(WeaponTags.Fighter_2_Tag);
		WeaponUpgrades.upgrades.Add(list91);
		List<string> list92 = new List<string>();
		list92.Add(WeaponTags.Gas_spreader_Tag);
		list92.Add(WeaponTags.Gas_spreader_up1_Tag);
		WeaponUpgrades.upgrades.Add(list92);
		List<string> list93 = new List<string>();
		list93.Add(WeaponTags.LaserBouncer_1_Tag);
		list93.Add(WeaponTags.LaserBouncer_2_Tag);
		WeaponUpgrades.upgrades.Add(list93);
		List<string> list94 = new List<string>();
		list94.Add(WeaponTags.magicbook_fireball_Tag);
		list94.Add(WeaponTags.magicbook_fireball_2_Tag);
		list94.Add(WeaponTags.magicbook_fireball_3_Tag);
		WeaponUpgrades.upgrades.Add(list94);
		List<string> list95 = new List<string>();
		list95.Add(WeaponTags.magicbook_frostbeam_Tag);
		list95.Add(WeaponTags.magicbook_frostbeam_2_Tag);
		WeaponUpgrades.upgrades.Add(list95);
		List<string> list96 = new List<string>();
		list96.Add(WeaponTags.magicbook_thunder_Tag);
		list96.Add(WeaponTags.magicbook_thunder_2_Tag);
		WeaponUpgrades.upgrades.Add(list96);
		List<string> list97 = new List<string>();
		list97.Add(WeaponTags.TurboPistols_1_Tag);
		list97.Add(WeaponTags.TurboPistols_2_Tag);
		list97.Add(WeaponTags.TurboPistols_3_Tag);
		WeaponUpgrades.upgrades.Add(list97);
		List<string> list98 = new List<string>();
		list98.Add(WeaponTags.Laser_Bow_1_Tag);
		list98.Add(WeaponTags.Laser_Bow_2_Tag);
		WeaponUpgrades.upgrades.Add(list98);
		List<string> list99 = new List<string>();
		list99.Add(WeaponTags.loud_piggy_Tag);
		list99.Add(WeaponTags.loud_piggy_up1_Tag);
		WeaponUpgrades.upgrades.Add(list99);
		List<string> list100 = new List<string>();
		list100.Add(WeaponTags.Trapper_1_Tag);
		list100.Add(WeaponTags.Trapper_2_Tag);
		WeaponUpgrades.upgrades.Add(list100);
		List<string> list101 = new List<string>();
		list101.Add(WeaponTags.chainsaw_sword_1_Tag);
		list101.Add(WeaponTags.chainsaw_sword_2_Tag);
		WeaponUpgrades.upgrades.Add(list101);
		List<string> list102 = new List<string>();
		list102.Add(WeaponTags.dark_star_Tag);
		list102.Add(WeaponTags.dark_star_up1_Tag);
		WeaponUpgrades.upgrades.Add(list102);
		List<string> list103 = new List<string>();
		list103.Add(WeaponTags.toy_bomber_Tag);
		list103.Add(WeaponTags.toy_bomber_up1_Tag);
		list103.Add(WeaponTags.toy_bomber_up2_Tag);
		WeaponUpgrades.upgrades.Add(list103);
		List<string> list104 = new List<string>();
		list104.Add(WeaponTags.zombie_head_Tag);
		list104.Add(WeaponTags.zombie_head_up1_Tag);
		list104.Add(WeaponTags.zombie_head_up2_Tag);
		WeaponUpgrades.upgrades.Add(list104);
		List<string> list105 = new List<string>();
		list105.Add(WeaponTags.mr_squido_Tag);
		list105.Add(WeaponTags.mr_squido_up1_Tag);
		list105.Add(WeaponTags.mr_squido_up2_Tag);
		WeaponUpgrades.upgrades.Add(list105);
		List<string> list106 = new List<string>();
		list106.Add(WeaponTags.RocketCrossbow_Tag);
		list106.Add(WeaponTags.RocketCrossbow_up1_Tag);
		list106.Add(WeaponTags.RocketCrossbow_up2_Tag);
		WeaponUpgrades.upgrades.Add(list106);
		List<string> list107 = new List<string>();
		list107.Add(WeaponTags.zombie_slayer_Tag);
		list107.Add(WeaponTags.zombie_slayer_up1_Tag);
		list107.Add(WeaponTags.zombie_slayer_up2_Tag);
		WeaponUpgrades.upgrades.Add(list107);
		List<string> list108 = new List<string>();
		list108.Add(WeaponTags.AcidCannon_Tag);
		list108.Add(WeaponTags.AcidCannon_up1_Tag);
		list108.Add(WeaponTags.AcidCannon_up2_Tag);
		WeaponUpgrades.upgrades.Add(list108);
		List<string> list109 = new List<string>();
		list109.Add(WeaponTags.frank_sheepone_Tag);
		list109.Add(WeaponTags.frank_sheepone_up1_Tag);
		list109.Add(WeaponTags.frank_sheepone_up2_Tag);
		WeaponUpgrades.upgrades.Add(list109);
		List<string> list110 = new List<string>();
		list110.Add(WeaponTags.Ghost_Lantern_Tag);
		list110.Add(WeaponTags.Ghost_Lantern_up1_Tag);
		WeaponUpgrades.upgrades.Add(list110);
		List<string> list111 = new List<string>();
		list111.Add(WeaponTags.autoaim_bazooka_Tag);
		list111.Add(WeaponTags.autoaim_bazooka_up1_Tag);
		WeaponUpgrades.upgrades.Add(list111);
		List<string> list112 = new List<string>();
		list112.Add(WeaponTags.Semiauto_sniper_Tag);
		list112.Add(WeaponTags.Semiauto_sniper_up1_Tag);
		WeaponUpgrades.upgrades.Add(list112);
		List<string> list113 = new List<string>();
		list113.Add(WeaponTags.Chain_electro_cannon_Tag);
		list113.Add(WeaponTags.Chain_electro_cannon_up1_Tag);
		WeaponUpgrades.upgrades.Add(list113);
		List<string> list114 = new List<string>();
		list114.Add(WeaponTags.Demoman_Tag);
		list114.Add(WeaponTags.Demoman_up1_Tag);
		WeaponUpgrades.upgrades.Add(list114);
		List<string> list115 = new List<string>();
		list115.Add(WeaponTags.Barier_Generator_Tag);
		list115.Add(WeaponTags.Barier_Generator_up1_Tag);
		list115.Add(WeaponTags.Barier_Generator_up2_Tag);
		WeaponUpgrades.upgrades.Add(list115);
		List<string> list116 = new List<string>();
		list116.Add(WeaponTags.charge_rifle_Tag);
		list116.Add(WeaponTags.charge_rifle_UP1_Tag);
		WeaponUpgrades.upgrades.Add(list116);
		List<string> list117 = new List<string>();
		list117.Add(WeaponTags.Bee_Swarm_Spell_Tag);
		list117.Add(WeaponTags.Bee_Swarm_Spell_up1_Tag);
		list117.Add(WeaponTags.Bee_Swarm_Spell_up2_Tag);
		WeaponUpgrades.upgrades.Add(list117);
		List<string> list118 = new List<string>();
		list118.Add(WeaponTags.minigun_pistol_Tag);
		list118.Add(WeaponTags.minigun_pistol_up1_Tag);
		list118.Add(WeaponTags.minigun_pistol_up2_Tag);
		WeaponUpgrades.upgrades.Add(list118);
		List<string> list119 = new List<string>();
		list119.Add(WeaponTags.bad_doctor_1_Tag);
		list119.Add(WeaponTags.bad_doctor_2_Tag);
		list119.Add(WeaponTags.bad_doctor_3_Tag);
		WeaponUpgrades.upgrades.Add(list119);
		List<string> list120 = new List<string>();
		list120.Add(WeaponTags.toxic_bane_Tag);
		list120.Add(WeaponTags.toxic_bane_up1_Tag);
		list120.Add(WeaponTags.toxic_bane_up2_Tag);
		WeaponUpgrades.upgrades.Add(list120);
		List<string> list121 = new List<string>();
		list121.Add(WeaponTags.dual_laser_blasters_Tag);
		list121.Add(WeaponTags.dual_laser_blasters_up1_Tag);
		WeaponUpgrades.upgrades.Add(list121);
		List<string> list122 = new List<string>();
		list122.Add(WeaponTags.Heavy_Shocker_Tag);
		list122.Add(WeaponTags.Heavy_Shocker_up1_Tag);
		list122.Add(WeaponTags.Heavy_Shocker_up2_Tag);
		WeaponUpgrades.upgrades.Add(list122);
		List<string> list123 = new List<string>();
		list123.Add(WeaponTags.Charge_Cannon_Tag);
		list123.Add(WeaponTags.Charge_Cannon_up1_Tag);
		WeaponUpgrades.upgrades.Add(list123);
		List<string> list124 = new List<string>();
		list124.Add(WeaponTags.ruler_sword_1_Tag);
		list124.Add(WeaponTags.ruler_sword_2_Tag);
		list124.Add(WeaponTags.ruler_sword_3_Tag);
		WeaponUpgrades.upgrades.Add(list124);
		List<string> list125 = new List<string>();
		list125.Add(WeaponTags.pencil_thrower_1_Tag);
		list125.Add(WeaponTags.pencil_thrower_2_Tag);
		list125.Add(WeaponTags.pencil_thrower_3_Tag);
		WeaponUpgrades.upgrades.Add(list125);
		List<string> list126 = new List<string>();
		list126.Add(WeaponTags.napalm_cannon_Tag);
		list126.Add(WeaponTags.napalm_cannon_up1_Tag);
		list126.Add(WeaponTags.napalm_cannon_up2_Tag);
		WeaponUpgrades.upgrades.Add(list126);
		List<string> list127 = new List<string>();
		list127.Add(WeaponTags.dracula_Tag);
		list127.Add(WeaponTags.dracula_up1_Tag);
		list127.Add(WeaponTags.dracula_up2_Tag);
		WeaponUpgrades.upgrades.Add(list127);
		List<string> list128 = new List<string>();
		list128.Add(WeaponTags.sword_of_shadows_Tag);
		list128.Add(WeaponTags.sword_of_shadows_up1_Tag);
		list128.Add(WeaponTags.sword_of_shadows_up2_Tag);
		WeaponUpgrades.upgrades.Add(list128);
		List<string> list129 = new List<string>();
		list129.Add(WeaponTags.xmas_destroyer_Tag);
		list129.Add(WeaponTags.xmas_destroyer_up1_Tag);
		list129.Add(WeaponTags.xmas_destroyer_up2_Tag);
		WeaponUpgrades.upgrades.Add(list129);
		List<string> list130 = new List<string>();
		list130.Add(WeaponTags.santa_sword_Tag);
		list130.Add(WeaponTags.santa_sword_up1_Tag);
		list130.Add(WeaponTags.santa_sword_up2_Tag);
		WeaponUpgrades.upgrades.Add(list130);
		List<string> list131 = new List<string>();
		list131.Add(WeaponTags.heavy_gifter_Tag);
		list131.Add(WeaponTags.heavy_gifter_up1_Tag);
		list131.Add(WeaponTags.heavy_gifter_up2_Tag);
		WeaponUpgrades.upgrades.Add(list131);
		List<string> list132 = new List<string>();
		list132.Add(WeaponTags.snow_storm_Tag);
		list132.Add(WeaponTags.snow_storm_up1_Tag);
		list132.Add(WeaponTags.snow_storm_up2_Tag);
		WeaponUpgrades.upgrades.Add(list132);
		List<string> list133 = new List<string>();
		list133.Add(WeaponTags.bell_revolver_Tag);
		list133.Add(WeaponTags.bell_revolver_up1_Tag);
		WeaponUpgrades.upgrades.Add(list133);
		List<string> list134 = new List<string>();
		list134.Add(WeaponTags.elfs_revenge_Tag);
		list134.Add(WeaponTags.elfs_revenge_up1_Tag);
		list134.Add(WeaponTags.elfs_revenge_up2_Tag);
		WeaponUpgrades.upgrades.Add(list134);
		List<string> list135 = new List<string>();
		list135.Add(WeaponTags.subzero_Tag);
		list135.Add(WeaponTags.subzero_up1_Tag);
		list135.Add(WeaponTags.subzero_up2_Tag);
		WeaponUpgrades.upgrades.Add(list135);
		List<string> list136 = new List<string>();
		list136.Add(WeaponTags.photon_sniper_rifle_Tag);
		list136.Add(WeaponTags.photon_sniper_rifle_up1_Tag);
		list136.Add(WeaponTags.photon_sniper_rifle_up2_Tag);
		WeaponUpgrades.upgrades.Add(list136);
		List<string> list137 = new List<string>();
		list137.Add(WeaponTags.mercenary_Tag);
		list137.Add(WeaponTags.mercenary_up1_Tag);
		list137.Add(WeaponTags.mercenary_up2_Tag);
		WeaponUpgrades.upgrades.Add(list137);
	}

	// Token: 0x06004B71 RID: 19313 RVA: 0x001B252C File Offset: 0x001B072C
	public static string TagOfFirstUpgrade(string tg)
	{
		if (tg == null)
		{
			return null;
		}
		int count = WeaponUpgrades.upgrades.Count;
		for (int num = 0; num != count; num++)
		{
			List<string> list = WeaponUpgrades.upgrades[num];
			if (list.Contains(tg))
			{
				return list[0];
			}
		}
		return tg;
	}

	// Token: 0x06004B72 RID: 19314 RVA: 0x001B2580 File Offset: 0x001B0780
	public static List<string> ChainForTag(string tg)
	{
		if (tg == null)
		{
			return null;
		}
		int count = WeaponUpgrades.upgrades.Count;
		for (int num = 0; num != count; num++)
		{
			List<string> list = WeaponUpgrades.upgrades[num];
			if (list.Contains(tg))
			{
				return list;
			}
		}
		return null;
	}

	// Token: 0x04003A92 RID: 14994
	public static readonly List<List<string>> upgrades = new List<List<string>>();
}
