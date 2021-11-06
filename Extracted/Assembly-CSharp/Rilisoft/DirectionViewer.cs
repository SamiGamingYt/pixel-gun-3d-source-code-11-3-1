using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200028E RID: 654
	public class DirectionViewer : MonoBehaviour
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x00052AB0 File Offset: 0x00050CB0
		// (set) Token: 0x060014E4 RID: 5348 RVA: 0x00052AB8 File Offset: 0x00050CB8
		public static DirectionViewer Instance { get; private set; }

		// Token: 0x060014E5 RID: 5349 RVA: 0x00052AC0 File Offset: 0x00050CC0
		private void Awake()
		{
			if (DirectionViewer.Instance != null)
			{
				return;
			}
			DirectionViewer.Instance = this;
			this._activePointers.Clear();
			this._freePointers.Clear();
			List<DirectionPointer> list = base.GetComponentsInChildren<DirectionPointer>(true).ToList<DirectionPointer>();
			foreach (DirectionPointer directionPointer in list)
			{
				if (this._freePointers.ContainsKey(directionPointer.ForPointerType))
				{
					this._freePointers[directionPointer.ForPointerType].Enqueue(directionPointer);
				}
				else
				{
					Queue<DirectionPointer> queue = new Queue<DirectionPointer>();
					queue.Enqueue(directionPointer);
					this._freePointers.Add(directionPointer.ForPointerType, queue);
				}
			}
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00052BA8 File Offset: 0x00050DA8
		private void OnDisable()
		{
			this._activePointers.ForEach(delegate(DirectionPointer p)
			{
				this.ForgetPointer(p);
			});
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00052BC4 File Offset: 0x00050DC4
		private void LateUpdate()
		{
			if (WeaponManager.sharedManager == null)
			{
				return;
			}
			int count = this._activePointers.Count;
			for (int i = 0; i < count; i++)
			{
				DirectionPointer pointerState = this._activePointers[i];
				this.SetPointerState(pointerState);
			}
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00052C14 File Offset: 0x00050E14
		public void LookToMe(DirectionViewerTarget target)
		{
			if (target == null || this._activePointers.Any((DirectionPointer p) => p.Target == target))
			{
				return;
			}
			if (!this._freePointers.ContainsKey(target.Type) || !this._freePointers[target.Type].Any<DirectionPointer>())
			{
				return;
			}
			DirectionPointer directionPointer = this._freePointers[target.Type].Dequeue();
			this._activePointers.Add(directionPointer);
			directionPointer.TurnOn(target);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00052CCC File Offset: 0x00050ECC
		public void ForgetMe(DirectionViewerTarget target)
		{
			DirectionPointer directionPointer = this._activePointers.FirstOrDefault((DirectionPointer p) => p.Target == target);
			if (directionPointer == null)
			{
				return;
			}
			directionPointer.TurnOff();
			this._activePointers.Remove(directionPointer);
			this._freePointers[directionPointer.ForPointerType].Enqueue(directionPointer);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00052D38 File Offset: 0x00050F38
		private void ForgetPointer(DirectionPointer pointer)
		{
			pointer.TurnOff();
			if (!this._activePointers.Contains(pointer))
			{
				return;
			}
			this._activePointers.Remove(pointer);
			this._freePointers[pointer.ForPointerType].Enqueue(pointer);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00052D84 File Offset: 0x00050F84
		private bool CheckDistance(DirectionViewerTarget poiner)
		{
			return !(WeaponManager.sharedManager == null) && !(poiner == null) && (WeaponManager.sharedManager.myPlayer.transform.position - poiner.Transform.position).sqrMagnitude < Mathf.Pow(this.GetSettings(poiner.Type).LookRadius, 2f);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00052DF8 File Offset: 0x00050FF8
		private void SetPointerState(DirectionPointer pointer)
		{
			if (!this.CheckDistance(pointer.Target))
			{
				pointer.OutOfRange = true;
				pointer.Hide();
				return;
			}
			if (pointer.OutOfRange)
			{
				pointer.OutOfRange = false;
				pointer.TurnOn(pointer.Target);
			}
			float angle = this.GetAngle(NickLabelController.currentCamera.transform, pointer.Target.transform.position, Vector3.up);
			float circleRadius = this.GetSettings(pointer.ForPointerType).CircleRadius;
			Vector3 localPosition = new Vector3(circleRadius * Mathf.Sin(angle * 0.017453292f), circleRadius * Mathf.Cos(angle * 0.017453292f), pointer.transform.position.z);
			pointer.transform.localPosition = localPosition;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00052EBC File Offset: 0x000510BC
		private float GetAngle(Transform from, Vector3 target, Vector3 n)
		{
			Vector3 forward = from.forward;
			Vector3 rhs = target - from.position;
			return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(forward, rhs)), Vector3.Dot(forward, rhs)) * 57.29578f;
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x00052EFC File Offset: 0x000510FC
		private DirectionViewerSettings GetSettings(DirectionViewTargetType type)
		{
			return this._settings.First((DirectionViewerSettings s) => s.ForType == type);
		}

		// Token: 0x04000C32 RID: 3122
		[SerializeField]
		private UIPanel _panel;

		// Token: 0x04000C33 RID: 3123
		[SerializeField]
		private List<DirectionViewerSettings> _settings = new List<DirectionViewerSettings>
		{
			new DirectionViewerSettings
			{
				ForType = DirectionViewTargetType.Grenade,
				LookRadius = 10f,
				CircleRadius = 200f
			},
			new DirectionViewerSettings
			{
				ForType = DirectionViewTargetType.Pet,
				LookRadius = 1000f,
				CircleRadius = 400f
			}
		};

		// Token: 0x04000C34 RID: 3124
		private readonly Dictionary<DirectionViewTargetType, Queue<DirectionPointer>> _freePointers = new Dictionary<DirectionViewTargetType, Queue<DirectionPointer>>();

		// Token: 0x04000C35 RID: 3125
		private readonly List<DirectionPointer> _activePointers = new List<DirectionPointer>();
	}
}
