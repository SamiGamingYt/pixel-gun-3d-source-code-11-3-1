using System;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000BE RID: 190
	internal class CanvasFacebookLoader : FB.CompiledFacebookLoader
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0002C35C File Offset: 0x0002A55C
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				CanvasFacebookGameObject component = ComponentFactory.GetComponent<CanvasFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				if (component.Facebook == null)
				{
					component.Facebook = new CanvasFacebook();
				}
				return component;
			}
		}
	}
}
