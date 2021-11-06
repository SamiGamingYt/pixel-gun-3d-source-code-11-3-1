using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Google.Developers
{
	// Token: 0x020001AE RID: 430
	public class JavaObjWrapper
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x000457E8 File Offset: 0x000439E8
		protected JavaObjWrapper()
		{
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000457FC File Offset: 0x000439FC
		public JavaObjWrapper(string clazzName)
		{
			this.raw = AndroidJNI.AllocObject(AndroidJNI.FindClass(clazzName));
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0004582C File Offset: 0x00043A2C
		public JavaObjWrapper(IntPtr rawObject)
		{
			this.raw = rawObject;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00045848 File Offset: 0x00043A48
		public IntPtr RawObject
		{
			get
			{
				return this.raw;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000DE9 RID: 3561 RVA: 0x00045850 File Offset: 0x00043A50
		public virtual IntPtr RawClass
		{
			get
			{
				if (this.cachedRawClass == IntPtr.Zero && this.raw != IntPtr.Zero)
				{
					this.cachedRawClass = AndroidJNI.GetObjectClass(this.raw);
				}
				return this.cachedRawClass;
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000458A0 File Offset: 0x00043AA0
		public void CreateInstance(string clazzName, params object[] args)
		{
			if (this.raw != IntPtr.Zero)
			{
				throw new Exception("Java object already set");
			}
			IntPtr constructorID = AndroidJNIHelper.GetConstructorID(this.RawClass, args);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			this.raw = AndroidJNI.NewObject(this.RawClass, constructorID, args2);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000458F4 File Offset: 0x00043AF4
		protected static jvalue[] ConstructArgArray(object[] theArgs)
		{
			object[] array = new object[theArgs.Length];
			for (int i = 0; i < theArgs.Length; i++)
			{
				if (theArgs[i] is JavaObjWrapper)
				{
					array[i] = ((JavaObjWrapper)theArgs[i]).raw;
				}
				else
				{
					array[i] = theArgs[i];
				}
			}
			jvalue[] array2 = AndroidJNIHelper.CreateJNIArgArray(array);
			for (int j = 0; j < theArgs.Length; j++)
			{
				if (theArgs[j] is JavaObjWrapper)
				{
					array2[j].l = ((JavaObjWrapper)theArgs[j]).raw;
				}
				else if (theArgs[j] is JavaInterfaceProxy)
				{
					IntPtr l = AndroidJNIHelper.CreateJavaProxy((AndroidJavaProxy)theArgs[j]);
					array2[j].l = l;
				}
			}
			if (array2.Length == 1)
			{
				for (int k = 0; k < array2.Length; k++)
				{
					Debug.Log(string.Concat(new object[]
					{
						"---- [",
						k,
						"] -- ",
						array2[k].l
					}));
				}
			}
			return array2;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00045A1C File Offset: 0x00043C1C
		public static T StaticInvokeObjectCall<T>(string type, string name, string sig, params object[] args)
		{
			IntPtr clazz = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			IntPtr intPtr = AndroidJNI.CallStaticObjectMethod(clazz, staticMethodID, args2);
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[]
			{
				intPtr.GetType()
			});
			if (constructor != null)
			{
				return (T)((object)constructor.Invoke(new object[]
				{
					intPtr
				}));
			}
			if (typeof(T).IsArray)
			{
				return AndroidJNIHelper.ConvertFromJNIArray<T>(intPtr);
			}
			Debug.Log("Trying cast....");
			Type typeFromHandle = typeof(T);
			return (T)((object)Marshal.PtrToStructure(intPtr, typeFromHandle));
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00045AD0 File Offset: 0x00043CD0
		public static void StaticInvokeCallVoid(string type, string name, string sig, params object[] args)
		{
			IntPtr clazz = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			AndroidJNI.CallStaticVoidMethod(clazz, staticMethodID, args2);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00045AFC File Offset: 0x00043CFC
		public static T GetStaticObjectField<T>(string clsName, string name, string sig)
		{
			IntPtr clazz = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, name, sig);
			IntPtr staticObjectField = AndroidJNI.GetStaticObjectField(clazz, staticFieldID);
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[]
			{
				staticObjectField.GetType()
			});
			if (constructor != null)
			{
				return (T)((object)constructor.Invoke(new object[]
				{
					staticObjectField
				}));
			}
			Type typeFromHandle = typeof(T);
			return (T)((object)Marshal.PtrToStructure(staticObjectField, typeFromHandle));
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x00045B80 File Offset: 0x00043D80
		public static int GetStaticIntField(string clsName, string name)
		{
			IntPtr clazz = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, name, "I");
			return AndroidJNI.GetStaticIntField(clazz, staticFieldID);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00045BA8 File Offset: 0x00043DA8
		public static string GetStaticStringField(string clsName, string name)
		{
			IntPtr clazz = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, name, "Ljava/lang/String;");
			return AndroidJNI.GetStaticStringField(clazz, staticFieldID);
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00045BD0 File Offset: 0x00043DD0
		public static float GetStaticFloatField(string clsName, string name)
		{
			IntPtr clazz = AndroidJNI.FindClass(clsName);
			IntPtr staticFieldID = AndroidJNI.GetStaticFieldID(clazz, name, "F");
			return AndroidJNI.GetStaticFloatField(clazz, staticFieldID);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00045BF8 File Offset: 0x00043DF8
		public void InvokeCallVoid(string name, string sig, params object[] args)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			AndroidJNI.CallVoidMethod(this.raw, methodID, args2);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00045C28 File Offset: 0x00043E28
		public T InvokeCall<T>(string name, string sig, params object[] args)
		{
			Type typeFromHandle = typeof(T);
			IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			if (methodID == IntPtr.Zero)
			{
				Debug.LogError("Cannot get method for " + name);
				throw new Exception("Cannot get method for " + name);
			}
			if (typeFromHandle == typeof(bool))
			{
				return (T)((object)AndroidJNI.CallBooleanMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(string))
			{
				return (T)((object)AndroidJNI.CallStringMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(int))
			{
				return (T)((object)AndroidJNI.CallIntMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(float))
			{
				return (T)((object)AndroidJNI.CallFloatMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(double))
			{
				return (T)((object)AndroidJNI.CallDoubleMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(byte))
			{
				return (T)((object)AndroidJNI.CallByteMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(char))
			{
				return (T)((object)AndroidJNI.CallCharMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(long))
			{
				return (T)((object)AndroidJNI.CallLongMethod(this.raw, methodID, args2));
			}
			if (typeFromHandle == typeof(short))
			{
				return (T)((object)AndroidJNI.CallShortMethod(this.raw, methodID, args2));
			}
			return this.InvokeObjectCall<T>(name, sig, args);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00045DF4 File Offset: 0x00043FF4
		public static T StaticInvokeCall<T>(string type, string name, string sig, params object[] args)
		{
			Type typeFromHandle = typeof(T);
			IntPtr clazz = AndroidJNI.FindClass(type);
			IntPtr staticMethodID = AndroidJNI.GetStaticMethodID(clazz, name, sig);
			jvalue[] args2 = JavaObjWrapper.ConstructArgArray(args);
			if (typeFromHandle == typeof(bool))
			{
				return (T)((object)AndroidJNI.CallStaticBooleanMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(string))
			{
				return (T)((object)AndroidJNI.CallStaticStringMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(int))
			{
				return (T)((object)AndroidJNI.CallStaticIntMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(float))
			{
				return (T)((object)AndroidJNI.CallStaticFloatMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(double))
			{
				return (T)((object)AndroidJNI.CallStaticDoubleMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(byte))
			{
				return (T)((object)AndroidJNI.CallStaticByteMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(char))
			{
				return (T)((object)AndroidJNI.CallStaticCharMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(long))
			{
				return (T)((object)AndroidJNI.CallStaticLongMethod(clazz, staticMethodID, args2));
			}
			if (typeFromHandle == typeof(short))
			{
				return (T)((object)AndroidJNI.CallStaticShortMethod(clazz, staticMethodID, args2));
			}
			return JavaObjWrapper.StaticInvokeObjectCall<T>(type, name, sig, args);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00045F64 File Offset: 0x00044164
		public T InvokeObjectCall<T>(string name, string sig, params object[] theArgs)
		{
			IntPtr methodID = AndroidJNI.GetMethodID(this.RawClass, name, sig);
			jvalue[] args = JavaObjWrapper.ConstructArgArray(theArgs);
			IntPtr intPtr = AndroidJNI.CallObjectMethod(this.raw, methodID, args);
			if (intPtr.Equals(IntPtr.Zero))
			{
				return default(T);
			}
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[]
			{
				intPtr.GetType()
			});
			if (constructor != null)
			{
				return (T)((object)constructor.Invoke(new object[]
				{
					intPtr
				}));
			}
			Type typeFromHandle = typeof(T);
			return (T)((object)Marshal.PtrToStructure(intPtr, typeFromHandle));
		}

		// Token: 0x04000AA4 RID: 2724
		private IntPtr raw;

		// Token: 0x04000AA5 RID: 2725
		private IntPtr cachedRawClass = IntPtr.Zero;
	}
}
