using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x0200054F RID: 1359
	[Obsolete]
	internal sealed class AnalyticsLegacy
	{
		// Token: 0x06002F44 RID: 12100 RVA: 0x000F6D98 File Offset: 0x000F4F98
		private static string GetFlurryApiKey()
		{
			return AnalyticsLegacy.GetFlurryApiKeyRelease();
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000F6DA0 File Offset: 0x000F4FA0
		private static string GetFlurryApiKeyDebug()
		{
			return string.Empty;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000F6DA8 File Offset: 0x000F4FA8
		private static string GetFlurryApiKeyRelease()
		{
			return "J8K92PR3VD22BX8ZSZ7W";
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000F6DB0 File Offset: 0x000F4FB0
		private static string GetPlayingMode()
		{
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			if (ordinalIgnoreCase.Equals(SceneManager.GetActiveScene().name, Defs.MainMenuScene))
			{
				return "Main Menu";
			}
			if (ordinalIgnoreCase.Equals(SceneLoader.ActiveSceneName, "ConnectScene"))
			{
				return "Connect Scene";
			}
			if (ordinalIgnoreCase.Equals(SceneLoader.ActiveSceneName, "ConnectSceneSandbox"))
			{
				return "Connect Scene Sandbox";
			}
			if (!Defs.IsSurvival && !Defs.isMulti)
			{
				return "Campaign";
			}
			if (Defs.IsSurvival)
			{
				return (!Defs.isMulti) ? "Survival" : "Time Survival";
			}
			if (Defs.isCompany)
			{
				return "Team Battle";
			}
			if (Defs.isFlag)
			{
				return "Flag Capture";
			}
			if (Defs.isHunger)
			{
				return "Deadly Games";
			}
			if (Defs.isCapturePoints)
			{
				return "Capture Points";
			}
			return (!Defs.isInet) ? "Deathmatch Local" : "Deathmatch Worldwide";
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000F6EB4 File Offset: 0x000F50B4
		private static string CurrentContextForNonePlaceInBecomePaying()
		{
			string text = string.Empty;
			try
			{
				if (Defs.inRespawnWindow)
				{
					text += " Killcam";
				}
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null)
				{
					text += " PlayerExists";
				}
				if (NetworkStartTableNGUIController.IsEndInterfaceShown())
				{
					text += " NetworkStartTable_End";
				}
				if (NetworkStartTableNGUIController.IsStartInterfaceShown())
				{
					text += " NetworkStartTable_Start";
				}
				if (ShopNGUIController.GuiActive)
				{
					text += " InShop";
				}
				string text2 = (AnalyticsLegacy.ModeNameForPurchasesAnalytics(false) ?? string.Empty).Replace(" ", string.Empty);
				if (text2 != string.Empty)
				{
					text = text + " " + text2;
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in CurrentContextForNonePlaceInBecomePaying: " + arg);
			}
			return text;
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000F6FC8 File Offset: 0x000F51C8
		private static string ModeNameForPurchasesAnalytics(bool forNormalMultyModesUseMultyplayer = false)
		{
			try
			{
				bool flag = !Defs.IsSurvival && !Defs.isMulti;
				if (flag)
				{
					return "Campaign";
				}
				if (Defs.IsSurvival && !Defs.isMulti)
				{
					return "Arena";
				}
				string name = SceneManager.GetActiveScene().name;
				if (Defs.isMulti && name != Defs.MainMenuScene && name != "Clans")
				{
					if (Defs.isDaterRegim)
					{
						return "Sandbox";
					}
					if (forNormalMultyModesUseMultyplayer)
					{
						return "Multiplayer";
					}
					if (Defs.isCompany)
					{
						return "Team Battle";
					}
					if (Defs.isCapturePoints)
					{
						return "Point Capture";
					}
					if (Defs.isCOOP)
					{
						return "COOP Survival";
					}
					if (Defs.isFlag)
					{
						return "Flag Capture";
					}
					if (Defs.isHunger)
					{
						return "Deadly Games";
					}
					return "Deathmatch";
				}
			}
			catch (Exception arg)
			{
				Debug.LogError("Exception in ModeNameForPurchasesAnalytics: " + arg);
				return string.Empty;
			}
			return string.Empty;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000F7138 File Offset: 0x000F5338
		private static bool IsAdditionalLoggingAvailable()
		{
			bool result;
			try
			{
				if (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer)
				{
					result = false;
				}
				else
				{
					result = (File.Exists(AnalyticsLegacy.ConvertFromBase64("L0FwcGxpY2F0aW9ucy9DeWRpYS5hcHA=")) || File.Exists(AnalyticsLegacy.ConvertFromBase64("L0xpYnJhcnkvTW9iaWxlU3Vic3RyYXRlL01vYmlsZVN1YnN0cmF0ZS5keWxpYg==")) || File.Exists(AnalyticsLegacy.ConvertFromBase64("L2Jpbi9iYXNo")) || File.Exists(AnalyticsLegacy.ConvertFromBase64("L3Vzci9zYmluL3NzaGQ=")) || File.Exists(AnalyticsLegacy.ConvertFromBase64("L2V0Yy9hcHQ=")) || Directory.Exists(AnalyticsLegacy.ConvertFromBase64("L3ByaXZhdGUvdmFyL2xpYi9hcHQv")));
				}
			}
			catch (Exception arg)
			{
				Debug.LogWarning("Exception in IsAdditionalLoggingAvailable: " + arg);
				result = false;
			}
			return result;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000F7214 File Offset: 0x000F5414
		private static string ConvertFromBase64(string s)
		{
			byte[] array = Convert.FromBase64String(s);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000F7238 File Offset: 0x000F5438
		private static void CheckForEdnermanApp()
		{
			Defs.EnderManAvailable = false;
			HttpWebRequest request = AnalyticsLegacy.RequestAppWithID(MainMenu.iTunesEnderManID);
			AnalyticsLegacy.DoWithResponse(request, delegate(HttpWebResponse response)
			{
				string text = new StreamReader(response.GetResponseStream()).ReadToEnd();
				if (text.Contains("Slender") && text.Length > 250)
				{
					Defs.EnderManAvailable = true;
				}
			});
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000F727C File Offset: 0x000F547C
		private static HttpWebRequest RequestAppWithID(string id)
		{
			return (HttpWebRequest)WebRequest.Create("http://itunes.apple.com/lookup?id=" + id);
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x000F72A0 File Offset: 0x000F54A0
		private static void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
		{
			Action action = delegate()
			{
				request.BeginGetResponse(delegate(IAsyncResult iar)
				{
					HttpWebResponse obj = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
					responseAction(obj);
				}, request);
			};
			action.BeginInvoke(delegate(IAsyncResult iar)
			{
				Action action2 = (Action)iar.AsyncState;
				action2.EndInvoke(iar);
			}, action);
		}
	}
}
