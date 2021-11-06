using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000840 RID: 2112
	[Serializable]
	public struct SkinsMemento : IEquatable<SkinsMemento>
	{
		// Token: 0x06004CAF RID: 19631 RVA: 0x001B9C80 File Offset: 0x001B7E80
		public SkinsMemento(IEnumerable<SkinMemento> skins, IEnumerable<string> deletedSkins, CapeMemento cape)
		{
			this = new SkinsMemento(skins, deletedSkins, cape, false);
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x001B9C8C File Offset: 0x001B7E8C
		public SkinsMemento(IEnumerable<SkinMemento> skins, IEnumerable<string> deletedSkins, CapeMemento cape, bool conflicted)
		{
			this.skins = ((skins != null) ? skins.ToList<SkinMemento>() : new List<SkinMemento>());
			this.deletedSkins = ((deletedSkins != null) ? deletedSkins.ToList<string>() : new List<string>());
			this.cape = cape;
			this._conflicted = conflicted;
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x001B9CE0 File Offset: 0x001B7EE0
		public bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x001B9CE8 File Offset: 0x001B7EE8
		public CapeMemento Cape
		{
			get
			{
				return this.cape;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004CB3 RID: 19635 RVA: 0x001B9CF0 File Offset: 0x001B7EF0
		public List<SkinMemento> Skins
		{
			get
			{
				if (this.skins == null)
				{
					this.skins = new List<SkinMemento>();
				}
				return this.skins;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x001B9D10 File Offset: 0x001B7F10
		public List<string> DeletedSkins
		{
			get
			{
				if (this.deletedSkins == null)
				{
					this.deletedSkins = new List<string>();
				}
				return this.deletedSkins;
			}
		}

		// Token: 0x06004CB5 RID: 19637 RVA: 0x001B9D30 File Offset: 0x001B7F30
		public Dictionary<string, SkinMemento> GetSkinsAsDictionary()
		{
			Dictionary<string, SkinMemento> dictionary = new Dictionary<string, SkinMemento>(this.Skins.Count);
			foreach (SkinMemento value in this.Skins)
			{
				dictionary[value.Id] = value;
			}
			return dictionary;
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x001B9DB0 File Offset: 0x001B7FB0
		public bool Equals(SkinsMemento other)
		{
			return this.Cape.Equals(other.Cape) && this.Skins.SequenceEqual(other.Skins);
		}

		// Token: 0x06004CB7 RID: 19639 RVA: 0x001B9DF4 File Offset: 0x001B7FF4
		public override bool Equals(object obj)
		{
			if (!(obj is SkinsMemento))
			{
				return false;
			}
			SkinsMemento other = (SkinsMemento)obj;
			return this.Equals(other);
		}

		// Token: 0x06004CB8 RID: 19640 RVA: 0x001B9E1C File Offset: 0x001B801C
		public override int GetHashCode()
		{
			return this.Cape.GetHashCode() ^ this.Skins.GetHashCode() ^ this.DeletedSkins.GetHashCode();
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x001B9E50 File Offset: 0x001B8050
		public override string ToString()
		{
			string[] value = (from s in this.Skins
			select string.Format("\"{0}\"", s.Name)).ToArray<string>();
			string text = string.Join(",", value);
			string text2 = string.Join(",", this.DeletedSkins.ToArray());
			return string.Format(CultureInfo.InvariantCulture, "{{ \"skins\":[{0}], \"cape\":\"{1}\", \"deletedSkins\":[{2}] }}", new object[]
			{
				text,
				this.cape,
				text2
			});
		}

		// Token: 0x06004CBA RID: 19642 RVA: 0x001B9EDC File Offset: 0x001B80DC
		internal static SkinsMemento Merge(SkinsMemento left, SkinsMemento right)
		{
			HashSet<string> hashSet = new HashSet<string>(left.DeletedSkins.Concat(right.DeletedSkins));
			Dictionary<string, SkinMemento> dictionary = new Dictionary<string, SkinMemento>();
			foreach (SkinMemento value in left.Skins)
			{
				if (!hashSet.Contains(value.Id))
				{
					dictionary[value.Id] = value;
				}
			}
			foreach (SkinMemento value2 in right.Skins)
			{
				if (!hashSet.Contains(value2.Id))
				{
					dictionary[value2.Id] = value2;
				}
			}
			bool conflicted = left.Conflicted || right.Conflicted;
			CapeMemento capeMemento = CapeMemento.ChooseCape(left.Cape, right.Cape);
			return new SkinsMemento(dictionary.Values.ToList<SkinMemento>(), hashSet, capeMemento, conflicted);
		}

		// Token: 0x04003B60 RID: 15200
		private readonly bool _conflicted;

		// Token: 0x04003B61 RID: 15201
		[SerializeField]
		private CapeMemento cape;

		// Token: 0x04003B62 RID: 15202
		[SerializeField]
		private List<SkinMemento> skins;

		// Token: 0x04003B63 RID: 15203
		[SerializeField]
		private List<string> deletedSkins;
	}
}
