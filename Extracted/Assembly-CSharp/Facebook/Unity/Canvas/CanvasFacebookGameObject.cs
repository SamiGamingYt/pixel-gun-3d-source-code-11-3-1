using System;
using UnityEngine;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000BD RID: 189
	internal class CanvasFacebookGameObject : FacebookGameObject, ICanvasFacebookCallbackHandler, IFacebookCallbackHandler
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0002C2CC File Offset: 0x0002A4CC
		protected ICanvasFacebookImplementation CanvasFacebookImpl
		{
			get
			{
				return (ICanvasFacebookImplementation)base.Facebook;
			}
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0002C2DC File Offset: 0x0002A4DC
		public void OnPayComplete(string result)
		{
			this.CanvasFacebookImpl.OnPayComplete(result);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0002C2EC File Offset: 0x0002A4EC
		public void OnFacebookAuthResponseChange(string message)
		{
			this.CanvasFacebookImpl.OnFacebookAuthResponseChange(message);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0002C2FC File Offset: 0x0002A4FC
		public void OnUrlResponse(string message)
		{
			this.CanvasFacebookImpl.OnUrlResponse(message);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0002C30C File Offset: 0x0002A50C
		public void OnHideUnity(bool hide)
		{
			this.CanvasFacebookImpl.OnHideUnity(hide);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0002C31C File Offset: 0x0002A51C
		protected override void OnAwake()
		{
			GameObject gameObject = new GameObject("FacebookJsBridge");
			gameObject.AddComponent<JsBridge>();
			gameObject.transform.parent = base.gameObject.transform;
		}
	}
}
