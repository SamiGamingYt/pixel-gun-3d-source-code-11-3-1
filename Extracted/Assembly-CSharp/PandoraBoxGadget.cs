using System;
using UnityEngine;

// Token: 0x0200062D RID: 1581
public class PandoraBoxGadget : ThrowGadget
{
	// Token: 0x0600369C RID: 13980 RVA: 0x00119CB8 File Offset: 0x00117EB8
	public PandoraBoxGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x00119CC4 File Offset: 0x00117EC4
	public override void PreUse()
	{
		this.StartCooldown();
		WeaponManager.sharedManager.myPlayerMoveC.GrenadePress(this);
		WeaponManager.sharedManager.myPlayerMoveC.GrenadeFire();
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x00119CEC File Offset: 0x00117EEC
	public override void Use()
	{
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x00119CF0 File Offset: 0x00117EF0
	public override void ShowThrowingEffect(float time)
	{
		this.isSuccess = (UnityEngine.Random.Range(0, 2) == 1);
		if (InGameGUI.sharedInGameGUI != null)
		{
			if (this.isSuccess)
			{
				InGameGUI.sharedInGameGUI.pandoraSuccessEffect.Play(time * 2f);
			}
			else
			{
				InGameGUI.sharedInGameGUI.pandoraFailEffect.Play(time * 2f);
			}
		}
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x00119D5C File Offset: 0x00117F5C
	public override void CreateRocket(WeaponSounds weapon)
	{
	}

	// Token: 0x060036A1 RID: 13985 RVA: 0x00119D60 File Offset: 0x00117F60
	public override void ThrowGrenade()
	{
		WeaponManager.sharedManager.myPlayerMoveC.UsePandoraBox(this.Info, this.isSuccess);
	}

	// Token: 0x170008F7 RID: 2295
	// (get) Token: 0x060036A2 RID: 13986 RVA: 0x00119D80 File Offset: 0x00117F80
	public override string GrenadeGadgetId
	{
		get
		{
			return GadgetsInfo.BaseName(this.Info.Id);
		}
	}

	// Token: 0x040027F3 RID: 10227
	private bool isSuccess;
}
