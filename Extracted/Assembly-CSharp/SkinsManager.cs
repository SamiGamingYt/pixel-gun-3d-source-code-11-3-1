using System;
using System.IO;
using UnityEngine;

// Token: 0x020007C6 RID: 1990
public sealed class SkinsManager
{
	// Token: 0x06004801 RID: 18433 RVA: 0x0018F4E8 File Offset: 0x0018D6E8
	private static void _WriteImageAtPathToGal(string pathToImage)
	{
		try
		{
		}
		catch (Exception arg)
		{
			Debug.Log("Exception in _ScreenshotWriteToAlbum: " + arg);
		}
	}

	// Token: 0x06004802 RID: 18434 RVA: 0x0018F52C File Offset: 0x0018D72C
	public static void SaveTextureToGallery(Texture2D t, string nm)
	{
		string pathToImage = Path.Combine(SkinsManager._PathBase, nm);
		SkinsManager._WriteImageAtPathToGal(pathToImage);
	}

	// Token: 0x06004803 RID: 18435 RVA: 0x0018F54C File Offset: 0x0018D74C
	public static bool SaveTextureWithName(Texture2D t, string nm, bool writeToGallery = true)
	{
		string text = Path.Combine(SkinsManager._PathBase, nm);
		try
		{
			byte[] array = t.EncodeToPNG();
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(array, 0, array.Length);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		if (writeToGallery)
		{
			SkinsManager._WriteImageAtPathToGal(text);
		}
		return true;
	}

	// Token: 0x06004804 RID: 18436 RVA: 0x0018F5F8 File Offset: 0x0018D7F8
	public static byte[] ReadAllBytes(string path)
	{
		byte[] array;
		using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
		{
			int num = 0;
			long length = fileStream.Length;
			if (length > 2147483647L)
			{
				throw new IOException("File is too long.");
			}
			int i = (int)length;
			array = new byte[i];
			while (i > 0)
			{
				int num2 = fileStream.Read(array, num, i);
				if (num2 == 0)
				{
					throw new EndOfStreamException("Read beyond end of file.");
				}
				num += num2;
				i -= num2;
			}
		}
		return array;
	}

	// Token: 0x06004805 RID: 18437 RVA: 0x0018F6A0 File Offset: 0x0018D8A0
	public static Texture2D TextureForName(string nm, int w = 64, int h = 32, bool disableMimMap = false)
	{
		Texture2D texture2D = (!disableMimMap) ? new Texture2D(w, h) : new Texture2D(w, h, TextureFormat.ARGB32, false);
		string text = Path.Combine(SkinsManager._PathBase, nm);
		try
		{
			byte[] data = SkinsManager.ReadAllBytes(text);
			texture2D.LoadImage(data);
		}
		catch (Exception arg)
		{
			string message = string.Format("Failed to read bytes from {0}\n{1}", text, arg);
			Debug.LogError(message);
		}
		return texture2D;
	}

	// Token: 0x06004806 RID: 18438 RVA: 0x0018F724 File Offset: 0x0018D924
	public static bool DeleteTexture(string nm)
	{
		try
		{
			File.Delete(Path.Combine(SkinsManager._PathBase, nm));
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
		return true;
	}

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x06004807 RID: 18439 RVA: 0x0018F770 File Offset: 0x0018D970
	public static string _PathBase
	{
		get
		{
			return Application.persistentDataPath;
		}
	}
}
