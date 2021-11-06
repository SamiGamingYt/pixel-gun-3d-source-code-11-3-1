using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000237 RID: 567
	internal static class Callbacks
	{
		// Token: 0x060011EE RID: 4590 RVA: 0x0004C8F8 File Offset: 0x0004AAF8
		internal static IntPtr ToIntPtr<T>(Action<T> callback, Func<IntPtr, T> conversionFunction) where T : BaseReferenceHolder
		{
			Action<IntPtr> callback2 = delegate(IntPtr result)
			{
				using (T t = conversionFunction(result))
				{
					if (callback != null)
					{
						callback(t);
					}
				}
			};
			return Callbacks.ToIntPtr(callback2);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x0004C92C File Offset: 0x0004AB2C
		internal static IntPtr ToIntPtr<T, P>(Action<T, P> callback, Func<IntPtr, P> conversionFunction) where P : BaseReferenceHolder
		{
			Action<T, IntPtr> callback2 = delegate(T param1, IntPtr param2)
			{
				using (P p = conversionFunction(param2))
				{
					if (callback != null)
					{
						callback(param1, p);
					}
				}
			};
			return Callbacks.ToIntPtr(callback2);
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x0004C960 File Offset: 0x0004AB60
		internal static IntPtr ToIntPtr(Delegate callback)
		{
			if (callback == null)
			{
				return IntPtr.Zero;
			}
			GCHandle value = GCHandle.Alloc(callback);
			return GCHandle.ToIntPtr(value);
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0004C988 File Offset: 0x0004AB88
		internal static T IntPtrToTempCallback<T>(IntPtr handle) where T : class
		{
			return Callbacks.IntPtrToCallback<T>(handle, true);
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0004C994 File Offset: 0x0004AB94
		private static T IntPtrToCallback<T>(IntPtr handle, bool unpinHandle) where T : class
		{
			if (PInvokeUtilities.IsNull(handle))
			{
				return (T)((object)null);
			}
			GCHandle gchandle = GCHandle.FromIntPtr(handle);
			T result;
			try
			{
				result = (T)((object)gchandle.Target);
			}
			catch (InvalidCastException ex)
			{
				Logger.e(string.Concat(new object[]
				{
					"GC Handle pointed to unexpected type: ",
					gchandle.Target.ToString(),
					". Expected ",
					typeof(T)
				}));
				throw ex;
			}
			finally
			{
				if (unpinHandle)
				{
					gchandle.Free();
				}
			}
			return result;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0004CA58 File Offset: 0x0004AC58
		internal static T IntPtrToPermanentCallback<T>(IntPtr handle) where T : class
		{
			return Callbacks.IntPtrToCallback<T>(handle, false);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0004CA64 File Offset: 0x0004AC64
		[MonoPInvokeCallback(typeof(Callbacks.ShowUICallbackInternal))]
		internal static void InternalShowUICallback(CommonErrorStatus.UIStatus status, IntPtr data)
		{
			Logger.d("Showing UI Internal callback: " + status);
			Action<CommonErrorStatus.UIStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.UIStatus>>(data);
			try
			{
				action(status);
			}
			catch (Exception arg)
			{
				Logger.e("Error encountered executing InternalShowAllUICallback. Smothering to avoid passing exception into Native: " + arg);
			}
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0004CACC File Offset: 0x0004ACCC
		internal static void PerformInternalCallback(string callbackName, Callbacks.Type callbackType, IntPtr response, IntPtr userData)
		{
			Logger.d("Entering internal callback for " + callbackName);
			Action<IntPtr> action = (callbackType != Callbacks.Type.Permanent) ? Callbacks.IntPtrToTempCallback<Action<IntPtr>>(userData) : Callbacks.IntPtrToPermanentCallback<Action<IntPtr>>(userData);
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (Exception ex)
			{
				Logger.e(string.Concat(new object[]
				{
					"Error encountered executing ",
					callbackName,
					". Smothering to avoid passing exception into Native: ",
					ex
				}));
			}
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x0004CB60 File Offset: 0x0004AD60
		internal static void PerformInternalCallback<T>(string callbackName, Callbacks.Type callbackType, T param1, IntPtr param2, IntPtr userData)
		{
			Logger.d("Entering internal callback for " + callbackName);
			Action<T, IntPtr> action = null;
			try
			{
				action = ((callbackType != Callbacks.Type.Permanent) ? Callbacks.IntPtrToTempCallback<Action<T, IntPtr>>(userData) : Callbacks.IntPtrToPermanentCallback<Action<T, IntPtr>>(userData));
			}
			catch (Exception ex)
			{
				Logger.e(string.Concat(new object[]
				{
					"Error encountered converting ",
					callbackName,
					". Smothering to avoid passing exception into Native: ",
					ex
				}));
				return;
			}
			Logger.d("Internal Callback converted to action");
			if (action == null)
			{
				return;
			}
			try
			{
				action(param1, param2);
			}
			catch (Exception ex2)
			{
				Logger.e(string.Concat(new object[]
				{
					"Error encountered executing ",
					callbackName,
					". Smothering to avoid passing exception into Native: ",
					ex2
				}));
			}
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0004CC54 File Offset: 0x0004AE54
		internal static Action<T> AsOnGameThreadCallback<T>(Action<T> toInvokeOnGameThread)
		{
			return delegate(T result)
			{
				if (toInvokeOnGameThread == null)
				{
					return;
				}
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toInvokeOnGameThread(result);
				});
			};
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0004CC7C File Offset: 0x0004AE7C
		internal static Action<T1, T2> AsOnGameThreadCallback<T1, T2>(Action<T1, T2> toInvokeOnGameThread)
		{
			return delegate(T1 result1, T2 result2)
			{
				if (toInvokeOnGameThread == null)
				{
					return;
				}
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toInvokeOnGameThread(result1, result2);
				});
			};
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0004CCA4 File Offset: 0x0004AEA4
		internal static void AsCoroutine(IEnumerator routine)
		{
			PlayGamesHelperObject.RunCoroutine(routine);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0004CCAC File Offset: 0x0004AEAC
		internal static byte[] IntPtrAndSizeToByteArray(IntPtr data, UIntPtr dataLength)
		{
			if (dataLength.ToUInt64() == 0UL)
			{
				return null;
			}
			byte[] array = new byte[dataLength.ToUInt32()];
			Marshal.Copy(data, array, 0, (int)dataLength.ToUInt32());
			return array;
		}

		// Token: 0x04000BF9 RID: 3065
		internal static readonly Action<CommonErrorStatus.UIStatus> NoopUICallback = delegate(CommonErrorStatus.UIStatus status)
		{
			Logger.d("Received UI callback: " + status);
		};

		// Token: 0x02000238 RID: 568
		internal enum Type
		{
			// Token: 0x04000BFC RID: 3068
			Permanent,
			// Token: 0x04000BFD RID: 3069
			Temporary
		}

		// Token: 0x020008DE RID: 2270
		// (Invoke) Token: 0x06005018 RID: 20504
		internal delegate void ShowUICallbackInternal(CommonErrorStatus.UIStatus status, IntPtr data);
	}
}
