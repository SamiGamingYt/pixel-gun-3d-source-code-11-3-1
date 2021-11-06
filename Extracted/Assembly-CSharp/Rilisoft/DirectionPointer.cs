using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200028C RID: 652
	[RequireComponent(typeof(UIWidget))]
	public class DirectionPointer : MonoBehaviour
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x000528F4 File Offset: 0x00050AF4
		public DirectionViewTargetType ForPointerType
		{
			get
			{
				return this._forPointerType;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x000528FC File Offset: 0x00050AFC
		// (set) Token: 0x060014DA RID: 5338 RVA: 0x00052904 File Offset: 0x00050B04
		public DirectionViewerTarget Target { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x00052910 File Offset: 0x00050B10
		public bool IsInited
		{
			get
			{
				return this.Target != null;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060014DC RID: 5340 RVA: 0x00052920 File Offset: 0x00050B20
		private UIWidget _widget
		{
			get
			{
				if (this._widgetVal == null)
				{
					this._widgetVal = base.GetComponent<UIWidget>();
				}
				return this._widgetVal;
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00052948 File Offset: 0x00050B48
		public void TurnOn(DirectionViewerTarget pointer)
		{
			this.Target = pointer;
			base.gameObject.SetActive(true);
			this._widget.alpha = 1f;
			this._widget.gameObject.transform.localScale = Vector3.one;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00052994 File Offset: 0x00050B94
		public void Hide()
		{
			if (base.gameObject.activeInHierarchy)
			{
				base.StartCoroutine(this.TurnOffCoroutine());
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000529D0 File Offset: 0x00050BD0
		public void TurnOff()
		{
			this.Target = null;
			this.OutOfRange = false;
			this.Hide();
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x000529E8 File Offset: 0x00050BE8
		private IEnumerator TurnOffCoroutine()
		{
			float elapsed = 0f;
			while (elapsed <= this._hideTime && this.Target == null)
			{
				elapsed += Time.deltaTime;
				this._widget.alpha = Mathf.Lerp(1f, 0.1f, elapsed / this._hideTime);
				float scalMltp = Mathf.Lerp(1f, 2f, elapsed / this._hideTime);
				this._widget.gameObject.transform.localScale = Vector3.one * scalMltp;
				yield return null;
			}
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x04000C2A RID: 3114
		[SerializeField]
		private DirectionViewTargetType _forPointerType;

		// Token: 0x04000C2B RID: 3115
		[Range(0f, 3f)]
		[SerializeField]
		private float _hideTime = 0.3f;

		// Token: 0x04000C2C RID: 3116
		public bool OutOfRange;

		// Token: 0x04000C2D RID: 3117
		private UIWidget _widgetVal;
	}
}
