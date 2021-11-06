using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005EA RID: 1514
	[CreateAssetMenu(fileName = "eggs_data", menuName = "Rilisoft/SO/EggsData")]
	public class EggsData : ScriptableObject
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x0010C434 File Offset: 0x0010A634
		public static EggsData Load()
		{
			EggsData eggsData = Resources.Load<EggsData>("Eggs/eggs_data");
			if (eggsData == null)
			{
				Debug.LogError("[EGGS] data not found");
				return null;
			}
			return eggsData;
		}

		// Token: 0x04002619 RID: 9753
		public const string EGGS_DATA_PATH = "Eggs/eggs_data";

		// Token: 0x0400261A RID: 9754
		[SerializeField]
		public List<EggData> Eggs = new List<EggData>();
	}
}
