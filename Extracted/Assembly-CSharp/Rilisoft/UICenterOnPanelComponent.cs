using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x02000778 RID: 1912
	public class UICenterOnPanelComponent : MonoBehaviour
	{
		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x0600432F RID: 17199 RVA: 0x00166DB8 File Offset: 0x00164FB8
		public Vector3 Center
		{
			get
			{
				Vector3[] worldCorners = this._panel.worldCorners;
				return (worldCorners[2] + worldCorners[0]) * 0.5f;
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06004330 RID: 17200 RVA: 0x00166DF8 File Offset: 0x00164FF8
		public CenterDirection CenterDirection
		{
			get
			{
				return (this.Center.x - base.transform.position.x <= 0f) ? CenterDirection.OnRight : CenterDirection.OnLeft;
			}
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x00166E38 File Offset: 0x00165038
		private void Awake()
		{
			this._panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			this.OnCentered = (this.OnCentered ?? new UnityEvent());
			this.OnCenteredLoss = (this.OnCenteredLoss ?? new UnityEvent());
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x00166E88 File Offset: 0x00165088
		private void OnEnable()
		{
			this._centered = false;
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x00166E94 File Offset: 0x00165094
		protected virtual void Update()
		{
			if (this._panel == null)
			{
				this._panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			}
			if (this._panel == null)
			{
				return;
			}
			this.Offset = new Vector2(Mathf.Abs(this.Center.x - base.transform.position.x), Mathf.Abs(this.Center.y - base.transform.position.y));
			float num = (this.Direction != Direction.Horizontal) ? this.Offset.y : this.Offset.x;
			if (num <= this.Slack)
			{
				if (!this._centered)
				{
					this._centered = true;
					if (this.OnCentered != null)
					{
						this.OnCentered.Invoke();
					}
				}
			}
			else if (this._centered)
			{
				this._centered = false;
				if (this.OnCenteredLoss != null)
				{
					this.OnCenteredLoss.Invoke();
				}
			}
		}

		// Token: 0x04003134 RID: 12596
		[SerializeField]
		[ReadOnly]
		protected UIPanel _panel;

		// Token: 0x04003135 RID: 12597
		[SerializeField]
		public Direction Direction = Direction.Horizontal;

		// Token: 0x04003136 RID: 12598
		[SerializeField]
		public float Slack = 0.1f;

		// Token: 0x04003137 RID: 12599
		[SerializeField]
		public UnityEvent OnCentered;

		// Token: 0x04003138 RID: 12600
		[SerializeField]
		public UnityEvent OnCenteredLoss;

		// Token: 0x04003139 RID: 12601
		[ReadOnly]
		[SerializeField]
		protected bool _centered;

		// Token: 0x0400313A RID: 12602
		protected Vector2 Offset;
	}
}
