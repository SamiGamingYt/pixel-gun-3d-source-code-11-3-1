using System;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020004B3 RID: 1203
public sealed class RatingSystem : MonoBehaviour
{
	// Token: 0x14000039 RID: 57
	// (add) Token: 0x06002B3C RID: 11068 RVA: 0x000E4200 File Offset: 0x000E2400
	// (remove) Token: 0x06002B3D RID: 11069 RVA: 0x000E4218 File Offset: 0x000E2418
	public static event Action OnRatingUpdate;

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x06002B3E RID: 11070 RVA: 0x000E4230 File Offset: 0x000E2430
	public static RatingSystem instance
	{
		get
		{
			if (RatingSystem._instance == null)
			{
				GameObject gameObject = new GameObject("RatingSystem");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				RatingSystem._instance = gameObject.AddComponent<RatingSystem>();
			}
			return RatingSystem._instance;
		}
	}

	// Token: 0x17000776 RID: 1910
	// (get) Token: 0x06002B3F RID: 11071 RVA: 0x000E4270 File Offset: 0x000E2470
	private int form_hungerCoef
	{
		get
		{
			return this.hungerLeagueCoefs[Mathf.Min((int)(this.currentLeague * RatingSystem.RatingLeague.Crystal + this.currentDivision), this.hungerLeagueCoefs.Length - 1)];
		}
	}

	// Token: 0x17000777 RID: 1911
	// (get) Token: 0x06002B40 RID: 11072 RVA: 0x000E4298 File Offset: 0x000E2498
	private int form_coef
	{
		get
		{
			return this.leagueCoefs[Mathf.Min((int)(this.currentLeague * RatingSystem.RatingLeague.Crystal + this.currentDivision), this.leagueCoefs.Length - 1)];
		}
	}

	// Token: 0x17000778 RID: 1912
	// (get) Token: 0x06002B41 RID: 11073 RVA: 0x000E42C0 File Offset: 0x000E24C0
	private int form_m_coef
	{
		get
		{
			return this.leagueMCoefs[Mathf.Min((int)(this.currentLeague * RatingSystem.RatingLeague.Crystal + this.currentDivision), this.leagueMCoefs.Length - 1)];
		}
	}

