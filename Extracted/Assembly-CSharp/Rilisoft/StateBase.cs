using System;

namespace Rilisoft
{
	// Token: 0x020006A5 RID: 1701
	internal abstract class StateBase<TInput>
	{
		// Token: 0x14000071 RID: 113
		// (add) Token: 0x06003B85 RID: 15237 RVA: 0x001355F0 File Offset: 0x001337F0
		// (remove) Token: 0x06003B86 RID: 15238 RVA: 0x0013560C File Offset: 0x0013380C
		public event StateBase<TInput>.EventHandler InputRequested;

		// Token: 0x06003B87 RID: 15239 RVA: 0x00135628 File Offset: 0x00133828
		public virtual void Enter(StateBase<TInput> oldState, TInput input)
		{
		}

		// Token: 0x06003B88 RID: 15240 RVA: 0x0013562C File Offset: 0x0013382C
		public virtual void Exit(StateBase<TInput> newState, TInput input)
		{
		}

		// Token: 0x06003B89 RID: 15241
		public abstract ReactionBase<TInput> React(TInput input);

		// Token: 0x02000919 RID: 2329
		// (Invoke) Token: 0x06005104 RID: 20740
		public delegate void EventHandler(object sender, TInput e);
	}
}
