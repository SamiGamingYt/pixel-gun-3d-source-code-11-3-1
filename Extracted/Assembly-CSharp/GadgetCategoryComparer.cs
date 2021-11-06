using System;
using System.Collections.Generic;

// Token: 0x0200063E RID: 1598
internal sealed class GadgetCategoryComparer : IEqualityComparer<GadgetInfo.GadgetCategory>
{
	// Token: 0x17000911 RID: 2321
	// (get) Token: 0x06003724 RID: 14116 RVA: 0x0011CEB8 File Offset: 0x0011B0B8
	public static GadgetCategoryComparer Instance
	{
		get
		{
			return GadgetCategoryComparer.s_instance;
		}
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0011CEC0 File Offset: 0x0011B0C0
	public bool Equals(GadgetInfo.GadgetCategory x, GadgetInfo.GadgetCategory y)
	{
		return x == y;
	}

	// Token: 0x06003726 RID: 14118 RVA: 0x0011CEC8 File Offset: 0x0011B0C8
	public int GetHashCode(GadgetInfo.GadgetCategory obj)
	{
		return (int)obj;
	}

	// Token: 0x04002830 RID: 10288
	private static readonly GadgetCategoryComparer s_instance = new GadgetCategoryComparer();
}
