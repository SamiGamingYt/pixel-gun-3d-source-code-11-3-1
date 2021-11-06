using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200083F RID: 2111
	[Serializable]
	public struct SkinMemento : IEquatable<SkinMemento>
	{
		// Token: 0x06004CA6 RID: 19622 RVA: 0x001B9AA8 File Offset: 0x001B7CA8
		public SkinMemento(string id, string name, string skin)
		{
			this.id = (id ?? string.Empty);
			this.name = (name ?? string.Empty);
			this.skin = (skin ?? string.Empty);
			this._skinHashCode = null;
		}

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004CA7 RID: 19623 RVA: 0x001B9B00 File Offset: 0x001B7D00
		public string Id
		{
			get
			{
				return this.id ?? string.Empty;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x001B9B14 File Offset: 0x001B7D14
		public string Name
		{
			get
			{
				return this.name ?? string.Empty;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004CA9 RID: 19625 RVA: 0x001B9B28 File Offset: 0x001B7D28
		public string Skin
		{
			get
			{
				return this.skin ?? string.Empty;
			}
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x001B9B3C File Offset: 0x001B7D3C
		public bool Equals(SkinMemento other)
		{
			return !(this.Id != other.Id) && !(this.Name != other.Name) && this.GetSkinHashCode() == other.GetSkinHashCode() && !(this.Skin != other.Skin);
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x001B9BAC File Offset: 0x001B7DAC
		public override bool Equals(object obj)
		{
			if (!(obj is SkinMemento))
			{
				return false;
			}
			SkinMemento other = (SkinMemento)obj;
			return this.Equals(other);
		}

		// Token: 0x06004CAC RID: 19628 RVA: 0x001B9BD4 File Offset: 0x001B7DD4
		public override int GetHashCode()
		{
			return this.Id.GetHashCode() ^ this.Name.GetHashCode() ^ this.GetSkinHashCode();
		}

		// Token: 0x06004CAD RID: 19629 RVA: 0x001B9C00 File Offset: 0x001B7E00
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{ \"id\":{0},\"name\":{1},\"skin\":{2} }}", new object[]
			{
				this.Id,
				this.Name,
				this.Skin
			});
		}

		// Token: 0x06004CAE RID: 19630 RVA: 0x001B9C40 File Offset: 0x001B7E40
		private int GetSkinHashCode()
		{
			if (this._skinHashCode == null)
			{
				this._skinHashCode = new int?(this.Skin.GetHashCode());
			}
			return this._skinHashCode.Value;
		}

		// Token: 0x04003B5C RID: 15196
		[SerializeField]
		private string id;

		// Token: 0x04003B5D RID: 15197
		[SerializeField]
		private string name;

		// Token: 0x04003B5E RID: 15198
		[SerializeField]
		private string skin;

		// Token: 0x04003B5F RID: 15199
		private int? _skinHashCode;
	}
}
