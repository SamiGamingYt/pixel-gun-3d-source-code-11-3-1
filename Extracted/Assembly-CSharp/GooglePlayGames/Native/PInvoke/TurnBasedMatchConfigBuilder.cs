using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000284 RID: 644
	internal class TurnBasedMatchConfigBuilder : BaseReferenceHolder
	{
		// Token: 0x060014AA RID: 5290 RVA: 0x00051FB0 File Offset: 0x000501B0
		private TurnBasedMatchConfigBuilder(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00051FBC File Offset: 0x000501BC
		internal TurnBasedMatchConfigBuilder PopulateFromUIResponse(PlayerSelectUIResponse response)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(base.SelfPtr(), response.AsPointer());
			return this;
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x00051FD0 File Offset: 0x000501D0
		internal TurnBasedMatchConfigBuilder SetVariant(uint variant)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetVariant(base.SelfPtr(), variant);
			return this;
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00051FE0 File Offset: 0x000501E0
		internal TurnBasedMatchConfigBuilder AddInvitedPlayer(string playerId)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_AddPlayerToInvite(base.SelfPtr(), playerId);
			return this;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00051FF0 File Offset: 0x000501F0
		internal TurnBasedMatchConfigBuilder SetExclusiveBitMask(ulong bitmask)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetExclusiveBitMask(base.SelfPtr(), bitmask);
			return this;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x00052000 File Offset: 0x00050200
		internal TurnBasedMatchConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(base.SelfPtr(), minimum);
			return this;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00052010 File Offset: 0x00050210
		internal TurnBasedMatchConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(base.SelfPtr(), maximum);
			return this;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x00052020 File Offset: 0x00050220
		internal TurnBasedMatchConfig Build()
		{
			return new TurnBasedMatchConfig(TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Create(base.SelfPtr()));
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00052034 File Offset: 0x00050234
		protected override void CallDispose(HandleRef selfPointer)
		{
			TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Dispose(selfPointer);
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x0005203C File Offset: 0x0005023C
		internal static TurnBasedMatchConfigBuilder Create()
		{
			return new TurnBasedMatchConfigBuilder(TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Construct());
		}
	}
}
