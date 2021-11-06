using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006FC RID: 1788
	[Serializable]
	public class PlayerPets
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x0014D890 File Offset: 0x0014BA90
		internal PlayerPets() : this(false)
		{
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x0014D89C File Offset: 0x0014BA9C
		internal PlayerPets(bool conflicted)
		{
			this.m_pets = new List<PlayerPet>();
			this.m_conflicted = conflicted;
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06003E2A RID: 15914 RVA: 0x0014D8C4 File Offset: 0x0014BAC4
		// (set) Token: 0x06003E2B RID: 15915 RVA: 0x0014D8CC File Offset: 0x0014BACC
		public List<PlayerPet> Pets
		{
			get
			{
				return this.m_pets;
			}
			set
			{
				this.m_pets = value;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003E2C RID: 15916 RVA: 0x0014D8D8 File Offset: 0x0014BAD8
		// (set) Token: 0x06003E2D RID: 15917 RVA: 0x0014D8E0 File Offset: 0x0014BAE0
		internal bool Conflicted
		{
			get
			{
				return this.m_conflicted;
			}
			set
			{
				this.m_conflicted = value;
			}
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x0014D8EC File Offset: 0x0014BAEC
		internal static PlayerPets Merge(PlayerPets localMemento, PlayerPets remoteMemento)
		{
			try
			{
				var first = from pet in localMemento.Pets
				select new
				{
					Pet = pet,
					Origin = PlayerPets.MergeVariantOrigin.Local
				};
				var second = from pet in remoteMemento.Pets
				select new
				{
					Pet = pet,
					Origin = PlayerPets.MergeVariantOrigin.Remote
				};
				var source = first.Concat(second);
				var source2 = from petWithOrigin in source
				group petWithOrigin by Singleton<PetsManager>.Instance.GetAllUpgrades(petWithOrigin.Pet.InfoId).FirstOrDefault<string>();
				IEnumerable<PlayerPet> collection = source2.Select(delegate(petsFromSameUpgradesChain)
				{
					Func<PlayerPet, int> indexOfPetInUpgrades = (PlayerPet pet) => pet.Info.Up;
					var <>__AnonType = petsFromSameUpgradesChain.Aggregate((highestUpPetAccumulated, currentPetWithOrigin) => (highestUpPetAccumulated != null && indexOfPetInUpgrades(currentPetWithOrigin.Pet) <= indexOfPetInUpgrades(highestUpPetAccumulated.Pet)) ? highestUpPetAccumulated : currentPetWithOrigin);
					string infoId = <>__AnonType.Pet.InfoId;
					int points = 0;
					var <>__AnonType2 = petsFromSameUpgradesChain.FirstOrDefault(petWithOrigin => petWithOrigin.Origin == PlayerPets.MergeVariantOrigin.Local);
					if (<>__AnonType2 != null)
					{
						points = <>__AnonType2.Pet.Points;
					}
					else if (<>__AnonType.Pet.Info.Up == 0)
					{
						points = 1;
					}
					var <>__AnonType3 = petsFromSameUpgradesChain.Aggregate((petWithNewestNameAccumulated, currentPetWithOrigin) => (petWithNewestNameAccumulated != null && currentPetWithOrigin.Pet.NameTimestamp <= petWithNewestNameAccumulated.Pet.NameTimestamp) ? petWithNewestNameAccumulated : currentPetWithOrigin);
					return new PlayerPet
					{
						InfoId = infoId,
						Points = points,
						PetName = <>__AnonType3.Pet.PetName,
						NameTimestamp = <>__AnonType3.Pet.NameTimestamp
					};
				});
				bool conflicted = localMemento.Conflicted || remoteMemento.Conflicted;
				PlayerPets playerPets = new PlayerPets(conflicted);
				playerPets.Pets.AddRange(collection);
				return playerPets;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in merge pets memenots: {0}", new object[]
				{
					ex
				});
			}
			return new PlayerPets(false)
			{
				Pets = localMemento.Pets
			};
		}

		// Token: 0x04002DF1 RID: 11761
		[SerializeField]
		private List<PlayerPet> m_pets = new List<PlayerPet>();

		// Token: 0x04002DF2 RID: 11762
		private bool m_conflicted;

		// Token: 0x020006FD RID: 1789
		private enum MergeVariantOrigin
		{
			// Token: 0x04002DFB RID: 11771
			Local,
			// Token: 0x04002DFC RID: 11772
			Remote
		}
	}
}
