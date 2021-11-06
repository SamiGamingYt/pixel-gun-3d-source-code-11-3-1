using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
internal sealed class MobileAdManager
{
	// Token: 0x06003C4F RID: 15439 RVA: 0x001390D4 File Offset: 0x001372D4
	private MobileAdManager()
	{
		this.ImageAdFailedToLoadMessage = string.Empty;
		this.VideoAdFailedToLoadMessage = string.Empty;
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x06003C51 RID: 15441 RVA: 0x00139134 File Offset: 0x00137334
	public static MobileAdManager Instance
	{
		get
		{
			return MobileAdManager._instance.Value;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x06003C52 RID: 15442 RVA: 0x00139140 File Offset: 0x00137340
	public MobileAdManager.State VideoInterstitialState
	{
		get
		{
			return MobileAdManager.State.None;
		}
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x00139144 File Offset: 0x00137344
	public static string GetReasonToDismissVideoChestInLobby()
	{
		AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
		if (lastLoadedConfig == null)
		{
			return "Ads config is `null`.";
		}
		if (lastLoadedConfig.Exception != null)
		{
			return lastLoadedConfig.Exception.Message;
		}
		string videoDisabledReason = AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig);
		if (!string.IsNullOrEmpty(videoDisabledReason))
		{
			return videoDisabledReason;
		}
		ChestInLobbyPointMemento chestInLobby = lastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return string.Format("`{0}` config is `null`", chestInLobby.Id);
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
		string disabledReason = chestInLobby.GetDisabledReason(playerCategory);
		if (!string.IsNullOrEmpty(disabledReason))
		{
			return disabledReason;
		}
		int lobbyLevel = ExpController.LobbyLevel;
		if (lobbyLevel < 3)
		{
			return string.Format(CultureInfo.InvariantCulture, "lobbyLevel: {0} < 3", new object[]
			{
				lobbyLevel
			});
		}
		return string.Empty;
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0013920C File Offset: 0x0013740C
	public void DestroyImageInterstitial()
	{
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x00139210 File Offset: 0x00137410
	public void DestroyVideoInterstitial()
	{
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06003C56 RID: 15446 RVA: 0x00139214 File Offset: 0x00137414
	// (set) Token: 0x06003C57 RID: 15447 RVA: 0x0013921C File Offset: 0x0013741C
	public string ImageAdFailedToLoadMessage { get; private set; }

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06003C58 RID: 15448 RVA: 0x00139228 File Offset: 0x00137428
	// (set) Token: 0x06003C59 RID: 15449 RVA: 0x00139230 File Offset: 0x00137430
	public string VideoAdFailedToLoadMessage { get; private set; }

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06003C5A RID: 15450 RVA: 0x0013923C File Offset: 0x0013743C
	// (set) Token: 0x06003C5B RID: 15451 RVA: 0x00139244 File Offset: 0x00137444
	internal bool SuppressShowOnReturnFromPause { get; set; }

	// Token: 0x06003C5C RID: 15452 RVA: 0x00139250 File Offset: 0x00137450
	public static bool UserPredicate(MobileAdManager.Type adType, bool verbose, bool showToPaying = false, bool showToNew = false)
	{
		bool flag = MobileAdManager.IsNewUser();
		bool flag2 = MobileAdManager.IsPayingUser();
		bool flag9;
		if (adType == MobileAdManager.Type.Video)
		{
			int lobbyLevel = ExpController.LobbyLevel;
			bool flag3 = lobbyLevel >= 3;
			bool flag4 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoEnabled;
			bool flag5 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoShowPaying;
			bool flag6 = PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.VideoShowNonpaying;
			bool flag7 = (flag2 && flag5) || (!flag2 && flag6);
			bool flag8 = PlayerPrefs.GetInt("CountRunMenu", 0) >= 3;
			flag9 = (flag4 && flag8 && flag7 && flag3);
			if (verbose)
			{
				Debug.LogFormat("AdIsApplicable ({0}): {1}    Paying: {2},  Need to show: {3},  Session count satisfied: {4},  Lobby level: {5}", new object[]
				{
					adType,
					flag9,
					flag2,
					(!flag2) ? flag6 : flag5,
					flag8,
					lobbyLevel
				});
			}
		}
		else
		{
			bool flag10 = MobileAdManager.IsLongTimeShowBaner();
			flag9 = (PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.ImageEnabled && (!flag || showToNew) && (!flag2 || showToPaying) && flag10);
			if (verbose)
			{
				Dictionary<string, bool> obj = new Dictionary<string, bool>(6)
				{
					{
						"ImageEnabled",
						PromoActionsManager.MobileAdvert != null && PromoActionsManager.MobileAdvert.ImageEnabled
					},
					{
						"isNewUser",
						flag
					},
					{
						"showToNew",
						showToNew
					},
					{
						"isPayingUser",
						flag2
					},
					{
						"showToPaying",
						showToPaying
					},
					{
						"longTimeShowBanner",
						flag10
					}
				};
				string message = string.Format("AdIsApplicable ({0}): {1}    Details: {2}", adType, flag9, Json.Serialize(obj));
				Debug.Log(message);
			}
		}
		return flag9;
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06003C5D RID: 15453 RVA: 0x00139454 File Offset: 0x00137654
	internal static byte[] GuidBytes
	{
		get
		{
			if (MobileAdManager._guid != null && MobileAdManager._guid.Length > 0)
			{
				return MobileAdManager._guid;
			}
			if (PlayerPrefs.HasKey("Guid"))
			{
				try
				{
					Guid guid = new Guid(PlayerPrefs.GetString("Guid"));
					MobileAdManager._guid = guid.ToByteArray();
				}
				catch
				{
					Guid guid2 = Guid.NewGuid();
					MobileAdManager._guid = guid2.ToByteArray();
					PlayerPrefs.SetString("Guid", guid2.ToString("D"));
					PlayerPrefs.Save();
				}
			}
			else
			{
				Guid guid3 = Guid.NewGuid();
				MobileAdManager._guid = guid3.ToByteArray();
				PlayerPrefs.SetString("Guid", guid3.ToString("D"));
				PlayerPrefs.Save();
			}
			return MobileAdManager._guid;
		}
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x00139538 File Offset: 0x00137738
	internal static void RefreshBytes()
	{
		Guid guid = new Guid(MobileAdManager._guid);
		PlayerPrefs.SetString("Guid", guid.ToString("D"));
		PlayerPrefs.Save();
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x0013956C File Offset: 0x0013776C
	internal static MobileAdManager.SampleGroup GetSempleGroup()
	{
		byte b = MobileAdManager.GuidBytes[0];
		return (b % 2 != 0) ? MobileAdManager.SampleGroup.Video : MobileAdManager.SampleGroup.Image;
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x00139590 File Offset: 0x00137790
	public static bool IsNewUserOldMetod()
	{
		string @string = PlayerPrefs.GetString("First Launch (Advertisement)", string.Empty);
		DateTimeOffset right;
		return string.IsNullOrEmpty(@string) || !DateTimeOffset.TryParse(@string, out right) || (DateTimeOffset.Now - right).TotalDays < 7.0;
	}

	// Token: 0x06003C61 RID: 15457 RVA: 0x001395E8 File Offset: 0x001377E8
	private static bool IsLongTimeShowBaner()
	{
		string @string = PlayerPrefs.GetString(Defs.LastTimeShowBanerKey, string.Empty);
		if (string.IsNullOrEmpty(@string))
		{
			return true;
		}
		DateTime d;
		if (!DateTime.TryParse(@string, out d))
		{
			return false;
		}
		DateTime utcNow = DateTime.UtcNow;
		double totalSeconds = (utcNow - d).TotalSeconds;
		return totalSeconds > (double)PromoActionsManager.MobileAdvert.TimeoutBetweenShowInterstitial;
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x0013964C File Offset: 0x0013784C
	private static bool IsNewUser()
	{
		int @int = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1);
		return @int <= PromoActionsManager.MobileAdvert.CountSessionNewPlayer;
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x00139678 File Offset: 0x00137878
	public static bool IsPayingUser()
	{
		return StoreKitEventListener.IsPayingUser();
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06003C64 RID: 15460 RVA: 0x00139680 File Offset: 0x00137880
	private string ImageInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null || PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0)
			{
				return "ca-app-pub-5590536419057381/1950086558";
			}
			return this.AdmobImageAdUnitIds[this._imageAdUnitIdIndex % this.AdmobImageAdUnitIds.Count];
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06003C65 RID: 15461 RVA: 0x001396D0 File Offset: 0x001378D0
	private string VideoInterstitialUnitId
	{
		get
		{
			if (PromoActionsManager.MobileAdvert == null)
			{
				return "ca-app-pub-5590536419057381/2096360557";
			}
			if (this.AdmobVideoAdUnitIds.Count == 0)
			{
				return (!string.IsNullOrEmpty(PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId)) ? PromoActionsManager.MobileAdvert.AdmobVideoAdUnitId : "ca-app-pub-5590536419057381/2096360557";
			}
			return this.AdmobVideoAdUnitIds[this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count];
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06003C66 RID: 15462 RVA: 0x00139744 File Offset: 0x00137944
	private List<string> AdmobVideoAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobVideoAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobVideoIdGroups[this._videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count];
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06003C67 RID: 15463 RVA: 0x00139798 File Offset: 0x00137998
	private List<string> AdmobImageAdUnitIds
	{
		get
		{
			if (PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0)
			{
				return PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds;
			}
			return PromoActionsManager.MobileAdvert.AdmobImageIdGroups[this._imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count];
		}
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x001397EC File Offset: 0x001379EC
	internal bool SwitchImageAdUnitId()
	{
		int imageAdUnitIdIndex = this._imageAdUnitIdIndex;
		string imageInterstitialUnitId = this.ImageInterstitialUnitId;
		this._imageAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Switching image ad unit id from {0} ({1}) to {2} ({3})", new object[]
			{
				imageAdUnitIdIndex,
				MobileAdManager.RemovePrefix(imageInterstitialUnitId),
				this._imageAdUnitIdIndex,
				MobileAdManager.RemovePrefix(this.ImageInterstitialUnitId)
			});
			Debug.Log(message);
		}
		return PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0 || this._imageAdUnitIdIndex % PromoActionsManager.MobileAdvert.AdmobImageAdUnitIds.Count == 0;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x00139894 File Offset: 0x00137A94
	internal bool SwitchVideoAdUnitId()
	{
		int videoAdUnitIdIndex = this._videoAdUnitIdIndex;
		string videoInterstitialUnitId = this.VideoInterstitialUnitId;
		this._videoAdUnitIdIndex++;
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Switching video ad unit id from {0} ({1}) to {2} ({3}); group index {4}", new object[]
			{
				videoAdUnitIdIndex,
				MobileAdManager.RemovePrefix(videoInterstitialUnitId),
				this._videoAdUnitIdIndex,
				MobileAdManager.RemovePrefix(this.VideoInterstitialUnitId),
				this._videoIdGroupIndex
			});
			Debug.Log(message);
		}
		return this.AdmobVideoAdUnitIds.Count == 0 || this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count == 0;
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00139944 File Offset: 0x00137B44
	internal bool SwitchImageIdGroup()
	{
		int imageIdGroupIndex = this._imageIdGroupIndex;
		List<string> obj = this.AdmobImageAdUnitIds.Select(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string text = Json.Serialize(obj);
		this._imageIdGroupIndex++;
		this._imageAdUnitIdIndex = 0;
		List<string> obj2 = this.AdmobImageAdUnitIds.Select(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string text2 = Json.Serialize(obj2);
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Switching image id group from {0} ({1}) to {2} ({3})", new object[]
			{
				imageIdGroupIndex,
				text,
				this._imageIdGroupIndex,
				text2
			});
			Debug.Log(message);
		}
		return PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0 || this._imageIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobImageIdGroups.Count == 0;
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00139A2C File Offset: 0x00137C2C
	internal bool SwitchVideoIdGroup()
	{
		int videoIdGroupIndex = this._videoIdGroupIndex;
		List<string> obj = this.AdmobVideoAdUnitIds.Select(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string text = Json.Serialize(obj);
		this._videoIdGroupIndex++;
		this._videoAdUnitIdIndex = 0;
		List<string> obj2 = this.AdmobVideoAdUnitIds.Select(new Func<string, string>(MobileAdManager.RemovePrefix)).ToList<string>();
		string text2 = Json.Serialize(obj2);
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Switching video id group from {0} ({1}) to {2} ({3})", new object[]
			{
				videoIdGroupIndex,
				text,
				this._videoIdGroupIndex,
				text2
			});
			Debug.Log(message);
		}
		return PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0 || this._videoIdGroupIndex % PromoActionsManager.MobileAdvert.AdmobVideoIdGroups.Count == 0;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x00139B14 File Offset: 0x00137D14
	internal static string RemovePrefix(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		int num = s.IndexOf('/');
		return (num <= 0) ? s : s.Remove(0, num);
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x00139B50 File Offset: 0x00137D50
	internal bool ResetVideoAdUnitId()
	{
		int videoAdUnitIdIndex = this._videoAdUnitIdIndex;
		string videoInterstitialUnitId = this.VideoInterstitialUnitId;
		int videoIdGroupIndex = this._videoIdGroupIndex;
		this._videoAdUnitIdIndex = 0;
		this._videoIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Resetting video group from {0} to {1}", videoIdGroupIndex, this._videoIdGroupIndex);
			Debug.Log(message);
		}
		return true;
	}

	// Token: 0x06003C6E RID: 15470 RVA: 0x00139BB0 File Offset: 0x00137DB0
	internal bool ResetImageAdUnitId()
	{
		int imageAdUnitIdIndex = this._imageAdUnitIdIndex;
		string imageInterstitialUnitId = this.ImageInterstitialUnitId;
		int imageIdGroupIndex = this._imageIdGroupIndex;
		this._imageAdUnitIdIndex = 0;
		this._imageIdGroupIndex = 0;
		if (Defs.IsDeveloperBuild)
		{
			string message = string.Format("Resetting image ad unit id from {0} to {1}; group index from {2} to 0", imageAdUnitIdIndex, this._imageAdUnitIdIndex, imageIdGroupIndex);
			Debug.Log(message);
		}
		return true;
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x06003C6F RID: 15471 RVA: 0x00139C14 File Offset: 0x00137E14
	internal int ImageAdUnitIndexClamped
	{
		get
		{
			if (this.AdmobImageAdUnitIds.Count == 0)
			{
				return -1;
			}
			return this._imageAdUnitIdIndex % this.AdmobImageAdUnitIds.Count;
		}
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00139C48 File Offset: 0x00137E48
	internal int VideoAdUnitIndexClamped
	{
		get
		{
			if (this.AdmobVideoAdUnitIds.Count == 0)
			{
				return -1;
			}
			return this._videoAdUnitIdIndex % this.AdmobVideoAdUnitIds.Count;
		}
	}

	// Token: 0x04002C88 RID: 11400
	internal const string TextInterstitialUnitId = "ca-app-pub-5590536419057381/7885668153";

	// Token: 0x04002C89 RID: 11401
	internal const string DefaultImageInterstitialUnitId = "ca-app-pub-5590536419057381/1950086558";

	// Token: 0x04002C8A RID: 11402
	internal const string DefaultVideoInterstitialUnitId = "ca-app-pub-5590536419057381/2096360557";

	// Token: 0x04002C8B RID: 11403
	private static byte[] _guid = new byte[0];

	// Token: 0x04002C8C RID: 11404
	private int _imageAdUnitIdIndex;

	// Token: 0x04002C8D RID: 11405
	private int _imageIdGroupIndex;

	// Token: 0x04002C8E RID: 11406
	private int _videoAdUnitIdIndex;

	// Token: 0x04002C8F RID: 11407
	private int _videoIdGroupIndex;

	// Token: 0x04002C90 RID: 11408
	private static readonly Lazy<MobileAdManager> _instance = new Lazy<MobileAdManager>(() => new MobileAdManager());

	// Token: 0x020006C5 RID: 1733
	public enum Type
	{
		// Token: 0x04002C96 RID: 11414
		Image,
		// Token: 0x04002C97 RID: 11415
		Video
	}

	// Token: 0x020006C6 RID: 1734
	public enum State
	{
		// Token: 0x04002C99 RID: 11417
		None,
		// Token: 0x04002C9A RID: 11418
		Idle,
		// Token: 0x04002C9B RID: 11419
		Loaded
	}

	// Token: 0x020006C7 RID: 1735
	internal enum SampleGroup
	{
		// Token: 0x04002C9D RID: 11421
		Unknown,
		// Token: 0x04002C9E RID: 11422
		Video,
		// Token: 0x04002C9F RID: 11423
		Image
	}
}
