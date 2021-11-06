using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public abstract class ABTestBase
{
	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06002C89 RID: 11401
	public abstract string currentFolder { get; }

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06002C8A RID: 11402 RVA: 0x000EC0D4 File Offset: 0x000EA2D4
	private string platformFolder
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "test";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				return (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "android" : "amazon";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "wp";
			}
			return "ios";
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06002C8B RID: 11403 RVA: 0x000EC130 File Offset: 0x000EA330
	private string url
	{
		get
		{
			return string.Format("{0}/{1}/abtestconfig_{2}.json", "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests", this.currentFolder, this.platformFolder);
		}
	}

	// Token: 0x1700079C RID: 1948
	// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000EC158 File Offset: 0x000EA358
	private string configNameKey
	{
		get
		{
			return string.Format("CN_{0}", this.currentFolder);
		}
	}

	// Token: 0x1700079D RID: 1949
	// (get) Token: 0x06002C8D RID: 11405 RVA: 0x000EC16C File Offset: 0x000EA36C
	// (set) Token: 0x06002C8E RID: 11406 RVA: 0x000EC1A8 File Offset: 0x000EA3A8
	public string configName
	{
		get
		{
			if (!this._isConfigNameInit)
			{
				this._configName = PlayerPrefs.GetString(this.configNameKey, "none");
				this._isConfigNameInit = true;
			}
			return this._configName;
		}
		set
		{
			this._isConfigNameInit = true;
			this._configName = value;
			PlayerPrefs.SetString(this.configNameKey, this._configName);
		}
	}

	// Token: 0x1700079E RID: 1950
	// (get) Token: 0x06002C8F RID: 11407 RVA: 0x000EC1CC File Offset: 0x000EA3CC
	private string cohortKey
	{
		get
		{
			return string.Format("cohort_{0}", this.currentFolder);
		}
	}

	// Token: 0x1700079F RID: 1951
	// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000EC1E0 File Offset: 0x000EA3E0
	// (set) Token: 0x06002C91 RID: 11409 RVA: 0x000EC218 File Offset: 0x000EA418
	public ABTestController.ABTestCohortsType cohort
	{
		get
		{
			if (!this._isInitCohort)
			{
				this._cohort = (ABTestController.ABTestCohortsType)PlayerPrefs.GetInt(this.cohortKey, 0);
				this._isInitCohort = true;
			}
			return this._cohort;
		}
		set
		{
			this._cohort = value;
			this._isInitCohort = true;
			PlayerPrefs.SetInt(this.cohortKey, (int)this._cohort);
		}
	}

	// Token: 0x170007A0 RID: 1952
	// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000EC23C File Offset: 0x000EA43C
	public string cohortName
	{
		get
		{
			return this.configName + this.cohort.ToString();
		}
	}

	// Token: 0x06002C93 RID: 11411 RVA: 0x000EC264 File Offset: 0x000EA464
	public void UpdateABTestConfig()
	{
		CoroutineRunner.Instance.StartCoroutine(this.GetABTestConfig());
	}

	// Token: 0x06002C94 RID: 11412 RVA: 0x000EC278 File Offset: 0x000EA478
	private IEnumerator GetABTestConfig()
	{
		while (!this.isRunGetABTestConfig)
		{
			this.isRunGetABTestConfig = true;
			WWWForm form = new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(URLs.ABTestQuestSystemURL);
			if (download == null)
			{
				yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
			}
			else
			{
				yield return download;
				if (string.IsNullOrEmpty(download.error))
				{
					string responseText = URLs.Sanitize(download);
					if (!string.IsNullOrEmpty(responseText))
					{
						Storager.setString(this.abTestConfigKey, responseText, false);
						this.ParseABTestConfig(false);
					}
					this.isRunGetABTestConfig = false;
					break;
				}
				if (Debug.isDebugBuild || Application.isEditor)
				{
					Debug.LogWarning(string.Format("GetABTest {0} error: {1}", this.currentFolder, download.error));
				}
				yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(30f + (float)UnityEngine.Random.Range(0, 10)));
			}
		}
		yield break;
	}

	// Token: 0x170007A1 RID: 1953
	// (get) Token: 0x06002C95 RID: 11413 RVA: 0x000EC294 File Offset: 0x000EA494
	private string abTestConfigKey
	{
		get
		{
			return string.Format("abTest{0}ConfigKey", this.currentFolder);
		}
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x000EC2A8 File Offset: 0x000EA4A8
	public void InitTest()
	{
		if (Storager.hasKey(this.abTestConfigKey))
		{
			this.ParseABTestConfig(false);
		}
		else
		{
			Storager.setString(this.abTestConfigKey, string.Empty, false);
		}
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x000EC2E4 File Offset: 0x000EA4E4
	private void ParseABTestConfig(bool isFromReset = false)
	{
		if (string.IsNullOrEmpty(Storager.getString(this.abTestConfigKey, false)))
		{
			return;
		}
		string @string = Storager.getString(this.abTestConfigKey, false);
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null && dictionary.ContainsKey("enableABTest"))
		{
			int num = Convert.ToInt32(dictionary["enableABTest"]);
			object settingsB = null;
			if (dictionary.ContainsKey("SettingsB"))
			{
				settingsB = dictionary["SettingsB"];
			}
			if (num == 1 && this.cohort != ABTestController.ABTestCohortsType.SKIP)
			{
				if (this.cohort == ABTestController.ABTestCohortsType.NONE)
				{
					this.configName = Convert.ToString(dictionary["configName"]);
					int cohort = UnityEngine.Random.Range(1, 3);
					this.cohort = (ABTestController.ABTestCohortsType)cohort;
					AnalyticsStuff.LogABTest(this.currentFolder, this.cohortName, true);
					if (FriendsController.sharedController != null)
					{
						FriendsController.sharedController.SendOurData(false);
					}
				}
				this.ApplyState(this.cohort, settingsB);
			}
			else
			{
				if (!isFromReset)
				{
					this.ResetABTest();
				}
				bool flag = false;
				if (dictionary.ContainsKey("currentStateIsB"))
				{
					flag = Convert.ToBoolean(dictionary["currentStateIsB"]);
				}
				this.ApplyState((!flag) ? ABTestController.ABTestCohortsType.A : ABTestController.ABTestCohortsType.B, settingsB);
			}
		}
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x000EC430 File Offset: 0x000EA630
	protected virtual void ApplyState(ABTestController.ABTestCohortsType _state, object settingsB)
	{
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x000EC434 File Offset: 0x000EA634
	public void ResetABTest()
	{
		if (this.cohort != ABTestController.ABTestCohortsType.SKIP)
		{
			if (this.cohort == ABTestController.ABTestCohortsType.A || this.cohort == ABTestController.ABTestCohortsType.B)
			{
				AnalyticsStuff.LogABTest(this.currentFolder, this.cohortName, false);
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData(false);
				}
			}
			this.cohort = ABTestController.ABTestCohortsType.SKIP;
			this.ParseABTestConfig(true);
		}
	}

	// Token: 0x04002193 RID: 8595
	private const string baseFolder = "https://secure.pixelgunserver.com/pixelgun3d-config/ABTests";

	// Token: 0x04002194 RID: 8596
	private bool _isConfigNameInit;

	// Token: 0x04002195 RID: 8597
	private string _configName = "none";

	// Token: 0x04002196 RID: 8598
	private ABTestController.ABTestCohortsType _cohort;

	// Token: 0x04002197 RID: 8599
	private bool _isInitCohort;

	// Token: 0x04002198 RID: 8600
	private bool isRunGetABTestConfig;
}
