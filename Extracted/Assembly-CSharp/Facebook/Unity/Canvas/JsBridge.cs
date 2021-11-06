using System;
using UnityEngine;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000C4 RID: 196
	internal class JsBridge : MonoBehaviour
	{
		// Token: 0x060005BA RID: 1466 RVA: 0x0002C3F8 File Offset: 0x0002A5F8
		public void Start()
		{
			this.facebook = ComponentFactory.GetComponent<CanvasFacebookGameObject>(ComponentFactory.IfNotExist.ReturnNull);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x0002C408 File Offset: 0x0002A608
		public void OnLoginComplete(string responseJsonData = "")
		{
			this.facebook.OnLoginComplete(responseJsonData);
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0002C418 File Offset: 0x0002A618
		public void OnFacebookAuthResponseChange(string responseJsonData = "")
		{
			this.facebook.OnFacebookAuthResponseChange(responseJsonData);
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x0002C428 File Offset: 0x0002A628
		public void OnPayComplete(string responseJsonData = "")
		{
			this.facebook.OnPayComplete(responseJsonData);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002C438 File Offset: 0x0002A638
		public void OnAppRequestsComplete(string responseJsonData = "")
		{
			this.facebook.OnAppRequestsComplete(responseJsonData);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0002C448 File Offset: 0x0002A648
		public void OnShareLinkComplete(string responseJsonData = "")
		{
			this.facebook.OnShareLinkComplete(responseJsonData);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002C458 File Offset: 0x0002A658
		public void OnGroupCreateComplete(string responseJsonData = "")
		{
			this.facebook.OnGroupCreateComplete(responseJsonData);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0002C468 File Offset: 0x0002A668
		public void OnJoinGroupComplete(string responseJsonData = "")
		{
			this.facebook.OnGroupJoinComplete(responseJsonData);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0002C478 File Offset: 0x0002A678
		public void OnFacebookFocus(string state)
		{
			this.facebook.OnHideUnity(state != "hide");
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0002C490 File Offset: 0x0002A690
		public void OnInitComplete(string responseJsonData = "")
		{
			this.facebook.OnInitComplete(responseJsonData);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0002C4A0 File Offset: 0x0002A6A0
		public void OnUrlResponse(string url = "")
		{
			this.facebook.OnUrlResponse(url);
		}

		// Token: 0x04000606 RID: 1542
		private ICanvasFacebookCallbackHandler facebook;
	}
}
