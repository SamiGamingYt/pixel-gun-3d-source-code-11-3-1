using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200057A RID: 1402
	public class BindedBillboard : MonoBehaviour
	{
		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06003092 RID: 12434 RVA: 0x000FCDB8 File Offset: 0x000FAFB8
		public Collider Collider
		{
			get
			{
				if (this._touchCollider == null)
				{
					this._touchCollider = base.gameObject.GetComponent<Collider>();
					if (this._touchCollider == null)
					{
						this._touchCollider = base.GetComponentInChildren<Collider>();
					}
				}
				return this._touchCollider;
			}
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000FCE0C File Offset: 0x000FB00C
		public void BindTo(Func<Transform> point)
		{
			this._pointInWorld = null;
			this._pointGetter = point;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000FCE1C File Offset: 0x000FB01C
		private void Awake()
		{
			if (this._pointGetter == null && this._pointInWorld != null)
			{
				this._pointGetter = (() => this._pointInWorld);
			}
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000FCE58 File Offset: 0x000FB058
		private void Update()
		{
			if (this._pointInWorld != null)
			{
				this._pointGetter = (() => this._pointInWorld);
			}
			if (this._pointGetter == null || this._pointGetter() == null || NickLabelController.currentCamera == null)
			{
				return;
			}
			base.transform.OverlayPosition(this._pointGetter());
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
			if (this.Collider != null)
			{
				this.Collider.enabled = this._pointGetter().gameObject.activeSelf;
			}
		}

		// Token: 0x040023AA RID: 9130
		[SerializeField]
		private Transform _pointInWorld;

		// Token: 0x040023AB RID: 9131
		[SerializeField]
		private Collider _touchCollider;

		// Token: 0x040023AC RID: 9132
		private Func<Transform> _pointGetter;
	}
}
