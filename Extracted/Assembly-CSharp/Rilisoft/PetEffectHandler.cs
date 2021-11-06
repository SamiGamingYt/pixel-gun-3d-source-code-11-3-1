using System;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x020006F0 RID: 1776
	public class PetEffectHandler : MonoBehaviour
	{
		// Token: 0x06003DBE RID: 15806 RVA: 0x0014101C File Offset: 0x0013F21C
		private void OnEnable()
		{
			this._thisTransform = base.transform;
			this._timeElapsed = 0f;
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00141038 File Offset: 0x0013F238
		private void OnDisable()
		{
			this.OnEffectCompleted.Invoke();
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00141048 File Offset: 0x0013F248
		private void Update()
		{
			this._thisTransform.rotation = Quaternion.Euler(new Vector3(0f, this._thisTransform.rotation.y, 0f));
			this._timeElapsed += Time.deltaTime;
			if (this._timeElapsed >= this.WaitTime)
			{
				base.gameObject.SetActiveSafe(false);
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x001410B8 File Offset: 0x0013F2B8
		public void Play()
		{
			base.gameObject.SetActiveSafe(true);
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x001410C8 File Offset: 0x0013F2C8
		public bool IsPlaying
		{
			get
			{
				return base.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x04002D8D RID: 11661
		public float WaitTime = 1f;

		// Token: 0x04002D8E RID: 11662
		public UnityEvent OnEffectCompleted;

		// Token: 0x04002D8F RID: 11663
		private float _timeElapsed;

		// Token: 0x04002D90 RID: 11664
		private Transform _thisTransform;
	}
}
