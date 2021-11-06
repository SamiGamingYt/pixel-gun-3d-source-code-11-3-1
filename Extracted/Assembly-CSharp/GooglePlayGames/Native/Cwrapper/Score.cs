using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001EB RID: 491
	internal static class Score
	{
		// Token: 0x06000FF4 RID: 4084
		[DllImport("gpg")]
		internal static extern ulong Score_Value(HandleRef self);

		// Token: 0x06000FF5 RID: 4085
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Score_Valid(HandleRef self);

		// Token: 0x06000FF6 RID: 4086
		[DllImport("gpg")]
		internal static extern ulong Score_Rank(HandleRef self);

		// Token: 0x06000FF7 RID: 4087
		[DllImport("gpg")]
		internal static extern void Score_Dispose(HandleRef self);

		// Token: 0x06000FF8 RID: 4088
		[DllImport("gpg")]
		internal static extern UIntPtr Score_Metadata(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
