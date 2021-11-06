using System;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000C5 RID: 197
	internal class ComponentFactory
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0002C4B8 File Offset: 0x0002A6B8
		private static GameObject FacebookGameObject
		{
			get
			{
				if (ComponentFactory.facebookGameObject == null)
				{
					ComponentFactory.facebookGameObject = new GameObject("UnityFacebookSDKPlugin");
				}
				return ComponentFactory.facebookGameObject;
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0002C4EC File Offset: 0x0002A6EC
		public static T GetComponent<T>(ComponentFactory.IfNotExist ifNotExist = ComponentFactory.IfNotExist.AddNew) where T : MonoBehaviour
		{
			GameObject gameObject = ComponentFactory.FacebookGameObject;
			T t = gameObject.GetComponent<T>();
			if (t == null && ifNotExist == ComponentFactory.IfNotExist.AddNew)
			{
				t = gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0002C528 File Offset: 0x0002A728
		public static T AddComponent<T>() where T : MonoBehaviour
		{
			return ComponentFactory.FacebookGameObject.AddComponent<T>();
		}

		// Token: 0x04000607 RID: 1543
		public const string GameObjectName = "UnityFacebookSDKPlugin";

		// Token: 0x04000608 RID: 1544
		private static GameObject facebookGameObject;

		// Token: 0x020000C6 RID: 198
		internal enum IfNotExist
		{
			// Token: 0x0400060A RID: 1546
			AddNew,
			// Token: 0x0400060B RID: 1547
			ReturnNull
		}
	}
}
