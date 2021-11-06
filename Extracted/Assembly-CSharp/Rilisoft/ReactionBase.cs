using System;

namespace Rilisoft
{
	// Token: 0x020006A6 RID: 1702
	internal abstract class ReactionBase<TInput>
	{
		// Token: 0x06003B8B RID: 15243
		internal abstract StateBase<TInput> GetNewState();
	}
}
