using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HeurekaGames
{
	// Token: 0x0200029A RID: 666
	public static class Extensions
	{
		// Token: 0x0600151F RID: 5407 RVA: 0x00053C90 File Offset: 0x00051E90
		public static Vector2 YZ(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00053CA8 File Offset: 0x00051EA8
		public static Vector2[] YZ(this Vector3[] v)
		{
			Vector2[] array = new Vector2[v.Length];
			for (int i = 0; i < v.Length; i++)
			{
				array[i] = new Vector2(v[i].x, v[i].z);
			}
			return array;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00053CFC File Offset: 0x00051EFC
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00053D0C File Offset: 0x00051F0C
		public static string ToCamelCase(this string camelCaseString)
		{
			return Regex.Replace(camelCaseString, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ").Trim();
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00053D24 File Offset: 0x00051F24
		public static void SetComponentRecursively<T>(this GameObject gameObject, bool tf) where T : Component
		{
			T[] componentsInChildren = gameObject.GetComponentsInChildren<T>();
			foreach (T t in componentsInChildren)
			{
				try
				{
					PropertyInfo property = typeof(T).GetProperty("enabled");
					if (property != null && property.CanWrite)
					{
						property.SetValue(t, tf, null);
					}
					else
					{
						Console.WriteLine("BLABLA");
						Debug.Log("Property does not exist, or cannot write");
					}
				}
				catch (NullReferenceException ex)
				{
					Debug.Log("The property does not exist in MyClass." + ex.Message);
				}
			}
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00053DEC File Offset: 0x00051FEC
		public static void CastList<T>(this List<T> targetList)
		{
			targetList = targetList.Cast<T>().ToList<T>();
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00053DFC File Offset: 0x00051FFC
		public static bool Has<T>(this Enum type, T value)
		{
			bool result;
			try
			{
				result = (((int)type & (int)((object)value)) == (int)((object)value));
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00053E60 File Offset: 0x00052060
		public static bool Is<T>(this Enum type, T value)
		{
			bool result;
			try
			{
				result = ((int)type == (int)((object)value));
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x00053EB8 File Offset: 0x000520B8
		public static T Add<T>(this Enum type, T value)
		{
			T result;
			try
			{
				result = (T)((object)((int)type | (int)((object)value)));
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), innerException);
			}
			return result;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00053F30 File Offset: 0x00052130
		public static T Remove<T>(this Enum type, T value)
		{
			T result;
			try
			{
				result = (T)((object)((int)type & ~(int)((object)value)));
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), innerException);
			}
			return result;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00053FA8 File Offset: 0x000521A8
		public static Color ModifiedAlpha(this Color color, float alpha)
		{
			Color result = color;
			result.a = alpha;
			return result;
		}
	}
}
