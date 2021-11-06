using System;

namespace Rilisoft
{
	// Token: 0x020005D8 RID: 1496
	internal interface IAead
	{
		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x06003364 RID: 13156
		int MaxOverhead { get; }

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x06003365 RID: 13157
		int MinOverhead { get; }

		// Token: 0x06003366 RID: 13158
		bool Authenticate(ArraySegment<byte> taggedCiphertext, byte[] salt);

		// Token: 0x06003367 RID: 13159
		ArraySegment<byte> Decrypt(ArraySegment<byte> taggedCiphertext, byte[] salt, byte[] outputBuffer);

		// Token: 0x06003368 RID: 13160
		ArraySegment<byte> Encrypt(ArraySegment<byte> plaintext, byte[] salt, byte[] outputBuffer);
	}
}
