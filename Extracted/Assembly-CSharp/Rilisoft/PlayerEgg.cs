using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F5 RID: 1525
	[Serializable]
	public class PlayerEgg
	{
		// Token: 0x06003430 RID: 13360 RVA: 0x0010E298 File Offset: 0x0010C498
		public PlayerEgg()
		{
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x0010E2C8 File Offset: 0x0010C4C8
		public PlayerEgg(string dataEggId, int thisId)
		{
			this.DataId = dataEggId;
			this.Id = thisId;
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x0010E304 File Offset: 0x0010C504
		public override string ToString()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x0010E30C File Offset: 0x0010C50C
		public static PlayerEggs Create(string raw)
		{
			return JsonUtility.FromJson<PlayerEggs>(raw);
		}

		// Token: 0x04002667 RID: 9831
		[SerializeField]
		public string DataId = string.Empty;

		// Token: 0x04002668 RID: 9832
		[SerializeField]
		public int Id = -1;

		// Token: 0x04002669 RID: 9833
		[SerializeField]
		public long IncubationStart = -1L;

		// Token: 0x0400266A RID: 9834
		[SerializeField]
		public bool IsReady;

		// Token: 0x0400266B RID: 9835
		[SerializeField]
		public int Wins;

		// Token: 0x0400266C RID: 9836
		[SerializeField]
		public int Rating;
	}
}
