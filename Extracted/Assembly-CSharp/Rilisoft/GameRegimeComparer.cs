using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200007D RID: 125
	internal sealed class GameRegimeComparer : IEqualityComparer<ConnectSceneNGUIController.RegimGame>
	{
		// Token: 0x060003D9 RID: 985 RVA: 0x0002209C File Offset: 0x0002029C
		public bool Equals(ConnectSceneNGUIController.RegimGame x, ConnectSceneNGUIController.RegimGame y)
		{
			return x == y;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000220A4 File Offset: 0x000202A4
		public int GetHashCode(ConnectSceneNGUIController.RegimGame obj)
		{
			return (int)obj;
		}
	}
}
