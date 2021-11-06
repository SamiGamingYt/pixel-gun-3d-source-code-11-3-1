using System;
using UnityEngine;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000BF RID: 191
	internal class CanvasJSWrapper : ICanvasJSWrapper
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0002C390 File Offset: 0x0002A590
		public string IntegrationMethodJs
		{
			get
			{
				TextAsset textAsset = Resources.Load("JSSDKBindings") as TextAsset;
				if (textAsset)
				{
					return textAsset.text;
				}
				return null;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
		public string GetSDKVersion()
		{
			return "v2.5";
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0002C3C8 File Offset: 0x0002A5C8
		public void ExternalCall(string functionName, params object[] args)
		{
			Application.ExternalCall(functionName, args);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		public void ExternalEval(string script)
		{
			Application.ExternalEval(script);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0002C3DC File Offset: 0x0002A5DC
		public void DisableFullScreen()
		{
			if (Screen.fullScreen)
			{
				Screen.fullScreen = false;
			}
		}

		// Token: 0x04000605 RID: 1541
		private const string JSSDKBindingFileName = "JSSDKBindings";
	}
}
