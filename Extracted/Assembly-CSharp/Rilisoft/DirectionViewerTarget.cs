using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000290 RID: 656
	public class DirectionViewerTarget : MonoBehaviour
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060014F1 RID: 5361 RVA: 0x00052F44 File Offset: 0x00051144
		public DirectionViewTargetType Type
		{
			get
			{
				return this._Type;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060014F2 RID: 5362 RVA: 0x00052F4C File Offset: 0x0005114C
		public Transform Transform
		{
			get
			{
				return base.gameObject.transform;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00052F5C File Offset: 0x0005115C
		private void OnEnable()
		{
			DirectionViewTargetType type = this.Type;
			if (type != DirectionViewTargetType.Grenade)
			{
				if (type == DirectionViewTargetType.Pet)
				{
					base.StartCoroutine(this.PetMonitorCoroutine());
				}
			}
			else
			{
				base.StartCoroutine(this.RocketMonitorCoroutine());
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00052FAC File Offset: 0x000511AC
		private void OnDisable()
		{
			this.HidePointer();
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00052FB4 File Offset: 0x000511B4
		private IEnumerator RocketMonitorCoroutine()
		{
			while (this._rocketComponent == null)
			{
				this._rocketComponent = base.transform.root.GetComponentInParent<Rocket>();
				yield return null;
			}
			while (!this._rocketComponent.isRun)
			{
				yield return null;
			}
			this.ShowPointer();
			while (this._rocketComponent.isRun)
			{
				yield return null;
			}
			this.HidePointer();
			yield break;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00052FD0 File Offset: 0x000511D0
		private IEnumerator PetMonitorCoroutine()
		{
			this.ShowPointer();
			yield return null;
			yield break;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00052FEC File Offset: 0x000511EC
		private void ShowPointer()
		{
			CoroutineRunner.WaitUntil(() => DirectionViewer.Instance != null, delegate
			{
				DirectionViewer.Instance.LookToMe(this);
			});
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00053028 File Offset: 0x00051228
		private void HidePointer()
		{
			if (DirectionViewer.Instance != null)
			{
				DirectionViewer.Instance.ForgetMe(this);
			}
		}

		// Token: 0x04000C3A RID: 3130
		[SerializeField]
		private DirectionViewTargetType _Type;

		// Token: 0x04000C3B RID: 3131
		[ReadOnly]
		[SerializeField]
		private Rocket _rocketComponent;
	}
}
