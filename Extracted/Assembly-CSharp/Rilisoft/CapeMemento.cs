using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000834 RID: 2100
	[Serializable]
	public struct CapeMemento : IEquatable<CapeMemento>
	{
		// Token: 0x06004C49 RID: 19529 RVA: 0x001B7F84 File Offset: 0x001B6184
		public CapeMemento(long id, string cape)
		{
			this.id = id;
			this.cape = (cape ?? string.Empty);
			this._capeHashCode = null;
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x001B7FBC File Offset: 0x001B61BC
		public long Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x001B7FC4 File Offset: 0x001B61C4
		public string Cape
		{
			get
			{
				return this.cape ?? string.Empty;
			}
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x001B7FD8 File Offset: 0x001B61D8
		public bool Equals(CapeMemento other)
		{
			return this.Id == other.Id && this.GetCapeHashCode() == other.GetCapeHashCode() && !(this.Cape != other.Cape);
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x001B8028 File Offset: 0x001B6228
		public override bool Equals(object obj)
		{
			if (!(obj is SkinMemento))
			{
				return false;
			}
			SkinMemento skinMemento = (SkinMemento)obj;
			return this.Equals(skinMemento);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x001B8058 File Offset: 0x001B6258
		public override int GetHashCode()
		{
			return this.Id.GetHashCode() ^ this.GetCapeHashCode();
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x001B807C File Offset: 0x001B627C
		public override string ToString()
		{
			string text = (this.Cape.Length > 4) ? this.Cape.Substring(this.Cape.Length - 4) : this.Cape;
			return string.Format(CultureInfo.InvariantCulture, "{{ \"id\":{0},\"cape\":\"{1}\" }}", new object[]
			{
				this.Id,
				text
			});
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x001B80E8 File Offset: 0x001B62E8
		internal static CapeMemento ChooseCape(CapeMemento left, CapeMemento right)
		{
			if (string.IsNullOrEmpty(left.Cape) && string.IsNullOrEmpty(right.Cape))
			{
				return (left.Id > right.Id) ? left : right;
			}
			if (!string.IsNullOrEmpty(left.Cape) && !string.IsNullOrEmpty(right.Cape))
			{
				return (left.Id > right.Id) ? left : right;
			}
			if (!string.IsNullOrEmpty(left.Cape))
			{
				return left;
			}
			return right;
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x001B8184 File Offset: 0x001B6384
		private int GetCapeHashCode()
		{
			if (this._capeHashCode == null)
			{
				this._capeHashCode = new int?(this.Cape.GetHashCode());
			}
			return this._capeHashCode.Value;
		}

		// Token: 0x04003B3C RID: 15164
		[SerializeField]
		private long id;

		// Token: 0x04003B3D RID: 15165
		[SerializeField]
		private string cape;

		// Token: 0x04003B3E RID: 15166
		private int? _capeHashCode;
	}
}
