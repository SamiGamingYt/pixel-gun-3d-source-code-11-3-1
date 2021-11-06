using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000097 RID: 151
	internal sealed class DigestStorager
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00024A80 File Offset: 0x00022C80
		public DigestStorager()
		{
			byte[] key = new byte[]
			{
				62,
				59,
				146,
				50,
				196,
				43,
				151,
				12,
				34,
				157,
				74,
				34,
				25,
				226,
				239,
				167,
				46,
				226,
				151,
				253,
				149,
				85,
				40,
				56,
				107,
				254,
				198,
				111,
				152,
				34,
				73,
				206,
				184,
				145,
				51,
				23,
				161,
				197,
				53,
				9,
				59,
				16,
				106,
				151,
				54,
				115,
				158,
				48,
				176,
				147,
				174,
				119,
				233,
				88,
				253,
				94,
				20,
				2,
				164,
				67,
				205,
				142,
				150,
				2
			};
			this._hmac = new HMACSHA1(key, true);
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x00024AE0 File Offset: 0x00022CE0
		public static DigestStorager Instance
		{
			get
			{
				return DigestStorager._instance.Value;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00024AEC File Offset: 0x00022CEC
		public void Clear()
		{
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00024AF0 File Offset: 0x00022CF0
		public bool ContainsKey(string key)
		{
			string backingStoreKey = this.GetBackingStoreKey(key);
			return (!this._useCryptoPlayerPrefs) ? PlayerPrefs.HasKey(backingStoreKey) : CryptoPlayerPrefsFacade.HasKey(backingStoreKey);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00024B24 File Offset: 0x00022D24
		public void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			string backingStoreKey = this.GetBackingStoreKey(key);
			if (this._useCryptoPlayerPrefs)
			{
				CryptoPlayerPrefsFacade.DeleteKey(backingStoreKey);
			}
			else
			{
				PlayerPrefs.DeleteKey(backingStoreKey);
			}
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00024B68 File Offset: 0x00022D68
		public void Save()
		{
			if (this._useCryptoPlayerPrefs)
			{
				CryptoPlayerPrefsFacade.Save();
			}
			else
			{
				PlayerPrefs.Save();
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00024B84 File Offset: 0x00022D84
		public void Set(string key, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			this.SetCore(key, bytes);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00024BA0 File Offset: 0x00022DA0
		public void Set(string key, string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
			this.SetCore(key, bytes);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00024BD0 File Offset: 0x00022DD0
		public void Set(string key, byte[] value)
		{
			byte[] valueBytes = value ?? new byte[0];
			this.SetCore(key, valueBytes);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00024BF4 File Offset: 0x00022DF4
		public bool Verify(string key, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			return this.VerifyCore(key, bytes);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00024C10 File Offset: 0x00022E10
		public bool Verify(string key, string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value ?? string.Empty);
			return this.VerifyCore(key, bytes);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00024C40 File Offset: 0x00022E40
		public bool Verify(string key, byte[] value)
		{
			byte[] valueBytes = value ?? new byte[0];
			return this.VerifyCore(key, valueBytes);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00024C64 File Offset: 0x00022E64
		public bool VerifyCore(string key, byte[] valueBytes)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!this.ContainsKey(key))
			{
				return false;
			}
			byte[] second = this.ComputeHash(key, valueBytes);
			string backingStoreKey = this.GetBackingStoreKey(key);
			string s = (!this._useCryptoPlayerPrefs) ? PlayerPrefs.GetString(backingStoreKey) : CryptoPlayerPrefsFacade.GetString(backingStoreKey);
			byte[] first = Convert.FromBase64String(s);
			return first.SequenceEqual(second);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00024CCC File Offset: 0x00022ECC
		private byte[] ComputeHash(string key, byte[] valueBytes)
		{
			byte[] bytes = BitConverter.GetBytes(key.GetHashCode());
			byte[] array = new byte[bytes.Length + valueBytes.Length];
			bytes.CopyTo(array, 0);
			valueBytes.CopyTo(array, bytes.Length);
			return this._hmac.ComputeHash(array);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00024D14 File Offset: 0x00022F14
		private void SetCore(string key, byte[] valueBytes)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			byte[] inArray = this.ComputeHash(key, valueBytes);
			string text = Convert.ToBase64String(inArray);
			string backingStoreKey = this.GetBackingStoreKey(key);
			if (this._useCryptoPlayerPrefs)
			{
				CryptoPlayerPrefsFacade.SetString(backingStoreKey, text);
			}
			else
			{
				PlayerPrefs.SetString(backingStoreKey, text);
			}
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00024D68 File Offset: 0x00022F68
		public string GetBackingStoreKey(string key)
		{
			string str = string.Format("Digest-9.4.1-{0:d}.", BuildSettings.BuildTargetPlatform);
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (Application.isEditor)
			{
				return str + key;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			string str2 = Convert.ToBase64String(bytes);
			return str + str2;
		}

		// Token: 0x040004D6 RID: 1238
		private readonly bool _useCryptoPlayerPrefs;

		// Token: 0x040004D7 RID: 1239
		private readonly HashAlgorithm _hmac;

		// Token: 0x040004D8 RID: 1240
		private static readonly Lazy<DigestStorager> _instance = new Lazy<DigestStorager>(() => new DigestStorager());
	}
}
