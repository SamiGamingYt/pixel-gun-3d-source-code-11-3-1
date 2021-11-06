using System;

namespace Facebook.Unity.Mobile.Android
{
	// Token: 0x020000DB RID: 219
	internal class AndroidFacebookLoader : FB.CompiledFacebookLoader
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0002DCE0 File Offset: 0x0002BEE0
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				AndroidFacebookGameObject component = ComponentFactory.GetComponent<AndroidFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				if (component.Facebook == null)
				{
					component.Facebook = new AndroidFacebook();
				}
				return component;
			}
		}
	}
}
