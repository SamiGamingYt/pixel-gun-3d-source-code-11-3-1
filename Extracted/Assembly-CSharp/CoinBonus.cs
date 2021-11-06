using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000070 RID: 112
internal sealed class CoinBonus : MonoBehaviour
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000334 RID: 820 RVA: 0x0001B8E0 File Offset: 0x00019AE0
	// (remove) Token: 0x06000335 RID: 821 RVA: 0x0001B8F8 File Offset: 0x00019AF8
	public static event Action StartBlinkShop;

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000336 RID: 822 RVA: 0x0001B910 File Offset: 0x00019B10
	// (set) Token: 0x06000337 RID: 823 RVA: 0x0001B918 File Offset: 0x00019B18
	public VirtualCurrencyBonusType BonusType { private get; set; }

	// Token: 0x06000338 RID: 824 RVA: 0x0001B924 File Offset: 0x00019B24
	public static List<string> GetLevelsWhereGotBonus(VirtualCurrencyBonusType bonusType)
	{
		if (bonusType == VirtualCurrencyBonusType.Coin)
		{
			string[] source = Storager.getString(Defs.LevelsWhereGetCoinS, false).Split(new char[]
			{
				'#'
			}, StringSplitOptions.RemoveEmptyEntries);
			return source.ToList<string>();
		}
		if (bonusType != VirtualCurrencyBonusType.Gem)
		{
			return new List<string>();
		}
		List<object> list = Json.Deserialize(Storager.getString(Defs.LevelsWhereGotGems, false)) as List<object>;
		if (list == null)
		{
			return new List<string>();
		}
		return list.OfType<string>().ToList<string>();
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0001B9A0 File Offset: 0x00019BA0
	internal static string[] GetLevelsWhereGotCoins()
	{
		string @string = Storager.getString(Defs.LevelsWhereGetCoinS, false);
		return @string.Split(new char[]
		{
			'#'
		}, StringSplitOptions.RemoveEmptyEntries);
	}

	// Token: 0x0600033A RID: 826 RVA: 0x0001B9D0 File Offset: 0x00019BD0
	internal static IEnumerable<string> GetLevelsWhereGotGems()
	{
		string @string = Storager.getString(Defs.LevelsWhereGotGems, false);
		if (!(Json.Deserialize(@string) is List<object>))
		{
			return new string[0];
		}
		return @string.OfType<string>();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x0001BA0C File Offset: 0x00019C0C
	public static bool SetLevelsWhereGotBonus(string[] levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string levelsWhereGotBonusSerialized = string.Empty;
		if (bonusType != VirtualCurrencyBonusType.Coin)
		{
			if (bonusType == VirtualCurrencyBonusType.Gem)
			{
				levelsWhereGotBonusSerialized = Json.Serialize(levelsWhereGotBonus);
			}
		}
		else
		{
			levelsWhereGotBonusSerialized = string.Join("#", levelsWhereGotBonus);
		}
		return CoinBonus.SetLevelsWhereGotBonus(levelsWhereGotBonusSerialized, bonusType);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x0001BA6C File Offset: 0x00019C6C
	public static bool SetLevelsWhereGotBonus(List<string> levelsWhereGotBonus, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonus == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonus");
		}
		string levelsWhereGotBonusSerialized = string.Empty;
		if (bonusType != VirtualCurrencyBonusType.Coin)
		{
			if (bonusType == VirtualCurrencyBonusType.Gem)
			{
				levelsWhereGotBonusSerialized = Json.Serialize(levelsWhereGotBonus);
			}
		}
		else
		{
			levelsWhereGotBonusSerialized = string.Join("#", levelsWhereGotBonus.ToArray());
		}
		return CoinBonus.SetLevelsWhereGotBonus(levelsWhereGotBonusSerialized, bonusType);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x0001BAD0 File Offset: 0x00019CD0
	internal static bool SetLevelsWhereGotBonus(string levelsWhereGotBonusSerialized, VirtualCurrencyBonusType bonusType)
	{
		if (levelsWhereGotBonusSerialized == null)
		{
			throw new ArgumentNullException("levelsWhereGotBonusAsString");
		}
		if (bonusType == VirtualCurrencyBonusType.Coin)
		{
			Storager.setString(Defs.LevelsWhereGetCoinS, levelsWhereGotBonusSerialized, false);
			return true;
		}
		if (bonusType != VirtualCurrencyBonusType.Gem)
		{
			return false;
		}
		Storager.setString(Defs.LevelsWhereGotGems, levelsWhereGotBonusSerialized, false);
		return true;
	}

	// Token: 0x0600033E RID: 830 RVA: 0x0001BB20 File Offset: 0x00019D20
	public void SetPlayer()
	{
		this.test = GameObject.FindGameObjectWithTag("PlayerGun").GetComponent<Player_move_c>();
		this.player = GameObject.FindGameObjectWithTag("Player");
	}

	// Token: 0x0600033F RID: 831 RVA: 0x0001BB48 File Offset: 0x00019D48
	private void Update()
	{
		if (this.test == null || this.player == null)
		{
			return;
		}
		if (this.BonusType == VirtualCurrencyBonusType.None)
		{
			return;
		}
		if (Vector3.SqrMagnitude(base.transform.position - this.player.transform.position) > 2.25f)
		{
			return;
		}
		try
		{
			if (TrainingController.TrainingCompleted || TrainingController.CompletedTrainingStage > TrainingController.NewTrainingCompletedStage.None)
			{
				int num = (!(PremiumAccountController.Instance != null)) ? 1 : PremiumAccountController.Instance.RewardCoeff;
				if (!Defs.IsSurvival && !Defs.isMulti)
				{
					num = 1;
				}
				VirtualCurrencyBonusType bonusType = this.BonusType;
				if (bonusType != VirtualCurrencyBonusType.Coin)
				{
					if (bonusType == VirtualCurrencyBonusType.Gem)
					{
						int @int = Storager.getInt("GemsCurrency", false);
						Storager.setInt("GemsCurrency", @int + 1 * num, false);
						AnalyticsFacade.CurrencyAccrual(1 * num, "GemsCurrency", AnalyticsConstants.AccrualType.Earned);
					}
				}
				else
				{
					int int2 = Storager.getInt("Coins", false);
					Storager.setInt("Coins", int2 + 1 * num, false);
					AnalyticsFacade.CurrencyAccrual(1 * num, "Coins", AnalyticsConstants.AccrualType.Earned);
				}
				if (Application.platform != RuntimePlatform.IPhonePlayer)
				{
					PlayerPrefs.Save();
				}
			}
			CoinsMessage.FireCoinsAddedEvent(this.BonusType == VirtualCurrencyBonusType.Gem, 1);
			if (!this.test.isSurvival && TrainingController.TrainingCompleted)
			{
				List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(this.BonusType);
				string name = SceneManager.GetActiveScene().name;
				if (!levelsWhereGotBonus.Contains(name))
				{
					levelsWhereGotBonus.Add(name);
					CoinBonus.SetLevelsWhereGotBonus(levelsWhereGotBonus, this.BonusType);
				}
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingController.isNextStep = TrainingState.GetTheCoin;
				if (CoinBonus.StartBlinkShop != null)
				{
					CoinBonus.StartBlinkShop();
				}
			}
		}
		finally
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000376 RID: 886
	public GameObject player;

	// Token: 0x04000377 RID: 887
	public AudioClip CoinItemUpAudioClip;

	// Token: 0x04000378 RID: 888
	private Player_move_c test;
}
