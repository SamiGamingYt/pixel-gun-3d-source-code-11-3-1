using System;
using Google.Developers;

namespace Com.Google.Android.Gms.Common
{
	// Token: 0x020001B5 RID: 437
	public class ConnectionResult : JavaObjWrapper
	{
		// Token: 0x06000E31 RID: 3633 RVA: 0x0004666C File Offset: 0x0004486C
		public ConnectionResult(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00046678 File Offset: 0x00044878
		public ConnectionResult(int arg_int_1, object arg_object_2, string arg_string_3)
		{
			base.CreateInstance("com/google/android/gms/common/ConnectionResult", new object[]
			{
				arg_int_1,
				arg_object_2,
				arg_string_3
			});
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000466B0 File Offset: 0x000448B0
		public ConnectionResult(int arg_int_1, object arg_object_2)
		{
			base.CreateInstance("com/google/android/gms/common/ConnectionResult", new object[]
			{
				arg_int_1,
				arg_object_2
			});
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000466E4 File Offset: 0x000448E4
		public ConnectionResult(int arg_int_1)
		{
			base.CreateInstance("com/google/android/gms/common/ConnectionResult", new object[]
			{
				arg_int_1
			});
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00046714 File Offset: 0x00044914
		public static int SUCCESS
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SUCCESS");
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00046728 File Offset: 0x00044928
		public static int SERVICE_MISSING
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING");
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x0004673C File Offset: 0x0004493C
		public static int SERVICE_VERSION_UPDATE_REQUIRED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_VERSION_UPDATE_REQUIRED");
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00046750 File Offset: 0x00044950
		public static int SERVICE_DISABLED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_DISABLED");
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00046764 File Offset: 0x00044964
		public static int SIGN_IN_REQUIRED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_REQUIRED");
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00046778 File Offset: 0x00044978
		public static int INVALID_ACCOUNT
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INVALID_ACCOUNT");
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x0004678C File Offset: 0x0004498C
		public static int RESOLUTION_REQUIRED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "RESOLUTION_REQUIRED");
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x000467A0 File Offset: 0x000449A0
		public static int NETWORK_ERROR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "NETWORK_ERROR");
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x000467B4 File Offset: 0x000449B4
		public static int INTERNAL_ERROR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERNAL_ERROR");
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x000467C8 File Offset: 0x000449C8
		public static int SERVICE_INVALID
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_INVALID");
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x000467DC File Offset: 0x000449DC
		public static int DEVELOPER_ERROR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DEVELOPER_ERROR");
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x000467F0 File Offset: 0x000449F0
		public static int LICENSE_CHECK_FAILED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "LICENSE_CHECK_FAILED");
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x00046804 File Offset: 0x00044A04
		public static int CANCELED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CANCELED");
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00046818 File Offset: 0x00044A18
		public static int TIMEOUT
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "TIMEOUT");
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x0004682C File Offset: 0x00044A2C
		public static int INTERRUPTED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERRUPTED");
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00046840 File Offset: 0x00044A40
		public static int API_UNAVAILABLE
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "API_UNAVAILABLE");
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00046854 File Offset: 0x00044A54
		public static int SIGN_IN_FAILED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_FAILED");
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00046868 File Offset: 0x00044A68
		public static int SERVICE_UPDATING
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_UPDATING");
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0004687C File Offset: 0x00044A7C
		public static int SERVICE_MISSING_PERMISSION
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING_PERMISSION");
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00046890 File Offset: 0x00044A90
		public static int DRIVE_EXTERNAL_STORAGE_REQUIRED
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DRIVE_EXTERNAL_STORAGE_REQUIRED");
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x000468A4 File Offset: 0x00044AA4
		public static object CREATOR
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/ConnectionResult", "CREATOR", "Landroid/os/Parcelable$Creator;");
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x000468BC File Offset: 0x00044ABC
		public static string NULL
		{
			get
			{
				return JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/ConnectionResult", "NULL");
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x000468D0 File Offset: 0x00044AD0
		public static int CONTENTS_FILE_DESCRIPTOR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CONTENTS_FILE_DESCRIPTOR");
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x000468E4 File Offset: 0x00044AE4
		public static int PARCELABLE_WRITE_RETURN_VALUE
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "PARCELABLE_WRITE_RETURN_VALUE");
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000468F8 File Offset: 0x00044AF8
		public bool equals(object arg_object_1)
		{
			return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x00046914 File Offset: 0x00044B14
		public string toString()
		{
			return base.InvokeCall<string>("toString", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0004692C File Offset: 0x00044B2C
		public int hashCode()
		{
			return base.InvokeCall<int>("hashCode", "()I", new object[0]);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00046944 File Offset: 0x00044B44
		public int describeContents()
		{
			return base.InvokeCall<int>("describeContents", "()I", new object[0]);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0004695C File Offset: 0x00044B5C
		public object getResolution()
		{
			return base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", new object[0]);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00046974 File Offset: 0x00044B74
		public bool hasResolution()
		{
			return base.InvokeCall<bool>("hasResolution", "()Z", new object[0]);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0004698C File Offset: 0x00044B8C
		public void startResolutionForResult(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", new object[]
			{
				arg_object_1,
				arg_int_2
			});
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x000469B4 File Offset: 0x00044BB4
		public void writeToParcel(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", new object[]
			{
				arg_object_1,
				arg_int_2
			});
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000469DC File Offset: 0x00044BDC
		public int getErrorCode()
		{
			return base.InvokeCall<int>("getErrorCode", "()I", new object[0]);
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x000469F4 File Offset: 0x00044BF4
		public string getErrorMessage()
		{
			return base.InvokeCall<string>("getErrorMessage", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00046A0C File Offset: 0x00044C0C
		public bool isSuccess()
		{
			return base.InvokeCall<bool>("isSuccess", "()Z", new object[0]);
		}

		// Token: 0x04000AAA RID: 2730
		private const string CLASS_NAME = "com/google/android/gms/common/ConnectionResult";
	}
}
