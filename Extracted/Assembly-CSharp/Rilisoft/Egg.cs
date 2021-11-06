using System;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005E9 RID: 1513
	public class Egg
	{
		// Token: 0x060033B9 RID: 13241 RVA: 0x0010C01C File Offset: 0x0010A21C
		public Egg(EggData staticData, PlayerEgg playerData)
		{
			this.Data = staticData;
			this.PlayerEggData = playerData;
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x0010C034 File Offset: 0x0010A234
		// (set) Token: 0x060033BB RID: 13243 RVA: 0x0010C03C File Offset: 0x0010A23C
		public EggData Data { get; private set; }

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060033BC RID: 13244 RVA: 0x0010C048 File Offset: 0x0010A248
		// (set) Token: 0x060033BD RID: 13245 RVA: 0x0010C050 File Offset: 0x0010A250
		public PlayerEgg PlayerEggData { get; private set; }

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x0010C05C File Offset: 0x0010A25C
		public int Id
		{
			get
			{
				return this.PlayerEggData.Id;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x0010C06C File Offset: 0x0010A26C
		public bool OnIncubator
		{
			get
			{
				return this.PlayerEggData != null && this.PlayerEggData.IncubationStart > 0L;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x0010C08C File Offset: 0x0010A28C
		private long CurrentTime
		{
			get
			{
				return (!(this.Data.Id == "egg_Training")) ? EggsManager.CurrentTime : RiliExtensions.SystemTime;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x0010C0B8 File Offset: 0x0010A2B8
		public long? IncubationTimeLeft
		{
			get
			{
				if (this.OnIncubator && this.CurrentTime < 0L)
				{
					return null;
				}
				return new long?(this.PlayerEggData.IncubationStart + this.Data.Secs - this.CurrentTime);
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x0010C10C File Offset: 0x0010A30C
		public long? IncubationTimeElapsed
		{
			get
			{
				if (this.OnIncubator && this.CurrentTime < 0L)
				{
					return null;
				}
				return new long?(this.CurrentTime - this.PlayerEggData.IncubationStart);
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x0010C154 File Offset: 0x0010A354
		public int WinsLeft
		{
			get
			{
				return (this.HatchedType != EggHatchedType.Wins) ? int.MaxValue : (this.Data.Wins - this.PlayerEggData.Wins);
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x0010C190 File Offset: 0x0010A390
		public int RatingLeft
		{
			get
			{
				if (this.HatchedType != EggHatchedType.Rating || RatingSystem.instance == null)
				{
					return int.MaxValue;
				}
				return Mathf.Clamp(this.Data.Rating - this.PlayerEggData.Rating, 0, int.MaxValue);
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x0010C1E4 File Offset: 0x0010A3E4
		public EggHatchedType HatchedType
		{
			get
			{
				return this.Data.HatchedType;
			}
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x0010C1F4 File Offset: 0x0010A3F4
		public bool CheckReady()
		{
			if (this.PlayerEggData.IsReady)
			{
				return true;
			}
			switch (this.HatchedType)
			{
			case EggHatchedType.Time:
				if (this.OnIncubator && this.IncubationTimeLeft != null && this.IncubationTimeLeft.Value <= 0L)
				{
					return true;
				}
				break;
			case EggHatchedType.League:
				if (RatingSystem.instance != null && RatingSystem.instance.currentLeague >= this.Data.League)
				{
					return true;
				}
				break;
			case EggHatchedType.Wins:
				if (this.PlayerEggData.Wins >= this.Data.Wins)
				{
					return true;
				}
				break;
			case EggHatchedType.Champion:
				return true;
			case EggHatchedType.Rating:
				return this.RatingLeft <= 0;
			}
			return false;
		}

		// Token: 0x060033C7 RID: 13255 RVA: 0x0010C2DC File Offset: 0x0010A4DC
		public ItemDb.ItemRarity DropPet()
		{
			ItemDb.ItemRarity result = ItemDb.ItemRarity.Common;
			EggPetInfo[] array = (from p in this.Data.Pets
			where p.Chance > 0f
			select p).ToArray<EggPetInfo>();
			if (!array.Any<EggPetInfo>())
			{
				return result;
			}
			float max = array.Sum((EggPetInfo p) => p.Chance);
			float num = UnityEngine.Random.Range(0f, max);
			float num2 = 0f;
			foreach (EggPetInfo eggPetInfo in array)
			{
				num2 += eggPetInfo.Chance;
				if (num2 > num)
				{
					result = eggPetInfo.Rarity;
					break;
				}
			}
			return result;
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0010C3A8 File Offset: 0x0010A5A8
		public string GetRelativeMeshTexturePath()
		{
			string result;
			if (this.HatchedType == EggHatchedType.League)
			{
				result = string.Format("Eggs/Textures/egg_champion_{0}_texture", this.Data.League.ToString());
			}
			else
			{
				result = string.Format("Eggs/Textures/{0}_texture", this.Data.Id);
			}
			return result;
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0010C400 File Offset: 0x0010A600
		public static void LogErrorFormat(string format, params object[] args)
		{
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0010C404 File Offset: 0x0010A604
		public static void LogFormat(string format, params object[] args)
		{
		}
	}
}
