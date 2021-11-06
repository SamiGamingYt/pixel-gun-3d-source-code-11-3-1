using System;
using Google.Developers;

namespace Com.Google.Android.Gms.Games.Stats
{
	// Token: 0x020001BA RID: 442
	public class PlayerStatsObject : JavaObjWrapper, PlayerStats
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x00046D4C File Offset: 0x00044F4C
		public PlayerStatsObject(IntPtr ptr) : base(ptr)
		{
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x00046D58 File Offset: 0x00044F58
		public static float UNSET_VALUE
		{
			get
			{
				return JavaObjWrapper.GetStaticFloatField("com/google/android/gms/games/stats/PlayerStats", "UNSET_VALUE");
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x00046D6C File Offset: 0x00044F6C
		public static int CONTENTS_FILE_DESCRIPTOR
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "CONTENTS_FILE_DESCRIPTOR");
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00046D80 File Offset: 0x00044F80
		public static int PARCELABLE_WRITE_RETURN_VALUE
		{
			get
			{
				return JavaObjWrapper.GetStaticIntField("com/google/android/gms/games/stats/PlayerStats", "PARCELABLE_WRITE_RETURN_VALUE");
			}
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00046D94 File Offset: 0x00044F94
		public float getAverageSessionLength()
		{
			return base.InvokeCall<float>("getAverageSessionLength", "()F", new object[0]);
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00046DAC File Offset: 0x00044FAC
		public float getChurnProbability()
		{
			return base.InvokeCall<float>("getChurnProbability", "()F", new object[0]);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00046DC4 File Offset: 0x00044FC4
		public int getDaysSinceLastPlayed()
		{
			return base.InvokeCall<int>("getDaysSinceLastPlayed", "()I", new object[0]);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00046DDC File Offset: 0x00044FDC
		public int getNumberOfPurchases()
		{
			return base.InvokeCall<int>("getNumberOfPurchases", "()I", new object[0]);
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00046DF4 File Offset: 0x00044FF4
		public int getNumberOfSessions()
		{
			return base.InvokeCall<int>("getNumberOfSessions", "()I", new object[0]);
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00046E0C File Offset: 0x0004500C
		public float getSessionPercentile()
		{
			return base.InvokeCall<float>("getSessionPercentile", "()F", new object[0]);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00046E24 File Offset: 0x00045024
		public float getSpendPercentile()
		{
			return base.InvokeCall<float>("getSpendPercentile", "()F", new object[0]);
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00046E3C File Offset: 0x0004503C
		public float getSpendProbability()
		{
			return base.InvokeCall<float>("getSpendProbability", "()F", new object[0]);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00046E54 File Offset: 0x00045054
		public float getHighSpenderProbability()
		{
			return base.InvokeCall<float>("getHighSpenderProbability", "()F", new object[0]);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00046E6C File Offset: 0x0004506C
		public float getTotalSpendNext28Days()
		{
			return base.InvokeCall<float>("getTotalSpendNext28Days", "()F", new object[0]);
		}

		// Token: 0x04000AAE RID: 2734
		private const string CLASS_NAME = "com/google/android/gms/games/stats/PlayerStats";
	}
}
