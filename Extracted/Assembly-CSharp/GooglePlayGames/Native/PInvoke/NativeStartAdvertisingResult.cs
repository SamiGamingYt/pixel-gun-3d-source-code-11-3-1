using System;
using System.Runtime.InteropServices;
using System.Text;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200025C RID: 604
	internal class NativeStartAdvertisingResult : BaseReferenceHolder
	{
		// Token: 0x06001348 RID: 4936 RVA: 0x0004F28C File Offset: 0x0004D48C
		internal NativeStartAdvertisingResult(IntPtr pointer) : base(pointer)
		{
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0004F298 File Offset: 0x0004D498
		internal int GetStatus()
		{
			return NearbyConnectionTypes.StartAdvertisingResult_GetStatus(base.SelfPtr());
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0004F2A8 File Offset: 0x0004D4A8
		internal string LocalEndpointName()
		{
			return PInvokeUtilities.OutParamsToString((StringBuilder out_arg, UIntPtr out_size) => NearbyConnectionTypes.StartAdvertisingResult_GetLocalEndpointName(base.SelfPtr(), out_arg, out_size));
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0004F2BC File Offset: 0x0004D4BC
		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionTypes.StartAdvertisingResult_Dispose(selfPointer);
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0004F2C4 File Offset: 0x0004D4C4
		internal AdvertisingResult AsResult()
		{
			return new AdvertisingResult((ResponseStatus)((int)Enum.ToObject(typeof(ResponseStatus), this.GetStatus())), this.LocalEndpointName());
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0004F2F8 File Offset: 0x0004D4F8
		internal static NativeStartAdvertisingResult FromPointer(IntPtr pointer)
		{
			if (pointer == IntPtr.Zero)
			{
				return null;
			}
			return new NativeStartAdvertisingResult(pointer);
		}
	}
}
