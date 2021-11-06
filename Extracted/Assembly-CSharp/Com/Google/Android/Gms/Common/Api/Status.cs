using System;
using Google.Developers;

namespace Com.Google.Android.Gms.Common.Api
{
	// Token: 0x020001B4 RID: 436
	public class Status : JavaObjWrapper, Result
	{
		// Token: 0x06000E1B RID: 3611 RVA: 0x000463FC File Offset: 0x000445FC
		public Status(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00046408 File Offset: 0x00044608
		public Status(int arg_int_1, string arg_string_2, object arg_object_3)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[]
			{
				arg_int_1,
				arg_string_2,
				arg_object_3
			});
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00046440 File Offset: 0x00044640
		public Status(int arg_int_1, string arg_string_2)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[]
			{
				arg_int_1,
				arg_string_2
			});
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00046474 File Offset: 0x00044674
		public Status(int arg_int_1)
		{
			base.CreateInstance("com/google/android/gms/common/api/Status", new object[]
			{
				arg_int_1
			});
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x000464A4 File Offset: 0x000446A4
		public static object CREATOR
		{
			get
			{
				return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/api/Status", "CREATOR", "Landroid/os/Parcelable$Creator;");
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x000464BC File Offset: 0x000446BC
		public static string NULL
		{
			get
			{
				return JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/api/Status", "NULL");
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x000464D0 File Offset: 0x000446D0
		public static int CONTENTS_FILE_DESCRIPTOR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "CONTENTS_FILE_DESCRIPTOR");
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x000464E4 File Offset: 0x000446E4
		public static int PARCELABLE_WRITE_RETURN_VALUE
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/api/Status", "PARCELABLE_WRITE_RETURN_VALUE");
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000464F8 File Offset: 0x000446F8
		public bool equals(object arg_object_1)
		{
			return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", new object[]
			{
				arg_object_1
			});
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00046514 File Offset: 0x00044714
		public string toString()
		{
			return base.InvokeCall<string>("toString", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0004652C File Offset: 0x0004472C
		public int hashCode()
		{
			return base.InvokeCall<int>("hashCode", "()I", new object[0]);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00046544 File Offset: 0x00044744
		public bool isInterrupted()
		{
			return base.InvokeCall<bool>("isInterrupted", "()Z", new object[0]);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0004655C File Offset: 0x0004475C
		public Status getStatus()
		{
			return base.InvokeCall<Status>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00046574 File Offset: 0x00044774
		public bool isCanceled()
		{
			return base.InvokeCall<bool>("isCanceled", "()Z", new object[0]);
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0004658C File Offset: 0x0004478C
		public int describeContents()
		{
			return base.InvokeCall<int>("describeContents", "()I", new object[0]);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x000465A4 File Offset: 0x000447A4
		public object getResolution()
		{
			return base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", new object[0]);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000465BC File Offset: 0x000447BC
		public int getStatusCode()
		{
			return base.InvokeCall<int>("getStatusCode", "()I", new object[0]);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000465D4 File Offset: 0x000447D4
		public string getStatusMessage()
		{
			return base.InvokeCall<string>("getStatusMessage", "()Ljava/lang/String;", new object[0]);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000465EC File Offset: 0x000447EC
		public bool hasResolution()
		{
			return base.InvokeCall<bool>("hasResolution", "()Z", new object[0]);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00046604 File Offset: 0x00044804
		public void startResolutionForResult(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", new object[]
			{
				arg_object_1,
				arg_int_2
			});
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0004662C File Offset: 0x0004482C
		public void writeToParcel(object arg_object_1, int arg_int_2)
		{
			base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", new object[]
			{
				arg_object_1,
				arg_int_2
			});
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00046654 File Offset: 0x00044854
		public bool isSuccess()
		{
			return base.InvokeCall<bool>("isSuccess", "()Z", new object[0]);
		}

		// Token: 0x04000AA9 RID: 2729
		private const string CLASS_NAME = "com/google/android/gms/common/api/Status";
	}
}
