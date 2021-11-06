using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005D3 RID: 1491
	internal class CryptoPlayerPrefsFacade
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x001094E8 File Offset: 0x001076E8
		internal static EncryptedPlayerPrefs EncryptedPlayerPrefs
		{
			get
			{
				if (CryptoPlayerPrefsFacade._encryptedPlayerPrefs == null)
				{
					HiddenSettings hiddenSettings = (!(MiscAppsMenu.Instance != null)) ? null : MiscAppsMenu.Instance.misc;
					byte[] array = null;
					if (hiddenSettings == null)
					{
						Debug.LogError("Settings are null.");
						array = new byte[40];
					}
					else
					{
						try
						{
							array = Convert.FromBase64String(hiddenSettings.PersistentCacheManagerKey);
						}
						catch (Exception exception)
						{
							Debug.LogException(exception);
							array = new byte[40];
						}
					}
					try
					{
						byte[] signatureHash = AndroidSystem.Instance.GetSignatureHash();
						int num = Math.Min(signatureHash.Length, array.Length);
						for (int num2 = 0; num2 != num; num2++)
						{
							array[num2] ^= signatureHash[num2];
						}
					}
					catch (Exception exception2)
					{
						Debug.LogException(exception2);
					}
					CryptoPlayerPrefsFacade._encryptedPlayerPrefs = new EncryptedPlayerPrefs(array);
				}
				return CryptoPlayerPrefsFacade._encryptedPlayerPrefs;
			}
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x00109604 File Offset: 0x00107804
		public static void DeleteKey(string key)
		{
			CryptoPlayerPrefs.DeleteKey(key);
			CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.DeleteKey(key);
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00109618 File Offset: 0x00107818
		public static int GetInt(string key)
		{
			if (!CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.HasKey(key))
			{
				int @int = CryptoPlayerPrefs.GetInt(key, 0);
				string value = @int.ToString(CultureInfo.InvariantCulture);
				CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.SetString(key, value);
				return @int;
			}
			string @string = CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.GetString(key);
			int result;
			if (int.TryParse(@string, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0010967C File Offset: 0x0010787C
		public static string GetString(string key)
		{
			if (CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.HasKey(key))
			{
				return CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.GetString(key);
			}
			string @string = CryptoPlayerPrefs.GetString(key, string.Empty);
			CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.SetString(key, @string);
			return @string;
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x001096C0 File Offset: 0x001078C0
		public static bool HasKey(string key)
		{
			return CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.HasKey(key) || CryptoPlayerPrefs.HasKey(key);
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x001096E8 File Offset: 0x001078E8
		public static void SetInt(string key, int val)
		{
			string value = val.ToString(CultureInfo.InvariantCulture);
			CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.SetString(key, value);
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00109710 File Offset: 0x00107910
		public static void SetString(string key, string val)
		{
			CryptoPlayerPrefsFacade.EncryptedPlayerPrefs.SetString(key, val);
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00109720 File Offset: 0x00107920
		public static void Save()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x040025AF RID: 9647
		private static EncryptedPlayerPrefs _encryptedPlayerPrefs;
	}
}
