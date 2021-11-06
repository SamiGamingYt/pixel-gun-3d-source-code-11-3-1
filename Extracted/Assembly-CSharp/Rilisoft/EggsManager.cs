using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005EE RID: 1518
	public class EggsManager : Singleton<EggsManager>
	{
		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060033D9 RID: 13273 RVA: 0x0010C6E0 File Offset: 0x0010A8E0
		// (remove) Token: 0x060033DA RID: 13274 RVA: 0x0010C6F8 File Offset: 0x0010A8F8
		public static event Action<Egg> OnReadyToUse;

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x060033DB RID: 13275 RVA: 0x0010C710 File Offset: 0x0010A910
		// (remove) Token: 0x060033DC RID: 13276 RVA: 0x0010C728 File Offset: 0x0010A928
		public static event Action<Egg, PetInfo> OnEggHatched;

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060033DD RID: 13277 RVA: 0x0010C740 File Offset: 0x0010A940
		public static long CurrentTime
		{
			get
			{
				return FriendsController.ServerTime;
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x0010C748 File Offset: 0x0010A948
		private void OnInstanceCreated()
		{
			this._eggsData = EggsData.Load();
			if (this._eggsData == null || this._eggsData.Eggs.IsNullOrEmpty<EggData>())
			{
				Debug.LogError("[EGGS] load static data fail");
				return;
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey("player_eggs"))
			{
				Storager.setString("player_eggs", string.Empty, false);
			}
			string @string = Storager.getString("player_eggs", false);
			PlayerEggs playerEggs = (!@string.IsNullOrEmpty()) ? PlayerEggs.Create(@string) : new PlayerEggs();
			PlayerEgg pData;
			foreach (PlayerEgg pData2 in playerEggs.Eggs)
			{
				pData = pData2;
				EggData eggData = this._eggsData.Eggs.FirstOrDefault((EggData d) => d.Id == pData.DataId);
				if (eggData != null)
				{
					Egg item = new Egg(eggData, pData);
					this._eggs.Add(item);
				}
				else
				{
					Debug.LogErrorFormat("[EGGS] not found egg data: '{0}'", new object[]
					{
						pData.DataId
					});
				}
			}
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x0010C8A8 File Offset: 0x0010AAA8
		protected override void Awake()
		{
			base.Awake();
			RatingSystem.OnRatingUpdate += this.RatingSystemOnRatingUpdate;
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x0010C8C4 File Offset: 0x0010AAC4
		private void Start()
		{
			CoroutineRunner.Instance.StartCoroutine(this.WaitRatingSystem());
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x0010C8D8 File Offset: 0x0010AAD8
		private void OnEnable()
		{
			base.StartCoroutine(this.UpdateEggsReadyCoroutine());
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x0010C8E8 File Offset: 0x0010AAE8
		public void OnMathEnded(bool isWinner)
		{
			if (!isWinner)
			{
				return;
			}
			IEnumerable<Egg> enumerable = from e in this._eggs
			where e.OnIncubator && !e.PlayerEggData.IsReady && e.HatchedType == EggHatchedType.Wins
			select e;
			foreach (Egg egg in enumerable)
			{
				egg.PlayerEggData.Wins++;
				if (egg.CheckReady())
				{
					this.EggReady(egg);
				}
				else
				{
					this.Save();
				}
			}
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0010C9A0 File Offset: 0x0010ABA0
		private IEnumerator WaitRatingSystem()
		{
			while (RatingSystem.instance == null)
			{
				yield return null;
			}
			this._prevPositiveRating = RatingSystem.instance.positiveRatingLocal;
			yield break;
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x0010C9BC File Offset: 0x0010ABBC
		private void RatingSystemOnRatingUpdate()
		{
			if (this._prevPositiveRating < 0)
			{
				return;
			}
			int num = RatingSystem.instance.positiveRatingLocal - this._prevPositiveRating;
			if (num < 1)
			{
				return;
			}
			this._prevPositiveRating = RatingSystem.instance.positiveRatingLocal;
			IEnumerable<Egg> enumerable = from e in this._eggs
			where e.OnIncubator && !e.PlayerEggData.IsReady && e.HatchedType == EggHatchedType.Rating
			select e;
			foreach (Egg egg in enumerable)
			{
				egg.PlayerEggData.Rating += num;
				if (egg.CheckReady())
				{
					this.EggReady(egg);
				}
				else
				{
					this.Save();
				}
			}
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x0010CAA4 File Offset: 0x0010ACA4
		public void AddEggsForSuperIncubator()
		{
			EggData eggData = this.GetEggData("SI_simple");
			this.AddEgg(eggData);
			EggData eggData2 = this.GetEggData("SI_ancient");
			this.AddEgg(eggData2);
			EggData eggData3 = this.GetEggData("SI_magical");
			this.AddEgg(eggData3);
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x0010CAF0 File Offset: 0x0010ACF0
		public List<EggData> GetAllEggs()
		{
			return this._eggsData.Eggs.ToList<EggData>();
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x0010CB04 File Offset: 0x0010AD04
		public List<Egg> GetPlayerEggs()
		{
			return this._eggs.ToList<Egg>();
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x0010CB14 File Offset: 0x0010AD14
		public List<Egg> GetPlayerEggsInIncubator()
		{
			return (from e in this.GetPlayerEggs()
			where !e.CheckReady()
			select e).ToList<Egg>();
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0010CB44 File Offset: 0x0010AD44
		public bool ExistsEgg(string eggId)
		{
			return this._eggs.Exists((Egg e) => e.Data.Id == eggId);
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x0010CB78 File Offset: 0x0010AD78
		public List<Egg> ReadyEggs()
		{
			return (from e in this._eggs
			where e.CheckReady()
			select e).ToList<Egg>();
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0010CBA8 File Offset: 0x0010ADA8
		public EggData GetEggData(string eggId)
		{
			EggData eggData = this._eggsData.Eggs.FirstOrDefault((EggData e) => e.Id == eggId);
			if (eggData == null)
			{
				Egg.LogFormat("data not found id: '{0}'", new object[]
				{
					eggId
				});
			}
			return eggData;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x0010CC00 File Offset: 0x0010AE00
		public Egg AddEgg(string eggId)
		{
			return this.AddEgg(this.GetEggData(eggId));
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x0010CC10 File Offset: 0x0010AE10
		public Egg AddEgg(EggData data)
		{
			if (data == null)
			{
				Egg.LogErrorFormat("egg data is null", new object[0]);
				return null;
			}
			if (EggsManager.CurrentTime < 1L && data.Id != "egg_Training" && data.HatchedType != EggHatchedType.Champion)
			{
				Egg.LogErrorFormat("server time not setted", new object[0]);
				return null;
			}
			int num;
			if (this._eggs.Any<Egg>())
			{
				num = this._eggs.Max((Egg e) => e.PlayerEggData.Id) + 1;
			}
			else
			{
				num = 1;
			}
			int thisId = num;
			Egg egg = new Egg(data, new PlayerEgg(data.Id, thisId));
			this._eggs.Add(egg);
			Egg.LogFormat("egg added '{0}'", new object[]
			{
				data.Id
			});
			if (data.Id == "egg_Training" || data.HatchedType == EggHatchedType.Champion)
			{
				egg.PlayerEggData.IncubationStart = RiliExtensions.SystemTime;
			}
			else if (EggsManager.CurrentTime > 0L)
			{
				egg.PlayerEggData.IncubationStart = EggsManager.CurrentTime;
			}
			this.Save();
			return egg;
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x0010CD48 File Offset: 0x0010AF48
		public Egg AddRandomEgg()
		{
			if (!this._eggsData.Eggs.Any<EggData>())
			{
				return null;
			}
			EggData[] array = (from e in this._eggsData.Eggs
			where e.HatchedType == EggHatchedType.Time || e.HatchedType == EggHatchedType.Rating
			where e.Rare == EggRarity.Simple || e.Rare == EggRarity.Ancient || e.Rare == EggRarity.Magical
			where e.Id != "egg_Training"
			select e).ToArray<EggData>();
			int num = UnityEngine.Random.Range(0, array.Count<EggData>());
			return this.AddEgg(array[num]);
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0010CDFC File Offset: 0x0010AFFC
		public string Use(Egg egg)
		{
			if (egg == null)
			{
				Egg.LogErrorFormat("egg is null", new object[0]);
				return string.Empty;
			}
			int id = egg.Id;
			egg = this._eggs.FirstOrDefault((Egg e) => e.Id == egg.Id);
			if (egg == null)
			{
				Egg.LogErrorFormat("unknown egg '{0}'", new object[]
				{
					id
				});
				return string.Empty;
			}
			if (!egg.CheckReady())
			{
				Egg.LogErrorFormat("use fail, egg not ready", new object[]
				{
					egg.Id
				});
				return string.Empty;
			}
			ItemDb.ItemRarity itemRarity = egg.DropPet();
			Egg.LogFormat("pet with rarity '{0}' dropped from egg type '{1}'", new object[]
			{
				itemRarity,
				egg.Data.Id
			});
			PetInfo randomInfo = Singleton<PetsManager>.Instance.GetRandomInfo(new ItemDb.ItemRarity?(itemRarity));
			this._eggs.Remove(egg);
			this.Save();
			if (randomInfo != null)
			{
				Egg.LogFormat("[EGGS] pet dropped: '{0}'", new object[]
				{
					randomInfo.Id
				});
				if (EggsManager.OnEggHatched != null)
				{
					EggsManager.OnEggHatched(egg, randomInfo);
				}
				return randomInfo.Id;
			}
			Egg.LogErrorFormat("[EGGS] dropped null pet. Pet rarity: {0}", new object[]
			{
				itemRarity.ToString()
			});
			return null;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0010CF84 File Offset: 0x0010B184
		private IEnumerator UpdateEggsReadyCoroutine()
		{
			for (;;)
			{
				foreach (Egg egg in this._eggs)
				{
					if (!egg.PlayerEggData.IsReady && egg.CheckReady())
					{
						this.EggReady(egg);
					}
				}
				yield return new WaitForRealSeconds(1f);
			}
			yield break;
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0010CFA0 File Offset: 0x0010B1A0
		private void EggReady(Egg egg)
		{
			egg.PlayerEggData.IsReady = true;
			this.Save();
			if (EggsManager.OnReadyToUse != null)
			{
				EggsManager.OnReadyToUse(egg);
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0010CFCC File Offset: 0x0010B1CC
		private void Save()
		{
			PlayerEggs playerEggs = new PlayerEggs();
			playerEggs.Eggs = (from e in this._eggs
			select e.PlayerEggData).ToList<PlayerEgg>();
			string val = playerEggs.ToString();
			Storager.setString("player_eggs", val, false);
		}

		// Token: 0x04002628 RID: 9768
		public const string EGG_AFTER_TRAINING = "egg_Training";

		// Token: 0x04002629 RID: 9769
		private const float UPDATE_DELAY_SECS = 1f;

		// Token: 0x0400262A RID: 9770
		public const string EGGS_PLAYER_DATA_KEY = "player_eggs";

		// Token: 0x0400262B RID: 9771
		private EggsData _eggsData;

		// Token: 0x0400262C RID: 9772
		private readonly List<Egg> _eggs = new List<Egg>();

		// Token: 0x0400262D RID: 9773
		private int _prevPositiveRating = -1;
	}
}
