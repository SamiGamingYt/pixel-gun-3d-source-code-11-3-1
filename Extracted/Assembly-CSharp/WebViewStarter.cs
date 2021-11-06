using System;
using UnityEngine;

// Token: 0x020007C7 RID: 1991
public class WebViewStarter
{
	// Token: 0x06004809 RID: 18441 RVA: 0x0018F780 File Offset: 0x0018D980
	public static WebViewObject StartBrowser(string Url)
	{
		WebViewObject webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
		webViewObject.Init(delegate(string msg)
		{
			Debug.Log(string.Format("CallFromJS[{0}]", msg));
		});
		webViewObject.LoadURL(Url);
		webViewObject.SetVisibility(true);
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.IPhonePlayer)
		{
			webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
		return webViewObject;
	}
}
