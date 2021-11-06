using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200055E RID: 1374
	[RequireComponent(typeof(Collider))]
	[ExecuteInEditMode]
	public abstract class AreaBase : MonoBehaviour
	{
		// Token: 0x06002FBF RID: 12223 RVA: 0x000F9950 File Offset: 0x000F7B50
		protected virtual void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (component == null)
			{
				throw new Exception("Collider not found");
			}
			if (!component.isTrigger)
			{
				Debug.LogWarningFormat("[AREA SYSTEM] collider now is trigger, go:'{0}'", new object[]
				{
					base.gameObject.name
				});
				component.isTrigger = true;
			}
			int num = LayerMask.NameToLayer("Ignore Raycast");
			if (base.gameObject.layer != num)
			{
				base.gameObject.layer = num;
			}
			if (!base.gameObject.CompareTag("Area"))
			{
				base.gameObject.tag = "Area";
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000F99F8 File Offset: 0x000F7BF8
		public virtual void CheckIn(GameObject to)
		{
			this._isActive = true;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000F9A04 File Offset: 0x000F7C04
		public virtual void CheckOut(GameObject from)
		{
			this._isActive = false;
		}

		// Token: 0x04002316 RID: 8982
		public const string AREA_OBJECT_TAG = "Area";

		// Token: 0x04002317 RID: 8983
		public const string AREA_OBJECT_LAYER = "Ignore Raycast";

		// Token: 0x04002318 RID: 8984
		[ReadOnly]
		[SerializeField]
		private bool _isActive;

		// Token: 0x04002319 RID: 8985
		[SerializeField]
		private string _description;
	}
}
