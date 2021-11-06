using System;
using System.Globalization;
using Rilisoft;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class RenderAllInSceneObj : MonoBehaviour
{
	// Token: 0x06002B9D RID: 11165 RVA: 0x000E5818 File Offset: 0x000E3A18
	private void Awake()
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0}.Awake()", new object[]
		{
			base.GetType().Name
		});
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			if (Device.IsLoweMemoryDevice)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				Transform transform = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("RenderAllInSceneObjInner")).transform;
				transform.parent = base.transform;
				transform.localPosition = Vector3.zero;
			}
		}
	}
}
