using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Rilisoft
{
	// Token: 0x020005D4 RID: 1492
	internal sealed class DefaultAead : IAead
	{
		// Token: 0x06003349 RID: 13129 RVA: 0x00109728 File Offset: 0x00107928
		public DefaultAead(byte[] masterKey)
		{
			if (masterKey == null)
			{
				throw new ArgumentNullException("masterKey");
			}
			this._aes = Rijndael.Create();
			this._aes.KeySize = 256;
			this._aes.BlockSize = 128;
			this._aes.Mode = CipherMode.CFB;
			this._aes.Padding = PaddingMode.PKCS7;
			int num = Math.Max(32, 8);
			if (masterKey.Length < num)
			{
				throw new ArgumentException("Master key should be at least " + num + " bytes.", "masterKey");
			}
			this._aesKeyBuffer = new byte[32];
			this._aesIVBuffer = new byte[this.AesIVLength];
			this._aesKeyTweak = new byte[32];
			Array.Copy(masterKey, 0, this._aesKeyTweak, 0, this._aesKeyTweak.Length);
			this._pbkdfSalt = new byte[8];
			Array.Copy(masterKey, masterKey.Length - 8, this._pbkdfSalt, 0, this._pbkdfSalt.Length);
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x0600334B RID: 13131 RVA: 0x00109844 File Offset: 0x00107A44
		public int MaxOverhead
		{
			get
			{
				return this.NonceLength + 20 + 16;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x00109854 File Offset: 0x00107A54
		public int MinOverhead
		{
			get
			{
				return this.NonceLength + 20;
			}
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00109860 File Offset: 0x00107A60
		public bool Authenticate(ArraySegment<byte> ciphertext, byte[] salt)
		{
			if (ciphertext.Count < this.NonceLength + 20)
			{
				return false;
			}
			byte[] macKey = this.GenerateMacKey(salt);
			return this.AuthenticateCore(ciphertext, macKey);
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00109898 File Offset: 0x00107A98
		private bool AuthenticateCore(ArraySegment<byte> ciphertext, byte[] macKey)
		{
			ArraySegment<byte> arraySegment = new ArraySegment<byte>(ciphertext.Array, ciphertext.Count - 20, 20);
			byte[] array = null;
			using (HMACSHA1 hmacsha = new HMACSHA1(macKey, true))
			{
				array = hmacsha.ComputeHash(ciphertext.Array, 0, ciphertext.Count - 20);
			}
			return DefaultAead.ConstantTimeEqual(arraySegment.Array, arraySegment.Offset, array, 0, array.Length);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x0010992C File Offset: 0x00107B2C
		public ArraySegment<byte> Decrypt(ArraySegment<byte> taggedCiphertext, byte[] salt, byte[] outputBuffer)
		{
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (outputBuffer.Length < taggedCiphertext.Count - this.MinOverhead)
			{
				throw new ArgumentException("Output buffer is too short: " + outputBuffer.Length, "outputBuffer");
			}
			byte[] macKey = this.GenerateMacKey(salt);
			if (!this.AuthenticateCore(taggedCiphertext, macKey))
			{
				return default(ArraySegment<byte>);
			}
			ArraySegment<byte> result;
			try
			{
				ArraySegment<byte> arraySegment = new ArraySegment<byte>(taggedCiphertext.Array, 0, 32);
				ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(taggedCiphertext.Array, arraySegment.Count, this.AesIVLength);
				ArraySegment<byte> arraySegment3 = new ArraySegment<byte>(taggedCiphertext.Array, this.NonceLength, taggedCiphertext.Count - this.NonceLength - 20);
				for (int num = 0; num != this._aesKeyBuffer.Length; num++)
				{
					this._aesKeyBuffer[num] = (this._aesKeyTweak[num] ^ arraySegment.Array[arraySegment.Offset + num]);
				}
				Array.Copy(arraySegment2.Array, arraySegment2.Offset, this._aesIVBuffer, 0, this._aesIVBuffer.Length);
				using (ICryptoTransform cryptoTransform = this._aes.CreateDecryptor(this._aesKeyBuffer, this._aesIVBuffer))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(arraySegment3.Array, arraySegment3.Offset, arraySegment3.Count);
							cryptoStream.Close();
							byte[] array = memoryStream.ToArray();
							ArraySegment<byte> arraySegment4 = new ArraySegment<byte>(outputBuffer, 0, array.Length);
							Array.Copy(array, 0, arraySegment4.Array, arraySegment4.Offset, arraySegment4.Count);
							result = arraySegment4;
						}
					}
				}
			}
			finally
			{
				Array.Clear(this._aesKeyBuffer, 0, this._aesKeyBuffer.Length);
				Array.Clear(this._aesIVBuffer, 0, this._aesIVBuffer.Length);
			}
			return result;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00109BB0 File Offset: 0x00107DB0
		public ArraySegment<byte> Encrypt(ArraySegment<byte> plaintext, byte[] salt, byte[] outputBuffer)
		{
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (outputBuffer.Length < plaintext.Count + this.MaxOverhead)
			{
				throw new ArgumentException("Output buffer is too short: " + outputBuffer.Length, "outputBuffer");
			}
			ArraySegment<byte> result;
			try
			{
				this._prng.GetBytes(this._aesKeyBuffer);
				this._prng.GetBytes(this._aesIVBuffer);
				ArraySegment<byte> arraySegment = new ArraySegment<byte>(outputBuffer, 0, 32);
				ArraySegment<byte> arraySegment2 = new ArraySegment<byte>(outputBuffer, arraySegment.Count, this.AesIVLength);
				Array.Copy(this._aesKeyBuffer, 0, arraySegment.Array, arraySegment.Offset, arraySegment.Count);
				Array.Copy(this._aesIVBuffer, 0, arraySegment2.Array, arraySegment2.Offset, arraySegment2.Count);
				for (int num = 0; num != this._aesKeyBuffer.Length; num++)
				{
					this._aesKeyBuffer[num] = (this._aesKeyTweak[num] ^ this._aesKeyBuffer[num]);
				}
				using (ICryptoTransform cryptoTransform = this._aes.CreateEncryptor(this._aesKeyBuffer, this._aesIVBuffer))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
						{
							cryptoStream.Write(plaintext.Array, plaintext.Offset, plaintext.Count);
							cryptoStream.Close();
							byte[] array = memoryStream.ToArray();
							ArraySegment<byte> arraySegment3 = new ArraySegment<byte>(outputBuffer, this.NonceLength, array.Length);
							Array.Copy(array, 0, arraySegment3.Array, arraySegment3.Offset, arraySegment3.Count);
							ArraySegment<byte> arraySegment4 = new ArraySegment<byte>(outputBuffer, 0, this.NonceLength + arraySegment3.Count + 20);
							byte[] key = this.GenerateMacKey(salt);
							using (HMACSHA1 hmacsha = new HMACSHA1(key, true))
							{
								ArraySegment<byte> arraySegment5 = new ArraySegment<byte>(outputBuffer, arraySegment4.Count - 20, 20);
								byte[] sourceArray = hmacsha.ComputeHash(arraySegment4.Array, 0, this.NonceLength + arraySegment3.Count);
								Array.Copy(sourceArray, 0, arraySegment5.Array, arraySegment5.Offset, arraySegment5.Count);
							}
							result = arraySegment4;
						}
					}
				}
			}
			finally
			{
				Array.Clear(this._aesKeyBuffer, 0, this._aesKeyBuffer.Length);
				Array.Clear(this._aesIVBuffer, 0, this._aesIVBuffer.Length);
			}
			return result;
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00109EC0 File Offset: 0x001080C0
		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool ConstantTimeEqual(byte[] left, int leftOffset, byte[] right, int rightOffset, int length)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}
			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			if (leftOffset < 0)
			{
				throw new ArgumentOutOfRangeException("leftOffset", "leftOffset < 0");
			}
			if (rightOffset < 0)
			{
				throw new ArgumentOutOfRangeException("rightOffset", "rightOffset < 0");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "length < 0");
			}
			if (leftOffset + length > left.Length)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "leftOffset + length > left.Length: {0} + {1} > {2}", new object[]
				{
					leftOffset,
					length,
					left.Length
				}));
			}
			if (rightOffset + length > right.Length)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "rightOffset + length > right.Length: {0} + {1} > {2}", new object[]
				{
					rightOffset,
					length,
					right.Length
				}));
			}
			int num = 0;
			for (int num2 = 0; num2 != length; num2++)
			{
				num |= (int)(left[leftOffset + num2] ^ right[rightOffset + num2]);
			}
			return num == 0;
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00109FE8 File Offset: 0x001081E8
		private byte[] GenerateMacKey(byte[] salt)
		{
			byte[] password = salt ?? DefaultAead.s_emptyByteArray;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, this._pbkdfSalt, 1);
			return rfc2898DeriveBytes.GetBytes(64);
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06003353 RID: 13139 RVA: 0x0010A01C File Offset: 0x0010821C
		private int AesIVLength
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x0010A020 File Offset: 0x00108220
		private int NonceLength
		{
			get
			{
				return 32 + this.AesIVLength;
			}
		}

		// Token: 0x040025B0 RID: 9648
		private const int AesBlockLength = 16;

		// Token: 0x040025B1 RID: 9649
		private const int AesKeyLength = 32;

		// Token: 0x040025B2 RID: 9650
		private const int TagLength = 20;

		// Token: 0x040025B3 RID: 9651
		private static readonly byte[] s_emptyByteArray = new byte[0];

		// Token: 0x040025B4 RID: 9652
		private readonly byte[] _aesKeyTweak;

		// Token: 0x040025B5 RID: 9653
		private readonly byte[] _pbkdfSalt;

		// Token: 0x040025B6 RID: 9654
		private readonly byte[] _aesKeyBuffer;

		// Token: 0x040025B7 RID: 9655
		private readonly byte[] _aesIVBuffer;

		// Token: 0x040025B8 RID: 9656
		private readonly SymmetricAlgorithm _aes;

		// Token: 0x040025B9 RID: 9657
		private readonly DefaultAead.CustomRandomNumberGenerator _prng = new DefaultAead.CustomRandomNumberGenerator();

		// Token: 0x020005D5 RID: 1493
		private sealed class CustomRandomNumberGenerator
		{
			// Token: 0x06003356 RID: 13142 RVA: 0x0010A040 File Offset: 0x00108240
			public void GetBytes(byte[] data)
			{
				if (data == null)
				{
					return;
				}
				if (data.Length == 0)
				{
					return;
				}
				this._prng.GetBytes(data);
			}

			// Token: 0x040025BA RID: 9658
			private readonly RNGCryptoServiceProvider _prng = new RNGCryptoServiceProvider();
		}
	}
}
