using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006FB RID: 1787
	[Serializable]
	public class PlayerPet
	{
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x0014D810 File Offset: 0x0014BA10
		// (set) Token: 0x06003E23 RID: 15907 RVA: 0x0014D818 File Offset: 0x0014BA18
		public long NameTimestamp
		{
			get
			{
				return this.m_timestamp;
			}
			set
			{
				this.m_timestamp = value;
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x0014D824 File Offset: 0x0014BA24
		// (set) Token: 0x06003E25 RID: 15909 RVA: 0x0014D82C File Offset: 0x0014BA2C
		public string InfoId
		{
			get
			{
				return this._infoId;
			}
			set
			{
				this._infoId = value;
				this._info = null;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06003E26 RID: 15910 RVA: 0x0014D83C File Offset: 0x0014BA3C
		public PetInfo Info
		{
			get
			{
				if (this._info == null)
				{
					this._info = ((!PetsManager.Infos.ContainsKey(this.InfoId)) ? null : PetsManager.Infos[this._infoId]);
				}
				return this._info;
			}
		}

		// Token: 0x06003E27 RID: 15911 RVA: 0x0014D88C File Offset: 0x0014BA8C
		internal static PlayerPet Merge(PlayerPet left, PlayerPet right)
		{
			return null;
		}

		// Token: 0x04002DEC RID: 11756
		[SerializeField]
		public int Points;

		// Token: 0x04002DED RID: 11757
		public string PetName;

		// Token: 0x04002DEE RID: 11758
		[SerializeField]
		private string _infoId;

		// Token: 0x04002DEF RID: 11759
		private PetInfo _info;

		// Token: 0x04002DF0 RID: 11760
		[SerializeField]
		private long m_timestamp;
	}
}
