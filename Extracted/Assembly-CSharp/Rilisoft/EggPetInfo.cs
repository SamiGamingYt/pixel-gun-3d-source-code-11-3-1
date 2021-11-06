using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005EC RID: 1516
	[Serializable]
	public class EggPetInfo
	{
		// Token: 0x04002621 RID: 9761
		[SerializeField]
		[Tooltip("Тип пета")]
		public ItemDb.ItemRarity Rarity;

		// Token: 0x04002622 RID: 9762
		[SerializeField]
		[Tooltip("шанс получения")]
		public float Chance;
	}
}
