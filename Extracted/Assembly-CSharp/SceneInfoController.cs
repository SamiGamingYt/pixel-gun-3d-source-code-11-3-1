using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x0200074C RID: 1868
public sealed class SceneInfoController : MonoBehaviour
{
	// Token: 0x14000093 RID: 147
	// (add) Token: 0x06004193 RID: 16787 RVA: 0x0015D200 File Offset: 0x0015B400
	// (remove) Token: 0x06004194 RID: 16788 RVA: 0x0015D218 File Offset: 0x0015B418
	public static event Action onChangeInfoMap;

	// Token: 0x06004195 RID: 16789 RVA: 0x0015D230 File Offset: 0x0015B430
	private void Awake()
	{
		SceneInfoController.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ExperienceController.onLevelChange += this.UpdateListAvaliableMap;
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnChangeLocalize));
		this.UpdateListAvaliableMap();
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0015D26C File Offset: 0x0015B46C
	private void OnDestroy()
	{
		ExperienceController.onLevelChange -= this.UpdateListAvaliableMap;
		SceneInfoController.instance = null;
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x0015D288 File Offset: 0x0015B488
	public void UpdateListAvaliableMap()
	{
		if (!this._isLoadingDataActive)
		{
			this._isLoadingDataActive = true;
			TextAsset textAsset = Resources.Load<TextAsset>("infomap_pixelgun_test");
			if (textAsset != null)
			{
				base.StartCoroutine(this.ParseLoadData(textAsset.text));
			}
			else
			{
				Debug.LogWarning("Bindata == null");
			}
		}
	}

	// Token: 0x17000AE7 RID: 2791
	// (get) Token: 0x06004198 RID: 16792 RVA: 0x0015D2E0 File Offset: 0x0015B4E0
	private Version CurrentVersion
	{
		get
		{
			return base.GetType().Assembly.GetName().Version;
		}
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0015D304 File Offset: 0x0015B504
	public SceneInfo GetInfoScene(string nameScene)
	{
		return this.allScenes.Find((SceneInfo curInf) => StringComparer.OrdinalIgnoreCase.Equals(curInf.gameObject.name, nameScene));
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0015D338 File Offset: 0x0015B538
	public SceneInfo GetInfoScene(string nameScene, List<SceneInfo> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((SceneInfo curInf) => StringComparer.OrdinalIgnoreCase.Equals(curInf.gameObject.name, nameScene));
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x0015D36C File Offset: 0x0015B56C
	public SceneInfo GetInfoScene(TypeModeGame needMode, int indexMap)
	{
		SceneInfo infoScene = this.GetInfoScene(indexMap);
		if (infoScene != null && infoScene.IsAvaliableForMode(needMode))
		{
			return infoScene;
		}
		return null;
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x0015D39C File Offset: 0x0015B59C
	public SceneInfo GetInfoScene(int indexMap)
	{
		return this.allScenes.Find((SceneInfo curInf) => curInf.indexMap == indexMap);
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x0015D3D0 File Offset: 0x0015B5D0
	public int GetMaxCountMapsInRegims()
	{
		int num = 0;
		foreach (AllScenesForMode allScenesForMode in this.modeInfo)
		{
			if (allScenesForMode.avaliableScenes.Count > num)
			{
				num = allScenesForMode.avaliableScenes.Count;
			}
		}
		return num;
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x0015D450 File Offset: 0x0015B650
	public AllScenesForMode GetListScenesForMode(TypeModeGame needMode)
	{
		return this.modeInfo.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x0015D484 File Offset: 0x0015B684
	public AllScenesForMode GetListScenesForMode(TypeModeGame needMode, List<AllScenesForMode> needList)
	{
		if (needList == null)
		{
			return null;
		}
		return needList.Find((AllScenesForMode mG) => mG.mode == needMode);
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x0015D4B8 File Offset: 0x0015B6B8
	public int GetCountScenesForMode(TypeModeGame needMode)
	{
		AllScenesForMode allScenesForMode = this.modeInfo.Find((AllScenesForMode nM) => nM.mode == needMode);
		if (allScenesForMode != null)
		{
			return allScenesForMode.avaliableScenes.Count;
		}
		return 0;
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x0015D500 File Offset: 0x0015B700
	private void AddSceneIfAvaliableVersion(string nameScene, string minVersion, string maxVersion)
	{
		SceneInfo infoScene = this.GetInfoScene(nameScene, this.copyAllScenes);
		if (infoScene == null)
		{
			Version currentVersion = this.CurrentVersion;
			Version v = new Version(maxVersion);
			Version v2 = new Version(minVersion);
			if (currentVersion >= v2 && currentVersion <= v)
			{
				GameObject gameObject = Resources.Load("SceneInfo/" + nameScene) as GameObject;
				SceneInfo component = gameObject.GetComponent<SceneInfo>();
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(component.gameObject);
				gameObject2.transform.SetParent(base.transform);
				gameObject2.gameObject.name = nameScene;
				component = gameObject2.GetComponent<SceneInfo>();
				component.minAvaliableVersion = minVersion;
				component.maxAvaliableVersion = maxVersion;
				component.UpdateKeyLoaded();
				this.copyAllScenes.Add(component);
			}
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0015D5D0 File Offset: 0x0015B7D0
	public bool MapExistInProject(string nameScene)
	{
		return true;
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0015D5D4 File Offset: 0x0015B7D4
	private void AddSceneInModeGame(string nameScene, TypeModeGame needMode)
	{
		SceneInfo infoScene = this.GetInfoScene(nameScene, this.copyAllScenes);
		if (infoScene != null)
		{
			infoScene.AddMode(needMode);
			if (infoScene.IsLoaded)
			{
				AllScenesForMode allScenesForMode = this.GetListScenesForMode(needMode, this.copyModeInfo);
				if (allScenesForMode == null)
				{
					allScenesForMode = new AllScenesForMode();
					allScenesForMode.mode = needMode;
					this.copyModeInfo.Add(allScenesForMode);
				}
				allScenesForMode.AddInfoScene(infoScene);
			}
		}
	}

	// Token: 0x17000AE8 RID: 2792
	// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0015D644 File Offset: 0x0015B844
	public static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/infomap_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_amazon.json";
				}
				return string.Empty;
			}
			else
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
				{
					return "https://secure.pixelgunserver.com/infomap_pixelgun_wp8.json";
				}
				return string.Empty;
			}
		}
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x0015D6C0 File Offset: 0x0015B8C0
	private IEnumerator GetDataFromServerLoop()
	{
		for (;;)
		{
			yield return base.StartCoroutine(this.DownloadDataFormServer());
			yield return new WaitForRealSeconds(870f);
		}
		yield break;
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x0015D6DC File Offset: 0x0015B8DC
	private IEnumerator DownloadDataFormServer()
	{
		if (this._isLoadingDataActive)
		{
			yield break;
		}
		this._isLoadingDataActive = true;
		string urlDataAddress = SceneInfoController.UrlForLoadData;
		WWW downloadData = null;
		for (int iter = 3; iter > 0; iter--)
		{
			downloadData = Tools.CreateWwwIfNotConnected(urlDataAddress);
			if (downloadData == null)
			{
				yield break;
			}
			while (!downloadData.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(downloadData.error))
			{
				break;
			}
			yield return new WaitForRealSeconds(5f);
		}
		if (downloadData == null || !string.IsNullOrEmpty(downloadData.error))
		{
			if (Defs.IsDeveloperBuild && downloadData != null)
			{
				Debug.LogWarningFormat("Request to {0} failed: {1}", new object[]
				{
					urlDataAddress,
					downloadData.error
				});
			}
			this._isLoadingDataActive = false;
			yield break;
		}
		string responseText = URLs.Sanitize(downloadData);
		yield return this.ParseLoadData(responseText);
		this._isLoadingDataActive = false;
		yield break;
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x0015D6F8 File Offset: 0x0015B8F8
	private IEnumerator ParseLoadData(string lData)
	{
		Dictionary<string, object> allData = Json.Deserialize(lData) as Dictionary<string, object>;
		if (allData == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogError("Bad response: " + lData);
			}
			this._isLoadingDataActive = false;
			yield break;
		}
		while (ExperienceController.sharedController == null)
		{
			yield return null;
		}
		this.copyAllScenes = new List<SceneInfo>();
		this.copyModeInfo = new List<AllScenesForMode>();
		this.copyModeInfo.Clear();
		if (allData.ContainsKey("allAvaliableMap"))
		{
			List<object> listMap = allData["allAvaliableMap"] as List<object>;
			for (int iM = 0; iM < listMap.Count; iM++)
			{
				Dictionary<string, object> infoMap = listMap[iM] as Dictionary<string, object>;
				if (infoMap != null)
				{
					string curNameScene = string.Empty;
					string minV = string.Empty;
					string maxV = string.Empty;
					if (infoMap.ContainsKey("nameScene"))
					{
						curNameScene = infoMap["nameScene"].ToString();
						if (infoMap.ContainsKey("minV"))
						{
							minV = infoMap["minV"].ToString();
						}
						if (infoMap.ContainsKey("maxV"))
						{
							maxV = infoMap["maxV"].ToString();
						}
						this.AddSceneIfAvaliableVersion(curNameScene, minV, maxV);
					}
				}
			}
		}
		if (allData.ContainsKey("modeMap"))
		{
			List<object> listAllMode = allData["modeMap"] as List<object>;
			for (int iMod = 0; iMod < listAllMode.Count; iMod++)
			{
				Dictionary<string, object> infoMode = listAllMode[iMod] as Dictionary<string, object>;
				if (infoMode != null)
				{
					if (infoMode.ContainsKey("modeId"))
					{
						TypeModeGame curModeMap = SceneInfoController.ConvertModeToEnum(infoMode["modeId"].ToString());
						if (infoMode.ContainsKey("scenesForMode"))
						{
							List<object> listModeScenes = infoMode["scenesForMode"] as List<object>;
							for (int iSc = 0; iSc < listModeScenes.Count; iSc++)
							{
								Dictionary<string, object> curSceneInf = listModeScenes[iSc] as Dictionary<string, object>;
								if (curSceneInf != null)
								{
									bool avalForCurLev = true;
									if (curSceneInf.ContainsKey("minLevPlayerForAval"))
									{
										int minAvalLev = Convert.ToInt32(curSceneInf["minLevPlayerForAval"]);
										if (ExperienceController.sharedController.currentLevel < minAvalLev)
										{
											avalForCurLev = false;
										}
									}
									if (curSceneInf.ContainsKey("ratingCount"))
									{
										int ratCount = Convert.ToInt32(curSceneInf["ratingCount"]);
										if (RatingSystem.instance.currentRating < ratCount)
										{
											avalForCurLev = false;
										}
									}
									if (avalForCurLev && curSceneInf.ContainsKey("nameScene"))
									{
										this.AddSceneInModeGame(curSceneInf["nameScene"].ToString(), curModeMap);
									}
								}
							}
						}
					}
				}
			}
		}
		this.OnDataLoaded();
		yield break;
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x0015D724 File Offset: 0x0015B924
	public static TypeModeGame ConvertModeToEnum(string modeStr)
	{
		switch (modeStr)
		{
		case "Deathmatch":
			return TypeModeGame.Deathmatch;
		case "TimeBattle":
			return TypeModeGame.TimeBattle;
		case "TeamFight":
			return TypeModeGame.TeamFight;
		case "DeadlyGames":
			return TypeModeGame.DeadlyGames;
		case "FlagCapture":
			return TypeModeGame.FlagCapture;
		case "CapturePoints":
			return TypeModeGame.CapturePoints;
		case "Dater":
			return TypeModeGame.Dater;
		case "Duel":
			return TypeModeGame.Duel;
		}
		return TypeModeGame.Deathmatch;
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x0015D800 File Offset: 0x0015BA00
	internal static HashSet<TypeModeGame> GetUnlockedModesByLevel(int level)
	{
		HashSet<TypeModeGame> hashSet = new HashSet<TypeModeGame>();
		foreach (KeyValuePair<TypeModeGame, int> keyValuePair in SceneInfoController._modeUnlockLevels)
		{
			if (keyValuePair.Value <= level)
			{
				hashSet.Add(keyValuePair.Key);
			}
		}
		return hashSet;
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x0015D880 File Offset: 0x0015BA80
	internal static HashSet<ConnectSceneNGUIController.RegimGame> SelectModes(IEnumerable<TypeModeGame> modes)
	{
		HashSet<ConnectSceneNGUIController.RegimGame> hashSet = new HashSet<ConnectSceneNGUIController.RegimGame>();
		foreach (TypeModeGame key in modes)
		{
			ConnectSceneNGUIController.RegimGame item;
			if (SceneInfoController._modesMap.TryGetValue(key, out item))
			{
				hashSet.Add(item);
			}
		}
		return hashSet;
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x0015D8F8 File Offset: 0x0015BAF8
	private void OnDataLoaded()
	{
		this.allScenes = this.copyAllScenes;
		this.modeInfo = this.copyModeInfo;
		this.OnChangeLocalize();
		if (SceneInfoController.onChangeInfoMap != null)
		{
			SceneInfoController.onChangeInfoMap();
		}
		this._isLoadingDataActive = false;
	}

	// Token: 0x060041AC RID: 16812 RVA: 0x0015D934 File Offset: 0x0015BB34
	private void OnChangeLocalize()
	{
		for (int i = 0; i < this.allScenes.Count; i++)
		{
			this.allScenes[i].UpdateLocalize();
		}
	}

	// Token: 0x04002FF4 RID: 12276
	private const float timerUpdateDataFromServer = 870f;

	// Token: 0x04002FF5 RID: 12277
	public static SceneInfoController instance = null;

	// Token: 0x04002FF6 RID: 12278
	public List<SceneInfo> allScenes = new List<SceneInfo>();

	// Token: 0x04002FF7 RID: 12279
	public List<AllScenesForMode> modeInfo = new List<AllScenesForMode>();

	// Token: 0x04002FF8 RID: 12280
	private bool _isLoadingDataActive;

	// Token: 0x04002FF9 RID: 12281
	private List<SceneInfo> copyAllScenes;

	// Token: 0x04002FFA RID: 12282
	private List<AllScenesForMode> copyModeInfo;

	// Token: 0x04002FFB RID: 12283
	private static readonly Dictionary<TypeModeGame, int> _modeUnlockLevels = new Dictionary<TypeModeGame, int>(TypeModeGameComparer.Instance)
	{
		{
			TypeModeGame.Deathmatch,
			1
		},
		{
			TypeModeGame.TeamFight,
			2
		},
		{
			TypeModeGame.TimeBattle,
			3
		},
		{
			TypeModeGame.FlagCapture,
			4
		},
		{
			TypeModeGame.DeadlyGames,
			5
		},
		{
			TypeModeGame.CapturePoints,
			6
		}
	};

	// Token: 0x04002FFC RID: 12284
	private static readonly Dictionary<TypeModeGame, ConnectSceneNGUIController.RegimGame> _modesMap = new Dictionary<TypeModeGame, ConnectSceneNGUIController.RegimGame>(TypeModeGameComparer.Instance)
	{
		{
			TypeModeGame.Deathmatch,
			ConnectSceneNGUIController.RegimGame.Deathmatch
		},
		{
			TypeModeGame.TeamFight,
			ConnectSceneNGUIController.RegimGame.TeamFight
		},
		{
			TypeModeGame.TimeBattle,
			ConnectSceneNGUIController.RegimGame.TimeBattle
		},
		{
			TypeModeGame.FlagCapture,
			ConnectSceneNGUIController.RegimGame.FlagCapture
		},
		{
			TypeModeGame.DeadlyGames,
			ConnectSceneNGUIController.RegimGame.DeadlyGames
		},
		{
			TypeModeGame.CapturePoints,
			ConnectSceneNGUIController.RegimGame.CapturePoints
		},
		{
			TypeModeGame.Duel,
			ConnectSceneNGUIController.RegimGame.Duel
		}
	};
}
