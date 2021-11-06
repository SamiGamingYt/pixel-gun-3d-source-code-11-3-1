using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using I2.Loc;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class NewsLobbyController : MonoBehaviour
{
	// Token: 0x0600233A RID: 9018 RVA: 0x000AEAE0 File Offset: 0x000ACCE0
	private void Awake()
	{
		NewsLobbyController.sharedController = this;
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000AEAE8 File Offset: 0x000ACCE8
	public void UpdateNewsList()
	{
		if (this.GetNews())
		{
			this.UpdateItemsCount();
			this.FillData();
			for (int i = 0; i < this.newsList.Count; i++)
			{
				this.newsList[i].GetComponent<UIToggle>().Set(false);
			}
			this.newsList[0].GetComponent<UIToggle>().Set(true);
			this.SetNewsIndex(0);
			this.newsScroll.enabled = (this.newsListInfo.Count > 4);
			base.StartCoroutine(this.ClearCacheFullPictures());
		}
		else
		{
			while (this.newsList.Count > 0)
			{
				UnityEngine.Object.Destroy(this.newsList[this.newsList.Count - 1].gameObject);
				this.newsList.RemoveAt(this.newsList.Count - 1);
			}
			this.headerLabel.text = LocalizationStore.Get("Key_1807");
			this.dateLabel.text = string.Empty;
			this.descLabel.text = string.Empty;
			this.desc2Label.text = string.Empty;
			this.newsPic.aspectRatio = 200f;
			this.newsPic.enabled = false;
			this.urlButton.SetActive(false);
		}
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000AEC48 File Offset: 0x000ACE48
	private void OnEnable()
	{
		this.UpdateNewsList();
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000AEC50 File Offset: 0x000ACE50
	private void OnDisable()
	{
		if (this.newsPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(this.newsPic.mainTexture);
			this.newsPic.mainTexture = null;
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000AEC90 File Offset: 0x000ACE90
	private bool GetNews()
	{
		string @string = PlayerPrefs.GetString("LobbyNewsKey", "[]");
		this.newsListInfo = (Json.Deserialize(@string) as List<object>).OfType<Dictionary<string, object>>().ToList<Dictionary<string, object>>();
		return this.newsListInfo != null && this.newsListInfo.Count != 0;
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000AECE8 File Offset: 0x000ACEE8
	private void FillData()
	{
		for (int i = 0; i < this.newsList.Count; i++)
		{
			Dictionary<string, object> dictionary = this.newsListInfo[i];
			Dictionary<string, object> dictionary2 = dictionary["short_header"] as Dictionary<string, object>;
			Dictionary<string, object> dictionary3 = dictionary["short_description"] as Dictionary<string, object>;
			if (dictionary2 != null && dictionary3 != null)
			{
				object obj;
				if (!dictionary2.TryGetValue(LocalizationManager.CurrentLanguage, out obj))
				{
					dictionary2.TryGetValue("English", out obj);
				}
				object obj2;
				if (!dictionary3.TryGetValue(LocalizationManager.CurrentLanguage, out obj2))
				{
					dictionary3.TryGetValue("English", out obj2);
				}
				this.newsList[i].headerLabel.text = (string)obj;
				if (Convert.ToInt32(dictionary["readed"]) == 0)
				{
					this.newsList[i].GetComponent<UISprite>().color = Color.white;
					this.newsList[i].indicatorNew.SetActive(true);
				}
				else
				{
					this.newsList[i].GetComponent<UISprite>().color = Color.gray;
					this.newsList[i].indicatorNew.SetActive(false);
				}
				this.newsList[i].shortDescLabel.text = (string)obj2;
				DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)(Convert.ToInt32(dictionary["date"]) + DateTimeOffset.Now.Offset.Hours * 3600));
				this.newsList[i].dateLabel.text = string.Concat(new object[]
				{
					currentTimeByUnixTime.Day.ToString("D2"),
					".",
					currentTimeByUnixTime.Month.ToString("D2"),
					".",
					currentTimeByUnixTime.Year,
					"\n",
					currentTimeByUnixTime.Hour,
					":",
					currentTimeByUnixTime.Minute.ToString("D2")
				});
				object obj3;
				if (dictionary.TryGetValue("previewpicture", out obj3))
				{
					this.newsList[i].LoadPreview((string)obj3);
				}
			}
		}
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000AEF54 File Offset: 0x000AD154
	public void OnNewsItemClick()
	{
		ButtonClickSound.TryPlayClick();
		for (int i = 0; i < this.newsList.Count; i++)
		{
			if (this.newsList[i].GetComponent<UIToggle>().value)
			{
				this.SetNewsIndex(i);
				break;
			}
		}
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000AEFAC File Offset: 0x000AD1AC
	public void OnURLClick()
	{
		if (string.IsNullOrEmpty(this.currentURL))
		{
			return;
		}
		try
		{
			AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
			{
				{
					"Conversion Total",
					"Source"
				},
				{
					"Conversion By News",
					this.currentNewsName
				}
			});
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in log News: " + arg);
		}
		Application.OpenURL(this.currentURL);
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000AF040 File Offset: 0x000AD240
	private IEnumerator LoadPictureForFullNews(int index, string picLink)
	{
		Texture2D picTexture = null;
		if (this.newsPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(this.newsPic.mainTexture);
			this.newsPic.mainTexture = null;
		}
		this.newsPic.aspectRatio = 200f;
		this.newsPic.mainTexture = null;
		string cachePath = PersistentCache.Instance.GetCachePathByUri(picLink);
		bool isLoadedFromCache = false;
		if (!string.IsNullOrEmpty(cachePath))
		{
			yield return null;
			try
			{
				bool cacheExists = File.Exists(cachePath);
				if (Defs.IsDeveloperBuild)
				{
					string formattedPath = (!Application.isEditor) ? cachePath : ("<color=orange>" + cachePath + "</color>");
					Debug.LogFormat("Trying to load news image from cache '{0}': {1}", new object[]
					{
						formattedPath,
						cacheExists
					});
				}
				if (cacheExists)
				{
					byte[] cacheBytes = File.ReadAllBytes(cachePath);
					Texture2D cachedTexture = new Texture2D(2, 2);
					cachedTexture.LoadImage(cacheBytes);
					cachedTexture.filterMode = FilterMode.Point;
					picTexture = cachedTexture;
					isLoadedFromCache = true;
				}
			}
			catch (Exception ex3)
			{
				Exception ex = ex3;
				Debug.LogWarning("Caught exception while reading cached news image. See next message for details.");
				Debug.LogException(ex);
			}
			if (!isLoadedFromCache)
			{
				WWW loadPic = Tools.CreateWwwIfNotConnected(picLink);
				if (loadPic == null)
				{
					yield break;
				}
				yield return loadPic;
				if (!string.IsNullOrEmpty(loadPic.error))
				{
					Debug.LogWarning("Download pic error: " + loadPic.error);
					yield break;
				}
				picTexture = loadPic.texture;
				picTexture.filterMode = FilterMode.Point;
				if (!string.IsNullOrEmpty(cachePath))
				{
					try
					{
						if (Defs.IsDeveloperBuild)
						{
							string formattedPath2 = (!Application.isEditor) ? cachePath : ("<color=orange>" + cachePath + "</color>");
							Debug.LogFormat("Trying to save news image to cache '{0}'", new object[]
							{
								formattedPath2
							});
						}
						string directoryPath = Path.GetDirectoryName(cachePath);
						if (!Directory.Exists(directoryPath))
						{
							Directory.CreateDirectory(directoryPath);
						}
						byte[] cacheBytes2 = loadPic.texture.EncodeToPNG();
						File.WriteAllBytes(cachePath, cacheBytes2);
						this.SaveCacheFullPicturesNameFile(picLink);
					}
					catch (Exception ex4)
					{
						Exception ex2 = ex4;
						Debug.LogWarning("Caught exception while saving news image to cache. See next message for details.");
						Debug.LogException(ex2);
					}
				}
				loadPic = null;
			}
		}
		if (this.selectedIndex == index)
		{
			BoxCollider collider = this.newsPic.GetComponent<BoxCollider>();
			if (collider == null)
			{
				collider = this.newsPic.gameObject.AddComponent<BoxCollider>();
				this.newsPic.gameObject.AddComponent<UIDragScrollView>();
				UIButton button = this.newsPic.gameObject.AddComponent<UIButton>();
				button.onClick.Add(new EventDelegate(new EventDelegate.Callback(this.OnURLClick)));
			}
			this.newsPic.mainTexture = picTexture;
			this.newsPic.aspectRatio = (float)picTexture.width / (float)picTexture.height;
			yield return null;
			picTexture = null;
			this.newsPic.ResizeCollider();
		}
		yield break;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x000AF078 File Offset: 0x000AD278
	private IEnumerator ClearCacheFullPictures()
	{
		string _cacheListStr = PlayerPrefs.GetString(this.cacheFullPictureNewsFileNamesKey, "[]");
		List<object> _cacheListObj = Json.Deserialize(_cacheListStr) as List<object>;
		List<string> _cacheList = new List<string>();
		List<string> _cacheListForRemove = new List<string>();
		for (int i = 0; i < _cacheListObj.Count; i++)
		{
			_cacheList.Add(_cacheListObj[i].ToString());
			_cacheListForRemove.Add(_cacheListObj[i].ToString());
		}
		for (int j = 0; j < this.newsListInfo.Count; j++)
		{
			if (this.newsListInfo[j].ContainsKey("fullpicture") && _cacheListForRemove.Contains(this.newsListInfo[j]["fullpicture"].ToString()))
			{
				_cacheListForRemove.Remove(this.newsListInfo[j]["fullpicture"].ToString());
			}
		}
		for (int k = 0; k < _cacheListForRemove.Count; k++)
		{
			string cachePath = PersistentCache.Instance.GetCachePathByUri(_cacheListForRemove[k]);
			File.Delete(cachePath);
			_cacheList.Remove(_cacheListForRemove[k]);
			yield return null;
		}
		PlayerPrefs.SetString(this.cacheFullPictureNewsFileNamesKey, Json.Serialize(_cacheList));
		yield break;
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000AF094 File Offset: 0x000AD294
	private void SaveCacheFullPicturesNameFile(string _nameFile)
	{
		string @string = PlayerPrefs.GetString(this.cacheFullPictureNewsFileNamesKey, "[]");
		List<object> list = Json.Deserialize(@string) as List<object>;
		List<string> list2 = new List<string>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].ToString());
		}
		if (!list2.Contains(_nameFile))
		{
			list2.Add(_nameFile);
		}
		PlayerPrefs.SetString(this.cacheFullPictureNewsFileNamesKey, Json.Serialize(list2));
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000AF114 File Offset: 0x000AD314
	private void SaveReaded()
	{
		PlayerPrefs.SetString("LobbyNewsKey", Json.Serialize(this.newsListInfo));
		bool flag = false;
		for (int i = 0; i < this.newsListInfo.Count; i++)
		{
			if (Convert.ToInt32(this.newsListInfo[i]["readed"]) == 0)
			{
				flag = true;
			}
		}
		PlayerPrefs.SetInt("LobbyIsAnyNewsKey", (!flag) ? 0 : 1);
		MainMenuController.sharedController.newsIndicator.SetActive(flag);
		PlayerPrefs.Save();
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000AF1A4 File Offset: 0x000AD3A4
	private void SetNewsIndex(int index)
	{
		this.selectedIndex = index;
		this.fullNewsScroll.ResetPosition();
		Dictionary<string, object> dictionary = this.newsListInfo[index];
		Dictionary<string, object> dictionary2 = dictionary["header"] as Dictionary<string, object>;
		Dictionary<string, object> dictionary3 = dictionary["description"] as Dictionary<string, object>;
		Dictionary<string, object> dictionary4 = dictionary["category"] as Dictionary<string, object>;
		if (dictionary2 == null || dictionary3 == null || dictionary4 == null)
		{
			return;
		}
		object obj;
		if (!dictionary2.TryGetValue(LocalizationManager.CurrentLanguage, out obj))
		{
			dictionary2.TryGetValue("English", out obj);
		}
		object obj2;
		if (!dictionary3.TryGetValue(LocalizationManager.CurrentLanguage, out obj2))
		{
			dictionary3.TryGetValue("English", out obj2);
		}
		object obj3;
		if (!dictionary4.TryGetValue(LocalizationManager.CurrentLanguage, out obj3))
		{
			dictionary4.TryGetValue("English", out obj3);
		}
		object obj4;
		if (dictionary.TryGetValue("URL", out obj4) && !obj4.Equals(string.Empty))
		{
			this.currentURL = (string)obj4;
			this.currentNewsName = ((!dictionary2.ContainsKey("English")) ? "NO ENGLISH TRANSLATION" : dictionary2["English"].ToString());
			this.urlButton.SetActive(true);
		}
		else
		{
			this.currentURL = string.Empty;
			this.urlButton.SetActive(false);
		}
		this.headerLabel.text = (string)obj;
		string text = (string)obj2;
		string[] array = text.Split(new string[]
		{
			"[news-pic]"
		}, StringSplitOptions.None);
		object obj5;
		dictionary.TryGetValue("fullpicture", out obj5);
		if (array.Length > 1 && !string.IsNullOrEmpty((string)obj5))
		{
			this.descLabel.text = array[0];
			this.desc2Label.text = array[1];
			this.newsPic.enabled = true;
			base.StartCoroutine(this.LoadPictureForFullNews(index, (string)obj5));
		}
		else
		{
			this.descLabel.text = (string)obj2;
			this.desc2Label.text = string.Empty;
			this.newsPic.aspectRatio = 200f;
			this.newsPic.enabled = false;
		}
		DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime((long)(Convert.ToInt32(dictionary["date"]) + DateTimeOffset.Now.Offset.Hours * 3600));
		this.dateLabel.text = string.Concat(new object[]
		{
			"[bababa]",
			currentTimeByUnixTime.Day.ToString("D2"),
			".",
			currentTimeByUnixTime.Month.ToString("D2"),
			".",
			currentTimeByUnixTime.Year,
			" / [-]",
			obj3
		});
		try
		{
			if (Convert.ToInt32(dictionary["readed"]) == 0)
			{
				AnalyticsFacade.SendCustomEvent("News", new Dictionary<string, object>
				{
					{
						"CTR",
						"Open"
					},
					{
						"Conversion Total",
						"Open"
					},
					{
						"News",
						(!dictionary2.ContainsKey("English")) ? "NO ENGLISH TRANSLATION" : dictionary2["English"].ToString()
					}
				});
			}
		}
		catch (Exception arg)
		{
			Debug.LogError("Exception in log News: " + arg);
		}
		dictionary["readed"] = 1;
		this.FillData();
		this.SaveReaded();
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000AF564 File Offset: 0x000AD764
	private void UpdateItemsCount()
	{
		while (this.newsList.Count < this.newsListInfo.Count)
		{
			GameObject gameObject = NGUITools.AddChild(this.newsGrid.gameObject, this.newsItemPrefab);
			gameObject.SetActive(true);
			this.newsList.Add(gameObject.GetComponent<NewsLobbyItem>());
		}
		while (this.newsList.Count > this.newsListInfo.Count)
		{
			UnityEngine.Object.Destroy(this.newsList[this.newsList.Count - 1].gameObject);
			this.newsList.RemoveAt(this.newsList.Count - 1);
		}
		this.newsGrid.Reposition();
		this.newsScroll.ResetPosition();
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000AF630 File Offset: 0x000AD830
	public void Close()
	{
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController._isCancellationRequested = true;
		}
	}

	// Token: 0x0400177D RID: 6013
	public UIGrid newsGrid;

	// Token: 0x0400177E RID: 6014
	public UIScrollView newsScroll;

	// Token: 0x0400177F RID: 6015
	public UIScrollView fullNewsScroll;

	// Token: 0x04001780 RID: 6016
	public UILabel headerLabel;

	// Token: 0x04001781 RID: 6017
	public UILabel descLabel;

	// Token: 0x04001782 RID: 6018
	public UILabel desc2Label;

	// Token: 0x04001783 RID: 6019
	public UILabel dateLabel;

	// Token: 0x04001784 RID: 6020
	public UITexture newsPic;

	// Token: 0x04001785 RID: 6021
	public string currentURL;

	// Token: 0x04001786 RID: 6022
	public string currentNewsName;

	// Token: 0x04001787 RID: 6023
	public int selectedIndex;

	// Token: 0x04001788 RID: 6024
	public GameObject newsItemPrefab;

	// Token: 0x04001789 RID: 6025
	public GameObject urlButton;

	// Token: 0x0400178A RID: 6026
	private List<NewsLobbyItem> newsList = new List<NewsLobbyItem>();

	// Token: 0x0400178B RID: 6027
	private List<Dictionary<string, object>> newsListInfo = new List<Dictionary<string, object>>();

	// Token: 0x0400178C RID: 6028
	private Texture2D[] newsFullPic;

	// Token: 0x0400178D RID: 6029
	public static NewsLobbyController sharedController;

	// Token: 0x0400178E RID: 6030
	private string cacheFullPictureNewsFileNamesKey = "cacheFullPictureFileNamesKey";
}
