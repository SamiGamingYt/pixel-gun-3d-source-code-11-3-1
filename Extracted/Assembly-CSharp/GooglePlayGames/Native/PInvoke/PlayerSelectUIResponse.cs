using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000268 RID: 616
	internal class PlayerSelectUIResponse : BaseReferenceHolder, IEnumerable, IEnumerable<string>
	{
		// Token: 0x060013B9 RID: 5049 RVA: 0x000502C4 File Offset: 0x0004E4C4
		internal PlayerSelectUIResponse(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x000502D0 File Offset: 0x0004E4D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000502D8 File Offset: 0x0004E4D8
		internal CommonErrorStatus.UIStatus Status()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetStatus(base.SelfPtr());
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000502E8 File Offset: 0x0004E4E8
		private string PlayerIdAtIndex(UIntPtr index)
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr size) => TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_GetElement(this.SelfPtr(), index, out_string, size));
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0005031C File Offset: 0x0004E51C
		public IEnumerator<string> GetEnumerator()
		{
			return PInvokeUtilities.ToEnumerator<string>(TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetPlayerIds_Length(base.SelfPtr()), new Func<UIntPtr, string>(this.PlayerIdAtIndex));
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x0005033C File Offset: 0x0004E53C
		internal uint MinimumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMinimumAutomatchingPlayers(base.SelfPtr());
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0005034C File Offset: 0x0004E54C
		internal uint MaximumAutomatchingPlayers()
		{
			return TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_GetMaximumAutomatchingPlayers(base.SelfPtr());
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005035C File Offset: 0x0004E55C
		protected override void CallDispose(HandleRef selfPointer)
		{
			TurnBasedMultiplayerManager.TurnBasedMultiplayerManager_PlayerSelectUIResponse_Dispose(selfPointer);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00050364 File Offset: 0x0004E564
		internal static PlayerSelectUIResponse FromPointer(IntPtr pointer)
		{
			if (PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new PlayerSelectUIResponse(pointer);
		}
	}
}
