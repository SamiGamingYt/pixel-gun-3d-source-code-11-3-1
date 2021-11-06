using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006D1 RID: 1745
	internal sealed class PersistentCacheManager : MonoBehaviour
	{
		// Token: 0x06003CB5 RID: 15541 RVA: 0x0013B6D0 File Offset: 0x001398D0
		private PersistentCacheManager()
		{
			this._encryptedPlayerPrefs = new Lazy<EncryptedPlayerPrefs>(new Func<EncryptedPlayerPrefs>(this.CreateEncryptedPlayerPrefs));
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x0013B734 File Offset: 0x00139934
		internal bool IsEnabled
		{
			get
			{
				return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x0013B744 File Offset: 0x00139944
		internal Task FirstResponse
		{
			get
			{
				return this._firstResponsePromise.Task;
			}
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x0013B754 File Offset: 0x00139954
		public Task StartDownloadSignaturesLoop()
		{
			TaskCompletionSource<Dictionary<string, string>> firstResponsePromise = this._firstResponsePromise;
			if (!this.IsEnabled)
			{
				firstResponsePromise.TrySetException(new NotSupportedException());
				return firstResponsePromise.Task;
			}
			this._cancellationTokenSource.Cancel();
			this._cancellationTokenSource = new CancellationTokenSource();
			float delaySecondsInCaseOfSuccess = (!Application.isEditor) ? ((!Defs.IsDeveloperBuild) ? 600f : 60f) : 30f;
			float delaySecondsInCaseOfFailure = (!Application.isEditor) ? ((!Defs.IsDeveloperBuild) ? 60f : 6f) : 3f;
			base.StartCoroutine(this.DownloadSignaturesLoopCoroutine(delaySecondsInCaseOfSuccess, delaySecondsInCaseOfFailure, this._cancellationTokenSource.Token, firstResponsePromise));
			return firstResponsePromise.Task;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x0013B81C File Offset: 0x00139A1C
		public string GetValue(string keyUrl)
		{
			if (keyUrl == null)
			{
				throw new ArgumentNullException("keyUrl");
			}
			if (!this.IsEnabled)
			{
				return null;
			}
			if (string.IsNullOrEmpty(keyUrl))
			{
				return string.Empty;
			}
			string segmentsAsString = PersistentCacheManager.GetSegmentsAsString(keyUrl);
			if (string.IsNullOrEmpty(segmentsAsString))
			{
				return string.Empty;
			}
			string storageKey = PersistentCacheManager.GetStorageKey(segmentsAsString);
			string @string = this.EncryptedPlayerPrefs.GetString(storageKey);
			if (string.IsNullOrEmpty(@string))
			{
				return string.Empty;
			}
			string result;
			try
			{
				PersistentCacheManager.CacheEntry cacheEntry = JsonUtility.FromJson<PersistentCacheManager.CacheEntry>(@string);
				string x;
				if (this._signatures.TryGetValue(segmentsAsString, out x) && StringComparer.Ordinal.Equals(x, cacheEntry.signature))
				{
					result = (cacheEntry.payload ?? string.Empty);
				}
				else
				{
					if (this.EncryptedPlayerPrefs.HasKey(storageKey))
					{
						this.EncryptedPlayerPrefs.SetString(storageKey, string.Empty);
					}
					result = string.Empty;
				}
			}
			catch (Exception exception)
			{
				Debug.LogErrorFormat("Failed to deserialize {0}: \"{1}\"", new object[]
				{
					typeof(PersistentCacheManager.CacheEntry).Name,
					@string
				});
				Debug.LogException(exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0013B970 File Offset: 0x00139B70
		public bool SetValue(string keyUrl, string value)
		{
			if (keyUrl == null)
			{
				throw new ArgumentNullException("keyUrl");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!this.IsEnabled)
			{
				return false;
			}
			string segmentsAsString = PersistentCacheManager.GetSegmentsAsString(keyUrl);
			if (string.IsNullOrEmpty(segmentsAsString))
			{
				return false;
			}
			string signature;
			if (!this._signatures.TryGetValue(segmentsAsString, out signature))
			{
				return false;
			}
			string storageKey = PersistentCacheManager.GetStorageKey(segmentsAsString);
			PersistentCacheManager.CacheEntry cacheEntry = new PersistentCacheManager.CacheEntry
			{
				signature = signature,
				payload = value
			};
			string value2 = JsonUtility.ToJson(cacheEntry);
			this.EncryptedPlayerPrefs.SetString(storageKey, value2);
			return true;
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06003CBC RID: 15548 RVA: 0x0013BA18 File Offset: 0x00139C18
		public static PersistentCacheManager Instance
		{
			get
			{
				return PersistentCacheManager._instance.Value;
			}
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0013BA24 File Offset: 0x00139C24
		internal static void DebugReportCacheHit(string url)
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			string text = string.Format("Cache hit: {0}", url ?? string.Empty);
			if (Application.isEditor)
			{
				Debug.LogFormat("<color=green><b>{0}</b></color>", new object[]
				{
					text
				});
			}
			else
			{
				Debug.Log(text);
			}
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0013BA80 File Offset: 0x00139C80
		internal static void DebugReportCacheMiss(string url)
		{
			if (!Defs.IsDeveloperBuild)
			{
				return;
			}
			string text = string.Format("Cache miss: {0}", url ?? string.Empty);
			if (Application.isEditor)
			{
				Debug.LogFormat("<color=orange><b>{0}</b></color>", new object[]
				{
					text
				});
			}
			else
			{
				Debug.Log(text);
			}
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0013BADC File Offset: 0x00139CDC
		private IEnumerator DownloadSignaturesLoopCoroutine(float delaySecondsInCaseOfSuccess, float delaySecondsInCaseOfFailure, CancellationToken cancellationToken, TaskCompletionSource<Dictionary<string, string>> promise)
		{
			int i = 0;
			while (!cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<Dictionary<string, string>> promiseMayFail = new TaskCompletionSource<Dictionary<string, string>>();
				base.StartCoroutine(this.DownloadSignaturesOnceCoroutine(cancellationToken, promiseMayFail, i));
				Task<Dictionary<string, string>> taskMayFail = promiseMayFail.Task;
				taskMayFail.ContinueWith(delegate(Task<Dictionary<string, string>> t)
				{
					if (t.IsFaulted || t.IsCanceled)
					{
						return;
					}
					promise.TrySetResult(t.Result);
				});
				float nextTimeToRequest = Time.realtimeSinceStartup + delaySecondsInCaseOfSuccess;
				while (Time.realtimeSinceStartup < nextTimeToRequest && !taskMayFail.IsFaulted)
				{
					yield return null;
				}
				if (taskMayFail.IsFaulted)
				{
					float restTimeToWait = nextTimeToRequest - Time.realtimeSinceStartup;
					float delaySeconds = Mathf.Clamp(restTimeToWait, 0f, delaySecondsInCaseOfFailure);
					yield return new WaitForSeconds(delaySeconds);
				}
				i++;
			}
			yield break;
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x0013BB34 File Offset: 0x00139D34
		private IEnumerator DownloadSignaturesOnceCoroutine(CancellationToken cancellationToken, TaskCompletionSource<Dictionary<string, string>> promise, int context = 0)
		{
			string[] urls = new string[]
			{
				URLs.PromoActions,
				URLs.PromoActionsTest,
				URLs.LobbyNews,
				URLs.Advert,
				URLs.NewAdvertisingConfig,
				URLs.NewPerelivConfig,
				ChestBonusModel.GetUrlForDownloadBonusesData(),
				(!ABTestController.useBuffSystem) ? URLs.BuffSettings1031 : URLs.BuffSettings1050,
				URLs.BestBuy,
				URLs.PixelbookSettings,
				BalanceController.balanceURL
			};
			TaskCompletionSource<string> signaturesStringPromise = new TaskCompletionSource<string>();
			yield return base.StartCoroutine(PersistentCacheManager.DownloadSignaturesCoroutine(urls, cancellationToken, signaturesStringPromise));
			yield return base.StartCoroutine(PersistentCacheManager.WaitCompletionAndParseSignaturesCoroutine(signaturesStringPromise.Task, promise));
			if (promise.Task.IsCanceled)
			{
				Debug.LogWarningFormat("DownloadSignaturesOnceCoroutine({0}) cancelled.", new object[]
				{
					context
				});
				yield break;
			}
			if (promise.Task.IsFaulted)
			{
				Debug.LogWarningFormat("DownloadSignaturesOnceCoroutine({0}) failed: {1}", new object[]
				{
					context,
					promise.Task.Exception.InnerException
				});
				foreach (string url in urls)
				{
					string segments = PersistentCacheManager.GetSegmentsAsString(url);
					string storageKey = PersistentCacheManager.GetStorageKey(segments);
					if (this.EncryptedPlayerPrefs.HasKey(storageKey))
					{
						this.EncryptedPlayerPrefs.SetString(storageKey, string.Empty);
					}
				}
				this._signatures.Clear();
				yield break;
			}
			Dictionary<string, string> d = promise.Task.Result;
			if (Defs.IsDeveloperBuild && !Application.isEditor)
			{
				string format = (!Application.isEditor) ? "DownloadSignaturesOnceCoroutine({0}) succeeded: {1}" : "DownloadSignaturesOnceCoroutine({0}) succeeded: <color=magenta>{1}</color>";
				Debug.LogFormat(format, new object[]
				{
					context,
					Json.Serialize(d)
				});
			}
			foreach (KeyValuePair<string, string> kv in d)
			{
				this._signatures[kv.Key] = kv.Value;
			}
			yield break;
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0013BB7C File Offset: 0x00139D7C
		private static IEnumerator WaitCompletionAndParseSignaturesCoroutine(Task<string> future, TaskCompletionSource<Dictionary<string, string>> promise)
		{
			try
			{
				yield return new WaitUntil(() => future.IsCompleted);
				if (future.IsCanceled)
				{
					promise.TrySetCanceled();
					yield break;
				}
				if (future.IsFaulted)
				{
					promise.TrySetException(future.Exception);
					yield break;
				}
				Dictionary<string, string> result = new Dictionary<string, string>();
				Dictionary<string, object> signaturesDictionaryObjects = Json.Deserialize(future.Result) as Dictionary<string, object>;
				if (signaturesDictionaryObjects == null)
				{
					promise.TrySetResult(result);
					yield break;
				}
				foreach (KeyValuePair<string, object> kv in signaturesDictionaryObjects)
				{
					string s = kv.Value as string;
					if (!string.IsNullOrEmpty(s))
					{
						result[kv.Key] = s;
					}
				}
				promise.TrySetResult(result);
			}
			finally
			{
			}
			yield break;
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x0013BBAC File Offset: 0x00139DAC
		private static IEnumerator DownloadSignaturesCoroutine(string[] urls, CancellationToken cancellationToken, TaskCompletionSource<string> promise)
		{
			if (urls.Length == 0)
			{
				promise.TrySetResult("{}");
				yield break;
			}
			List<string> names = urls.Select(new Func<string, string>(PersistentCacheManager.GetSegmentsAsString)).ToList<string>();
			string namesSerialized = Json.Serialize(names);
			WWWForm form = new WWWForm();
			form.AddField("app_version", string.Format("{0}:{1}", ProtocolListGetter.CurrentPlatform, GlobalGameController.AppVersion));
			form.AddField("names", namesSerialized);
			WWW request = Tools.CreateWwwIfNotConnected("https://secure.pixelgunserver.com/get_files_info.php", form, "DownloadSignaturesCoroutine()", null);
			if (request == null)
			{
				promise.TrySetCanceled();
				yield break;
			}
			while (!request.isDone)
			{
				yield return null;
				if (cancellationToken.IsCancellationRequested)
				{
					promise.TrySetCanceled();
					yield break;
				}
			}
			try
			{
				if (!string.IsNullOrEmpty(request.error))
				{
					promise.TrySetException(new InvalidOperationException(request.error));
					yield break;
				}
				string response = URLs.Sanitize(request);
				promise.TrySetResult(response);
			}
			finally
			{
				if (Application.isEditor)
				{
					Debug.Log("<color=teal>PersistentCacheManager.DownloadSignaturesCoroutine(): request.Dispose()</color>");
				}
				request.Dispose();
			}
			yield break;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0013BBEC File Offset: 0x00139DEC
		private static string GetStorageKey(string segments)
		{
			return string.Format("Cache:{0}", segments);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0013BBFC File Offset: 0x00139DFC
		private static string GetSegmentsAsString(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			string result;
			try
			{
				Uri uri = new Uri(url);
				string[] segments = uri.Segments;
				if (segments.Length == 0)
				{
					result = string.Empty;
				}
				else
				{
					StringComparer c = StringComparer.OrdinalIgnoreCase;
					string[] values = segments.SkipWhile((string s, int i) => i == 0 && c.Equals(s, "/")).SkipWhile((string s, int i) => i == 0 && c.Equals(s, "pixelgun3d-config/")).ToArray<string>();
					string text = string.Concat(values).TrimStart(new char[]
					{
						'/'
					});
					result = text;
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0013BCD0 File Offset: 0x00139ED0
		private static PersistentCacheManager Create()
		{
			PersistentCacheManager persistentCacheManager = UnityEngine.Object.FindObjectOfType<PersistentCacheManager>();
			if (persistentCacheManager != null)
			{
				UnityEngine.Object.DontDestroyOnLoad(persistentCacheManager.gameObject);
				return persistentCacheManager;
			}
			GameObject gameObject = new GameObject("Rilisoft.PersistentCacheManager");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return gameObject.AddComponent<PersistentCacheManager>();
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0013BD18 File Offset: 0x00139F18
		private EncryptedPlayerPrefs CreateEncryptedPlayerPrefs()
		{
			HiddenSettings hiddenSettings = (!(MiscAppsMenu.Instance != null)) ? null : MiscAppsMenu.Instance.misc;
			byte[] masterKey = null;
			if (hiddenSettings == null)
			{
				Debug.LogError("CreateEncryptedPlayerPrefs(): settings are null.");
				masterKey = new byte[40];
			}
			else
			{
				try
				{
					masterKey = Convert.FromBase64String(hiddenSettings.PersistentCacheManagerKey);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					masterKey = new byte[40];
				}
			}
			return new EncryptedPlayerPrefs(masterKey);
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x0013BDB8 File Offset: 0x00139FB8
		private EncryptedPlayerPrefs EncryptedPlayerPrefs
		{
			get
			{
				return this._encryptedPlayerPrefs.Value;
			}
		}

		// Token: 0x04002CE4 RID: 11492
		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04002CE5 RID: 11493
		private readonly TaskCompletionSource<Dictionary<string, string>> _firstResponsePromise = new TaskCompletionSource<Dictionary<string, string>>();

		// Token: 0x04002CE6 RID: 11494
		private readonly Dictionary<string, string> _signatures = new Dictionary<string, string>();

		// Token: 0x04002CE7 RID: 11495
		private readonly Lazy<EncryptedPlayerPrefs> _encryptedPlayerPrefs;

		// Token: 0x04002CE8 RID: 11496
		private static readonly Lazy<PersistentCacheManager> _instance = new Lazy<PersistentCacheManager>(new Func<PersistentCacheManager>(PersistentCacheManager.Create));

		// Token: 0x020006D2 RID: 1746
		[Serializable]
		private struct CacheEntry
		{
			// Token: 0x04002CE9 RID: 11497
			public string signature;

			// Token: 0x04002CEA RID: 11498
			public string payload;
		}
	}
}
