using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005EB RID: 1515
	[Serializable]
	public class EggData
	{
		// Token: 0x060033D0 RID: 13264 RVA: 0x0010C478 File Offset: 0x0010A678
		public static string LkeyForRarity(EggRarity rarity)
		{
			switch (rarity)
			{
			case EggRarity.Simple:
				return "Key_2685";
			case EggRarity.Ancient:
				return "Key_2686";
			case EggRarity.Magical:
				return "Key_2687";
			case EggRarity.Champion:
				return "Key_2688";
			default:
				Debug.LogErrorFormat("LkeyForRarity: incorrect rarity: " + rarity.ToString(), new object[0]);
				return "Key_2685";
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060033D1 RID: 13265 RVA: 0x0010C4E0 File Offset: 0x0010A6E0
		// (set) Token: 0x060033D2 RID: 13266 RVA: 0x0010C514 File Offset: 0x0010A714
		public long Secs
		{
			get
			{
				if (BalanceController.timeEggs.ContainsKey(this.Id))
				{
					return (long)BalanceController.timeEggs[this.Id] * 60L;
				}
				return this._secs;
			}
			private set
			{
				this._secs = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060033D3 RID: 13267 RVA: 0x0010C520 File Offset: 0x0010A720
		public int Wins
		{
			get
			{
				if (BalanceController.victoriasEggs.ContainsKey(this.Id))
				{
					return BalanceController.victoriasEggs[this.Id];
				}
				return -1;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x0010C54C File Offset: 0x0010A74C
		public int Rating
		{
			get
			{
				if (BalanceController.ratingEggs.ContainsKey(this.Id))
				{
					return BalanceController.ratingEggs[this.Id];
				}
				return -1;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060033D5 RID: 13269 RVA: 0x0010C578 File Offset: 0x0010A778
		public List<EggPetInfo> Pets
		{
			get
			{
				if (BalanceController.rarityPetsInEggs.ContainsKey(this.Id))
				{
					return BalanceController.rarityPetsInEggs[this.Id];
				}
				return new List<EggPetInfo>();
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x0010C5A8 File Offset: 0x0010A7A8
		public EggHatchedType HatchedType
		{
			get
			{
				if (this._hatchedType == null)
				{
					if (this.Secs > 0L || this.Id == "egg_Training")
					{
						this._hatchedType = new EggHatchedType?(EggHatchedType.Time);
					}
					else if (this.League != RatingSystem.RatingLeague.none)
					{
						this._hatchedType = new EggHatchedType?(EggHatchedType.League);
					}
					else if (this.Wins > 0)
					{
						this._hatchedType = new EggHatchedType?(EggHatchedType.Wins);
					}
					else if (this.Rating > 0)
					{
						this._hatchedType = new EggHatchedType?(EggHatchedType.Rating);
					}
					else if (this.Id.IndexOf("SI_") == 0)
					{
						this._hatchedType = new EggHatchedType?(EggHatchedType.Champion);
					}
				}
				if (this._hatchedType == null)
				{
					Debug.LogErrorFormat("[EGGS] : unrecognized hatched type for egg '{0}'", new object[]
					{
						this.Id
					});
					this._hatchedType = new EggHatchedType?(EggHatchedType.Time);
					this.Secs = 3600L;
				}
				return this._hatchedType.Value;
			}
		}

		// Token: 0x0400261B RID: 9755
		[Tooltip("уникальный идентификатор")]
		[Header("Common settings")]
		public string Id;

		// Token: 0x0400261C RID: 9756
		[Tooltip("Рарность яйца")]
		public EggRarity Rare;

		// Token: 0x0400261D RID: 9757
		[Tooltip("Ключ локализации")]
		public string LKey;

		// Token: 0x0400261E RID: 9758
		private long _secs;

		// Token: 0x0400261F RID: 9759
		[Tooltip("яйцо вылупляется при достижении этой лиги")]
		public RatingSystem.RatingLeague League = RatingSystem.RatingLeague.none;

		// Token: 0x04002620 RID: 9760
		private EggHatchedType? _hatchedType;
	}
}
