using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000697 RID: 1687
public class LicenseVerificationController : MonoBehaviour
{
	// Token: 0x06003B2E RID: 15150 RVA: 0x001337A8 File Offset: 0x001319A8
	public LicenseVerificationController()
	{
		LicenseVerificationController <>f__this = this;
		bool startExecuted = false;
		this._start = delegate()
		{
			try
			{
				<>f__this._licenseVerificationManager = <>f__this.GetComponent<LicenseVerificationManager>();
				if (!(<>f__this._licenseVerificationManager == null))
				{
					if (Application.platform == RuntimePlatform.Android)
					{
						if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
						{
							UnityEngine.Object.DontDestroyOnLoad(<>f__this.gameObject);
							<>f__this.StartCoroutine(<>f__this.WaitThenVerifyLicenseCoroutine());
						}
					}
				}
			}
			finally
			{
				startExecuted = true;
			}
		};
		this._update = delegate()
		{
			if (!startExecuted)
			{
				SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(4086105548U));
			}
		};
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0013384C File Offset: 0x00131A4C
	private static string GetTerminalSceneName_f38d05cc(uint gamma)
	{
		return "Clf38d05ccosingScene".Replace(gamma.ToString("x"), string.Empty);
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0013386C File Offset: 0x00131A6C
	private void Start()
	{
		if (this._start == null)
		{
			SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(4086105548U));
		}
		else
		{
			this._start();
		}
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x001338A4 File Offset: 0x00131AA4
	private void Update()
	{
		if (this._update == null)
		{
			SceneManager.LoadScene(LicenseVerificationController.GetTerminalSceneName_f38d05cc(4086105548U));
		}
		else
		{
			this._update();
		}
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x001338DC File Offset: 0x00131ADC
	internal static LicenseVerificationController.PackageInfo GetPackageInfo()
	{
		LicenseVerificationController.PackageInfo packageInfo = default(LicenseVerificationController.PackageInfo);
		LicenseVerificationController.PackageInfo packageInfo2 = packageInfo;
		packageInfo2.PackageName = string.Empty;
		packageInfo2.SignatureHash = string.Empty;
		packageInfo = packageInfo2;
		try
		{
			AndroidJavaObject currentActivity = AndroidSystem.Instance.CurrentActivity;
			if (currentActivity == null)
			{
				Debug.LogWarning("activity == null");
				return packageInfo;
			}
			packageInfo.PackageName = (currentActivity.Call<string>("getPackageName", new object[0]) ?? string.Empty);
			AndroidJavaObject androidJavaObject = currentActivity.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			if (androidJavaObject == null)
			{
				Debug.LogWarning("manager == null");
				return packageInfo;
			}
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[]
			{
				packageInfo.PackageName,
				64
			});
			if (androidJavaObject2 == null)
			{
				Debug.LogWarning("packageInfo == null");
				return packageInfo;
			}
			AndroidJavaObject[] array = androidJavaObject2.Get<AndroidJavaObject[]>("signatures");
			if (array == null)
			{
				Debug.LogWarning("signatures == null");
				return packageInfo;
			}
			if (array.Length != 1)
			{
				Debug.LogWarning("signatures.Length == " + array.Length);
				return packageInfo;
			}
			AndroidJavaObject androidJavaObject3 = array[0];
			byte[] buffer = androidJavaObject3.Call<byte[]>("toByteArray", new object[0]);
			using (SHA1Managed sha1Managed = new SHA1Managed())
			{
				byte[] inArray = sha1Managed.ComputeHash(buffer);
				packageInfo.SignatureHash = Convert.ToBase64String(inArray);
			}
		}
		catch (Exception arg)
		{
			string message = string.Format("Error while retrieving Android package info:    {0}", arg);
			Debug.LogError(message);
		}
		return packageInfo;
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x00133AB8 File Offset: 0x00131CB8
	private static string GetErrorMessage(VerificationErrorCode errorCode)
	{
		string text;
		return (!LicenseVerificationController._errorMessages.TryGetValue(errorCode, out text)) ? "Unknown" : text;
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x00133AE4 File Offset: 0x00131CE4
	private void HandleVerificationResponse(VerificationEventArgs e)
	{
		string errorMessage = LicenseVerificationController.GetErrorMessage(e.ErrorCode);
		Debug.Log("HandleVerificationResponse(): Verification Error: " + errorMessage);
		if (e.ErrorCode == VerificationErrorCode.InvalidSignature)
		{
		}
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x00133B1C File Offset: 0x00131D1C
	private IEnumerator WaitThenVerifyLicenseCoroutine()
	{
		yield return new WaitForSeconds(20f);
		this._licenseVerificationManager.Verify(new Action<VerificationEventArgs>(this.HandleVerificationResponse));
		yield break;
	}

	// Token: 0x04002BC4 RID: 11204
	private static readonly IDictionary<VerificationErrorCode, string> _errorMessages = new Dictionary<VerificationErrorCode, string>
	{
		{
			VerificationErrorCode.None,
			"None"
		},
		{
			VerificationErrorCode.BadResonceOrMessageOrSignature,
			"Bad responce code, or message, or signature"
		},
		{
			VerificationErrorCode.InvalidSignature,
			"Invalid signature"
		},
		{
			VerificationErrorCode.InsufficientFieldCount,
			"Insufficient field count"
		},
		{
			VerificationErrorCode.ResponceMismatch,
			"Response mismatch"
		}
	};

	// Token: 0x04002BC5 RID: 11205
	private LicenseVerificationManager _licenseVerificationManager;

	// Token: 0x04002BC6 RID: 11206
	private readonly Action _start;

	// Token: 0x04002BC7 RID: 11207
	private readonly Action _update;

	// Token: 0x02000698 RID: 1688
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct PackageInfo
	{
		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003B37 RID: 15159 RVA: 0x00133B38 File Offset: 0x00131D38
		// (set) Token: 0x06003B38 RID: 15160 RVA: 0x00133B40 File Offset: 0x00131D40
		internal string PackageName { get; set; }

		// Token: 0x170009BB RID: 2491
		// (get) Token: 0x06003B39 RID: 15161 RVA: 0x00133B4C File Offset: 0x00131D4C
		// (set) Token: 0x06003B3A RID: 15162 RVA: 0x00133B54 File Offset: 0x00131D54
		internal string SignatureHash { get; set; }
	}

	// Token: 0x02000699 RID: 1689
	[Flags]
	private enum PackageInfoFlags
	{
		// Token: 0x04002BCB RID: 11211
		GetSignatures = 64
	}
}
