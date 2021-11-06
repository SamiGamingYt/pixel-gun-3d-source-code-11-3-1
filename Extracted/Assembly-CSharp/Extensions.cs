using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public static class Extensions
{
	// Token: 0x060023D1 RID: 9169 RVA: 0x000B27D8 File Offset: 0x000B09D8
	public static ParameterInfo[] GetCachedParemeters(this MethodInfo mo)
	{
		ParameterInfo[] parameters;
		if (!Extensions.parametersOfMethods.TryGetValue(mo, out parameters))
		{
			parameters = mo.GetParameters();
			Extensions.parametersOfMethods[mo] = parameters;
		}
		return parameters;
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000B2810 File Offset: 0x000B0A10
	public static PhotonView[] GetPhotonViewsInChildren(this GameObject go)
	{
		return go.GetComponentsInChildren<PhotonView>(true);
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x000B281C File Offset: 0x000B0A1C
	public static PhotonView GetPhotonView(this GameObject go)
	{
		return go.GetComponent<PhotonView>();
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000B2824 File Offset: 0x000B0A24
	public static bool AlmostEquals(this Vector3 target, Vector3 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x060023D5 RID: 9173 RVA: 0x000B2844 File Offset: 0x000B0A44
	public static bool AlmostEquals(this Vector2 target, Vector2 second, float sqrMagnitudePrecision)
	{
		return (target - second).sqrMagnitude < sqrMagnitudePrecision;
	}

	// Token: 0x060023D6 RID: 9174 RVA: 0x000B2864 File Offset: 0x000B0A64
	public static bool AlmostEquals(this Quaternion target, Quaternion second, float maxAngle)
	{
		return Quaternion.Angle(target, second) < maxAngle;
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x000B2870 File Offset: 0x000B0A70
	public static bool AlmostEquals(this float target, float second, float floatDiff)
	{
		return Mathf.Abs(target - second) < floatDiff;
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000B2880 File Offset: 0x000B0A80
	public static void Merge(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		foreach (object key in addHash.Keys)
		{
			target[key] = addHash[key];
		}
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000B2904 File Offset: 0x000B0B04
	public static void MergeStringKeys(this IDictionary target, IDictionary addHash)
	{
		if (addHash == null || target.Equals(addHash))
		{
			return;
		}
		foreach (object obj in addHash.Keys)
		{
			if (obj is string)
			{
				target[obj] = addHash[obj];
			}
		}
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000B2994 File Offset: 0x000B0B94
	public static string ToStringFull(this IDictionary origin)
	{
		return SupportClass.DictionaryToString(origin, false);
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000B29A0 File Offset: 0x000B0BA0
	public static string ToStringFull(this object[] data)
	{
		if (data == null)
		{
			return "null";
		}
		string[] array = new string[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			object obj = data[i];
			array[i] = ((obj == null) ? "null" : obj.ToString());
		}
		return string.Join(", ", array);
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000B2A00 File Offset: 0x000B0C00
	public static ExitGames.Client.Photon.Hashtable StripToStringKeys(this IDictionary original)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (original != null)
		{
			foreach (object obj in original.Keys)
			{
				if (obj is string)
				{
					hashtable[obj] = original[obj];
				}
			}
		}
		return hashtable;
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000B2A8C File Offset: 0x000B0C8C
	public static void StripKeysWithNullValues(this IDictionary original)
	{
		object[] array = new object[original.Count];
		int num = 0;
		foreach (object obj in original.Keys)
		{
			array[num++] = obj;
		}
		foreach (object key in array)
		{
			if (original[key] == null)
			{
				original.Remove(key);
			}
		}
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000B2B3C File Offset: 0x000B0D3C
	public static bool Contains(this int[] target, int nr)
	{
		if (target == null)
		{
			return false;
		}
		for (int i = 0; i < target.Length; i++)
		{
			if (target[i] == nr)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0400189C RID: 6300
	public static Dictionary<MethodInfo, ParameterInfo[]> parametersOfMethods = new Dictionary<MethodInfo, ParameterInfo[]>();
}
