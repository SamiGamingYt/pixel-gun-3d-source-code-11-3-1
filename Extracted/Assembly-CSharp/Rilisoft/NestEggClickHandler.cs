using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F2 RID: 1522
	public class NestEggClickHandler : MonoBehaviour
	{
		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x0010DEF0 File Offset: 0x0010C0F0
		private float Threshhold
		{
			get
			{
				return (float)Screen.width * this._thresholdPercent * 0.01f;
			}
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x0010DF08 File Offset: 0x0010C108
		private void Awake()
		{
			this._eggBaseRotation = this._eggObject.transform.localRotation;
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x0010DF20 File Offset: 0x0010C120
		private void Update()
		{
			if (!this._banner.IsVisible)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.MouseDown();
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.MouseUp();
			}
			if (this._isClicking && !this._isDragging && Mathf.Abs(this._mouseDownPos.x - Input.mousePosition.x) >= this.Threshhold)
			{
				this._isDragging = true;
			}
			if (this._isDragging)
			{
				Vector3 vector = this._prevMousePos - Input.mousePosition;
				Vector3 euler = new Vector3(this._eggObject.transform.localEulerAngles.x, this._eggObject.transform.localEulerAngles.y + vector.x * this._sensitivity, this._eggObject.transform.localEulerAngles.z);
				this._eggObject.transform.localRotation = Quaternion.Euler(euler);
			}
			this._prevMousePos = Input.mousePosition;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x0010E044 File Offset: 0x0010C244
		private void MouseDown()
		{
			if (!this._banner.IsVisible)
			{
				return;
			}
			this._isClicking = true;
			this._mouseDownPos = Input.mousePosition;
			this._prevMousePos = this._mouseDownPos;
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x0010E078 File Offset: 0x0010C278
		private void MouseUp()
		{
			if (!this._banner.IsVisible)
			{
				return;
			}
			if (!this._isDragging && Mathf.Abs(this._mouseDownPos.magnitude - Input.mousePosition.magnitude) < this.Threshhold)
			{
				this._banner.Hide();
			}
			this._isClicking = false;
			this._isDragging = false;
			this._mouseDownPos = Vector3.zero;
			this._prevMousePos = this._mouseDownPos;
		}

		// Token: 0x04002656 RID: 9814
		[SerializeField]
		private GameObject _eggObject;

		// Token: 0x04002657 RID: 9815
		[SerializeField]
		private NestBanner _banner;

		// Token: 0x04002658 RID: 9816
		[SerializeField]
		private float _sensitivity = 0.1f;

		// Token: 0x04002659 RID: 9817
		[SerializeField]
		private float _thresholdPercent = 1f;

		// Token: 0x0400265A RID: 9818
		private Quaternion _eggBaseRotation;

		// Token: 0x0400265B RID: 9819
		private bool _isClicking;

		// Token: 0x0400265C RID: 9820
		private bool _isDragging;

		// Token: 0x0400265D RID: 9821
		private Vector3 _mouseDownPos;

		// Token: 0x0400265E RID: 9822
		private Vector3 _prevMousePos;
	}
}
