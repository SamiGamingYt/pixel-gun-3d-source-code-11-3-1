using System;

namespace Facebook.Unity.Mobile.IOS
{
	// Token: 0x020000E6 RID: 230
	internal class IOSFacebookLoader : FB.CompiledFacebookLoader
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0002E3DC File Offset: 0x0002C5DC
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				IOSFacebookGameObject component = ComponentFactory.GetComponent<IOSFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				if (component.Facebook == null)
				{
					component.Facebook = new IOSFacebook();
				}
				return component;
			}
		}
	}
}
