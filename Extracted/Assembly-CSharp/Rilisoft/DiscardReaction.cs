using System;

namespace Rilisoft
{
	// Token: 0x020006A7 RID: 1703
	internal sealed class DiscardReaction<TInput> : ReactionBase<TInput>
	{
		// Token: 0x06003B8E RID: 15246 RVA: 0x0013564C File Offset: 0x0013384C
		internal override StateBase<TInput> GetNewState()
		{
			return null;
		}

		// Token: 0x04002C06 RID: 11270
		public static readonly DiscardReaction<TInput> Default = new DiscardReaction<TInput>();
	}
}
