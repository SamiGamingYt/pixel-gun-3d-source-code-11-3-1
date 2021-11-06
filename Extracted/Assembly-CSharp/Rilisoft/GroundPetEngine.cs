using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006D8 RID: 1752
	public class GroundPetEngine : PetEngine
	{
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003CE7 RID: 15591 RVA: 0x0013C71C File Offset: 0x0013A91C
		public override Vector3 MovePosition
		{
			get
			{
				return (!(base.Owner.GetPointForGroundPet() != null)) ? base.Owner.transform.position : base.Owner.GetPointForGroundPet().position;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06003CE8 RID: 15592 RVA: 0x0013C764 File Offset: 0x0013A964
		protected override StateMachine<PetState>.State<PetState> MoveToOwnerState
		{
			get
			{
				return new GroundPetEngine.PetMoveToOwnerState(this);
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003CE9 RID: 15593 RVA: 0x0013C76C File Offset: 0x0013A96C
		protected override StateMachine<PetState>.State<PetState> MoveToTargetState
		{
			get
			{
				return new GroundPetEngine.PetMoveToTargetState(this);
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003CEA RID: 15594 RVA: 0x0013C774 File Offset: 0x0013A974
		public override bool CanMoveToPlayer
		{
			get
			{
				if (this.Path == null)
				{
					this.Path = new NavMeshPath();
				}
				if (this.ThisTransform.position == base.Owner.myPlayerTransform.position)
				{
					return false;
				}
				NavMesh.CalculatePath(this.ThisTransform.position, base.Owner.myPlayerTransform.position, -1, this.Path);
				if (this.Path.corners.Length < 1)
				{
					return false;
				}
				float num = 0f;
				for (int i = 1; i < this.Path.corners.Length; i++)
				{
					num += Vector3.Magnitude(this.Path.corners[i - 1] - this.Path.corners[i]);
				}
				return num <= base.Info.MaxToOwnerDistance * 2f;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06003CEB RID: 15595 RVA: 0x0013C874 File Offset: 0x0013AA74
		public bool CanTeleportToTarget
		{
			get
			{
				if (this.Target == null || Defs.isMulti)
				{
					return false;
				}
				if (this.ThisTransform.position == this.Target.position)
				{
					return false;
				}
				float magnitude = (this.Target.position - this.ThisTransform.position).magnitude;
				if (magnitude > base.Info.ToTargetTeleportDistance)
				{
					return false;
				}
				if (this.Target.root != null && Initializer.enemiesObj.Contains(this.Target.root.gameObject))
				{
					return false;
				}
				if (this.Path == null)
				{
					this.Path = new NavMeshPath();
				}
				NavMesh.CalculatePath(this.ThisTransform.position, this.Target.position, -1, this.Path);
				return this.Path.corners.Length < 1 || (this.Path.corners[this.Path.corners.Length - 1] - this.Target.transform.position).magnitude > base.Info.AttackDistance;
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0013C9D0 File Offset: 0x0013ABD0
		public override void WarpToOwner()
		{
			this.Nma.Warp(this.MovePosition);
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0013C9E4 File Offset: 0x0013ABE4
		protected override void Awake()
		{
			base.Awake();
			this.Nma = base.GetComponent<NavMeshAgent>();
			if (base.gameObject.GetComponent<AgentLinkMover>() == null)
			{
				base.gameObject.AddComponent<AgentLinkMover>();
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0013CA28 File Offset: 0x0013AC28
		protected override void StopEngine()
		{
			base.StopEngine();
			this.Nma.enabled = false;
		}

		// Token: 0x04002CFF RID: 11519
		[ReadOnly]
		public NavMeshAgent Nma;

		// Token: 0x04002D00 RID: 11520
		public NavMeshPath Path;

		// Token: 0x020006D9 RID: 1753
		private class PetMoveToOwnerState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003CEF RID: 15599 RVA: 0x0013CA3C File Offset: 0x0013AC3C
			public PetMoveToOwnerState(PetEngine context) : base(PetState.moveToOwner, context)
			{
				this.ctx = (context as GroundPetEngine);
			}

			// Token: 0x06003CF0 RID: 15600 RVA: 0x0013CA54 File Offset: 0x0013AC54
			public override void Update()
			{
				base.Update();
				if (this.ctx.Owner == null || !this.ctx.IsAlive)
				{
					base.To(PetState.idle);
					return;
				}
				if (this.ctx.Owner.isKilled)
				{
					base.To(PetState.teleport);
					return;
				}
				if (this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.MovePosition, this.ctx.Info.MinToOwnerDistance))
				{
					base.To(PetState.idle);
					return;
				}
				if (this.ctx.Target != null)
				{
					base.To(PetState.moveToTarget);
					return;
				}
				if (!this.ctx.InRange(this.ctx.MovePosition, this.ctx.ThisTransform.position, this.ctx.Info.MaxToOwnerDistance))
				{
					base.To(PetState.teleport);
					return;
				}
				if (!this.ctx.Nma.enabled || this.ctx.InRange(this.ctx.MovePosition, this.ctx.ThisTransform.position, this.ctx.Info.MinToOwnerDistance))
				{
					base.To(PetState.teleport);
					return;
				}
				if (this.ctx.CanMoveToPlayer || !this.ctx.OwnerIsGrounded)
				{
					if (!this.ctx.Nma.isOnOffMeshLink)
					{
						NavMesh.CalculatePath(this.ctx.gameObject.transform.position, this.ctx.Owner.myPlayerTransform.position, -1, this.ctx.Path);
						this.ctx.Nma.SetPath(this.ctx.Path);
						this.ctx.Nma.speed = this.ctx.GetCalculatedSpeedMultyplier() * this.ctx.Info.SpeedModif;
						this.ctx.Nma.Resume();
					}
					return;
				}
				base.To(PetState.teleport);
			}

			// Token: 0x06003CF1 RID: 15601 RVA: 0x0013CC8C File Offset: 0x0013AE8C
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this.ctx.Nma.ResetPath();
				this.ctx.Nma.Stop();
			}

			// Token: 0x04002D01 RID: 11521
			private GroundPetEngine ctx;
		}

		// Token: 0x020006DA RID: 1754
		private class PetMoveToTargetState : StateMachine<PetState>.State<PetState>
		{
			// Token: 0x06003CF2 RID: 15602 RVA: 0x0013CCB8 File Offset: 0x0013AEB8
			public PetMoveToTargetState(PetEngine context) : base(PetState.moveToTarget, context)
			{
				this.ctx = (context as GroundPetEngine);
			}

			// Token: 0x06003CF3 RID: 15603 RVA: 0x0013CCD0 File Offset: 0x0013AED0
			public override void In(PetState fromState)
			{
				base.In(fromState);
				if (this.ctx.Target == null)
				{
					this.ctx.Nma.Stop();
					base.To(PetState.idle);
					return;
				}
				if (this.ctx.Owner.isKilled)
				{
					this.ctx.Nma.Stop();
					base.To(PetState.teleport);
					return;
				}
			}

			// Token: 0x06003CF4 RID: 15604 RVA: 0x0013CD40 File Offset: 0x0013AF40
			public override void Update()
			{
				base.Update();
				if (this.ctx.Target == null || !this.ctx.IsAlive || !this.ctx.IsVisible(this.ctx.Target.gameObject, 200f))
				{
					base.To(PetState.idle);
					return;
				}
				if (!this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.Owner.transform.position, this.ctx.Info.MaxToOwnerDistance))
				{
					base.To(PetState.teleport);
					return;
				}
				if (this.ctx.InRange(this.ctx.ThisTransform.position, this.ctx.Target.position, this.ctx.Info.AttackDistance))
				{
					if (!this.ctx.InAttackState)
					{
						base.To(PetState.attack);
					}
					return;
				}
				if (this.ctx.CanTeleportToTarget)
				{
					this.ctx.Nma.Stop();
					this.ctx.Nma.ResetPath();
					this.ctx.Nma.Warp(this.ctx.Target.position);
					this.ctx.EffectShow.Play();
					if (Defs.isMulti && this.ctx.IsMine)
					{
						this.ctx.synchScript.isTeleported = true;
					}
				}
				else if (!this.ctx.Nma.isOnOffMeshLink)
				{
					this.ctx.Nma.SetDestination(this.ctx.Target.position);
					this.ctx.Nma.speed = Mathf.Clamp(this.ctx.GetCalculatedSpeedMultyplier() * this.ctx.Info.SpeedModif, 0f, this.ctx.Info.SpeedModif);
					this.ctx.Nma.Resume();
				}
			}

			// Token: 0x06003CF5 RID: 15605 RVA: 0x0013CF68 File Offset: 0x0013B168
			public override void Out(PetState toState)
			{
				base.Out(toState);
				this.ctx.Nma.Stop();
			}

			// Token: 0x04002D02 RID: 11522
			private GroundPetEngine ctx;
		}
	}
}
