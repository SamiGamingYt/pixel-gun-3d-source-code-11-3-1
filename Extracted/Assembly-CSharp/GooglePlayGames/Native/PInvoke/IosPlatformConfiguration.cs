using System;
using System.Runtime.InteropServices;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x0200023F RID: 575
	internal sealed class IosPlatformConfiguration : PlatformConfiguration
	{
		// Token: 0x06001231 RID: 4657 RVA: 0x0004D318 File Offset: 0x0004B518
		private IosPlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0004D324 File Offset: 0x0004B524
		internal void SetClientId(string clientId)
		{
			Misc.CheckNotNull<string>(clientId);
			IosPlatformConfiguration.IosPlatformConfiguration_SetClientID(base.SelfPtr(), clientId);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x0004D33C File Offset: 0x0004B53C
		protected override void CallDispose(HandleRef selfPointer)
		{
			IosPlatformConfiguration.IosPlatformConfiguration_Dispose(selfPointer);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x0004D344 File Offset: 0x0004B544
		internal static IosPlatformConfiguration Create()
		{
			return new IosPlatformConfiguration(IosPlatformConfiguration.IosPlatformConfiguration_Construct());
		}
	}
}
