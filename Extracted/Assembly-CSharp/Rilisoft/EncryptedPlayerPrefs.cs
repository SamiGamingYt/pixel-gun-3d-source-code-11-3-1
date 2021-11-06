using System;
using System.Collections.Generic;
using System.Text;
using Rilisoft.Details;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005D6 RID: 1494
	internal sealed class EncryptedPlayerPrefs
	{
		// Token: 0x06003357 RID: 13143 RVA: 0x0010A060 File Offset: 0x00108260
		public EncryptedPlayerPrefs(byte[] masterKey)
		{
			if (masterKey == null)
			{
				throw new ArgumentNullException("masterKey");
			}
			this._aesFacade = new AesFacade(masterKey);
			this._aead = new DefaultAead(masterKey);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x0010A0BC File Offset: 0x001082BC
		public void ClearChache()
		{
			this._bufferWeakReference = null;
			this._wrappedKeys.Clear();
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x0010A0D0 File Offset: 0x001082D0
		public void DeleteKey(string key)
		{
			if (key == null)
			{
				return;
			}
			try
			{
				string key2 = this.WrapKey(key);
				PlayerPrefs.DeleteKey(key2);
			}
			catch (Exception message)
			{
				Debug.LogWarningFormat("`DeleteKey()` failed with key: `{0}`", new object[]
				{
					key
				});
				Debug.LogWarning(message);
			}
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x0010A134 File Offset: 0x00108334
		public bool HasKey(string key)
		{
			if (key == null)
			{
				return false;
			}
			bool result;
			try
			{
				string key2 = this.WrapKey(key);
				result = PlayerPrefs.HasKey(key2);
			}
			catch (Exception message)
			{
				Debug.LogWarningFormat("`HasKey()` failed with key: `{0}`", new object[]
				{
					key
				});
				Debug.LogWarning(message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x0010A1A8 File Offset: 0x001083A8
		public string GetString(string key)
		{
			if (key == null)
			{
				return string.Empty;
			}
			byte[] bytes = EncryptedPlayerPrefs.s_defaultEncoding.GetBytes(key);
			string result;
			try
			{
				string key2 = this.WrapKey(bytes);
				string @string = PlayerPrefs.GetString(key2);
				if (string.IsNullOrEmpty(@string))
				{
					result = string.Empty;
				}
				else
				{
					byte[] array = Convert.FromBase64String(@string);
					if (array.Length - this._aead.MinOverhead < 0)
					{
						result = string.Empty;
					}
					else
					{
						byte[] buffer = this.GetBuffer(array.Length - this._aead.MinOverhead);
						try
						{
							ArraySegment<byte> arraySegment = this._aead.Decrypt(new ArraySegment<byte>(array), bytes, buffer);
							if (arraySegment.Array == null)
							{
								result = string.Empty;
							}
							else
							{
								string string2 = EncryptedPlayerPrefs.s_defaultEncoding.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
								result = string2;
							}
						}
						finally
						{
							Array.Clear(buffer, 0, buffer.Length);
						}
					}
				}
			}
			catch (Exception message)
			{
				Debug.LogWarningFormat("`GetString()` failed with key: `{0}`", new object[]
				{
					key
				});
				Debug.LogWarning(message);
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x0010A308 File Offset: 0x00108508
		public void SetString(string key, string value)
		{
			if (key == null)
			{
				return;
			}
			byte[] bytes = EncryptedPlayerPrefs.s_defaultEncoding.GetBytes(key);
			try
			{
				string key2 = this.WrapKey(bytes);
				byte[] bytes2 = EncryptedPlayerPrefs.s_defaultEncoding.GetBytes(value);
				byte[] buffer = this.GetBuffer(bytes2.Length + this._aead.MaxOverhead);
				try
				{
					ArraySegment<byte> arraySegment = this._aead.Encrypt(new ArraySegment<byte>(bytes2), bytes, buffer);
					string value2 = Convert.ToBase64String(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
					PlayerPrefs.SetString(key2, value2);
				}
				finally
				{
					Array.Clear(buffer, 0, buffer.Length);
				}
			}
			catch (Exception message)
			{
				Debug.LogWarningFormat("`SetString()` failed with key: `{0}`", new object[]
				{
					key
				});
				Debug.LogWarning(message);
			}
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x0010A3F8 File Offset: 0x001085F8
		private string WrapKey(string key)
		{
			byte[] bytes = EncryptedPlayerPrefs.s_defaultEncoding.GetBytes(key);
			return this.WrapKey(bytes);
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x0010A41C File Offset: 0x0010861C
		private string WrapKey(byte[] keyBytes)
		{
			string result;
			if (this._wrappedKeys.TryGetValue(keyBytes, out result))
			{
				return result;
			}
			byte[] buffer = this.GetBuffer(keyBytes.Length + this._aead.MaxOverhead);
			string result2;
			try
			{
				byte[] inArray = this._aesFacade.Encrypt(keyBytes);
				string text = "AEAD:" + Convert.ToBase64String(inArray);
				this._wrappedKeys[keyBytes] = text;
				result2 = text;
			}
			catch (Exception message)
			{
				string text2 = Convert.ToBase64String(keyBytes);
				Debug.LogWarningFormat("`WrapKey()` failed with key bytes: `{0}`", new object[]
				{
					text2
				});
				Debug.LogWarning(message);
				result2 = text2;
			}
			finally
			{
				Array.Clear(buffer, 0, buffer.Length);
			}
			return result2;
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x0010A504 File Offset: 0x00108704
		private byte[] GetBuffer(int length)
		{
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", length, "Must be non-negative.");
			}
			if (this._bufferWeakReference != null && this._bufferWeakReference.IsAlive)
			{
				byte[] array = (byte[])this._bufferWeakReference.Target;
				if (array.Length >= length)
				{
					return array;
				}
			}
			byte[] array2 = new byte[length];
			this._bufferWeakReference = new WeakReference(array2, false);
			return array2;
		}

		// Token: 0x040025BB RID: 9659
		private static readonly UTF8Encoding s_defaultEncoding = new UTF8Encoding(false, false);

		// Token: 0x040025BC RID: 9660
		private readonly Dictionary<byte[], string> _wrappedKeys = new Dictionary<byte[], string>(new ByteArrayComparer());

		// Token: 0x040025BD RID: 9661
		private WeakReference _bufferWeakReference;

		// Token: 0x040025BE RID: 9662
		private readonly AesFacade _aesFacade;

		// Token: 0x040025BF RID: 9663
		private readonly DefaultAead _aead;
	}
}
