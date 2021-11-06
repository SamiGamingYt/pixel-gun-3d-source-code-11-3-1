using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000704 RID: 1796
public class PremiumAccountController : MonoBehaviour
{
	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06003E64 RID: 15972 RVA: 0x0014EAE0 File Offset: 0x0014CCE0
	// (remove) Token: 0x06003E65 RID: 15973 RVA: 0x0014EAF8 File Offset: 0x0014CCF8
	public static event PremiumAccountController.OnAccountChangedDelegate OnAccountChanged;

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x06003E67 RID: 15975 RVA: 0x0014EB18 File Offset: 0x0014CD18
	// (set) Token: 0x06003E66 RID: 15974 RVA: 0x0014EB10 File Offset: 0x0014CD10
	public static PremiumAccountController Instance { get; private set; }

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x06003E69 RID: 15977 RVA: 0x0014EB2C File Offset: 0x0014CD2C
	// (set) Token: 0x06003E68 RID: 15976 RVA: 0x0014EB20 File Offset: 0x0014CD20
	public bool isAccountActive { get; private set; }

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x06003E6A RID: 15978 RVA: 0x0014EB34 File Offset: 0x0014CD34
	// (set) Token: 0x06003E6B RID: 15979 RVA: 0x0014EB3C File Offset: 0x0014CD3C
	public static bool AccountHasExpired { get; set; }

	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x06003E6C RID: 15980 RVA: 0x0014EB44 File Offset: 0x0014CD44
	public int RewardCoeff
	{
		get
		{
			return (!this.isAccountActive) ? 1 : 2;
		}
	}

	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x06003E6D RID: 15981 RVA: 0x0014EB58 File Offset: 0x0014CD58
	public static float VirtualCurrencyMultiplier
	{
		get
		{
			if (PremiumAccountController.Instance == null)
			{
				Debug.LogError("VirtualCurrencyMultiplier Instance == null");
				return 1f;
			}
			PremiumAccountController.AccountType currentAccount = PremiumAccountController.Instance.GetCurrentAccount();
			if (currentAccount == PremiumAccountController.AccountType.SevenDays)
			{
				return 1.05f;
			}
			if (currentAccount == PremiumAccountController.AccountType.Month)
			{
				return 1.1f;
			}
			return 1f;
		}
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x0014EBB0 File Offset: 0x0014CDB0
	public static bool MapAvailableDueToPremiumAccount(string mapName)
	{
		return mapName != null && !(PremiumAccountController.Instance == null) && (Defs.PremiumMaps != null && Defs.PremiumMaps.ContainsKey(mapName)) && PremiumAccountController.Instance.isAccountActive;
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x0014EBFC File Offset: 0x0014CDFC
	private void Start()
	{
		PremiumAccountController.Instance = this;
		this._timeToEndAccount = default(TimeSpan);
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		this.isAccountActive = this.CheckInitializeCurrentAccount();
		this.CheckTimeHack();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.StartCoroutine(this.GetPremInfoLoop());
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x0014EC54 File Offset: 0x0014CE54
	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.UpdateLastLoggedTime();
		}
		else
		{
			this.CheckTimeHack();
			yield return null;
			yield return null;
			yield return null;
			base.StartCoroutine(this.DownloadPremInfo());
		}
		yield break;
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x0014EC80 File Offset: 0x0014CE80
	private void Destroy()
	{
		this.UpdateLastLoggedTime();
		PremiumAccountController.Instance = null;
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x0014EC90 File Offset: 0x0014CE90
	private void CheckTimeHack()
	{
		this._lastLoggedAccountTime = this.GetLastLoggedTime();
		if (this._lastLoggedAccountTime != 0L && PromoActionsManager.CurrentUnixTime < this._lastLoggedAccountTime)
		{
			this.StopAccountsWork();
		}
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x0014ECC0 File Offset: 0x0014CEC0
	private long GetLastLoggedTime()
	{
		if (!this.isAccountActive)
		{
			return 0L;
		}
		if (!Storager.hasKey("LastLoggedTimePremiumAccount"))
		{
			return 0L;
		}
		string @string = Storager.getString("LastLoggedTimePremiumAccount", false);
		long result;
		long.TryParse(@string, out result);
		return result;
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x0014ED04 File Offset: 0x0014CF04
	private void UpdateLastLoggedTime()
	{
		if (!this.isAccountActive)
		{
			return;
		}
		Storager.setString("LastLoggedTimePremiumAccount", PromoActionsManager.CurrentUnixTime.ToString(), false);
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x0014ED38 File Offset: 0x0014CF38
	private bool CheckInitializeCurrentAccount()
	{
		DateTime timeStart = default(DateTime);
		bool flag = Tools.ParseDateTimeFromPlayerPrefs("StartTimePremiumAccount", out timeStart);
		DateTime timeEnd = default(DateTime);
		bool flag2 = Tools.ParseDateTimeFromPlayerPrefs("EndTimePremiumAccount", out timeEnd);
		if (!flag || !flag2)
		{
			return false;
		}
		this._timeStart = timeStart;
		this._timeEnd = timeEnd;
		return true;
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0014ED8C File Offset: 0x0014CF8C
	private void Update()
	{
		if (!this.isAccountActive)
		{
			return;
		}
		if (Time.realtimeSinceStartup - this._lastCheckTime >= 1f)
		{
			this._timeToEndAccount = this._timeEnd - DateTime.UtcNow;
			this.isAccountActive = (DateTime.UtcNow <= this._timeEnd);
			if (!this.isAccountActive)
			{
				this.ChangeCurrentAccount();
			}
			this._lastCheckTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x0014EE04 File Offset: 0x0014D004
	private void ChangeCurrentAccount()
	{
		if (!this.ChangeAccountOnNext())
		{
			this.StopAccountsWork();
		}
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x0014EE24 File Offset: 0x0014D024
	private DateTime GetTimeEndAccount(DateTime startTime, PremiumAccountController.AccountType accountType)
	{
		DateTime result = startTime;
		switch (accountType)
		{
		case PremiumAccountController.AccountType.OneDay:
			result = result.AddDays(1.0);
			break;
		case PremiumAccountController.AccountType.ThreeDay:
			result = result.AddDays(3.0);
			break;
		case PremiumAccountController.AccountType.SevenDays:
			result = result.AddDays(7.0);
			break;
		case PremiumAccountController.AccountType.Month:
			result = result.AddDays(30.0);
			break;
		}
		return result;
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x0014EEAC File Offset: 0x0014D0AC
	public void BuyAccount(PremiumAccountController.AccountType accountType)
	{
		PremiumAccountController.AccountType currentAccount = this.GetCurrentAccount();
		if (currentAccount == PremiumAccountController.AccountType.None)
		{
			this.StartNewAccount(accountType);
		}
		this.AddBoughtAccountInHistory(accountType);
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		PremiumAccountController.AccountHasExpired = false;
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0014EEF8 File Offset: 0x0014D0F8
	private void StartNewAccount(PremiumAccountController.AccountType accountType)
	{
		this.isAccountActive = true;
		this._timeStart = DateTime.UtcNow;
		Storager.setString("StartTimePremiumAccount", this._timeStart.ToString("s"), false);
		this._timeEnd = this.GetTimeEndAccount(this._timeStart, accountType);
		Storager.setString("EndTimePremiumAccount", this._timeEnd.ToString("s"), false);
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x0014EF60 File Offset: 0x0014D160
	private void AddBoughtAccountInHistory(PremiumAccountController.AccountType accountType)
	{
		string text = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(text))
		{
			text = string.Format("{0}", (int)accountType);
		}
		else
		{
			text += string.Format(",{0}", (int)accountType);
		}
		Storager.setString("BuyHistoryPremiumAccount", text, false);
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x0014EFC0 File Offset: 0x0014D1C0
	private void DeleteBoughtAccountFromHistory()
	{
		string text = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		int num = text.IndexOf(',');
		if (num > 0)
		{
			text = text.Remove(0, num + 1);
		}
		else
		{
			text = string.Empty;
		}
		Storager.setString("BuyHistoryPremiumAccount", text, false);
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x0014F018 File Offset: 0x0014D218
	public PremiumAccountController.AccountType GetCurrentAccount()
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(@string))
		{
			return PremiumAccountController.AccountType.None;
		}
		string[] array = @string.Split(new char[]
		{
			','
		});
		if (array.Length == 0)
		{
			return PremiumAccountController.AccountType.None;
		}
		int result = 0;
		if (!int.TryParse(array[0], out result))
		{
			return PremiumAccountController.AccountType.None;
		}
		return (PremiumAccountController.AccountType)result;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x0014F070 File Offset: 0x0014D270
	private bool ChangeAccountOnNext()
	{
		this.DeleteBoughtAccountFromHistory();
		this._additionalAccountDays = this.GetAllTimeOtherAccountFromHistory();
		PremiumAccountController.AccountType currentAccount = this.GetCurrentAccount();
		if (currentAccount == PremiumAccountController.AccountType.None)
		{
			return false;
		}
		this.StartNewAccount(currentAccount);
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return true;
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x0014F0CC File Offset: 0x0014D2CC
	private void StopAccountsWork()
	{
		this.isAccountActive = false;
		if (PremiumAccountController.OnAccountChanged != null)
		{
			PremiumAccountController.OnAccountChanged();
		}
		Storager.setString("StartTimePremiumAccount", string.Empty, false);
		Storager.setString("EndTimePremiumAccount", string.Empty, false);
		Storager.setString("BuyHistoryPremiumAccount", string.Empty, false);
		this._timeToEndAccount = TimeSpan.FromMinutes(0.0);
		this._additionalAccountDays = 0;
		this._countCeilDays = 0;
		PremiumAccountController.AccountHasExpired = true;
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x0014F14C File Offset: 0x0014D34C
	private int GetDaysAccountByType(int codeAccount)
	{
		switch (codeAccount)
		{
		case 0:
			return 1;
		case 1:
			return 3;
		case 2:
			return 7;
		case 3:
			return 30;
		default:
			return 0;
		}
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x0014F184 File Offset: 0x0014D384
	private int GetAllTimeOtherAccountFromHistory()
	{
		string @string = Storager.getString("BuyHistoryPremiumAccount", false);
		if (string.IsNullOrEmpty(@string))
		{
			return 0;
		}
		string[] array = @string.Split(new char[]
		{
			','
		});
		if (array.Length == 0)
		{
			return 0;
		}
		int codeAccount = 0;
		int num = 0;
		for (int i = 1; i < array.Length; i++)
		{
			int.TryParse(array[i], out codeAccount);
			num += this.GetDaysAccountByType(codeAccount);
		}
		return num;
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x0014F1FC File Offset: 0x0014D3FC
	public string GetTimeToEndAllAccounts()
	{
		if (!this.isAccountActive)
		{
			return string.Empty;
		}
		TimeSpan timeSpan = this._timeToEndAccount.Add(TimeSpan.FromDays((double)this._additionalAccountDays));
		if (timeSpan.Days > 0)
		{
			string arg = "Days";
			this._countCeilDays = Mathf.CeilToInt((float)this._timeToEndAccount.TotalDays) + this._additionalAccountDays;
			return string.Format("{0}: {1}", arg, this._countCeilDays);
		}
		return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x0014F2A8 File Offset: 0x0014D4A8
	public int GetDaysToEndAllAccounts()
	{
		return this._countCeilDays + this._additionalAccountDays;
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x0014F2B8 File Offset: 0x0014D4B8
	private IEnumerator GetPremInfoLoop()
	{
		while (!TrainingController.TrainingCompleted)
		{
			yield return null;
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadPremInfo());
			while (Time.realtimeSinceStartup - this._premGetInfoStartTime < 1200f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x0014F2D4 File Offset: 0x0014D4D4
	private IEnumerator DownloadPremInfo()
	{
		if (this._isGetPremInfoRunning)
		{
			yield break;
		}
		this._premGetInfoStartTime = Time.realtimeSinceStartup;
		this._isGetPremInfoRunning = true;
		if (string.IsNullOrEmpty(URLs.PremiumAccount))
		{
			this._isGetPremInfoRunning = false;
			yield break;
		}
		WWW response = Tools.CreateWwwIfNotConnected(URLs.PremiumAccount);
		if (response == null)
		{
			this._isGetPremInfoRunning = false;
			yield break;
		}
		yield return response;
		string responseText = URLs.Sanitize(response);
		if (!string.IsNullOrEmpty(response.error))
		{
			Debug.LogWarningFormat("Premium Account response error: {0}", new object[]
			{
				response.error
			});
			this._isGetPremInfoRunning = false;
			yield break;
		}
		if (string.IsNullOrEmpty(responseText))
		{
			Debug.LogWarning("Prem response is empty");
			this._isGetPremInfoRunning = false;
			yield break;
		}
		object premInfoObj = Json.Deserialize(responseText);
		Dictionary<string, object> premInfo = premInfoObj as Dictionary<string, object>;
		if (premInfo == null)
		{
			Debug.LogWarning("Prem response is bad");
			this._isGetPremInfoRunning = false;
			yield break;
		}
		if (premInfo.ContainsKey("enable"))
		{
			long _enable = (long)premInfo["enable"];
			Storager.setInt(Defs.PremiumEnabledFromServer, (_enable != 1L) ? 0 : 1, false);
		}
		this._isGetPremInfoRunning = false;
		yield break;
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x0014F2F0 File Offset: 0x0014D4F0
	public bool IsActiveOrWasActiveBeforeStartMatch()
	{
		if (this.isAccountActive)
		{
			return true;
		}
		Player_move_c player_move_c = (!(WeaponManager.sharedManager == null)) ? WeaponManager.sharedManager.myPlayerMoveC : null;
		return !(player_move_c == null) && player_move_c.isNeedTakePremiumAccountRewards;
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x0014F340 File Offset: 0x0014D540
	public int GetRewardCoeffByActiveOrActiveBeforeMatch()
	{
		return (!this.IsActiveOrWasActiveBeforeStartMatch()) ? 1 : 2;
	}

	// Token: 0x04002E19 RID: 11801
	private const float PremInfoTimeout = 1200f;

	// Token: 0x04002E1A RID: 11802
	private DateTime _timeStart;

	// Token: 0x04002E1B RID: 11803
	private DateTime _timeEnd;

	// Token: 0x04002E1C RID: 11804
	private TimeSpan _timeToEndAccount;

	// Token: 0x04002E1D RID: 11805
	private float _lastCheckTime;

	// Token: 0x04002E1E RID: 11806
	private int _additionalAccountDays;

	// Token: 0x04002E1F RID: 11807
	private long _lastLoggedAccountTime;

	// Token: 0x04002E20 RID: 11808
	private int _countCeilDays;

	// Token: 0x04002E21 RID: 11809
	private bool _isGetPremInfoRunning;

	// Token: 0x04002E22 RID: 11810
	private float _premGetInfoStartTime;

	// Token: 0x02000705 RID: 1797
	public enum AccountType
	{
		// Token: 0x04002E28 RID: 11816
		OneDay,
		// Token: 0x04002E29 RID: 11817
		ThreeDay,
		// Token: 0x04002E2A RID: 11818
		SevenDays,
		// Token: 0x04002E2B RID: 11819
		Month,
		// Token: 0x04002E2C RID: 11820
		None
	}

	// Token: 0x02000924 RID: 2340
	// (Invoke) Token: 0x06005130 RID: 20784
	public delegate void OnAccountChangedDelegate();
}
