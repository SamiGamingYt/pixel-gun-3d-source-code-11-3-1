using System;
using System.IO;
using System.Security.Cryptography;

namespace Rilisoft
{
	// Token: 0x020005D2 RID: 1490
	internal sealed class AesFacade
	{
		// Token: 0x0600333D RID: 13117 RVA: 0x00109244 File Offset: 0x00107444
		public AesFacade(byte[] masterKey)
		{
			if (masterKey == null)
			{
				throw new ArgumentNullException("masterKey");
			}
			byte[] salt = new byte[]
			{
				76,
				130,
				161,
				24,
				36,
				100,
				21,
				150
			};
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(masterKey, salt, 1);
			byte[] bytes = rfc2898DeriveBytes.GetBytes(48);
			byte[] array = new byte[32];
			Array.Copy(bytes, 0, array, 0, array.Length);
			byte[] array2 = new byte[16];
			Array.Copy(bytes, array.Length, array2, 0, array2.Length);
			this._aes = Rijndael.Create();
			this._aes.KeySize = 256;
			this._aes.BlockSize = 128;
			this._aes.Key = array;
			this._aes.IV = array2;
			this._aes.Mode = CipherMode.CFB;
			this._aes.Padding = PaddingMode.PKCS7;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00109320 File Offset: 0x00107520
		public byte[] Decrypt(byte[] ciphertext)
		{
			if (ciphertext == null)
			{
				throw new ArgumentNullException("ciphertext");
			}
			byte[] result;
			using (ICryptoTransform cryptoTransform = this._aes.CreateDecryptor())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(ciphertext, 0, ciphertext.Length);
						cryptoStream.Close();
						byte[] array = memoryStream.ToArray();
						result = array;
					}
				}
			}
			return result;
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00109400 File Offset: 0x00107600
		public byte[] Encrypt(byte[] plaintext)
		{
			if (plaintext == null)
			{
				throw new ArgumentNullException("plaintext");
			}
			byte[] result;
			using (ICryptoTransform cryptoTransform = this._aes.CreateEncryptor())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
					{
						cryptoStream.Write(plaintext, 0, plaintext.Length);
						cryptoStream.Close();
						byte[] array = memoryStream.ToArray();
						result = array;
					}
				}
			}
			return result;
		}

		// Token: 0x040025AE RID: 9646
		private readonly SymmetricAlgorithm _aes;
	}
}
