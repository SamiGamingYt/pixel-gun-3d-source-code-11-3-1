using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000283 RID: 643
	internal class TurnBasedMatchConfig : BaseReferenceHolder
	{
		// Token: 0x060014A2 RID: 5282 RVA: 0x00051F08 File Offset: 0x00050108
		internal TurnBasedMatchConfig(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00051F14 File Offset: 0x00050114
		private string PlayerIdAtIndex(UIntPtr index)
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(this.SelfPtr(), index, out_string, size));
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00051F48 File Offset: 0x00050148
		internal IEnumerator<string> PlayerIdsToInvite()
		{
			return PInvokeUtilities.ToEnumerator<string>(TurnBasedMatchConfig.TurnBasedMatchConfig_PlayerIdsToInvite_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00051F68 File Offset: 0x00050168
		internal uint Variant()
		{
			return TurnBasedMatchConfig.TurnBasedMatchConfig_Variant(base.SelfPtr());
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00051F78 File Offset: 0x00050178
		internal long ExclusiveBitMask()
		{
			return TurnBasedMatchConfig.TurnBasedMatchConfig_ExclusiveBitMask(base.SelfPtr());
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00051F88 File Offset: 0x00050188
		internal uint MinimumAutomatchingPlayers()
		{
			return TurnBasedMatchConfig.TurnBasedMatchConfig_MinimumAutomatchingPlayers(base.SelfPtr());
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00051F98 File Offset: 0x00050198
		internal uint MaximumAutomatchingPlayers()
		{
			return TurnBasedMatchConfig.TurnBasedMatchConfig_MaximumAutomatchingPlayers(base.SelfPtr());
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00051FA8 File Offset: 0x000501A8
		protected override void CallDispose(HandleRef selfPointer)
		{
			TurnBasedMatchConfig.TurnBasedMatchConfig_Dispose(selfPointer);
		}
	}
}
