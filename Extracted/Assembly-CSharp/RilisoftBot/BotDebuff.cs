using System;

namespace RilisoftBot
{
	// Token: 0x02000583 RID: 1411
	public class BotDebuff
	{
		// Token: 0x06003123 RID: 12579 RVA: 0x001001F0 File Offset: 0x000FE3F0
		public BotDebuff(BotDebuffType typeDebuff, float timeLifeDebuff, object parametrsDebuff)
		{
			this.type = typeDebuff;
			this.timeLife = timeLifeDebuff;
			this.parametrs = parametrsDebuff;
			this.isRun = false;
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06003124 RID: 12580 RVA: 0x00100220 File Offset: 0x000FE420
		// (remove) Token: 0x06003125 RID: 12581 RVA: 0x0010023C File Offset: 0x000FE43C
		public event BotDebuff.OnRunDelegate OnRun;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06003126 RID: 12582 RVA: 0x00100258 File Offset: 0x000FE458
		// (remove) Token: 0x06003127 RID: 12583 RVA: 0x00100274 File Offset: 0x000FE474
		public event BotDebuff.OnStopDelegate OnStop;

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x0010029C File Offset: 0x000FE49C
		// (set) Token: 0x06003128 RID: 12584 RVA: 0x00100290 File Offset: 0x000FE490
		public BotDebuffType type { get; private set; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x001002B0 File Offset: 0x000FE4B0
		// (set) Token: 0x0600312A RID: 12586 RVA: 0x001002A4 File Offset: 0x000FE4A4
		public float timeLife { get; set; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x001002C4 File Offset: 0x000FE4C4
		// (set) Token: 0x0600312C RID: 12588 RVA: 0x001002B8 File Offset: 0x000FE4B8
		public object parametrs { get; private set; }

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001002D8 File Offset: 0x000FE4D8
		// (set) Token: 0x0600312E RID: 12590 RVA: 0x001002CC File Offset: 0x000FE4CC
		public bool isRun { get; private set; }

		// Token: 0x06003130 RID: 12592 RVA: 0x001002E0 File Offset: 0x000FE4E0
		public float GetFloatParametr()
		{
			if (this.parametrs == null)
			{
				return 0f;
			}
			return (float)this.parametrs;
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x00100300 File Offset: 0x000FE500
		public void Run()
		{
			if (this.OnRun == null)
			{
				return;
			}
			this.isRun = true;
			this.OnRun(this);
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x00100324 File Offset: 0x000FE524
		public void Stop()
		{
			if (this.OnStop == null)
			{
				return;
			}
			this.isRun = false;
			this.OnStop(this);
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x00100348 File Offset: 0x000FE548
		public void ReplaceValues(float newTimeLife, object newParametrs)
		{
			this.timeLife = newTimeLife;
			this.parametrs = newParametrs;
		}

		// Token: 0x0200091B RID: 2331
		// (Invoke) Token: 0x0600510C RID: 20748
		public delegate void OnRunDelegate(BotDebuff debuff);

		// Token: 0x0200091C RID: 2332
		// (Invoke) Token: 0x06005110 RID: 20752
		public delegate void OnStopDelegate(BotDebuff debuff);
	}
}
