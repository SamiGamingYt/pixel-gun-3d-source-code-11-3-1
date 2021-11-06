using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006E4 RID: 1764
	public class StateMachine<T> : MonoBehaviour
	{
		// Token: 0x06003D6C RID: 15724 RVA: 0x0013F2C8 File Offset: 0x0013D4C8
		public StateMachine()
		{
			this._registeredStates = new Dictionary<T, StateMachine<T>.State<T>>();
			this._transitions = new List<StateMachine<T>.Transition<T>>();
		}

		// Token: 0x14000073 RID: 115
		// (add) Token: 0x06003D6D RID: 15725 RVA: 0x0013F2FC File Offset: 0x0013D4FC
		// (remove) Token: 0x06003D6E RID: 15726 RVA: 0x0013F318 File Offset: 0x0013D518
		public event Action<T, T> OnStateChanged;

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x0013F334 File Offset: 0x0013D534
		// (set) Token: 0x06003D70 RID: 15728 RVA: 0x0013F33C File Offset: 0x0013D53C
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003D71 RID: 15729 RVA: 0x0013F348 File Offset: 0x0013D548
		// (set) Token: 0x06003D72 RID: 15730 RVA: 0x0013F350 File Offset: 0x0013D550
		public StateMachine<T>.State<T> CurrentState { get; private set; }

		// Token: 0x06003D73 RID: 15731 RVA: 0x0013F35C File Offset: 0x0013D55C
		protected StateMachine<T> Register(StateMachine<T>.State<T> state)
		{
			if (this._registeredStates.Keys.Contains(state.StateId))
			{
				Debug.LogErrorFormat("state for '{0}' allready exists", new object[]
				{
					state.StateId
				});
				return this;
			}
			this._registeredStates.Add(state.StateId, state);
			return this;
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x0013F3B8 File Offset: 0x0013D5B8
		protected void Clean()
		{
			this._registeredStates.Clear();
			this._transitions.Clear();
			this.CurrentState = null;
			this._prevState = null;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0013F3EC File Offset: 0x0013D5EC
		protected StateMachine<T> RegisterTransition(StateMachine<T>.Transition<T> transition)
		{
			this._transitions.Add(transition);
			return this;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0013F3FC File Offset: 0x0013D5FC
		public void To(T stateId)
		{
			if (!this._registeredStates.Keys.Contains(stateId))
			{
				Debug.LogErrorFormat("state for '{0}' not found", new object[]
				{
					stateId
				});
				return;
			}
			if (this.CurrentState != null)
			{
				this._prevState = this.CurrentState;
				this._prevState.Out(stateId);
				foreach (StateMachine<T>.Transition<T> transition in this._transitions)
				{
					T from = transition.From;
					if (from.Equals(this._prevState.StateId))
					{
						T to = transition.To;
						if (to.Equals(stateId))
						{
							transition.Action();
						}
					}
				}
			}
			this.CurrentState = this._registeredStates[stateId];
			this.CurrentState.In((this._prevState == null) ? default(T) : this._prevState.StateId);
			if (this.OnStateChanged != null)
			{
				this.OnStateChanged(this.CurrentState.StateId, (this._prevState == null) ? default(T) : this._prevState.StateId);
			}
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0013F584 File Offset: 0x0013D784
		protected virtual void Tick()
		{
			if (this.Enabled && this.CurrentState != null)
			{
				this.CurrentState.Update();
			}
		}

		// Token: 0x04002D50 RID: 11600
		private bool _enabled;

		// Token: 0x04002D51 RID: 11601
		private StateMachine<T>.State<T> _prevState;

		// Token: 0x04002D52 RID: 11602
		private Dictionary<T, StateMachine<T>.State<T>> _registeredStates;

		// Token: 0x04002D53 RID: 11603
		private List<StateMachine<T>.Transition<T>> _transitions;

		// Token: 0x04002D54 RID: 11604
		private int _callStackSize = 10;

		// Token: 0x04002D55 RID: 11605
		[SerializeField]
		[TextArea(5, 10)]
		private string _callHistory;

		// Token: 0x04002D56 RID: 11606
		private List<string> _callStack;

		// Token: 0x020006E5 RID: 1765
		public abstract class State<T>
		{
			// Token: 0x06003D78 RID: 15736 RVA: 0x0013F5A8 File Offset: 0x0013D7A8
			public State(T stateId, StateMachine<T> context)
			{
				this.StateId = stateId;
				this.Ctx = context;
			}

			// Token: 0x17000A3B RID: 2619
			// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0013F5C0 File Offset: 0x0013D7C0
			// (set) Token: 0x06003D7A RID: 15738 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
			public T StateId { get; private set; }

			// Token: 0x06003D7B RID: 15739 RVA: 0x0013F5D4 File Offset: 0x0013D7D4
			public virtual void In(T fromState)
			{
			}

			// Token: 0x06003D7C RID: 15740 RVA: 0x0013F5D8 File Offset: 0x0013D7D8
			public virtual void Out(T toState)
			{
			}

			// Token: 0x06003D7D RID: 15741 RVA: 0x0013F5DC File Offset: 0x0013D7DC
			public virtual void Update()
			{
			}

			// Token: 0x06003D7E RID: 15742 RVA: 0x0013F5E0 File Offset: 0x0013D7E0
			protected void To(T state)
			{
				this.Ctx.To(state);
			}

			// Token: 0x04002D59 RID: 11609
			protected StateMachine<T> Ctx;
		}

		// Token: 0x020006E6 RID: 1766
		public abstract class Transition<T>
		{
			// Token: 0x06003D7F RID: 15743 RVA: 0x0013F5F0 File Offset: 0x0013D7F0
			public Transition(T from, T to, StateMachine<T> context)
			{
				this.From = from;
				this.To = to;
				this.Ctx = context;
			}

			// Token: 0x17000A3C RID: 2620
			// (get) Token: 0x06003D80 RID: 15744 RVA: 0x0013F610 File Offset: 0x0013D810
			// (set) Token: 0x06003D81 RID: 15745 RVA: 0x0013F618 File Offset: 0x0013D818
			public T From { get; private set; }

			// Token: 0x17000A3D RID: 2621
			// (get) Token: 0x06003D82 RID: 15746 RVA: 0x0013F624 File Offset: 0x0013D824
			// (set) Token: 0x06003D83 RID: 15747 RVA: 0x0013F62C File Offset: 0x0013D82C
			public T To { get; private set; }

			// Token: 0x06003D84 RID: 15748 RVA: 0x0013F638 File Offset: 0x0013D838
			public virtual void Action()
			{
			}

			// Token: 0x04002D5B RID: 11611
			protected StateMachine<T> Ctx;
		}
	}
}
