using System;
using System.IO;
using System.Net;
using UnityEngine;

// Token: 0x020002D0 RID: 720
internal sealed class InternetChecker : MonoBehaviour
{
	// Token: 0x06001954 RID: 6484 RVA: 0x000642B8 File Offset: 0x000624B8
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000642C8 File Offset: 0x000624C8
	public static void CheckForInternetConn()
	{
		string htmlFromUri = InternetChecker.GetHtmlFromUri("http://google.com");
		if (htmlFromUri == string.Empty)
		{
			InternetChecker.InternetAvailable = false;
		}
		else if (!htmlFromUri.Contains("schema.org/WebPage"))
		{
			InternetChecker.InternetAvailable = false;
		}
		else
		{
			InternetChecker.InternetAvailable = true;
		}
	}

	// Token: 0x06001956 RID: 6486 RVA: 0x0006431C File Offset: 0x0006251C
	public static string GetHtmlFromUri(string resource)
	{
		string text = string.Empty;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(resource);
		try
		{
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
			{
				bool flag = httpWebResponse.StatusCode < (HttpStatusCode)299 && httpWebResponse.StatusCode >= HttpStatusCode.OK;
				if (flag)
				{
					Debug.Log("Trying to check internet");
					using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
					{
						char[] array = new char[80];
						streamReader.Read(array, 0, array.Length);
						foreach (char c in array)
						{
							text += c;
						}
					}
				}
			}
		}
		catch
		{
			return string.Empty;
		}
		return text;
	}

	// Token: 0x04000E5D RID: 3677
	public static bool InternetAvailable;
}
