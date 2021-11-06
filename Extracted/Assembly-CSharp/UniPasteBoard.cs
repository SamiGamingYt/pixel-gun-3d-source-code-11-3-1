using System;
using UnityEngine;

// Token: 0x02000878 RID: 2168
public class UniPasteBoard
{
	// Token: 0x06004E5B RID: 20059 RVA: 0x001C6770 File Offset: 0x001C4970
	public static string GetClipBoardString()
	{
		return UniPasteBoard.androidGetClipBoardString();
	}

	// Token: 0x06004E5C RID: 20060 RVA: 0x001C6778 File Offset: 0x001C4978
	public static void SetClipBoardString(string text)
	{
		UniPasteBoard.androidSetClipBoardString(text);
	}

	// Token: 0x17000CC0 RID: 3264
	// (get) Token: 0x06004E5D RID: 20061 RVA: 0x001C6780 File Offset: 0x001C4980
	private static AndroidJavaClass JavaClass
	{
		get
		{
			if (UniPasteBoard._javaClass == null)
			{
				try
				{
					UniPasteBoard._javaClass = new AndroidJavaClass("com.onevcat.UniPasteBoard.PasteBoard");
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
				}
			}
			return UniPasteBoard._javaClass;
		}
	}

	// Token: 0x06004E5E RID: 20062 RVA: 0x001C67E0 File Offset: 0x001C49E0
	private static string androidGetClipBoardString()
	{
		string result = null;
		if (UniPasteBoard.JavaClass != null)
		{
			result = UniPasteBoard.JavaClass.CallStatic<string>("getClipBoardString", new object[0]);
		}
		return result;
	}

	// Token: 0x06004E5F RID: 20063 RVA: 0x001C6810 File Offset: 0x001C4A10
	private static void androidSetClipBoardString(string text)
	{
		if (UniPasteBoard.JavaClass != null)
		{
			UniPasteBoard.JavaClass.CallStatic("setClipBoardString", new object[]
			{
				text
			});
		}
	}

	// Token: 0x04003CF8 RID: 15608
	private static AndroidJavaClass _javaClass;
}
