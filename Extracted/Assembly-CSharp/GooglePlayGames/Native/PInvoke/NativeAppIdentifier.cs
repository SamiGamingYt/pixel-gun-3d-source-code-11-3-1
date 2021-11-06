using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000247 RID: 583
	internal class NativeAppIdentifier : BaseReferenceHolder
	{
		// Token: 0x0600127E RID: 4734 RVA: 0x0004DEE4 File Offset: 0x0004C0E4
		internal NativeAppIdentifier(IntPtr pointer) : base(pointer)
		{
		}

		// Token: 0x0600127F RID: 4735
		[DllImport("gpg")]
		internal static extern IntPtr NearbyUtils_ConstructAppIdentifier(string appId);

		// Token: 0x06001280 RID: 4736 RVA: 0x0004DEF0 File Offset: 0x0004C0F0
		internal string Id()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.AppIdentifier_GetIdentifier(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0004DF04 File Offset: 0x0004C104
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.AppIdentifier_Dispose(selfPointer);
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x0004DF0C File Offset: 0x0004C10C
		internal static NativeAppIdentifier FromString(string appId)
		{
			return new NativeAppIdentifier(NativeAppIdentifier.NearbyUtils_ConstructAppIdentifier(appId));
		}
	}
}
