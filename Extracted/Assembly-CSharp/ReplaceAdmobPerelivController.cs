using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000801 RID: 2049
public sealed class ReplaceAdmobPerelivController : MonoBehaviour
{
	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x06004A8C RID: 19084 RVA: 0x001A77B0 File Offset: 0x001A59B0
	// (set) Token: 0x06004A8D RID: 19085 RVA: 0x001A77B8 File Offset: 0x001A59B8
	public static ReplaceAdmobPerelivController sharedController { get; private set; }

	// Token: 0x06004A8E RID: 19086 RVA: 0x001A77C0 File Offset: 0x001A59C0
	public static void IncreaseTimesCounter()
	{
		ReplaceAdmobPerelivController._timesWantToShow++;
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06004A8F RID: 19087 RVA: 0x001A77D0 File Offset: 0x001A59D0
	public static bool ShouldShowAtThisTime
	{
		get
		{
			return PromoActionsManager.ReplaceAdmobPereliv != null && PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes > 0 && ReplaceAdmobPerelivController._timesWantToShow % PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes == 0;
		}
	}

	// Token: 0x06004A90 RID: 19088 RVA: 0x001A7810 File Offset: 0x001A5A10
	public static void TryShowPereliv(string context)
	{
		if (ReplaceAdmobPerelivController.sharedController != null && ReplaceAdmobPerelivController.sharedController.Image != null && ReplaceAdmobPerelivController.sharedController.AdUrl != null)
		{
			AdmobPerelivWindow.admobTexture = ReplaceAdmobPerelivController.sharedController.Image;
			AdmobPerelivWindow.admobUrl = ReplaceAdmobPerelivController.sharedController.AdUrl;
			AdmobPerelivWindow.Context = context;
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, DateTime.UtcNow.ToString("s"));
			FyberFacade.Instance.IncrementCurrentDailyInterstitialCount();
			ReplaceAdmobPerelivController._timesShown++;
			InterstitialCounter.Instance.IncrementFakeInterstitialCount();
		}
	}

