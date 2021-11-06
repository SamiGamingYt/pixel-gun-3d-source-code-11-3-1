using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x020006FA RID: 1786
	public class PetsManager : Singleton<PetsManager>
	{
		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06003DEF RID: 15855 RVA: 0x0014C78C File Offset: 0x0014A98C
		// (remove) Token: 0x06003DF0 RID: 15856 RVA: 0x0014C7A4 File Offset: 0x0014A9A4
		public static event Action OnPetsUpdated;

		// Token: 0x14000076 RID: 118
		// (add) Token: 0x06003DF1 RID: 15857 RVA: 0x0014C7BC File Offset: 0x0014A9BC
		// (remove) Token: 0x06003DF2 RID: 15858 RVA: 0x0014C7D8 File Offset: 0x0014A9D8
		public event Action<string> OnPlayerPetAdded;

		// Token: 0x14000077 RID: 119
		// (add) Token: 0x06003DF3 RID: 15859 RVA: 0x0014C7F4 File Offset: 0x0014A9F4
		// (remove) Token: 0x06003DF4 RID: 15860 RVA: 0x0014C810 File Offset: 0x0014AA10
		public event Action<PetUpdateInfo> OnPlayerPetChanged;

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x0014C82C File Offset: 0x0014AA2C
		public static Dictionary<string, PetInfo> Infos
		{
			get
			{
				return PetsInfo.info;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x0014C834 File Offset: 0x0014AA34
		// (set) Token: 0x06003DF7 RID: 15863 RVA: 0x0014C83C File Offset: 0x0014AA3C
		public List<PlayerPet> PlayerPets { get; private set; }

		// Token: 0x06003DF8 RID: 15864 RVA: 0x0014C848 File Offset: 0x0014AA48
		private void OnInstanceCreated()
		{
			this.LoadPlayerPets();
			this.ActualizeEquippedPet();
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0014C858 File Offset: 0x0014AA58
		public static bool IsEmptySlotId(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("PetsManager.IsEmptySlotId: petId.IsNullOrEmpty()", new object[0]);
				return false;
			}
			return petId.Contains("_empty_slot");
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x0014C890 File Offset: 0x0014AA90
		public IEnumerable<string> PlayerPetIdsAndEmptySlots()
		{
			IEnumerable<string> enumerable = from pet in this.PlayerPets
			select pet.InfoId;
			IEnumerable<string> first = (from petId in PetsInfo.info.Keys
			select this.GetIdWithoutUp(petId)).Distinct<string>();
			IEnumerable<string> source = first.Except(from petId in enumerable
			select this.GetIdWithoutUp(petId));
			IEnumerable<string> second = from petId in source
			select string.Format("{0}{1}", petId, "_empty_slot");
			return from petId in enumerable.Concat(second)
			orderby PetsManager.PetsSortOrder.IndexOf(PetsManager.PetIdWithoutSuffixes(petId))
			select petId;
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0014C954 File Offset: 0x0014AB54
		public static string PetIdWithoutSuffixes(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("PetIdWithoutSuffixes: petId.IsNullOrEmpty()", new object[0]);
				return string.Empty;
			}
			if (Singleton<PetsManager>.Instance == null)
			{
				Debug.LogErrorFormat("PetIdWithoutSuffixes: Instance == null, petId = {0}", new object[]
				{
					petId
				});
				return petId;
			}
			string idWithoutUp = Singleton<PetsManager>.Instance.GetIdWithoutUp(petId);
			int num = idWithoutUp.IndexOf("_empty_slot");
			if (num == -1)
			{
				return idWithoutUp;
			}
			return idWithoutUp.Substring(0, num);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x0014C9D4 File Offset: 0x0014ABD4
		public static void LoadPetsToMemory()
		{
			try
			{
				bool flag = SceneManager.GetActiveScene().name == Defs.MainMenuScene;
				if (flag && !ShopNGUIController.GuiActive)
				{
					Singleton<PetsManager>.Instance.LoadPlayerPets();
					Singleton<PetsManager>.Instance.ActualizeEquippedPet();
					Action onPetsUpdated = PetsManager.OnPetsUpdated;
					if (onPetsUpdated != null)
					{
						onPetsUpdated();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in LoadPetsToMemory: {0}", new object[]
				{
					ex
				});
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x0014CA70 File Offset: 0x0014AC70
		public string GetFirstSmallestUpPet(List<string> fromPets)
		{
			if (fromPets == null || !fromPets.Any<string>())
			{
				return string.Empty;
			}
			List<string> uniquePets = (from id in fromPets.Distinct<string>()
			select this.GetIdWithoutUp(id)).ToList<string>();
			List<PlayerPet> existsPets = (from pp in this.PlayerPets
			where uniquePets.Contains(pp.Info.IdWithoutUp)
			orderby uniquePets.IndexOf(pp.Info.IdWithoutUp)
			select pp).ToList<PlayerPet>();
			if (!existsPets.Any<PlayerPet>())
			{
				return uniquePets.First<string>();
			}
			if (existsPets.All((PlayerPet pp) => pp.Info.Up == 5))
			{
				return null;
			}
			if (existsPets.Count < uniquePets.Count)
			{
				return uniquePets.First((string p) => existsPets.All((PlayerPet pp) => pp.Info.IdWithoutUp != p));
			}
			PlayerPet playerPet = existsPets.First<PlayerPet>();
			foreach (PlayerPet playerPet2 in existsPets)
			{
				if (playerPet.Info.Up > playerPet2.Info.Up)
				{
					playerPet = playerPet2;
				}
			}
			return playerPet.Info.IdWithoutUp;
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x0014CBFC File Offset: 0x0014ADFC
		public string GetIdWithoutUp(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return petId;
			}
			if (!PetsManager.Infos.Keys.Contains(petId))
			{
				return petId;
			}
			return PetsManager.Infos[petId].IdWithoutUp;
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x0014CC40 File Offset: 0x0014AE40
		public int GetUp(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return -1;
			}
			if (!PetsManager.Infos.Keys.Contains(petId))
			{
				return -1;
			}
			return PetsManager.Infos[petId].Up;
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0014CC84 File Offset: 0x0014AE84
		public string GetRelativePrefabPath(string petId)
		{
			return (!PetsManager.Infos.ContainsKey(petId)) ? string.Empty : PetsManager.Infos[petId].GetRelativePrefabPath();
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0014CCBC File Offset: 0x0014AEBC
		public PetInfo GetInfo(string petId)
		{
			return (!PetsManager.Infos.ContainsKey(petId)) ? this.GetFirstUpgrade(petId) : PetsManager.Infos[petId];
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0014CCF0 File Offset: 0x0014AEF0
		public PetInfo GetRandomInfo(ItemDb.ItemRarity? rarity)
		{
			string[] array = this.PetsIdsByRarity(rarity, true);
			if (array.Length > 0)
			{
				int num = UnityEngine.Random.Range(0, array.Length);
				return this.GetFirstUpgrade(array[num]);
			}
			return null;
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0014CD24 File Offset: 0x0014AF24
		public string[] PetsIdsByRarity(ItemDb.ItemRarity? rarity, bool ignoreUpgrades = false)
		{
			IEnumerable<PetInfo> enumerable;
			if (rarity == null)
			{
				IEnumerable<PetInfo> values = PetsManager.Infos.Values;
				enumerable = values;
			}
			else
			{
				enumerable = from i in PetsManager.Infos.Values
				where i.Rarity == rarity.GetValueOrDefault() && rarity != null
				select i;
			}
			IEnumerable<PetInfo> source = enumerable;
			string[] result;
			if (!ignoreUpgrades)
			{
				result = (from i in source
				select i.Id).ToArray<string>();
			}
			else
			{
				result = (from i in source
				select i.IdWithoutUp).Distinct<string>().ToArray<string>();
			}
			return result;
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x0014CDE0 File Offset: 0x0014AFE0
		public string GetEqipedPetId()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey("EquppedPetSN"))
			{
				Storager.setString("EquppedPetSN", string.Empty, false);
			}
			return Storager.getString("EquppedPetSN", false);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x0014CE18 File Offset: 0x0014B018
		public string GetBaseEquipedPetId()
		{
			string eqipedPetId = this.GetEqipedPetId();
			return (!eqipedPetId.IsNullOrEmpty()) ? this.GetIdWithoutUp(eqipedPetId) : string.Empty;
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x0014CE48 File Offset: 0x0014B048
		public void SetEquipedPet(string petId)
		{
			string eqipedPetId = this.GetEqipedPetId();
			if (!petId.IsNullOrEmpty() && !this.IsExistsPet(petId))
			{
				Debug.LogErrorFormat("[PETS] pet '{0}' not found", new object[]
				{
					petId
				});
				return;
			}
			Storager.setString("EquppedPetSN", petId, false);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x0014CE94 File Offset: 0x0014B094
		public bool IsExistsPet(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return false;
			}
			string baseId = this.GetIdWithoutUp(petId);
			return !baseId.IsNullOrEmpty() && this.PlayerPets.Any((PlayerPet p) => p.Info.IdWithoutUp == baseId);
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x0014CEEC File Offset: 0x0014B0EC
		public PlayerPet GetPlayerPet(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return null;
			}
			string baseId = this.GetIdWithoutUp(petId);
			if (baseId.IsNullOrEmpty())
			{
				return null;
			}
			return this.PlayerPets.FirstOrDefault((PlayerPet p) => p.Info.IdWithoutUp == baseId);
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0014CF44 File Offset: 0x0014B144
		public bool SetPetName(string petId, string newName)
		{
			if (newName.IsNullOrEmpty())
			{
				return false;
			}
			PlayerPet playerPet = this.GetPlayerPet(petId);
			if (playerPet == null)
			{
				return false;
			}
			playerPet.PetName = newName;
			playerPet.NameTimestamp = DateTime.UtcNow.Ticks;
			this.Save();
			return true;
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x0014CF90 File Offset: 0x0014B190
		public PetUpdateInfo AddOrUpdatePet(string petId)
		{
			if (this.GetInfo(petId) == null)
			{
				Debug.LogErrorFormat("[PETS] pet data '{0}' not found", new object[]
				{
					petId
				});
				return null;
			}
			PetUpdateInfo petUpdateInfo = new PetUpdateInfo();
			if (this.IsExistsPet(petId))
			{
				PlayerPet playerPet = this.GetPlayerPet(petId);
				petUpdateInfo.PetOld = new PlayerPet
				{
					InfoId = playerPet.InfoId,
					Points = playerPet.Points,
					NameTimestamp = playerPet.NameTimestamp
				};
				playerPet.Points++;
				petUpdateInfo.PetNew = playerPet;
				this.Save();
			}
			else
			{
				string id = this.GetFirstUpgrade(petId).Id;
				PetInfo info = this.GetInfo(id);
				PlayerPet playerPet2 = new PlayerPet
				{
					InfoId = id,
					Points = 1,
					NameTimestamp = DateTime.UtcNow.Ticks,
					PetName = ((info == null) ? string.Empty : LocalizationStore.Get(info.Lkey))
				};
				this.PlayerPets.Add(playerPet2);
				this.Save();
				if (this.OnPlayerPetAdded != null)
				{
					this.OnPlayerPetAdded(petId);
				}
				petUpdateInfo.PetNew = playerPet2;
			}
			if (this.OnPlayerPetChanged != null)
			{
				this.OnPlayerPetChanged(petUpdateInfo);
			}
			return petUpdateInfo;
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x0014D0E8 File Offset: 0x0014B2E8
		public PetUpdateInfo Upgrade(string petId)
		{
			if (this.GetInfo(petId) == null)
			{
				Debug.LogErrorFormat("[PETS] pet data '{0}' not found", new object[]
				{
					petId
				});
				return null;
			}
			PlayerPet playerPet = this.GetPlayerPet(petId);
			if (playerPet == null)
			{
				Debug.LogErrorFormat("[PETS] pet '{0}' not found", new object[]
				{
					petId
				});
				return null;
			}
			PetInfo nextUp = this.GetNextUp(playerPet.InfoId);
			if (nextUp == null)
			{
				Debug.LogErrorFormat("[PETS] pet '{0}' buy error: next UP not found", new object[]
				{
					petId
				});
				return null;
			}
			int num = playerPet.Info.ToUpPoints - playerPet.Points;
			if (num > 0)
			{
				Debug.LogErrorFormat("[PETS] pet '{0}' buy error: lacks points", new object[]
				{
					petId
				});
				return null;
			}
			PetUpdateInfo petUpdateInfo = new PetUpdateInfo
			{
				PetOld = new PlayerPet
				{
					InfoId = playerPet.InfoId,
					Points = playerPet.Points,
					NameTimestamp = playerPet.NameTimestamp
				}
			};
			playerPet.InfoId = nextUp.Id;
			playerPet.Points = Mathf.Abs(num);
			this.Save();
			petUpdateInfo.PetNew = playerPet;
			if (this.OnPlayerPetChanged != null)
			{
				this.OnPlayerPetChanged(petUpdateInfo);
			}
			if (petUpdateInfo.PetOld.InfoId == this.GetEqipedPetId())
			{
				this.SetEquipedPet(petUpdateInfo.PetNew.InfoId);
			}
			return petUpdateInfo;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x0014D248 File Offset: 0x0014B448
		public bool NextUpAvailable(string petId)
		{
			if (this.GetInfo(petId) == null)
			{
				return false;
			}
			PlayerPet playerPet = this.GetPlayerPet(petId);
			return playerPet != null && this.GetNextUp(playerPet.InfoId) != null && playerPet.Info.ToUpPoints - playerPet.Points < 1;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x0014D2A0 File Offset: 0x0014B4A0
		public string[] GetAllUpgrades(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return null;
			}
			PetInfo info = this.GetInfo(petId);
			if (info == null)
			{
				info = this.GetFirstUpgrade(petId);
			}
			if (info == null)
			{
				return null;
			}
			return (from i in PetsManager.Infos.Values
			where i.IdWithoutUp == info.IdWithoutUp
			orderby i.Up
			select i.Id).ToArray<string>();
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x0014D358 File Offset: 0x0014B558
		public PetInfo GetFirstUpgrade(string petId)
		{
			string id = this.GetIdWithoutUp(petId);
			if (id.IsNullOrEmpty())
			{
				return null;
			}
			return (from i in PetsManager.Infos.Values
			where i.IdWithoutUp == id
			orderby i.Up
			select i).FirstOrDefault<PetInfo>();
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x0014D3CC File Offset: 0x0014B5CC
		public PetInfo GetFirstUnboughtPet(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				Debug.LogErrorFormat("GetFirstUnboughtPet: petId.IsNullOrEmpty()", new object[0]);
				return null;
			}
			if (PetsManager.IsEmptySlotId(petId))
			{
				Debug.LogErrorFormat("GetFirstUnboughtPet: IsEmptySlotId(petId), petId = {0}", new object[]
				{
					petId
				});
				return null;
			}
			string text = null;
			try
			{
				List<string> list = this.GetAllUpgrades(petId).ToList<string>();
				string idWithoutUp = this.GetIdWithoutUp(petId);
				PlayerPet playerPet = this.PlayerPets.FirstOrDefault((PlayerPet p) => this.GetIdWithoutUp(p.InfoId) == idWithoutUp);
				if (playerPet != null)
				{
					int num = list.IndexOf(playerPet.InfoId);
					if (num == -1)
					{
						Debug.LogErrorFormat("GetFirstUnboughtPet: indexOfPlayerPetInUpgrades == -1, petId = {0}", new object[]
						{
							petId
						});
					}
					text = list[Math.Min(num + 1, list.Count - 1)];
				}
				else
				{
					text = list.First<string>();
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in GetFirstUnboughtPet: {0}", new object[]
				{
					ex
				});
			}
			PetInfo result = null;
			try
			{
				if (text != null)
				{
					result = this.GetInfo(text);
				}
				else
				{
					Debug.LogErrorFormat("GetFirstUnboughtPet: firstUnbought == null, petId = {0}", new object[]
					{
						petId
					});
				}
			}
			catch (Exception ex2)
			{
				Debug.LogErrorFormat("Exception in GetFirstUnboughtPet, getting info of first unbought: {0}", new object[]
				{
					ex2
				});
			}
			return result;
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0014D550 File Offset: 0x0014B750
		public PetInfo GetNextUp(string petId)
		{
			if (petId.IsNullOrEmpty())
			{
				return null;
			}
			PetInfo info = this.GetInfo(petId);
			if (info == null)
			{
				return null;
			}
			return PetsManager.Infos.Values.FirstOrDefault((PetInfo i) => i.IdWithoutUp == info.IdWithoutUp && i.Up == info.Up + 1);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0014D5A8 File Offset: 0x0014B7A8
		public void ActualizeEquippedPet()
		{
			try
			{
				string eqipedPetId = this.GetEqipedPetId();
				if (!eqipedPetId.IsNullOrEmpty())
				{
					string baseEquippedPetId = this.GetIdWithoutUp(eqipedPetId);
					PlayerPet playerPet2 = this.PlayerPets.FirstOrDefault((PlayerPet playerPet) => this.GetIdWithoutUp(playerPet.InfoId) == baseEquippedPetId);
					if (playerPet2 == null)
					{
						this.SetEquipedPet(string.Empty);
						Debug.LogErrorFormat("ActualizeEquippedPet: equippedPetOrItsUpgrade == null, equippedPetId = {0}", new object[]
						{
							eqipedPetId
						});
					}
					else if (!(playerPet2.InfoId == eqipedPetId))
					{
						if (playerPet2.InfoId.IsNullOrEmpty())
						{
							Debug.LogErrorFormat("ActualizeEquippedPet: equippedPetOrItsUpgrade.InfoId.IsNullOrEmpty(), equippedPetId = {0}", new object[]
							{
								eqipedPetId
							});
						}
						else
						{
							this.SetEquipedPet(playerPet2.InfoId);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in ActualizeEquippedPet: {0}", new object[]
				{
					ex
				});
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0014D6B0 File Offset: 0x0014B8B0
		public void LoadPlayerPets()
		{
			PlayerPets playerPets = PetsManager.LoadPlayerPetsMemento();
			this.PlayerPets = playerPets.Pets;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0014D6D0 File Offset: 0x0014B8D0
		private void Save()
		{
			PetsManager.OverwritePlayerPetsMemento(new PlayerPets(false)
			{
				Pets = this.PlayerPets
			});
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x0014D6F8 File Offset: 0x0014B8F8
		internal static PlayerPets LoadPlayerPetsMemento()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey("pets"))
			{
				Storager.setString("pets", string.Empty, false);
			}
			string @string = Storager.getString("pets", false);
			if (@string == string.Empty)
			{
				return new PlayerPets(false);
			}
			return JsonUtility.FromJson<PlayerPets>(@string);
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x0014D758 File Offset: 0x0014B958
		internal static void OverwritePlayerPetsMemento(PlayerPets memento)
		{
			string val = JsonUtility.ToJson(memento ?? new PlayerPets(false));
			Storager.setString("pets", val, false);
		}

		// Token: 0x04002DDC RID: 11740
		private const string KEY_PLAYER_PETS = "pets";

		// Token: 0x04002DDD RID: 11741
		public const string PET_EMPTY_SLOT_SUFFIX = "_empty_slot";

		// Token: 0x04002DDE RID: 11742
		public static readonly List<string> PetsSortOrder = new List<string>
		{
			"pet_cat",
			"pet_dog",
			"pet_parrot",
			"pet_rabbit",
			"pet_bat",
			"pet_fox",
			"pet_penguin",
			"pet_wolf",
			"pet_owl",
			"pet_snake",
			"pet_lion",
			"pet_panda",
			"pet_eagle",
			"pet_deer",
			"pet_bear",
			"pet_pterodactyl",
			"pet_dinosaur",
			"pet_arnold_3000",
			"pet_unicorn",
			"pet_phoenix",
			"pet_magical_dragon"
		};
	}
}
