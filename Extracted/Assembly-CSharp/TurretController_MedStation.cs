using System;
using UnityEngine;

// Token: 0x0200086F RID: 2159
public class TurretController_MedStation : TurretController
{
	// Token: 0x06004E10 RID: 19984 RVA: 0x001C510C File Offset: 0x001C330C
	protected override void UpdateTurret()
	{
		base.UpdateTurret();
		if (!this.isRun || this.isKilled)
		{
			return;
		}
		if ((!this.isEnemyTurret || Defs.isCOOP) && WeaponManager.sharedManager.myPlayerMoveC != null && this.nextHealTime < Time.time && (base.transform.position - WeaponManager.sharedManager.myPlayerMoveC.myPlayerTransform.position).sqrMagnitude < this.healRadius * this.healRadius)
		{
			if (WeaponManager.sharedManager.myPlayerMoveC.AddHealth(this.healValue) && InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.medStationEffect.Play();
			}
			if (WeaponManager.sharedManager.myPlayerMoveC.myPetEngine != null && WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.IsAlive)
			{
				WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.UpdateCurrentHealth(WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.CurrentHealth += this.healValue);
			}
			this.nextHealTime = Time.time + this.timeHeal;
		}
	}

	// Token: 0x06004E11 RID: 19985 RVA: 0x001C5264 File Offset: 0x001C3464
	protected override void SetParametersFromGadgets(GadgetInfo info)
	{
		base.SetParametersFromGadgets(info);
		this.healValue = info.HPS;
	}

	// Token: 0x04003CCF RID: 15567
	public float healRadius = 4f;

	// Token: 0x04003CD0 RID: 15568
	public float timeHeal = 1f;

	// Token: 0x04003CD1 RID: 15569
	private float nextHealTime;

	// Token: 0x04003CD2 RID: 15570
	private float healValue = 1f;
}
