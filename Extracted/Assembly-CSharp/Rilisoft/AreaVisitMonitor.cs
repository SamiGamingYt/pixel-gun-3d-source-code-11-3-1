using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200055D RID: 1373
	[RequireComponent(typeof(Collider))]
	public class AreaVisitMonitor : MonoBehaviour
	{
		// Token: 0x06002FB8 RID: 12216 RVA: 0x000F9820 File Offset: 0x000F7A20
		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				throw new Exception("Collider not found");
			}
			if (!component.isTrigger)
			{
				Debug.LogWarningFormat("[AREA SYSTEM] collider now is trigger go:'{0}'", new object[]
				{
					base.gameObject.name
				});
				component.isTrigger = true;
			}
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x000F987C File Offset: 0x000F7A7C
		private void OnTriggerEnter(Collider other)
		{
			AreaBase component = other.GetComponent<AreaBase>();
			if (component == null)
			{
				return;
			}
			this._activeAreas.Add(component);
			component.CheckIn(base.gameObject);
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000F98B8 File Offset: 0x000F7AB8
		private void OnTriggerExit(Collider other)
		{
			AreaBase component = other.GetComponent<AreaBase>();
			if (component == null)
			{
				return;
			}
			if (this._activeAreas.Contains(component))
			{
				this._activeAreas.Remove(component);
			}
			component.CheckOut(base.gameObject);
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000F9904 File Offset: 0x000F7B04
		private void OnDisable()
		{
			this._activeAreas.ForEach(delegate(AreaBase a)
			{
				a.CheckOut(base.gameObject);
			});
			this._activeAreas.Clear();
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000F9934 File Offset: 0x000F7B34
		private void Update()
		{
		}

		// Token: 0x04002315 RID: 8981
		[SerializeField]
		[ReadOnly]
		private List<AreaBase> _activeAreas = new List<AreaBase>();
	}
}
