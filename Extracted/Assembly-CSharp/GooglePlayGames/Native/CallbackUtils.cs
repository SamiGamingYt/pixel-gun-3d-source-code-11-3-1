using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	// Token: 0x020001BF RID: 447
	internal static class CallbackUtils
	{
		// Token: 0x06000E95 RID: 3733 RVA: 0x00046EC8 File Offset: 0x000450C8
		internal static Action<T> ToOnGameThread<T>(Action<T> toConvert)
		{
			if (toConvert == null)
			{
				return delegate(T A_0)
				{
				};
			}
			return delegate(T val)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val);
				});
			};
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x00046F08 File Offset: 0x00045108
		internal static Action<T1, T2> ToOnGameThread<T1, T2>(Action<T1, T2> toConvert)
		{
			if (toConvert == null)
			{
				return delegate(T1 A_0, T2 A_1)
				{
				};
			}
			return delegate(T1 val1, T2 val2)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00046F48 File Offset: 0x00045148
		internal static Action<T1, T2, T3> ToOnGameThread<T1, T2, T3>(Action<T1, T2, T3> toConvert)
		{
			if (toConvert == null)
			{
				return delegate(T1 A_0, T2 A_1, T3 A_2)
				{
				};
			}
			return delegate(T1 val1, T2 val2, T3 val3)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2, val3);
				});
			};
		}
	}
}