	// Token: 0x06004A91 RID: 19089 RVA: 0x001A78B4 File Offset: 0x001A5AB4
	public void DestroyImage()
	{
		if (this.Image != null)
		{
			this._image = null;
		}
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06004A92 RID: 19090 RVA: 0x001A78D0 File Offset: 0x001A5AD0
	public Texture2D Image
	{
		get
		{
			return this._image;
		}
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06004A93 RID: 19091 RVA: 0x001A78D8 File Offset: 0x001A5AD8
	public string AdUrl
	{
		get
		{
			return this._adUrl;
		}
	}

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06004A94 RID: 19092 RVA: 0x001A78E0 File Offset: 0x001A5AE0
	public bool DataLoaded
	{
		get
		{
			return this._image != null && this._adUrl != null;
		}
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x06004A95 RID: 19093 RVA: 0x001A7910 File Offset: 0x001A5B10
	// (set) Token: 0x06004A96 RID: 19094 RVA: 0x001A7918 File Offset: 0x001A5B18
	public bool DataLoading { get; private set; }

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06004A97 RID: 19095 RVA: 0x001A7924 File Offset: 0x001A5B24
	// (set) Token: 0x06004A98 RID: 19096 RVA: 0x001A792C File Offset: 0x001A5B2C
	public bool ShouldShowInLobby { get; set; }

	// Token: 0x06004A99 RID: 19097 RVA: 0x001A7938 File Offset: 0x001A5B38
	public void LoadPerelivData()
	{
		try
		{
			if (this.DataLoading)
			{
				Debug.LogWarning("ReplaceAdmobPerelivController: data is already loading. returning...");
			}
			else
			{
				if (this._image != null)
				{
					UnityEngine.Object.Destroy(this._image);
				}
				this._image = null;
				this._adUrl = null;
				if (AdsConfigManager.Instance.LastLoadedConfig == null)
				{
					Debug.LogWarning("LoadPerelivData(): AdsConfigManager.Instance.LastLoadedConfig == null");
				}
				else
				{
					FakeInterstitialConfigMemento fakeInterstitialConfig = AdsConfigManager.Instance.LastLoadedConfig.FakeInterstitialConfig;
					int count = fakeInterstitialConfig.ImageUrls.Count;
					if (count <= 0)
					{
						Debug.LogWarning("LoadPerelivData(): fakeInterstitialConfig.ImageUrls.Count == 0");
					}
					else
					{
						int index = UnityEngine.Random.Range(0, count);
						base.StartCoroutine(this.LoadDataCoroutine(fakeInterstitialConfig, index));
					}
				}
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x06004A9A RID: 19098 RVA: 0x001A7A20 File Offset: 0x001A5C20
	private string GetImageURLForOurQuality(string urlString)
	{
		string value = string.Empty;
		if (Screen.height >= 500)
		{
			value = "-Medium";
		}
		if (Screen.height >= 900)
		{
			value = "-Hi";
		}
		urlString = urlString.Insert(urlString.LastIndexOf("."), value);
		return urlString;
	}

	// Token: 0x06004A9B RID: 19099 RVA: 0x001A7A74 File Offset: 0x001A5C74
	private IEnumerator LoadDataCoroutine(FakeInterstitialConfigMemento fakeInterstitialConfig, int index)
	{
		this.DataLoading = true;
		if (fakeInterstitialConfig.ImageUrls.Count == 0)
		{
			Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.ImageUrls.Count == 0");
			yield break;
		}
		string imageUrl = fakeInterstitialConfig.ImageUrls[index % fakeInterstitialConfig.ImageUrls.Count];
		string replaceAdmobUrl = this.GetImageURLForOurQuality(imageUrl);
		WWW imageRequest = Tools.CreateWwwIfNotConnected(replaceAdmobUrl);
		if (imageRequest == null)
		{
			this.DataLoading = false;
			yield break;
		}
		yield return imageRequest;
		if (!string.IsNullOrEmpty(imageRequest.error))
		{
			Debug.LogWarningFormat("ReplaceAdmobPerelivController: {0}", new object[]
			{
				imageRequest.error
			});
			this.DataLoading = false;
			yield break;
		}
		if (!imageRequest.texture)
		{
			this.DataLoading = false;
			Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture = null. returning...");
			yield break;
		}
		if (imageRequest.texture.width < 20)
		{
			this.DataLoading = false;
			Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture is dummy. returning...");
			yield break;
		}
		this._image = imageRequest.texture;
		if (fakeInterstitialConfig.RedirectUrls.Count == 0)
		{
			Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.RedirectUrls.Count == 0");
			yield break;
		}
		this._adUrl = fakeInterstitialConfig.RedirectUrls[index % fakeInterstitialConfig.RedirectUrls.Count];
		this.DataLoading = false;
		yield break;
	}

	// Token: 0x06004A9C RID: 19100 RVA: 0x001A7AAC File Offset: 0x001A5CAC
	private void Awake()
	{
		ReplaceAdmobPerelivController.sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06004A9D RID: 19101 RVA: 0x001A7AC0 File Offset: 0x001A5CC0
	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			if (PromoActionsManager.CurrentUnixTime - this._timeSuspended > 3600L)
			{
				ReplaceAdmobPerelivController._timesShown = 0;
				InterstitialCounter.Instance.Reset();
			}
		}
		else
		{
			this._timeSuspended = PromoActionsManager.CurrentUnixTime;
		}
	}

	// Token: 0x04003725 RID: 14117
	private Texture2D _image;

	// Token: 0x04003726 RID: 14118
	private string _adUrl;

	// Token: 0x04003727 RID: 14119
	private static int _timesWantToShow = -1;

	// Token: 0x04003728 RID: 14120
	private static int _timesShown;

	// Token: 0x04003729 RID: 14121
	private long _timeSuspended;
}
