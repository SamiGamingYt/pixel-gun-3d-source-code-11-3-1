using System;
using UnityEngine;

// Token: 0x020007CA RID: 1994
public class Save : MonoBehaviour
{
	// Token: 0x0600483C RID: 18492 RVA: 0x001908A0 File Offset: 0x0018EAA0
	public static void SavePos(string name, GameObject gameObject)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[]
		{
			gameObject.transform.position.x,
			"&",
			gameObject.transform.position.y,
			"&",
			gameObject.transform.position.z
		}));
	}

	// Token: 0x0600483D RID: 18493 RVA: 0x00190920 File Offset: 0x0018EB20
	public static void SaveString(string name, string variable)
	{
		PlayerPrefs.SetString(name, variable);
	}

	// Token: 0x0600483E RID: 18494 RVA: 0x0019092C File Offset: 0x0018EB2C
	public static void SaveStringArray(string name, string[] variable)
	{
		Debug.Log(string.Concat(new object[]
		{
			"SaveStringArray name: ",
			name,
			"  variable=",
			variable
		}));
		PlayerPrefs.SetString(name, string.Join("#", variable));
	}

	// Token: 0x0600483F RID: 18495 RVA: 0x00190968 File Offset: 0x0018EB68
	public static void SaveStringArray(string name, string[] variable, char separator)
	{
		PlayerPrefs.SetString(name, string.Join(separator.ToString(), variable));
	}

	// Token: 0x06004840 RID: 18496 RVA: 0x00190980 File Offset: 0x0018EB80
	public static void SaveInt(string name, int variable)
	{
		PlayerPrefs.SetInt(name, variable);
	}

	// Token: 0x06004841 RID: 18497 RVA: 0x0019098C File Offset: 0x0018EB8C
	public static void SaveIntArray(string name, int[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004842 RID: 18498 RVA: 0x001909D8 File Offset: 0x0018EBD8
	public static void SaveUInt(string name, uint variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004843 RID: 18499 RVA: 0x001909E8 File Offset: 0x0018EBE8
	public static void SaveUIntArray(string name, uint[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004844 RID: 18500 RVA: 0x00190A34 File Offset: 0x0018EC34
	public static void SaveLong(string name, long variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x00190A44 File Offset: 0x0018EC44
	public static void SaveLongArray(string name, long[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x00190A90 File Offset: 0x0018EC90
	public static void SaveULong(string name, ulong variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x00190AA0 File Offset: 0x0018ECA0
	public static void SaveULongArray(string name, ulong[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x00190AEC File Offset: 0x0018ECEC
	public static void SaveShort(string name, short variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004849 RID: 18505 RVA: 0x00190AFC File Offset: 0x0018ECFC
	public static void SaveShortArray(string name, short[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x00190B48 File Offset: 0x0018ED48
	public static void SaveUShort(string name, ushort variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x00190B58 File Offset: 0x0018ED58
	public static void SaveUShortArray(string name, ushort[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x00190BA4 File Offset: 0x0018EDA4
	public static void SaveFloat(string name, float variable)
	{
		PlayerPrefs.SetFloat(name, variable);
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x00190BB0 File Offset: 0x0018EDB0
	public static void SaveFloatArray(string name, float[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x00190BFC File Offset: 0x0018EDFC
	public static void SaveDouble(string name, double variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x0600484F RID: 18511 RVA: 0x00190C0C File Offset: 0x0018EE0C
	public static void SaveDoubleArray(string name, double[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004850 RID: 18512 RVA: 0x00190C58 File Offset: 0x0018EE58
	public static void SaveBool(string name, bool variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x00190C68 File Offset: 0x0018EE68
	public static void SaveBoolArray(string name, bool[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004852 RID: 18514 RVA: 0x00190CB4 File Offset: 0x0018EEB4
	public static void SaveChar(string name, char variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x00190CC4 File Offset: 0x0018EEC4
	public static void SaveCharArray(string name, char[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004854 RID: 18516 RVA: 0x00190D10 File Offset: 0x0018EF10
	public static void SaveCharArray(string name, char[] variable, char separator)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + separator.ToString();
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004855 RID: 18517 RVA: 0x00190D60 File Offset: 0x0018EF60
	public static void SaveDecimal(string name, decimal variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004856 RID: 18518 RVA: 0x00190D70 File Offset: 0x0018EF70
	public static void SaveDecimalArray(string name, decimal[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004857 RID: 18519 RVA: 0x00190DBC File Offset: 0x0018EFBC
	public static void SaveByte(string name, byte variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004858 RID: 18520 RVA: 0x00190DCC File Offset: 0x0018EFCC
	public static void SaveByteArray(string name, byte[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004859 RID: 18521 RVA: 0x00190E18 File Offset: 0x0018F018
	public static void SaveSByte(string name, sbyte variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x0600485A RID: 18522 RVA: 0x00190E28 File Offset: 0x0018F028
	public static void SaveSByteArray(string name, sbyte[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600485B RID: 18523 RVA: 0x00190E74 File Offset: 0x0018F074
	public static void SaveVector4(string name, Vector4 variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[]
		{
			variable.x,
			"&",
			variable.y,
			"&",
			variable.z,
			"&",
			variable.w
		}));
	}

	// Token: 0x0600485C RID: 18524 RVA: 0x00190EE8 File Offset: 0x0018F0E8
	public static void SaveVector4Array(string name, Vector4[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				variable[i].x,
				"&",
				variable[i].y,
				"&",
				variable[i].z,
				"&",
				variable[i].w,
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600485D RID: 18525 RVA: 0x00190FA0 File Offset: 0x0018F1A0
	public static void SaveVector3(string name, Vector3 variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[]
		{
			variable.x,
			"&",
			variable.y,
			"&",
			variable.z
		}));
	}

	// Token: 0x0600485E RID: 18526 RVA: 0x00190FFC File Offset: 0x0018F1FC
	public static void SaveVector3Array(string name, Vector3[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				variable[i].x,
				"&",
				variable[i].y,
				"&",
				variable[i].z,
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x0600485F RID: 18527 RVA: 0x00191098 File Offset: 0x0018F298
	public static void SaveVector2(string name, Vector2 variable)
	{
		PlayerPrefs.SetString(name, variable.x + "&" + variable.y);
	}

	// Token: 0x06004860 RID: 18528 RVA: 0x001910D0 File Offset: 0x0018F2D0
	public static void SaveVector2Array(string name, Vector2[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				variable[i].x,
				"&",
				variable[i].y,
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004861 RID: 18529 RVA: 0x00191150 File Offset: 0x0018F350
	public static void SaveQuaternion(string name, Quaternion variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new object[]
		{
			variable.x,
			"&",
			variable.y,
			"&",
			variable.z,
			"&",
			variable.w
		}));
	}

	// Token: 0x06004862 RID: 18530 RVA: 0x001911C4 File Offset: 0x0018F3C4
	public static void SaveQuaternionArray(string name, Quaternion[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				variable[i].x,
				"&",
				variable[i].y,
				"&",
				variable[i].z,
				"&",
				variable[i].w,
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004863 RID: 18531 RVA: 0x0019127C File Offset: 0x0018F47C
	public static void SaveColor(string name, Color variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new string[]
		{
			variable.r.ToString(),
			"&",
			variable.g.ToString(),
			"&",
			variable.b.ToString(),
			"&",
			variable.a.ToString()
		}));
	}

	// Token: 0x06004864 RID: 18532 RVA: 0x001912F0 File Offset: 0x0018F4F0
	public static void SaveColorArray(string name, Color[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				variable[i].r.ToString(),
				"&",
				variable[i].g.ToString(),
				"&",
				variable[i].b.ToString(),
				"&",
				variable[i].a.ToString(),
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004865 RID: 18533 RVA: 0x001913A8 File Offset: 0x0018F5A8
	public static void SaveKeyCode(string name, KeyCode variable)
	{
		PlayerPrefs.SetString(name, variable.ToString());
	}

	// Token: 0x06004866 RID: 18534 RVA: 0x001913BC File Offset: 0x0018F5BC
	public static void SaveKeyCodeArray(string name, KeyCode[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			text = text + variable[i].ToString() + "#";
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004867 RID: 18535 RVA: 0x00191408 File Offset: 0x0018F608
	public static void SaveRect(string name, Rect variable)
	{
		PlayerPrefs.SetString(name, string.Concat(new string[]
		{
			variable.x.ToString(),
			"&",
			variable.y.ToString(),
			"&",
			variable.width.ToString(),
			"&",
			variable.height.ToString()
		}));
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x00191488 File Offset: 0x0018F688
	public static void SaveRectArray(string name, Rect[] variable)
	{
		string text = string.Empty;
		for (int i = 0; i < variable.Length; i++)
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				variable[i].x.ToString(),
				"&",
				variable[i].y.ToString(),
				"&",
				variable[i].width.ToString(),
				"&",
				variable[i].height.ToString(),
				"#"
			});
		}
		PlayerPrefs.SetString(name, text.ToString());
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x00191550 File Offset: 0x0018F750
	public static void SaveTexture2D(string name, Texture2D variable)
	{
		byte[] inArray = variable.EncodeToPNG();
		int width = variable.width;
		int height = variable.height;
		string value = string.Concat(new string[]
		{
			width.ToString(),
			"&",
			height.ToString(),
			"&",
			Convert.ToBase64String(inArray)
		});
		PlayerPrefs.SetString(name, value);
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x001915B4 File Offset: 0x0018F7B4
	public static void Delete(string name)
	{
		PlayerPrefs.DeleteKey(name);
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x001915BC File Offset: 0x0018F7BC
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
