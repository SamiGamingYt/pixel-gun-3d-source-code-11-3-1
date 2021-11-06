using System;
using System.Security.Cryptography;
using System.Text;

namespace Rilisoft
{
	// Token: 0x02000701 RID: 1793
	public sealed class PlayerPrefsHelper : IDisposable
	{
		// Token: 0x06003E3E RID: 15934 RVA: 0x0014DCC0 File Offset: 0x0014BEC0
		internal PlayerPrefsHelper()
		{
			using (HashAlgorithm hashAlgorithm = new SHA256Managed())
			{
				byte[] bytes = Encoding.UTF8.GetBytes("PrefsKey");
				byte[] value = hashAlgorithm.ComputeHash(bytes);
				this._hmacPrefsKey = BitConverter.ToString(value).Replace("-", string.Empty);
				this._hmacPrefsKey = this._hmacPrefsKey.Substring(0, Math.Min(32, this._hmacPrefsKey.Length)).ToLower();
				byte[] bytes2 = Encoding.UTF8.GetBytes("HmacKey");
				byte[] key = hashAlgorithm.ComputeHash(bytes2);
				this._hmac = new HMACSHA256(key);
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0014DD8C File Offset: 0x0014BF8C
		public bool Verify()
		{
			return true;
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0014DD9C File Offset: 0x0014BF9C
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._hmac.Clear();
			this._disposed = true;
		}

		// Token: 0x04002E05 RID: 11781
		private bool _disposed;

		// Token: 0x04002E06 RID: 11782
		private readonly HMAC _hmac;

		// Token: 0x04002E07 RID: 11783
		private readonly string _hmacPrefsKey;
	}
}
