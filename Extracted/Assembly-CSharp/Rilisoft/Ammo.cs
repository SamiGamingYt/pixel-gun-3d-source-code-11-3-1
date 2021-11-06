using System;

namespace Rilisoft
{
	// Token: 0x0200054A RID: 1354
	public struct Ammo : IEquatable<Ammo>
	{
		// Token: 0x06002F0F RID: 12047 RVA: 0x000F5D24 File Offset: 0x000F3F24
		public Ammo(int clip, int backpack)
		{
			this._clip = clip;
			this._backpack = backpack;
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002F10 RID: 12048 RVA: 0x000F5D34 File Offset: 0x000F3F34
		public int Backpack
		{
			get
			{
				return this._backpack;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002F11 RID: 12049 RVA: 0x000F5D3C File Offset: 0x000F3F3C
		public int Clip
		{
			get
			{
				return this._clip;
			}
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000F5D44 File Offset: 0x000F3F44
		public bool Equals(Ammo other)
		{
			return this.Backpack == other.Backpack && this.Clip == other.Clip;
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000F5D7C File Offset: 0x000F3F7C
		public override bool Equals(object obj)
		{
			return obj is Ammo && ((Ammo)obj).Equals(this);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000F5DAC File Offset: 0x000F3FAC
		public override int GetHashCode()
		{
			return this._backpack.GetHashCode() ^ this._clip.GetHashCode();
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000F5DD8 File Offset: 0x000F3FD8
		public override string ToString()
		{
			return this.Clip + "/" + this.Backpack;
		}

		// Token: 0x040022C3 RID: 8899
		private readonly int _backpack;

		// Token: 0x040022C4 RID: 8900
		private readonly int _clip;
	}
}
