using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GooglePlayGames.Native.Cwrapper
{
	// Token: 0x020001CF RID: 463
	internal static class Event
	{
		// Token: 0x06000ED4 RID: 3796
		[DllImport("gpg")]
		internal static extern ulong Event_Count(HandleRef self);

		// Token: 0x06000ED5 RID: 3797
		[DllImport("gpg")]
		internal static extern UIntPtr Event_Description(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000ED6 RID: 3798
		[DllImport("gpg")]
		internal static extern UIntPtr Event_ImageUrl(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000ED7 RID: 3799
		[DllImport("gpg")]
		internal static extern Types.EventVisibility Event_Visibility(HandleRef self);

		// Token: 0x06000ED8 RID: 3800
		[DllImport("gpg")]
		internal static extern UIntPtr Event_Id(HandleRef self, StringBuilder out_arg, UIntPtr out_size);

		// Token: 0x06000ED9 RID: 3801
		[DllImport("gpg")]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool Event_Valid(HandleRef self);

		// Token: 0x06000EDA RID: 3802
		[DllImport("gpg")]
		internal static extern void Event_Dispose(HandleRef self);

		// Token: 0x06000EDB RID: 3803
		[DllImport("gpg")]
		internal static extern IntPtr Event_Copy(HandleRef self);

		// Token: 0x06000EDC RID: 3804
		[DllImport("gpg")]
		internal static extern UIntPtr Event_Name(HandleRef self, StringBuilder out_arg, UIntPtr out_size);
	}
}
