using System;

// Token: 0x02000629 RID: 1577
public abstract class ThrowGadget : Gadget
{
	// Token: 0x06003686 RID: 13958 RVA: 0x00119874 File Offset: 0x00117A74
	public ThrowGadget(GadgetInfo _info) : base(_info)
	{
	}

	// Token: 0x06003687 RID: 13959 RVA: 0x00119880 File Offset: 0x00117A80
	public void SetCurrentRocket(Rocket rocket)
	{
		this.currentRocket = rocket;
		rocket.OnExplode = new Action(this.ClearCurrentRocket);
	}

	// Token: 0x06003688 RID: 13960 RVA: 0x0011989C File Offset: 0x00117A9C
	protected void KillCurrentRocket()
	{
		if (this.currentRocket == null)
		{
			return;
		}
		this.currentRocket.KillRocket();
	}

	// Token: 0x06003689 RID: 13961 RVA: 0x001198BC File Offset: 0x00117ABC
	private void ClearCurrentRocket()
	{
		this.currentRocket = null;
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x001198C8 File Offset: 0x00117AC8
	public override void OnMatchEnd()
	{
		this.KillCurrentRocket();
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x001198D0 File Offset: 0x00117AD0
	public virtual void ShowThrowingEffect(float time)
	{
	}

	// Token: 0x0600368C RID: 13964
	public abstract void CreateRocket(WeaponSounds weapon);

	// Token: 0x0600368D RID: 13965
	public abstract void ThrowGrenade();

	// Token: 0x170008F4 RID: 2292
	// (get) Token: 0x0600368E RID: 13966
	public abstract string GrenadeGadgetId { get; }

	// Token: 0x040027F0 RID: 10224
	private Rocket currentRocket;
}
