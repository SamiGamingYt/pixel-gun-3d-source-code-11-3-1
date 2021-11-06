using System;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
public class SampleWebView : MonoBehaviour
{
	// Token: 0x060047FE RID: 18430 RVA: 0x0018F420 File Offset: 0x0018D620
	private void Start()
	{
		this.webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
		this.webViewObject.Init(delegate(string msg)
		{
			Debug.Log(string.Format("CallFromJS[{0}]", msg));
		});
		this.webViewObject.LoadURL(this.Url);
		this.webViewObject.SetVisibility(true);
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.IPhonePlayer)
		{
			this.webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.Unity = {\t\tcall:function(msg) {\t\t\tvar iframe = document.createElement('IFRAME');\t\t\tiframe.setAttribute('src', 'unity:' + msg);\t\t\tdocument.documentElement.appendChild(iframe);\t\t\tiframe.parentNode.removeChild(iframe);\t\t\tiframe = null;\t\t}\t}}, false);");
		}
		this.webViewObject.EvaluateJS("window.addEventListener('load', function() {\twindow.addEventListener('click', function() {\t\tUnity.call('clicked');\t}, false);}, false);");
	}

	// Token: 0x0400354B RID: 13643
	public string Url;

	// Token: 0x0400354C RID: 13644
	private WebViewObject webViewObject;
}
