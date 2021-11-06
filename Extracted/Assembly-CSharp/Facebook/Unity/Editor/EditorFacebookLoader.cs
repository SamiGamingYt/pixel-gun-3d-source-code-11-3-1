using System;

namespace Facebook.Unity.Editor
{
	// Token: 0x020000EE RID: 238
	internal class EditorFacebookLoader : FB.CompiledFacebookLoader
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0002EC44 File Offset: 0x0002CE44
		protected override FacebookGameObject FBGameObject
		{
			get
			{
				EditorFacebookGameObject component = ComponentFactory.GetComponent<EditorFacebookGameObject>(ComponentFactory.IfNotExist.AddNew);
				component.Facebook = new EditorFacebook();
				return component;
			}
		}
	}
}
