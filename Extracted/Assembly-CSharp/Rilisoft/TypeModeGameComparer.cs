using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200074D RID: 1869
	public sealed class TypeModeGameComparer : IEqualityComparer<TypeModeGame>
	{
		// Token: 0x060041AD RID: 16813 RVA: 0x0015D970 File Offset: 0x0015BB70
		private TypeModeGameComparer()
		{
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x0015D984 File Offset: 0x0015BB84
		public static TypeModeGameComparer Instance
		{
			get
			{
				return TypeModeGameComparer.s_instance;
			}
		}

		// Token: 0x060041B0 RID: 16816 RVA: 0x0015D98C File Offset: 0x0015BB8C
		public bool Equals(TypeModeGame x, TypeModeGame y)
		{
			return x == y;
		}

		// Token: 0x060041B1 RID: 16817 RVA: 0x0015D994 File Offset: 0x0015BB94
		public int GetHashCode(TypeModeGame obj)
		{
			return (int)obj;
		}

		// Token: 0x04002FFF RID: 12287
		private static readonly TypeModeGameComparer s_instance = new TypeModeGameComparer();
	}
}
