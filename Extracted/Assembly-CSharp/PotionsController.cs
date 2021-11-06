using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000702 RID: 1794
public sealed class PotionsController : MonoBehaviour
{
	// Token: 0x06003E42 RID: 15938 RVA: 0x0014DDDC File Offset: 0x0014BFDC
	static PotionsController()
	{
		PotionsController.potionMethods.Add(PotionsController.HastePotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.HastePotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.HastePotionDeactivation)));
		PotionsController.potionMethods.Add(PotionsController.MightPotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionDeactivation)));
		PotionsController.potionMethods.Add("InvisibilityPotion", new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.InvisibilityPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.InvisibilityPotionDeactivation)));
		PotionsController.potionMethods.Add(PotionsController.RegenerationPotion, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Jetpack, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MightPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Turret, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.RegenerationPotionActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.TurretPotionDeactivation)));
		PotionsController.potionMethods.Add(GearManager.Mech, new KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>(new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MechActivation), new Action<Player_move_c, Dictionary<string, object>>(PotionsController.MechDeactivation)));
		PotionsController.potionDurations.Add(PotionsController.HastePotion, 180f);
		PotionsController.potionDurations.Add(PotionsController.MightPotion, 60f);
		PotionsController.potionDurations.Add(PotionsController.RegenerationPotion, 300f);
		PotionsController.potionDurations.Add("InvisibilityPotion0", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "0", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "0", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "0", 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion1", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "1", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "1", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "1", 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion2", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "2", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "2", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "2", 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion3", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "3", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "3", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "3", 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion4", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "4", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "4", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "4", 30f);
		PotionsController.potionDurations.Add("InvisibilityPotion5", 30f);
		PotionsController.potionDurations.Add(GearManager.Turret + "5", 60f);
		PotionsController.potionDurations.Add(GearManager.Jetpack + "5", 60f);
		PotionsController.potionDurations.Add(GearManager.Mech + "5", 30f);
	}

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06003E43 RID: 15939 RVA: 0x0014E25C File Offset: 0x0014C45C
	// (remove) Token: 0x06003E44 RID: 15940 RVA: 0x0014E274 File Offset: 0x0014C474
	public static event Action<string> PotionActivated;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06003E45 RID: 15941 RVA: 0x0014E28C File Offset: 0x0014C48C
	// (remove) Token: 0x06003E46 RID: 15942 RVA: 0x0014E2A4 File Offset: 0x0014C4A4
	public static event Action<string> PotionDisactivated;

	// Token: 0x06003E47 RID: 15943 RVA: 0x0014E2BC File Offset: 0x0014C4BC
	public bool PotionIsActive(string nm)
	{
		return nm != null && this.activePotions != null && this.activePotions.ContainsKey(nm);
	}

	// Token: 0x06003E48 RID: 15944 RVA: 0x0014E2EC File Offset: 0x0014C4EC
	public static void HastePotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c._player && move_c._player != null)
		{
			FirstPersonControlSharp component = move_c._player.GetComponent<FirstPersonControlSharp>();
			if (component && component != null)
			{
				component.gravityMultiplier *= PotionsController.AntiGravityMult;
			}
		}
	}

	// Token: 0x06003E49 RID: 15945 RVA: 0x0014E350 File Offset: 0x0014C550
	public static void HastePotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c._player && move_c._player != null)
		{
			FirstPersonControlSharp component = move_c._player.GetComponent<FirstPersonControlSharp>();
			if (component && component != null)
			{
				component.gravityMultiplier /= PotionsController.AntiGravityMult;
			}
		}
	}

	// Token: 0x06003E4A RID: 15946 RVA: 0x0014E3B4 File Offset: 0x0014C5B4
	public static void MightPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		GameObject gameObject;
		if (Defs.isMulti)
		{
			gameObject = WeaponManager.sharedManager.myPlayer;
		}
		else
		{
			gameObject = GameObject.FindGameObjectWithTag("Player");
		}
		if (gameObject != null)
		{
			gameObject.GetComponent<SkinName>().playerMoveC.SetJetpackEnabled(true);
		}
	}

	// Token: 0x06003E4B RID: 15947 RVA: 0x0014E408 File Offset: 0x0014C608
	public static void MightPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		GameObject gameObject;
		if (Defs.isMulti)
		{
			gameObject = WeaponManager.sharedManager.myPlayer;
		}
		else
		{
			gameObject = GameObject.FindGameObjectWithTag("Player");
		}
		if (gameObject != null)
		{
			gameObject.GetComponent<SkinName>().playerMoveC.SetJetpackEnabled(false);
		}
	}

	// Token: 0x06003E4C RID: 15948 RVA: 0x0014E45C File Offset: 0x0014C65C
	public static void RegenerationPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	// Token: 0x06003E4D RID: 15949 RVA: 0x0014E460 File Offset: 0x0014C660
	public static void RegenerationPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	// Token: 0x06003E4E RID: 15950 RVA: 0x0014E464 File Offset: 0x0014C664
	public static void TurretPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		Player_move_c player_move_c = null;
		if (Defs.isMulti)
		{
			player_move_c = WeaponManager.sharedManager.myPlayerMoveC;
		}
		else
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				player_move_c = gameObject.GetComponent<SkinName>().playerMoveC;
			}
		}
		if (player_move_c == null || player_move_c.currentTurret == null)
		{
			return;
		}
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				PhotonNetwork.Destroy(player_move_c.currentTurret);
			}
			else
			{
				Network.Destroy(player_move_c.currentTurret);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(player_move_c.currentTurret);
		}
	}

	// Token: 0x06003E4F RID: 15951 RVA: 0x0014E510 File Offset: 0x0014C710
	public static void NightVisionPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c.inGameGUI && move_c.inGameGUI.nightVisionEffect != null)
		{
			move_c.inGameGUI.nightVisionEffect.SetActive(true);
		}
	}

	// Token: 0x06003E50 RID: 15952 RVA: 0x0014E54C File Offset: 0x0014C74C
	public static void NightVisionPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		if (move_c.inGameGUI && move_c.inGameGUI.nightVisionEffect != null)
		{
			move_c.inGameGUI.nightVisionEffect.SetActive(false);
		}
	}

	// Token: 0x06003E51 RID: 15953 RVA: 0x0014E588 File Offset: 0x0014C788
	public static void InvisibilityPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		move_c.SetInvisible(true, false);
	}

	// Token: 0x06003E52 RID: 15954 RVA: 0x0014E594 File Offset: 0x0014C794
	public static void InvisibilityPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
		move_c.SetInvisible(false, false);
	}

	// Token: 0x17000A5A RID: 2650
	// (get) Token: 0x06003E53 RID: 15955 RVA: 0x0014E5A0 File Offset: 0x0014C7A0
	public static float AntiGravityMult
	{
		get
		{
			return 0.75f;
		}
	}

	// Token: 0x06003E54 RID: 15956 RVA: 0x0014E5A8 File Offset: 0x0014C7A8
	public static void AntiGravityPotionActivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	// Token: 0x06003E55 RID: 15957 RVA: 0x0014E5AC File Offset: 0x0014C7AC
	public static void AntiGravityPotionDeactivation(Player_move_c move_c, Dictionary<string, object> pars)
	{
	}

	// Token: 0x06003E56 RID: 15958 RVA: 0x0014E5B0 File Offset: 0x0014C7B0
	private static void MechActivation(Player_move_c arg1, Dictionary<string, object> arg2)
	{
		Debug.Log("Mech ON");
		arg1.ActivateMech(string.Empty);
	}

	// Token: 0x06003E57 RID: 15959 RVA: 0x0014E5C8 File Offset: 0x0014C7C8
	private static void MechDeactivation(Player_move_c arg1, Dictionary<string, object> arg2)
	{
		Debug.Log("Mech OFF");
		arg1.DeactivateMech();
	}

	// Token: 0x06003E58 RID: 15960 RVA: 0x0014E5DC File Offset: 0x0014C7DC
	public float RemainDuratioForPotion(string potion)
	{
		if (potion == null || !this.activePotions.ContainsKey(potion))
		{
			return 0f;
		}
		return this.activePotions[potion] + EffectsController.AddingForPotionDuration(potion);
	}

	// Token: 0x06003E59 RID: 15961 RVA: 0x0014E61C File Offset: 0x0014C81C
	public void ReactivatePotions(Player_move_c move_c, Dictionary<string, object> pars)
	{
		foreach (string potion in this.activePotions.Keys)
		{
			this.ActivatePotion(potion, move_c, pars, false);
		}
	}

	// Token: 0x06003E5A RID: 15962 RVA: 0x0014E68C File Offset: 0x0014C88C
	public void ActivatePotion(string potion, Player_move_c move_c, Dictionary<string, object> pars, bool isAddTimeOnActive = false)
	{
		if (!this.activePotions.ContainsKey(potion))
		{
			this.activePotions.Add(potion, (!Defs.isDaterRegim) ? PotionsController.potionDurations[potion + GearManager.CurrentNumberOfUphradesForGear(potion).ToString()] : 180f);
			this.activePotionsList.Add(potion);
		}
		else if (isAddTimeOnActive)
		{
			this.activePotions.Remove(potion);
			this.activePotions.Add(potion, (!Defs.isDaterRegim) ? PotionsController.potionDurations[potion + GearManager.CurrentNumberOfUphradesForGear(potion).ToString()] : 180f);
			this.activePotionsList.Remove(potion);
			this.activePotionsList.Add(potion);
			if (TableGearController.sharedController != null)
			{
				TableGearController.sharedController.ReactivatePotion(potion);
			}
		}
		if (PotionsController.potionMethods.ContainsKey(potion))
		{
			PotionsController.potionMethods[potion].Key(move_c, pars);
		}
		if (PotionsController.PotionActivated != null)
		{
			PotionsController.PotionActivated(potion);
		}
	}

	// Token: 0x06003E5B RID: 15963 RVA: 0x0014E7C0 File Offset: 0x0014C9C0
	public void Step(float tm, Player_move_c p)
	{
		if (this._stepPotionsToRemove == null)
		{
			this._stepPotionsToRemove = new List<string>();
		}
		else
		{
			this._stepPotionsToRemove.Clear();
		}
		if (this._stepActivePotionKeys == null)
		{
			this._stepActivePotionKeys = new List<string>();
		}
		else
		{
			this._stepActivePotionKeys.Clear();
		}
		Dictionary<string, float>.Enumerator enumerator = this.activePotions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, float> keyValuePair = enumerator.Current;
			this._stepActivePotionKeys.Add(keyValuePair.Key);
		}
		enumerator.Dispose();
		int count = this._stepActivePotionKeys.Count;
		for (int i = 0; i < count; i++)
		{
			string text = this._stepActivePotionKeys[i];
			Dictionary<string, float> dictionary2;
			Dictionary<string, float> dictionary = dictionary2 = this.activePotions;
			string key2;
			string key = key2 = text;
			float num = dictionary2[key2];
			dictionary[key] = num - tm;
			if (this.RemainDuratioForPotion(text) <= 0f)
			{
				this._stepPotionsToRemove.Add(text);
			}
		}
		int count2 = this._stepPotionsToRemove.Count;
		for (int j = 0; j < count2; j++)
		{
			string potion = this._stepPotionsToRemove[j];
			this.DeActivePotion(potion, p, true);
		}
		this._stepPotionsToRemove.Clear();
		this._stepActivePotionKeys.Clear();
	}

	// Token: 0x06003E5C RID: 15964 RVA: 0x0014E918 File Offset: 0x0014CB18
	public void DeActivePotion(string _potion, Player_move_c p, bool isDeleteObject = true)
	{
		if (PotionsController.PotionDisactivated != null)
		{
			PotionsController.PotionDisactivated(_potion);
		}
		if (!this.activePotions.ContainsKey(_potion))
		{
			return;
		}
		this.activePotions.Remove(_potion);
		this.activePotionsList.Remove(_potion);
		if (isDeleteObject)
		{
			PotionsController.potionMethods[_potion].Value(p, new Dictionary<string, object>());
		}
	}

	// Token: 0x06003E5D RID: 15965 RVA: 0x0014E98C File Offset: 0x0014CB8C
	private void OnLevelWasLoaded(int lev)
	{
		this.activePotions.Clear();
		this.activePotionsList.Clear();
	}

	// Token: 0x06003E5E RID: 15966 RVA: 0x0014E9A4 File Offset: 0x0014CBA4
	private void Start()
	{
		PotionsController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04002E08 RID: 11784
	public const string InvisibilityPotion = "InvisibilityPotion";

	// Token: 0x04002E09 RID: 11785
	public static string HastePotion = "HastePotion";

	// Token: 0x04002E0A RID: 11786
	public static string RegenerationPotion = "RegenerationPotion";

	// Token: 0x04002E0B RID: 11787
	public static string MightPotion = "MightPotion";

	// Token: 0x04002E0C RID: 11788
	public static int MaxNumOFPotions = 1000000;

	// Token: 0x04002E0D RID: 11789
	public static PotionsController sharedController = null;

	// Token: 0x04002E0E RID: 11790
	public static Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>> potionMethods = new Dictionary<string, KeyValuePair<Action<Player_move_c, Dictionary<string, object>>, Action<Player_move_c, Dictionary<string, object>>>>();

	// Token: 0x04002E0F RID: 11791
	public static Dictionary<string, float> potionDurations = new Dictionary<string, float>();

	// Token: 0x04002E10 RID: 11792
	public static string[] potions = new string[]
	{
		PotionsController.HastePotion,
		PotionsController.MightPotion,
		PotionsController.RegenerationPotion,
		"InvisibilityPotion"
	};

	// Token: 0x04002E11 RID: 11793
	public Dictionary<string, float> activePotions = new Dictionary<string, float>();

	// Token: 0x04002E12 RID: 11794
	public List<string> activePotionsList = new List<string>();

	// Token: 0x04002E13 RID: 11795
	private List<string> _stepPotionsToRemove;

	// Token: 0x04002E14 RID: 11796
	private List<string> _stepActivePotionKeys;
}
