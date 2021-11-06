using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002C6 RID: 710
	public class ResourceManager : MonoBehaviour
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x0005B830 File Offset: 0x00059A30
		public static ResourceManager pInstance
		{
			get
			{
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[]
					{
						typeof(ResourceManager)
					});
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
				}
				if (Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x0005B8CC File Offset: 0x00059ACC
		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x0005B908 File Offset: 0x00059B08
		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x0005B970 File Offset: 0x00059B70
		public bool HasAsset(UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x0005B994 File Offset: 0x00059B94
		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			UnityEngine.Object @object;
			if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
			{
				return @object as T;
			}
			T t = Resources.Load<T>(Path);
			this.mResourcesCache[Path] = t;
			if (!this.mCleaningScheduled)
			{
				base.Invoke("CleanResourceCache", 0.1f);
				this.mCleaningScheduled = true;
			}
			return t;
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0005BA08 File Offset: 0x00059C08
		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			base.CancelInvoke();
			this.mCleaningScheduled = false;
		}

		// Token: 0x04000D30 RID: 3376
		private static ResourceManager mInstance;

		// Token: 0x04000D31 RID: 3377
		public UnityEngine.Object[] Assets;

		// Token: 0x04000D32 RID: 3378
		private Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>();

		// Token: 0x04000D33 RID: 3379
		private bool mCleaningScheduled;
	}
}