	// Token: 0x17000779 RID: 1913
	// (get) Token: 0x06002B42 RID: 11074 RVA: 0x000E42E8 File Offset: 0x000E24E8
	internal int TrophiesSeasonThreshold
	{
		get
		{
			return 3100;
		}
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x000E42F0 File Offset: 0x000E24F0
	public int RatingNeededForLeague(RatingSystem.RatingLeague league)
	{
		int num = (int)(RatingSystem.RatingLeague.Crystal * league);
		if (num >= this.leagueRatings.Length)
		{
			num = this.leagueRatings.Length - 1;
		}
		return this.leagueRatings[num];
	}

	// Token: 0x1700077A RID: 1914
	// (get) Token: 0x06002B44 RID: 11076 RVA: 0x000E4324 File Offset: 0x000E2524
	public int currentRating
	{
		get
		{
			return this.positiveRating - this.negativeRating;
		}
	}

	// Token: 0x1700077B RID: 1915
	// (get) Token: 0x06002B45 RID: 11077 RVA: 0x000E4334 File Offset: 0x000E2534
	public RatingSystem.RatingChange currentRatingChange
	{
		get
		{
			return new RatingSystem.RatingChange(this.currentLeague, this.currentDivision, this.currentRating);
		}
	}

	// Token: 0x1700077C RID: 1916
	// (get) Token: 0x06002B46 RID: 11078 RVA: 0x000E4350 File Offset: 0x000E2550
	// (set) Token: 0x06002B47 RID: 11079 RVA: 0x000E4370 File Offset: 0x000E2570
	public int positiveRating
	{
		get
		{
			if (Storager.hasKey("RatingPositive"))
			{
				return Storager.getInt("RatingPositive", false);
			}
			return 0;
		}
		set
		{
			Storager.setInt("RatingPositive", value, false);
		}
	}

	// Token: 0x1700077D RID: 1917
	// (get) Token: 0x06002B48 RID: 11080 RVA: 0x000E4380 File Offset: 0x000E2580
	// (set) Token: 0x06002B49 RID: 11081 RVA: 0x000E43B0 File Offset: 0x000E25B0
	public int positiveRatingLocal
	{
		get
		{
			return (!Storager.hasKey("RatingPositiveLocal")) ? 0 : Storager.getInt("RatingPositiveLocal", false);
		}
		set
		{
			Storager.setInt("RatingPositiveLocal", value, false);
		}
	}

	// Token: 0x1700077E RID: 1918
	// (get) Token: 0x06002B4A RID: 11082 RVA: 0x000E43C0 File Offset: 0x000E25C0
	// (set) Token: 0x06002B4B RID: 11083 RVA: 0x000E43E0 File Offset: 0x000E25E0
	public int negativeRating
	{
		get
		{
			if (Storager.hasKey("RatingNegative"))
			{
				return Storager.getInt("RatingNegative", false);
			}
			return 0;
		}
		set
		{
			Storager.setInt("RatingNegative", value, false);
		}
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x000E43F0 File Offset: 0x000E25F0
	public float GetRatingAmountForLeague(RatingSystem.RatingLeague league)
	{
		float num = (float)this.leagueRatings[Mathf.Clamp((int)(league * RatingSystem.RatingLeague.Crystal), 0, this.leagueRatings.Length - 1)];
		float num2 = (float)this.leagueRatings[Mathf.Clamp((int)((league + 1) * RatingSystem.RatingLeague.Crystal), 0, this.leagueRatings.Length - 1)];
		float a = 0.03f;
		if (this.currentRating == 0)
		{
			a = 0f;
		}
		return Mathf.Max(a, Mathf.Clamp01(((float)this.currentRating - num) / (num2 - num)));
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x000E4468 File Offset: 0x000E2668
	public int MaxRatingInDivision(RatingSystem.RatingLeague league, int division)
	{
		if (league * RatingSystem.RatingLeague.Crystal + division + 1 >= (RatingSystem.RatingLeague)this.leagueRatings.Length)
		{
			return int.MaxValue;
		}
		return this.leagueRatings[(int)(league * RatingSystem.RatingLeague.Crystal + division + 1)];
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x000E44A0 File Offset: 0x000E26A0
	public int MaxRatingInLeague(RatingSystem.RatingLeague league)
	{
		return this.MaxRatingInDivision(league, 2);
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000E44AC File Offset: 0x000E26AC
	public int DivisionInLeague(RatingSystem.RatingLeague league)
	{
		if (league < this.currentLeague)
		{
			return 2;
		}
		if (league > this.currentLeague)
		{
			return 0;
		}
		return this.currentDivision;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000E44DC File Offset: 0x000E26DC
	private void AddToRating(int rating)
	{
		if (rating > 0)
		{
			this.positiveRating += rating;
			this.positiveRatingLocal += rating;
		}
		else
		{
			this.negativeRating -= rating;
			this.negativeRating = Mathf.Min(this.negativeRating, this.positiveRating);
		}
		this.UpdateLeague(rating > 0);
		this.SaveValues();
		Debug.Log(string.Format("<color=yellow>Add {0} rating.</color>", rating));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
		TrophiesSynchronizer.Instance.Sync();
		base.StartCoroutine(FriendsController.sharedController.SynchRating(this.currentRating));
		if (RatingSystem.OnRatingUpdate != null)
		{
			RatingSystem.OnRatingUpdate();
		}
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x000E45C8 File Offset: 0x000E27C8
	public int GetRatingValueForParams(int playersCount, int place, float matchKillrate, bool deadheat = false)
	{
		int num = (matchKillrate > this.form_kd_top) ? Mathf.RoundToInt(this.form_kd_b) : Mathf.RoundToInt(matchKillrate * this.form_kd_b - this.form_kd_a);
		int num2 = Mathf.Max(playersCount - 1, 1);
		float num3 = ((float)num2 / 2f - (float)place) / ((float)num2 / this.form_place_coeff);
		int num4 = Mathf.RoundToInt((float)this.form_coef * num3);
		if (num4 >= 0 && num4 + num >= 0)
		{
			num4 += num;
		}
		if (num4 < 0)
		{
			num4 = Mathf.RoundToInt((float)(this.form_coef / this.form_m_coef) * num3);
		}
		if (deadheat)
		{
			num4 = num;
		}
		return num4;
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x000E4670 File Offset: 0x000E2870
	public RatingSystem.RatingChange CalculateRating(int playersCount, int place, float matchKillrate, bool deadheat = false)
	{
		RatingSystem.RatingChange result = this.currentRatingChange;
		int ratingValueForParams = this.GetRatingValueForParams(playersCount, place, matchKillrate, deadheat);
		this.AddToRating(ratingValueForParams);
		result = result.AddChange(this.currentLeague, this.currentDivision, this.currentRating);
		this.lastRatingChange.Value = result.addRating;
		if (result.oldLeague != result.newLeague && result.newLeague == RatingSystem.RatingLeague.Adamant)
		{
			TournamentAvailableBannerWindow.CanShow = true;
		}
		if (result.newLeague != RatingSystem.RatingLeague.Adamant)
		{
			TournamentAvailableBannerWindow.CanShow = false;
		}
		return result;
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x000E4700 File Offset: 0x000E2900
	public void BackupLastRatingTake()
	{
		if (this.lastRatingChange.Value >= 0)
		{
			return;
		}
		this.negativeRating += this.lastRatingChange.Value;
		this.UpdateLeague(true);
		this.SaveValues();
		Debug.Log(string.Format("<color=yellow>Rating backup: {0} rating.</color>", this.lastRatingChange.Value));
		Debug.Log(string.Format("<color=yellow>I'm in {0} league, division: {1}. Rating: {2}</color>", this.currentLeague.ToString(), 3 - this.currentDivision, this.currentRating));
		this.lastRatingChange.Value = 0;
		PlayerPrefs.SetInt("leave_from_duel_penalty", 0);
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x000E47B4 File Offset: 0x000E29B4
	private void UpdateLeague(bool up)
	{
		int currentRating = this.currentRating;
		int num = (int)(this.currentLeague * RatingSystem.RatingLeague.Crystal + this.currentDivision);
		for (int i = (!up) ? 0 : num; i < ((!up) ? (num + 1) : this.leagueRatings.Length); i++)
		{
			if (currentRating >= this.leagueRatings[i] + ((!up) ? -100 : 0))
			{
				this.currentLeague = (RatingSystem.RatingLeague)Mathf.FloorToInt((float)i / 3f);
				this.currentDivision = i - (int)(this.currentLeague * RatingSystem.RatingLeague.Crystal);
			}
		}
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x000E484C File Offset: 0x000E2A4C
	public void UpdateLeagueEvent(object o, EventArgs arg)
	{
		this.UpdateLeagueByRating();
		this.SaveValues();
		if (RatingSystem.OnRatingUpdate != null)
		{
			RatingSystem.OnRatingUpdate();
		}
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000E487C File Offset: 0x000E2A7C
	private void UpdateLeagueByRating()
	{
		int currentRating = this.currentRating;
		for (int i = 0; i < this.leagueRatings.Length; i++)
		{
			if (currentRating >= this.leagueRatings[i])
			{
				this.currentLeague = (RatingSystem.RatingLeague)Mathf.FloorToInt((float)i / 3f);
				this.currentDivision = i - (int)(this.currentLeague * RatingSystem.RatingLeague.Crystal);
			}
		}
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000E48DC File Offset: 0x000E2ADC
	public void OnGetCloudValues(int ratingPositive, int ratingNegative)
	{
		this.positiveRating = ratingPositive;
		this.negativeRating = ratingNegative;
		this.UpdateLeagueByRating();
		this.SaveValues();
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x000E48F8 File Offset: 0x000E2AF8
	private void LoadValues()
	{
		string @string = Storager.getString("RatingSystem", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("League"))
			{
				this.currentLeague = (RatingSystem.RatingLeague)Convert.ToInt32(dictionary["League"]);
			}
			if (dictionary.ContainsKey("Division"))
			{
				this.currentDivision = Convert.ToInt32(dictionary["Division"]);
			}
		}
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x000E4970 File Offset: 0x000E2B70
	private void SaveValues()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["League"] = (int)this.currentLeague;
		dictionary["Division"] = this.currentDivision;
		Storager.setString("RatingSystem", Json.Serialize(dictionary), false);
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000E49C0 File Offset: 0x000E2BC0
	private void Awake()
	{
		this.LoadValues();
		this.ParseConfig();
		TrophiesSynchronizer.Instance.Updated += this.UpdateLeagueEvent;
		Storager.RatingUpdated += this.UpdateLeagueEvent;
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x000E4A00 File Offset: 0x000E2C00
	public void ParseConfig()
	{
		string @string = Storager.getString("rSCKeyV2", false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return;
		}
		if (dictionary.ContainsKey("min"))
		{
			this.form_min = Convert.ToInt32(dictionary["min"]);
		}
		if (dictionary.ContainsKey("max"))
		{
			this.form_max = Convert.ToInt32(dictionary["max"]);
		}
		if (dictionary.ContainsKey("leveling"))
		{
			List<object> list = dictionary["leagueRatings"] as List<object>;
			for (int i = 0; i < this.leagueRatings.Length; i++)
			{
				if (list.Count > i)
				{
					this.leagueRatings[i] = Convert.ToInt32(list[i]);
				}
			}
		}
		if (dictionary.ContainsKey("leagueCoefs"))
		{
			List<object> list2 = dictionary["leagueCoefs"] as List<object>;
			for (int j = 0; j < this.leagueCoefs.Length; j++)
			{
				if (list2.Count > j)
				{
					this.leagueCoefs[j] = Convert.ToInt32(list2[j]);
				}
			}
		}
		if (dictionary.ContainsKey("leagueMCoefs"))
		{
			List<object> list3 = dictionary["leagueMCoefs"] as List<object>;
			for (int k = 0; k < this.leagueMCoefs.Length; k++)
			{
				if (list3.Count > k)
				{
					this.leagueMCoefs[k] = Convert.ToInt32(list3[k]);
				}
			}
		}
		if (dictionary.ContainsKey("form_kd_a"))
		{
			this.form_kd_a = (float)Convert.ToDouble(dictionary["form_kd_a"]);
		}
		if (dictionary.ContainsKey("form_kd_b"))
		{
			this.form_kd_b = (float)Convert.ToInt32(dictionary["form_kd_b"]);
		}
		if (dictionary.ContainsKey("form_kd_top"))
		{
			this.form_kd_top = (float)Convert.ToInt32(dictionary["form_kd_top"]);
		}
		if (dictionary.ContainsKey("form_place_coeff"))
		{
			this.form_min = Convert.ToInt32(dictionary["form_place_coeff"]);
		}
	}

	// Token: 0x04002051 RID: 8273
	private const string POSITIVE_RATING_LOCAL_KEY = "RatingPositiveLocal";

	// Token: 0x04002052 RID: 8274
	private static RatingSystem _instance;

	// Token: 0x04002053 RID: 8275
	public static readonly string[] divisionByIndex = new string[]
	{
		"III",
		"II",
		"I"
	};

	// Token: 0x04002054 RID: 8276
	public static readonly string[] leagueChangeLocalizations = new string[]
	{
		"Key_2139",
		"Key_2140",
		"Key_2141",
		"Key_2142",
		"Key_2143",
		"Key_2144"
	};

	// Token: 0x04002055 RID: 8277
	public static readonly string[] leagueLocalizations = new string[]
	{
		"Key_1953",
		"Key_1954",
		"Key_1955",
		"Key_1956",
		"Key_1957",
		"Key_1958"
	};

	// Token: 0x04002056 RID: 8278
	private float[] winRaitingFactorByPlace = new float[]
	{
		1.2f,
		1.1f,
		1f,
		0.9f,
		0.8f
	};

	// Token: 0x04002057 RID: 8279
	private float[] looseRaitingFactorByPlace = new float[]
	{
		0.8f,
		0.9f,
		1f,
		1.1f,
		1.2f
	};

	// Token: 0x04002058 RID: 8280
	private float form_kd_a = 3f;

	// Token: 0x04002059 RID: 8281
	private float form_kd_b = 3f;

	// Token: 0x0400205A RID: 8282
	private float form_kd_top = 2f;

	// Token: 0x0400205B RID: 8283
	private float form_place_coeff = 3f;

	// Token: 0x0400205C RID: 8284
	private float form_hunger_a = 0.5f;

	// Token: 0x0400205D RID: 8285
	private float form_hunger_b = 5f;

	// Token: 0x0400205E RID: 8286
	private int[] hungerLeagueCoefs = new int[]
	{
		40,
		40,
		40,
		35,
		35,
		35,
		30,
		30,
		30,
		25,
		25,
		25,
		20,
		20,
		20,
		20
	};

	// Token: 0x0400205F RID: 8287
	private int form_min = 1;

	// Token: 0x04002060 RID: 8288
	private int form_max = 1;

	// Token: 0x04002061 RID: 8289
	private int[] leagueCoefs = new int[]
	{
		50,
		40,
		40,
		40,
		30,
		30,
		30,
		20,
		20,
		20,
		10,
		10,
		10,
		5,
		5,
		5
	};

	// Token: 0x04002062 RID: 8290
	private int[] leagueMCoefs = new int[]
	{
		8,
		8,
		8,
		6,
		6,
		6,
		4,
		4,
		4,
		2,
		2,
		2,
		1,
		1,
		1,
		1
	};

	// Token: 0x04002063 RID: 8291
	private int[] leagueRatings = new int[]
	{
		0,
		200,
		400,
		600,
		800,
		1000,
		1200,
		1400,
		1600,
		1800,
		2000,
		2200,
		2400,
		2600,
		2800,
		3000
	};

	// Token: 0x04002064 RID: 8292
	public RatingSystem.RatingLeague currentLeague;

	// Token: 0x04002065 RID: 8293
	public int currentDivision;

	// Token: 0x04002066 RID: 8294
	public SaltedInt lastRatingChange = new SaltedInt(210674148);

	// Token: 0x04002067 RID: 8295
	public bool ratingMatch = true;

	// Token: 0x020004B4 RID: 1204
	public struct MatchStat
	{
		// Token: 0x06002B5C RID: 11100 RVA: 0x000E4C38 File Offset: 0x000E2E38
		public MatchStat(int place, int playerCount, bool winner, bool deadHeat)
		{
			this.place = place;
			this.playerCount = playerCount;
			this.winner = winner;
			this.deadHeat = deadHeat;
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x000E4C58 File Offset: 0x000E2E58
		public static RatingSystem.MatchStat LooseStat
		{
			get
			{
				return new RatingSystem.MatchStat(1, 2, false, false);
			}
		}

		// Token: 0x04002069 RID: 8297
		public int place;

		// Token: 0x0400206A RID: 8298
		public int playerCount;

		// Token: 0x0400206B RID: 8299
		public bool winner;

		// Token: 0x0400206C RID: 8300
		public bool deadHeat;
	}

	// Token: 0x020004B5 RID: 1205
	public struct RatingChange
	{
		// Token: 0x06002B5E RID: 11102 RVA: 0x000E4C64 File Offset: 0x000E2E64
		public RatingChange(RatingSystem.RatingLeague currentLeague, int currentDivision, int currentRating)
		{
			this.oldLeague = currentLeague;
			this.oldDivision = currentDivision;
			this.oldRating = currentRating;
			this.newLeague = currentLeague;
			this.newDivision = currentDivision;
			this.newRating = currentRating;
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000E4C9C File Offset: 0x000E2E9C
		public RatingChange(RatingSystem.RatingLeague oldLeague, RatingSystem.RatingLeague newLeague, int oldDivision, int newDivision, int oldRating, int newRating)
		{
			this.oldLeague = oldLeague;
			this.oldDivision = oldDivision;
			this.oldRating = oldRating;
			this.newLeague = newLeague;
			this.newDivision = newDivision;
			this.newRating = newRating;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000E4CCC File Offset: 0x000E2ECC
		public RatingSystem.RatingChange AddChange(RatingSystem.RatingLeague league, int division, int rating)
		{
			return new RatingSystem.RatingChange(this.oldLeague, league, this.oldDivision, division, this.oldRating, rating);
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000E4CE8 File Offset: 0x000E2EE8
		public float oldRatingAmount
		{
			get
			{
				float num = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex(), 0, RatingSystem.instance.leagueRatings.Length - 1)];
				float num2 = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex() + 1, 0, RatingSystem.instance.leagueRatings.Length - 1)];
				float a = (this.oldRating <= this.newRating) ? 0.015f : 0.03f;
				if (this.oldRating == 0)
				{
					a = 0f;
				}
				if (this.oldLeague == RatingSystem.RatingLeague.Adamant && this.newLeague == this.oldLeague)
				{
					return 1f;
				}
				return Mathf.Max(a, Mathf.Clamp01(((float)this.oldRating - num) / (num2 - num)));
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000E4DB4 File Offset: 0x000E2FB4
		public float newRatingAmount
		{
			get
			{
				float num = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex(), 0, RatingSystem.instance.leagueRatings.Length - 1)];
				float num2 = (float)RatingSystem.instance.leagueRatings[Mathf.Clamp(this.GetOldLeagueIndex() + 1, 0, RatingSystem.instance.leagueRatings.Length - 1)];
				float a = (this.newRating <= this.oldRating) ? 0.015f : 0.03f;
				if ((float)this.newRating - (num - 100f) < 0f)
				{
					a = 0f;
				}
				return Mathf.Max(a, Mathf.Clamp01(((float)this.newRating - num) / (num2 - num)));
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x000E4E6C File Offset: 0x000E306C
		public int addRating
		{
			get
			{
				return this.newRating - this.oldRating;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06002B64 RID: 11108 RVA: 0x000E4E7C File Offset: 0x000E307C
		public int maxRating
		{
			get
			{
				return RatingSystem.instance.MaxRatingInDivision(this.oldLeague, this.oldDivision);
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x000E4E94 File Offset: 0x000E3094
		public bool leagueChanged
		{
			get
			{
				return this.oldLeague != this.newLeague;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000E4EA8 File Offset: 0x000E30A8
		public bool divisionChanged
		{
			get
			{
				return this.oldDivision != this.newDivision;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000E4EBC File Offset: 0x000E30BC
		public bool isUp
		{
			get
			{
				return this.GetNewLeagueIndex() > this.GetOldLeagueIndex();
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x000E4ECC File Offset: 0x000E30CC
		public bool isDown
		{
			get
			{
				return this.GetNewLeagueIndex() < this.GetOldLeagueIndex();
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000E4EDC File Offset: 0x000E30DC
		private int GetNewLeagueIndex()
		{
			return (int)(this.newLeague * RatingSystem.RatingLeague.Crystal + this.newDivision);
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000E4EF0 File Offset: 0x000E30F0
		private int GetOldLeagueIndex()
		{
			return (int)(this.oldLeague * RatingSystem.RatingLeague.Crystal + this.oldDivision);
		}

		// Token: 0x0400206D RID: 8301
		public RatingSystem.RatingLeague oldLeague;

		// Token: 0x0400206E RID: 8302
		public RatingSystem.RatingLeague newLeague;

		// Token: 0x0400206F RID: 8303
		public int oldDivision;

		// Token: 0x04002070 RID: 8304
		public int newDivision;

		// Token: 0x04002071 RID: 8305
		public int oldRating;

		// Token: 0x04002072 RID: 8306
		public int newRating;
	}

	// Token: 0x020004B6 RID: 1206
	public enum RatingLeague
	{
		// Token: 0x04002074 RID: 8308
		none = -1,
		// Token: 0x04002075 RID: 8309
		Wood,
		// Token: 0x04002076 RID: 8310
		Steel,
		// Token: 0x04002077 RID: 8311
		Gold,
		// Token: 0x04002078 RID: 8312
		Crystal,
		// Token: 0x04002079 RID: 8313
		Ruby,
		// Token: 0x0400207A RID: 8314
		Adamant
	}

	// Token: 0x020004B7 RID: 1207
	internal sealed class RatingLeagueComparer : IEqualityComparer<RatingSystem.RatingLeague>
	{
		// Token: 0x06002B6B RID: 11115 RVA: 0x000E4F04 File Offset: 0x000E3104
		private RatingLeagueComparer()
		{
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000E4F18 File Offset: 0x000E3118
		public static RatingSystem.RatingLeagueComparer Instance
		{
			get
			{
				return RatingSystem.RatingLeagueComparer.s_instance;
			}
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000E4F20 File Offset: 0x000E3120
		public bool Equals(RatingSystem.RatingLeague x, RatingSystem.RatingLeague y)
		{
			return x == y;
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000E4F28 File Offset: 0x000E3128
		public int GetHashCode(RatingSystem.RatingLeague obj)
		{
			return (int)obj;
		}

		// Token: 0x0400207B RID: 8315
		private static readonly RatingSystem.RatingLeagueComparer s_instance = new RatingSystem.RatingLeagueComparer();
	}
}
