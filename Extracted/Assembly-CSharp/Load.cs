using System;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public sealed class Load : MonoBehaviour
{
	// Token: 0x0600480E RID: 18446 RVA: 0x0018F850 File Offset: 0x0018DA50
	public static void LoadPos(string name, GameObject gameObject)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			gameObject.transform.position = new Vector3(0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Vector3 position = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
		gameObject.transform.position = position;
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x0018F8D4 File Offset: 0x0018DAD4
	public static string LoadString(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return string.Empty;
		}
		return PlayerPrefs.GetString(name);
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x0018F8FC File Offset: 0x0018DAFC
	public static string[] LoadStringArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			Debug.Log("LoadStringArray(): Cannot find key " + name);
			return null;
		}
		return PlayerPrefs.GetString(name).Split(new char[]
		{
			'#'
		});
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x0018F940 File Offset: 0x0018DB40
	public static string[] LoadStringArray(string name, char separator)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		return PlayerPrefs.GetString(name).Split(new char[]
		{
			separator
		});
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x0018F974 File Offset: 0x0018DB74
	public static int LoadInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return PlayerPrefs.GetInt(name);
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x0018F998 File Offset: 0x0018DB98
	public static int[] LoadIntArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		int[] array2 = new int[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = Convert.ToInt32(array[i]);
		}
		return array2;
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x0018FA00 File Offset: 0x0018DC00
	public static uint LoadUInt(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0U;
		}
		return uint.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x0018FA28 File Offset: 0x0018DC28
	public static uint[] LoadUIntArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		uint[] array2 = new uint[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = Convert.ToUInt32(array[i]);
		}
		return array2;
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x0018FA90 File Offset: 0x0018DC90
	public static long LoadLong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0L;
		}
		return long.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x0018FAB8 File Offset: 0x0018DCB8
	public static long[] LoadLongArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		long[] array2 = new long[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = long.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x0018FB20 File Offset: 0x0018DD20
	public static ulong LoadULong(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0UL;
		}
		return ulong.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x0018FB48 File Offset: 0x0018DD48
	public static ulong[] LoadULongArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		ulong[] array2 = new ulong[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = ulong.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x0018FBB0 File Offset: 0x0018DDB0
	public static short LoadShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return short.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x0018FBD8 File Offset: 0x0018DDD8
	public static short[] LoadShortArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		short[] array2 = new short[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = short.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x0018FC40 File Offset: 0x0018DE40
	public static ushort LoadUShort(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return ushort.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x0018FC68 File Offset: 0x0018DE68
	public static ushort[] LoadUShortArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		ushort[] array2 = new ushort[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = ushort.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x0018FCD0 File Offset: 0x0018DED0
	public static float LoadFloat(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0f;
		}
		return PlayerPrefs.GetFloat(name);
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x0018FCF8 File Offset: 0x0018DEF8
	public static float[] LoadFloatArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		float[] array2 = new float[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = float.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x0018FD60 File Offset: 0x0018DF60
	public static double LoadDouble(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0.0;
		}
		return double.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x0018FD90 File Offset: 0x0018DF90
	public static double[] LoadDoubleArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		double[] array2 = new double[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = double.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x0018FDF8 File Offset: 0x0018DFF8
	public static bool LoadBool(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return false;
		}
		string @string = PlayerPrefs.GetString(name);
		bool flag;
		return bool.TryParse(@string, out flag) && flag;
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x0018FE2C File Offset: 0x0018E02C
	public static bool[] LoadBoolArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		bool[] array2 = new bool[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = bool.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x0018FE94 File Offset: 0x0018E094
	public static char LoadChar(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return '\0';
		}
		char result = '\0';
		char.TryParse(PlayerPrefs.GetString(name), out result);
		return result;
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x0018FEC0 File Offset: 0x0018E0C0
	public static char[] LoadCharArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		char[] array2 = new char[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			char.TryParse(array[i], out array2[i]);
		}
		return array2;
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x0018FF2C File Offset: 0x0018E12C
	public static decimal LoadDecimal(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0m;
		}
		return decimal.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x0018FF58 File Offset: 0x0018E158
	public static decimal[] LoadDecimalArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		decimal[] array2 = new decimal[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = decimal.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x0018FFC8 File Offset: 0x0018E1C8
	public static byte LoadByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return byte.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x06004829 RID: 18473 RVA: 0x0018FFF0 File Offset: 0x0018E1F0
	public static byte[] LoadByteArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		byte[] array2 = new byte[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = byte.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x0600482A RID: 18474 RVA: 0x00190058 File Offset: 0x0018E258
	public static sbyte LoadSByte(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return 0;
		}
		return sbyte.Parse(PlayerPrefs.GetString(name));
	}

	// Token: 0x0600482B RID: 18475 RVA: 0x00190080 File Offset: 0x0018E280
	public static sbyte[] LoadSByteArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		sbyte[] array2 = new sbyte[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = sbyte.Parse(array[i]);
		}
		return array2;
	}

	// Token: 0x0600482C RID: 18476 RVA: 0x001900E8 File Offset: 0x0018E2E8
	public static Vector4 LoadVector4(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector4(0f, 0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Vector4 result = new Vector4(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		return result;
	}

	// Token: 0x0600482D RID: 18477 RVA: 0x00190164 File Offset: 0x0018E364
	public static Vector4[] LoadVector4Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		Vector4[] array2 = new Vector4[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				"&"[0]
			});
			array2[i] = new Vector4(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]));
		}
		return array2;
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x00190210 File Offset: 0x0018E410
	public static Vector3 LoadVector3(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector3(0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Vector3 result = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
		return result;
	}

	// Token: 0x0600482F RID: 18479 RVA: 0x00190280 File Offset: 0x0018E480
	public static Vector3[] LoadVector3Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			'#'
		});
		Vector3[] array2 = new Vector3[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				'&'
			});
			array2[i] = new Vector3(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]));
		}
		return array2;
	}

	// Token: 0x06004830 RID: 18480 RVA: 0x00190310 File Offset: 0x0018E510
	public static Vector2 LoadVector2(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Vector2(0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Vector2 result = new Vector2(float.Parse(array[0]), float.Parse(array[1]));
		return result;
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x00190370 File Offset: 0x0018E570
	public static Vector2[] LoadVector2Array(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		Vector2[] array2 = new Vector2[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				"&"[0]
			});
			array2[i] = new Vector2(float.Parse(array3[0]), float.Parse(array3[1]));
		}
		return array2;
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x0019040C File Offset: 0x0018E60C
	public static Quaternion LoadQuaternion(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Quaternion(0f, 0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Quaternion result = new Quaternion(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		return result;
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x00190488 File Offset: 0x0018E688
	public static Quaternion[] LoadQuaternionArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		Quaternion[] array2 = new Quaternion[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				"&"[0]
			});
			array2[i] = new Quaternion(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]));
		}
		return array2;
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x00190534 File Offset: 0x0018E734
	public static Color LoadColor(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Color(0f, 0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Color result = new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		return result;
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x001905B0 File Offset: 0x0018E7B0
	public static Color[] LoadColorArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		Color[] array2 = new Color[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				"&"[0]
			});
			array2[i] = new Color(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]));
		}
		return array2;
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x0019065C File Offset: 0x0018E85C
	public static KeyCode LoadKeyCode(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return KeyCode.Space;
		}
		return (KeyCode)((int)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name)));
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x00190694 File Offset: 0x0018E894
	public static KeyCode[] LoadKeyCodeArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		KeyCode[] array2 = new KeyCode[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			array2[i] = (KeyCode)((int)Enum.Parse(typeof(KeyCode), array[i]));
		}
		return array2;
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x0019070C File Offset: 0x0018E90C
	public static Rect LoadRect(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return new Rect(0f, 0f, 0f, 0f);
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		Rect result = new Rect(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		return result;
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x00190788 File Offset: 0x0018E988
	public static Rect[] LoadRectArray(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"#"[0]
		});
		Rect[] array2 = new Rect[array.Length - 1];
		for (int i = 0; i < array.Length - 1; i++)
		{
			string[] array3 = array[i].Split(new char[]
			{
				"&"[0]
			});
			array2[i] = new Rect(float.Parse(array3[0]), float.Parse(array3[1]), float.Parse(array3[2]), float.Parse(array3[3]));
		}
		return array2;
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x00190834 File Offset: 0x0018EA34
	public static Texture2D LoadTexture2D(string name)
	{
		if (!PlayerPrefs.HasKey(name))
		{
			return null;
		}
		string[] array = PlayerPrefs.GetString(name).Split(new char[]
		{
			"&"[0]
		});
		byte[] data = Convert.FromBase64String(array[2]);
		Texture2D texture2D = new Texture2D(int.Parse(array[0]), int.Parse(array[1]));
		texture2D.LoadImage(data);
		return texture2D;
	}
}
