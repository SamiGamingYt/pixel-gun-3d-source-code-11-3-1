using System;

namespace Rilisoft
{
	// Token: 0x020006A8 RID: 1704
	internal sealed class TransitReaction<TState, TInput> : ReactionBase<TInput> where TState : StateBase<TInput>
	{
		// Token: 0x06003B8F RID: 15247 RVA: 0x00135650 File Offset: 0x00133850
		public TransitReaction(StateBase<TInput> newState)
		{
			if (newState == null)
			{
				throw new ArgumentNullException("newState");
			}
			this._newState = newState;
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x00135670 File Offset: 0x00133870
		internal override StateBase<TInput> GetNewState()
		{
			return this._newState;
		}

		// Token: 0x04002C07 RID: 11271
		private readonly StateBase<TInput> _newState;
	}
}
