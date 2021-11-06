using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200024A RID: 586
	internal class NativeEndpointDetails : BaseReferenceHolder
	{
		// Token: 0x06001299 RID: 4761 RVA: 0x0004E154 File Offset: 0x0004C354
		internal NativeEndpointDetails(IntPtr pointer) : base(pointer)
		{
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0004E160 File Offset: 0x0004C360
		internal string EndpointId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.EndpointDetails_GetEndpointId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0004E174 File Offset: 0x0004C374
		internal string DeviceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.EndpointDetails_GetDeviceId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0004E188 File Offset: 0x0004C388
		internal string Name()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.EndpointDetails_GetName(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0004E19C File Offset: 0x0004C39C
		internal string ServiceId()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.EndpointDetails_GetServiceId(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0004E1B0 File Offset: 0x0004C3B0
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.EndpointDetails_Dispose(selfPointer);
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0004E1B8 File Offset: 0x0004C3B8
		internal EndpointDetails ToDetails()
		{
			return new EndpointDetails(this.EndpointId(), this.DeviceId(), this.Name(), this.ServiceId());
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0004E1E4 File Offset: 0x0004C3E4
		internal static NativeEndpointDetails FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeEndpointDetails(pointer);
		}
	}
}
