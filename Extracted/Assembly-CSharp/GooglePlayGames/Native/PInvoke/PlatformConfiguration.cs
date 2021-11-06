using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000262 RID: 610
	internal abstract class PlatformConfiguration : BaseReferenceHolder
	{
		// Token: 0x0600139A RID: 5018 RVA: 0x0004FE98 File Offset: 0x0004E098
		protected PlatformConfiguration(IntPtr selfPointer) : base(selfPointer)
		{
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0004FEA4 File Offset: 0x0004E0A4
		internal HandleRef AsHandle()
		{
			return base.SelfPtr();
		}
	}
}
