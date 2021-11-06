using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class CheckLVLButton : MonoBehaviour
{
	// Token: 0x06000B83 RID: 2947 RVA: 0x00040D40 File Offset: 0x0003EF40
	private void Start()
	{
		Debug.Log("private string m_PublicKey_Modulus_Base64 = \"" + this.m_PublicKey_Modulus_Base64 + "\";");
		Debug.Log("private string m_PublicKey_Exponent_Base64 = \"" + this.m_PublicKey_Exponent_Base64 + "\";");
		this.m_PublicKey.Modulus = Convert.FromBase64String(this.m_PublicKey_Modulus_Base64);
		this.m_PublicKey.Exponent = Convert.FromBase64String(this.m_PublicKey_Exponent_Base64);
		this.m_RunningOnAndroid = (new AndroidJavaClass("android.os.Build").GetRawClass() != IntPtr.Zero);
		if (!this.m_RunningOnAndroid)
		{
			return;
		}
		this.LoadServiceBinder();
		new SHA1CryptoServiceProvider();
		this.m_ButtonMessage = "Check LVL";
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00040DF0 File Offset: 0x0003EFF0
	private void LoadServiceBinder()
	{
		byte[] bytes = this.ServiceBinder.bytes;
		this.m_Activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		this.m_PackageName = this.m_Activity.Call<string>("getPackageName", new object[0]);
		string text = Path.Combine(this.m_Activity.Call<AndroidJavaObject>("getCacheDir", new object[0]).Call<string>("getPath", new object[0]), this.m_PackageName);
		Directory.CreateDirectory(text);
		File.WriteAllBytes(text + "/classes.jar", bytes);
		Directory.CreateDirectory(text + "/odex");
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("dalvik.system.DexClassLoader", new object[]
		{
			text + "/classes.jar",
			text + "/odex",
			null,
			this.m_Activity.Call<AndroidJavaObject>("getClassLoader", new object[0])
		});
		this.m_LVLCheckType = androidJavaObject.Call<AndroidJavaObject>("findClass", new object[]
		{
			"com.unity3d.plugin.lvl.ServiceBinder"
		});
		Directory.Delete(text, true);
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00040F08 File Offset: 0x0003F108
	private void OnGUI()
	{
		if (!this.m_RunningOnAndroid)
		{
			GUI.Label(new Rect(10f, 10f, (float)(Screen.width - 10), 20f), "Use LVL checks only on the Android device!");
			return;
		}
		GUI.enabled = this.m_ButtonEnabled;
		if (GUI.Button(new Rect(10f, 10f, 450f, 300f), this.m_ButtonMessage))
		{
			this.m_Nonce = new System.Random().Next();
			object[] args = new object[]
			{
				new AndroidJavaObject[]
				{
					this.m_Activity
				}
			};
			AndroidJavaObject[] array = this.m_LVLCheckType.Call<AndroidJavaObject[]>("getConstructors", new object[0]);
			this.m_LVLCheck = array[0].Call<AndroidJavaObject>("newInstance", args);
			this.m_LVLCheck.Call("create", new object[]
			{
				this.m_Nonce,
				new AndroidJavaRunnable(this.Process)
			});
			this.m_ButtonMessage = "Checking...";
			this.m_ButtonEnabled = false;
		}
		GUI.enabled = true;
		if (this.m_LVLCheck != null || this.m_LVL_Received)
		{
			GUI.Label(new Rect(10f, 320f, 450f, 20f), "Requesting LVL response:");
			GUI.Label(new Rect(20f, 340f, 450f, 20f), "Package name  = " + this.m_PackageName);
			GUI.Label(new Rect(20f, 360f, 450f, 20f), "Request nonce = 0x" + this.m_Nonce.ToString("X"));
		}
		if (this.m_LVLCheck == null && this.m_LVL_Received)
		{
			GUI.Label(new Rect(10f, 420f, 450f, 20f), "Received LVL response:");
			GUI.Label(new Rect(20f, 440f, 450f, 20f), "Response code  = " + this.m_ResponseCode_Received);
			GUI.Label(new Rect(20f, 460f, 450f, 20f), "Package name   = " + this.m_PackageName_Received);
			GUI.Label(new Rect(20f, 480f, 450f, 20f), "Received nonce = 0x" + this.m_Nonce_Received.ToString("X"));
			GUI.Label(new Rect(20f, 500f, 450f, 20f), "Version code = " + this.m_VersionCode_Received);
			GUI.Label(new Rect(20f, 520f, 450f, 20f), "User ID   = " + this.m_UserID_Received);
			GUI.Label(new Rect(20f, 540f, 450f, 20f), "Timestamp = " + this.m_Timestamp_Received);
			GUI.Label(new Rect(20f, 560f, 450f, 20f), "Max Retry = " + this.m_MaxRetry_Received);
			GUI.Label(new Rect(20f, 580f, 450f, 20f), "License Validity = " + this.m_LicenceValidityTimestamp_Received);
			GUI.Label(new Rect(20f, 600f, 450f, 20f), "Grace Period = " + this.m_GracePeriodTimestamp_Received);
			GUI.Label(new Rect(20f, 620f, 450f, 20f), "Update Since = " + this.m_UpdateTimestamp_Received);
			GUI.Label(new Rect(20f, 640f, 450f, 20f), "Main OBB URL = " + this.m_FileURL1_Received.Substring(0, Mathf.Min(this.m_FileURL1_Received.Length, 50)) + "...");
			GUI.Label(new Rect(20f, 660f, 450f, 20f), "Main OBB Name = " + this.m_FileName1_Received);
			GUI.Label(new Rect(20f, 680f, 450f, 20f), "Main OBB Size = " + this.m_FileSize1_Received);
			GUI.Label(new Rect(20f, 700f, 450f, 20f), "Patch OBB URL = " + this.m_FileURL2_Received.Substring(0, Mathf.Min(this.m_FileURL2_Received.Length, 50)) + "...");
			GUI.Label(new Rect(20f, 720f, 450f, 20f), "Patch OBB Name = " + this.m_FileName2_Received);
			GUI.Label(new Rect(20f, 740f, 450f, 20f), "Patch OBB Size = " + this.m_FileSize2_Received);
		}
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0004142C File Offset: 0x0003F62C
	internal static Dictionary<string, string> DecodeExtras(string query)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (query.Length == 0)
		{
			return dictionary;
		}
		int length = query.Length;
		int i = 0;
		bool flag = true;
		while (i <= length)
		{
			int num = -1;
			int num2 = -1;
			for (int j = i; j < length; j++)
			{
				if (num == -1 && query[j] == '=')
				{
					num = j + 1;
				}
				else if (query[j] == '&')
				{
					num2 = j;
					break;
				}
			}
			if (flag)
			{
				flag = false;
				if (query[i] == '?')
				{
					i++;
				}
			}
			string key;
			if (num == -1)
			{
				key = null;
				num = i;
			}
			else
			{
				key = WWW.UnEscapeURL(query.Substring(i, num - i - 1));
			}
			if (num2 < 0)
			{
				i = -1;
				num2 = query.Length;
			}
			else
			{
				i = num2 + 1;
			}
			string value = WWW.UnEscapeURL(query.Substring(num, num2 - num));
			dictionary.Add(key, value);
			if (i == -1)
			{
				break;
			}
		}
		return dictionary;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00041548 File Offset: 0x0003F748
	private long ConvertEpochSecondsToTicks(long secs)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		long num = 10000L;
		long num2 = (DateTime.MaxValue.Ticks - dateTime.Ticks) / num;
		if (secs < 0L)
		{
			secs = 0L;
		}
		if (secs > num2)
		{
			secs = num2;
		}
		return dateTime.Ticks + secs * num;
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000415A8 File Offset: 0x0003F7A8
	private void Process()
	{
		this.m_LVL_Received = true;
		this.m_ButtonMessage = "Check LVL";
		this.m_ButtonEnabled = true;
		if (this.m_LVLCheck == null)
		{
			return;
		}
		int num = this.m_LVLCheck.Get<int>("_arg0");
		string text = this.m_LVLCheck.Get<string>("_arg1");
		string text2 = this.m_LVLCheck.Get<string>("_arg2");
		this.m_LVLCheck = null;
		this.m_ResponseCode_Received = num.ToString();
		if (num < 0 || string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
		{
			this.m_PackageName_Received = "<Failed>";
			return;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] rgbSignature = Convert.FromBase64String(text2);
		RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
		rsacryptoServiceProvider.ImportParameters(this.m_PublicKey);
		SHA1Managed sha1Managed = new SHA1Managed();
		if (!rsacryptoServiceProvider.VerifyHash(sha1Managed.ComputeHash(bytes), CryptoConfig.MapNameToOID("SHA1"), rgbSignature))
		{
			this.m_ResponseCode_Received = "<Failed>";
			this.m_PackageName_Received = "<Invalid Signature>";
			return;
		}
		int num2 = text.IndexOf(':');
		string text3;
		string text4;
		if (num2 == -1)
		{
			text3 = text;
			text4 = string.Empty;
		}
		else
		{
			text3 = text.Substring(0, num2);
			text4 = ((num2 < text.Length) ? text.Substring(num2 + 1) : string.Empty);
		}
		string[] array = text3.Split(new char[]
		{
			'|'
		});
		if (array[0].CompareTo(num.ToString()) != 0)
		{
			this.m_ResponseCode_Received = "<Failed>";
			this.m_PackageName_Received = "<Response Mismatch>";
			return;
		}
		this.m_ResponseCode_Received = array[0];
		this.m_Nonce_Received = Convert.ToInt32(array[1]);
		this.m_PackageName_Received = array[2];
		this.m_VersionCode_Received = Convert.ToInt32(array[3]);
		this.m_UserID_Received = array[4];
		long ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(array[5]));
		DateTime dateTime = new DateTime(ticks);
		this.m_Timestamp_Received = dateTime.ToLocalTime().ToString();
		if (!string.IsNullOrEmpty(text4))
		{
			Dictionary<string, string> dictionary = CheckLVLButton.DecodeExtras(text4);
			if (dictionary.ContainsKey("GR"))
			{
				this.m_MaxRetry_Received = Convert.ToInt32(dictionary["GR"]);
			}
			else
			{
				this.m_MaxRetry_Received = 0;
			}
			if (dictionary.ContainsKey("VT"))
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(dictionary["VT"]));
				DateTime dateTime2 = new DateTime(ticks);
				this.m_LicenceValidityTimestamp_Received = dateTime2.ToLocalTime().ToString();
			}
			else
			{
				this.m_LicenceValidityTimestamp_Received = null;
			}
			if (dictionary.ContainsKey("GT"))
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(dictionary["GT"]));
				DateTime dateTime3 = new DateTime(ticks);
				this.m_GracePeriodTimestamp_Received = dateTime3.ToLocalTime().ToString();
			}
			else
			{
				this.m_GracePeriodTimestamp_Received = null;
			}
			if (dictionary.ContainsKey("UT"))
			{
				ticks = this.ConvertEpochSecondsToTicks(Convert.ToInt64(dictionary["UT"]));
				DateTime dateTime4 = new DateTime(ticks);
				this.m_UpdateTimestamp_Received = dateTime4.ToLocalTime().ToString();
			}
			else
			{
				this.m_UpdateTimestamp_Received = null;
			}
			if (dictionary.ContainsKey("FILE_URL1"))
			{
				this.m_FileURL1_Received = dictionary["FILE_URL1"];
			}
			else
			{
				this.m_FileURL1_Received = string.Empty;
			}
			if (dictionary.ContainsKey("FILE_URL2"))
			{
				this.m_FileURL2_Received = dictionary["FILE_URL2"];
			}
			else
			{
				this.m_FileURL2_Received = string.Empty;
			}
			if (dictionary.ContainsKey("FILE_NAME1"))
			{
				this.m_FileName1_Received = dictionary["FILE_NAME1"];
			}
			else
			{
				this.m_FileName1_Received = null;
			}
			if (dictionary.ContainsKey("FILE_NAME2"))
			{
				this.m_FileName2_Received = dictionary["FILE_NAME2"];
			}
			else
			{
				this.m_FileName2_Received = null;
			}
			if (dictionary.ContainsKey("FILE_SIZE1"))
			{
				this.m_FileSize1_Received = Convert.ToInt32(dictionary["FILE_SIZE1"]);
			}
			else
			{
				this.m_FileSize1_Received = 0;
			}
			if (dictionary.ContainsKey("FILE_SIZE2"))
			{
				this.m_FileSize2_Received = Convert.ToInt32(dictionary["FILE_SIZE2"]);
			}
			else
			{
				this.m_FileSize2_Received = 0;
			}
		}
	}

	// Token: 0x0400091E RID: 2334
	public TextAsset ServiceBinder;

	// Token: 0x0400091F RID: 2335
	private string m_PublicKey_Modulus_Base64 = "AKE8zE2qrBYWssLwhSmsBcC+SjPnYyyXzgk/62xkEXx5h2JjmNrATIA1rAP+u0s8ehFU2T1ZRcxzIJAKtU8HS/wOWmBqRs2gl+6PenMqfEesEhfsQro/viwJE2g5y1tL3iNm5HtxFR7twLfcYDOOnvlQ7nEVKkC73+kkEW9eZ7n5WOYtPMSp3sIF+yOFNh9EbwGQ8qIqfqmT5iOGb2rnwH3NI8GW8O1xoleNc4m2Ny1NDee5mCpOdVsfrTmie05HPUZdalQk42/m8F7IU6oVV1T+q+JGmy1sP/DiVIdEpuvZW6bOmpj+7z8ue9V47HAkzC310Gp9fefax2zYJG9piy0=";

	// Token: 0x04000920 RID: 2336
	private string m_PublicKey_Exponent_Base64 = "AQAB";

	// Token: 0x04000921 RID: 2337
	private RSAParameters m_PublicKey = default(RSAParameters);

	// Token: 0x04000922 RID: 2338
	private bool m_RunningOnAndroid;

	// Token: 0x04000923 RID: 2339
	private AndroidJavaObject m_Activity;

	// Token: 0x04000924 RID: 2340
	private AndroidJavaObject m_LVLCheckType;

	// Token: 0x04000925 RID: 2341
	private AndroidJavaObject m_LVLCheck;

	// Token: 0x04000926 RID: 2342
	private string m_ButtonMessage = "Invalid LVL key!\nCheck the source...";

	// Token: 0x04000927 RID: 2343
	private bool m_ButtonEnabled = true;

	// Token: 0x04000928 RID: 2344
	private string m_PackageName;

	// Token: 0x04000929 RID: 2345
	private int m_Nonce;

	// Token: 0x0400092A RID: 2346
	private bool m_LVL_Received;

	// Token: 0x0400092B RID: 2347
	private string m_ResponseCode_Received;

	// Token: 0x0400092C RID: 2348
	private string m_PackageName_Received;

	// Token: 0x0400092D RID: 2349
	private int m_Nonce_Received;

	// Token: 0x0400092E RID: 2350
	private int m_VersionCode_Received;

	// Token: 0x0400092F RID: 2351
	private string m_UserID_Received;

	// Token: 0x04000930 RID: 2352
	private string m_Timestamp_Received;

	// Token: 0x04000931 RID: 2353
	private int m_MaxRetry_Received;

	// Token: 0x04000932 RID: 2354
	private string m_LicenceValidityTimestamp_Received;

	// Token: 0x04000933 RID: 2355
	private string m_GracePeriodTimestamp_Received;

	// Token: 0x04000934 RID: 2356
	private string m_UpdateTimestamp_Received;

	// Token: 0x04000935 RID: 2357
	private string m_FileURL1_Received = string.Empty;

	// Token: 0x04000936 RID: 2358
	private string m_FileURL2_Received = string.Empty;

	// Token: 0x04000937 RID: 2359
	private string m_FileName1_Received;

	// Token: 0x04000938 RID: 2360
	private string m_FileName2_Received;

	// Token: 0x04000939 RID: 2361
	private int m_FileSize1_Received;

	// Token: 0x0400093A RID: 2362
	private int m_FileSize2_Received;
}
