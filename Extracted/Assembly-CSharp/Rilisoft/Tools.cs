using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000742 RID: 1858
	public sealed class Tools
	{
		// Token: 0x06004158 RID: 16728 RVA: 0x0015C3B8 File Offset: 0x0015A5B8
		public static bool EscapePressed()
		{
			return !BackSystem.Active && Input.GetKeyUp(KeyCode.Escape);
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06004159 RID: 16729 RVA: 0x0015C3D0 File Offset: 0x0015A5D0
		public static RuntimePlatform RuntimePlatform
		{
			get
			{
				return Application.platform;
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x0015C3D8 File Offset: 0x0015A5D8
		private static bool ConnectedToPhoton()
		{
			return PhotonNetwork.room != null;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x0015C3E8 File Offset: 0x0015A5E8
		internal static WWW CreateWww(string url)
		{
			WWW www = new WWW(url);
			if (Application.isEditor && FriendsController.isDebugLogWWW)
			{
				string[] source = url.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = source.LastOrDefault<string>() ?? url;
				if (www != null)
				{
					Debug.LogFormat("<color=yellow>{0}</color>", new object[]
					{
						text
					});
				}
				else
				{
					Debug.LogFormat("<color=orange>Skipping {0}</color>", new object[]
					{
						text
					});
				}
			}
			return www;
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x0015C468 File Offset: 0x0015A668
		internal static WWW CreateWww(string url, WWWForm form, string comment = "")
		{
			return Tools.CreateWwwIf(true, url, form, comment, null);
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0015C474 File Offset: 0x0015A674
		internal static WWW CreateWwwIfNotConnected(string url, WWWForm form, string comment = "", Dictionary<string, string> headers = null)
		{
			return Tools.CreateWwwIf(!Tools.ConnectedToPhoton(), url, form, comment, headers);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x0015C488 File Offset: 0x0015A688
		internal static WWW CreateWwwIf(bool condition, string url, WWWForm form, string comment = "", Dictionary<string, string> headers = null)
		{
			WWW result = (!condition) ? null : ((headers == null) ? new WWW(url, form) : new WWW(url, form.data, headers));
			if (Application.isEditor && FriendsController.isDebugLogWWW)
			{
				byte[] array = form.data ?? new byte[0];
				string @string = Encoding.UTF8.GetString(array, 0, array.Length);
				string[] source = @string.Split(new char[]
				{
					'&'
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = source.FirstOrDefault((string p) => p.StartsWith("action=")) ?? url;
				string text2 = (!string.IsNullOrEmpty(comment)) ? string.Format("{0}; {1}", text, comment) : text;
				if (condition)
				{
					Debug.LogFormat("<b><color=yellow>{0}</color></b>", new object[]
					{
						text2
					});
				}
				else
				{
					Debug.LogFormat("<b><color=orange>Skipping {0}</color></b>", new object[]
					{
						text2
					});
				}
			}
			return result;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x0015C594 File Offset: 0x0015A794
		internal static WWW CreateWwwIfNotConnected(string url)
		{
			WWW www = (!Tools.ConnectedToPhoton()) ? new WWW(url) : null;
			if (Application.isEditor && FriendsController.isDebugLogWWW)
			{
				string[] source = url.Split(new char[]
				{
					'/'
				}, StringSplitOptions.RemoveEmptyEntries);
				string text = source.LastOrDefault<string>() ?? url;
				if (www != null)
				{
					Debug.LogFormat("<color=yellow>{0}</color>", new object[]
					{
						text
					});
				}
				else
				{
					Debug.LogFormat("<color=orange>Skipping {0}</color>", new object[]
					{
						text
					});
				}
			}
			return www;
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x0015C624 File Offset: 0x0015A824
		public static bool ParseDateTimeFromPlayerPrefs(string dateKey, out DateTime parsedDate)
		{
			string @string = Storager.getString(dateKey, false);
			return DateTime.TryParse(@string, out parsedDate);
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0015C640 File Offset: 0x0015A840
		public static DateTime GetCurrentTimeByUnixTime(long unixTime)
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return dateTime.AddSeconds((double)unixTime);
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x0015C66C File Offset: 0x0015A86C
		public static void AddSessionNumber()
		{
			int @int = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1);
			PlayerPrefs.SetInt(Defs.SessionNumberKey, @int + 1);
			string @string = PlayerPrefs.GetString(Defs.LastTimeSessionDayKey, string.Empty);
			DateTimeOffset left;
			DateTimeOffset.TryParse(DateTimeOffset.UtcNow.ToString("s"), out left);
			DateTimeOffset right;
			if (string.IsNullOrEmpty(@string) || (DateTimeOffset.TryParse(@string, out right) && ((!Defs.IsDeveloperBuild && (left - right).TotalHours > 23.0) || (Defs.IsDeveloperBuild && (left - right).TotalMinutes > 3.0))))
			{
				int int2 = PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 0);
				int num = int2 + 1;
				PlayerPrefs.SetInt(Defs.SessionDayNumberKey, num);
				PlayerPrefs.SetString(Defs.LastTimeSessionDayKey, DateTimeOffset.UtcNow.ToString("s"));
				GlobalGameController.CountDaySessionInCurrentVersion = GlobalGameController.CountDaySessionInCurrentVersion;
				AnalyticsStuff.SendInGameDay(num);
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06004163 RID: 16739 RVA: 0x0015C774 File Offset: 0x0015A974
		public static int AllWithoutDamageCollidersMask
		{
			get
			{
				return -5 & ~(1 << LayerMask.NameToLayer("DamageCollider"));
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06004164 RID: 16740 RVA: 0x0015C78C File Offset: 0x0015A98C
		public static int AllWithoutMyPlayerMask
		{
			get
			{
				return -5 & ~(1 << LayerMask.NameToLayer("MyPlayer"));
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06004165 RID: 16741 RVA: 0x0015C7A4 File Offset: 0x0015A9A4
		public static int AllWithoutDamageCollidersMaskAndWithoutRocket
		{
			get
			{
				return -5 & ~(1 << LayerMask.NameToLayer("DamageCollider")) & ~(1 << LayerMask.NameToLayer("Rocket")) & ~(1 << LayerMask.NameToLayer("TransparentFX"));
			}
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x0015C7DC File Offset: 0x0015A9DC
		public static int AllAvailabelBotRaycastMask
		{
			get
			{
				return -5 & ~(1 << LayerMask.NameToLayer("DamageCollider")) & ~(1 << LayerMask.NameToLayer("NotDetectMobRaycast"));
			}
		}

		// Token: 0x06004167 RID: 16743 RVA: 0x0015C810 File Offset: 0x0015AA10
		public static Rect SuccessMessageRect()
		{
			return new Rect((float)(Screen.width / 2) - (float)Screen.height * 0.5f, (float)Screen.height * 0.5f - (float)Screen.height * 0.0525f, (float)Screen.height, (float)Screen.height * 0.105f);
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x0015C864 File Offset: 0x0015AA64
		public static long CurrentUnixTime
		{
			get
			{
				DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
				return (long)(DateTime.UtcNow - d).TotalSeconds;
			}
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x0015C89C File Offset: 0x0015AA9C
		public static void SetVibibleNguiObjectByAlpha(GameObject nguiObject, bool isVisible)
		{
			UIWidget component = nguiObject.GetComponent<UIWidget>();
			if (component == null)
			{
				return;
			}
			component.alpha = ((!isVisible) ? 0.001f : 1f);
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0015C8D8 File Offset: 0x0015AAD8
		public static void SetLayerRecursively(GameObject obj, int newLayer)
		{
			if (null == obj)
			{
				return;
			}
			obj.layer = newLayer;
			int childCount = obj.transform.childCount;
			Transform transform = obj.transform;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (!(null == child))
				{
					Tools.SetLayerRecursively(child.gameObject, newLayer);
				}
			}
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x0015C944 File Offset: 0x0015AB44
		public static void SetTextureRecursivelyFrom(GameObject obj, Texture txt, GameObject[] stopObjs)
		{
			Transform transform = obj.transform;
			int childCount = obj.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = transform.GetChild(i);
				bool flag = false;
				foreach (GameObject o in stopObjs)
				{
					if (child.gameObject.Equals(o))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (child.gameObject.GetComponent<Renderer>() && child.gameObject.GetComponent<Renderer>().material)
					{
						child.gameObject.GetComponent<Renderer>().material.mainTexture = txt;
					}
					flag = false;
					foreach (GameObject o2 in stopObjs)
					{
						if (child.gameObject.Equals(o2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Tools.SetTextureRecursivelyFrom(child.gameObject, txt, stopObjs);
					}
				}
			}
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x0015CA5C File Offset: 0x0015AC5C
		public static Color[] FlipColorsHorizontally(Color[] colors, int width, int height)
		{
			Color[] array = new Color[colors.Length];
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					array[i + width * j] = colors[width - i - 1 + width * j];
				}
			}
			return array;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x0015CABC File Offset: 0x0015ACBC
		public static Texture2D GetPreviewFromSkin(string skinStr, Tools.PreviewType type)
		{
			Texture2D texture2D;
			if (!string.IsNullOrEmpty(skinStr) && !skinStr.Equals("empty"))
			{
				texture2D = new Texture2D(64, 32);
				texture2D.LoadImage(Convert.FromBase64String(skinStr));
			}
			else
			{
				texture2D = Resources.Load<Texture2D>(ResPath.Combine(Defs.MultSkinsDirectoryName, "multi_skin_1"));
			}
			Texture2D texture2D2 = null;
			if (type != Tools.PreviewType.Head)
			{
				if (type == Tools.PreviewType.HeadAndBody)
				{
					texture2D2 = new Texture2D(16, 14, TextureFormat.ARGB32, false);
					for (int i = 0; i < texture2D2.width; i++)
					{
						for (int j = 0; j < texture2D2.height; j++)
						{
							texture2D2.SetPixel(i, j, Color.clear);
						}
					}
					texture2D2.SetPixels(4, 6, 8, 8, texture2D.GetPixels(8, 16, 8, 8));
					texture2D2.SetPixels(4, 0, 8, 6, texture2D.GetPixels(20, 6, 8, 6));
					texture2D2.SetPixels(0, 0, 4, 6, texture2D.GetPixels(44, 6, 4, 6));
					texture2D2.SetPixels(12, 0, 4, 6, Tools.FlipColorsHorizontally(texture2D.GetPixels(44, 6, 4, 6), 4, 6));
				}
			}
			else
			{
				texture2D2 = new Texture2D(8, 8, TextureFormat.ARGB32, false);
				for (int k = 0; k < texture2D2.width; k++)
				{
					for (int l = 0; l < texture2D2.height; l++)
					{
						texture2D2.SetPixel(k, l, Color.clear);
					}
				}
				texture2D2.SetPixels(0, 0, 8, 8, texture2D.GetPixels(8, 16, 8, 8));
			}
			texture2D2.anisoLevel = 1;
			texture2D2.mipMapBias = -0.5f;
			texture2D2.Apply();
			texture2D2.filterMode = FilterMode.Point;
			return texture2D2;
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x0015CC64 File Offset: 0x0015AE64
		internal static T DeserializeJson<T>(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return default(T);
			}
			T result;
			try
			{
				result = JsonUtility.FromJson<T>(json);
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
				result = default(T);
			}
			return result;
		}

		// Token: 0x02000743 RID: 1859
		public enum PreviewType
		{
			// Token: 0x04002FC4 RID: 12228
			Head,
			// Token: 0x04002FC5 RID: 12229
			HeadAndBody
		}
	}
}
