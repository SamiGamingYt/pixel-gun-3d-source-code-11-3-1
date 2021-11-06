using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000681 RID: 1665
	public class LazyObject<T> where T : MonoBehaviour
	{
		// Token: 0x060039E6 RID: 14822 RVA: 0x0012C06C File Offset: 0x0012A26C
		public LazyObject(GameObject prefab, GameObject attachTo)
		{
			this._prefab = prefab;
			this._attachTo = attachTo;
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x0012C084 File Offset: 0x0012A284
		public LazyObject(string resourcePath, GameObject attachTo)
		{
			this._resourcePath = resourcePath;
			this._attachTo = attachTo;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x060039E8 RID: 14824 RVA: 0x0012C09C File Offset: 0x0012A29C
		// (set) Token: 0x060039E9 RID: 14825 RVA: 0x0012C0E4 File Offset: 0x0012A2E4
		private GameObject _prefab
		{
			get
			{
				if (this._prefabVal == null && !this._resourcePath.IsNullOrEmpty())
				{
					this._prefabVal = Resources.Load<GameObject>(this._resourcePath);
				}
				return this._prefabVal;
			}
			set
			{
				this._prefabVal = value;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x0012C0F0 File Offset: 0x0012A2F0
		public T Value
		{
			get
			{
				if (this._value == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._prefab);
					this._value = gameObject.GetComponent<T>();
					if (this._attachTo != null)
					{
						gameObject.transform.SetParent(this._attachTo.transform);
						gameObject.transform.localPosition = this._attachTo.transform.localPosition;
						gameObject.transform.localScale = Vector3.one;
					}
				}
				return this._value;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060039EB RID: 14827 RVA: 0x0012C184 File Offset: 0x0012A384
		public bool HasValue
		{
			get
			{
				return this._value != null;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x0012C198 File Offset: 0x0012A398
		public bool ObjectIsLoaded
		{
			get
			{
				return this._value != null;
			}
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x0012C1AC File Offset: 0x0012A3AC
		public bool ObjectIsActive
		{
			get
			{
				return this.ObjectIsLoaded && this._value.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x0012C1E0 File Offset: 0x0012A3E0
		public void DestroyValue()
		{
			if (this._value != null)
			{
				UnityEngine.Object.DestroyImmediate(this._value.gameObject);
			}
		}

		// Token: 0x04002AA1 RID: 10913
		private readonly string _resourcePath;

		// Token: 0x04002AA2 RID: 10914
		private GameObject _prefabVal;

		// Token: 0x04002AA3 RID: 10915
		private T _value;

		// Token: 0x04002AA4 RID: 10916
		private readonly GameObject _attachTo;
	}
}
