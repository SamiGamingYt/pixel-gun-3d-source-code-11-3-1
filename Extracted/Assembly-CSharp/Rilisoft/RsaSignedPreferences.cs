using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rilisoft
{
	// Token: 0x020004D7 RID: 1239
	internal sealed class RsaSignedPreferences : SignedPreferences
	{
		// Token: 0x06002C44 RID: 11332 RVA: 0x000EB01C File Offset: 0x000E921C
		public RsaSignedPreferences(Preferences backPreferences, RSACryptoServiceProvider rsaCsp, string salt) : base(backPreferences)
		{
			this._signingRsaCsp = rsaCsp;
			this._verificationRsaCsp = new RSACryptoServiceProvider();
			this._verificationRsaCsp.ImportParameters(rsaCsp.ExportParameters(false));
			this._salt = salt;
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x000EB07C File Offset: 0x000E927C
		protected override void AddSignedCore(string key, string value)
		{
			if (key.StartsWith("com.Rilisoft"))
			{
				throw new ArgumentException("Key starts with reserved prefix.", "key");
			}
			base.BackPreferences.Add(key, value);
			base.BackPreferences.Add(this.GetSignatureKey(key), this.Sign(key, value));
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000EB0D0 File Offset: 0x000E92D0
		protected override bool RemoveSignedCore(string key)
		{
			if (key.StartsWith("com.Rilisoft"))
			{
				throw new ArgumentException("Key starts with reserved prefix.", "key");
			}
			base.BackPreferences.Remove(this.GetSignatureKey(key));
			return base.BackPreferences.Remove(key);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000EB11C File Offset: 0x000E931C
		protected override bool VerifyCore(string key)
		{
			string arg;
			if (!this.TryGetValueCore(key, out arg))
			{
				throw new KeyNotFoundException(string.Format("The given key was not present in the dictionary: {0}", key));
			}
			string s = string.Format("{0}__{1}__{2}", this._salt, key, arg);
			string s2;
			if (!this.TryGetValueCore(this.GetSignatureKey(key), out s2))
			{
				return false;
			}
			bool result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				byte[] signature = Convert.FromBase64String(s2);
				result = this._verificationRsaCsp.VerifyData(bytes, this._hashAlgorithm, signature);
			}
			catch (FormatException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000EB1D4 File Offset: 0x000E93D4
		private string GetSignatureKey(string key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(this._salt);
			byte[] bytes2 = Encoding.UTF8.GetBytes(key);
			byte[] buffer = bytes.Concat(this._prefixBytes).Concat(bytes2).ToArray<byte>();
			byte[] inArray = this._hashAlgorithm.ComputeHash(buffer);
			return string.Format("{0}_{1}", "com.Rilisoft", Convert.ToBase64String(inArray));
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000EB238 File Offset: 0x000E9438
		private string Sign(string key, string value)
		{
			string s = string.Format("{0}__{1}__{2}", this._salt, key, value);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] inArray = this._signingRsaCsp.SignData(bytes, this._hashAlgorithm);
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x0400214D RID: 8525
		private const string SaltKeyValueFormat = "{0}__{1}__{2}";

		// Token: 0x0400214E RID: 8526
		private const string Prefix = "com.Rilisoft";

		// Token: 0x0400214F RID: 8527
		private readonly HashAlgorithm _hashAlgorithm = new SHA1CryptoServiceProvider();

		// Token: 0x04002150 RID: 8528
		private readonly RSACryptoServiceProvider _signingRsaCsp;

		// Token: 0x04002151 RID: 8529
		private readonly RSACryptoServiceProvider _verificationRsaCsp;

		// Token: 0x04002152 RID: 8530
		private readonly string _salt;

		// Token: 0x04002153 RID: 8531
		private readonly byte[] _prefixBytes = Encoding.UTF8.GetBytes("com.Rilisoft");
	}
}
