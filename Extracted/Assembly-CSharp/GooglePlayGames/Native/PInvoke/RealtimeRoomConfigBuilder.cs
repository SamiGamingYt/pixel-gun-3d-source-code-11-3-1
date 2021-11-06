using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000276 RID: 630
	internal class RealtimeRoomConfigBuilder : BaseReferenceHolder
	{
		// Token: 0x0600142F RID: 5167 RVA: 0x00051184 File Offset: 0x0004F384
		internal RealtimeRoomConfigBuilder(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x00051190 File Offset: 0x0004F390
		internal RealtimeRoomConfigBuilder PopulateFromUIResponse(PlayerSelectUIResponse response)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_PopulateFromPlayerSelectUIResponse(base.SelfPtr(), response.AsPointer());
			return this;
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x000511A4 File Offset: 0x0004F3A4
		internal RealtimeRoomConfigBuilder SetVariant(uint variantValue)
		{
			uint variant = (variantValue != 0U) ? variantValue : uint.MaxValue;
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetVariant(base.SelfPtr(), variant);
			return this;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000511CC File Offset: 0x0004F3CC
		internal RealtimeRoomConfigBuilder AddInvitedPlayer(string playerId)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_AddPlayerToInvite(base.SelfPtr(), playerId);
			return this;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x000511DC File Offset: 0x0004F3DC
		internal RealtimeRoomConfigBuilder SetExclusiveBitMask(ulong bitmask)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetExclusiveBitMask(base.SelfPtr(), bitmask);
			return this;
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x000511EC File Offset: 0x0004F3EC
		internal RealtimeRoomConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMinimumAutomatchingPlayers(base.SelfPtr(), minimum);
			return this;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x000511FC File Offset: 0x0004F3FC
		internal RealtimeRoomConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMaximumAutomatchingPlayers(base.SelfPtr(), maximum);
			return this;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0005120C File Offset: 0x0004F40C
		internal RealtimeRoomConfig Build()
		{
			return new RealtimeRoomConfig(RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Create(base.SelfPtr()));
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00051220 File Offset: 0x0004F420
		protected override void CallDispose(HandleRef selfPointer)
		{
			RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Dispose(selfPointer);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x00051228 File Offset: 0x0004F428
		internal static RealtimeRoomConfigBuilder Create()
		{
			return new RealtimeRoomConfigBuilder(RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Construct());
		}
	}
}
