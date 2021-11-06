using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006D5 RID: 1749
	public class FlyingPetEngine : PetEngine
	{
		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06003CCF RID: 15567 RVA: 0x0013BEDC File Offset: 0x0013A0DC
		public override Vector3 MovePosition
		{
			get
			{
				return (!(base.Owner.GetPointForFlyingPet() != null)) ? base.Owner.transform.position : base.Owner.GetPointForFlyingPet().position;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x0013BF24 File Offset: 0x0013A124
		protected override Vector3? MoveToTargetPosition
		{
			get
			{
				if (this.Target != null)
				{
					return new Vector3?(new Vector3(this.Target.position.x, this.Target.position.y + 1.5f, this.Target.position.z));
				}
				return null;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x0013BF98 File Offset: 0x0013A198
		protected override StateMachine<PetState>.State<PetState> MoveToOwnerState
		{
			get
			{
				return new FlyingPetEngine.PetMoveToOwnerState(this);
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x0013BFA0 File Offset: 0x0013A1A0
		protected override StateMachine<PetState>.State<PetState> MoveToTargetState
		{
			get
			{
				return new FlyingPetEngine.PetMoveToTargetState(this);
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003CD3 RID: 15571 RVA: 0x0013BFA8 File Offset: 0x0013A1A8
		private TargetPositionMonitor TargetPosMon
		{
			get
			{
				if (this._posMonVal == null)
				{
					this._posMonVal = (base.GetComponent<TargetPositionMonitor>() ?? base.gameObject.AddComponent<TargetPositionMonitor>());
				}
				return this._posMonVal;
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0013BFEC File Offset: 0x0013A1EC
		protected override void Awake()
		{
			base.Awake();
			this.Character = base.GetComponent<CharacterController>();
			foreach (Collider collider in base.gameObject.GetComponents<Collider>())
			{
				if (collider != this.BodyCollider)
				{
					this._characterControllerCollider = collider;
				}
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0013C048 File Offset: 0x0013A248
		protected override void StopEngine()
		{
			base.StopEngine();
			this.Character.enabled = false;
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0013C05C File Offset: 0x0013A25C
		protected override void InitSM()
		{
			base.InitSM();
			if (this.Character != null)
			{
				this.Character.enabled = base.IsMine;
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0013C094 File Offset: 0x0013A294
		public void Move()
		{
			Vector3 vector = this.TargetPosMon.GetCurrentPoint();
			if (Vector3.Distance(this.ThisTransform.position, vector) < 0.2f)
			{
				vector = ((!this.TargetPosMon.HasNextPoint()) ? Vector3.zero : this.TargetPosMon.GetNextPoint());
			}
			if (vector == Vector3.zero)
			{
				return;
			}
			Quaternion b = Quaternion.LookRotation(vector - base.transform.position, Vector3.up);
			this.ThisTransform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * 10f);
			Vector3 vector2 = (vector - this.ThisTransform.position).normalized;
			if (base.CurrentState.StateId == PetState.moveToTarget)
			{
				vector2 *= Mathf.Clamp(this.GetCalculatedSpeedMultyplier() * base.Info.SpeedModif, 0f, base.Info.SpeedModif) * 0.015f;
			}
			else
			{
				vector2 *= this.GetCalculatedSpeedMultyplier() * base.Info.SpeedModif * 0.015f;
			}
			this.Character.Move(vector2);
			base.PlaySound(this.ClipWalk);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0013C1E0 File Offset: 0x0013A3E0
		private IEnumerator CheckIsMoving()
		{
			this._unwaikableElapsedTime = 0f;
			for (;;)
			{
				if (base.IsMoving)
				{
					this._unwaikableElapsedTime = 0f;
				}
				else
				{
					this._unwaikableElapsedTime += Time.deltaTime;
					if (this._unwaikableElapsedTime >= 1f)
					{
						this._unwaikableElapsedTime = 0f;
						base.To(PetState.teleport);
						base.StopCoroutine("CheckIsMoving");
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0013C1FC File Offset: 0x0013A3FC
		public void CheckIsMovingStart()
		{
			base.StopCoroutine("CheckIsMoving");
			base.StartCoroutine("CheckIsMoving");
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x0013C218 File Offset: 0x0013A418
		public void CheckIsMovingStop()
		{
			base.StopCoroutine("CheckIsMoving");
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0013C228 File Offset: 0x0013A428
		public override void SetCollidersEnabled(bool enabled)
		{
			base.SetCollidersEnabled(enabled);
			if (this._characterControllerCollider.enabled != enabled)
			{
				this._characterControllerCollider.enabled = enabled;
			}
		}

		// Token: 0x04002CF4 RID: 11508
		private const float _unwaikableTeleportTime = 1f;

		// Token: 0x04002CF5 RID: 11509
		public CharacterController Character;

		// Token: 0x04002CF6 RID: 11510
		private TargetPositionMonitor _posMonVal;

		// Token: 0x04002CF7 RID: 11511
		private Collider _characterControllerCollider;

		// Token: 0x04002CF8 RID: 11512
		private float _unwaikableElapsedTime;

		// Token: 0x020006D6 RID: 1750
		private class PetMoveToOwnerState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003CDC RID: 15580 RVA: 0x0013C25C File Offset: 0x0013A45C
			public PetMoveToOwnerState(FlyingPetEngine context) : base(PetState.moveToOwner, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003CDD RID: 15581 RVA: 0x0013C270 File Offset: 0x0013A470
			public override void In(PetState fromState)
			{
				base.In(fromState);
				if (!this.ctx.IsVisible(this.ctx.Owner.gameObject, 200f))
				{
					base.To(PetState.teleport);
					return;
				}
				this.ctx.TargetPosMon.StartMonitoring(() => this.ctx.Owner.transform.position, 0.1f, 0.1f);
				this.ctx.CheckIsMovingStart();
			}

			// Token: 0x06003CDE RID: 15582 RVA: 0x0013C2E4 File Offset: 0x0013A4E4
			public override void Update()
			{
				base.Update();
				if (this.ctx.Owner == null || !this.ctx.IsAlive)
				{
					base.To(PetState.idle);
				}
				else if (!this.ctx.InRange(this.ctx.MovePosition, this.ctx.ThisTransform.position, this.ctx.Info.MaxToOwnerDistance) || this.ctx.Owner.isKilled)
				{
					base.To(PetState.teleport);
				}
				else if (this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.MovePosition, this.ctx.Info.MinToOwnerDistance))
				{
					base.To(PetState.idle);
				}
				else if (this.ctx.Target != null)
				{
					base.To(PetState.moveToTarget);
				}
				else
				{
					this._resetMonitorTimeElapsed += Time.deltaTime;
					if (this._resetMonitorTimeElapsed >= 0.3f)
					{
						this._resetMonitorTimeElapsed = 0f;
						if (this.ctx.IsVisible(this.ctx.Owner.gameObject, 200f))
						{
							this.ctx.TargetPosMon.Reset();
						}
					}
					this.ctx.Move();
				}
			}

			// Token: 0x06003CDF RID: 15583 RVA: 0x0013C45C File Offset: 0x0013A65C
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this.ctx.TargetPosMon.StopMonitoring();
				this._resetMonitorTimeElapsed = 0f;
				this.ctx.CheckIsMovingStop();
			}

			// Token: 0x04002CF9 RID: 11513
			private const float _resetMonitorInterval = 0.3f;

			// Token: 0x04002CFA RID: 11514
			private FlyingPetEngine ctx;

			// Token: 0x04002CFB RID: 11515
			private float _resetMonitorTimeElapsed;
		}

		// Token: 0x020006D7 RID: 1751
		private class PetMoveToTargetState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003CE1 RID: 15585 RVA: 0x0013C4A4 File Offset: 0x0013A6A4
			public PetMoveToTargetState(FlyingPetEngine context) : base(PetState.moveToTarget, context)
			{
				this.ctx = context;
			}

			// Token: 0x06003CE2 RID: 15586 RVA: 0x0013C4B8 File Offset: 0x0013A6B8
			public override void In(PetState fromState)
			{
				base.In(fromState);
				if (this.ctx.Target == null)
				{
					base.To(PetState.idle);
					return;
				}
				this.ctx.TargetPosMon.StartMonitoring(() => this.ctx.MoveToTargetPosition.Value, 0.1f, 0.1f);
				this.ctx.CheckIsMovingStart();
			}

			// Token: 0x06003CE3 RID: 15587 RVA: 0x0013C51C File Offset: 0x0013A71C
			public override void Update()
			{
				base.Update();
				if (this.ctx.Target == null || !this.ctx.IsAlive || this.ctx.MoveToTargetPosition == null)
				{
					base.To(PetState.idle);
					return;
				}
				if (!this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.Owner.transform.position, this.ctx.Info.MaxToOwnerDistance))
				{
					base.To(PetState.teleport);
					return;
				}
				if (this.ctx.Owner.isKilled)
				{
					base.To(PetState.teleport);
				}
				else
				{
					if (this.ctx.Target != null && this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.MoveToTargetPosition.Value, this.ctx.Info.AttackDistance))
					{
						if (!this.ctx.InAttackState)
						{
							base.To(PetState.attack);
						}
						return;
					}
					this._resetMonitorTimeElapsed += Time.deltaTime;
					if (this._resetMonitorTimeElapsed >= 0.3f)
					{
						this._resetMonitorTimeElapsed = 0f;
						if (this.ctx.IsVisible(this.ctx.Target.gameObject, 200f))
						{
							this.ctx.TargetPosMon.Reset();
						}
					}
					this.ctx.Move();
				}
			}

			// Token: 0x06003CE4 RID: 15588 RVA: 0x0013C6C4 File Offset: 0x0013A8C4
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this.ctx.TargetPosMon.StopMonitoring();
				this._resetMonitorTimeElapsed = 0f;
				this.ctx.CheckIsMovingStop();
			}

			// Token: 0x04002CFC RID: 11516
			private const float _resetMonitorInterval = 0.3f;

			// Token: 0x04002CFD RID: 11517
			private FlyingPetEngine ctx;

			// Token: 0x04002CFE RID: 11518
			private float _resetMonitorTimeElapsed;
		}
	}
}
