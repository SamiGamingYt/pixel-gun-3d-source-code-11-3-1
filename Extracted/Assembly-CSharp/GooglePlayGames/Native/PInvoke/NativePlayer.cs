using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200024F RID: 591
	internal class NativePlayer : BaseReferenceHolder
	{
		// Token: 0x060012C3 RID: 4803 RVA: 0x0004E5FC File Offset: 0x0004C7FC
		internal NativePlayer(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0004E608 File Offset: 0x0004C808
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_Id(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0004E61C File Offset: 0x0004C81C
		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_Name(base.SelfPtr(), out_string, out_size));
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0004E630 File Offset: 0x0004C830
		internal string AvatarURL()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_string, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.Player.Player_AvatarUrl(base.SelfPtr(), Types.ImageResolution.ICON, out_string, out_size));
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0004E644 File Offset: 0x0004C844
		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.Player.Player_Dispose(selfPointer);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0004E64C File Offset: 0x0004C84C
		internal GooglePlayGames.BasicApi.Multiplayer.Player AsPlayer()
		{
			return new GooglePlayGames.BasicApi.Multiplayer.Player(this.Name(), this.Id(), this.AvatarURL());
		}
	}
}
