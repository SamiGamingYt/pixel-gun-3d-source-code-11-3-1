using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Linq
{
	// Token: 0x020002E5 RID: 741
	public class GameObjectBuilder
	{
		// Token: 0x060019D9 RID: 6617 RVA: 0x00068858 File Offset: 0x00066A58
		public GameObjectBuilder(GameObject original, params GameObjectBuilder[] children) : this(original, children)
		{
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00068864 File Offset: 0x00066A64
		public GameObjectBuilder(GameObject original, IEnumerable<GameObjectBuilder> children)
		{
			this.original = original;
			this.children = children;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x0006887C File Offset: 0x00066A7C
		public GameObject Instantiate()
		{
			return this.Instantiate(TransformCloneType.KeepOriginal);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00068888 File Offset: 0x00066A88
		public GameObject Instantiate(bool setActive)
		{
			return this.Instantiate(TransformCloneType.KeepOriginal);
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00068894 File Offset: 0x00066A94
		public GameObject Instantiate(TransformCloneType cloneType)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.original);
			this.InstantiateChildren(gameObject, cloneType, null);
			return gameObject;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x000688C0 File Offset: 0x00066AC0
		public GameObject Instantiate(TransformCloneType cloneType, bool setActive)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.original);
			this.InstantiateChildren(gameObject, cloneType, new bool?(setActive));
			return gameObject;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000688E8 File Offset: 0x00066AE8
		private void InstantiateChildren(GameObject root, TransformCloneType cloneType, bool? setActive)
		{
			foreach (GameObjectBuilder gameObjectBuilder in this.children)
			{
				GameObject root2 = root.Add(gameObjectBuilder.original, cloneType, setActive, null);
				gameObjectBuilder.InstantiateChildren(root2, cloneType, setActive);
			}
		}

		// Token: 0x04000F2C RID: 3884
		private readonly GameObject original;

		// Token: 0x04000F2D RID: 3885
		private readonly IEnumerable<GameObjectBuilder> children;
	}
}
