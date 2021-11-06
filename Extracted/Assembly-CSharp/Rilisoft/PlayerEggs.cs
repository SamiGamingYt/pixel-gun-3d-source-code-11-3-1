using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F4 RID: 1524
	[Serializable]
	public class PlayerEggs
	{
		// Token: 0x0600342E RID: 13358 RVA: 0x0010E288 File Offset: 0x0010C488
		public override string ToString()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0010E290 File Offset: 0x0010C490
		public static PlayerEggs Create(string raw)
		{
			return JsonUtility.FromJson<PlayerEggs>(raw);
		}

		// Token: 0x04002666 RID: 9830
		[SerializeField]
		public List<PlayerEgg> Eggs = new List<PlayerEgg>();
	}
}
