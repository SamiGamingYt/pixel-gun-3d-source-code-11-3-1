using System;

namespace Rilisoft
{
	// Token: 0x02000734 RID: 1844
	public sealed class QuestCompletedEventArgs : EventArgs
	{
		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060040D3 RID: 16595 RVA: 0x0015A4E4 File Offset: 0x001586E4
		// (set) Token: 0x060040D4 RID: 16596 RVA: 0x0015A4EC File Offset: 0x001586EC
		public QuestBase Quest { get; set; }
	}
}
