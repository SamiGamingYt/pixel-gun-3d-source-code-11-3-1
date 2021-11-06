using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public static class CryptoPlayerPrefs
{
	// Token: 0x06004EC0 RID: 20160 RVA: 0x001C8BEC File Offset: 0x001C6DEC
	public static bool HasKey(string key)
	{
		string key2 = CryptoPlayerPrefs.hashedKey(key);
		return PlayerPrefs.HasKey(key2);
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x001C8C08 File Offset: 0x001C6E08
	public static void DeleteKey(string key)
	{
		string key2 = CryptoPlayerPrefs.hashedKey(key);
		PlayerPrefs.DeleteKey(key2);
	}

	// Token: 0x06004EC2 RID: 20162 RVA: 0x001C8C24 File Offset: 0x001C6E24
	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}

	// Token: 0x06004EC3 RID: 20163 RVA: 0x001C8C2C File Offset: 0x001C6E2C
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x06004EC4 RID: 20164 RVA: 0x001C8C34 File Offset: 0x001C6E34
	public static void SetInt(string key, int val)
	{
		string text = CryptoPlayerPrefs.hashedKey(key);
		int value = val;
		if (CryptoPlayerPrefs._useXor)
		{
			int num = CryptoPlayerPrefs.computeXorOperand(key, text);
			int num2 = CryptoPlayerPrefs.computePlusOperand(num);
			value = (val + num2 ^ num);
		}
		if (CryptoPlayerPrefs._useRijndael)
		{
			PlayerPrefs.SetString(text, CryptoPlayerPrefs.encrypt(text, value.ToString()));
		}
		else
		{
			PlayerPrefs.SetInt(text, value);
		}
	}

	// Token: 0x06004EC5 RID: 20165 RVA: 0x001C8C94 File Offset: 0x001C6E94
	public static void SetLong(string key, long val)
	{
		CryptoPlayerPrefs.SetString(key, val.ToString());
	}

	// Token: 0x06004EC6 RID: 20166 RVA: 0x001C8CA4 File Offset: 0x001C6EA4
	public static void SetString(string key, string val)
	{
		string text = CryptoPlayerPrefs.hashedKey(key);
		string text2 = val;
		if (CryptoPlayerPrefs._useXor)
		{
			int num = CryptoPlayerPrefs.computeXorOperand(key, text);
			int num2 = CryptoPlayerPrefs.computePlusOperand(num);
			text2 = string.Empty;
			foreach (char c in val)
			{
				char c2 = (char)((int)c + num2 ^ num);
				text2 += c2;
			}
		}
		if (CryptoPlayerPrefs._useRijndael)
		{
			PlayerPrefs.SetString(text, CryptoPlayerPrefs.encrypt(text, text2));
		}
		else
		{
			PlayerPrefs.SetString(text, text2);
		}
	}

	// Token: 0x06004EC7 RID: 20167 RVA: 0x001C8D40 File Offset: 0x001C6F40
	public static void SetFloat(string key, float val)
	{
		CryptoPlayerPrefs.SetString(key, val.ToString());
	}

	// Token: 0x06004EC8 RID: 20168 RVA: 0x001C8D50 File Offset: 0x001C6F50
	public static int GetInt(string key, int defaultValue = 0)
	{
		string text = CryptoPlayerPrefs.hashedKey(key);
		if (!PlayerPrefs.HasKey(text))
		{
			return defaultValue;
		}
		int num;
		if (CryptoPlayerPrefs._useRijndael)
		{
			num = int.Parse(CryptoPlayerPrefs.decrypt(text));
		}
		else
		{
			num = PlayerPrefs.GetInt(text);
		}
		int result = num;
		if (CryptoPlayerPrefs._useXor)
		{
			int num2 = CryptoPlayerPrefs.computeXorOperand(key, text);
			int num3 = CryptoPlayerPrefs.computePlusOperand(num2);
			result = (num2 ^ num) - num3;
		}
		return result;
	}

	// Token: 0x06004EC9 RID: 20169 RVA: 0x001C8DB8 File Offset: 0x001C6FB8
	public static long GetLong(string key, long defaultValue = 0L)
	{
		return long.Parse(CryptoPlayerPrefs.GetString(key, defaultValue.ToString()));
	}

	// Token: 0x06004ECA RID: 20170 RVA: 0x001C8DCC File Offset: 0x001C6FCC
	public static string GetString(string key, string defaultValue = "")
	{
		string text = CryptoPlayerPrefs.hashedKey(key);
		if (!PlayerPrefs.HasKey(text))
		{
			return defaultValue;
		}
		string text2;
		if (CryptoPlayerPrefs._useRijndael)
		{
			text2 = CryptoPlayerPrefs.decrypt(text);
		}
		else
		{
			text2 = PlayerPrefs.GetString(text);
		}
		string text3 = text2;
		if (CryptoPlayerPrefs._useXor)
		{
			int num = CryptoPlayerPrefs.computeXorOperand(key, text);
			int num2 = CryptoPlayerPrefs.computePlusOperand(num);
			text3 = string.Empty;
			foreach (char c in text2)
			{
				char c2 = (char)((num ^ (int)c) - num2);
				text3 += c2;
			}
		}
		return text3;
	}

	// Token: 0x06004ECB RID: 20171 RVA: 0x001C8E70 File Offset: 0x001C7070
	public static float GetFloat(string key, float defaultValue = 0f)
	{
		return float.Parse(CryptoPlayerPrefs.GetString(key, defaultValue.ToString()));
	}

	// Token: 0x06004ECC RID: 20172 RVA: 0x001C8E84 File Offset: 0x001C7084
	private static string encrypt(string cKey, string data)
	{
		return CryptoPlayerPrefs.EncryptString(data, CryptoPlayerPrefs.getEncryptionPassword(cKey));
	}

	// Token: 0x06004ECD RID: 20173 RVA: 0x001C8E94 File Offset: 0x001C7094
	private static string decrypt(string cKey)
	{
		return CryptoPlayerPrefs.DecryptString(PlayerPrefs.GetString(cKey), CryptoPlayerPrefs.getEncryptionPassword(cKey));
	}

	// Token: 0x06004ECE RID: 20174 RVA: 0x001C8EA8 File Offset: 0x001C70A8
	private static string hashedKey(string key)
	{
		if (CryptoPlayerPrefs.keyHashs == null)
		{
			CryptoPlayerPrefs.keyHashs = new Dictionary<string, string>();
		}
		string result;
		if (CryptoPlayerPrefs.keyHashs.TryGetValue(key, out result))
		{
			return result;
		}
		string text = CryptoPlayerPrefs.Md5Sum(key);
		CryptoPlayerPrefs.keyHashs.Add(key, text);
		return text;
	}

	// Token: 0x06004ECF RID: 20175 RVA: 0x001C8EF4 File Offset: 0x001C70F4
	private static int computeXorOperand(string key, string cryptedKey)
	{
		if (CryptoPlayerPrefs.xorOperands == null)
		{
			CryptoPlayerPrefs.xorOperands = new Dictionary<string, int>();
		}
		if (CryptoPlayerPrefs.xorOperands.ContainsKey(key))
		{
			return CryptoPlayerPrefs.xorOperands[key];
		}
		int num = 0;
		foreach (char c in cryptedKey)
		{
			num += (int)c;
		}
		num += CryptoPlayerPrefs.salt;
		CryptoPlayerPrefs.xorOperands.Add(key, num);
		return num;
	}

	// Token: 0x06004ED0 RID: 20176 RVA: 0x001C8F70 File Offset: 0x001C7170
	private static int computePlusOperand(int xor)
	{
		return xor & xor << 1;
	}

	// Token: 0x17000CC4 RID: 3268
	// (get) Token: 0x06004ED1 RID: 20177 RVA: 0x001C8F78 File Offset: 0x001C7178
	private static HashAlgorithm HashAlgorithm
	{
		get
		{
			if (CryptoPlayerPrefs._hashAlgorithm == null)
			{
				CryptoPlayerPrefs._hashAlgorithm = new MD5CryptoServiceProvider();
			}
			return CryptoPlayerPrefs._hashAlgorithm;
		}
	}

	// Token: 0x06004ED2 RID: 20178 RVA: 0x001C8F94 File Offset: 0x001C7194
	public static string Md5Sum(string strToEncrypt)
	{
		Encoding utf = Encoding.UTF8;
		byte[] bytes = utf.GetBytes(strToEncrypt);
		HashAlgorithm hashAlgorithm = CryptoPlayerPrefs.HashAlgorithm;
		byte[] array = hashAlgorithm.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder(32);
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(Convert.ToString(array[i], 16).PadLeft(2, '0'));
		}
		string text = stringBuilder.ToString();
		if (text.Length >= 32)
		{
			return text;
		}
		return text.PadLeft(32, '0');
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001C9020 File Offset: 0x001C7220
	private static byte[] EncryptString(byte[] clearText, SymmetricAlgorithm alg)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(clearText, 0, clearText.Length);
				cryptoStream.Close();
				byte[] array = memoryStream.ToArray();
				result = array;
			}
		}
		return result;
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001C90BC File Offset: 0x001C72BC
	private static string EncryptString(string clearText, string Password)
	{
		SymmetricAlgorithm rijndaelForKey = CryptoPlayerPrefs.getRijndaelForKey(Password);
		byte[] bytes = Encoding.Unicode.GetBytes(clearText);
		byte[] inArray = CryptoPlayerPrefs.EncryptString(bytes, rijndaelForKey);
		return Convert.ToBase64String(inArray);
	}

	// Token: 0x06004ED5 RID: 20181 RVA: 0x001C90EC File Offset: 0x001C72EC
	private static byte[] DecryptString(byte[] cipherData, SymmetricAlgorithm alg)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, alg.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(cipherData, 0, cipherData.Length);
				cryptoStream.Close();
				byte[] array = memoryStream.ToArray();
				result = array;
			}
		}
		return result;
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x001C9188 File Offset: 0x001C7388
	private static string DecryptString(string cipherText, string Password)
	{
		if (CryptoPlayerPrefs.rijndaelDict == null)
		{
			CryptoPlayerPrefs.rijndaelDict = new Dictionary<string, SymmetricAlgorithm>();
		}
		byte[] cipherData = Convert.FromBase64String(cipherText);
		SymmetricAlgorithm rijndaelForKey = CryptoPlayerPrefs.getRijndaelForKey(Password);
		byte[] array = CryptoPlayerPrefs.DecryptString(cipherData, rijndaelForKey);
		return Encoding.Unicode.GetString(array, 0, array.Length);
	}

	// Token: 0x06004ED7 RID: 20183 RVA: 0x001C91D0 File Offset: 0x001C73D0
	private static SymmetricAlgorithm getRijndaelForKey(string key)
	{
		if (CryptoPlayerPrefs.rijndaelDict == null)
		{
			CryptoPlayerPrefs.rijndaelDict = new Dictionary<string, SymmetricAlgorithm>();
		}
		SymmetricAlgorithm symmetricAlgorithm;
		if (!CryptoPlayerPrefs.rijndaelDict.TryGetValue(key, out symmetricAlgorithm))
		{
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, new byte[]
			{
				73,
				97,
				110,
				32,
				77,
				100,
				118,
				101,
				101,
				100,
				101,
				118,
				118
			});
			symmetricAlgorithm = Rijndael.Create();
			symmetricAlgorithm.Key = rfc2898DeriveBytes.GetBytes(32);
			symmetricAlgorithm.IV = rfc2898DeriveBytes.GetBytes(16);
			CryptoPlayerPrefs.rijndaelDict.Add(key, symmetricAlgorithm);
		}
		return symmetricAlgorithm;
	}

	// Token: 0x06004ED8 RID: 20184 RVA: 0x001C9250 File Offset: 0x001C7450
	private static string getEncryptionPassword(string pw)
	{
		return CryptoPlayerPrefs.Md5Sum(pw + CryptoPlayerPrefs.SaltString);
	}

	// Token: 0x06004ED9 RID: 20185 RVA: 0x001C9264 File Offset: 0x001C7464
	private static bool test(bool use_Rijndael, bool use_Xor)
	{
		bool flag = true;
		bool useRijndael = CryptoPlayerPrefs._useRijndael;
		bool useXor = CryptoPlayerPrefs._useXor;
		CryptoPlayerPrefs.useRijndael(use_Rijndael);
		CryptoPlayerPrefs.useXor(use_Xor);
		int num = 0;
		string text = "cryptotest_int";
		string text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetInt(text, num);
		int @int = CryptoPlayerPrefs.GetInt(text, 0);
		bool flag2 = num == @int;
		flag = (flag && flag2);
		Debug.Log("INT Bordertest Zero: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@int,
			")"
		}));
		num = int.MaxValue;
		text = "cryptotest_intmax";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetInt(text, num);
		@int = CryptoPlayerPrefs.GetInt(text, 0);
		flag2 = (num == @int);
		flag = (flag && flag2);
		Debug.Log("INT Bordertest Max: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@int,
			")"
		}));
		num = int.MinValue;
		text = "cryptotest_intmin";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetInt(text, num);
		@int = CryptoPlayerPrefs.GetInt(text, 0);
		flag2 = (num == @int);
		flag = (flag && flag2);
		Debug.Log("INT Bordertest Min: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@int,
			")"
		}));
		text = "cryptotest_intrand";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		bool flag3 = true;
		for (int i = 0; i < 100; i++)
		{
			int num2 = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			num = num2;
			CryptoPlayerPrefs.SetInt(text, num);
			@int = CryptoPlayerPrefs.GetInt(text, 0);
			flag2 = (num == @int);
			flag3 = (flag3 && flag2);
			flag = (flag && flag2);
		}
		Debug.Log("INT Test Random: " + ((!flag3) ? "fail" : "ok"));
		float num3 = 0f;
		text = "cryptotest_float";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetFloat(text, num3);
		float @float = CryptoPlayerPrefs.GetFloat(text, 0f);
		flag2 = num3.ToString().Equals(@float.ToString());
		flag = (flag && flag2);
		Debug.Log("FLOAT Bordertest Zero: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num3,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@float,
			")"
		}));
		num3 = float.MaxValue;
		text = "cryptotest_floatmax";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetFloat(text, num3);
		@float = CryptoPlayerPrefs.GetFloat(text, 0f);
		flag2 = num3.ToString().Equals(@float.ToString());
		flag = (flag && flag2);
		Debug.Log("FLOAT Bordertest Max: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num3,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@float,
			")"
		}));
		num3 = float.MinValue;
		text = "cryptotest_floatmin";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		CryptoPlayerPrefs.SetFloat(text, num3);
		@float = CryptoPlayerPrefs.GetFloat(text, 0f);
		flag2 = num3.ToString().Equals(@float.ToString());
		flag = (flag && flag2);
		Debug.Log("FLOAT Bordertest Min: " + ((!flag2) ? "fail" : "ok"));
		Debug.Log(string.Concat(new object[]
		{
			"(Key: ",
			text,
			"; Crypted Key: ",
			text2,
			"; Input value: ",
			num3,
			"; Saved value: ",
			PlayerPrefs.GetString(text2),
			"; Return value: ",
			@float,
			")"
		}));
		text = "cryptotest_floatrand";
		text2 = CryptoPlayerPrefs.hashedKey(text);
		flag3 = true;
		for (int j = 0; j < 100; j++)
		{
			float num4 = (float)UnityEngine.Random.Range(int.MinValue, int.MaxValue) * UnityEngine.Random.value;
			num3 = num4;
			CryptoPlayerPrefs.SetFloat(text, num3);
			@float = CryptoPlayerPrefs.GetFloat(text, 0f);
			flag2 = num3.ToString().Equals(@float.ToString());
			flag3 = (flag3 && flag2);
			flag = (flag && flag2);
		}
		Debug.Log("FLOAT Test Random: " + ((!flag3) ? "fail" : "ok"));
		CryptoPlayerPrefs.DeleteKey("cryptotest_int");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intmax");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intmin");
		CryptoPlayerPrefs.DeleteKey("cryptotest_intrandom");
		CryptoPlayerPrefs.DeleteKey("cryptotest_float");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatmax");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatmin");
		CryptoPlayerPrefs.DeleteKey("cryptotest_floatrandom");
		CryptoPlayerPrefs.useRijndael(useRijndael);
		CryptoPlayerPrefs.useXor(useXor);
		return flag;
	}

	// Token: 0x06004EDA RID: 20186 RVA: 0x001C98E0 File Offset: 0x001C7AE0
	public static bool test()
	{
		bool flag = CryptoPlayerPrefs.test(true, true);
		bool flag2 = CryptoPlayerPrefs.test(true, false);
		bool flag3 = CryptoPlayerPrefs.test(false, true);
		bool flag4 = CryptoPlayerPrefs.test(false, false);
		return flag && flag2 && flag3 && flag4;
	}

	// Token: 0x17000CC5 RID: 3269
	// (get) Token: 0x06004EDB RID: 20187 RVA: 0x001C9928 File Offset: 0x001C7B28
	private static string SaltString
	{
		get
		{
			if (string.IsNullOrEmpty(CryptoPlayerPrefs._saltString))
			{
				CryptoPlayerPrefs._saltString = CryptoPlayerPrefs.salt.ToString();
			}
			return CryptoPlayerPrefs._saltString;
		}
	}

	// Token: 0x06004EDC RID: 20188 RVA: 0x001C9950 File Offset: 0x001C7B50
	public static void setSalt(int s)
	{
		CryptoPlayerPrefs.salt = s;
		CryptoPlayerPrefs._saltString = s.ToString();
	}

	// Token: 0x06004EDD RID: 20189 RVA: 0x001C9964 File Offset: 0x001C7B64
	public static void useRijndael(bool use)
	{
		CryptoPlayerPrefs._useRijndael = use;
	}

	// Token: 0x06004EDE RID: 20190 RVA: 0x001C996C File Offset: 0x001C7B6C
	public static void useXor(bool use)
	{
		CryptoPlayerPrefs._useXor = use;
	}

	// Token: 0x04003D4A RID: 15690
	private static Dictionary<string, string> keyHashs;

	// Token: 0x04003D4B RID: 15691
	private static Dictionary<string, int> xorOperands;

	// Token: 0x04003D4C RID: 15692
	private static HashAlgorithm _hashAlgorithm;

	// Token: 0x04003D4D RID: 15693
	private static Dictionary<string, SymmetricAlgorithm> rijndaelDict;

	// Token: 0x04003D4E RID: 15694
	private static int salt = int.MaxValue;

	// Token: 0x04003D4F RID: 15695
	private static string _saltString;

	// Token: 0x04003D50 RID: 15696
	private static bool _useRijndael = true;

	// Token: 0x04003D51 RID: 15697
	private static bool _useXor = true;
}
