using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200055C RID: 1372
	internal sealed class AndroidSystem
	{
		// Token: 0x06002FA8 RID: 12200 RVA: 0x000F92F4 File Offset: 0x000F74F4
		private AndroidSystem()
		{
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x000F9364 File Offset: 0x000F7564
		public static AndroidSystem Instance
		{
			get
			{
				return AndroidSystem._instance.Value;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002FAB RID: 12203 RVA: 0x000F9370 File Offset: 0x000F7570
		public AndroidJavaObject CurrentActivity
		{
			get
			{
				AndroidJavaObject result;
				try
				{
					if (this._currentActivity != null && this._currentActivity.IsAlive)
					{
						result = (this._currentActivity.Target as AndroidJavaObject);
					}
					else
					{
						AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
						AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
						if (@static == null)
						{
							this._currentActivity = null;
							result = null;
						}
						else
						{
							this._currentActivity = new WeakReference(@static, false);
							result = @static;
						}
					}
				}
				catch (Exception exception)
				{
					Debug.LogWarning("Exception occured while getting Android current activity. See next log entry for details.");
					Debug.LogException(exception);
					result = null;
				}
				return result;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002FAC RID: 12204 RVA: 0x000F942C File Offset: 0x000F762C
		public long FirstInstallTime
		{
			get
			{
				return this._firstInstallTime.Value;
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002FAD RID: 12205 RVA: 0x000F943C File Offset: 0x000F763C
		public string PackageName
		{
			get
			{
				return this._packageName.Value;
			}
		}

		// Token: 0x06002FAE RID: 12206 RVA: 0x000F944C File Offset: 0x000F764C
		public byte[] GetSignatureHash()
		{
			Lazy<byte[]> lazy = new Lazy<byte[]>(() => new byte[20]);
			if (Application.platform != RuntimePlatform.Android)
			{
				return lazy.Value;
			}
			AndroidJavaObject androidJavaObject = this.CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				this.PackageName,
				64
			});
			if (androidJavaObject2 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			AndroidJavaObject[] array = androidJavaObject2.Get<AndroidJavaObject[]>("signatures");
			if (array == null)
			{
				throw new InvalidOperationException("signatures() == null");
			}
			byte[] result;
			using (SHA1Managed sha1Managed = new SHA1Managed())
			{
				IEnumerable<byte[]> source = (from s in array
				select s.Call<byte[]>("toByteArray", new object[0]) into s
				where s != null
				select s).Select(new Func<byte[], byte[]>(sha1Managed.ComputeHash));
				byte[] array2 = source.FirstOrDefault<byte[]>() ?? lazy.Value;
				result = array2;
			}
			return result;
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x000F95C0 File Offset: 0x000F77C0
		public string GetAdvertisingId()
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return string.Empty;
			}
			string result;
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient");
				AndroidJavaObject currentActivity = this.CurrentActivity;
				if (currentActivity == null)
				{
					Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "currentActivity"));
					result = string.Empty;
				}
				else
				{
					AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", new object[]
					{
						currentActivity
					});
					if (androidJavaObject == null)
					{
						Debug.LogWarning(string.Format("Failed to get Android advertising id: {0} == null", "adInfo"));
						result = string.Empty;
					}
					else
					{
						string text = androidJavaObject.Call<string>("getId", new object[0]);
						result = (text ?? string.Empty);
					}
				}
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Exception occured while getting Android advertising id. See next log entry for details.");
				Debug.LogException(exception);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06002FB0 RID: 12208 RVA: 0x000F96C4 File Offset: 0x000F78C4
		private static long GetFirstInstallTime()
		{
			if (Application.isEditor)
			{
				return 0L;
			}
			AndroidJavaObject androidJavaObject = AndroidSystem.Instance.CurrentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				throw new InvalidOperationException("getPackageManager() == null");
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				AndroidSystem.Instance.PackageName,
				0
			});
			if (androidJavaObject2 == null)
			{
				throw new InvalidOperationException("getPackageInfo() == null");
			}
			return androidJavaObject2.Get<long>("firstInstallTime");
		}

		// Token: 0x06002FB1 RID: 12209 RVA: 0x000F9750 File Offset: 0x000F7950
		internal static int GetSdkVersion()
		{
			if (Application.isEditor)
			{
				return 0;
			}
			IntPtr clazz = AndroidJNI.FindClass("android/os/Build$VERSION");
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
			return AndroidJNI.GetStaticIntField(clazz, staticFieldID);
		}

		// Token: 0x06002FB2 RID: 12210 RVA: 0x000F9790 File Offset: 0x000F7990
		private static string GetPackageName()
		{
			if (Application.isEditor)
			{
				return string.Empty;
			}
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			return currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty;
		}

		// Token: 0x0400230D RID: 8973
		private WeakReference _currentActivity;

		// Token: 0x0400230E RID: 8974
		private static readonly Lazy<AndroidSystem> _instance = new Lazy<AndroidSystem>(() => new AndroidSystem());

		// Token: 0x0400230F RID: 8975
		private readonly Lazy<long> _firstInstallTime = new Lazy<long>(new Func<long>(AndroidSystem.GetFirstInstallTime));

		// Token: 0x04002310 RID: 8976
		private readonly Lazy<string> _packageName = new Lazy<string>(new Func<string>(AndroidSystem.GetPackageName));
	}
}
