using System;
using System.Collections.Generic;
using Rilisoft;

// Token: 0x02000658 RID: 1624
public static class InGameGadgetSet
{
	// Token: 0x0600385B RID: 14427 RVA: 0x00123D1C File Offset: 0x00121F1C
	public static void Renew()
	{
		string[] array = new string[]
		{
			GadgetsInfo.EquippedForCategory(GadgetInfo.GadgetCategory.Throwing),
			GadgetsInfo.EquippedForCategory(GadgetInfo.GadgetCategory.Tools),
			GadgetsInfo.EquippedForCategory(GadgetInfo.GadgetCategory.Support)
		};
		InGameGadgetSet._inGameGadgets.Clear();
		if (!array[0].IsNullOrEmpty())
		{
			InGameGadgetSet._inGameGadgets.Add(GadgetInfo.GadgetCategory.Throwing, Gadget.Create(GadgetsInfo.info[array[0]]));
		}
		if (!array[1].IsNullOrEmpty())
		{
			InGameGadgetSet._inGameGadgets.Add(GadgetInfo.GadgetCategory.Tools, Gadget.Create(GadgetsInfo.info[array[1]]));
		}
		if (!array[2].IsNullOrEmpty())
		{
			InGameGadgetSet._inGameGadgets.Add(GadgetInfo.GadgetCategory.Support, Gadget.Create(GadgetsInfo.info[array[2]]));
		}
		InGameGadgetSet._inGameGadgetsInitialized = true;
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x0600385C RID: 14428 RVA: 0x00123DF4 File Offset: 0x00121FF4
	public static Dictionary<GadgetInfo.GadgetCategory, Gadget> CurrentSet
	{
		get
		{
			if (!InGameGadgetSet._inGameGadgetsInitialized)
			{
				InGameGadgetSet.Renew();
			}
			return InGameGadgetSet._inGameGadgets;
		}
	}

	// Token: 0x04002948 RID: 10568
	private static readonly Dictionary<GadgetInfo.GadgetCategory, Gadget> _inGameGadgets = new Dictionary<GadgetInfo.GadgetCategory, Gadget>(3, GadgetCategoryComparer.Instance);

	// Token: 0x04002949 RID: 10569
	private static bool _inGameGadgetsInitialized = false;
}
