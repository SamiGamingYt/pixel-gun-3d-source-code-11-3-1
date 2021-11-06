using System;

// Token: 0x02000633 RID: 1587
public class TurretGadget : ImmediateGadget
{
	// Token: 0x060036B7 RID: 14007 RVA: 0x0011A070 File Offset: 0x00118270
	public TurretGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x170008F9 RID: 2297
	// (get) Token: 0x060036B8 RID: 14008 RVA: 0x0011A07C File Offset: 0x0011827C
	public override bool CanUse
	{
		get
		{
			return this._cooldownTime.value == 0f;
		}
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x0011A090 File Offset: 0x00118290
	public override void Use()
	{
		if (this.currentTurret != null)
		{
			this.currentTurret.SendImKilledRPC();
			this.currentTurret = null;
		}
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x0011A0B8 File Offset: 0x001182B8
	public override void PreUse()
	{
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x0011A0BC File Offset: 0x001182BC
	public override void OnTimeExpire()
	{
		if (this.currentTurret != null)
		{
			this.currentTurret.SendImKilledRPC();
			this.currentTurret = null;
		}
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x0011A0E4 File Offset: 0x001182E4
	public void StartedCurrentTurret(TurretController _curTurret)
	{
		_curTurret.GadgetOnKill = new Action(base.ResetUseCounter);
		this.StartUseTimer();
		this.StartCooldown();
		this.currentTurret = _curTurret;
	}

	// Token: 0x040027F7 RID: 10231
	private TurretController currentTurret;
}
