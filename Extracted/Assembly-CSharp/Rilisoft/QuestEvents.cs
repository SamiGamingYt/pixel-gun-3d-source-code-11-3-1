using System;
using Rilisoft.NullExtensions;

namespace Rilisoft
{
	// Token: 0x0200072A RID: 1834
	public class QuestEvents
	{
		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06003FF4 RID: 16372 RVA: 0x001562F4 File Offset: 0x001544F4
		// (remove) Token: 0x06003FF5 RID: 16373 RVA: 0x00156310 File Offset: 0x00154510
		public event EventHandler<WinEventArgs> Win;

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06003FF6 RID: 16374 RVA: 0x0015632C File Offset: 0x0015452C
		// (remove) Token: 0x06003FF7 RID: 16375 RVA: 0x00156348 File Offset: 0x00154548
		public event EventHandler<KillOtherPlayerEventArgs> KillOtherPlayer;

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x06003FF8 RID: 16376 RVA: 0x00156364 File Offset: 0x00154564
		// (remove) Token: 0x06003FF9 RID: 16377 RVA: 0x00156380 File Offset: 0x00154580
		public event EventHandler KillOtherPlayerWithFlag;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06003FFA RID: 16378 RVA: 0x0015639C File Offset: 0x0015459C
		// (remove) Token: 0x06003FFB RID: 16379 RVA: 0x001563B8 File Offset: 0x001545B8
		public event EventHandler<CaptureEventArgs> Capture;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06003FFC RID: 16380 RVA: 0x001563D4 File Offset: 0x001545D4
		// (remove) Token: 0x06003FFD RID: 16381 RVA: 0x001563F0 File Offset: 0x001545F0
		public event EventHandler<KillMonsterEventArgs> KillMonster;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06003FFE RID: 16382 RVA: 0x0015640C File Offset: 0x0015460C
		// (remove) Token: 0x06003FFF RID: 16383 RVA: 0x00156428 File Offset: 0x00154628
		public event EventHandler BreakSeries;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06004000 RID: 16384 RVA: 0x00156444 File Offset: 0x00154644
		// (remove) Token: 0x06004001 RID: 16385 RVA: 0x00156460 File Offset: 0x00154660
		public event EventHandler MakeSeries;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06004002 RID: 16386 RVA: 0x0015647C File Offset: 0x0015467C
		// (remove) Token: 0x06004003 RID: 16387 RVA: 0x00156498 File Offset: 0x00154698
		public event EventHandler<SurviveWaveInArenaEventArgs> SurviveWaveInArena;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06004004 RID: 16388 RVA: 0x001564B4 File Offset: 0x001546B4
		// (remove) Token: 0x06004005 RID: 16389 RVA: 0x001564D0 File Offset: 0x001546D0
		public event EventHandler GetGotcha;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06004006 RID: 16390 RVA: 0x001564EC File Offset: 0x001546EC
		// (remove) Token: 0x06004007 RID: 16391 RVA: 0x00156508 File Offset: 0x00154708
		public event EventHandler<SocialInteractionEventArgs> SocialInteraction;

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06004008 RID: 16392 RVA: 0x00156524 File Offset: 0x00154724
		// (remove) Token: 0x06004009 RID: 16393 RVA: 0x00156540 File Offset: 0x00154740
		public event EventHandler Jump;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x0600400A RID: 16394 RVA: 0x0015655C File Offset: 0x0015475C
		// (remove) Token: 0x0600400B RID: 16395 RVA: 0x00156578 File Offset: 0x00154778
		public event EventHandler TurretKill;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x0600400C RID: 16396 RVA: 0x00156594 File Offset: 0x00154794
		// (remove) Token: 0x0600400D RID: 16397 RVA: 0x001565B0 File Offset: 0x001547B0
		public event EventHandler<KillOtherPlayerOnFlyEventArgs> KillOtherPlayerOnFly;

		// Token: 0x0600400E RID: 16398 RVA: 0x001565CC File Offset: 0x001547CC
		protected void RaiseWin(WinEventArgs e)
		{
			this.Win.Do(delegate(EventHandler<WinEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x00156608 File Offset: 0x00154808
		protected void RaiseKillOtherPlayer(KillOtherPlayerEventArgs e)
		{
			this.KillOtherPlayer.Do(delegate(EventHandler<KillOtherPlayerEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00156644 File Offset: 0x00154844
		protected void RaiseKillOtherPlayerWithFlag(EventArgs e)
		{
			this.KillOtherPlayerWithFlag.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x00156680 File Offset: 0x00154880
		protected void RaiseCapture(CaptureEventArgs e)
		{
			this.Capture.Do(delegate(EventHandler<CaptureEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x001566BC File Offset: 0x001548BC
		protected void RaiseKillMonster(KillMonsterEventArgs e)
		{
			this.KillMonster.Do(delegate(EventHandler<KillMonsterEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x001566F8 File Offset: 0x001548F8
		protected void RaiseBreakSeries(EventArgs e)
		{
			this.BreakSeries.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x00156734 File Offset: 0x00154934
		protected void RaiseMakeSeries(EventArgs e)
		{
			this.MakeSeries.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x00156770 File Offset: 0x00154970
		protected void RaiseSurviveWaveInArena(SurviveWaveInArenaEventArgs e)
		{
			this.SurviveWaveInArena.Do(delegate(EventHandler<SurviveWaveInArenaEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x001567AC File Offset: 0x001549AC
		protected void RaiseGetGotcha(EventArgs e)
		{
			this.GetGotcha.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x001567E8 File Offset: 0x001549E8
		protected void RaiseSocialInteraction(SocialInteractionEventArgs e)
		{
			this.SocialInteraction.Do(delegate(EventHandler<SocialInteractionEventArgs> handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x00156824 File Offset: 0x00154A24
		protected void RaiseJump(EventArgs e)
		{
			this.Jump.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x00156860 File Offset: 0x00154A60
		protected void RaiseTurretKill(EventArgs e)
		{
			this.TurretKill.Do(delegate(EventHandler handler)
			{
				handler(this, e);
			});
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x0015689C File Offset: 0x00154A9C
		protected void RaiseKillOtherPlayerOnFly(KillOtherPlayerOnFlyEventArgs e)
		{
			this.KillOtherPlayerOnFly.Do(delegate(EventHandler<KillOtherPlayerOnFlyEventArgs> handler)
			{
				handler(this, e);
			});
		}
	}
}
