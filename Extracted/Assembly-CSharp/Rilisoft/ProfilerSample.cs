using System;

namespace Rilisoft
{
	// Token: 0x0200070F RID: 1807
	internal struct ProfilerSample : IDisposable, IEquatable<ProfilerSample>
	{
		// Token: 0x06003F1D RID: 16157 RVA: 0x00151C58 File Offset: 0x0014FE58
		public ProfilerSample(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._disposed = false;
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x00151C74 File Offset: 0x0014FE74
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x00151C8C File Offset: 0x0014FE8C
		public bool Equals(ProfilerSample other)
		{
			return true;
		}

		// Token: 0x04002E83 RID: 11907
		private bool _disposed;
	}
}
