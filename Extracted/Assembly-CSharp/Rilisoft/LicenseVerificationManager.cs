using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200069D RID: 1693
	public class LicenseVerificationManager : MonoBehaviour, IDisposable
	{
		// Token: 0x06003B50 RID: 15184 RVA: 0x00133C68 File Offset: 0x00131E68
		private void OnDestroy()
		{
			this.Dispose();
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00133C70 File Offset: 0x00131E70
		private void Start()
		{
			Debug.LogFormat("> {0}.Start()", new object[]
			{
				base.GetType().Name
			});
			try
			{
				if (this.serviceBinder == null || string.IsNullOrEmpty(this.publicKeyModulusBase64) || string.IsNullOrEmpty(this.publicKeyExponentBase64))
				{
					Debug.LogWarning("Object not properly initialized.");
				}
				else
				{
					this._publicKey.Modulus = Convert.FromBase64String(this.publicKeyModulusBase64);
					this._publicKey.Exponent = Convert.FromBase64String(this.publicKeyExponentBase64);
					bool flag = false;
					try
					{
						if (Application.platform == RuntimePlatform.Android)
						{
							flag = (new AndroidJavaClass("android.os.Build").GetRawClass() != IntPtr.Zero);
						}
					}
					catch (Exception message)
					{
						Debug.LogWarning(message);
					}
					if (!flag)
					{
						Debug.LogWarning("Not running on Android.");
					}
					else
					{
						Debug.LogFormat("{0}.Start() > LoadServiceBinder()", new object[]
						{
							base.GetType().Name
						});
						try
						{
							this.LoadServiceBinder();
						}
						finally
						{
							Debug.LogFormat("{0}.Start() < LoadServiceBinder()", new object[]
							{
								base.GetType().Name
							});
						}
						this._disposed = false;
					}
				}
			}
			finally
			{
				Debug.LogFormat("< {0}.Start()", new object[]
				{
					base.GetType().Name
				});
			}
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x00133E14 File Offset: 0x00132014
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			Resources.UnloadAsset(this.serviceBinder);
			this.serviceBinder = null;
			if (this._activity != null)
			{
				this._activity.Dispose();
				this._activity = null;
			}
			if (this._lvlCheckType != null)
			{
				this._lvlCheckType.Dispose();
				this._lvlCheckType = null;
			}
			this._disposed = true;
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x00133E80 File Offset: 0x00132080
		public void Verify(Action<VerificationEventArgs> completionHandler)
		{
			if (this._disposed)
			{
				Debug.LogWarningFormat("Object disposed: {0}", new object[]
				{
					base.GetType().Name
				});
				return;
			}
			if (completionHandler == null)
			{
				Debug.LogWarning("Completion handler should not be null.");
				return;
			}
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			int nonce = this._prng.Next();
			object[] args = new object[]
			{
				new AndroidJavaObject[]
				{
					this._activity
				}
			};
			if (this._lvlCheckType == null)
			{
				Debug.LogWarning("LvlCheck is null.");
				return;
			}
			AndroidJavaObject[] array = this._lvlCheckType.Call<AndroidJavaObject[]>("getConstructors", new object[0]);
			AndroidJavaObject lvlCheck = array[0].Call<AndroidJavaObject>("newInstance", args);
			lvlCheck.Call("create", new object[]
			{
				nonce,
				new AndroidJavaRunnable(delegate()
				{
					this.Process(lvlCheck, nonce, completionHandler);
				})
			});
		}

		// Token: 0x06003B54 RID: 15188 RVA: 0x00133F8C File Offset: 0x0013218C
		private void Process(AndroidJavaObject lvlCheck, int nonce, Action<VerificationEventArgs> completionHandler)
		{
			Debug.LogFormat("> {0}.Process()", new object[]
			{
				base.GetType().Name
			});
			try
			{
				int num = lvlCheck.Get<int>("_arg0");
				string text = lvlCheck.Get<string>("_arg1");
				string text2 = lvlCheck.Get<string>("_arg2");
				VerificationEventArgs verificationEventArgs = new VerificationEventArgs
				{
					ReceivedResponseCode = (ResponseCode)num,
					SentNonce = nonce,
					SentPackageName = this._packageName
				};
				if (num < 0 || string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					verificationEventArgs.ErrorCode = VerificationErrorCode.BadResonceOrMessageOrSignature;
					completionHandler(verificationEventArgs);
				}
				else
				{
					try
					{
						byte[] bytes = Encoding.UTF8.GetBytes(text);
						byte[] rgbSignature = Convert.FromBase64String(text2);
						RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
						rsacryptoServiceProvider.ImportParameters(this._publicKey);
						SHA1Managed sha1Managed = new SHA1Managed();
						if (!rsacryptoServiceProvider.VerifyHash(sha1Managed.ComputeHash(bytes), CryptoConfig.MapNameToOID("SHA1"), rgbSignature))
						{
							verificationEventArgs.ErrorCode = VerificationErrorCode.InvalidSignature;
							completionHandler(verificationEventArgs);
							goto IL_1EE;
						}
					}
					catch (FormatException)
					{
						verificationEventArgs.ErrorCode = VerificationErrorCode.FormatError;
						completionHandler(verificationEventArgs);
						goto IL_1EE;
					}
					int num2 = text.IndexOf(':');
					string text3 = (num2 != -1) ? text.Substring(0, num2) : text;
					string[] array = text3.Split(new char[]
					{
						'|'
					});
					if (array.Length < 6)
					{
						verificationEventArgs.ErrorCode = VerificationErrorCode.InsufficientFieldCount;
						completionHandler(verificationEventArgs);
					}
					else
					{
						if (array[0].CompareTo(num.ToString()) == 0)
						{
							verificationEventArgs.ReceivedNonce = Convert.ToInt32(array[1]);
							verificationEventArgs.ReceivedPackageName = array[2];
							verificationEventArgs.ReceivedVersionCode = Convert.ToInt32(array[3]);
							verificationEventArgs.ReceivedUserId = array[4];
							verificationEventArgs.ReceivedTimestamp = Convert.ToInt64(array[5]);
							lvlCheck.Dispose();
							completionHandler(verificationEventArgs);
							return;
						}
						verificationEventArgs.ErrorCode = VerificationErrorCode.ResponceMismatch;
						completionHandler(verificationEventArgs);
					}
				}
				IL_1EE:
				Debug.LogWarningFormat("Response code: {0}    Message: '{1}'    Signature: '{2}'", new object[]
				{
					num,
					text,
					text2
				});
			}
			finally
			{
				Debug.LogFormat("< {0}.Process()", new object[]
				{
					base.GetType().Name
				});
			}
		}

		// Token: 0x06003B55 RID: 15189 RVA: 0x00134200 File Offset: 0x00132400
		private void LoadServiceBinder()
		{
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				return;
			}
			this._activity = AndroidSystem.Instance.CurrentActivity;
			this._packageName = this._activity.Call<string>("getPackageName", new object[0]);
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogFormat("{0}.LoadServiceBinder(): Skipping when target platform is Amazon.", new object[]
				{
					base.GetType().Name
				});
				return;
			}
			string text = Path.Combine(this._activity.Call<AndroidJavaObject>("getCacheDir", new object[0]).Call<string>("getPath", new object[0]), this._packageName);
			if (Defs.IsDeveloperBuild)
			{
				Debug.LogFormat("Cache directory: {0}", new object[]
				{
					text
				});
			}
			byte[] bytes = this.serviceBinder.bytes;
			Directory.CreateDirectory(text);
			File.WriteAllBytes(text + "/classes.jar", bytes);
			Directory.CreateDirectory(text + "/odex");
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("dalvik.system.DexClassLoader", new object[]
			{
				text + "/classes.jar",
				text + "/odex",
				null,
				this._activity.Call<AndroidJavaObject>("getClassLoader", new object[0])
			}))
			{
				this._lvlCheckType = androidJavaObject.Call<AndroidJavaObject>("findClass", new object[]
				{
					"com.unity3d.plugin.lvl.ServiceBinder"
				});
			}
			if (this._lvlCheckType == null)
			{
				Debug.Log("Could not instantiate ServiceBinder.");
				this.Dispose();
			}
			Directory.Delete(text, true);
		}

		// Token: 0x04002BE6 RID: 11238
		public TextAsset serviceBinder;

		// Token: 0x04002BE7 RID: 11239
		public string publicKeyModulusBase64;

		// Token: 0x04002BE8 RID: 11240
		public string publicKeyExponentBase64;

		// Token: 0x04002BE9 RID: 11241
		private static readonly SHA1 _dummy = new SHA1CryptoServiceProvider();

		// Token: 0x04002BEA RID: 11242
		private AndroidJavaObject _activity;

		// Token: 0x04002BEB RID: 11243
		private AndroidJavaObject _lvlCheckType;

		// Token: 0x04002BEC RID: 11244
		private bool _disposed = true;

		// Token: 0x04002BED RID: 11245
		private string _packageName = string.Empty;

		// Token: 0x04002BEE RID: 11246
		private readonly System.Random _prng = new System.Random();

		// Token: 0x04002BEF RID: 11247
		private RSAParameters _publicKey = default(RSAParameters);
	}
}
