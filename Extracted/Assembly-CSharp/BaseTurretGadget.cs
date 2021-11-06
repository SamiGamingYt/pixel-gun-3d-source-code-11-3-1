using System;

// Token: 0x02000634 RID: 1588
public class BaseTurretGadget : TurretGadget
{
	// Token: 0x060036BD RID: 14013 RVA: 0x0011A10C File Offset: 0x0011830C
	public BaseTurretGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x0011A118 File Offset: 0x00118318
	public override void Use()
	{
		base.Use();
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.ShowTurretInterface(GadgetsInfo.BaseName(this.Info.Id));
		}
	}
}
